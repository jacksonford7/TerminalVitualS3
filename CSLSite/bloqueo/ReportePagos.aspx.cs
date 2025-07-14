using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace CSLSite.bloqueo
{
    public partial class ReportePagos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Master.Titulo = "REPORTE DE PAGOS";

            if (!IsPostBack)
            {
                //if (ASPxDateEdit_hasta.Value == null)
                ASPxDateEdit_hasta.Text = DateTime.Now.ToString("MM/dd/yyyy");
                ASPxDateEdit_desde.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Session["FECHA_INI"] = null ;
                Session["FECHA_FIN"] = null;
                Session["TIPO"] = null;
            }
        }



        protected void ImageButton_RPT_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    if (ASPxDateEdit_desde.Text == null)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ingrese la Fecha Desde')", true);
                        return;
                    }
                    if (ASPxDateEdit_hasta.Text == null)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ingrese la Fecha Hasta')", true);
                        return;
                    }

                    string desde = Convert.ToDateTime(ASPxDateEdit_desde.Text).ToShortDateString();
                    string hasta = Convert.ToDateTime(ASPxDateEdit_hasta.Text).ToShortDateString();

                    ReportParameter ReportParameter_desde_ = new ReportParameter("FECHA_INI", desde);
                    ReportParameter ReportParameter_hasta_ = new ReportParameter("FECHA_FIN", hasta);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_desde_, ReportParameter_hasta_ });

                    ReportViewer1.SizeToReportContent = true;

                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = true;

                    Session["FECHA_INI"] = ASPxDateEdit_desde.Text;
                    Session["FECHA_FIN"] = Convert.ToDateTime(ASPxDateEdit_hasta.Text).ToShortDateString();
                    ReportViewer1.LocalReport.Refresh();
                }
            }
            catch (Exception)
            {
            }
        }

        protected void DataSet_Daily_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 3000;
        }

        protected void btn_RPT_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    if (ASPxDateEdit_desde.Text == null)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ingrese la Fecha Desde')", true);
                        return;
                    }
                    if (ASPxDateEdit_hasta.Text == null)
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ingrese la Fecha Hasta')", true);
                        return;
                    }

                    string desde = Convert.ToDateTime(ASPxDateEdit_desde.Text).ToShortDateString();
                    string hasta = Convert.ToDateTime(ASPxDateEdit_hasta.Text).ToShortDateString();

                    ReportParameter ReportParameter_desde_ = new ReportParameter("FECHA_INI", desde);
                    ReportParameter ReportParameter_hasta_ = new ReportParameter("FECHA_FIN", hasta);
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_desde_, ReportParameter_hasta_ });

                    ReportViewer1.SizeToReportContent = true;

                    ReportViewer1.ShowToolBar = true;
                    ReportViewer1.ShowPrintButton = true;

                    Session["FECHA_INI"] = ASPxDateEdit_desde.Text;
                    Session["FECHA_FIN"] = Convert.ToDateTime(ASPxDateEdit_hasta.Text).ToShortDateString();
                    ReportViewer1.LocalReport.Refresh();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}