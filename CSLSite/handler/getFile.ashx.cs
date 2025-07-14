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
    public class getFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            long? ert = 0;
            int id = 0;
            if (string.IsNullOrEmpty(context.Request.QueryString["id"]) || !int.TryParse(context.Request.QueryString["id"], out id))
            {
                //no pasó el parametro
                //ert = lerror.Salvar<ApplicationException>(new ApplicationException("El parametro ID no existe o es nulo"), MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, new List<string>() { "Descarga de archivo" }, "Service");

                ert = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("El archivo id={0}, no fué encontrado o hubo un error", id)),
                "getFile", "ProcessRequest", context.Request.QueryString["id"], "Portal");
                context.Response.ContentType = "text/html";
                context.Response.Write(FileError(ert.Value, 0, "null", "El parametro id no se encontró y no será posible devolver el archivo"));
                return;
            }
            string sms = string.Empty;
            var ar = app_start.oFile.getFile(id,null);

            if (ar == null)
            {
                //no se encontró el archivo
                ert= csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("El archivo id={0}, no fué encontrado o hubo un error", id)),
                "getFile", "ProcessRequest", context.Request.QueryString["id"], "Portal");
                context.Response.ContentType = "text/html";
                context.Response.Write(FileError(ert.Value, id, "Archivo nulo", string.Format("El archivo con id={0}, no existe y no podrá ser devuelto", id)));
                return;
            }
            if (string.IsNullOrEmpty(ar.rutafisica) && string.IsNullOrEmpty(ar.rutavirtual))
            {
                ert = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("El archivo id={0}, no contiene una ruta válida de archivo", id)),
                "getFile", "ProcessRequest", context.Request.QueryString["id"], "Portal");
                context.Response.ContentType = "text/html";
                context.Response.Write(FileError(ert.Value, id, ar.rutavirtual, string.Format("El archivo con ID={0}, no contiene una ruta válida de archivo"
                    , ar.id)));
                return;
            }
            string ruta_completa = string.Empty;
           string archivo_r = string.Empty;
           string carpeta_r = string.Empty;
           ruta_completa = ar.rutafisica;
           ruta_completa = ruta_completa.Trim();
           archivo_r = Path.GetFileName(ruta_completa);
           carpeta_r = Path.GetDirectoryName(ruta_completa);

          if (!Directory.Exists(carpeta_r))
           {

               ert = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("Arrchivo id={0}, carpeta {1} no existe  ", id, carpeta_r)),
              "getFile", "ProcessRequest", context.Request.QueryString["id"], "Portal");
               context.Response.ContentType = "text/html";
               context.Response.Write(FileError(ert.Value, id, carpeta_r, string.Format("La carpeta {0}, no existe o no fue posible acceder a ella", carpeta_r)));
               return;
           }
           else
           {
               if (!File.Exists(ruta_completa))
               {
                   ert = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(string.Format("Arrchivo id={0}, ruta completa {1} no existe  ", id, ruta_completa)),
             "getFile", "ProcessRequest", context.Request.QueryString["id"], "Portal");
                   context.Response.ContentType = "text/html";
                   context.Response.Write(FileError(ert.Value, id, archivo_r, string.Format("El archivo {0}, no existe o no fue posible acceder a el mismo dentro de su ruta {1}", archivo_r, carpeta_r)));
                   return;
               }
               context.Response.Clear();
               context.Response.ContentType = "application/octet-stream";
               context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", archivo_r));
               context.Response.WriteFile(ruta_completa);
           }
           

        



        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static string FileError(long? codigo, long id, string archivo, string razon)
        {
            var sb = string.Empty;
            sb = string.Concat(sb, "<p style='color:red; font-size:1em; text-aling:center;'>Ha ocurrido un problema al intentar buscar el archivo</p>");
            sb = string.Concat(sb, string.Format("<p style='font-size:1em; text-aling:center;'>Nombre de archivo: {0}</p>", archivo));
            sb = string.Concat(sb, string.Format("<p style='font-size:1em; text-aling:center;'>Posible causa: {0}</p>", razon));
            sb = string.Concat(sb, string.Format("<p style='font-size:1em; text-aling:center;'>Codigo de servicio: {0}</p>", codigo));
            sb = string.Concat(sb, string.Format("<p style='font-size:1em; text-aling:center;'>Codigo de archivo: {0}</p>", id));
            sb = string.Concat(sb, string.Format("<p style='font-size:1em; text-aling:center;'>Por favor comuniquese con sistemas, informando el no. de código de servicio,m para poder darle mas detalles.</p>", id));
            sb = string.Concat(sb, "<hr/>");
            sb = sb = string.Concat(sb, "<a href='javascript:history.back();'>[&gt;&gt; Click aquí para volver a la página anterior&lt;&lt;]</a>");
            return sb;
        }


    }

}