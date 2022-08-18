using System.Linq;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public static class CurrencyHelper
    {
        //=============================================================================================
        public static CurrencyData FilterCurrencyData(CurrencyData currencyData, string[] availableCurrencies, 
            string baseCurrency, string[] currencies)
        {
            var wantedCurrencies = currencyData.Currencies;
            var baseCurrencyValue = currencyData.Currencies[baseCurrency];

            if (currencies.Length > 0 
                && currencies.All(c => availableCurrencies.Contains(c)))
            {
                wantedCurrencies = wantedCurrencies.Where(c => currencies.Contains(c.Key))
                    .ToDictionary(c => c.Key, c => c.Value);
            }
                
            foreach (var currencyPair in wantedCurrencies)
            {
                wantedCurrencies[currencyPair.Key] = currencyPair.Value / baseCurrencyValue;
            }
                
            return new CurrencyData
            {
                Date = currencyData.Date,
                Currencies = wantedCurrencies
            };
        }
    }
}