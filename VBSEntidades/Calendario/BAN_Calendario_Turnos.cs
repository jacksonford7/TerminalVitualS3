
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Calendario
{
    public class BAN_Calendario_Turnos
    {
        public DateTime Dia { get; set; }
        public int Total_Turnos { get; set; }
        public long Detalle { get; set; }
        public string HoraFinal { get; set; }
        public string Tipo_Categoria { get; set; }
        public string HoraInicial { get; set; }
        public int HoraInicialId { get; set; }
        public int Total_Disponibles { get; set; }
        public string codLinea { get; set; }
    }
}
