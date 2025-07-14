using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ClsAppCgsa;
using System.Globalization;
using BillionEntidades;

namespace CSLSite
{
    public partial class inactivar_sellos : System.Web.UI.Page
    {

        public static usuario sUser = null;
        public static UsuarioSeguridad us;
        private Cls_ImpoContenedor obj = new Cls_ImpoContenedor();
        private Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();

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
                    this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "buscar", "BtnBuscarCliente_Click", sUser.loginname, sUser.loginname));
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

                List<Cls_ImpoContenedor> Listado = Cls_ImpoContenedor.Listado_Sellos_Clientes_filtro(pdesde, phasta, criterio, out OError);
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
            if (!IsPostBack)
            {
                Page.SslOn();
            }

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);

                return;
            }


         

            this.TxtFechaDesde.Text = Server.HtmlEncode(this.TxtFechaDesde.Text);
            this.TxtFechaHasta.Text = Server.HtmlEncode(this.TxtFechaHasta.Text);
            this.TxtBusCliente.Text = Server.HtmlEncode(this.TxtBusCliente.Text);
            this.TxtRuta1.Text = Server.HtmlEncode(this.TxtRuta1.Text);


#if !DEBUG
                this.IsAllowAccess();
#endif

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

                        TextBox Txtcomentario = e.Item.FindControl("Txtcomentario") as TextBox;
                        Label LblClientId = e.Item.FindControl("LblClientId") as Label;

                        if (string.IsNullOrEmpty(Txtcomentario.Text))
                        {
                            alerta.InnerText = "Debe ingresar un comentario";
                            alerta.Visible = true;
                            return;
                        }

                        if (string.IsNullOrEmpty(this.TxtRuta1.Text))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Por favor debe cargar un archivo pdf de autorización, para desactivar el servicio";
                            this.TxtRuta1.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.ruta_completa.Value))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Por favor debe cargar un archivo pdf de autorización, para desactivar el servicio";
                            this.TxtRuta1.Focus();
                            return;
                        }


                        obj = new Cls_ImpoContenedor();
                        obj.ID = Id;
                        obj.Comment = Txtcomentario.Text;
                        obj.file_pdf = this.ruta_completa.Value;
                        obj.Create_user = ClsUsuario.loginname;
                        if (obj.Delete_Sellos(out OError))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = string.Format("Servicio de Certificado de Sellos [{0}], inactivado con éxito ", LblClientId.Text);
                            this.TxtRuta1.Text = string.Empty;
                            this.ruta_completa.Value = string.Empty;

                        }
                        else
                        {
                            alerta.Visible = true;
                            alerta.InnerText = string.Format("Error! No se pudo inactivar el Servicio de Certificado de Sellos de la empresa:{0}-{1}", Id, OError);
                        }

                        this.Carga_Paquetes_Clientes();


                    }//fin actualizar

                }
                catch (Exception ex)
                {
                    this.error.Visible = true;
                    this.error.InnerText = string.Format("Se produjo un error durante la eliminación, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "eliminar", "tablePagination_ItemCommand", sUser.loginname, sUser.loginname));

                }
            }

        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {

        }
    }
}