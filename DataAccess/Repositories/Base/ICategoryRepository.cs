using DataAccess.Entities;
using DataAccess.Repositories.Base.Generics;

namespace DataAccess.Repositories.Base
{
    public interface ICategoryRepository : IGenericRepository<CategoryEntity>
    {
        public Task<CategoryEntity> GetForDetailDisplayByIdAsync(
            Guid id,
            CancellationToken cancellationToken);
    }
}
