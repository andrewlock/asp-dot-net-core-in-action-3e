var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseExceptionHandler("/error");

// normally this would only be used in Production, and you'd rely on the DeveloperExceptionPage middleware
// that's automatically added by WebApplication in Development
// e.g.:
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

app.MapGet("/", () => BadService.GetValues());
app.MapGet("/error", () => "Sorry, there was a problem processing your request");

app.Run();

class BadService
{
    public static string? GetValues()
    {
        throw new Exception("Oops, something bad happened!");
    }
}