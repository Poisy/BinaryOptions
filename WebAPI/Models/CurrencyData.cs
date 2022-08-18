using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class CurrencyData
    {
        public DateTime Date { get; set; }

        public Dictionary<string, double> Currencies { get; set; }
    }
}