using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class OrderRepository :
        GenericRepository<OrderEntity>,
        IOrderRepository
    {
        public OrderRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<OrderEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<OrderEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<OrderEntity>> GetAllByExpressionAsync(Expression<Func<OrderEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
