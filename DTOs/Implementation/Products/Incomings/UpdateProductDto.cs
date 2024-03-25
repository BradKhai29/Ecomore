using DTOs.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Implementation.Products.Incomings
{
    public class UpdateProductDto
    {
        [Required]
        [IsValidGuid]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Product Name cannot be empty.")]
        public string Name { get; set; }

        [Required]
        [IsValidGuid]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Product Description cannot be empty.")]
        public string Description { get; set; }

        [Required]
        [Range(minimum: 10000, maximum: int.MaxValue, ErrorMessage = "Unit Price value must be larger than 10.000 VND")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Quantity in stock must be larger than 0.")]
        public int QuantityInStock { get; set; }

        [Required]
        [IsValidGuid]
        public Guid ProductStatusId { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }

        public IFormFile[] ProductImageFiles { get; set; }
    }
}
