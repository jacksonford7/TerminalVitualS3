using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Security;
using System.Data;
using System.Globalization;
using BillionEntidades;
using System.Text;

using Microsoft.Reporting.WebForms;



namespace CSLSite
{
    public partial class pase_puerta_cfs_preview : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private List<Cls_Bil_CargaSuelta_Gkey> Lista { set; get; }
        #endregion

        #region "Variables"
        private string NombreUsuario = string.Empty;
        private string id_comp = string.Empty;
        string cMensaje = string.Empty;

        private Int64 id_pase = 0;
        #endregion



        #region "Metodos del Reporte"

        public Boolean inicializaReporte(String Reporte)
        {

            String wuser = Page.User.Identity.Name;
            if (System.IO.File.Exists(Reporte) != true)
            {
                this.Mostrar_Mensaje( string.Format("<b>Informativo! </b>Reporte no existe"));
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
        public Boolean AcceptAllCertifications(Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


        #endregion

        #region "Metodos"
        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();

        }

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

        private void Poblar_PasePuerta()
        {
            List<string> ListaImprimir = new List<string>();

            //cabecera de transaccion
            Lista = new List<Cls_Bil_CargaSuelta_Gkey>();
            Lista = Session["ConsultaPaseCFS" + this.hf_BrowserWindowName.Value] as List<Cls_Bil_CargaSuelta_Gkey>;

            if (Lista == null)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para emitir pase"));
                return;
            }
            else
            {

                foreach (var Det in Lista)
                {
                    ListaImprimir.Add(Det.Gkey);
                }

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var DsReporte = PasePuerta.Pase_CFS.ImprimirPasesCFS_ALT(ListaImprimir, ClsUsuario.ruc);
                if (DsReporte.Exitoso)
                {
                    DataSet wdataset = new DataSet();
                    String Reporte = "";
                    ReportDataSource wdatasourc;

                    wdataset = DsReporte.Resultado;

                    ServicePointManager.ServerCertificateValidationCallback += AcceptAllCertifications;

                    Reporte = "rptpasepuertacfs.rdlc";
                    Reporte = this.Server.MapPath(@"..\reportes\" + Reporte);
                    if (inicializaReporte(Reporte) != true)
                    {
                        return;
                    }

                    wdatasourc = new ReportDataSource("dsPasePuerta", wdataset.Tables[0]);
                    AñadeDatasorurce(wdatasourc);
                    rwReporte.LocalReport.Refresh();
                    this.rwReporte.Visible = true;
                    rwReporte.DataBind();
                    this.imagen.Visible = false;


                }
            }

            this.Ocultar_Mensaje();

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

                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {
                    NombreUsuario = ClsUsuario.nombres + " " + ClsUsuario.apellidos;
                }


                id_comp = QuerySegura.DecryptQueryString(Request.QueryString["id_pase"]);
                if (Request.QueryString["id_pase"] == null || string.IsNullOrEmpty(id_comp))
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para emitir pase"));  
                    return;
                }

            }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...{0}", ex.Message));

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

                if (!IsPostBack)
                {
                    try
                    {
                        id_comp = id_comp.Trim().Replace("\0", string.Empty);

                        if (!Int64.TryParse(id_comp, out id_pase))
                        {
                            //campo no es numerico
                            this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error al convertir {0}", id_comp));
                            return;
                        }
                        this.hf_BrowserWindowName.Value = id_pase.ToString();

                        this.Poblar_PasePuerta();
                    }
                    catch (Exception ex)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...{0}", ex.Message));
                    }
                }
            }

        }
    }
}