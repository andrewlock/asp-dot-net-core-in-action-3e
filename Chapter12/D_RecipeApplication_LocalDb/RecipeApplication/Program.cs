using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo { Title = "Recipe App", Version = "v1" }));

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connString!));
builder.Services.AddScoped<RecipeService>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

var routes = app.MapGroup("")
    .WithParameterValidation()
    .WithOpenApi()
    .WithTags("Recipes");

routes.MapGet("/", async (RecipeService service) =>
{
    return await service.GetRecipes();
})
    .WithSummary("List recipes");

routes.MapPost("/", async (CreateRecipeCommand input, RecipeService service) =>
{
    var id = await service.CreateRecipe(input);
    return Results.CreatedAtRoute("view-recipe", new { id });
})
    .WithSummary("Create recipe")
    .Produces(StatusCodes.Status201Created);

routes.MapGet("/{id}", async (int id, RecipeService service) =>
{
    var recipe = await service.GetRecipeDetail(id);
    return recipe is null
        ? Results.Problem(statusCode: 404)
        : Results.Ok(recipe);
})
    .WithName("view-recipe")
    .WithSummary("Get recipe")
    .ProducesProblem(404)
    .Produces<RecipeDetailViewModel>();

routes.MapDelete("/{id}", async (int id, RecipeService service) =>
{
    await service.DeleteRecipe(id);
    return Results.NoContent();
})
    .WithSummary("Delete recipe")
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
    .ProducesProblem(404)
    .Produces(204);

app.Run();

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

    public Recipe ToRecipe()
    {
        return new Recipe
        {
            Name = Name,
            TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
            Method = Method,
            IsVegetarian = IsVegetarian,
            IsVegan = IsVegan,
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