# CRM API Test Runner - Sistema de Pruebas Python

Sistema de pruebas cross-platform para APIs con soporte completo para precondiciones SQL, llamadas HTTP y postcondiciones SQL.

## Caracteristicas

- **Cross-platform**: Funciona en Windows, Linux y macOS
- **Declarativo**: Define pruebas en archivos de texto simples
- **SQL completo**: Precondiciones y postcondiciones con PostgreSQL
- **HTTP flexible**: Soporta GET, POST, PUT, DELETE
- **Aserciones personalizadas**: Expresiones Python para validar resultados
- **Reportes HTML**: Reportes profesionales con detalles completos
- **Verbose mode**: Modo detallado para debugging

## Requisitos

### Python 3.7+

```bash
python3 --version
```

### Dependencias

```bash
pip3 install psycopg2-binary requests
```

## Instalacion

### Windows

```cmd
# Instalar Python desde python.org
# Abrir PowerShell o CMD

pip install psycopg2-binary requests
```

### Linux/macOS

```bash
pip3 install psycopg2-binary requests
```

## Uso Basico

### 1. Crear archivo de configuracion (config.json)

```json
{
  "db_host": "localhost",
  "db_port": 5432,
  "db_name": "crmdb",
  "db_user": "crmuser",
  "db_password": "crm123456",
  "api_base_url": "http://localhost:5000/api"
}
```

### 2. Crear archivo de pruebas (tests.txt)

```
### Nombre del Test
# Descripcion opcional del test
# Puede tener multiples lineas de comentarios

pre_sql:
SELECT COUNT(*) as total FROM "Prospectos"

curl:
curl -X 'GET' 'http://localhost:5000/api/Prospectos' -H 'accept: application/json'

post_sql:
SELECT COUNT(*) as total FROM "Prospectos"

assert:
status_code == 200 and len(json) > 0

---

### Otro Test
...
---
```

### 3. Ejecutar pruebas

```bash
# Basico
python3 test_runner.py tests.txt

# Con configuracion personalizada
python3 test_runner.py tests.txt --config config.json

# Modo verbose
python3 test_runner.py tests.txt --verbose

# Especificar archivo de salida HTML
python3 test_runner.py tests.txt --output mi_reporte.html
```

## Formato de Archivo de Pruebas

### Estructura General

```
### Nombre del Test
Descripcion opcional

pre_sql:
  Query SQL antes de la prueba

curl:
  Comando curl completo

post_sql:
  Query SQL despues de la prueba

assert:
  Expresion Python para validar

---
```

### Secciones

#### 1. Nombre (Requerido)

```
### Test GET Prospectos
```

- Debe empezar con `###`
- Es el titulo que aparecera en los reportes

#### 2. Descripcion (Opcional)

```
# Verificar que el endpoint devuelve todos los prospectos
# Esta es una linea adicional de descripcion
```

- Lineas que empiezan con `#`
- Se muestran en el reporte HTML

#### 3. pre_sql (Opcional)

```
pre_sql:
SELECT "Id", "CodigoProspecto" FROM "Prospectos" ORDER BY "Id"
```

- Query SQL a ejecutar ANTES de la llamada HTTP
- Util para verificar estado inicial
- Los resultados estan disponibles en la asercion como `pre_sql`

#### 4. curl (Requerido)

```
curl:
curl -X 'GET' 'http://localhost:5000/api/Prospectos' -H 'accept: application/json'
```

- Comando curl completo
- Se parsea automaticamente para extraer:
  - Metodo HTTP (`-X 'GET'`)
  - URL (`'http://...'`)
  - Headers (`-H 'nombre: valor'`)
  - Body (`-d '...'`)

**Ejemplos:**

