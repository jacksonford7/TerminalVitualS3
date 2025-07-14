using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
 public   class VBS_ConsultarLineas : Cls_Bil_Base
    {



        public string IdLinea { get; set; }
        public string Linea { get ; set; }

      

        public VBS_ConsultarLineas()
        {
            init();
        }

        public static List<VBS_ConsultarLineas> ConsultarLineas(out string OnError)
        {
            parametros.Clear();       
            return sql_punteroVBS.ExecuteSelectControl<VBS_ConsultarLineas>(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_CONSULTAR_LINEAS", null, out OnError);
        }


        public static List<VBS_ConsultarLineas> ConsultarLineasTodas(out string OnError)
        {
            parametros.Clear();
            return sql_punteroVBS.ExecuteSelectControl<VBS_ConsultarLineas>(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_CONSULTAR_LINEAS_TODAS", null, out OnError);
        }



    }




}
