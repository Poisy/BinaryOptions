using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class CurrencyData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Currency> Currencies { get; set; }

        public CurrencySingleData GetSingleData(string currencyName)
        {
            var currencyModel = Currencies.FirstOrDefault(currency1 => currency1.Name == currencyName);
            
            return new CurrencySingleData
            {
                StartDate = StartDate,
                EndDate = EndDate,
                Name = currencyModel.Name,
                High = currencyModel.OHLC.High,
                Low = currencyModel.OHLC.Low,
                Open = currencyModel.OHLC.Open,
                Close = currencyModel.OHLC.Close
            };
        }
    }
}