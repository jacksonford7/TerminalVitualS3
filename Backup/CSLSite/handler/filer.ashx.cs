using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;

namespace CSLSite.handler
{
    /// <summary>
    /// Descripción breve de filer
    /// </summary>
    public class filer : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string region = HttpContext.Current.Request.QueryString["file"];
            HttpContext.Current.Response.Clear();
            string attachment = "attachment; filename=" + region + DateTime.Now.ToString("HHmmss") + ".csv";
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //aqui tomo la sesión
            if (HttpContext.Current.Session[region] == null)
            {
                HttpContext.Current.Response.End();
                return;
            }
            var x = (DataTable)HttpContext.Current.Session[region];
            if (x.Rows.Count <= 0)
            {
                HttpContext.Current.Response.End();
                return;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("sep=;\n");
             foreach (DataColumn c in x.Columns)
            {
                if(!c.ColumnName.Contains("tool"))
                 sb.AppendFormat("{0};", c.ColumnName.ToUpper());
            }
            foreach (DataRow f in x.Rows)
            {
                sb.Append("\n");
                foreach (DataColumn dc in x.Columns)
                {
                    if (!dc.ColumnName.Contains("tool"))
                    sb.AppendFormat("{0};",f[dc].ToString());
                }
            }
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static string StripHtml(string html, bool allowHarmlessTags=true)
        {
            if (html == null || html == string.Empty)
                return string.Empty;
            if (allowHarmlessTags)
                return System.Text.RegularExpressions.Regex.Replace(html, "</?(?i:script|embed|object|frameset|frame|iframe|meta|link|style|a|div|p|br|h3|span)(.|\\n)*?>", string.Empty);
               return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", string.Empty);
        } 
    }
}
