using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace ClsAppCgsa
{
    public abstract class Base
    {
        protected static Cls_Conexion sql_puntero = null;
        protected static Dictionary<string, object> parametros = null;
        public static String nueva_conexion = string.Empty;

        private string _Create_user = string.Empty;
        private string _Modifie_user = string.Empty;
        private DateTime? _Create_date = null;
        private DateTime? _Modifie_date = null;
        private bool? _Status = null;

        public string Create_user { get => _Create_user; set => _Create_user = value; }
        public string Modifie_user { get => _Modifie_user; set => _Modifie_user = value; }
        public DateTime? Create_date { get => _Create_date; set => _Create_date = value; }
        public DateTime? Modifie_date { get => _Modifie_date; set => _Modifie_date = value; }
        public bool? Status { get => _Status; set => _Status = value; }

        protected virtual void init()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
        }

    }
}
