using DataAccess.Entities;
using DataAccess.Repositories.Base.Generics;

namespace DataAccess.Repositories.Base;

public interface IUserTokenRepository :
    IGenericRepository<UserTokenEntity>
{
    Task<int> BulkDeleteAsync(
        UserTokenEntity userToken,
        CancellationToken cancellationToken);
}
