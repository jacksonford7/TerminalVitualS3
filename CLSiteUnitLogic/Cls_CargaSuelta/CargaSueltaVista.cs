using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLSiteUnitLogic.Cls_CargaSuelta
{
    public class CargaSueltaVista
    {
        public string LINEA { get; set; }
        public string NAVE { get; set; }
        public string DAI { get; set; }
        public int BULTOS { get; set; }
        public string ESTADO { get; set; }
        public DateTime? FECHAINGRESO { get; set; }
        public DateTime? FECHADESCONSOLIDA { get; set; }
        public DateTime? FECHADESPACHO { get; set; }
    }
}
