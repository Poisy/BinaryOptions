using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Dto;

namespace Domain.Helpers
{
    public static class OptionHelper
    {
        public static List<OptionReadDto> ToReadDtos(this IEnumerable<Option> options, List<CurrencyPair> currencyPairs)
        {
            return options.Select(option => new OptionReadDto(
                option,
                currencyPairs.FirstOrDefault(currency => currency.Id == option.CurrencyPairId)?.Name
            )).ToList();
        }
    }
}