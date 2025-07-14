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
using ClsAutorizaciones;

namespace CSLSite.autorizaciones
{
    public partial class listado_veh_chof_excluidos : System.Web.UI.Page
    {
        public static string v_mensaje = string.Empty;
      
       
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

     


        #region "Metodos"

        private void OcultarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
        }


        private void Carga_Empresas(string LINEA_NAVIERA)
        {
            try
            {
                DataSet dsRetorno = new DataSet();

                List<MantenimientoEmpresa> Listado = MantenimientoEmpresa.ComboBox_Empresas(LINEA_NAVIERA, out v_mensaje);

                this.CboEmpresas.DataSource = Listado;
                this.CboEmpresas.DataTextField = "RAZON_SOCIAL";
                this.CboEmpresas.DataValueField = "ID_EMPRESA";
                this.CboEmpresas.DataBind();
    

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.InnerText = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_Empresas", "Hubo un error al cargar empresas", t.loginname));
                sinresultado.Visible = true;

            }

        }

      
      

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        #region "Eventos Formulario"

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
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "autorizaciones", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
                //rucempresa = user.ruc;
                useremail = user.email;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }

            this.TxtLineaNaviera.Text = Server.HtmlEncode(this.TxtLineaNaviera.Text);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    var user = Page.Tracker();
                    if (user != null && !string.IsNullOrEmpty(user.ruc))
                    {

                        var ruc_cgsa = System.Configuration.ConfigurationManager.AppSettings["ruc_cgsa"];
                        if (user.ruc.Trim().Equals(ruc_cgsa.Trim()))
                        {
                            this.TxtLineaNaviera.Text = string.Empty;
                            this.buscar_linea.Visible = true;
                            this.AuxLinea_Naviera.Value = string.Empty;
                            rucempresa = string.Empty;
                        }
                        else
                        {
                            this.TxtLineaNaviera.Text = user.ruc;
                            this.buscar_linea.Visible = false;
                            this.AuxLinea_Naviera.Value = user.ruc;
                            rucempresa = user.ruc;
                        }

                       
                        this.Carga_Empresas(this.TxtLineaNaviera.Text);
                        this.CboTipo.Items.Clear();
                        this.CboTipo.Items.Add("1-Vehículos");
                        this.CboTipo.Items.Add("2-Choferes");
                        this.banmsg_det.Visible = false;
                    }
                    else
                    {
                        this.TxtLineaNaviera.Text = string.Empty;
                        this.buscar_linea.Visible = false;
                        this.AuxLinea_Naviera.Value = string.Empty;
                        rucempresa = string.Empty;
                    }


                    sinresultado.Visible = false;

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Page_Load", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
            else
            {
                this.TxtLineaNaviera.Text = this.AuxLinea_Naviera.Value;
                if (rucempresa != this.AuxLinea_Naviera.Value)
                {
                    this.Carga_Empresas(this.TxtLineaNaviera.Text);
                    this.vehiculo.Visible = false;
                    this.choferes.Visible = false;
                }
                rucempresa = this.AuxLinea_Naviera.Value;
            }

        }

        #endregion

        #region "Eventos Controles Vehiculo"

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        OcultarLoading();
                    }
                    alerta.Visible = true;

                    

                    sinresultado.Visible = false;

                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";

                    if (string.IsNullOrEmpty(this.TxtLineaNaviera.Text))
                    {
                        OcultarLoading();
                        this.Alerta("Debe seleccionar la líneas naviera");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtLineaNaviera.Focus();
                        return;
                    }


                    var user = Page.getUserBySesion();

                    string LINEA_NAVIERA = this.TxtLineaNaviera.Text.Trim();
                    string ID_EMPRESA = this.CboEmpresas.SelectedValue;
                    Int32 TIPO = this.CboTipo.SelectedIndex + 1;

                    Session["LINEA_NAVIERA"] = LINEA_NAVIERA.Trim();
                    Session["ID_EMPRESA"] = ID_EMPRESA.Trim();
                    Session["TIPO"] = TIPO.ToString();

                    string cUsuario = string.Format("Usuario: {0} {1} ", user.nombres, user.apellidos);
                    ReportParameter ReportParameter_filtro_ = new ReportParameter("ReportParameter_filtro", cUsuario);

                    if (this.CboTipo.SelectedIndex == 0)
                    {
                        this.vehiculo.Visible = true;
                        this.choferes.Visible = false;
                        ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });
                        ReportViewer1.SizeToReportContent = true;
                        ReportViewer1.ShowToolBar = true;
                        ReportViewer1.ShowPrintButton = true;
                        ReportViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        this.vehiculo.Visible = false;
                        this.choferes.Visible = true;
                        ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });
                        ReportViewer2.SizeToReportContent = true;
                        ReportViewer2.ShowToolBar = true;
                        ReportViewer2.ShowPrintButton = true;
                        ReportViewer2.LocalReport.Refresh();
                    }


                    this.banmsg_det.Visible = true;
                    Session["usuarioemail"] = useremail;
                    Session["rucempresa"] = rucempresa;
                    OcultarLoading();
                   
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "BtnBuscar_Click", "Hubo un error al buscar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                    OcultarLoading();
                }
            }
        }
   
        
        #endregion

      


      


        }
}