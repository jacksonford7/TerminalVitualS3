using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Configuration;
using System.Globalization;
using ConectorN4;

namespace CSLSite.cliente
{
    public partial class consultasolicitudempresa : System.Web.UI.Page
    {
        private String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudconsultasolicitudempresa"]; }
            set { Session["numsolicitudconsultasolicitudempresa"] = value; }
        }
        private string rucempresa
        {
            get { return (string)Session["rucempresaconsultasolicitudempresa"]; }
            set { Session["rucempresaconsultasolicitudempresa"] = value; }
        }
        private string useremail
        {
            get { return (string)Session["useremailconsultasolicitudempresa"]; }
            set { Session["useremailconsultasolicitudempresa"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
            //this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../csl/login", true);
                }
                //this.agencia.Value = user.ruc;
                rucempresa = user.ruc;
                useremail = user.email;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
                try
                {
                    ConsultaInfoSolicitud();
                }
                catch (Exception ex)
                {
                    this.Alerta(ex.Message);
                }
            }
        }
        private void ConsultaInfoSolicitud()
        {
            foreach (string var in Request.QueryString)
            {
                numsolicitudempresa = Request.QueryString[var];
            }
            var tablixDocumentos = credenciales.GetDocumentosEmpresaXNumSolicitudCliente(numsolicitudempresa);
            tablePaginationDocumentos.DataSource = tablixDocumentos;
            tablePaginationDocumentos.DataBind();
            var tablixSolicitudEmpresa = credenciales.GetSolicitudEmpresa(numsolicitudempresa);
            for (int i = 0; i <= tablixSolicitudEmpresa.Rows.Count - 1; i++)
            {
                txttipcli.Text = tablixSolicitudEmpresa.Rows[i][0].ToString();
                txtrazonsocial.Text = tablixSolicitudEmpresa.Rows[i][3].ToString();
                txtruccipas.Text = tablixSolicitudEmpresa.Rows[i][4].ToString();
                txtactividadcomercial.Text = tablixSolicitudEmpresa.Rows[i][5].ToString();
                txtdireccion.Text = tablixSolicitudEmpresa.Rows[i][6].ToString();
                txttelofi.Text = tablixSolicitudEmpresa.Rows[i][7].ToString();
                txtcontacto.Text = tablixSolicitudEmpresa.Rows[i][8].ToString();
                txttelcelcon.Text = tablixSolicitudEmpresa.Rows[i][9].ToString();
                txtmailinfocli.Text = tablixSolicitudEmpresa.Rows[i][10].ToString();
                txtcertificaciones.Text = tablixSolicitudEmpresa.Rows[i][11].ToString();
                turl.Text = tablixSolicitudEmpresa.Rows[i][12].ToString();
                txtafigremios.Text = tablixSolicitudEmpresa.Rows[i][13].ToString();
                txtrefcom.Text = tablixSolicitudEmpresa.Rows[i][14].ToString();
                txtreplegal.Text = tablixSolicitudEmpresa.Rows[i][15].ToString();
                txttelreplegal.Text = tablixSolicitudEmpresa.Rows[i][16].ToString();
                txtdirdomreplegal.Text = tablixSolicitudEmpresa.Rows[i][17].ToString();
                txtci.Text = tablixSolicitudEmpresa.Rows[i][18].ToString();
                tmailRepLegal.Text = tablixSolicitudEmpresa.Rows[i][19].ToString();
                txtmailebilling.Text = tablixSolicitudEmpresa.Rows[i][20].ToString();
                txtmotivorechazo.Text = tablixSolicitudEmpresa.Rows[i]["OBSERVACION"].ToString();
            }
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Write("<script language='JavaScript'>window.close();</script>");
            }
            catch (Exception ex)
            {
                this.Alerta(ex.Message);
            }
        }
    }
}