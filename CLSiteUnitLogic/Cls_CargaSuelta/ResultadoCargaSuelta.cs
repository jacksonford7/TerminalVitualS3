using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLSiteUnitLogic.Cls_CargaSuelta
{
    public class ResultadoCargaSuelta
    {
        public Cls_Bil_Cabecera Cabecera { get; set; }
        public bool TieneErrores { get; set; }
        public bool TieneBloqueos { get; set; }
        public bool SinAutorizacion { get; set; }
        public bool SinDesconsolidar { get; set; }
        public string Mensaje { get; set; }
    }

}
