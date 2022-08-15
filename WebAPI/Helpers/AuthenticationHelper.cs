using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Configuration;

namespace WebAPI.Helpers
{
    public static class AuthenticationHelper
    {
        public static object GenerateToken(ApplicationUser identityUser, JwtBearerTokenSettings jwtSettings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                    new Claim(ClaimTypes.Email, identityUser.Email)
                }),

                Expires = DateTime.UtcNow.AddSeconds(jwtSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtSettings.Audience,
                Issuer = jwtSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public static async Task<ApplicationUser> ValidateUser(LoginUser userModel, UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByEmailAsync(userModel.Email);
            if (user != null)
            {
                var result = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userModel.Password);
                return result == PasswordVerificationResult.Failed ? null : user;
            }

            return null;
        }
    }
}