using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace ClsAutorizaciones
{
   public  class Naves : Base
    {
        #region "Variables"

        private Int64 _number;
        private string _codigo = string.Empty;
        private string _descripcion = string.Empty;
        private string _rol = string.Empty;
        #endregion

        #region "Propiedades"
        public Int64 number { get => _number; set => _number = value; }
        public string codigo { get => _codigo; set => _codigo = value; }
        public string descripcion { get => _descripcion; set => _descripcion = value; }
        public string rol { get => _rol; set => _rol = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public Naves()
        {
            init();
        }

        #region "Metodos"
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("N5_DB");
        }

        public static List<Naves> Lista_Naves(string pista, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("pista", pista);
            return sql_pointer.ExecuteSelectControl<Naves>(v_conexion, 6000, "RVA_BUSCADOR_NAVES", parametros, out OnError);

        }

       

        #endregion
    }
}
