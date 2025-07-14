using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_MULTI_cfs_imprimir_pase : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID_PPWEB;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;

        private string _FACTURA = string.Empty;
        private string _CONTENEDOR = string.Empty;
        private string _CIATRANS = string.Empty;
        private string _ORDEN = string.Empty;
        private string _NUMERO_CARGA = string.Empty;
        private string _NUMERO_PASE_N4 = string.Empty;
        private string _MARCA = string.Empty;
        private string _TRANSPORTISTA_DESC = string.Empty;
        private int _CANTIDAD_CARGA = 0;
        private decimal _ID_PASE ;
        private string _D_TURNO = string.Empty;
        private DateTime _FECHA_EXPIRACION;
        private string _AGENTE = string.Empty;
        private string _AGENTE_DESC = string.Empty;
        private string _IMPORTADOR = string.Empty;
        private string _IMPORTADOR_DESC = string.Empty;
        private string _FACTURA_SERVICIO = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 ID_PPWEB { get => _ID_PPWEB; set => _ID_PPWEB = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }


        public string FACTURA { get => _FACTURA; set => _FACTURA = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public string CIATRANS { get => _CIATRANS; set => _CIATRANS = value; }
        public string ORDEN { get => _ORDEN; set => _ORDEN = value; }
        public string NUMERO_CARGA { get => _NUMERO_CARGA; set => _NUMERO_CARGA = value; }

        public string NUMERO_PASE_N4 { get => _NUMERO_PASE_N4; set => _NUMERO_PASE_N4 = value; }
        public string MARCA { get => _MARCA; set => _MARCA = value; }
        public string TRANSPORTISTA_DESC { get => _TRANSPORTISTA_DESC; set => _TRANSPORTISTA_DESC = value; }
        public int CANTIDAD_CARGA { get => _CANTIDAD_CARGA; set => _CANTIDAD_CARGA = value; }
        public decimal ID_PASE { get => _ID_PASE; set => _ID_PASE = value; }
        public string D_TURNO { get => _D_TURNO; set => _D_TURNO = value; }
        public DateTime FECHA_EXPIRACION { get => _FECHA_EXPIRACION; set => _FECHA_EXPIRACION = value; }
        public string AGENTE { get => _AGENTE; set => _AGENTE = value; }
        public string AGENTE_DESC { get => _AGENTE_DESC; set => _AGENTE_DESC = value; }

        public string IMPORTADOR { get => _IMPORTADOR; set => _IMPORTADOR = value; }
        public string IMPORTADOR_DESC { get => _IMPORTADOR_DESC; set => _IMPORTADOR_DESC = value; }
        public string FACTURA_SERVICIO { get => _FACTURA_SERVICIO; set => _FACTURA_SERVICIO = value; }
        public string DIRECCION { get ; set; }
        #endregion

        public P2D_MULTI_cfs_imprimir_pase()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<P2D_MULTI_cfs_imprimir_pase> Datos_Pase(string mrn, string ruc, DateTime? fecha, out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();
            parametros.Add("mrn", mrn);
            parametros.Add("ruc", ruc);
            parametros.Add("fecha", fecha);
            return sql_puntero.ExecuteSelectControl<P2D_MULTI_cfs_imprimir_pase>(nueva_conexion, 6000, "P2D_MULTI_lista_pase_web_cfs", parametros, out OnError);

        }

      
    }
}
