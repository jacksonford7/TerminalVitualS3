using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CSLSite.cuenta
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) 
            {
                this.banmsg.InnerText = string.Empty;
                this.banmsg.Visible = false;
            }
                
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

        protected void btstart_Click(object sender, EventArgs e)
        {
            

            var valor = System.Configuration.ConfigurationManager.AppSettings["DESARROLLO"];
            if (valor == null || valor.Contains("0"))
            {
                //indica si estamos en una conexión segura de sockets, habilitada solo en producción
                if (!Request.IsSecureConnection)
                {
                    this.banmsg.InnerText = "La conexión no es segura, para proteger su identidad no continuará";
                    return;
                }
            }

            //Pregunta si sigue conectado.
            if (Response.IsClientConnected)
            {
                try
                {
                    usuario usero = new usuario();
                    usero.loginname = Server.HtmlEncode(this.user.Text.Trim());
                    var rs = usero.autenticate(Server.HtmlEncode(this.pass.Text.Trim()));

                    if (rs == 0)
                    {
                        this.banmsg.InnerText = string.Format("La cuenta de usuario {0}, no existe", this.user.Text);
                        return;
                    }
                    if (rs == 1)
                    {
                        this.banmsg.InnerText = "Su usuario se encuentra inactivo. Favor envíe un correo a csluser@cgsa.com.ec para que su solicitud sea atendida.";
                        return;
                    }
                    if (rs == 2)
                    {
                        var jmsg = new jMessage();
                        string mail = string.Empty;
                        string destinatarios = turnoConsolidacion.GetMails();
                      //  mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/>Este es un mensaje de bloqueo<br/>", "","","","")); //DEFINIR FORMATO CON EL USUARIO
                        Seguridad s = new Seguridad();
                        UsuarioSeguridad usAutenticado = new UsuarioSeguridad();
                        usAutenticado = s.consultarUsuarioPorUserName(usero.loginname.Trim());

                        string errorMail = string.Empty;
                        var user_email = usAutenticado.usuarioCorreo;
                       // destinatarios = user_email;
                        var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
                        destinatarios = string.Format("{0};{1}", user_email, correoBackUp);

                        CLSDataSeguridad.addMail(out errorMail, destinatarios, "Desbloqueo de Usuario CGSA", mail, user_email, usero.loginname, "", "");

                        if (!string.IsNullOrEmpty(errorMail))
                        {
                            this.banmsg.InnerText = errorMail;
                            return;

                        }
                        this.banmsg.InnerText = "Su usuario ha sido bloqueado, por favor espere 30 minutos o revise su correo para mayor información para realizar el desbloqueo de usuario.";
                        
                        
                        
                        return;
                    }
                    if (rs == 3)
                    {
                        this.banmsg.InnerText = "Ha fallado el nombre de usuario || contraseña, por favor reintente";
                        return;
                    }

                    if (rs == 5)
                    {
                        this.banmsg.InnerText = "Se ha producido un problema inesperado, por favor reintente mas tarde.";
                        return;
                    }
                    /*AÑADIDO POR LMOLINA 09/12/2015*/
                    if (rs == 6)
                    {
                        Session["control"] = usero.ToString();
                        //-------------------->registro de una cabecera de tracking--------------------------------Incia la sesión>
                        csl_log.log_csl.saveTracking_cab(this.Session.SessionID, usero.idcorporacion.HasValue ? usero.idcorporacion.Value : 0, usero.id, this.Request.UserHostAddress, string.Format("{0}({1}.{2})", this.Request.Browser.Browser, this.Request.Browser.MajorVersion, this.Request.Browser.MinorVersion), (new Guid().ToString()));
                        Response.Redirect("~/csl/recuperarpassword", false);
                        return;
                    }
                    /*********************************/


                    Session["control"] = usero.ToString();
                    //-------------------->registro de una cabecera de tracking--------------------------------Incia la sesión>
                    csl_log.log_csl.saveTracking_cab(this.Session.SessionID, usero.idcorporacion.HasValue ? usero.idcorporacion.Value : 0, usero.id, this.Request.UserHostAddress, string.Format("{0}({1}.{2})", this.Request.Browser.Browser, this.Request.Browser.MajorVersion, this.Request.Browser.MinorVersion), (new Guid().ToString()));
                    Response.Redirect("~/csl/menu", false);
                    return;
                }
                catch (Exception ex)
                {
                    csl_log.log_csl.save_log<Exception>(ex, "login", "btstart_Click",this.pass.Text, this.user.Text);
                    this.banmsg.InnerText = "Se produjo un problema durante el inicio de sesión y no podrá continuar, por favor comuníquese con nosotros.";
                    return;
                }
            }
        }
    }
}