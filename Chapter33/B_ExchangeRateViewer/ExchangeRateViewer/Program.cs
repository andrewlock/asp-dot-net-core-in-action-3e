using ExchangeRateViewer;
using Microsoft.Net.Http.Headers;
using Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddHttpClient("rates", (HttpClient client) =>
{
    client.BaseAddress = new Uri("https://open.er-api.com/v6/");
    client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "ExchangeRateViewer");
})
.ConfigureHttpClient((IServiceProvider provider, HttpClient client) => { }); // additional configuration

builder.Services.AddHttpClient<ExchangeRatesClient>()
    .AddHttpMessageHandler<ApiKeyMessageHandler>()
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(new[] {
                        TimeSpan.FromMilliseconds(200),
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5)
        })
);

builder.Services.AddTransient<ApiKeyMessageHandler>();
builder.Services.AddOptions<ExchangeRateApiSettings>().BindConfiguration("ExchangeRateApiSettings");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

HttpClient staticClient = new HttpClient
{
    BaseAddress = new Uri("https://open.er-api.com/v6/"),
};
staticClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "ExchangeRateViewer");

app.MapGet("/httpclient", [EndpointSummary("Send requests using a singleton HttpClient instance")] async () =>
{
    var result = await staticClient.GetAsync("latest");
    result.EnsureSuccessStatusCode();

    // Return results as json.
    return TypedResults.Stream(await result.Content.ReadAsStreamAsync(), "application/json");
});

app.MapGet("/httpclientfactory", [EndpointSummary("Send requests using IHttpClientFactory")] async (IHttpClientFactory clientFactory) =>
{
    var httpClient = clientFactory.CreateClient();

    httpClient.BaseAddress = new Uri("https://open.er-api.com/v6/");
    httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "ExchangeRateViewer");

    var result = await httpClient.GetAsync("latest");
    result.EnsureSuccessStatusCode();
    return TypedResults.Stream(await result.Content.ReadAsStreamAsync(), "application/json");
});

app.MapGet("/namedclient", [EndpointSummary("Send requests using named client")] async (IHttpClientFactory clientFactory) =>
{
    var httpClient = clientFactory.CreateClient("rates");

    var result = await httpClient.GetAsync("latest");
    result.EnsureSuccessStatusCode();
    return TypedResults.Stream(await result.Content.ReadAsStreamAsync(), "application/json");
});

app.MapGet("/typedclient", [EndpointSummary("Send requests using typed client")] async (ExchangeRatesClient client)
       => await client.GetLatestRatesAsync());

app.Run();

