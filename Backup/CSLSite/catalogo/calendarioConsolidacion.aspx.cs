using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class calendarioConsolidacion : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                var r = Request.QueryString["bk"];
                sinresultado.Visible = false;
                var u = this.getUserBySesion();
                var t = turnoConsolidacion.GetProxHorario(DateTime.Now.Date, u.ruc, r);
                try
                {
                    if (t != null && t.Count > 0)
                    {
                        string info = turnoConsolidacion.GetInfoCalendarList();
                        this.tablePagination.DataSource = t;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;

                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "msg-info";
                        this.alerta.InnerHtml = info;
                        return;
                    }
                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "agente", "find_Click", "Sin parametro", u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }
            }
        }
      
    }
}