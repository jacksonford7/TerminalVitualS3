﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class chofer : System.Web.UI.Page
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
                sinresultado.Visible = false;
            }
        }
        protected void find_Click(object sender, EventArgs e)
        {
            var table = new Catalogos.ChoferesDataTable();
            var ta = new CatalogosTableAdapters.ChoferesTableAdapter();

            var u = new usuario();
#if !DEBUG

           
             u = this.getUserBySesion();
#endif

           

            try
            {
                if (Response.IsClientConnected)
                {
                  
                    ta.ClearBeforeFill = true;
                    ta.Fill(table,txtfinder.Text.Trim());
                    if (table.Rows.Count > 0)
                    {
                        this.tablePagination.DataSource = table;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        return;
                    }
                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                }
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "choferes", "find_Click", txtfinder.Text, u != null ? u.loginname : "catalogoChoferes"));
                sinresultado.Visible = true;
            }
            finally
            {
                table.Dispose();
                ta.Dispose();
            }
        }
    }
}