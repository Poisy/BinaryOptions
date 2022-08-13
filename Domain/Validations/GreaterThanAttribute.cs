using System.ComponentModel.DataAnnotations;

namespace Domain.Validations
{
    public class GreaterThanAttribute : ValidationAttribute
    {
        //=============================================================================================
        private readonly float _value;

        
        //=============================================================================================
        public GreaterThanAttribute(float value)
        {
            _value = value;
        }

        
        //=============================================================================================
        public override bool IsValid(object value)
        {
            return (int)value > _value;
        }
    }
}