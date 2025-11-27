# Refactorización Sistema CRM - HTMX + Razor Pages

## Estado Actual de la Refactorización

### ✅ Completado

#### 1. Estructura del Proyecto
- ✅ Carpetas Razor Pages creadas (`Pages/`, `Pages/Shared/`, `Pages/Partials/`)
- ✅ `Program.cs` actualizado con soporte para Razor Pages
- ✅ `_ViewImports.cshtml` y `_ViewStart.cshtml` configurados
- ✅ `_Layout.cshtml` creado con Bootstrap 5, HTMX, Chart.js y FullCalendar
- ✅ Generación de documentación XML habilitada en `.csproj`

#### 2. Esquema de Base de Datos
- ✅ SQL completo traducido al español (`esquema-crm-espanol-completo.sql`)
- ✅ 15+ tablas con nombres en español
- ✅ Todas las columnas traducidas
- ✅ Índices y relaciones configuradas
- ✅ Datos de ejemplo (seed data) en español

**Tablas principales:**
- `RolesUsuario`, `Sucursales`, `Usuarios`
- `CategoriasCliente`, `Clientes`, `ContactosCliente`
- `FuentesProspecto`, `Prospectos`, `HistorialProspectos`
- `CategoriasProducto`, `Productos`, `HistorialPrecios`
- `Cotizaciones`, `DetallesCotizacion`, `HistorialCotizaciones`
- `Visitas`, `Tareas`, `EventosCalendario`
- `Documentos`, `MetricasVentas`, `OrganigramaClientes`

#### 3. Modelos C# (Entity Framework)
- ✅ Todos los modelos traducidos al español
- ✅ Documentación XML completa en cada propiedad
- ✅ Atributos `[Table]` apuntando a tablas en español
- ✅ Propiedades de navegación actualizadas
- ✅ Propiedades computadas (`NombreCompleto`, `ContactoCompleto`)

**Modelos principales:**
- `RolUsuario`, `Sucursal`, `Usuario`
- `CategoriaCliente`, `Cliente`, `ContactoCliente`
- `FuenteProspecto`, `Prospecto`
- `CategoriaProducto`, `Producto`
- `Cotizacion`, `DetalleCotizacion`
- `Visita`, `Tarea`, `EventoCalendario`

#### 4. DbContext
- ✅ `CRMDbContext` actualizado con DbSets en español
- ✅ Todas las relaciones configuradas (Restrict, SetNull, Cascade)
- ✅ Índices únicos y de búsqueda
- ✅ Seed data con datos de ejemplo

#### 5. Vistas Parciales Razor
- ✅ `_ProspectosList.cshtml` - Tabla de prospectos con HTMX
- ✅ `_ClientesCards.cshtml` - Tarjetas de clientes
- ✅ `_DashboardStats.cshtml` - Estadísticas y gráficas
- ✅ `_CotizacionesList.cshtml` - Lista de cotizaciones
- ✅ `_CalendarioEventos.cshtml` - Calendario con FullCalendar

**Características de las vistas:**
- Atributos HTMX (`hx-get`, `hx-post`, `hx-delete`, `hx-target`, `hx-swap`)
- Badges dinámicos según estado
- Botones de acción con confirmación
- Formateo de moneda en español (es-MX)
- Integración con modales Bootstrap
- Gráficas con Chart.js

#### 6. Controladores API
- ✅ `ProspectosController.cs` - CRUD completo de prospectos
- ✅ `ClientesController.cs` - CRUD completo de clientes

**Características de los controladores:**
- Detección automática de peticiones HTMX
- Respuesta dual: JSON para API / HTML parcial para HTMX
- Documentación XML completa en español
- Generación automática de códigos
- Filtros avanzados (sucursal, estado, vendedor, búsqueda)
- Paginación con headers HTTP
- Endpoints de estadísticas
- Headers de éxito (`X-Success-Message`) para notificaciones

**Endpoints ProspectosController:**
```
GET    /api/prospectos                           - Lista con filtros
GET    /api/prospectos/{id}                      - Detalle
POST   /api/prospectos                           - Crear
PUT    /api/prospectos/{id}                      - Actualizar
DELETE /api/prospectos/{id}                      - Eliminar
POST   /api/prospectos/{id}/convertir-a-cliente  - Conversión
GET    /api/prospectos/embudo-ventas             - Estadísticas
GET    /api/prospectos/fuentes                   - Fuentes disponibles
```

