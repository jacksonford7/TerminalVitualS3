using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
    public class VBS_ReporteTurnosLineas
    {
        public Int64 SECUENCIA { get; set; }
        public Int64 IdTurno { get; set; }
        public Int64 IdCabeceraPlantilla { get; set; }
        public Int64 IdDetallePlantilla { get; set; }
        public string IdLinea { get; set; } 
        public string descLinea { get; set; }
        public string Categoria { get; set; }
        public DateTime VigenciaInicial { get; set; }
        public DateTime VigenciaFinal { get; set; }
        public int Cantidad { get; set; }
        public TimeSpan hora_real { get; set; }     
        public int Disponible { get; set; }
        public int Asignados { get; set; }
        public string Horario { get; set; }

    }
}
