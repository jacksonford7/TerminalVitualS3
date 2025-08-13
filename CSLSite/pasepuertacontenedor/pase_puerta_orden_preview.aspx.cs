using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Security;
using System.Web;
using BillionEntidades;
using Microsoft.Reporting.WebForms;

namespace CSLSite
{
    public partial class pase_puerta_orden_preview : System.Web.UI.Page
    {
        #region "Variables"
        private long id_pase = 0;
        #endregion

        #region "Metodos del Reporte"
        public Boolean inicializaReporte(String Reporte)
        {
            String wuser = Page.User.Identity.Name;
            if (System.IO.File.Exists(Reporte) != true)
            {
                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Reporte no existe"));
                return false;
            }

            rwReporte.ProcessingMode = ProcessingMode.Local;
            rwReporte.LocalReport.ReportPath = Reporte;
            rwReporte.LocalReport.EnableExternalImages = true;
            rwReporte.LocalReport.Refresh();
            rwReporte.Visible = true;

            return true;
        }

        public void AñadeDatasorurce(ReportDataSource wdatasourc)
        {
            rwReporte.LocalReport.DataSources.Clear();
            rwReporte.LocalReport.DataSources.Add(wdatasourc);
        }

        public Boolean AcceptAllCertifications(Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion

        #region "Metodos"
        private void Actualiza_Paneles()
        {
            UPPASEPUERTA.Update();
            UPMENSAJE.Update();
        }

        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
        }

        private void Poblar_PasePuerta()
        {
            try
            {
                DataSet wdataset = new DataSet();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand("[dbo].[lista_pase_despacho_por_idpase_orden]", conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pase", id_pase);
                    da.Fill(wdataset);
                }

                if (wdataset.Tables.Count == 0 || wdataset.Tables[0].Rows.Count == 0)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para emitir pase"));
                    return;
                }

                ServicePointManager.ServerCertificateValidationCallback += AcceptAllCertifications;

                string Reporte = Server.MapPath(@"..\\reportes\\rptpasepuerta_orden.rdlc");
                if (inicializaReporte(Reporte) != true)
                {
                    return;
                }

                var table = wdataset.Tables[0];
                var row = table.Rows[0];

                object payload = table.Columns.Contains("NUMERO_PASE_N4")
                    ? row["NUMERO_PASE_N4"]
                    : (object)id_pase.ToString();

                string relQr = $"~/barcode/handler/qr.ashx?data={HttpUtility.UrlEncode(Convert.ToString(payload))}";
                string absQr = new Uri(Request.Url, ResolveUrl(relQr)).ToString();

                var ds = new ReportDataSource("dsPasePuerta", table);
                AñadeDatasorurce(ds);

                var prmQr = new ReportParameter("QrUrl", string.IsNullOrWhiteSpace(absQr) ? string.Empty : absQr);
                rwReporte.LocalReport.SetParameters(new[] { prmQr });

                rwReporte.LocalReport.Refresh();
                this.rwReporte.Visible = true;
                rwReporte.DataBind();
                this.imagen.Visible = false;
                this.Ocultar_Mensaje();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...{0}", ex.Message));
            }
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);
                    return;
                }

                if (!IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;

                    string idParam = Request.QueryString["id_pase"];
                    if (string.IsNullOrEmpty(idParam) || !long.TryParse(idParam, out id_pase))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para emitir pase"));
                        return;
                    }

                    this.hf_BrowserWindowName.Value = id_pase.ToString();
                    Poblar_PasePuerta();
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...{0}", ex.Message));
            }
        }
    }
}
