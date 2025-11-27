-- =============================================
-- Sistema CRM - Esquema de Base de Datos SQL Server
-- Esquema completo traducido al español
-- =============================================

-- Crear Base de Datos
CREATE DATABASE SistemaCRM;
GO

USE SistemaCRM;
GO

-- =============================================
-- Tablas de Gestión de Usuarios
-- =============================================

-- Tabla de Roles de Usuario
CREATE TABLE RolesUsuario (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255),
    Permisos NVARCHAR(MAX), -- Cadena JSON de permisos
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE()
);

-- Insertar roles predeterminados
INSERT INTO RolesUsuario (NombreRol, Descripcion, Permisos) VALUES
('Vendedor', 'Representante de ventas', '["prospectos:leer", "prospectos:crear", "clientes:leer", "tareas:gestionar"]'),
('Cotizador', 'Especialista en cotizaciones', '["cotizaciones:gestionar", "productos:leer", "clientes:leer"]'),
('Gerente', 'Gerente de área', '["prospectos:gestionar", "cotizaciones:gestionar", "reportes:leer", "equipo:gestionar"]'),
('Director', 'Director de operaciones', '["todos:leer", "todos:gestionar", "reportes:avanzados", "usuarios:gestionar"]'),
('Sistemas', 'Tecnologías de información', '["sistema:admin", "usuarios:gestionar", "datos:exportar"]'),
('Contador', 'Contador', '["financiero:leer", "reportes:financiero", "cotizaciones:leer"]'),
('Director de Sucursal', 'Director de sucursal', '["sucursal:gestionar", "reportes:sucursal", "equipo:gestionar"]'),
('Consejero', 'Consejero estratégico', '["reportes:leer", "analitica:avanzada", "estrategia:ver"]'),
('Dirección General', 'Dirección general', '["todos:admin", "sistema:configurar", "estrategico:gestionar"]');

-- Tabla de Sucursales
CREATE TABLE Sucursales (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CodigoSucursal NVARCHAR(10) NOT NULL UNIQUE,
    NombreSucursal NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(255),
    Ciudad NVARCHAR(50),
    Estado NVARCHAR(50),
    CodigoPostal NVARCHAR(10),
    Telefono NVARCHAR(20),
    Email NVARCHAR(100),
    GerenteId INT NULL,
    EstaActivo BIT DEFAULT 1,
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE()
);

-- Insertar sucursales predeterminadas
INSERT INTO Sucursales (CodigoSucursal, NombreSucursal, Direccion, Ciudad, Estado, Telefono, Email) VALUES
('NORTE', 'Sucursal Norte', 'Av. Revolución 1234', 'Monterrey', 'Nuevo León', '+52-81-1234-5678', 'norte@crm.com'),
('CENTRO', 'Sucursal Centro', 'Av. Juárez 5678', 'Guadalajara', 'Jalisco', '+52-33-2345-6789', 'centro@crm.com'),
('SUR', 'Sucursal Sur', 'Av. Insurgentes 9012', 'Mérida', 'Yucatán', '+52-99-3456-7890', 'sur@crm.com');

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    HashContrasena NVARCHAR(255) NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    RolId INT NOT NULL,
    SucursalId INT NOT NULL,
    EstaActivo BIT DEFAULT 1,
    UltimoAcceso DATETIME2 NULL,
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (RolId) REFERENCES RolesUsuario(Id),
    FOREIGN KEY (SucursalId) REFERENCES Sucursales(Id)
);

-- =============================================
-- Tablas de Gestión de Clientes
-- =============================================

-- Tabla de Categorías de Cliente
CREATE TABLE CategoriasCliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreCategoria NVARCHAR(50) NOT NULL UNIQUE,
    PorcentajeDescuento DECIMAL(5,2) DEFAULT 0,
    Descripcion NVARCHAR(255),
    FechaCreacion DATETIME2 DEFAULT GETDATE()
);

