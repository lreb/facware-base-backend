namespace FacwareBase.API.Helpers.Okta
{
    /// <summary>
    /// Okta helper
    /// </summary>
    public class OktaHelper
    {
        /// <summary>
        /// origin autority
        /// </summary>
        /// <code>https://{replace-with-okta-domain}.okta.com/oauth2/default</code>
        public string Authority { get; set; }
        /// <summary>
        /// allowed audience
        /// </summary>
        /// <code>api://default</code>
        public string Audience { get; set; }
        public string RequireHttpsMetadata { get; set; }
    }
}