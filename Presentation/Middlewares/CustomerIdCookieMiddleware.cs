using Presentation.Commons.Constants;
using Presentation.ExtensionMethods.Others;

namespace Presentation.Middlewares
{
    public class CustomerIdCookieMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var customerId = Guid.NewGuid();
            var isAuthenticated = context.IsAuthenticated();

            if (isAuthenticated)
            {
                customerId = context.GetUserId();
            }

            // Check if cookie is set or not.
            var cookies = context.Request.Cookies;
            var isCookieSet = cookies.ContainsKey(CookieNames.CustomerId);

            if (!isCookieSet)
            {
                // Set the cookie to identify the guest.
                var key = CookieNames.CustomerId;
                var value = customerId.ToString();
                var cookieOptions = new CookieOptions
                {
                    SameSite = SameSiteMode.Strict,
                    MaxAge = TimeSpan.FromDays(7),
                };

                context.Response.Cookies.Append(key, value, cookieOptions);
            }

            return next.Invoke(context);
        }
    }
}
