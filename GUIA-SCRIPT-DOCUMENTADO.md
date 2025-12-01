# Guía Completa del Script test-api-visual-documentado.sh

## 📋 Tabla de Contenidos

1. [Introducción](#introducción)
2. [Estructura del Script](#estructura-del-script)
3. [Explicación Detallada por Secciones](#explicación-detallada-por-secciones)
4. [Conceptos de Bash Utilizados](#conceptos-de-bash-utilizados)
5. [Flujo de Ejecución](#flujo-de-ejecución)
6. [Ejemplos de Uso](#ejemplos-de-uso)
7. [Personalización](#personalización)
8. [Solución de Problemas](#solución-de-problemas)

---

## 📖 Introducción

### ¿Qué es este script?

`test-api-visual-documentado.sh` es un script de Bash **extremadamente documentado** que automatiza las pruebas de la API CRM mostrando:

- ✅ **Precondiciones SQL** - Estado de la BD antes de cada prueba
- ✅ **Llamadas API** - Request completo con método, URL y payload
- ✅ **Respuestas API** - HTTP status y JSON formateado
- ✅ **Postcondiciones SQL** - Estado de la BD después de cada prueba
- ✅ **Validación automática** - Comparación de resultados esperados vs obtenidos

### ¿Por qué está tan documentado?

Este script tiene **616 líneas** con comentarios exhaustivos para que:

1. **Aprendas Bash** - Cada línea está explicada en español
2. **Entiendas el flujo** - Sabrás exactamente qué hace cada parte
3. **Puedas modificarlo** - Tendrás la confianza para personalizarlo
4. **Sirva de referencia** - Es un ejemplo completo de testing automatizado

### Estadísticas del Script

| Métrica | Valor |
|---------|-------|
| **Total de líneas** | 616 |
| **Líneas de código** | ~150 |
| **Líneas de comentarios** | ~450 |
| **Ratio comentarios/código** | 3:1 |
| **Secciones** | 7 |
| **Funciones** | 1 (test_endpoint) |
| **Pruebas incluidas** | 5 |

---

## 🏗️ Estructura del Script

El script está organizado en **7 secciones principales**:

```
┌─────────────────────────────────────────────────────────┐
│ SECCIÓN 1: Configuración y Variables Globales          │
│ - URL de la API                                         │
│ - Credenciales de PostgreSQL                           │
├─────────────────────────────────────────────────────────┤
│ SECCIÓN 2: Códigos de Color ANSI                       │
│ - Definición de colores para output                    │
├─────────────────────────────────────────────────────────┤
│ SECCIÓN 3: Contadores de Pruebas                       │
│ - Variables para estadísticas                           │
├─────────────────────────────────────────────────────────┤
│ SECCIÓN 4: Función Principal - test_endpoint           │
│ - Lógica completa de una prueba                        │
│ - Precondición → API Call → Postcondición → Validación│
├─────────────────────────────────────────────────────────┤
│ SECCIÓN 5: Inicio del Script - Encabezado             │
│ - Limpiar pantalla y mostrar título                    │
├─────────────────────────────────────────────────────────┤
│ SECCIÓN 6: Ejecución de las Pruebas                    │
│ - 5 llamadas a test_endpoint con diferentes endpoints  │
├─────────────────────────────────────────────────────────┤
│ SECCIÓN 7: Reporte Final                               │
│ - Estadísticas y resultado general                      │
└─────────────────────────────────────────────────────────┘
```

---

## 📚 Explicación Detallada por Secciones

### SECCIÓN 1: Configuración y Variables Globales

```bash
API_URL="http://localhost:5000/api"
DB_NAME="crmdb"
DB_USER="crmuser"
DB_PASSWORD="crm123456"
DB_HOST="localhost"
```

**¿Qué hace?**
Define las constantes que se usan en todo el script.

**¿Por qué es importante?**
Centraliza la configuración en un solo lugar. Si cambias de servidor o base de datos, solo modificas estas líneas.

**Ejemplo de modificación:**
```bash
# Para probar contra un servidor remoto:
API_URL="https://api.miempresa.com/api"
DB_HOST="192.168.1.100"
```

---

### SECCIÓN 2: Códigos de Color ANSI

```bash
GREEN='\033[0;32m'
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
WHITE='\033[1;37m'
NC='\033[0m'
BOLD='\033[1m'
```

**¿Qué hace?**
Define códigos ANSI para mostrar texto en colores.

**¿Cómo funcionan los códigos ANSI?**

| Código | Color/Efecto |
|--------|--------------|
| `\033[0;31m` | Rojo normal |
| `\033[0;32m` | Verde normal |
| `\033[1;33m` | Amarillo brillante |
| `\033[0;36m` | Cyan normal |
| `\033[1;37m` | Blanco brillante |
| `\033[1m` | Negrita |
| `\033[0m` | Reset (sin color) |

**Ejemplo de uso:**
```bash
echo -e "${GREEN}Texto en verde${NC}"
echo -e "${BOLD}${CYAN}Texto en cyan y negrita${NC}"
```

**Salida:**
```
Texto en verde  ← (en verde)
Texto en cyan y negrita  ← (en cyan y negrita)
```

---

### SECCIÓN 3: Contadores de Pruebas

```bash
PASSED=0
FAILED=0
TOTAL=0
```

**¿Qué hace?**
Inicializa contadores para llevar estadísticas.

**¿Cómo se actualizan?**
```bash
TOTAL=$((TOTAL + 1))    # Incrementa en 1
PASSED=$((PASSED + 1))  # Si la prueba pasa
FAILED=$((FAILED + 1))  # Si la prueba falla
```

**¿Qué es `$(( ))`?**
Es la sintaxis de Bash para aritmética. Dentro de `$(( ))` puedes hacer operaciones matemáticas:
```bash
suma=$((5 + 3))           # suma=8
resta=$((10 - 4))         # resta=6
multiplicacion=$((6 * 7)) # multiplicacion=42
division=$((20 / 4))      # division=5
```

---

### SECCIÓN 4: Función Principal - test_endpoint

Esta es la función más importante del script. Veamos su estructura:

#### Parámetros de la Función

```bash
function test_endpoint() {
    local test_num="$1"        # Número de prueba
    local method="$2"          # GET, POST, PUT, DELETE
    local endpoint="$3"        # /Prospectos, /Clientes/1, etc.
    local data="$4"            # JSON payload (vacío para GET)
    local expected_code="$5"   # 200, 201, 404, etc.
    local description="$6"     # Descripción legible
    local pre_sql="$7"         # Query SQL precondición
    local post_sql="$8"        # Query SQL postcondición
```

**¿Qué es `local`?**
Declara una variable local a la función. Sin `local`, la variable sería global.

**¿Qué es `$1`, `$2`, etc.?**
Son los parámetros posicionales. `$1` es el primer argumento, `$2` el segundo, etc.

#### Paso 1: Imprimir Encabezado

```bash
echo -e "\n${BOLD}${CYAN}═══════════════...${NC}"
echo -e "${BOLD}${WHITE}TEST #$test_num: $description${NC}"
```

**¿Qué es `echo -e`?**
El flag `-e` habilita la interpretación de secuencias de escape como `\n` (nueva línea) y códigos ANSI.

**Ejemplo:**
```bash
echo "Hola\nMundo"    # Imprime: Hola\nMundo (literal)
echo -e "Hola\nMundo" # Imprime: Hola
                      #          Mundo
```

#### Paso 2: Ejecutar Precondición SQL

```bash
if [ -n "$pre_sql" ]; then
    echo -e "${CYAN}📊 PRECONDICIÓN SQL:${NC}"
    echo -e "${YELLOW}$pre_sql${NC}"
    PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "$pre_sql"
    echo ""
fi
```

**¿Qué es `[ -n "$pre_sql" ]`?**
Verifica si la variable `$pre_sql` NO está vacía.

| Operador | Significado |
|----------|-------------|
| `-n "$var"` | Verdadero si $var NO está vacía |
| `-z "$var"` | Verdadero si $var está vacía |
| `"$a" == "$b"` | Verdadero si $a es igual a $b |
| `"$a" != "$b"` | Verdadero si $a es diferente de $b |

**¿Qué hace `PGPASSWORD=$DB_PASSWORD`?**
Establece una variable de entorno solo para ese comando. `psql` lee `PGPASSWORD` automáticamente para autenticarse.

**Comando psql completo:**
```bash
PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "$pre_sql"
```

| Flag | Significado |
|------|-------------|
| `-h $DB_HOST` | Host de la base de datos |
| `-U $DB_USER` | Usuario de PostgreSQL |
| `-d $DB_NAME` | Nombre de la base de datos |
| `-c "$pre_sql"` | Comando SQL a ejecutar |

#### Paso 3: Ejecutar Llamada API

```bash
if [ "$method" == "GET" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" "${API_URL}${endpoint}")
elif [ "$method" == "POST" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X POST "${API_URL}${endpoint}" \
        -H "Content-Type: application/json" \
        -d "$data")
fi
```

**¿Qué es `$( )`?**
Ejecuta un comando y captura su salida en una variable.

**Ejemplo:**
```bash
fecha=$(date)
echo $fecha  # Imprime: Thu Nov 28 16:00:00 UTC 2025
```

**Comando curl para GET:**
```bash
curl -s -w "\n%{http_code}" "${API_URL}${endpoint}"
```

| Flag | Significado |
|------|-------------|
| `-s` | Modo silencioso (no muestra progreso) |
| `-w "\n%{http_code}"` | Escribe el código HTTP al final |
| `"${API_URL}${endpoint}"` | URL completa |

**¿Qué es `%{http_code}`?**
Es una variable especial de curl que contiene el código HTTP de la respuesta (200, 201, 404, etc.).

**Comando curl para POST:**
```bash
curl -s -w "\n%{http_code}" -X POST "${API_URL}${endpoint}" \
    -H "Content-Type: application/json" \
    -d "$data"
```

| Flag | Significado |
|------|-------------|
| `-X POST` | Método HTTP POST |
| `-H "Content-Type: ..."` | Header HTTP |
| `-d "$data"` | Datos a enviar (payload) |

**¿Qué es `\` al final de línea?**
Continúa el comando en la siguiente línea. Es para legibilidad.

#### Extraer HTTP Code y Body

```bash
HTTP_CODE=$(echo "$RESPONSE" | tail -n1)
BODY=$(echo "$RESPONSE" | sed '$d')
```

**¿Cómo funciona?**

La respuesta de curl tiene este formato:
```
{"id":1,"nombre":"Test"}
200
```

- `tail -n1` obtiene la última línea (200)
- `sed '$d'` elimina la última línea (deja solo el JSON)

**¿Qué es `|` (pipe)?**
Pasa la salida de un comando como entrada del siguiente.

**Ejemplo:**
```bash
echo "Hola Mundo" | wc -w  # Cuenta palabras: 2
```

#### Formatear JSON con jq

```bash
echo "$BODY" | jq '.' 2>/dev/null | head -20
```

**¿Qué hace cada parte?**

| Comando | Función |
|---------|---------|
| `echo "$BODY"` | Imprime el JSON |
| `jq '.'` | Formatea el JSON con indentación |
| `2>/dev/null` | Redirige errores a /dev/null (los oculta) |
| `head -20` | Muestra solo las primeras 20 líneas |

**¿Qué es `2>/dev/null`?**
- `2` es el descriptor de archivo para stderr (errores)
- `>` redirige la salida
- `/dev/null` es un "agujero negro" que descarta todo

**¿Por qué se usa?**
Si el JSON es inválido, `jq` muestra un error. Con `2>/dev/null` ocultamos ese error.

#### Paso 4: Ejecutar Postcondición SQL

```bash
if [ -n "$post_sql" ]; then
    PGPASSWORD=$DB_PASSWORD psql -h $DB_HOST -U $DB_USER -d $DB_NAME -c "$post_sql"
fi
```

Mismo proceso que la precondición.

#### Paso 5: Validar Resultado

```bash
if [ "$HTTP_CODE" == "$expected_code" ]; then
    echo -e "${GREEN}✓ EXITOSO${NC} - $description"
    PASSED=$((PASSED + 1))
else
    echo -e "\033[0;31m✗ FALLIDO${NC} - Esperado $expected_code, obtenido $HTTP_CODE"
    FAILED=$((FAILED + 1))
fi
```

**Lógica:**
1. Compara el código HTTP obtenido con el esperado
2. Si coinciden → prueba exitosa (incrementa PASSED)
3. Si no coinciden → prueba fallida (incrementa FAILED)

---

### SECCIÓN 5: Inicio del Script

```bash
clear
echo -e "${BOLD}${CYAN}╔═══════════════...╗${NC}"
echo -e "${BOLD}${CYAN}║  PRUEBAS API CRM ...  ║${NC}"
echo -e "${BOLD}${CYAN}╚═══════════════...╝${NC}\n"
```

**¿Qué hace `clear`?**
Limpia la pantalla de la terminal (borra todo el contenido anterior).

---

### SECCIÓN 6: Ejecución de las Pruebas

```bash
test_endpoint "1" "GET" "/Prospectos" "" "200" \
    "Listar todos los prospectos" \
    "SELECT \"Id\", \"CodigoProspecto\"..." \
    "SELECT COUNT(*)..."
```

**Estructura de la llamada:**

| Parámetro | Valor | Explicación |
|-----------|-------|-------------|
| `"1"` | test_num | Número de prueba |
| `"GET"` | method | Método HTTP |
| `"/Prospectos"` | endpoint | Ruta del endpoint |
| `""` | data | Sin payload (GET no envía datos) |
| `"200"` | expected_code | Esperamos HTTP 200 |
| `"Listar..."` | description | Descripción legible |
| `"SELECT..."` | pre_sql | Query de precondición |
| `"SELECT..."` | post_sql | Query de postcondición |

**¿Por qué `\` al final?**
Para continuar el comando en múltiples líneas (legibilidad).

---

### SECCIÓN 7: Reporte Final

```bash
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
```

**Cálculo de tasa de éxito:**
```bash
SUCCESS_RATE=$((PASSED * 100 / TOTAL))
```

Ejemplo: Si PASSED=5 y TOTAL=5:
```
SUCCESS_RATE = (5 * 100) / 5 = 500 / 5 = 100
```

**¿Qué es `-eq`?**
Operador de comparación numérica (equal).

| Operador | Significado |
|----------|-------------|
| `-eq` | Igual a (equal) |
| `-ne` | No igual a (not equal) |
| `-lt` | Menor que (less than) |
| `-le` | Menor o igual (less or equal) |
| `-gt` | Mayor que (greater than) |
| `-ge` | Mayor o igual (greater or equal) |

---

## 🧠 Conceptos de Bash Utilizados

### 1. Variables

```bash
# Asignar
nombre="Juan"
edad=25

# Usar
echo $nombre
echo ${nombre}  # Más seguro
```

### 2. Strings

```bash
# Comillas simples (literal)
echo 'Hola $nombre'  # Imprime: Hola $nombre

# Comillas dobles (interpola variables)
echo "Hola $nombre"  # Imprime: Hola Juan
```

### 3. Condicionales

```bash
if [ condición ]; then
    # código si verdadero
elif [ otra_condición ]; then
    # código si otra_condición es verdadera
else
    # código si todo es falso
fi
```

### 4. Funciones

```bash
function mi_funcion() {
    local param1="$1"
    local param2="$2"
    echo "Parámetro 1: $param1"
    echo "Parámetro 2: $param2"
}

# Llamar
mi_funcion "valor1" "valor2"
```

### 5. Command Substitution

```bash
# Ejecutar comando y capturar salida
resultado=$(ls -l)
fecha=$(date +%Y-%m-%d)
```

### 6. Aritmética

```bash
suma=$((5 + 3))
resta=$((10 - 4))
multiplicacion=$((6 * 7))
division=$((20 / 4))
```

### 7. Pipes y Redirección

```bash
# Pipe (|): pasar salida como entrada
cat archivo.txt | grep "buscar" | wc -l

# Redirección de salida (>)
echo "texto" > archivo.txt

# Redirección de errores (2>)
comando 2> errores.log

# Redirección de todo (stdout y stderr)
comando &> todo.log
```

---

## 🔄 Flujo de Ejecución

```
┌─────────────────────────────────────────┐
│ 1. Inicializar variables y colores     │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│ 2. Limpiar pantalla y mostrar título   │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│ 3. LOOP: Para cada prueba               │
│    ┌─────────────────────────────────┐  │
│    │ a. Mostrar encabezado           │  │
│    └────────────┬────────────────────┘  │
│                 ▼                        │
│    ┌─────────────────────────────────┐  │
│    │ b. Ejecutar precondición SQL    │  │
│    └────────────┬────────────────────┘  │
│                 ▼                        │
│    ┌─────────────────────────────────┐  │
│    │ c. Llamar API con curl          │  │
│    └────────────┬────────────────────┘  │
│                 ▼                        │
│    ┌─────────────────────────────────┐  │
│    │ d. Extraer HTTP code y body     │  │
│    └────────────┬────────────────────┘  │
│                 ▼                        │
│    ┌─────────────────────────────────┐  │
│    │ e. Ejecutar postcondición SQL   │  │
│    └────────────┬────────────────────┘  │
│                 ▼                        │
│    ┌─────────────────────────────────┐  │
│    │ f. Validar resultado            │  │
│    └────────────┬────────────────────┘  │
│                 ▼                        │
│    ┌─────────────────────────────────┐  │
│    │ g. Actualizar contadores        │  │
│    └─────────────────────────────────┘  │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│ 4. Calcular estadísticas                │
└──────────────┬──────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│ 5. Mostrar reporte final                │
└─────────────────────────────────────────┘
```

---

## 💡 Ejemplos de Uso

### Uso Básico

```bash
# Ejecutar el script
./test-api-visual-documentado.sh
```

### Guardar Salida en Archivo

```bash
# Guardar en archivo de texto
./test-api-visual-documentado.sh > resultados.txt

# Guardar y ver en pantalla simultáneamente
./test-api-visual-documentado.sh | tee resultados.txt
```

### Filtrar Solo Errores

```bash
# Mostrar solo líneas con "FALLIDO"
./test-api-visual-documentado.sh | grep "FALLIDO"
```

### Ejecutar con Timestamp

```bash
# Agregar timestamp a cada línea
./test-api-visual-documentado.sh | while read line; do
    echo "$(date '+%H:%M:%S') $line"
done
```

---

## 🔧 Personalización

### Agregar una Nueva Prueba

```bash
# Al final de la SECCIÓN 6, agregar:

test_endpoint "6" "GET" "/Productos" "" "200" \
    "Listar todos los productos" \
    "SELECT COUNT(*) as total FROM \"Productos\";" \
    "SELECT COUNT(*) as total FROM \"Productos\";"
```

### Cambiar la Base de Datos

```bash
# En SECCIÓN 1, modificar:
DB_NAME="otra_base"
DB_USER="otro_usuario"
DB_PASSWORD="otra_contraseña"
DB_HOST="192.168.1.100"
```

### Agregar Prueba de PUT

```bash
# Definir datos
PUT_DATA='{
  "nombreEmpresa": "Empresa Actualizada",
  "estadoProspecto": "Calificado"
}'

# Ejecutar prueba
test_endpoint "X" "PUT" "/Prospectos/1" "$PUT_DATA" "200" \
    "Actualizar prospecto" \
    "SELECT \"NombreEmpresa\", \"EstadoProspecto\" FROM \"Prospectos\" WHERE \"Id\" = 1;" \
    "SELECT \"NombreEmpresa\", \"EstadoProspecto\" FROM \"Prospectos\" WHERE \"Id\" = 1;"
```

Pero necesitas modificar la función `test_endpoint` para soportar PUT:

```bash
elif [ "$method" == "PUT" ]; then
    RESPONSE=$(curl -s -w "\n%{http_code}" -X PUT "${API_URL}${endpoint}" \
        -H "Content-Type: application/json" \
        -d "$data")
```

### Agregar Timeout a las Pruebas

```bash
# Modificar las llamadas curl agregando --max-time:
curl -s --max-time 10 -w "\n%{http_code}" "${API_URL}${endpoint}"
```

---

## 🐛 Solución de Problemas

### Problema: "Permission denied"

**Error:**
```
bash: ./test-api-visual-documentado.sh: Permission denied
```

**Solución:**
```bash
chmod +x test-api-visual-documentado.sh
```

---

### Problema: "psql: command not found"

**Error:**
```
psql: command not found
```

**Solución:**
```bash
# Ubuntu/Debian
sudo apt-get install postgresql-client

# macOS
brew install postgresql
```

---

### Problema: "jq: command not found"

**Error:**
```
jq: command not found
```

**Solución:**
```bash
# Ubuntu/Debian
sudo apt-get install jq

# macOS
brew install jq
```

---

### Problema: "curl: command not found"

**Error:**
```
curl: command not found
```

**Solución:**
```bash
# Ubuntu/Debian
sudo apt-get install curl

# macOS (ya viene instalado)
```

---

### Problema: API no responde

**Error:**
```
✗ API no está respondiendo
```

**Solución:**
```bash
# Verificar que la API está corriendo
curl http://localhost:5000/api/Prospectos

# Si no responde, iniciar la API:
cd CRMSystem.API
dotnet run
```

---

### Problema: PostgreSQL no está corriendo

**Error:**
```
✗ PostgreSQL no está corriendo
```

**Solución:**
```bash
# Ubuntu/Debian
sudo service postgresql start

# Verificar estado
sudo service postgresql status
```

---

### Problema: Error de autenticación en PostgreSQL

**Error:**
```
psql: FATAL: password authentication failed for user "crmuser"
```

**Solución:**
```bash
# Verificar credenciales en el script
# Deben coincidir con appsettings.json

# O crear el usuario:
sudo -u postgres psql
CREATE USER crmuser WITH PASSWORD 'crm123456';
GRANT ALL PRIVILEGES ON DATABASE crmdb TO crmuser;
```

---

### Problema: Colores no se muestran

**Síntoma:**
Ves códigos como `\033[0;32m` en lugar de colores.

**Solución:**
Tu terminal no soporta colores ANSI. Usa una terminal moderna como:
- Ubuntu: GNOME Terminal
- macOS: Terminal.app o iTerm2
- Windows: Windows Terminal

---

## 📊 Comparación con Otros Métodos de Testing

| Método | Precondiciones | Postcondiciones | Automatización | Legibilidad |
|--------|----------------|-----------------|----------------|-------------|
| **Manual (Postman)** | ❌ No | ❌ No | ❌ No | ⚠️ Media |
| **Script básico** | ❌ No | ❌ No | ✅ Sí | ⚠️ Media |
| **Este script** | ✅ Sí | ✅ Sí | ✅ Sí | ✅ Alta |
| **Framework (Jest)** | ✅ Sí | ✅ Sí | ✅ Sí | ⚠️ Media |

---

## 🎓 Aprendizajes Clave

### 1. Bash es Poderoso

Con ~150 líneas de código Bash puedes:
- Automatizar pruebas completas
- Interactuar con bases de datos
- Hacer llamadas HTTP
- Formatear salida con colores
- Generar reportes

### 2. Documentación es Esencial

Este script tiene 3 veces más comentarios que código, pero eso:
- Facilita el mantenimiento
- Permite que otros lo entiendan
- Sirve como material de aprendizaje
- Reduce errores al modificar

### 3. Precondiciones/Postcondiciones son Cruciales

Ver el estado de la BD antes y después:
- Verifica que los cambios ocurrieron
- Detecta efectos secundarios no deseados
- Proporciona contexto completo
- Facilita el debugging

### 4. Automatización Ahorra Tiempo

Ejecutar 5 pruebas manualmente: ~10 minutos  
Ejecutar este script: ~10 segundos  
**Ahorro: 98%**

---

## 📚 Recursos Adicionales

### Documentación Oficial

- [Bash Reference Manual](https://www.gnu.org/software/bash/manual/)
- [PostgreSQL psql](https://www.postgresql.org/docs/current/app-psql.html)
- [curl Manual](https://curl.se/docs/manual.html)
- [jq Manual](https://stedolan.github.io/jq/manual/)

### Tutoriales Recomendados

- [Bash Scripting Tutorial](https://www.shellscript.sh/)
- [Advanced Bash-Scripting Guide](https://tldp.org/LDP/abs/html/)
- [Bash Cheat Sheet](https://devhints.io/bash)

---

## ✅ Checklist de Comprensión

Después de leer esta guía, deberías poder:

- [ ] Explicar qué hace cada sección del script
- [ ] Entender la sintaxis de Bash utilizada
- [ ] Modificar las variables de configuración
- [ ] Agregar una nueva prueba
- [ ] Interpretar la salida del script
- [ ] Solucionar problemas comunes
- [ ] Personalizar el script para tus necesidades

---

## 🎉 Conclusión

Este script es:

1. ✅ **Completo** - Cubre precondiciones, API calls y postcondiciones
2. ✅ **Educativo** - Cada línea está explicada
3. ✅ **Práctico** - Funciona en producción
4. ✅ **Extensible** - Fácil de modificar y ampliar
5. ✅ **Profesional** - Sigue mejores prácticas

**¡Ahora tienes el conocimiento para dominarlo!** 🚀

---

**Última actualización:** 2025-11-28  
**Versión:** 1.0  
**Autor:** Sistema CRM

