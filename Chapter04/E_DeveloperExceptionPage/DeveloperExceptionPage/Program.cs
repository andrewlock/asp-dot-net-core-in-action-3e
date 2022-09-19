var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDeveloperExceptionPage(); // In development, this is already added by WebApplication
app.MapGet("/", () => BadService.GetValues());

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