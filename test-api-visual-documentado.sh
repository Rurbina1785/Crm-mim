#!/bin/bash

################################################################################
# SCRIPT DE PRUEBAS AUTOMATIZADAS PARA API CRM CON POSTGRESQL
################################################################################
#
# PROPOSITO:
# ----------
# Este script automatiza las pruebas de los endpoints de la API CRM, mostrando
# de forma visual y completa:
#   1. El estado de la base de datos ANTES de cada prueba (PRECONDICION)
#   2. La llamada a la API con todos sus detalles (request y response)
#   3. El estado de la base de datos DESPUES de cada prueba (POSTCONDICION)
#   4. La validacion automatica del resultado esperado vs obtenido
#
# CARACTERISTICAS:
# ----------------
# - Ejecuta consultas SQL directamente con psql para mostrar datos reales
# - Formatea JSON con jq para mejor legibilidad
# - Usa colores ANSI para resaltar informacion importante
# - Valida automaticamente codigos HTTP esperados
# - Genera reporte final con estadisticas
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
# VERSION: 1.0
#
################################################################################

################################################################################
# SECCION 1: CONFIGURACION Y VARIABLES GLOBALES
################################################################################

# URL base de la API
# ------------------
# Esta es la URL donde esta corriendo nuestra API REST.
# Todos los endpoints se construiran a partir de esta base.
# Ejemplo: ${API_URL}/Prospectos se convierte en http://localhost:5000/api/Prospectos
API_URL="http://localhost:5000/api"

# Configuracion de conexion a PostgreSQL
# ---------------------------------------
# Estas variables se usan para conectarse a la base de datos PostgreSQL
# mediante el comando psql. Son las mismas credenciales que usa la API
# en appsettings.json

# Nombre de la base de datos
DB_NAME="crmdb"

# Usuario de PostgreSQL (debe tener permisos de lectura/escritura)
DB_USER="crmuser"

# Contrasena del usuario (se pasa mediante variable de entorno PGPASSWORD)
DB_PASSWORD="crm123456"

# Host donde esta corriendo PostgreSQL (localhost = esta misma maquina)
DB_HOST="localhost"

################################################################################
# SECCION 2: CODIGOS DE COLOR ANSI
################################################################################
#
# Los codigos ANSI permiten mostrar texto en colores en la terminal.
# Formato: \033[CODIGO_COLORm
# Para terminar el color, usamos \033[0m (NC = No Color)
#
# Referencia de codigos:
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

# Color verde - usado para mensajes de exito
GREEN='\033[0;32m'

# Color cyan - usado para titulos y encabezados
CYAN='\033[0;36m'

# Color amarillo brillante - usado para queries SQL
YELLOW='\033[1;33m'

# Color blanco brillante - usado para informacion importante
WHITE='\033[1;37m'

# Sin color - resetea el formato a normal
NC='\033[0m'

# Texto en negrita - hace el texto mas grueso
BOLD='\033[1m'

################################################################################
# SECCION 3: CONTADORES DE PRUEBAS
################################################################################
#
# Estas variables llevan el registro de cuantas pruebas se ejecutaron
# y cuantas pasaron o fallaron. Se actualizan en cada prueba.
#
################################################################################

# Contador de pruebas que pasaron exitosamente
PASSED=0

# Contador de pruebas que fallaron
FAILED=0

# Contador total de pruebas ejecutadas
TOTAL=0

