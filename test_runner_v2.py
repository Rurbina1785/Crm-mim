#!/usr/bin/env python3
"""
CRM API Test Runner v2.0
Sistema de pruebas con soporte para SQL Server y PostgreSQL
Soporta pasos multiples numerados (tsql_1, psql_1, curl_1, etc.)
"""

import re
import json
import argparse
import sys
from datetime import datetime
from typing import Dict, List, Any, Optional, Tuple
import requests

# Colores para terminal
class Colors:
    CYAN = '\033[96m'
    GREEN = '\033[92m'
    YELLOW = '\033[93m'
    RED = '\033[91m'
    BOLD = '\033[1m'
    END = '\033[0m'

def print_color(text: str, color: str):
    """Imprime texto con color"""
    print(f"{color}{text}{Colors.END}")

def print_section(text: str):
    """Imprime seccion principal"""
    print(f"\n{Colors.BOLD}{Colors.CYAN}{'='*70}{Colors.END}")
    print(f"{Colors.BOLD}{Colors.CYAN}{text}{Colors.END}")
    print(f"{Colors.BOLD}{Colors.CYAN}{'='*70}{Colors.END}\n")

class DatabaseConnection:
    """Maneja conexiones a PostgreSQL y SQL Server"""
    
    def __init__(self, config: Dict[str, Any]):
        self.config = config
        self.pg_conn = None
        self.mssql_conn = None
        
    def connect_postgresql(self):
        """Conecta a PostgreSQL"""
        try:
            import psycopg2
            pg_config = self.config.get('postgresql', {})
            self.pg_conn = psycopg2.connect(
                host=pg_config.get('host', 'localhost'),
                port=pg_config.get('port', 5432),
                database=pg_config.get('database', 'crmdb'),
                user=pg_config.get('user', 'crmuser'),
                password=pg_config.get('password', 'crm123456')
            )
            print_color(f"[OK] Conectado a PostgreSQL: {pg_config.get('database')}", Colors.GREEN)
            return True
        except Exception as e:
            print_color(f"[ERROR] No se pudo conectar a PostgreSQL: {e}", Colors.RED)
            return False
    
    def connect_sqlserver(self):
        """Conecta a SQL Server"""
        try:
            import pymssql
            mssql_config = self.config.get('sqlserver', {})
            self.mssql_conn = pymssql.connect(
                server=mssql_config.get('host', 'localhost'),
                port=mssql_config.get('port', 1433),
                database=mssql_config.get('database', 'CRMDB'),
                user=mssql_config.get('user', 'sa'),
                password=mssql_config.get('password', 'YourStrong!Passw0rd')
            )
            print_color(f"[OK] Conectado a SQL Server: {mssql_config.get('database')}", Colors.GREEN)
            return True
        except ImportError:
            print_color("[WARN] pymssql no instalado. Instalar con: pip3 install pymssql", Colors.YELLOW)
            return False
        except Exception as e:
            print_color(f"[ERROR] No se pudo conectar a SQL Server: {e}", Colors.RED)
            return False
    
    def execute_postgresql(self, query: str) -> Optional[Dict[str, Any]]:
        """Ejecuta query en PostgreSQL y retorna primera fila como dict"""
        if not self.pg_conn:
            return None
        try:
            cursor = self.pg_conn.cursor()
            cursor.execute(query)
            
            # Si es SELECT, obtener resultados
            if query.strip().upper().startswith('SELECT'):
                columns = [desc[0] for desc in cursor.description]
                row = cursor.fetchone()
                if row:
                    return dict(zip(columns, row))
                return {}
            else:
                self.pg_conn.commit()
                return {"affected_rows": cursor.rowcount}
        except Exception as e:
            print_color(f"  [ERROR] PostgreSQL: {e}", Colors.RED)
            self.pg_conn.rollback()  # Rollback en caso de error
            return None
        finally:
            cursor.close()
    
    def execute_sqlserver(self, query: str) -> Optional[Dict[str, Any]]:
        """Ejecuta query en SQL Server y retorna primera fila como dict"""
        if not self.mssql_conn:
            return None
        try:
            cursor = self.mssql_conn.cursor(as_dict=True)
            cursor.execute(query)
            
            # Si es SELECT, obtener resultados
            if query.strip().upper().startswith('SELECT'):
                row = cursor.fetchone()
                if row:
                    return dict(row)
                return {}
            else:
                self.mssql_conn.commit()
                return {"affected_rows": cursor.rowcount}
        except Exception as e:
            print_color(f"  [ERROR] SQL Server: {e}", Colors.RED)
            self.mssql_conn.rollback()  # Rollback en caso de error
            return None
        finally:
            cursor.close()
    
    def close(self):
        """Cierra conexiones"""
        if self.pg_conn:
            self.pg_conn.close()
            print_color("[OK] Desconectado de PostgreSQL", Colors.GREEN)
        if self.mssql_conn:
            self.mssql_conn.close()
            print_color("[OK] Desconectado de SQL Server", Colors.GREEN)

