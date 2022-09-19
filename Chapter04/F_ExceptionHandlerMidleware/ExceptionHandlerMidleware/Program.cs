var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseExceptionHandler("/error");


app.MapGet("/", () => BadService.GetValues());
app.MapGet("/error", () => "Sorry, there was a problem processing your request");

app.Run();

class BadService
{
    public static string? GetValues()
    {
        // Causes a NullReferenceException when run
        object nullObject = null!;
        return nullObject.ToString();
    }
}