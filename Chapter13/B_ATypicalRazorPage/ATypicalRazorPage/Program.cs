using ATypicalRazorPage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ToDoService>();

var app = builder.Build();

app.MapRazorPages();

app.Run();
