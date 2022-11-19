using SystemdService.Data;
using SystemdService;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(hostContext.Configuration.GetConnectionString("SqlLiteConnection")));

        services.AddHttpClient<ExchangeRatesClient>();
        services.AddHostedService<ExchangeRatesHostedService>();

    })
    .UseSystemd()
    .Build();

host.Run();
