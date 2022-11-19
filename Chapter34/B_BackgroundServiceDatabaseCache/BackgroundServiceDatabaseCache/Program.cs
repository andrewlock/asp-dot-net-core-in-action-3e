using BackgroundServiceDatabaseCache;
using BackgroundServiceDatabaseCache.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqlLiteConnection")));

builder.Services.AddHttpClient<ExchangeRatesClient>();
builder.Services.AddHostedService<ExchangeRatesHostedService>();

var app = builder.Build();

app.MapGet("/", (AppDbContext context)
    => context.ExchangeRates
            .Include(rates => rates.Rates)
            .OrderByDescending(rates => rates.ExchangeRatesId)
            .FirstOrDefaultAsync());

app.Run();
