using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class ZAL_Detalle_Error : Cls_Bil_Base
    {


        #region "Propiedades"
        public int fila { get; set; }
        public string contenedor { get; set; }
        public string cliente { get; set; }
        public string error { get; set; }
       
        #endregion


        public ZAL_Detalle_Error()
        {
            init();
        }

      
       

    }
}
