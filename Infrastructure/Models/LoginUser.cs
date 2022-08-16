using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class LoginUser
    {
        [Required]
        public string Email { get; set; }
        
        public string Password { get; set; }

        public bool ThirdParty { get; set; } = false;
    }
}