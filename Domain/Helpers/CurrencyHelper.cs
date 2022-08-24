using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Helpers
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

        public static double RoundCurrency(double currencyValue)
        {
            var precision = currencyValue switch
            {
                > 100 => 2,
                > 10 => 3,
                > 1 => 4,
                _ => 5
            };

            return Math.Round(currencyValue, precision);
        }
    }
}