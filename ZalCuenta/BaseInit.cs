using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZalCuenta
{
    public abstract class BaseInit
    {
        protected static SQLHandler.sql_handler sql_pointer = null;
        protected static Dictionary<string, object> parametros = null;
        public static string error_mensaje;

        protected virtual void init()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }
    }
}
