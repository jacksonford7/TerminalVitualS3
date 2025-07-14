using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Calendario
{
    public class ActuEventoLineas
    {

        public int secuencia { get; set; }
        public int cantidad { get; set; }
        public Int64 idTurno { get; set; }
        public int disponible { get; set; }
        public int asignados { get; set; }
    }
}
