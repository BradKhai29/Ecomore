using DTOs.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Implementation.Products.Incomings
{
    public class CreateProductDto :
        IDtoNormalization
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int QuantityInStock { get; set; }

        [Required]
        public IFormFile[] ProductImages { get; set; }

        public void NormalizeAllProperties()
        {
            Name = Name.Trim();
            Description = Description.Trim();
        }
    }
}
