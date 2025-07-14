<%@ WebHandler Language="C#" Class="exporter" %>

using System;
using System.Web;
using System.Web.SessionState;


public class exporter : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        string region = HttpContext.Current.Request.QueryString["file"];
        HttpContext.Current.Response.Clear();
        string attachment = "attachment; filename=" + region + DateTime.Now.ToString("HHmmss") + ".csv";
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", attachment);
        //aqui tomo la sesión
        if (HttpContext.Current.Session[region] == null)
        {
            HttpContext.Current.Response.End();
            return;
        }
        var x = (System.Collections.Generic.List<CSLSite.unidadAdvice>)HttpContext.Current.Session[region];
        if (x.Count <= 0)
        {
            HttpContext.Current.Response.End();
            return;
        }
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("sep=;\n");
        sb.Append("CONTENEDOR;FECHA DE TURNO; HORA DE TURNO; PROBLEMA\n");
        foreach (var f in x)
        {
            sb.AppendFormat("{0};{1};{2};{3}\n", f.id, f.fechaTurno,f.horaTurno,f.data);
        }
        HttpContext.Current.Response.Write(sb.ToString());
        HttpContext.Current.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}