-- =============================================
-- Sistema CRM - Esquema de Base de Datos en Español
-- SQL Server - Versión Completa
-- =============================================

USE master;
GO

-- Crear base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SistemaCRM')
BEGIN
    CREATE DATABASE [SistemaCRM];
END
GO

USE [SistemaCRM];
GO

-- =============================================
-- TABLAS DE CONFIGURACIÓN Y USUARIOS
-- =============================================

-- Tabla de Sucursales
CREATE TABLE Sucursales (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo NVARCHAR(10) NOT NULL UNIQUE,
    Nombre NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(500),
    Telefono NVARCHAR(20),
    Email NVARCHAR(100),
    Gerente NVARCHAR(100),
    EsActiva BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreadoPor INT,
    ActualizadoPor INT
);

-- Tabla de Roles de Usuario
CREATE TABLE RolesUsuario (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(200),
    Permisos NVARCHAR(MAX), -- JSON con permisos específicos
    EsActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo NVARCHAR(20) NOT NULL UNIQUE,
    NombreCompleto NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Telefono NVARCHAR(20),
    RolId INT NOT NULL,
    SucursalId INT NOT NULL,
    EsActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreadoPor INT,
    ActualizadoPor INT,
    FOREIGN KEY (RolId) REFERENCES RolesUsuario(Id),
    FOREIGN KEY (SucursalId) REFERENCES Sucursales(Id)
);

-- =============================================
-- TABLAS DE LEADS Y PROSPECTOS
-- =============================================

-- Tabla de Fuentes de Leads
CREATE TABLE FuentesLead (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(200),
    Color NVARCHAR(7), -- Código hexadecimal para UI
    EsActiva BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Tabla de Estados de Lead
CREATE TABLE EstadosLead (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(200),
    Orden INT NOT NULL,
    Color NVARCHAR(7), -- Código hexadecimal para UI
    EsEstadoFinal BIT NOT NULL DEFAULT 0,
    EsActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Tabla Principal de Leads
CREATE TABLE Leads (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CodigoLead NVARCHAR(20) NOT NULL UNIQUE,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    NombreEmpresa NVARCHAR(200),
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),
    TelefonoSecundario NVARCHAR(20),
    FuenteId INT NOT NULL,
    EstadoId INT NOT NULL,
    SucursalId INT NOT NULL,
    UsuarioAsignadoId INT,
    Puntuacion INT DEFAULT 0 CHECK (Puntuacion >= 0 AND Puntuacion <= 100),
    ValorEstimado DECIMAL(18,2) DEFAULT 0,
    Probabilidad INT DEFAULT 0 CHECK (Probabilidad >= 0 AND Probabilidad <= 100),
    FechaContacto DATETIME2,
    FechaSeguimiento DATETIME2,
    Notas NVARCHAR(MAX),
    EsActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreadoPor INT NOT NULL,
    ActualizadoPor INT,
    FOREIGN KEY (FuenteId) REFERENCES FuentesLead(Id),
    FOREIGN KEY (EstadoId) REFERENCES EstadosLead(Id),
    FOREIGN KEY (SucursalId) REFERENCES Sucursales(Id),
    FOREIGN KEY (UsuarioAsignadoId) REFERENCES Usuarios(Id),
    FOREIGN KEY (CreadoPor) REFERENCES Usuarios(Id),
    FOREIGN KEY (ActualizadoPor) REFERENCES Usuarios(Id)
);

-- Tabla de Actividades de Lead
CREATE TABLE ActividadesLead (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    LeadId INT NOT NULL,
    TipoActividad NVARCHAR(50) NOT NULL, -- 'Llamada', 'Email', 'Reunión', 'Nota', etc.
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    FechaActividad DATETIME2 NOT NULL,
    UsuarioId INT NOT NULL,
    EsCompletada BIT NOT NULL DEFAULT 0,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (LeadId) REFERENCES Leads(Id) ON DELETE CASCADE,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

-- =============================================
-- TABLAS DE CLIENTES
-- =============================================

-- Tabla Principal de Clientes
CREATE TABLE Clientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CodigoCliente NVARCHAR(20) NOT NULL UNIQUE,
    RazonSocial NVARCHAR(200) NOT NULL,
    NombreComercial NVARCHAR(200),
    RFC NVARCHAR(13),
    TipoCliente NVARCHAR(50) NOT NULL DEFAULT 'Empresa', -- 'Empresa', 'Persona Física'
    SucursalId INT NOT NULL,
    UsuarioAsignadoId INT,
    LeadOrigenId INT, -- Referencia al lead que se convirtió
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETDATE(),
    EsActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreadoPor INT NOT NULL,
    ActualizadoPor INT,
    FOREIGN KEY (SucursalId) REFERENCES Sucursales(Id),
    FOREIGN KEY (UsuarioAsignadoId) REFERENCES Usuarios(Id),
    FOREIGN KEY (LeadOrigenId) REFERENCES Leads(Id),
    FOREIGN KEY (CreadoPor) REFERENCES Usuarios(Id),
    FOREIGN KEY (ActualizadoPor) REFERENCES Usuarios(Id)
);

-- Tabla de Contactos de Cliente
CREATE TABLE ContactosCliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Cargo NVARCHAR(100),
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),
    TelefonoSecundario NVARCHAR(20),
    EsPrincipal BIT NOT NULL DEFAULT 0,
    EsActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id) ON DELETE CASCADE
);

