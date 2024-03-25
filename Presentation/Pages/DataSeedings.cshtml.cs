using BusinessLogic.Services.Cores.Base;
using DataAccess.Commons.SystemConstants;
using DataAccess.DataSeedings;
using DataAccess.DbContexts;
using DataAccess.Entities;
using DataAccess.UnitOfWorks.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages
{
    public class DataSeedingsModel : PageModel
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly ISystemAccountAuthHandlingService _systemAccountAuthHandlingService;
        private readonly IUserAuthHandlingService _userAuthHandlingService;

        public DataSeedingsModel(
            IUnitOfWork<AppDbContext> unitOfWork,
            ISystemAccountAuthHandlingService systemAccountAuthHandlingService,
            IUserAuthHandlingService userAuthHandlingService)
        {
            _unitOfWork = unitOfWork;
            _systemAccountAuthHandlingService = systemAccountAuthHandlingService;
            _userAuthHandlingService = userAuthHandlingService;
        }

        public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
        {
            var notAllowSeeding = await _unitOfWork.SystemAccountRepository.IsFoundByExpressionAsync(
                findExpresison: account => account.Id == DefaultValues.SystemId,
                cancellationToken: cancellationToken);

            if (notAllowSeeding)
            {
                return Page();
            }

            await AddAccountStatuses(cancellationToken);
            await _unitOfWork.SaveChangesToDatabaseAsync(cancellationToken);

            var adminResult = await _systemAccountAuthHandlingService.RegisterDefaultAccountAsync(
                cancellationToken: cancellationToken);

            var user = new UserEntity
            {
                Id = AppUsers.DoNotRemove.Id,
                FullName = $"none{UserEntity.NameSeparator}none",
                UserName = AppUsers.DoNotRemove.UserName,
                PasswordHash = AppUsers.DoNotRemove.PasswordHash,
                AccountStatusId = AccountStatuses.EmailConfirmed.Id,
                Gender = true,
                Email = "abc@gmail.com",
                CreatedAt = DateTime.UtcNow,
                AvatarUrl = "unknown",
                UpdatedAt = DateTime.UtcNow,
                EmailConfirmed = true,
                PhoneNumber = "0123456789",
            };

            var userResult = await _userAuthHandlingService.RegisterSystemUserAsync(user, cancellationToken);

            if (adminResult.IsSuccess && userResult.IsSuccess)
            {
                await AddRoles();
                await AddCategories(cancellationToken);
                await AddProductStatuses(cancellationToken);
                await AddProducts(cancellationToken);
                await AddTokenTypes(cancellationToken);
                await AddOrderStatuses(cancellationToken);
                await AddPaymentMethods(cancellationToken);

                await _unitOfWork.SaveChangesToDatabaseAsync(cancellationToken);
            }

            return Page();
        }

        private async Task AddRoles()
        {
            var roles = new List<RoleEntity>
            {
                new()
                {
                    Id = AppRoles.System.Id,
                    Name = AppRoles.System.Name
                },
                new()
                {
                    Id = AppRoles.Admin.Id,
                    Name = AppRoles.Admin.Name
                },
                new()
                {
                    Id = AppRoles.Employee.Id,
                    Name = AppRoles.Employee.Name
                },
                new()
                {
                    Id = AppRoles.DoNotRemove.Id,
                    Name = AppRoles.DoNotRemove.Name
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
                    Name = Categories.ProcessedNut.Name,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = DefaultValues.SystemId
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

        private async Task AddProductStatuses(CancellationToken cancellationToken)
        {
            var statuses = ProductStatuses.GetValues();

            await _unitOfWork.ProductStatusRepository.AddRangeAsync(statuses, cancellationToken);
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
            var orderStatuses = OrderStatuses.GetValues();

            await _unitOfWork.OrderStatusRepository.AddRangeAsync(orderStatuses, cancellationToken);
        }

        private async Task AddPaymentMethods(CancellationToken cancellationToken)
        {
            var paymentMethods = PaymentMethods.GetValues();

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
                    ProductStatusId = ProductStatuses.InStock.Id,
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