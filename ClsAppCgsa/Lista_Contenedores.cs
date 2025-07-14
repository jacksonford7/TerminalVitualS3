using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace ClsAppCgsa
{
    public class Lista_Contenedores : Base
    {
        #region "Variables"

        private Int64? _UnitGkey = 0;
        private string _Container = string.Empty;
        private string _categoria = string.Empty;
        private DateTime? _event_date;

        private Int64? _idNotification;
        private string _Name = string.Empty;
        private Int64? _Id;
        private string _cuerpo = string.Empty;
        private string _Photo1 = string.Empty;
        private string _Photo2 = string.Empty;
        private string _Photo3 = string.Empty;
        private string _Photo4 = string.Empty;

        private string _cert_secuencia = string.Empty;
        private Int64? _id_certificado_carbo;
        private Int64? _idNotificacion;
        private bool _tiene_imagen;
        #endregion

        #region "Propiedades"
        public Int64? UnitGkey { get => _UnitGkey; set => _UnitGkey = value; }
        public string Container { get => _Container; set => _Container = value; }
        public string categoria { get => _categoria; set => _categoria = value; }
        public DateTime? event_date { get => _event_date; set => _event_date = value; }

        public Int64? idNotification { get => _idNotification; set => _idNotification = value; }
        public string Name { get => _Name; set => _Name = value; }
        public Int64? Id { get => _Id; set => _Id = value; }
        public string cuerpo { get => _cuerpo; set => _cuerpo = value; }
        public string Photo1 { get => _Photo1; set => _Photo1 = value; }
        public string Photo2 { get => _Photo2; set => _Photo2 = value; }
        public string Photo3 { get => _Photo3; set => _Photo3 = value; }
        public string Photo4 { get => _Photo4; set => _Photo4 = value; }

        public string cert_secuencia { get => _cert_secuencia; set => _cert_secuencia = value; }
        public Int64?   id_certificado_carbo { get => _id_certificado_carbo; set => _id_certificado_carbo = value; }
        public Int64? idNotificacion { get => _idNotificacion; set => _idNotificacion = value; }
        public bool tiene_imagen { get => _tiene_imagen; set => _tiene_imagen = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public Lista_Contenedores()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("APPCGSA");
        }


        public static List<Lista_Contenedores> Listado_Contenedores(string client_id, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("client_id", client_id);
            return sql_puntero.ExecuteSelectControl<Lista_Contenedores>(nueva_conexion, 5000, "APC_List_container", parametros, out OnError);

        }

        public static List<Lista_Contenedores> Listado_Eventos_Contenedores(string client_id, Int64 container_i, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("client_id", client_id);
            parametros.Add("container_i", container_i);
            return sql_puntero.ExecuteSelectControl<Lista_Contenedores>(nueva_conexion, 5000, "APC_List_event_container_tv", parametros, out OnError);

        }

        public static List<Lista_Contenedores> Listado_Contenedores_Filtro(string client_id, string contenedor, DateTime inicio, DateTime fin,  out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("client_id", client_id);
            parametros.Add("contenedor", contenedor);
            parametros.Add("inicio", inicio);
            parametros.Add("fin", fin);
            return sql_puntero.ExecuteSelectControl<Lista_Contenedores>(nueva_conexion, 5000, "APC_List_container_filtro", parametros, out OnError);

        }

        #region "NUEVOS SP"
        public static List<Lista_Contenedores> Listado_Contenedores_AppCgsa(string client_id, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("client_id", client_id);
            return sql_puntero.ExecuteSelectControl<Lista_Contenedores>(nueva_conexion, 5000, "APC_List_container_detail_tv", parametros, out OnError);

        }

        public static List<Lista_Contenedores> Listado_Eventos_AppCgsa(Int64 notificacion_id, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("notificacion_id", notificacion_id);
            return sql_puntero.ExecuteSelectControl<Lista_Contenedores>(nueva_conexion, 5000, "APC_id_event_container_tv", parametros, out OnError);

        }

        public static List<Lista_Contenedores> Listado_Contenedores_Filtro_AppCgsa(string client_id, DateTime fecha_desde, DateTime fecha_hasta, string numero_carga, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("client_id", client_id);
            parametros.Add("fecha_desde", fecha_desde);
            parametros.Add("fecha_hasta", fecha_hasta);
            parametros.Add("numero_carga", numero_carga);
            return sql_puntero.ExecuteSelectControl<Lista_Contenedores>(nueva_conexion, 5000, "APC_List_container_detail_filtro_tv", parametros, out OnError);

        }


        #endregion

    }
}
