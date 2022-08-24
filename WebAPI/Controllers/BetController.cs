using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/bet")]
    public class BetController : ControllerBase
    {
        //=============================================================================================
        private readonly string[] _availableCurrencies;
        private readonly ILogger<BetController> _logger;
        private readonly OptionService _optionService;
        private readonly LinkedList<CurrencyData> _data;
        private readonly UserService _userService;
        private readonly CurrencyService _currencyService;
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;


        //=============================================================================================
        public BetController(ILogger<BetController> logger, IConfiguration configuration, IMemoryCache cache, 
            OptionService optionService, UserService userService, CurrencyService currencyService,
            [FromServices]IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            _logger = logger;
            _optionService = optionService;
            _userService = userService;
            _currencyService = currencyService;
            _apiBehaviorOptions = apiBehaviorOptions;
            _data = cache.Get<LinkedList<CurrencyData>>(CurrencyHostedService.CACHE_KEY);
            _availableCurrencies = configuration.GetSection("Currencies").Get<string[]>();
        }

        
        //=============================================================================================
        [HttpGet("{currency}")]
        public IActionResult GetInfo(string currency)
        {
            if (!_availableCurrencies.Contains(currency))
            {
                ModelState.AddModelError("Currency", $"Unknown currency '{currency}'");
                
                return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            var betPreview = _optionService.GetBetPreview(currency, _data);

            return Ok(betPreview);
        }


        //=============================================================================================
        [HttpPost]
        public async Task<IActionResult> MakeABed(BetArg bet)
        {
            if (!_availableCurrencies.Contains(bet.Currency))
            {
                ModelState.AddModelError("Currency", $"Unknown currency '{bet.Currency}'");
                
                return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }
            
            var user = await _userService.GetByUsername(User?.Identity?.Name);

            var canUserBet = await _userService.RemoveFromBalance(user, bet.Payout);

            if (!canUserBet)
            {
                ModelState.AddModelError("Balance", $"User balance is too low!");
                
                return _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }
            
            await _currencyService.AddAsync(bet.Currency);

            var currency = await _currencyService.GetAsync(bet.Currency);
            var option = bet.ToOption(user, currency);
            
            await _optionService.AddOptionAsync(option);
            
            _logger.LogInformation($"New Option created for user {user.Id} !");
            
            return Ok(new
            {
                Status = 200
            });
        }
    }
}