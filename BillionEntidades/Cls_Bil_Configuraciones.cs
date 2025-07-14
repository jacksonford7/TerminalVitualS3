using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_Bil_Configuraciones : Cls_Bil_Base
    {
        #region "Variables"

        private string _NOMBRE = string.Empty;
        private string _VALOR = string.Empty;
       
        #endregion

        #region "Propiedades"

        public string NOMBRE { get => _NOMBRE; set => _NOMBRE = value; }
        public string VALOR { get => _VALOR; set => _VALOR = value; }



        #endregion

        public Cls_Bil_Configuraciones()
        {
            init();

        }

        /*carga todas las notas de credito pendientes de aprobar de un uusario*/
        public static List<Cls_Bil_Configuraciones> Parametros(out string OnError)
        {

            return sql_puntero.ExecuteSelectControl<Cls_Bil_Configuraciones>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Extrae_Configuraciones", null, out OnError);

        }

        public static List<Cls_Bil_Configuraciones> Get_Validacion(string TipoValidacion, out string OnError)
        {
            parametros.Clear();
            parametros.Add("nombre", TipoValidacion);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Configuraciones>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Extrae_Validaciones", parametros, out OnError);

        }
    }
}
