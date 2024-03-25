using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Implementation.Categories.Outgoings
{
    public class GetCategoryByIdDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TotalProducts { get; set; }
    }
}
