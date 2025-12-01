# Test Runner v2.0 - Documentacion Completa

Sistema de pruebas avanzado con soporte para SQL Server y PostgreSQL, pasos multiples numerados, interpolacion de variables y salida detallada en consola.

**Fecha**: 1 de Diciembre 2025  
**Version**: 2.0  
**Autor**: Sistema CRM Team

---

## Caracteristicas Principales

### 1. Soporte Dual de Bases de Datos

- **PostgreSQL** - Base de datos principal del CRM
- **SQL Server** - Base de datos secundaria para sincronizacion

### 2. Pasos Multiples Numerados

- `tsql_1`, `tsql_2`, `tsql_3`, ... (SQL Server)
- `psql_1`, `psql_2`, `psql_3`, ... (PostgreSQL)
- `curl_1`, `curl_2`, `curl_3`, ... (API calls)

### 3. Interpolacion de Variables

Usa resultados de pasos anteriores en pasos posteriores:

```
psql_1:
SELECT MAX("Id") as ultimo_id FROM "Productos"

curl_1:
curl -X 'GET' 'http://localhost:5000/api/Productos/{psql_1["ultimo_id"]}'

psql_2:
SELECT * FROM "Productos" WHERE "Id" = {psql_1["ultimo_id"]}
```

### 4. Aserciones Avanzadas

```python
assert:
tsql_1["total"] == psql_1["total"] and curl_1["status_code"] == 200
```

### 5. Salida Detallada en Consola

Todos los resultados SQL y API se imprimen en pantalla con formato JSON.

### 6. Manejo de Errores

- Si un paso falla, el test se marca como FAIL
- Se continua con el siguiente test
- No se ejecutan pasos restantes del test fallido

---

## Instalacion

### Requisitos

- Python 3.7+
- PostgreSQL 14+
- SQL Server 2019+ (opcional)

### Dependencias Python

```bash
# Obligatorias
pip3 install psycopg2-binary requests

# Opcionales (para SQL Server)
pip3 install pymssql
```

---

## Configuracion

### Archivo config.json

```json
{
  "postgresql": {
    "host": "localhost",
    "port": 5432,
    "database": "crmdb",
    "user": "crmuser",
    "password": "crm123456"
  },
  "sqlserver": {
    "host": "localhost",
    "port": 1433,
    "database": "CRMDB",
    "user": "sa",
    "password": "YourStrong!Passw0rd"
  },
  "api_base_url": "http://localhost:5000/api"
}
```

**Nota**: SQL Server es opcional. El sistema funciona solo con PostgreSQL.

---

## Formato de Archivos de Prueba

### Estructura Basica

```
### Nombre del Test
# Descripcion del test

psql_1:
SELECT COUNT(*) as total FROM "Tabla"

curl_1:
curl -X 'GET' 'http://localhost:5000/api/Endpoint' -H 'accept: application/json'

assert:
psql_1["total"] > 0 and curl_1["status_code"] == 200

---
```

### Elementos

| Elemento | Descripcion |
|----------|-------------|
| `###` | Nombre del test |
| `#` | Comentarios/descripcion |
| `tsql_N:` | Query SQL Server (paso N) |
| `psql_N:` | Query PostgreSQL (paso N) |
| `curl_N:` | Llamada API (paso N) |
| `assert:` | Expresion de validacion |
| `---` | Separador de tests |

### Orden de Ejecucion

Los pasos se ejecutan **en el orden que aparecen en el archivo**, NO por numero.

Ejemplo:
```
tsql_1:     <- Se ejecuta primero
psql_1:     <- Se ejecuta segundo
curl_1:     <- Se ejecuta tercero
psql_2:     <- Se ejecuta cuarto
curl_2:     <- Se ejecuta quinto
tsql_2:     <- Se ejecuta sexto
assert:     <- Se ejecuta ultimo
```

---

## Interpolacion de Variables

### Sintaxis

```
{paso_nombre["campo"]}
```

### Ejemplos

#### Ejemplo 1: Usar ID de query en API call

```
psql_1:
SELECT MAX("Id") as ultimo_id FROM "Productos"

curl_1:
curl -X 'GET' 'http://localhost:5000/api/Productos/{psql_1["ultimo_id"]}'
```

#### Ejemplo 2: Usar ID de API en query

