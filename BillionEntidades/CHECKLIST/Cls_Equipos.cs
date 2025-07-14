using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Equipos : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID_EQUIPO;
        private string _NOMBRE = string.Empty;
     
        #endregion

        #region "Propiedades"
        public Int64 ID_EQUIPO { get => _ID_EQUIPO; set => _ID_EQUIPO = value; }
        public string NOMBRE { get => _NOMBRE; set => _NOMBRE = value; }
      
        #endregion

        private static String v_mensaje = string.Empty;

        public Cls_Equipos()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_Equipos> Lista_Equipos(Int64 ID_TIPO_EQUIPO, out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();
            parametros.Add("ID_TIPO_EQUIPO", ID_TIPO_EQUIPO);
            return sql_puntero.ExecuteSelectControl<Cls_Equipos>(nueva_conexion, 6000, "checklist_equipos", parametros, out OnError);

        }

        public static List<Cls_Equipos> Lista_Equipos_Todos(Int64 ID_TIPO_EQUIPO, out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();
            parametros.Add("ID_TIPO_EQUIPO", ID_TIPO_EQUIPO);
            return sql_puntero.ExecuteSelectControl<Cls_Equipos>(nueva_conexion, 6000, "checklist_equipos_todos", parametros, out OnError);

        }


    }
}
