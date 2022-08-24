using System;
using System.ComponentModel.DataAnnotations;
using Domain.Validations;

namespace Domain.Entities
{
    public class Option
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid CurrencyPairId { get; set; }

        [Required]  
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [GreaterThan(0)]
        public float Payout { get; set; }

        [Required]
        public Slope Slope { get; set; }

        [Required]
        [GreaterThan(0)]
        public double Barrier { get; set; }

        [Required]
        [GreaterThan(0)]
        public double PercentageReward { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}