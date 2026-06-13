# Historias de usuario - Envíos Rápidos GT

## 1. Registro de cliente

**Como** encargado de oficina, **quiero** registrar clientes con sus datos principales, **para** usarlos como remitentes o destinatarios en los envíos.

**Criterios de aceptación:**

- Dado un nombre, teléfono y dirección válidos, cuando se registre el cliente, entonces el sistema debe guardarlo.
- Si el cliente tiene NIT, entonces el sistema debe almacenarlo para evaluar descuento.
- El sistema debe permitir consultar la lista de clientes registrados.

## 2. Consulta de clientes

**Como** encargado de oficina, **quiero** ver todos los clientes registrados, **para** seleccionar rápidamente remitentes y destinatarios.

**Criterios de aceptación:**

- Cuando se consulte `/api/clientes`, entonces el sistema debe devolver la lista de clientes.
- La respuesta debe incluir ID, nombre, teléfono, dirección y NIT.

## 3. Registro de envío

**Como** operador de logística, **quiero** registrar un envío con remitente, destinatario, peso y direcciones, **para** iniciar el proceso de entrega.

**Criterios de aceptación:**

- Dado un remitente y destinatario existentes, cuando se registre el envío, entonces el sistema debe crear el envío.
- El sistema debe calcular automáticamente tarifa base, descuento y total.
- El sistema debe generar automáticamente el código de rastreo.
- El estado inicial debe ser `Registrado`.

## 4. Cálculo automático de tarifa

**Como** operador de logística, **quiero** que la tarifa se calcule según el peso, **para** evitar errores manuales en el cobro.

**Criterios de aceptación:**

- Si el peso es menor o igual a 1 kg, la tarifa debe ser Q25.00.
- Si el peso está entre 1.01 y 5 kg, la tarifa debe ser Q45.00.
- Si el peso está entre 5.01 y 10 kg, la tarifa debe ser Q75.00.
- Si el peso es mayor a 10 kg, la tarifa debe ser Q100.00.

## 5. Descuento por NIT válido

**Como** cliente, **quiero** recibir 5% de descuento si mi NIT es válido, **para** pagar menos en el servicio.

**Criterios de aceptación:**

- Si el remitente o destinatario tiene NIT válido, entonces se aplica 5% de descuento.
- Si ninguno tiene NIT válido, entonces no se aplica descuento.
- El sistema debe guardar tarifa base, descuento y tarifa final.

## 6. Rastreo de paquete

**Como** cliente, **quiero** buscar mi paquete por código de rastreo, **para** conocer su estado actual.

**Criterios de aceptación:**

- Dado un código con formato `ENV-YYYYMMDD-XXXX`, cuando el cliente consulte el rastreo, entonces el sistema debe mostrar el envío.
- La consulta debe mostrar estado actual, ubicación, historial y datos generales.
- Si el código no existe, debe devolver error 404.

## 7. Actualización de estado

**Como** encargado de logística, **quiero** actualizar el estado de un envío, **para** reflejar el avance real del paquete.

**Criterios de aceptación:**

- El sistema solo debe permitir avanzar en el orden correcto.
- No debe permitir saltar de `Registrado` a `Entregado`.
- Cada cambio debe guardar estado, ubicación, fecha automática y notas.

## 8. Registro de intento fallido

**Como** mensajero, **quiero** registrar un intento fallido de entrega, **para** dejar evidencia cuando no se logra entregar el paquete.

**Criterios de aceptación:**

- Solo se pueden registrar intentos fallidos si el envío está en `EnReparto`.
- El sistema debe aumentar el contador de intentos fallidos.
- Al tercer intento fallido, el estado debe cambiar automáticamente a `EnDevolucion`.
- No deben permitirse más de 3 intentos fallidos.

## 9. Consulta de historial

**Como** supervisor, **quiero** consultar el historial de un envío, **para** auditar cada cambio realizado.

**Criterios de aceptación:**

- La consulta `/api/envios/{id}/historial` debe devolver todos los registros del envío.
- Cada registro debe incluir estado, ubicación, fecha/hora y notas.
- El historial debe ordenarse desde el evento más reciente.

## 10. Reporte de eficiencia

**Como** gerente de operaciones, **quiero** generar un reporte de eficiencia, **para** medir entregas, devoluciones e intentos fallidos.

**Criterios de aceptación:**

- El reporte debe mostrar total de envíos.
- Debe mostrar cuántos fueron entregados, devueltos o siguen en proceso.
- Debe calcular porcentaje de eficiencia de entrega.
- Debe mostrar total de intentos fallidos y montos facturados.
