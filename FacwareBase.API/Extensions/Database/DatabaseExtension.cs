using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
// using QSS.DataAccess.DataContext;

namespace FacwareBase.Api.Extensions
{
    /// <summary>
    /// The Service Collection Extensions
    /// </summary>
    /// <seealso cref="IServiceCollection"/>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Set up the Service SQL DB Context
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/></param>
        /// <param name="applicationConfigurationConnectionString">The data migration connection string</param>
        public static void UseSqlServer(this IServiceCollection serviceCollection, string applicationConfigurationConnectionString)
        {
          // TODO: use your context
          //serviceCollection.AddDbContext<QSSContext>(o => o.UseSqlServer(applicationConfigurationConnectionString));
        }

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
          //serviceCollection.AddDbContext<QSSContext>(options => options.UseNpgsql(applicationConfigurationConnectionString));
        }

    }
}
