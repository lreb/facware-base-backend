using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FacwareBase.Api.Extensions.HealthCheck
{
	/// <summary>
	/// Custom health check
	/// </summary>
	public class CustomHealthCheckExtension: IHealthCheck
	{
		/// <summary>
		/// App settings data DI
		/// </summary>
		private readonly IConfiguration _config;

		public CustomHealthCheckExtension(IConfiguration config)
		{
			_config = config;
		}

		/// <summary>
		/// custom health check 
		/// </summary>
		/// <param name="context">Health check context <see cref="HealthCheckContext"/></param>
		/// <param name="cancellationToken">cancellation token <see cref="CancellationToken"/></param>
		/// <returns></returns>
		public Task<HealthCheckResult> CheckHealthAsync(
			HealthCheckContext context,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["environment"] = _config["HealthChecks:Environment"];
			data["corsAllowedOrigin"] = _config.GetSection("Cors:AllowedOrigin").Get<string[]>();
			data["authenticationMode"] = _config["Authentication:AuthenticationMode"];
			// data["connection"] = _config["ConnectionStrings:ApplicationConfigurationConnectionString"];
			return Task.FromResult(
				HealthCheckResult.Healthy("Test health check data", data));
		}
    }
}