using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class RegisterUser
    {
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Nationality { get; set; }

        public bool ThirdParty { get; set; } = false;
    }
}