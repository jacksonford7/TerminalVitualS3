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
using ClsNotasCreditos;
using Microsoft.Reporting.WebForms;

namespace CSLSite
{
    public partial class frm_rpt_niveles_aprobacion : System.Web.UI.Page
    {
        //AntiXRCFG
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
                //sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "CboConcepto", "Carga_ListadoConceptos", "Hubo un error al cargar conceptos", user2 != null ? user2.loginname : "Nologin"));
                //this.Alerta(sg);
                //return;
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
                this.Carga_ListadoConceptos();
                this.CboConcepto.SelectedIndex = 0;

                Session["id_concept"] = int.Parse(this.CboConcepto.SelectedValue);

                populate();
               
            }
        }
       
       private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
        
            string vr = string.Empty;
            try
           {
               
                 Session["id_concept"] = int.Parse(this.CboConcepto.SelectedValue);
               var user = Page.getUserBySesion();

                Session["id_concept"] = int.Parse(this.CboConcepto.SelectedValue);

                string cUsuario = string.Format("Usuario: {0} {1} ", user.nombres, user.apellidos);
               ReportParameter ReportParameter_filtro_ = new ReportParameter("ReportParameter_filtro", cUsuario);

                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });


                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.ShowToolBar = true;
                ReportViewer1.ShowPrintButton = true;

                ReportViewer1.LocalReport.Refresh();

              
              
             
               xfinder.Visible = true;


             

           }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
              vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "frm_rpt_niveles_aprobacion", "populate()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
                sinresultado.Visible = true;
                this.sinresultado.InnerText = vr;
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
                    populate();
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_listado_res_nota_credito", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
    }
}