using ConvertingToMVC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ToDoService>();

var app = builder.Build();

app.MapDefaultControllerRoute();

app.Run();
