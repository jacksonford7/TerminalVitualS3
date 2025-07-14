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
    public partial class wbareportezal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var dtresult = GetDescripcion(Session["idpaseszal"].ToString());
                ReportParameter exportador = new ReportParameter("EXPORTADOR", dtresult.Rows[0]["EXPORTADOR"].ToString());
                ReportParameter cliente = new ReportParameter("CLIENTE", dtresult.Rows[0]["RUC"].ToString());
                rwReporte.LocalReport.SetParameters(new ReportParameter[] { cliente, exportador });
                rwReporte.LocalReport.EnableHyperlinks = true;
                rwReporte.LocalReport.Refresh();
            }
        }

        private static SqlConnection conexionmidle()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
        }

        public static DataTable GetDescripcion(string xml)
        {
            var d = new DataTable();
            using (var c = conexionmidle())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "VBS_P_RPT_PASE_PUERTA_ZAL_CLIENTS";
                coman.Parameters.AddWithValue("@ID", xml);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    var error = log_csl.save_log<Exception>(ex, "man_pro_expo", "GetCatalagoLineaAsumeHorasReefer", DateTime.Now.ToShortDateString(), "sistema");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        /*
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
        */
    }
}