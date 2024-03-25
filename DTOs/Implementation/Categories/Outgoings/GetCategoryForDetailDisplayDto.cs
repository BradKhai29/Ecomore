using DTOs.Implementation.Products.Outgoings;
using System;
using System.Collections.Generic;

namespace DTOs.Implementation.Categories.Outgoings
{
    public class GetCategoryForDetailDisplayDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<GetProductByIdDto> BelongingProducts { get; set; }
    }
}
