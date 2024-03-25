using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Commons.Constants;

namespace Presentation.Areas.Admin.Pages
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationSchemes.SystemAccountScheme)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
