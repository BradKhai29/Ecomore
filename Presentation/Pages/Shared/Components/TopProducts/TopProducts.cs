using BusinessLogic.Services.Cores.Base;
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

            ViewData[nameof(TopProducts)] = products;

            return View();
        }
    }
}
