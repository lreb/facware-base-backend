using System.Collections.Generic;
using System.Threading.Tasks;
using FacwareBase.API.Helpers.Domain.POCO;
using FacwareBase.API.Helpers.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace FacwareBase.API.Controllers.Authentication
{
    [Route("api/[controller]")]
    public class AuthenticationController: ControllerBase
    {
        private readonly IJwtUtility _jwtUtility;
        public AuthenticationController(IJwtUtility jwtUtility)
        {
            _jwtUtility = jwtUtility;
        }

        /// <summary>
        /// Create session
        /// </summary>
        /// <param name="user">User <see cref="User"/></param>
        /// <returns>JWT data</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]User user)
        {
            IList<string> roles = new List<string>(){"Administrator","DemoRole"};

            return Ok(await _jwtUtility.GenerateJwt(user, roles));
            //return Ok();
        }
    }
}