using System.Collections.Generic;

namespace ExchangeRateViewer
{
    public class ExchangeRates
    {
        public string base_code { get; set; }
        public string time_last_update_utc { get; set; }
        public Dictionary<string, decimal> rates { get; set; }
    }
}
