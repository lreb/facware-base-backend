namespace Facware.Base.Api.Helpers
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public SwaggerConfiguration SwaggerConfiguration { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string MicrosoftHostingLifetime { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class ConnectionStrings
    {
        public string FacwareConnectionString { get; set; }
    }

    public class SwaggerConfiguration
    {
        public string SwaggerJSONEndpoints { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string TermsOfService { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string LicenseName { get; set; }
        public string LicenseUrl { get; set; }
    }

    
}