-- Tabla de Direcciones de Cliente
CREATE TABLE DireccionesCliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    TipoDireccion NVARCHAR(50) NOT NULL DEFAULT 'Fiscal', -- 'Fiscal', 'Entrega', 'Facturación'
    Calle NVARCHAR(200),
    NumeroExterior NVARCHAR(10),
    NumeroInterior NVARCHAR(10),
    Colonia NVARCHAR(100),
    Ciudad NVARCHAR(100),
    Estado NVARCHAR(100),
    CodigoPostal NVARCHAR(10),
    Pais NVARCHAR(50) DEFAULT 'México',
    EsPrincipal BIT NOT NULL DEFAULT 0,
    EsActiva BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id) ON DELETE CASCADE
);

-- =============================================
-- TABLAS DE PRODUCTOS Y SERVICIOS
-- =============================================

-- Tabla de Categorías de Producto
CREATE TABLE CategoriasProducto (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500),
    CategoriaPadreId INT,
    EsActiva BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (CategoriaPadreId) REFERENCES CategoriasProducto(Id)
);

-- Tabla de Productos
CREATE TABLE Productos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo NVARCHAR(50) NOT NULL UNIQUE,
    Nombre NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    CategoriaId INT NOT NULL,
    UnidadMedida NVARCHAR(20) NOT NULL DEFAULT 'PZA',
    PrecioBase DECIMAL(18,2) NOT NULL DEFAULT 0,
    Costo DECIMAL(18,2) DEFAULT 0,
    Stock INT DEFAULT 0,
    StockMinimo INT DEFAULT 0,
    EsActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreadoPor INT NOT NULL,
    ActualizadoPor INT,
    FOREIGN KEY (CategoriaId) REFERENCES CategoriasProducto(Id),
    FOREIGN KEY (CreadoPor) REFERENCES Usuarios(Id),
    FOREIGN KEY (ActualizadoPor) REFERENCES Usuarios(Id)
);

-- =============================================
-- TABLAS DE COTIZACIONES Y VENTAS
-- =============================================

