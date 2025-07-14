using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConexion
{
    class Cls_Parametros
    {
        public string nombre { get; set; }
        public string valor { get; set; }
        public Cls_Parametros()
        {

        }
        public Cls_Parametros(string _nombre, string _valor)
        {
            this.nombre = _nombre;
            this.valor = _valor;
        }

    }


}
