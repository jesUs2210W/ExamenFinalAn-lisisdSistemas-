# EnviosRapidosGT.Api

API REST en C# con SQLite para el caso de **Envíos Rápidos GT**.

## Lo que incluye

- Proyecto ASP.NET Core Web API en C#.
- Base de datos SQLite automática.
- Modelos principales: `Cliente`, `Envio`, `HistorialEstado`.
- Endpoints solicitados en el examen.
- Cálculo automático de tarifa.
- Código de rastreo automático con formato `ENV-YYYYMMDD-XXXX`.
- Control de flujo de estados.
- Registro de historial por ubicación/oficina.
- Máximo de 3 intentos fallidos; al tercero pasa a `EnDevolucion`.
- Descuento del 5% si remitente o destinatario tiene NIT válido.
- Pruebas unitarias con xUnit.
- Archivos de documentación: historias de usuario, uso de IA y diagramas.
- Dockerfile y `render.yaml` para despliegue en Render.

## Requisitos

Instalar:

1. Visual Studio Code.
2. .NET 8 SDK.
3. Extensión de C# para VS Code.
4. Extensión REST Client para usar el archivo `requests.http`.

## Cómo correrlo en Visual Studio Code

Abra una terminal en la carpeta raíz del proyecto y ejecute:

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/EnviosRapidosGT.Api/EnviosRapidosGT.Api.csproj
```

Luego abra:

```text
http://localhost:5000/swagger
```

La base de datos SQLite se crea automáticamente como:

```text
src/EnviosRapidosGT.Api/enviosrapidosgt.db
```

No necesita instalar SQL Server ni configurar un servidor externo.

## Endpoints solicitados

| Método | Ruta | Función |
|---|---|---|
| POST | `/api/clientes` | Crear cliente |
| GET | `/api/clientes` | Listar clientes |
| POST | `/api/envios` | Crear envío |
| GET | `/api/envios` | Listar envíos |
| GET | `/api/envios/{id}` | Obtener envío por ID |
| GET | `/api/envios/rastreo/{codigo}` | Rastrear envío |
| PUT | `/api/envios/{id}/estado` | Actualizar estado |
| POST | `/api/envios/{id}/intento-fallido` | Registrar intento fallido |
| GET | `/api/envios/{id}/historial` | Ver historial |
| GET | `/api/reportes/eficiencia` | Reporte de eficiencia |

## Tarifas aplicadas

| Peso | Tarifa |
|---|---:|
| `<= 1 kg` | Q25.00 |
| `1.01 - 5 kg` | Q45.00 |
| `5.01 - 10 kg` | Q75.00 |
| `> 10 kg` | Q100.00 |

Si el remitente o destinatario tiene NIT válido, se aplica 5% de descuento.

## Flujo de estados permitido

```text
Registrado -> EnTransito -> EnReparto -> Entregado
Registrado -> EnTransito -> EnReparto -> EnDevolucion -> Devuelto
```

El sistema no permite saltarse estados ni regresar a estados anteriores.

## Prueba rápida desde `requests.http`

1. Ejecute la API.
2. Abra el archivo `requests.http`.
3. Envíe las peticiones en orden:
   - Crear remitente.
   - Crear destinatario.
   - Crear envío.
   - Pasar a `EnTransito`.
   - Pasar a `EnReparto`.
   - Probar intento fallido o entrega.
   - Consultar historial y reporte.

## Pruebas unitarias

Ejecute:

```bash
dotnet test
```

Las pruebas validan:

- Cálculo de tarifas por rangos de peso.
- Aplicación de descuento del 5%.
- Validación básica de NIT.
- Flujo correcto de estados.
- Formato del código de rastreo.

## Despliegue en Render

Este proyecto incluye `Dockerfile` y `render.yaml`.

Pasos generales:

1. Suba el proyecto a GitHub con el nombre solicitado por el examen, por ejemplo:

```text
CARNET_ANALISISA2026FINAL
```

2. Entre a Render.
3. Cree un nuevo Web Service desde GitHub.
4. Seleccione el repositorio.
5. Use despliegue con Docker.
6. Render detectará el `Dockerfile`.
7. Al finalizar, pruebe la URL pública agregando `/swagger`.

Ejemplo:

```text
https://su-api-en-render.onrender.com/swagger
```

Nota: SQLite funciona para prototipo académico. En producción real convendría usar PostgreSQL o SQL Server, porque el disco de servicios gratuitos puede ser temporal.

## Documentos incluidos

- `docs/historias_usuario.md`
- `docs/uso_ia.md`
- `docs/diagrama_secuencia.md`
- `docs/diagrama_flujo.md`
- `requests.http`

## Recomendación para la entrega

Antes de subirlo, edite el nombre del repositorio con su carné real:

```text
CARNET_ANALISISA2026FINAL
```

También puede editar el archivo `docs/uso_ia.md` para colocar sus propias palabras en la reflexión.
