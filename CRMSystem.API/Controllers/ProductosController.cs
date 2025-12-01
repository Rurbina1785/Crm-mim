using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMSystem.API.Data;
using CRMSystem.API.Models;

namespace CRMSystem.API.Controllers;

/// <summary>
/// Controlador para gestionar productos del catálogo
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductosController : ControllerBase
{
    private readonly ContextoBDCRM _context;

    public ProductosController(ContextoBDCRM context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene la lista de productos con filtros opcionales
    /// </summary>
    /// <param name="categoriaId">ID de categoría para filtrar</param>
    /// <param name="busqueda">Término de búsqueda en código, nombre o descripción</param>
    /// <param name="activo">Filtrar por productos activos/inactivos</param>
    /// <param name="pagina">Número de página (default: 1)</param>
    /// <param name="tamanoPagina">Tamaño de página (default: 50)</param>
    /// <returns>Lista de productos</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> ObtenerProductos(
        [FromQuery] int? categoriaId = null,
        [FromQuery] string? busqueda = null,
        [FromQuery] bool? activo = null,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 50)
    {
        var query = _context.Productos
            .Include(p => p.Categoria)
            .AsQueryable();

        // Aplicar filtros
        if (categoriaId.HasValue)
        {
            query = query.Where(p => p.CategoriaId == categoriaId.Value);
        }

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            query = query.Where(p =>
                p.CodigoProducto.Contains(busqueda) ||
                p.NombreProducto.Contains(busqueda) ||
                (p.Descripcion != null && p.Descripcion.Contains(busqueda)));
        }

        if (activo.HasValue)
        {
            query = query.Where(p => p.EstaActivo == activo.Value);
        }

        // Paginación
        var total = await query.CountAsync();
        var productos = await query
            .OrderBy(p => p.NombreProducto)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();

        // Headers de paginación
        Response.Headers.Append("X-Total-Count", total.ToString());
        Response.Headers.Append("X-Page", pagina.ToString());
        Response.Headers.Append("X-Page-Size", tamanoPagina.ToString());

        return Ok(productos);
    }

    /// <summary>
    /// Obtiene un producto por su ID
    /// </summary>
    /// <param name="id">ID del producto</param>
    /// <returns>Producto encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> ObtenerProducto(int id)
    {
        var producto = await _context.Productos
            .Include(p => p.Categoria)

            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
        {
            return NotFound(new { mensaje = $"Producto con ID {id} no encontrado" });
        }

        return Ok(producto);
    }

    /// <summary>
    /// Obtiene todas las categorías de productos
    /// </summary>
    /// <returns>Lista de categorías</returns>
    [HttpGet("categorias")]
    public async Task<ActionResult<IEnumerable<CategoriaProducto>>> ObtenerCategorias()
    {
        var categorias = await _context.CategoriasProducto
            .OrderBy(c => c.NombreCategoria)
            .ToListAsync();

        return Ok(categorias);
    }



    /// <summary>
    /// Obtiene estadísticas de productos por categoría
    /// </summary>
    /// <returns>Estadísticas agrupadas por categoría</returns>
    [HttpGet("estadisticas-categorias")]
    public async Task<ActionResult> ObtenerEstadisticasCategorias()
    {
        var estadisticas = await _context.Productos
            .Include(p => p.Categoria)
            .GroupBy(p => new { p.CategoriaId, p.Categoria!.NombreCategoria })
            .Select(g => new
            {
                categoriaId = g.Key.CategoriaId,
                nombreCategoria = g.Key.NombreCategoria,
                totalProductos = g.Count(),
                productosActivos = g.Count(p => p.EstaActivo),
                productosInactivos = g.Count(p => !p.EstaActivo),
                precioPromedio = g.Average(p => p.PrecioLista),
                precioMinimo = g.Min(p => p.PrecioLista),
                precioMaximo = g.Max(p => p.PrecioLista)
            })
            .ToListAsync();

        return Ok(estadisticas);
    }

    /// <summary>
    /// Crea un nuevo producto
    /// </summary>
    /// <param name="dto">Datos del producto a crear</param>
    /// <returns>Producto creado</returns>
    [HttpPost]
    public async Task<ActionResult<Producto>> CrearProducto(CrearProductoDto dto)
    {
        // Verificar que la categoría existe
        var categoria = await _context.CategoriasProducto.FindAsync(dto.CategoriaId);
        if (categoria == null)
        {
            return BadRequest(new { mensaje = "La categoría especificada no existe" });
        }

        // Generar código único si no se proporciona
        string codigoProducto = dto.CodigoProducto ?? await GenerarCodigoProducto();

        // Verificar que el código no exista
        if (await _context.Productos.AnyAsync(p => p.CodigoProducto == codigoProducto))
        {
            return BadRequest(new { mensaje = $"Ya existe un producto con el código {codigoProducto}" });
        }

        var producto = new Producto
        {
            CodigoProducto = codigoProducto,
            NombreProducto = dto.NombreProducto,
            Descripcion = dto.Descripcion,
            CategoriaId = dto.CategoriaId,
            PrecioLista = dto.PrecioUnitario,
            Moneda = dto.Moneda ?? "MXN",
            UnidadMedida = dto.UnidadMedida ?? "Pieza",
            Stock = dto.StockDisponible ?? 0,
            StockMinimo = dto.StockMinimo ?? 0,
            EstaActivo = dto.EstaActivo ?? true,
            FechaCreacion = DateTime.UtcNow,
            FechaActualizacion = DateTime.UtcNow
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();



        return CreatedAtAction(nameof(ObtenerProducto), new { id = producto.Id }, producto);
    }

    /// <summary>
    /// Actualiza un producto existente
    /// </summary>
    /// <param name="id">ID del producto</param>
    /// <param name="dto">Datos actualizados</param>
    /// <returns>Producto actualizado</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarProducto(int id, ActualizarProductoDto dto)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound(new { mensaje = $"Producto con ID {id} no encontrado" });
        }

        // Verificar cambio de precio
        decimal precioAnterior = producto.PrecioLista;
        bool cambioPrecio = dto.PrecioUnitario.HasValue && dto.PrecioUnitario.Value != precioAnterior;

        // Actualizar campos
        if (!string.IsNullOrWhiteSpace(dto.NombreProducto))
            producto.NombreProducto = dto.NombreProducto;

        if (dto.Descripcion != null)
            producto.Descripcion = dto.Descripcion;

        if (dto.CategoriaId.HasValue)
            producto.CategoriaId = dto.CategoriaId.Value;

        if (dto.PrecioUnitario.HasValue)
            producto.PrecioLista = dto.PrecioUnitario.Value;

        if (dto.StockDisponible.HasValue)
            producto.Stock = dto.StockDisponible.Value;

        if (dto.StockMinimo.HasValue)
            producto.StockMinimo = dto.StockMinimo.Value;

        if (dto.EstaActivo.HasValue)
            producto.EstaActivo = dto.EstaActivo.Value;

        producto.FechaActualizacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();



        return NoContent();
    }

    /// <summary>
    /// Elimina un producto (soft delete)
    /// </summary>
    /// <param name="id">ID del producto</param>
    /// <returns>Confirmación de eliminación</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarProducto(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound(new { mensaje = $"Producto con ID {id} no encontrado" });
        }

        // Soft delete
        producto.EstaActivo = false;
        producto.FechaActualizacion = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Genera un código único para un producto
    /// </summary>
    private async Task<string> GenerarCodigoProducto()
    {
        var ultimoProducto = await _context.Productos
            .OrderByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        int siguienteNumero = (ultimoProducto?.Id ?? 0) + 1;
        return $"PROD-{DateTime.UtcNow.Year}-{siguienteNumero:D4}";
    }
}

/// <summary>
/// DTO para crear un producto
/// </summary>
public class CrearProductoDto
{
    public string? CodigoProducto { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int CategoriaId { get; set; }
    public decimal PrecioUnitario { get; set; }
    public string? Moneda { get; set; }
    public string? UnidadMedida { get; set; }
    public int? StockDisponible { get; set; }
    public int? StockMinimo { get; set; }
    public bool? EstaActivo { get; set; }
}

/// <summary>
/// DTO para actualizar un producto
/// </summary>
public class ActualizarProductoDto
{
    public string? NombreProducto { get; set; }
    public string? Descripcion { get; set; }
    public int? CategoriaId { get; set; }
    public decimal? PrecioUnitario { get; set; }
    public int? StockDisponible { get; set; }
    public int? StockMinimo { get; set; }
    public bool? EstaActivo { get; set; }
    public string? MotivoCambioPrecio { get; set; }
}

