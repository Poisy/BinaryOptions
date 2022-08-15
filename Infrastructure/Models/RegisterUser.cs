using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class RegisterUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Nationality { get; set; }
    }
}