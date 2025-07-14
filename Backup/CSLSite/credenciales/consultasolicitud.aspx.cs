using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Xml.Linq;

namespace CSLSite
{
    public partial class consultasolicitud : System.Web.UI.Page
    {
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
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../csl/login", true);
                }
                //this.agencia.Value = user.ruc;
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
                    ConsultaInformacion();
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
        private void ConsultaInformacion()
        {
            sinresultado.Visible = false;
            alerta.Visible = false;
            populateDrop(dptiposolicitud, credenciales.getTipoSolicitudes());
            if (dptiposolicitud.Items.Count > 0)
            {
                if (dptiposolicitud.Items.FindByValue("000") != null)
                {
                    dptiposolicitud.Items.FindByValue("000").Selected = true;
                }
                dptiposolicitud.SelectedValue = "%";
            }
            /*populateDrop(dptipousuario, credenciales.getTipoUsuarios());
            if (dptipousuario.Items.Count > 0)
            {
                if (dptipousuario.Items.FindByValue("000") != null)
                {
                    dptipousuario.Items.FindByValue("000").Selected = true;
                }
                dptipousuario.SelectedValue = "%";
            }*/
            populateLixtBox(cblestados, credenciales.getTipoEstadosList());
            if (cblestados.Items.Count > 0)
            {
                //if (cblestados.Items.FindByValue("000") != null)
                //{
                //    cblestados.Items.FindByValue("000").Selected = true;
                //}
                for (int i = 0; i < cblestados.Items.Count; i++)
                {
                    if (cblestados.Items[i].Text == "PENDIENTE")
                    {
                        cblestados.Items[i].Selected = true;
                    }
                }
            }
            txtsolicitud.Text = "";
            txtruccipas.Text = "";
            tfechaini.Text = "";
            tfechafin.Text = "";
        }
        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }
        private void populateLixtBox(CheckBoxList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            List<string> listEstados = new List<string>();
            for (int i = 0; i < cblestados.Items.Count; i++)
            {
                if (cblestados.Items[i].Selected)
                {
                    listEstados.Add(cblestados.Items[i].Value);
                }
            }

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
                            string.IsNullOrEmpty(txtruccipas.Text) &&
                            string.IsNullOrEmpty(tfechaini.Text) &&
                            string.IsNullOrEmpty(tfechafin.Text) &&
                            dptiposolicitud.SelectedItem.Value.ToString() == "%" && 
                            listEstados.Count == 0
                            )
                        {
                            //this.alerta.InnerHtml = 
                            this.Alerta("Seleccione al menos un criterio de consulta.");
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                            sinresultado.Visible = false;
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                            tfechaini.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(tfechafin.Text))
                    {
                        if (!DateTime.TryParseExact(tfechafin.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                            //this.alerta.InnerHtml = 
                            this.Alerta(string.Format("<strong>&nbsp;El formato de la fecha hasta debe ser: dia/Mes/Anio {0}</strong>", tfechafin.Text));
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                            tfechafin.Focus();
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

                    var estadosXml = listEstados.Select(i => new XElement("estado",
                                                    new XAttribute("id", i)));
                    var bodyXml = new XElement("Estados", estadosXml);

                    var tablix = credenciales.GetSolicitudes(
                                 chkTodos.Checked,
                                 txtruccipas.Text,
                                 txtsolicitud.Text,
                                 dptiposolicitud.SelectedItem.Value.ToString() == "%" ? string.Empty : dptiposolicitud.SelectedItem.Value.ToString(),
                                 listEstados.Count == 0 ? string.Empty : bodyXml.ToString(),
                                 tfechaini.Text,
                                 tfechafin.Text);
                    tablePagination.DataSource = tablix;
                    tablePagination.DataBind();
                    xfinder.Visible = true;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    /*foreach (RepeaterItem item in tablePagination.Items)
                    {
                        Label lruccipas = item.FindControl("lruccipas") as Label;
                        Label lempresa = item.FindControl("lempresa") as Label;
                        var t = CslHelper.getShiperName(lruccipas.Text);
                        lempresa.Text = t;
                    }*/
                    //var sid = QuerySegura.EncryptQueryString(string.Format("{0};{1};{2};{3}", tbooking.Text, this.agencia.Value, this.tfechaini.Text, this.tfechafin.Text));
                    //this.aprint.HRef = string.Format("rptreservas.aspx?sid={0}", sid);
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
                ConsultaInformacion();
            }
        }
    }
}