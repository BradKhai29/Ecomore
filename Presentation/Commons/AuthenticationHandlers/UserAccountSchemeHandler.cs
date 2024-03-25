using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Options.Models.Jwts;
using Presentation.Commons.Constants;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Presentation.Commons.AuthenticationHandlers
{
    public class UserAccountSchemeHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly UserAccountJwtOptions _jwtOptions;
        private readonly SecurityTokenHandler _securityTokenHandler;

        public UserAccountSchemeHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            SecurityTokenHandler securityTokenHandler,
            IOptions<UserAccountJwtOptions> jwtOptions) : base(options, logger, encoder, clock)
        {
            _jwtOptions = jwtOptions.Value;
            _securityTokenHandler = securityTokenHandler;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationResult = AuthenticateResult.Fail("Authenticated Failed", new AuthenticationProperties
            {
                RedirectUri = Pages.Auth.LoginModel.Path
            });

            var cookie = Request.Cookies.FirstOrDefault(cookie => cookie.Key.Equals(CookieNames.UserAccessToken));

            if (string.IsNullOrEmpty(cookie.Value))
            {
                return authenticationResult;
            }

            string tokenValue = cookie.Value;
            var validationResult = await ValidateTokenParametersAsync(tokenValue);

            if (!validationResult.IsValid)
            {
                return authenticationResult;
            }

            var authenticationTicket = CreateTicketFromValidationResult(validationResult: validationResult);
            authenticationResult = AuthenticateResult.Success(authenticationTicket);

            return authenticationResult;
        }

        #region Private Methods
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
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidAudience = _jwtOptions.Audience,
                    IssuerSigningKey = _jwtOptions.GetSecurityKey()
                });

            return validationResult;
        }

        private AuthenticationTicket CreateTicketFromValidationResult(TokenValidationResult validationResult)
        {
            var claimsPrincipal = new ClaimsPrincipal(identity: validationResult.ClaimsIdentity);
            var authenticationScheme = CookieAuthenticationSchemes.UserAccountScheme;

            var authenticationTicket = new AuthenticationTicket(
                principal: claimsPrincipal,
                authenticationScheme: authenticationScheme);

            return authenticationTicket;
        }
        #endregion
    }
}
