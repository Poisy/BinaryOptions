using System;

namespace Domain.Entities
{
    public class CurrencySingleData : OHLCModel
    {
        public string Name { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }

        public double Price { get; set; }
    }
}