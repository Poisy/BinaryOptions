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
using WebAPI.Helpers;
using Domain.Entities;
using Infrastructure.Seeds;
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
        private CurrencyDataGenerator _dataGenerator;
        private Dictionary<string, double> _initialCurrencyValues = new Dictionary<string, double>();
        private const int UPDATE_INTERVAL = 10;
        public const string CACHE_KEY = "currency_data";


        //=============================================================================================
        public CurrencyHostedService(ILogger<CurrencyHostedService> logger, IMemoryCache cache, IConfiguration configuration)
        {
            _logger = logger;
            _cache = cache;
            _apiURL = configuration["CurrencyApi:URL"];
            _apiKey = configuration["CurrencyApi:Key"];
            _availableCurrencies = configuration.GetSection("Currencies").Get<string[]>();

            _dataGenerator = new CurrencyDataGenerator();

            foreach (var currency in _availableCurrencies)
            {
                _initialCurrencyValues.Add(currency, double.Parse(configuration["CurrencySeed:InitialValues:" + currency]));
            }
        }
        
        
        //=============================================================================================
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Beginning fetching currency data!");
            
            _timer = new Timer(SeedData, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(UPDATE_INTERVAL));
            
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

            if (response != null)
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

                if (currencyData.Count > 1000)
                {
                    currencyData.RemoveFirst();
                }

                currencyData.AddLast(new CurrencyData
                {
                    // Add the response data to this model
                });  
            }
            
            _logger.LogInformation("Current time: " + DateTime.Now.ToString("T"));
        }
        
        
        //=============================================================================================
        public void SeedData(object state)
        {
            LinkedList<CurrencyData> cachedData;

            if (!_cache.TryGetValue("currency_data", out cachedData))
            {
                cachedData = new LinkedList<CurrencyData>();
                cachedData.AddLast(new CurrencyData
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddSeconds(UPDATE_INTERVAL),
                    Currencies = _initialCurrencyValues.CreateInitialCurrencies().ToList()
                });

                _cache.Set(CACHE_KEY, cachedData, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.High
                });

                for (int i = 0; i < 25; i++)
                {
                    cachedData.AddLast(_dataGenerator.GenerateCurrencyData(cachedData.Last.Value, UPDATE_INTERVAL));
                }
            }

            if (cachedData.Count > 1000)
            {
                cachedData.RemoveFirst();
            }

            var currencyData = _dataGenerator.GenerateCurrencyData(cachedData.Last.Value, UPDATE_INTERVAL);

            cachedData.AddLast(currencyData);
            
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