using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Security;
using System.Web;
using System.Linq;
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

            rwReporte.LocalReport.ReportPath = Reporte;
            rwReporte.Visible = true;

            return true;
        }

        public void AñadeDatasorurce(ReportDataSource wdatasourc)
        {
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
                int idPase = Convert.ToInt32(Request.QueryString["id_pase"]);

                // 2) Ejecutar SP y llenar DataTable
                var table = new DataTable();
                using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                using (var cmd = new SqlCommand("dbo.lista_pase_despacho_por_idpase", cn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_pase", idPase);
                    da.Fill(table);
                }

                if (table.Rows.Count == 0)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para emitir pase"));
                    return;
                }

                // 3) Homologar columnas que el RDLC espera (crear si faltan)
                string[] esperado = {
                    "NUMERO_PASE_N4","CONTAINER","SELLO","SELLO_GEO","TIPO_ISO","IMPORTADOR",
                    "RUC_EMP","EMP_TRANS","PLACA","LICENCIA","CONDUCTOR","MRN","DOCUMENTO","SN"
                };
                foreach (var col in esperado)
                    if (!table.Columns.Contains(col)) table.Columns.Add(col, typeof(string));

                // 4) Construir QR_URL absoluta
                if (!table.Columns.Contains("QR_URL")) table.Columns.Add("QR_URL", typeof(string));
                string baseQr(object payload)
                {
                    var rel = $"~/barcode/handler/qr.ashx?data={HttpUtility.UrlEncode(Convert.ToString(payload ?? ""))}";
                    return new Uri(Request.Url, ResolveUrl(rel)).ToString();
                }
                foreach (DataRow r in table.Rows)
                {
                    var payload = table.Columns.Contains("NUMERO_PASE_N4") && r["NUMERO_PASE_N4"] != DBNull.Value
                        ? r["NUMERO_PASE_N4"] : (object)idPase;
                    r["QR_URL"] = baseQr(payload);
                }

                ServicePointManager.ServerCertificateValidationCallback += AcceptAllCertifications;

                // 5) Inicializar ReportViewer con RDLC clonado + dsPasePuerta
                var path = Server.MapPath(@"..\reportes\rptpasepuerta_orden.rdlc");
                if (!inicializaReporte(path)) return;

                rwReporte.LocalReport.EnableExternalImages = true;
                rwReporte.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                rwReporte.LocalReport.DataSources.Clear();
                var ds = new Microsoft.Reporting.WebForms.ReportDataSource("dsPasePuerta", table);
                AñadeDatasorurce(ds);
                rwReporte.LocalReport.Refresh();

                rwReporte.Visible = true;
                rwReporte.DataBind();
                imagen.Visible = false;
                Ocultar_Mensaje();
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
