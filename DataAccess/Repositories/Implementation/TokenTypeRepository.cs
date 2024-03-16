using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation
{
    internal class TokenTypeRepository :
        GenericRepository<TokenTypeEntity>,
        ITokenTypeRepository
    {
        public TokenTypeRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override Task<TokenTypeEntity> FindByIdAsync(Guid id, bool asNoTracking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TokenTypeEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TokenTypeEntity>> GetAllByExpressionAsync(Expression<Func<TokenTypeEntity, bool>> findExpresison, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
