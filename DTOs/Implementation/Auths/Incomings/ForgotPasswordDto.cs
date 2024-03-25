using DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Implementation.Auths.Incomings
{
    public class ForgotPasswordDto : IDtoNormalization
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
