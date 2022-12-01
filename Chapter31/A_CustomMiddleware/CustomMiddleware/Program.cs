using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();

var app = builder.Build();

// Uncomment to always return the time as plain text
//app.Run(async (HttpContext context) =>
//{
//    context.Response.ContentType = "text/plain";
//    await context.Response.WriteAsync(DateTime.UtcNow.ToString());
//});

app.UseExceptionHandler();
app.UseMiddleware<SecurityTxtHandler>();

// Responds to /ping1
app.Map("/ping1", (IApplicationBuilder branch) =>
{
    branch.Run(async ctx =>
    {
        ctx.Response.ContentType = "text/plain";
        await ctx.Response.WriteAsync("pong");
    });
});

// Responds to /ping2
app.Use(async (HttpContext context, Func<Task> next) =>
{
    if (context.Request.Path.StartsWithSegments("/ping2"))
    {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("pong2");
    }
    else
    {
        await next();
    }
});

app.Map("/branch", branch =>
{
    // Responds to /branch/ping
    branch.UsePingMiddleware();

    // Adds the X-Content-Type-Options:nosniff header
    branch.UseMiddleware<HeadersMiddleware>();

    // Responds to /branch
    branch.UseMiddleware<VersionMiddleware>();
});


// Endpoints 
app.UseRouting();
app.UseAuthorization();

app.MapFallback(() => """
    Try one of the following Map/Run/Use routes:
    /ping1
    /ping2
    /branch (check the response headers for the the X-Content-Type-Options:nosniff header)
    /branch/ping
    /.well-known/security.txt
    
    Or try one of the following endpoint routes:
    /hello
    /version
    /ping
    /pip-pip
    /add/1/2
    /healthz
    """);

app.MapGet("/hello", () => "Hello World!");

//app.MapRazorPages(); // for example

// See EndpointRouteBuilderExtensions for the definition of these methods
app.MapVersion("/version");
app.MapPingPong("/ping");

// Build the pipeline in-line
var endpoint = ((IEndpointRouteBuilder)app)
    .CreateApplicationBuilder()
    .UseMiddleware<PingPongMiddleware>()
    .Build();

app.Map("/pip-pip", endpoint);

// Example of using route parameters in endpoint routes
app.MapMiddlewareAsEndpoint<CalculatorMiddleware>("/add/{a}/{b}")
    .WithDisplayName("Calculator");

// Requires authorization (must be logged in)
app.MapHealthChecks("/healthz")
    .RequireAuthorization();

app.Run();

public class HeadersMiddleware
{
    private readonly RequestDelegate _next;
    const int MaxAgeInSeconds = 60;
    public HeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            return Task.CompletedTask;
        });

        await _next(context);
    }
}
public class VersionMiddleware
{
    static readonly string? _version = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly()!.Location)?.FileVersion;
    public VersionMiddleware(RequestDelegate next) { }

    public async Task Invoke(HttpContext context)
    {
        await context.Response.WriteAsJsonAsync(new { Version = _version });
    }
}

public class PingPongMiddleware
{
    public PingPongMiddleware(RequestDelegate next)
    {
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("pong");
    }
}

public class CalculatorMiddleware
{
    public CalculatorMiddleware(RequestDelegate next)
    {
    }

    public async Task Invoke(HttpContext context)
    {
        // see https://andrewlock.net/accessing-route-values-in-endpoint-middleware-in-aspnetcore-3/ for 
        // a more robust solution
        string aValue = (string)context.GetRouteValue("a")!;
        string bValue = (string)context.GetRouteValue("b")!;

        var a = int.Parse(aValue);
        var b = int.Parse(bValue);

        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync($"{a} + {b} = {a + b}");
    }
}

public class SecurityTxtHandler
{
    private readonly RequestDelegate _next;
    public SecurityTxtHandler(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;
        if(path.StartsWithSegments("/.well-known/security.txt"))
        {
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync("Contact: mailto:security@example.com");
        }

        return _next.Invoke(context);
    }
}

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UsePingMiddleware(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/ping"))
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("pong");
            }
            else
            {
                await next.Invoke();
            }
        });
    }
}

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointConventionBuilder MapVersion(this IEndpointRouteBuilder endpoints, string pattern)
    {
        var pipeline = endpoints.CreateApplicationBuilder()
            .UseMiddleware<VersionMiddleware>()
            .Build();

        return endpoints.Map(pattern, pipeline)
            .WithDisplayName("Version number");
    }

    public static IEndpointConventionBuilder MapPingPong(this IEndpointRouteBuilder endpoints, string pattern)
    {
        var pipeline = endpoints.CreateApplicationBuilder()
            .UseMiddleware<PingPongMiddleware>()
            .Build();

        return endpoints
            .Map(pattern, pipeline)
            .WithDisplayName("Ping-pong");
    }

    public static IEndpointConventionBuilder MapMiddlewareAsEndpoint<T>(this IEndpointRouteBuilder endpoints, string pattern)
    {
        var pipeline = endpoints.CreateApplicationBuilder()
            .UseMiddleware<T>()
            .Build();

        return endpoints.Map(pattern, pipeline);
    }
}