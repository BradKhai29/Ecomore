using BusinessLogic.Models;
using BusinessLogic.Models.Base;
using BusinessLogic.Services.Cores.Base;
using DataAccess.Entities;
using DTOs.Implementation.Auths.Incomings;
using Helpers.Commons.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Commons.Constants;
using Presentation.ExtensionMethods.HttpResponses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Presentation.Areas.Admin.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public const string Path = "/Admin/Login";

        private readonly ISystemAccountAuthHandlingService _systemAccountAuthService;
        private readonly ISystemAccountTokenHandlingService _systemAccountTokenService;

        public LoginModel(
            ISystemAccountAuthHandlingService systemAccountAuthService,
            ISystemAccountTokenHandlingService systemAccountTokenService)
        {
            _systemAccountAuthService = systemAccountAuthService;
            _systemAccountTokenService = systemAccountTokenService;
        }

        [BindProperty]
        public LoginDto LoginDto { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            LoginDto.NormalizeAllProperties();
            var isLoginByUserName = LoginDto.IsLoginByUsername();

            var isExisted = false;
            if (isLoginByUserName)
            {
                isExisted = await _systemAccountAuthService.IsUsernameExistedAsync(
                    username: LoginDto.UsernameOrEmail,
                    cancellationToken: cancellationToken);

                if (!isExisted)
                {
                    ErrorMessage = "Username is not existed, please check again.";

                    return Page();
                }
            }
            else
            {
                isExisted = await _systemAccountAuthService.IsEmailExistedAsync(
                    email: LoginDto.UsernameOrEmail,
                    cancellationToken: cancellationToken);

                if (!isExisted)
                {
                    ErrorMessage = "Email is not existed, please check again.";

                    return Page();
                }
            }

            IResult<SystemAccountEntity> result = Result<SystemAccountEntity>.Failed();

            if (LoginDto.IsLoginByUsername())
            {
                result = await _systemAccountAuthService.LoginByUserNameAsync(
                    username: LoginDto.Username,
                    password: LoginDto.Password,
                    cancellationToken: cancellationToken);
            }
            else
            {
                result = await _systemAccountAuthService.LoginByEmailAsync(
                    email: LoginDto.Email,
                    password: LoginDto.Password,
                    cancellationToken: cancellationToken);
            }

            if (!result.IsSuccess)
            {
                ErrorMessage = "Invalid login information, please try again.";

                return Page();
            }

            var systemAccount = result.Value;

            // Generate access-token section.
            var claims = new List<Claim>(4)
            {
                new(type: JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(type: JwtRegisteredClaimNames.Sub, systemAccount.Id.ToString()),
                new(type: CustomClaimTypes.AdminId, systemAccount.Id.ToString()),
                new(type: CustomClaimTypes.Username, systemAccount.UserName),
            };

            var accessToken = _systemAccountTokenService.GenerateAccessToken(claims, TimeSpan.FromDays(1));

            Response.AddSystemAccountAccessToken(accessToken: accessToken);

            return RedirectToPage("/Index");
        }
    }
}
