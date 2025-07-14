using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace CSLSite.services
{
    /// <summary>
    /// Summary description for Graficos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Graficos : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string GraficoData(int id, string ruc, DateTime desde , DateTime hasta)
        {
          return  DataChart.GetJSONData(id, ruc, desde, hasta);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string GraficoDataMulti(int id, string ruc, DateTime desde, DateTime hasta)
        {
            return DataChart.GetJSONDataMulti(id, ruc, desde, hasta);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string GraficoDataMultiSerie(int id, string ruc, DateTime desde, DateTime hasta)
        {
            return DataChart.GetJSONDataMultiSerie(id, ruc, desde, hasta);
        }
    }
}
