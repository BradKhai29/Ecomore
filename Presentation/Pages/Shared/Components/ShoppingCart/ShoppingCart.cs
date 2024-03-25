using BusinessLogic.Services.Externals.Base;
using Microsoft.AspNetCore.Mvc;
using Presentation.ExtensionMethods.HttpContexts;

namespace Presentation.Pages.Shared.Components.ShoppingCart
{
    [ViewComponent]
    public class ShoppingCart : ViewComponent
    {
        // Backing fields.
        private readonly IShoppingCartHandlingService _shoppingCartHandlingService;

        public ShoppingCart(
            IShoppingCartHandlingService shoppingCartHandlingService)
        {
            _shoppingCartHandlingService = shoppingCartHandlingService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var shoppingCart = HttpContext.GetShoppingCart();

            var cartItems = await _shoppingCartHandlingService.GetItemsFromShoppingCartAsync(
                shoppingCartDto: shoppingCart,
                cancellationToken: cancellationToken);

            ViewData[nameof(ShoppingCart)] = cartItems;

            return View(nameof(ShoppingCart));
        }
    }
}
