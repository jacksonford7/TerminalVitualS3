using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClsProformas;
using ControlOPC.Entidades;

namespace ClsProformas
{
   public class Activar_Turnos : Base
    {
        #region "Variables"

        private string _config_value = string.Empty;
       

        #endregion

        #region "Propiedades"

        public string config_value { get => _config_value; set => _config_value = value; }
       

        #endregion


        private static void OnInit_N4()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }


        public Activar_Turnos()
        {
            OnInit_N4();

        }

        public bool Obtener_Parametro(out string OnError)
        {
           
            var t = sql_pointer.ExecuteSelectOnly<Activar_Turnos>(v_conexion, 4000, "SP_GET_PARAMETRO", null);
            if (t == null)
            {
                this.config_value = "NO";
                OnError = "No fue posible obtener el parametro";
                return false;
            }
            this.config_value = t.config_value;
           
            OnError = string.Empty;
            return true;
        }
    }
}
