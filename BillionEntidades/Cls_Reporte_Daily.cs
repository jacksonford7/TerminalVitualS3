using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Reporte_Daily : Cls_Bil_Base
    {

        public Cls_Reporte_Daily()
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
        public static System.Data.DataTable Reporte_Daily (DateTime FECHA_INI, DateTime FECHA_FIN, int TIPO, int TIPO_FECHA, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("FECHA_INI", FECHA_INI);
            parametros.Add("FECHA_FIN", FECHA_FIN);
            parametros.Add("TIPO", TIPO);
            parametros.Add("TIPO_FECHA", TIPO_FECHA);
            return sql_puntero.ComadoSelectADatatable(nueva_conexion, 6000, "INV_PRO_INVOICES_EBILL_SYS_PAGO_NEW", parametros, out OnError);
        }


    }
}
