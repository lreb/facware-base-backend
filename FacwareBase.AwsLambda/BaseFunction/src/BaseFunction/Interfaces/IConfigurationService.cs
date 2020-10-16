using Microsoft.Extensions.Configuration;

namespace BaseFunction.Interfaces
{
		public interface IConfigurationService
		{
				IConfiguration Configuration { get; }

		}
}
