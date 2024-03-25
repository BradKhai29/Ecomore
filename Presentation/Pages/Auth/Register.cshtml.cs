using BusinessLogic.Services.Cores.Base;
using DTOs.Implementation.Auths.Incomings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IUserAuthHandlingService _userAuthHandlingService;

        [BindProperty]
        public RegisterDto RegisterDto { get; set; }

        public string ErrorMessage { get; set; }

        public RegisterModel(IUserAuthHandlingService userAuthHandlingService)
        {
            _userAuthHandlingService = userAuthHandlingService;
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            RegisterDto.NormalizeAllProperties();

            var isExisted = await _userAuthHandlingService.IsUsernameExistedAsync(
                username: RegisterDto.Username,
                cancellationToken: cancellationToken);

            if (isExisted)
            {
                ErrorMessage = "Username is already existed";
                return Page();
            }

            isExisted = await _userAuthHandlingService.IsEmailExistedAsync(
                email: RegisterDto.Email,
                cancellationToken: cancellationToken);

            if (isExisted)
            {
                ErrorMessage = "Email is already existed";
                return Page();
            }

            var result = await _userAuthHandlingService.RegisterAsync(
                registerDto: RegisterDto,
                cancellationToken: cancellationToken);

            if (!result.IsSuccess)
            {
                return Page();
            }

            return RedirectToPage("Login");
        }
    }
}