```
# GET
curl:
curl -X 'GET' 'http://localhost:5000/api/Prospectos' -H 'accept: application/json'

# POST con JSON
curl:
curl -X 'POST' 'http://localhost:5000/api/Prospectos' -H 'Content-Type: application/json' -d '{"nombre":"Test","email":"test@test.com"}'

# PUT
curl:
curl -X 'PUT' 'http://localhost:5000/api/Prospectos/1' -H 'Content-Type: application/json' -d '{"nombre":"Actualizado"}'

# DELETE
curl:
curl -X 'DELETE' 'http://localhost:5000/api/Prospectos/1'
```

#### 5. post_sql (Opcional)

```
post_sql:
SELECT COUNT(*) as total FROM "Prospectos"
```

- Query SQL a ejecutar DESPUES de la llamada HTTP
- Util para verificar cambios en la base de datos
- Los resultados estan disponibles en la asercion como `post_sql`

#### 6. assert (Opcional)

```
assert:
status_code == 200 and len(json) > 0
```

- Expresion Python que debe evaluar a `True` para que la prueba pase
- Variables disponibles:
  - `pre_sql`: Lista de diccionarios con resultados de pre_sql
  - `post_sql`: Lista de diccionarios con resultados de post_sql
  - `response`: Objeto Response de requests
  - `status_code`: Codigo HTTP de la respuesta
  - `json`: response.json() si la respuesta es JSON
  - `len()`: Funcion para obtener longitud
  - `str()`, `int()`, `float()`: Funciones de conversion

**Ejemplos de aserciones:**

```
# Verificar codigo HTTP
assert:
status_code == 200

# Verificar que retorna datos
assert:
len(json) > 0

# Verificar campo especifico
assert:
json['id'] == 1 and json['nombre'] == 'Test'

# Verificar que se creo un registro
assert:
post_sql[0]['total'] == pre_sql[0]['total'] + 1

# Verificar multiples condiciones
assert:
status_code == 201 and json['codigoProspecto'] is not None and len(post_sql) > 0

# Comparar con resultado SQL
assert:
len(json) == post_sql[0]['total_prospectos']
```

### Separador de Pruebas

```
---
```

- Tres guiones separan una prueba de otra
- Debe estar en su propia linea

## Ejemplos Completos

### Ejemplo 1: GET Simple

```
### Listar Prospectos
# Obtiene todos los prospectos de la base de datos

pre_sql:
SELECT COUNT(*) as total FROM "Prospectos"

curl:
curl -X 'GET' 'http://localhost:5000/api/Prospectos' -H 'accept: application/json'

post_sql:
SELECT COUNT(*) as total FROM "Prospectos"

assert:
status_code == 200 and len(json) == pre_sql[0]['total']

---
```

### Ejemplo 2: POST con Validacion

```
### Crear Prospecto
# Crea un nuevo prospecto y verifica que se guardo correctamente

pre_sql:
SELECT COUNT(*) as total_antes FROM "Prospectos"

curl:
curl -X 'POST' 'http://localhost:5000/api/Prospectos' -H 'Content-Type: application/json' -d '{"nombreEmpresa":"Test","nombreContacto":"Juan","apellidoContacto":"Perez","email":"juan@test.com","telefono":"+52-55-1234-5678","fuenteId":1,"sucursalId":1,"vendedorAsignadoId":1,"estadoProspecto":"Nuevo","prioridad":"Alta","valorEstimado":100000,"probabilidadCierre":80}'

post_sql:
SELECT COUNT(*) as total_despues, MAX("Id") as ultimo_id FROM "Prospectos"

assert:
status_code == 201 and post_sql[0]['total_despues'] == pre_sql[0]['total_antes'] + 1

---
```

### Ejemplo 3: GET con Parametros

```
### Buscar Prospectos por Estado
# Filtra prospectos por estado

pre_sql:
SELECT COUNT(*) as total FROM "Prospectos" WHERE "EstadoProspecto" = 'Nuevo'

curl:
curl -X 'GET' 'http://localhost:5000/api/Prospectos?estado=Nuevo' -H 'accept: application/json'

post_sql:
SELECT COUNT(*) as total FROM "Prospectos" WHERE "EstadoProspecto" = 'Nuevo'

assert:
status_code == 200 and len(json) == pre_sql[0]['total']

---
```

