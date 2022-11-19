using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RecipeApplication.Data;
using RecipeApplication.Models;

namespace RecipeApplication.Tests;

public class RecipeServiceTests
{
    [Fact]
    public async Task CreateRecipe_InsertsNewRecipe()
    {
        const string recipeName = "Test Recipe";
        const string method = "TBD";
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        // Run the test against one instance of the context
        using (var context = new AppDbContext(options))
        {
            context.Database.EnsureCreated();
            var service = new RecipeService(context);
            var cmd = new CreateRecipeCommand
            {
                Name = recipeName,
                Method = method,
            };
            var user = new ApplicationUser
            {
                Id = 123.ToString()
            };
            await service.CreateRecipe(cmd, user);
        }

        // Use a separate instance of the context to verify correct data was saved to database
        using (var context = new AppDbContext(options))
        {
            Assert.Equal(1, await context.Recipes.CountAsync());

            var recipe = await context.Recipes.SingleAsync();
            Assert.Equal(recipeName, recipe.Name);
            Assert.Equal(method, recipe.Method);
        }
    }

    [Fact]
    public async Task GetRecipeDetails_CanLoadFromContextAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        // Insert seed data into the database using one instance of the context
        using (var context = new AppDbContext(options))
        {
            context.Database.EnsureCreated();
            var userId = "123";
            context.Add(new ApplicationUser { Id = userId });
            context.Recipes.AddRange(
                new Recipe { RecipeId = 1, Name = "Recipe1", Method = "TBD", CreatedById = userId },
                new Recipe { RecipeId = 2, Name = "Recipe2", Method = "TBD", CreatedById = userId },
                new Recipe { RecipeId = 3, Name = "Recipe3", Method = "TBD", CreatedById = userId });
            await context.SaveChangesAsync();
        }

        // Use a separate instance of the context to verify correct data was saved to database
        using (var context = new AppDbContext(options))
        {
            var service = new RecipeService(context);

            var recipe = await service.GetRecipeDetail(2);

            Assert.NotNull(recipe);
            Assert.Equal(2, recipe.Id);
            Assert.Equal("Recipe2", recipe.Name);
            Assert.Equal("TBD", recipe.Method);
        }
    }

    [Fact]
    public async Task GetRecipeDetails_DoesNotLoadDeletedRecipeAsync()
    {
        const string recipeName = "Test Recipe";
        const int recipeId = 2;

        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        // Insert seed data into the database using one instance of the context
        using (var context = new AppDbContext(options))
        {
            context.Database.EnsureCreated();
            var userId = "123";
            context.Add(new ApplicationUser { Id = userId });
            context.Recipes.Add(new Recipe { RecipeId = 1, Name = "Recipe1", CreatedById = userId, Method = "TBD" });
            context.Recipes.Add(new Recipe { RecipeId = 2, Name = recipeName, CreatedById = userId, Method = "TBD", IsDeleted = true });
            context.Recipes.Add(new Recipe { RecipeId = 3, Name = "Recipe3", CreatedById = userId, Method = "TBD" });
            await context.SaveChangesAsync();
        }

        // Use a separate instance of the context to verify correct data was saved to database
        using (var context = new AppDbContext(options))
        {
            var service = new RecipeService(context);

            var recipe = await service.GetRecipeDetail(recipeId);

            Assert.Null(recipe);
        }
    }
}
