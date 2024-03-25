using BusinessLogic.Services.Externals.Base;
using DataAccess.Entities;
using DTOs.Implementation.Products.Outgoings;
using DTOs.Implementation.ShoppingCarts.Incomings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ExtensionMethods.HttpContexts;

namespace Presentation.Pages.ShoppingCart
{
    public class IndexModel : PageModel
    {
        // Backing fields.
        private readonly IShoppingCartHandlingService _shoppingCartHandlingService;

        public IEnumerable<CartItemDto> CartItems { get; set; }
        public decimal Total { get; set; }

        public IndexModel(IShoppingCartHandlingService shoppingCartHandlingService)
        {
            _shoppingCartHandlingService = shoppingCartHandlingService;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var shoppingCart = HttpContext.GetShoppingCart();

            if (shoppingCart.IsEmpty())
            {
                CartItems = new List<CartItemDto>();

                return Page();
            }

            CartItems = await _shoppingCartHandlingService.GetItemsFromShoppingCartAsync(
                shoppingCartDto: shoppingCart,
                cancellationToken: cancellationToken);

            Total = CartItems.Sum(item => item.SubTotal);

            return Page();
        }
    }
}
