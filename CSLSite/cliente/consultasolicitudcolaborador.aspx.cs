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

namespace CSLSite.cliente
{
    public partial class consultasolicitudcolaborador : System.Web.UI.Page
    {
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudrevisaconsultasolicitudcolaborador"]; }
            set { Session["numsolicitudrevisaconsultasolicitudcolaborador"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["rucempresaconsultasolicitudcolaborador"]; }
            set { Session["rucempresaconsultasolicitudcolaborador"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailconsultasolicitudcolaborador"]; }
            set { Session["useremailconsultasolicitudcolaborador"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            //this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
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
                ConsultaInfoSolicitud();
            }
        }
        private void ConsultaInfoSolicitud()
        {
            try
            {
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString[var];
                }
                var DescripcionTipoSolicitud = credenciales.GetDescripcionTipoSolicitudXNumSolicitud(numsolicitudempresa);
                for (int i = 0; i <= DescripcionTipoSolicitud.Rows.Count - 1; i++)
                {
                    txttipcli.Text = DescripcionTipoSolicitud.Rows[i][0].ToString();
                }
                var tablixVehiculo = credenciales.GetSolicitudColaboradorCliente_New(numsolicitudempresa, rucempresa);
                tablePagination.DataSource = tablixVehiculo;
                tablePagination.DataBind();
            }
            catch (Exception ex)
            {
                this.Alerta(ex.Message);
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