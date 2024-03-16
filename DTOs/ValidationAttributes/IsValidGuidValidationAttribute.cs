using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ValidationAttributes
{
    public class IsValidGuid : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                Guid guid = (Guid) value;

                if (guid.Equals(Guid.Empty))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
