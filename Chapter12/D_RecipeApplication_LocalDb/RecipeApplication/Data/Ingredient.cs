using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

public class Ingredient
{
    public int IngredientId { get; set; }
    public int RecipeId { get; set; }
    public required string Name { get; set; }
    [Column(TypeName = "decimal(18,2)")] // SQL Server complains if you don't set a precision for decimal quantities
    public decimal Quantity { get; set; }
    public required string Unit { get; set; }
}
