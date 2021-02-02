using System.Collections.Generic;
using System.Threading.Tasks;
using FacwareBase.API.Helpers.Domain.POCO;

namespace FacwareBase.API.Helpers.Jwt
{
    /// <summary>
    /// JWT tools 
    /// </summary>
    public interface IJwtUtility
    {
        /// <summary>
        /// Generates JWT token
        /// </summary>
        /// <param name="user">System user entity<see cref="User"/></param>
        /// <param name="roles">role asseigned to user, string list<see cref="string"/></param>
        /// <returns></returns>
        Task<string> GenerateJwt(User user, IList<string> roles);
    }
}