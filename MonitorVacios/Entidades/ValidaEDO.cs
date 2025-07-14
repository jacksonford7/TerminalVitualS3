using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace MonitorVacios.Entidades
{
    public  class ValidaEDO : Base
    {
        #region "Variables"

        private Int64 _TOTAL = 0;
        private string _LINEA = string.Empty;
        private Int64 _GKEY = 0;

       
        #endregion

        #region "Propiedades"
        public Int64 TOTAL { get => _TOTAL; set => _TOTAL = value; }   
        public string LINEA { get => _LINEA; set => _LINEA = value; }  
        public Int64 GKEY { get => _GKEY; set => _GKEY = value; }

       
        #endregion

        private static String v_mensaje = string.Empty;


        public ValidaEDO()
        {
            init();

            OnInit();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("N5_DB");
        }

        public static bool Existe_EDO(Int64 GKEY, string LINEA, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("gkey", GKEY);
            parametros.Add("linea", LINEA);

            var t = sql_pointer.ExecuteSelectOnly<ValidaEDO>(v_conexion, 6000, "mb_get_edo_cnt", parametros);
            if (t == null)
            {
                OnError = "Error al validar EDO N4";
                return false;
            }

            if (t.TOTAL == 1)
            {
                OnError = string.Empty;
                return true;
            }
            else {
                OnError = string.Empty;
                return false;
            }
            
            
        }


    }
}