-- Tabla de Estados de Cotización
CREATE TABLE EstadosCotizacion (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(200),
    Orden INT NOT NULL,
    Color NVARCHAR(7),
    EsEstadoFinal BIT NOT NULL DEFAULT 0,
    EsActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Tabla Principal de Cotizaciones
CREATE TABLE Cotizaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NumeroCotizacion NVARCHAR(20) NOT NULL UNIQUE,
    ClienteId INT,
    LeadId INT,
    SucursalId INT NOT NULL,
    UsuarioId INT NOT NULL,
    EstadoId INT NOT NULL,
    FechaCotizacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaVencimiento DATETIME2,
    Subtotal DECIMAL(18,2) NOT NULL DEFAULT 0,
    Descuento DECIMAL(18,2) DEFAULT 0,
    PorcentajeDescuento DECIMAL(5,2) DEFAULT 0,
    Impuestos DECIMAL(18,2) DEFAULT 0,
    Total DECIMAL(18,2) NOT NULL DEFAULT 0,
    Moneda NVARCHAR(3) DEFAULT 'MXN',
    TipoCambio DECIMAL(10,4) DEFAULT 1,
    Observaciones NVARCHAR(MAX),
    TerminosCondiciones NVARCHAR(MAX),
    Probabilidad INT DEFAULT 50 CHECK (Probabilidad >= 0 AND Probabilidad <= 100),
    EsActiva BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreadoPor INT NOT NULL,
    ActualizadoPor INT,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY (LeadId) REFERENCES Leads(Id),
    FOREIGN KEY (SucursalId) REFERENCES Sucursales(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (EstadoId) REFERENCES EstadosCotizacion(Id),
    FOREIGN KEY (CreadoPor) REFERENCES Usuarios(Id),
    FOREIGN KEY (ActualizadoPor) REFERENCES Usuarios(Id)
);

-- Tabla de Partidas de Cotización
CREATE TABLE PartidasCotizacion (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CotizacionId INT NOT NULL,
    ProductoId INT,
    Descripcion NVARCHAR(500) NOT NULL,
    Cantidad DECIMAL(10,2) NOT NULL DEFAULT 1,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    Descuento DECIMAL(18,2) DEFAULT 0,
    Subtotal DECIMAL(18,2) NOT NULL,
    Orden INT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (CotizacionId) REFERENCES Cotizaciones(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductoId) REFERENCES Productos(Id)
);

-- =============================================
-- TABLAS DE VISITAS Y ACTIVIDADES
-- =============================================

-- Tabla de Tipos de Visita
CREATE TABLE TiposVisita (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(200),
    Color NVARCHAR(7),
    EsActivo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Tabla de Visitas
CREATE TABLE Visitas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT,
    LeadId INT,
    TipoVisitaId INT NOT NULL,
    UsuarioId INT NOT NULL,
    FechaVisita DATETIME2 NOT NULL,
    Duracion INT, -- Duración en minutos
    Objetivo NVARCHAR(500),
    Resumen NVARCHAR(MAX),
    Resultado NVARCHAR(MAX),
    ProximaAccion NVARCHAR(500),
    FechaProximaAccion DATETIME2,
    EsCompletada BIT NOT NULL DEFAULT 0,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreadoPor INT NOT NULL,
    ActualizadoPor INT,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY (LeadId) REFERENCES Leads(Id),
    FOREIGN KEY (TipoVisitaId) REFERENCES TiposVisita(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (CreadoPor) REFERENCES Usuarios(Id),
    FOREIGN KEY (ActualizadoPor) REFERENCES Usuarios(Id)
);

-- Tabla de Documentos de Visita
CREATE TABLE DocumentosVisita (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VisitaId INT NOT NULL,
    NombreArchivo NVARCHAR(255) NOT NULL,
    RutaArchivo NVARCHAR(500) NOT NULL,
    TipoArchivo NVARCHAR(50),
    TamanoArchivo BIGINT,
    Descripcion NVARCHAR(500),
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreadoPor INT NOT NULL,
    FOREIGN KEY (VisitaId) REFERENCES Visitas(Id) ON DELETE CASCADE,
    FOREIGN KEY (CreadoPor) REFERENCES Usuarios(Id)
);

-- =============================================
-- TABLAS DE TAREAS Y NOTIFICACIONES
-- =============================================

-- Tabla de Tareas
CREATE TABLE Tareas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Prioridad NVARCHAR(20) NOT NULL DEFAULT 'Media', -- 'Baja', 'Media', 'Alta', 'Urgente'
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Pendiente', -- 'Pendiente', 'En Proceso', 'Completada', 'Cancelada'
    FechaVencimiento DATETIME2,
    UsuarioAsignadoId INT NOT NULL,
    ClienteId INT,
    LeadId INT,
    CotizacionId INT,
    EsCompletada BIT NOT NULL DEFAULT 0,
    FechaCompletada DATETIME2,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FechaActualizacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CreadoPor INT NOT NULL,
    ActualizadoPor INT,
    FOREIGN KEY (UsuarioAsignadoId) REFERENCES Usuarios(Id),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY (LeadId) REFERENCES Leads(Id),
    FOREIGN KEY (CotizacionId) REFERENCES Cotizaciones(Id),
    FOREIGN KEY (CreadoPor) REFERENCES Usuarios(Id),
    FOREIGN KEY (ActualizadoPor) REFERENCES Usuarios(Id)
);

-- Tabla de Notificaciones
CREATE TABLE Notificaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    Titulo NVARCHAR(200) NOT NULL,
    Mensaje NVARCHAR(MAX) NOT NULL,
    Tipo NVARCHAR(50) NOT NULL DEFAULT 'Info', -- 'Info', 'Advertencia', 'Error', 'Éxito'
    EsLeida BIT NOT NULL DEFAULT 0,
    FechaLeida DATETIME2,
    EntidadTipo NVARCHAR(50), -- 'Lead', 'Cliente', 'Cotización', etc.
    EntidadId INT,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE
);

