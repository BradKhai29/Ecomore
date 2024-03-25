using DTOs.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Implementation.Categories.Incomings
{
    public class UpdateCategoryDto
    {
        [Required]
        [IsValidGuid]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public void NormalizeAllProperties()
        {
            Name = Name.Trim();
        }
    }
}
