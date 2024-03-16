using BusinessLogic.Services.Externals.Base;
using DataAccess.DbContexts;
using DataAccess.Entities;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.ShoppingCarts.Incomings;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Externals.Implementation
{
    internal class ShoppingCartHandlingService :
        IShoppingCartHandlingService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public ShoppingCartHandlingService(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsFromShoppingCartAsync(
            ShoppingCartDto shoppingCartDto,
            CancellationToken cancellationToken)
        {
            var products = new List<ProductEntity>(capacity: shoppingCartDto.NumberOfItems());

            foreach (var cartItem in shoppingCartDto.CartItems)
            {
                var productId = cartItem.ProductId;

                var foundProduct = await _unitOfWork.ProductRepository.FindByIdAsync(
                    id: productId,
                    asNoTracking: true,
                    cancellationToken: cancellationToken);

                // If no product is found, remove the
                // invalid item from the shopping cart.
                if (Equals(foundProduct, null))
                {
                    shoppingCartDto.RemoveItem(productId);

                    continue;
                }

                if (!Equals(foundProduct, null))
                {
                    foundProduct.QuantityInStock = cartItem.Quantity;

                    products.Add(foundProduct);
                }
            }

            return products;
        }
    }
}
