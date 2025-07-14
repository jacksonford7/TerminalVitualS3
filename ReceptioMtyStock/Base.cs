using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ReceptioMtyStock
{
    public abstract class Base
    {

        protected static SQLHandler.sql_handler sql_pointer = null;
        protected static Dictionary<string, object> parametros = null;

        private Nullable<DateTime> _create_date = null;
        private string _create_user = string.Empty;
        private Nullable<DateTime> _mod_date = null;
        private string _mod_user = string.Empty;
        private string _mod_data = string.Empty;

        public DateTime? Create_date { get => _create_date; set => _create_date = value; }
        public string Create_user { get => _create_user; set => _create_user = value; }
        public DateTime? Mod_date { get => _mod_date; set => _mod_date = value; }
        public string Mod_user { get => _mod_user; set => _mod_user = value; }
        public string Mod_data { get => _mod_data; set => _mod_data = value; }
        public bool? active { get; set; }
        public static String v_conexion = string.Empty;

        protected virtual void init()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }

    }
}
