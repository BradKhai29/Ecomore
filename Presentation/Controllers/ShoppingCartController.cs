using BusinessLogic.Services.Cores.Base;
using DTOs.Implementation.ShoppingCarts.Incomings;
using DTOs.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using Presentation.ExtensionMethods.Others;
using Presentation.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Presentation.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IProductHandlingService _productHandlingService;

        public ShoppingCartController(IProductHandlingService productHandlingService)
        {
            _productHandlingService = productHandlingService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItemToCart(
            [FromBody] CartItemDto cartItemDto,
            CancellationToken cancellationToken)
        {
            // Verify the customerId cookie from the current HttpContext.
            var customerId = HttpContext.GetCustomerIdFromCookie();

            if (customerId.Equals(Guid.Empty))
            {
                HttpContext.RemoveCustomerIdCookie();

                return StatusCode(
                    statusCode: StatusCodes.Status400BadRequest,
                    value: CommonResponse.Failed(new List<string>
                    {
                        "Invalid customerId cookie."
                    }));
            }

            // Verify the shopping cart cookie is valid or not.
            var shoppingCart = HttpContext.GetShoppingCart();

            if (!shoppingCart.VerifyCustomerId(customerId))
            {
                HttpContext.RemoveShoppingCartCookie();

                return StatusCode(
                    statusCode: StatusCodes.Status400BadRequest,
                    value: CommonResponse.Failed(new List<string>
                    {
                        "Invalid shopping-cart cookie."
                    }));
            }

            // Verify if the productItem is existed or not.
            var productId = cartItemDto.ProductId;

            var isProductExisted = await _productHandlingService.IsProductExistedByIdAsync(
                productId: productId,
                cancellationToken: cancellationToken);

            if (!isProductExisted)
            {
                return StatusCode(
                    statusCode: StatusCodes.Status404NotFound,
                    value: CommonResponse.Failed(new List<string>
                    {
                        CommonResponse.ResourceNotExistedMessage
                    }));
            }

            // Verify if the add-to-cart-quantity is valid or not.
            var productInStockQuantity = await _productHandlingService.GetQuantityInStockByProductIdAsync(
                productId: productId,
                cancellationToken: cancellationToken);

            var addToCartQuantity = cartItemDto.Quantity + shoppingCart.GetItemQuantity(productId);
            
            var isValidQuantity = addToCartQuantity <= productInStockQuantity;

            if (!isValidQuantity)
            {
                return StatusCode(
                    statusCode: StatusCodes.Status400BadRequest,
                    value: CommonResponse.Failed(new List<string>
                    {
                        "Invalid add-to-cart quantity."
                    }));
            }

            // Add the item to the shopping cart.
            shoppingCart.AddItem(cartItemDto);

            HttpContext.SetShoppingCart(shoppingCart);

            return Ok();
        }

        [HttpDelete("remove/{productId:guid}")]
        public IActionResult RemoveItemFromCart(
            [FromRoute]
            [Required]
            [IsValidGuid] Guid productId)
        {
            // Verify the customerId cookie from the current HttpContext.
            var customerId = HttpContext.GetCustomerIdFromCookie();

            if (customerId.Equals(Guid.Empty))
            {
                HttpContext.RemoveCustomerIdCookie();

                return StatusCode(
                    statusCode: StatusCodes.Status400BadRequest,
                    value: CommonResponse.Failed(new List<string>
                    {
                        "Invalid customerId cookie."
                    }));
            }

            // Verify the shopping cart cookie is valid or not.
            var shoppingCart = HttpContext.GetShoppingCart();

            if (!shoppingCart.VerifyCustomerId(customerId))
            {
                HttpContext.RemoveShoppingCartCookie();

                return StatusCode(
                    statusCode: StatusCodes.Status400BadRequest,
                    value: CommonResponse.Failed(new List<string>
                    {
                        "Invalid shopping-cart cookie."
                    }));
            }

            shoppingCart.RemoveItem(productId);

            HttpContext.SetShoppingCart(shoppingCart);

            return Ok();
        }
    }
}
