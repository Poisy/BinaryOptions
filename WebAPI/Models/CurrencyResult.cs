using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class CurrencyResult
    {
        [JsonPropertyName("end_date")]
        public string EndDate { get; set; }
        
        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        public Dictionary<DateTime, Dictionary<string, OHLCModel>> Price { get; set; }
    }
}