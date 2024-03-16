using DataAccess.Entities;
using DataAccess.Repositories.Base.Generics;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Repositories.Base
{
    public interface IUserRepository
        : IBaseIdentityRepository<UserEntity, Guid>
    {
        UserManager<UserEntity> Manager { get; }

        Task<UserEntity> FindByUsernameAsync(string username);

        Task<int> BulkUpdateForEmailConfirmationAsync(
            UserEntity foundUser,
            CancellationToken cancellationToken);

        Task<int> BulkUpdatePasswordAsync(
            Guid userId,
            string passwordHash,
            CancellationToken cancellationToken);
    }
}
