using System.ComponentModel.DataAnnotations;

namespace Domain.Validations
{
    public class GreaterThanAttribute : ValidationAttribute
    {
        //=============================================================================================
        private readonly double _value;

        
        //=============================================================================================
        public GreaterThanAttribute(double value)
        {
            _value = value;
        }

        
        //=============================================================================================
        public override bool IsValid(object value)
        {
            return (double)value > _value;
        }
    }
}