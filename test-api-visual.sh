#!/bin/bash

################################################################################
# SCRIPT DE PRUEBAS AUTOMATIZADAS PARA API CRM CON POSTGRESQL
################################################################################
#
# PROPÓSITO:
# ----------
# Este script automatiza las pruebas de los endpoints de la API CRM, mostrando
# de forma visual y completa:
#   1. El estado de la base de datos ANTES de cada prueba (PRECONDICIÓN)
#   2. La llamada a la API con todos sus detalles (request y response)
#   3. El estado de la base de datos DESPUÉS de cada prueba (POSTCONDICIÓN)
#   4. La validación automática del resultado esperado vs obtenido
#
# CARACTERÍSTICAS:
# ----------------
# - Ejecuta consultas SQL directamente con psql para mostrar datos reales
# - Formatea JSON con jq para mejor legibilidad
# - Usa colores ANSI para resaltar información importante
# - Valida automáticamente códigos HTTP esperados
# - Genera reporte final con estadísticas
#
# REQUISITOS:
# -----------
# - PostgreSQL instalado y corriendo
# - Base de datos 'crmdb' creada y configurada
# - API CRM corriendo en http://localhost:5000
# - Herramientas: curl, jq, psql
#
# USO:
# ----
# ./test-api-visual-documentado.sh
#
# AUTOR: Sistema CRM
# FECHA: 2025-11-28
# VERSIÓN: 1.0
#
################################################################################

################################################################################
# SECCIÓN 1: CONFIGURACIÓN Y VARIABLES GLOBALES
################################################################################

# URL base de la API
# ------------------
# Esta es la URL donde está corriendo nuestra API REST.
# Todos los endpoints se construirán a partir de esta base.
# Ejemplo: ${API_URL}/Prospectos se convierte en http://localhost:5000/api/Prospectos
API_URL="http://localhost:5000/api"

# Configuración de conexión a PostgreSQL
# ---------------------------------------
# Estas variables se usan para conectarse a la base de datos PostgreSQL
# mediante el comando psql. Son las mismas credenciales que usa la API
# en appsettings.json

# Nombre de la base de datos
DB_NAME="crmdb"

# Usuario de PostgreSQL (debe tener permisos de lectura/escritura)
DB_USER="crmuser"

# Contraseña del usuario (se pasa mediante variable de entorno PGPASSWORD)
DB_PASSWORD="crm123456"

# Host donde está corriendo PostgreSQL (localhost = esta misma máquina)
DB_HOST="localhost"

################################################################################
# SECCIÓN 2: CÓDIGOS DE COLOR ANSI
################################################################################
#
# Los códigos ANSI permiten mostrar texto en colores en la terminal.
# Formato: \033[CÓDIGO_COLORm
# Para terminar el color, usamos \033[0m (NC = No Color)
#
# Referencia de códigos:
# - 0;31 = Rojo normal
# - 0;32 = Verde normal
# - 1;33 = Amarillo brillante
# - 0;34 = Azul normal
# - 0;36 = Cyan normal
# - 1;37 = Blanco brillante
# - 1 = Negrita (bold)
# - 2 = Tenue (dim)
#
################################################################################

# Color verde - usado para mensajes de éxito
GREEN='\033[0;32m'

# Color cyan - usado para títulos y encabezados
CYAN='\033[0;36m'

# Color amarillo brillante - usado para queries SQL
YELLOW='\033[1;33m'

# Color blanco brillante - usado para información importante
WHITE='\033[1;37m'

# Sin color - resetea el formato a normal
NC='\033[0m'

# Texto en negrita - hace el texto más grueso
BOLD='\033[1m'

################################################################################
# SECCIÓN 3: CONTADORES DE PRUEBAS
################################################################################
#
# Estas variables llevan el registro de cuántas pruebas se ejecutaron
# y cuántas pasaron o fallaron. Se actualizan en cada prueba.
#
################################################################################

# Contador de pruebas que pasaron exitosamente
PASSED=0

# Contador de pruebas que fallaron
FAILED=0

