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
    public partial class recuperacion : System.Web.UI.Page
    {
        /// <summary>
        /// Código que se ejecuta al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                
                alerta.Visible = false;
                alerta.InnerText = "";

                error.Visible = false;
                error.InnerText = "";
            }

        }

        /// <summary>
        /// Código que llama al método para realizar la recuperación de la contraseña
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


                if (string.IsNullOrEmpty(txtUsuarioRecuperacion.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                Seguridad s = new Seguridad();
                UsuarioSeguridad usRecuperacion = new UsuarioSeguridad();
                usRecuperacion = s.consultarUsuarioPorUserName(txtUsuarioRecuperacion.Text.Trim());

                if (usRecuperacion == null)
                {
                    alerta.Visible = true;
                    alerta.InnerText = "No existe un usuario creado con el username ingresado";
                    return;
                }

                if (string.IsNullOrEmpty(usRecuperacion.usuarioCorreo.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "El usuario no posee una cuenta de correo configurada, no se puede proseguir con el proceso de recuperación de contraseña.";
                    return;
                }

                string passwordPlano = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10);
                string password = string.Empty;
                Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
                if (componente == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El componente AISVUtils.GSS ha fallado"), "usuario", "recuperacion_contraseña", txtUsuarioRecuperacion.Text.Trim(), usRecuperacion.usuario);
                    alerta.Visible = true;
                    alerta.InnerText = "Hubo un problema durante la recuperación de la contraseña";
                    return;
                }
                else
                {
                    dynamic instancia = Activator.CreateInstance(componente);
                    var dcpass = instancia.Encrypt(txtUsuarioRecuperacion.Text.Trim(), passwordPlano) as string;
                    if (string.IsNullOrEmpty(dcpass))
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado"), "usuario", "recuperacion_contraseña", txtUsuarioRecuperacion.Text.Trim(), usRecuperacion.usuario);
                        alerta.Visible = true;
                        alerta.InnerText = "Hubo un problema durante el cambio de contraseña.";
                        return;

                    }
                    password = dcpass;
                    instancia = null;
                    componente = null;
                }



                string resultado = seguridad.CambioPasswordUsuario(password, usRecuperacion.idUsuario,"S", "RC", usRecuperacion.idUsuario, usRecuperacion.usuario);
                alerta.Visible = false;
                alerta.InnerText = "";
                if (resultado == "ok")
                {

                    var jmsg = new jMessage();
                    string mail = string.Empty;
                    string destinatarios = turnoConsolidacion.GetMails();

                    mail = Utility.recupera_clave(usRecuperacion.usuario, passwordPlano, DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                       
                    string errorMail = string.Empty;
                    var user_email = usRecuperacion.usuarioCorreo;// usRecuperacion.usuarioCorreo;
                    //destinatarios = user_email;
                    var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
                    destinatarios = string.Format("{0};{1}", user_email, correoBackUp);

                    CLSDataSeguridad.addMail(out errorMail, user_email, "Recuperación de contraseña CGSA", mail,"" , usRecuperacion.usuario, "", "");

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
                        Utils.mostrarMensajeRedireccionando(this.Page, "Se le enviará un correo a "+usRecuperacion.usuarioCorreo+" con su nueva contraseña generada.", "../csl/login");
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
                this.error.InnerText = string.Format("Se produjo un error durante la recuperación de contraseña, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuario", "recuperacion_contraseña", txtUsuarioRecuperacion.Text.Trim(), txtUsuarioRecuperacion.Text.Trim()));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }

        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        
    }
}