using BusinessLogic.Models.Base;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Cores.Base
{
    /// <summary>
    ///     This interface provides methods to generate a token.
    /// </summary>
    public interface IUserTokenHandlingService
    {
        /// <summary>
        ///     Generate a jwt-format access-token 
        ///     from the received credentials.
        /// </summary>
        /// <param name="claims">
        ///     The claims this access token will encapsulate.
        /// </param>
        /// <param name="lifeSpan">
        ///     The live span of this access-token.
        /// </param>
        /// 
        /// <returns>
        ///     A string that represents the access-token value.
        /// </returns>
        string GenerateAccessToken(
            IEnumerable<Claim> claims,
            TimeSpan lifeSpan);

        /// <summary>
        ///     Generate a jwt-format email confirmation token
        ///     from the received credentials.
        /// </summary>
        /// <param name="claims">
        ///     The claims this email confirmation token will encapsulate.
        /// </param>
        /// 
        /// <returns>
        ///     A string that represents the email confirmation token value.
        /// </returns>
        string GenerateEmailConfirmationToken(IEnumerable<Claim> claims);

        UserTokenEntity CreateResetPasswordToken(Guid userId);

        Task<bool> RemoveResetPasswordTokenAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<UserTokenEntity> FindResetPasswordTokenByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<bool> SaveTokenAsync(
            UserTokenEntity resetPasswordToken,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Generate a jwt-format reset-password-token 
        ///     from the received credentials.
        /// </summary>
        /// <param name="claims">
        ///     The claims this reset-password-token will encapsulate.
        /// </param>
        /// 
        /// <returns>
        ///     A string that represents the reset-password-token value.
        /// </returns>
        string GenerateResetPasswordToken(IEnumerable<Claim> claims);

        /// <summary>
        ///     Verify the input reset-password-token and
        ///     extract the user credentials that will be used
        ///     to process reset-password operation.
        /// </summary>
        /// <param name="resetPasswordToken">
        ///     The input token value to verify.
        /// </param>
        /// <returns></returns>
        Task<IResult<UserEntity>> VerifyResetPasswordTokenAsync(
            string resetPasswordToken,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Verify the input email-confirmation-token and
        ///     extract the user credentials that will be used
        ///     to process email-confirmation operation.
        /// </summary>
        /// <param name="confirmationToken">
        ///     The input token value to verify.
        /// </param>
        /// <returns></returns>
        Task<IResult<UserEntity>> VerifyEmailConfirmationTokenAsync(
            string confirmationToken,
            CancellationToken cancellationToken);
    }
}
