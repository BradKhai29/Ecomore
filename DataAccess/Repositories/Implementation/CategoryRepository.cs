using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class CategoryRepository :
        GenericRepository<CategoryEntity>,
        ICategoryRepository
    {
        public CategoryRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<CategoryEntity> FindByIdAsync(
            Guid id,
            bool asNoTracking,
            CancellationToken cancellationToken)
        {
            IQueryable<CategoryEntity> queryable = _dbSet;

            if (asNoTracking)
            {
                queryable = queryable.AsNoTracking();
            }

            return queryable
                .Where(category => category.Id == id)
                .Select(category => new CategoryEntity
                {
                    Id = category.Id,
                    Name = category.Name,
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public override async Task<IEnumerable<CategoryEntity>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return await _dbSet
                .AsNoTracking()
                .Select(category => new CategoryEntity
                {
                    Id = category.Id,
                    Name = category.Name,
                    Products = category.Products.Select(product => new ProductEntity { Id = product.Id })
                })
                .ToListAsync(cancellationToken);
        }

        public override Task<IEnumerable<CategoryEntity>> GetAllByExpressionAsync(
            Expression<Func<CategoryEntity, bool>> findExpresison,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryEntity> GetForDetailDisplayByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            return _dbSet
                .AsNoTracking()
                .Where(category => category.Id == id)
                .Select(category => new CategoryEntity
                {
                    Id = category.Id,
                    Name = category.Name,
                    Products = category.Products.Select(product => new ProductEntity
                    {
                        Id = product.Id,
                        Name = product.Name,
                        UnitPrice = product.UnitPrice,
                        QuantityInStock = product.QuantityInStock,
                        ProductStatusId = product.ProductStatusId,
                        ProductImages = product.ProductImages.Select(image => new ProductImageEntity
                        {
                            FileName = image.FileName,
                            StorageUrl = image.StorageUrl,
                        })
                    })
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
