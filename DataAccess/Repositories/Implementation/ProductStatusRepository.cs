using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class ProductStatusRepository :
        GenericRepository<ProductStatusEntity>,
        IProductStatusRepository
    {
        public ProductStatusRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<ProductStatusEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<ProductStatusEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<ProductStatusEntity>> GetAllByExpressionAsync(Expression<Func<ProductStatusEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
