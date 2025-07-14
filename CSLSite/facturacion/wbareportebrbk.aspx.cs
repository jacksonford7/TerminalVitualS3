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
    public partial class wbareportebrbk : System.Web.UI.Page
    {



        protected void Page_Init(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    GetReporte();
            //}
        }

        public void GetReporte()
        {
            String Opcion = "";
            String Reporte = "";
            String wxml = "";
            String wvalor = "";
            ReportDataSource wdatasourc;
            ReportParameter wrparameter = new ReportParameter();
            DataSet wdataset = new DataSet();

            ServicePointManager.ServerCertificateValidationCallback += AcceptAllCertifications;

            Opcion = Request.QueryString["opcion"].ToString().Trim();
            // wxml =  Session["XmlDatos"].ToString();
            try
            {
                switch (Opcion)
                {

                    case "PasePuerta":

                        Reporte = "RptPasePuertaBreakBulk.rdlc";
                        Reporte = this.Server.MapPath(@"..\facturacion\" + Reporte);
                        if (inicializaReporte(Reporte) != true)
                        {
                            return;
                        }

                        wdataset = (DataSet)Session["dsPasePuertaBrbk"];

                        if (wdataset == null || wdataset.Tables.Count <= 0)
                        {
                            Sindatos();
                        }
                        else
                        {
                            for (int i = 0; i < wdataset.Tables[0].Rows.Count; i++)
                            {

                                /*BRBK*/

                                var cargarep = wdataset.Tables[0].Rows[i]["MRN"].ToString() + "-" +
                                               wdataset.Tables[0].Rows[i]["MSN"].ToString() + "-" +
                                               wdataset.Tables[0].Rows[i]["HSN"].ToString();

                                if (string.IsNullOrEmpty(wdataset.Tables[0].Rows[i]["DOCUMENTO"].ToString()))
                                {
                                    var infoppweb = pasePuerta.GetInfoPPWebBRBK(wdataset.Tables[0].Rows[i]["FACTURA"].ToString(), cargarep.Trim());
                                    if (infoppweb != null)
                                    {
                                        if (infoppweb.Rows.Count > 0)
                                        {
                                            wdataset.Tables[0].Rows[i]["DOCUMENTO"] = infoppweb.Rows[0]["DOCUMENTO"].ToString();
                                            wdataset.Tables[0].AcceptChanges();
                                        }
                                    }
                                }

                                if (string.IsNullOrEmpty(wdataset.Tables[0].Rows[i]["IMPORTADOR"].ToString()))
                                {
                                    var infoppweb = pasePuerta.GetInfoPPWebBRBK(wdataset.Tables[0].Rows[i]["FACTURA"].ToString(), cargarep.Trim());
                                    if (infoppweb != null)
                                    {
                                        if (infoppweb.Rows.Count > 0)
                                        {
                                            wdataset.Tables[0].Rows[i]["IMPORTADOR"] = infoppweb.Rows[0]["IMPORTADOR"].ToString();
                                            wdataset.Tables[0].AcceptChanges();
                                        }
                                    }
                                }

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

                            rwReporte.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { wrparameter });

                            rwReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSourceBRBK);

                        }

                        break;
                    case "ReprePasePuerta":
                         String Wcarga = Request.QueryString["Carga"].ToString().Trim();
                        String Wpase = Request.QueryString["Pase"].ToString().Trim();
                        String Wtipo = Request.QueryString["Tipo"].ToString().Trim();


                        System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                           new System.Xml.Linq.XElement("PASEPUERTAN4", new System.Xml.Linq.XElement("VBS_P_PASE_PUERTA_N4",
                                                           new System.Xml.Linq.XAttribute("CARGA", Wcarga == null ? "" : Wcarga),
                                                           new System.Xml.Linq.XAttribute("PASE", Wpase == null ? "" : Wpase)
                                                          )));

                        DataSet dsRetorno = new DataSet();
                        WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
                        wdataset = WPASEPUERTA.RImprePasePuerta(docXML.ToString(), Wtipo);

                        var cargarep_ = wdataset.Tables[0].Rows[0]["MRN"].ToString() + "-" +
                                                       wdataset.Tables[0].Rows[0]["MSN"].ToString() + "-" +
                                                       wdataset.Tables[0].Rows[0]["HSN"].ToString();

                        if (string.IsNullOrEmpty(wdataset.Tables[0].Rows[0]["DOCUMENTO"].ToString()))
                        {
                            var infoppweb = pasePuerta.GetInfoPPWebBRBK(wdataset.Tables[0].Rows[0]["FACTURA"].ToString(), cargarep_.Trim());
                            if (infoppweb != null)
                            {
                                if (infoppweb.Rows.Count > 0)
                                {
                                    wdataset.Tables[0].Rows[0]["DOCUMENTO"] = infoppweb.Rows[0]["DOCUMENTO"].ToString();
                                    wdataset.Tables[0].AcceptChanges();
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(wdataset.Tables[0].Rows[0]["IMPORTADOR"].ToString()))
                        {
                            var infoppweb = pasePuerta.GetInfoPPWebBRBK(wdataset.Tables[0].Rows[0]["FACTURA"].ToString(), cargarep_.Trim());
                            if (infoppweb != null)
                            {
                                if (infoppweb.Rows.Count > 0)
                                {
                                    wdataset.Tables[0].Rows[0]["IMPORTADOR"] = infoppweb.Rows[0]["IMPORTADOR"].ToString();
                                    wdataset.Tables[0].AcceptChanges();
                                }
                            }
                        }


                        Reporte = "RptPasePuertaBreakBulk.rdlc";
                        Reporte = this.Server.MapPath(@"..\facturacion\" + Reporte);
                        if (inicializaReporte(Reporte) != true)
                        {
                            return;
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
                            rwReporte.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { wrparameter });
                            rwReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSourceBRBK);
                        }

                        break;
                    default:
                        break;
                }

                rwReporte.LocalReport.Refresh();
            }

            catch (Exception exc)
            {
                //utilform.MessageBox("Error al Cargar el Reporte", this);
                this.Alerta("Error al Cargar el Reporte");

                String JScript = "";
                JScript = "<script language='javascript'>";
                //JScript += "opener.location.reload();";
                JScript += "window.close();";
                JScript += "</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarpagina", JScript, false);
            }
        }
        public void SetSubDataSourceBRBK(object sender, SubreportProcessingEventArgs e)
        {
            Int64 iPP = Convert.ToInt64(e.Parameters[0].Values[0]);
            Session["IDPPBRBK"] = iPP;
            e.DataSources.Add(new ReportDataSource("dsHorariosBRBK", "odsdsHorariosPasePuertaBRBK"));
            /*e.DataSources.Add(new ReportDataSource()
            {
                Name = "dsContenedoresPorBooking",
                Value = sBooking
            });*/
        }
        public static SqlConnection getConex()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        }
        private DataTable getFechaVencimiento(out String str_err, String ruc, String role)
        {
            DataTable dt = new DataTable();
            str_err = String.Empty;
            using (var cx = getConex())
            {
                try
                {
                    SqlDataAdapter t = new SqlDataAdapter();
                    t.SelectCommand = new SqlCommand();
                    t.SelectCommand.CommandTimeout = 0;
                    t.SelectCommand.Connection = cx;
                    t.SelectCommand.CommandText = "SP_PROCESO_PAGO_EN_LINEA";
                    t.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    t.SelectCommand.Parameters.AddWithValue("@TIPO", 12);
                    t.SelectCommand.Parameters.AddWithValue("@RUC", ruc);
                    t.SelectCommand.Parameters.AddWithValue("@ROL", role);
                    t.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        str_err = "No se encontraron datos.";
                        dt = null;
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    dt = null;
                    str_err = "Hubo un problema con el metodo getFechaVencimiento():\\n" + ex.Message;
                    return dt;
                }
            }
            return dt;
        }

        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            Int64 iPP = Convert.ToInt64(e.Parameters[0].Values[0]);
            Session["IDPP"] = iPP;
            e.DataSources.Add(new ReportDataSource("dsHorariosPP", "odsdsHorariosPasePuerta"));
            /*e.DataSources.Add(new ReportDataSource()
            {
                Name = "dsContenedoresPorBooking",
                Value = sBooking
            });*/
        }

        public void Sindatos()
        {
            //utilform.MessageBox("No existe Informacion para Presentar", this);

            this.Alerta("No existe Informacion para Presentar");
            String JScript=""; 
            JScript = "<script language='javascript'>";
            //JScript += "opener.location.reload();";
            JScript += "window.close();";
            JScript += "</script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarpagina", JScript, false);
                        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetReporte();
            }
        }



        public Boolean inicializaReporte(String Reporte)
        {
            ReportParameter wrparameter = new ReportParameter();
            String wuser = Page.User.Identity.Name; //Session["puser"].ToString();  //Session["gUser"].ToString();
            if (System.IO.File.Exists(Reporte) != true)
            {
                //utilform.MessageBox("El Reporte No Existe..", this);
                this.Alerta("El Reporte No Existe..");
                String JScript = "";
                JScript = "<script language='javascript'>";
                // JScript += "opener.location.reload();";
                JScript += "window.close();";
                JScript += "</script>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarpagina", JScript, false);

                return false;
            }
            wrparameter = new ReportParameter("rp_user", wuser);

            rwReporte.ProcessingMode = ProcessingMode.Local;
            rwReporte.LocalReport.ReportPath = Reporte;
            //rwReporte.LocalReport.DataSources = null;
            //rwReporte.LocalReport.Refresh();
            rwReporte.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { wrparameter });

            rwReporte.Visible = true;


            return true;
        }
    

    public void  AñadeDatasorurce(ReportDataSource wdatasourc ){
        rwReporte.LocalReport.DataSources.Add(wdatasourc);
    }
    public Boolean AcceptAllCertifications(Object sender , System.Security.Cryptography.X509Certificates.X509Certificate certification,System.Security.Cryptography.X509Certificates.X509Chain chain  , System.Net.Security.SslPolicyErrors sslPolicyErrors )
    {
        return true;
}

    }
}