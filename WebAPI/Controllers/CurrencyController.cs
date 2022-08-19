using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        //=============================================================================================
        private readonly LinkedList<CurrencyData> _data;
        private readonly string[] _availableCurrencies;

        
        //=============================================================================================
        public CurrencyController(IMemoryCache cache, IConfiguration configuration)
        {
            _data = cache.Get<LinkedList<CurrencyData>>(CurrencyHostedService.CACHE_KEY);
            _availableCurrencies = configuration.GetSection("Currencies").Get<string[]>();
        }

        
        
        //=============================================================================================
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_data);
        }
        
        
        //=============================================================================================
        [HttpGet("{currency}")]
        public IActionResult GetAll(string currency)
        {
            if (_availableCurrencies.Contains(currency))
            {
                var wantedCurrencies = _data.Select(currencyData =>
                {
                    var ohlcModel = currencyData.Currencies[currency];
                
                    return new CurrencySingleData
                    {
                        StartDate = currencyData.StartDate,
                        EndDate = currencyData.EndDate,
                        Name = currency,
                        High = ohlcModel.High,
                        Low = ohlcModel.Low,
                        Open = ohlcModel.Open,
                        Close = ohlcModel.Close
                    };
                });
                
                return Ok(wantedCurrencies);
            }
        
            return BadRequest($"Unknown currency '{currency}'");
        }
        
        
        //=============================================================================================
        [HttpGet("latest/{currency}")]
        public IActionResult GetLatest(string currency)
        {
            if (_availableCurrencies.Contains(currency) && _data.Last != null)
            {
                var currencyData = _data.Last.Value;
                var ohlcModel = currencyData.Currencies[currency];
                
                return Ok(new CurrencySingleData
                {
                    StartDate = currencyData.StartDate,
                    EndDate = currencyData.EndDate,
                    Name = currency,
                    High = ohlcModel.High,
                    Low = ohlcModel.Low,
                    Open = ohlcModel.Open,
                    Close = ohlcModel.Close
                });
            }

            return BadRequest($"Unknown currency '{currency}'");
        }
    }
}