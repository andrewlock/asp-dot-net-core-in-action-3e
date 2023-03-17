using System.Collections.Concurrent;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

var _fruit = new ConcurrentDictionary<string, Fruit>();

// WithoutRouteGroups();
UsingRouteGroups();

app.Run();

void WithoutRouteGroups()
{
    // Same /fruit prefix 👇
    app.MapGet("/fruit", () => _fruit);

    // Same /fruit prefix 👇
    app.MapGet("/fruit/{id}", (string id) =>
        _fruit.TryGetValue(id, out var fruit)
            ? TypedResults.Ok(fruit)
            : Results.Problem(statusCode: 404))
        .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory); // 👈 Duplicated on every endpoint

    // Same /fruit prefix 👇
    app.MapPost("/fruit/{id}", (Fruit fruit, string id) =>
        _fruit.TryAdd(id, fruit)
            ? TypedResults.Created($"/fruit/{id}", fruit)
            : Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "id", new[] { "A fruit with this id already exists" } }
            }))
        .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory); // 👈 Duplicated on every endpoint

    // Same /fruit prefix 👇
    app.MapPut("/fruit/{id}", (string id, Fruit fruit) =>
    {
        _fruit[id] = fruit;
        return Results.NoContent();
    })
        .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory); // 👈 Duplicated on every endpoint

    // Same /fruit prefix 👇
    app.MapDelete("/fruit/{id}", (string id) =>
    {
        _fruit.TryRemove(id, out _);
        return Results.NoContent();
    })
        .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory); // 👈 Duplicated on every endpoint
}

void UsingRouteGroups()
{
    // Fruit prefix only defined in one place        👇
    RouteGroupBuilder fruitApi = app.MapGroup("/fruit");

    fruitApi.MapGet("/", () => _fruit);

    // Nested group. "/" prefix means still only using "/fruit" prefix 👇
    RouteGroupBuilder fruitApiWithValidation = fruitApi.MapGroup("/")
        .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory); // 👈 Validation filter only declared once

    fruitApiWithValidation.MapGet("/{id}", (string id) =>
        _fruit.TryGetValue(id, out var fruit)
            ? TypedResults.Ok(fruit)
            : Results.Problem(statusCode: 404));

    fruitApiWithValidation.MapPost("/{id}", (Fruit fruit, string id) =>
        _fruit.TryAdd(id, fruit)
            ? TypedResults.Created($"/fruit/{id}", fruit)
            : Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "id", new[] { "A fruit with this id already exists" } }
            }));

    fruitApiWithValidation.MapPut("/{id}", (string id, Fruit fruit) =>
    {
        _fruit[id] = fruit;
        return Results.NoContent();
    });

    fruitApiWithValidation.MapDelete("/{id}", (string id) =>
    {
        _fruit.TryRemove(id, out _);
        return Results.NoContent();
    });
}

record Fruit(string Name, int Stock);
class ValidationHelper
{
    internal static EndpointFilterDelegate ValidateIdFactory(
        EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        ParameterInfo[] parameters = context.MethodInfo.GetParameters();
        int? idPosition = null;
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].Name == "id" &&
                parameters[i].ParameterType == typeof(string))
            {
                idPosition = i;
                break;
            }
        }

        if (!idPosition.HasValue)
        {
            return next;
        }

        return async (invocationContext) =>
        {
            var id = invocationContext.GetArgument<string>(idPosition.Value);
            if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "id", new[] { "Invalid format. Id must start with 'f'" } }
                });
            }
            return await next(invocationContext);
        };
    }
}
