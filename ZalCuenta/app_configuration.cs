using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalCuenta
{
    class app_configuration 
    {
        public string name { get; set; }
        public string value { get; set; }
        public app_configuration()
        {

        }
        public app_configuration(string _name, string _value)
        {
            this.name = _name;
            this.value = _value;
           
        }

 

    }
}
