using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Domain.Entities;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public static class CurrencyFetchHelper
    {
        public static CurrencyData Fetch(HttpClient httpClient, string requestUri, string apiKey,
            DateTime startDate, DateTime endDate, string[] availableCurrencies)
        {
            throw new NotImplementedException();
        }
    }
}