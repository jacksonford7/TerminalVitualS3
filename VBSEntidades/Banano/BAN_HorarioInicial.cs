using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.BananoMuelle
{

    public class BAN_HorarioInicial : Cls_Bil_Base
    {
        public int Id_Hora { get; set; }
        public string Desc_Hora { get; set; }
        public bool estado { get; set; }

        public BAN_HorarioInicial()
        {
            init();
        }
        public static List<BAN_HorarioInicial> ConsultarHorariosIniciales(out string OnError)
        {
            parametros.Clear();

            return sql_punteroVBS.ExecuteSelectControl<BAN_HorarioInicial>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_CONSULTAR_HORARIO_INI]", null, out OnError);

        }

        public static BAN_HorarioInicial GetHorarioInicio(int _id)
        {
            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            return sql_puntero.ExecuteSelectOnly<BAN_HorarioInicial>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_HORARIO_INI_X_ID", parametros);
        }

    }
}
