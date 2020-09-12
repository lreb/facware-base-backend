using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacwareBase.Api.Extensions;
using FacwareBase.Api.Extensions.Cors;
using FacwareBase.Api.Extensions.Environment;
using FacwareBase.Api.Extensions.Swagger;
using FacwareBase.API.Helpers.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using FacwareBase.Api.Extensions.HealthCheck;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

namespace FacwareBase.API
{
    public class Startup
    {
        /// <summary>
        /// The ConnectionStrings Options snapshot
        /// </summary>
        private IOptionsSnapshot<ConnectionString> _connectionString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigureConfigSettings(IServiceCollection services)
        {
            services
               .AddOptions()
               .Configure<ConnectionString>(Configuration.GetSection(nameof(ConnectionString)));

            var serviceProvider = services.BuildServiceProvider();

            _connectionString = serviceProvider.GetService<IOptionsSnapshot<ConnectionString>>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // enable policy cors service
	        services.ConfigureCors(Configuration);

            services.AddHealthChecks();

            services.AddMvc()
            .AddNewtonsoftJson(options => 
            { 
                options.SerializerSettings.ContractResolver  = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.ConfigureSwaggerExtension(Configuration);

            services.AddControllers();

            // TODO: before use this, create your own dbcontext
            // services.UsePostgreSqlServer(_connectionString.Value.ApplicationConfigurationConnectionString);
            
            services.AddHealthChecks()
	            .AddCheck<CustomHealthCheckExtension>("custom");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(CorsExtension.QSSAllowSpecificOrigins);
	        if (env.IsLocal())
            {
                app.UseDeveloperExceptionPage();
                app.EnableSwaggerPipeline(Configuration);
            }
	        else if (env.IsDevelopment())
	        {
		        app.UseDeveloperExceptionPage();
		        app.EnableSwaggerPipeline(Configuration);
            }
            else if(env.IsStaging())
            {
	            app.EnableSwaggerPipeline(Configuration);
            }
            else
            {
	            app.EnableSwaggerPipeline(Configuration);
            }
            
            //adding health check point used by the UI
			app.UseHealthChecks("/healthcheck", new HealthCheckOptions()
			{
				Predicate = _ => true,
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");

                endpoints.MapControllers();
            });
        }
    }
}