-- =============================================
-- ÍNDICES PARA OPTIMIZACIÓN
-- =============================================

-- Índices en Leads
CREATE INDEX IX_Leads_Sucursal ON Leads(SucursalId);
CREATE INDEX IX_Leads_Usuario ON Leads(UsuarioAsignadoId);
CREATE INDEX IX_Leads_Estado ON Leads(EstadoId);
CREATE INDEX IX_Leads_Fuente ON Leads(FuenteId);
CREATE INDEX IX_Leads_FechaCreacion ON Leads(FechaCreacion);
CREATE INDEX IX_Leads_Email ON Leads(Email);

-- Índices en Clientes
CREATE INDEX IX_Clientes_Sucursal ON Clientes(SucursalId);
CREATE INDEX IX_Clientes_Usuario ON Clientes(UsuarioAsignadoId);
CREATE INDEX IX_Clientes_RFC ON Clientes(RFC);

-- Índices en Cotizaciones
CREATE INDEX IX_Cotizaciones_Cliente ON Cotizaciones(ClienteId);
CREATE INDEX IX_Cotizaciones_Lead ON Cotizaciones(LeadId);
CREATE INDEX IX_Cotizaciones_Usuario ON Cotizaciones(UsuarioId);
CREATE INDEX IX_Cotizaciones_Estado ON Cotizaciones(EstadoId);
CREATE INDEX IX_Cotizaciones_Fecha ON Cotizaciones(FechaCotizacion);

-- Índices en Visitas
CREATE INDEX IX_Visitas_Cliente ON Visitas(ClienteId);
CREATE INDEX IX_Visitas_Lead ON Visitas(LeadId);
CREATE INDEX IX_Visitas_Usuario ON Visitas(UsuarioId);
CREATE INDEX IX_Visitas_Fecha ON Visitas(FechaVisita);

-- Índices en Tareas
CREATE INDEX IX_Tareas_Usuario ON Tareas(UsuarioAsignadoId);
CREATE INDEX IX_Tareas_Estado ON Tareas(Estado);
CREATE INDEX IX_Tareas_Vencimiento ON Tareas(FechaVencimiento);

-- =============================================
-- DATOS INICIALES
-- =============================================

-- Insertar Sucursales
INSERT INTO Sucursales (Codigo, Nombre, Direccion, Telefono, Email, Gerente) VALUES
('NORTE', 'Sucursal Norte', 'Av. Revolución 1234, Col. Centro, Monterrey, NL', '+52 81 1234 5678', 'norte@empresa.com', 'Manuel García'),
('CENTRO', 'Sucursal Centro', 'Insurgentes Sur 567, Col. Roma, CDMX', '+52 55 8765 4321', 'centro@empresa.com', 'Jorge López'),
('SUR', 'Sucursal Sur', 'Av. Juárez 890, Col. Centro, Guadalajara, JAL', '+52 33 2468 1357', 'sur@empresa.com', 'Ana Martínez');

-- Insertar Roles de Usuario
INSERT INTO RolesUsuario (Nombre, Descripcion) VALUES
('Vendedor', 'Gestión básica de leads y clientes asignados'),
('Cotizador', 'Creación y gestión de cotizaciones'),
('Gerente', 'Gestión de equipo y sucursal'),
('Director', 'Acceso a múltiples sucursales y reportes ejecutivos'),
('Sistemas', 'Administración técnica del sistema'),
('Contador', 'Acceso a información financiera y reportes'),
('Director de Sucursal', 'Gestión completa de una sucursal específica'),
('Consejero', 'Acceso de consulta y asesoría'),
('Dirección General', 'Acceso completo al sistema');

