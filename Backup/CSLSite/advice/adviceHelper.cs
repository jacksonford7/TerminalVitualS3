using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace CSLSite
{
    public class adviceHelper
    {
        public static HashSet<Tuple<string,string,string,string,string> >getMyAdvices(string desde, string hasta, string usuario,string contenedor,string booking, string nave = null)
        {
            //la cultura del server
            CultureInfo enUS = new CultureInfo("en-US");
            var salida = new HashSet<Tuple<string, string, string, string, string>>();
            var dicto = new Dictionary<string, string>();
            DateTime fecha;
            if (!DateTime.TryParseExact(desde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                return salida; ;
            }
            dicto.Add("desde", fecha.ToString("yyyy/MM/dd"));
            if (!DateTime.TryParseExact(hasta, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                return salida; ;
            }
            dicto.Add("hasta", fecha.ToString("yyyy/MM/dd"));
            dicto.Add("usuario", usuario);
            if (!string.IsNullOrEmpty(nave))
            {
                dicto.Add("nave", nave);
            }
            if (!string.IsNullOrEmpty(contenedor))
            {
                dicto.Add("contenedor", contenedor);
            }
            if (!string.IsNullOrEmpty(booking))
            {
                dicto.Add("bookin", booking);
            }
            foreach (var item in CLSData.ValorLecturas("sp_get_preavisos", tComando.Procedure, dicto))
            {
                salida.Add(Tuple.Create(item.GetString(0), item.GetString(1), item.GetString(2), item.GetString(3), item.GetString(4)));
            }
            return salida;
        }
    }
}