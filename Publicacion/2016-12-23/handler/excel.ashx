<%@ WebHandler Language="C#" Class="excel" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ClosedXML;
using System.Data;

  public class excel : IHttpHandler, IRequiresSessionState
    {
        
            public void ProcessRequest(HttpContext context)
            {
                string region = HttpContext.Current.Request.QueryString["file"];
                HttpContext.Current.Response.Clear();
                string attachment = "attachment; filename=" + region + DateTime.Now.ToString("HHmmss") + ".xls";
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
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
                ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook();
                wb.Worksheets.Add(x, string.Format("hoja_{0}", DateTime.Now.Year));
                try
                {
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                        wb.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    csl_log.log_csl.save_log<Exception>(ex, "excel", "ProcessRequest", "Error de conversion", "N4");
                    HttpContext.Current.Response.End();
                 }

                HttpContext.Current.Response.End();
            }
            public bool IsReusable
            {
                get
                {
                    return false;
                }
            }

        }