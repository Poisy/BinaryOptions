using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Dto;
using Domain.Helpers;
using Infrastructure.Repos;

namespace Infrastructure.Services
{
    public class OptionService
    {
        //=============================================================================================
        private readonly IGenericRepository<Option> _optionRepo;
        private readonly IGenericRepository<BidResult> _bidResultRepo;
        private readonly CurrencyService _currencyService;
        private readonly UserService _userService;


        //=============================================================================================
        public OptionService(IGenericRepository<Option> optionRepo, IGenericRepository<BidResult> bidResultRepo,
            CurrencyService currencyService, UserService userService)
        {
            _optionRepo = optionRepo;
            _bidResultRepo = bidResultRepo;
            _currencyService = currencyService;
            _userService = userService;
        }
        

        //=============================================================================================
        public async Task AddOptionAsync(Option option)
        {
            await _optionRepo.AddAsync(option);
            await _optionRepo.CompleteAsync();
        }
        
        
        //=============================================================================================
        public async Task<List<Option>> GetAllOptionsForUser(Guid userId)
            => (await _optionRepo.FindAsync(option => option.UserId == userId))
                .ToList();
        
        
        //=============================================================================================
        public async Task<List<BidResult>> GetAllBidResultsForUser(Guid userId)
            => (await _bidResultRepo.FindAsync(result => result.UserId == userId))
                .ToList();
        
        
        //=============================================================================================
        public async Task<BidResult> GetBidResults(Guid optionId)
            => await _bidResultRepo.FirstOrDefaultAsync(result => result.OptionId == optionId);

        
        //=============================================================================================
        public async Task<List<Option>> GetAllUnexpiredOptionsAsync()
            => (await _optionRepo.FindAsync(option => option.IsActive))
                .ToList();

        
        //=============================================================================================
        public async Task AddBidResultAsync(Option option, CurrencyData data)
        {
            var isWinning = IsOptionWinning(option, data);
            var bidToAdd = CreateBidResult(option, isWinning);
            var user = await _userService.GetByIdAsync(option.UserId);

            var wonPrice = (isWinning ? option.Payout : 0) + option.Payout * option.PercentageReward;
            
            await _bidResultRepo.AddAsync(bidToAdd);
            await _userService.TransferMoneyFromSystemToUserAsync(user, wonPrice);
            
            option.IsActive = false;
            _optionRepo.Update(option.Id, option);
            
            await _bidResultRepo.CompleteAsync();
        }
        
        
        //=============================================================================================
        private bool IsOptionWinning(Option option, CurrencyData data)
        {
            var currency = data.Currencies.First(currency =>
            {
                var currentCurrency = _currencyService.GetAsync(currency.Name).Result;

                return currentCurrency?.Id == option.CurrencyPairId;
            });
            
            var expectedValue = option.Barrier;
            var actualValue = currency.Value;

            return option.Slope switch
            {
                Slope.Higher => expectedValue < actualValue,
                Slope.Lower => expectedValue > actualValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        

        //=============================================================================================
        private static BidResult CreateBidResult(Option option, bool isWinning)
            => new BidResult
            {
                OptionId = option.Id,
                UserId = option.UserId,
                Value = (isWinning ? option.Payout : 0) + option.Payout*option.PercentageReward - option.Payout,
                DidWin = isWinning
            };


        //=============================================================================================
        public async Task BreakOption(Option option)
        {
            var user = await _userService.GetByIdAsync(option.UserId);

            await _userService.TransferMoneyFromSystemToUserAsync(user, option.Payout);
            _optionRepo.Remove(option);

            await _optionRepo.CompleteAsync();
        }
        

        //=============================================================================================
        public static BetPreview GetBetPreview(string currency, LinkedList<CurrencyData> data)
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

                foreach (var stake in new double[] { 0.9, 0.75, 0.5, 0.25, 0.1 })
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