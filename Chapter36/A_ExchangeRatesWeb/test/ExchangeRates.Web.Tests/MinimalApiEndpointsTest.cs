using Xunit;

namespace ExchangeRates.Web.Tests;

/// <summary>
/// Unit tests for the <see cref="MinimalApiEndpoints"/> (see section 36.2)
/// </summary>
public class MinimalApiEndpointsTest
{
    [Fact]
    public void Convert_ReturnsValue()
    {
        var result = MinimalApiEndpoints.GetConversion(new()
        {
            Converter = new CurrencyConverter(),
            Value = 1,
            ExchangeRate = 2,
            DecimalPlaces = 2,
        });

        Assert.Equal(200, result.StatusCode);
    }
}
