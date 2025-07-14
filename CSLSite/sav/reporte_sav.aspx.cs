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

using ClsProformas;
using CSLSite.sav;

namespace CSLSite.zal
{
    public partial class reporte_sav : System.Web.UI.Page
    {

           
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

        public void LlenaComboDepositos()
        {
            try
            {
                cmbDeposito.DataSource = man_pro_expo.consultaDepositosFiltrado("1"); //ds.Tables[0].DefaultView;
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

                Session["depot"] = IdDepot;

                
            }
            catch
            {

            }
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
                    alerta.Visible = false;
                    this.CboEstado.Items.Clear();
                    this.CboEstado.Items.Add("TODOS");
                    this.CboEstado.Items.Add("ACTIVOS");
                    this.CboEstado.Items.Add("ANULADOS");
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

                 
                    sinresultado.Visible = false;

                    LlenaComboDepositos();

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "reporte_sav", "Page_Load", "Hubo un error al buscar", t.loginname));
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

                            alerta.InnerHtml = string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", tfechaini.Text);
                            alerta.Visible = true;

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

                            alerta.InnerHtml = string.Format("El formato de la fecha hasta debe ser: dia/Mes/Anio {0}", tfechafin.Text);
                            alerta.Visible = true;

                            return;
                        }
                    }
                    TimeSpan tsDias = fechahasta - fechadesde;
                    int diferenciaEnDias = tsDias.Days;
                    if (diferenciaEnDias < 0)
                    {
                        this.Alerta("La Fecha de Ingreso: " + tfechaini.Text + "\\nNO deber ser mayor a la\\nFecha final: " + tfechafin.Text);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                        alerta.InnerHtml = "La Fecha de Ingreso: " + tfechaini.Text + "\\nNO deber ser mayor a la\\nFecha final: " + tfechafin.Text;
                        alerta.Visible = true;

                        return;
                    }
                    if (diferenciaEnDias > 31)
                    {
                        this.Alerta("Solo puede consultar las solicitudes de hasta un mes.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                        alerta.InnerHtml = "Solo puede consultar las solicitudes de hasta un mes.";
                        alerta.Visible = true;
                        //upresult.Update();
                        xfinder.Visible = false;
                        return;
                    }
                    sinresultado.Visible = false;
                 
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";

                    Session["estado"] = Convert.ToInt32(this.CboEstado.SelectedIndex);
                    Session["fecha_desde"] = fechadesde.ToString("yyyy/MM/dd");
                    Session["fecha_hasta"] = fechahasta.ToString("yyyy/MM/dd");

                    long IdDepot = long.Parse(cmbDeposito.SelectedValue);
                    Session["depot"] = IdDepot;

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
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "reporte_sav", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                }
            }
        }
       
    }
}