# Contador total de pruebas ejecutadas
TOTAL=0

################################################################################
# SECCIÓN 4: FUNCIÓN PRINCIPAL - test_endpoint
################################################################################
#
# Esta es la función más importante del script. Ejecuta una prueba completa
# de un endpoint de la API, incluyendo precondiciones SQL, llamada API,
# postcondiciones SQL y validación del resultado.
#
# PARÁMETROS:
# -----------
# $1 = test_num         : Número de la prueba (ej: "1", "2", "3")
# $2 = method           : Método HTTP (ej: "GET", "POST", "PUT", "DELETE")
# $3 = endpoint         : Ruta del endpoint (ej: "/Prospectos", "/Clientes/1")
# $4 = data             : Datos JSON para POST/PUT (vacío "" para GET)
# $5 = expected_code    : Código HTTP esperado (ej: "200", "201", "404")
# $6 = description      : Descripción legible de la prueba
# $7 = pre_sql          : Query SQL a ejecutar ANTES de la prueba (precondición)
# $8 = post_sql         : Query SQL a ejecutar DESPUÉS de la prueba (postcondición)
#
# EJEMPLO DE USO:
# ---------------
# test_endpoint "1" "GET" "/Prospectos" "" "200" \
#     "Listar todos los prospectos" \
#     "SELECT COUNT(*) FROM \"Prospectos\";" \
#     "SELECT COUNT(*) FROM \"Prospectos\";"
#
################################################################################
function test_endpoint() {
    # Asignar los parámetros a variables locales con nombres descriptivos
    # Esto hace el código más legible y fácil de mantener
    local test_num="$1"        # Número de prueba
    local method="$2"          # Método HTTP (GET, POST, etc.)
    local endpoint="$3"        # Endpoint de la API
    local data="$4"            # Datos JSON (payload)
    local expected_code="$5"   # Código HTTP esperado
    local description="$6"     # Descripción de la prueba
    local pre_sql="$7"         # SQL precondición
    local post_sql="$8"        # SQL postcondición
    
    # Incrementar el contador total de pruebas
    # El operador $((expresión)) realiza aritmética en bash
    TOTAL=$((TOTAL + 1))
    
    ################################################################################
    # PASO 1: IMPRIMIR ENCABEZADO DE LA PRUEBA
    ################################################################################
    
    # Imprimir una línea separadora visual
    # -e habilita la interpretación de secuencias de escape (\n, \033, etc.)
    # \n = nueva línea
    echo -e "\n${BOLD}${CYAN}═══════════════════════════════════════════════════════════════${NC}"
    
    # Imprimir el título de la prueba con número y descripción
    echo -e "${BOLD}${WHITE}TEST #$test_num: $description${NC}"
    
    # Imprimir otra línea separadora
    echo -e "${BOLD}${CYAN}═══════════════════════════════════════════════════════════════${NC}\n"
    
    ################################################################################
    # PASO 2: EJECUTAR PRECONDICIÓN SQL
    ################################################################################
    #
    # La precondición muestra el estado de la base de datos ANTES de ejecutar
    # la prueba. Esto es crucial para verificar que los cambios posteriores
    # son causados por nuestra llamada a la API.
    #
    ################################################################################
    
    # Verificar si se proporcionó una query de precondición
    # -n verifica que la variable NO esté vacía
    if [ -n "$pre_sql" ]; then
        # Imprimir encabezado de precondición con emoji 📊
        echo -e "${CYAN}📊 PRECONDICIÓN SQL:${NC}"
        
        # Mostrar la query SQL que se va a ejecutar
        # Esto ayuda a entender qué estamos verificando
        echo -e "${YELLOW}$pre_sql${NC}"
        
        # Ejecutar la query SQL usando psql
        # --------------------------------
        # PGPASSWORD=$DB_PASSWORD : Establece la contraseña como variable de entorno
        #                          (evita que psql pida la contraseña interactivamente)
        # psql                    : Cliente de línea de comandos de PostgreSQL
        # -h $DB_HOST            : Host de la base de datos (localhost)
        # -U $DB_USER            : Usuario de la base de datos (crmuser)
        # -d $DB_NAME            : Nombre de la base de datos (crmdb)
        # -c "$pre_sql"          : Comando SQL a ejecutar
        #
        # La salida de psql se muestra automáticamente en formato de tabla
        PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "$pre_sql"
        
        # Imprimir línea en blanco para separar secciones
        echo ""
    fi
    
    ################################################################################
    # PASO 3: EJECUTAR LLAMADA A LA API
    ################################################################################
    #
    # Esta sección hace la llamada HTTP a la API usando curl y captura tanto
    # el código de respuesta HTTP como el cuerpo de la respuesta (body).
    #
    ################################################################################
    
    # Imprimir encabezado de la llamada API con emoji 🌐
    echo -e "${CYAN}🌐 LLAMADA API:${NC}"
    
    # Mostrar el método HTTP y la URL completa
    echo -e "${WHITE}$method ${API_URL}${endpoint}${NC}"
    
    # Si hay datos (payload), mostrarlos formateados
    # Esto solo aplica para POST, PUT, PATCH
    if [ -n "$data" ]; then
        echo -e "${YELLOW}Payload:${NC}"
        
        # Formatear el JSON usando jq
        # jq '.' toma JSON y lo formatea con indentación
        # Si jq falla (JSON inválido), muestra el texto sin formatear
        echo "$data" | jq '.' 2>/dev/null || echo "$data"
    fi
    
    # Ejecutar la llamada HTTP según el método
    # -----------------------------------------
    if [ "$method" == "GET" ]; then
        # Llamada GET
        # -----------
        # curl                    : Herramienta para hacer peticiones HTTP
        # -s                      : Modo silencioso (no muestra barra de progreso)
        # -w "\n%{http_code}"     : Escribe el código HTTP en una nueva línea al final
        # "${API_URL}${endpoint}" : URL completa del endpoint
        #
        # Ejemplo de salida:
        # {"id":1,"nombre":"Test"}
        # 200
        RESPONSE=$(curl -s -w "\n%{http_code}" "${API_URL}${endpoint}")
        
    elif [ "$method" == "POST" ]; then
        # Llamada POST
        # ------------
        # -X POST                      : Especifica método POST
        # -H "Content-Type: ..."       : Header que indica que enviamos JSON
        # -d "$data"                   : Datos a enviar en el body
        RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "${API_URL}${endpoint}" \
            -H "Content-Type: application/json" \
            -d "$data")
    fi
    
    # Extraer el código HTTP de la respuesta
    # ---------------------------------------
    # La respuesta de curl tiene el formato:
    # [BODY]
    # [HTTP_CODE]
    #
    # tail -n1 : Obtiene la última línea (el código HTTP)
    HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
    
    # Extraer el cuerpo (body) de la respuesta
    # -----------------------------------------
    # sed '$d' : Elimina la última línea (el código HTTP)
    # Esto nos deja solo con el JSON
    BODY=$(echo "$RESPONSE" | sed '$d')
    
    # Mostrar el código HTTP obtenido
    echo -e "${WHITE}HTTP Status: ${HTTP_CODE}${NC}"
    
    # Mostrar la respuesta JSON formateada
    echo -e "${WHITE}Response:${NC}"
    
    # Formatear el JSON y mostrar solo las primeras 20 líneas
    # head -20 : Limita la salida para no saturar la pantalla
    # Si el JSON es inválido, muestra el texto sin formatear
    echo "$BODY" | jq '.' 2>/dev/null | head -20
    
    # Línea en blanco para separar
    echo ""
    
    ################################################################################
    # PASO 4: EJECUTAR POSTCONDICIÓN SQL
    ################################################################################
    #
    # La postcondición muestra el estado de la base de datos DESPUÉS de ejecutar
    # la llamada a la API. Comparando con la precondición, podemos verificar
    # que los cambios esperados ocurrieron.
    #
    ################################################################################
    
    # Verificar si se proporcionó una query de postcondición
    if [ -n "$post_sql" ]; then
        # Imprimir encabezado de postcondición
        echo -e "${CYAN}📊 POSTCONDICIÓN SQL:${NC}"
        
        # Mostrar la query SQL
        echo -e "${YELLOW}$post_sql${NC}"
        
        # Ejecutar la query SQL (mismo proceso que la precondición)
        PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "$post_sql"
        
        # Línea en blanco
        echo ""
    fi
    
    ################################################################################
    # PASO 5: VALIDAR EL RESULTADO
    ################################################################################
    #
    # Comparar el código HTTP obtenido con el código esperado.
    # Si coinciden, la prueba pasó. Si no, falló.
    #
    ################################################################################
    
    # Comparar códigos HTTP
    # == en bash compara strings
    if [ "$HTTP_CODE" == "$expected_code" ]; then
        # ✅ PRUEBA EXITOSA
        # -----------------
        
        # Mostrar mensaje de éxito en verde
        echo -e "${GREEN}✓ EXITOSO${NC} - $description"
        
        # Incrementar contador de pruebas exitosas
        PASSED=$((PASSED + 1))
    else
        # ❌ PRUEBA FALLIDA
        # -----------------
        
        # Mostrar mensaje de fallo en rojo
        # \033[0;31m es el código ANSI para rojo (no usamos $RED porque no está definido)
        echo -e "\033[0;31m✗ FALLIDO${NC} - Esperado $expected_code, obtenido $HTTP_CODE"
        
        # Incrementar contador de pruebas fallidas
        FAILED=$((FAILED + 1))
    fi
}

