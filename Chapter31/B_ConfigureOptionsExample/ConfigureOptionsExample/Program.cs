using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// json layout
builder.Services.ConfigureHttpJsonOptions(x => x.SerializerOptions.WriteIndented = true);

// Configure using the builder approach
builder.Services.AddOptions<CurrencyOptions>()
    .BindConfiguration("Currencies")
    .Configure(x => x.Currencies = new string[] { "GBP", "USD", "EUR" })
    .Configure<ILocalisationOptionsProvider>((opts, service) => {
        opts.Currencies = service.GetCurrencies();
    });

// Configure using alternative approach
builder.Services.Configure<LanguageOptions>(builder.Configuration.GetSection("Languages"));
builder.Services.Configure<LanguageOptions>(options =>
    options.Languages = new string[] { "English", "French", "Spanish" });
builder.Services.AddSingleton<IConfigureOptions<LanguageOptions>, ConfigureLanguageOptions>();

builder.Services.AddSingleton<ILocalisationOptionsProvider, CurrencyProvider>();

var app = builder.Build();

app.MapGet("/", (IOptions<CurrencyOptions> currencies, IOptions<LanguageOptions> languages)
    => new { currencies = currencies.Value, languages = languages.Value });

app.Run();

public class CurrencyOptions
{
    public string[]? Currencies { get; set; }
    public string? DefaultCurrency { get; set; }
}

public class LanguageOptions
{
    public string[]? Languages { get; set; }
    public string? DefaultLanguage { get; set; }
}

public interface ILocalisationOptionsProvider
{
    string[] GetCurrencies();
    string[] GetLanguages();
}

public class ConfigureLanguageOptions: IConfigureOptions<LanguageOptions>
{
    private readonly ILocalisationOptionsProvider _provider;
    public ConfigureLanguageOptions(ILocalisationOptionsProvider provider)
    {
        _provider = provider;
    }

    public void Configure(LanguageOptions options)
    {
        options.Languages = _provider.GetLanguages();
    }
}

public class CurrencyProvider : ILocalisationOptionsProvider
{
    public string[] GetCurrencies()
    {
        // Load the settings from a database / remote API for example
        return new string[] { "GBP", "USD", "EUR", "CAD" };
    }
    public string[] GetLanguages()
    {
        // Load the settings from a database / remote API for example
        return new string[] { "English", "French", "Spanish", "German" };
    }
}