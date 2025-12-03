using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CRMSystem.API.Data;
using CRMSystem.API.Models;

namespace CRMSystem.API.Pages.Partials
{
    public class ProspectosListModel : PageModel
    {
        private readonly ContextoBDCRM _context;

        public ProspectosListModel(ContextoBDCRM context)
        {
            _context = context;
        }

        public IEnumerable<Prospecto> Prospectos { get; set; } = new List<Prospecto>();

        [BindProperty(SupportsGet = true)]
        public int? SucursalId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FuenteId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Estado { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? VendedorId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Busqueda { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var query = _context.Prospectos
                .Include(p => p.Fuente)
                .Include(p => p.VendedorAsignado)
                .Include(p => p.Sucursal)
                .AsQueryable();

            // Apply filters
            if (SucursalId.HasValue)
                query = query.Where(p => p.SucursalId == SucursalId.Value);

            if (FuenteId.HasValue)
                query = query.Where(p => p.FuenteId == FuenteId.Value);

            if (!string.IsNullOrEmpty(Estado))
                query = query.Where(p => p.EstadoProspecto == Estado);

            if (VendedorId.HasValue)
                query = query.Where(p => p.VendedorAsignadoId == VendedorId.Value);

            if (!string.IsNullOrEmpty(Busqueda))
            {
                query = query.Where(p =>
                    p.NombreContacto.Contains(Busqueda) ||
                    p.ApellidoContacto!.Contains(Busqueda) ||
                    p.NombreEmpresa.Contains(Busqueda) ||
                    p.Email!.Contains(Busqueda));
            }

            // Get results
            Prospectos = await query
                .OrderByDescending(p => p.FechaCreacion)
                .Take(50) // Limit for performance
                .ToListAsync();

            // Return the page with the model
            return Page();
        }
    }
}