-- Insertar Usuarios
INSERT INTO Usuarios (Codigo, NombreCompleto, Email, Telefono, RolId, SucursalId, CreadoPor) VALUES
('MAN001', 'Manuel García Rodríguez', 'manuel.garcia@empresa.com', '+52 81 1111 1111', 4, 1, 1),
('JOR001', 'Jorge López Hernández', 'jorge.lopez@empresa.com', '+52 55 2222 2222', 4, 2, 1),
('ANA001', 'Ana Martínez Sánchez', 'ana.martinez@empresa.com', '+52 33 3333 3333', 7, 3, 1),
('CAR001', 'Carlos Pérez Gómez', 'carlos.perez@empresa.com', '+52 81 4444 4444', 1, 1, 1),
('MAR001', 'María González Torres', 'maria.gonzalez@empresa.com', '+52 55 5555 5555', 1, 2, 1);

-- Insertar Fuentes de Lead
INSERT INTO FuentesLead (Nombre, Descripcion, Color) VALUES
('Sitio Web', 'Leads generados desde el sitio web corporativo', '#007bff'),
('Referencia', 'Referencias de clientes existentes', '#28a745'),
('Redes Sociales', 'Leads de Facebook, LinkedIn, Instagram', '#6f42c1'),
('Email Marketing', 'Campañas de email marketing', '#fd7e14'),
('Llamada Fría', 'Prospección telefónica directa', '#dc3545'),
('Exposición/Feria', 'Eventos y ferias comerciales', '#20c997'),
('Campaña Marketing', 'Campañas publicitarias pagadas', '#ffc107');

-- Insertar Estados de Lead
INSERT INTO EstadosLead (Nombre, Descripcion, Orden, Color, EsEstadoFinal) VALUES
('Nuevo', 'Lead recién creado, sin contactar', 1, '#6c757d', 0),
('Contactado', 'Primer contacto realizado', 2, '#0dcaf0', 0),
('Calificado', 'Lead calificado como prospecto válido', 3, '#0d6efd', 0),
('Propuesta', 'Propuesta comercial enviada', 4, '#6f42c1', 0),
('Negociación', 'En proceso de negociación', 5, '#fd7e14', 0),
('Cerrado Ganado', 'Lead convertido en cliente', 6, '#198754', 1),
('Cerrado Perdido', 'Lead perdido o descartado', 7, '#dc3545', 1);

-- Insertar Estados de Cotización
INSERT INTO EstadosCotizacion (Nombre, Descripcion, Orden, Color, EsEstadoFinal) VALUES
('Borrador', 'Cotización en proceso de creación', 1, '#6c757d', 0),
('Enviada', 'Cotización enviada al cliente', 2, '#0d6efd', 0),
('En Revisión', 'Cliente revisando la cotización', 3, '#fd7e14', 0),
('Aprobada', 'Cotización aprobada por el cliente', 4, '#198754', 1),
('Rechazada', 'Cotización rechazada', 5, '#dc3545', 1),
('Vencida', 'Cotización vencida sin respuesta', 6, '#6c757d', 1);

-- Insertar Tipos de Visita
INSERT INTO TiposVisita (Nombre, Descripcion, Color) VALUES
('Presencial', 'Visita física en las instalaciones del cliente', '#198754'),
('Virtual', 'Reunión por videoconferencia', '#0d6efd'),
('Llamada', 'Llamada telefónica', '#fd7e14'),
('Email', 'Comunicación por correo electrónico', '#6f42c1'),
('WhatsApp', 'Comunicación por WhatsApp', '#20c997');

-- Insertar Categorías de Producto
INSERT INTO CategoriasProducto (Nombre, Descripcion) VALUES
('Construcción', 'Materiales y servicios de construcción'),
('Herramientas', 'Herramientas y equipos'),
('Materiales', 'Materiales de construcción básicos'),
('Equipos', 'Maquinaria y equipos especializados'),
('Servicios', 'Servicios profesionales y consultoría');

