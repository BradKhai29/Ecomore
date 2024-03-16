using DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Implementation.Auths.Incomings
{
    public class ResetPasswordDto : IDtoNormalization
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(otherProperty: nameof(Password))]
        public string AgainPassword { get; set; }

        [Required]
        public string Token { get; set; }

        public void NormalizeAllProperties()
        {
            Password = Password.Trim();
        }
    }
}
