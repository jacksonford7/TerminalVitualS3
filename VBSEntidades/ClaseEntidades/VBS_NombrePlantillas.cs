using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBSEntidades.Plantilla;

namespace VBSEntidades
{
   public class VBS_NombrePlantillas
    {
        
        public int IdCabecera { get; set; }
        public string NOMBRE_PLANTILLA { get; set; }

        public List<VBS_ConsultarPlantillaDetalle> listDetalle { get; set; }
    }

   
}
