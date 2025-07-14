using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BillionEntidades;
using System.Globalization;

namespace CSLSite
{
    public partial class pan_consulta : System.Web.UI.Page
    {

        public static usuario sUser = null;
      
        private string cMensajes;
        #region "Variables"

      
        private string OError;

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        private void OcultarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
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

                    this.Carga_Contenedores();


                }
                catch (Exception ex)
                {
                    this.error.Visible = true;
                    this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "buscar", "BtnBuscar_Click","terminal", sUser.loginname));
                    Utils.mostrarMensaje(this.Page, ex.Message.ToString());

                   

                }
            }
        }

        private void Carga_Contenedores()
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
                if (!DateTime.TryParseExact(this.TxtFechaDesde.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                {
                    OcultarLoading();
                    xfinder.Visible = false;
                    this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
                    sinresultado.Visible = true;
                    return;
                }
                if (!DateTime.TryParseExact(this.TxtFechaHasta.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                {
                    xfinder.Visible = false;
                    OcultarLoading();
                    this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
                    sinresultado.Visible = true;
                    return;
                }
                //if (desde.Year != hasta.Year)
                //{
                //    xfinder.Visible = false;
                //    OcultarLoading();
                //    this.sinresultado.InnerText = "El rango máximo de consulta es de una semana, gracias por entender.";
                //    sinresultado.Visible = true;
                //    return;
                //}
                //TimeSpan ts = desde - hasta;
                //// Difference in days.
                //if (ts.Days > 7)
                //{
                //    xfinder.Visible = false;
                //    OcultarLoading();
                //    this.sinresultado.InnerText = "El rango máximo de consulta es de una semana, gracias por entender.";
                //    sinresultado.Visible = true;
                //    return;
                //}

               
                
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                string ruc = string.Empty;
                if (sUser == null)
                {
                    ruc = "";

                }
                else
                {
                    ruc = sUser.ruc;
                }
                List<Cls_Pan_Contenedores> Listado = Cls_Pan_Contenedores.Pan_Contenedores(desde, hasta, criterio, ruc, out OError);
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

                OcultarLoading();


            }
            catch (Exception ex)
            {
                OcultarLoading();
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "Carga_Contenedores", "Terminal", sUser.loginname));
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
           
            if (!IsPostBack)
            {
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                // xfinder.Visible = true;
                alerta.Visible = false;
                sinresultado.Visible = false;

                string desde = DateTime.Today.Month.ToString("D2") + "/01/" + DateTime.Today.Year.ToString();
                string hasta = DateTime.Today.Month.ToString("D2") + "/01/" + DateTime.Today.Year.ToString();

                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                DateTime fdesde;
                DateTime fhasta;

                if (!DateTime.TryParseExact(desde, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fdesde))
                {
                    this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha desde";
                    sinresultado.Visible = true;

                   
                    return;
                }

                if (!DateTime.TryParseExact(hasta, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fhasta))
                {
                    this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha hasta";
                    sinresultado.Visible = true;
                  
                    return;
                }

                int v_dias = int.Parse(Cls_Stc_configuracion.obtenerConfiguracion("stcDayFirst", out cMensajes).FirstOrDefault().c_valor);
                DateTime FechaInicio = DateTime.Today.AddDays(-v_dias);
                this.TxtFechaDesde.Text = FechaInicio.ToString("dd/MM/yyyy");
                this.TxtFechaHasta.Text = DateTime.Today.ToString("dd/MM/yyyy");

                this.Carga_Contenedores();
            
            }
        }


      

      
     
        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (e.CommandArgument == null)
                    {
                        alerta.Visible = true;
                        alerta.InnerText = string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos");
                      
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "argumento null");
                        return;

                    }

                    if (e.CommandName == "Eliminar")
                    {
                        string pId = t.ToString();

                        Int64 Id = 0;

                        if (!Int64.TryParse(pId, out Id))
                        {
                            Id = 0;
                        }

                        //obj = new MantenimientoPaqueteCliente();
                        //obj.Id = Id;
                        //if (obj.Delete(out OError))
                        //{
                        //    alerta.Visible = true;
                        //    alerta.InnerText = string.Format("Paquete de empresa inactivado con éxito {0}", "");
                        //}
                        //else
                        //{
                        //    alerta.Visible = true;
                        //    alerta.InnerText = string.Format("Error! No se pudo inactivar el paquete de empresa:{0}-{1}", Id, OError);
                        //}

                        //this.Carga_Paquetes_Clientes();


                    }//fin actualizar

                }
                catch (Exception ex)
                {
                   
                }
            }

        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {

        }
    }
}