using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_TipoEquipos : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID_TIPO_EQUIPO;
        private string _DESCRIPCION = string.Empty;
     
        #endregion

        #region "Propiedades"
        public Int64 ID_TIPO_EQUIPO { get => _ID_TIPO_EQUIPO; set => _ID_TIPO_EQUIPO = value; }
        public string DESCRIPCION { get => _DESCRIPCION; set => _DESCRIPCION = value; }
      
        #endregion

        private static String v_mensaje = string.Empty;

        public Cls_TipoEquipos()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_TipoEquipos> Lista_TipoEquipos( out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();
            
            return sql_puntero.ExecuteSelectControl<Cls_TipoEquipos>(nueva_conexion, 6000, "checklist_tipos", null, out OnError);

        }

        public static List<Cls_TipoEquipos> Lista_TipoEquipos_Todos(out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();

            return sql_puntero.ExecuteSelectControl<Cls_TipoEquipos>(nueva_conexion, 6000, "checklist_tipos_Todos", null, out OnError);

        }


    }
}
