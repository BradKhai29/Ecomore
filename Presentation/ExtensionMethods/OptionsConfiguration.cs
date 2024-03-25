using Microsoft.Extensions.Options;
using Presentation.OptionsSetup;

namespace Presentation.ExtensionMethods
{
    /// <summary>
    ///     This class contains extension methods for all <see cref="IOptions{T}"/>
    ///     configuration in this application.
    /// </summary>
    public static class OptionsConfiguration
    {
        public static IServiceCollection AddOptionsConfiguration(this IServiceCollection services)
        {
            services.ConfigureOptions<UserAccountJwtOptionsSetup>();
            services.ConfigureOptions<SystemAccountJwtOptionsSetup>();
            services.ConfigureOptions<PasswordHashOptionsSetup>();
            services.ConfigureOptions<ResetPasswordOptionsSetup>();
            services.ConfigureOptions<MailOptionsSetup>();

            return services;
        }
    }
}
