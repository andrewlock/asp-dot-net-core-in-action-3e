var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("extrasettings.json", optional: false, reloadOnChange: true);

// displaying json for clarity
builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.WriteIndented = true);

var app = builder.Build();

app.MapGet("/", (IConfiguration config) => config.AsEnumerable());

app.Run();
