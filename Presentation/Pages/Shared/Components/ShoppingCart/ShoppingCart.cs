using BusinessLogic.Services.Externals.Base;
using Microsoft.AspNetCore.Mvc;
using Presentation.ExtensionMethods.Others;

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

            var products = await _shoppingCartHandlingService.GetProductsFromShoppingCartAsync(
                shoppingCartDto: shoppingCart,
                cancellationToken: cancellationToken);

            ViewData[nameof(ShoppingCart)] = products;
            ViewData["Total"] = products.Sum(item => item.QuantityInStock * item.UnitPrice);

            return View();
        }
    }
}