class TestParser:
    """Parser para archivos de pruebas"""
    
    @staticmethod
    def parse_file(filepath: str) -> List[Dict[str, Any]]:
        """Parsea archivo de pruebas y retorna lista de tests"""
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()
        
        tests = []
        test_blocks = content.split('---')
        
        for block in test_blocks:
            block = block.strip()
            if not block:
                continue
            
            test = TestParser.parse_test_block(block)
            if test:
                tests.append(test)
        
        return tests
    
    @staticmethod
    def parse_test_block(block: str) -> Optional[Dict[str, Any]]:
        """Parsea un bloque de test individual"""
        lines = block.split('\n')
        
        test = {
            'name': '',
            'description': '',
            'steps': [],  # Lista de pasos en orden
            'assert': ''
        }
        
        current_step = None
        current_content = []
        
        for line in lines:
            line_stripped = line.strip()
            
            # Nombre del test
            if line_stripped.startswith('###'):
                test['name'] = line_stripped[3:].strip()
                continue
            
            # Descripcion
            if line_stripped.startswith('#') and not test['name']:
                continue
            if line_stripped.startswith('#'):
                test['description'] += line_stripped[1:].strip() + ' '
                continue
            
            # Detectar inicio de paso: tsql_N, psql_N, curl_N
            step_match = re.match(r'^(tsql|psql|curl)_(\d+):', line_stripped)
            if step_match:
                # Guardar paso anterior si existe
                if current_step:
                    current_step['content'] = '\n'.join(current_content).strip()
                    test['steps'].append(current_step)
                
                # Nuevo paso
                step_type = step_match.group(1)
                step_num = int(step_match.group(2))
                current_step = {
                    'type': step_type,
                    'number': step_num,
                    'name': f"{step_type}_{step_num}",
                    'content': ''
                }
                current_content = []
                continue
            
            # Asercion
            if line_stripped.startswith('assert:'):
                # Guardar paso anterior si existe
                if current_step:
                    current_step['content'] = '\n'.join(current_content).strip()
                    test['steps'].append(current_step)
                    current_step = None
                    current_content = []
                continue
            
            # Contenido del paso o asercion
            if current_step:
                current_content.append(line)
            elif line_stripped and not line_stripped.startswith('#'):
                test['assert'] += line_stripped + ' '
        
        # Guardar ultimo paso
        if current_step:
            current_step['content'] = '\n'.join(current_content).strip()
            test['steps'].append(current_step)
        
        # Ordenar pasos por tipo y numero para ejecucion secuencial
        # NO ordenar, mantener orden de aparicion en archivo
        
        if test['name']:
            return test
        return None

