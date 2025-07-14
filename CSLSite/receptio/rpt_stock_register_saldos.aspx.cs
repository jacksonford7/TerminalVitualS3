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
using ControlOPC.Entidades;
using Microsoft.Reporting.WebForms;
using ReceptioMtyStock;

namespace CSLSite
{
    public partial class rpt_stock_register_saldos : System.Web.UI.Page
    {
        usuario user;

        private void MessageBox(String msg, Page page)
        {
            StringBuilder s = new StringBuilder();
            msg = msg.Replace("'", " ");
            msg = msg.Replace(System.Environment.NewLine, " ");
            msg = msg.Replace("/", "");
            s.Append(" alert('").Append(msg).Append("');");
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), s.ToString(), true);

        }

        private void CargaLineaNaviera(string codigoempresa)
        {
            try
            {
                List<Line> Lista = Line.ListLine(codigoempresa);
                var xList = Lista.FirstOrDefault();

                if (xList != null)
                {
                    this.LblNombre.Text = xList.name;
                    Session["id_line"] = xList.id_line;
                }
                else
                {
                    Session["id_line"] = "0";
                    this.LblNombre.Text = null;
                }
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de navieras, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "CargaLineaNaviera()", "Hubo un error al cargar", t.loginname));
                sinresultado.Visible = true;
            }
        }

        private void CargaDepositos()
        {
            try
            {
                List<Line_Depot> Lista = Line_Depot.ListLineDepotReports(Convert.ToInt32 (Session["id_line"]));

                if (Lista != null && Lista.Count > 0)
                {
                    this.CboDeposito.DataSource = Lista;
                    CboDeposito.DataBind();
                }
                else
                {
                    CboDeposito.DataSource = null;
                    CboDeposito.DataBind();
                }
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de bodegas, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "CargaDepositos()", "Hubo un error al cargar", t.loginname));
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

            user = Page.Tracker();
            if (user != null)
            {             
                this.CargaLineaNaviera(user.codigoempresa);
                if (this.LblNombre.Text == String.Empty)
                {
                    this.AbortResponse("Su usuario no tiene una línea de naviera asignada", "../cuenta/menu.aspx", true);
                    return;
                }
            }

            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
           // this.CboDeposito.SelectedValue = Server.HtmlEncode(this.CboDeposito.SelectedValue);
            this.desded.Text = Server.HtmlEncode(this.desded.Text);
            this.desded.Text = Server.HtmlEncode(this.hastad.Text);
            
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

                this.CargaDepositos();
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
                        Session.Clear();
                        return;
                    }

                    validar();

                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    DateTime desde;
                    DateTime hasta;

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    //dd/MM/yyyy
                    if (!DateTime.TryParseExact(desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
                        sinresultado.Visible = true;
                        return;
                    }
                    if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
                        sinresultado.Visible = true;
                        return;
                    }

                    Session["id_depot"] = Convert.ToInt32(this.CboDeposito.SelectedValue);
                    Session["fecha_desde"] = desde.ToString("yyyy/MM/dd");
                    Session["fecha_hasta"] = hasta.ToString("yyyy/MM/dd");


                    string cUsuario = string.Format("Usuario: {0} {1} ", sUser.nombres, sUser.apellidos);
                    ReportParameter ReportParameter_filtro_ = new ReportParameter("ReportParameter_filtro", cUsuario);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });
                 
                    ReportViewer1.SizeToReportContent = true;
                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = true;
                   
                    ReportViewer1.LocalReport.Refresh();
         
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
       


 

       private void validar()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
         
            string vr = string.Empty;
            try
           {
               DateTime desde;
               DateTime hasta;
               if (!DateTime.TryParseExact(  desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
                   sinresultado.Visible = true;
                   return;
               }
               if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
                   sinresultado.Visible = true;
                   return;
               }

                var table = StockRegister.RptListStockRegister(Convert.ToInt32 (Session["id_line"]), Convert.ToInt32 (this.CboDeposito.SelectedValue) , desde, hasta);
                if (table == null)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = vr;
                    sinresultado.Visible = true;
                    return;
                }
                if (table.Count <= 0)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = "No se encontraron resultados, revise los parámetros";
                    sinresultado.Visible = true;
                    return;
                }

                xfinder.Visible = true;
           }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
                vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "rpt_stock_register", "validar()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
                sinresultado.Visible = true;
           }

       }

 

      
    }
}