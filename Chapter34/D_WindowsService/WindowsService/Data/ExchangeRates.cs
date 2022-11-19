using System.ComponentModel.DataAnnotations.Schema;

namespace WindowsService;

public class ExchangeRates
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ExchangeRatesId { get; set; }
    public required string Base { get; init; }
    public required string Date { get; init; }
    public required ICollection<ExchangeRateValues> Rates { get; init; }
}
