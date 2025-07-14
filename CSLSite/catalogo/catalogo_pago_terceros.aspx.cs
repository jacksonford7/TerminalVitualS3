using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class catalogo_pago_terceros : System.Web.UI.Page
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
            if (Response.IsClientConnected)
            {
              var u = this.getUserBySesion();
                var t = new DataTable(); //turno.GetBookings(txtname.Text.Trim(), u.ruc);
                try
                {
                    if ( t!=null &&  t.Rows.Count > 0)
                    {
                        //this.tablePagination.DataSource = t;
                        //this.tablePagination.DataBind();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "agente", "find_Click", "sistema", u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }
            }
        }

      
    }
}