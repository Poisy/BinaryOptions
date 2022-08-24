using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Validations;
using Infrastructure.Models;

namespace WebAPI.Models
{
    public class BetArg
    {
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Currency need to be exact 6 characters!")]
        public string Currency { get; set; }

        [Required]
        [Range(10, 1000)]
        public int Payout { get; set; }
        
        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [StringRange(AllowableValues = new []{ "higher", "lower" })]
        public string Slope { get; set; }

        [Required]
        [Range(0.1, 0.9)]
        public double PercentageReward { get; set; }

        [Required]
        [GreaterThan(0)]
        public double Barrier { get; set; }



        public Option ToOption(ApplicationUser user, CurrencyPair currencyPair)
        {
            return new Option
            {
                UserId = new Guid(user.Id),
                CurrencyPairId = currencyPair.Id,
                StartDate = DateTime.Now,
                ExpirationDate = ExpirationDate,
                Payout = Payout,
                Slope = Slope == "higher" ? Domain.Entities.Slope.Higher : Domain.Entities.Slope.Lower,
                PercentageReward = PercentageReward,
                Barrier = Barrier,
                IsActive = true
            };
        }
    }
}