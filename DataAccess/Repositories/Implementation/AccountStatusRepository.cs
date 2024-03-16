using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation;

public class AccountStatusRepository :
    GenericRepository<AccountStatusEntity>,
    IAccountStatusRepository
{
    public AccountStatusRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public override Task<AccountStatusEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<AccountStatusEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<IEnumerable<AccountStatusEntity>> GetAllByExpressionAsync(Expression<Func<AccountStatusEntity, bool>> findExpresison, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
