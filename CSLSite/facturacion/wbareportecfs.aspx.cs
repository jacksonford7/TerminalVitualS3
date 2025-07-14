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
    public partial class wbareportecfs : System.Web.UI.Page
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

                    case "Factura":

                        String p_camara = (String)Session["pcamara"];
                        if (p_camara == "NO")
                        {
                            Reporte = "RptFactura.rdlc";
                        }
                        else
                        {
                            Reporte = "RptFacturaCamara.rdlc";
                        }
                        // Reporte = this.Server.MapPath(@"~\facturacion\Reporte\" + Reporte);
                        Reporte = this.Server.MapPath(@"..\facturacion\" + Reporte);
                        if (inicializaReporte(Reporte) != true)
                        {
                            return;
                        }

                        wdataset = (DataSet)Session["dsFactura"];

                        if (wdataset == null || wdataset.Tables.Count <= 0)
                        {
                            Sindatos();
                        }
                        else
                        {


                            CultureInfo info = CultureInfo.GetCultureInfo("en-US");

                            wvalor = (from report in wdataset.Tables[0].AsEnumerable()
                                      select report.Field<decimal>("TOTAL") == null ? 0 : Decimal.Parse(report.Field<decimal>("TOTAL").ToString(), info)
                                               ).Sum().ToString();


                            if (wdataset != null && wdataset.Tables[0].Rows.Count < 15)
                            {
                                for (int i = wdataset.Tables[0].Rows.Count - 1; i < 15; i++)
                                {

                                    DataRow newCustomersRow = wdataset.Tables[0].NewRow();
                                    newCustomersRow["TOTAL"] = 0;
                                    newCustomersRow["DESCRIPCION"] = "";
                                    newCustomersRow["CODIGO"] = "";
                                    newCustomersRow["CANTIDAD"] = 0;
                                    newCustomersRow["PRECIO"] = 0;
                                    newCustomersRow["IVA"] = 0;
                                    newCustomersRow["FACTURA"] = "";
                                    newCustomersRow["RUC"] = "";
                                    newCustomersRow["CLIENTE"] = 0;
                                    newCustomersRow["DIRECCION"] = "";
                                    newCustomersRow["CIUDAD"] = "";
                                    newCustomersRow["PROVINCIA"] = "";
                                    wdataset.Tables[0].Rows.Add(newCustomersRow);
                                    wdataset.Tables[0].AcceptChanges();
                                    wdataset.AcceptChanges();
                                }
                            }

                            var str_err = string.Empty;
                            ReportParameter fecha_wrparameter = new ReportParameter();
                            var fv_ruc = (String)Session["fv_ruc_empresa"];
                            var fv_role = (String)Session["fv_role_empresa"];
                            var dt_fecha_vencimiento = getFechaVencimiento(out str_err, fv_ruc, fv_role);
                            if (!string.IsNullOrEmpty(str_err) || dt_fecha_vencimiento == null)
                            {
                                //UIHelper.Sindatos(this, );
                                this.Alerta("Hubo un problema conel metodo PoblarCodigoSap():\\n" + str_err);
                                return;
                            }

                            wdatasourc = new ReportDataSource("dsFactura", wdataset.Tables[0]);
                            AñadeDatasorurce(wdatasourc);

                            //wvalor = (String)Session["RIva"];

                            wrparameter = new ReportParameter("rp_rtotal", wvalor);
                            fecha_wrparameter = new ReportParameter("rp_fecha_vencimiento", dt_fecha_vencimiento.Rows[0]["FECHA_VENCIMIENTO"].ToString());
                            rwReporte.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { wrparameter, fecha_wrparameter });
                        }

                        break;

                    case "PasePuerta":
                        if ((String)Session["ptypeReport"] == "CONTENEDOR")
                        {
                            Reporte = "RptPasePuert.rdlc";
                        }
                        else
                        {
                            if (Convert.ToBoolean(Session["EstadoPPSinTurno"]) == true)
                            {
                                Reporte = "RptPasePuertaBreakBulk.rdlc";
                            }
                            else
                            {
                                Reporte = "RptPasePuertaBreakBulkSubRpt.rdlc";
                            }
                            //Reporte = "RptPasePuertaBreakBulk.rdlc";
                        }
                        // Reporte = this.Server.MapPath(@"~\facturacion\Reporte\" + Reporte);
                        Reporte = this.Server.MapPath(@"..\facturacion\" + Reporte);
                        if (inicializaReporte(Reporte) != true)
                        {
                            return;
                        }

                        wdataset = (DataSet)Session["dsPasePuerta"];

                        if (wdataset == null || wdataset.Tables.Count <= 0)
                        {
                            Sindatos();
                        }
                        else
                        {
                            for (int i = 0; i < wdataset.Tables[0].Rows.Count; i++)
                            {
                                if ((String)Session["ptypeReport"] == "CONTENEDOR")
                                {
                                    wdataset.Tables[0].Rows[i]["LINE"] = pasePuerta.GetInfoCerrojo(wdataset.Tables[0].Rows[i]["DOCUMENTO"].ToString()).Rows[0]["CERROJO"].ToString();
                                }
                                else
                                {
                                    if (Convert.ToBoolean(Session["EstadoPPSinTurno"]) != true)
                                    {

                                        wdataset.Tables[0].Rows[i]["FACTURADOR"] = pasePuerta.GetInfoCerrojoCFS(
                                        wdataset.Tables[0].Rows[i]["MRN"].ToString(),
                                        wdataset.Tables[0].Rows[i]["MSN"].ToString(),
                                        wdataset.Tables[0].Rows[i]["HSN"].ToString()).Rows[0]["CERROJO"].ToString();
                                        
                                        var cargarep = wdataset.Tables[0].Rows[i]["MRN"].ToString() + "-" +
                                                       wdataset.Tables[0].Rows[i]["MSN"].ToString() + "-" +
                                                       wdataset.Tables[0].Rows[i]["HSN"].ToString();
                                        if (string.IsNullOrEmpty(wdataset.Tables[0].Rows[i]["DOCUMENTO"].ToString()))
                                        {
                                            var infoppweb = pasePuerta.GetInfoPPWebCFS(wdataset.Tables[0].Rows[i]["FACTURA"].ToString(), cargarep.Trim());
                                            if (infoppweb != null || infoppweb.Rows.Count > 0)
                                            {
                                                wdataset.Tables[0].Rows[i]["DOCUMENTO"] = infoppweb.Rows[0]["DOCUMENTO"].ToString();
                                                wdataset.Tables[0].AcceptChanges();
                                            }
                                        }
                                        if (string.IsNullOrEmpty(wdataset.Tables[0].Rows[i]["IMPORTADOR"].ToString()))
                                        {
                                            var infoppweb = pasePuerta.GetInfoPPWebCFS(wdataset.Tables[0].Rows[i]["FACTURA"].ToString(), cargarep.Trim());
                                            if (infoppweb != null || infoppweb.Rows.Count > 0)
                                            {
                                                wdataset.Tables[0].Rows[i]["IMPORTADOR"] = infoppweb.Rows[0]["IMPORTADOR"].ToString();
                                                wdataset.Tables[0].AcceptChanges();
                                            }
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
                            rwReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
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

                        if (Wtipo == "CNTR")
                        {
                            Reporte = "RptPasePuert.rdlc";
                        }
                        else
                        {
                            if (Convert.ToBoolean(Session["EstadoPPSinTurno"]) == true)
                            {
                                Reporte = "RptPasePuertaBreakBulk.rdlc";
                            }
                            else
                            {
                                wdataset.Tables[0].Rows[0]["FACTURADOR"] = pasePuerta.GetInfoCerrojoCFS(
                                wdataset.Tables[0].Rows[0]["MRN"].ToString(),
                                wdataset.Tables[0].Rows[0]["MSN"].ToString(),
                                wdataset.Tables[0].Rows[0]["HSN"].ToString()).Rows[0]["CERROJO"].ToString();


                                if (string.IsNullOrEmpty(wdataset.Tables[0].Rows[0]["DOCUMENTO"].ToString()))
                                {
                                    var infoppweb = pasePuerta.GetInfoPPWebCFS(wdataset.Tables[0].Rows[0]["FACTURA"].ToString(), Request.QueryString["Carga"].ToString().Trim());
                                    if (infoppweb != null || infoppweb.Rows.Count > 0)
                                    {
                                        wdataset.Tables[0].Rows[0]["DOCUMENTO"] = infoppweb.Rows[0]["DOCUMENTO"].ToString();
                                        //wdataset.Tables[0].Rows[0]["IMPORTADOR"] = infoppweb.Rows[0]["IMPORTADOR"].ToString();
                                        wdataset.Tables[0].AcceptChanges();
                                    }
                                }
                                if (string.IsNullOrEmpty(wdataset.Tables[0].Rows[0]["IMPORTADOR"].ToString()))
                                {
                                    var infoppweb = pasePuerta.GetInfoPPWebCFS(wdataset.Tables[0].Rows[0]["FACTURA"].ToString(), Request.QueryString["Carga"].ToString().Trim());
                                    if (infoppweb != null || infoppweb.Rows.Count > 0)
                                    {
                                        //wdataset.Tables[0].Rows[0]["DOCUMENTO"] = infoppweb.Rows[0]["DOCUMENTO"].ToString();
                                        wdataset.Tables[0].Rows[0]["IMPORTADOR"] = infoppweb.Rows[0]["IMPORTADOR"].ToString();
                                        wdataset.Tables[0].AcceptChanges();
                                    }
                                }

                                Reporte = "RptPasePuertaBreakBulkSubRpt.rdlc";
                            }
                        }

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
                            if (Convert.ToBoolean(Session["EstadoPPSinTurno"]) == false)
                            {
                                rwReporte.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
                            }
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