using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft;
using System.Web.Script.Services;

namespace CSLSite.services
{
    /// <summary>
    /// Descripción breve de atraque
    /// </summary>
    [WebService(Namespace = "http://www.cgsa.com.ec")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class atraque : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public List<item> get_lines(string servicio)
        {
          return atraqueHelper.get_lines(servicio);
        }
    }
}
