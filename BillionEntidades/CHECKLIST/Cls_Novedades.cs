using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Novedades : Cls_Bil_Base
    {
        #region "Variables"

        private string _ID = string.Empty;
        private string _NAME = string.Empty;

        private string _MENSAJE = string.Empty;


        private Int64 _ID_NOVEDAD;
        private string _NOVEDAD = string.Empty;
        private string _DESCRIPCION = string.Empty;
        #endregion

        #region "Propiedades"
        public string ID { get => _ID; set => _ID = value; }
        public string NAME { get => _NAME; set => _NAME = value; }

        public Int64 ID_NOVEDAD { get => _ID_NOVEDAD; set => _ID_NOVEDAD = value; }
        public string NOVEDAD { get => _NOVEDAD; set => _NOVEDAD = value; }
        public string DESCRIPCION { get => _DESCRIPCION; set => _DESCRIPCION = value; }

        #endregion
        private static String v_mensaje = string.Empty;

        public Cls_Novedades()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_Novedades> Lista_Novedades(string _criterio, Int64 ID_TIPO_EQUIPO, out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();
            parametros.Add("ID_TIPO_EQUIPO", ID_TIPO_EQUIPO);
            parametros.Add("pista", _criterio);
            return sql_puntero.ExecuteSelectControl<Cls_Novedades>(nueva_conexion, 6000, "checklist_novedades", parametros, out OnError);

        }

        public static List<Cls_Novedades> Buscador_Novedades(string _criterio, Int64 ID_TIPO_EQUIPO, out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();
            parametros.Add("ID_TIPO_EQUIPO", ID_TIPO_EQUIPO);
            parametros.Add("pista", _criterio);
            return sql_puntero.ExecuteSelectControl<Cls_Novedades>(nueva_conexion, 6000, "checklist_buscador_novedades", parametros, out OnError);

        }

        public bool PopulateMyData(out string OnError)
        {
            OnInit("N4Middleware");

            parametros.Clear();
            parametros.Add("ID_NOVEDAD", this.ID_NOVEDAD);
           
            var t = sql_puntero.ExecuteSelectOnly<Cls_Novedades>(nueva_conexion, 6000, "carga_info_novedades", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener datos de la novedad";
                return false;
            }

            this.ID_NOVEDAD = t.ID_NOVEDAD;
            this.NOVEDAD = t.NOVEDAD;
            this.DESCRIPCION = t.DESCRIPCION;

            OnError = string.Empty;
            return true;
        }


    }
}
