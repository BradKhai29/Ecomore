using Presentation.Middlewares;

namespace Presentation.ExtensionMethods
{
    public static class CustomCookieConfiguration
    {
        public static IServiceCollection AddCustomCookieConfiguration(this IServiceCollection services)
        {
            services.AddScoped<CustomerIdCookieMiddleware>();

            return services;
        }
    }
}
