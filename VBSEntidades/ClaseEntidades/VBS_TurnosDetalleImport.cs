using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
    public class VBS_TurnosDetalle_Import
    {
        public Int64 SECUENCIA { get; set; }
        public Int64 IdTurno { get; set; }
        public int idBloque { get; set; }
        public string CodigoBloque { get; set; }
        public DateTime VigenciaInicial { get; set; }
        public DateTime VigenciaFinal { get; set; }
        public int Cantidad { get; set; }
        public TimeSpan Horario { get; set; }
        public int Frecuencia { get; set; }
        public int Disponible { get; set; }
        public int Asignados { get; set; }

    }

    public class VBS_TurnosDetalle_Consolidacion
    {
        public Int64 SECUENCIA { get; set; }
        public Int64 IdTurno { get; set; }
        public string idBloque { get; set; }
        public string CodigoBloque { get; set; }
        public DateTime VigenciaInicial { get; set; }
        public DateTime VigenciaFinal { get; set; }
        public int Cantidad { get; set; }
        public TimeSpan Horario { get; set; }
        public int Frecuencia { get; set; }
        public int Disponible { get; set; }
        public int Asignados { get; set; }

    }
}