################################################################################
# SECCION 4: FUNCION PRINCIPAL - test_endpoint
################################################################################
#
# Esta es la funcion mas importante del script. Ejecuta una prueba completa
# de un endpoint de la API, incluyendo precondiciones SQL, llamada API,
# postcondiciones SQL y validacion del resultado.
#
# PARAMETROS:
# -----------
# $1 = test_num         : Numero de la prueba (ej: "1", "2", "3")
# $2 = method           : Metodo HTTP (ej: "GET", "POST", "PUT", "DELETE")
# $3 = endpoint         : Ruta del endpoint (ej: "/Prospectos", "/Clientes/1")
# $4 = data             : Datos JSON para POST/PUT (vacio "" para GET)
# $5 = expected_code    : Codigo HTTP esperado (ej: "200", "201", "404")
# $6 = description      : Descripcion legible de la prueba
# $7 = pre_sql          : Query SQL a ejecutar ANTES de la prueba (precondicion)
# $8 = post_sql         : Query SQL a ejecutar DESPUES de la prueba (postcondicion)
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
    # Asignar los parametros a variables locales con nombres descriptivos
    # Esto hace el codigo mas legible y facil de mantener
    local test_num="$1"        # Numero de prueba
    local method="$2"          # Metodo HTTP (GET, POST, etc.)
    local endpoint="$3"        # Endpoint de la API
    local data="$4"            # Datos JSON (payload)
    local expected_code="$5"   # Codigo HTTP esperado
    local description="$6"     # Descripcion de la prueba
    local pre_sql="$7"         # SQL precondicion
    local post_sql="$8"        # SQL postcondicion
    
    # Incrementar el contador total de pruebas
    # El operador $((expresion)) realiza aritmetica en bash
    TOTAL=$((TOTAL + 1))
    
    ################################################################################
    # PASO 1: IMPRIMIR ENCABEZADO DE LA PRUEBA
    ################################################################################
    
    # Imprimir una linea separadora visual
    # -e habilita la interpretacion de secuencias de escape (\n, \033, etc.)
    # \n = nueva linea
    echo -e "\n${BOLD}${CYAN}===============================================================${NC}"
    
    # Imprimir el titulo de la prueba con numero y descripcion
    echo -e "${BOLD}${WHITE}TEST #$test_num: $description${NC}"
    
    # Imprimir otra linea separadora
    echo -e "${BOLD}${CYAN}===============================================================${NC}\n"
    
    ################################################################################
    # PASO 2: EJECUTAR PRECONDICION SQL
    ################################################################################
    #
    # La precondicion muestra el estado de la base de datos ANTES de ejecutar
    # la prueba. Esto es crucial para verificar que los cambios posteriores
    # son causados por nuestra llamada a la API.
    #
    ################################################################################
    
    # Verificar si se proporciono una query de precondicion
    # -n verifica que la variable NO este vacia
    if [ -n "$pre_sql" ]; then
        # Imprimir encabezado de precondicion
        echo -e "${CYAN}[PRECONDICION SQL]${NC}"
        
        # Mostrar la query SQL que se va a ejecutar
        # Esto ayuda a entender que estamos verificando
        echo -e "${YELLOW}$pre_sql${NC}"
        
        # Ejecutar la query SQL usando psql
        # --------------------------------
        # PGPASSWORD=$DB_PASSWORD : Establece la contrasena como variable de entorno
        #                          (evita que psql pida la contrasena interactivamente)
        # psql                    : Cliente de linea de comandos de PostgreSQL
        # -h $DB_HOST            : Host de la base de datos (localhost)
        # -U $DB_USER            : Usuario de la base de datos (crmuser)
        # -d $DB_NAME            : Nombre de la base de datos (crmdb)
        # -c "$pre_sql"          : Comando SQL a ejecutar
        #
        # La salida de psql se muestra automaticamente en formato de tabla
        PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "$pre_sql"
        
        # Imprimir linea en blanco para separar secciones
        echo ""
    fi
    
    ################################################################################
    # PASO 3: EJECUTAR LLAMADA A LA API
    ################################################################################
    #
    # Esta seccion hace la llamada HTTP a la API usando curl y captura tanto
    # el codigo de respuesta HTTP como el cuerpo de la respuesta (body).
    #
    ################################################################################
    
    # Imprimir encabezado de la llamada API
    echo -e "${CYAN}[LLAMADA API]${NC}"
    
    # Mostrar el metodo HTTP y la URL completa
    echo -e "${WHITE}$method ${API_URL}${endpoint}${NC}"
    
    # Si hay datos (payload), mostrarlos formateados
    # Esto solo aplica para POST, PUT, PATCH
    if [ -n "$data" ]; then
        echo -e "${YELLOW}Payload:${NC}"
        
        # Formatear el JSON usando jq
        # jq '.' toma JSON y lo formatea con indentacion
        # Si jq falla (JSON invalido), muestra el texto sin formatear
        echo "$data" | jq '.' 2>/dev/null || echo "$data"
    fi
    
    # Ejecutar la llamada HTTP segun el metodo
    # -----------------------------------------
    if [ "$method" == "GET" ]; then
        # Llamada GET
        # -----------
        # curl                    : Herramienta para hacer peticiones HTTP
        # -s                      : Modo silencioso (no muestra barra de progreso)
        # -w "\n%{http_code}"     : Escribe el codigo HTTP en una nueva linea al final
        # "${API_URL}${endpoint}" : URL completa del endpoint
        #
        # Ejemplo de salida:
        # {"id":1,"nombre":"Test"}
        # 200
        RESPONSE=$(curl -s -w "\n%{http_code}" "${API_URL}${endpoint}")
        
    elif [ "$method" == "POST" ]; then
        # Llamada POST
        # ------------
        # -X POST                      : Especifica metodo POST
        # -H "Content-Type: ..."       : Header que indica que enviamos JSON
        # -d "$data"                   : Datos a enviar en el body
        RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "${API_URL}${endpoint}" \
            -H "Content-Type: application/json" \
            -d "$data")
    fi
    
    # Extraer el codigo HTTP de la respuesta
    # ---------------------------------------
    # La respuesta de curl tiene el formato:
    # [BODY]
    # [HTTP_CODE]
    #
    # tail -n1 : Obtiene la ultima linea (el codigo HTTP)
    HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
    
    # Extraer el cuerpo (body) de la respuesta
    # -----------------------------------------
    # sed '$d' : Elimina la ultima linea (el codigo HTTP)
    # Esto nos deja solo con el JSON
    BODY=$(echo "$RESPONSE" | sed '$d')
    
    # Mostrar el codigo HTTP obtenido
    echo -e "${WHITE}HTTP Status: ${HTTP_CODE}${NC}"
    
    # Mostrar la respuesta JSON formateada
    echo -e "${WHITE}Response:${NC}"
    
    # Formatear el JSON y mostrar solo las primeras 20 lineas
    # head -20 : Limita la salida para no saturar la pantalla
    # Si el JSON es invalido, muestra el texto sin formatear
    echo "$BODY" | jq '.' 2>/dev/null | head -20
    
    # Linea en blanco para separar
    echo ""
    
    ################################################################################
    # PASO 4: EJECUTAR POSTCONDICION SQL
    ################################################################################
    #
    # La postcondicion muestra el estado de la base de datos DESPUES de ejecutar
    # la llamada a la API. Comparando con la precondicion, podemos verificar
    # que los cambios esperados ocurrieron.
    #
    ################################################################################
    
    # Verificar si se proporciono una query de postcondicion
    if [ -n "$post_sql" ]; then
        # Imprimir encabezado de postcondicion
        echo -e "${CYAN}[POSTCONDICION SQL]${NC}"
        
        # Mostrar la query SQL
        echo -e "${YELLOW}$post_sql${NC}"
        
        # Ejecutar la query SQL (mismo proceso que la precondicion)
        PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "$post_sql"
        
        # Linea en blanco
        echo ""
    fi
    
    ################################################################################
    # PASO 5: VALIDAR EL RESULTADO
    ################################################################################
    #
    # Comparar el codigo HTTP obtenido con el codigo esperado.
    # Si coinciden, la prueba paso. Si no, fallo.
    #
    ################################################################################
    
    # Comparar codigos HTTP
    # == en bash compara strings
    if [ "$HTTP_CODE" == "$expected_code" ]; then
        # [EXITO] PRUEBA EXITOSA
        # -----------------
        
        # Mostrar mensaje de exito en verde
        echo -e "${GREEN}[EXITO]${NC} - $description"
        
        # Incrementar contador de pruebas exitosas
        PASSED=$((PASSED + 1))
    else
        # [FALLO] PRUEBA FALLIDA
        # -----------------
        
        # Mostrar mensaje de fallo en rojo
        # \033[0;31m es el codigo ANSI para rojo (no usamos $RED porque no esta definido)
        echo -e "\033[0;31m[FALLO]${NC} - Esperado $expected_code, obtenido $HTTP_CODE"
        
        # Incrementar contador de pruebas fallidas
        FAILED=$((FAILED + 1))
    fi
}

