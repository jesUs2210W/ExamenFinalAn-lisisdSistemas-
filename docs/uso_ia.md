# Informe de utilización de IA

## Herramienta utilizada

Se utilizó una herramienta de inteligencia artificial para apoyar la planificación, estructura y revisión del prototipo de API REST solicitado en la evaluación.

## Prompts enviados

1. Crear la base de un proyecto ASP.NET Core Web API en C# para una empresa de envíos llamada Envíos Rápidos GT.
2. Diseñar modelos para Cliente, Envio e HistorialEstado usando SQLite.
3. Crear endpoints para registrar clientes, envíos, rastreo, actualización de estado, intentos fallidos, historial y reportes.
4. Implementar reglas de negocio para tarifas por peso, descuento por NIT válido, flujo de estados y máximo de 3 intentos fallidos.
5. Crear pruebas unitarias usando xUnit.
6. Redactar historias de usuario en formato: Como, quiero, para.
7. Elaborar un README con instrucciones para ejecutar el proyecto en Visual Studio Code.
8. Crear una guía básica para despliegue en Render.

## Correcciones realizadas

- Se ajustó el flujo de estados para que solo avance en la dirección permitida.
- Se agregó registro automático en el historial cada vez que cambia el estado.
- Se agregó ubicación obligatoria en cada actualización de estado.
- Se agregó validación para no permitir más de 3 intentos fallidos.
- Se configuró SQLite para simplificar la ejecución local.
- Se agregaron pruebas unitarias para las reglas principales del sistema.
- Se organizó el proyecto en carpetas separadas para modelos, controladores, DTOs, servicios y datos.

## Reflexión personal

El uso de IA sirvió como apoyo para ordenar mejor las ideas del problema y convertir los requisitos en una solución funcional. La herramienta ayudó a proponer una estructura inicial del proyecto, pero fue necesario revisar las reglas del enunciado para adaptar el código a lo solicitado por la evaluación. También permitió identificar partes importantes que no debían faltar, como el historial de estados, la ubicación de cada cambio y el reporte de eficiencia.

La IA no reemplaza el análisis del estudiante, porque las decisiones principales deben salir de la comprensión del problema. En este caso, se usó como una guía para acelerar el desarrollo, corregir errores y documentar mejor el proyecto. El resultado final debe ser revisado, probado y explicado por el estudiante para demostrar que entiende cómo funciona la API.

## Aprendizaje obtenido

Con este proyecto se reforzó el uso de ASP.NET Core Web API, SQLite, controladores, servicios, DTOs, pruebas unitarias y documentación técnica. También se practicó la forma de transformar reglas de negocio reales en código funcional.