-- Insertar categorías predeterminadas
INSERT INTO CategoriasCliente (NombreCategoria, PorcentajeDescuento, Descripcion) VALUES
('Premium', 20.00, 'Clientes premium con descuento máximo'),
('Corporativo', 15.00, 'Clientes corporativos con descuento medio'),
('Regular', 10.00, 'Clientes regulares con descuento estándar'),
('Nuevo', 5.00, 'Clientes nuevos con descuento mínimo');

-- Tabla de Clientes
CREATE TABLE Clientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CodigoCliente NVARCHAR(20) NOT NULL UNIQUE,
    NombreEmpresa NVARCHAR(200) NOT NULL,
    RFC NVARCHAR(13),
    Industria NVARCHAR(100),
    Sitio Web NVARCHAR(255),
    Telefono NVARCHAR(20),
    Email NVARCHAR(100),
    Direccion NVARCHAR(255),
    Ciudad NVARCHAR(50),
    Estado NVARCHAR(50),
    CodigoPostal NVARCHAR(10),
    Pais NVARCHAR(50) DEFAULT 'México',
    CategoriaId INT NOT NULL,
    SucursalId INT NOT NULL,
    VendedorAsignadoId INT NULL,
    Estado NVARCHAR(20) DEFAULT 'Activo', -- Activo, Inactivo, Suspendido
    FechaRegistro DATETIME2 DEFAULT GETDATE(),
    FechaUltimaCompra DATETIME2 NULL,
    ValorVidaCliente DECIMAL(18,2) DEFAULT 0,
    Notas NVARCHAR(MAX),
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CategoriaId) REFERENCES CategoriasCliente(Id),
    FOREIGN KEY (SucursalId) REFERENCES Sucursales(Id),
    FOREIGN KEY (VendedorAsignadoId) REFERENCES Usuarios(Id)
);

-- Tabla de Contactos de Cliente
CREATE TABLE ContactosCliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Cargo NVARCHAR(100),
    Email NVARCHAR(100),
    TelefonoDirecto NVARCHAR(20),
    TelefonoMovil NVARCHAR(20),
    EsContactoPrincipal BIT DEFAULT 0,
    Departamento NVARCHAR(100),
    FechaCumpleanos DATE NULL,
    Notas NVARCHAR(MAX),
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id) ON DELETE CASCADE
);

-- =============================================
-- Tablas de Gestión de Prospectos (Leads)
-- =============================================

-- Tabla de Fuentes de Prospecto
CREATE TABLE FuentesProspecto (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreFuente NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255),
    TipoFuente NVARCHAR(50), -- Expo, Campaña, Referido, Web, Llamada Fría, etc.
    FechaCreacion DATETIME2 DEFAULT GETDATE()
);

-- Insertar fuentes predeterminadas
INSERT INTO FuentesProspecto (NombreFuente, Descripcion, TipoFuente) VALUES
('Expo Industrial 2024', 'Exposición industrial anual', 'Expo'),
('Campaña Digital Q1', 'Campaña de marketing digital primer trimestre', 'Campaña'),
('Referido Cliente', 'Referencia de cliente existente', 'Referido'),
('Sitio Web', 'Formulario de contacto en sitio web', 'Web'),
('Llamada Fría', 'Prospección telefónica directa', 'Llamada Fría'),
('LinkedIn', 'Red social profesional', 'Redes Sociales'),
('Evento Networking', 'Eventos de networking empresarial', 'Evento');

