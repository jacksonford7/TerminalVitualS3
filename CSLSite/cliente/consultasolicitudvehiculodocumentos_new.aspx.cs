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
    public partial class consultasolicitudvehiculodocumentos_new : System.Web.UI.Page
    {
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudconsultasolicitudvehiculodocumentos"]; }
            set { Session["numsolicitudconsultasolicitudvehiculodocumentos"] = value; }
        }
        public String idsolveh
        {
            get { return (String)Session["idsolvehconsultasolicitudvehiculodocumentos"]; }
            set { Session["idsolvehconsultasolicitudvehiculodocumentos"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
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
            numsolicitudempresa = Request.QueryString["numsolicitud"];
            idsolveh = Request.QueryString["idsolveh"];

            DataTable dtDocumentos = credenciales.GetDocumentosVehiculoXNumSolicitudCliente_New(numsolicitudempresa, idsolveh);
            tablePaginationDocumentos.DataSource = dtDocumentos;
            tablePaginationDocumentos.DataBind();
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