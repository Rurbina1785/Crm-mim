#!/usr/bin/env python3
"""
CRM API Test Runner
===================

Sistema de pruebas cross-platform para APIs con soporte para:
- Precondiciones SQL
- Llamadas HTTP (curl o requests)
- Postcondiciones SQL
- Aserciones personalizadas
- Reportes detallados en HTML y consola

Uso:
    python test_runner.py tests.txt
    python test_runner.py tests.txt --config config.json
    python test_runner.py tests.txt --verbose
"""

import sys
import re
import json
import subprocess
import psycopg2
import requests
from datetime import datetime
from typing import Dict, List, Any, Optional, Tuple
from dataclasses import dataclass, field
from pathlib import Path


@dataclass
class TestConfig:
    """Configuracion de conexion a base de datos y API"""
    db_host: str = "localhost"
    db_port: int = 5432
    db_name: str = "crmdb"
    db_user: str = "crmuser"
    db_password: str = "crm123456"
    api_base_url: str = "http://localhost:5000/api"
    
    @classmethod
    def from_file(cls, config_file: str) -> 'TestConfig':
        """Carga configuracion desde archivo JSON"""
        with open(config_file, 'r', encoding='utf-8') as f:
            data = json.load(f)
        return cls(**data)


@dataclass
class TestCase:
    """Representa un caso de prueba individual"""
    name: str
    description: str = ""
    pre_sql: Optional[str] = None
    curl: Optional[str] = None
    post_sql: Optional[str] = None
    assertion: Optional[str] = None
    http_method: str = "GET"
    url: str = ""
    headers: Dict[str, str] = field(default_factory=dict)
    body: Optional[str] = None
    
    # Resultados de ejecucion
    pre_sql_result: Optional[List[Dict]] = None
    http_response: Optional[requests.Response] = None
    post_sql_result: Optional[List[Dict]] = None
    assertion_result: bool = False
    assertion_message: str = ""
    error: Optional[str] = None
    passed: bool = False


class TestParser:
    """Parser para archivos de definicion de pruebas"""
    
    def __init__(self):
        self.tests: List[TestCase] = []
    
    def parse_file(self, filepath: str) -> List[TestCase]:
        """
        Parse un archivo de pruebas y retorna lista de TestCase
        
        Formato esperado:
        ### Nombre del test
        Descripcion opcional
        # Comentarios
        
        pre_sql:
          SELECT ...
        
        curl:
          curl -X 'GET' 'http://...' -H 'header: value'
        
        post_sql:
          SELECT ...
        
        assert:
          expresion_python
        
        ---
        """
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Dividir por separador ---
        test_blocks = content.split('---')
        
        for block in test_blocks:
            block = block.strip()
            if not block:
                continue
            
            test = self._parse_test_block(block)
            if test:
                self.tests.append(test)
        
        return self.tests
    
    def _parse_test_block(self, block: str) -> Optional[TestCase]:
        """Parse un bloque de prueba individual"""
        lines = block.split('\n')
        
        # Extraer nombre (primera linea con ###)
        name = ""
        description_lines = []
        section = None
        section_content = []
        
        test = TestCase(name="")
        
        for line in lines:
            line_stripped = line.strip()
            
            # Nombre del test (### ...)
            if line_stripped.startswith('###'):
                test.name = line_stripped[3:].strip()
                continue
            
            # Comentarios (ignorar)
            if line_stripped.startswith('#'):
                description_lines.append(line_stripped[1:].strip())
                continue
            
            # Detectar seccion
            if line_stripped.endswith(':') and not line.startswith(' '):
                # Guardar seccion anterior
                if section and section_content:
                    self._set_section_content(test, section, '\n'.join(section_content))
                
                section = line_stripped[:-1].strip()
                section_content = []
                continue
            
            # Contenido de seccion
            if section:
                section_content.append(line)
        
        # Guardar ultima seccion
        if section and section_content:
            self._set_section_content(test, section, '\n'.join(section_content))
        
        # Descripcion
        test.description = ' '.join(description_lines)
        
        # Parse curl para extraer metodo, URL, headers
        if test.curl:
            self._parse_curl(test)
        
        return test if test.name else None
    
    def _set_section_content(self, test: TestCase, section: str, content: str):
        """Asigna contenido a la seccion correspondiente del test"""
        content = content.strip()
        
        if section == 'pre_sql':
            test.pre_sql = content
        elif section == 'curl':
            test.curl = content
        elif section == 'post_sql':
            test.post_sql = content
        elif section == 'assert' or section == 'assertion':
            test.assertion = content
    
    def _parse_curl(self, test: TestCase):
        """Extrae metodo, URL, headers y body de un comando curl"""
        curl_cmd = test.curl
        
        # Extraer metodo HTTP
        method_match = re.search(r"-X\s+'([A-Z]+)'", curl_cmd)
        if method_match:
            test.http_method = method_match.group(1)
        
        # Extraer URL
        url_match = re.search(r"'(https?://[^']+)'", curl_cmd)
        if url_match:
            test.url = url_match.group(1)
        
        # Extraer headers
        header_matches = re.findall(r"-H\s+'([^:]+):\s*([^']+)'", curl_cmd)
        for header_name, header_value in header_matches:
            test.headers[header_name.strip()] = header_value.strip()
        
        # Extraer body (si existe)
        body_match = re.search(r"-d\s+'([^']+)'", curl_cmd)
        if body_match:
            test.body = body_match.group(1)


