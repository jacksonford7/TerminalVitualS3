using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite.services
{
    //for json trasnport validation
    public class entidad
    {
        public string mid { get; set; }
        public List <parametro> parametros { get; set; }
        public entidad()
        {
            this.parametros = new List<parametro>();
        }
    }

    public class parametro
    {
        public string nombre { get; set; }
        public string valor { get; set; }
    }


    public class resultado
    {
        public string esvalido { get; set; } // 0 , 1
        public string mensaje { get; set; } // a pantalla
        public string accion { get; set; } //en caso sea un metodo a llamar
    }

}