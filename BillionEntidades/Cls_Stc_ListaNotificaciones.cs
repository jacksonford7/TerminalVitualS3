using System;
using System.Collections.Generic;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Stc_ListaNotificaciones : Cls_Bil_Base
    {
        public int? dex_row_id { get; set; }
        public string ms_para { get; set; }
        public string ms_copia { get; set; }
        public string ms_copia_oculta { get; set; }
        public string ms_asunto { get; set; }
        public string ms_mensaje { get; set; }
        public int? ms_proceso_id { get; set; }
        public int? ms_evento_id { get; set; }
        public long? ms_ext_fila { get; set; }
        public string formato { get; set; }
        public DateTime? fecha_reg { get; set; }
        public string ruc { get; set; }
        public string cliente { get; set; }
        public string contenedor { get; set; }
        public string mrn { get; set; }
        public string msn { get; set; }
        public string hsn { get; set; }
        public string tipoNotificacion { get; set; }
        public string bl { get; set; }
        public string booking { get; set; }
        public string nave { get; set; }
        public string categoria { get; set; }
        public string tipoCarga { get; set; }
        public DateTime? fechaEvento { get; set; }
        public string tituloNotificacion { get; set; }
        
        public Cls_Stc_ListaNotificaciones()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_STC");
        }

        public static List<Cls_Stc_ListaNotificaciones> Listado_Notificaciones( DateTime fechaDesde, DateTime fechaHasta, string contenedor, string evento, string numeroCarga, string ruc, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("i_fechaDesde", fechaDesde);
            parametros.Add("i_fechaHasta", fechaHasta);
            parametros.Add("i_container", contenedor);
            parametros.Add("i_evento", evento);
            parametros.Add("i_numeroCarga", numeroCarga);
            parametros.Add("i_ruc", ruc);
            return sql_puntero.ExecuteSelectControl<Cls_Stc_ListaNotificaciones>(nueva_conexion, 5000, "st_consultarNotificacionesImpo", parametros, out OnError);
        }
    }

    public class Cls_Stc_evento : Cls_Bil_Base
    {
        public int? dex_row_id { get; set; }
        public int? ev_id { get; set; }
        public string ev_descripcion { get; set; }
        public bool? ev_activo { get; set; }
        public string ev_notas { get; set; }
        public int? ev_proceso_id { get; set; }

        public Cls_Stc_evento()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_STC");
        }

        public static List<Cls_Stc_evento> Listado_Evento(string _tipo, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("i_tipo", _tipo);
            return sql_puntero.ExecuteSelectControl<Cls_Stc_evento>(nueva_conexion, 5000, "st_consultar_eventos", parametros, out OnError);
        }
    }

    public class Cls_Stc_configuracion : Cls_Bil_Base
    {
        public int c_modulo { get; set; }
        public string c_id { get; set; }
        public string c_valor { get; set; }
        public bool c_activo { get; set; }
        public string c_notas { get; set; }
        public int dex_row_id { get; set; }
        public string clase { get; set; }


        public Cls_Stc_configuracion()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_STC");
        }

        public static List<Cls_Stc_configuracion> obtenerConfiguracion(string _cID, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("i_c_id", _cID);
            return sql_puntero.ExecuteSelectControl<Cls_Stc_configuracion>(nueva_conexion, 5000, "st_obtenerConfiguracion", parametros, out OnError);
        }
    }

    public class Cls_Stc_Imagen : Cls_Bil_Base
    {
        public int dex_row_id { get; set; }
        public string ruc { get; set; }
        public long gkey { get; set; }
        public string contenedor { get; set; }
        public string mrn { get; set; }
        public string msn { get; set; }
        public string categoria { get; set; }
        public string tipoCarga { get; set; }
        public string tipoNotificacion { get; set; }
        public string imagen { get; set; }

        public Cls_Stc_Imagen()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_STC");
        }

        public static List<Cls_Stc_Imagen> obtenerImagenes(string ruc, string mrn, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("i_ruc", ruc);
            parametros.Add("i_mrn", mrn);
            return sql_puntero.ExecuteSelectControl<Cls_Stc_Imagen>(nueva_conexion, 5000, "st_obtenerImagenes", parametros, out OnError);
        }
    }
}