**Endpoints ClientesController:**
```
GET    /api/clientes                             - Lista con filtros
GET    /api/clientes/{id}                        - Detalle
POST   /api/clientes                             - Crear
PUT    /api/clientes/{id}                        - Actualizar
DELETE /api/clientes/{id}                        - Eliminar
GET    /api/clientes/{id}/contactos              - Contactos del cliente
POST   /api/clientes/{id}/contactos              - Agregar contacto
GET    /api/clientes/categorias                  - Categorías disponibles
GET    /api/clientes/estadisticas-categorias     - Stats por categoría
GET    /api/clientes/estadisticas-sucursales     - Stats por sucursal
```

---

### 🔄 Pendiente de Completar

#### 7. Controladores Adicionales

##### CotizacionesController
```csharp
GET    /api/cotizaciones                         - Lista con filtros
GET    /api/cotizaciones/{id}                    - Detalle
POST   /api/cotizaciones                         - Crear
PUT    /api/cotizaciones/{id}                    - Actualizar
DELETE /api/cotizaciones/{id}                    - Eliminar
GET    /api/cotizaciones/{id}/pdf                - Generar PDF
POST   /api/cotizaciones/{id}/enviar             - Enviar por email
PUT    /api/cotizaciones/{id}/aprobar            - Aprobar
PUT    /api/cotizaciones/{id}/rechazar           - Rechazar
GET    /api/cotizaciones/estadisticas            - Estadísticas
```

##### ProductosController
```csharp
GET    /api/productos                            - Lista con filtros
GET    /api/productos/{id}                       - Detalle
POST   /api/productos                            - Crear
PUT    /api/productos/{id}                       - Actualizar
DELETE /api/productos/{id}                       - Eliminar
GET    /api/productos/categorias                 - Categorías
GET    /api/productos/buscar                     - Búsqueda rápida
```

##### VisitasController
```csharp
GET    /api/visitas                              - Lista con filtros
GET    /api/visitas/{id}                         - Detalle
POST   /api/visitas                              - Crear
PUT    /api/visitas/{id}                         - Actualizar
DELETE /api/visitas/{id}                         - Eliminar
POST   /api/visitas/{id}/documentos              - Subir documento
```

##### TareasController
```csharp
GET    /api/tareas                               - Lista con filtros
GET    /api/tareas/{id}                          - Detalle
POST   /api/tareas                               - Crear
PUT    /api/tareas/{id}                          - Actualizar
DELETE /api/tareas/{id}                          - Eliminar
PUT    /api/tareas/{id}/completar                - Marcar completada
GET    /api/tareas/pendientes                    - Tareas pendientes
```

##### EventosController
```csharp
GET    /api/eventos                              - Lista con filtros
GET    /api/eventos/calendario                   - Eventos para calendario
GET    /api/eventos/{id}                         - Detalle
POST   /api/eventos                              - Crear
PUT    /api/eventos/{id}                         - Actualizar
DELETE /api/eventos/{id}                         - Eliminar
PUT    /api/eventos/{id}/mover                   - Mover evento
PUT    /api/eventos/{id}/redimensionar           - Redimensionar
```

##### DashboardController
```csharp
GET    /api/dashboard/estadisticas               - Estadísticas generales
GET    /api/dashboard/embudo-ventas              - Datos embudo
GET    /api/dashboard/ventas-mensuales           - Ventas por mes
GET    /api/dashboard/distribucion-sucursales    - Distribución
GET    /api/dashboard/actividad-reciente         - Actividad
GET    /api/dashboard/tareas-pendientes          - Tareas
```

#### 8. Vistas Parciales Adicionales

##### Formularios
- `_ProspectoForm.cshtml` - Formulario crear/editar prospecto
- `_ClienteForm.cshtml` - Formulario crear/editar cliente
- `_CotizacionForm.cshtml` - Formulario de cotización
- `_ProductoForm.cshtml` - Formulario de producto
- `_VisitaForm.cshtml` - Formulario de visita
- `_TareaForm.cshtml` - Formulario de tarea
- `_EventoForm.cshtml` - Formulario de evento

##### Detalles
- `_ProspectoDetalle.cshtml` - Vista detalle de prospecto
- `_ClienteDetalle.cshtml` - Vista detalle de cliente
- `_CotizacionDetalle.cshtml` - Vista detalle de cotización
- `_VisitaDetalle.cshtml` - Vista detalle de visita

##### Componentes
- `_ContactosCliente.cshtml` - Lista de contactos
- `_HistorialActividad.cshtml` - Historial de actividades
- `_DocumentosAdjuntos.cshtml` - Lista de documentos

#### 9. Página Principal Razor

##### Index.cshtml
Crear página principal que reemplace `index.html` con:
- Navegación lateral con HTMX
- Secciones dinámicas cargadas con HTMX
- Modales para formularios
- Integración con todos los controladores

