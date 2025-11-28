#!/bin/bash

################################################################################
# SCRIPT DE PRUEBAS MEJORADO - API CRM CON POSTGRESQL
################################################################################
# Este script prueba todos los endpoints de la API con:
# - Precondiciones SQL con salida completa de psql
# - Llamadas API con curl y respuestas completas
# - Postcondiciones SQL con salida completa de psql
# - Validación de respuestas JSON
# - Reporte detallado con colores y formato de tabla
################################################################################

# Configuración
API_URL="http://localhost:5000/api"
DB_NAME="crmdb"
DB_USER="crmuser"
DB_PASSWORD="crm123456"
DB_HOST="localhost"

# Colores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
MAGENTA='\033[0;35m'
WHITE='\033[1;37m'
NC='\033[0m' # No Color
BOLD='\033[1m'
DIM='\033[2m'

# Contadores
TOTAL_TESTS=0
PASSED_TESTS=0
FAILED_TESTS=0
SKIPPED_TESTS=0

# Timestamp para archivos
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
LOG_FILE="/tmp/crm-api-test-${TIMESTAMP}.log"
REPORT_FILE="/tmp/crm-api-test-report-${TIMESTAMP}.html"

################################################################################
# FUNCIONES AUXILIARES
################################################################################

# Función para ejecutar consultas SQL con salida completa
function sql_query_with_output() {
    local query="$1"
    local title="$2"
    
    echo -e "${CYAN}${BOLD}📊 $title${NC}"
    echo -e "${DIM}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${WHITE}SQL:${NC} ${DIM}$query${NC}"
    echo -e "${DIM}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    
    # Ejecutar query y capturar salida
    local output=$(PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "$query" 2>&1)
    local exit_code=$?
    
    if [ $exit_code -eq 0 ]; then
        echo -e "${output}"
    else
        echo -e "${RED}Error ejecutando SQL:${NC}"
        echo -e "${output}"
    fi
    
    echo -e "${DIM}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}\n"
}

# Función para ejecutar API call con salida completa
function api_call_with_output() {
    local method="$1"
    local endpoint="$2"
    local data="$3"
    local title="$4"
    
    echo -e "${CYAN}${BOLD}🌐 $title${NC}"
    echo -e "${DIM}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${WHITE}Método:${NC} $method"
    echo -e "${WHITE}URL:${NC} ${API_URL}${endpoint}"
    
    if [ -n "$data" ]; then
        echo -e "${WHITE}Payload:${NC}"
        echo "$data" | jq '.' 2>/dev/null || echo "$data"
    fi
    
    echo -e "${DIM}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    
    # Ejecutar API call
    if [ "$method" == "GET" ]; then
        RESPONSE=$(curl -s -w "\n%{http_code}" "${API_URL}${endpoint}")
    elif [ "$method" == "POST" ]; then
        RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "${API_URL}${endpoint}" \
            -H "Content-Type: application/json" \
            -d "$data")
    fi
    
    HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    
    echo -e "${WHITE}HTTP Status:${NC} ${GREEN}${HTTP_CODE}${NC}"
    echo -e "${WHITE}Response:${NC}"
    echo "$BODY" | jq '.' 2>/dev/null || echo "$BODY"
    echo -e "${DIM}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}\n"
    
    # Retornar código HTTP
    echo "$HTTP_CODE"
}

# Función para imprimir encabezado de sección
function print_section() {
    echo -e "\n${BOLD}${BLUE}╔═══════════════════════════════════════════════════════════════╗${NC}"
    echo -e "${BOLD}${BLUE}║${NC} ${BOLD}${WHITE}$1${NC}"
    echo -e "${BOLD}${BLUE}╚═══════════════════════════════════════════════════════════════╝${NC}\n"
}

# Función para imprimir subsección de prueba
function print_test_header() {
    local test_num="$1"
    local test_name="$2"
    
    echo -e "\n${BOLD}${MAGENTA}┌─────────────────────────────────────────────────────────────────┐${NC}"
    echo -e "${BOLD}${MAGENTA}│${NC} ${BOLD}${WHITE}TEST #$test_num: $test_name${NC}"
    echo -e "${BOLD}${MAGENTA}└─────────────────────────────────────────────────────────────────┘${NC}\n"
}

