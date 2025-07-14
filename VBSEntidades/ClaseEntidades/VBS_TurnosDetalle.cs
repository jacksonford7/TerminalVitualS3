using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
    public class VBS_TurnosDetalle
    {
        public Int64 SECUENCIA { get; set; }
        public int IdTurno { get; set; }
        public int IdCabeceraPlantilla { get; set; }
        public int IdDetallePlantilla { get; set; }
        public string TipoCargas { get; set; } 
        public int TipoCargaId { get; set; }
        public string Categoria { get; set; }
        public DateTime VigenciaInicial { get; set; }
        public DateTime VigenciaFinal { get; set; }
        public int Cantidad { get; set; }
        public TimeSpan Horario { get; set; }
        public string TipoContenedor { get; set; }
        public int Disponible { get; set; }
        public int Asignados { get; set; }
        public string TipoContenedorId { get; set; }
    }
}
