using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace ExchangeRates.Web.Tests;

/// <summary>
/// Integration tests for the <see cref="StatusMiddleware"/> using TestHost. (See section 36.3.1)
/// </summary>
public class StatusMiddlewareTestHostTests
{
    [Fact]
    public async Task StatusMiddlewareReturnsPong()
    {
        var hostBuilder = new HostBuilder()
          .ConfigureWebHost(webHost =>
          {
              webHost.UseTestServer();
              webHost.Configure(app =>
                app.UseMiddleware<StatusMiddleware>()
              );
          });

        IHost host = await hostBuilder.StartAsync();
        HttpClient client = host.GetTestClient();

        // Act
        var response = await client.GetAsync("/ping");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal("pong", content);
    }

    [Fact]
    public async Task StatusMiddlewareReturnsPongWithWebApplicationBuilder()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();

        var app = builder.Build();
        app.UseMiddleware<StatusMiddleware>();

        await app.StartAsync();
        HttpClient client = app.GetTestClient();

        // Act
        var response = await client.GetAsync("/ping");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal("pong", content);
    }
}
