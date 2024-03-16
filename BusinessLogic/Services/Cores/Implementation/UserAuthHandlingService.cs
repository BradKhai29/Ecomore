using BusinessLogic.Models;
using BusinessLogic.Models.Base;
using BusinessLogic.Services.Cores.Base;
using BusinessLogic.Services.Externals.Base;
using DataAccess.Commons.SystemConstants;
using DataAccess.DataSeedings;
using DataAccess.DbContexts;
using DataAccess.Entities;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Auths.Incomings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Cores.Implementation
{
    internal class UserAuthHandlingService : IUserAuthHandlingService
    {
        // Backing fields.
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IPasswordHandlingService _passwordService;

        public UserAuthHandlingService(
            IUnitOfWork<AppDbContext> unitOfWork,
            IPasswordHandlingService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public async Task<bool> ConfirmEmailForUserAsync(
            UserEntity user,
            CancellationToken cancellationToken)
        {
            var result = false;

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(operation: async () =>
            {
                await _unitOfWork.CreateTransactionAsync(cancellationToken: cancellationToken);

                try
                {
                    await _unitOfWork.UserRepository.BulkUpdateForEmailConfirmationAsync(
                        foundUser: user,
                        cancellationToken: cancellationToken);

                    await _unitOfWork.SaveChangesToDatabaseAsync(
                        cancellationToken: cancellationToken);

                    await _unitOfWork.CommitTransactionAsync(
                        cancellationToken: cancellationToken);

                    result = true;
                }
                catch
                {
                    await _unitOfWork.RollBackTransactionAsync(
                        cancellationToken: cancellationToken);
                }
                finally
                {
                    await _unitOfWork.DisposeTransactionAsync(
                        cancellationToken: cancellationToken);
                }
            });

            return result;
        }

        public Task<bool> IsEmailExistedAsync(string email, CancellationToken cancellationToken)
        {
            throw new Exception();
        }

        public Task<bool> IsUsernameExistedAsync(string username, CancellationToken cancellationToken)
        {
            throw new Exception();
        }

        public async Task<IResult<UserEntity>> LoginAsync(
            string username,
            string password,
            CancellationToken cancellationToken)
        {
            var result = Result<UserEntity>.Failed();
            var passwordHash = _passwordService.GetHashPassword(password);

            var foundUser = new UserEntity();

            if (Equals(foundUser, null))
            {
                return result;
            }

            return Result<UserEntity>.Success(foundUser);
        }

        public async Task<IResult<Guid>> RegisterAsync(
            RegisterDto registerDto,
            CancellationToken cancellationToken)
        {
            var result = Result<Guid>.Failed();

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(operation: async () =>
            {
                await _unitOfWork.CreateTransactionAsync(cancellationToken: cancellationToken);

                try
                {
                    var user = new UserEntity
                    {
                        Id = Guid.NewGuid(),
                        UserName = registerDto.Username,
                        Email = registerDto.Email,
                        PasswordHash = _passwordService.GetHashPassword(registerDto.Password),
                        AccountStatusId = AccountStatuses.EmailConfirmed.Id,
                        AvatarUrl = DefaultValues.UserAvatarUrl,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        PhoneNumber = string.Empty
                    };

                    await _unitOfWork.UserRepository.AddAsync(newEntity: user);

                    await _unitOfWork.SaveChangesToDatabaseAsync(
                        cancellationToken: cancellationToken);

                    await _unitOfWork.CommitTransactionAsync(
                        cancellationToken: cancellationToken);

                    result = Result<Guid>.Success(user.Id);
                }
                catch
                {
                    await _unitOfWork.RollBackTransactionAsync(
                        cancellationToken: cancellationToken);
                }
                finally
                {
                    await _unitOfWork.DisposeTransactionAsync(
                        cancellationToken: cancellationToken);
                }
            });

            return result;
        }
    }
}
