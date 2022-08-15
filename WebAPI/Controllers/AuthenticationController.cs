using System;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using WebAPI.Configuration;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, 
            UserManager<ApplicationUser> userManager, UserService userService)
        {
            _userService = userService;
            _userManager = userManager;
            _jwtBearerTokenSettings = jwtTokenOptions.Value;
        }
        
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterUser userModel)
        {
            if (!ModelState.IsValid || userModel is null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var newUser = new ApplicationUser
            {
                UserName = userModel.UserName, 
                Email = userModel.Email,
                Nationality = userModel.Nationality,
                Balance = 10000
            };
            
            var result = await _userManager.CreateAsync(newUser, userModel.Password);
            
            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
            }

            return Ok(new { Message = "User Reigstration Successful" });
        }
        
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginUser userModel)
        {
            ApplicationUser user;

            if (!ModelState.IsValid || userModel is null || 
                (user = await AuthenticationHelper.ValidateUser(userModel, _userManager)) == null)
            {
                return new BadRequestObjectResult(new { Message = "Login failed" });
            }

            var token = AuthenticationHelper.GenerateToken(user, _jwtBearerTokenSettings);
            
            return Ok(new { Token = token, Message = "Success" });
        }
    }
}