using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace ExchangeRates.Web.Tests;

/// <summary>
/// Integration tests that use the application defined in ExchangeRates.Web.Web. (See section 36.3.2)
/// </summary>
public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    public IntegrationTests(WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task StatusMiddlewareReturnsPong()
    {
        HttpClient client = _fixture.CreateClient();

        // Act
        var response = await client.GetAsync("/ping");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal("pong", content);
    }

    [Fact]
    public async Task HomePageReturnsHtml()
    {
        HttpClient client = _fixture.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Enter values and click convert", content);
    }

    [Fact]
    public async Task ConvertReturnsExpectedValue()
    {
        var customFactory = _fixture.WithWebHostBuilder(hostBuilder =>
        {
            hostBuilder.ConfigureTestServices(services =>
            {
                services.RemoveAll<ICurrencyConverter>();
                services.AddSingleton<ICurrencyConverter, StubExchangeRateConverter>();
            });
        });

        HttpClient client = customFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/currency");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal("3", content);
    }

    [Fact]
    public async Task MinimalApiConvertReturnsExpectedValue()
    {
        var customFactory = _fixture.WithWebHostBuilder(hostBuilder =>
        {
            hostBuilder.ConfigureTestServices(services =>
            {
                services.RemoveAll<ICurrencyConverter>();
                services.AddSingleton<ICurrencyConverter, StubExchangeRateConverter>();
            });
        });

        HttpClient client = customFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/convert-currency?Value=5&ExchangeRate=0.33&DecimalPlaces=2");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal("3", content);
    }
}
