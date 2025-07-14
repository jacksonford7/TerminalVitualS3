using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Plantilla
{
    public class BAN_ConsultarPlantillaDetalle
    {
        public long SECUENCIA { get; set; }
        public long ID_DETALLEPLANTILLA { get; set; }
        public string IDHORAFINAL { get; set; }
        public string HORAFINAL { get; set; }
        public string IDHORAINICIAL { get; set; }
        public string HORAINICIAL { get; set; }
        public int CANTIDAD { get; set; }
        public string CATEGORIA { get; set; }
        public int IDLINEA { get; set; }
        public string LINEANAVIERA { get; set; }
    }
}
