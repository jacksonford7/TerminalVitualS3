using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Calendario
{
    public class BAN_EventoCalendario
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int idDetalle { get; set; }
        public string color { get; set; }
        public int cantidad { get; set; }
        public string horario { get; set; }
        public string horarioFinal { get; set; }
        public string horarioInicial { get; set; }
        public string horarioInicialId { get; set; }
        public string horarioFinalId { get; set; }
        public int tipoBloqueId { get; set; }
        public string codigoBloque { get; set; }
        public int idLinea { get; set; }
        public string LineaNaviera { get; set; }

        public BAN_EventoCalendario()
        {
            // Constructor vacío requerido para la serialización
        }
    }
}
