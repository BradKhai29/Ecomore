using DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Implementation.Categories.Incomings
{
    public class CreateCategoryDto :
        IDtoNormalization
    {
        [Required]
        public string Name { get; set; }

        public void NormalizeAllProperties()
        {
            Name = Name.Trim();
        }
    }
}
