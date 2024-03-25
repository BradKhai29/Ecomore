using BusinessLogic.Models;
using BusinessLogic.Models.Base;
using BusinessLogic.Services.Cores.Base;
using DataAccess.Entities;
using DTOs.Implementation.Auths.Incomings;
using Helpers.Commons.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Options.Models.Jwts;
using Presentation.Commons.Constants;
using Presentation.ExtensionMethods.HttpContexts;
using Presentation.ExtensionMethods.HttpResponses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Presentation.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public const string Path = "/Auth/Login";

        private readonly IUserAuthHandlingService _userAuthHandlingService;
        private readonly IUserTokenHandlingService _userTokenHandlingService;
        private readonly UserAccountJwtOptions _jwtOptions;

        [BindProperty]
        public LoginDto LoginDto { get; set; }
        public string ErrorMessage { get; set; }

        public LoginModel(
            IUserAuthHandlingService userAuthHandlingService,
            IUserTokenHandlingService userTokenHandlingService,
            IOptions<UserAccountJwtOptions> jwtOptions)
        {
            _userAuthHandlingService = userAuthHandlingService;
            _userTokenHandlingService = userTokenHandlingService;
            _jwtOptions = jwtOptions.Value;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.IsUserAuthenticated())
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            LoginDto.NormalizeAllProperties();
            var isLoginByUserName = LoginDto.IsLoginByUsername();

            var isExisted = false;
            if (isLoginByUserName)
            {
                isExisted = await _userAuthHandlingService.IsUsernameExistedAsync(
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
                isExisted = await _userAuthHandlingService.IsEmailExistedAsync(
                    email: LoginDto.UsernameOrEmail,
                    cancellationToken: cancellationToken);

                if (!isExisted)
                {
                    ErrorMessage = "Email is not existed, please check again.";

                    return Page();
                }
            }

            IResult<UserEntity> result = Result<UserEntity>.Failed();

            if (LoginDto.IsLoginByUsername())
            {
                result = await _userAuthHandlingService.LoginByUsernameAsync(
                    username: LoginDto.Username,
                    password: LoginDto.Password,
                    cancellationToken: cancellationToken);
            }
            else
            {
                result = await _userAuthHandlingService.LoginByEmailAsync(
                    email: LoginDto.Email,
                    password: LoginDto.Password,
                    cancellationToken: cancellationToken);
            }

            if (!result.IsSuccess)
            {
                ErrorMessage = "Invalid login information, please try again.";

                return Page();
            }

            var user = result.Value;

            // Generate access-token section.
            var claims = new List<Claim>(4)
            {
                new(type: JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(type: JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(type: CustomClaimTypes.UserId, user.Id.ToString()),
                new(type: CustomClaimTypes.Username, user.UserName),
                new(type: CustomClaimTypes.AvatarUrl, user.AvatarUrl)
            };

            var lifeSpan = _jwtOptions.GetLifeSpan(isLongLive: LoginDto.RememberMe);

            var accessToken = _userTokenHandlingService.GenerateAccessToken(claims, lifeSpan);

            Response.AddUserAccessToken(accessToken: accessToken, lifeSpan: lifeSpan);

            return RedirectToPage("/Index");
        }
    }
}