-- Tabla de Prospectos (Leads)
CREATE TABLE Prospectos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CodigoProspecto NVARCHAR(20) NOT NULL UNIQUE,
    NombreEmpresa NVARCHAR(200) NOT NULL,
    NombreContacto NVARCHAR(100) NOT NULL,
    ApellidoContacto NVARCHAR(100),
    CargoContacto NVARCHAR(100),
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),
    Industria NVARCHAR(100),
    TamanoEmpresa NVARCHAR(50), -- Pequeña, Mediana, Grande, Corporativo
    Direccion NVARCHAR(255),
    Ciudad NVARCHAR(50),
    Estado NVARCHAR(50),
    Pais NVARCHAR(50) DEFAULT 'México',
    FuenteId INT NOT NULL,
    EstadoProspecto NVARCHAR(50) DEFAULT 'Nuevo', -- Nuevo, Contactado, Calificado, Propuesta, Negociación, Ganado, Perdido
    Prioridad NVARCHAR(20) DEFAULT 'Media', -- Alta, Media, Baja
    ValorEstimado DECIMAL(18,2) NULL,
    ProbabilidadCierre INT DEFAULT 0, -- 0-100%
    FechaEstimadaCierre DATE NULL,
    VendedorAsignadoId INT NULL,
    SucursalId INT NOT NULL,
    Notas NVARCHAR(MAX),
    MotivoRechazo NVARCHAR(255) NULL,
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    FechaConversion DATETIME2 NULL, -- Fecha en que se convirtió a cliente
    FOREIGN KEY (FuenteId) REFERENCES FuentesProspecto(Id),
    FOREIGN KEY (VendedorAsignadoId) REFERENCES Usuarios(Id),
    FOREIGN KEY (SucursalId) REFERENCES Sucursales(Id)
);

-- Tabla de Historial de Prospectos
CREATE TABLE HistorialProspectos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProspectoId INT NOT NULL,
    EstadoAnterior NVARCHAR(50),
    EstadoNuevo NVARCHAR(50) NOT NULL,
    UsuarioId INT NOT NULL,
    Comentario NVARCHAR(MAX),
    FechaCambio DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ProspectoId) REFERENCES Prospectos(Id) ON DELETE CASCADE,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

-- =============================================
-- Tablas de Productos y Servicios
-- =============================================

-- Tabla de Categorías de Producto
CREATE TABLE CategoriasProducto (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreCategoria NVARCHAR(100) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255),
    CategoriaPadreId INT NULL,
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CategoriaPadreId) REFERENCES CategoriasProducto(Id)
);

-- Insertar categorías predeterminadas
INSERT INTO CategoriasProducto (NombreCategoria, Descripcion) VALUES
('Hardware', 'Equipos y componentes físicos'),
('Software', 'Licencias y aplicaciones de software'),
('Servicios', 'Servicios profesionales y consultoría'),
('Mantenimiento', 'Servicios de mantenimiento y soporte'),
('Capacitación', 'Cursos y capacitación técnica');

-- Tabla de Productos
CREATE TABLE Productos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CodigoProducto NVARCHAR(50) NOT NULL UNIQUE,
    NombreProducto NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    CategoriaId INT NOT NULL,
    UnidadMedida NVARCHAR(20) DEFAULT 'Pieza', -- Pieza, Servicio, Licencia, Hora, etc.
    PrecioLista DECIMAL(18,2) NOT NULL,
    PrecioCosto DECIMAL(18,2) NULL,
    Moneda NVARCHAR(3) DEFAULT 'MXN',
    TiempoEntrega INT DEFAULT 0, -- Días
    Stock INT DEFAULT 0,
    StockMinimo INT DEFAULT 0,
    EstaActivo BIT DEFAULT 1,
    Fabricante NVARCHAR(100),
    Modelo NVARCHAR(100),
    Especificaciones NVARCHAR(MAX), -- JSON con especificaciones técnicas
    ImagenURL NVARCHAR(500),
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CategoriaId) REFERENCES CategoriasProducto(Id)
);

-- Tabla de Historial de Precios
CREATE TABLE HistorialPrecios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductoId INT NOT NULL,
    PrecioAnterior DECIMAL(18,2) NOT NULL,
    PrecioNuevo DECIMAL(18,2) NOT NULL,
    UsuarioId INT NOT NULL,
    Motivo NVARCHAR(255),
    FechaCambio DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ProductoId) REFERENCES Productos(Id) ON DELETE CASCADE,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

-- =============================================
-- Tablas de Cotizaciones
-- =============================================

