using Microsoft.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace SystemdService;

public class ExchangeRatesClient
{
    private readonly HttpClient _httpClient;
    public ExchangeRatesClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://open.er-api.com/v6/");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "ExchangeRateViewer");
    }

    public async Task<ExchangeRates> GetLatestRatesAsync()
    {
        var rates = await _httpClient.GetFromJsonAsync<ExchangeRateDto>("latest");
        return rates!.ToRates();
    }

    public class ExchangeRateDto
    {
        [JsonPropertyName("base_code")]
        public required string Base { get; init; }

        [JsonPropertyName("time_last_update_utc")]
        public required string Date { get; init; }
        
        [JsonPropertyName("rates")]
        public required Dictionary<string, decimal> Rates { get; init; }

        public ExchangeRates ToRates()
        {
            return new ExchangeRates
            {
                Base = this.Base,
                Date = this.Date,
                Rates = this.Rates
                    .Select(pair => new ExchangeRateValues
                    {
                        Rate = pair.Key,
                        Value = pair.Value,
                    })
                    .ToList(),
            };
        }
    }
}
