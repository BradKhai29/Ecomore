using BusinessLogic.Services.Cores.Base;
using DTOs.Implementation.Categories.Outgoings;
using DTOs.Implementation.Products.Outgoings;
using DTOs.Implementation.ProductStatuses.Outgoings;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Pages.Shared.Components.TopProducts
{
    [ViewComponent]
    public class TopProducts : ViewComponent
    {
        // Backing fields.
        private readonly IProductHandlingService _productHandlingService;

        public TopProducts(IProductHandlingService productHandlingService)
        {
            _productHandlingService = productHandlingService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            var products = await _productHandlingService.GetTopProductsAsync(
                pageSize: 4,
                cancellationToken: cancellationToken);

            var detailDisplayProducts = products.Select(product => new DetailDisplayProductForUserDto()
            {
                Id = product.Id,
                Category = new GetCategoryByIdDto
                {
                    Id = product.CategoryId,
                    Name  = product.Category.Name,
                },
                Name = product.Name,
                ProductStatus = new GetProductStatusByIdDto
                {
                    Id = product.ProductStatusId,
                    Name = product.ProductStatus.Name
                },
                UnitPrice = product.UnitPrice,
                ImageUrls = product.ProductImages.Select(image => image.StorageUrl),
            });

            ViewData[nameof(TopProducts)] = detailDisplayProducts;

            return View(nameof(TopProducts));
        }
    }
}
