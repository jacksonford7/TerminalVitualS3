using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    class app_message:Base
    {
        public string keyword { get; set; }
        public string primary_message { get; set; }
        public string secondary_message { get; set; }
        public string what_i_do { get; set; }

    

   
    }
}
