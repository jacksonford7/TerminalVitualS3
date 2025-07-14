using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;

namespace CSLSite.cliente
{
    public partial class consultasolicitud : System.Web.UI.Page
    {
        public string rucempresa
        {
            get { return (string)Session["rucempresaclienteconsultasolicitud"]; }
            set { Session["rucempresaclienteconsultasolicitud"] = value; }
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
                rucempresa = user.ruc;
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
                alerta.Visible = false;
                //populateDrop(dptiposolicitud, credenciales.getTipoSolicitudes());
                //if (dptiposolicitud.Items.Count > 0)
                //{
                //    if (dptiposolicitud.Items.FindByValue("000") != null)
                //    {
                //        dptiposolicitud.Items.FindByValue("000").Selected = true;
                //    }
                //    dptiposolicitud.SelectedValue = "%";
                //}
                //populateDrop(dptipousuario, credenciales.getTipoUsuarios());
                //if (dptipousuario.Items.Count > 0)
                //{
                //    if (dptipousuario.Items.FindByValue("000") != null)
                //    {
                //        dptipousuario.Items.FindByValue("000").Selected = true;
                //    }
                //    dptipousuario.SelectedValue = "%";
                //}
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
                    alerta.Visible = true;
                    if (!chkTodos.Checked)
                    {
                        if (string.IsNullOrEmpty(txtsolicitud.Text) &&
                            string.IsNullOrEmpty(txtruccipas.Text) &&
                            string.IsNullOrEmpty(tfechaini.Text) &&
                            string.IsNullOrEmpty(tfechafin.Text) &&
                            dpestados.SelectedItem.Value.ToString() == "%"
                            )
                        {
                            this.alerta.InnerHtml = string.Format("<strong>&nbsp;Seleccione al menos un criterio de consulta. {0}</strong>", tfechaini.Text);
                            sinresultado.Visible = false;
                            return;
                        }
                    }
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
                    tablePagination.DataSource = null;
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
                    var tablix = credenciales.GetSolicitudesEmpresa(
                                 chkTodos.Checked,
                                 rucempresa,
                                 txtsolicitud.Text,
                                 dpestados.SelectedItem.Value.ToString() == "%" ? string.Empty : dpestados.SelectedItem.Value.ToString(),
                                 tfechaini.Text,
                                 tfechafin.Text);
                    DataTable dtSolEmp = new DataTable();
                    dtSolEmp = tablix.Copy().Clone();
                    dtSolEmp = tablix;
                    dtSolEmp.Columns.Add("ENCRYPTNUMSOLICITUD");
                    for (int i = 0; i < tablix.Rows.Count; i++)
                    {
                        dtSolEmp.Rows[i]["ENCRYPTNUMSOLICITUD"] = QuerySegura.EncryptQueryString(tablix.Rows[i][0].ToString());
                    }
                    tablePagination.DataSource = dtSolEmp;
                    tablePagination.DataBind();
                    xfinder.Visible = true;
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