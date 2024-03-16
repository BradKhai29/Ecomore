using DataAccess.Entities.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Base.Generics;

public abstract class BaseIdentityRepository<TEntity> :
    IBaseIdentityRepository<TEntity, Guid>
    where TEntity : class, IEntity
{
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseIdentityRepository(DbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    public abstract Task<IdentityResult> AddAsync(TEntity newEntity);

    public abstract Task<IdentityResult> RemoveAsync(Guid id);

    public abstract Task<IdentityResult> UpdateAsync(TEntity foundEntity);

    public abstract Task<TEntity> FindByIdAsync(Guid id);

    public abstract Task<TEntity> FindByNameAsync(string name);
}