class TestExecutor:
    """Ejecutor de pruebas"""
    
    def __init__(self, config: TestConfig, verbose: bool = False):
        self.config = config
        self.verbose = verbose
        self.db_conn = None
    
    def connect_db(self):
        """Conecta a la base de datos PostgreSQL"""
        try:
            self.db_conn = psycopg2.connect(
                host=self.config.db_host,
                port=self.config.db_port,
                database=self.config.db_name,
                user=self.config.db_user,
                password=self.config.db_password
            )
            if self.verbose:
                print(f"[OK] Conectado a PostgreSQL: {self.config.db_name}")
        except Exception as e:
            print(f"[ERROR] No se pudo conectar a PostgreSQL: {e}")
            raise
    
    def disconnect_db(self):
        """Cierra conexion a base de datos"""
        if self.db_conn:
            self.db_conn.close()
            if self.verbose:
                print("[OK] Desconectado de PostgreSQL")
    
    def execute_sql(self, sql: str) -> List[Dict[str, Any]]:
        """
        Ejecuta query SQL y retorna resultados como lista de diccionarios
        """
        cursor = self.db_conn.cursor()
        try:
            cursor.execute(sql)
            
            # Si es SELECT, retornar resultados
            if sql.strip().upper().startswith('SELECT'):
                columns = [desc[0] for desc in cursor.description]
                results = []
                for row in cursor.fetchall():
                    results.append(dict(zip(columns, row)))
                return results
            else:
                # Para INSERT, UPDATE, DELETE
                self.db_conn.commit()
                return [{"affected_rows": cursor.rowcount}]
        except Exception as e:
            self.db_conn.rollback()
            raise Exception(f"Error SQL: {e}")
        finally:
            cursor.close()
    
    def execute_http(self, test: TestCase) -> requests.Response:
        """Ejecuta llamada HTTP usando requests"""
        url = test.url
        method = test.http_method.upper()
        headers = test.headers
        
        # Si la URL es relativa, agregar base URL
        if not url.startswith('http'):
            url = f"{self.config.api_base_url}{url}"
        
        try:
            if method == 'GET':
                response = requests.get(url, headers=headers)
            elif method == 'POST':
                response = requests.post(url, headers=headers, data=test.body)
            elif method == 'PUT':
                response = requests.put(url, headers=headers, data=test.body)
            elif method == 'DELETE':
                response = requests.delete(url, headers=headers)
            else:
                raise ValueError(f"Metodo HTTP no soportado: {method}")
            
            return response
        except Exception as e:
            raise Exception(f"Error HTTP: {e}")
    
    def evaluate_assertion(self, test: TestCase) -> Tuple[bool, str]:
        """
        Evalua la asercion del test
        
        Variables disponibles:
        - pre_sql: resultado de pre_sql (lista de dicts)
        - post_sql: resultado de post_sql (lista de dicts)
        - response: objeto Response de requests
        - status_code: codigo HTTP
        - json: response.json() si es JSON
        """
        if not test.assertion:
            return True, "Sin asercion definida"
        
        # Preparar contexto para eval
        context = {
            'pre_sql': test.pre_sql_result or [],
            'post_sql': test.post_sql_result or [],
            'response': test.http_response,
            'status_code': test.http_response.status_code if test.http_response else None,
            'len': len,
            'str': str,
            'int': int,
            'float': float,
        }
        
        # Agregar json si la respuesta es JSON
        if test.http_response:
            try:
                context['json'] = test.http_response.json()
            except:
                context['json'] = None
        
        try:
            # Evaluar asercion
            result = eval(test.assertion, {"__builtins__": {}}, context)
            
            if result:
                return True, "Asercion pasada"
            else:
                return False, f"Asercion fallida: {test.assertion}"
        except Exception as e:
            return False, f"Error evaluando asercion: {e}"
    
    def run_test(self, test: TestCase) -> TestCase:
        """Ejecuta un test completo"""
        try:
            # 1. Ejecutar pre_sql
            if test.pre_sql:
                if self.verbose:
                    print(f"  [SQL PRE] {test.pre_sql[:50]}...")
                test.pre_sql_result = self.execute_sql(test.pre_sql)
            
            # 2. Ejecutar HTTP
            if test.curl or test.url:
                if self.verbose:
                    print(f"  [HTTP] {test.http_method} {test.url}")
                test.http_response = self.execute_http(test)
            
            # 3. Ejecutar post_sql
            if test.post_sql:
                if self.verbose:
                    print(f"  [SQL POST] {test.post_sql[:50]}...")
                test.post_sql_result = self.execute_sql(test.post_sql)
            
            # 4. Evaluar asercion
            if test.assertion:
                test.assertion_result, test.assertion_message = self.evaluate_assertion(test)
                test.passed = test.assertion_result
            else:
                # Sin asercion, verificar solo que HTTP fue exitoso
                if test.http_response:
                    test.passed = test.http_response.status_code < 400
                    test.assertion_message = f"HTTP {test.http_response.status_code}"
                else:
                    test.passed = True
                    test.assertion_message = "Sin validacion HTTP"
        
        except Exception as e:
            test.error = str(e)
            test.passed = False
            test.assertion_message = f"Error: {e}"
        
        return test
    
    def run_tests(self, tests: List[TestCase]) -> List[TestCase]:
        """Ejecuta todos los tests"""
        self.connect_db()
        
        results = []
        for i, test in enumerate(tests, 1):
            print(f"\n[{i}/{len(tests)}] {test.name}")
            result = self.run_test(test)
            results.append(result)
            
            # Mostrar resultado
            if result.passed:
                print(f"  [PASS] {result.assertion_message}")
            else:
                print(f"  [FAIL] {result.assertion_message}")
        
        self.disconnect_db()
        return results


