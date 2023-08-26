using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Optional customization of serialization
// Make sure to update your JSON posts if you change the naming policy
//builder.Services.ConfigureHttpJsonOptions(o =>
//{
//    o.SerializerOptions.AllowTrailingCommas = true;
//    o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//    o.SerializerOptions.PropertyNameCaseInsensitive = true;
//});
//OR
//builder.Services.Configure<JsonOptions>(o =>
//{
//    o.JsonSerializerOptions.AllowTrailingCommas = true;
//    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
//    o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
//});

var app = builder.Build();

app.MapGet("/", () => @"
Try visiting one of the following paths (using a GET request):
/products/123
/products/123?id=456
/products?id=456
/product/p123
/products/search?id=123&id=456
/stock/
/stock/123
/stock2
/stock2?id=123
/stock3
/stock3?id=123
/well-known
/links
/category/123?q=abc&page=3

Use postman to send a GET request to the following, and set the PageSize header:
/products/123/paged?page=2

Use postman to send a POST request to /product and include the following JSON
{ ""id"": 1, ""Name"": ""Shoes"", ""Stock"": 12 }

Use postman to send a POST request to /product/search and include the following JSON
[123, 456]

Use postman to send a POST request to /products and include the following JSON
[{ ""id"": 1, ""Name"": ""Shoes"", ""Stock"": 12 },  { ""id"": 2, ""Name"": ""Socks"", ""Stock"": 3 }]

Use postman to send a POST request to /sizes and include the following plain text
1.234
654.21
");

// id will be bound to the route parameter value.
// The following requests will both have id = 123
// /products/123
// /products/123?id=456
app.MapGet("/products/{id?}", (int? id) => $"Received {id}");

// id will be bound to the querystring
// The following request will both have id = 456
// /products?id=456
app.MapGet("/products", (int id) => $"Received {id}");

// Specifically define where parameters will be bound from
// The following request will have id = 123, page = 2
// page size must be sent as a header value using e.g. PostMan
// /products/123/page?paged=2
app.MapGet("/products/{id}/paged",
    ([FromRoute] int id,
     [FromQuery] int page,
     [FromHeader(Name = "PageSize")] int pageSize)
     => $"Received id {id}, page {page}, pageSize {pageSize}");

// ProductId contains a TryParse method, so it is treated as a simple type
// and is bound to the route parameter
// /product/p123
// /product/p0
// /product/p953
app.MapGet("/product/{id}", (ProductId id) => $"Received productID {id}");

// Product is not a simple type, so it is bound from the body
// Use postman to send a POST request containing the
// following JSON to /product:
// { "id": 1, "Name": "Shoes", "Stock": 12 }
app.MapPost("/product", (Product product) => $"Received {product}");


// Simple types will be bound to multiple querystring values for GET requests
// /products/search?id=123&id=456
app.MapGet("/products/search", ([FromQuery(Name = "id")] int[] ids) => $"Received {ids.Length} ids: {string.Join(", ", ids)}");

// Simple types will be bound from the request body for POST request
// Use postman to send a POST request containing the
// following JSON to /products/search:
// [123, 456]
app.MapPost("/products/search", (int[] ids) => $"Received {ids.Length} ids: {string.Join(", ", ids)}");

// Complex types will be bound from the request body for POST request
// Use postman to send a POST request containing the
// following JSON to /products:
// [{ "id": 1, "Name": "Shoes", "Stock": 12 },  { "id": 2, "Name": "Socks", "Stock": 3 }]
app.MapPost("/products", (Product[] products) => $"Received {products.Length} items: {string.Join(", ", products.Select(x => x))}");

// Use ? for optional parameters.
// id binds to the optional route value. If the value is not provided, id will be null
// /stock
// /stock/123
app.MapGet("/stock/{id?}", (int? id) => $"Received {id}");

// Use ? for optional parameters.
// id binds to the optional query string value. If the value is not provided, id will be null
// /stock2
// /stock2?id=123
app.MapGet("/stock2", (int? id) => $"Received {id}");

// Use ? for optional parameters.
// product binds to the body. If there is no request body, or it contains the value "null"
// then product will be null
// Use postman to send a POST request to /stock:
app.MapPost("/stock", (Product? product) => $"Received {product}");

// Alternatively to optional values, you can use a default values.
// Note that you can't use default values with Lambdas, so using a local function instead
// id binds to the query string value. If the value is not provided, id will be 0
// /stock3
// /stock3?id=123
string StockWithDefaultValue(int id = 0) => $"Received {id}";

app.MapGet("/stock3", StockWithDefaultValue);

// Accessing well-known types
app.MapGet("/well-known", (HttpContext httpContext) => httpContext.Response.WriteAsync("Hello World!"));

// Using services
// The LinkGenerator is registered in the DI container so it can be used as a parameter
app.MapGet("/links", ([FromServices] LinkGenerator links) => $"The Links API can be found at {links.GetPathByName("LinksApi")}")
    .WithName("LinksApi");

// Using custom binding with BindAsync
// Send a post request to PostMan at /sizes with a body like:
// 1.234
// 2.3455
app.MapPost("/sizes", (SizeDetails size) => $"Received {size}");

// Using [AsParameters]
app.MapGet("/category/{id}", ([AsParameters] SearchModel model) => $"Received {model}");

app.Run();

record Product(int Id, string Name, int Stock);

readonly record struct ProductId(int Id)
    : IParsable<ProductId>
{
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out ProductId result)
    {
        if (s is not null
            && s.StartsWith('p')
            && int.TryParse(s.AsSpan().Slice(1), out var id))
        {
            result = new ProductId(id);
            return true;
        }

        result = default;
        return false;
    }

    static ProductId IParsable<ProductId>.Parse(string s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw new InvalidOperationException($"Cannot convert '{s}' to type {nameof(ProductId)}");
    }
}

record SizeDetails(double Height, double Width)
{
    public static async ValueTask<SizeDetails?> BindAsync(HttpContext context)
    {
        using var sr = new StreamReader(context.Request.Body);

        var line1 = await sr.ReadLineAsync(context.RequestAborted);
        if (line1 is null)
        {
            return null;
        }
        var line2 = await sr.ReadLineAsync(context.RequestAborted);
        if (line2 is null)
        {
            return null;
        }

        return double.TryParse(line1, out var height)
            && double.TryParse(line2, out var width)
            ? new SizeDetails(height, width)
            : null;
    }
}

record struct SearchModel(
    int Id,
    int Page,
    [FromHeader(Name = "sort")] bool? SortAsc,
    [FromQuery(Name = "q")] string Search);