using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Calendario
{
    public class EventoCalendario
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int idDetalle { get; set; }
        public string color { get; set; }
        public int cantidad { get; set; }
        public string horario { get; set; }
        public string tipoContenedor { get; set; }
        public string tipoCarga{ get; set; }
        public int tipoCargaId { get; set; }
        public string tipoContenedorId { get; set; }
        public int tipoBloqueId { get; set; }
        public string codigoBloque { get; set; }
        public string IdLinea { get; set; }
        public string descLinea { get; set; }

        public EventoCalendario()
        {
            // Constructor vacío requerido para la serialización
        }
    }

    public class EventoCalendarioConsolidacion
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string idDetalle { get; set; }
        public string color { get; set; }
        public int cantidad { get; set; }
        public string horario { get; set; }
        public string tipoBloqueId { get; set; }
        public string codigoBloque { get; set; }
        public string IdBodega { get; set; }
        public string Bodega { get; set; }

        public EventoCalendarioConsolidacion()
        {
            // Constructor vacío requerido para la serialización
        }
    }
}
