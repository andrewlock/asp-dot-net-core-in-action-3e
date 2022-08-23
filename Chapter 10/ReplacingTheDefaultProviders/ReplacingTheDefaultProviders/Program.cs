var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", optional: true);

builder.Services.ConfigureRouteHandlerJsonOptions(o => o.SerializerOptions.WriteIndented = true);

var app = builder.Build();

app.MapGet("/", (IConfiguration config) => config.AsEnumerable());

app.Run();
