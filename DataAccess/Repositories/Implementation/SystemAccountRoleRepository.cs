using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class SystemAccountRoleRepository :
        GenericRepository<SystemAccountRoleEntity>,
        ISystemAccountRoleRepository
    {
        public SystemAccountRoleRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<SystemAccountRoleEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<SystemAccountRoleEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<SystemAccountRoleEntity>> GetAllByExpressionAsync(Expression<Func<SystemAccountRoleEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
