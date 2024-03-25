using BusinessLogic.Services.Cores.Base;
using DTOs.Implementation.Auths.Incomings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Areas.Admin.Pages.Auth
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUserTokenHandlingService _userTokenHandlingService;

        [BindProperty]
        public ForgotPasswordDto ForgotPassword { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }
    }
}
