using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace ClsAutorizaciones
{
    public class CantidadUnidades : Base
    {

        #region "Variables"

        private Int64 _TOTAL;
      
        #endregion

        #region "Propiedades"
        public Int64 TOTAL { get => _TOTAL; set => _TOTAL = value; }
        

        #endregion

        private static String v_mensaje = string.Empty;

    
        public CantidadUnidades()
        {
            init();
  
        }

      
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        public static List<CantidadUnidades> CANTIDAD_UNIDADES(string _REFERENCIA, string _AUTORIZACION, string _LINEA_NAVIERA, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("REFERENCIA", _REFERENCIA);
            parametros.Add("AUTORIZACION", _AUTORIZACION);
            parametros.Add("LINEA_NAVIERA", _LINEA_NAVIERA);
            return sql_pointer.ExecuteSelectControl<CantidadUnidades>(v_conexion, 6000, "RVA_CANTIDAD_CONTENEDORES", parametros, out OnError);
          

        }

    }
}
