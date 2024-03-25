using DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Implementation.Auths.Incomings
{
    public class RegisterDto : IDtoNormalization
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public void NormalizeAllProperties()
        {
            Username = Username.Trim();
            Password = Password.Trim();
            Email = Email.Trim();
        }
    }
}
