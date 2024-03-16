using BusinessLogic.Services.Cores.Base;
using DataAccess.DbContexts;
using DataAccess.Entities;
using DataAccess.UnitOfWorks.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Cores.Implementation
{
    internal class ProductHandlingService :
        IProductHandlingService
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public ProductHandlingService(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(
            Guid systemAccountId,
            ProductEntity product,
            CancellationToken cancellationToken)
        {
            var result = false;

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(operation: async () =>
            {
                await _unitOfWork.CreateTransactionAsync(cancellationToken: cancellationToken);

                try
                {
                    // Tracking the CRUD operation on this product.
                    product.CreatedBy = systemAccountId;
                    product.CreatedAt = DateTime.UtcNow;
                    product.UpdatedBy = systemAccountId;
                    product.UpdatedAt = DateTime.UtcNow;

                    await _unitOfWork.ProductRepository.AddAsync(
                        newEntity: product,
                        cancellationToken: cancellationToken);

                    await _unitOfWork.SaveChangesToDatabaseAsync(
                        cancellationToken: cancellationToken);

                    await _unitOfWork.CommitTransactionAsync(
                        cancellationToken: cancellationToken);

                    result = true;
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

            return result;
        }

        public Task<ProductEntity> FindByIdAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            return _unitOfWork.ProductRepository.FindByIdAsync(
                id: productId,
                asNoTracking: true,
                cancellationToken: cancellationToken);
        }

        public Task<IEnumerable<ProductEntity>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return _unitOfWork.ProductRepository.GetAllAsync(cancellationToken);
        }

        public Task<int> GetQuantityInStockByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            return _unitOfWork.ProductRepository.GetQuantityInStockByIdAsync(
                productId: productId,
                cancellationToken: cancellationToken);
        }

        public Task<IEnumerable<ProductEntity>> GetTopProductsAsync(
            int pageSize,
            CancellationToken cancellationToken)
        {
            return _unitOfWork.ProductRepository.GetTopProductsAsync(
                pageSize: pageSize,
                cancellationToken: cancellationToken);
        }

        public Task<bool> IsProductExistedByIdAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            Expression<Func<ProductEntity, bool>> expression = product => product.Id == productId;

            return _unitOfWork.ProductRepository.IsFoundByExpressionAsync(
                findExpresison: expression,
                cancellationToken: cancellationToken);
        }

        public async Task<bool> PermanentlyRemoveByIdAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            var result = false;

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(operation: async () =>
            {
                await _unitOfWork.CreateTransactionAsync(cancellationToken: cancellationToken);

                try
                {
                    _unitOfWork.ProductRepository.Remove(
                        foundEntity: new ProductEntity { Id = productId });

                    await _unitOfWork.SaveChangesToDatabaseAsync(
                        cancellationToken: cancellationToken);

                    await _unitOfWork.CommitTransactionAsync(
                        cancellationToken: cancellationToken);

                    result = true;
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

            return result;
        }

        public async Task<bool> RemoveTemporarilyByIdAsync(
            Guid systemAccountId,
            Guid productId,
            CancellationToken cancellationToken)
        {
            var result = false;

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(operation: async () =>
            {
                await _unitOfWork.CreateTransactionAsync(cancellationToken: cancellationToken);

                try
                {
                    await _unitOfWork.ProductRepository.BulkTemporarilyRemoveByIdAsync(
                        systemAccountId: systemAccountId,
                        productId: productId,
                        cancellationToken: cancellationToken);

                    await _unitOfWork.CommitTransactionAsync(
                        cancellationToken: cancellationToken);

                    result = true;
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

            return result;
        }

        public Task<bool> UpdateAsync(
            Guid systemAccountId,
            ProductEntity product,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
