using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models
{
    public class ExchangeRatesApiPage
    {
        [JsonProperty("rates")]
        public CurrencyCoefItem Rates { get; set; }
    }

    public class CurrencyCoefItem
    {
        [JsonProperty("BYN")]
        public float? BYN { get; set; }
        [JsonProperty("USD")]
        public float? USD { get; set; }
        [JsonProperty("RUB")]
        public float? RUB { get; set; }
    }
}
