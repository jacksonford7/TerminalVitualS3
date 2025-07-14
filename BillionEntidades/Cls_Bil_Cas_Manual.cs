using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionEntidades
{
   public class Cls_Bil_Cas_Manual
    {
        public string CARGA { set; get; }
        public DateTime? FECHA_AUTORIZACION { set; get; }
        public bool AUTORIZADO { set; get; }
        public string USUARIO_AUTORIZA { set; get; }
        public string CONSIGNATARIO { set; get; }
    }
}
