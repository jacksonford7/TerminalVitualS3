using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;

namespace CSLSite
{
    public partial class recuperarpassword : System.Web.UI.Page
    {

        public static usuario sUser = null;
        public static UsuarioSeguridad usAutenticado;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

        /// <summary>
        /// Código que se ejecuta al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Seguridad s = new Seguridad();
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                usAutenticado = s.consultarUsuarioPorId(sUser.id);
                alerta.Visible = false;
                alerta.InnerText = "";

                error.Visible = false;
                error.InnerText = "";
            }

        }

        /// <summary>
        /// Código que llama al método para realizar el cambio de contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            try
            {
                alerta.Visible = false;
                alerta.InnerText = "";

                error.Visible = false;
                error.InnerText = "";

                Seguridad seguridad = new Seguridad();


                if (string.IsNullOrEmpty(txtNuevoPassword.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (string.IsNullOrEmpty(txtPasswordAnterior.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (string.IsNullOrEmpty(txtPasswordConfirmado.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (txtPasswordConfirmado.Text.Trim() != txtNuevoPassword.Text.Trim())
                {
                    alerta.Visible = true;
                    alerta.InnerText = "La nueva contraseña ingresada no coincide con su confirmación.";
                    return;
                }

                if (txtPasswordAnterior.Text.Trim() != usAutenticado.password.Trim())
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor verifique que la contraseña anterior ingresada sea la correcta.";
                    return;
                }


                string password = "";
                Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
                if (componente == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El componente AISVUtils.GSS ha fallado"), "usuario", "cambio_password", txtNuevoPassword.Text.Trim(), usAutenticado.usuario);
                    alerta.Visible = true;
                    alerta.InnerText = "Hubo un problema durante el cambio de contraseña.";
                    return;
                }
                else
                {
                    dynamic instancia = Activator.CreateInstance(componente);
                    var dcpass = instancia.Encrypt(sUser.loginname.Trim(), txtNuevoPassword.Text.Trim()) as string;
                    if (string.IsNullOrEmpty(dcpass))
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado"), "usuario", "cambio_password", txtNuevoPassword.Text.Trim(), usAutenticado.usuario);
                        alerta.Visible = true;
                        alerta.InnerText = "Hubo un problema durante el cambio de contraseña.";
                        return;

                    }
                    password = dcpass;
                    instancia = null;
                    componente = null;
                }



                string resultado = seguridad.CambioPasswordUsuario(password, sUser.id, "N", "CC", sUser.id, sUser.loginname);
                alerta.Visible = false;
                alerta.InnerText = "";
                if (resultado == "ok")
                {

                    var jmsg = new jMessage();
                    string mail = string.Empty;
                    string destinatarios = sUser.email;


                    mail = Utility.cambio_clave(sUser.loginname, DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                       
                    string errorMail = string.Empty;
                    var user_email = usAutenticado.usuarioCorreo;// usAutenticado.usuarioCorreo;
                   // destinatarios = user_email;
                    var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
                    destinatarios = string.Format("{0};{1}", user_email, correoBackUp);

                    CLSDataSeguridad.addMail(out errorMail, destinatarios, "Terminal Virtual - Cambio de contraseña", mail, user_email, sUser.loginname, "", "");

                    if (!string.IsNullOrEmpty(errorMail))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = errorMail;
                        return;

                    }
                    else
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
                        Utils.mostrarMensajeRedireccionando(this.Page, "Cambio de contraseña exitoso.", "../login.aspx");
                    }
                }
                else
                {
                    Utils.mostrarMensaje(this.Page, resultado);
                }
            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante el guardado de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuario", "cambio_contraseña", txtNuevoPassword.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }

        }

        /// <summary>
        /// Código que se ejecuta al iniciar la página.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

   
                if (Session["control"] == null)
                {
                    Response.Redirect("../login.aspx");
                }
                var user = usuario.Deserialize(Session["control"].ToString());
                this.namelogin.InnerText = user.loginname;

            }
        }
        
        /// <summary>
        /// Código que cierra sesión del usuario autenticado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        //public string Informacion
        //{
        //   // set { this.panel_info.InnerText = value.Trim(); }
        //}

        public string LoginPage
        {
            get { return this.namelogin.InnerText; }
        }


    }
}