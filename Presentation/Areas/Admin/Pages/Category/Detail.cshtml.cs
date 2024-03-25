using DataAccess.DbContexts;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Categories.Outgoings;
using DTOs.Implementation.Products.Outgoings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Areas.Admin.Pages.Category
{
    public class DetailModel : PageModel
    {
        // Backing fields.
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public bool IsCategoryExisted { get; set; }

        public GetCategoryForDetailDisplayDto Category { get; set; }

        public DetailModel(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync(
            [Required]
            [FromRoute]
            Guid categoryId,
            CancellationToken cancellationToken)
        {
            IsCategoryExisted = await _unitOfWork.CategoryRepository.IsFoundByExpressionAsync(
                findExpresison: category => category.Id == categoryId,
                cancellationToken: cancellationToken);

            if (!IsCategoryExisted)
            {
                return Page();
            }

            var category = await _unitOfWork.CategoryRepository.GetForDetailDisplayByIdAsync(
                id: categoryId,
                cancellationToken: cancellationToken);

            Category = new GetCategoryForDetailDisplayDto
            {
                Id = category.Id,
                Name = category.Name,
                BelongingProducts = category.Products.Select(product => new GetProductByIdDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    ImageUrls = product.ProductImages.Select(image => image.StorageUrl),
                    UnitPrice = product.UnitPrice,
                    QuantityInStock = product.QuantityInStock,
                    ProductStatusId = product.ProductStatusId,
                })
            };

            return Page();
        }
    }
}
