using DataAccess.DataSeedings;
using DataAccess.DbContexts;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Products.Outgoings;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Pages.Shared.Components.ProductDetail
{
    [ViewComponent]
    public class ProductDetail : ViewComponent
    {
        // Backing Fields.
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public ProductDetail(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            Guid.TryParse($"{ViewData["productId"]}", out var productId);

            bool isFound = await _unitOfWork.ProductRepository.IsFoundByExpressionAsync(
                findExpresison: product => product.Id == productId,
                cancellationToken: cancellationToken);

            var view = View(nameof(ProductDetail));

            if (!isFound)
            {
                return view;
            }

            var foundProduct = await _unitOfWork.ProductRepository.FindByIdAsync(
                id: productId,
                asNoTracking: true,
                cancellationToken: cancellationToken);

            var Product = new DetailDisplayProductForUserDto
            {
                Id = foundProduct.Id,
                Name = foundProduct.Name,
                Category = new DTOs.Implementation.Categories.Outgoings.GetCategoryByIdDto
                {
                    Id = foundProduct.Category.Id,
                    Name = foundProduct.Category.Name,
                },
                Description = foundProduct.Description,
                ImageUrls = foundProduct.ProductImages.Select(image => image.StorageUrl),
                QuantityInStock = foundProduct.QuantityInStock,
                UnitPrice = foundProduct.UnitPrice,
                ProductStatus = new DTOs.Implementation.ProductStatuses.Outgoings.GetProductStatusByIdDto
                {
                    Id = foundProduct.ProductStatus.Id,
                    Name = foundProduct.ProductStatus.Name
                },
            };

            ViewData[nameof(Product)] = Product;

            return view;
        }
    }
}
