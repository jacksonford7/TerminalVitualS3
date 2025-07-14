using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_CheckList_Reporte : Cls_Bil_Base
    {

        public Cls_CheckList_Reporte()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        /*listado de pases a visualizar archivo*/
        public static System.Data.DataTable Reporte_Checklist (DateTime FECHA_INI, DateTime FECHA_FIN, Int64 ID_EQUIPO, Int64 ID_TURNO, Int64 ID_TIPO_EQUIPO, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("FECHA_DESDE", FECHA_INI);
            parametros.Add("FECHA_HASTA", FECHA_FIN);
            parametros.Add("ID_EQUIPO", ID_EQUIPO);
            parametros.Add("ID_TURNO", ID_TURNO);
            parametros.Add("ID_TIPO_EQUIPO", ID_TIPO_EQUIPO);
            return sql_puntero.ComadoSelectADatatable(nueva_conexion, 6000, "checklist_reporte", parametros, out OnError);
        }


    }
}
