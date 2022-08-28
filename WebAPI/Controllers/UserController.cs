using System;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet()]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userService.GetByUsernameAsync(User.Identity.Name);

            return Ok(new
            {
                Username = user?.UserName,
                Email = user?.Email,
                Nationality = user?.Nationality,
                Balance = user?.Balance
            });
        }
    }
}