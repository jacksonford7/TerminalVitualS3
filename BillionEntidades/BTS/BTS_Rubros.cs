using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Rubros : Cls_Bil_Base
    {


        #region "Propiedades"
       
        public string INVOICETYPE { get; set; }
        public string CODIGO_TARIJA_N4 { get; set; }
        public string VALUE_TARIJA_N4 { get; set; }
        public string CONCEPTO { get; set; }
        public int CANTIDAD { get; set; }
     
        #endregion


        public BTS_Rubros()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        public static List<BTS_Rubros> Carga_RubrosBodegas(string REFERENCIA, string LINEA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            parametros.Add("LINEA", LINEA);
            return sql_puntero.ExecuteSelectControl<BTS_Rubros>(sql_puntero.Conexion_Local, 6000, "BTS_DETALLE_BODEGAS_RUBROS_N4", parametros, out OnError);
        }

        public static List<BTS_Rubros> Carga_RubrosMuelle(string REFERENCIA, string LINEA, out string OnError)
        {
            OnInit("Portal_Servicios");

            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            parametros.Add("LINEA", LINEA);
            return sql_puntero.ExecuteSelectControl<BTS_Rubros>(nueva_conexion, 6000, "BTS_DETALLE_MUELLE_RUBROS_N4", parametros, out OnError);
        }



    }


}