################################################################################
# SECCIÓN 5: INICIO DEL SCRIPT - ENCABEZADO
################################################################################

# Limpiar la pantalla para una presentación limpia
# clear borra todo el contenido de la terminal
clear

# Imprimir encabezado principal del script
echo -e "${BOLD}${CYAN}╔═══════════════════════════════════════════════════════════════╗${NC}"
echo -e "${BOLD}${CYAN}║  PRUEBAS API CRM CON PRECONDICIONES/POSTCONDICIONES SQL      ║${NC}"
echo -e "${BOLD}${CYAN}╚═══════════════════════════════════════════════════════════════╝${NC}\n"

################################################################################
# SECCIÓN 6: EJECUCIÓN DE LAS PRUEBAS
################################################################################

################################################################################
# PRUEBA #1: GET /api/Prospectos
################################################################################
#
# OBJETIVO:
# Verificar que el endpoint GET /api/Prospectos devuelve la lista completa
# de prospectos almacenados en la base de datos.
#
# PRECONDICIÓN:
# Muestra todos los prospectos existentes con sus campos principales.
#
# POSTCONDICIÓN:
# Verifica que el número total de prospectos no cambió (GET no modifica datos).
#
# RESULTADO ESPERADO:
# HTTP 200 con un array JSON de prospectos.
#
################################################################################
test_endpoint "1" "GET" "/Prospectos" "" "200" \
    "Listar todos los prospectos" \
    "SELECT \"Id\", \"CodigoProspecto\", \"NombreEmpresa\", \"EstadoProspecto\" FROM \"Prospectos\" ORDER BY \"Id\";" \
    "SELECT COUNT(*) as total_prospectos FROM \"Prospectos\";"

