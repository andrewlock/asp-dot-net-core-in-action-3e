using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var _fruit = new ConcurrentDictionary<string, Fruit>();

app.MapGet("/fruit/{id}", (string id) =>
    _fruit.TryGetValue(id, out var fruit)
        ? TypedResults.Ok(fruit)
        : Results.Problem(statusCode: 404))
    .WithName("GetFruit")
    .WithTags("fruit")
    .Produces<Fruit>()
    .ProducesProblem(404)
    .WithSummary("Fetches a fruit")
    .WithDescription("Fetches a fruit by id, or returns 404 if no fruit with the ID exists")
    .WithOpenApi(o =>
    {
        o.Parameters[0].Description = "The id of the fruit to fetch";
        //o.Deprecated = true;
        //o.Summary = "Fetches a fruit";
        return o;
    });

app.MapGet("/fruit2/{id}",
    [EndpointName("GetFruit2")]
    [EndpointSummary("Fetches a fruit")]
    [EndpointDescription("Fetches a fruit by id, or returns 404 if no fruit with the ID exists")]
    [ProducesResponseType(typeof(Fruit), 200)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), 404, "application/problem+json")]
    [Tags("fruit")]
    (string id) =>
        _fruit.TryGetValue(id, out var fruit)
            ? TypedResults.Ok(fruit)
            : Results.Problem(statusCode: 404))
    .WithOpenApi(o =>
    {
        o.Parameters[0].Description = "The id of the fruit to fetch";
        //o.Deprecated = true;
        //o.Summary = "Fetches a fruit";
        return o;
    });

var handler = new FruitHandler(_fruit);
app.MapGet("/fruit3/{id}", handler.GetFruit)
    .WithName("GetFruit3");

app.MapPost("/fruit/{id}",
[EndpointSummary("Creates a fruit")]
[EndpointDescription("Creates a new fruit entry with the provided ID. The fruit must not already exist")]
[EndpointName("CreateFruit")]
[ProducesResponseType(typeof(Fruit), 201)]
[ProducesResponseType(typeof(HttpValidationProblemDetails), 400, "application/problem+json")]
[Tags("fruit")]
(string id, Fruit fruit) =>
    _fruit.TryAdd(id, fruit)
        ? TypedResults.Created($"/fruit/{id}", fruit)
        : Results.ValidationProblem(new Dictionary<string, string[]>
          {
              { "id", new[] { "A fruit with this id already exists" } }
        }))
        .WithOpenApi(o =>
        {
            o.Parameters[0].Description = "The id of the fruit to create";
            //o.Deprecated = true;
            //o.Summary = "Creates a fruit";
            return o;
        });


app.Run();
record Fruit(string Name, int Stock);

internal class FruitHandler
{
    private readonly ConcurrentDictionary<string, Fruit> _fruit;
    public FruitHandler(ConcurrentDictionary<string, Fruit> fruit)
    {
        _fruit = fruit;
    }

    /// <summary>
    /// Fetches a fruit by id, or returns 404 if no fruit with the ID exists
    /// </summary>
    /// <param name="id" >The ID of the fruit to fetch</param>
    /// <response code="200">Returns the fruit if it exists</response>
    /// <response code="404">If the fruit doesn't exist</response>
    [ProducesResponseType(typeof(Fruit), 200)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), 404, "application/problem+json")]
    [Tags("fruit")]
    public IResult GetFruit(string id)
        => _fruit.TryGetValue(id, out var fruit)
            ? TypedResults.Ok(fruit)
            : Results.Problem(statusCode: 404);
}
