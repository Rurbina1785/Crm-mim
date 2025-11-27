using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMSystem.API.Data;
using CRMSystem.API.Models;

namespace CRMSystem.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de clientes en el sistema CRM
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json", "text/html")]
    public class ClientesController : Controller
    {
        private readonly ContextoBDCRM _context;

        public ClientesController(ContextoBDCRM context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de clientes con filtros opcionales
        /// </summary>
        /// <param name="sucursalId">ID de la sucursal para filtrar</param>
        /// <param name="categoriaId">ID de la categoría de cliente</param>
        /// <param name="estado">Estado del cliente (Activo, Inactivo, Suspendido)</param>
        /// <param name="vendedorId">ID del vendedor asignado</param>
        /// <param name="busqueda">Término de búsqueda en nombre de empresa, RFC o email</param>
        /// <param name="pagina">Número de página para paginación</param>
        /// <param name="tamañoPagina">Cantidad de registros por página</param>
        /// <returns>Lista de clientes o vista parcial HTML para HTMX</returns>
        [HttpGet]
        public async Task<IActionResult> ObtenerClientes(
            [FromQuery] int? sucursalId = null,
            [FromQuery] int? categoriaId = null,
            [FromQuery] string? estado = null,
            [FromQuery] int? vendedorId = null,
            [FromQuery] string? busqueda = null,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamañoPagina = 50)
        {
            var query = _context.Clientes
                .Include(c => c.Categoria)
                .Include(c => c.VendedorAsignado)
                .Include(c => c.Sucursal)
                .Include(c => c.Contactos)
                .AsQueryable();

            // Aplicar filtros
            if (sucursalId.HasValue)
                query = query.Where(c => c.SucursalId == sucursalId.Value);

            if (categoriaId.HasValue)
                query = query.Where(c => c.CategoriaId == categoriaId.Value);

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(c => c.EstadoCliente == estado);

            if (vendedorId.HasValue)
                query = query.Where(c => c.VendedorAsignadoId == vendedorId.Value);

            if (!string.IsNullOrEmpty(busqueda))
            {
                query = query.Where(c => 
                    c.NombreEmpresa.Contains(busqueda) ||
                    c.RFC!.Contains(busqueda) ||
                    c.Email!.Contains(busqueda));
            }

            // Aplicar paginación
            var totalRegistros = await query.CountAsync();
            var clientes = await query
                .OrderByDescending(c => c.FechaCreacion)
                .Skip((pagina - 1) * tamañoPagina)
                .Take(tamañoPagina)
                .ToListAsync();

            // Agregar headers de paginación
            Response.Headers.Add("X-Total-Count", totalRegistros.ToString());
            Response.Headers.Add("X-Page", pagina.ToString());
            Response.Headers.Add("X-Page-Size", tamañoPagina.ToString());

            // Si la petición es HTMX, devolver vista parcial
            if (Request.Headers["HX-Request"] == "true")
            {
                return PartialView("~/Pages/Partials/_ClientesCards.cshtml", clientes);
            }

            // Si es petición API normal, devolver JSON
            return Ok(clientes);
        }

        /// <summary>
        /// Obtiene un cliente específico por su ID
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Datos del cliente o vista parcial HTML para HTMX</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Categoria)
                .Include(c => c.VendedorAsignado)
                .Include(c => c.Sucursal)
                .Include(c => c.Contactos)
                .Include(c => c.Cotizaciones)
                .Include(c => c.Visitas)
                .Include(c => c.Tareas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado" });
            }

            // Si la petición es HTMX, devolver vista parcial de detalles
            if (Request.Headers["HX-Request"] == "true")
            {
                return PartialView("~/Pages/Partials/_ClienteDetalle.cshtml", cliente);
            }

            return Ok(cliente);
        }

        /// <summary>
        /// Crea un nuevo cliente en el sistema
        /// </summary>
        /// <param name="cliente">Datos del nuevo cliente</param>
        /// <returns>Cliente creado con código generado automáticamente</returns>
        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Generar código de cliente automáticamente
            var año = DateTime.Now.Year;
            var ultimoCliente = await _context.Clientes
                .Where(c => c.CodigoCliente.StartsWith($"CLI-{año}-"))
                .OrderByDescending(c => c.CodigoCliente)
                .FirstOrDefaultAsync();

            int siguienteNumero = 1;
            if (ultimoCliente != null)
            {
                var partes = ultimoCliente.CodigoCliente.Split('-');
                if (partes.Length == 3 && int.TryParse(partes[2], out int numero))
                {
                    siguienteNumero = numero + 1;
                }
            }

            cliente.CodigoCliente = $"CLI-{año}-{siguienteNumero:D3}";
            cliente.FechaRegistro = DateTime.Now;
            cliente.FechaCreacion = DateTime.Now;
            cliente.FechaActualizacion = DateTime.Now;

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Si la petición es HTMX, devolver lista actualizada
            if (Request.Headers["HX-Request"] == "true")
            {
                var clientes = await _context.Clientes
                    .Include(c => c.Categoria)
                    .Include(c => c.VendedorAsignado)
                    .Include(c => c.Sucursal)
                    .OrderByDescending(c => c.FechaCreacion)
                    .Take(50)
                    .ToListAsync();

                Response.Headers.Add("X-Success-Message", "Cliente creado exitosamente");
                return PartialView("~/Pages/Partials/_ClientesCards.cshtml", clientes);
            }

            return CreatedAtAction(nameof(ObtenerCliente), new { id = cliente.Id }, cliente);
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        /// <param name="id">ID del cliente a actualizar</param>
        /// <param name="cliente">Datos actualizados del cliente</param>
        /// <returns>Cliente actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest(new { mensaje = "El ID del cliente no coincide" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clienteExistente = await _context.Clientes.FindAsync(id);
            if (clienteExistente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado" });
            }

            // Actualizar campos
            clienteExistente.NombreEmpresa = cliente.NombreEmpresa;
            clienteExistente.RFC = cliente.RFC;
            clienteExistente.Industria = cliente.Industria;
            clienteExistente.SitioWeb = cliente.SitioWeb;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Email = cliente.Email;
            clienteExistente.Direccion = cliente.Direccion;
            clienteExistente.Ciudad = cliente.Ciudad;
            clienteExistente.Estado = cliente.Estado;
            clienteExistente.CodigoPostal = cliente.CodigoPostal;
            clienteExistente.Pais = cliente.Pais;
            clienteExistente.CategoriaId = cliente.CategoriaId;
            clienteExistente.VendedorAsignadoId = cliente.VendedorAsignadoId;
            clienteExistente.EstadoCliente = cliente.EstadoCliente;
            clienteExistente.Notas = cliente.Notas;
            clienteExistente.FechaActualizacion = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClienteExiste(id))
                {
                    return NotFound(new { mensaje = "Cliente no encontrado" });
                }
                throw;
            }

            // Si la petición es HTMX, devolver lista actualizada
            if (Request.Headers["HX-Request"] == "true")
            {
                var clientes = await _context.Clientes
                    .Include(c => c.Categoria)
                    .Include(c => c.VendedorAsignado)
                    .Include(c => c.Sucursal)
                    .OrderByDescending(c => c.FechaCreacion)
                    .Take(50)
                    .ToListAsync();

                Response.Headers.Add("X-Success-Message", "Cliente actualizado exitosamente");
                return PartialView("~/Pages/Partials/_ClientesCards.cshtml", clientes);
            }

            return Ok(clienteExistente);
        }

        /// <summary>
        /// Elimina un cliente del sistema
        /// </summary>
        /// <param name="id">ID del cliente a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado" });
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            // Si la petición es HTMX, devolver lista actualizada
            if (Request.Headers["HX-Request"] == "true")
            {
                var clientes = await _context.Clientes
                    .Include(c => c.Categoria)
                    .Include(c => c.VendedorAsignado)
                    .Include(c => c.Sucursal)
                    .OrderByDescending(c => c.FechaCreacion)
                    .Take(50)
                    .ToListAsync();

                Response.Headers.Add("X-Success-Message", "Cliente eliminado exitosamente");
                return PartialView("~/Pages/Partials/_ClientesCards.cshtml", clientes);
            }

            return Ok(new { mensaje = "Cliente eliminado exitosamente" });
        }

        /// <summary>
        /// Obtiene los contactos de un cliente específico
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Lista de contactos del cliente</returns>
        [HttpGet("{id}/contactos")]
        public async Task<IActionResult> ObtenerContactosCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Contactos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado" });
            }

            // Si la petición es HTMX, devolver vista parcial
            if (Request.Headers["HX-Request"] == "true")
            {
                return PartialView("~/Pages/Partials/_ContactosCliente.cshtml", cliente.Contactos);
            }

            return Ok(cliente.Contactos);
        }

        /// <summary>
        /// Agrega un nuevo contacto a un cliente
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <param name="contacto">Datos del nuevo contacto</param>
        /// <returns>Contacto creado</returns>
        [HttpPost("{id}/contactos")]
        public async Task<IActionResult> AgregarContacto(int id, [FromBody] ContactoCliente contacto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado" });
            }

            contacto.ClienteId = id;
            contacto.FechaCreacion = DateTime.Now;
            contacto.FechaActualizacion = DateTime.Now;

            _context.ContactosCliente.Add(contacto);
            await _context.SaveChangesAsync();

            return Ok(contacto);
        }

        /// <summary>
        /// Obtiene categorías de clientes disponibles
        /// </summary>
        /// <returns>Lista de categorías de clientes</returns>
        [HttpGet("categorias")]
        public async Task<IActionResult> ObtenerCategorias()
        {
            var categorias = await _context.CategoriasCliente.ToListAsync();
            return Ok(categorias);
        }

        /// <summary>
        /// Obtiene estadísticas de clientes por categoría
        /// </summary>
        /// <returns>Distribución de clientes por categoría</returns>
        [HttpGet("estadisticas-categorias")]
        public async Task<IActionResult> ObtenerEstadisticasCategorias()
        {
            var estadisticas = await _context.Clientes
                .GroupBy(c => c.Categoria.NombreCategoria)
                .Select(g => new 
                {
                    Categoria = g.Key,
                    Cantidad = g.Count(),
                    ValorTotal = g.Sum(c => c.ValorVidaCliente)
                })
                .ToListAsync();

            return Ok(estadisticas);
        }

        /// <summary>
        /// Obtiene estadísticas de clientes por sucursal
        /// </summary>
        /// <returns>Distribución de clientes por sucursal</returns>
        [HttpGet("estadisticas-sucursales")]
        public async Task<IActionResult> ObtenerEstadisticasSucursales()
        {
            var estadisticas = await _context.Clientes
                .GroupBy(c => c.Sucursal.NombreSucursal)
                .Select(g => new 
                {
                    Sucursal = g.Key,
                    Cantidad = g.Count(),
                    ValorTotal = g.Sum(c => c.ValorVidaCliente)
                })
                .ToListAsync();

            return Ok(estadisticas);
        }

        private async Task<bool> ClienteExiste(int id)
        {
            return await _context.Clientes.AnyAsync(c => c.Id == id);
        }
    }
}

