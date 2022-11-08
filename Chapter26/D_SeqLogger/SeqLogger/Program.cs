var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSeq();

builder.Services.AddSingleton<ValuesService>();

var app = builder.Build();

app.MapGet("/", () => @"View the the two endpoints at /scopes and /values
Check Seq to see the different scopes");


app.MapGet("/scopes", () =>
{
    app.Logger.LogInformation("No, I don’t have scope");

    using (app.Logger.BeginScope("Scope value"))
    using (app.Logger.BeginScope(new Dictionary<string, object> { { "CustomValue1", 12345 } }))
    {
        app.Logger.LogInformation("Yes, I have the scope!");
    }

    app.Logger.LogInformation("No, I lost it again");

    return Results.Ok("Ok");
});

app.MapGet("/values", Handler.GetValues);

app.Run();

public class Handler
{
    public static IEnumerable<string> GetValues(ValuesService service, ILogger<Handler> logger)
    {
        logger.LogInformation("Inside handler, outside scope");

        using (logger.BeginScope("Some scope value"))
        using (logger.BeginScope(123))
        using (logger.BeginScope(new Dictionary<string, object> { { "ScopeValue1", "outer scope" } }))
        {
            logger.LogInformation("Inside controller, inside scope");
            return service.GetValues();
        }
    }
}

public class ValuesService
{
    public readonly ILogger<ValuesService> _logger;
    public ValuesService(ILogger<ValuesService> logger)
    {
        _logger = logger;
    }
    public IEnumerable<string> GetValues()
    {
        _logger.LogInformation("Inside service, outside scope");
        using (_logger.BeginScope(new Dictionary<string, object> { { "ScopeValue2", "inner scope" } }))
        {
            _logger.LogInformation("Inside service, inside scope");
            return new string[] { "value1", "value2" };
        }
    }
}