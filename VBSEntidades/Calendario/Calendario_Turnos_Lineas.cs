using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Calendario
{
  public  class Calendario_Turnos_Lineas
    {
        public string IdLinea { get; set; }
        public DateTime Dia { get; set; }
        public int Total_Turnos { get; set; }
        public int Total_Disponibles { get; set; }
        public string DescLinea { get; set; }
    }

    public class Calendario_Turnos_carga_BRBK
    {
        public string IdBodega { get; set; }
        public DateTime Dia { get; set; }
        public int Total_Turnos { get; set; }
        public int Total_Disponibles { get; set; }
        public string Bodega { get; set; }
    }
}
