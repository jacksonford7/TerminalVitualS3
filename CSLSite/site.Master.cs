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
                    Response.Redirect("../login.aspx");
                }

                var xu = this.Page.Tracker();
                var hs = new HashSet<string>();
                if (xu != null)
                {
                    //this.ruc_cliente.Value = xu.ruc;
                    if (xu.IsPaidLock)
                    {
                        //lanzar el aviso al cliente
                       // var script = "<script type='text/javascript'> viewModal(); </script>";
                        //this.ClientScript.RegisterStartupScript(this.GetType(), "bloque_alerta", script);
                    }
                }

                var user = usuario.Deserialize(Session["control"].ToString());
                this.namelogin.InnerText = string.Format(" Bienvenido: {0} {1}", user.nombres, user.apellidos);
                if ((string.IsNullOrEmpty(user.tipousuario) ? string.Empty : user.tipousuario).Equals("A"))
                {
                    this.imguser.Src = "~/img/user_a.png";
                    this.imguser.Alt = "Usuario Administrador";
                }
                else
                {
                    this.imguser.Src = "~/img/user_o.png";
                    this.imguser.Alt = "Usuario Operador";
                }
                ////habilitar para producción
                this.sidebar.InnerHtml = this.Page.Opciones(xu, ref hs);
              //  Session["acceso"] = hs;
            }
        }
        public string Informacion
        {
            set {
               // this.panel_info.InnerText = value.Trim();
            }
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
            Response.RedirectPermanent("../login.aspx");
        }

        public string LoginPage
        {
            get { return this.namelogin.InnerText; }
        }

        protected void btpass_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent("../cuenta/cambiarpassword.aspx");
        }

        //protected void btnCambiarPassword_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.RedirectPermanent("~/csl/cambiopassword");
        //}
    }
}