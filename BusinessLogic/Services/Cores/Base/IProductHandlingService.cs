using BusinessLogic.Models.Base;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Cores.Base
{
    public interface IProductHandlingService
    {
        Task<bool> CreateAsync(
            Guid systemAccountId,
            ProductEntity product,
            CancellationToken cancellationToken);

        Task<bool> UpdateAsync(
            Guid systemAccountId,
            ProductEntity product,
            CancellationToken cancellationToken);

        Task<bool> RemoveTemporarilyByIdAsync(
            Guid systemAccountId,
            Guid productId,
            CancellationToken cancellationToken);

        Task<bool> PermanentlyRemoveByIdAsync(
            Guid productId,
            CancellationToken cancellationToken);

        Task<IEnumerable<ProductEntity>> GetAllAsync(
            CancellationToken cancellationToken);

        Task<IEnumerable<ProductEntity>> GetTopProductsAsync(
            int pageSize,
            CancellationToken cancellationToken);

        Task<bool> IsProductExistedByIdAsync(
            Guid productId,
            CancellationToken cancellationToken);

        Task<ProductEntity> FindByIdAsync(
            Guid productId,
            CancellationToken cancellationToken);

        Task<int> GetQuantityInStockByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken);
    }
}
