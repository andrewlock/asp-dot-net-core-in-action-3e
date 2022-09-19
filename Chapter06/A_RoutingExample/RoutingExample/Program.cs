using RoutingExample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductService>();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGet("/", (LinkGenerator links, HttpContext context) => GetHomePage(links, context));

app.MapHealthChecks("/healthz").WithName("healthcheck");

app.MapGet("/test", () => "Hello world!").WithName("hello");

app.MapGet("/{name}", (ProductService service, string name) =>
{
    var product = service.GetProduct(name);
    return product is null
        ? Results.NotFound()
        : Results.Ok(product);
}).WithName("product");

app.MapGet("/redirect-me", () => Results.RedirectToRoute("hello"))
    .WithName("redirect");

app.Run();

static string GetHomePage(LinkGenerator links, HttpContext context)
{

    var healthcheck = links.GetPathByName("healthcheck");
    var helloWorld = links.GetPathByName("hello");
    var redirect = links.GetPathByName("redirect");
    var bigWidget = links.GetPathByName("product", new { Name = "big-widget"});
    var fancyWidget = links.GetUriByName(context, "product", new { Name = "super-fancy-widget"});

    return $@"Try navigating to one of the following paths:
    {healthcheck} (standard health check)
    {helloWorld} (Hello world! response)
    {redirect} (Redirects to the {helloWorld} endpoint)
    {bigWidget} or {fancyWidget} (returns the Product details)
    /not-a-product (returns a 404)";
}