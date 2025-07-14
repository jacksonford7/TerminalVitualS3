using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
    public class VBS_MAQUINA
    {
        public int IdMaquina { get; set; }
        public string Codigo { get; set; }
        public string Estado { get; set; }
        public DateTime InicioConfiguracion { get; set; }
        public DateTime FinConfiguracion { get; set; }
        public int   IdConfiguracionPrevia { get; set; }
        public int CapacidadOperativa { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
    }
}
