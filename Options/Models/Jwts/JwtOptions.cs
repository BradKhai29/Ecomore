using Microsoft.IdentityModel.Tokens;
using Options.Commons.Constants;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Options.Models.Jwts
{
    public abstract class JwtOptions
    {
        public const string ParentSectionName = AuthenticationSections.RootSection;
        public const string SystemAccountSection = AuthenticationSections.SystemAccountSection;
        public const string UserAccountSection = AuthenticationSections.UserAccountSection;
        public const string JwtSection = AuthenticationSections.JwtSection;

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string PrivateKey { get; set; }

        /// <summary>
        ///     The number of days this token will live in short term.
        /// </summary>
        public int DefaultShortLiveDays { get; set; }

        /// <summary>
        ///     The number of days this token will live in long term.
        /// </summary>
        public int DefaultLongLiveDays { get; set; }

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

        public TimeSpan GetLifeSpan(bool isLongLive)
        {
            if (isLongLive)
            {
                return TimeSpan.FromDays(DefaultLongLiveDays);
            }

            return TimeSpan.FromDays(DefaultShortLiveDays);
        }

        public TimeSpan GetLongLifeSpan()
        {
            return TimeSpan.FromDays(DefaultLongLiveDays);
        }

        public TimeSpan GetShortLifeSpan()
        {
            return TimeSpan.FromDays(DefaultShortLiveDays);
        }
    }
}
