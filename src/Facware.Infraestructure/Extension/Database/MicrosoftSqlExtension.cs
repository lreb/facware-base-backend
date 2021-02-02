using Facware.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Facware.Infrastructure.Extension.Database
{
    /// <summary>
    /// The Service Collection Extensions
    /// </summary>
    /// <seealso cref="IServiceCollection"/>
    public static class MicrosoftSqlExtension
    {
        /// <summary>
        /// Set up the Service SQL DB Context
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/></param>
        public static void UseInMemoryDatabase(this IServiceCollection serviceCollection)
        {
          // TODO: use your context
          serviceCollection.AddDbContext<ApplicationDbContext>(opts => 
            opts.UseInMemoryDatabase("AlbumsDB")
            .EnableSensitiveDataLogging());
        }

        /// <summary>
        /// Set up the Service SQL DB Context
        /// </summary>
        /// <param name="serviceCollection">The <see cref="IServiceCollection"/></param>
        /// <param name="applicationConfigurationConnectionString">The data migration connection string</param>
        public static void UseSqlServer(this IServiceCollection serviceCollection, string applicationConfigurationConnectionString)
        {
          // TODO: use your context
          //serviceCollection.AddDbContext<Context>(o => o.UseSqlServer(applicationConfigurationConnectionString));
        }
    }
}