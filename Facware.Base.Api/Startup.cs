using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facware.Base.Api.Extensions.Database;
using Facware.Base.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Facware.Base.Api.Helpers;

namespace Facware.Base.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        /// <summary>
        /// Load configuration settings
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureConfigSettings(IServiceCollection services)
        {
            services
               .AddOptions()
               .Configure<ConnectionStrings>(_configuration.GetSection(nameof(ConnectionStrings)));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Database context service

            var cn = _configuration.GetSection("ConnectionStrings:FacwareConnectionString");

            // TODO: before use this, create your own dbcontext
            services.UsePostgreSqlServer(cn.Value);

            // in memory db
            // services.UseInMemoryDatabase();
            #endregion

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Facware.Base.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Facware.Base.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
