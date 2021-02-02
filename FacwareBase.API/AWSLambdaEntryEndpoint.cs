using System;
using System.IO;
using Amazon;
using Amazon.CloudWatchLogs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.AwsCloudWatch;

namespace FacwareBase.API
{
    public class LambdaEntryPoint :

        // The base class must be set to match the AWS service invoking the Lambda function. If not Amazon.Lambda.AspNetCoreServer
        // will fail to convert the incoming request correctly into a valid ASP.NET Core request.
        //
        // API Gateway REST API                         -> Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
        // API Gateway HTTP API payload version 1.0     -> Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
        // API Gateway HTTP API payload version 2.0     -> Amazon.Lambda.AspNetCoreServer.APIGatewayHttpApiV2ProxyFunction
        // Application Load Balancer                    -> Amazon.Lambda.AspNetCoreServer.ApplicationLoadBalancerFunction
        // 
        // Note: When using the AWS::Serverless::Function resource with an event type of "HttpApi" then payload version 2.0
        // will be the default and you must make Amazon.Lambda.AspNetCoreServer.APIGatewayHttpApiV2ProxyFunction the base class.
        Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        public static IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();


		/// <summary>
		/// The builder has configuration, logging and Amazon API Gateway already configured. The startup class
		/// needs to be configured in this method using the UseStartup() method.
		/// </summary>
		/// <param name="builder"></param>
		protected override void Init(IWebHostBuilder builder)
        {
            // name of the log group
            var logGroupName = $"FacwareBaseAPI/{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}";

            // customer formatter
            var formatter = new CustomTextFormatter();

            // options for the sink defaults in https://github.com/Cimpress-MCP/serilog-sinks-awscloudwatch/blob/master/src/Serilog.Sinks.AwsCloudWatch/CloudWatchSinkOptions.cs
            var options = new CloudWatchSinkOptions
            {
                // the name of the CloudWatch Log group for logging
                LogGroupName = logGroupName,

                // the main formatter of the log event
                TextFormatter = formatter,
                
                // other defaults defaults
                MinimumLogEventLevel = LogEventLevel.Information,
                BatchSizeLimit = 100,
                QueueSizeLimit = 10000,
                Period = TimeSpan.FromSeconds(10),
                CreateLogGroup = true,
                LogStreamNameProvider = new DefaultLogStreamProvider(),
                RetryAttempts = 5
            };
            
            // setup AWS CloudWatch client
            var client = new AmazonCloudWatchLogsClient(RegionEndpoint.USEast1);
            
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.AmazonCloudWatch(options, client)
            .CreateLogger();

            try
            {
                Log.Information("Staring up AWS Lambda");
                Log.Information($"Options: { JsonConvert.SerializeObject(options)}");
                builder
                .UseSerilog()
                .UseStartup<Startup>();
            }
            catch (System.Exception exception)
            {
                Log.Fatal(exception, "Application fails to start in AWS Lambda");
            }
            finally
            {
                Log.CloseAndFlush();
            }            
        }

		/// <summary>
        /// Use this override to customize the services registered with the IHostBuilder. 
        /// 
        /// It is recommended not to call ConfigureWebHostDefaults to configure the IWebHostBuilder inside this method.
        /// Instead customize the IWebHostBuilder in the Init(IWebHostBuilder) overload.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Init(IHostBuilder builder)
        {

        }
    }

    public class CustomTextFormatter : ITextFormatter  
    {  
       public void Format(LogEvent logEvent, TextWriter output)  
       {  
           output.Write($"Timestamp - {logEvent.Timestamp} | Level - {logEvent.Level} | Message {logEvent.MessageTemplate} {JsonConvert.SerializeObject(logEvent.Properties)} {output.NewLine}");  
           if (logEvent.Exception != null)  
           {  
               output.Write($"Exception - {logEvent.Exception}");  
           }  
       }  
    }  
}