-- Tabla de Cotizaciones
CREATE TABLE Cotizaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NumeroCotizacion NVARCHAR(20) NOT NULL UNIQUE,
    ClienteId INT NULL,
    ProspectoId INT NULL,
    NombreCliente NVARCHAR(200) NOT NULL,
    EmailCliente NVARCHAR(100),
    TelefonoCliente NVARCHAR(20),
    VendedorId INT NOT NULL,
    SucursalId INT NOT NULL,
    FechaCotizacion DATE DEFAULT CAST(GETDATE() AS DATE),
    FechaVencimiento DATE NOT NULL,
    EstadoCotizacion NVARCHAR(50) DEFAULT 'Borrador', -- Borrador, Enviada, Aprobada, Rechazada, Vencida
    Subtotal DECIMAL(18,2) DEFAULT 0,
    PorcentajeDescuento DECIMAL(5,2) DEFAULT 0,
    MontoDescuento DECIMAL(18,2) DEFAULT 0,
    PorcentajeIVA DECIMAL(5,2) DEFAULT 16.00,
    MontoIVA DECIMAL(18,2) DEFAULT 0,
    Total DECIMAL(18,2) DEFAULT 0,
    Moneda NVARCHAR(3) DEFAULT 'MXN',
    TipoCambio DECIMAL(10,4) DEFAULT 1.0000,
    CondicionesPago NVARCHAR(500),
    TiempoEntrega NVARCHAR(100),
    Vigencia NVARCHAR(100) DEFAULT '30 días',
    Notas NVARCHAR(MAX),
    TerminosCondiciones NVARCHAR(MAX),
    ArchivoAdjunto NVARCHAR(500),
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    FechaAprobacion DATETIME2 NULL,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY (ProspectoId) REFERENCES Prospectos(Id),
    FOREIGN KEY (VendedorId) REFERENCES Usuarios(Id),
    FOREIGN KEY (SucursalId) REFERENCES Sucursales(Id),
    CHECK (ClienteId IS NOT NULL OR ProspectoId IS NOT NULL)
);

-- Tabla de Detalle de Cotización
CREATE TABLE DetallesCotizacion (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CotizacionId INT NOT NULL,
    ProductoId INT NOT NULL,
    Descripcion NVARCHAR(500) NOT NULL,
    Cantidad DECIMAL(10,2) NOT NULL,
    UnidadMedida NVARCHAR(20),
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    PorcentajeDescuento DECIMAL(5,2) DEFAULT 0,
    MontoDescuento DECIMAL(18,2) DEFAULT 0,
    Subtotal DECIMAL(18,2) NOT NULL,
    Orden INT DEFAULT 0,
    Notas NVARCHAR(MAX),
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CotizacionId) REFERENCES Cotizaciones(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductoId) REFERENCES Productos(Id)
);

-- Tabla de Historial de Cotizaciones
CREATE TABLE HistorialCotizaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CotizacionId INT NOT NULL,
    EstadoAnterior NVARCHAR(50),
    EstadoNuevo NVARCHAR(50) NOT NULL,
    UsuarioId INT NOT NULL,
    Comentario NVARCHAR(MAX),
    FechaCambio DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CotizacionId) REFERENCES Cotizaciones(Id) ON DELETE CASCADE,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

-- =============================================
-- Tablas de Visitas y Actividades
-- =============================================

-- Tabla de Visitas
CREATE TABLE Visitas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NULL,
    ProspectoId INT NULL,
    UsuarioId INT NOT NULL,
    TipoVisita NVARCHAR(50) NOT NULL, -- Presencial, Virtual, Llamada, Email
    FechaVisita DATETIME2 NOT NULL,
    Duracion INT NULL, -- Minutos
    Ubicacion NVARCHAR(255),
    Asunto NVARCHAR(255) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Resultado NVARCHAR(50), -- Exitosa, Pendiente, Cancelada, Reprogramada
    ProximaAccion NVARCHAR(500),
    FechaProximaAccion DATETIME2 NULL,
    Asistentes NVARCHAR(MAX), -- JSON con lista de asistentes
    DocumentosAdjuntos NVARCHAR(MAX), -- JSON con URLs de documentos
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY (ProspectoId) REFERENCES Prospectos(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    CHECK (ClienteId IS NOT NULL OR ProspectoId IS NOT NULL)
);

