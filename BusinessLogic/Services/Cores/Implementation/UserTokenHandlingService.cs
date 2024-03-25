using BusinessLogic.Commons.Enums;
using BusinessLogic.Models;
using BusinessLogic.Models.Base;
using BusinessLogic.Services.Cores.Base;
using DataAccess.DbContexts;
using DataAccess.Entities;
using DataAccess.UnitOfWorks.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Options.Models;
using Options.Models.Jwts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Cores.Implementation
{
    internal class UserTokenHandlingService :
        IUserTokenHandlingService
    {
        private readonly UserAccountJwtOptions _jwtOptions;
        private readonly ResetPasswordOptions _resetPasswordOptions;
        private readonly SecurityTokenHandler _securityTokenHandler;
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public UserTokenHandlingService(
            IOptions<UserAccountJwtOptions> jwtOptions,
            IOptions<ResetPasswordOptions> resetPasswordOptions,
            SecurityTokenHandler securityTokenHandler,
            IUnitOfWork<AppDbContext> unitOfWork)
        {
            _jwtOptions = jwtOptions.Value;
            _resetPasswordOptions = resetPasswordOptions.Value;
            _securityTokenHandler = securityTokenHandler;
            _unitOfWork = unitOfWork;
        }

        public string GenerateAccessToken(
            IEnumerable<Claim> claims,
            TimeSpan lifeSpan)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                Subject = new ClaimsIdentity(claims: claims),
                SigningCredentials = new SigningCredentials(
                    key: _jwtOptions.GetSecurityKey(),
                    algorithm: SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.Add(lifeSpan)
            };

            // Generate token.
            var token = _securityTokenHandler.CreateToken(tokenDescriptor: tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #region Generate One-time-use token.
        public string GenerateEmailConfirmationToken(IEnumerable<Claim> claims)
        {
            return GenerateOneTimeUseToken(claims: claims);
        }

        public string GenerateResetPasswordToken(IEnumerable<Claim> claims)
        {
            return GenerateOneTimeUseToken(claims: claims);
        }

        private string GenerateOneTimeUseToken(IEnumerable<Claim> claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _resetPasswordOptions.Issuer,
                Audience = _resetPasswordOptions.Audience,
                Subject = new ClaimsIdentity(claims: claims),
                SigningCredentials = new SigningCredentials(
                    key: _resetPasswordOptions.GetSecurityKey(),
                    algorithm: SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.Add(_resetPasswordOptions.GetLifeSpan())
            };

            // Generate token.
            var token = _securityTokenHandler.CreateToken(tokenDescriptor: tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        public async Task<IResult<UserEntity>> VerifyResetPasswordTokenAsync(
            string resetPasswordToken,
            CancellationToken cancellationToken)
        {
            var tokenValidationResult = await ValidateTokenParametersAsync(token: resetPasswordToken);

            return await VerifyTokenClaimsAsync(
                validationResult: tokenValidationResult,
                purpose: TokenPurpose.ResetPassword,
                cancellationToken: cancellationToken);
        }

        public async Task<IResult<UserEntity>> VerifyEmailConfirmationTokenAsync(
            string confirmationToken,
            CancellationToken cancellationToken)
        {
            var tokenValidationResult = await ValidateTokenParametersAsync(token: confirmationToken);

            return await VerifyTokenClaimsAsync(
                validationResult: tokenValidationResult,
                purpose: TokenPurpose.EmailConfirmation,
                cancellationToken: cancellationToken);
        }

        public UserTokenEntity CreateResetPasswordToken(Guid userId)
        {
            string resetPasswordTokenName = GetTokenName(purpose: TokenPurpose.ResetPassword, userId: userId);
            byte liveMinutes = 30;

            return new UserTokenEntity
            {
                UserId = userId,
                LoginProvider = _jwtOptions.Issuer,
                Name = resetPasswordTokenName,
                Value = Guid.NewGuid().ToString(),
                ExpiredAt = DateTime.UtcNow.AddMinutes(value: liveMinutes),
            };
        }

        public Task<UserTokenEntity> FindResetPasswordTokenByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            string resetPasswordTokenName = GetTokenName(purpose: TokenPurpose.ResetPassword, userId: userId);

            return null;
        }

        public async Task<bool> SaveTokenAsync(
            UserTokenEntity userToken,
            CancellationToken cancellationToken)
        {
            var result = false;

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(operation: async () =>
            {
                await _unitOfWork.CreateTransactionAsync(cancellationToken: cancellationToken);

                try
                {
                    await _unitOfWork.UserTokenRepository.AddAsync(
                        newEntity: userToken,
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

        public async Task<bool> RemoveResetPasswordTokenAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var result = false;

            var executionStrategy = _unitOfWork.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(operation: async () =>
            {
                await _unitOfWork.CreateTransactionAsync(cancellationToken: cancellationToken);

                try
                {
                    var resetPasswordToken = new UserTokenEntity
                    {
                        UserId = userId,
                        Name = GetTokenName(TokenPurpose.ResetPassword, userId)
                    };

                    //await _unitOfWork.UserTokenRepository.BulkDeleteForResetPasswordAsync(
                    //    resetPasswordToken: resetPasswordToken,
                    //    cancellationToken: cancellationToken);

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

        #region Private Methods.
        /// <summary>
        ///     Validate the parameters of the input token is valid 
        ///     to this application's requirements or not.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<TokenValidationResult> ValidateTokenParametersAsync(string token)
        {
            // Validate the token credentials.
            var validationResult = await _securityTokenHandler.ValidateTokenAsync(
                token: token,
                validationParameters: new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _resetPasswordOptions.Issuer,
                    ValidAudience = _resetPasswordOptions.Audience,
                    IssuerSigningKey = _resetPasswordOptions.GetSecurityKey()
                });

            return validationResult;
        }

        private async Task<IResult<UserEntity>> VerifyTokenClaimsAsync(
            TokenValidationResult validationResult,
            TokenPurpose purpose,
            CancellationToken cancellationToken)
        {
            var result = Result<UserEntity>.Failed();

            // Verify if token is valid.
            object subValue = string.Empty;
            object jtiValue = string.Empty;

            var isClaimExisted = validationResult.Claims.TryGetValue(
                key: JwtRegisteredClaimNames.Sub,
                value: out subValue);

            if (purpose == TokenPurpose.ResetPassword)
            {
                isClaimExisted = isClaimExisted && validationResult.Claims.TryGetValue(
                    key: JwtRegisteredClaimNames.Jti,
                    value: out jtiValue);
            }

            if (!isClaimExisted)
            {
                return result;
            }

            var userId = Guid.Empty;
            Guid.TryParse(subValue.ToString(), out userId);

            // If for reset password purpose, have a step to verify under the database.
            if (purpose == TokenPurpose.ResetPassword)
            {
                //bool isValid = await _unitOfWork.UserTokenRepository.IsFoundBySpecificationsAsync(
                //    cancellationToken: cancellationToken,
                //    _specificationManager.UserToken.Where.ForResetPasswordValidation(
                //        userId: userId,
                //        tokenValue: jtiValue.ToString()));

                //if (!isValid)
                //{
                //    return result;
                //}
            }

            return Result<UserEntity>.Success(new()
            {
                Id = userId,
            });
        }

        /// <summary>
        ///     Get a token name by the specified token-purpose.
        /// </summary>
        /// <param name="purpose">
        ///     The purpose of this token.
        /// </param>
        /// <param name="userId">
        ///     The Id of the user this token belonged to.
        /// </param>
        /// <returns>
        ///     The name of the token.
        /// </returns>
        private string GetTokenName(TokenPurpose purpose, Guid userId)
        {
            const string resetPasswordPurpose = "reset_password_token";

            var tokenName = string.Empty;

            switch (purpose)
            {
                case TokenPurpose.ResetPassword:
                    tokenName = $"{resetPasswordPurpose}.{userId}";
                    break;

                default:
                    tokenName = $"{resetPasswordPurpose}.{userId}";
                    break;
            }

            return tokenName;
        }
        #endregion
    }
}
