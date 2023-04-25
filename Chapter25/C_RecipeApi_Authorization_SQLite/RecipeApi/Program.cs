using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RecipeApi.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();

// Configure authorization policies
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("CanManageRecipe", policyBuilder => 
        policyBuilder.AddRequirements(new IsRecipeOwnerRequirement()))
    .AddPolicy("CanCreateRecipe", policyBuilder => 
        policyBuilder.RequireClaim("Subscription", "Pro"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Recipe App", Version = "v1" });
    var security = new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    x.AddSecurityDefinition(security.Reference.Id, security);
    x.AddSecurityRequirement(new OpenApiSecurityRequirement { { security, Array.Empty<string>() } });
});

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connString!));
builder.Services.AddScoped<RecipeService>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

var routes = app.MapGroup("recipe")
    .WithParameterValidation()
    .WithOpenApi()
    .WithTags("Recipes")
    .RequireAuthorization(); // <-- Require authorization for all endpoints in the group

routes.MapGet("/", async (RecipeService service) =>
{
    return await service.GetRecipes();
})
    .WithSummary("List recipes")
    .ProducesProblem(StatusCodes.Status401Unauthorized);

routes.MapPost("/", async (CreateRecipeCommand input, RecipeService service, ClaimsPrincipal user) =>
{
    var id = await service.CreateRecipe(input, user);
    return Results.CreatedAtRoute("view-recipe", new { id });
})
    .WithSummary("Create recipe")
    .Produces(StatusCodes.Status201Created)
    .ProducesProblem(StatusCodes.Status401Unauthorized)
    .RequireAuthorization("CanCreateRecipe"); // <-- Add authorization policy

routes.MapGet("/{id}", async (int id, RecipeService service) =>
{
    var recipe = await service.GetRecipeDetail(id);
    return recipe is null
        ? Results.Problem(statusCode: 404)
        : Results.Ok(recipe);
})
    .WithName("view-recipe")
    .WithSummary("Get recipe")
    .AddEndpointFilter<CanManageRecipeFilter>() // <-- Apply policy using a filter
    .ProducesProblem(404)
    .Produces<RecipeDetailViewModel>()
    .ProducesProblem(StatusCodes.Status401Unauthorized);

routes.MapDelete("/{id}", async (int id, RecipeService service) =>
{
    await service.DeleteRecipe(id);
    return Results.NoContent();
})
    .WithSummary("Delete recipe")
    .AddEndpointFilter<CanManageRecipeFilter>() // <-- Apply policy using a filter
    .Produces(201);

routes.MapPut("/{id}", async (int id, UpdateRecipeCommand input, RecipeService service) =>
{
    if (await service.IsAvailableForUpdate(id))
    {
        await service.UpdateRecipe(input);
        return Results.NoContent();
    }
    return Results.Problem(statusCode: 404);
})
    .WithSummary("Update recipe")
    .AddEndpointFilter<CanManageRecipeFilter>() // <-- Apply policy using a filter
    .ProducesProblem(404)
    .ProducesProblem(401)
    .ProducesProblem(403)
    .Produces(204);

app.Run();

public class CanManageRecipeFilter : IEndpointFilter
{
    private readonly IAuthorizationService _authService;
    private readonly RecipeService _recipeService;
    public CanManageRecipeFilter(IAuthorizationService authService, RecipeService recipeService)
    {
        _authService = authService;
        _recipeService = recipeService;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var id = (int)context.Arguments[0];
        var recipe = await _recipeService.GetRecipe(id);

        var result = await _authService.AuthorizeAsync(context.HttpContext.User, recipe, "CanManageRecipe");
        if (!result.Succeeded)
        {
            return Results.Forbid();
        }

        return await next(context);
    }
}

#region Models
public class EditRecipeBase
{
    [Required, StringLength(100)]
    public required string Name { get; set; }
    [Range(0, 23), DisplayName("Time to cook (hrs)")]
    public int TimeToCookHrs { get; set; }
    [Range(0, 59), DisplayName("Time to cook (mins)")]
    public int TimeToCookMins { get; set; }
    [Required]
    public required string Method { get; set; }
    [DisplayName("Vegetarian?")]
    public bool IsVegetarian { get; set; }
    [DisplayName("Vegan?")]
    public bool IsVegan { get; set; }
}

public class CreateRecipeCommand : EditRecipeBase
{
    public IList<CreateIngredientCommand> Ingredients { get; set; } = new List<CreateIngredientCommand>();

    public Recipe ToRecipe(ClaimsPrincipal createdBy)
    {
        return new Recipe
        {
            Name = Name,
            TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
            Method = Method,
            IsVegetarian = IsVegetarian,
            IsVegan = IsVegan,
            CreatedById = createdBy.Identity?.Name ?? "Unknown",
            Ingredients = Ingredients.Select(x => x.ToIngredient()).ToList()
        };
    }
}
public class CreateIngredientCommand
{
    [Required, StringLength(100)]
    public required string Name { get; set; }
    [Range(0, int.MaxValue)]
    public decimal Quantity { get; set; }
    [Required, StringLength(20)]
    public required string Unit { get; set; }

    public Ingredient ToIngredient()
    {
        return new Ingredient
        {
            Name = Name,
            Quantity = Quantity,
            Unit = Unit,
        };
    }
}

public class RecipeDetailViewModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Method { get; set; }

    public required IEnumerable<Item> Ingredients { get; set; }

    public class Item
    {
        public required string Name { get; set; }
        public required string Quantity { get; set; }
    }
}

public class RecipeSummaryViewModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string TimeToCook { get; set; }
    public int NumberOfIngredients { get; set; }

    public static RecipeSummaryViewModel FromRecipe(Recipe recipe)
    {
        return new RecipeSummaryViewModel
        {
            Id = recipe.RecipeId,
            Name = recipe.Name,
            TimeToCook = $"{recipe.TimeToCook.Hours}hrs {recipe.TimeToCook.Minutes}mins",
        };
    }
}

public class UpdateRecipeCommand : EditRecipeBase
{
    public int Id { get; set; }

    public void UpdateRecipe(Recipe recipe)
    {
        recipe.Name = Name;
        recipe.TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0);
        recipe.Method = Method;
        recipe.IsVegetarian = IsVegetarian;
        recipe.IsVegan = IsVegan;
    }
}

#endregion