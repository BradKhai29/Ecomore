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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Cores.Implementation
{
    internal class SystemAccountAuthHandlingService :
        ISystemAccountAuthHandlingService
    {
        // Backing fields.
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly IPasswordHandlingService _passwordService;

        public SystemAccountAuthHandlingService(
            IUnitOfWork<AppDbContext> unitOfWork,
            IPasswordHandlingService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public Task<bool> ConfirmEmailAsync(
            SystemAccountEntity systemAccount,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailExistedAsync(
            string email,
            CancellationToken cancellationToken)
        {
            Expression<Func<SystemAccountEntity, bool>> findExpression = account => account.Email.Equals(email);

            return _unitOfWork.SystemAccountRepository.IsFoundByExpressionAsync(
                findExpresison: findExpression,
                cancellationToken: cancellationToken);
        }

        public Task<bool> IsUsernameExistedAsync(
            string username,
            CancellationToken cancellationToken)
        {
            Expression<Func<SystemAccountEntity, bool>> findExpression = account => account.UserName.Equals(username);

            return _unitOfWork.SystemAccountRepository.IsFoundByExpressionAsync(
                findExpresison: findExpression,
                cancellationToken: cancellationToken);
        }

        public async Task<IResult<SystemAccountEntity>> LoginAsync(
            string username,
            string password,
            CancellationToken cancellationToken)
        {
            var result = Result<SystemAccountEntity>.Failed();

            var passwordHash = _passwordService.GetHashPassword(password);

            Expression<Func<SystemAccountEntity, bool>> findExpression = (SystemAccountEntity account)
                => account.UserName.Equals(username)
                && account.PasswordHash.Equals(passwordHash);

            var foundAccount = await _unitOfWork.SystemAccountRepository.FindByExpressionAsync(
                findExpresison: findExpression,
                asNoTracking: true,
                cancellationToken: cancellationToken);

            if (!Equals(foundAccount, null))
            {
                return Result<SystemAccountEntity>.Success(foundAccount);
            }

            return result;
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
                    var account = new SystemAccountEntity
                    {
                        Id = Guid.NewGuid(),
                        UserName = registerDto.Username,
                        Email = registerDto.Email,
                        PasswordHash = _passwordService.GetHashPassword(registerDto.Password),
                        AccountStatusId = AccountStatuses.PendingConfirmed.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };

                    await _unitOfWork.SystemAccountRepository.AddAsync(
                        newEntity: account,
                        cancellationToken: cancellationToken);

                    await _unitOfWork.SaveChangesToDatabaseAsync(
                        cancellationToken: cancellationToken);

                    await _unitOfWork.CommitTransactionAsync(
                        cancellationToken: cancellationToken);

                    result = Result<Guid>.Success(account.Id);
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
