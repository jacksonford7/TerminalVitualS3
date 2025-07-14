
using BillionEntidades;
using CSLSite;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.reportesexpo
{
    public partial class rptContenedorExpo : System.Web.UI.Page
    {
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;

        #region "Variables"
        private string nave = string.Empty;
        private string cMensajes;
        private string LoginName = string.Empty;
        #endregion

        #region "Propiedades"

        private Int64? nSesion
        {
            get
            {
                return (Int64)Session["nSesion"];
            }
            set
            {
                Session["nSesion"] = value;
            }

        }
        #endregion

        #region "Metodos"

        private void Actualiza_Paneles()
        {
            UPCARGA.Update();
            UPBOTONES.Update();
            UPPrincipal.Update();
        }


        private void Limpia_Campos()
        {
            this.TXTMRN.Text = string.Empty;
            this.Actualiza_Paneles();
        }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.imagen.Visible = true;
            this.ReportViewer1.Visible = false;

            OcultarLoading("1");

            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
            OcultarLoading("2");
        }

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();
        }
        #endregion

        #region "Metodos del Reporte"

        public Boolean inicializaReporte(String Reporte)
        {

            String wuser = Page.User.Identity.Name;
            if (System.IO.File.Exists(Reporte) != true)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Reporte no existe"));
                return false;
            }

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Reporte;
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.Visible = true;

            return true;
        }

        public void AñadeDatasorurce(ReportDataSource wdatasourc)
        {
            ReportViewer1.LocalReport.DataSources.Add(wdatasourc);
        }
        public Boolean AcceptAllCertifications(Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void GetReporte()
        {
            String Reporte = "";
            ReportDataSource wdatasourc;
            DataSet wdataset = new DataSet();

            DataTable dt = new DataTable();
            wdataset.Tables.Add(dt);
            dt.Columns.Add("CONTAINER", typeof(string));
            dt.Columns.Add("MRN", typeof(string));


            Reporte = "rptpasepuerta.rdlc";
            Reporte = this.Server.MapPath(@"..\reportes\" + Reporte);
            if (inicializaReporte(Reporte) != true)
            {
                return;
            }

            wdatasourc = new ReportDataSource("dsPasePuerta", wdataset.Tables[0]);
            AñadeDatasorurce(wdatasourc);
            ReportViewer1.LocalReport.Refresh();
        }

        #endregion

        #region "Eventos page"
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);

                return;
            }

            this.banmsg.Visible = IsPostBack;
            this.banmsg_det.Visible = IsPostBack;

            ClsUsuario = Page.Tracker();
            if (ClsUsuario != null)
            {
                if (!Page.IsPostBack)
                {
                    this.Limpia_Campos();
                    ReportViewer1.Visible = false;
                    this.imagen.Visible = true;
                    this.Actualiza_Paneles();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                Server.HtmlEncode(this.TXTMRN.Text.Trim());
             
                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion

        protected void Btnbuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.TXTMRN.Text))
            {
                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor ingresar NAVE REFERENCIA"));
                this.TXTMRN.Focus();
                return;
            }

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
                    Populate();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
                }
            }
        }

        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }

        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            Session["i_NaveRef"] = TXTMRN.Text;
            e.DataSources.Add(new ReportDataSource("dsContenedorExpo", "odsContenedorExport"));
        }

        private void Populate()
        {
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            Session["resultado"] = null;

            string vr = string.Empty;
            try
            {
                Session["i_NaveRef"] =  TXTMRN.Text;


               // ReportViewer1.

//                xfinder.Visible = true;
                ReportParameter ReportParameter_filtro_ = new ReportParameter("ReportParameter_filtro", "");
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });

                ReportViewer1.SizeToReportContent = true;
                ReportViewer1.ShowToolBar = true;
                ReportViewer1.ShowPrintButton = true;

                ReportViewer1.LocalReport.Refresh();
                
                this.ReportViewer1.Visible = true;
                this.imagen.Visible = false;
                this.Ocultar_Mensaje();
                this.Actualiza_Paneles();

            }
            catch (Exception ex)
            {
                //var t = this.getUserBySesion();
                //sinresultado.Attributes["class"] = string.Empty;
                //sinresultado.Attributes["class"] = "msg-critico";
                //vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "frm_listado_det_nota_credito", "populate()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
//                sinresultado.Visible = true;
            }

        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
    }
}