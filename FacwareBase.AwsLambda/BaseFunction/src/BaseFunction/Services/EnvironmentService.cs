using System;
using BaseFunction.Interfaces;
using BaseFunction.Constants;

namespace BaseFunction.src.Services
{
	public class EnvironmentService : IEnvironmentService
	{
		public EnvironmentService()
		{
			EnvironmentName = Environment.GetEnvironmentVariable(GlobalConstants.EnvironmentVariables.AspnetCoreEnvironment)
				?? GlobalConstants.Environments.Production;
		}

		public string EnvironmentName { get; set; }
	}
}
