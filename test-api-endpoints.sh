#!/bin/bash

# Script para probar todos los endpoints de la API CRM
# Autor: Sistema CRM
# Fecha: 27 de noviembre de 2024

BASE_URL="http://localhost:5000/api"
RESULTS_FILE="/tmp/api-test-results.txt"

echo "========================================" > $RESULTS_FILE
echo "Pruebas de Endpoints API - Sistema CRM" >> $RESULTS_FILE
echo "Fecha: $(date)" >> $RESULTS_FILE
echo "========================================" >> $RESULTS_FILE
echo "" >> $RESULTS_FILE

# Función para probar un endpoint
test_endpoint() {
    local method=$1
    local endpoint=$2
    local description=$3
    local data=$4
    
    echo "-----------------------------------" >> $RESULTS_FILE
    echo "Prueba: $description" >> $RESULTS_FILE
    echo "Método: $method" >> $RESULTS_FILE
    echo "Endpoint: $endpoint" >> $RESULTS_FILE
    echo "-----------------------------------" >> $RESULTS_FILE
    
    if [ "$method" == "GET" ]; then
        response=$(curl -s -w "\nHTTP_CODE:%{http_code}" -X GET "$BASE_URL$endpoint" -H "accept: application/json")
    elif [ "$method" == "POST" ]; then
        response=$(curl -s -w "\nHTTP_CODE:%{http_code}" -X POST "$BASE_URL$endpoint" -H "accept: application/json" -H "Content-Type: application/json" -d "$data")
    elif [ "$method" == "PUT" ]; then
        response=$(curl -s -w "\nHTTP_CODE:%{http_code}" -X PUT "$BASE_URL$endpoint" -H "accept: application/json" -H "Content-Type: application/json" -d "$data")
    elif [ "$method" == "DELETE" ]; then
        response=$(curl -s -w "\nHTTP_CODE:%{http_code}" -X DELETE "$BASE_URL$endpoint" -H "accept: application/json")
    fi
    
    http_code=$(echo "$response" | grep "HTTP_CODE" | cut -d: -f2)
    body=$(echo "$response" | sed '/HTTP_CODE/d')
    
    echo "Código HTTP: $http_code" >> $RESULTS_FILE
    
    if [ $http_code -ge 200 ] && [ $http_code -lt 300 ]; then
        echo "Estado: ✅ ÉXITO" >> $RESULTS_FILE
    else
        echo "Estado: ❌ ERROR" >> $RESULTS_FILE
    fi
    
    echo "Respuesta:" >> $RESULTS_FILE
    echo "$body" | head -20 >> $RESULTS_FILE
    echo "" >> $RESULTS_FILE
}

echo "Iniciando pruebas de endpoints..."

# ========================================
# PROSPECTOS CONTROLLER
# ========================================

echo "" >> $RESULTS_FILE
echo "========================================" >> $RESULTS_FILE
echo "CONTROLADOR: PROSPECTOS" >> $RESULTS_FILE
echo "========================================" >> $RESULTS_FILE
echo "" >> $RESULTS_FILE

# 1. GET /api/Prospectos - Obtener lista
test_endpoint "GET" "/Prospectos" "Obtener lista de prospectos"

# 2. GET /api/Prospectos con filtros
test_endpoint "GET" "/Prospectos?estado=Nuevo&pagina=1&tamañoPagina=5" "Obtener prospectos con filtro de estado"

# 3. GET /api/Prospectos/{id} - Obtener por ID
test_endpoint "GET" "/Prospectos/1" "Obtener prospecto por ID"

# 4. POST /api/Prospectos - Crear prospecto
nuevo_prospecto='{
  "nombreEmpresa": "Empresa Test API",
  "nombreContacto": "Juan",
  "apellidoContacto": "Prueba",
  "email": "juan.prueba@test.com",
  "telefono": "+52-55-1234-5678",
  "fuenteId": 1,
  "estadoProspecto": "Nuevo",
  "prioridad": "Alta",
  "valorEstimado": 50000,
  "probabilidadCierre": 70,
  "vendedorAsignadoId": 1,
  "sucursalId": 1
}'
test_endpoint "POST" "/Prospectos" "Crear nuevo prospecto" "$nuevo_prospecto"

