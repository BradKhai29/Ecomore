using DataAccess.Entities;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.Base.Generics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Implementation;

public class UserRepository :
    BaseIdentityRepository<UserEntity>,
    IUserRepository
{
    private readonly UserManager<UserEntity> _userManager;

    public UserManager<UserEntity> Manager => _userManager;

    public UserRepository(
        DbContext dbContext,
        UserManager<UserEntity> userManager) : base(dbContext)
    {
        _userManager = userManager;
    }

    public override Task<IdentityResult> AddAsync(UserEntity newEntity)
    {
        return _userManager.CreateAsync(newEntity);
    }

    public override Task<UserEntity> FindByIdAsync(Guid id)
    {
        return _userManager.FindByIdAsync(id.ToString());
    }

    public override Task<UserEntity> FindByNameAsync(string username)
    {
        return _userManager.FindByNameAsync(username);
    }

    public Task<UserEntity> FindByUsernameAsync(string username)
    {
        return _userManager.FindByNameAsync(username);
    }

    public override async Task<IdentityResult> RemoveAsync(Guid id)
    {
        var foundUser = await _userManager.FindByIdAsync(id.ToString());

        return await _userManager.DeleteAsync(foundUser);
    }

    public override Task<IdentityResult> UpdateAsync(UserEntity foundEntity)
    {
        return _userManager.UpdateAsync(foundEntity);
    }

    public Task<int> BulkUpdateForEmailConfirmationAsync(
        UserEntity foundUser,
        CancellationToken cancellationToken)
    {
        var emailConfirmStatusId = foundUser.AccountStatusId;

        return _dbSet
            .Where(user => user.Id.Equals(foundUser.Id))
            .ExecuteUpdateAsync(user => user
                .SetProperty(
                    user => user.AccountStatusId,
                    user => emailConfirmStatusId),
                cancellationToken: cancellationToken);
    }

    public Task<int> BulkUpdatePasswordAsync(
        Guid userId,
        string passwordHash,
        CancellationToken cancellationToken)
    {
        var concurrencyStamp = Guid.NewGuid().ToString();
        var updatedAt = DateTime.UtcNow;
        var updatedBy = userId;

        return _dbSet
            .Where(user => userId.Equals(userId))
            .ExecuteUpdateAsync(user => user
                .SetProperty(
                    user => user.PasswordHash,
                    user => passwordHash)
                .SetProperty(
                    user => user.ConcurrencyStamp,
                    user => concurrencyStamp)
                .SetProperty(
                    user => user.UpdatedAt,
                    user => updatedAt));
    }

    public Task<bool> IsUserNameExistedAsync(
        string userName,
        CancellationToken cancellationToken)
    {
        return _dbSet.AnyAsync(
            predicate: user => user.UserName == userName,
            cancellationToken: cancellationToken);
    }

    public Task<bool> IsEmailExistedAsync(string email, CancellationToken cancellationToken)
    {
        return _dbSet.AnyAsync(
            predicate: user => user.Email == email,
            cancellationToken: cancellationToken);
    }

    public Task<UserEntity> FindByExpressionAsync(
        Expression<Func<UserEntity, bool>> findExpression,
        CancellationToken cancellationToken)
    {
        return _dbSet
            .AsNoTracking()
            .Where(predicate: findExpression)
            .Select(selector: user => new UserEntity
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                AvatarUrl = user.AvatarUrl,
                Email = user.Email
            })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}
