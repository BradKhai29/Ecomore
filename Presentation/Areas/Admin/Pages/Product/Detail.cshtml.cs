using DataAccess.DbContexts;
using DataAccess.UnitOfWorks.Base;
using DTOs.Implementation.Products.Outgoings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Areas.Admin.Pages.Product
{
    public class DetailModel : PageModel
    {
        public DetailModel()
        {
        }

        public IActionResult OnGet(
            [Required]
            [FromRoute] 
            Guid productId)
        {
            ViewData[nameof(productId)] = productId;

            return Page();
        }
    }
}
