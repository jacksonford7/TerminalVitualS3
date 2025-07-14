using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace ClsAutorizaciones
{
    public class ValidaContenedor : Base
    {
        #region "Variables"

        private Int64 _CNTR_CONSECUTIVO = 0;
       
        private string _MENSAJE = string.Empty;

        #endregion

        #region "Propiedades"
        public Int64 CNTR_CONSECUTIVO { get => _CNTR_CONSECUTIVO; set => _CNTR_CONSECUTIVO = value; }
    

        #endregion

        private static String v_mensaje = string.Empty;

        public ValidaContenedor()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("N5_DB");
        }

        /*listado*/
        public static List<ValidaContenedor> Retorna_Gkey(string _referencia, string _xml , out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("ref", _referencia);
            parametros.Add("unit_all", _xml);
            return sql_pointer.ExecuteSelectControl<ValidaContenedor>(v_conexion, 6000, "FNA_FUN_CONTAINERS_EDO_REF", parametros, out OnError);
        }
    }
}
