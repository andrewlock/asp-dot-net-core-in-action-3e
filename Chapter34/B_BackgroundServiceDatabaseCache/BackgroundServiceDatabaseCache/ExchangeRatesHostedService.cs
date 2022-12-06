using BackgroundServiceDatabaseCache.Data;

namespace BackgroundServiceDatabaseCache;

public class ExchangeRatesHostedService : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(5);
    private readonly ILogger<ExchangeRatesHostedService> _logger;

    public ExchangeRatesHostedService(
        IServiceProvider provider,
        ILogger<ExchangeRatesHostedService> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting update loop");
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_refreshInterval, stoppingToken);
            await TryUpdateRatesAsync();
        }
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        // block until we successfully update rates
        _logger.LogInformation("Updating initial rates");
        var success = false;
        while(!success && !cancellationToken.IsCancellationRequested)
        {
            success = await TryUpdateRatesAsync();
        }

        await base.StartAsync(cancellationToken);
    }

    private async Task<bool> TryUpdateRatesAsync()
    {
        try
        {
            using var scope = _provider.CreateScope();
            _logger.LogInformation("Fetching latest rates");
            var client = _provider.GetRequiredService<ExchangeRatesClient>();
            var latestRates = await client.GetLatestRatesAsync();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Add(latestRates);
            await context.SaveChangesAsync();
            _logger.LogInformation("Latest rates updated");
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error updating rates");
            return false;
        }
    }
}