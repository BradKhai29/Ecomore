using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace BusinessLogic.Services.Cores.Base
{
    public interface ISystemAccountTokenHandlingService
    {
        /// <summary>
        ///     Generate a jwt-format access-token 
        ///     from the received credentials.
        /// </summary>
        /// <param name="claims">
        ///     The claims this access token will encapsulate.
        /// </param>
        /// <param name="lifeSpan">
        ///     The live span of this access-token.
        /// </param>
        /// 
        /// <returns>
        ///     A string that represents the access-token value.
        /// </returns>
        string GenerateAccessToken(
            IEnumerable<Claim> claims,
            TimeSpan lifeSpan);
    }
}
