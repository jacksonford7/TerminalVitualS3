using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace CSLSite.handler
{
    /// <summary>
    /// Descripción breve de getFile
    /// </summary>
    public class aisv : IHttpHandler
    {
        //VALIDACION CHOFER + EMPRESA = T=01, DIRVER=, COMPANY=
        public void ProcessRequest(HttpContext context)
        {
           

            context.Response.Write("OK");

            return;
            //if (string.IsNullOrEmpty(context.Request.QueryString["id"]) || !int.TryParse(context.Request.QueryString["id"], out id))
            //{
            //    //no pasó el parametro
            //    //ert = lerror.Salvar<ApplicationException>(new ApplicationException("El parametro ID no existe o es nulo"), MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, new List<string>() { "Descarga de archivo" }, "Service");

            //    ert = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("El archivo id={0}, no fué encontrado o hubo un error", id)),
            //    "getFile", "ProcessRequest", context.Request.QueryString["id"], "Portal");
            //    context.Response.ContentType = "text/html";
               
            //    return;
            //}
           

        



        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

       

    }

}