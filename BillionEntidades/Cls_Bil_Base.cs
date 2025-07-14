using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public abstract class Cls_Bil_Base
    {
        protected static Cls_Conexion sql_puntero = null;
        protected static Cls_ConexionVBS sql_punteroVBS = null;
        protected static Dictionary<string, object> parametros = null;
        public static String nueva_conexion = string.Empty;

        private string _IV_USUARIO_CREA = string.Empty;
        private DateTime? _IV_FECHA_CREA = null;

        public string IV_USUARIO_CREA { get => _IV_USUARIO_CREA; set => _IV_USUARIO_CREA = value; }
        public DateTime? IV_FECHA_CREA { get => _IV_FECHA_CREA; set => _IV_FECHA_CREA = value; }


        protected virtual void init()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            sql_punteroVBS = (sql_punteroVBS == null) ? Cls_ConexionVBS.Conexion() : sql_punteroVBS;
            parametros = new Dictionary<string, object>();
        }

    }
}
