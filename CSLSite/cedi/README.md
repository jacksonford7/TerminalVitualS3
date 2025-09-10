# Despacho CEDI

Este módulo replica el flujo de **Despacho de Vehículos** pero trabajando de forma aislada para el contexto CEDI.

## Flujo funcional
1. Entrada de filtros MRN/MSN/HSN.
2. Consulta al servicio `ICediDespachoService`.
3. Enlazado de resultados en la grilla.
4. Acciones por fila: asignar, despachar y exportar.
5. Mensajes de retroalimentación al usuario.

## Dependencias y consultas
- Servicio: `Core/Cedi/CediDespachoService`.
- Repositorio: `Infra/Cedi/CediDespachoRepository`.
- Procedimientos almacenados propuestos:
  - `cedi.buscar_despacho`
  - `cedi.obtener_despacho`
  - `cedi.asignar_despacho`
  - `cedi.despachar`
  - `cedi.exportar_despacho`

## Permisos y menú
- Roles sugeridos: `CEDI_Despacho_View` y `CEDI_Despacho_Edit`.
- Agregar la opción en `Web.sitemap` bajo el menú **CEDI**.

## Configuración
- Utiliza la cadena de conexión `midle`.
- Constante de contexto `ContextoModulo = "CEDI"` en `CediDespachoService`.