-- Insertar Productos de Ejemplo
INSERT INTO Productos (Codigo, Nombre, Descripcion, CategoriaId, UnidadMedida, PrecioBase, Costo, CreadoPor) VALUES
('CEMENTO-001', 'Cemento Portland Tipo I', 'Cemento Portland para uso general en construcción', 3, 'TON', 3500.00, 2800.00, 1),
('VARILLA-001', 'Varilla Corrugada #4', 'Varilla de acero corrugada de 1/2 pulgada', 3, 'TON', 18500.00, 15000.00, 1),
('BLOCK-001', 'Block de Concreto 15x20x40', 'Block hueco de concreto para muros', 3, 'PZA', 25.00, 18.00, 1),
('TALADRO-001', 'Taladro Percutor Industrial', 'Taladro percutor para uso industrial', 2, 'PZA', 4500.00, 3200.00, 1),
('CONSULTA-001', 'Consultoría Estructural', 'Servicios de consultoría en ingeniería estructural', 5, 'HR', 1200.00, 800.00, 1);

-- =============================================
-- TRIGGERS PARA AUDITORÍA
-- =============================================

-- Trigger para actualizar FechaActualizacion en Leads
CREATE TRIGGER TR_Leads_UpdateTimestamp
ON Leads
AFTER UPDATE
AS
BEGIN
    UPDATE Leads 
    SET FechaActualizacion = GETDATE()
    WHERE Id IN (SELECT Id FROM inserted);
END;
GO

-- Trigger para actualizar FechaActualizacion en Clientes
CREATE TRIGGER TR_Clientes_UpdateTimestamp
ON Clientes
AFTER UPDATE
AS
BEGIN
    UPDATE Clientes 
    SET FechaActualizacion = GETDATE()
    WHERE Id IN (SELECT Id FROM inserted);
END;
GO

-- Trigger para actualizar FechaActualizacion en Cotizaciones
CREATE TRIGGER TR_Cotizaciones_UpdateTimestamp
ON Cotizaciones
AFTER UPDATE
AS
BEGIN
    UPDATE Cotizaciones 
    SET FechaActualizacion = GETDATE()
    WHERE Id IN (SELECT Id FROM inserted);
END;
GO

-- =============================================
-- VISTAS PARA REPORTES
-- =============================================

-- Vista de Leads con información completa
CREATE VIEW VW_LeadsCompletos AS
SELECT 
    l.Id,
    l.CodigoLead,
    l.Nombres + ' ' + l.Apellidos AS NombreCompleto,
    l.NombreEmpresa,
    l.Email,
    l.Telefono,
    fl.Nombre AS Fuente,
    el.Nombre AS Estado,
    s.Nombre AS Sucursal,
    u.NombreCompleto AS UsuarioAsignado,
    l.Puntuacion,
    l.ValorEstimado,
    l.Probabilidad,
    l.FechaCreacion,
    l.FechaActualizacion
FROM Leads l
    LEFT JOIN FuentesLead fl ON l.FuenteId = fl.Id
    LEFT JOIN EstadosLead el ON l.EstadoId = el.Id
    LEFT JOIN Sucursales s ON l.SucursalId = s.Id
    LEFT JOIN Usuarios u ON l.UsuarioAsignadoId = u.Id
WHERE l.EsActivo = 1;
GO

-- Vista de Cotizaciones con información completa
CREATE VIEW VW_CotizacionesCompletas AS
SELECT 
    c.Id,
    c.NumeroCotizacion,
    COALESCE(cl.RazonSocial, l.Nombres + ' ' + l.Apellidos) AS Cliente,
    s.Nombre AS Sucursal,
    u.NombreCompleto AS Usuario,
    ec.Nombre AS Estado,
    c.FechaCotizacion,
    c.FechaVencimiento,
    c.Total,
    c.Moneda,
    c.Probabilidad
FROM Cotizaciones c
    LEFT JOIN Clientes cl ON c.ClienteId = cl.Id
    LEFT JOIN Leads l ON c.LeadId = l.Id
    LEFT JOIN Sucursales s ON c.SucursalId = s.Id
    LEFT JOIN Usuarios u ON c.UsuarioId = u.Id
    LEFT JOIN EstadosCotizacion ec ON c.EstadoId = ec.Id
WHERE c.EsActiva = 1;
GO

PRINT 'Esquema de base de datos CRM en español creado exitosamente.';
PRINT 'Tablas creadas: 20';
PRINT 'Índices creados: 15';
PRINT 'Triggers creados: 3';
PRINT 'Vistas creadas: 2';
PRINT 'Registros de datos iniciales insertados.';
