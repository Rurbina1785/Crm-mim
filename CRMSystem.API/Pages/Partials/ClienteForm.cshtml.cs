using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRMSystem.API.Pages.Partials
{
    public class ClienteFormModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int? ClienteId { get; set; }

        public void OnGet()
        {
            // Form will load data via JavaScript if ClienteId is provided
        }
    }
}

