using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMSystem.API.Data;
using CRMSystem.API.Models;

namespace CRMSystem.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar cotizaciones
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CotizacionesController : ControllerBase
    {
        private readonly ContextoBDCRM _context;

        public CotizacionesController(ContextoBDCRM context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de cotizaciones con filtros opcionales
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cotizacion>>> ObtenerCotizaciones(
            [FromQuery] int? clienteId,
            [FromQuery] string? estadoCotizacion,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 50)
        {
            var query = _context.Cotizaciones
                .Include(c => c.Cliente)
                .Include(c => c.Vendedor)
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.Producto)
                .AsQueryable();

            if (clienteId.HasValue)
                query = query.Where(c => c.ClienteId == clienteId);

            if (!string.IsNullOrEmpty(estadoCotizacion))
                query = query.Where(c => c.EstadoCotizacion == estadoCotizacion);

            if (fechaDesde.HasValue)
                query = query.Where(c => c.FechaCotizacion >= fechaDesde);

            if (fechaHasta.HasValue)
                query = query.Where(c => c.FechaCotizacion <= fechaHasta);

            var total = await query.CountAsync();
            var cotizaciones = await query
                .OrderByDescending(c => c.FechaCotizacion)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            Response.Headers.Add("X-Total-Count", total.ToString());
            Response.Headers.Add("X-Page", pagina.ToString());
            Response.Headers.Add("X-Page-Size", tamanoPagina.ToString());

            return Ok(cotizaciones);
        }

        /// <summary>
        /// Obtiene una cotización por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cotizacion>> ObtenerCotizacion(int id)
        {
            var cotizacion = await _context.Cotizaciones
                .Include(c => c.Cliente)
                .Include(c => c.Vendedor)
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cotizacion == null)
                return NotFound();

            return Ok(cotizacion);
        }

        /// <summary>
        /// Crea una nueva cotización
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Cotizacion>> CrearCotizacion([FromBody] CotizacionCreateDto dto)
        {
            // Generate codigo
            var ultimaCotizacion = await _context.Cotizaciones
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync();
            
            var numero = (ultimaCotizacion?.Id ?? 0) + 1;
            var numeroCotizacion = $"COT-{DateTime.UtcNow.Year}-{numero:D4}";

            var cotizacion = new Cotizacion
            {
                NumeroCotizacion = numeroCotizacion,
                ClienteId = dto.ClienteId,
                VendedorId = dto.VendedorId,
                SucursalId = dto.SucursalId,
                FechaCotizacion = DateTime.UtcNow,
                FechaVencimiento = dto.FechaVencimiento,
                EstadoCotizacion = "Borrador",
                Subtotal = 0,
                MontoIVA = 0,
                Total = 0,
                Notas = dto.Notas,
                TerminosCondiciones = dto.TerminosCondiciones
            };

            // Add details
            if (dto.Detalles != null && dto.Detalles.Any())
            {
                decimal subtotal = 0;
                foreach (var detalleDto in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(detalleDto.ProductoId);
                    if (producto == null)
                        return BadRequest($"Producto {detalleDto.ProductoId} no encontrado");

                    var precioUnitario = detalleDto.PrecioUnitario ?? producto.PrecioLista;
                    var subtotalLinea = detalleDto.Cantidad * precioUnitario;
                    var descuento = subtotalLinea * (detalleDto.PorcentajeDescuento / 100);
                    var totalLinea = subtotalLinea - descuento;

                    var detalle = new DetalleCotizacion
                    {
                        ProductoId = detalleDto.ProductoId,
                        Cantidad = detalleDto.Cantidad,
                        PrecioUnitario = precioUnitario,
                        PorcentajeDescuento = detalleDto.PorcentajeDescuento,
                        MontoDescuento = descuento,
                        Subtotal = totalLinea
                    };

                    cotizacion.Detalles.Add(detalle);
                    subtotal += totalLinea;
                }

                cotizacion.Subtotal = subtotal;
                cotizacion.MontoIVA = subtotal * 0.16m; // 16% IVA
                cotizacion.Total = cotizacion.Subtotal + cotizacion.MontoIVA;
            }

            _context.Cotizaciones.Add(cotizacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerCotizacion), new { id = cotizacion.Id }, cotizacion);
        }

        /// <summary>
        /// Actualiza una cotización existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCotizacion(int id, [FromBody] CotizacionUpdateDto dto)
        {
            var cotizacion = await _context.Cotizaciones
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cotizacion == null)
                return NotFound();

            // Update basic fields
            if (dto.FechaVencimiento.HasValue)
                cotizacion.FechaVencimiento = dto.FechaVencimiento.Value;
            
            if (!string.IsNullOrEmpty(dto.Notas))
                cotizacion.Notas = dto.Notas;
            
            if (!string.IsNullOrEmpty(dto.TerminosCondiciones))
                cotizacion.TerminosCondiciones = dto.TerminosCondiciones;

            // Update details if provided
            if (dto.Detalles != null)
            {
                // Remove existing details
                _context.DetallesCotizacion.RemoveRange(cotizacion.Detalles);

                // Add new details
                decimal subtotal = 0;
                foreach (var detalleDto in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(detalleDto.ProductoId);
                    if (producto == null)
                        return BadRequest($"Producto {detalleDto.ProductoId} no encontrado");

                    var precioUnitario = detalleDto.PrecioUnitario ?? producto.PrecioLista;
                    var subtotalLinea = detalleDto.Cantidad * precioUnitario;
                    var descuento = subtotalLinea * (detalleDto.PorcentajeDescuento / 100);
                    var totalLinea = subtotalLinea - descuento;

                    var detalle = new DetalleCotizacion
                    {
                        CotizacionId = id,
                        ProductoId = detalleDto.ProductoId,
                        Cantidad = detalleDto.Cantidad,
                        PrecioUnitario = precioUnitario,
                        PorcentajeDescuento = detalleDto.PorcentajeDescuento,
                        MontoDescuento = descuento,
                        Subtotal = totalLinea
                    };

                    cotizacion.Detalles.Add(detalle);
                    subtotal += totalLinea;
                }

                cotizacion.Subtotal = subtotal;
                cotizacion.MontoIVA = subtotal * 0.16m;
                cotizacion.Total = cotizacion.Subtotal + cotizacion.MontoIVA;
            }

            cotizacion.FechaActualizacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Elimina una cotización
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCotizacion(int id)
        {
            var cotizacion = await _context.Cotizaciones.FindAsync(id);
            if (cotizacion == null)
                return NotFound();

            _context.Cotizaciones.Remove(cotizacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Cambia el estado de una cotización
        /// </summary>
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoDto dto)
        {
            var cotizacion = await _context.Cotizaciones.FindAsync(id);
            if (cotizacion == null)
                return NotFound();

            cotizacion.EstadoCotizacion = dto.NuevoEstado;
            cotizacion.FechaActualizacion = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Obtiene estadísticas de cotizaciones
        /// </summary>
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> ObtenerEstadisticas()
        {
            var total = await _context.Cotizaciones.CountAsync();
            var porEstado = await _context.Cotizaciones
                .GroupBy(c => c.EstadoCotizacion)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count(), Total = g.Sum(c => c.Total) })
                .ToListAsync();

            var totalMonto = await _context.Cotizaciones.SumAsync(c => c.Total);

            return Ok(new
            {
                TotalCotizaciones = total,
                PorEstado = porEstado,
                MontoTotal = totalMonto
            });
        }
    }

    // DTOs
    public class CotizacionCreateDto
    {
        public int ClienteId { get; set; }
        public int VendedorId { get; set; }
        public int SucursalId { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string? Notas { get; set; }
        public string? TerminosCondiciones { get; set; }
        public List<DetalleCotizacionDto>? Detalles { get; set; }
    }

    public class CotizacionUpdateDto
    {
        public DateTime? FechaVencimiento { get; set; }
        public string? Notas { get; set; }
        public string? TerminosCondiciones { get; set; }
        public List<DetalleCotizacionDto>? Detalles { get; set; }
    }

    public class DetalleCotizacionDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal PorcentajeDescuento { get; set; }
    }

    public class CambiarEstadoDto
    {
        public string NuevoEstado { get; set; } = string.Empty;
    }
}

