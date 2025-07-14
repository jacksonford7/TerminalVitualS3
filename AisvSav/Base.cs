using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClsAisvSav
{
    public abstract class Base
    {

        protected static SQLHandler.sql_handler sql_pointer = null;
        protected static Dictionary<string, object> parametros = null;

        private DateTime? _create_date = null;
        private string _create_user = string.Empty;
        private DateTime? _mod_date = null;
        private string _mod_user = string.Empty;
        private string _mod_data = string.Empty;
        private bool? _state = false;
        private string _estado = string.Empty;
        private int _indice = 0;
        private string _action = string.Empty;

        public DateTime? Create_date { get => _create_date; set => _create_date = value; }
        public string Create_user { get => _create_user; set => _create_user = value; }
        public DateTime? Mod_date { get => _mod_date; set => _mod_date = value; }
        public string Mod_user { get => _mod_user; set => _mod_user = value; }
        public string Mod_data { get => _mod_data; set => _mod_data = value; }
        public bool? state { get => _state; set => _state = value; }
        public static String v_conexion = string.Empty;
        public string Estado { get => _estado; set => _estado = value; }
        public int Indice { get => _indice; set => _indice = value; }
        public string Action { get => _action; set => _action = value; }

        protected virtual void init()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }

    }
}