class TestExecutor:
    """Ejecuta pruebas"""
    
    def __init__(self, db: DatabaseConnection, api_base_url: str):
        self.db = db
        self.api_base_url = api_base_url
        self.results = {}  # Almacena resultados de cada paso
    
    def interpolate_variables(self, text: str) -> str:
        """Reemplaza variables {step_name["field"]} con valores reales"""
        # Patron para {curl_1["id"]}, {psql_1["total"]}, etc.
        pattern = r'\{([a-z]+_\d+)\["([^"]+)"\]\}'
        
        def replace_var(match):
            step_name = match.group(1)
            field_name = match.group(2)
            
            if step_name in self.results and self.results[step_name]:
                value = self.results[step_name].get(field_name)
                if value is not None:
                    return str(value)
            # Si no se encuentra, retornar el placeholder original
            return match.group(0)
        
        result = re.sub(pattern, replace_var, text)
        return result
    
    def execute_tsql(self, step: Dict[str, Any]) -> Tuple[bool, Optional[Dict[str, Any]]]:
        """Ejecuta paso de SQL Server"""
        query = self.interpolate_variables(step['content'])
        
        print(f"\n  {Colors.CYAN}[{step['name'].upper()}]{Colors.END} {query[:80]}...")
        
        result = self.db.execute_sqlserver(query)
        
        if result is not None:
            print(f"  {Colors.GREEN}Resultado:{Colors.END} {json.dumps(result, indent=2, default=str)}")
            return True, result
        else:
            print(f"  {Colors.RED}[ERROR] Fallo la ejecucion{Colors.END}")
            return False, None
    
    def execute_psql(self, step: Dict[str, Any]) -> Tuple[bool, Optional[Dict[str, Any]]]:
        """Ejecuta paso de PostgreSQL"""
        query = self.interpolate_variables(step['content'])
        
        print(f"\n  {Colors.CYAN}[{step['name'].upper()}]{Colors.END} {query[:80]}...")
        
        result = self.db.execute_postgresql(query)
        
        if result is not None:
            print(f"  {Colors.GREEN}Resultado:{Colors.END} {json.dumps(result, indent=2, default=str)}")
            return True, result
        else:
            print(f"  {Colors.RED}[ERROR] Fallo la ejecucion{Colors.END}")
            return False, None
    
    def execute_curl(self, step: Dict[str, Any]) -> Tuple[bool, Optional[Dict[str, Any]]]:
        """Ejecuta paso de API call"""
        curl_command = self.interpolate_variables(step['content'])
        
        # Parsear comando curl
        method = 'GET'
        url = ''
        headers = {}
        data = None
        
        # Extraer metodo
        method_match = re.search(r"-X\s+'([A-Z]+)'", curl_command)
        if method_match:
            method = method_match.group(1)
        
        # Extraer URL
        url_match = re.search(r"'(https?://[^']+)'", curl_command)
        if url_match:
            url = url_match.group(1)
        
        # Extraer headers
        header_matches = re.findall(r"-H\s+'([^:]+):\s*([^']+)'", curl_command)
        for header_name, header_value in header_matches:
            headers[header_name] = header_value
        
        # Extraer data
        data_match = re.search(r"-d\s+'([^']+)'", curl_command)
        if data_match:
            data = data_match.group(1)
        
        print(f"\n  {Colors.CYAN}[{step['name'].upper()}]{Colors.END} {method} {url}")
        
        try:
            if method == 'GET':
                response = requests.get(url, headers=headers)
            elif method == 'POST':
                response = requests.post(url, headers=headers, data=data)
            elif method == 'PUT':
                response = requests.put(url, headers=headers, data=data)
            elif method == 'DELETE':
                response = requests.delete(url, headers=headers)
            else:
                print(f"  {Colors.RED}[ERROR] Metodo HTTP no soportado: {method}{Colors.END}")
                return False, None
            
            print(f"  {Colors.GREEN}Status:{Colors.END} {response.status_code}")
            
            # Intentar parsear JSON
            result = {'status_code': response.status_code}
            try:
                json_response = response.json()
                result.update(json_response)
                print(f"  {Colors.GREEN}Response:{Colors.END} {json.dumps(json_response, indent=2, default=str)[:200]}...")
            except:
                result['text'] = response.text
                if response.text:
                    print(f"  {Colors.GREEN}Response:{Colors.END} {response.text[:200]}...")
            
            return True, result
            
        except Exception as e:
            print(f"  {Colors.RED}[ERROR] {e}{Colors.END}")
            return False, None
    
    def execute_test(self, test: Dict[str, Any]) -> bool:
        """Ejecuta un test completo"""
        self.results = {}  # Limpiar resultados anteriores
        
        # Ejecutar pasos en orden
        for step in test['steps']:
            step_name = step['name']
            
            if step['type'] == 'tsql':
                success, result = self.execute_tsql(step)
            elif step['type'] == 'psql':
                success, result = self.execute_psql(step)
            elif step['type'] == 'curl':
                success, result = self.execute_curl(step)
            else:
                print(f"  {Colors.RED}[ERROR] Tipo de paso desconocido: {step['type']}{Colors.END}")
                return False
            
            if not success:
                print(f"\n  {Colors.RED}[FAIL] Test fallido en paso {step_name}{Colors.END}")
                return False
            
            # Guardar resultado para interpolacion
            self.results[step_name] = result
        
        # Ejecutar asercion
        if test['assert']:
            return self.execute_assert(test['assert'])
        
        return True
    
    def execute_assert(self, assert_expr: str) -> bool:
        """Ejecuta asercion"""
        print(f"\n  {Colors.CYAN}[ASSERT]{Colors.END} {assert_expr}")
        
        try:
            # Crear contexto con todos los resultados
            context = {}
            for step_name, result in self.results.items():
                context[step_name] = result
            
            # Evaluar expresion
            result = eval(assert_expr, {"__builtins__": {}}, context)
            
            print(f"  {Colors.GREEN}Resultado:{Colors.END} {result}")
            
            if result:
                print(f"\n  {Colors.GREEN}{Colors.BOLD}[PASS] Test exitoso{Colors.END}")
                return True
            else:
                print(f"\n  {Colors.RED}{Colors.BOLD}[FAIL] Asercion fallo{Colors.END}")
                return False
                
        except Exception as e:
            print(f"  {Colors.RED}[ERROR] Error en asercion: {e}{Colors.END}")
            print(f"\n  {Colors.RED}{Colors.BOLD}[FAIL] Test fallido{Colors.END}")
            return False

