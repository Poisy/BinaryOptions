using System;
using Domain.Helpers;

namespace Domain.Entities.Dto
{
    public class OptionReadDto
    {
        public OptionReadDto(Option option, string currencyName, bool didWin, double price)
        {
            Currency = currencyName;
            StartDate = option.StartDate;
            ExpirationDate = option.ExpirationDate;
            Payout = option.Payout;
            Slope = option.Slope == Entities.Slope.Higher ? "higher" : "lower";
            Barrier = option.Barrier;
            PercentageReward = option.PercentageReward;
            IsActive = option.IsActive;
            DidWin = didWin;
            Price = CurrencyHelper.RoundCurrency(price);
        }

        public string Currency { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public float Payout { get; set; }

        public string Slope { get; set; }

        public double Barrier { get; set; }

        public double PercentageReward { get; set; }

        public bool IsActive { get; set; }

        public bool DidWin { get; set; }

        public double Price { get; set; }
    }
}