################################################################################
# SECCION 5: INICIO DEL SCRIPT - ENCABEZADO
################################################################################

# Limpiar la pantalla para una presentacion limpia
# clear borra todo el contenido de la terminal
clear

# Imprimir encabezado principal del script
echo -e "${BOLD}${CYAN}===============================================================${NC}"
echo -e "${BOLD}${CYAN}  PRUEBAS API CRM CON PRECONDICIONES/POSTCONDICIONES SQL     ${NC}"
echo -e "${BOLD}${CYAN}===============================================================${NC}\n"

################################################################################
# SECCION 6: EJECUCION DE LAS PRUEBAS
################################################################################

################################################################################
# PRUEBA #1: GET /api/Prospectos
################################################################################
#
# OBJETIVO:
# Verificar que el endpoint GET /api/Prospectos devuelve la lista completa
# de prospectos almacenados en la base de datos.
#
# PRECONDICION:
# Muestra todos los prospectos existentes con sus campos principales.
#
# POSTCONDICION:
# Verifica que el numero total de prospectos no cambio (GET no modifica datos).
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
# especifico con todos sus detalles y relaciones (fuente, vendedor, sucursal).
#
# PRECONDICION:
# Muestra los datos del prospecto con ID=1 antes de la consulta.
#
# POSTCONDICION:
# Verifica que la fecha de actualizacion no cambio (GET no modifica datos).
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
# PRECONDICION:
# Muestra el total de prospectos y el ultimo ID antes de crear.
#
# POSTCONDICION:
# Muestra el prospecto recien creado con su codigo generado automaticamente.
#
# RESULTADO ESPERADO:
# HTTP 201 (Created) con el objeto JSON del prospecto creado.
#
# VERIFICACIONES:
# - El total de prospectos aumenta en 1
# - Se genera un codigo unico (ej: PROS-2025-003)
# - Todos los campos se guardan correctamente
# - Las relaciones FK se establecen correctamente
#
################################################################################