# Función para imprimir resultado de prueba
function print_test_result() {
    local test_name="$1"
    local status="$2"
    local details="$3"
    
    TOTAL_TESTS=$((TOTAL_TESTS + 1))
    
    echo -e "\n${BOLD}${WHITE}Resultado:${NC}"
    
    if [ "$status" == "PASS" ]; then
        echo -e "${GREEN}${BOLD}✓ EXITOSO${NC} - $test_name"
        [ -n "$details" ] && echo -e "${GREEN}  └─→${NC} $details"
        PASSED_TESTS=$((PASSED_TESTS + 1))
    elif [ "$status" == "FAIL" ]; then
        echo -e "${RED}${BOLD}✗ FALLIDO${NC} - $test_name"
        [ -n "$details" ] && echo -e "${RED}  └─→${NC} $details"
        FAILED_TESTS=$((FAILED_TESTS + 1))
    elif [ "$status" == "SKIP" ]; then
        echo -e "${YELLOW}${BOLD}⊘ OMITIDO${NC} - $test_name"
        [ -n "$details" ] && echo -e "${YELLOW}  └─→${NC} $details"
        SKIPPED_TESTS=$((SKIPPED_TESTS + 1))
    fi
    
    echo -e "${MAGENTA}═══════════════════════════════════════════════════════════════${NC}\n"
}

################################################################################
# INICIO DEL SCRIPT
################################################################################

clear
print_section "PRUEBAS COMPLETAS DE API CRM - $(date '+%Y-%m-%d %H:%M:%S')"

# Verificar prerrequisitos
echo -e "${CYAN}Verificando prerrequisitos...${NC}\n"

# PostgreSQL
if sudo service postgresql status > /dev/null 2>&1; then
    echo -e "${GREEN}✓${NC} PostgreSQL está corriendo"
else
    echo -e "${RED}✗${NC} PostgreSQL no está corriendo"
    exit 1
fi

# Base de datos
if PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "SELECT 1" > /dev/null 2>&1; then
    echo -e "${GREEN}✓${NC} Conexión a base de datos exitosa"
else
    echo -e "${RED}✗${NC} No se puede conectar a la base de datos"
    exit 1
fi

# API
if curl -s http://localhost:5000/api/Prospectos > /dev/null 2>&1; then
    echo -e "${GREEN}✓${NC} API está respondiendo"
else
    echo -e "${RED}✗${NC} API no está respondiendo"
    exit 1
fi

################################################################################
# ESTADO INICIAL
################################################################################

print_section "ESTADO INICIAL DE LA BASE DE DATOS"

sql_query_with_output "SELECT COUNT(*) as total_prospectos FROM \"Prospectos\"" "Total de Prospectos"
sql_query_with_output "SELECT COUNT(*) as total_clientes FROM \"Clientes\"" "Total de Clientes"
sql_query_with_output "SELECT COUNT(*) as total_usuarios FROM \"Usuarios\"" "Total de Usuarios"
sql_query_with_output "SELECT COUNT(*) as total_sucursales FROM \"Sucursales\"" "Total de Sucursales"

################################################################################
# TEST 1: GET /api/Prospectos
################################################################################

print_section "PRUEBAS DE ENDPOINTS"
print_test_header "1" "GET /api/Prospectos - Listar todos los prospectos"

# Precondición
sql_query_with_output "SELECT \"Id\", \"CodigoProspecto\", \"NombreEmpresa\", \"EstadoProspecto\" FROM \"Prospectos\" ORDER BY \"Id\"" "PRECONDICIÓN: Prospectos existentes"

# API Call
HTTP_CODE=$(api_call_with_output "GET" "/Prospectos" "" "Llamada API")

# Postcondición
sql_query_with_output "SELECT COUNT(*) as total FROM \"Prospectos\"" "POSTCONDICIÓN: Verificar que no cambió el total"

# Validación
if [ "$HTTP_CODE" == "200" ]; then
    print_test_result "GET /api/Prospectos" "PASS" "Retornó lista de prospectos correctamente"
else
    print_test_result "GET /api/Prospectos" "FAIL" "HTTP $HTTP_CODE (esperado 200)"
