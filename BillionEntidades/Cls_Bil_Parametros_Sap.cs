using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
namespace BillionEntidades
{
    public class Cls_Bil_Parametros_Sap : Cls_Bil_Base
    {
        #region "Variables"

        private string _USER = string.Empty;
        private string _PASSWORD = string.Empty;
        private bool _VALIDACION = false;
        #endregion

        #region "Propiedades"

        public string USER { get => _USER; set => _USER = value; }
        public string PASSWORD { get => _PASSWORD; set => _PASSWORD = value; }
        public bool VALIDACION { get => _VALIDACION; set => _VALIDACION = value; }


        #endregion

        public Cls_Bil_Parametros_Sap()
        {
            init();

        }

        /*carga todas las notas de credito pendientes de aprobar de un uusario*/
        public static List<Cls_Bil_Parametros_Sap> Parametros(out string OnError)
        {
         
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Parametros_Sap>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Parametros_SAP", null, out OnError);

        }
    }
}
