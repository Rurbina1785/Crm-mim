using Microsoft.EntityFrameworkCore;
using CRMSystem.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Add Razor Pages support for HTMX integration
builder.Services.AddRazorPages();

// Add Entity Framework - Using PostgreSQL database
builder.Services.AddDbContext<ContextoBDCRM>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Swagger/OpenAPI with Spanish documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Sistema CRM - API",
        Version = "v1",
        Description = "API RESTful para el Sistema de Gestión de Relaciones con Clientes (CRM) con soporte completo para prospectos, clientes, cotizaciones, visitas y análisis de ventas.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Equipo de Desarrollo CRM",
            Email = "soporte@crm.com"
        }
    });
    
    // Include XML comments for Swagger documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API v1");
        options.DocumentTitle = "Sistema CRM - Documentación API";
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

// Serve static files for assets (CSS, JS, images)
app.UseStaticFiles();

// Map controllers for API endpoints
app.MapControllers();

// Map Razor Pages for HTMX integration
app.MapRazorPages();

// Initialize PostgreSQL database with migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ContextoBDCRM>();
    try
    {
        // Only apply migrations if there are pending ones
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
            Console.WriteLine("Migraciones aplicadas correctamente.");
        }
        else
        {
            Console.WriteLine("Base de datos PostgreSQL ya está actualizada.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
    }
}

app.Run("http://localhost:5000");

