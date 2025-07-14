using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace CSLSite
{
    public partial class site : System.Web.UI.MasterPage
    {
        
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                if (Session["control"] == null)
                {
                    Response.Redirect("~/csl/login");
                }

                var user = usuario.Deserialize(Session["control"].ToString());
                this.namelogin.InnerText = user.loginname;

            }
        }
        public string Informacion
        {
            set { this.panel_info.InnerText = value.Trim(); }
        }
        protected void btexit_Click(object sender, EventArgs e)
        {
            if (Session["control"] != null)
            {
                var user = usuario.Deserialize(Session["control"].ToString());
                var token = HttpContext.Current.Request.Cookies["token"];
                csl_log.log_csl.closeTracking_cab(token != null ? token.Value : "NoHallado", user.idcorporacion.Value, user.id);
            }
            FormsAuthentication.SignOut();
            this.Session.Abandon();
            this.Session.Clear();
            Response.RedirectPermanent("~/csl/login");
        }

        public string LoginPage
        {
            get { return this.namelogin.InnerText; }
        }

        protected void btnCambiarPassword_Click(object sender, ImageClickEventArgs e)
        {
            Response.RedirectPermanent("~/csl/cambiopassword");
        }
    }
}