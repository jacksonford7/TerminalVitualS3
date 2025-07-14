using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class Cls_Prev_Detalle_Bodega : Cls_Bil_Base
    {

  

        #region "Propiedades"

        public Int64 ID { get; set ; }
        public int Fila { get; set; }
        public string idNave { get; set; }
        public string codLine { get; set; }
        public string nave { get; set; }
        public string ruc { get; set; }
        public string Exportador { get; set; }
        public string booking { get; set; }
        public int idModalidad { get; set; }
        public string desc_modalidad { get; set; }
        public int id_bodega { get; set; }
        public string desc_bodega { get; set; }
        public string tipo_bodega { get; set; }
        public int id_tipo_bodega { get; set; }
        public int QTY_out { get; set; }
        public string referencia { get; set; }
        public string linea { get; set; }
        public int cajas { get; set; }

       
       

        #endregion

        public Cls_Prev_Detalle_Bodega()
        {
            init();
        }


       


    }
}
