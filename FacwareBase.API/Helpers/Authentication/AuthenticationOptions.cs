namespace FacwareBase.API.Helpers.Authentication
{
    /// <summary>
    /// Authentication configurations
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        /// Section in app settings
        /// </summary>
        public const string AuthenticationOptionsSection = "Authentication";
        /// <summary>
        /// Mode value in basis to AuthenticationModes <see cref="AuthenticationModes"/>
        /// </summary>
        /// <value></value>
        public string AuthenticationMode { get; set; }
    }
    /// <summary>
    /// Authentication modes
    /// </summary>
    public static class AuthenticationModes
    {
        /// <summary>
        /// Okta mode
        /// </summary>
        public const string Okta = "Okta";
        /// <summary>
        /// Bearer JWT
        /// </summary>
        public const string Jwt = "Jwt";
    }
}