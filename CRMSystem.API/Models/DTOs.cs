using System.ComponentModel.DataAnnotations;

namespace CRMSystem.API.Models;

/// <summary>
/// DTOs (Data Transfer Objects) para la API CRM
/// Estos objetos se usan para recibir datos del cliente sin exponer los modelos de dominio
/// </summary>

#region Prospectos DTOs

/// <summary>
/// DTO para crear un nuevo prospecto
/// </summary>
public class CrearProspectoDto
{
    [Required(ErrorMessage = "El nombre de la empresa es requerido")]
    [StringLength(200)]
    public string NombreEmpresa { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre del contacto es requerido")]
    [StringLength(100)]
    public string NombreContacto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido del contacto es requerido")]
    [StringLength(100)]
    public string ApellidoContacto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El formato del teléfono no es válido")]
    [StringLength(20)]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La fuente es requerida")]
    public int FuenteProspectoId { get; set; }

    [Required(ErrorMessage = "La sucursal es requerida")]
    public int SucursalId { get; set; }

    public int? VendedorAsignadoId { get; set; }

    [StringLength(50)]
    public string EstadoProspecto { get; set; } = "Nuevo";

    [StringLength(50)]
    public string? Prioridad { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El valor estimado debe ser mayor o igual a 0")]
    public decimal? ValorEstimado { get; set; }

    [Range(0, 100, ErrorMessage = "La probabilidad debe estar entre 0 y 100")]
    public int? ProbabilidadCierre { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }
}

/// <summary>
/// DTO para actualizar un prospecto existente
/// </summary>
public class ActualizarProspectoDto
{
    [StringLength(200)]
    public string? NombreEmpresa { get; set; }

    [StringLength(100)]
    public string? NombreContacto { get; set; }

    [StringLength(100)]
    public string? ApellidoContacto { get; set; }

    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }

    [Phone]
    [StringLength(20)]
    public string? Telefono { get; set; }

    public int? FuenteId { get; set; }

    public int? SucursalId { get; set; }

    public int? VendedorAsignadoId { get; set; }

    [StringLength(50)]
    public string? EstadoProspecto { get; set; }

    [StringLength(50)]
    public string? Prioridad { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? ValorEstimado { get; set; }

    [Range(0, 100)]
    public int? ProbabilidadCierre { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }
}

#endregion

#region Clientes DTOs

/// <summary>
/// DTO para crear un nuevo cliente
/// </summary>
public class CrearClienteDto
{
    [Required(ErrorMessage = "El nombre de la empresa es requerido")]
    [StringLength(200)]
    public string NombreEmpresa { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre comercial es requerido")]
    [StringLength(200)]
    public string NombreComercial { get; set; } = string.Empty;

    [Required(ErrorMessage = "El RFC es requerido")]
    [StringLength(13)]
    public string RFC { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Direccion { get; set; }

    [StringLength(100)]
    public string? Ciudad { get; set; }

    [StringLength(100)]
    public string? Estado { get; set; }

    [StringLength(10)]
    public string? CodigoPostal { get; set; }

    [StringLength(100)]
    public string? Pais { get; set; } = "México";

    [Phone]
    [StringLength(20)]
    public string? TelefonoPrincipal { get; set; }

    [EmailAddress]
    [StringLength(150)]
    public string? EmailPrincipal { get; set; }

    [Url]
    [StringLength(200)]
    public string? SitioWeb { get; set; }

    [Required(ErrorMessage = "La categoría es requerida")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "La sucursal es requerida")]
    public int SucursalId { get; set; }

    public int? EjecutivoCuentaId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? LimiteCredito { get; set; }

    [Range(0, 365)]
    public int? DiasCredito { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }
}

/// <summary>
/// DTO para actualizar un cliente existente
/// </summary>
public class ActualizarClienteDto
{
    [StringLength(200)]
    public string? NombreEmpresa { get; set; }

    [StringLength(200)]
    public string? NombreComercial { get; set; }

    [StringLength(13)]
    public string? RFC { get; set; }

    [StringLength(500)]
    public string? Direccion { get; set; }

    [StringLength(100)]
    public string? Ciudad { get; set; }

    [StringLength(100)]
    public string? Estado { get; set; }

    [StringLength(10)]
    public string? CodigoPostal { get; set; }

    [StringLength(100)]
    public string? Pais { get; set; }

    [Phone]
    [StringLength(20)]
    public string? TelefonoPrincipal { get; set; }

    [EmailAddress]
    [StringLength(150)]
    public string? EmailPrincipal { get; set; }

    [Url]
    [StringLength(200)]
    public string? SitioWeb { get; set; }

    public int? CategoriaId { get; set; }

    public int? SucursalId { get; set; }

    public int? EjecutivoCuentaId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? LimiteCredito { get; set; }

    [Range(0, 365)]
    public int? DiasCredito { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }

    public bool? EstaActivo { get; set; }
}

#endregion

#region Productos DTOs

/// <summary>
/// DTO para crear un nuevo producto
/// </summary>
public class CrearProductoDto
{
    [Required(ErrorMessage = "El nombre del producto es requerido")]
    [StringLength(200)]
    public string NombreProducto { get; set; } = string.Empty;

    [StringLength(50)]
    public string? SKU { get; set; }

    [StringLength(1000)]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "La categoría es requerida")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
    public decimal PrecioUnitario { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Costo { get; set; }

    [StringLength(20)]
    public string? UnidadMedida { get; set; }

    [Range(0, int.MaxValue)]
    public int? Stock { get; set; }

    [Range(0, int.MaxValue)]
    public int? StockMinimo { get; set; }

    public bool EstaActivo { get; set; } = true;
}

/// <summary>
/// DTO para actualizar un producto existente
/// </summary>
public class ActualizarProductoDto
{
    [StringLength(200)]
    public string? NombreProducto { get; set; }

    [StringLength(50)]
    public string? SKU { get; set; }

    [StringLength(1000)]
    public string? Descripcion { get; set; }

    public int? CategoriaId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? PrecioUnitario { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Costo { get; set; }

    [StringLength(20)]
    public string? UnidadMedida { get; set; }

    [Range(0, int.MaxValue)]
    public int? Stock { get; set; }

    [Range(0, int.MaxValue)]
    public int? StockMinimo { get; set; }

    public bool? EstaActivo { get; set; }
}

#endregion

#region Cotizaciones DTOs

/// <summary>
/// DTO para crear una nueva cotización
/// </summary>
public class CrearCotizacionDto
{
    [Required(ErrorMessage = "El cliente es requerido")]
    public int ClienteId { get; set; }

    public int? ProspectoId { get; set; }

    [Required(ErrorMessage = "El vendedor es requerido")]
    public int VendedorId { get; set; }

    [Required(ErrorMessage = "La fecha de emisión es requerida")]
    public DateTime FechaEmision { get; set; }

    [Required(ErrorMessage = "La fecha de vigencia es requerida")]
    public DateTime FechaVigencia { get; set; }

    [Required(ErrorMessage = "El subtotal es requerido")]
    [Range(0, double.MaxValue)]
    public decimal Subtotal { get; set; }

    [Range(0, 100)]
    public decimal? PorcentajeDescuento { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MontoDescuento { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? IVA { get; set; }

    [Required(ErrorMessage = "El total es requerido")]
    [Range(0, double.MaxValue)]
    public decimal Total { get; set; }

    [StringLength(50)]
    public string EstadoCotizacion { get; set; } = "Borrador";

    [StringLength(500)]
    public string? Notas { get; set; }

    [StringLength(500)]
    public string? TerminosCondiciones { get; set; }

    public List<CrearDetalleCotizacionDto> Detalles { get; set; } = new();
}

/// <summary>
/// DTO para detalle de cotización
/// </summary>
public class CrearDetalleCotizacionDto
{
    [Required]
    public int ProductoId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal PrecioUnitario { get; set; }

    [Range(0, 100)]
    public decimal? PorcentajeDescuento { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MontoDescuento { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Subtotal { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }
}

#endregion

#region Visitas DTOs

/// <summary>
/// DTO para crear una nueva visita
/// </summary>
public class CrearVisitaDto
{
    public int? ClienteId { get; set; }

    public int? ProspectoId { get; set; }

    [Required(ErrorMessage = "El vendedor es requerido")]
    public int VendedorId { get; set; }

    [Required(ErrorMessage = "La fecha de visita es requerida")]
    public DateTime FechaVisita { get; set; }

    [StringLength(200)]
    public string? Ubicacion { get; set; }

    [Required(ErrorMessage = "El tipo de visita es requerido")]
    [StringLength(50)]
    public string TipoVisita { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Objetivo { get; set; }

    [StringLength(2000)]
    public string? Resumen { get; set; }

    [StringLength(1000)]
    public string? AccionesSeguimiento { get; set; }

    [StringLength(50)]
    public string EstadoVisita { get; set; } = "Programada";

    [Range(1, 10)]
    public int? CalificacionVisita { get; set; }
}

#endregion

#region Tareas DTOs

/// <summary>
/// DTO para crear una nueva tarea
/// </summary>
public class CrearTareaDto
{
    [Required(ErrorMessage = "El título es requerido")]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El usuario asignado es requerido")]
    public int UsuarioAsignadoId { get; set; }

    public int? ClienteId { get; set; }

    public int? ProspectoId { get; set; }

    [Required(ErrorMessage = "La fecha de vencimiento es requerida")]
    public DateTime FechaVencimiento { get; set; }

    [Required(ErrorMessage = "La prioridad es requerida")]
    [StringLength(50)]
    public string Prioridad { get; set; } = "Media";

    [StringLength(50)]
    public string EstadoTarea { get; set; } = "Pendiente";

    [StringLength(50)]
    public string? TipoTarea { get; set; }
}

#endregion