Estructura:
```html
@page
@model IndexModel

<!-- Navegación lateral -->
<div class="sidebar">
    <a hx-get="/api/dashboard/estadisticas" 
       hx-target="#main-content" 
       hx-swap="innerHTML">Dashboard</a>
    <a hx-get="/api/prospectos" 
       hx-target="#main-content">Prospectos</a>
    <a hx-get="/api/clientes" 
       hx-target="#main-content">Clientes</a>
    <!-- ... más enlaces -->
</div>

<!-- Contenido principal -->
<div id="main-content">
    <!-- Contenido cargado dinámicamente -->
</div>

<!-- Modales -->
<div id="detailModal" class="modal">...</div>
<div id="editModal" class="modal">...</div>
<div id="eventModal" class="modal">...</div>
```

#### 10. Configuración Swagger
Actualizar `Program.cs` para:
- Incluir comentarios XML en Swagger
- Configurar títulos y descripciones en español
- Agregar ejemplos de peticiones/respuestas
- Configurar autenticación (si aplica)

#### 11. Migraciones de Base de Datos
```bash
# Eliminar migraciones anteriores
dotnet ef migrations remove --force

# Crear nueva migración con esquema en español
dotnet ef migrations add MigracionInicialEspanol

# Aplicar migración
dotnet ef database update
```

#### 12. Pruebas
- Probar todos los endpoints con Postman/Swagger
- Verificar respuestas JSON y HTML
- Probar flujo HTMX completo
- Verificar notificaciones y mensajes de éxito
- Probar paginación y filtros
- Verificar gráficas y calendario

---

## Guía de Implementación Rápida

### Paso 1: Completar Controladores Faltantes
Usar `ProspectosController.cs` como plantilla para crear:
1. `CotizacionesController.cs`
2. `ProductosController.cs`
3. `VisitasController.cs`
4. `TareasController.cs`
5. `EventosController.cs`
6. `DashboardController.cs`

### Paso 2: Crear Vistas Parciales de Formularios
Estructura base para formularios:
```cshtml
@model CRMSystem.API.Models.Prospecto

<form hx-post="/api/prospectos" 
      hx-target="#prospectos-list" 
      hx-swap="outerHTML">
    
    <div class="mb-3">
        <label asp-for="NombreEmpresa" class="form-label"></label>
        <input asp-for="NombreEmpresa" class="form-control" />
        <span asp-validation-for="NombreEmpresa" class="text-danger"></span>
    </div>
    
    <!-- Más campos -->
    
    <button type="submit" class="btn btn-primary">Guardar</button>
</form>
```

### Paso 3: Crear Página Principal
```csharp
// Pages/Index.cshtml.cs
public class IndexModel : PageModel
{
    private readonly CRMDbContext _context;
    
    public IndexModel(CRMDbContext context)
    {
        _context = context;
    }
    
    public async Task OnGetAsync()
    {
        ViewData["CurrentUser"] = "Usuario Demo";
    }
}
```

### Paso 4: Aplicar Migraciones
```bash
cd /home/ubuntu/CRMSystem/CRMSystem.API
dotnet ef migrations add MigracionInicialEspanol
dotnet ef database update
```

### Paso 5: Ejecutar y Probar
```bash
dotnet run
```

Acceder a:
- Frontend: `https://localhost:5001/`
- Swagger: `https://localhost:5001/swagger`

---

## Patrones de Código

### Controlador con HTMX
```csharp
[HttpGet]
public async Task<IActionResult> ObtenerLista()
{
    var datos = await _context.Entidades.ToListAsync();
    
    if (Request.Headers["HX-Request"] == "true")
    {
        return PartialView("~/Pages/Partials/_Lista.cshtml", datos);
    }
    
    return Ok(datos);
}
```

### Vista Parcial con HTMX
```cshtml
<div id="lista-entidades">
    @foreach (var item in Model)
    {
        <div class="item">
            <button hx-get="/api/entidades/@item.Id" 
                    hx-target="#detalle" 
                    hx-swap="innerHTML">
                Ver
            </button>
        </div>
    }
</div>
```

### Notificaciones de Éxito
```csharp
Response.Headers.Add("X-Success-Message", "Operación exitosa");
```

```javascript
// En _Layout.cshtml
document.body.addEventListener('htmx:afterSwap', (event) => {
    const successMessage = event.detail.xhr.getResponseHeader('X-Success-Message');
    if (successMessage) {
        showNotification(successMessage, 'success');
    }
});
```

---

## Checklist Final

