using BusinessLogic.Services.Externals.Base;
using DataAccess.DbContexts;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.ShoppingCarts.Incomings;
using System;
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

        public async Task<IEnumerable<CartItemDto>> GetItemsFromShoppingCartAsync(
            ShoppingCartDto shoppingCartDto,
            CancellationToken cancellationToken)
        {
            var invalidIds = new List<Guid>();

            foreach (var cartItem in shoppingCartDto.CartItems)
            {
                Guid productId = cartItem.ProductId;

                var foundProduct = await _unitOfWork.ProductRepository.FindByIdAsync(
                    id: productId,
                    asNoTracking: true,
                    cancellationToken: cancellationToken);

                // If no product is found, remove the invalid item from the shopping cart.
                if (Equals(foundProduct, null))
                {
                    invalidIds.Add(productId);

                    continue;
                }

                cartItem.ProductName = foundProduct.Name;
                cartItem.UnitPrice = foundProduct.UnitPrice;
                cartItem.ImageUrl = foundProduct.ProductImages.First().StorageUrl;
            }

            invalidIds.ForEach(shoppingCartDto.RemoveItem);

            return shoppingCartDto.CartItems;
        }
    }
}
