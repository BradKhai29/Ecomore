using DataAccess.DataSeedings;
using DataAccess.DbContexts;
using DataAccess.Entities;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Categories.Outgoings;
using DTOs.Implementation.Products.Incomings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Areas.Admin.Pages.Product
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public bool IsProductExisted { get; set; }
        public bool IsInUpdateMode { get; set; }

        public UpdateProductDto Product { get; set; }

        public IEnumerable<GetCategoryByIdDto> Categories { get; set; }

        public EditModel(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync(
            [Required]
            Guid productId,
            CancellationToken cancellationToken)
        {
            IsProductExisted = await _unitOfWork.ProductRepository.IsFoundByExpressionAsync(
                findExpresison: product => product.Id == productId,
                cancellationToken: cancellationToken);

            if (!IsProductExisted)
            {
                return Page();
            }

            IsInUpdateMode = true;

            var foundProduct = await _unitOfWork.ProductRepository.FindByIdAsync(
                id: productId,
                asNoTracking: true,
                cancellationToken: cancellationToken);

            Product = new UpdateProductDto
            {
                Id = foundProduct.Id,
                Name = foundProduct.Name,
                CategoryId = foundProduct.CategoryId,
                Description = foundProduct.Description,
                ImageUrls = foundProduct.ProductImages.Select(image => image.StorageUrl),
                QuantityInStock = foundProduct.QuantityInStock,
                UnitPrice = foundProduct.UnitPrice,
                ProductStatusId = foundProduct.ProductStatusId,
            };

            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(
                cancellationToken: cancellationToken);

            Categories = categories.Select(category => new GetCategoryByIdDto
            {
                Id = category.Id,
                Name = category.Name,
            });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            UpdateProductDto product,
            CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(
            cancellationToken: cancellationToken);

            if (!ModelState.IsValid)
            {
                IsProductExisted = true;
                Product = product;

                Categories = categories.Select(category => new GetCategoryByIdDto
                {
                    Id = category.Id,
                    Name = category.Name,
                });

                return Page();
            }

            IsProductExisted = await _unitOfWork.ProductRepository.IsFoundByExpressionAsync(
                foundProduct => foundProduct.Id == product.Id,
                cancellationToken: cancellationToken);

            if (!IsProductExisted)
            {
                return Page();
            }

            ViewData["productId"] = product.Id;

            return Page();
        }
    }
}
