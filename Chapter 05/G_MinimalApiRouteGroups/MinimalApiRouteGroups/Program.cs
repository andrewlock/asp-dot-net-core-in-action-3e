using System.Collections.Concurrent;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

var _fruit = new ConcurrentDictionary<string, Fruit>();

GroupRouteBuilder fruitApi = app.MapGroup("/fruit");

fruitApi.MapGet("/", () => _fruit);

GroupRouteBuilder fruitApiWithValidation = fruitApi.MapGroup("/");
    //.AddFilter(ValidationHelper.ValidateIdFactory);

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

fruitApiWithValidation.MapDelete("/fruit/{id}", (string id) =>
{
    _fruit.TryRemove(id, out _);
    return Results.NoContent();
});

app.Run();
record Fruit(string Name, int stock);
class ValidationHelper
{
    internal static RouteHandlerFilterDelegate ValidateIdFactory(
        RouteHandlerContext context, RouteHandlerFilterDelegate next)
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

        return async (invocationContext) =>
        {
            if (idPosition.HasValue)
            {
                var id = invocationContext.GetArgument<string>(idPosition.Value);
                if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        { "id", new[] { "Invalid format. Id must start with 'f'" } }
                    });
                }
            }
            return await next(invocationContext);
        };
    }
}