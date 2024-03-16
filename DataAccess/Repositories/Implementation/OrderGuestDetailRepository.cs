using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class OrderGuestDetailRepository :
        GenericRepository<OrderGuestDetailEntity>,
        IOrderGuestDetailRepository
    {
        public OrderGuestDetailRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<OrderGuestDetailEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<OrderGuestDetailEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<OrderGuestDetailEntity>> GetAllByExpressionAsync(Expression<Func<OrderGuestDetailEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
