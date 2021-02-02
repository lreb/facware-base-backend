using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using BaseFunction.Interfaces;

namespace BaseFunction.src.Services
{
	public class ConfigurationService : IConfigurationService
	{
		public IEnvironmentService EnvService { get; }

		public IConfiguration Configuration => new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
			.Build();

		public ConfigurationService(IEnvironmentService envService)
		{
			EnvService = envService;
		}

		public IConfiguration GetConfiguration()
		{
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{EnvService.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables()
				.Build();
		}
	}
}
