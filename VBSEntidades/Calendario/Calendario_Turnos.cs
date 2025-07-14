using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Calendario
{
  public  class Calendario_Turnos
    {
        public DateTime Dia { get; set; }
        public int Total_Turnos { get; set; }
        public int Detalle { get; set; }
        public string Tipo_Contenedor { get; set; }
        public string Tipo_Categoria { get; set; }
        public string Tipo_Cargas { get; set; }
        public int TipoCargaId { get; set; }
        public int Total_Disponibles { get; set; }
    }
}
