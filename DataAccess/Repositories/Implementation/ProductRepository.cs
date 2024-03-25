using DataAccess.DataSeedings;
using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class ProductRepository :
        GenericRepository<ProductEntity>,
        IProductRepository
    {
        public ProductRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<int> BulkTemporarilyRemoveByIdAsync(
            Guid systemAccountId,
            Guid productId,
            CancellationToken cancellationToken)
        {
            var updatedAt = DateTime.UtcNow;
            var updatedBy = systemAccountId;

            return _dbSet
                .Where(product => product.Id.Equals(productId))
                .ExecuteUpdateAsync(product => product
                    .SetProperty(
                        product => product.ProductStatusId,
                        product => ProductStatuses.NotAvailable.Id)
                    .SetProperty(
                        product => product.UpdatedAt,
                        product => updatedAt)
                    .SetProperty(
                        product => product.UpdatedBy,
                        product => updatedBy),
                cancellationToken: cancellationToken);
        }

        public Task<int> BulkUpdateAsync(ProductEntity updatedProduct, CancellationToken cancellationToken)
        {
            return _dbSet
                .Where(product => product.Id.Equals(updatedProduct.Id))
                .ExecuteUpdateAsync(product => product
                    .SetProperty(
                        product => product.Name,
                        product => updatedProduct.Name)
                    .SetProperty(
                        product => product.CategoryId,
                        product => updatedProduct.CategoryId)
                    .SetProperty(
                        product => product.Description,
                        product => updatedProduct.Description)
                    .SetProperty(
                        product => product.QuantityInStock,
                        product => updatedProduct.QuantityInStock)
                    .SetProperty(
                        product => product.UnitPrice,
                        product => updatedProduct.UnitPrice)
                    .SetProperty(
                        product => product.ProductStatusId,
                        product => updatedProduct.ProductStatusId)
                    .SetProperty(
                        product => product.UpdatedAt,
                        product => updatedProduct.UpdatedAt),
                cancellationToken: cancellationToken);
        }

        public override Task<ProductEntity> FindByIdAsync(
            Guid id,
            bool asNoTracking,
            CancellationToken cancellationToken)
        {
            IQueryable<ProductEntity> queryable = _dbSet;

            if (asNoTracking)
            {
                queryable = queryable.AsNoTracking();
            }

            return queryable
                .Where(product => product.Id.Equals(id))
                .Select(product => new ProductEntity
                {
                    Id = product.Id,
                    Category = new CategoryEntity
                    {
                        Id = product.CategoryId,
                        Name = product.Category.Name
                    },
                    Name = product.Name,
                    Description = product.Description,
                    ProductStatusId = product.ProductStatusId,
                    UnitPrice = product.UnitPrice,
                    QuantityInStock = product.QuantityInStock,
                    ProductImages = product.ProductImages.Select(image => new ProductImageEntity
                    {
                        Id = image.Id,
                        FileName = image.FileName,
                        StorageUrl = image.StorageUrl,
                    }),
                    ProductStatus = new ProductStatusEntity
                    {
                        Id = product.ProductStatusId,
                        Name = product.ProductStatus.Name
                    },
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public Task<ProductEntity> FindByIdForDisplayShoppingCartAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            return _dbSet
                .AsNoTracking()
                .Select(product => new ProductEntity
                {
                    Id = product.Id,
                    Name = product.Name,
                    UnitPrice = product.UnitPrice,
                    ProductImages = product.ProductImages.Select(image => new ProductImageEntity
                    {
                        Id = image.Id,
                        FileName = image.FileName,
                        StorageUrl = image.StorageUrl,
                    })
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public override async Task<IEnumerable<ProductEntity>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsNoTracking()
                .Select(product => new ProductEntity
                {
                    Id = product.Id,
                    Category = new CategoryEntity
                    {
                        Id = product.CategoryId,
                        Name = product.Category.Name
                    },
                    Name = product.Name,
                    Description = product.Description,
                    ProductStatusId = product.ProductStatusId,
                    UnitPrice = product.UnitPrice,
                    QuantityInStock = product.QuantityInStock,
                    ProductImages = product.ProductImages.Select(image => new ProductImageEntity
                    {
                        Id = image.Id,
                        FileName = image.FileName,
                        StorageUrl = image.StorageUrl,
                    })
                })
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async override Task<IEnumerable<ProductEntity>> GetAllByExpressionAsync(
            Expression<Func<ProductEntity, bool>> findExpresison,
            CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(predicate: findExpresison)
                .AsNoTracking()
                .Select(product => new ProductEntity
                {
                    Id = product.Id,
                    Category = new CategoryEntity
                    {
                        Id = product.CategoryId,
                        Name = product.Category.Name
                    },
                    Name = product.Name,
                    Description = product.Description,
                    ProductStatusId = product.ProductStatusId,
                    UnitPrice = product.UnitPrice,
                    QuantityInStock = product.QuantityInStock,
                    ProductImages = product.ProductImages.Select(image => new ProductImageEntity
                    {
                        Id = image.Id,
                        FileName = image.FileName,
                        StorageUrl = image.StorageUrl,
                    })
                })
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public Task<int> GetQuantityInStockByIdAsync(
            Guid productId,
            CancellationToken cancellationToken)
        {
            return _dbSet
                .Where(product => product.Id.Equals(productId))
                .Select(product => product.QuantityInStock)
                .SingleAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductEntity>> GetTopProductsAsync(
            int pageSize,
            CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsNoTracking()
                .Take(count: pageSize)
                .Select(product => new ProductEntity
                {
                    Id = product.Id,
                    Category = new CategoryEntity
                    {
                        Id = product.CategoryId,
                        Name = product.Category.Name
                    },
                    Name = product.Name,
                    Description = product.Description,
                    ProductStatusId = product.ProductStatusId,
                    UnitPrice = product.UnitPrice,
                    QuantityInStock = product.QuantityInStock,
                    ProductImages = product.ProductImages.Select(image => new ProductImageEntity
                    {
                        Id = image.Id,
                        FileName = image.FileName,
                        StorageUrl = image.StorageUrl,
                    }),
                    ProductStatus = new ProductStatusEntity
                    {
                        Id = product.ProductStatusId,
                        Name = product.ProductStatus.Name
                    },
                })
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
