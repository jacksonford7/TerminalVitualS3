using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class bookinListRef : System.Web.UI.Page
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
                //sinresultado.Visible = false;
                var refe = Request.QueryString["ref"].ToString();
                var ruc = Request.QueryString["ruc"].ToString();
                var u = this.getUserBySesion();
                try
                {
                    var table = new System.Data.DataTable();
                    table = turnoConsolidacion.GetBookingsRef(txtname.Text.Trim(), ruc, refe);
                    if (table.Rows.Count <= 0)
                    {
                        xfinder.Visible = false;
                        sinresultado.Visible = true;
                    }
                    else
                    {
                        string info = ""; //turnoConsolidacion.GetInfoBkgList();
                        this.tablePagination.DataSource = table;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "msg-info";
                        this.alerta.InnerHtml = info;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "agente", "find_Click", txtname.Text, u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }
            }
        }
        protected void find_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                var refe = Request.QueryString["ref"].ToString();
                var ruc = Request.QueryString["ruc"].ToString();
                var u = this.getUserBySesion();
                try
                {
                    var table = new System.Data.DataTable();
                    table = turnoConsolidacion.GetBookingsRef(txtname.Text.Trim(), ruc, refe);
                    if (table.Rows.Count <= 0)
                    {
                        xfinder.Visible = false;
                        sinresultado.Visible = true;
                    }
                    else
                    {
                        string info = "";// turnoConsolidacion.GetInfoBkgList();
                        this.tablePagination.DataSource = table;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "msg-info";
                        this.alerta.InnerHtml = info;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "agente", "find_Click", txtname.Text, u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }
            }
        }
      
    }
}