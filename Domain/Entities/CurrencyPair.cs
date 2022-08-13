using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CurrencyPair
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}