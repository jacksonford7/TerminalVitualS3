using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
    public  class VBS_ListTurnos
    {
        public int SECUENCIA { get; set; }
        public int IdTurno { get; set; }
        public int IdCabeceraPlantilla { get; set; }
        public int IdDetallePlantilla { get; set; }
        public string TipoCargas { get; set; }
        public string Categoria { get; set; }
        public DateTime VigenciaInicial { get; set; }
        public DateTime VigenciaFinal { get; set; }
        public int Cantidad { get; set; }
        public TimeSpan Horario { get; set; }
        public string TipoContenedor { get; set; }

    }
}
