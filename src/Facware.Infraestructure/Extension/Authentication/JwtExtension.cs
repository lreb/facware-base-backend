using Facware.Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FacwareBase.API.Extensions.Authentication
{
    /// <summary>
    /// Utility to JWT
    /// </summary>
    public static class JwtExtension
    {
        /// <summary>
        /// app setting for okta
        /// </summary>
        public static readonly string JwtOptionsSection = "Okta";
        /// <summary>
        /// enables okata service
        /// </summary>
        /// <param name="services">application service <see cref="IServiceCollection"/></param>
        /// <param name="configuration">application configuratio <see cref="IConfiguration"/></param>
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwt = configuration.GetSection(JwtOptions.JwtOptionsSection).Get<JwtOptions>();
            
            services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}