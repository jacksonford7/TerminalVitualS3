using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using BillionEntidades;
namespace BillionAccesoDatos
{
    public class DA_Bill_Proforma_Listado : Cls_Bil_Base 
    {
        public DA_Bill_Proforma_Listado()
        {
            init();

        }

        /*lista todas las proformas por rango de fechas*/
        public  List<Cls_Bil_Proforma_Listado> BackOffice_Listado_Proformas(DateTime PF_FECHA_DESDE, DateTime PF_FECHA_HASTA,  out string OnError)
        {

            parametros.Clear();
            parametros.Add("PF_FECHA_DESDE", PF_FECHA_DESDE);
            parametros.Add("PF_FECHA_HASTA", PF_FECHA_HASTA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Proforma_Listado>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Rpt_BackOffice_Listado_Proformas", parametros, out OnError);

        }

        /*lista todas las proformas por rango de fechas*/
        public  List<Cls_Bil_Proforma_Listado> Listado_Proformas(DateTime PF_FECHA_DESDE, DateTime PF_FECHA_HASTA, string Usuario, string Agente, out string OnError)
        {
          
            parametros.Clear();
            parametros.Add("PF_FECHA_DESDE", PF_FECHA_DESDE);
            parametros.Add("PF_FECHA_HASTA", PF_FECHA_HASTA);
            parametros.Add("PF_USUARIO", Usuario);
            parametros.Add("PF_RUC", Agente);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Proforma_Listado>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Rpt_Listado_Proformas", parametros, out OnError);

        }


    }
}
