using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace CSLSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //no enviar trazas
            routes.Ignore("{resource}.axd/{*pathInfo}");
            //no enviar configs
            routes.Ignore("{resource}.config/{*pathInfo}");
            //no enviar las master
            routes.Ignore("{resource}.master/{*pathInfo}");
            //sitio
            routes.MapPageRoute("login_form", "csl/login", "../login.aspx");
            routes.MapPageRoute("form_menu", "csl/menu", "~/cuenta/zones.aspx");
            //objetos



            routes.MapPageRoute("aisv_cc", "aisv/container", "~/aisv/container.aspx");
            routes.MapPageRoute("aisv_bs", "aisv/buscar", "~/aisv/consulta.aspx");
            routes.MapPageRoute("aisv_ccc", "aisv/reserva", "~/aisv/consulta_booking.aspx");

            //sna ADMIN
            routes.MapPageRoute("sna_bus", "sna/consulta", "~/sna/plus_consola.aspx");
            routes.MapPageRoute("sna_nue", "sna/nuevo", "~/sna/plus_usuario.aspx");


            routes.MapPageRoute("aisv_cs", "aisv/general", "~/aisv/cargasuelta.aspx");
            routes.MapPageRoute("aisv_csc", "aisv/consolidar", "~/aisv/cargaconsolidar.aspx");
            routes.MapPageRoute("aisv_cx", "aisv/consolidadora", "~/aisv/consolidadora.aspx");

            routes.MapPageRoute("ecuapass_dae", "aisv/consultar-dae", "~/ecuapass/consulta.aspx");



            //error
            routes.MapPageRoute("csl_err", "site/error", "~/shared/error.aspx");
            routes.MapPageRoute("advice_mty", "preaviso/nuevo", "~/advice/vacios.aspx");
            routes.MapPageRoute("advice_mty_c", "preaviso/cancelar", "~/advice/cancel.aspx");
            routes.MapPageRoute("advice_rpt", "preaviso/reporte", "~/advice/preimpresion.aspx");
            routes.MapPageRoute("advice_print", "preaviso/preaviso/print", "~/advice/preaviso.aspx");
            //catalogos
            routes.MapPageRoute("cat_line", "catalogo/lineas", "~/catalogo/lineas.aspx");
            routes.MapPageRoute("cat_agent", "catalogo/agentes", "~/catalogo/agente.aspx");
            routes.MapPageRoute("cat_nav", "catalogo/naves", "~/catalogo/naves.aspx");
            routes.MapPageRoute("cat_na", "catalogo/exportadores", "~/catalogo/exportador.aspx");
            routes.MapPageRoute("cat_cho", "catalogo/chofer", "~/catalogo/chofer.aspx");


            //turnos
            routes.MapPageRoute("cfs_asigna", "turnos/asignar", "~/turnos/turnos.aspx");
            routes.MapPageRoute("cfs_cancela", "turnos/cancelar", "~/turnos/cancelar.aspx");
            routes.MapPageRoute("cat_bk_no", "catalogo/book", "~/catalogo/bookinList.aspx");
            routes.MapPageRoute("cat_bk_cal", "catalogo/calendar", "~/catalogo/Calendario.aspx");
            routes.MapPageRoute("cfs_report", "turnos/reportes", "~/turnos/reservas.aspx");
            routes.MapPageRoute("cat_bk_aut", "catalogo/bookaut", "~/catalogo/bookinListAut.aspx");

            //consolidacion
            routes.MapPageRoute("consolidacion_asigna", "consolidacion/asignar", "~/consolidacion/turnos.aspx");
            routes.MapPageRoute("consolidacion_cancela", "consolidacion/cancelar", "~/consolidacion/cancelar.aspx");
            routes.MapPageRoute("catcon_bk_no", "catalogo/bookcon", "~/catalogo/bookinListConsolidacion.aspx");
            routes.MapPageRoute("catcon_bk_cal", "catalogo/calendarcon", "~/catalogo/calendarioConsolidacion.aspx");
            routes.MapPageRoute("consolidacion_report", "consolidacion/reportes", "~/consolidacion/reservas.aspx");

            //proformas
            routes.MapPageRoute("aisv_pro", "servicios/proforma", "~/portal/proforma.aspx");
            routes.MapPageRoute("cat_bo_pro", "catalogo/reserva", "~/catalogo/bookingPro.aspx");
            routes.MapPageRoute("aisv_pro_cancel", "servicios/anular", "~/portal/consultapro.aspx");


            //solicitud de atraque
            routes.MapPageRoute("cat_line1", "catalogo/linea", "~/catalogo/linea.aspx");
            routes.MapPageRoute("cat_buq", "catalogo/buque", "~/catalogo/buque.aspx");
            routes.MapPageRoute("cat_pto", "catalogo/puerto", "~/catalogo/puerto.aspx");
            routes.MapPageRoute("cat_soli", "solicitud/nueva", "~/atraque/solicitud.aspx");
            routes.MapPageRoute("cat_soli_cons", "solicitud/consultar", "~/atraque/consulta.aspx");
            //catalogo/operario

            //seguridad
            routes.MapPageRoute("seguridad_usuario", "seguridad/usuario", "~/seguridad/consultausuario.aspx");
            routes.MapPageRoute("seguridad_datosusuario", "seguridad/datosusuario", "~/seguridad/datosusuario.aspx");
            routes.MapPageRoute("seguridad_area", "seguridad/area", "~/seguridad/datosarea.aspx");
            routes.MapPageRoute("seguridad_grupo", "seguridad/grupo", "~/seguridad/datosgrupos.aspx");
            routes.MapPageRoute("seguridad_usuariogrupos", "seguridad/grupousuario", "~/seguridad/datosgruposusuarios.aspx");
            routes.MapPageRoute("seguridad_opciones", "seguridad/opcion", "~/seguridad/datosopcionservicios.aspx");
            routes.MapPageRoute("seguridad_permisos", "seguridad/permiso", "~/seguridad/datospermisos.aspx");
            routes.MapPageRoute("seguridad_servicio", "seguridad/servicio", "~/seguridad/datosservicio.aspx");
            routes.MapPageRoute("seguridad_empresa", "catalogo/empresas", "~/catalogo/empresas.aspx");
            routes.MapPageRoute("seguridad_cambiopassword", "csl/cambiopassword", "~/cuenta/cambiarpassword.aspx");
            routes.MapPageRoute("seguridad_recuperarpassword", "csl/recuperarpassword", "~/cuenta/recuperarpassword.aspx");
            routes.MapPageRoute("seguridad_recuperacion", "csl/recuperacioncliente", "~/recuperar/recuperacion.aspx");
            routes.MapPageRoute("seguridad_consultaadministrador", "seguridad/consultaadministrador", "~/seguridad/consultausuarioadministrador.aspx");
            routes.MapPageRoute("seguridad_operadorgrupos", "seguridad/grupooperador", "~/seguridad/datosgruposoperadores.aspx");
            routes.MapPageRoute("form_menuadmin", "csl/menuadmin", "~/cuenta/zonesAdmin.aspx");
            routes.MapPageRoute("form_menudefault", "csl/menudefault", "~/cuenta/menu.aspx");

            //Centro de servicios
            routes.MapPageRoute("censer_operario", "servicios/operario", "~/poolServicios/mantenimientoOperario.aspx");
            routes.MapPageRoute("censer_analista", "servicios/analistas", "~/poolServicios/mesaServicio.aspx");
            routes.MapPageRoute("censer_usugene", "servicios/solicitudusugen", "~/poolServicios/solicitudUsuarios.aspx");
            routes.MapPageRoute("censer_usurees", "servicios/solicitudusuree", "~/poolServicios/solicitudUsuariosReestiba.aspx");
            routes.MapPageRoute("censer_usucon", "servicios/solicitudusucons", "~/poolServicios/consultaSolicitudUsuario.aspx");
            routes.MapPageRoute("censer_usucere", "servicios/solicitudusucer", "~/poolServicios/solicitudUsuariosCerrojoElectronico.aspx");
            routes.MapPageRoute("censer_usulate", "servicios/solicitudusular", "~/poolServicios/solicitudUsuariosLateArrival.aspx");
            routes.MapPageRoute("censer_usucie", "servicios/solicitudusucie", "~/poolServicios/solicitudCorreccionExportacion.aspx");
            routes.MapPageRoute("censer_listemb", "servicios/listadoembarque", "~/poolServicios/listadoEmbarque.aspx");
            routes.MapPageRoute("censer_repLat", "servicios/reporteLateArrival", "~/poolServicios/reporteLateArrival.aspx");

            routes.MapPageRoute("censer_rtr", "servicios/revisiontecnica", "~/poolServicios/revisiontecnica.aspx");
            routes.MapPageRoute("censer_sel", "servicios/verficarsellos", "~/poolServicios/verficacion.aspx");
            routes.MapPageRoute("censer_rep", "servicios/repesaje", "~/poolServicios/repesaje.aspx");
            routes.MapPageRoute("censer_imo", "servicios/etiqueta", "~/poolServicios/etiquetar.aspx");


            //nuevo para cerrojo finalización
            routes.MapPageRoute("cerrojo_admin", "servicios/administrarCerrojo", "~/poolServicios/mantenimientoCerrojo.aspx");
            //../servicios/administrarCerrojo


            //reportes
            routes.MapPageRoute("GateIn", "navieras/gateIn", "~/navieras/GateIn.aspx");
            routes.MapPageRoute("GateOut", "navieras/gateOut", "~/navieras/GateOut.aspx");
            routes.MapPageRoute("RefCont", "navieras/contRefeer", "~/navieras/RefCont.aspx");
            routes.MapPageRoute("FullCntLoad", "navieras/fullLoad", "~/navieras/FullCntLoad.aspx");
            routes.MapPageRoute("EmptyCntLoad", "navieras/emptyLoad", "~/navieras/EmptyCntLoad.aspx");
            routes.MapPageRoute("FullCntDesc", "navieras/fullDischarge", "~/navieras/FullCntDesc.aspx");
            routes.MapPageRoute("EmptyCntDesc", "navieras/emptyDischarge", "~/navieras/EmptyCntDesc.aspx");
            routes.MapPageRoute("ConsCas", "navieras/cas", "~/navieras/ConsCas.aspx");
            routes.MapPageRoute("ConsTurn", "navieras/turnos", "~/navieras/ConsTurn.aspx");
            routes.MapPageRoute("ConsReqService", "navieras/request", "~/navieras/ConsReqService.aspx");


            //SIA ->NUEVO
            routes.MapPageRoute("sia_cntr_imo", "sia/contenedorrimo", "~/sia/cntrimo.aspx");
            routes.MapPageRoute("sia_bbk_imo", "sia/breakbulkimo", "~/sia/bbkimo.aspx");
            routes.MapPageRoute("sia_emp_in", "sia/vistantes", "~/sia/visitantes.aspx");
            routes.MapPageRoute("sia_emp_in_ex", "sia/externos", "~/sia/externoscgsa.aspx");
            routes.MapPageRoute("sia_emp_in_genera", "sia/conteconpersonal", "~/sia/conteconpersonal.aspx");

            //replicador

            routes.MapPageRoute("rep_cntr_aisv", "aisv/replicar", "~/aisv/replicar.aspx");
            //casvacios.aspx
            routes.MapPageRoute("aisv_cas", "aisv/retorno", "~/advice/casvacios.aspx");

            // Pago EN Linea
            routes.MapPageRoute("pago_en_linea_compesacion", "PagoEnLinea/Compensacion", "~/Pago En Linea/Compensacion.aspx");
            routes.MapPageRoute("pago_en_linea_consulta_anticipo", "PagoEnLinea/ConsultaAnticipo", "~/Pago En Linea/ConsultaAnticipo.aspx");

            //credenciales
            routes.MapPageRoute("sca_consultasolicitud", "credenciales/consultar-solicitud", "~/credenciales/consultasolicitud.aspx");
            routes.MapPageRoute("sca_revisasolicitudempresa", "credenciales/revision-solicitud-empresa", "~/credenciales/revisasolicitudempresa.aspx");
            routes.MapPageRoute("sca_revisasolicitudvehiculo", "credenciales/revision-solicitud-vehiculo", "~/credenciales/revisasolicitudvehiculo.aspx");
            routes.MapPageRoute("sca_revisasolicitudvehiculodocumentos", "credenciales/consulta/documentos-solicitud-vehiculo", "~/credenciales/revisasolicitudvehiculodocumentos.aspx");
            routes.MapPageRoute("sca_revisasolicitudcolaborador", "credenciales/revision-solicitud-colaborador", "~/credenciales/revisasolicitudcolaborador.aspx");
            routes.MapPageRoute("sca_revisasolicitudcolaboradordocumentos", "credenciales/consulta/documentos-solicitud-colaborador", "~/credenciales/revisasolicitudcolaboradordocumentos.aspx");
            routes.MapPageRoute("sca_revisasolicitudpermisoprovisional", "credenciales/revision-solicitud-permiso-provisional", "~/credenciales/revisasolicitudpermisoprovisional.aspx");
            routes.MapPageRoute("sca_revisasolicitudpermisoprovisionaldocumentos", "credenciales/consulta/documentos-solicitud-permiso-provisional", "~/credenciales/revisasolicitudpermisoprovisionaldocumentos.aspx");

            routes.MapPageRoute("sca_consultacomprobantedepagocolaborador", "credenciales/consultar-comprobante-de-pago-colaborador", "~/credenciales/consultacomprobantedepagocolaborador.aspx");
            routes.MapPageRoute("sca_consultacomprobantedepagovehiculo", "credenciales/consultar-comprobante-de-pago-vehiculo", "~/credenciales/consultacomprobantedepagovehiculos.aspx");

            routes.MapPageRoute("sca_consultasolicitudvehiculo", "credenciales/consultar-informacion-vehiculo", "~/credenciales/consultasolicitudvehiculo.aspx");
            routes.MapPageRoute("sca_consultasolicitudcolaborador", "credenciales/consultar-informacion-colaborador", "~/credenciales/consultasolicitudcolaborador.aspx");
            routes.MapPageRoute("sca_nohaydocumento", "credenciales/no-se-encontro-documento", "~/credenciales/nosecargodocumento.aspx");

            //cliente credenciales
            routes.MapPageRoute("sca_clienteconsultasolicitudfacturacolaborador", "cliente/consultar-factura-colaborador", "~/cliente/consultafacturacolaborador.aspx");
            routes.MapPageRoute("sca_clienteconsultasolicitudfacturavehiculo", "cliente/consultar-factura-vehiculo", "~/cliente/consultafacturavehiculo.aspx");
            routes.MapPageRoute("sca_clienteconsultaestadosolicitud", "cliente/consultar-estado-solicitud", "~/cliente/consultaestadosolicitud.aspx");
            routes.MapPageRoute("sca_clienteconsultacolaborador", "cliente/consulta-colaborador", "~/cliente/consultasolicitudcolaborador.aspx");
            routes.MapPageRoute("sca_clienteconsultapermisodeacceso", "cliente/consulta-permiso-de-acceso", "~/cliente/consultasolicitudpermisodeacceso.aspx");
            routes.MapPageRoute("sca_clienteconsultapermisodeaccesovehicular", "cliente/consulta-permiso-de-acceso-vehicular", "~/cliente/consultasolicitudpermisodeaccesovehiculo.aspx");
            routes.MapPageRoute("sca_clienteconsultadocumentoscolaborador", "cliente/consulta-documentos-colaborador", "~/cliente/consultasolicitudcolaboradordocumentos.aspx");
            routes.MapPageRoute("sca_clienteconsultavehiculos", "cliente/consulta-vehiculo", "~/cliente/consultasolicitudvehiculo.aspx");
            routes.MapPageRoute("sca_clienteconsultadocumentosvehiculos", "cliente/consulta-documentos-vehiculo", "~/cliente/consultasolicitudvehiculodocumentos.aspx");
            routes.MapPageRoute("sca_clienteconsultasolicitudpermisoprovisional", "cliente/consulta-permiso-provisional", "~/cliente/consultasolicitudpermisoprovisional.aspx");
            routes.MapPageRoute("sca_clienteconsultasolicitudpermisoprovisionaldocumentos", "cliente/consulta/consulta-documentos-permiso-provisional", "~/cliente/consultasolicitudpermisoprovisionaldocumentos.aspx");
            routes.MapPageRoute("sca_clienteconsultaempresa", "cliente/consulta-empresa", "~/cliente/consultasolicitudempresa.aspx");
            routes.MapPageRoute("sca_clientesolicitudempresa", "cliente/solicitud-empresa", "~/cliente/solicitudempresa.aspx");
            routes.MapPageRoute("sca_clientesolicitudcredencial", "cliente/solicitud-colaborador", "~/cliente/solicitudcolaborador.aspx");
            routes.MapPageRoute("sca_clientesolicitudcredencialprovisional", "cliente/solicitud-colaborador-provisional", "~/cliente/solicitudcolaboradorprovisional.aspx");
            routes.MapPageRoute("sca_clientesolicitudvehiculo", "cliente/solicitud-vehiculo", "~/cliente/solicitudvehiculo.aspx");
            routes.MapPageRoute("sca_clienteconsultasolicitud", "cliente/consultar-solicitud", "~/cliente/consultasolicitud.aspx");
            routes.MapPageRoute("sca_clienteactualizasolicitudempresa", "cliente/actualizar-datos-empresa", "~/cliente/actualizasolicitudempresa.aspx");
            routes.MapPageRoute("sca_clientelogin", "cliente/login", "~/logincliente/login.aspx");
            routes.MapPageRoute("sca_clienteconsultarechazosolicitudempresa", "cliente/consulta-rechazo-solicitud-empresa", "~/logincliente/consultasolicitudrechazoempresa.aspx");
            routes.MapPageRoute("sca_clientepermisosdeacceso", "cliente/permisos-de-acceso", "~/cliente/solicitudpermisodeacceso.aspx");
            routes.MapPageRoute("sca_clientepermisosdeaccesovehiculo", "cliente/permisos-de-acceso-vehiculo", "~/cliente/solicitudpermisodeaccesovehiculo.aspx");
            routes.MapPageRoute("sca_listadodeplacas", "catalogo/listado-de-placas", "~/catalogo/consultaPlacas.aspx");
            routes.MapPageRoute("sca_listadodeconductores", "catalogo/listado-de-conductores", "~/catalogo/consultaCoductorDesignado.aspx");
            routes.MapPageRoute("sca_listadodecolaboradores", "catalogo/listado-de-colaboradores", "~/catalogo/consultaColaboradorNominaOnlyControl.aspx");
            routes.MapPageRoute("sca_listacolaboradores", "catalogo/colaboradores", "~/catalogo/consultaColaboradorSCA.aspx");
            routes.MapPageRoute("sca_listacolaboradorespeaton", "catalogo/peaton", "~/catalogo/consultaColaboradorNominaPeaton.aspx");

            routes.MapPageRoute("sca_consultadatosvehiculo", "cliente/consultar-informacion-vehiculo", "~/cliente/consultadatosvehiculo.aspx");
            routes.MapPageRoute("sca_consultadatoscolaborador", "cliente/consultar-informacion-colaborador", "~/cliente/consultadatoscolaborador.aspx");
            //tesoreria credenciales
            routes.MapPageRoute("sca_tesoreriaconsultasolicitudfactura", "tesoreria/consultar-factura", "~/tesoreria/consultafactura.aspx");

            //mantenimientos
            routes.MapPageRoute("fac_mantcatpagoterceros", "mantenimientos_proforma_expo/mantenimiento-pago-terceros", "~/man_pro_expo/mant_pago_terceros.aspx");
            routes.MapPageRoute("fac_manthorasreefer", "mantenimientos_proforma_expo/horas-reefer", "~/man_pro_expo/horas_reefer.aspx");
            routes.MapPageRoute("fac_mantpagoterceros", "mantenimientos_proforma_expo/pago-terceros", "~/man_pro_expo/pago_terceros.aspx");
            routes.MapPageRoute("fac_mantdespbook", "mantenimientos_proforma_expo/despacho-bookings", "~/man_pro_expo/booking_autoriza.aspx");
            routes.MapPageRoute("fac_conpagoterceroscli", "mantenimientos_proforma_expo/consulta-pagos-asumidos-terceros", "~/man_pro_expo/consulta_pago_terceros_cli.aspx");
            routes.MapPageRoute("fac_conpagotercerosexpo", "mantenimientos_proforma_expo/consulta-pagos-terceros", "~/man_pro_expo/consulta_pago_terceros_expo.aspx");
            routes.MapPageRoute("fac_catbkg", "mantenimientos_proforma_expo/consulta-booking", "~/catalogo/bookinListRef.aspx");
            routes.MapPageRoute("fac_autbkg", "mantenimientos_proforma_expo/autoriza-booking", "~/catalogo/bookinListAut.aspx");
            routes.MapPageRoute("fac_getdae", "mantenimientos_proforma_expo/consulta-dae", "~/man_pro_expo/consulta_dae.aspx");

            //nueva estado de cuentaturnod
            routes.MapPageRoute("estado_cuenta", "PagoEnLinea/ConsultaSaldos", "~/portal/estadocta.aspx");

            //DAE
            routes.MapPageRoute("asignacion_dae", "consolidacion/asignacion-dae", "~/consolidacion/asociacion_dae.aspx");
            routes.MapPageRoute("consulta_asignacion_dae", "consolidacion/consulta-asignacion-dae", "~/catalogo/catalogo_asignacion_dae.aspx");

            //Pase de Puerta
            routes.MapPageRoute("emision_ppweb", "facturacion/emision-pase-de-puerta", "~/facturacion/emision_pasepuerta.aspx");
            routes.MapPageRoute("emision_ppweb_cfs", "facturacion/emision-pase-de-puerta-cfs", "~/facturacion/emision_pasepuerta_cfs.aspx");
            routes.MapPageRoute("emision_ppweb_brbk", "facturacion/emision-pase-de-puerta-breakbull", "~/facturacion/emision_pasepuerta_brbk.aspx");
            routes.MapPageRoute("impresion_ppweb", "facturacion/impresion-pase-de-puerta", "~/facturacion/wbareporte.aspx");
            routes.MapPageRoute("impresion_ppweb_cfs", "facturacion/impresion-pase-de-puerta-carga-suelta-cfs", "~/facturacion/wbareportecfs.aspx");
            routes.MapPageRoute("impresion_ppweb_brbk", "facturacion/impresion-pase-de-puerta-carga-breakbull", "~/facturacion/wbareportebrbk.aspx");
            routes.MapPageRoute("actualizacion_cancelacion_ppweb", "facturacion/actualizacion-o-cancelacion-pase-de-puerta", "~/facturacion/cancelacion_actualizacion_pasepuerta.aspx");
            routes.MapPageRoute("actualizacion_cancelacion_ppweb_cfs", "facturacion/actualizacion-o-cancelacion-pase-de-puerta-cfs", "~/facturacion/cancelacion_actualizacion_pasepuerta_cfs.aspx");
            routes.MapPageRoute("actualizacion_cancelacion_ppweb_breakbull", "facturacion/actualizacion-pase-de-puerta-breakbull", "~/facturacion/cancelacion_actualizacion_pasepuerta_brbk.aspx");
            routes.MapPageRoute("det_actualizacion_cancelacion_ppweb", "facturacion/detalle-cancelacion-actualizacion-pase-de-puerta", "~/facturacion/detalle_can_act_pasepuerta.aspx");

            //asociacion de transportista
            routes.MapPageRoute("adt_clientesolicitudcredencial", "transportista/crear-solicitud", "~/transportista/solicitud_colaborador.aspx");
            routes.MapPageRoute("adt_clienteregistrasolicitudentrada", "transportista/registra-solicitud-entrada-chofer-tag", "~/transportista/solicitud_entrada_chofer_tag.aspx");
            routes.MapPageRoute("adt_catalogociatrans", "transportista/catalogo-cia-transporte", "~/catalogo/ciatransporte.aspx");
            routes.MapPageRoute("adt_clienteconsultaestadosolicitud", "transportista/consulta-solicitud", "~/transportista/consulta_solicitud.aspx");
            routes.MapPageRoute("adt_consultacomprobantedepagocolaborador", "transportista/consulta-solicitud-colaborador", "~/transportista/consulta_solicitud_colaborador.aspx");

            routes.MapPageRoute("adt_consultacomprobantedepagocolaboradortagexistente", "transportista/consulta-solicitud-colaborador-tag-existente", "~/transportista/consulta_solicitud_colaborador_existente.aspx");
            routes.MapPageRoute("adt_consultacomprobantedepagocolaboradortagnuevo", "transportista/consulta-solicitud-colaborador-tag-nuevo", "~/transportista/consulta_solicitud_colaborador_nuevo.aspx");

            routes.MapPageRoute("adt_consultacomprobantedepagocolaboradorinfo", "transportista/consulta-solicitud-colaborador-info", "~/transportista/consulta_solicitud_colaborador_info.aspx");
            routes.MapPageRoute("adt_revisasolicitudcolaboradordocumentos", "transportista/consulta/documentos-solicitud-colaborador", "~/credenciales/revisasolicitudcolaboradordocumentos.aspx");
            routes.MapPageRoute("adt_bloqueapermisodeacceso", "transportista/bloquea-permiso-de-acceso", "~/transportista/bloquea_solicitud_colaborador.aspx");

            //zal
            routes.MapPageRoute("emision_ppwebzal", "zal/emision-pase-de-puerta-zal", "~/zal/proforma_zal.aspx");
            routes.MapPageRoute("cancelacion_ppwebzal", "zal/cancelacion-reimpresion-pase-de-puerta-zal", "~/zal/pases_zal.aspx");
            routes.MapPageRoute("impresion_ppwebzal", "zal/impresion-pase-zal", "~/zal/wbareportezal.aspx");
            routes.MapPageRoute("cat_bk_ZAL", "catalogo/bookZAL", "~/catalogo/bookinZAL.aspx");
            routes.MapPageRoute("actualizarturno_ppwebzal", "zal/actualizar-turno-pase-de-puerta-zal", "~/zal/turnos_zal.aspx");
            routes.MapPageRoute("transferencia_ppwebzal", "zal/transferencia-pase-de-puerta-zal", "~/zal/transferencia_zal.aspx");
            routes.MapPageRoute("reporte_zal", "zal/reporte-pase-de-puerta-zal", "~/zal/reporte_zal.aspx");
            routes.MapPageRoute("archivos_zal", "zal/subir-archivos-pase-de-puerta-zal", "~/zal/subirarchivo.aspx");
            routes.MapPageRoute("ver_archivos_zal", "zal/visualizar-archivos-pase-de-puerta-zal", "~/zal/visualizararchivo.aspx");

            //routes.MapPageRoute("mesnaje_zal", "zal/mensaje", "~/zal/mensaje.html");
            ///zal/emision-pase-de-puerta-zal
            ///
            //zona OPC
            routes.MapPageRoute("nuevo_plan", "opc/nueva", "~/opc/transaccionopc.aspx"); //1 nueva
            routes.MapPageRoute("aprueba_plan", "opc/aprobar_cuadrilla", "~/opc/iniciar_trabajos.aspx"); // 2 aprobar cuadrilla
            routes.MapPageRoute("anula_proformas", "opc/anular_proforma", "~/opc/anulacion_proformas.aspx");//3 anular
            routes.MapPageRoute("aprueba_plam_web", "opc/publicar", "~/opc/aprueba_plan_web.aspx"); //4 publicar 
            routes.MapPageRoute("inactiva_plan", "opc/inactivar", "~/opc/inactivar_trabajos.aspx"); //5 inactivar

            routes.MapPageRoute("genera_proforma", "opc/generar", "~/opc/generar_proforma.aspx"); // 6 generar
            routes.MapPageRoute("genera_adicional", "opc/adicional", "~/opc/generar_proforma_adicional.aspx");//7 adicional
            routes.MapPageRoute("desactiva_turno", "opc/finalizar", "~/opc/desactiva_turnosopc.aspx"); //8 desactivar turno
            routes.MapPageRoute("op_opc", "catalogo/operario", "~/catalogo/operario.aspx"); //catalogo
            routes.MapPageRoute("consulta_proforma", "opc/consultar_proforma", "~/opc/consulta_pro.aspx"); //9 consultar proforma
            routes.MapPageRoute("consulta_turno", "opc/turnos", "~/opc/consulta_turno.aspx"); //10 turnos disponibles para OPC
            routes.MapPageRoute("consulta_SUP", "opc/consulta", "~/opc/consulta_plan.aspx"); //11 consulta planes

            //Grupos
            routes.MapPageRoute("grupos", "opc/nuevo_grupo", "~/opc/grupo.aspx"); //11 consulta planes
            routes.MapPageRoute("grupos_consulta", "opc/consulta_grupo", "~/opc/consulta_grupo.aspx"); //11 consulta planes
            routes.MapPageRoute("opc_consulta", "opc/consulta_documentos", "~/opc/consulta_trabajos.aspx"); //11 consulta planes

            //stock MTY-RECEPTIO
            routes.MapPageRoute("line_depots", "inventario/depositos", "~/receptio/line_depots.aspx"); //1 agregue depositos
            routes.MapPageRoute("line_stock", "inventario/transacciones", "~/receptio/stock_register.aspx"); //1 agregue stock

            routes.MapPageRoute("line_stock_rpt_basico", "inventario/resumen-movimientos", "~/receptio/rpt_stock_register.aspx"); //1 agregue stock
            routes.MapPageRoute("line_stock_rpt_detalle", "inventario/detalle-movimientos", "~/receptio/rpt_stock_register_saldos.aspx"); //1 agregue stock

            //nota de credito
            routes.MapPageRoute("conceptos_nc", "notacredito/concepto", "~/nota_credito/frm_concepts.aspx");
            routes.MapPageRoute("usuarios_nc", "notacredito/usuarios", "~/nota_credito/frm_usuarios.aspx");
            routes.MapPageRoute("grupos_nc", "notacredito/grupos", "~/nota_credito/frm_grupos.aspx");
            routes.MapPageRoute("niveles_nc", "notacredito/niveles", "~/nota_credito/frm_nivel_aprobacion.aspx");
            routes.MapPageRoute("notacredito_nc", "notacredito/ncredito", "~/nota_credito/frm_nota_credito.aspx");
            routes.MapPageRoute("aprobar_notacredito", "notacredito/aprobar", "~/nota_credito/frm_aprobar_nota_credito.aspx");
            routes.MapPageRoute("notacredito_pendidentes", "notacredito/pendientes", "~/nota_credito/frm_pendientes_nota_credito.aspx");
            routes.MapPageRoute("rpt_niveles", "notacredito/rptniveles", "~/nota_credito/frm_rpt_niveles_aprobacion.aspx");
            routes.MapPageRoute("listado_gen_nc", "notacredito/listado", "~/nota_credito/frm_listado_res_nota_credito.aspx");
            routes.MapPageRoute("listado_det_nc", "notacredito/detalle", "~/nota_credito/frm_listado_det_nota_credito.aspx");
            routes.MapPageRoute("listado_res_nc", "notacredito/resumen", "~/nota_credito/frm_rpt_resumen_nota_credito.aspx");

            //retiros vacios
            routes.MapPageRoute("autorizacion", "autorizaciones/transaccion-autorizacion", "~/autorizaciones/autorizacion.aspx");
            routes.MapPageRoute("excluir_veh_chof", "autorizaciones/transaccion-solicitud-vacios", "~/autorizaciones/excluir_veh_chof.aspx");
            routes.MapPageRoute("list_veh_cho_exclu", "autorizaciones/listado-vehiculo-chofer-excluido", "~/autorizaciones/listado_veh_chof_excluidos.aspx");
            routes.MapPageRoute("list_veh_cho_aut", "autorizaciones/listado-vehiculo-chofer-autorizado", "~/autorizaciones/listado_veh_chof_autorizados.aspx");
            routes.MapPageRoute("archivo_veh_chof", "autorizaciones/enviar-archivo-vehiculo-chofer", "~/autorizaciones/cargar_archivo_contenedores.aspx");
            routes.MapPageRoute("list_orden_retiro", "autorizaciones/listado-orden-retiro", "~/autorizaciones/listado_orden_retiro.aspx");
            routes.MapPageRoute("log_errores_contenedor", "autorizaciones/listado-errores-proceso", "~/autorizaciones/errores_contenedores.aspx");

            routes.MapPageRoute("aisv_cc_sav", "sav/containermty", "~/sav/aisv_sav.aspx");
            routes.MapPageRoute("aisv_bs_sav", "sav/buscarmty", "~/sav/consulta_sav.aspx");
            routes.MapPageRoute("aisv_tr_sav", "sav/transfcontainermty", "~/sav/aisv_sav_transf.aspx");
            routes.MapPageRoute("aisv_tu_sav", "sav/turnosmty", "~/sav/turnosSav.aspx");
            routes.MapPageRoute("aisv_tu_rpe", "sav/reportcontainermty", "~/sav/reporte_sav.aspx");

            //nueva seccion de exportables
            routes.MapPageRoute("expo_unit_mty", "ret/unidad_vacia", "~/exportables/vacios.aspx");
            routes.MapPageRoute("expo_unit_wd", "ret/retiros", "~/exportables/vacios_despacho.aspx");
            //NUEVA OPCION CARBONO NEUTRO
            routes.MapPageRoute("cbn_consulta", "carbono/consulta", "~/carbono_neutro/consulta_cn.aspx");
          //  routes.MapPageRoute("aisv_pro", "servicios/proforma", "~/portal/proforma_cn.aspx");

        }
    }
}