using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMSystem.API.Models
{
    // =============================================
    // Modelos de Gestión de Usuarios
    // =============================================

    /// <summary>
    /// Representa un rol de usuario en el sistema CRM con permisos asociados
    /// </summary>
    [Table("RolesUsuario")]
    public class RolUsuario
    {
        /// <summary>
        /// Identificador único del rol
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del rol (ej: Vendedor, Gerente, Director)
        /// </summary>
        [Required, MaxLength(50)]
        public string NombreRol { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada del rol y sus responsabilidades
        /// </summary>
        [MaxLength(255)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Cadena JSON con los permisos asignados al rol
        /// </summary>
        public string? Permisos { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }

    /// <summary>
    /// Representa una sucursal o ubicación física de la empresa
    /// </summary>
    [Table("Sucursales")]
    public class Sucursal
    {
        /// <summary>
        /// Identificador único de la sucursal
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código único de la sucursal (ej: NORTE, CENTRO, SUR)
        /// </summary>
        [Required, MaxLength(10)]
        public string CodigoSucursal { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo de la sucursal
        /// </summary>
        [Required, MaxLength(100)]
        public string NombreSucursal { get; set; } = string.Empty;

        /// <summary>
        /// Dirección física de la sucursal
        /// </summary>
        [MaxLength(255)]
        public string? Direccion { get; set; }

        /// <summary>
        /// Ciudad donde se ubica la sucursal
        /// </summary>
        [MaxLength(50)]
        public string? Ciudad { get; set; }

        /// <summary>
        /// Estado o provincia de la sucursal
        /// </summary>
        [MaxLength(50)]
        public string? Estado { get; set; }

        /// <summary>
        /// Código postal de la sucursal
        /// </summary>
        [MaxLength(10)]
        public string? CodigoPostal { get; set; }

        /// <summary>
        /// Teléfono de contacto de la sucursal
        /// </summary>
        [MaxLength(20)]
        public string? Telefono { get; set; }

        /// <summary>
        /// Correo electrónico de la sucursal
        /// </summary>
        [MaxLength(100)]
        public string? Email { get; set; }

        /// <summary>
        /// ID del gerente responsable de la sucursal
        /// </summary>
        public int? GerenteId { get; set; }

        /// <summary>
        /// Indica si la sucursal está activa
        /// </summary>
        public bool EstaActivo { get; set; } = true;

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
        public virtual ICollection<Prospecto> Prospectos { get; set; } = new List<Prospecto>();
        public virtual ICollection<Cotizacion> Cotizaciones { get; set; } = new List<Cotizacion>();
    }

    /// <summary>
    /// Representa un usuario del sistema CRM con credenciales y permisos
    /// </summary>
    [Table("Usuarios")]
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de usuario para inicio de sesión
        /// </summary>
        [Required, MaxLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico del usuario
        /// </summary>
        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash de la contraseña del usuario
        /// </summary>
        [Required, MaxLength(255)]
        public string HashContrasena { get; set; } = string.Empty;

        /// <summary>
        /// Nombre(s) del usuario
        /// </summary>
        [Required, MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Apellido(s) del usuario
        /// </summary>
        [Required, MaxLength(50)]
        public string Apellido { get; set; } = string.Empty;

        /// <summary>
        /// ID del rol asignado al usuario
        /// </summary>
        public int RolId { get; set; }

        /// <summary>
        /// ID de la sucursal a la que pertenece el usuario
        /// </summary>
        public int SucursalId { get; set; }

        /// <summary>
        /// Indica si el usuario está activo en el sistema
        /// </summary>
        public bool EstaActivo { get; set; } = true;

        /// <summary>
        /// Fecha y hora del último inicio de sesión
        /// </summary>
        public DateTime? UltimoAcceso { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual RolUsuario Rol { get; set; } = null!;
        public virtual Sucursal Sucursal { get; set; } = null!;
        public virtual ICollection<Cliente> ClientesAsignados { get; set; } = new List<Cliente>();
        public virtual ICollection<Prospecto> ProspectosAsignados { get; set; } = new List<Prospecto>();
        public virtual ICollection<Cotizacion> Cotizaciones { get; set; } = new List<Cotizacion>();
        public virtual ICollection<Visita> Visitas { get; set; } = new List<Visita>();
        public virtual ICollection<Tarea> TareasAsignadas { get; set; } = new List<Tarea>();
        public virtual ICollection<Tarea> TareasCreadas { get; set; } = new List<Tarea>();
        public virtual ICollection<EventoCalendario> Eventos { get; set; } = new List<EventoCalendario>();

        /// <summary>
        /// Nombre completo del usuario (Nombre + Apellido)
        /// </summary>
        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellido}";
    }

    // =============================================
    // Modelos de Gestión de Clientes
    // =============================================

    /// <summary>
    /// Representa una categoría de cliente con descuentos asociados
    /// </summary>
    [Table("CategoriasCliente")]
    public class CategoriaCliente
    {
        /// <summary>
        /// Identificador único de la categoría
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la categoría (ej: Premium, Corporativo, Regular)
        /// </summary>
        [Required, MaxLength(50)]
        public string NombreCategoria { get; set; } = string.Empty;

        /// <summary>
        /// Porcentaje de descuento aplicable a esta categoría
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeDescuento { get; set; } = 0;

        /// <summary>
        /// Descripción de la categoría y sus beneficios
        /// </summary>
        [MaxLength(255)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    }

    /// <summary>
    /// Representa un cliente activo de la empresa
    /// </summary>
    [Table("Clientes")]
    public class Cliente
    {
        /// <summary>
        /// Identificador único del cliente
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código único del cliente en el sistema
        /// </summary>
        [Required, MaxLength(20)]
        public string CodigoCliente { get; set; } = string.Empty;

        /// <summary>
        /// Nombre o razón social de la empresa cliente
        /// </summary>
        [Required, MaxLength(200)]
        public string NombreEmpresa { get; set; } = string.Empty;

        /// <summary>
        /// RFC (Registro Federal de Contribuyentes) del cliente
        /// </summary>
        [MaxLength(13)]
        public string? RFC { get; set; }

        /// <summary>
        /// Industria o sector al que pertenece el cliente
        /// </summary>
        [MaxLength(100)]
        public string? Industria { get; set; }

        /// <summary>
        /// Sitio web corporativo del cliente
        /// </summary>
        [MaxLength(255)]
        public string? SitioWeb { get; set; }

        /// <summary>
        /// Teléfono principal del cliente
        /// </summary>
        [MaxLength(20)]
        public string? Telefono { get; set; }

        /// <summary>
        /// Correo electrónico principal del cliente
        /// </summary>
        [MaxLength(100)]
        public string? Email { get; set; }

        /// <summary>
        /// Dirección física del cliente
        /// </summary>
        [MaxLength(255)]
        public string? Direccion { get; set; }

        /// <summary>
        /// Ciudad del cliente
        /// </summary>
        [MaxLength(50)]
        public string? Ciudad { get; set; }

        /// <summary>
        /// Estado o provincia del cliente
        /// </summary>
        [MaxLength(50)]
        public string? Estado { get; set; }

        /// <summary>
        /// Código postal del cliente
        /// </summary>
        [MaxLength(10)]
        public string? CodigoPostal { get; set; }

        /// <summary>
        /// País del cliente
        /// </summary>
        [MaxLength(50)]
        public string Pais { get; set; } = "México";

        /// <summary>
        /// ID de la categoría del cliente
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// ID de la sucursal que atiende al cliente
        /// </summary>
        public int SucursalId { get; set; }

        /// <summary>
        /// ID del vendedor asignado al cliente
        /// </summary>
        public int? VendedorAsignadoId { get; set; }

        /// <summary>
        /// Estado del cliente (Activo, Inactivo, Suspendido)
        /// </summary>
        [MaxLength(20)]
        public string EstadoCliente { get; set; } = "Activo";

        /// <summary>
        /// Fecha de registro del cliente en el sistema
        /// </summary>
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de la última compra realizada por el cliente
        /// </summary>
        public DateTime? FechaUltimaCompra { get; set; }

        /// <summary>
        /// Valor total de vida del cliente (CLV - Customer Lifetime Value)
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorVidaCliente { get; set; } = 0;

        /// <summary>
        /// Notas adicionales sobre el cliente
        /// </summary>
        public string? Notas { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual CategoriaCliente Categoria { get; set; } = null!;
        public virtual Sucursal Sucursal { get; set; } = null!;
        public virtual Usuario? VendedorAsignado { get; set; }
        public virtual ICollection<ContactoCliente> Contactos { get; set; } = new List<ContactoCliente>();
        public virtual ICollection<Cotizacion> Cotizaciones { get; set; } = new List<Cotizacion>();
        public virtual ICollection<Visita> Visitas { get; set; } = new List<Visita>();
        public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
        public virtual ICollection<EventoCalendario> Eventos { get; set; } = new List<EventoCalendario>();
    }

    /// <summary>
    /// Representa un contacto dentro de una empresa cliente
    /// </summary>
    [Table("ContactosCliente")]
    public class ContactoCliente
    {
        /// <summary>
        /// Identificador único del contacto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID del cliente al que pertenece el contacto
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Nombre(s) del contacto
        /// </summary>
        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Apellido(s) del contacto
        /// </summary>
        [Required, MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        /// <summary>
        /// Cargo o posición del contacto en la empresa
        /// </summary>
        [MaxLength(100)]
        public string? Cargo { get; set; }

        /// <summary>
        /// Correo electrónico del contacto
        /// </summary>
        [MaxLength(100)]
        public string? Email { get; set; }

        /// <summary>
        /// Teléfono directo del contacto
        /// </summary>
        [MaxLength(20)]
        public string? TelefonoDirecto { get; set; }

        /// <summary>
        /// Teléfono móvil del contacto
        /// </summary>
        [MaxLength(20)]
        public string? TelefonoMovil { get; set; }

        /// <summary>
        /// Indica si es el contacto principal del cliente
        /// </summary>
        public bool EsContactoPrincipal { get; set; } = false;

        /// <summary>
        /// Departamento al que pertenece el contacto
        /// </summary>
        [MaxLength(100)]
        public string? Departamento { get; set; }

        /// <summary>
        /// Fecha de cumpleaños del contacto
        /// </summary>
        public DateTime? FechaCumpleanos { get; set; }

        /// <summary>
        /// Notas adicionales sobre el contacto
        /// </summary>
        public string? Notas { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual Cliente Cliente { get; set; } = null!;

        /// <summary>
        /// Nombre completo del contacto
        /// </summary>
        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellido}";
    }

    // =============================================
    // Modelos de Gestión de Prospectos (Leads)
    // =============================================

    /// <summary>
    /// Representa una fuente de origen de prospectos
    /// </summary>
    [Table("FuentesProspecto")]
    public class FuenteProspecto
    {
        /// <summary>
        /// Identificador único de la fuente
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la fuente (ej: Expo Industrial 2024, Campaña Digital Q1)
        /// </summary>
        [Required, MaxLength(50)]
        public string NombreFuente { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada de la fuente
        /// </summary>
        [MaxLength(255)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Tipo de fuente (Expo, Campaña, Referido, Web, Llamada Fría, etc.)
        /// </summary>
        [MaxLength(50)]
        public string? TipoFuente { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual ICollection<Prospecto> Prospectos { get; set; } = new List<Prospecto>();
    }

    /// <summary>
    /// Representa un prospecto o lead potencial de ventas
    /// </summary>
    [Table("Prospectos")]
    public class Prospecto
    {
        /// <summary>
        /// Identificador único del prospecto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código único del prospecto en el sistema
        /// </summary>
        [Required, MaxLength(20)]
        public string CodigoProspecto { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la empresa prospecto
        /// </summary>
        [Required, MaxLength(200)]
        public string NombreEmpresa { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del contacto principal
        /// </summary>
        [Required, MaxLength(100)]
        public string NombreContacto { get; set; } = string.Empty;

        /// <summary>
        /// Apellido del contacto principal
        /// </summary>
        [MaxLength(100)]
        public string? ApellidoContacto { get; set; }

        /// <summary>
        /// Cargo del contacto en la empresa
        /// </summary>
        [MaxLength(100)]
        public string? CargoContacto { get; set; }

        /// <summary>
        /// Correo electrónico del contacto
        /// </summary>
        [MaxLength(100)]
        public string? Email { get; set; }

        /// <summary>
        /// Teléfono del contacto
        /// </summary>
        [MaxLength(20)]
        public string? Telefono { get; set; }

        /// <summary>
        /// Industria o sector del prospecto
        /// </summary>
        [MaxLength(100)]
        public string? Industria { get; set; }

        /// <summary>
        /// Tamaño de la empresa (Pequeña, Mediana, Grande, Corporativo)
        /// </summary>
        [MaxLength(50)]
        public string? TamanoEmpresa { get; set; }

        /// <summary>
        /// Dirección física del prospecto
        /// </summary>
        [MaxLength(255)]
        public string? Direccion { get; set; }

        /// <summary>
        /// Ciudad del prospecto
        /// </summary>
        [MaxLength(50)]
        public string? Ciudad { get; set; }

        /// <summary>
        /// Estado o provincia del prospecto
        /// </summary>
        [MaxLength(50)]
        public string? EstadoDireccion { get; set; }

        /// <summary>
        /// País del prospecto
        /// </summary>
        [MaxLength(50)]
        public string Pais { get; set; } = "México";

        /// <summary>
        /// ID de la fuente de origen del prospecto
        /// </summary>
        public int FuenteId { get; set; }

        /// <summary>
        /// Estado actual del prospecto en el embudo de ventas
        /// </summary>
        [MaxLength(50)]
        public string EstadoProspecto { get; set; } = "Nuevo";

        /// <summary>
        /// Prioridad del prospecto (Alta, Media, Baja)
        /// </summary>
        [MaxLength(20)]
        public string Prioridad { get; set; } = "Media";

        /// <summary>
        /// Valor estimado de la oportunidad de venta
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorEstimado { get; set; }

        /// <summary>
        /// Probabilidad de cierre (0-100%)
        /// </summary>
        public int ProbabilidadCierre { get; set; } = 0;

        /// <summary>
        /// Fecha estimada de cierre de la venta
        /// </summary>
        public DateTime? FechaEstimadaCierre { get; set; }

        /// <summary>
        /// ID del vendedor asignado al prospecto
        /// </summary>
        public int? VendedorAsignadoId { get; set; }

        /// <summary>
        /// ID de la sucursal que atiende al prospecto
        /// </summary>
        public int SucursalId { get; set; }

        /// <summary>
        /// Notas adicionales sobre el prospecto
        /// </summary>
        public string? Notas { get; set; }

        /// <summary>
        /// Motivo de rechazo si el prospecto fue descalificado
        /// </summary>
        [MaxLength(255)]
        public string? MotivoRechazo { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha en que el prospecto se convirtió a cliente
        /// </summary>
        public DateTime? FechaConversion { get; set; }

        // Propiedades de navegación
        public virtual FuenteProspecto Fuente { get; set; } = null!;
        public virtual Usuario? VendedorAsignado { get; set; }
        public virtual Sucursal Sucursal { get; set; } = null!;
        public virtual ICollection<Cotizacion> Cotizaciones { get; set; } = new List<Cotizacion>();
        public virtual ICollection<Visita> Visitas { get; set; } = new List<Visita>();
        public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
        public virtual ICollection<EventoCalendario> Eventos { get; set; } = new List<EventoCalendario>();

        /// <summary>
        /// Nombre completo del contacto del prospecto
        /// </summary>
        [NotMapped]
        public string ContactoCompleto => $"{NombreContacto} {ApellidoContacto}".Trim();
    }

    // =============================================
    // Modelos de Productos y Servicios
    // =============================================

    /// <summary>
    /// Representa una categoría de productos o servicios
    /// </summary>
    [Table("CategoriasProducto")]
    public class CategoriaProducto
    {
        /// <summary>
        /// Identificador único de la categoría
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la categoría
        /// </summary>
        [Required, MaxLength(100)]
        public string NombreCategoria { get; set; } = string.Empty;

        /// <summary>
        /// Descripción de la categoría
        /// </summary>
        [MaxLength(255)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// ID de la categoría padre (para jerarquía)
        /// </summary>
        public int? CategoriaPadreId { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual CategoriaProducto? CategoriaPadre { get; set; }
        public virtual ICollection<CategoriaProducto> SubCategorias { get; set; } = new List<CategoriaProducto>();
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }

    /// <summary>
    /// Representa un producto o servicio ofrecido por la empresa
    /// </summary>
    [Table("Productos")]
    public class Producto
    {
        /// <summary>
        /// Identificador único del producto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código único del producto (SKU)
        /// </summary>
        [Required, MaxLength(50)]
        public string CodigoProducto { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del producto o servicio
        /// </summary>
        [Required, MaxLength(200)]
        public string NombreProducto { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada del producto
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// ID de la categoría del producto
        /// </summary>
        public int CategoriaId { get; set; }

        /// <summary>
        /// Unidad de medida (Pieza, Servicio, Licencia, Hora, etc.)
        /// </summary>
        [MaxLength(20)]
        public string UnidadMedida { get; set; } = "Pieza";

        /// <summary>
        /// Precio de lista del producto
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioLista { get; set; }

        /// <summary>
        /// Precio de costo del producto
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrecioCosto { get; set; }

        /// <summary>
        /// Moneda del precio (MXN, USD, EUR, etc.)
        /// </summary>
        [MaxLength(3)]
        public string Moneda { get; set; } = "MXN";

        /// <summary>
        /// Tiempo de entrega en días
        /// </summary>
        public int TiempoEntrega { get; set; } = 0;

        /// <summary>
        /// Cantidad en stock
        /// </summary>
        public int Stock { get; set; } = 0;

        /// <summary>
        /// Stock mínimo requerido
        /// </summary>
        public int StockMinimo { get; set; } = 0;

        /// <summary>
        /// Indica si el producto está activo
        /// </summary>
        public bool EstaActivo { get; set; } = true;

        /// <summary>
        /// Fabricante del producto
        /// </summary>
        [MaxLength(100)]
        public string? Fabricante { get; set; }

        /// <summary>
        /// Modelo del producto
        /// </summary>
        [MaxLength(100)]
        public string? Modelo { get; set; }

        /// <summary>
        /// Especificaciones técnicas en formato JSON
        /// </summary>
        public string? Especificaciones { get; set; }

        /// <summary>
        /// URL de la imagen del producto
        /// </summary>
        [MaxLength(500)]
        public string? ImagenURL { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual CategoriaProducto Categoria { get; set; } = null!;
        public virtual ICollection<DetalleCotizacion> DetallesCotizacion { get; set; } = new List<DetalleCotizacion>();
    }

    // =============================================
    // Modelos de Cotizaciones
    // =============================================

    /// <summary>
    /// Representa una cotización o propuesta comercial
    /// </summary>
    [Table("Cotizaciones")]
    public class Cotizacion
    {
        /// <summary>
        /// Identificador único de la cotización
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Número único de la cotización
        /// </summary>
        [Required, MaxLength(20)]
        public string NumeroCotizacion { get; set; } = string.Empty;

        /// <summary>
        /// ID del cliente (si aplica)
        /// </summary>
        public int? ClienteId { get; set; }

        /// <summary>
        /// ID del prospecto (si aplica)
        /// </summary>
        public int? ProspectoId { get; set; }

        /// <summary>
        /// Nombre del cliente o prospecto
        /// </summary>
        [Required, MaxLength(200)]
        public string NombreCliente { get; set; } = string.Empty;

        /// <summary>
        /// Email del cliente o prospecto
        /// </summary>
        [MaxLength(100)]
        public string? EmailCliente { get; set; }

        /// <summary>
        /// Teléfono del cliente o prospecto
        /// </summary>
        [MaxLength(20)]
        public string? TelefonoCliente { get; set; }

        /// <summary>
        /// ID del vendedor que genera la cotización
        /// </summary>
        public int VendedorId { get; set; }

        /// <summary>
        /// ID de la sucursal que emite la cotización
        /// </summary>
        public int SucursalId { get; set; }

        /// <summary>
        /// Fecha de emisión de la cotización
        /// </summary>
        public DateTime FechaCotizacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de vencimiento de la cotización
        /// </summary>
        public DateTime FechaVencimiento { get; set; }

        /// <summary>
        /// Estado de la cotización (Borrador, Enviada, Aprobada, Rechazada, Vencida)
        /// </summary>
        [MaxLength(50)]
        public string EstadoCotizacion { get; set; } = "Borrador";

        /// <summary>
        /// Subtotal de la cotización (antes de descuentos e impuestos)
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; } = 0;

        /// <summary>
        /// Porcentaje de descuento aplicado
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeDescuento { get; set; } = 0;

        /// <summary>
        /// Monto del descuento aplicado
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoDescuento { get; set; } = 0;

        /// <summary>
        /// Porcentaje de IVA aplicado
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeIVA { get; set; } = 16.00M;

        /// <summary>
        /// Monto del IVA
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoIVA { get; set; } = 0;

        /// <summary>
        /// Total de la cotización
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; } = 0;

        /// <summary>
        /// Moneda de la cotización
        /// </summary>
        [MaxLength(3)]
        public string Moneda { get; set; } = "MXN";

        /// <summary>
        /// Tipo de cambio aplicado
        /// </summary>
        [Column(TypeName = "decimal(10,4)")]
        public decimal TipoCambio { get; set; } = 1.0000M;

        /// <summary>
        /// Condiciones de pago
        /// </summary>
        [MaxLength(500)]
        public string? CondicionesPago { get; set; }

        /// <summary>
        /// Tiempo de entrega estimado
        /// </summary>
        [MaxLength(100)]
        public string? TiempoEntrega { get; set; }

        /// <summary>
        /// Vigencia de la cotización
        /// </summary>
        [MaxLength(100)]
        public string Vigencia { get; set; } = "30 días";

        /// <summary>
        /// Notas adicionales de la cotización
        /// </summary>
        public string? Notas { get; set; }

        /// <summary>
        /// Términos y condiciones
        /// </summary>
        public string? TerminosCondiciones { get; set; }

        /// <summary>
        /// Ruta del archivo adjunto
        /// </summary>
        [MaxLength(500)]
        public string? ArchivoAdjunto { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de aprobación de la cotización
        /// </summary>
        public DateTime? FechaAprobacion { get; set; }

        // Propiedades de navegación
        public virtual Cliente? Cliente { get; set; }
        public virtual Prospecto? Prospecto { get; set; }
        public virtual Usuario Vendedor { get; set; } = null!;
        public virtual Sucursal Sucursal { get; set; } = null!;
        public virtual ICollection<DetalleCotizacion> Detalles { get; set; } = new List<DetalleCotizacion>();
    }

    /// <summary>
    /// Representa una línea de detalle en una cotización
    /// </summary>
    [Table("DetallesCotizacion")]
    public class DetalleCotizacion
    {
        /// <summary>
        /// Identificador único del detalle
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID de la cotización a la que pertenece
        /// </summary>
        public int CotizacionId { get; set; }

        /// <summary>
        /// ID del producto cotizado
        /// </summary>
        public int ProductoId { get; set; }

        /// <summary>
        /// Descripción del producto en la cotización
        /// </summary>
        [Required, MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad cotizada
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal Cantidad { get; set; }

        /// <summary>
        /// Unidad de medida
        /// </summary>
        [MaxLength(20)]
        public string? UnidadMedida { get; set; }

        /// <summary>
        /// Precio unitario
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        /// <summary>
        /// Porcentaje de descuento aplicado a esta línea
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeDescuento { get; set; } = 0;

        /// <summary>
        /// Monto del descuento de esta línea
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoDescuento { get; set; } = 0;

        /// <summary>
        /// Subtotal de esta línea
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Orden de visualización
        /// </summary>
        public int Orden { get; set; } = 0;

        /// <summary>
        /// Notas adicionales de esta línea
        /// </summary>
        public string? Notas { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual Cotizacion Cotizacion { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
    }

    // =============================================
    // Modelos de Visitas y Actividades
    // =============================================

    /// <summary>
    /// Representa una visita a cliente o prospecto
    /// </summary>
    [Table("Visitas")]
    public class Visita
    {
        /// <summary>
        /// Identificador único de la visita
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID del cliente visitado (si aplica)
        /// </summary>
        public int? ClienteId { get; set; }

        /// <summary>
        /// ID del prospecto visitado (si aplica)
        /// </summary>
        public int? ProspectoId { get; set; }

        /// <summary>
        /// ID del usuario que realiza la visita
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Tipo de visita (Presencial, Virtual, Llamada, Email)
        /// </summary>
        [Required, MaxLength(50)]
        public string TipoVisita { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de la visita
        /// </summary>
        public DateTime FechaVisita { get; set; }

        /// <summary>
        /// Duración de la visita en minutos
        /// </summary>
        public int? Duracion { get; set; }

        /// <summary>
        /// Ubicación de la visita
        /// </summary>
        [MaxLength(255)]
        public string? Ubicacion { get; set; }

        /// <summary>
        /// Asunto o tema de la visita
        /// </summary>
        [Required, MaxLength(255)]
        public string Asunto { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada de la visita
        /// </summary>
        public string? DescripcionVisita { get; set; }

        /// <summary>
        /// Resultado de la visita (Exitosa, Pendiente, Cancelada, Reprogramada)
        /// </summary>
        [MaxLength(50)]
        public string? Resultado { get; set; }

        /// <summary>
        /// Próxima acción a realizar
        /// </summary>
        [MaxLength(500)]
        public string? ProximaAccion { get; set; }

        /// <summary>
        /// Fecha de la próxima acción
        /// </summary>
        public DateTime? FechaProximaAccion { get; set; }

        /// <summary>
        /// Lista de asistentes en formato JSON
        /// </summary>
        public string? Asistentes { get; set; }

        /// <summary>
        /// URLs de documentos adjuntos en formato JSON
        /// </summary>
        public string? DocumentosAdjuntos { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual Cliente? Cliente { get; set; }
        public virtual Prospecto? Prospecto { get; set; }
        public virtual Usuario Usuario { get; set; } = null!;
    }

    /// <summary>
    /// Representa una tarea asignada a un usuario
    /// </summary>
    [Table("Tareas")]
    public class Tarea
    {
        /// <summary>
        /// Identificador único de la tarea
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título de la tarea
        /// </summary>
        [Required, MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada de la tarea
        /// </summary>
        public string? DescripcionTarea { get; set; }

        /// <summary>
        /// Tipo de tarea (Llamada, Email, Reunión, Seguimiento, Otro)
        /// </summary>
        [Required, MaxLength(50)]
        public string TipoTarea { get; set; } = string.Empty;

        /// <summary>
        /// Prioridad de la tarea (Alta, Media, Baja)
        /// </summary>
        [MaxLength(20)]
        public string Prioridad { get; set; } = "Media";

        /// <summary>
        /// Estado de la tarea (Pendiente, En Progreso, Completada, Cancelada)
        /// </summary>
        [MaxLength(50)]
        public string EstadoTarea { get; set; } = "Pendiente";

        /// <summary>
        /// Fecha de vencimiento de la tarea
        /// </summary>
        public DateTime FechaVencimiento { get; set; }

        /// <summary>
        /// Fecha en que se completó la tarea
        /// </summary>
        public DateTime? FechaCompletada { get; set; }

        /// <summary>
        /// ID del usuario al que se asigna la tarea
        /// </summary>
        public int AsignadoA { get; set; }

        /// <summary>
        /// ID del usuario que creó la tarea
        /// </summary>
        public int CreadoPor { get; set; }

        /// <summary>
        /// ID del cliente relacionado (si aplica)
        /// </summary>
        public int? ClienteId { get; set; }

        /// <summary>
        /// ID del prospecto relacionado (si aplica)
        /// </summary>
        public int? ProspectoId { get; set; }

        /// <summary>
        /// ID de la cotización relacionada (si aplica)
        /// </summary>
        public int? CotizacionId { get; set; }

        /// <summary>
        /// Notas adicionales de la tarea
        /// </summary>
        public string? Notas { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual Usuario UsuarioAsignado { get; set; } = null!;
        public virtual Usuario UsuarioCreador { get; set; } = null!;
        public virtual Cliente? Cliente { get; set; }
        public virtual Prospecto? Prospecto { get; set; }
        public virtual Cotizacion? Cotizacion { get; set; }
    }

    /// <summary>
    /// Representa un evento en el calendario
    /// </summary>
    [Table("EventosCalendario")]
    public class EventoCalendario
    {
        /// <summary>
        /// Identificador único del evento
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título del evento
        /// </summary>
        [Required, MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del evento
        /// </summary>
        public string? DescripcionEvento { get; set; }

        /// <summary>
        /// Fecha y hora de inicio del evento
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha y hora de fin del evento
        /// </summary>
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Indica si el evento dura todo el día
        /// </summary>
        public bool TodoElDia { get; set; } = false;

        /// <summary>
        /// Ubicación del evento
        /// </summary>
        [MaxLength(255)]
        public string? Ubicacion { get; set; }

        /// <summary>
        /// Tipo de evento (Reunión, Llamada, Visita, Capacitación, Otro)
        /// </summary>
        [MaxLength(50)]
        public string? TipoEvento { get; set; }

        /// <summary>
        /// ID del usuario propietario del evento
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// ID del cliente relacionado (si aplica)
        /// </summary>
        public int? ClienteId { get; set; }

        /// <summary>
        /// ID del prospecto relacionado (si aplica)
        /// </summary>
        public int? ProspectoId { get; set; }

        /// <summary>
        /// Color del evento en formato hexadecimal
        /// </summary>
        [MaxLength(7)]
        public string Color { get; set; } = "#0d6efd";

        /// <summary>
        /// Minutos antes del evento para recordatorio
        /// </summary>
        public int? Recordatorio { get; set; }

        /// <summary>
        /// Indica si el evento es recurrente
        /// </summary>
        public bool EsRecurrente { get; set; } = false;

        /// <summary>
        /// Patrón de recurrencia en formato JSON
        /// </summary>
        [MaxLength(255)]
        public string? PatronRecurrencia { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// </summary>
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public virtual Usuario Usuario { get; set; } = null!;
        public virtual Cliente? Cliente { get; set; }
        public virtual Prospecto? Prospecto { get; set; }
    }
}

