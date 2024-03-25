using Microsoft.IdentityModel.Tokens;
using Presentation.Commons.AuthenticationHandlers;
using Presentation.Commons.Constants;
using Presentation.Middlewares;
using System.IdentityModel.Tokens.Jwt;

namespace Presentation.ExtensionMethods
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
        {
            // More details: https://stackoverflow.com/questions/57998262/why-is-claimtypes-nameidentifier-not-mapping-to-sub
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddScoped<SecurityTokenHandler, JwtSecurityTokenHandler>();
            services.AddScoped<CustomAuthenticationMiddleware>();
            
            // Add authentication-scheme handlers.
            services.AddScoped<UserAccountSchemeHandler>();
            services.AddScoped<SystemAccountSchemeHandler>();

            services
                .AddAuthentication(options =>
                {
                    options.AddScheme<UserAccountSchemeHandler>(
                        name: CookieAuthenticationSchemes.UserAccountScheme,
                        displayName: CookieAuthenticationSchemes.UserAccountScheme);

                    options.AddScheme<SystemAccountSchemeHandler>(
                        name: CookieAuthenticationSchemes.SystemAccountScheme,
                        displayName: CookieAuthenticationSchemes.SystemAccountScheme);

                    options.DefaultScheme = CookieAuthenticationSchemes.UserAccountScheme;
                    options.DefaultAuthenticateScheme = CookieAuthenticationSchemes.UserAccountScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationSchemes.UserAccountScheme;
                });

            return services;
        }
    }
}
