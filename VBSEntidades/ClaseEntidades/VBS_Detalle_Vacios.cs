using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
   public class VBS_Detalle_Vacios
    {
        public Int64 IdDetalle { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int Asignados { get; set; }

    }
}
