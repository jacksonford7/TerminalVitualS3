using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite.services
{
    public class dpitem
    {
        public dpitem() { }
        public dpitem(string clave, string valor) 
        {
            this.clave = clave != null ? clave.Trim() : string.Empty;
            this.valor = valor != null ? valor.Trim() : string.Empty;
        }
        public string clave { get; set; }
        public string valor { get; set; }
    }

    public class bkObject
    {
        //key del item
        public string gkey { get; set; }
        //boking
        public string nbr { get; set; }
        //referencia
        public string referencia { get; set; }
        //iso de la unidad
        public string iso { get; set; }
        //descripcion básica
        public string descripcion { get; set; }
        //reserva
        public string cantidad { get; set; }
        //reserva
        public string linea { get; set; }
        //imo
        public string imo { get; set; }
    }


    public class ObtenCatalogo
    {
        public static HashSet<dpitem> getList(string idCatalogo, string filtro = null)
        {
            var xresult = new HashSet<dpitem>();
            switch (idCatalogo.ToLower().Trim())
            { 
                case "provincia":
                    //obtener el catalogo de provincias.
                    xresult = ConvertUI(dataServiceHelper.ReturnProvincias());
                    break;
                case "canton":
                    //obtener el catalogo de cantones
                    xresult = ConvertUI(dataServiceHelper.ReturnCantones(filtro));
                    break;
                case "institucion":
                    //obtener el catalogo de instituciones.
                    xresult = ConvertUI(dataServiceHelper.ReturnInstitucion());
                    break;
                case "regla":
                    //obtener reglas bajo filtro
                    xresult = ConvertUI(dataServiceHelper.ReturnReglas(filtro));
                    break;
                default:
                    xresult.Add(new dpitem("0","No hay catálogo"));
                    break;
            }
            return xresult;
        }

        //parsea la data a pantalla
        public static HashSet<dpitem> ConvertUI(IEnumerable<Tuple<string, string>> datalist)
        {
            var res = new HashSet<dpitem>();
            foreach (var i in datalist)
            {
                res.Add( new  dpitem(i.Item1, i.Item2));
            }
            return res;
        }
    }

}