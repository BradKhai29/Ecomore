using DTOs.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;

namespace DTOs.Implementation.ShoppingCarts.Incomings
{
    public class ShoppingCartDto
    {
        [Required]
        [IsValidGuid]
        public Guid CartId { get; set; }

        [Required]
        [IsValidGuid]
        public Guid CustomerId { get; set; }

        public IList<CartItemDto> CartItems { get; set; }

        public static ShoppingCartDto ConvertFromJson(string jsonValue)
        {
            if (string.IsNullOrEmpty(jsonValue))
            {
                return new ShoppingCartDto
                {
                    CartId = Guid.NewGuid(),
                    CartItems = new List<CartItemDto>()
                };
            }

            try
            {
                var shoppingCart = JsonSerializer.Deserialize<ShoppingCartDto>(jsonValue);

                return shoppingCart;
            }
            catch (Exception)
            {
                return new ShoppingCartDto
                {
                    CartId = Guid.NewGuid(),
                    CartItems = new List<CartItemDto>()
                };
            }
        }

        public void RemoveInvalidItems()
        {
            if (CartItems != null && CartItems.Count > 0)
            {
                foreach (var item in CartItems)
                {
                    var inValid = item.Quantity <= 0;

                    if (inValid)
                    {
                        CartItems.Remove(item);
                    }
                }
            }
        }

        public bool IsEmpty()
        {
            return CartItems.Count == 0;
        }

        /// <summary>
        ///     Verify if the input <paramref name="customerId"/> is
        ///     similar with this shopping cart <see cref="CustomerId"/>
        /// </summary>
        /// <param name="customerId">
        ///     The input customerId to verify.
        /// </param>
        /// <returns>
        ///     The verification result.
        /// </returns>
        public bool VerifyCustomerId(Guid customerId)
        {
            return CustomerId.Equals(customerId);
        }

        public int NumberOfItems()
        {
            if (CartItems == null)
            {
                return 0;
            }

            return CartItems.Count;
        }

        public int GetItemQuantity(Guid productId)
        {
            var product = CartItems
                .FirstOrDefault(item => item.ProductId.Equals(productId));

            if (product == null)
            {
                return 0;
            }

            return product.Quantity;
        }

        public void AddItem(CartItemDto cartItem)
        {
            if (Equals(CartItems, null))
            {
                CartItems = new List<CartItemDto>
                {
                    cartItem
                };

                return;
            }

            bool isItemExisted = false;
            for (int itemIndex = 0; itemIndex < CartItems.Count; itemIndex++)
            {
                var existedItem = CartItems[itemIndex];

                if (existedItem.ProductId.Equals(cartItem.ProductId))
                {
                    isItemExisted = true;

                    existedItem.Quantity += cartItem.Quantity;
                    existedItem.UnitPrice = cartItem.UnitPrice;

                    break;
                }
            }

            if (!isItemExisted)
            {
                CartItems.Add(cartItem);
            }
        }

        public void RemoveItem(Guid productId)
        {
            if (Equals(CartItems, null) || IsEmpty())
            {
                return;
            }

            for (int itemIndex = 0; itemIndex < CartItems.Count; itemIndex++)
            {
                var existedItem = CartItems[itemIndex];

                if (existedItem.ProductId.Equals(productId))
                {
                    CartItems.RemoveAt(itemIndex);

                    break;
                }
            }
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
