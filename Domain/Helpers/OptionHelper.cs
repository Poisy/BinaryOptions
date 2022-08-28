using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Dto;

namespace Domain.Helpers
{
    public static class OptionHelper
    {
        public static List<OptionReadDto> ToReadDtos(this IEnumerable<Option> options, List<CurrencyPair> currencyPairs, List<BidResult> bidResults)
        {
            return options.Select(option => new OptionReadDto(
                option,
                currencyPairs.FirstOrDefault(currency => currency.Id == option.CurrencyPairId)?.Name,
                bidResults.FirstOrDefault(result => result.OptionId == option.Id)?.DidWin ?? false,
                bidResults.FirstOrDefault(result => result.OptionId == option.Id)?.Value ?? 0
            )).ToList();
        }
    }
}