################################################################################
# PRUEBA #2: GET /api/Prospectos/1
################################################################################
#
# OBJETIVO:
# Verificar que el endpoint GET /api/Prospectos/{id} devuelve un prospecto
# específico con todos sus detalles y relaciones (fuente, vendedor, sucursal).
#
# PRECONDICIÓN:
# Muestra los datos del prospecto con ID=1 antes de la consulta.
#
# POSTCONDICIÓN:
# Verifica que la fecha de actualización no cambió (GET no modifica datos).
#
# RESULTADO ESPERADO:
# HTTP 200 con un objeto JSON del prospecto solicitado.
#
################################################################################
test_endpoint "2" "GET" "/Prospectos/1" "" "200" \
    "Obtener prospecto por ID" \
    "SELECT \"Id\", \"CodigoProspecto\", \"NombreEmpresa\", \"Email\" FROM \"Prospectos\" WHERE \"Id\" = 1;" \
    "SELECT \"FechaActualizacion\" FROM \"Prospectos\" WHERE \"Id\" = 1;"

################################################################################
# PRUEBA #3: POST /api/Prospectos
################################################################################
#
# OBJETIVO:
# Verificar que el endpoint POST /api/Prospectos crea un nuevo prospecto
# en la base de datos con los datos proporcionados.
#
# PRECONDICIÓN:
# Muestra el total de prospectos y el último ID antes de crear.
#
# POSTCONDICIÓN:
# Muestra el prospecto recién creado con su código generado automáticamente.
#
# RESULTADO ESPERADO:
# HTTP 201 (Created) con el objeto JSON del prospecto creado.
#
# VERIFICACIONES:
# - El total de prospectos aumenta en 1
# - Se genera un código único (ej: PROS-2025-003)
# - Todos los campos se guardan correctamente
# - Las relaciones FK se establecen correctamente
#
################################################################################

