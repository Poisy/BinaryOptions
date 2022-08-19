using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class CurrencyData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Dictionary<string, OHLCModel> Currencies { get; set; }
    }
}