using Microsoft.EntityFrameworkCore;
using CRMSystem.API.Models;

namespace CRMSystem.API.Data
{
    /// <summary>
    /// Contexto de base de datos para el Sistema CRM
    /// </summary>
    public class ContextoBDCRM : DbContext
    {
        public ContextoBDCRM(DbContextOptions<ContextoBDCRM> options) : base(options)
        {
        }

        // Gestión de Usuarios
        public DbSet<RolUsuario> RolesUsuario { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // Gestión de Clientes
        public DbSet<CategoriaCliente> CategoriasCliente { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ContactoCliente> ContactosCliente { get; set; }

        // Gestión de Prospectos
        public DbSet<FuenteProspecto> FuentesProspecto { get; set; }
        public DbSet<Prospecto> Prospectos { get; set; }

        // Gestión de Productos
        public DbSet<CategoriaProducto> CategoriasProducto { get; set; }
        public DbSet<Producto> Productos { get; set; }

        // Gestión de Cotizaciones
        public DbSet<Cotizacion> Cotizaciones { get; set; }
        public DbSet<DetalleCotizacion> DetallesCotizacion { get; set; }

        // Gestión de Visitas y Actividades
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Tarea> Tareas { get; set; }

        // Calendario y Eventos
        public DbSet<EventoCalendario> EventosCalendario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =============================================
            // Configurar relaciones y restricciones
            // =============================================

            // Relaciones de Usuario
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Sucursal)
                .WithMany(b => b.Usuarios)
                .HasForeignKey(u => u.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de Cliente
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Categoria)
                .WithMany(cc => cc.Clientes)
                .HasForeignKey(c => c.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.VendedorAsignado)
                .WithMany(u => u.ClientesAsignados)
                .HasForeignKey(c => c.VendedorAsignadoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Sucursal)
                .WithMany(b => b.Clientes)
                .HasForeignKey(c => c.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de ContactoCliente
            modelBuilder.Entity<ContactoCliente>()
                .HasOne(cc => cc.Cliente)
                .WithMany(c => c.Contactos)
                .HasForeignKey(cc => cc.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones de Prospecto
            modelBuilder.Entity<Prospecto>()
                .HasOne(l => l.Fuente)
                .WithMany(ls => ls.Prospectos)
                .HasForeignKey(l => l.FuenteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prospecto>()
                .HasOne(l => l.VendedorAsignado)
                .WithMany(u => u.ProspectosAsignados)
                .HasForeignKey(l => l.VendedorAsignadoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Prospecto>()
                .HasOne(l => l.Sucursal)
                .WithMany(b => b.Prospectos)
                .HasForeignKey(l => l.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de CategoriaProducto (auto-referencia para jerarquía)
            modelBuilder.Entity<CategoriaProducto>()
                .HasOne(cp => cp.CategoriaPadre)
                .WithMany(cp => cp.SubCategorias)
                .HasForeignKey(cp => cp.CategoriaPadreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de Producto
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(pc => pc.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de Cotización
            modelBuilder.Entity<Cotizacion>()
                .HasOne(q => q.Cliente)
                .WithMany(c => c.Cotizaciones)
                .HasForeignKey(q => q.ClienteId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Cotizacion>()
                .HasOne(q => q.Prospecto)
                .WithMany(l => l.Cotizaciones)
                .HasForeignKey(q => q.ProspectoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Cotizacion>()
                .HasOne(q => q.Vendedor)
                .WithMany(u => u.Cotizaciones)
                .HasForeignKey(q => q.VendedorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cotizacion>()
                .HasOne(q => q.Sucursal)
                .WithMany(b => b.Cotizaciones)
                .HasForeignKey(q => q.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de DetalleCotizacion
            modelBuilder.Entity<DetalleCotizacion>()
                .HasOne(qi => qi.Cotizacion)
                .WithMany(q => q.Detalles)
                .HasForeignKey(qi => qi.CotizacionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetalleCotizacion>()
                .HasOne(qi => qi.Producto)
                .WithMany(p => p.DetallesCotizacion)
                .HasForeignKey(qi => qi.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de Visita
            modelBuilder.Entity<Visita>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Visitas)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Visita>()
                .HasOne(v => v.Prospecto)
                .WithMany(l => l.Visitas)
                .HasForeignKey(v => v.ProspectoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Visita>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.Visitas)
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de Tarea
            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.UsuarioAsignado)
                .WithMany(u => u.TareasAsignadas)
                .HasForeignKey(t => t.AsignadoA)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.UsuarioCreador)
                .WithMany(u => u.TareasCreadas)
                .HasForeignKey(t => t.CreadoPor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.Cliente)
                .WithMany(c => c.Tareas)
                .HasForeignKey(t => t.ClienteId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.Prospecto)
                .WithMany(l => l.Tareas)
                .HasForeignKey(t => t.ProspectoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.Cotizacion)
                .WithMany()
                .HasForeignKey(t => t.CotizacionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relaciones de EventoCalendario
            modelBuilder.Entity<EventoCalendario>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Eventos)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventoCalendario>()
                .HasOne(e => e.Cliente)
                .WithMany(c => c.Eventos)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<EventoCalendario>()
                .HasOne(e => e.Prospecto)
                .WithMany(l => l.Eventos)
                .HasForeignKey(e => e.ProspectoId)
                .OnDelete(DeleteBehavior.SetNull);

            // =============================================
            // Configurar índices para optimización
            // =============================================

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NombreUsuario)
                .IsUnique();

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.CodigoCliente)
                .IsUnique();

            modelBuilder.Entity<Prospecto>()
                .HasIndex(l => l.CodigoProspecto)
                .IsUnique();

            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.CodigoProducto)
                .IsUnique();

            modelBuilder.Entity<Cotizacion>()
                .HasIndex(q => q.NumeroCotizacion)
                .IsUnique();

            modelBuilder.Entity<Sucursal>()
                .HasIndex(b => b.CodigoSucursal)
                .IsUnique();

            // Índices adicionales para búsquedas frecuentes
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.NombreEmpresa);

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.RFC);

            modelBuilder.Entity<Prospecto>()
                .HasIndex(p => p.EstadoProspecto);

            modelBuilder.Entity<Prospecto>()
                .HasIndex(p => p.FechaCreacion);

            modelBuilder.Entity<Cotizacion>()
                .HasIndex(q => q.EstadoCotizacion);

            modelBuilder.Entity<Cotizacion>()
                .HasIndex(q => q.FechaCotizacion);

            modelBuilder.Entity<Tarea>()
                .HasIndex(t => t.EstadoTarea);

            modelBuilder.Entity<Tarea>()
                .HasIndex(t => t.FechaVencimiento);

            modelBuilder.Entity<EventoCalendario>()
                .HasIndex(e => e.FechaInicio);

            // =============================================
            // Datos iniciales (Seed Data)
            // =============================================
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Roles de Usuario
            modelBuilder.Entity<RolUsuario>().HasData(
                new RolUsuario { Id = 1, NombreRol = "Vendedor", Descripcion = "Representante de ventas", Permisos = "[\"prospectos:leer\", \"prospectos:crear\", \"clientes:leer\", \"tareas:gestionar\"]" },
                new RolUsuario { Id = 2, NombreRol = "Cotizador", Descripcion = "Especialista en cotizaciones", Permisos = "[\"cotizaciones:gestionar\", \"productos:leer\", \"clientes:leer\"]" },
                new RolUsuario { Id = 3, NombreRol = "Gerente", Descripcion = "Gerente de área", Permisos = "[\"prospectos:gestionar\", \"cotizaciones:gestionar\", \"reportes:leer\", \"equipo:gestionar\"]" },
                new RolUsuario { Id = 4, NombreRol = "Director", Descripcion = "Director de operaciones", Permisos = "[\"todos:leer\", \"todos:gestionar\", \"reportes:avanzados\", \"usuarios:gestionar\"]" },
                new RolUsuario { Id = 5, NombreRol = "Sistemas", Descripcion = "Tecnologías de información", Permisos = "[\"sistema:admin\", \"usuarios:gestionar\", \"datos:exportar\"]" },
                new RolUsuario { Id = 6, NombreRol = "Contador", Descripcion = "Contador", Permisos = "[\"financiero:leer\", \"reportes:financiero\", \"cotizaciones:leer\"]" },
                new RolUsuario { Id = 7, NombreRol = "Director de Sucursal", Descripcion = "Director de sucursal", Permisos = "[\"sucursal:gestionar\", \"reportes:sucursal\", \"equipo:gestionar\"]" },
                new RolUsuario { Id = 8, NombreRol = "Consejero", Descripcion = "Consejero estratégico", Permisos = "[\"reportes:leer\", \"analitica:avanzada\", \"estrategia:ver\"]" },
                new RolUsuario { Id = 9, NombreRol = "Dirección General", Descripcion = "Dirección general", Permisos = "[\"todos:admin\", \"sistema:configurar\", \"estrategico:gestionar\"]" }
            );

            // Sucursales
            modelBuilder.Entity<Sucursal>().HasData(
                new Sucursal { Id = 1, CodigoSucursal = "NORTE", NombreSucursal = "Sucursal Norte", Direccion = "Av. Revolución 1234", Ciudad = "Monterrey", Estado = "Nuevo León", Telefono = "+52-81-1234-5678", Email = "norte@crm.com" },
                new Sucursal { Id = 2, CodigoSucursal = "CENTRO", NombreSucursal = "Sucursal Centro", Direccion = "Av. Juárez 5678", Ciudad = "Guadalajara", Estado = "Jalisco", Telefono = "+52-33-2345-6789", Email = "centro@crm.com" },
                new Sucursal { Id = 3, CodigoSucursal = "SUR", NombreSucursal = "Sucursal Sur", Direccion = "Av. Insurgentes 9012", Ciudad = "Mérida", Estado = "Yucatán", Telefono = "+52-99-3456-7890", Email = "sur@crm.com" }
            );

            // Categorías de Cliente
            modelBuilder.Entity<CategoriaCliente>().HasData(
                new CategoriaCliente { Id = 1, NombreCategoria = "Premium", PorcentajeDescuento = 20.00M, Descripcion = "Clientes premium con descuento máximo" },
                new CategoriaCliente { Id = 2, NombreCategoria = "Corporativo", PorcentajeDescuento = 15.00M, Descripcion = "Clientes corporativos con descuento medio" },
                new CategoriaCliente { Id = 3, NombreCategoria = "Regular", PorcentajeDescuento = 10.00M, Descripcion = "Clientes regulares con descuento estándar" },
                new CategoriaCliente { Id = 4, NombreCategoria = "Nuevo", PorcentajeDescuento = 5.00M, Descripcion = "Clientes nuevos con descuento mínimo" }
            );

            // Fuentes de Prospecto
            modelBuilder.Entity<FuenteProspecto>().HasData(
                new FuenteProspecto { Id = 1, NombreFuente = "Expo Industrial 2024", Descripcion = "Exposición industrial anual", TipoFuente = "Expo" },
                new FuenteProspecto { Id = 2, NombreFuente = "Campaña Digital Q1", Descripcion = "Campaña de marketing digital primer trimestre", TipoFuente = "Campaña" },
                new FuenteProspecto { Id = 3, NombreFuente = "Referido Cliente", Descripcion = "Referencia de cliente existente", TipoFuente = "Referido" },
                new FuenteProspecto { Id = 4, NombreFuente = "Sitio Web", Descripcion = "Formulario de contacto en sitio web", TipoFuente = "Web" },
                new FuenteProspecto { Id = 5, NombreFuente = "Llamada Fría", Descripcion = "Prospección telefónica directa", TipoFuente = "Llamada Fría" },
                new FuenteProspecto { Id = 6, NombreFuente = "LinkedIn", Descripcion = "Red social profesional", TipoFuente = "Redes Sociales" },
                new FuenteProspecto { Id = 7, NombreFuente = "Evento Networking", Descripcion = "Eventos de networking empresarial", TipoFuente = "Evento" }
            );

            // Categorías de Producto
            modelBuilder.Entity<CategoriaProducto>().HasData(
                new CategoriaProducto { Id = 1, NombreCategoria = "Hardware", Descripcion = "Equipos y componentes físicos" },
                new CategoriaProducto { Id = 2, NombreCategoria = "Software", Descripcion = "Licencias y aplicaciones de software" },
                new CategoriaProducto { Id = 3, NombreCategoria = "Servicios", Descripcion = "Servicios profesionales y consultoría" },
                new CategoriaProducto { Id = 4, NombreCategoria = "Mantenimiento", Descripcion = "Servicios de mantenimiento y soporte" },
                new CategoriaProducto { Id = 5, NombreCategoria = "Capacitación", Descripcion = "Cursos y capacitación técnica" }
            );

            // Usuarios de ejemplo
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, NombreUsuario = "jperez", Email = "jperez@crm.com", HashContrasena = "hash123", Nombre = "Juan", Apellido = "Pérez", RolId = 1, SucursalId = 1 },
                new Usuario { Id = 2, NombreUsuario = "mgarcia", Email = "mgarcia@crm.com", HashContrasena = "hash456", Nombre = "María", Apellido = "García", RolId = 2, SucursalId = 2 },
                new Usuario { Id = 3, NombreUsuario = "rlopez", Email = "rlopez@crm.com", HashContrasena = "hash789", Nombre = "Roberto", Apellido = "López", RolId = 3, SucursalId = 1 },
                new Usuario { Id = 4, NombreUsuario = "asanchez", Email = "asanchez@crm.com", HashContrasena = "hash101", Nombre = "Ana", Apellido = "Sánchez", RolId = 1, SucursalId = 3 }
            );

            // Productos de ejemplo
            modelBuilder.Entity<Producto>().HasData(
                new Producto { Id = 1, CodigoProducto = "HW-001", NombreProducto = "Servidor Dell PowerEdge R740", Descripcion = "Servidor rack 2U con procesador Intel Xeon", CategoriaId = 1, PrecioLista = 85000.00M, PrecioCosto = 65000.00M },
                new Producto { Id = 2, CodigoProducto = "SW-001", NombreProducto = "Licencia Microsoft Office 365 Business", Descripcion = "Suite de productividad empresarial", CategoriaId = 2, PrecioLista = 2500.00M, PrecioCosto = 1800.00M },
                new Producto { Id = 3, CodigoProducto = "SV-001", NombreProducto = "Consultoría de Infraestructura TI", Descripcion = "Análisis y diseño de infraestructura", CategoriaId = 3, PrecioLista = 15000.00M, PrecioCosto = 8000.00M },
                new Producto { Id = 4, CodigoProducto = "HW-002", NombreProducto = "Laptop HP EliteBook 840 G8", Descripcion = "Laptop empresarial con Intel Core i7", CategoriaId = 1, PrecioLista = 28000.00M, PrecioCosto = 22000.00M },
                new Producto { Id = 5, CodigoProducto = "SW-002", NombreProducto = "Licencia Windows Server 2022 Standard", Descripcion = "Sistema operativo servidor", CategoriaId = 2, PrecioLista = 12000.00M, PrecioCosto = 9000.00M }
            );

            // Prospectos de ejemplo
            modelBuilder.Entity<Prospecto>().HasData(
                new Prospecto { Id = 1, CodigoProspecto = "PROS-2024-001", NombreEmpresa = "Tecnología Avanzada SA", NombreContacto = "Carlos", ApellidoContacto = "Ramírez", Email = "cramirez@tecavanzada.com", Telefono = "+52-81-5555-1234", FuenteId = 1, VendedorAsignadoId = 1, SucursalId = 1, ValorEstimado = 150000.00M },
                new Prospecto { Id = 2, CodigoProspecto = "PROS-2024-002", NombreEmpresa = "Soluciones Empresariales MX", NombreContacto = "Laura", ApellidoContacto = "Martínez", Email = "lmartinez@solempresariales.com", Telefono = "+52-33-5555-5678", FuenteId = 2, VendedorAsignadoId = 2, SucursalId = 2, ValorEstimado = 85000.00M },
                new Prospecto { Id = 3, CodigoProspecto = "PROS-2024-003", NombreEmpresa = "Grupo Industrial del Sureste", NombreContacto = "Fernando", ApellidoContacto = "Hernández", Email = "fhernandez@gis.com", Telefono = "+52-99-5555-9012", FuenteId = 3, VendedorAsignadoId = 4, SucursalId = 3, ValorEstimado = 220000.00M }
            );
        }
    }
}

