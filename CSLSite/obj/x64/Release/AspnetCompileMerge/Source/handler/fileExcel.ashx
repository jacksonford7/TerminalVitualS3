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
        string name = HttpContext.Current.Request.QueryString["name"];
        string page = HttpContext.Current.Request.QueryString["page"];
        string obj = HttpContext.Current.Request.QueryString["obj"];

        HttpContext.Current.Response.Clear();
        string attachment = "attachment;filename=" +name+".xlsx";
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AddHeader("content-disposition", attachment);
        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        if (HttpContext.Current.Session[obj] == null)
        {
            HttpContext.Current.Response.End();
            return;
        }
        var x = HttpContext.Current.Session[obj] as DataTable;
        if (x==null || x.Rows.Count <= 0)
        {
            HttpContext.Current.Response.End();
            return;
        }
        ClosedXML.Excel.XLWorkbook wb = new ClosedXML.Excel.XLWorkbook();
        wb.Worksheets.Add(x, page);
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
            csl_log.log_csl.save_log<Exception>(ex, "FileExcel", "ProcessRequest", "Error de conversion de archivo", "EXPORTER2020");
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