### Backend
- [ ] Todos los controladores creados
- [ ] Documentación XML completa
- [ ] Migraciones aplicadas
- [ ] Seed data cargado
- [ ] Swagger configurado

### Frontend
- [ ] Página principal Index.cshtml
- [ ] Todas las vistas parciales
- [ ] Formularios con validación
- [ ] Modales configurados
- [ ] Gráficas funcionando
- [ ] Calendario funcionando

### Integración
- [ ] HTMX funcionando en todos los endpoints
- [ ] Notificaciones mostrándose
- [ ] Paginación funcionando
- [ ] Filtros funcionando
- [ ] Búsqueda funcionando

### Pruebas
- [ ] CRUD de prospectos
- [ ] CRUD de clientes
- [ ] CRUD de cotizaciones
- [ ] CRUD de productos
- [ ] CRUD de visitas
- [ ] CRUD de tareas
- [ ] CRUD de eventos
- [ ] Dashboard con gráficas
- [ ] Calendario interactivo

---

## Comandos Útiles

```bash
# Restaurar paquetes
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run

# Crear migración
dotnet ef migrations add NombreMigracion

# Aplicar migración
dotnet ef database update

# Revertir migración
dotnet ef database update MigracionAnterior

# Eliminar última migración
dotnet ef migrations remove

# Ver migraciones
dotnet ef migrations list

# Generar script SQL
dotnet ef migrations script

# Limpiar y reconstruir
dotnet clean && dotnet build
```

---

## Estructura Final del Proyecto

```
CRMSystem.API/
├── Controllers/
│   ├── ClientesController.cs
│   ├── CotizacionesController.cs
│   ├── DashboardController.cs
│   ├── EventosController.cs
│   ├── ProductosController.cs
│   ├── ProspectosController.cs
│   ├── TareasController.cs
│   └── VisitasController.cs
├── Data/
│   └── CRMDbContext.cs
├── Models/
│   └── CRMModels.cs
├── Pages/
│   ├── Shared/
│   │   └── _Layout.cshtml
│   ├── Partials/
│   │   ├── _CalendarioEventos.cshtml
│   │   ├── _ClienteDetalle.cshtml
│   │   ├── _ClienteForm.cshtml
│   │   ├── _ClientesCards.cshtml
│   │   ├── _ContactosCliente.cshtml
│   │   ├── _CotizacionDetalle.cshtml
│   │   ├── _CotizacionForm.cshtml
│   │   ├── _CotizacionesList.cshtml
│   │   ├── _DashboardStats.cshtml
│   │   ├── _EventoForm.cshtml
│   │   ├── _ProspectoDetalle.cshtml
│   │   ├── _ProspectoForm.cshtml
│   │   ├── _ProspectosList.cshtml
│   │   ├── _TareaForm.cshtml
│   │   └── _VisitaForm.cshtml
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Index.cshtml
│   └── Index.cshtml.cs
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── images/
├── appsettings.json
├── CRMSystem.API.csproj
└── Program.cs
```

---

## Recursos y Referencias

### Documentación
- [HTMX Documentation](https://htmx.org/docs/)
- [ASP.NET Core Razor Pages](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Bootstrap 5](https://getbootstrap.com/docs/5.3/)
- [Chart.js](https://www.chartjs.org/docs/)
- [FullCalendar](https://fullcalendar.io/docs)

### Ejemplos de Código
- Ver controladores existentes: `ProspectosController.cs`, `ClientesController.cs`
- Ver vistas parciales: `_ProspectosList.cshtml`, `_ClientesCards.cshtml`
- Ver layout: `_Layout.cshtml`

---

## Notas Importantes

1. **Consistencia**: Mantener el mismo patrón en todos los controladores
2. **Documentación**: Agregar comentarios XML a todos los métodos públicos
3. **Validación**: Usar Data Annotations en los modelos
4. **Seguridad**: Implementar autenticación y autorización
5. **Performance**: Usar paginación en todas las listas
6. **UX**: Agregar loading indicators para operaciones largas
7. **Errores**: Manejar errores con mensajes claros en español

---

## Próximos Pasos Recomendados

1. **Completar controladores faltantes** (CotizacionesController, ProductosController, etc.)
2. **Crear vistas parciales de formularios**
3. **Crear página principal Index.cshtml**
4. **Aplicar migraciones y probar con datos reales**
5. **Implementar autenticación y autorización**
6. **Agregar pruebas unitarias**
7. **Optimizar consultas LINQ**
8. **Implementar caché para datos estáticos**
9. **Agregar logging y monitoreo**
10. **Preparar para despliegue en producción**

