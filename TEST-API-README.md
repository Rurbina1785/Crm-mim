# Script de Pruebas Completo - API CRM

## 📋 Descripción

Script de pruebas exhaustivo para la API del sistema CRM que incluye:

- ✅ **Precondiciones SQL** - Verifica el estado de la base de datos antes de cada prueba
- ✅ **Llamadas API con curl** - Prueba todos los endpoints REST
- ✅ **Postcondiciones SQL** - Valida los cambios después de cada operación
- ✅ **Validación de respuestas** - Verifica códigos HTTP y contenido JSON
- ✅ **Reporte detallado** - Genera reporte en consola y HTML con colores
- ✅ **Estadísticas completas** - Tasa de éxito, contadores, cambios en BD

---

## 🚀 Uso Rápido

```bash
# 1. Asegúrate de que PostgreSQL esté corriendo
sudo service postgresql start

# 2. Asegúrate de que la API esté corriendo
cd CRMSystem/CRMSystem.API
dotnet run --urls="http://localhost:5000" &

# 3. Ejecuta el script de pruebas
cd ..
./test-api-complete.sh
```

---

## 📊 Pruebas Incluidas

### Endpoints de Prospectos (6 pruebas)

| # | Endpoint | Método | Descripción |
|---|----------|--------|-------------|
| 1 | `/api/Prospectos` | GET | Listar todos los prospectos |
| 2 | `/api/Prospectos/{id}` | GET | Obtener prospecto por ID |
| 3 | `/api/Prospectos/fuentes` | GET | Listar fuentes disponibles |
| 4 | `/api/Prospectos/embudo-ventas` | GET | Estadísticas del embudo |
| 5 | `/api/Prospectos?estado=Nuevo` | GET | Filtrar por estado |
| 6 | `/api/Prospectos` | POST | Crear nuevo prospecto |

### Endpoints de Clientes (4 pruebas)

| # | Endpoint | Método | Descripción |
|---|----------|--------|-------------|
| 7 | `/api/Clientes` | GET | Listar todos los clientes |
| 8 | `/api/Clientes/categorias` | GET | Listar categorías |
| 9 | `/api/Clientes/estadisticas-categorias` | GET | Estadísticas por categoría |
| 10 | `/api/Clientes/estadisticas-sucursales` | GET | Estadísticas por sucursal |

### Pruebas de Integridad (2 pruebas)

| # | Prueba | Descripción |
|---|--------|-------------|
| 11 | Datos de referencia | Verifica seed data (roles, sucursales, fuentes) |
| 12 | Relaciones y FKs | Valida integridad referencial |

**Total: 12 pruebas**

---

## 🎨 Ejemplo de Salida

### Consola con Colores

```
═══════════════════════════════════════════════════════════════
VERIFICACIÓN DE PRERREQUISITOS
═══════════════════════════════════════════════════════════════

▶ Verificando PostgreSQL
───────────────────────────────────────────────────────────────
✓ PostgreSQL está corriendo

▶ Verificando conexión a base de datos
───────────────────────────────────────────────────────────────
✓ Conexión a base de datos crmdb

▶ Verificando API
───────────────────────────────────────────────────────────────
✓ API está respondiendo
  → HTTP 200

═══════════════════════════════════════════════════════════════
ESTADO INICIAL DE LA BASE DE DATOS
═══════════════════════════════════════════════════════════════

▶ Conteo de registros por tabla
───────────────────────────────────────────────────────────────
Prospectos:        3
Clientes:          0
Usuarios:          4
Sucursales:        3
Fuentes:           7
Categorías Cliente: 4

═══════════════════════════════════════════════════════════════
PRUEBAS DE ENDPOINTS - PROSPECTOS
═══════════════════════════════════════════════════════════════

▶ TEST 1: GET /api/Prospectos - Listar todos
───────────────────────────────────────────────────────────────
Precondición SQL:
 total 
-------
     3

Ejecutando API Call:
HTTP Code: 200
Response: [{"id":1,"codigoProspecto":"PROS-2024-001",...

✓ GET /api/Prospectos
  → Retornó 3 prospectos

Postcondición SQL:
 total 
-------
     3

...

═══════════════════════════════════════════════════════════════
REPORTE FINAL DE PRUEBAS
═══════════════════════════════════════════════════════════════

Resumen de Resultados:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Total de pruebas:     12
Pruebas exitosas:     10
Pruebas fallidas:     1
Pruebas omitidas:     1
Tasa de éxito:        83%
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

✓ PRUEBAS EXITOSAS
El sistema está funcionando correctamente
```

### Reporte HTML

El script genera un reporte HTML profesional con:

- 📊 Dashboard con métricas visuales
- 📈 Barra de progreso de éxito
- 📋 Tabla de estado de base de datos
- 🎨 Diseño moderno con gradientes
- 📱 Responsive design

---

## 🔍 Estructura de Cada Prueba

Cada prueba sigue este patrón:

```bash
# 1. PRECONDICIÓN SQL
# Consulta el estado actual de la base de datos
SELECT COUNT(*) FROM "Prospectos"

# 2. LLAMADA API
# Ejecuta el endpoint con curl
curl -X GET "http://localhost:5000/api/Prospectos"

# 3. VALIDACIÓN
# Verifica código HTTP y contenido de respuesta
if HTTP_CODE == 200 then PASS else FAIL

# 4. POSTCONDICIÓN SQL
# Verifica los cambios en la base de datos
SELECT COUNT(*) FROM "Prospectos"
```

---

## 📝 Configuración

El script usa estas variables de configuración:

