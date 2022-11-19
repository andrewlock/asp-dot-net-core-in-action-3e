namespace ExchangeRates.Web.Tests;

public class StubExchangeRateConverter : ICurrencyConverter
{
    public decimal ConvertToGbp(decimal value, decimal rate, int dps)
    {
        return 3;
    }
}
