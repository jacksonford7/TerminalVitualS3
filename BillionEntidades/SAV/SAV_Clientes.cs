using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class SAV_Clientes : Cls_Bil_Base
    {
   
        public string CLNT_CUSTOMER { get; set; }
        public string CLNT_NAME { get; set; }
        public string CLNT_ADRESS { get; set; }
        public string CLNT_EMAIL { get; set; }
        public string CLNT_TYPE { get; set; }
        public string CLNT_EBILLING { get; set; }
        public string CLNT_FAX_INVC { get; set; }
        public string CLNT_RFC { get; set; }
        public string CLNT_ACTIVE { get; set; }
        public string CLNT_ROLE { get; set; }
        public string CODIGO_SAP { get; set; }
        public Int64? DIAS_CREDITO { get; set; }
        public string CLNT_CITY { get; set; }
        public Int64? gkey { get; set; }

        public SAV_Clientes()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<SAV_Clientes> Buscador_Clientes(string filtro, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("filtro", filtro);
            return sql_puntero.ExecuteSelectControl<SAV_Clientes>(nueva_conexion, 6000, "sav_cliente_informacion", parametros, out OnError);
        }

        public static List<SAV_Clientes> Clientes_Todos(out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<SAV_Clientes>(nueva_conexion, 6000, "sav_cliente_todos", null, out OnError);
        }


        public bool PopulateMyData(out string OnError)
        {

            OnInit("N5");

            parametros.Clear();
            parametros.Add("gkey", this.gkey);

            var t = sql_puntero.ExecuteSelectOnly<SAV_Clientes>(nueva_conexion, 6000, "sav_cargar_cliente", parametros);

            if (t == null)
            {
                OnError = "No fue posible obtener datos del exportador";
                return false;
            }

            this.CLNT_CUSTOMER = t.CLNT_CUSTOMER;
            this.CLNT_NAME = t.CLNT_NAME;
            this.CLNT_ADRESS = t.CLNT_ADRESS;
            this.CLNT_EMAIL = t.CLNT_EMAIL;
            this.CLNT_TYPE = t.CLNT_TYPE;
            this.CLNT_EBILLING = t.CLNT_EBILLING;
            this.CLNT_FAX_INVC = t.CLNT_FAX_INVC;
            this.CLNT_RFC = t.CLNT_RFC;
            this.CLNT_ACTIVE = t.CLNT_ACTIVE;
            this.CLNT_ROLE = t.CLNT_ROLE;
            this.CODIGO_SAP = t.CODIGO_SAP;
            this.DIAS_CREDITO = t.DIAS_CREDITO;
            this.CLNT_CITY = t.CLNT_CITY;
            this.gkey = t.gkey;

          

            OnError = string.Empty;
            return true;
        }

    }
}
