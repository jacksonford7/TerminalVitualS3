using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Horarios_Aisv : Cls_Bil_Base
    {


        #region "Propiedades"
        public Int64 id { get; set; }
        public string value { get; set; }
        public string hora { get; set; }
      
        #endregion


        public BTS_Horarios_Aisv()
        {
            init();
        }

        public static List<BTS_Horarios_Aisv> CboLineas(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BTS_Horarios_Aisv>(sql_puntero.Conexion_Local, 6000, "BTS_HORAS_AISV", null, out OnError);
        }


    }

   
}
