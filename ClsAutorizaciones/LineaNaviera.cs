using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsAutorizaciones
{
    public class LineaNaviera : Base
    {

        #region "Variables"

        private string _ID = string.Empty;
        private string _NAME = string.Empty;
        
        private string _MENSAJE = string.Empty;

        #endregion

        #region "Propiedades"
        public string ID { get => _ID; set => _ID = value; }
        public string NAME { get => _NAME; set => _NAME = value; }
        
        #endregion

        private static String v_mensaje = string.Empty;

        public LineaNaviera()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("N5_DB");
        }

        /*listado*/
        public static List<LineaNaviera> Buscador_Lineas(string _criterio, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("pista", _criterio);
            return sql_pointer.ExecuteSelectControl<LineaNaviera>(v_conexion, 6000, "RVA_FUN_GET_INFO_LINE", parametros, out OnError);
        }

    }
}