-- Tabla de Tareas
CREATE TABLE Tareas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    TipoTarea NVARCHAR(50) NOT NULL, -- Llamada, Email, Reunión, Seguimiento, Otro
    Prioridad NVARCHAR(20) DEFAULT 'Media', -- Alta, Media, Baja
    Estado NVARCHAR(50) DEFAULT 'Pendiente', -- Pendiente, En Progreso, Completada, Cancelada
    FechaVencimiento DATETIME2 NOT NULL,
    FechaCompletada DATETIME2 NULL,
    AsignadoA INT NOT NULL,
    CreadoPor INT NOT NULL,
    ClienteId INT NULL,
    ProspectoId INT NULL,
    CotizacionId INT NULL,
    Notas NVARCHAR(MAX),
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (AsignadoA) REFERENCES Usuarios(Id),
    FOREIGN KEY (CreadoPor) REFERENCES Usuarios(Id),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY (ProspectoId) REFERENCES Prospectos(Id),
    FOREIGN KEY (CotizacionId) REFERENCES Cotizaciones(Id)
);

-- Tabla de Eventos de Calendario
CREATE TABLE EventosCalendario (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    FechaInicio DATETIME2 NOT NULL,
    FechaFin DATETIME2 NOT NULL,
    TodoElDia BIT DEFAULT 0,
    Ubicacion NVARCHAR(255),
    TipoEvento NVARCHAR(50), -- Reunión, Llamada, Visita, Capacitación, Otro
    UsuarioId INT NOT NULL,
    ClienteId INT NULL,
    ProspectoId INT NULL,
    Color NVARCHAR(7) DEFAULT '#0d6efd', -- Color hex para el calendario
    Recordatorio INT NULL, -- Minutos antes del evento
    EsRecurrente BIT DEFAULT 0,
    PatronRecurrencia NVARCHAR(255), -- JSON con patrón de recurrencia
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY (ProspectoId) REFERENCES Prospectos(Id)
);

-- =============================================
-- Tablas de Documentos y Archivos
-- =============================================

-- Tabla de Documentos
CREATE TABLE Documentos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreArchivo NVARCHAR(255) NOT NULL,
    TipoDocumento NVARCHAR(50) NOT NULL, -- Cotización, Contrato, Propuesta, Presentación, Otro
    RutaArchivo NVARCHAR(500) NOT NULL,
    TamanoArchivo BIGINT, -- Bytes
    TipoMIME NVARCHAR(100),
    ClienteId INT NULL,
    ProspectoId INT NULL,
    CotizacionId INT NULL,
    VisitaId INT NULL,
    SubidoPor INT NOT NULL,
    Descripcion NVARCHAR(500),
    EsPublico BIT DEFAULT 0,
    FechaSubida DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY (ProspectoId) REFERENCES Prospectos(Id),
    FOREIGN KEY (CotizacionId) REFERENCES Cotizaciones(Id),
    FOREIGN KEY (VisitaId) REFERENCES Visitas(Id),
    FOREIGN KEY (SubidoPor) REFERENCES Usuarios(Id)
);

-- =============================================
-- Tablas de Análisis y Reportes
-- =============================================

