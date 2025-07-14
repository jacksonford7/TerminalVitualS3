using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CSLSite.N4Object;
using ConectorN4;
using System.Globalization;
using System.Web.Script.Services;
using System.Configuration;
using Newtonsoft.Json;
using csl_log;
namespace CSLSite
{
    public partial class horas_reefer : System.Web.UI.Page
    {
        //AntiXRCFG.
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
                this.agencia.Value = user.ruc;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    var t = man_pro_expo.GetCatalagoLineaAsumeHorasReefer();
                    this.tablePagination.DataSource = t;
                    this.tablePagination.DataBind();
                    sinresultado.Visible = false;
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "horas_reefer", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
        }
           
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                lineanav.InnerText = xlineanav.Value;
                ncantidadhoras.InnerText = xcantidadhoras.Value;
                fechavigencia.InnerText = xfechavigencia.Value;
                if (chkAsumeTodo.Checked)
                {
                    txtcanthoras.Enabled = false;
                    txtcanthoras.BackColor = System.Drawing.Color.Gray;
                    txtcanthoras.Text = "";
                    txtfecvigencia.Focus();
                }
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime dfechavigencia = new DateTime();
                if (!DateTime.TryParseExact(txtfecvigencia.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechavigencia))
                {
                    this.Alerta(string.Format("¡ El formato de fecha debe ser 'dia/Mes/Anio' {0}", txtfecvigencia.Text));
                    return;
                }
                int asumetodo = 0;
                if (chkAsumeTodo.Checked)
                {
                    asumetodo = 1;
                }
                string mensaje = null;
                if (!man_pro_expo.AddManHorasReefer(
                    xlineanav.Value,
                    txtcanthoras.Text,
                    dfechavigencia.ToString("yyyy-MM-dd"),
                    asumetodo,
                    Page.User.Identity.Name.ToUpper(),
                    out mensaje))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta('¡ " + mensaje + "');", true);
                    //this.Alerta(mensaje);
                }
                else
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Transacción registrada exitosamente.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>");
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "horas_reefer", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }
    }
}