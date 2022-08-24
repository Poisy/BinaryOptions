using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Helpers;
using Infrastructure.Repos;

namespace Infrastructure.Services
{
    public class OptionService
    {
        //=============================================================================================
        private readonly IGenericRepository<Option> _optionRepo;

        
        //=============================================================================================
        public OptionService(IGenericRepository<Option> optionRepo)
        {
            _optionRepo = optionRepo;
        }

        
        //=============================================================================================
        public async Task AddOptionAsync(Option option)
        {
            await _optionRepo.AddAsync(option);
            await _optionRepo.CompleteAsync();
        }
        
        //=============================================================================================
        
        
        
        //=============================================================================================
        public BetPreview GetBetPreview(string currency, LinkedList<CurrencyData> data)
        {
            var lastData = data.Last.Value.GetSingleData(currency);
            
            var expirationDates = new[]
            {
                DateTime.Now.AddMinutes(10),
                DateTime.Now.AddMinutes(30),
                DateTime.Now.AddMinutes(60)
            };
            
            var result = new BetPreview
            {
                Currency = currency,
                Options = new List<BetPreview.Option>()
            };

            var moveRate = lastData.Price - 0.999 * lastData.Price;
            
            foreach (var dateTime in expirationDates)
            {
                var options = new List<BetPreview.Option.OptionInfo>();
                var barrier = moveRate * 2;

                foreach (var stake in new double[] { 0.1, 0.25, 0.5, 0.75, 0.9 })
                {
                   options.Add(new BetPreview.Option.OptionInfo
                   {
                       Barrier = CurrencyHelper.RoundCurrency(lastData.Price + barrier),
                       PercentageRewards = new BetPreview.Option.OptionInfo.PercentageReward
                       {
                           Higher = stake,
                           Lower =  Math.Round(1-stake, 2) 
                       }
                   });

                   barrier -= moveRate;
                }
                
                result.Options.Add(new BetPreview.Option
                {
                    ExpirationDate = dateTime,
                    OptionInfos = options
                });
            }
            
            
            return result;
        }
    }
}