# Definir los datos JSON a enviar
# --------------------------------
# Esta variable contiene el payload completo para crear un prospecto.
# Incluye todos los campos requeridos segun el DTO CrearProspectoDto.
#
# CAMPOS:
# - nombreEmpresa          : Nombre de la empresa del prospecto (requerido)
# - nombreContacto         : Nombre del contacto (requerido)
# - apellidoContacto       : Apellido del contacto (requerido)
# - email                  : Email del contacto (requerido, validado)
# - telefono               : Telefono del contacto (requerido, validado)
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

# Ejecutar la prueba de creacion
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
# PRECONDICION:
# Muestra todas las fuentes existentes con sus nombres.
#
# POSTCONDICION:
# Verifica que el total de fuentes no cambio.
#
# RESULTADO ESPERADO:
# HTTP 200 con un array JSON de fuentes.
#
# USO:
# Este endpoint es util para poblar dropdowns/selects en formularios.
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
# categorias de clientes con sus porcentajes de descuento.
#
# PRECONDICION:
# Muestra todas las categorias ordenadas por descuento (mayor a menor).
#
# POSTCONDICION:
# Verifica que el total de categorias no cambio.
#
# RESULTADO ESPERADO:
# HTTP 200 con un array JSON de categorias.
#
# CATEGORIAS ESPERADAS:
# - Premium (20% descuento)
# - Corporativo (15% descuento)
# - Regular (10% descuento)
# - Nuevo (5% descuento)
#
################################################################################
test_endpoint "5" "GET" "/Clientes/categorias" "" "200" \
    "Listar categorias de clientes" \
    "SELECT \"Id\", \"NombreCategoria\", \"PorcentajeDescuento\" FROM \"CategoriasCliente\" ORDER BY \"PorcentajeDescuento\" DESC;" \
    "SELECT COUNT(*) as total_categorias FROM \"CategoriasCliente\";"

