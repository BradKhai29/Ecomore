using Microsoft.IdentityModel.Tokens;
using Options.Commons.Constants;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Options.Models
{
    public class ResetPasswordOptions
    {
        public const string ParentSectionName = AuthenticationSections.RootSection;
        public const string SectionName = "ResetPassword";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string PrivateKey { get; set; }

        public int LiveMinutes { get; set; }

        #region Get Key
        public byte[] GetKey()
        {
            var encryptedKey = new HMACSHA256(
                key: Encoding.UTF8.GetBytes(PrivateKey)).Key;

            return encryptedKey;
        }

        public SymmetricSecurityKey GetSecurityKey()
        {
            var encryptedKey = GetKey();

            return new SymmetricSecurityKey(key: encryptedKey);
        }
        #endregion

        public TimeSpan GetLifeSpan()
        {
            return TimeSpan.FromMinutes(value: LiveMinutes);
        }
    }
}
