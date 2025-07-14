using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Plantilla
{
    public class VBS_ConsultarPlantillaDetalleLineas
    {
        public int SECUENCIA { get; set; }
        public Int64 ID_DETALLEPLANTILLA { get; set; }
        public string ID_LINEA { get; set; }
        public string DESC_LINEA { get; set; }
        public int CANTIDAD { get; set; }
        public string CATEGORIA { get; set; }

    }
}
