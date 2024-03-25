using BusinessLogic.Services.Cores.Base;
using BusinessLogic.Services.Cores.Implementation;
using BusinessLogic.Services.Externals.Base;
using BusinessLogic.Services.Externals.Implementation;
using BusinessLogic.Services.Externals.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogic
{
    public static class DepedencyInjection
    {
        /// <summary>
        ///     Inject the services from the BusinessLogic layer.
        /// </summary>
        /// <param name="services">Current Service Collection</param>
        /// <returns>
        ///     The current service collection after injecting BusinessLogic layer's services.
        /// </returns>
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddCoreServices();
            services.AddExternalServices();

            return services;
        }

        /// <summary>
        ///     Add the services that interact
        ///     with core-entites to the dependency container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAuthHandlingService, UserAuthHandlingService>();
            services.AddScoped<IUserTokenHandlingService, UserTokenHandlingService>();
            services.AddScoped<ISystemAccountTokenHandlingService, SystemAccountTokenHandlingService>();
            services.AddScoped<ISystemAccountAuthHandlingService, SystemAccountAuthHandlingService>();
            services.AddScoped<IProductHandlingService, ProductHandlingService>();

            return services;
        }

        private static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHandlingService, PasswordHandlingService>();
            services.AddScoped<IShoppingCartHandlingService, ShoppingCartHandlingService>();

            return services;
        }
    }
}