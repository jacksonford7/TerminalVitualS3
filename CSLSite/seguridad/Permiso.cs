using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite
{
    public class Permiso
    {

        public int idOpcion { get; set; }
        public string nombreOpcion { get; set; }
        public string descripcion { get; set; }
        public bool permiso { get; set; }
        public int idServicio { get; set; }
        public int idGrupo { get; set; }

    }
}