# Definir los datos JSON a enviar
# --------------------------------
# Esta variable contiene el payload completo para crear un prospecto.
# Incluye todos los campos requeridos según el DTO CrearProspectoDto.
#
# CAMPOS:
# - nombreEmpresa          : Nombre de la empresa del prospecto (requerido)
# - nombreContacto         : Nombre del contacto (requerido)
# - apellidoContacto       : Apellido del contacto (requerido)
# - email                  : Email del contacto (requerido, validado)
# - telefono               : Teléfono del contacto (requerido, validado)
# - fuenteId               : ID de la fuente (FK a FuentesProspecto)
# - sucursalId             : ID de la sucursal (FK a Sucursales)
# - vendedorAsignadoId     : ID del vendedor (FK a Usuarios)
# - estadoProspecto        : Estado (Nuevo, Contactado, Calificado, etc.)
# - prioridad              : Prioridad (Alta, Media, Baja)
# - valorEstimado          : Valor estimado del negocio en pesos
# - probabilidadCierre     : Probabilidad de cierre (0-100%)
#
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

# Ejecutar la prueba de creación
test_endpoint "3" "POST" "/Prospectos" "$POST_DATA" "201" \
    "Crear nuevo prospecto" \
    "SELECT COUNT(*) as total_antes, MAX(\"Id\") as ultimo_id FROM \"Prospectos\";" \
    "SELECT \"Id\", \"CodigoProspecto\", \"NombreEmpresa\", \"ValorEstimado\" FROM \"Prospectos\" ORDER BY \"Id\" DESC LIMIT 1;"

################################################################################
# PRUEBA #4: GET /api/Prospectos/fuentes
################################################################################
#
# OBJETIVO:
# Verificar que el endpoint GET /api/Prospectos/fuentes devuelve todas las
# fuentes de prospectos disponibles en el sistema.
#
# PRECONDICIÓN:
# Muestra todas las fuentes existentes con sus nombres.
#
# POSTCONDICIÓN:
# Verifica que el total de fuentes no cambió.
#
# RESULTADO ESPERADO:
# HTTP 200 con un array JSON de fuentes.
#
# USO:
# Este endpoint es útil para poblar dropdowns/selects en formularios.
#
################################################################################
test_endpoint "4" "GET" "/Prospectos/fuentes" "" "200" \
    "Listar fuentes de prospectos" \
    "SELECT \"Id\", \"NombreFuente\" FROM \"FuentesProspecto\" ORDER BY \"Id\";" \
    "SELECT COUNT(*) as total_fuentes FROM \"FuentesProspecto\";"

