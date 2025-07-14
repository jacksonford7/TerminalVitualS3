using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.BananoBodega
{

    public class BAN_HorarioInicial_Bodega : Cls_Bil_Base
    {
        public int Id_Hora { get; set; }
        public string Desc_Hora { get; set; }
        public bool estado { get; set; }

        public BAN_HorarioInicial_Bodega()
        {
            init();
        }
        public static List<BAN_HorarioInicial_Bodega> ConsultarHorariosIniciales(out string OnError)
        {
            parametros.Clear();

            return sql_punteroVBS.ExecuteSelectControl<BAN_HorarioInicial_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_CONSULTAR_HORARIO_BOD_INI]", null, out OnError);

        }

        public static BAN_HorarioInicial_Bodega GetHorarioInicio(int _id)
        {
            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            return sql_puntero.ExecuteSelectOnly<BAN_HorarioInicial_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_HORARIO_BOD_INI_X_ID", parametros);
        }

    }
}
