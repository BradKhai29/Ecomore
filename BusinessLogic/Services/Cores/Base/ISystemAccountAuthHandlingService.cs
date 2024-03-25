using BusinessLogic.Models.Base;
using DataAccess.Entities;
using System.Threading.Tasks;
using System.Threading;
using System;
using DTOs.Implementation.Auths.Incomings;

namespace BusinessLogic.Services.Cores.Base
{
    public interface ISystemAccountAuthHandlingService
    {
        Task<IResult<SystemAccountEntity>> LoginByUserNameAsync(
            string username,
            string password,
            CancellationToken cancellationToken);

        Task<IResult<SystemAccountEntity>> LoginByEmailAsync(
            string email,
            string password,
            CancellationToken cancellationToken);

        Task<bool> IsUsernameExistedAsync(
            string username,
            CancellationToken cancellationToken);

        Task<bool> IsEmailExistedAsync(
            string email,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Process to register a new user account with provided register info.
        /// </summary>
        /// <param name="registerDto">
        ///     The register information that used to create a new user account.
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        ///     The <see cref="IResult{Guid}"/> that contains UserId of the registered account.
        /// </returns>
        Task<IResult<Guid>> RegisterAsync(
            RegisterDto registerDto,
            CancellationToken cancellationToken);

        Task<IResult<Guid>> RegisterDefaultAccountAsync(CancellationToken cancellationToken);

        Task<bool> ConfirmEmailAsync(
            SystemAccountEntity systemAccount,
            CancellationToken cancellationToken);
    }
}
