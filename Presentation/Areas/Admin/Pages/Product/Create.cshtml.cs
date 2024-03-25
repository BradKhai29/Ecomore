using DataAccess.Commons.SystemConstants;
using DataAccess.DataSeedings;
using DataAccess.DbContexts;
using DataAccess.Entities;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Categories.Outgoings;
using DTOs.Implementation.Products.Incomings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Areas.Admin.Pages.Product
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateProductDto ProductInfo { get; set; }
        public IEnumerable<GetCategoryByIdDto> Categories { get; set; }
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(IUnitOfWork<AppDbContext> unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            await GetCategoriesAsync(cancellationToken);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            await GetCategoriesAsync(cancellationToken);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Guid productId = Guid.NewGuid();
            const string imageFolder = "images";
            const string productFolder = "products";
            string currentProductFolder = productId.ToString();
            int uploadOrder = 1;

            var productFolderPath = Path.Combine(_webHostEnvironment.WebRootPath,
                    imageFolder,
                    productFolder);
            if(!Directory.Exists(productFolderPath))
            {
                Directory.CreateDirectory(productFolderPath);
            }

            var currentProductFolderPath = Path.Combine(productFolderPath, currentProductFolder);
            if(!Directory.Exists(currentProductFolderPath))
            {
                Directory.CreateDirectory(currentProductFolderPath);
            }
            var productImages = new List<ProductImageEntity>(capacity: ProductInfo.ProductImages.Length);

            foreach(var item in ProductInfo.ProductImages)
            {
                var fileExtensionIndex = item.FileName.LastIndexOf('.');
                var fileExtension = item.FileName.Substring(fileExtensionIndex);
                var actualStorageFileName = $"{uploadOrder}{fileExtension}";

                var storageUrl = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    imageFolder,
                    productFolder,
                    currentProductFolder,
                    actualStorageFileName);

                FileStream fileStream = System.IO.File.OpenWrite(storageUrl);
                await item.CopyToAsync(fileStream);

                productImages.Add(new ProductImageEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    FileName = actualStorageFileName,
                    StorageUrl = storageUrl,
                });
            }

            var newProduct = new ProductEntity
            {
                Id = productId,
                Name = ProductInfo.Name,
                CategoryId = ProductInfo.CategoryId,
                Description = ProductInfo.Description,
                QuantityInStock = ProductInfo.QuantityInStock,
                UnitPrice = ProductInfo.UnitPrice,
                ProductStatusId = ProductStatuses.InStock.Id,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = DefaultValues.SystemId,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = DefaultValues.SystemId,
            };

            await _unitOfWork.ProductRepository.AddAsync(newProduct, cancellationToken);
            await _unitOfWork.ProductImageRepository.AddRangeAsync(productImages, cancellationToken);

            await _unitOfWork.SaveChangesToDatabaseAsync(cancellationToken);

            return RedirectToPage(pageName: "Detail", routeValues: productId);
        }

        private async Task GetCategoriesAsync(CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(
                cancellationToken: cancellationToken);

            Categories = categories.Select(category => new GetCategoryByIdDto
            {
                Id = category.Id,
                Name = category.Name,
            });
        }
    }
}