### Ejemplo 4: Validacion Compleja

```
### Verificar Embudo de Ventas
# Valida que las estadisticas del embudo sean correctas

pre_sql:
SELECT "EstadoProspecto", COUNT(*) as total FROM "Prospectos" GROUP BY "EstadoProspecto"

curl:
curl -X 'GET' 'http://localhost:5000/api/Prospectos/embudo-ventas' -H 'accept: application/json'

post_sql:
SELECT COUNT(DISTINCT "EstadoProspecto") as estados_distintos FROM "Prospectos"

assert:
status_code == 200 and len(json) > 0 and len(json) <= post_sql[0]['estados_distintos']

---
```

## Opciones de Linea de Comandos

```
usage: test_runner.py [-h] [--config CONFIG] [--verbose] [--output OUTPUT] testfile

CRM API Test Runner

positional arguments:
  testfile              Archivo de definicion de pruebas

optional arguments:
  -h, --help            show this help message and exit
  --config CONFIG       Archivo de configuracion JSON
  --verbose, -v         Modo verbose
  --output OUTPUT, -o OUTPUT
                        Archivo de salida HTML
```

### Ejemplos

```bash
# Basico
python3 test_runner.py tests.txt

# Con configuracion personalizada
python3 test_runner.py tests.txt --config prod_config.json

# Modo verbose para debugging
python3 test_runner.py tests.txt -v

# Guardar reporte con nombre personalizado
python3 test_runner.py tests.txt -o reporte_$(date +%Y%m%d).html

# Todo junto
python3 test_runner.py tests.txt --config config.json --verbose --output mi_reporte.html
```

## Salida

### Consola

```
[INFO] Parseando archivo: tests.txt
[INFO] 9 pruebas encontradas
[OK] Conectado a PostgreSQL: crmdb

[1/9] Test GET Prospectos
  [SQL PRE] SELECT "Id", "CodigoProspecto"...
  [HTTP] GET http://localhost:5000/api/Prospectos
  [SQL POST] SELECT COUNT(*) as total...
  [PASS] Asercion pasada

[2/9] Test POST Crear Prospecto
  [SQL PRE] SELECT COUNT(*) as total_antes...
  [HTTP] POST http://localhost:5000/api/Prospectos
  [SQL POST] SELECT COUNT(*) as total_despues...
  [PASS] Asercion pasada

...

[OK] Desconectado de PostgreSQL

======================================================================
REPORTE FINAL
======================================================================

Total de pruebas:     9
Pruebas exitosas:     9
Pruebas fallidas:     0
Tasa de exito:        100.0%

======================================================================

[OK] Reporte HTML generado: test_report.html
```

### Reporte HTML

El reporte HTML incluye:

- **Resumen ejecutivo**: Total, exitosas, fallidas, tasa de exito
- **Detalles de cada prueba**: Expandibles con click
- **Precondiciones SQL**: Query y resultados
- **Request HTTP**: Metodo, URL, headers, body
- **Response HTTP**: Status code, body JSON
- **Postcondiciones SQL**: Query y resultados
- **Aserciones**: Expresion y resultado
- **Colores**: Verde para PASS, rojo para FAIL

## Integracion con CI/CD

### GitHub Actions

