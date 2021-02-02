// using Amazon.S3;
// using FacwareBase.API.Core.File.Management;
// using FacwareBase.API.Helpers.Jwt;
// using FacwareBase.API.Services.Amazon.S3.Core.Interfaces;
// using FacwareBase.API.Services.Amazon.S3.Infraestructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Facware.Infrastructure.Extension.DependencyInjection
{
    /// <summary>
    /// register all dependencies
    /// </summary>
    public static class DependencyInyectionExtension
    {
        /// <summary>
        /// Enables JWT features
        /// </summary>
        /// <param name="services">Application services<see cref="IServiceCollection"/></param>
        public static void JwtDependency(this IServiceCollection services)
        {
            //services.AddScoped<IJwtUtility,JwtUtility>();
        }

        /// <summary>
        /// enables AWS storage
        /// </summary>
        /// <param name="services">Application services<see cref="IServiceCollection"/></param>
        /// <param name="configuration"></param>
        public static void AwsStorageDependency(this IServiceCollection services, IConfiguration configuration)
        {
	        // services.AddAWSService<IAmazonS3>(configuration.GetAWSOptions());
	        // services.AddSingleton<IBucketRepository, BucketRepository>();
	        // services.AddSingleton<IFilesRepository, FilesRepository>();
        }

        /// <summary>
        /// file management dependencies
        /// </summary>
        /// <param name="services">Application services<see cref="IServiceCollection"/></param>
        public static void FileManagementDependency(this IServiceCollection services)
        {
	        // services.AddSingleton<IFileManagement, FileManagement>();
        }
    }
}