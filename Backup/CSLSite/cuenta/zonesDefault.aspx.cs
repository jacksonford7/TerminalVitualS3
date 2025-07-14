using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CSLSite.cuenta
{
    public partial class zonesDefault : System.Web.UI.Page
    {
        public static usuario sUser = null;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        /// <summary>
        /// Código que se ejecuta al iniciar la página (verifica que el usuario este autenticado)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "zones", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
                return;
            }
            Page.SslOn();
        }
        
        /// <summary>
        /// Código que se ejecuta al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    //sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    sUser = this.Page.Tracker();
                    Seguridad s = new Seguridad();
                    UsuarioSeguridad us = s.consultarUsuarioPorId(sUser.id);
                    if (us != null)
                    {
                        if (us.tipoUsuario == ConfigurationManager.AppSettings["tipoAdministradorInterno"].ToString() || us.tipoUsuario == ConfigurationManager.AppSettings["tipoAdministradorEmpresa"].ToString())
                        {
                            Response.Redirect("../csl/menuadmin", false);
                        }
                        else
                        {
                            Response.Redirect("../csl/menu", false);
                        }
                    }
                    else
                    {
                        Response.Redirect("../csl/login", false);
                    }
                }
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                this.PersonalResponse(string.Format("Ops.. ha ocurrido un error inesperado, por favor repórtelo con este código EX-{0} y reintente en unos minutos.", csl_log.log_csl.save_log<Exception>(ex, "zones", "Page_Load", "Hubo un error al cargar menú", t.loginname)), "../csl/login", true);
            }
        }
    }
}