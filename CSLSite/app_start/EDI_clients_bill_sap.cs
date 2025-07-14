using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite
{
    public class EDI_clients_bill_sap : BillionEntidades.Cls_Bil_Base
    {
        public string CLNT_CUSTOMER { get; set; }
        public string CLNT_NAME { get; set; }
        public string CLNT_CITY { get; set; }
        public string CLNT_STATE { get; set; }
        public string CLNT_ADRESS1 { get; set; }
        public DateTime CLNT_TRANSACTION_DATE { get; set; }
        public string CLNT_EMAIL { get; set; }
        public string CLNT_TYPE { get; set; }
        public string CLNT_EBILLING { get; set; }
        public string CLNT_FAX_INVC { get; set; }
        public string CLNT_RFC { get; set; }
        public string CLNT_ACTIVE { get; set; }
        public int? CLNT_CONT_CONSECUTIVO { get; set; }
        public string ROLE { get; set; }
        public string CODIGO_SAP { get; set; }
        public long CLNT_DIA_CREDITO { get; set; }

        #region "Constructores"
        public EDI_clients_bill_sap()
        {
            base.init();
        }
        #endregion

        #region "Metodos"
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }
        
        public static List<EDI_clients_bill_sap> ListClientes(string identificacion)
        {
            OnInit("N4Middleware");
            string msg;
            parametros.Clear();
            parametros.Add("i_CLNT_CUSTOMER", identificacion);
            return sql_puntero.ExecuteSelectControl<EDI_clients_bill_sap>(nueva_conexion, 2000, "EDI_consultarConsignatario", parametros, out msg);
        }

        #endregion
    }
}