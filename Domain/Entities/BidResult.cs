using System;
using System.ComponentModel.DataAnnotations;
using Domain.Validations;

namespace Domain.Entities
{
    public class BidResult
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OptionId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [GreaterThan(0)]
        public double Value { get; set; }

        [Required]
        public bool DidWin { get; set; }
    }
}