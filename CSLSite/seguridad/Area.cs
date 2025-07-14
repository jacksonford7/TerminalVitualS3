using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite
{
    public class Area
    {
        public int idServicio { get; set; }
        public string nombreArea { get; set; }
        public string icono { get; set; }
        public string titulo { get; set; }
        public string estado { get; set; }
        public string nombreServicio { get; set; }
        public bool areaAdministrativa { get; set; }
    }
}