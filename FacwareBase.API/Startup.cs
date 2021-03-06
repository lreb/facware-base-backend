using System.Linq;
using FacwareBase.Api.Extensions;
using FacwareBase.Api.Extensions.Cors;
using FacwareBase.Api.Extensions.Environment;
using FacwareBase.Api.Extensions.Swagger;
using FacwareBase.API.Helpers.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using FacwareBase.Api.Extensions.HealthCheck;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Serilog;
using Microsoft.AspNet.OData.Extensions;
using FacwareBase.Api.Extensions.OData;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Net.Http.Headers;
using FacwareBase.API.Helpers.OData;
using FacwareBase.API.Helpers.Jwt;
using FacwareBase.API.Extensions.DependencyInyection;
using FacwareBase.API.Extensions.Authentication;
using FacwareBase.API.Helpers.Authentication;
using Amazon.S3;
using FacwareBase.API.Core.File.Management;
using FacwareBase.API.Helpers.Aws;
using FacwareBase.API.Helpers.FileManagement;
using FacwareBase.API.Services.Amazon.S3.Core.Interfaces;
using FacwareBase.API.Services.Amazon.S3.Infraestructure.Repositories;

namespace FacwareBase.API
{
    /// <summary>
    /// Main class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The ConnectionStrings Options snapshot
        /// </summary>
        private IOptionsSnapshot<ConnectionString> _connectionString;
        /// <summary>
		/// Load project configurations
		/// </summary>
		/// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// _configuration values from app settings
        /// </summary>
        public IConfiguration _configuration { get; }
        /// <summary>
        /// Load configuration settings
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureConfigSettings(IServiceCollection services)
        {
            services
               .AddOptions()
               .Configure<ConnectionString>(_configuration.GetSection(nameof(ConnectionString)));

            services
                .AddOptions()
                .Configure<JwtOptions>(_configuration.GetSection(JwtOptions.JwtOptionsSection));

            services
	            .AddOptions()
	            .Configure<SessionAwsCredentialsOptions>(_configuration.GetSection(nameof(SessionAwsCredentialsOptions)));

            services
	            .AddOptions()
	            .Configure<FileStorageOptions>(_configuration.GetSection(nameof(FileStorageOptions)));

            var serviceProvider = services.BuildServiceProvider();

            _connectionString = serviceProvider.GetService<IOptionsSnapshot<ConnectionString>>();
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services">Application services <see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region enable app settings
            ConfigureConfigSettings(services);
            #endregion

            #region Cors service
            // enable policy cors service
	        services.ConfigureCors(_configuration);
            #endregion

            #region Health check service
            // enable healtcheck
            services.AddHealthChecks();
            #endregion

            services.AddMvc(Options=>
            {
                Options.EnableEndpointRouting = false;
            })
            .AddNewtonsoftJson(options => 
            { 
                options.SerializerSettings.ContractResolver  = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.JwtDependency();

            #region Swagger service
            // enable swagger service
            services.ConfigureSwaggerExtension(_configuration);
            #endregion

            services.AddControllers();

            #region Database context service
            // TODO: before use this, create your own dbcontext
            // services.UsePostgreSqlServer(_connectionString.Value.ApplicationConfigurationConnectionString);

            // in memory db
            services.UseInMemoryDatabase();
            #endregion

            #region OData service
            // enable custom healt check
            services.AddHealthChecks()
	            .AddCheck<CustomHealthCheckExtension>("custom");
            // enable odata
            services.AddOData();

            // OData Workaround: https://github.com/OData/WebApi/issues/1177
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
            
            // AWS odata attribute
            services.AddScoped<EnableQueryFromODataToAWS>();
            #endregion    

            #region Authentication service

            var authenticationOptions = _configuration.GetSection(AuthenticationOptions.AuthenticationOptionsSection).Get<AuthenticationOptions>();
            
            if(authenticationOptions.AuthenticationMode.Contains(AuthenticationModes.Okta))
            {
                // enalbe Okta service
                services.ConfigureOkta(_configuration);      
            }
            else if(authenticationOptions.AuthenticationMode.Contains(AuthenticationModes.Jwt)) 
            {
                // enalbe JWT bearer
                services.ConfigureJwt(_configuration);
            }
            #endregion

            services.JwtDependency();

            services.AwsStorageDependency(_configuration);

            services.FileManagementDependency();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        /// <param name="app">application pipes <see cref="IApplicationBuilder"/></param>
        /// <param name="env">application environments <see cref="IWebHostEnvironment"/></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Cors pipe
            app.UseCors(CorsExtension.AllowSpecificOrigins);
            // handle several environments
	        if (env.IsLocal())
            {
                app.UseDeveloperExceptionPage();
                app.EnableSwaggerPipeline(_configuration);
            }
	        else if (env.IsDevelopment())
	        {
		        app.UseDeveloperExceptionPage();
		        app.EnableSwaggerPipeline(_configuration);
            }
            else if(env.IsStaging())
            {
	            app.EnableSwaggerPipeline(_configuration);
            }
            else
            {
	            app.EnableSwaggerPipeline(_configuration);
            }
            
            //adding health check point used by the UI
			app.UseHealthChecks("/healthcheck", new HealthCheckOptions()
			{
				Predicate = _ => true,
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            
            app.UseHttpsRedirection();

            // Serilog pipe
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");

                endpoints.MapControllers();
            });

            app.UseMvc(build =>
            {
                // EnableQuery attribute enables an endpoint to have OData capabilities.
                build.Select().Expand().Count().Filter().OrderBy().MaxTop(100).SkipToken().Build();
                build.MapODataServiceRoute("odata","odata", app.GetODataModels());
            });
        }
    }
}
