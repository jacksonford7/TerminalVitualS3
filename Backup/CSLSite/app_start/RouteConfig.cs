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
            routes.MapPageRoute("login_form", "csl/login", "~/cgsa.aspx");
            routes.MapPageRoute("form_menu", "csl/menu", "~/cuenta/zones.aspx");
            //objetos
            routes.MapPageRoute("aisv_cc", "aisv/container","~/aisv/container.aspx");
            routes.MapPageRoute("aisv_cs", "aisv/general", "~/aisv/cargasuelta.aspx");
            routes.MapPageRoute("aisv_csc","aisv/consolidar","~/aisv/cargaconsolidar.aspx");
            routes.MapPageRoute("aisv_cx", "aisv/consolidadora", "~/aisv/consolidadora.aspx");
            routes.MapPageRoute("aisv_bs", "aisv/buscar","~/aisv/consulta.aspx");
            //error
            routes.MapPageRoute("csl_err", "site/error", "~/shared/error.aspx");
            routes.MapPageRoute("advice_mty", "preaviso/nuevo", "~/advice/vacios.aspx");
            routes.MapPageRoute("advice_mty_c", "preaviso/cancelar", "~/advice/cancel.aspx");
            routes.MapPageRoute("advice_rpt", "preaviso/reporte", "~/advice/preimpresion.aspx");
            routes.MapPageRoute("advice_print", "preaviso/preaviso/print", "~/advice/preaviso.aspx");
            //catalogos
            routes.MapPageRoute("cat_line", "catalogo/lineas", "~/catalogo/lineas.aspx");
            routes.MapPageRoute("cat_agent","catalogo/agentes", "~/catalogo/agente.aspx");
            routes.MapPageRoute("cat_nav", "catalogo/naves", "~/catalogo/naves.aspx");
            routes.MapPageRoute("cat_na", "catalogo/exportadores", "~/catalogo/exportador.aspx");
            routes.MapPageRoute("cat_cho", "catalogo/chofer", "~/catalogo/chofer.aspx");
        

            //turnos
            routes.MapPageRoute("cfs_asigna", "turnos/asignar", "~/turnos/turnos.aspx");
            routes.MapPageRoute("cfs_cancela", "turnos/cancelar", "~/turnos/cancelar.aspx");
            routes.MapPageRoute("cat_bk_no", "catalogo/book", "~/catalogo/bookinList.aspx");
            routes.MapPageRoute("cat_bk_cal", "catalogo/calendar", "~/catalogo/Calendario.aspx");
            routes.MapPageRoute("cfs_report", "turnos/reportes", "~/turnos/reservas.aspx");

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
            routes.MapPageRoute("form_menudefault", "csl/menudefault", "~/cuenta/zonesDefault.aspx");

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
            routes.MapPageRoute("fac_manthorasreefer", "mantenimientos_proforma_expo/horas-reefer", "~/man_pro_expo/horas_reefer.aspx");
            routes.MapPageRoute("fac_mantpagoterceros", "mantenimientos_proforma_expo/pago-terceros", "~/man_pro_expo/pago_terceros.aspx");
            routes.MapPageRoute("fac_cathorasreefer", "mantenimientos_proforma_expo/lineas-asume-horas-reefer", "~/man_pro_expo/lineas_horas_reefer.aspx");
        }
    }
}