```
curl_1:
curl -X 'POST' 'http://localhost:5000/api/Clientes' -H 'Content-Type: application/json' -d '{"nombreEmpresa":"Test"}'

psql_1:
SELECT * FROM "Clientes" WHERE "Id" = {curl_1["id"]}
```

#### Ejemplo 3: Cadena de interpolaciones

```
psql_1:
SELECT "Id" as cliente_id FROM "Clientes" LIMIT 1

curl_1:
curl -X 'GET' 'http://localhost:5000/api/Clientes/{psql_1["cliente_id"]}'

psql_2:
SELECT COUNT(*) as total_contactos FROM "ContactosCliente" WHERE "ClienteId" = {psql_1["cliente_id"]}

curl_2:
curl -X 'GET' 'http://localhost:5000/api/Clientes/{psql_1["cliente_id"]}/contactos'

assert:
psql_2["total_contactos"] == len(curl_2)
```

---

## Aserciones

### Sintaxis

Las aserciones son expresiones Python que se evaluan con los resultados de todos los pasos.

### Variables Disponibles

Todos los resultados de pasos anteriores estan disponibles:

- `tsql_1`, `tsql_2`, ... (diccionarios con resultados SQL Server)
- `psql_1`, `psql_2`, ... (diccionarios con resultados PostgreSQL)
- `curl_1`, `curl_2`, ... (diccionarios con resultados API)

### Operadores

```python
# Comparacion
==, !=, <, >, <=, >=

# Logicos
and, or, not

# Pertenencia
in, not in

# Funciones
len(), str(), int(), float()
```

### Ejemplos de Aserciones

```python
# Comparacion simple
assert:
psql_1["total"] > 0

# Comparacion entre bases de datos
assert:
tsql_1["total_sqlserver"] == psql_1["total_postgres"]

# Verificar API y BD
assert:
curl_1["id"] == psql_1["ultimo_id"]

# Multiples condiciones
assert:
psql_1["total"] > 0 and curl_1["status_code"] == 200 and len(curl_1) > 0

# Comparacion de strings
assert:
curl_1["nombreEmpresa"] == psql_1["NombreEmpresa"]

# Verificar incremento
assert:
psql_2["total"] == psql_1["total"] + 1

# Verificar array
assert:
len(curl_1) == psql_1["total"]
```

---

## Uso

### Ejecutar Pruebas

```bash
# Basico
python3 test_runner_v2.py tests_simple_v2.txt

# Con configuracion personalizada
python3 test_runner_v2.py tests_simple_v2.txt --config mi_config.json

# Modo verbose (futuro)
python3 test_runner_v2.py tests_simple_v2.txt --verbose
```

### Salida en Consola

```
[INFO] Parseando archivo: tests_simple_v2.txt
[INFO] 5 pruebas encontradas
[OK] Conectado a PostgreSQL: crmdb
[OK] Conectado a SQL Server: CRMDB

[1/5] Test Contar Productos
Verificar que hay productos en la base de datos

  [PSQL_1] SELECT COUNT(*) as total FROM "Productos"...
  Resultado: {
    "total": 8
  }

  [ASSERT] psql_1["total"] > 0
  Resultado: True

  [PASS] Test exitoso

[2/5] Test Obtener Producto por ID
...

======================================================================
REPORTE FINAL
======================================================================
Total de pruebas:     5
Pruebas exitosas:     5
Pruebas fallidas:     0
Tasa de exito:        100.0%
======================================================================

[OK] Desconectado de PostgreSQL
[OK] Desconectado de SQL Server
```

---

## Ejemplos Completos

### Ejemplo 1: Test Simple

```
### Test Contar Productos
# Verificar que hay productos en la base de datos

psql_1:
SELECT COUNT(*) as total FROM "Productos"

assert:
psql_1["total"] > 0

---
```

### Ejemplo 2: Test con API

```
### Test Obtener Productos Activos
# Listar productos activos y verificar conteo

psql_1:
SELECT COUNT(*) as total_activos FROM "Productos" WHERE "EstaActivo" = true

curl_1:
curl -X 'GET' 'http://localhost:5000/api/Productos?activo=true' -H 'accept: application/json'

assert:
psql_1["total_activos"] > 0 and curl_1["status_code"] == 200

---
```

### Ejemplo 3: Test con Interpolacion

