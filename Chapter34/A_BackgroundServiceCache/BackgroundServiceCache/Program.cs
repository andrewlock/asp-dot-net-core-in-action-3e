using Microsoft.Net.Http.Headers;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ExchangeRatesClient>(client =>
{
    client.BaseAddress = new Uri("https://open.er-api.com/v6/");
    client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "ExchangeRateViewer");
})
                .AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));

builder.Services.AddSingleton<ExchangeRatesCache>();
builder.Services.AddHostedService<ExchangeRatesHostedService>();

var app = builder.Build();

app.MapGet("/", (ExchangeRatesClient client) => client.GetLatestRatesAsync());

app.Run();

public class ExchangeRatesHostedService : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ExchangeRatesCache _cache;
    private readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(5);
    private readonly ILogger<ExchangeRatesHostedService> _logger;

    public ExchangeRatesHostedService(
        ExchangeRatesCache cache,
        IServiceProvider provider,
        ILogger<ExchangeRatesHostedService> logger)
    {
        _cache = cache;
        _provider = provider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Fetching latest rates");
            var client = _provider.GetRequiredService<ExchangeRatesClient>();
            var latest = await client.GetLatestRatesAsync();
            _cache.SetRates(latest);
            _logger.LogInformation("Latest rates updated");

            await Task.Delay(_refreshInterval, stoppingToken);
        }
    }
}

public class ExchangeRates
{
    public string base_code { get; set; }
    public string time_last_update_utc { get; set; }
    public Dictionary<string, decimal> rates { get; set; }
}

public class ExchangeRatesCache
{
    private ExchangeRates? _rates;
    public ExchangeRates? GetLatestRates() => _rates;

    public void SetRates(ExchangeRates newRates)
    {
        Interlocked.Exchange(ref _rates, newRates);
    }
}

public class ExchangeRatesClient
{
    private readonly HttpClient _httpClient;
    public ExchangeRatesClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ExchangeRates> GetLatestRatesAsync() 
        => _httpClient.GetFromJsonAsync<ExchangeRates>("latest");
}