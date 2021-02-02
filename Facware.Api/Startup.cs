using Facware.Api.Extensions;
using Facware.Data.Access.Base.Base;
using Facware.Data.Access.Repository.Implementation;
using Facware.Data.Access.Repository.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Facware.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Swagger service
            // enable swagger service
            services.ConfigureSwaggerExtension(_configuration);
            #endregion

            services.AddControllers();
            /*services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Facware.Api", Version = "v1" });
            });*/

            //var connectionString = _configuration["DbContextSettings:ConnectionString"];
            /* services.AddDbContext<FacwareDbContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    x => x.MigrationsAssembly("Facware.Data.Access"))); */


            #region Database context service
            // TODO: before use this, create your own dbcontext
            services.UsePostgreSqlServer(_configuration);

            // in memory db
            // services.UseInMemoryDatabase();
            #endregion

            #region Cors service
            // enable policy cors service
            services.ConfigureCors(_configuration);
            #endregion

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IItemRepository, ItemRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            else if (env.IsStaging())
            {
                app.EnableSwaggerPipeline(_configuration);
            }
            else
            {
                app.EnableSwaggerPipeline(_configuration);
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
