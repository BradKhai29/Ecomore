using BusinessLogic.Services.Externals.Base;
using DataAccess.Commons.SystemConstants;
using DataAccess.DataSeedings;
using DataAccess.DbContexts;
using DataAccess.Entities;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Orders.Incomings;
using DTOs.Implementation.ShoppingCarts.Incomings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Presentation.ExtensionMethods.HttpContexts;

namespace Presentation.Pages.CheckOut
{
    public class IndexModel : PageModel
    {
        // Backing fields.
        private readonly IShoppingCartHandlingService _shoppingCartHandlingService;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public IEnumerable<CartItemDto> CartItems { get; set; }
        public decimal Total { get; set; }

        [BindProperty]
        public OrderBillingDetailDto BillingDetail { get; set; }

        public IndexModel(
            IShoppingCartHandlingService shoppingCartHandlingService,
            IUnitOfWork<AppDbContext> unitOfWork)
        {
            _shoppingCartHandlingService = shoppingCartHandlingService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var shoppingCart = HttpContext.GetShoppingCart();
            var isUserAuthenticated = HttpContext.IsUserAuthenticated();

            if (isUserAuthenticated)
            {
                var userId = HttpContext.GetUserId();
                var foundUser = await _unitOfWork.UserRepository.FindByExpressionAsync(
                    findExpression: user => user.Id == userId,
                    cancellationToken: cancellationToken);
                BillingDetail = new OrderBillingDetailDto();

                BillingDetail.FirstName = foundUser.FirstName();
                BillingDetail.LastName = foundUser.LastName();
                BillingDetail.Email = foundUser.Email;
            }

            if (shoppingCart.IsEmpty())
            {
                CartItems = new List<CartItemDto>();
            }

            CartItems = await _shoppingCartHandlingService.GetItemsFromShoppingCartAsync(
                shoppingCartDto: shoppingCart,
                cancellationToken: cancellationToken);

            Total = CartItems.Sum(item => item.SubTotal);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var shoppingCart = HttpContext.GetShoppingCart();

            if (shoppingCart.IsEmpty())
            {
                CartItems = Enumerable.Empty<CartItemDto>();
                return Page();
            }

            BillingDetail.NormalizeAllProperties();

            var cartItems = await _shoppingCartHandlingService.GetItemsFromShoppingCartAsync(
                shoppingCartDto: shoppingCart,
                cancellationToken: cancellationToken);

            var totalPrice = cartItems.Sum(item => item.SubTotal);

            var isUserAuthenticated = HttpContext.IsUserAuthenticated();

            var userId = isUserAuthenticated ? HttpContext.GetUserId() : AppUsers.DoNotRemove.Id;
            var guestId = isUserAuthenticated ? userId : shoppingCart.GuestId;

            var order = new OrderEntity
            {
                Id = Guid.NewGuid(),
                TransactionCode = DateTime.UtcNow.ToLongDateString(),
                StatusId = OrderStatuses.Pending.Id,
                UserId = userId,
                GuestId = guestId,
                PaymentMethodId = PaymentMethods.OnlineBanking.Id,
                DeliveredAt = DateTime.UtcNow,
                OrderNote = BillingDetail.UserNote,
                TotalPrice = totalPrice,
                DeliveredAddress = BillingDetail.DeliveryAddress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = DefaultValues.SystemId,
            };

            var orderItems = new List<OrderItemEntity>(capacity: cartItems.Count());
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItemEntity
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    SellingPrice = item.UnitPrice,
                    SellingQuantity = item.Quantity,
                };

                orderItems.Add(orderItem);
            }

            var success = false;
            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(operation: async () =>
            {
                await _unitOfWork.CreateTransactionAsync(cancellationToken: cancellationToken);

                try
                {
                    await _unitOfWork.OrderRepository.AddAsync(order, cancellationToken);

                    if (!HttpContext.IsUserAuthenticated())
                    {
                        var orderGuestDetail = new OrderGuestDetailEntity
                        {
                            OrderId = order.Id,
                            Email = BillingDetail.Email,
                            GuestName = $"{BillingDetail.FirstName}{UserEntity.NameSeparator}{BillingDetail.LastName}",
                            PhoneNumber = BillingDetail.PhoneNumber,
                        };
                        
                        await _unitOfWork.OrderGuestDetailRepository.AddAsync(orderGuestDetail, cancellationToken);
                    }

                    await _unitOfWork.OrderItemRepository.AddRangeAsync(orderItems, cancellationToken);

                    await _unitOfWork.SaveChangesToDatabaseAsync(cancellationToken);

                    await _unitOfWork.CommitTransactionAsync(cancellationToken);

                    success = true;
                }
                catch
                {
                    await _unitOfWork.RollBackTransactionAsync(
                        cancellationToken: cancellationToken);
                }
                finally
                {
                    await _unitOfWork.DisposeTransactionAsync(
                        cancellationToken: cancellationToken);
                }
            });

            if (success)
            {
                shoppingCart.Clear();
                HttpContext.SetShoppingCart(shoppingCart);
            }

            return RedirectToPage("/Index");
        }
    }
}
