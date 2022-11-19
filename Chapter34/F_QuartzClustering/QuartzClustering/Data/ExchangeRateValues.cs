using System.ComponentModel.DataAnnotations.Schema;

namespace QuartzClustering;

public class ExchangeRateValues
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ExchangeRateValuesId { get; set; }
    public required string Rate { get; init; }
    public required decimal Value { get; init; }
}