-- Tabla de Métricas de Ventas
CREATE TABLE MetricasVentas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Periodo DATE NOT NULL, -- Primer día del período (mes/trimestre/año)
    TipoPeriodo NVARCHAR(20) NOT NULL, -- Diario, Semanal, Mensual, Trimestral, Anual
    SucursalId INT NOT NULL,
    VendedorId INT NULL,
    NumeroProspectos INT DEFAULT 0,
    NumeroClientes INT DEFAULT 0,
    NumeroCotizaciones INT DEFAULT 0,
    CotizacionesAprobadas INT DEFAULT 0,
    MontoTotalCotizado DECIMAL(18,2) DEFAULT 0,
    MontoTotalVendido DECIMAL(18,2) DEFAULT 0,
    TasaConversion DECIMAL(5,2) DEFAULT 0, -- Porcentaje
    TasaCierre DECIMAL(5,2) DEFAULT 0, -- Porcentaje
    PromedioTicket DECIMAL(18,2) DEFAULT 0,
    FechaCalculo DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (SucursalId) REFERENCES Sucursales(Id),
    FOREIGN KEY (VendedorId) REFERENCES Usuarios(Id)
);

-- Tabla de Organigrama de Clientes
CREATE TABLE OrganigramaClientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    ContactoId INT NOT NULL,
    SupervisorId INT NULL, -- Referencia a otro ContactoId
    NivelJerarquico INT DEFAULT 1,
    Departamento NVARCHAR(100),
    InfluenciaCompra NVARCHAR(50), -- Alta, Media, Baja
    RolDecision NVARCHAR(50), -- Decisor, Influenciador, Usuario, Bloqueador
    Notas NVARCHAR(MAX),
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id) ON DELETE CASCADE,
    FOREIGN KEY (ContactoId) REFERENCES ContactosCliente(Id),
    FOREIGN KEY (SupervisorId) REFERENCES ContactosCliente(Id)
);

-- =============================================
-- Índices para Optimización de Consultas
-- =============================================

-- Índices en Usuarios
CREATE INDEX IX_Usuarios_RolId ON Usuarios(RolId);
CREATE INDEX IX_Usuarios_SucursalId ON Usuarios(SucursalId);
CREATE INDEX IX_Usuarios_Email ON Usuarios(Email);

-- Índices en Clientes
CREATE INDEX IX_Clientes_CodigoCliente ON Clientes(CodigoCliente);
CREATE INDEX IX_Clientes_CategoriaId ON Clientes(CategoriaId);
CREATE INDEX IX_Clientes_SucursalId ON Clientes(SucursalId);
CREATE INDEX IX_Clientes_VendedorAsignadoId ON Clientes(VendedorAsignadoId);
CREATE INDEX IX_Clientes_Estado ON Clientes(Estado);

-- Índices en Prospectos
CREATE INDEX IX_Prospectos_CodigoProspecto ON Prospectos(CodigoProspecto);
CREATE INDEX IX_Prospectos_FuenteId ON Prospectos(FuenteId);
CREATE INDEX IX_Prospectos_EstadoProspecto ON Prospectos(EstadoProspecto);
CREATE INDEX IX_Prospectos_VendedorAsignadoId ON Prospectos(VendedorAsignadoId);
CREATE INDEX IX_Prospectos_SucursalId ON Prospectos(SucursalId);
CREATE INDEX IX_Prospectos_FechaCreacion ON Prospectos(FechaCreacion);

-- Índices en Productos
CREATE INDEX IX_Productos_CodigoProducto ON Productos(CodigoProducto);
CREATE INDEX IX_Productos_CategoriaId ON Productos(CategoriaId);
CREATE INDEX IX_Productos_EstaActivo ON Productos(EstaActivo);

-- Índices en Cotizaciones
CREATE INDEX IX_Cotizaciones_NumeroCotizacion ON Cotizaciones(NumeroCotizacion);
CREATE INDEX IX_Cotizaciones_ClienteId ON Cotizaciones(ClienteId);
CREATE INDEX IX_Cotizaciones_ProspectoId ON Cotizaciones(ProspectoId);
CREATE INDEX IX_Cotizaciones_VendedorId ON Cotizaciones(VendedorId);
CREATE INDEX IX_Cotizaciones_SucursalId ON Cotizaciones(SucursalId);
CREATE INDEX IX_Cotizaciones_EstadoCotizacion ON Cotizaciones(EstadoCotizacion);
CREATE INDEX IX_Cotizaciones_FechaCotizacion ON Cotizaciones(FechaCotizacion);

