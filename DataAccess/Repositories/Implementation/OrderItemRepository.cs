using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class OrderItemRepository :
        GenericRepository<OrderItemEntity>,
        IOrderItemRepository
    {
        public OrderItemRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<OrderItemEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<OrderItemEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<OrderItemEntity>> GetAllByExpressionAsync(Expression<Func<OrderItemEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
