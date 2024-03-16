using DataAccess.Repositories.Base;

namespace DataAccess.UnitOfWorks.Base
{
    /// <summary>
    ///     The base interface to implement UnitOfWork with repository.
    /// </summary>
    public interface IUnitOfWorkRepositoryProvider
    {
        IAccountStatusRepository AccountStatusRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        IOrderGuestDetailRepository OrderGuestDetailRepository { get; }

        IOrderItemRepository OrderItemRepository { get; }

        IOrderRepository OrderRepository { get; }

        IOrderStatusRepository OrderStatusRepository { get; }

        IPaymentMethodRepository PaymentMethodRepository { get; }

        IProductImageRepository ProductImageRepository { get; }

        IProductRepository ProductRepository { get; }

        IRoleRepository RoleRepository { get; }

        ISystemAccountRepository SystemAccountRepository { get; }

        ISystemAccountRoleRepository SystemAccountRoleRepository { get; }

        ITokenTypeRepository TokenTypeRepository { get; }

        IUserRepository UserRepository { get; }

        IUserRoleRepository UserRoleRepository { get; }

        IUserTokenRepository UserTokenRepository { get; }
    }
}
