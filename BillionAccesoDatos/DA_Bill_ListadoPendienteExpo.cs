using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using BillionEntidades;

namespace BillionAccesoDatos
{
    public class DA_Bill_ListadoPendienteExpo : Cls_Bil_Base
    {
        public DA_Bill_ListadoPendienteExpo()
        {
            init();

        }

        /*lista todas las proformas por rango de fechas*/
        public List<Cls_Bill_CabeceraExpo> Listado_Pendiente_Expo(out string OnError)
        {

            return sql_puntero.ExecuteSelectControl<Cls_Bill_CabeceraExpo>(sql_puntero.Conexion_Local, 5000, "sp_Bil_Rpt_Listado_Pendiente_Expo", null, out OnError);

        }
    }
}
