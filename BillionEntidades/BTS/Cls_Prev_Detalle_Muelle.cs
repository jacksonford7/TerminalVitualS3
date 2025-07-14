using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class Cls_Prev_Detalle_Muelle : Cls_Bil_Base
    {

  

        #region "Propiedades"

        public Int64 ID { get; set ; }
        public int Fila { get; set; }
        public string idNave { get; set; }
        public string codLine { get; set; }
        public string nave { get; set; }
        public string aisv_codig_clte { get; set; }
        public string aisv_nom_expor { get; set; }
        public string aisv_estado { get; set; }
        public int cajas { get; set; }
        public int cajas_confirmar { get; set; }
        public int cajas_paletizado { get; set; }
        public int idExportador { get; set; }
        public string Exportador { get; set; }
        public string ruc { get; set; }
        public string linea { get; set; }
        public string referencia { get; set; }
        #endregion

        public Cls_Prev_Detalle_Muelle()
        {
            init();
        }


       


    }
}
