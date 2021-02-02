namespace FacwareBase.API.Helpers.Jwt
{
    /// <summary>
    /// Defines JWT configure strongly typed settings objects
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Section in app settings json file
        /// </summary>
        public const string JwtOptionsSection = "Jwt";
        /// <summary>
        /// Domain that generates jwt
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Secret private key to encode and decode JWT
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Expitation in days
        /// </summary>
        public int ExpirationInDays { get; set; }
        /// <summary>
        /// Expiration in hours
        /// </summary>
        public int ExpirationInHours { get; set; }
    }
}