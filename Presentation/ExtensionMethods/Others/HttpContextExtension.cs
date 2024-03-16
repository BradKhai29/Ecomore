using DTOs.Implementation.ShoppingCarts.Incomings;
using Presentation.Commons.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace Presentation.ExtensionMethods.Others
{
    public static class HttpContextExtension
    {
        public static bool IsAuthenticated(this HttpContext context)
        {
            return !Equals(objA: context.User.Identity, objB: null)
                && context.User.Identity.IsAuthenticated;
        }

        public static Guid GetCustomerIdFromCookie(this HttpContext context)
        {
            var cookies = context.Request.Cookies;
            var customerIdCookie = cookies
                .FirstOrDefault(predicate: cookie => cookie.Key.Equals(CookieNames.CustomerId));

            if (string.IsNullOrEmpty(customerIdCookie.Value))
            {
                return Guid.Empty;
            }

            Guid.TryParse(customerIdCookie.Value, out Guid customerId);
            return customerId;
        }

        public static void RemoveCustomerIdCookie(this HttpContext context)
        {
            context.Response.Cookies.Delete(CookieNames.CustomerId);
        }

        public static Guid GetUserId(this HttpContext context)
        {
            var subClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (subClaim != null)
            {
                Guid.TryParse(subClaim.Value, out var userId);
                return userId;
            }

            return Guid.Empty;
        }

        #region Shopping Cart section
        public static void RemoveShoppingCartCookie(this HttpContext context)
        {
            context.Response.Cookies.Delete(CookieNames.ShoppingCart);
        }

        public static ShoppingCartDto GetShoppingCart(this HttpContext context)
        {
            var cookies = context.Request.Cookies;
            
            var shoppingCartCookie = cookies
                .FirstOrDefault(cookie => cookie.Key.Equals(CookieNames.ShoppingCart));

            var shoppingCart = ShoppingCartDto.ConvertFromJson(shoppingCartCookie.Value);

            // Check if the shopping cart is set with customerId.
            bool isCartContainedCustomerId = !shoppingCart.CustomerId.Equals(Guid.Empty);

            if (isCartContainedCustomerId)
            {
                return shoppingCart;
            }

            shoppingCart.CustomerId = context.GetCustomerIdFromCookie();

            return shoppingCart;
        }

        /// <summary>
        ///     Set the shopping cart object to the shopping-cart cookie.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="shoppingCartDto"></param>
        public static void SetShoppingCart(
            this HttpContext context,
            ShoppingCartDto shoppingCartDto)
        {
            var key = CookieNames.ShoppingCart;
            var value = shoppingCartDto.ToJson();
            var cookieOptions = new CookieOptions
            {
                MaxAge = TimeSpan.FromDays(7),
                SameSite = SameSiteMode.Strict
            };

            context.Response.Cookies.Append(key, value, cookieOptions);
        }
        #endregion
    }
}
