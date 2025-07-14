using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Web.Services;
using csl_log;

namespace CSLSite.cliente
{
    public partial class consultaestadosolicitud : System.Web.UI.Page
    {
        private string _Id_Opcion_Servicio = string.Empty;

        public string rucempresa
        {
            get { return (string)Session["rucempresaconsultaestadosolicitud"]; }
            set { Session["rucempresaconsultaestadosolicitud"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailconsultaestadosolicitud"]; }
            set { Session["useremailconsultaestadosolicitud"] = value; }
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
                //para que puedar dar click en el titulo de la pantalla y regresar al menu principal de la zona
                //_Id_Opcion_Servicio = Request.QueryString["opcion"];
                //this.opcion_principal.InnerHtml = string.Format("<a href=\"../cuenta/subopciones.aspx?opcion={0}\">{1}</a>", _Id_Opcion_Servicio, "Permisos");

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
            /*populateDrop(dptiposolicitud, credenciales.getTipoSolicitudes());
            if (dptiposolicitud.Items.Count > 0)
            {
                if (dptiposolicitud.Items.FindByValue("000") != null)
                {
                    dptiposolicitud.Items.FindByValue("000").Selected = true;
                }
                dptiposolicitud.SelectedValue = "%";
            }*/
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
            txtsolicitud.Text = "";
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
                            dpestados.SelectedItem.Value.ToString() == "%" &&
                            chkAsoTrans.Checked == false
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
                    var tablix = credenciales.GetSolicitudesCliente(
                    chkTodos.Checked, txtsolicitud.Text,
                    rucempresa,
                    dpestados.SelectedItem.Value.ToString() == "%" ? string.Empty : dpestados.SelectedItem.Value.ToString(),
                    tfechaini.Text,
                    tfechafin.Text);

                    tablix.Columns.Add("ASO_TRANSPORTISTA");
                    tablix.Columns.Add("RUC_ASO_TRANSPORTISTA");
                    tablix.Columns.Add("ASOTRANS");
                    for (int i = 0; i < tablix.Rows.Count; i++)
                    {
                        var dtasotrans = aso_transportistas.GetAsoTrans(tablix.Rows[i]["NUMSOLICITUD"].ToString());
                        if (dtasotrans.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dtasotrans.Rows[0]["ASOCIACION"].ToString()))
                            {
                                tablix.Rows[i]["ASO_TRANSPORTISTA"] = dtasotrans.Rows[0]["ASOCIACION"].ToString();
                                tablix.Rows[i]["RUC_ASO_TRANSPORTISTA"] = dtasotrans.Rows[0]["RUC"].ToString();
                                tablix.Rows[i]["ASOTRANS"] = "1";
                            }
                        }
                    }
                    if (chkAsoTrans.Checked)
                    {
                        tablix = tablix.Select("ASOTRANS = 1").Count() == 0 ? new DataTable() : tablix.Select("ASOTRANS = 1").CopyToDataTable();
                    }

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
                ConsultaInformacion();
            }
        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var v_argumentos = e.CommandArgument.ToString().Split(',');
            lblNumLiq.Text = v_argumentos[0].ToString();
            lblMonto.Text = v_argumentos[1].ToString();
          
           UPMENSAJE.Update();
        }

        protected void tablePagination_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
    }
}