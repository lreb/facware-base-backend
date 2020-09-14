using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FacwareBase.API.Helpers.OData
{
    // BUG: https://github.com/aws/aws-lambda-dotnet/issues/694
    // BUG: https://github.com/OData/WebApi/issues/1227
    public class CustomEnableQueryAttribute : EnableQueryAttribute
    {

        private readonly ILogger<CustomEnableQueryAttribute> _logger;  

        public CustomEnableQueryAttribute (ILogger<CustomEnableQueryAttribute> logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            HttpResponse response = actionExecutedContext.HttpContext.Response;
            string parameters = "Test "+ response.StatusCode+ " "+ (response != null).ToString() +" " + 
                response.IsSuccessStatusCode().ToString()+ " " + (actionExecutedContext.Result != null).ToString();
            _logger.LogInformation(parameters);     
                       
            actionExecutedContext.HttpContext.Response.StatusCode = 200;
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}