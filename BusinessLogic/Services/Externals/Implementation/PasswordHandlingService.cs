using BusinessLogic.Services.Externals.Base;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Options.Models;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Services.Externals.Implementations
{
    internal class PasswordHandlingService :
        IPasswordHandlingService
    {
        private readonly PasswordHashOptions _passwordHashOptions;
        private readonly HMACSHA256 _hasher;

        public PasswordHandlingService(IOptions<PasswordHashOptions> passwordHashOptions)
        {
            _passwordHashOptions = passwordHashOptions.Value;
            _hasher = new HMACSHA256(key: Encoding.UTF8.GetBytes(_passwordHashOptions.PrivateKey));
        }

        public string GetHashPassword(string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            var hashPasswordBytes = _hasher.ComputeHash(passwordBytes);

            return Base64UrlEncoder.Encode(hashPasswordBytes);
        }
    }
}
