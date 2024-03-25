using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Implementation;
using DataAccess.UnitOfWorks.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.UnitOfWorks.Implementations;

public class EcommerceAppUnitOfWork<TContext> : IUnitOfWork<TContext>
    where TContext : DbContext
{
    // Backing stores.
    private readonly TContext _dbContext;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<RoleEntity> _roleManager;

    // Backing fields for transaction handling.
    private IDbContextTransaction _dbTransaction;

    // Backing fields for implement UnitOfWork Repository Provider.
    private IAccountStatusRepository _accountStatusRepository;
    private ICategoryRepository _categoryRepository;
    private IOrderGuestDetailRepository _orderGuestDetailRepository;
    private IOrderItemRepository _orderItemRepository;
    private IOrderRepository _orderRepository;
    private IOrderStatusRepository _orderStatusRepository;
    private IPaymentMethodRepository _paymentMethodRepository;
    private IProductRepository _productRepository;
    private IProductImageRepository _productImageRepository;
    private IProductStatusRepository _productStatusRepository;
    private IRoleRepository _roleRepository;
    private ISystemAccountRepository _systemAccountRepository;
    private ISystemAccountRoleRepository _systemAccountRoleRepository;
    private ITokenTypeRepository _tokenTypeRepository;
    private IUserRepository _userRepository;
    private IUserRoleRepository _userRoleRepository;
    private IUserTokenRepository _userTokenRepository;

    // Properties.
    public IAccountStatusRepository AccountStatusRepository
    {
        get
        {
            _accountStatusRepository ??= new AccountStatusRepository(_dbContext);
            return _accountStatusRepository;
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            _userRepository ??= new UserRepository(_dbContext, _userManager);
            return _userRepository;
        }
    }

    public IRoleRepository RoleRepository
    {
        get
        {
            _roleRepository ??= new RoleRepository(_dbContext, _roleManager);
            return _roleRepository;
        }
    }

    public IUserRoleRepository UserRoleRepository
    {
        get
        {
            _userRoleRepository ??= new UserRoleRepository(_dbContext);
            return _userRoleRepository;
        }
    }

    public IUserTokenRepository UserTokenRepository
    {
        get
        {
            _userTokenRepository ??= new UserTokenRepository(_dbContext);
            return _userTokenRepository;
        }
    }

    public ICategoryRepository CategoryRepository
    {
        get
        {
            _categoryRepository ??= new CategoryRepository(_dbContext);
            return _categoryRepository;
        }
    }

    public IOrderGuestDetailRepository OrderGuestDetailRepository
    {
        get
        {
            _orderGuestDetailRepository ??= new OrderGuestDetailRepository(_dbContext);
            return _orderGuestDetailRepository;
        }
    }

    public IOrderItemRepository OrderItemRepository
    {
        get
        {
            _orderItemRepository ??= new OrderItemRepository(_dbContext);
            return _orderItemRepository;
        }
    }

    public IOrderRepository OrderRepository
    {
        get
        {
            _orderRepository ??= new OrderRepository(_dbContext);
            return _orderRepository;
        }
    }

    public IProductRepository ProductRepository
    {
        get
        {
            _productRepository ??= new ProductRepository(_dbContext);
            return _productRepository;
        }
    }

    public IProductImageRepository ProductImageRepository
    {
        get
        {
            _productImageRepository ??= new ProductImageRepository(_dbContext);
            return _productImageRepository;
        }
    }

    public IProductStatusRepository ProductStatusRepository
    {
        get
        {
            _productStatusRepository ??= new ProductStatusRepository(_dbContext);
            return _productStatusRepository;
        }
    }

    public ISystemAccountRepository SystemAccountRepository
    {
        get
        {
            _systemAccountRepository ??= new SystemAccountRepository(_dbContext);
            return _systemAccountRepository;
        }
    }

    public ISystemAccountRoleRepository SystemAccountRoleRepository
    {
        get
        {
            _systemAccountRoleRepository ??= new SystemAccountRoleRepository(_dbContext);
            return _systemAccountRoleRepository;
        }
    }

    public IOrderStatusRepository OrderStatusRepository
    {
        get
        {
            _orderStatusRepository ??= new OrderStatusRepository(_dbContext);
            return _orderStatusRepository;
        }
    }

    public IPaymentMethodRepository PaymentMethodRepository
    {
        get
        {
            _paymentMethodRepository ??= new PaymentMethodRepository(_dbContext);
            return _paymentMethodRepository;
        }
    }

    public ITokenTypeRepository TokenTypeRepository
    {
        get
        {
            _tokenTypeRepository ??= new TokenTypeRepository(_dbContext);
            return _tokenTypeRepository;
        }
    }

    public EcommerceAppUnitOfWork(
        TContext dbContext,
        UserManager<UserEntity> userManager,
        RoleManager<RoleEntity> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _dbContext.Database.CreateExecutionStrategy();
    }

    public async Task CreateTransactionAsync(CancellationToken cancellationToken)
    {
        _dbTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        return _dbTransaction.CommitAsync(cancellationToken);
    }

    public ValueTask DisposeTransactionAsync(CancellationToken cancellationToken)
    {
        return _dbTransaction.DisposeAsync();
    }

    public Task RollBackTransactionAsync(CancellationToken cancellationToken)
    {
        return _dbTransaction.RollbackAsync(cancellationToken);
    }

    public Task SaveChangesToDatabaseAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}