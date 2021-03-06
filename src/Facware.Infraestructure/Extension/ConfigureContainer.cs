//using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Facware.Infrastructure.Middleware;
using Serilog;

namespace Facware.Infrastructure.Extension
{
    public static class ConfigureContainer
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }

        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                //setupAction.SwaggerEndpoint("/swagger/OpenAPISpecification/swagger.json", "Facware");
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                // setupAction.RoutePrefix = "OpenAPI";
                //setupAction.RoutePrefix = string.Empty;
            });
        }

        public static void ConfigureSwagger(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
        }

        //public static void ConfigureHealthCheck(this IApplicationBuilder app)
        //{
        //    app.UseHealthChecks("/healthz", new HealthCheckOptions
        //    {
        //        Predicate = _ => true,
        //        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        //        ResultStatusCodes =
        //        {
        //            [HealthStatus.Healthy] = StatusCodes.Status200OK,
        //            [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
        //            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
        //        },
        //    })
        //   .UseHealthChecksUI(setup =>
        //   {
        //       setup.ApiPath = "/healthcheck";
        //       setup.UIPath = "/healthcheck-ui";
        //       setup.AddCustomStylesheet("./Customization/custom.css");
        //   });
        //}






    }
}