-- Índices en Visitas
CREATE INDEX IX_Visitas_ClienteId ON Visitas(ClienteId);
CREATE INDEX IX_Visitas_ProspectoId ON Visitas(ProspectoId);
CREATE INDEX IX_Visitas_UsuarioId ON Visitas(UsuarioId);
CREATE INDEX IX_Visitas_FechaVisita ON Visitas(FechaVisita);

-- Índices en Tareas
CREATE INDEX IX_Tareas_AsignadoA ON Tareas(AsignadoA);
CREATE INDEX IX_Tareas_Estado ON Tareas(Estado);
CREATE INDEX IX_Tareas_FechaVencimiento ON Tareas(FechaVencimiento);
CREATE INDEX IX_Tareas_ClienteId ON Tareas(ClienteId);
CREATE INDEX IX_Tareas_ProspectoId ON Tareas(ProspectoId);

-- Índices en Eventos de Calendario
CREATE INDEX IX_EventosCalendario_UsuarioId ON EventosCalendario(UsuarioId);
CREATE INDEX IX_EventosCalendario_FechaInicio ON EventosCalendario(FechaInicio);
CREATE INDEX IX_EventosCalendario_ClienteId ON EventosCalendario(ClienteId);
CREATE INDEX IX_EventosCalendario_ProspectoId ON EventosCalendario(ProspectoId);

GO

-- =============================================
-- Datos de Ejemplo para Pruebas
-- =============================================

-- Insertar usuarios de ejemplo
INSERT INTO Usuarios (NombreUsuario, Email, HashContrasena, Nombre, Apellido, RolId, SucursalId) VALUES
('jperez', 'jperez@crm.com', 'hash123', 'Juan', 'Pérez', 1, 1),
('mgarcia', 'mgarcia@crm.com', 'hash456', 'María', 'García', 2, 2),
('rlopez', 'rlopez@crm.com', 'hash789', 'Roberto', 'López', 3, 1),
('asanchez', 'asanchez@crm.com', 'hash101', 'Ana', 'Sánchez', 1, 3);

-- Insertar productos de ejemplo
INSERT INTO Productos (CodigoProducto, NombreProducto, Descripcion, CategoriaId, PrecioLista, PrecioCosto) VALUES
('HW-001', 'Servidor Dell PowerEdge R740', 'Servidor rack 2U con procesador Intel Xeon', 1, 85000.00, 65000.00),
('SW-001', 'Licencia Microsoft Office 365 Business', 'Suite de productividad empresarial', 2, 2500.00, 1800.00),
('SV-001', 'Consultoría de Infraestructura TI', 'Análisis y diseño de infraestructura', 3, 15000.00, 8000.00),
('HW-002', 'Laptop HP EliteBook 840 G8', 'Laptop empresarial con Intel Core i7', 1, 28000.00, 22000.00),
('SW-002', 'Licencia Windows Server 2022 Standard', 'Sistema operativo servidor', 2, 12000.00, 9000.00);

-- Insertar prospectos de ejemplo
INSERT INTO Prospectos (CodigoProspecto, NombreEmpresa, NombreContacto, ApellidoContacto, Email, Telefono, FuenteId, VendedorAsignadoId, SucursalId, ValorEstimado) VALUES
('PROS-2024-001', 'Tecnología Avanzada SA', 'Carlos', 'Ramírez', 'cramirez@tecavanzada.com', '+52-81-5555-1234', 1, 1, 1, 150000.00),
('PROS-2024-002', 'Soluciones Empresariales MX', 'Laura', 'Martínez', 'lmartinez@solempresariales.com', '+52-33-5555-5678', 2, 2, 2, 85000.00),
('PROS-2024-003', 'Grupo Industrial del Sureste', 'Fernando', 'Hernández', 'fhernandez@gis.com', '+52-99-5555-9012', 3, 4, 3, 220000.00);

GO

PRINT 'Esquema de base de datos CRM en español creado exitosamente.';

