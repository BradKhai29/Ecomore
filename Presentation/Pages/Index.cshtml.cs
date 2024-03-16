using BusinessLogic.Services.Cores.Base;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class IndexModel : PageModel
    {
        // Backing fields.
        private readonly IProductHandlingService _productHandlingService;

        // Properties.
        public IEnumerable<ProductEntity> Products { get; set; }

        public IndexModel(
            IProductHandlingService productHandlingService)
        {
            _productHandlingService = productHandlingService;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            Products = await _productHandlingService.GetAllAsync(cancellationToken: cancellationToken);
            
            //Products = new List<ProductEntity>();
            return Page();
        }
    }
}
