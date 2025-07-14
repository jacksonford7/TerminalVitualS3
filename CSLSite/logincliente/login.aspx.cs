using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Configuration;
using System.Globalization;
using ConectorN4;

namespace CSLSite.logincliente
{
    public partial class login : System.Web.UI.Page
    {
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
                //Page.SslOn();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.banmsg.Visible = IsPostBack;
            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
            }
        }
        protected void btstart_Click(object sender, EventArgs e)
        {
            //Pregunta si sigue conectado.
            if (Response.IsClientConnected)
            {
                try
                {
                    string idusuario = string.Empty;
                    try
                    {
                        idusuario = Request.QueryString["sid"].ToString();
                    }
                    catch (Exception)
                    {
                        Response.Write("<script language='JavaScript'>var r=alert('Ocurrió un problema al cargar la pagina, la URL ha cambiado.');if(r==true){window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';};</script>");
                    }
                    var result = credenciales.GetValidaUsuarioYClave(user.Text, QuerySegura.EncryptQueryString(pass.Text), idusuario);
                    if (result.Rows.Count == 0)
                    {
                        this.banmsg.InnerText = "Ha fallado el nombre de usuario o contraseña, por favor reintente.";
                        //user.Text = "";
                        pass.Text = "";
                        pass.Focus();
                        return;
                    }
                    if (result.Rows[0][3].ToString() == "C")
                    {
                        this.banmsg.InnerText = "Su clave y usuario ha expirado o ya realizo el reenvio de la solicitud.";
                        user.Text = "";
                        pass.Text = "";
                        return;
                    }
                    usuario usero = new usuario();
                    usero.loginname = Server.HtmlEncode(this.user.Text.Trim());
                    Session["control"] = usero.ToString();
                    //-------------------->registro de una cabecera de tracking--------------------------------Incia la sesión>
                    csl_log.log_csl.saveTracking_cab(Session.SessionID, usero.idcorporacion.HasValue ? usero.idcorporacion.Value : 0, usero.id, this.Request.UserHostAddress, string.Format("{0}({1}.{2})", this.Request.Browser.Browser, this.Request.Browser.MajorVersion, this.Request.Browser.MinorVersion), (new Guid().ToString()));
                    var numsolicitud = QuerySegura.EncryptQueryString(result.Rows[0][0].ToString());
                    var url = "../consultasolicitudrechazoempresa.aspx/?sid=" + numsolicitud + "&sid1=" + idusuario;
                    Response.Redirect(url, false);
                    return;
                }
                catch (Exception ex)
                {
                    csl_log.log_csl.save_log<Exception>(ex, "login", "btstart_Click", this.pass.Text, this.user.Text);
                    this.banmsg.InnerText = "Ups, se produjo un problema durante el inicio de sesión y no podrá continuar, por favor comuníquese con nosotros.";
                    return;
                }
            }
        }
    }
}