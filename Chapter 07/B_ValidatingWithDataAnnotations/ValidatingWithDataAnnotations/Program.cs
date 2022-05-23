using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/users", (CreateUserModel user) => user.ToString())
    .WithParameterValidation();

// Note that this does NOT work
//app.MapGet("/user/{id}", ([Range(0, 10)] int id) => id.ToString())
//    .WithParameterValidation();

app.MapGet("/user1/{id}", ([AsParameters] GetUserModel model) => model.Id.ToString())
    .WithParameterValidation();

app.MapGet("/user2/{id}", ([AsParameters] GetUserModel2 model) => model.Id.ToString())
    .WithParameterValidation();

app.MapGet("/user3/{id}", (UserId id) => id.ToString())
    .WithParameterValidation();

app.Run();

public record CreateUserModel
{
    [Required]
    [StringLength(100)]
    [Display(Name = "Your name")]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Last name")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }
}

struct GetUserModel
{
    [Range(1, 10)] 
    public int Id { get; set; }
}
record struct GetUserModel2([property: Range(1, 10)]int Id);

readonly record struct UserId([property: Range(1, 10)] int Id)
{
    public static bool TryParse(string? s, out UserId result)
    {
        if (int.TryParse(s, out var id))
        {
            result = new UserId(id);
            return true;
        }

        result = default;    
        return false;     
    }
}
