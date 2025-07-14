using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Listado_Agentes : Cls_Bil_Base
    {
        #region "Variables"

      
        private string _ruc = string.Empty;
        private string _nombres = string.Empty;
        private string _codigo = string.Empty;
     

        #endregion

        #region "Propiedades"

        public string ruc { get => _ruc; set => _ruc = value; }
        public string nombres { get => _nombres; set => _nombres = value; }
        public string codigo { get => _codigo; set => _codigo = value; }
        


        #endregion

        public Cls_Bil_Listado_Agentes()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("N5");
        }

        public static List<Cls_Bil_Listado_Agentes> Listado_Agentes( out string OnError)
        {

            OnInit();
            //parametros.Clear();
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Listado_Agentes>(nueva_conexion, 6000, "[Bill].[listado_agente_aduana]", null, out OnError);

        }
    }


}
