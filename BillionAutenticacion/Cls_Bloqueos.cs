using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionAutenticacion
{
    public class Cls_Bloqueos
    {
        public int id { get; set; }
        public string tipo { get; set; }
        public string subtipo { get; set; }
        public int rol { get; set; }
        public string cliente { get; set; }
        public int id_servicio { get; set; }
        public int id_opcion { get; set; }
    }
}
