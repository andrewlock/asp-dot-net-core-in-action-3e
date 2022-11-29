namespace RecipeApplication.Data;

public class Recipe
{
    public int RecipeId { get; set; }
    public required string Name { get; set; }
    public TimeSpan TimeToCook { get; set; }
    public bool IsDeleted { get; set; }
    public required string Method { get; set; }
    public bool IsVegetarian { get; set; }
    public bool IsVegan { get; set; }
    public DateTimeOffset LastModified { get; set; }


    public required ICollection<Ingredient> Ingredients { get; set; }
}
