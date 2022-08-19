using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public static class CurrencyFetchHelper
    {
        public static CurrencyData Fetch(HttpClient httpClient, string requestUri, string apiKey,
            DateTime startDate, DateTime endDate, string[] availableCurrencies)
        {
            requestUri = requestUri +
                $"?api_key={apiKey}" +
                $"&start_date={startDate.ToString("yyyy-MM-dd-HH:mm")}" +
                $"&end_date={endDate.ToString("yyyy-MM-dd-HH:mm")}" +
                $"&currency={string.Join(",", availableCurrencies)}";
            
            var response = httpClient
                .GetFromJsonAsync<CurrencyResult>(requestUri)
                .Result;

            if (response is null)
            {
                return null;
            }

            return new CurrencyData
            {
                StartDate = DateTime.ParseExact(response.StartDate, "yyyy-MM-dd-HH:mm", null),
                EndDate = DateTime.ParseExact(response.EndDate, "yyyy-MM-dd-HH:mm", null),
                Currencies = response.Price.Values.First()
            };
        }
    }
}