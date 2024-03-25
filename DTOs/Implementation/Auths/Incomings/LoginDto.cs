using DTOs.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTOs.Implementation.Auths.Incomings
{
    public sealed class LoginDto : IDtoNormalization
    {
        [Required]
        public string UsernameOrEmail { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }

        public string Username => UsernameOrEmail;

        public string Email => UsernameOrEmail;

        public void NormalizeAllProperties()
        {
            UsernameOrEmail = UsernameOrEmail.Trim();
            Password = Password.Trim();
        }

        public bool IsLoginByUsername()
        {
            return !UsernameOrEmail.Contains('@');
        }
    }
}
