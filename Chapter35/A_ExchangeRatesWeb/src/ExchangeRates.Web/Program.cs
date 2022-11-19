using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSingleton<ICurrencyConverter, CurrencyConverter>();
builder.Services.AddOptions<CurrencySettings>().BindConfiguration("CurrencySettings");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<StatusMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapCurrencyEndpoints();

app.Run();

public static class MinimalApiEndpoints
{
    public static RouteGroupBuilder MapCurrencyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api")
            .WithParameterValidation();

        group.MapGet("/convert-currency", GetConversion).WithName("GetCurrencyConversion");
        return group;
    }

    public static Ok<decimal> GetConversion([AsParameters] InputModel input)
    {
        return TypedResults.Ok(input.Converter.ConvertToGbp(
            input.Value,
            input.ExchangeRate,
            input.DecimalPlaces));
    }

    public class InputModel
    {
        public ICurrencyConverter Converter { get; set; } = null!;

        [DisplayName("Value in GBP")]
        public decimal Value { get; set; } = 0;

        [DisplayName("Exchange rate from GBP to alternate currency")]
        [Range(0, double.MaxValue)]
        public decimal ExchangeRate { get; set; }

        [DisplayName("Round to decimal places")]
        [Range(0, int.MaxValue)]
        public int DecimalPlaces { get; set; }
    }
}

public class CurrencySettings
{
    public decimal DefaultValue { get; set; }
    public decimal DefaultExchangeRate { get; set; }
    public int DefaultDecimalPlaces { get; set; }
}

public interface ICurrencyConverter
{
    decimal ConvertToGbp(decimal value, decimal exchangeRate, int decimalPlaces);
}

public class CurrencyConverter : ICurrencyConverter
{
    /// <summary>
    /// Converts a value in one currency to GBP
    /// </summary>
    /// <param name="value">The value in the other currency</param>
    /// <param name="exchangeRate">The exchange rate from GBP to the currency. e.g. 1 GBP = <paramref name="exchangeRate"/>USD  </param>
    /// <param name="decimalPlaces"></param>
    /// <returns></returns>
    public decimal ConvertToGbp(decimal value, decimal exchangeRate, int decimalPlaces)
    {
        if (exchangeRate <= 0)
        {
            throw new ArgumentException("Exchange rate must be greater than zero", nameof(exchangeRate));
        }

        if (decimalPlaces < 0)
        {
            throw new ArgumentException("Decimal places must not be less than zero", nameof(decimalPlaces));
        }

        var valueInGbp = value / exchangeRate;

        return decimal.Round(valueInGbp, decimalPlaces);
    }
}

public class StatusMiddleware
{
    private readonly RequestDelegate _next;
    public StatusMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/ping"))
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("pong");
            return;
        }
        await _next(context);
    }
}
