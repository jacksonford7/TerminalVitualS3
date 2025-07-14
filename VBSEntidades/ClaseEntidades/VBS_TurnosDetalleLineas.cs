using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
    public class VBS_TurnosDetalleLineas
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
        public TimeSpan Horario { get; set; }     
        public int Disponible { get; set; }
        public int Asignados { get; set; }
       
    }

    public class VBS_TurnosDetalleCargaBRBK
    {
        public Int64 SECUENCIA { get; set; }
        public Int64 IdTurno { get; set; }
        public string IdBodega { get; set; }
        public string Bodega { get; set; }
        public DateTime VigenciaInicial { get; set; }
        public DateTime VigenciaFinal { get; set; }
        public int Cantidad { get; set; }
        public TimeSpan Horario { get; set; }
        public int Disponible { get; set; }
        public int Asignados { get; set; }

    }
}