# 5. GET /api/Prospectos/fuentes - Obtener fuentes
test_endpoint "GET" "/Prospectos/fuentes" "Obtener fuentes de prospectos"

# 6. GET /api/Prospectos/embudo-ventas - Estadísticas
test_endpoint "GET" "/Prospectos/embudo-ventas" "Obtener embudo de ventas"

# ========================================
# CLIENTES CONTROLLER
# ========================================

echo "" >> $RESULTS_FILE
echo "========================================" >> $RESULTS_FILE
echo "CONTROLADOR: CLIENTES" >> $RESULTS_FILE
echo "========================================" >> $RESULTS_FILE
echo "" >> $RESULTS_FILE

# 1. GET /api/Clientes - Obtener lista
test_endpoint "GET" "/Clientes" "Obtener lista de clientes"

# 2. GET /api/Clientes con filtros
test_endpoint "GET" "/Clientes?estado=Activo&pagina=1&tamañoPagina=5" "Obtener clientes con filtro de estado"

# 3. GET /api/Clientes/{id} - Obtener por ID
test_endpoint "GET" "/Clientes/1" "Obtener cliente por ID"

# 4. POST /api/Clientes - Crear cliente
nuevo_cliente='{
  "nombreEmpresa": "Cliente Test API",
  "rfc": "TST123456ABC",
  "industria": "Tecnología",
  "telefono": "+52-55-9876-5432",
  "email": "contacto@clientetest.com",
  "direccion": "Calle Test 123",
  "ciudad": "Ciudad de México",
  "estado": "CDMX",
  "codigoPostal": "01000",
  "pais": "México",
  "categoriaId": 1,
  "vendedorAsignadoId": 1,
  "sucursalId": 1,
  "estadoCliente": "Activo"
}'
test_endpoint "POST" "/Clientes" "Crear nuevo cliente" "$nuevo_cliente"

# 5. GET /api/Clientes/categorias - Obtener categorías
test_endpoint "GET" "/Clientes/categorias" "Obtener categorías de clientes"

# 6. GET /api/Clientes/estadisticas-categorias - Estadísticas por categoría
test_endpoint "GET" "/Clientes/estadisticas-categorias" "Obtener estadísticas por categoría"

# 7. GET /api/Clientes/estadisticas-sucursales - Estadísticas por sucursal
test_endpoint "GET" "/Clientes/estadisticas-sucursales" "Obtener estadísticas por sucursal"

# ========================================
# RESUMEN
# ========================================

echo "" >> $RESULTS_FILE
echo "========================================" >> $RESULTS_FILE
echo "RESUMEN DE PRUEBAS" >> $RESULTS_FILE
echo "========================================" >> $RESULTS_FILE
echo "" >> $RESULTS_FILE

total_tests=$(grep -c "Prueba:" $RESULTS_FILE)
successful_tests=$(grep -c "✅ ÉXITO" $RESULTS_FILE)
failed_tests=$(grep -c "❌ ERROR" $RESULTS_FILE)

echo "Total de pruebas: $total_tests" >> $RESULTS_FILE
echo "Pruebas exitosas: $successful_tests" >> $RESULTS_FILE
echo "Pruebas fallidas: $failed_tests" >> $RESULTS_FILE
echo "" >> $RESULTS_FILE

if [ $failed_tests -eq 0 ]; then
    echo "✅ TODAS LAS PRUEBAS PASARON EXITOSAMENTE" >> $RESULTS_FILE
else
    echo "⚠️  ALGUNAS PRUEBAS FALLARON" >> $RESULTS_FILE
fi

echo "" >> $RESULTS_FILE
echo "Resultados guardados en: $RESULTS_FILE" >> $RESULTS_FILE

# Mostrar resultados
cat $RESULTS_FILE

echo ""
echo "========================================="
echo "Pruebas completadas. Resultados guardados en:"
echo "$RESULTS_FILE"
echo "========================================="

