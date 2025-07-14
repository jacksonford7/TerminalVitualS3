using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace CSLSite
{
    public partial class consultasolicitudcolaborador : System.Web.UI.Page
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
                sinresultado.Visible = false;
                populateDrop(dptiposolicitud, credenciales.getTipoSolicitudes());
                if (dptiposolicitud.Items.Count > 0)
                {
                    if (dptiposolicitud.Items.FindByValue("000") != null)
                    {
                        dptiposolicitud.Items.FindByValue("000").Selected = true;
                    }
                    dptiposolicitud.SelectedValue = "%";
                }
                populateDrop(dptipousuario, credenciales.getTipoUsuarios());
                if (dptipousuario.Items.Count > 0)
                {
                    if (dptipousuario.Items.FindByValue("000") != null)
                    {
                        dptipousuario.Items.FindByValue("000").Selected = true;
                    }
                    dptipousuario.SelectedValue = "%";
                }
                populateDrop(dpestados, credenciales.getTipoEstados());
                if (dpestados.Items.Count > 0)
                {
                    if (dpestados.Items.FindByValue("000") != null)
                    {
                        dpestados.Items.FindByValue("000").Selected = true;
                    }
                    dpestados.SelectedValue = "%";
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
                    //System.Threading.Thread.Sleep(1000);
                    sinresultado.Visible = false;
                    tablePagination.DataSource = null;
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
                    //if (string.IsNullOrEmpty(txtCedula.Text))
                    //{
                    //    this.Alerta("Ingrese la Cedula a consultar.");
                    //    return;
                    //}
                    if (!chkTodos.Checked)
                    {
                        if (string.IsNullOrEmpty(txtsolicitud.Text) &&
                            string.IsNullOrEmpty(tfechaini.Text) &&
                            string.IsNullOrEmpty(tfechafin.Text) &&
                            string.IsNullOrEmpty(txtCedula.Text)
                            )
                        {
                            this.alerta.InnerHtml = string.Format("<strong>&nbsp;Seleccione al menos un criterio de consulta. {0}</strong>", "");
                            sinresultado.Visible = false;
                            return;
                        }
                    }
                    //if (string.IsNullOrEmpty(txtCedula.Text))
                    //{
                    //    this.Alerta("Ingrese la Cedula a consultar.");
                    //    return;
                    //}
                    CultureInfo enUS = new CultureInfo("en-US");
                    if (!string.IsNullOrEmpty(tfechaini.Text))
                    {
                        DateTime fechadesde;
                        if (!DateTime.TryParseExact(tfechaini.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                        {
                            this.alerta.InnerHtml = string.Format("<strong>&nbsp;El formato de la fecha desde debe ser: dia/Mes/Anio {0}</strong>", tfechaini.Text);
                            tfechaini.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(tfechafin.Text))
                    {
                        DateTime fechahasta;
                        if (!DateTime.TryParseExact(tfechafin.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                            this.alerta.InnerHtml = string.Format("<strong>&nbsp;El formato de la fecha hasta debe ser: dia/Mes/Anio {0}</strong>", tfechafin.Text);
                            tfechafin.Focus();
                            return;
                        }
                    }
                    var tablix = credenciales.GetSolicitudesColaborador(txtsolicitud.Text.Trim(), txtCedula.Text.Trim(), string.Empty, tfechaini.Text, tfechafin.Text, chkTodos.Checked);
                    tablePagination.DataSource = tablix;
                    tablePagination.DataBind();
                    xfinder.Visible = true;
                    //foreach (RepeaterItem item in tablePagination.Items)
                    //{
                    //    Label lruccipas = item.FindControl("lruccipas") as Label;
                    //    Label lempresa = item.FindControl("lempresa") as Label;
                    //    var t = CslHelper.getShiperName(lruccipas.Text);
                    //    lempresa.Text = t;
                    //}
                    //var sid = QuerySegura.EncryptQueryString(string.Format("{0};{1};{2};{3}", tbooking.Text, this.agencia.Value, this.tfechaini.Text, this.tfechafin.Text));
                    //this.aprint.HRef = string.Format("rptreservas.aspx?sid={0}", sid);
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
    }
}