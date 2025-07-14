using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using csl_log;
namespace CSLSite.facturacion
{
    public partial class wbareporte : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                String Reporte = "";
                ReportDataSource wdatasourc;
                //ReportParameter wrparameter = new ReportParameter();
                DataSet wdataset = new DataSet();
                ServicePointManager.ServerCertificateValidationCallback += AcceptAllCertifications;
                try
                {
                    Reporte = "rptpasepuerta.rdlc";
                    Reporte = this.Server.MapPath(@"..\facturacion\" + Reporte);
                    if (inicializaReporte(Reporte) != true)
                    {
                        return;
                    }
                    var Opcion = Request.QueryString["opcion"].ToString().Trim();
                    switch (Opcion)
                    {
                        case "emision":
                            wdataset = (DataSet)Session["dsPasePuerta"];
                            break;
                        case "reimpresion":
                            String Wcarga = Request.QueryString["carga"].ToString().Trim();
                            String Wpase = Request.QueryString["pase"].ToString().Trim();
                            String Wtipo = Request.QueryString["tipo"].ToString().Trim();
                            System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                               new System.Xml.Linq.XElement("PASEPUERTAN4", new System.Xml.Linq.XElement("VBS_P_PASE_PUERTA_N4",
                                                               new System.Xml.Linq.XAttribute("CARGA", Wcarga == null ? "" : Wcarga),
                                                               new System.Xml.Linq.XAttribute("PASE", Wpase == null ? "" : Wpase)
                                                              )));
                            DataSet dsRetorno = new DataSet();
                            WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                            wdataset = WPASEPUERTA.RImprePasePuerta(docXML.ToString(), Wtipo);
                            break;
                    }
                    if (wdataset == null || wdataset.Tables.Count <= 0)
                    {
                        Sindatos();
                    }
                    else
                    {
                        for (int i = 0; i < wdataset.Tables[0].Rows.Count; i++)
                        {
                            wdataset.Tables[0].Rows[i]["LINE"] = pasePuerta.GetInfoCerrojo(wdataset.Tables[0].Rows[i]["DOCUMENTO"].ToString()).Rows[0]["CERROJO"].ToString();
                            if (wdataset.Tables[0].Rows[i]["PLACA"].ToString().Trim() == "" || wdataset.Tables[0].Rows[i]["PLACA"].ToString().Trim() == "NULL")
                            {
                                wdataset.Tables[0].Rows[i]["PLACA"] = "N/A";
                            }
                            if (wdataset.Tables[0].Rows[i]["LICENCIA"].ToString().Trim() == "" || wdataset.Tables[0].Rows[i]["LICENCIA"].ToString().Trim() == "NULL")
                            {
                                wdataset.Tables[0].Rows[i]["LICENCIA"] = "N/A";
                            }
                            if (wdataset.Tables[0].Rows[i]["CONDUCTOR"].ToString().Trim() == "" || wdataset.Tables[0].Rows[i]["CONDUCTOR"].ToString().Trim() == "NULL")
                            {
                                wdataset.Tables[0].Rows[i]["CONDUCTOR"] = "N/A";
                            }
                        }
                        wdatasourc = new ReportDataSource("dsPasePuerta", wdataset.Tables[0]);
                        AñadeDatasorurce(wdatasourc);
                        //rwReporte.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { wrparameter });
                        rwReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
                    }
                    rwReporte.LocalReport.Refresh();
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "wbareporte", "Page_Init()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                }
            }
        }

        public Boolean inicializaReporte(String Reporte)
        {
            //ReportParameter wrparameter = new ReportParameter();
            String wuser = Page.User.Identity.Name;  //Session["gUser"].ToString();
            if (System.IO.File.Exists(Reporte) != true)
            {
                this.Alerta("El Reporte No Existe...");
                return false;
            }
            //wrparameter = new ReportParameter("rp_user", wuser);

            rwReporte.ProcessingMode = ProcessingMode.Local;
            rwReporte.LocalReport.ReportPath = Reporte;
            
            //rwReporte.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { wrparameter });

            rwReporte.Visible = true;

            return true;
        }

        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            Int64 iPP = Convert.ToInt64(e.Parameters[0].Values[0]);
            Session["IDPP"] = iPP;
            e.DataSources.Add(new ReportDataSource("dsHorariosPP", "odsdsHorariosPasePuerta"));
        }

        public void Sindatos()
        {
            this.Alerta("No existe Información para Presentar.");
        }

        public void AñadeDatasorurce(ReportDataSource wdatasourc)
        {
            rwReporte.LocalReport.DataSources.Add(wdatasourc);
        }

        public Boolean AcceptAllCertifications(Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}