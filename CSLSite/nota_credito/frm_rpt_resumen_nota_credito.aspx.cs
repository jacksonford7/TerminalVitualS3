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
using ClsNotasCreditos;

namespace CSLSite.zal
{
    public partial class frm_rpt_resumen_nota_credito : System.Web.UI.Page
    {

        private credit_head objcredit_head = new credit_head();

        private void Carga_ListadoConceptos()
        {
            try
            {

                List<concepts> ListConceptos = concepts.ListConceptosGeneral();
                if (ListConceptos != null)
                {
                    this.CboConcepto.DataSource = ListConceptos;
                    this.CboConcepto.DataBind();
                }
                else
                {
                    this.CboConcepto.DataSource = null;
                    this.CboConcepto.DataBind();
                }
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_listado_res_nota_credito", "Carga_ListadoConceptos", "Hubo un error al buscar", t.loginname));
                sinresultado.Visible = true;



            }

        }

        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            this.IsAllowAccess();
            Page.Tracker();
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
            if (Response.IsClientConnected)
            {
                xfinder.Visible = IsPostBack;
                sinresultado.Visible = false;
            }

            if (!IsPostBack)
            {
                try
                {

                    this.CboEstado.Items.Clear();
                    this.CboEstado.Items.Add("TODOS");
                    this.CboEstado.Items.Add("PENDIENTE");
                    this.CboEstado.Items.Add("FINALIZADO");
                    this.CboEstado.SelectedIndex = 1;

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

                    Session["estado"] = Convert.ToInt32(this.CboEstado.SelectedIndex);

                  
                    Session["fecha_desde"] = fdesde.ToString("yyyy/MM/dd");
                    Session["fecha_hasta"] = DateTime.Today.ToString("yyyy/MM/dd");

                    this.Carga_ListadoConceptos();
                    this.CboConcepto.SelectedIndex = 0;

                    Session["id_concept"] = int.Parse(this.CboConcepto.SelectedValue);

                    sinresultado.Visible = false;

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_rpt_resumen_nota_credito", "Page_Load", "Hubo un error al buscar", t.loginname));
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



                    Session["estado"] = Convert.ToInt32(this.CboEstado.SelectedIndex);
                    Session["fecha_desde"] = fechadesde.ToString("yyyy/MM/dd");
                    Session["fecha_hasta"] = fechahasta.ToString("yyyy/MM/dd");
                    Session["id_concept"] = int.Parse(this.CboConcepto.SelectedValue);

                    var user = Page.getUserBySesion();

                    string cUsuario = string.Format("Usuario: {0} {1} ", user.nombres, user.apellidos);
                    ReportParameter ReportParameter_filtro_ = new ReportParameter("ReportParameter_filtro", cUsuario);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });

                    ReportViewer1.SizeToReportContent = true;
                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = true;

                    ReportViewer1.LocalReport.Refresh();

                    xfinder.Visible = true;
                    
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_rpt_resumen_nota_credito", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                }
            }
        }
       
    }
}