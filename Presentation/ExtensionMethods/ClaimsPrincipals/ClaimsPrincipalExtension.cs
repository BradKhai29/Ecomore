using DataAccess.Entities;
using Helpers.Commons.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Presentation.ExtensionMethods.ClaimsPrincipals
{
    public static class ClaimsPrincipalExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            string value = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            Guid.TryParse(value, out var userId);

            return userId;
        }

        public static UserEntity GetUserEntity(this ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.GetUserId();
            var username = claimsPrincipal.FindFirstValue(CustomClaimTypes.Username);

            return new UserEntity
            {
                Id = userId,
                UserName = username
            };
        }
    }
}
