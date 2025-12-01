#!/bin/bash

# Script de pruebas con salida visual completa de SQL
API_URL="http://localhost:5000/api"
DB_NAME="crmdb"
DB_USER="crmuser"
DB_PASSWORD="crm123456"
DB_HOST="localhost"

# Colores
GREEN='\033[0;32m'
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
WHITE='\033[1;37m'
NC='\033[0m'
BOLD='\033[1m'

PASSED=0
FAILED=0
TOTAL=0

function test_endpoint() {
    local test_num="$1"
    local method="$2"
    local endpoint="$3"
    local data="$4"
    local expected_code="$5"
    local description="$6"
    local pre_sql="$7"
    local post_sql="$8"
    
    TOTAL=$((TOTAL + 1))
    
    echo -e "\n${BOLD}${CYAN}═══════════════════════════════════════════════════════════════${NC}"
    echo -e "${BOLD}${WHITE}TEST #$test_num: $description${NC}"
    echo -e "${BOLD}${CYAN}═══════════════════════════════════════════════════════════════${NC}\n"
    
    # Precondición SQL
    if [ -n "$pre_sql" ]; then
        echo -e "${CYAN}📊 PRECONDICIÓN SQL:${NC}"
        echo -e "${YELLOW}$pre_sql${NC}"
        PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "$pre_sql"
        echo ""
    fi
    
    # Llamada API
    echo -e "${CYAN}🌐 LLAMADA API:${NC}"
    echo -e "${WHITE}$method ${API_URL}${endpoint}${NC}"
    
    if [ -n "$data" ]; then
        echo -e "${YELLOW}Payload:${NC}"
        echo "$data" | jq '.'
    fi
    
    if [ "$method" == "GET" ]; then
        RESPONSE=$(curl -s -w "\n%{http_code}" "${API_URL}${endpoint}")
    else
        RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "${API_URL}${endpoint}" \
            -H "Content-Type: application/json" -d "$data")
    fi
    
    HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
    BODY=$(echo "$RESPONSE" | sed '$d')
    
    echo -e "${WHITE}HTTP Status: ${HTTP_CODE}${NC}"
    echo -e "${WHITE}Response:${NC}"
    echo "$BODY" | jq '.' 2>/dev/null | head -20
    echo ""
    
    # Postcondición SQL
    if [ -n "$post_sql" ]; then
        echo -e "${CYAN}📊 POSTCONDICIÓN SQL:${NC}"
        echo -e "${YELLOW}$post_sql${NC}"
        PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "$post_sql"
        echo ""
    fi
    
    # Resultado
    if [ "$HTTP_CODE" == "$expected_code" ]; then
        echo -e "${GREEN}✓ EXITOSO${NC} - $description"
        PASSED=$((PASSED + 1))
    else
        echo -e "\033[0;31m✗ FALLIDO${NC} - Esperado $expected_code, obtenido $HTTP_CODE"
        FAILED=$((FAILED + 1))
    fi
}

clear
echo -e "${BOLD}${CYAN}╔═══════════════════════════════════════════════════════════════╗${NC}"
echo -e "${BOLD}${CYAN}║  PRUEBAS API CRM CON PRECONDICIONES/POSTCONDICIONES SQL      ║${NC}"
echo -e "${BOLD}${CYAN}╚═══════════════════════════════════════════════════════════════╝${NC}\n"

# TEST 1
test_endpoint "1" "GET" "/Prospectos" "" "200" \
    "Listar todos los prospectos" \
    "SELECT \"Id\", \"CodigoProspecto\", \"NombreEmpresa\", \"EstadoProspecto\" FROM \"Prospectos\" ORDER BY \"Id\";" \
    "SELECT COUNT(*) as total_prospectos FROM \"Prospectos\";"

# TEST 2
test_endpoint "2" "GET" "/Prospectos/1" "" "200" \
    "Obtener prospecto por ID" \
    "SELECT \"Id\", \"CodigoProspecto\", \"NombreEmpresa\", \"Email\" FROM \"Prospectos\" WHERE \"Id\" = 1;" \
    "SELECT \"FechaActualizacion\" FROM \"Prospectos\" WHERE \"Id\" = 1;"

# TEST 3
POST_DATA='{
  "nombreEmpresa": "Test Visual Script",
  "nombreContacto": "Pedro",
  "apellidoContacto": "Visual",
  "email": "pedro.visual@test.com",
  "telefono": "+52-55-7777-6666",
  "fuenteId": 3,
  "sucursalId": 3,
  "vendedorAsignadoId": 1,
  "estadoProspecto": "Nuevo",
  "prioridad": "Alta",
  "valorEstimado": 100000,
  "probabilidadCierre": 80
}'

test_endpoint "3" "POST" "/Prospectos" "$POST_DATA" "201" \
    "Crear nuevo prospecto" \
    "SELECT COUNT(*) as total_antes, MAX(\"Id\") as ultimo_id FROM \"Prospectos\";" \
    "SELECT \"Id\", \"CodigoProspecto\", \"NombreEmpresa\", \"ValorEstimado\" FROM \"Prospectos\" ORDER BY \"Id\" DESC LIMIT 1;"

# TEST 4
test_endpoint "4" "GET" "/Prospectos/fuentes" "" "200" \
    "Listar fuentes de prospectos" \
    "SELECT \"Id\", \"NombreFuente\" FROM \"FuentesProspecto\" ORDER BY \"Id\";" \
    "SELECT COUNT(*) as total_fuentes FROM \"FuentesProspecto\";"

# TEST 5
test_endpoint "5" "GET" "/Clientes/categorias" "" "200" \
    "Listar categorías de clientes" \
    "SELECT \"Id\", \"NombreCategoria\", \"PorcentajeDescuento\" FROM \"CategoriasCliente\" ORDER BY \"PorcentajeDescuento\" DESC;" \
    "SELECT COUNT(*) as total_categorias FROM \"CategoriasCliente\";"

# Reporte final
echo -e "\n${BOLD}${CYAN}╔═══════════════════════════════════════════════════════════════╗${NC}"
echo -e "${BOLD}${CYAN}║  REPORTE FINAL                                                ║${NC}"
echo -e "${BOLD}${CYAN}╚═══════════════════════════════════════════════════════════════╝${NC}\n"

SUCCESS_RATE=$((PASSED * 100 / TOTAL))

echo -e "${WHITE}Total de pruebas:${NC}     $TOTAL"
echo -e "${GREEN}Pruebas exitosas:${NC}     $PASSED"
echo -e "\033[0;31mPruebas fallidas:${NC}     $FAILED"
echo -e "${CYAN}Tasa de éxito:${NC}        ${SUCCESS_RATE}%"

if [ $FAILED -eq 0 ]; then
    echo -e "\n${GREEN}${BOLD}✓ TODAS LAS PRUEBAS EXITOSAS${NC}\n"
else
    echo -e "\n\033[0;31m${BOLD}✗ ALGUNAS PRUEBAS FALLARON${NC}\n"
fi
