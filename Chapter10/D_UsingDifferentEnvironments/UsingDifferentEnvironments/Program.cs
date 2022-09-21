using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MyValues>(
    builder.Configuration.GetSection(nameof(MyValues)));

builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.WriteIndented = true);

var app = builder.Build();

app.MapGet("/", (IOptions<MyValues> opts) => opts.Value);

app.Run();

public class MyValues
{
    public string SingleValue { get; set; }
    public List<string> List { get; set; }
}