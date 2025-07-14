using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class BTS_OTROS_Detalle_ExportadoresUnico : Cls_Bil_Base
    {

  

        #region "Propiedades"

        public Int64 ID { get; set ; }
        public int Fila { get; set; }
        public string idNave { get; set; }
        public string codLine { get; set; }
        public string linea { get; set; }
        public string ruc { get; set; }
        public string exportador { get; set; }
        public Int64 idStowageCab { get; set; }
        public int idExportador { get; set; }
        public int cantidad { get; set; }
        public string ruc_asume { get; set; }
        public string exportador_asume { get; set; }
        public int idExportador_asume { get; set; }
        public string booking { get; set; }
        public string referencia { get; set; }
        #endregion

        public BTS_OTROS_Detalle_ExportadoresUnico()
        {
            init();
        }


     




    }
}
