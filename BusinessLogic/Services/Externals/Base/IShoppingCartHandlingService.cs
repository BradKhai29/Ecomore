using DataAccess.Entities;
using DTOs.Implementation.ShoppingCarts.Incomings;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Externals.Base
{
    public interface IShoppingCartHandlingService
    {
        Task<IEnumerable<ProductEntity>> GetProductsFromShoppingCartAsync(
            ShoppingCartDto shoppingCartDto,
            CancellationToken cancellationToken);
    }
}
