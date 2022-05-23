using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(opts 
    => opts.LoggingFields = HttpLoggingFields.RequestProperties);

// The following filter is already configured in appsettings.Development.json
// builder.Logging.AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information);

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseHttpLogging();
}

app.MapGet("/", () => "Hello World!");
app.MapGet("/person", () => new Person("Andrew", "Lock"));

app.Run();

public record Person(string FirstName, string LastName);
