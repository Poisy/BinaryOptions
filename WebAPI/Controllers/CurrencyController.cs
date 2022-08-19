using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
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
        [HttpGet("ohlc/{currency}")]
        public IActionResult GetAllOHLC(string currency)
        {
            if (_availableCurrencies.Contains(currency))
            {
                var wantedCurrencies = _data
                    .Select(currencyData => currencyData.GetSingleData(currency));
                
                return Ok(wantedCurrencies);
            }
        
            return BadRequest($"Unknown currency '{currency}'");
        }
        
        
        //=============================================================================================
        [HttpGet("ohlc/latest/{currency}")]
        public IActionResult GetLatestOHLC(string currency)
        {
            if (_availableCurrencies.Contains(currency) && _data.Last != null)
            {
                var currencySingleData = _data.Last.Value.GetSingleData(currency);

                return Ok(currencySingleData);
            }

            return BadRequest($"Unknown currency '{currency}'");
        }
        
        
        //=============================================================================================
        [HttpGet("{currency}")]
        public IActionResult GetLatest(string currency)
        {
            if (_availableCurrencies.Contains(currency) && _data.Last != null)
            {
                var currencyValue = _data.Last.Value.Currencies
                    .FirstOrDefault(currency1 => currency1.Name == currency)?.Value;
                
                return Ok(new { name = currency, value = currencyValue });
            }

            return BadRequest($"Unknown currency '{currency}'");
        }
    }
}