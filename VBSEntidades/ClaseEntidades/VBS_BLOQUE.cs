using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
   public class VBS_BLOQUE
    {
        public int IdBloque { get; set; }
        public string Codigo { get; set; }
        public int NumeroFilas { get; set; }
        public int NumeroColumnas { get; set; }
        public string Estado { get; set; }
        public int IdConfiguracionPrevia { get; set; }
        public DateTime InicioConfiguracion { get; set; }
        public DateTime FinConfiguracion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public string TipoContenedor { get; set; }
        public bool EsVisible { get; set; }
    }
}
