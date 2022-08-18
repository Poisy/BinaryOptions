using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class CurrencyHostedService : IHostedService, IDisposable
    {
        //=============================================================================================
        private readonly ILogger<CurrencyHostedService> _logger;
        private readonly IMemoryCache _cache;
        private readonly string _apiURL;
        private readonly string _apiKey;
        private readonly string[] _availableCurrencies;
        private Timer _timer;
        private const int FETCH_DELAY = 30;
        public const string CACHE_KEY = "currency_data";


        //=============================================================================================
        public CurrencyHostedService(ILogger<CurrencyHostedService> logger, IMemoryCache cache, IConfiguration configuration)
        {
            _logger = logger;
            _cache = cache;
            _apiURL = configuration["CurrencyApi:URL"];
            _apiKey = configuration["CurrencyApi:Key"];
            _availableCurrencies = configuration.GetSection("Currencies").Get<string[]>();
        }
        
        
        //=============================================================================================
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Beginning fetching currency data!");
            
            _timer = new Timer(FetchData, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(FETCH_DELAY));
            
            return Task.CompletedTask;
        }

        
        //=============================================================================================
        private void FetchData(object state)
        {
            using var client = new HttpClient
            {
                BaseAddress = new Uri(_apiURL)
            };

            var response = client
                .GetFromJsonAsync<CurrencyResult>($"?api_key={_apiKey}")
                .Result;

            if (response?.Meta.Code == 200)
            {
                LinkedList<CurrencyData> currencyData;

                if (!_cache.TryGetValue("currency_data", out currencyData))
                {
                    currencyData = new LinkedList<CurrencyData>();

                    _cache.Set(CACHE_KEY, currencyData, new MemoryCacheEntryOptions
                    {
                        Priority = CacheItemPriority.High
                    });
                }

                if (currencyData.Count > 100)
                {
                    currencyData.RemoveFirst();
                }

                currencyData.AddLast(new CurrencyData
                {
                    Date = response.Response.Date,
                    Currencies = response.Response.Rates
                        .Where(c => _availableCurrencies.Contains(c.Key))
                        .ToDictionary(c => c.Key, c => c.Value)
                });  
            }
            
            _logger.LogInformation("Current time: " + DateTime.Now.ToString("T"));
        }

        
        //=============================================================================================
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ending fetching currency data!");
            _timer?.Change(Timeout.Infinite, 0);
            
            return Task.CompletedTask;
        }

        
        //=============================================================================================
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}