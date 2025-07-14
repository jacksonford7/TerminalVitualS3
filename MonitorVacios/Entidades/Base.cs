using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorVacios.Entidades
{
    public abstract class Base
    {
        protected static SQLHandler.sql_handler sql_pointer = null;
        protected static Dictionary<string, object> parametros = null;
        public static String v_conexion = string.Empty;

        private string _action = string.Empty;

        
        public string Action { get => _action; set => _action = value; }

        protected virtual void init()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }

    }
}
