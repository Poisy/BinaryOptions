using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Helpers;

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
                High = CurrencyHelper.RoundCurrency(currencyModel.OHLC.High),
                Low = CurrencyHelper.RoundCurrency(currencyModel.OHLC.Low),
                Open = CurrencyHelper.RoundCurrency(currencyModel.OHLC.Open),
                Close = CurrencyHelper.RoundCurrency(currencyModel.OHLC.Close),
                Price = CurrencyHelper.RoundCurrency(currencyModel.Value)
            };
        }
    }
}