var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowShoppingApp", policy =>
        policy.WithOrigins("https://localhost:6333") //shopping.com
            .AllowAnyMethod()
    );
});

var app = builder.Build();

app.UseCors(); // Add CORS without a default policy. Define the policy using RequireCors("AllowShoppingApp") instead
// app.UseCors("AllowShoppingApp"); // Applies the default policy to all endpoints.

app.MapGet("/api/products", () => new string[]
{
    "Aunt Hattie's",
    "Danish",
    "Cobblestone",
    "Dave's Killer Bread",
    "Entenmann's",
    "Famous Amos",
    "Home Pride",
    "Hovis",
    "Keebler Company",
    "Kits",
    "McVitie's",
    "Mother's Pride",
})
.RequireCors("AllowShoppingApp"); // Applies the policy and combines it with the default policy, if specified


app.Run();
