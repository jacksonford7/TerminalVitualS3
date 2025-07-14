using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using CSLSite.services;
namespace CSLSite.services
{
    /// <summary>
    /// Este servicio será controlado por acceso de usuarios registrados unicamente
    /// Devuelve catalogos para listas deplegables dinamicas
    /// Devuelve catalogos para pantallas de buscar.
    /// [id=nombre, filtro (opcional)=filtro id que sera buscado]
    /// </summary>
    [WebService(Namespace = "http://www.cgsa.com.ec")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class catalogo : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet=false)]
        //Este catalogador será para las listas dinamicas consumidas por jquery
        public HashSet<dpitem> GetDPCatalog(string catalogID, string filtro = null)
        {   
            return ObtenCatalogo.getList(catalogID, filtro);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void PostSesion(string tokenID)
        {
              var tk = QuerySegura.DecryptQueryString(tokenID);
              if (tk != null)
              {
                  var xtk = tk.Trim().Split('.');
                  //token tiene la session
                  csl_log.log_csl.closeTracking_cab(xtk[2].ToString(), int.Parse(xtk[0].ToString()), int.Parse(xtk[1].ToString()));
              }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet=false)]
         public HashSet<bkObject> getBokings(string referencia, string fk)
        {
            return services.dataServiceHelper.GetBookins(referencia, fk);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string getalertas(string formulario)
        {
            return dataServiceHelper.getAvisosCGSA(formulario);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string getBulkData(string item)
        {
            int t = 0;
           if (!int.TryParse(item, out t))
            {
                return string.Empty;
            }
            return dataServiceHelper.getBulkCount(t);
        }
    }
}
