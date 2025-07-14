using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Lineas : Cls_Bil_Base
    {


        #region "Propiedades"
        public int id { get; set; }
        public string ruc { get; set; }
        public string codLine { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        #endregion


        public BTS_Lineas()
        {
            init();
        }

        public static List<BTS_Lineas> CboLineas(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BTS_Lineas>(sql_puntero.Conexion_Local, 6000, "BTS_CARGA_LINEAS", null, out OnError);
        }


    }

   
}
