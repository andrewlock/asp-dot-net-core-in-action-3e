var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStatusCodePages();
app.MapGet("/", () => "Try navigating to /missing to see the status code pages in action");

app.Run();
