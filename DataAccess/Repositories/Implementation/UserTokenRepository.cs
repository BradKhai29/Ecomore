using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class UserTokenRepository :
        GenericRepository<UserTokenEntity>,
        IUserTokenRepository
    {
        public UserTokenRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<int> BulkDeleteAsync(
            UserTokenEntity userToken,
            CancellationToken cancellationToken)
        {
            return _dbSet
                .Where(token =>
                    token.UserId.Equals(userToken.UserId)
                    && token.Value.Equals(userToken.Value))
                .ExecuteDeleteAsync(cancellationToken: cancellationToken);
        }

        public override Task<UserTokenEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserTokenEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<UserTokenEntity>> GetAllByExpressionAsync(Expression<Func<UserTokenEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
