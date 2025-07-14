using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Calendario
{
    public  class Calendario_Turnos_Import
    {
        public int idBloque { get; set; }
        public DateTime Dia { get; set; }
        public int Total_Turnos { get; set; }
        public int Total_Disponibles { get; set; }
        public string Bloque { get; set; }
    }

    public class Calendario_Turnos_Consolidacion
    {
        public string idBloque { get; set; }
        public DateTime Dia { get; set; }
        public int Total_Turnos { get; set; }
        public int Total_Disponibles { get; set; }
        public string Bloque { get; set; }
    }
}
