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
        [HttpGet("{baseCurrency}")]
        public IActionResult GetByBase(string baseCurrency, [FromBody]string[] currencies = null)
        {
            if (_availableCurrencies.Contains(baseCurrency))
            {
                var wantedCurrencies = _data.Select(currencyData =>
                    CurrencyHelper.FilterCurrencyData(currencyData, _availableCurrencies,
                        baseCurrency, currencies));
                
                return Ok(wantedCurrencies);
            }

            return BadRequest($"Unknown currency '{baseCurrency}'");
        }
        
        
        //=============================================================================================
        [HttpGet("latest/{baseCurrency}")]
        public IActionResult GetLastByBase(string baseCurrency, [FromBody]string[] currencies = null)
        {
            if (_availableCurrencies.Contains(baseCurrency) && _data.Last != null)
            {
                return Ok(CurrencyHelper.FilterCurrencyData(_data.Last.Value, _availableCurrencies, 
                    baseCurrency, currencies));
            }

            return BadRequest($"Unknown currency '{baseCurrency}'");
        }
    }
}