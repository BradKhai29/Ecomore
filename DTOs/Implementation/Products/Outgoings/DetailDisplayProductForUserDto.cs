using DTOs.Implementation.Categories.Outgoings;
using DTOs.Implementation.ProductStatuses.Outgoings;
using System;
using System.Collections.Generic;

namespace DTOs.Implementation.Products.Outgoings
{
    public class DetailDisplayProductForUserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public GetCategoryByIdDto Category { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public int QuantityInStock { get; set; }

        public GetProductStatusByIdDto ProductStatus { get; set; }

        public bool IsAvailable { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }
    }
}
