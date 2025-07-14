using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Turnos : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID_TURNO;
        private string _DESCRIPCION = string.Empty;
     
        #endregion

        #region "Propiedades"
        public Int64 ID_TURNO { get => _ID_TURNO; set => _ID_TURNO = value; }
        public string DESCRIPCION { get => _DESCRIPCION; set => _DESCRIPCION = value; }
      
        #endregion

        private static String v_mensaje = string.Empty;

        public Cls_Turnos()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_Turnos> Lista_Turnos( out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();
            
            return sql_puntero.ExecuteSelectControl<Cls_Turnos>(nueva_conexion, 6000, "checklist_turnos", null, out OnError);

        }


        public static List<Cls_Turnos> Lista_Turnos_Todos(out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();

            return sql_puntero.ExecuteSelectControl<Cls_Turnos>(nueva_conexion, 6000, "checklist_turnos_todos", null, out OnError);

        }


    }
}
