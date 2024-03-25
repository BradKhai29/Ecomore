using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Commons.Constants;
using Presentation.ExtensionMethods.HttpResponses;

namespace Presentation.Areas.Admin.Pages.Auth
{
    //[Authorize(AuthenticationSchemes = CookieAuthenticationSchemes.SystemAccountScheme)]
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Response.RemoveAdminAccessToken();

            return RedirectToPage("/Admin/Index");
        }
    }
}