fi

################################################################################
# TEST 2: GET /api/Prospectos/{id}
################################################################################

print_test_header "2" "GET /api/Prospectos/1 - Obtener prospecto por ID"

# Precondición
sql_query_with_output "SELECT \"Id\", \"CodigoProspecto\", \"NombreEmpresa\", \"NombreContacto\", \"Email\" FROM \"Prospectos\" WHERE \"Id\" = 1" "PRECONDICIÓN: Datos del prospecto ID 1"

# API Call
HTTP_CODE=$(api_call_with_output "GET" "/Prospectos/1" "" "Llamada API")

# Postcondición
sql_query_with_output "SELECT \"Id\", \"CodigoProspecto\", \"FechaActualizacion\" FROM \"Prospectos\" WHERE \"Id\" = 1" "POSTCONDICIÓN: Verificar que no cambió"

# Validación
if [ "$HTTP_CODE" == "200" ]; then
    print_test_result "GET /api/Prospectos/1" "PASS" "Retornó prospecto correctamente"
else
    print_test_result "GET /api/Prospectos/1" "FAIL" "HTTP $HTTP_CODE (esperado 200)"
fi

################################################################################
# TEST 3: POST /api/Prospectos
################################################################################

print_test_header "3" "POST /api/Prospectos - Crear nuevo prospecto"

# Precondición
sql_query_with_output "SELECT COUNT(*) as total_antes FROM \"Prospectos\"" "PRECONDICIÓN: Total de prospectos antes de crear"
sql_query_with_output "SELECT \"Id\", \"CodigoProspecto\" FROM \"Prospectos\" ORDER BY \"Id\" DESC LIMIT 3" "PRECONDICIÓN: Últimos 3 prospectos"

# Preparar datos
POST_DATA='{
  "nombreEmpresa": "Test Company Enhanced",
  "nombreContacto": "Carlos",
  "apellidoContacto": "Test",
  "email": "carlos.test@enhanced.com",
  "telefono": "+52-55-9999-8888",
  "fuenteId": 2,
  "sucursalId": 2,
  "vendedorAsignadoId": 2,
  "estadoProspecto": "Nuevo",
  "prioridad": "Media",
  "valorEstimado": 75000,
  "probabilidadCierre": 70
}'

# API Call
HTTP_CODE=$(api_call_with_output "POST" "/Prospectos" "$POST_DATA" "Llamada API")

# Postcondición
sql_query_with_output "SELECT COUNT(*) as total_despues FROM \"Prospectos\"" "POSTCONDICIÓN: Total de prospectos después de crear"
sql_query_with_output "SELECT \"Id\", \"CodigoProspecto\", \"NombreEmpresa\", \"Email\", \"ValorEstimado\" FROM \"Prospectos\" ORDER BY \"Id\" DESC LIMIT 1" "POSTCONDICIÓN: Prospecto recién creado"

# Validación
if [ "$HTTP_CODE" == "201" ]; then
    print_test_result "POST /api/Prospectos" "PASS" "Prospecto creado exitosamente (HTTP 201)"
else
    print_test_result "POST /api/Prospectos" "FAIL" "HTTP $HTTP_CODE (esperado 201)"
fi

################################################################################
# TEST 4: GET /api/Prospectos/fuentes
################################################################################

print_test_header "4" "GET /api/Prospectos/fuentes - Listar fuentes disponibles"

# Precondición
sql_query_with_output "SELECT \"Id\", \"NombreFuente\", \"Descripcion\" FROM \"FuentesProspecto\" ORDER BY \"Id\"" "PRECONDICIÓN: Fuentes en base de datos"

# API Call
HTTP_CODE=$(api_call_with_output "GET" "/Prospectos/fuentes" "" "Llamada API")

# Postcondición
sql_query_with_output "SELECT COUNT(*) as total FROM \"FuentesProspecto\"" "POSTCONDICIÓN: Verificar que no cambió el total"

# Validación
if [ "$HTTP_CODE" == "200" ]; then
    print_test_result "GET /api/Prospectos/fuentes" "PASS" "Retornó lista de fuentes"
