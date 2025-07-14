using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    public abstract class Base
    {
        protected static SQLHandler.sql_handler sql_pointer = null;
        protected static Dictionary<string, object> parametros = null;

        private Nullable<DateTime> _create_date = null;
        private string _create_user = string.Empty;
        private Nullable<DateTime> _mod_date = null;
        private string _mod_user =  string.Empty;
        private string _mod_data = string.Empty;

        public DateTime? Create_date { get => _create_date; set => _create_date = value; }
        public string Create_user { get => _create_user; set => _create_user = value; }
        public DateTime? Mod_date { get => _mod_date; set => _mod_date = value; }
        public string Mod_user { get => _mod_user; set => _mod_user = value; }
        public string Mod_data { get => _mod_data; set => _mod_data = value; }
        public bool? active { get; set; }

        protected virtual void init()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }


        public static int RevisaCadena(string cadena, string field, out string msg, int min = 0, int max = 100)
        {
            if (string.IsNullOrEmpty(cadena)) { msg = string.Format("El valor de {0} no puede ser vacío o nulo",field); return 0; }
            if (cadena.Length < min || cadena.Length > max) { msg = string.Format("El valor de {0} debe tener entre {1} y {2} caracteres", field, min,max); return 0; }
            msg = string.Empty;
            return 1;
        }


        public static decimal? RevisaDecimal(string valor, out string mx)
        {
            if (RevisaCadena(valor, "valor", out mx) != 1)
            {
                return null;
            }

            CultureInfo enUS = new CultureInfo("en-US");
            //el stylo de numero normal
            NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;

            var clean = valor.Trim();
           var coma = clean.IndexOf(",", 1);
           var punto= clean.IndexOf(".", 1);
            if (coma > 0 && punto > 0)
            {
                if (coma > punto)
                {
                    //coma aparece primero-->eliminar
                    clean = clean.Replace(",", string.Empty);
                }
                else
                {
                    //punt aprece primero pero tambien coma
                    //borre el punto
                    clean = clean.Replace(".", string.Empty);
                    //reemplaza la coma por punto
                    clean = clean.Replace(",", ".");
                }
            }

            decimal d = 0;

            var r = decimal.TryParse(clean,style,enUS, out d);
            if (!r)
            {
                mx = string.Format("Fue imposible convertir el valor {0} a un numero",valor);
                return null;
            }
            mx = string.Empty;
            return d;
        }
    }
}
