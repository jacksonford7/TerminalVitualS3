using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
  public  class VBS_DETALLE_MONITOR
    {
        public string Tipo_Carga { get; set; }
        public string Tipo_contenedor { get; set; }
        public string CodigoBloque { get; set; }
        public int Planeado1 { get; set; }
        public int Reservado1 { get; set; }
        public int Disponible1 { get; set; }
        public TimeSpan Inicio1 { get; set; }
        public TimeSpan Fin1 { get; set; }
        public int Planeado2 { get; set; }
        public int Reservado2 { get; set; }
        public int Disponible2 { get; set; }
        public TimeSpan Inicio2 { get; set; }
        public TimeSpan Fin2 { get; set; }
        public int Planeado3 { get; set; }
        public int Reservado3 { get; set; }
        public int Disponible3 { get; set; }
        public TimeSpan Inicio3 { get; set; }
        public TimeSpan Fin3 { get; set; }
        public int Planeado4 { get; set; }
        public int Reservado4 { get; set; }
        public int Disponible4 { get; set; }
        public TimeSpan Inicio4 { get; set; }
        public TimeSpan Fin4 { get; set; }

    }
}
