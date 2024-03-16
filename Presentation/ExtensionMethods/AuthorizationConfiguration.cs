using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.ExtensionMethods
{
    public static class AuthorizationConfiguration
    {
        public static IServiceCollection AddAuthorizationConfiguration(
            this IServiceCollection services)
        {
            services.AddAuthorization(configure: options =>
            {
                // Default policy configuration.
                var jwtBearerPolicy = GetJwtBearerPolicy();

                options.AddPolicy(
                    name: JwtBearerDefaults.AuthenticationScheme,
                    policy: jwtBearerPolicy);

                options.DefaultPolicy = jwtBearerPolicy;

                // Chat-group-manager policy configuration.
            });

            return services;
        }

        private static AuthorizationPolicy GetJwtBearerPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(schemes: JwtBearerDefaults.AuthenticationScheme‌​)
                .RequireAuthenticatedUser()
                .Build();
        }
    }
}
