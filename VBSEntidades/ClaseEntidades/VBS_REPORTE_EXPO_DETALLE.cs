using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
  public  class VBS_REPORTE_EXPO_DETALLE
    {
        public TimeSpan Inicio { get; set; }
        public TimeSpan Fin { get; set; }
        public int Planeado { get; set; }
        public int Reservado { get; set; }
        public int Disponible { get; set; }
        public string Tipo_Carga { get; set; }
        public string Tipo_contenedor { get; set; }

    }
}
