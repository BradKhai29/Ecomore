using BusinessLogic.Services.Cores.Base;
using DTOs.Implementation.ShoppingCarts.Incomings;
using Microsoft.AspNetCore.Mvc;
using Presentation.ExtensionMethods.HttpContexts;
using Presentation.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IUserAuthHandlingService _userAuthHandlingService;

        public OrderController(IUserAuthHandlingService userAuthHandlingService)
        {
            _userAuthHandlingService = userAuthHandlingService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOutAsync(CancellationToken cancellationToken)
        {
            var shoppingCart = HttpContext.GetShoppingCart();

            if (shoppingCart.IsEmpty())
            {
                return BadRequest(CommonResponse.Failed(new List<string>
                {
                    "The shopping cart is empty."
                }));
            }

            return Ok();
        }
    }
}
