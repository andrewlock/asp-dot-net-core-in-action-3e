using WindowsService.Data;
using WindowsService;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(hostContext.Configuration.GetConnectionString("SqlLiteConnection")));

        services.AddHttpClient<ExchangeRatesClient>();
        services.AddHostedService<ExchangeRatesHostedService>();

    })
    .UseWindowsService()
    .Build();

host.Run();
