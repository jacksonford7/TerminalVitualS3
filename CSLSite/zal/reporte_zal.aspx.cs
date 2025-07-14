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
using Microsoft.Reporting.WebForms;

namespace CSLSite.zal
{
    public partial class reporte_zal : System.Web.UI.Page
    {
       
        public string rucempresa
        {
            get { return (string)Session["rucempresa"]; }
            set { Session["rucempresa"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["usuarioemail"]; }
            set { Session["usuarioemail"] = value; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        public void LlenaComboDepositos()
        {
            try
            {
                cmbDeposito.DataSource = man_pro_expo.consultaDepositos(); //ds.Tables[0].DefaultView;
                cmbDeposito.DataValueField = "ID";
                cmbDeposito.DataTextField = "DESCRIPCION";
                cmbDeposito.DataBind();
                cmbDeposito.Enabled = true;
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "pases_zal", "LlenaComboDepositos()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void cmbDeposito_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long IdDepot = long.Parse(cmbDeposito.SelectedValue);
                if (cmbDeposito.SelectedValue == "-1")
                {
                    this.Alerta("Seleccione un depósito.");
                    cmbDeposito.Focus();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                    return;
                }

                if (IdDepot == 1)//ZAL
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }

                if (IdDepot == 2)//OPACIFIC
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPOPA"];
                }
                //06-abr-2020
                if (IdDepot == 3)//ZAL
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }

                if (IdDepot == 4)//SAV-DER
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }

                if (IdDepot == 5)//REPCONTVER
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }

                this.limpiar();
            }
            catch
            {

            }
        }

        protected void limpiar()
        {
           
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "~/login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "~/login.aspx", true);
                }
                rucempresa = user.ruc;
                useremail = user.email;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }

            this.tfechaini.Text = Server.HtmlEncode(this.tfechaini.Text);
            this.tfechafin.Text = Server.HtmlEncode(this.tfechafin.Text);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    string desde = "01/" + DateTime.Today.Month.ToString("D2") + "/" + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;


                    if (!DateTime.TryParseExact(desde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha desde";
                        sinresultado.Visible = true;
                        return;
                    }

                    this.tfechaini.Text = fdesde.ToString("dd/MM/yyyy");
                    this.tfechafin.Text = DateTime.Today.ToString("dd/MM/yyyy");

                    Session["booking"] = this.txtsolicitud.Text;
                    Session["exportador"] = this.TxtExportador.Text;
                    Session["fecha_desde"] = fdesde.ToString("yyyy/MM/dd");
                    Session["fecha_hasta"] = DateTime.Today.ToString("yyyy/MM/dd");
                    sinresultado.Visible = false;

                    LlenaComboDepositos();


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
                    
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime fechadesde = new DateTime();
                    DateTime fechahasta = new DateTime();
                    if (!string.IsNullOrEmpty(tfechaini.Text))
                    {
                        if (!DateTime.TryParseExact(tfechaini.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                        {
                            //this.alerta.InnerHtml = 
                            this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", tfechaini.Text));
                            tfechaini.Focus();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(tfechafin.Text))
                    {
                        if (!DateTime.TryParseExact(tfechafin.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                 
                            this.Alerta(string.Format("El formato de la fecha hasta debe ser: dia/Mes/Anio {0}", tfechafin.Text));
                            tfechafin.Focus();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                            return;
                        }
                    }
                    TimeSpan tsDias = fechahasta - fechadesde;
                    int diferenciaEnDias = tsDias.Days;
                    if (diferenciaEnDias < 0)
                    {
                        this.Alerta("La Fecha de Ingreso: " + tfechaini.Text + "\\nNO deber ser mayor a la\\nFecha final: " + tfechafin.Text);
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
                 
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";

                    string cBooking = this.txtsolicitud.Text;
                    string cExportador = this.TxtExportador.Text;

                    Session["booking"] = cBooking.Trim();
                    Session["fecha_desde"] = fechadesde.ToString("yyyy/MM/dd");
                    Session["fecha_hasta"] = fechahasta.ToString("yyyy/MM/dd");
                    Session["exportador"] = cExportador.Trim() ;


                    Int64 ID_DEPORT;

                    if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                    {
                        ID_DEPORT = 0;
                    }

                    Session["depot"] = ID_DEPORT.ToString().Trim();

                    var user = Page.getUserBySesion();

                    string cUsuario = string.Format("Usuario: {0} {1} ", user.nombres, user.apellidos);
                    ReportParameter ReportParameter_filtro_ = new ReportParameter("ReportParameter_filtro", cUsuario);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });

                    ReportViewer1.SizeToReportContent = true;
                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = true;

                    ReportViewer1.LocalReport.Refresh();

                    xfinder.Visible = true;
                    Session["usuarioemail"] = useremail;
                    Session["rucempresa"] = rucempresa;
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
       
    }
}