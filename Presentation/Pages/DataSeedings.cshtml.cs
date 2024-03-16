using BusinessLogic.Services.Cores.Base;
using DataAccess.Commons.SystemConstants;
using DataAccess.DataSeedings;
using DataAccess.DbContexts;
using DataAccess.Entities;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Auths.Incomings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class DataSeedingsModel : PageModel
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly ISystemAccountAuthHandlingService _systemAccountAuthHandlingService;
        private readonly bool _allowSeeding = false;

        public DataSeedingsModel(
            IUnitOfWork<AppDbContext> unitOfWork,
            ISystemAccountAuthHandlingService systemAccountAuthHandlingService)
        {
            _unitOfWork = unitOfWork;
            _systemAccountAuthHandlingService = systemAccountAuthHandlingService;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            if (!_allowSeeding)
            {
                return Page();
            }

            await AddRoles();
            await AddCategories(cancellationToken);
            await AddAccountStatuses(cancellationToken);
            await AddTokenTypes(cancellationToken);
            await AddOrderStatuses(cancellationToken);
            await AddPaymentMethods(cancellationToken);

            await _unitOfWork.SaveChangesToDatabaseAsync(cancellationToken);

            var registerDto = new RegisterDto
            {
                Username = "system",
                Email = "duongkhai.dev@gmail.com",
                Password = "khai2904"
            };

            var result = await _systemAccountAuthHandlingService.RegisterAsync(
                registerDto: registerDto,
                cancellationToken: cancellationToken);

            if (result.IsSuccess)
            {
                await AddProducts(cancellationToken);
            }

            return Page();
        }

        private async Task AddRoles()
        {
            var roles = new List<RoleEntity>
            {
                new()
                {
                    Id = Roles.System.Id,
                    Name = Roles.System.Name
                },
                new()
                {
                    Id = Roles.Admin.Id,
                    Name = Roles.Admin.Name
                },
                new()
                {
                    Id = Roles.Employee.Id,
                    Name = Roles.Employee.Name
                },
                new()
                {
                    Id = Roles.DoNotRemove.Id,
                    Name = Roles.DoNotRemove.Name
                }
            };

            foreach (var role in roles)
            {
                await _unitOfWork.RoleRepository.AddAsync(role);
            }
        }

        private async Task AddCategories(CancellationToken cancellationToken)
        {
            var categories = new List<CategoryEntity>
            {
                new()
                {
                    Id = Categories.ProcessedNut.Id,
                    Name = Categories.ProcessedNut.Name
                }
            };

            await _unitOfWork.CategoryRepository.AddRangeAsync(categories, cancellationToken);
        }

        private async Task AddAccountStatuses(CancellationToken cancellationToken)
        {
            var statuses = new List<AccountStatusEntity>
            {
                new()
                {
                    Id = AccountStatuses.PendingConfirmed.Id,
                    Name = AccountStatuses.PendingConfirmed.Name
                },
                new()
                {
                    Id = AccountStatuses.EmailConfirmed.Id,
                    Name = AccountStatuses.EmailConfirmed.Name
                },
                new()
                {
                    Id = AccountStatuses.Banned.Id,
                    Name = AccountStatuses.Banned.Name
                }
            };

            await _unitOfWork.AccountStatusRepository.AddRangeAsync(statuses, cancellationToken);
        }

        private async Task AddTokenTypes(CancellationToken cancellationToken)
        {
            var tokenTypes = new List<TokenTypeEntity>
            {
                new()
                {
                    Id = TokenTypes.RefreshToken.Id,
                    Name = TokenTypes.RefreshToken.Name
                },
                new()
                {
                    Id = TokenTypes.ResetPasswordToken.Id,
                    Name = TokenTypes.ResetPasswordToken.Name
                }
            };

            await _unitOfWork.TokenTypeRepository.AddRangeAsync(tokenTypes, cancellationToken);
        }

        private async Task AddOrderStatuses(CancellationToken cancellationToken)
        {
            var orderStatuses = new List<OrderStatusEntity>
            {
                new()
                {
                    Id = OrderStatuses.Pending.Id,
                    Name = OrderStatuses.Pending.Name
                },
                new()
                {
                    Id = OrderStatuses.InProcessing.Id,
                    Name = OrderStatuses.InProcessing.Name
                },
                new()
                {
                    Id = OrderStatuses.Done.Id,
                    Name = OrderStatuses.Done.Name
                },
                new()
                {
                    Id = OrderStatuses.Cancelled.Id,
                    Name = OrderStatuses.Cancelled.Name
                }
            };

            await _unitOfWork.OrderStatusRepository.AddRangeAsync(orderStatuses, cancellationToken);
        }

        private async Task AddPaymentMethods(CancellationToken cancellationToken)
        {
            var paymentMethods = new List<PaymentMethodEntity>
            {
                new()
                {
                    Id = PaymentMethods.OnlineBanking.Id,
                    Name = PaymentMethods.OnlineBanking.Name
                },
                new()
                {
                    Id = PaymentMethods.CashOnDelivery.Id,
                    Name = PaymentMethods.CashOnDelivery.Name
                }
            };

            await _unitOfWork.PaymentMethodRepository.AddRangeAsync(paymentMethods, cancellationToken);
        }

        private async Task AddProducts(CancellationToken cancellationToken)
        {
            var categoryId = Categories.ProcessedNut.Id;
            var products = new List<ProductEntity>();

            for (int i = 1; i <= 20; i++)
            {
                ProductEntity product = new()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = categoryId,
                    Name = $"Hat dinh duong [{i}]",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = DefaultValues.SystemId,
                    IsAvailable = true,
                    Description = "San pham ngon",
                    QuantityInStock = 100,
                    UnitPrice = 50_000,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = DefaultValues.SystemId
                };

                ProductImageEntity productImage = new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    FileName = $"hat_dinh_duong_{i}.jpg",
                    StorageUrl = "https://nangmo.vn/wp-content/uploads/2021/04/mix-hat-dinh-duong-4.jpg"
                };

                product.ProductImages = new List<ProductImageEntity>()
                {
                    productImage
                };

                products.Add(product);
            }

            await _unitOfWork.ProductRepository.AddRangeAsync(products, cancellationToken);
            await _unitOfWork.SaveChangesToDatabaseAsync(cancellationToken);
        }
    }
}