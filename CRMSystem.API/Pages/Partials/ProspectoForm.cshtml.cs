using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRMSystem.API.Pages.Partials
{
    public class ProspectoFormModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? ProspectoId { get; set; }

        public void OnGet()
        {
            // Form will load data via JavaScript if ProspectoId is provided
        }
    }
}

