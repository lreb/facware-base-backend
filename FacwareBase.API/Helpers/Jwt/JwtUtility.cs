using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FacwareBase.API.Helpers.Domain.POCO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace FacwareBase.API.Helpers.Jwt
{
    public class JwtUtility : IJwtUtility
    {
        private readonly JwtOptions _jwtSettingsOptions;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="jwtSettingsOptions">App settings from appsettings.json<see cref="JwtOptions"/></param>
		public JwtUtility
		(
            IOptions<JwtOptions> jwtSettingsOptions
		)
		{
			_jwtSettingsOptions = jwtSettingsOptions.Value;
            //_configuration = configuration;
		}

        //public static readonly string OktaConfiguartion = "Jwt";
        public async Task<string> GenerateJwt(User user, IList<string> roles)
        {
            //_jwtSettingsOptions = _configuration.GetSection(JwtOptions.JwtOptionsSection).Get<JwtOptions>();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettingsOptions.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // set expiration in hours
            // var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings.ExpirationInDays));
            // set expiration in hours
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_jwtSettingsOptions.ExpirationInHours));

            var token = new JwtSecurityToken(
                issuer: _jwtSettingsOptions.Issuer,
                audience: _jwtSettingsOptions.Issuer,
                claims,
                expires : expires,
                signingCredentials : creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}