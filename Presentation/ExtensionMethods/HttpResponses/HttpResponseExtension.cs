using Azure;
using Options.Models.Jwts;
using Presentation.Commons.Constants;

namespace Presentation.ExtensionMethods.HttpResponses
{
    public static class HttpResponseExtension
    {
        public static void AddUserAccessToken(
            this HttpResponse response,
            string accessToken,
            TimeSpan lifeSpan)
        {
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                MaxAge = lifeSpan,
            };
            
            response.Cookies.Append(
                key: CookieNames.UserAccessToken,
                value: accessToken,
                options: accessTokenCookieOptions);
        }

        public static void RemoveUserAccessToken(this HttpResponse response)
        {
            response.Cookies.Delete(CookieNames.UserAccessToken);
        }

        public static void AddSystemAccountAccessToken(
            this HttpResponse response,
            string accessToken)
        {
            var accessTokenCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                MaxAge = TimeSpan.FromDays(1),
            };
            
            response.Cookies.Append(
                key: CookieNames.ManagerAccessToken,
                value: accessToken,
                options: accessTokenCookieOptions);
        }

        public static void RemoveAdminAccessToken(this HttpResponse response)
        {
            response.Cookies.Delete(CookieNames.ManagerAccessToken);
        }
    }
}
