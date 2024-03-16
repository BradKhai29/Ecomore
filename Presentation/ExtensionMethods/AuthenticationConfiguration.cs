using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Options.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Presentation.ExtensionMethods
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthenticationConfiguration(
            this IServiceCollection services,
            ConfigurationManager configurationManager)
        {
            var _jwtOptions = new JwtOptions();

            // More details: https://stackoverflow.com/questions/57998262/why-is-claimtypes-nameidentifier-not-mapping-to-sub
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            configurationManager
                .GetRequiredSection(JwtOptions.ParentSectionName)
                .GetRequiredSection(JwtOptions.SectionName)
                .Bind(_jwtOptions);

            services.AddScoped<SecurityTokenHandler, JwtSecurityTokenHandler>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _jwtOptions.Issuer,
                        ValidAudience = _jwtOptions.Audience,
                        IssuerSigningKey = _jwtOptions.GetSecurityKey()
                    };
                });

            return services;
        }
    }
}
