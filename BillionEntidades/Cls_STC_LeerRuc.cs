using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_STC_LeerRuc : Cls_Bil_Base
    {
        #region "Variables"

       

        private string _ruc = string.Empty;
        private string _CLNT_CUSTOMER = string.Empty;
        private string _CLNT_NAME = string.Empty;
        private string _CLNT_ADRESS = string.Empty;
        private string _CLNT_EMAIL = string.Empty;
        private string _telefono = string.Empty;
        #endregion

        #region "Propiedades"


        public string ruc { get => _ruc; set => _ruc = value; }
        public string CLNT_CUSTOMER { get => _CLNT_CUSTOMER; set => _CLNT_CUSTOMER = value; }
        public string CLNT_NAME { get => _CLNT_NAME; set => _CLNT_NAME = value; }
        public string CLNT_ADRESS { get => _CLNT_ADRESS; set => _CLNT_ADRESS = value; }
        public string telefono { get => _telefono; set => _telefono = value; }
        public string CLNT_EMAIL { get => _CLNT_EMAIL; set => _CLNT_EMAIL = value; }

        #endregion

        public Cls_STC_LeerRuc()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        public static List<Cls_STC_LeerRuc> Leer( string ruc_codificado, out string OnError)
        {
            OnInit("PORTAL_BILLION");
            parametros.Clear();
            parametros.Add("ruc_codificado", ruc_codificado);
  
            return sql_puntero.ExecuteSelectControl<Cls_STC_LeerRuc>(sql_puntero.Conexion_Local, 6000, "stc_leer_ruc", parametros, out OnError);

        }

        public static List<Cls_STC_LeerRuc> Info_Cliente(string id, out string OnError)
        {
            OnInit("N5");
            parametros.Clear();
            parametros.Add("id", id);

            return sql_puntero.ExecuteSelectControl<Cls_STC_LeerRuc>(nueva_conexion, 6000, "[Bill].[cliente_informacion_stc]", parametros, out OnError);

        }
    }
}
