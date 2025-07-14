using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using PasePuerta;

namespace BillionEntidades
{
    public class Cls_Bil_PasePuertaContenedor_InOut : Cls_Bil_Base
    {

        #region "Variables"

        
        private Int64 _VALOR = 0;
        private Int64 _GKEY = 0;

        #endregion

        #region "Propiedades"


        public Int64 VALOR { get => _VALOR; set => _VALOR = value; }
        public Int64 GKEY { get => _GKEY; set => _GKEY = value; }
       

        private static String v_mensaje = string.Empty;

        #endregion


        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("N5");
        }

        /*lista todas las proformas por rango de fechas*/
        public static List<Cls_Bil_PasePuertaContenedor_InOut> Pase_Utilizado(Int64 GKEY, out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("lv_GKEY", GKEY);
           
            return sql_puntero.ExecuteSelectControl<Cls_Bil_PasePuertaContenedor_InOut>(nueva_conexion, 4000, "FNA_FUN_CONTAINERS_IMPO_OUT", parametros, out OnError);

        }

    }
}
