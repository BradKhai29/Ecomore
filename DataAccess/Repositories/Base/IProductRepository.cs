using DataAccess.Entities;
using DataAccess.Repositories.Base.Generics;

namespace DataAccess.Repositories.Base
{
    public interface IProductRepository : IGenericRepository<ProductEntity>
    {
        Task<IEnumerable<ProductEntity>> GetTopProductsAsync(
            int pageSize,
            CancellationToken cancellationToken);

        Task<int> BulkTemporarilyRemoveByIdAsync(
            Guid systemAccountId,
            Guid productId,
            CancellationToken cancellationToken);

        Task<int> GetQuantityInStockByIdAsync(
            Guid productId,
            CancellationToken cancellationToken);

        Task<ProductEntity> FindByIdForDisplayShoppingCartAsync(
            Guid productId,
            CancellationToken cancellationToken);
    }
}
