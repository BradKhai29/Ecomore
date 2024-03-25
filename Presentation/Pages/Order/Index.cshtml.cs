using DataAccess.DbContexts;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Orders.Outgoings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ExtensionMethods.HttpContexts;

namespace Presentation.Pages.Order
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        public IEnumerable<GetOrderByIdDto> Orders { get; set; }
        public bool HasOrder { get; set; }

        public IndexModel(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            bool isUserAuthenticated = HttpContext.IsUserAuthenticated();

            if (isUserAuthenticated)
            {
                var userId = HttpContext.GetUserId();
                var orders = await _unitOfWork.OrderRepository.GetAllByExpressionAsync(
                    findExpresison: order => order.UserId == userId,
                    cancellationToken: cancellationToken);

                Orders = orders.Select(order => new GetOrderByIdDto
                {
                    OrderId = order.Id,
                    CreatedAt = order.CreatedAt.ToUniversalTime(),
                    TotalItems = order.OrderItems.Count(),
                    TotalPrice = order.TotalPrice,
                    StatusId = order.StatusId,
                    StatusName = order.Status.Name
                });
            }
            else
            {
                var guestId = HttpContext.GetShoppingCart().GuestId;
                var orders = await _unitOfWork.OrderRepository.GetAllByExpressionAsync(
                    findExpresison: order => order.GuestId == guestId,
                    cancellationToken: cancellationToken);

                Orders = orders.Select(order => new GetOrderByIdDto
                {
                    OrderId = order.Id,
                    CreatedAt = order.CreatedAt.ToUniversalTime(),
                    TotalItems = order.OrderItems.Count(),
                    TotalPrice = order.TotalPrice,
                    StatusId = order.StatusId,
                    StatusName = order.Status.Name
                });
            }

            HasOrder = (Orders != null) && (Orders.Count() > 0);

            return Page();
        }
    }
}