else
    print_test_result "GET /api/Prospectos/fuentes" "FAIL" "HTTP $HTTP_CODE (esperado 200)"
fi

################################################################################
# TEST 5: GET /api/Clientes/categorias
################################################################################

print_test_header "5" "GET /api/Clientes/categorias - Listar categorías de clientes"

# Precondición
sql_query_with_output "SELECT \"Id\", \"NombreCategoria\", \"PorcentajeDescuento\" FROM \"CategoriasCliente\" ORDER BY \"PorcentajeDescuento\" DESC" "PRECONDICIÓN: Categorías en base de datos"

# API Call
HTTP_CODE=$(api_call_with_output "GET" "/Clientes/categorias" "" "Llamada API")

# Postcondición
sql_query_with_output "SELECT COUNT(*) as total FROM \"CategoriasCliente\"" "POSTCONDICIÓN: Verificar que no cambió el total"

# Validación
if [ "$HTTP_CODE" == "200" ]; then
    print_test_result "GET /api/Clientes/categorias" "PASS" "Retornó lista de categorías"
else
    print_test_result "GET /api/Clientes/categorias" "FAIL" "HTTP $HTTP_CODE (esperado 200)"
fi

################################################################################
# ESTADO FINAL
################################################################################

print_section "ESTADO FINAL DE LA BASE DE DATOS"

sql_query_with_output "SELECT 
    (SELECT COUNT(*) FROM \"Prospectos\") as prospectos,
    (SELECT COUNT(*) FROM \"Clientes\") as clientes,
    (SELECT COUNT(*) FROM \"Usuarios\") as usuarios,
    (SELECT COUNT(*) FROM \"Sucursales\") as sucursales,
    (SELECT COUNT(*) FROM \"FuentesProspecto\") as fuentes,
    (SELECT COUNT(*) FROM \"CategoriasCliente\") as categorias" "Resumen de registros por tabla"

sql_query_with_output "SELECT \"EstadoProspecto\", COUNT(*) as cantidad, SUM(\"ValorEstimado\") as valor_total 
FROM \"Prospectos\" 
GROUP BY \"EstadoProspecto\" 
ORDER BY cantidad DESC" "Prospectos por estado"

################################################################################
# REPORTE FINAL
################################################################################

print_section "REPORTE FINAL"

SUCCESS_RATE=$((PASSED_TESTS * 100 / TOTAL_TESTS))

echo -e "${BOLD}${WHITE}Resumen de Pruebas:${NC}"
echo -e "${DIM}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${WHITE}Total de pruebas:${NC}     ${BOLD}$TOTAL_TESTS${NC}"
echo -e "${GREEN}Pruebas exitosas:${NC}     ${BOLD}$PASSED_TESTS${NC}"
echo -e "${RED}Pruebas fallidas:${NC}     ${BOLD}$FAILED_TESTS${NC}"
echo -e "${YELLOW}Pruebas omitidas:${NC}     ${BOLD}$SKIPPED_TESTS${NC}"
echo -e "${CYAN}Tasa de éxito:${NC}        ${BOLD}${SUCCESS_RATE}%${NC}"
echo -e "${DIM}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}\n"

if [ $FAILED_TESTS -eq 0 ]; then
    echo -e "${GREEN}${BOLD}✓ TODAS LAS PRUEBAS EXITOSAS${NC}"
    echo -e "${GREEN}El sistema está funcionando correctamente${NC}\n"
else
    echo -e "${RED}${BOLD}✗ ALGUNAS PRUEBAS FALLARON${NC}"
    echo -e "${RED}Revisa los detalles arriba para más información${NC}\n"
fi

echo -e "${CYAN}Fecha de ejecución:${NC} $(date '+%Y-%m-%d %H:%M:%S')"
echo -e "${CYAN}Archivo de log:${NC} Este output completo"
echo -e "\n${BOLD}${BLUE}╔═══════════════════════════════════════════════════════════════╗${NC}"
echo -e "${BOLD}${BLUE}║${NC} ${BOLD}${GREEN}✓ PRUEBAS COMPLETADAS${NC}"
echo -e "${BOLD}${BLUE}╚═══════════════════════════════════════════════════════════════╝${NC}\n"