def main():
    parser = argparse.ArgumentParser(description='CRM API Test Runner v2.0')
    parser.add_argument('testfile', help='Archivo de definicion de pruebas')
    parser.add_argument('--config', default='config.json', help='Archivo de configuracion JSON')
    parser.add_argument('--verbose', '-v', action='store_true', help='Modo verbose')
    
    args = parser.parse_args()
    
    # Cargar configuracion
    try:
        with open(args.config, 'r') as f:
            config = json.load(f)
    except FileNotFoundError:
        print_color(f"[ERROR] Archivo de configuracion no encontrado: {args.config}", Colors.RED)
        sys.exit(1)
    
    # Parsear archivo de pruebas
    print_color(f"[INFO] Parseando archivo: {args.testfile}", Colors.CYAN)
    tests = TestParser.parse_file(args.testfile)
    print_color(f"[INFO] {len(tests)} pruebas encontradas", Colors.CYAN)
    
    # Conectar a bases de datos
    db = DatabaseConnection(config)
    
    pg_connected = db.connect_postgresql()
    mssql_connected = db.connect_sqlserver()
    
    if not pg_connected and not mssql_connected:
        print_color("[ERROR] No se pudo conectar a ninguna base de datos", Colors.RED)
        sys.exit(1)
    
    # Ejecutar pruebas
    api_base_url = config.get('api_base_url', 'http://localhost:5000/api')
    executor = TestExecutor(db, api_base_url)
    
    passed = 0
    failed = 0
    
    for i, test in enumerate(tests, 1):
        print(f"\n{Colors.BOLD}[{i}/{len(tests)}] {test['name']}{Colors.END}")
        if test['description']:
            print(f"{Colors.CYAN}{test['description']}{Colors.END}")
        
        if executor.execute_test(test):
            passed += 1
        else:
            failed += 1
    
    # Reporte final
    print_section("REPORTE FINAL")
    
    print(f"Total de pruebas:     {len(tests)}")
    print(f"Pruebas exitosas:     {Colors.GREEN}{passed}{Colors.END}")
    print(f"Pruebas fallidas:     {Colors.RED}{failed}{Colors.END}")
    
    success_rate = (passed / len(tests) * 100) if len(tests) > 0 else 0
    print(f"Tasa de exito:        {success_rate:.1f}%")
    
    print_section("")
    
    # Cerrar conexiones
    db.close()
    
    # Exit code
    sys.exit(0 if failed == 0 else 1)

if __name__ == '__main__':
    main()

