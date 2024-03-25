using DataAccess.Entities;
using DTOs.Implementation.ShoppingCarts.Incomings;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Externals.Base
{
    public interface IShoppingCartHandlingService
    {
        /// <summary>
        ///     Return a list contains all cart-items in the shopping cart.
        /// </summary>
        /// <param name="shoppingCartDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<CartItemDto>> GetItemsFromShoppingCartAsync(
            ShoppingCartDto shoppingCartDto,
            CancellationToken cancellationToken);
    }
}