```yaml
name: API Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    services:
      postgres:
        image: postgres:14
        env:
          POSTGRES_DB: crmdb
          POSTGRES_USER: crmuser
          POSTGRES_PASSWORD: crm123456
        ports:
          - 5432:5432
    
    steps:
      - uses: actions/checkout@v2
      
      - name: Setup Python
        uses: actions/setup-python@v2
        with:
          python-version: '3.9'
      
      - name: Install dependencies
        run: pip install psycopg2-binary requests
      
      - name: Run API
        run: dotnet run &
        working-directory: ./CRMSystem.API
      
      - name: Wait for API
        run: sleep 10
      
      - name: Run tests
        run: python3 test_runner.py tests.txt --verbose
        working-directory: ./CRMSystem
      
      - name: Upload report
        uses: actions/upload-artifact@v2
        with:
          name: test-report
          path: CRMSystem/test_report.html
```

### GitLab CI

```yaml
test:
  image: python:3.9
  services:
    - postgres:14
  variables:
    POSTGRES_DB: crmdb
    POSTGRES_USER: crmuser
    POSTGRES_PASSWORD: crm123456
  script:
    - pip install psycopg2-binary requests
    - cd CRMSystem
    - python3 test_runner.py tests.txt --verbose
  artifacts:
    paths:
      - CRMSystem/test_report.html
    expire_in: 1 week
```

## Troubleshooting

### Error: ModuleNotFoundError: No module named 'psycopg2'

**Solucion:**
```bash
pip3 install psycopg2-binary
```

### Error: No se puede conectar a PostgreSQL

**Verificar:**
1. PostgreSQL esta corriendo: `sudo service postgresql status`
2. Base de datos existe: `psql -U postgres -l`
3. Credenciales correctas en config.json
4. Puerto correcto (default: 5432)

### Error: Connection refused al llamar API

**Verificar:**
1. API esta corriendo: `curl http://localhost:5000/api/Prospectos`
2. Puerto correcto en config.json
3. URL correcta (con /api)

### Prueba falla pero deberia pasar

**Debugging:**
1. Ejecutar con `--verbose` para ver detalles
2. Verificar la asercion con `print()`
3. Revisar pre_sql y post_sql en el reporte HTML
4. Verificar que la respuesta JSON tiene la estructura esperada

### Encoding issues en Windows

**Solucion:**
Guardar archivos .txt con encoding UTF-8:
- Notepad++: Encoding > UTF-8
- VS Code: Save with Encoding > UTF-8

## Ventajas vs Bash

| Caracteristica | Bash | Python |
|----------------|------|--------|
| **Cross-platform** | No (solo Linux/macOS) | Si (Windows/Linux/macOS) |
| **Encoding** | Problemas con UTF-8 | Sin problemas |
| **Debugging** | Dificil | Facil con --verbose |
| **Reportes HTML** | Complejo | Incluido |
| **Aserciones** | Limitadas | Expresiones Python completas |
| **Mantenibilidad** | Media | Alta |
| **Legibilidad** | Media | Alta |
| **CI/CD** | Problemas posibles | Sin problemas |

## Mejores Practicas

### 1. Organizar pruebas por modulo

```
tests_prospectos.txt
tests_clientes.txt
tests_cotizaciones.txt
```

### 2. Usar nombres descriptivos

```
### Test GET Prospectos - Listar todos
### Test POST Prospectos - Crear con datos validos
### Test POST Prospectos - Fallar con email invalido
```

### 3. Verificar precondiciones y postcondiciones

```
pre_sql:
SELECT COUNT(*) as total_antes FROM "Prospectos"

post_sql:
SELECT COUNT(*) as total_despues FROM "Prospectos"

assert:
post_sql[0]['total_despues'] == pre_sql[0]['total_antes'] + 1
```

### 4. Probar casos de error

```
### Test POST Prospecto - Email invalido
curl:
curl -X 'POST' '...' -d '{"email":"invalido"}'

assert:
status_code == 400
```

### 5. Usar configuraciones por ambiente

```
config_dev.json
config_staging.json
config_prod.json
```

```bash
python3 test_runner.py tests.txt --config config_staging.json
```

## Licencia

MIT

## Autor

Sistema CRM - 2025

## Soporte

Para problemas o preguntas, crear un issue en el repositorio.

