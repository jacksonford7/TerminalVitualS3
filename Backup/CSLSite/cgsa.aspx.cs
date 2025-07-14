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
    public partial class cgsa : System.Web.UI.Page
    {
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.banmsg.Visible = IsPostBack;
            Server.HtmlEncode(this.user.Text.Trim());
            Server.HtmlEncode(this.pass.Text.Trim());
            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
            }
        }
        protected void btstart_Click(object sender, EventArgs e)
        {

            //indica si estamos en una conexión segura de sockets, habilitada solo en producción
                        var valor = System.Configuration.ConfigurationManager.AppSettings["DESARROLLO"];
                        if (valor == null || valor.Contains("0"))
                        {
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
                        this.banmsg.InnerText = this.banmsg.InnerText = "Ha fallado el nombre de usuario o contraseña, por favor reintente";
                        return;
                    }
                    if (rs == 1)
                    {
                        this.banmsg.InnerText = "Su usuario se encuentra inactivo. Favor envíe un correo a csluser@cgsa.com.ec para que su solicitud sea atendida.";
                        return;
                    }
                    if (rs == 2)
                    {
                        Seguridad s = new Seguridad();
                        UsuarioSeguridad usAutenticado = new UsuarioSeguridad();
                        usAutenticado = s.consultarUsuarioPorUserName(usero.loginname.Trim());

                        var jmsg = new jMessage();
                        string mail = string.Empty;
                        string destinatarios = turnoConsolidacion.GetMails();
                        mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/>Este es un mensaje del Centro de Servicios en Linea de Contecon S.A, para comunicarle lo siguiente:<br/><br/>", usAutenticado.nombreUsuario, usAutenticado.apellidoUsuario)); //DEFINIR FORMATO CON EL USUARIO
                        mail = string.Concat(mail, string.Format("<ul>"));
                        mail = string.Concat(mail, string.Format("<li>Su usuario ha sido bloqueado.</li>"));
                        mail = string.Concat(mail, string.Format("<li>Este usuario será automáticamente desbloqueado en 30 minutos; si no ha realizado un nuevo intento de ingreso.</li>"));
                        mail = string.Concat(mail, string.Format("<li>Si desea ingresar en este momento, puede restablecer su contraseña accediendo al siguiente enlace: {0}</li>",ConfigurationManager.AppSettings["enlaceRecuperacion"].ToString()));
                        mail = string.Concat(mail, string.Format("</ul><br/><br/>"));
                        mail = string.Concat(mail, string.Format("Es un placer servirle, <br/><br/>"));
                        mail = string.Concat(mail, string.Format("Atentamente, <br/><br/>"));
                        mail = string.Concat(mail, string.Format("<strong>S3 - Sistema de Solicitud de Servicios</strong> <br/>"));
                        mail = string.Concat(mail, string.Format("Contecon Guayaquil S.A. CGSA <br/>"));
                        mail = string.Concat(mail, string.Format("An ICTSI Group Company <br/><br/>"));
                        string errorMail = string.Empty;
                        var user_email =  usAutenticado.usuarioCorreo;// usAutenticado.usuarioCorreo;
                        //Nuevo añadir las configuraciones una vez que loguea.
                        var cfgs = dbconfig.GetActiveConfig(null, null, null);
                        var correoBackUp = cfgs.Where(a => a.config_name.Contains("correoBackUp")).FirstOrDefault();
                        Session["parametros"] = cfgs;
                        destinatarios = string.Format("{0};{1}", user_email, correoBackUp!=null?correoBackUp.config_value:"alertasdesarrollo@cgsa.com.ec");
                        CLSDataSeguridad.addMail(out errorMail, destinatarios, "S3 - Usuario Bloqueado", mail, user_email, usero.loginname, "", "");
                        if (!string.IsNullOrEmpty(errorMail))
                        {
                            this.banmsg.InnerText = errorMail;
                            return;

                        }
                        this.banmsg.InnerText = "Su usuario ha sido bloqueado, por favor espere 30 minutos o revise su correo dentro de unos minutos para mayor información y realizar el desbloqueo de su usuario.";
                        return;
                    }

                    if (rs == 3)
                    {
                        this.banmsg.InnerText = "Ha fallado el nombre de usuario o contraseña, por favor reintente";
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
                        FormsAuthentication.SetAuthCookie(Server.HtmlEncode(user.Text),false);
                        //-------------------->registro de una cabecera de tracking--------------------------------Incia la sesión>
                        if (usero.id == 0)
                        {
                            this.banmsg.InnerText = 
                                "Se produjo un problema durante el inicio de sesión es posible que no le hayan asignado un rol, contáctese con su administrador de empresa.";
                            return;
                        }
                        else
                        {
                            csl_log.log_csl.saveTracking_cab(this.Session.SessionID, usero.idcorporacion.HasValue ? usero.idcorporacion.Value : 0, usero.id, this.Request.UserHostAddress, string.Format("{0}({1}.{2})", this.Request.Browser.Browser, this.Request.Browser.MajorVersion, this.Request.Browser.MinorVersion), (new Guid().ToString()));
                            Response.Redirect("~/csl/recuperarpassword", false);
                            return;
                        }
                    }
                    /*********************************/

                    if (rs == 7)
                    {
                        
                        Session["control"] = usero.ToString();
                        //-------------------->registro de una cabecera de tracking--------------------------------Incia la sesión>
                        csl_log.log_csl.saveTracking_cab(Session.SessionID, usero.idcorporacion.HasValue ? usero.idcorporacion.Value : 0, usero.id, this.Request.UserHostAddress, string.Format("{0}({1}.{2})", this.Request.Browser.Browser, this.Request.Browser.MajorVersion, this.Request.Browser.MinorVersion), (new Guid().ToString()));
                        FormsAuthentication.SetAuthCookie(Server.HtmlEncode(user.Text), false);
                        Response.Redirect("~/csl/menudefault", false);
                        return;                    
                    }
                    Session["control"] = usero.ToString();
                    //-------------------->registro de una cabecera de tracking--------------------------------Incia la sesión>
                    csl_log.log_csl.saveTracking_cab(Session.SessionID, usero.idcorporacion.HasValue ? usero.idcorporacion.Value : 0, usero.id, this.Request.UserHostAddress, string.Format("{0}({1}.{2})", this.Request.Browser.Browser, this.Request.Browser.MajorVersion, this.Request.Browser.MinorVersion), (new Guid().ToString()));
                    FormsAuthentication.SetAuthCookie(Server.HtmlEncode(user.Text), false);
                    Response.Redirect("~/csl/menudefault", false);
                    return;
                }
                catch (Exception ex)
                {
                    csl_log.log_csl.save_log<Exception>(ex, "login", "btstart_Click", this.pass.Text, this.user.Text);
                    this.banmsg.InnerText = "Se produjo un problema durante el inicio de sesión y no podrá continuar, por favor comuníquese con nosotros.";
                    return;
                }
            }
        }
    }
}