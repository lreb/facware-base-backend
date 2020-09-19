using FacwareBase.API.Helpers.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace FacwareBase.API.Extensions.DependencyInyection
{
    /// <summary>
    /// register all dependencies
    /// </summary>
    public static class DependencyInyectionExtension
    {
        /// <summary>
        /// Configure all dependecy inyection
        /// </summary>
        /// <param name="services">Application services<see cref="IServiceCollection"/></param>
        public static void DependencyInyectionConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IJwtUtility,JwtUtility>();
        }
    }
}