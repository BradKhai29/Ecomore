using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class PaymentMethodRepository :
        GenericRepository<PaymentMethodEntity>,
        IPaymentMethodRepository
    {
        public PaymentMethodRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<PaymentMethodEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<PaymentMethodEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<PaymentMethodEntity>> GetAllByExpressionAsync(Expression<Func<PaymentMethodEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
