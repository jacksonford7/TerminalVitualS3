using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace CSLSite.transportista
{
    public partial class consulta_solicitud : System.Web.UI.Page
    {
        public string rucempresa
        {
            get { return (string)Session["rucempresaasotrans"]; }
            set { Session["rucempresaasotrans"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailasotrans"]; }
            set { Session["useremailasotrans"] = value; }
        }
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
                rucempresa = user.ruc;
                useremail = user.email;
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
                    tfechafin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    tfechaini.Text = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }
                    alerta.Visible = true;
                    if (!chkTodos.Checked)
                    {
                        if (string.IsNullOrEmpty(txtsolicitud.Text) &&
                            string.IsNullOrEmpty(tfechaini.Text) &&
                            string.IsNullOrEmpty(tfechafin.Text) &&
                            string.IsNullOrEmpty(txtruccipas.Text) /*&&
                            dpestados.SelectedItem.Value.ToString() == "%"*/
                            )
                        {
                            //this.alerta.InnerHtml = 
                            this.Alerta("Seleccione al menos un criterio de consulta.");
                            sinresultado.Visible = false;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                            return;
                        }
                    }
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime fechadesde = new DateTime();
                    DateTime fechahasta = new DateTime();
                    if (!string.IsNullOrEmpty(tfechaini.Text))
                    {
                        if (!DateTime.TryParseExact(tfechaini.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                        {
                            //this.alerta.InnerHtml = 
                            this.Alerta(string.Format("<strong>&nbsp;El formato de la fecha desde debe ser: dia/Mes/Anio {0}</strong>", tfechaini.Text));
                            tfechaini.Focus();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(tfechafin.Text))
                    {
                        if (!DateTime.TryParseExact(tfechafin.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                            //this.alerta.InnerHtml = 
                            this.Alerta(string.Format("<strong>&nbsp;El formato de la fecha hasta debe ser: dia/Mes/Anio {0}</strong>", tfechafin.Text));
                            tfechafin.Focus();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                            return;
                        }
                    }
                    TimeSpan tsDias = fechahasta - fechadesde;
                    int diferenciaEnDias = tsDias.Days;
                    if (diferenciaEnDias < 0)
                    {
                        this.Alerta("La Fecha de Ingreso: " + tfechaini.Text + "\\nNO deber ser mayor a la\\nFecha de Caducidad: " + tfechafin.Text);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        return;
                    }
                    if (diferenciaEnDias > 31)
                    {
                        this.Alerta("Solo puede consultar las solicitudes de hasta un mes.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        return;
                    }
                    sinresultado.Visible = false;
                    tablePagination.DataSource = null;
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
                    var tablix = aso_transportistas.GetSolicitudes(
                    chkTodos.Checked, txtsolicitud.Text,
                    txtruccipas.Text.Trim(),
                    dpestados.SelectedItem.Value.ToString() == "%" ? string.Empty : dpestados.SelectedItem.Value.ToString(),
                    tfechaini.Text,
                    tfechafin.Text);
                    tablePagination.DataSource = tablix;
                    tablePagination.DataBind();
                    xfinder.Visible = true;
                    Session["useremailconsultafactura"] = useremail;
                    Session["rucempresaconsultafactura"] = rucempresa;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                }
            }
        }
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTodos.Checked)
            {
            }
        }
    }
}