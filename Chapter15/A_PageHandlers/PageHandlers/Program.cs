using PageHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<SearchService>();

var app = builder.Build();

app.MapRazorPages();

app.Run();
