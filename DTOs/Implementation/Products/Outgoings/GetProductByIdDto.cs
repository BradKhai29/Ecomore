using System;
using System.Collections.Generic;

namespace DTOs.Implementation.Products.Outgoings
{
    public class GetProductByIdDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public int QuantityInStock { get; set; }

        public Guid ProductStatusId { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }
    }
}
