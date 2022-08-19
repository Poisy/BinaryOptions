using System;
using System.Linq;
using Domain.Entities;

namespace Infrastructure.Seeds
{
    public class CurrencyDataGenerator
    {
        private readonly Random _rnd;
        
        public CurrencyDataGenerator()
        {
            _rnd = new Random();
        }
        
        public CurrencyData GenerateCurrencyData(CurrencyData previousData, int interval)
        {
            var newCurrencyData = new CurrencyData
            {
                StartDate = previousData.EndDate,
                EndDate = previousData.EndDate.AddSeconds(interval)
            };

            newCurrencyData.Currencies = previousData.Currencies.Select(currency =>
            {
                double minRange = currency.OHLC.Low - 0.03 * currency.OHLC.Low;
                double maxRange = currency.OHLC.High + 0.03 * currency.OHLC.High;

                var randomNumbers = new []
                {
                    GetRandomNumber(minRange, maxRange),
                    GetRandomNumber(minRange, maxRange)
                };
                
                return new Currency
                {
                    Name = currency.Name,
                    OHLC = new OHLCModel
                    {
                        Open = currency.OHLC.Close,
                        Close = randomNumbers.Average(),
                        High = randomNumbers.Max(),
                        Low = randomNumbers.Min()
                    },
                    Value = GetRandomNumber(minRange, maxRange)
                };
            }).ToList();

            return newCurrencyData;
        }
        
        private double GetRandomNumber(double minimum, double maximum)
            => _rnd.NextDouble() * (maximum - minimum) + minimum;
    }
}