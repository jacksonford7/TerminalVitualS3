using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ClsAppCgsa;
using System.Globalization;

namespace CSLSite
{
    public partial class appPaquetesAgentesInactivos : System.Web.UI.Page
    {

        public static usuario sUser = null;
        public static UsuarioSeguridad us;
        private MantenimientoPaqueteCliente obj = new MantenimientoPaqueteCliente();

        #region "Variables"

        private static Int64? lm = -3;
        private string OError;

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

   
  
        protected void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
          
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }

                    this.Carga_Paquetes_Clientes();


                }
                catch (Exception ex)
                {
                    this.error.Visible = true;
                    this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "buscar", "BtnBuscar_Click", sUser.loginname, sUser.loginname));
                    Utils.mostrarMensaje(this.Page, ex.Message.ToString());

                   

                }
            }
        }

        private void Carga_Paquetes_Clientes()
        {
            try
            {

                string criterio = this.TxtBusCliente.Text.Trim();
                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");

                if (string.IsNullOrEmpty(criterio))
                {
                    criterio = null;
                }

                DateTime desde;
                DateTime hasta;

                DateTime? pdesde;
                DateTime? phasta;

                if (!DateTime.TryParseExact(this.TxtFechaDesde.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                {
                    pdesde = null;
                }
                else
                {
                    pdesde = desde;
                }
                if (!DateTime.TryParseExact(this.TxtFechaHasta.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                {
                    phasta = null;
                }
                else
                {
                    phasta = hasta;
                }

                if (pdesde.HasValue && phasta.HasValue)
                {
                    if (pdesde.Value.Year != phasta.Value.Year)
                    {
                        xfinder.Visible = false;
                        this.sinresultado.InnerText = "El rango máximo de consulta es de 1 año, gracias por entender.";
                        sinresultado.Visible = true;
                        return;
                    }
                }

                List<MantenimientoPaqueteCliente> Listado = MantenimientoPaqueteCliente.Listado_Paquetes_Agentes_Inactivos(pdesde, phasta, criterio,out OError);
                if (Listado != null)
                {
                    xfinder.Visible = true;
                    tablePagination.DataSource = Listado;
                    tablePagination.DataBind();

               
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                }

                ///this.upresult.Update();

            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "Carga_Paquetes_Clientes", sUser.loginname, sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());

            }

        }

        /// <summary>
        /// Código que se ejecutará al iniciar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "consultausuario", "Page_Init", "No autenticado", "No disponible");
                Response.Redirect("../cuenta/menu.aspx", false);
                return;
            }
            Page.SslOn();

            this.TxtFechaDesde.Text = Server.HtmlEncode(this.TxtFechaDesde.Text);
            this.TxtFechaHasta.Text = Server.HtmlEncode(this.TxtFechaHasta.Text);
            this.TxtBusCliente.Text = Server.HtmlEncode(this.TxtBusCliente.Text);
            
        }

        /// <summary>
        /// Código que se ejecutará al cargar la página 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["usuarioId"] = null;
            if (!IsPostBack)
            {
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                // xfinder.Visible = true;
                alerta.Visible = false;
                sinresultado.Visible = false;
                Seguridad s = new Seguridad();
                us = s.consultarUsuarioPorId(sUser.id);
               
               
                this.Carga_Paquetes_Clientes();
               
            }
        }


        /// <summary>
        /// Consulta los usuarios según los filtros de búsqueda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["usuarioId"] = null;
            Response.Redirect("../admin/datosusuario.aspx");
        }


      

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Session["usuarioId"] = null;
            Response.Redirect("../appcgsa/appPaquetesClientes.aspx");
        }

    
        
   
      

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
        
        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {

        }
    }
}