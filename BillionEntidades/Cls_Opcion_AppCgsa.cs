using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Opcion_AppCgsa : Cls_Bil_Base
    {

        #region "Variables"

        private int? _valida;
        
        #endregion

        #region "Propiedades"

        public int? valida { get => _valida; set => _valida = value; }

        #endregion

        public Cls_Opcion_AppCgsa()
        {
            init();

        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_MASTER");
        }

        /*carga todas las facturas*/
        public static List<Cls_Opcion_AppCgsa> Tiene_Opcion(string usuario ,out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("usuario", usuario);
            return sql_puntero.ExecuteSelectControl<Cls_Opcion_AppCgsa>(nueva_conexion, 6000, "MA_SE_ConsultaOpcionAPP", parametros, out OnError);

        }

    }
}