```
### Test Obtener Producto por ID Dinamico
# Obtener el ultimo producto creado

psql_1:
SELECT MAX("Id") as ultimo_id FROM "Productos"

curl_1:
curl -X 'GET' 'http://localhost:5000/api/Productos/{psql_1["ultimo_id"]}' -H 'accept: application/json'

psql_2:
SELECT "NombreProducto" FROM "Productos" WHERE "Id" = {psql_1["ultimo_id"]}

assert:
curl_1["id"] == psql_1["ultimo_id"] and curl_1["nombreProducto"] == psql_2["NombreProducto"]

---
```

### Ejemplo 4: Test Dual Database

```
### Test Sincronizacion SQL Server y PostgreSQL
# Verificar que ambas BD tienen el mismo numero de clientes

tsql_1:
SELECT COUNT(*) as total_sqlserver FROM Clientes WHERE Activo = 1

psql_1:
SELECT COUNT(*) as total_postgres FROM "Clientes" WHERE "EstaActivo" = true

assert:
tsql_1["total_sqlserver"] == psql_1["total_postgres"]

---
```

### Ejemplo 5: Test Complejo con Multiples Pasos

```
### Test Flujo Completo de Cliente
# Crear cliente, agregar contacto, verificar en ambas BD

psql_1:
SELECT COUNT(*) as total_antes FROM "Clientes"

curl_1:
curl -X 'POST' 'http://localhost:5000/api/Clientes' -H 'Content-Type: application/json' -d '{"nombreEmpresa":"Test","categoriaId":1,"sucursalId":1}'

psql_2:
SELECT "NombreEmpresa" FROM "Clientes" WHERE "Id" = {curl_1["id"]}

curl_2:
curl -X 'POST' 'http://localhost:5000/api/Clientes/{curl_1["id"]}/contactos' -H 'Content-Type: application/json' -d '{"nombreCompleto":"Juan Lopez","email":"juan@test.com"}'

psql_3:
SELECT COUNT(*) as total_contactos FROM "ContactosCliente" WHERE "ClienteId" = {curl_1["id"]}

psql_4:
SELECT COUNT(*) as total_despues FROM "Clientes"

tsql_1:
SELECT COUNT(*) as total_sqlserver FROM Clientes

assert:
psql_4["total_despues"] == psql_1["total_antes"] + 1 and psql_3["total_contactos"] == 1 and curl_1["nombreEmpresa"] == "Test"

---
```

---

## Resultados SQL

### Formato

Los resultados SQL se devuelven como **diccionarios con la primera fila**.

### Ejemplo

```sql
SELECT "Id", "NombreProducto", "PrecioLista" FROM "Productos" WHERE "Id" = 1
```

Resultado:
```python
{
  "Id": 1,
  "NombreProducto": "Servidor Dell R740",
  "PrecioLista": "45000.00"
}
```

### Acceso a Campos

```python
psql_1["Id"]              # 1
psql_1["NombreProducto"]  # "Servidor Dell R740"
psql_1["PrecioLista"]     # "45000.00"
```

### Query sin Resultados

Si el query no devuelve filas, el resultado es un diccionario vacio `{}`.

```python
assert:
len(psql_1) == 0  # True si no hay resultados
```

---

## Resultados API

### Formato

Los resultados API incluyen el status code y el body parseado como JSON.

### Ejemplo

```bash
curl -X 'GET' 'http://localhost:5000/api/Productos/1'
```

Resultado:
```python
{
  "status_code": 200,
  "id": 1,
  "codigoProducto": "HW-001",
  "nombreProducto": "Servidor Dell R740",
  "precioLista": 45000.00
}
```

### Acceso a Campos

```python
curl_1["status_code"]     # 200
curl_1["id"]              # 1
curl_1["nombreProducto"]  # "Servidor Dell R740"
```

### Arrays

Si la API devuelve un array, el resultado es una lista:

```python
curl_1 = [
  {"id": 1, "nombre": "Producto 1"},
  {"id": 2, "nombre": "Producto 2"}
]

# Acceso
len(curl_1)        # 2
curl_1[0]["id"]    # 1 (NO SOPORTADO en aserciones directamente)
```

Para arrays, usa `len()`:
```python
assert:
len(curl_1) > 0 and curl_1["status_code"] == 200
```

---

## Manejo de Errores

### Error en Paso SQL

Si un query SQL falla:
- Se imprime el error en consola
- Se hace rollback de la transaccion
- El test se marca como FAIL
- Se continua con el siguiente test

