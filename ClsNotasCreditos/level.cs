using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsNotasCreditos
{
    public class level : Base
    {

        #region "Variables"

        private int _id_level;
        private string _description = string.Empty;
        #endregion

        #region "Propiedades"
        public int id_level { get => _id_level; set => _id_level = value; }
        public string description { get => _description; set => _description = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public level()
        {
            init();
        }
        public level(int _id_level, string _description = null)
        {
            this.id_level = _id_level; this.description = _description;
        }

        #region "Metodos"

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("NOTA_CREDITO");
        }

        public static List<level> ListNiveles()
        {
            OnInit();

            return sql_pointer.ExecuteSelectControl<level>(v_conexion, 2000, "nc_c_level", null, out v_mensaje);

        }

        #endregion

    }
}
