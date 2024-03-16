using DTOs.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTOs.Implementation.ShoppingCarts.Incomings
{
    public class CartItemDto
    {
        [Required]
        [IsValidGuid]
        public Guid ProductId { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int Quantity { get; set; }

        [JsonIgnore]
        public string ProductName { get; set; }

        [JsonIgnore]
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public decimal UnitPrice { get; set; }

        [JsonIgnore]
        public decimal SubTotal => UnitPrice * Quantity;
    }
}
