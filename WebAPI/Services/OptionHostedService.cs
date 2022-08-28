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
using Domain.Helpers;
using Domain.Entities;
using Infrastructure.Seeds;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class OptionHostedService : IHostedService, IDisposable
    {
        //=============================================================================================
        private readonly ILogger<OptionHostedService> _logger;
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string[] _availableCurrencies;
        private Timer _timer;
        private const int UPDATE_INTERVAL = 10;
        public const string CACHE_KEY = "currency_data";


        //=============================================================================================
        public OptionHostedService(ILogger<OptionHostedService> logger, IMemoryCache cache, 
            IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _cache = cache;
            _scopeFactory = scopeFactory;
            _availableCurrencies = configuration.GetSection("Currencies").Get<string[]>();
        }
        
        
        //=============================================================================================
        public Task StartAsync(CancellationToken cancellationToken)
        { 
            _timer = new Timer(MonitorOptions, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(UPDATE_INTERVAL));
            
            return Task.CompletedTask;
        }
        
        
        //=============================================================================================
        public async void MonitorOptions(object state)
        {
            LinkedList<CurrencyData> cachedData;

            if (_cache.TryGetValue("currency_data", out cachedData))
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var optionService = scope.ServiceProvider.GetRequiredService<OptionService>();

                    var activeOptions = await optionService.GetAllUnexpiredOptionsAsync();

                    foreach (var option in activeOptions)
                    {
                        if (!option.IsActive)
                        {
                            continue;
                        }
                        
                        if (cachedData.First.Value.EndDate > option.StartDate && option.IsActive)
                        {
                            await optionService.BreakOption(option);
                        }
                        else if (cachedData.Last.Value.EndDate > option.ExpirationDate)
                        {
                            await optionService.AddBidResultAsync(option, cachedData.Last.Value);
                        }
                    }
                }
            }
        }
        
        
        //=============================================================================================
        public Task StopAsync(CancellationToken cancellationToken)
        {
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