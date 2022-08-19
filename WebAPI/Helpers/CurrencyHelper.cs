using System.Collections.Generic;
using Domain.Entities;

namespace WebAPI.Helpers
{
    public static class CurrencyHelper
    {
        public static IEnumerable<Currency> CreateInitialCurrencies(this Dictionary<string, double> currencies)
        {
            foreach (var currency in currencies)
            {
                yield return new Currency
                {
                    Name = currency.Key,
                    Value = currency.Value,
                    OHLC = new OHLCModel
                    {
                        Open = currency.Value,
                        Close = currency.Value,
                        High = currency.Value,
                        Low = currency.Value
                    }
                };
            }
        }
    }
}