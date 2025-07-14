using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.BananoBodega
{

    public class BAN_HorarioFinal_Bodega : Cls_Bil_Base
    {

        public string Id_Hora { get; set; }
        public int Id_HorarioIni { get; set; }
        public string codigo { get; set; }
        public string Desc_Hora { get; set; }
        public bool estado { get; set; }

        public BAN_HorarioFinal_Bodega()
        {
            init();
        }
        public static List<BAN_HorarioFinal_Bodega> ConsultarHorarioFinal(out string OnError)
        {
            parametros.Clear();
            return sql_punteroVBS.ExecuteSelectControl<BAN_HorarioFinal_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 8000, "BAN_CONSULTAR_HORARIO_BOD_FIN", null, out OnError);

        }

        public static BAN_HorarioFinal_Bodega GetHorarioFinal(int _id)
        {
            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            return sql_puntero.ExecuteSelectOnly<BAN_HorarioFinal_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_HORARIO_BOD_FIN_X_ID", parametros);
        }
    }
}