class ReportGenerator:
    """Generador de reportes"""
    
    @staticmethod
    def generate_console_report(tests: List[TestCase]):
        """Genera reporte en consola"""
        print("\n" + "="*70)
        print("REPORTE FINAL")
        print("="*70)
        
        total = len(tests)
        passed = sum(1 for t in tests if t.passed)
        failed = total - passed
        success_rate = (passed / total * 100) if total > 0 else 0
        
        print(f"\nTotal de pruebas:     {total}")
        print(f"Pruebas exitosas:     {passed}")
        print(f"Pruebas fallidas:     {failed}")
        print(f"Tasa de exito:        {success_rate:.1f}%")
        
        if failed > 0:
            print("\nPruebas fallidas:")
            for test in tests:
                if not test.passed:
                    print(f"  - {test.name}: {test.assertion_message}")
        
        print("\n" + "="*70 + "\n")
    
    @staticmethod
    def generate_html_report(tests: List[TestCase], output_file: str = "test_report.html"):
        """Genera reporte HTML"""
        total = len(tests)
        passed = sum(1 for t in tests if t.passed)
        failed = total - passed
        success_rate = (passed / total * 100) if total > 0 else 0
        
        html = f"""<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Reporte de Pruebas API CRM</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; background: #f5f5f5; }}
        .container {{ max-width: 1200px; margin: 0 auto; background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
        h1 {{ color: #333; border-bottom: 3px solid #007bff; padding-bottom: 10px; }}
        .summary {{ display: grid; grid-template-columns: repeat(4, 1fr); gap: 15px; margin: 20px 0; }}
        .stat {{ padding: 15px; border-radius: 6px; text-align: center; }}
        .stat.total {{ background: #e3f2fd; }}
        .stat.passed {{ background: #c8e6c9; }}
        .stat.failed {{ background: #ffcdd2; }}
        .stat.rate {{ background: #fff9c4; }}
        .stat-value {{ font-size: 32px; font-weight: bold; }}
        .stat-label {{ font-size: 14px; color: #666; margin-top: 5px; }}
        .test {{ border: 1px solid #ddd; margin: 15px 0; border-radius: 6px; overflow: hidden; }}
        .test-header {{ padding: 15px; background: #f8f9fa; cursor: pointer; display: flex; justify-content: space-between; align-items: center; }}
        .test-header:hover {{ background: #e9ecef; }}
        .test-header.passed {{ border-left: 4px solid #4caf50; }}
        .test-header.failed {{ border-left: 4px solid #f44336; }}
        .test-body {{ padding: 15px; display: none; background: #fafafa; }}
        .test-body.show {{ display: block; }}
        .badge {{ padding: 4px 12px; border-radius: 12px; font-size: 12px; font-weight: bold; }}
        .badge.pass {{ background: #4caf50; color: white; }}
        .badge.fail {{ background: #f44336; color: white; }}
        .section {{ margin: 15px 0; }}
        .section-title {{ font-weight: bold; color: #555; margin-bottom: 8px; }}
        pre {{ background: #f5f5f5; padding: 10px; border-radius: 4px; overflow-x: auto; }}
        .json {{ background: #263238; color: #aed581; padding: 10px; border-radius: 4px; overflow-x: auto; }}
    </style>
    <script>
        function toggleTest(id) {{
            const body = document.getElementById('test-body-' + id);
            body.classList.toggle('show');
        }}
    </script>
</head>
<body>
    <div class="container">
        <h1>Reporte de Pruebas API CRM</h1>
        <p>Generado: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}</p>
        
        <div class="summary">
            <div class="stat total">
                <div class="stat-value">{total}</div>
                <div class="stat-label">Total de Pruebas</div>
            </div>
            <div class="stat passed">
                <div class="stat-value">{passed}</div>
                <div class="stat-label">Exitosas</div>
            </div>
            <div class="stat failed">
                <div class="stat-value">{failed}</div>
                <div class="stat-label">Fallidas</div>
            </div>
            <div class="stat rate">
                <div class="stat-value">{success_rate:.1f}%</div>
                <div class="stat-label">Tasa de Exito</div>
            </div>
        </div>
        
        <h2>Detalle de Pruebas</h2>
"""
        
        for i, test in enumerate(tests):
            status_class = "passed" if test.passed else "failed"
            badge_class = "pass" if test.passed else "fail"
            badge_text = "PASS" if test.passed else "FAIL"
            
            html += f"""
        <div class="test">
            <div class="test-header {status_class}" onclick="toggleTest({i})">
                <div>
                    <strong>{test.name}</strong>
                    <div style="font-size: 12px; color: #666; margin-top: 4px;">{test.description}</div>
                </div>
                <span class="badge {badge_class}">{badge_text}</span>
            </div>
            <div id="test-body-{i}" class="test-body">
"""
            
            if test.pre_sql:
                html += f"""
                <div class="section">
                    <div class="section-title">Precondicion SQL:</div>
                    <pre>{test.pre_sql}</pre>
                    <div class="section-title">Resultado:</div>
                    <pre>{json.dumps(test.pre_sql_result, indent=2, default=str)}</pre>
                </div>
"""
            
            if test.http_response:
                html += f"""
                <div class="section">
                    <div class="section-title">HTTP Request:</div>
                    <pre>{test.http_method} {test.url}</pre>
                    <div class="section-title">HTTP Response:</div>
                    <pre>Status: {test.http_response.status_code}</pre>
                    <div class="json">{test.http_response.text[:500]}</div>
                </div>
"""
            
            if test.post_sql:
                html += f"""
                <div class="section">
                    <div class="section-title">Postcondicion SQL:</div>
                    <pre>{test.post_sql}</pre>
                    <div class="section-title">Resultado:</div>
                    <pre>{json.dumps(test.post_sql_result, indent=2, default=str)}</pre>
                </div>
"""
            
            if test.assertion:
                html += f"""
                <div class="section">
                    <div class="section-title">Asercion:</div>
                    <pre>{test.assertion}</pre>
                    <div class="section-title">Resultado:</div>
                    <pre>{test.assertion_message}</pre>
                </div>
"""
            
            html += """
            </div>
        </div>
"""
        
        html += """
    </div>
</body>
</html>
"""
        
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write(html)
        
        print(f"\n[OK] Reporte HTML generado: {output_file}")


def main():
    """Funcion principal"""
    import argparse
    
    parser = argparse.ArgumentParser(description='CRM API Test Runner')
    parser.add_argument('testfile', help='Archivo de definicion de pruebas')
    parser.add_argument('--config', help='Archivo de configuracion JSON', default=None)
    parser.add_argument('--verbose', '-v', action='store_true', help='Modo verbose')
    parser.add_argument('--output', '-o', help='Archivo de salida HTML', default='test_report.html')
    
    args = parser.parse_args()
    
    # Cargar configuracion
    if args.config:
        config = TestConfig.from_file(args.config)
    else:
        config = TestConfig()
    
    # Parse tests
    print(f"[INFO] Parseando archivo: {args.testfile}")
    parser_obj = TestParser()
    tests = parser_obj.parse_file(args.testfile)
    print(f"[INFO] {len(tests)} pruebas encontradas")
    
    # Ejecutar tests
    executor = TestExecutor(config, verbose=args.verbose)
    results = executor.run_tests(tests)
    
    # Generar reportes
    ReportGenerator.generate_console_report(results)
    ReportGenerator.generate_html_report(results, args.output)


if __name__ == '__main__':
    main()

