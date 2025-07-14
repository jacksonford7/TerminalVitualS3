using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.ClaseEntidades
{
   public class VBS_CONTAINER_VIP
    {
        public int config_id { get; set; }
        public Int64? container_id { get; set; }
        public string container { get; set; }
        public string description { get; set; }
        public DateTime? crea_date { get; set; }
        public string crea_user { get; set; }
      
    }
}
