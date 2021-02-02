using Facware.Data.Access;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Facware.Api.Extensions
{
    /// <summary>
    /// The Service Collection Extensions
    /// </summary>
    /// <seealso cref="IServiceCollection"/>
    public static class DatabaseExtension
    {
        /// <summary>
        /// Set up the Service PostgreSQL DB Context
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="configuration">app settings configuration <see cref="IConfiguration"/></param>
        public static void UsePostgreSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            // https://www.npgsql.org/efcore/api/Microsoft.Extensions.DependencyInjection.NpgsqlServiceCollectionExtensions.html#Microsoft_Extensions_DependencyInjection_NpgsqlServiceCollectionExtensions_AddEntityFrameworkNpgsql_IServiceCollection_
            // https://www.npgsql.org/efcore/index.html#additional-configuration-for-aspnet-core-applications

            services.AddDbContext<FacwareDbContext>(options =>
                options.UseNpgsql(
                    configuration["DbContextSettings:ConnectionString"],
                    x => x.MigrationsAssembly("Facware.Data.Access")));
        }
    }
}
