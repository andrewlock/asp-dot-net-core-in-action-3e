var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// In development, this is already added by WebApplication
// Note: You should NEVER do this in Production as it can leak secrets
app.UseDeveloperExceptionPage();
app.MapGet("/", () => BadService.GetValues());

app.Run();

class BadService
{
    public static string? GetValues()
    {
        throw new Exception("Oops, something bad happened!");
    }
}