################################################################################
# SECCION 7: REPORTE FINAL
################################################################################
#
# Esta seccion genera un resumen de todas las pruebas ejecutadas,
# mostrando estadisticas y el resultado general.
#
################################################################################

# Imprimir encabezado del reporte
echo -e "\n${BOLD}${CYAN}===============================================================${NC}"
echo -e "${BOLD}${CYAN}  REPORTE FINAL                                               ${NC}"
echo -e "${BOLD}${CYAN}===============================================================${NC}\n"

# Calcular la tasa de exito
# -------------------------
# Formula: (pruebas_exitosas / total_pruebas) * 100
# El resultado es un porcentaje entero (sin decimales)
SUCCESS_RATE=$((PASSED * 100 / TOTAL))

# Mostrar estadisticas
echo -e "${WHITE}Total de pruebas:${NC}     $TOTAL"
echo -e "${GREEN}Pruebas exitosas:${NC}     $PASSED"
echo -e "\033[0;31mPruebas fallidas:${NC}     $FAILED"
echo -e "${CYAN}Tasa de exito:${NC}        ${SUCCESS_RATE}%"

# Mostrar mensaje final segun el resultado
# -----------------------------------------
# Si no hay pruebas fallidas (FAILED == 0), todo esta bien
if [ $FAILED -eq 0 ]; then
    # [EXITO] TODAS LAS PRUEBAS PASARON
    echo -e "\n${GREEN}${BOLD}[EXITO] TODAS LAS PRUEBAS EXITOSAS${NC}\n"
else
    # [FALLO] ALGUNAS PRUEBAS FALLARON
    echo -e "\n\033[0;31m${BOLD}[FALLO] ALGUNAS PRUEBAS FALLARON${NC}\n"
fi

################################################################################
# FIN DEL SCRIPT
################################################################################
#
# NOTAS ADICIONALES:
# ------------------
#
# 1. CODIGOS DE SALIDA:
#    El script no usa exit codes explicitos. Siempre termina con codigo 0.
#    Para CI/CD, se podria agregar: exit $FAILED
#
# 2. LOGS:
#    Para guardar la salida en un archivo:
#    ./test-api-visual-documentado.sh | tee pruebas.log
#
# 3. PERSONALIZACION:
#    - Modificar API_URL para apuntar a otro servidor
#    - Modificar DB_* para usar otra base de datos
#    - Agregar mas pruebas siguiendo el patron de test_endpoint
#
# 4. DEPURACION:
#    Para ver cada comando ejecutado, agregar al inicio:
#    set -x
#
# 5. MANEJO DE ERRORES:
#    Para detener el script si algun comando falla:
#    set -e
#
# 6. EXTENSIONES POSIBLES:
#    - Agregar pruebas de PUT y DELETE
#    - Agregar validacion de esquema JSON
#    - Agregar pruebas de rendimiento (tiempo de respuesta)
#    - Generar reporte HTML
#    - Enviar notificaciones por email/Slack
#
################################################################################

