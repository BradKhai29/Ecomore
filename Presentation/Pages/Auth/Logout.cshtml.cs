using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.ExtensionMethods.HttpResponses;

namespace Presentation.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public const string Path = "/Auth/Logout";

        public IActionResult OnGet()
        {
            HttpContext.Response.RemoveUserAccessToken();

            return RedirectToPage("/Index");
        }
    }
}
