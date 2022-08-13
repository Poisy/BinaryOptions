using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public float Balance { get; set; }

        [Required]
        public string Nationality { get; set; }
    }
}