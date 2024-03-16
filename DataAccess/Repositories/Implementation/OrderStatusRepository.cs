using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class OrderStatusRepository :
        GenericRepository<OrderStatusEntity>,
        IOrderStatusRepository
    {
        public OrderStatusRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<OrderStatusEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<OrderStatusEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<OrderStatusEntity>> GetAllByExpressionAsync(Expression<Func<OrderStatusEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
