using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Globalization;


namespace CSLSite.stc
{
    public partial class inicioSesion : System.Web.UI.Page
    {
        //string cMensajes;
        //string OutMensaje;
        private static Int64? lm = -3;
        private string OError;

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
#if DEBUG
            //this.user.Text = "demofab";
            //this.pass.Text = "1";
#endif

        }

        protected void btstart_Click(object sender, EventArgs e)
        {

            //Pregunta si sigue conectado.
            if (Response.IsClientConnected)
            {
                var cfgs = new List<dbconfig>();
                cfgs = dbconfig.GetActiveConfig(null, null, null);
                if (cfgs == null || cfgs.Count <= 0)
                {
                    this.banmsg.InnerText = "Fue imposible obtener las configuraciones de la aplicación, para proteger su identidad el proceso no continuará";
                    return;
                }

                // si la configuracion no existe o si la configuracion es cadena vacia, o si contiene 0 es produccion
                var cf_dev = cfgs.Where(u => u.config_name.Contains("DESARROLLO")).FirstOrDefault();

#if DEBUG
                cf_dev.config_value = "1";
#endif

                if (cf_dev == null || string.IsNullOrEmpty(cf_dev.config_value) || cf_dev.config_value.Contains("0"))
                {
                    if (!Request.IsSecureConnection)
                    {
                        this.banmsg.InnerText = "La conexión no es segura, para proteger su identidad le proceso no continuará";
                        return;
                    }
                }

                string cUser = Server.HtmlEncode(this.user.Text.Trim());
                string cClave = Server.HtmlEncode(this.pass.Text.Trim());

                try
                {
                    usuario usero = new usuario();
                    usero.loginname = Server.HtmlEncode(this.user.Text.Trim());
                    var rs = usero.autenticate(Server.HtmlEncode(this.pass.Text.Trim()));

                    //----------------------------//
                    if (rs == 0)
                    {
                        this.banmsg.InnerText = "Ha fallado el nombre de usuario o contraseña, por favor reintente";
                        return;
                    }
                    if (rs == 1)
                    {
                        this.banmsg.InnerText = "Su usuario se encuentra inactivo. Favor envíe un correo a ec.sac@contecon.com.ec para que su solicitud sea atendida.";
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
                        mail = string.Concat(mail, string.Format("<li>Si desea ingresar en este momento, puede restablecer su contraseña accediendo al siguiente enlace: {0}</li>", ConfigurationManager.AppSettings["enlaceRecuperacion"].ToString()));
                        mail = string.Concat(mail, string.Format("</ul><br/><br/>"));
                        mail = string.Concat(mail, string.Format("Es un placer servirle, <br/><br/>"));
                        mail = string.Concat(mail, string.Format("Atentamente, <br/><br/>"));
                        mail = string.Concat(mail, string.Format("<strong>Terminal Virtual</strong> <br/>"));
                        mail = string.Concat(mail, string.Format("Contecon Guayaquil S.A. CGSA <br/>"));
                        mail = string.Concat(mail, string.Format("An ICTSI Group Company <br/><br/>"));
                        string errorMail = string.Empty;
                        var user_email = usAutenticado.usuarioCorreo;// usAutenticado.usuarioCorreo;
                        //Nuevo añadir las configuraciones una vez que loguea.
                        var correoBackUp = cfgs.Where(a => a.config_name.Contains("correoBackUp")).FirstOrDefault();
                        destinatarios = string.Format("{0};{1}", user_email, correoBackUp != null ? correoBackUp.config_value : "alertasdesarrollo@cgsa.com.ec");
                        CLSDataSeguridad.addMail(out errorMail, destinatarios, "Terminal Virtual - Usuario Bloqueado", mail, user_email, usero.loginname, "", "");
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
                        Session["parametros"] = cfgs;
                        FormsAuthentication.SetAuthCookie(Server.HtmlEncode(user.Text), false);
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
                            Response.Redirect("~/cuenta/recuperarpassword.aspx", false);
                            return;
                        }
                    }
                    /*********************************/
                    if (rs == 7)
                    {
                        Session["control"] = usero.ToString();
                        Session["parametros"] = cfgs;
                        //-------------------->registro de una cabecera de tracking--------------------------------Incia la sesión>
                        csl_log.log_csl.saveTracking_cab(Session.SessionID, usero.idcorporacion.HasValue ? usero.idcorporacion.Value : 0, usero.id, this.Request.UserHostAddress, string.Format("{0}({1}.{2})", this.Request.Browser.Browser, this.Request.Browser.MajorVersion, this.Request.Browser.MinorVersion), (new Guid().ToString()));
                        FormsAuthentication.SetAuthCookie(Server.HtmlEncode(user.Text), false);
                        Response.Redirect("~/stc/notificacionesConsulta.aspx", false);
                        return;
                    }

                    //-------------------->registro de una cabecera de tracking--------------------------------Incia la sesión>
                    csl_log.log_csl.saveTracking_cab(Session.SessionID, usero.idcorporacion.HasValue ? usero.idcorporacion.Value : 0, usero.id, this.Request.UserHostAddress, string.Format("{0}({1}.{2})", this.Request.Browser.Browser, this.Request.Browser.MajorVersion, this.Request.Browser.MinorVersion), (new Guid().ToString()));
                    FormsAuthentication.SetAuthCookie(Server.HtmlEncode(user.Text), false);
                    Session["parametros"] = cfgs;


                    //2020->Verificar cgfs, para detener sap
                    var sap_activo = cfgs.Where(f => f.config_name.Contains("sap_activo")).FirstOrDefault();
                    bool rev_sap = true;
                    if (sap_activo != null && !string.IsNullOrEmpty(sap_activo.config_value))
                    {
                        if (sap_activo.config_value.Contains("0"))
                        {
                            rev_sap = false;
                        }
                    }
                    bool popup = false;


                    #region "SAP"
#if !DEBUG
                    //CODIGO NUEVO 2017--> CONTROL DE PENDIENTES
                    //si sap esta activo
                    if (rev_sap)
                    { 
                       var ec = new EstadoCuenta.Ws_Sap_EstadoDeCuentaSoapClient();
                       string sap_user =string.Empty;
                       string sap_pass = string.Empty;
                       var cf = cfgs.Where(f => f.config_name.Contains("sap_user")).FirstOrDefault();
                       if (cf != null &&!string.IsNullOrEmpty( cf.config_value))
                       {
                           sap_user = cf.config_value;
                           cf = null;
                       }
                       cf= cfgs.Where(f => f.config_name.Contains("sap_pass")).FirstOrDefault();
                       if (cf != null && !string.IsNullOrEmpty(cf.config_value))
                       {
                           sap_pass = cf.config_value;
                       }
                        if (string.IsNullOrEmpty(sap_pass) || string.IsNullOrEmpty(sap_user))
                        {
                            sap_user = "admin";
                            sap_pass = "*123456";
                        }

                        var promise = ec.SI_Customer_Statement_NAVIS_CGSA(usero.ruc, sap_user, sap_pass);
                        var tieneBloqueo = false;
                        if (promise == null)
                        {
                            csl_log.log_csl.save_log<Exception>(new ApplicationException("Error de WEBSERVICE, OBJETO NULO"), "login", "SAP", usero.ruc, this.user.Text);
                        }
                        var error = promise.Descendants("ERROR").FirstOrDefault();
                        if (error != null)
                        {
                            csl_log.log_csl.save_log<Exception>(new ApplicationException(error.Value), "login", "SAP", usero.ruc, this.user.Text);
                         }
                        else
                        {
                            //obtener datos de cabecera
                            var cab = promise.Descendants("CABECERA").FirstOrDefault();
                           //me indica que tiene bloqueo de algun tipo
                           var n = cab.Element("BLOQUEO");
                           if (n != null && n.Value.Contains("1"))
                           {
                               tieneBloqueo = true;
                               popup = true;
                           }

//#if DEBUG
//                           tieneBloqueo = true;
//                           popup = true;
//#endif
                            NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
                            CultureInfo enUS = new CultureInfo("en-US");
                            //no tiene valores pendientes
                           decimal ssal = 0;
                            var saldo = cab.Element("SALDO");
                            if (saldo == null || string.IsNullOrEmpty(saldo.Value) || !decimal.TryParse(saldo.Value, style, enUS, out  ssal))
                            {
                                csl_log.log_csl.save_log<Exception>(new ApplicationException("El saldo fué nulo cuando se consulto el servicio"), "login", "SAP", usero.ruc, this.user.Text);
                            }
                            //ocultar mensaje de login
                            this.banmsg.Visible = false;
                            this.monto_fac.InnerText = ssal.ToString("c");
                           // var det = promise.Descendants("DETALLE").FirstOrDefault();

                            var fv = cab.Element("FACTURAS_VENCIDAS");
                            var fp = cab.Element("FACTURAS_PENDIENTES");

                            decimal dec = 0;

                            //elementos tendran 0
                            if (fv != null && !string.IsNullOrEmpty(fv.Value))
                            {
                                if (decimal.TryParse(fv.Value, out dec))
                                {
                                    fac_ven.InnerText = string.Format("{0:c}", dec);
                                }
                                else
                                {
                                    fac_ven.InnerText = "$0:EE";
                                }
                             }
                            dec = 0;
                            if (fp != null && !string.IsNullOrEmpty(fp.Value))
                            {
                                 if (decimal.TryParse(fp.Value, out dec))
                                {
                                     fac_pend.InnerText = string.Format("{0:c}", dec);
                                }
                                else
                                {
                                    fac_pend.InnerText = "$0:EE";
                                }
                            }
                            //if (det != null)
                            //{
                            //    var p = det.Descendants("FACTURA").Count();
                            //}
                            //var script = "<script type='text/javascript'> viewModal(); </script>";
                            //this.ClientScript.RegisterStartupScript(this.GetType(), "bloque_alerta", script);

                }


                    //se muestra el popup entonces tiene cartera vencida
                    usero.bloqueo_cartera = tieneBloqueo;

                    }
#endif
                    #endregion

                    /*if (!popup) {

                        Response.Redirect("~/cuenta/menu.aspx", false);

                    }*/
                    Response.Redirect("~/stc/notificacionesConsulta.aspx", false);
                    Session["control"] = usero.ToString();
                    return;



                }

                //error local
                catch (Exception ex)
                {
                    csl_log.log_csl.save_log<Exception>(ex, "inicioSesion", "btstart_Click", this.pass.Text, this.user.Text);
                    this.banmsg.InnerText = "Se produjo un problema durante el inicio de sesión y no podrá continuar, por favor comuníquese con nosotros.";
                    return;
                }
            }
        }



    }


}