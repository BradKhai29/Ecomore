using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class ProductImageRepository :
        GenericRepository<ProductImageEntity>,
        IProductImageRepository
    {
        public ProductImageRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<ProductImageEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<ProductImageEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<ProductImageEntity>> GetAllByExpressionAsync(Expression<Func<ProductImageEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