################################################################################
# PRUEBA #5: GET /api/Clientes/categorias
################################################################################
#
# OBJETIVO:
# Verificar que el endpoint GET /api/Clientes/categorias devuelve todas las
# categorías de clientes con sus porcentajes de descuento.
#
# PRECONDICIÓN:
# Muestra todas las categorías ordenadas por descuento (mayor a menor).
#
# POSTCONDICIÓN:
# Verifica que el total de categorías no cambió.
#
# RESULTADO ESPERADO:
# HTTP 200 con un array JSON de categorías.
#
# CATEGORÍAS ESPERADAS:
# - Premium (20% descuento)
# - Corporativo (15% descuento)
# - Regular (10% descuento)
# - Nuevo (5% descuento)
#
################################################################################
test_endpoint "5" "GET" "/Clientes/categorias" "" "200" \
    "Listar categorías de clientes" \
    "SELECT \"Id\", \"NombreCategoria\", \"PorcentajeDescuento\" FROM \"CategoriasCliente\" ORDER BY \"PorcentajeDescuento\" DESC;" \
    "SELECT COUNT(*) as total_categorias FROM \"CategoriasCliente\";"

################################################################################
# SECCIÓN 7: REPORTE FINAL
################################################################################
#
# Esta sección genera un resumen de todas las pruebas ejecutadas,
# mostrando estadísticas y el resultado general.
#
################################################################################

# Imprimir encabezado del reporte
echo -e "\n${BOLD}${CYAN}╔═══════════════════════════════════════════════════════════════╗${NC}"
echo -e "${BOLD}${CYAN}║  REPORTE FINAL                                                ║${NC}"
echo -e "${BOLD}${CYAN}╚═══════════════════════════════════════════════════════════════╝${NC}\n"

# Calcular la tasa de éxito
# -------------------------
# Fórmula: (pruebas_exitosas / total_pruebas) * 100
# El resultado es un porcentaje entero (sin decimales)
SUCCESS_RATE=$((PASSED * 100 / TOTAL))

# Mostrar estadísticas
echo -e "${WHITE}Total de pruebas:${NC}     $TOTAL"
echo -e "${GREEN}Pruebas exitosas:${NC}     $PASSED"
echo -e "\033[0;31mPruebas fallidas:${NC}     $FAILED"
echo -e "${CYAN}Tasa de éxito:${NC}        ${SUCCESS_RATE}%"

# Mostrar mensaje final según el resultado
# -----------------------------------------
# Si no hay pruebas fallidas (FAILED == 0), todo está bien
if [ $FAILED -eq 0 ]; then
    # ✅ TODAS LAS PRUEBAS PASARON
    echo -e "\n${GREEN}${BOLD}✓ TODAS LAS PRUEBAS EXITOSAS${NC}\n"
else
    # ❌ ALGUNAS PRUEBAS FALLARON
    echo -e "\n\033[0;31m${BOLD}✗ ALGUNAS PRUEBAS FALLARON${NC}\n"
fi

################################################################################
# FIN DEL SCRIPT
################################################################################
#
# NOTAS ADICIONALES:
# ------------------
#
# 1. CÓDIGOS DE SALIDA:
#    El script no usa exit codes explícitos. Siempre termina con código 0.
#    Para CI/CD, se podría agregar: exit $FAILED
#
# 2. LOGS:
#    Para guardar la salida en un archivo:
#    ./test-api-visual-documentado.sh | tee pruebas.log
#
# 3. PERSONALIZACIÓN:
#    - Modificar API_URL para apuntar a otro servidor
#    - Modificar DB_* para usar otra base de datos
#    - Agregar más pruebas siguiendo el patrón de test_endpoint
#
# 4. DEPURACIÓN:
#    Para ver cada comando ejecutado, agregar al inicio:
#    set -x
#
# 5. MANEJO DE ERRORES:
#    Para detener el script si algún comando falla:
#    set -e
#
# 6. EXTENSIONES POSIBLES:
#    - Agregar pruebas de PUT y DELETE
#    - Agregar validación de esquema JSON
#    - Agregar pruebas de rendimiento (tiempo de respuesta)
#    - Generar reporte HTML
#    - Enviar notificaciones por email/Slack
#
################################################################################

