using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Calendario
{
    public class ActuEvento
    {

        public int secuencia { get; set; }
        public int cantidad { get; set; }
        public int idTurno { get; set; }
        public int disponible { get; set; }
        public int asignados { get; set; }
    }
}
