using DataAccess.Entities;
using DataAccess.Repositories.Base.Generics;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Base
{
    public interface IUserRepository
        : IBaseIdentityRepository<UserEntity, Guid>
    {
        UserManager<UserEntity> Manager { get; }

        Task<bool> IsUserNameExistedAsync(string userName, CancellationToken cancellationToken);

        Task<bool> IsEmailExistedAsync(string email, CancellationToken cancellationToken);

        Task<UserEntity> FindByUsernameAsync(string username);

        Task<UserEntity> FindByExpressionAsync(
            Expression<Func<UserEntity, bool>> findExpression,
            CancellationToken cancellationToken);

        Task<int> BulkUpdateForEmailConfirmationAsync(
            UserEntity foundUser,
            CancellationToken cancellationToken);

        Task<int> BulkUpdatePasswordAsync(
            Guid userId,
            string passwordHash,
            CancellationToken cancellationToken);
    }
}
