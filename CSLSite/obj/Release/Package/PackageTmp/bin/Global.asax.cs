using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;

namespace CSLSite
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciarse la aplicación
            //Registra todas las rutas en la aplicación.
            // RouteConfig.RegisterRoutes(RouteTable.Routes);
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Código que se ejecuta cuando se cierra la aplicación

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Código que se ejecuta al producirse un error no controlado

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando se inicia una nueva sesión
            //declaro en mensaje de texto, para los errores.
           var mensj = string.Empty;
            if (System.Web.HttpContext.Current.Cache["display"] == null)
            {
                mensj = System.Configuration.ConfigurationManager.AppSettings["cliente"] != null ? System.Configuration.ConfigurationManager.AppSettings["cliente"] : string.Empty;
                System.Web.HttpContext.Current.Cache.Add("display", mensj, null, DateTime.Now.AddDays(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
           
            if (System.Web.HttpContext.Current.Cache["dataset"] == null)
            {
                

              

            }
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Session_End(object sender, EventArgs e)
        {
            // Código que se ejecuta cuando finaliza una sesión.
            // Nota: el evento Session_End se desencadena sólo cuando el modo sessionstate
            // se establece como InProc en el archivo Web.config. Si el modo de sesión se establece como StateServer 
            // o SQLServer, el evento no se genera.

        }

    }
}
