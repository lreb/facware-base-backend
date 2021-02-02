// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Okta.AspNetCore;
// using FacwareBase.API.Helpers.Okta;
// using System;

// namespace FacwareBase.Api.Extensions
// {
//     /// <summary>
//     /// enables okata service
//     /// </summary>
//     public static class OktaExtension
//     {
//         /// <summary>
//         /// app setting for okta
//         /// </summary>
//         public static readonly string OktaConfiguartion = "Okta";
//         /// <summary>
//         /// enables okata service
//         /// </summary>
//         /// <param name="services">application service <see cref="IServiceCollection"/></param>
//         /// <param name="configuration">application configuratio <see cref="IConfiguration"/></param>
//         public static void ConfigureOkta(this IServiceCollection services, IConfiguration configuration)
//         {
//             var okta = configuration.GetSection(OktaConfiguartion).Get<OktaHelper>();
            
//             services.AddAuthentication(options =>
//             {
//                 options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
//                 options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
//                 options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
//             })
//             .AddCookie()
//             .AddJwtBearer(options =>
//             {
//                 options.Authority = okta.Authority;
//                 options.Audience = okta.Audience;
//                 options.RequireHttpsMetadata = Convert.ToBoolean(okta.RequireHttpsMetadata);
//             });
//         }
//     }
// }