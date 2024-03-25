using DataAccess.DbContexts;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Categories.Outgoings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Areas.Admin.Pages.Category
{
    public class IndexModel : PageModel
    {
        // Backing fields.
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public IEnumerable<GetCategoryByIdDto> Categories { get; set; }

        public IndexModel(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(cancellationToken: cancellationToken);

            Categories = categories.Select(category => new GetCategoryByIdDto
            {
                Id = category.Id,
                Name = category.Name,
                TotalProducts = category.Products.Count(),
            });

            return Page();
        }
    }
}
