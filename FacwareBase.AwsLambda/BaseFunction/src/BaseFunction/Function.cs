using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace BaseFunction
{
    public class Function
    {
        // Configuration Service
	    private IConfigurationService _configuration { get; }

	    private readonly ServiceProvider _serviceProvider;

        /// <summary>
        /// function constructor
        /// </summary>
        public Function()
	    {
		    // Set up Dependency Injection
		    var serviceCollection = new ServiceCollection();
		    ConfigureServices(serviceCollection);
		    _serviceProvider = serviceCollection.BuildServiceProvider();

            // Get Configuration Service from DI system
            _configuration = _serviceProvider.GetService<IConfigurationService>();
	    }

        /// <summary>
        /// dependency injection service
        /// </summary>
        /// <param name="serviceCollection"></param>
        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            // function main services
	        serviceCollection.AddTransient<IEnvironmentService, EnvironmentService>();
	        serviceCollection.AddTransient<IConfigurationService, ConfigurationService>();

            // database IOptions
            serviceCollection.Configure<DataBaseParameters>(x =>
            {
	            x.ApplicationConfigurationConnectionString = _configuration.Configuration["ConnectionStrings:ConnectionString"];
            });

            // lambda function dependency
            serviceCollection.AddTransient<ExecuteFunction>();

            // other dependencies
            // serviceCollection.AddScoped<IDapperService, DapperService>();

            // database context dependencies
            serviceCollection.AddDbContext<MyContext>(options =>
	            options.UseNpgsql(_configuration.Configuration["ConnectionStrings:ConnectionString"]));
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(string input, ILambdaContext context)
        {
            LambdaLogger.Log($"call lambda function: {context.FunctionName} \\n");

            // entry to run app.
            return _serviceProvider.GetService<ExecuteFunction>().Run();
        }
    }

     /// <summary>
    /// Lambda function
    /// </summary>
    public class ExecuteFunction
    {
	    private readonly DataBaseParameters _connection = null;

        public ExecuteFunction(
	        IOptions<DataBaseParameters> connection)
        {
	        _connection = connection.Value;
        }

        public string Run()
        {
	        LambdaLogger.Log($"Conn: {_connection.ConnectionString}");

            LambdaLogger.Log($"welcome Luis Espinoza - {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");

            return input?.ToUpper();
        }
    }
}
