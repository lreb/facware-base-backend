using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Facware.Infrastructure.Extension.Database
{
    /// <summary>
    /// The Service Collection Extensions
    /// </summary>
    /// <seealso cref="IServiceCollection"/>
    public static class PostgreSQLExtension
    {
        /// <summary>
        /// Set up the Service PostgreSQL DB Context
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/></param>
        /// <param name="applicationConfigurationConnectionString">The data migration connection string</param>
        public static void UsePostgreSqlServer(this IServiceCollection serviceCollection, string applicationConfigurationConnectionString)
        {
          // https://www.npgsql.org/efcore/api/Microsoft.Extensions.DependencyInjection.NpgsqlServiceCollectionExtensions.html#Microsoft_Extensions_DependencyInjection_NpgsqlServiceCollectionExtensions_AddEntityFrameworkNpgsql_IServiceCollection_
          // https://www.npgsql.org/efcore/index.html#additional-configuration-for-aspnet-core-applications

          // TODO: use your context
          //serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(applicationConfigurationConnectionString));
        }

    }
}