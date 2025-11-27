using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRMSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicialPostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreCategoria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasCliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreCategoria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CategoriaPadreId = table.Column<int>(type: "integer", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriasProducto_CategoriasProducto_CategoriaPadreId",
                        column: x => x.CategoriaPadreId,
                        principalTable: "CategoriasProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FuentesProspecto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreFuente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TipoFuente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuentesProspecto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolesUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreRol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Permisos = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sucursales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoSucursal = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    NombreSucursal = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Ciudad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CodigoPostal = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    GerenteId = table.Column<int>(type: "integer", nullable: true),
                    EstaActivo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sucursales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoProducto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NombreProducto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false),
                    UnidadMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PrecioLista = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PrecioCosto = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Moneda = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    TiempoEntrega = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    StockMinimo = table.Column<int>(type: "integer", nullable: false),
                    EstaActivo = table.Column<bool>(type: "boolean", nullable: false),
                    Fabricante = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Modelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Especificaciones = table.Column<string>(type: "text", nullable: true),
                    ImagenURL = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_CategoriasProducto_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriasProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreUsuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HashContrasena = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    SucursalId = table.Column<int>(type: "integer", nullable: false),
                    EstaActivo = table.Column<bool>(type: "boolean", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_RolesUsuario_RolId",
                        column: x => x.RolId,
                        principalTable: "RolesUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuarios_Sucursales_SucursalId",
                        column: x => x.SucursalId,
                        principalTable: "Sucursales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoCliente = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NombreEmpresa = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RFC = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: true),
                    Industria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SitioWeb = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Ciudad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CodigoPostal = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Pais = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false),
                    SucursalId = table.Column<int>(type: "integer", nullable: false),
                    VendedorAsignadoId = table.Column<int>(type: "integer", nullable: true),
                    EstadoCliente = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaUltimaCompra = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValorVidaCliente = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_CategoriasCliente_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriasCliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clientes_Sucursales_SucursalId",
                        column: x => x.SucursalId,
                        principalTable: "Sucursales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clientes_Usuarios_VendedorAsignadoId",
                        column: x => x.VendedorAsignadoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Prospectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoProspecto = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NombreEmpresa = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NombreContacto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ApellidoContacto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CargoContacto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Industria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TamanoEmpresa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Direccion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Ciudad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EstadoDireccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Pais = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FuenteId = table.Column<int>(type: "integer", nullable: false),
                    EstadoProspecto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Prioridad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ValorEstimado = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ProbabilidadCierre = table.Column<int>(type: "integer", nullable: false),
                    FechaEstimadaCierre = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VendedorAsignadoId = table.Column<int>(type: "integer", nullable: true),
                    SucursalId = table.Column<int>(type: "integer", nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    MotivoRechazo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaConversion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prospectos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prospectos_FuentesProspecto_FuenteId",
                        column: x => x.FuenteId,
                        principalTable: "FuentesProspecto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prospectos_Sucursales_SucursalId",
                        column: x => x.SucursalId,
                        principalTable: "Sucursales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prospectos_Usuarios_VendedorAsignadoId",
                        column: x => x.VendedorAsignadoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ContactosCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TelefonoDirecto = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TelefonoMovil = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EsContactoPrincipal = table.Column<bool>(type: "boolean", nullable: false),
                    Departamento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FechaCumpleanos = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactosCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactosCliente_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cotizaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroCotizacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: true),
                    ProspectoId = table.Column<int>(type: "integer", nullable: true),
                    NombreCliente = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EmailCliente = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TelefonoCliente = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    VendedorId = table.Column<int>(type: "integer", nullable: false),
                    SucursalId = table.Column<int>(type: "integer", nullable: false),
                    FechaCotizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstadoCotizacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    MontoDescuento = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PorcentajeIVA = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    MontoIVA = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Moneda = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    TipoCambio = table.Column<decimal>(type: "numeric(10,4)", nullable: false),
                    CondicionesPago = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TiempoEntrega = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Vigencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    TerminosCondiciones = table.Column<string>(type: "text", nullable: true),
                    ArchivoAdjunto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaAprobacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotizaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cotizaciones_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Cotizaciones_Prospectos_ProspectoId",
                        column: x => x.ProspectoId,
                        principalTable: "Prospectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Cotizaciones_Sucursales_SucursalId",
                        column: x => x.SucursalId,
                        principalTable: "Sucursales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cotizaciones_Usuarios_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventosCalendario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DescripcionEvento = table.Column<string>(type: "text", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TodoElDia = table.Column<bool>(type: "boolean", nullable: false),
                    Ubicacion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TipoEvento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: true),
                    ProspectoId = table.Column<int>(type: "integer", nullable: true),
                    Color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    Recordatorio = table.Column<int>(type: "integer", nullable: true),
                    EsRecurrente = table.Column<bool>(type: "boolean", nullable: false),
                    PatronRecurrencia = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosCalendario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventosCalendario_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EventosCalendario_Prospectos_ProspectoId",
                        column: x => x.ProspectoId,
                        principalTable: "Prospectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EventosCalendario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Visitas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: true),
                    ProspectoId = table.Column<int>(type: "integer", nullable: true),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    TipoVisita = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FechaVisita = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Duracion = table.Column<int>(type: "integer", nullable: true),
                    Ubicacion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Asunto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DescripcionVisita = table.Column<string>(type: "text", nullable: true),
                    Resultado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ProximaAccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaProximaAccion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Asistentes = table.Column<string>(type: "text", nullable: true),
                    DocumentosAdjuntos = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visitas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Visitas_Prospectos_ProspectoId",
                        column: x => x.ProspectoId,
                        principalTable: "Prospectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Visitas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesCotizacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CotizacionId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PrecioUnitario = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    MontoDescuento = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesCotizacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesCotizacion_Cotizaciones_CotizacionId",
                        column: x => x.CotizacionId,
                        principalTable: "Cotizaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesCotizacion_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DescripcionTarea = table.Column<string>(type: "text", nullable: true),
                    TipoTarea = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Prioridad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EstadoTarea = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCompletada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AsignadoA = table.Column<int>(type: "integer", nullable: false),
                    CreadoPor = table.Column<int>(type: "integer", nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: true),
                    ProspectoId = table.Column<int>(type: "integer", nullable: true),
                    CotizacionId = table.Column<int>(type: "integer", nullable: true),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tareas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tareas_Cotizaciones_CotizacionId",
                        column: x => x.CotizacionId,
                        principalTable: "Cotizaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tareas_Prospectos_ProspectoId",
                        column: x => x.ProspectoId,
                        principalTable: "Prospectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tareas_Usuarios_AsignadoA",
                        column: x => x.AsignadoA,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tareas_Usuarios_CreadoPor",
                        column: x => x.CreadoPor,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CategoriasCliente",
                columns: new[] { "Id", "Descripcion", "FechaCreacion", "NombreCategoria", "PorcentajeDescuento" },
                values: new object[,]
                {
                    { 1, "Clientes premium con descuento máximo", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5464), "Premium", 20.00m },
                    { 2, "Clientes corporativos con descuento medio", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5476), "Corporativo", 15.00m },
                    { 3, "Clientes regulares con descuento estándar", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5477), "Regular", 10.00m },
                    { 4, "Clientes nuevos con descuento mínimo", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5479), "Nuevo", 5.00m }
                });

            migrationBuilder.InsertData(
                table: "CategoriasProducto",
                columns: new[] { "Id", "CategoriaPadreId", "Descripcion", "FechaCreacion", "NombreCategoria" },
                values: new object[,]
                {
                    { 1, null, "Equipos y componentes físicos", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5597), "Hardware" },
                    { 2, null, "Licencias y aplicaciones de software", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5607), "Software" },
                    { 3, null, "Servicios profesionales y consultoría", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5608), "Servicios" },
                    { 4, null, "Servicios de mantenimiento y soporte", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5610), "Mantenimiento" },
                    { 5, null, "Cursos y capacitación técnica", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5616), "Capacitación" }
                });

            migrationBuilder.InsertData(
                table: "FuentesProspecto",
                columns: new[] { "Id", "Descripcion", "FechaCreacion", "NombreFuente", "TipoFuente" },
                values: new object[,]
                {
                    { 1, "Exposición industrial anual", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5531), "Expo Industrial 2024", "Expo" },
                    { 2, "Campaña de marketing digital primer trimestre", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5539), "Campaña Digital Q1", "Campaña" },
                    { 3, "Referencia de cliente existente", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5540), "Referido Cliente", "Referido" },
                    { 4, "Formulario de contacto en sitio web", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5541), "Sitio Web", "Web" },
                    { 5, "Prospección telefónica directa", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5543), "Llamada Fría", "Llamada Fría" },
                    { 6, "Red social profesional", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5544), "LinkedIn", "Redes Sociales" },
                    { 7, "Eventos de networking empresarial", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5545), "Evento Networking", "Evento" }
                });

            migrationBuilder.InsertData(
                table: "RolesUsuario",
                columns: new[] { "Id", "Descripcion", "FechaActualizacion", "FechaCreacion", "NombreRol", "Permisos" },
                values: new object[,]
                {
                    { 1, "Representante de ventas", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4969), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4967), "Vendedor", "[\"prospectos:leer\", \"prospectos:crear\", \"clientes:leer\", \"tareas:gestionar\"]" },
                    { 2, "Especialista en cotizaciones", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4978), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4977), "Cotizador", "[\"cotizaciones:gestionar\", \"productos:leer\", \"clientes:leer\"]" },
                    { 3, "Gerente de área", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4980), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4979), "Gerente", "[\"prospectos:gestionar\", \"cotizaciones:gestionar\", \"reportes:leer\", \"equipo:gestionar\"]" },
                    { 4, "Director de operaciones", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4981), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4981), "Director", "[\"todos:leer\", \"todos:gestionar\", \"reportes:avanzados\", \"usuarios:gestionar\"]" },
                    { 5, "Tecnologías de información", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4983), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4983), "Sistemas", "[\"sistema:admin\", \"usuarios:gestionar\", \"datos:exportar\"]" },
                    { 6, "Contador", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4985), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4984), "Contador", "[\"financiero:leer\", \"reportes:financiero\", \"cotizaciones:leer\"]" },
                    { 7, "Director de sucursal", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4986), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4986), "Director de Sucursal", "[\"sucursal:gestionar\", \"reportes:sucursal\", \"equipo:gestionar\"]" },
                    { 8, "Consejero estratégico", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4988), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4987), "Consejero", "[\"reportes:leer\", \"analitica:avanzada\", \"estrategia:ver\"]" },
                    { 9, "Dirección general", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4989), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(4989), "Dirección General", "[\"todos:admin\", \"sistema:configurar\", \"estrategico:gestionar\"]" }
                });

            migrationBuilder.InsertData(
                table: "Sucursales",
                columns: new[] { "Id", "Ciudad", "CodigoPostal", "CodigoSucursal", "Direccion", "Email", "EstaActivo", "Estado", "FechaActualizacion", "FechaCreacion", "GerenteId", "NombreSucursal", "Telefono" },
                values: new object[,]
                {
                    { 1, "Monterrey", null, "NORTE", "Av. Revolución 1234", "norte@crm.com", true, "Nuevo León", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5222), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5222), null, "Sucursal Norte", "+52-81-1234-5678" },
                    { 2, "Guadalajara", null, "CENTRO", "Av. Juárez 5678", "centro@crm.com", true, "Jalisco", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5248), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5248), null, "Sucursal Centro", "+52-33-2345-6789" },
                    { 3, "Mérida", null, "SUR", "Av. Insurgentes 9012", "sur@crm.com", true, "Yucatán", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5252), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5252), null, "Sucursal Sur", "+52-99-3456-7890" }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "CategoriaId", "CodigoProducto", "Descripcion", "Especificaciones", "EstaActivo", "Fabricante", "FechaActualizacion", "FechaCreacion", "ImagenURL", "Modelo", "Moneda", "NombreProducto", "PrecioCosto", "PrecioLista", "Stock", "StockMinimo", "TiempoEntrega", "UnidadMedida" },
                values: new object[,]
                {
                    { 1, 1, "HW-001", "Servidor rack 2U con procesador Intel Xeon", null, true, null, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5713), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5712), null, null, "MXN", "Servidor Dell PowerEdge R740", 65000.00m, 85000.00m, 0, 0, 0, "Pieza" },
                    { 2, 2, "SW-001", "Suite de productividad empresarial", null, true, null, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5724), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5723), null, null, "MXN", "Licencia Microsoft Office 365 Business", 1800.00m, 2500.00m, 0, 0, 0, "Pieza" },
                    { 3, 3, "SV-001", "Análisis y diseño de infraestructura", null, true, null, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5727), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5727), null, null, "MXN", "Consultoría de Infraestructura TI", 8000.00m, 15000.00m, 0, 0, 0, "Pieza" },
                    { 4, 1, "HW-002", "Laptop empresarial con Intel Core i7", null, true, null, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5730), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5729), null, null, "MXN", "Laptop HP EliteBook 840 G8", 22000.00m, 28000.00m, 0, 0, 0, "Pieza" },
                    { 5, 2, "SW-002", "Sistema operativo servidor", null, true, null, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5732), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5732), null, null, "MXN", "Licencia Windows Server 2022 Standard", 9000.00m, 12000.00m, 0, 0, 0, "Pieza" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Apellido", "Email", "EstaActivo", "FechaActualizacion", "FechaCreacion", "HashContrasena", "Nombre", "NombreUsuario", "RolId", "SucursalId", "UltimoAcceso" },
                values: new object[,]
                {
                    { 1, "Pérez", "jperez@crm.com", true, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5651), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5650), "hash123", "Juan", "jperez", 1, 1, null },
                    { 2, "García", "mgarcia@crm.com", true, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5674), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5674), "hash456", "María", "mgarcia", 2, 2, null },
                    { 3, "López", "rlopez@crm.com", true, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5678), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5677), "hash789", "Roberto", "rlopez", 3, 1, null },
                    { 4, "Sánchez", "asanchez@crm.com", true, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5682), new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5681), "hash101", "Ana", "asanchez", 1, 3, null }
                });

            migrationBuilder.InsertData(
                table: "Prospectos",
                columns: new[] { "Id", "ApellidoContacto", "CargoContacto", "Ciudad", "CodigoProspecto", "Direccion", "Email", "EstadoDireccion", "EstadoProspecto", "FechaActualizacion", "FechaConversion", "FechaCreacion", "FechaEstimadaCierre", "FuenteId", "Industria", "MotivoRechazo", "NombreContacto", "NombreEmpresa", "Notas", "Pais", "Prioridad", "ProbabilidadCierre", "SucursalId", "TamanoEmpresa", "Telefono", "ValorEstimado", "VendedorAsignadoId" },
                values: new object[,]
                {
                    { 1, "Ramírez", null, null, "PROS-2024-001", null, "cramirez@tecavanzada.com", null, "Nuevo", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5765), null, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5764), null, 1, null, null, "Carlos", "Tecnología Avanzada SA", null, "México", "Media", 0, 1, null, "+52-81-5555-1234", 150000.00m, 1 },
                    { 2, "Martínez", null, null, "PROS-2024-002", null, "lmartinez@solempresariales.com", null, "Nuevo", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5784), null, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5783), null, 2, null, null, "Laura", "Soluciones Empresariales MX", null, "México", "Media", 0, 2, null, "+52-33-5555-5678", 85000.00m, 2 },
                    { 3, "Hernández", null, null, "PROS-2024-003", null, "fhernandez@gis.com", null, "Nuevo", new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5788), null, new DateTime(2025, 11, 27, 20, 10, 35, 206, DateTimeKind.Utc).AddTicks(5788), null, 3, null, null, "Fernando", "Grupo Industrial del Sureste", null, "México", "Media", 0, 3, null, "+52-99-5555-9012", 220000.00m, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasProducto_CategoriaPadreId",
                table: "CategoriasProducto",
                column: "CategoriaPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CategoriaId",
                table: "Clientes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CodigoCliente",
                table: "Clientes",
                column: "CodigoCliente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_NombreEmpresa",
                table: "Clientes",
                column: "NombreEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_RFC",
                table: "Clientes",
                column: "RFC");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_SucursalId",
                table: "Clientes",
                column: "SucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_VendedorAsignadoId",
                table: "Clientes",
                column: "VendedorAsignadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactosCliente_ClienteId",
                table: "ContactosCliente",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_ClienteId",
                table: "Cotizaciones",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_EstadoCotizacion",
                table: "Cotizaciones",
                column: "EstadoCotizacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_FechaCotizacion",
                table: "Cotizaciones",
                column: "FechaCotizacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_NumeroCotizacion",
                table: "Cotizaciones",
                column: "NumeroCotizacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_ProspectoId",
                table: "Cotizaciones",
                column: "ProspectoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_SucursalId",
                table: "Cotizaciones",
                column: "SucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizaciones_VendedorId",
                table: "Cotizaciones",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCotizacion_CotizacionId",
                table: "DetallesCotizacion",
                column: "CotizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCotizacion_ProductoId",
                table: "DetallesCotizacion",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_EventosCalendario_ClienteId",
                table: "EventosCalendario",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_EventosCalendario_FechaInicio",
                table: "EventosCalendario",
                column: "FechaInicio");

            migrationBuilder.CreateIndex(
                name: "IX_EventosCalendario_ProspectoId",
                table: "EventosCalendario",
                column: "ProspectoId");

            migrationBuilder.CreateIndex(
                name: "IX_EventosCalendario_UsuarioId",
                table: "EventosCalendario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaId",
                table: "Productos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CodigoProducto",
                table: "Productos",
                column: "CodigoProducto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prospectos_CodigoProspecto",
                table: "Prospectos",
                column: "CodigoProspecto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prospectos_EstadoProspecto",
                table: "Prospectos",
                column: "EstadoProspecto");

            migrationBuilder.CreateIndex(
                name: "IX_Prospectos_FechaCreacion",
                table: "Prospectos",
                column: "FechaCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Prospectos_FuenteId",
                table: "Prospectos",
                column: "FuenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Prospectos_SucursalId",
                table: "Prospectos",
                column: "SucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_Prospectos_VendedorAsignadoId",
                table: "Prospectos",
                column: "VendedorAsignadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sucursales_CodigoSucursal",
                table: "Sucursales",
                column: "CodigoSucursal",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_AsignadoA",
                table: "Tareas",
                column: "AsignadoA");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ClienteId",
                table: "Tareas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_CotizacionId",
                table: "Tareas",
                column: "CotizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_CreadoPor",
                table: "Tareas",
                column: "CreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_EstadoTarea",
                table: "Tareas",
                column: "EstadoTarea");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_FechaVencimiento",
                table: "Tareas",
                column: "FechaVencimiento");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ProspectoId",
                table: "Tareas",
                column: "ProspectoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_SucursalId",
                table: "Usuarios",
                column: "SucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitas_ClienteId",
                table: "Visitas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitas_ProspectoId",
                table: "Visitas",
                column: "ProspectoId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitas_UsuarioId",
                table: "Visitas",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactosCliente");

            migrationBuilder.DropTable(
                name: "DetallesCotizacion");

            migrationBuilder.DropTable(
                name: "EventosCalendario");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "Visitas");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Cotizaciones");

            migrationBuilder.DropTable(
                name: "CategoriasProducto");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Prospectos");

            migrationBuilder.DropTable(
                name: "CategoriasCliente");

            migrationBuilder.DropTable(
                name: "FuentesProspecto");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "RolesUsuario");

            migrationBuilder.DropTable(
                name: "Sucursales");
        }
    }
}
