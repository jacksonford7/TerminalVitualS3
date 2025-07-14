using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBSEntidades.Plantilla;

namespace VBSEntidades
{
    public class BAN_NombrePlantillas
    {

        public long IdCabecera { get; set; }
        public string NOMBRE_PLANTILLA { get; set; }

        public List<BAN_ConsultarPlantillaDetalle> listDetalle { get; set; }
    }


}
