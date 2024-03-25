using BusinessLogic.Services.Cores.Base;
using DTOs.Implementation.Categories.Outgoings;
using DTOs.Implementation.Products.Outgoings;
using DTOs.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Product
{
    public class DetailModel : PageModel
    {
        // Backing fields.
        private readonly IProductHandlingService _productHandlingService;

        public DetailDisplayProductForUserDto Product { get; set; }

        public DetailModel(IProductHandlingService productHandlingService)
        {
            _productHandlingService = productHandlingService;
        }

        public async Task<IActionResult> OnGetAsync(
            [FromRoute]
            [IsValidGuid] Guid productId,
            CancellationToken cancellationToken)
        {
            var isProductExisted = await _productHandlingService.IsProductExistedByIdAsync(
                productId: productId,
                cancellationToken: cancellationToken);

            if (!isProductExisted)
            {
                return RedirectToPage("/Index");
            }

            var foundProduct = await _productHandlingService.FindByIdAsync(
                productId: productId,
                cancellationToken: cancellationToken);

            Product = new DetailDisplayProductForUserDto
            {
                Id = foundProduct.Id,
                Category = new GetCategoryByIdDto
                {
                    Id = foundProduct.Category.Id,
                    Name = foundProduct.Category.Name,
                },
                Name = foundProduct.Name,
                Description = foundProduct.Description,
                ProductStatus = new DTOs.Implementation.ProductStatuses.Outgoings.GetProductStatusByIdDto
                {
                    Id = foundProduct.ProductStatus.Id,
                    Name = foundProduct.ProductStatus.Name
                },
                QuantityInStock = foundProduct.QuantityInStock,
                UnitPrice = foundProduct.UnitPrice,
                ImageUrls = foundProduct.ProductImages.Select(image => image.StorageUrl)
            };

            return Page();
        }
    }
}
