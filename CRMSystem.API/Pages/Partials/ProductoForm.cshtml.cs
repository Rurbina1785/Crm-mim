using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRMSystem.API.Pages.Partials
{
    public class ProductoFormModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? ProductoId { get; set; }

        public void OnGet()
        {
            // Form will load data via JavaScript if ProductoId is provided
        }
    }
}

