using System;
using System.ComponentModel.DataAnnotations;
using Domain.Validations;

namespace Domain.Entities
{
    public class Withdraw
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [GreaterThan(0)]
        public float Value { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}