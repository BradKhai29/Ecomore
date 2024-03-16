using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation;

public class UserRoleRepository :
    GenericRepository<UserRoleEntity>,
    IUserRoleRepository
{
    public UserRoleRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public override Task<UserRoleEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<UserRoleEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<UserRoleEntity>> GetAllByExpressionAsync(Expression<Func<UserRoleEntity, bool>> findExpresison, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
