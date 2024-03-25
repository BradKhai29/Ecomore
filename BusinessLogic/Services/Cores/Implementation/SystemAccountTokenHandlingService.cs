using BusinessLogic.Services.Cores.Base;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Options.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BusinessLogic.Services.Cores.Implementation
{
    internal class SystemAccountTokenHandlingService : ISystemAccountTokenHandlingService
    {
        private readonly SystemAccountJwtOptions _jwtOptions;
        private readonly SecurityTokenHandler _securityTokenHandler;

        public SystemAccountTokenHandlingService(
            IOptions<SystemAccountJwtOptions> jwtOptions,
            SecurityTokenHandler securityTokenHandler)
        {
            _jwtOptions = jwtOptions.Value;
            _securityTokenHandler = securityTokenHandler;
        }


        public string GenerateAccessToken(
            IEnumerable<Claim> claims,
            TimeSpan lifeSpan)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                Subject = new ClaimsIdentity(claims: claims),
                SigningCredentials = new SigningCredentials(
                    key: _jwtOptions.GetSecurityKey(),
                    algorithm: SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.Add(lifeSpan)
            };

            // Generate token.
            var token = _securityTokenHandler.CreateToken(tokenDescriptor: tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
