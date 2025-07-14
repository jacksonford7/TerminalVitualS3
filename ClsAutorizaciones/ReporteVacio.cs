using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace ClsAutorizaciones
{
    public class ReporteVacio : Base
    {

        #region "Variables"

        private Int64 _ID;
        private string _AUTORIZACION = string.Empty;
        private string _REFERENCIA = string.Empty;
        private string _USUARIO_CRE = string.Empty;
        private string _LINEA_NAVIERA = string.Empty;
        private int _TOTAL_CONTENEDORES = 0;
        private int _TOTAL_PENDIENTES = 0;
        private int _TOTAL_PROCESADOS = 0;
        private string _ESTADO_TRANSACCION = string.Empty;
      

        private DateTime? _FECHA;
        private DateTime? _FECHA_CREA;

        private string _XML_STRING = string.Empty;
        private Int64 _CNTR_CONSECUTIVO=0;
        private string _CNTR_YARD_STATUS = string.Empty;
        private string _CNTR_VEPR_REFERENCE = string.Empty;
        private string _CNTR_CLNT_CUSTOMER_LINE = string.Empty;
        private string _NAVE = string.Empty;
        private string _CHOFER = string.Empty;
        private string _CAMION = string.Empty;
        private string _XML_CONTENEDORES = string.Empty;
        private int _RETIRADA = 0;
        private int _POR_RETIRADA = 0;

        #endregion

        #region "Propiedades"
        public Int64 ID { get => _ID; set => _ID = value; }
        public string AUTORIZACION { get => _AUTORIZACION; set => _AUTORIZACION = value; }
        public string REFERENCIA { get => _REFERENCIA; set => _REFERENCIA = value; }
        public string USUARIO_CRE { get => _USUARIO_CRE; set => _USUARIO_CRE = value; }
        public string LINEA_NAVIERA { get => _LINEA_NAVIERA; set => _LINEA_NAVIERA = value; }
        public int TOTAL_CONTENEDORES { get => _TOTAL_CONTENEDORES; set => _TOTAL_CONTENEDORES = value; }
        public int TOTAL_PENDIENTES { get => _TOTAL_PENDIENTES; set => _TOTAL_PENDIENTES = value; }
        public int TOTAL_PROCESADOS { get => _TOTAL_PROCESADOS; set => _TOTAL_PROCESADOS = value; }

        public string ESTADO_TRANSACCION { get => _ESTADO_TRANSACCION; set => _ESTADO_TRANSACCION = value; }
       
        public DateTime? FECHA { get => _FECHA; set => _FECHA = value; }
        public DateTime? FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }

        public string XML_STRING { get => _XML_STRING; set => _XML_STRING = value; }
        public Int64 CNTR_CONSECUTIVO { get => _CNTR_CONSECUTIVO; set => _CNTR_CONSECUTIVO = value; }
        public string CNTR_YARD_STATUS { get => _CNTR_YARD_STATUS; set => _CNTR_YARD_STATUS = value; }
        public string CNTR_VEPR_REFERENCE { get => _CNTR_VEPR_REFERENCE; set => _CNTR_VEPR_REFERENCE = value; }
        public string CNTR_CLNT_CUSTOMER_LINE { get => _CNTR_CLNT_CUSTOMER_LINE; set => _CNTR_CLNT_CUSTOMER_LINE = value; }

        public string NAVE { get => _NAVE; set => _NAVE = value; }
        public string CHOFER { get => _CHOFER; set => _CHOFER = value; }
        public string CAMION { get => _CAMION; set => _CAMION = value; }

        public int RETIRADA { get => _RETIRADA; set => _RETIRADA = value; }
        public int POR_RETIRADA { get => _POR_RETIRADA; set => _POR_RETIRADA = value; }
        public string XML_CONTENEDORES { get => _XML_CONTENEDORES; set => _XML_CONTENEDORES = value; }
        #endregion

        private static String v_mensaje = string.Empty;

      

        public ReporteVacio()
        {
            init();

        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        private static void OnInitN5()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("N5_DB");
        }

        public static List<ReporteVacio> Listado_Ordenes(DateTime desde, DateTime hasta,string linea, string referencia,  out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("DESDE", desde);
            parametros.Add("HASTA", hasta);
            parametros.Add("LINEA", linea);
            parametros.Add("REFERENCIA", referencia);
            return sql_pointer.ExecuteSelectControl<ReporteVacio>(v_conexion, 6000, "RVA_LISTADO_ORDENES_RETIRO", parametros, out OnError);

        }

        public static string Xml_Contenedores(Int64 ID, out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("ID", ID);
           
            var t = sql_pointer.ExecuteSelectOnly<ReporteVacio>(v_conexion, 6000, "RVA_LISTADO_GKEY_CONTENEDORES", parametros);
            if (t == null)
            {
                OnError = string.Empty;
                return string.Empty;
            }

           
            OnError = string.Empty;
            return t.XML_STRING;
        }

        public static List<ReporteVacio> Estados_Contenedores(string _unit_all, out string OnError)
        {
            OnInitN5();
            parametros.Clear();
            parametros.Add("type", "IMPO");
            parametros.Add("unit_all", _unit_all);
            return sql_pointer.ExecuteSelectControl<ReporteVacio>(v_conexion, 6000, "FNA_FUN_CONTAINERS_EDO", parametros, out OnError);
        }

        public static List<ReporteVacio> Cantidad_Contenedores(string _unit_all, out string OnError)
        {
            OnInitN5();
            parametros.Clear();
            parametros.Add("type", "IMPO");
            parametros.Add("unit_all", _unit_all);
            return sql_pointer.ExecuteSelectControl<ReporteVacio>(v_conexion, 6000, "FNA_FUN_CONTAINERS_EDO_RESUMEN", parametros, out OnError);
        }

    }
}
