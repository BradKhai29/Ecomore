using DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Implementation.Auths.Incomings
{
    public class InputEmailDto : IDtoNormalization
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public void NormalizeAllProperties()
        {
            Email = Email.Trim();
        }
    }
}
