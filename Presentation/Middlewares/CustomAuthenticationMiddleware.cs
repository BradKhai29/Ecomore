using Microsoft.AspNetCore.Authentication;

namespace Presentation.Middlewares
{
    public class CustomAuthenticationMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var hasRedirectUri = context.Items.TryGetValue("RedirectUri", out var redirectUri);

            if (hasRedirectUri)
            {
                context.Response.Redirect(redirectUri.ToString());
            }

            return next.Invoke(context: context);
        }
    }
}