### Error en Paso API

Si una llamada API falla:
- Se imprime el error en consola
- El test se marca como FAIL
- Se continua con el siguiente test

### Error en Asercion

Si una asercion falla:
- Se imprime el resultado (False)
- El test se marca como FAIL
- Se continua con el siguiente test

---

## Archivos de Ejemplo

### tests_simple_v2.txt

Pruebas basicas sin interpolacion:
- Contar productos
- Obtener productos activos
- Obtener producto por ID
- Obtener prospectos
- Obtener fuentes

**Resultado**: 5/5 pruebas exitosas

### tests_interpolation.txt

Pruebas con interpolacion de variables:
- Obtener producto por ID dinamico
- Obtener prospecto por ID dinamico
- Multiples interpolaciones

**Resultado**: 3/3 pruebas exitosas

### tests_sync_example.txt

Pruebas de sincronizacion dual database:
- Comparar conteos entre SQL Server y PostgreSQL
- Crear en PostgreSQL y verificar en SQL Server
- Flujos complejos

**Nota**: Requiere SQL Server configurado

---

## Comparacion con Version 1.0

| Caracteristica | v1.0 | v2.0 |
|----------------|------|------|
| **Bases de datos** | Solo PostgreSQL | PostgreSQL + SQL Server |
| **Pasos** | pre_sql, curl, post_sql | tsql_N, psql_N, curl_N |
| **Interpolacion** | No | Si |
| **Orden de ejecucion** | Fijo | Flexible (orden en archivo) |
| **Salida** | Basica | Detallada con JSON |
| **Manejo de errores** | Basico | Rollback automatico |

---

## Mejores Practicas

### 1. Nombres Descriptivos

```
### Test Crear Cliente y Verificar en Ambas BD
# NO: Test 1
```

### 2. Comentarios Claros

```
# Verificar que el cliente se creo correctamente
# NO: # Test
```

### 3. Aserciones Especificas

```
assert:
curl_1["id"] == psql_1["ultimo_id"] and curl_1["nombreEmpresa"] == "Test"
# NO: assert: True
```

### 4. Usar Interpolacion

```
psql_1:
SELECT MAX("Id") as ultimo_id FROM "Productos"

curl_1:
curl -X 'GET' 'http://localhost:5000/api/Productos/{psql_1["ultimo_id"]}'
# NO: curl -X 'GET' 'http://localhost:5000/api/Productos/999'
```

### 5. Verificar Pre y Post Condiciones

```
psql_1:
SELECT COUNT(*) as total_antes FROM "Clientes"

curl_1:
curl -X 'POST' 'http://localhost:5000/api/Clientes' ...

psql_2:
SELECT COUNT(*) as total_despues FROM "Clientes"

assert:
psql_2["total_despues"] == psql_1["total_antes"] + 1
```

---

## Troubleshooting

### Error: "pymssql no instalado"

**Solucion**:
```bash
pip3 install pymssql
```

O ignorar si no necesitas SQL Server.

### Error: "No se pudo conectar a PostgreSQL"

**Verificar**:
1. PostgreSQL esta corriendo: `sudo service postgresql status`
2. Credenciales en config.json son correctas
3. Base de datos existe: `psql -l | grep crmdb`

### Error: "syntax error at or near {"

**Causa**: Interpolacion no funciono (variable no existe)

**Solucion**: Verificar que el paso anterior se ejecuto correctamente y devolvio el campo esperado.

### Error: "current transaction is aborted"

**Causa**: Error anterior en la transaccion

**Solucion**: Ya esta corregido en v2.0 con rollback automatico.

---

## Roadmap

### Version 2.1 (Futuro)

- [ ] Soporte para MySQL
- [ ] Reporte HTML generado automaticamente
- [ ] Modo verbose con mas detalles
- [ ] Soporte para variables de entorno
- [ ] Timeout configurable por paso

### Version 2.2 (Futuro)

- [ ] Soporte para MongoDB
- [ ] Paralelizacion de tests
- [ ] Integracion con CI/CD
- [ ] Dashboard web de resultados

---

## Contribuciones

Para reportar bugs o sugerir mejoras, crear un issue en el repositorio.

---

## Licencia

MIT License

---

**Ultima Actualizacion**: 1 de Diciembre 2025  
**Version**: 2.0  
**Autor**: Sistema CRM Team

