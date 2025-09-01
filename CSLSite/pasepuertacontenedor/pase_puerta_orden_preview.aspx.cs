using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
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

            rwReporte.LocalReport.ReportPath = Reporte;
            rwReporte.Visible = true;

            return true;
        }

        public void AñadeDatasorurce(ReportDataSource wdatasourc)
        {
            rwReporte.LocalReport.DataSources.Add(wdatasourc);
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


        private void Poblar_PasePuerta_Orden(string numeroPase)
        {
            try
            {
                var table = new DataTable();

                using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString))
                using (var cmd = new SqlCommand("vhs.SP_RPT_PasePuerta_Orden", cn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NumeroPase", SqlDbType.VarChar).Value = numeroPase;
                    da.Fill(table);
                }

                rwReporte.LocalReport.DataSources.Clear();
                rwReporte.LocalReport.ReportPath = "reportes/rptpasepuerta_orden.rdlc";
                rwReporte.LocalReport.DataSources.Add(new ReportDataSource("dsPasePuerta", table));
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

                    string numeroPase = Request.QueryString["NumeroPase"];
                    if (!string.IsNullOrEmpty(numeroPase))
                    {
                        Poblar_PasePuerta_Orden(numeroPase);
                        return;
                    }

                    string idParam = Request.QueryString["id_pase"];
                    if (string.IsNullOrEmpty(idParam) || !long.TryParse(idParam, out id_pase))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para emitir pase"));
                        return;
                    }

                    this.hf_BrowserWindowName.Value = id_pase.ToString();
                    Poblar_PasePuerta_Orden(id_pase.ToString());
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...{0}", ex.Message));
            }
        }
    }
}