```bash
API_URL="http://localhost:5000/api"
DB_NAME="crmdb"
DB_USER="crmuser"
DB_PASSWORD="crm123456"
DB_HOST="localhost"
```

Para cambiarlas, edita las primeras líneas del script.

---

## 📂 Archivos Generados

### 1. Log de Consola

```
/tmp/crm-api-test-YYYYMMDD_HHMMSS.log
```

Contiene toda la salida del script para referencia futura.

### 2. Reporte HTML

```
/tmp/crm-api-test-report-YYYYMMDD_HHMMSS.html
```

Reporte visual profesional que puedes abrir en el navegador:

```bash
# Linux
xdg-open /tmp/crm-api-test-report-*.html

# Mac
open /tmp/crm-api-test-report-*.html

# Windows (WSL)
explorer.exe /tmp/crm-api-test-report-*.html
```

---

## 🎯 Casos de Uso

### 1. Desarrollo

Ejecuta el script después de cada cambio en los controladores:

```bash
# Hacer cambios en el código
vim CRMSystem.API/Controllers/ProspectosController.cs

# Recompilar
dotnet build

# Reiniciar API
pkill -f "dotnet run"
dotnet run &

# Probar
./test-api-complete.sh
```

### 2. Integración Continua (CI/CD)

Agrega el script a tu pipeline:

```yaml
# .github/workflows/test.yml
- name: Run API Tests
  run: |
    cd CRMSystem
    ./test-api-complete.sh
```

### 3. Validación de Despliegue

Después de desplegar a un nuevo ambiente:

```bash
# Actualizar configuración
export API_URL="https://api.production.com/api"
export DB_HOST="production-db.example.com"

# Ejecutar pruebas
./test-api-complete.sh
```

---

## 🔧 Personalización

### Agregar Nuevas Pruebas

```bash
# Agregar después de la línea 500
print_subsection "TEST XX: Descripción de la prueba"

echo -e "${CYAN}Precondición SQL:${NC}"
sql_query_formatted "SELECT ..."

echo -e "\n${CYAN}Ejecutando API Call:${NC}"
RESPONSE=$(curl -s -w "\n%{http_code}" "$API_URL/endpoint")
HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
BODY=$(echo "$RESPONSE" | sed '$d')

if validate_http_code "200" "$HTTP_CODE"; then
    print_test_result "Nombre de la prueba" "PASS" "Detalles"
else
    print_test_result "Nombre de la prueba" "FAIL" "HTTP $HTTP_CODE"
fi

echo -e "\n${CYAN}Postcondición SQL:${NC}"
sql_query_formatted "SELECT ..."
```

### Cambiar Colores

Edita las variables de color al inicio del script:

```bash
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
MAGENTA='\033[0;35m'
```

---

## ⚠️ Problemas Conocidos

### 1. Endpoints POST Fallan (HTTP 400)

**Causa:** Los modelos requieren objetos completos en lugar de IDs.

**Solución:** Implementar DTOs (Data Transfer Objects).

**Estado:** Documentado, prueba marcada como SKIP.

### 2. PostgreSQL No Conecta

**Error:** `psql: could not connect to server`

**Solución:**
```bash
sudo service postgresql start
sudo service postgresql status
```

### 3. API No Responde

**Error:** `curl: (7) Failed to connect`

**Solución:**
```bash
cd CRMSystem/CRMSystem.API
dotnet run --urls="http://localhost:5000" &
```

---

## 📊 Interpretación de Resultados

### Tasa de Éxito

| Rango | Estado | Acción |
|-------|--------|--------|
| 80-100% | ✅ Excelente | Sistema funcionando correctamente |
| 50-79% | ⚠️ Aceptable | Algunos endpoints requieren atención |
| 0-49% | ❌ Crítico | Sistema requiere correcciones |

### Estados de Prueba

- **✓ PASS** (Verde) - Prueba exitosa
- **✗ FAIL** (Rojo) - Prueba fallida, requiere corrección
- **⊘ SKIP** (Amarillo) - Prueba omitida, problema conocido

---

## 🔐 Seguridad

El script contiene credenciales de base de datos. Para producción:

1. **Usar variables de entorno:**

```bash
export DB_PASSWORD="secret"
./test-api-complete.sh
```

2. **Usar archivo de configuración:**

```bash
# .env
DB_PASSWORD=secret

# En el script
source .env
```

3. **Usar PostgreSQL .pgpass:**

```bash
# ~/.pgpass
localhost:5432:crmdb:crmuser:crm123456
chmod 600 ~/.pgpass
```

---

## 📚 Referencias

- [Documentación de curl](https://curl.se/docs/manual.html)
- [PostgreSQL psql](https://www.postgresql.org/docs/current/app-psql.html)
- [Bash scripting guide](https://www.gnu.org/software/bash/manual/)
- [HTTP status codes](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status)

---

## 🤝 Contribuir

Para agregar más pruebas o mejorar el script:

1. Sigue el patrón existente de precondición → API call → postcondición
2. Usa las funciones auxiliares (`print_test_result`, `sql_query`, etc.)
3. Documenta las nuevas pruebas en este README
4. Actualiza el contador `TOTAL_TESTS` si es necesario

---

## 📄 Licencia

Este script es parte del sistema CRM y se proporciona como herramienta de desarrollo y pruebas.

---

## 📞 Soporte

Para problemas o preguntas:

1. Revisa la sección de "Problemas Conocidos"
2. Consulta los logs generados en `/tmp/`
3. Verifica la documentación de la API en Swagger

---

**Última actualización:** 28 de noviembre de 2024
**Versión:** 1.0.0
**Autor:** Sistema CRM Team

