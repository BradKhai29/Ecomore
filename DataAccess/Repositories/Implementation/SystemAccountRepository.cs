using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class SystemAccountRepository :
        GenericRepository<SystemAccountEntity>,
        ISystemAccountRepository
    {
        public SystemAccountRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<SystemAccountEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<SystemAccountEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<SystemAccountEntity>> GetAllByExpressionAsync(Expression<Func<SystemAccountEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
