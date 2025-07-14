using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class SAV_Detalle_Servicios : Cls_Bil_Base
    {


        #region "Propiedades"

        public Int64 ID { get; set ; }
        public int LINEA { get ; set ; }
        public string ID_SERVICIO { get ; set ; }
        public string DESC_SERVICIO { get; set ; }
        public string CARGA { get; set ; }
        public DateTime? FECHA { get ; set; }

        public string TIPO_SERVICIO { get ; set ; }
        public decimal CANTIDAD { get ; set ; }
        public decimal PRECIO { get ; set; }
        public decimal SUBTOTAL { get ; set; }
        public decimal IVA { get ; set; }

        public string DRAFT { get ; set ; }
        public string ID_CODIGO { get ; set ; }

        #endregion


        public SAV_Detalle_Servicios()
        {
            init();
        }

      
       

    }
}
