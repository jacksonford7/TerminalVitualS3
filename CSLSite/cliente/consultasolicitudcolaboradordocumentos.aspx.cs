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
    public partial class consultasolicitudcolaboradordocumentos : System.Web.UI.Page
    {
        public String cedula
        {
            get { return (String)Session["cedulaconsultasolicitudcolaboradordocumentos"]; }
            set { Session["cedulaconsultasolicitudcolaboradordocumentos"] = value; }
        }
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudconsultasolicitudcolaboradordocumentos"]; }
            set { Session["numsolicitudconsultasolicitudcolaboradordocumentos"] = value; }
        }
        public String idsolcol
        {
            get { return (String)Session["idsolcolconsultasolicitudcolaboradordocumentos"]; }
            set { Session["idsolcolconsultasolicitudcolaboradordocumentos"] = value; }
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
            idsolcol = Request.QueryString["idsolcol"];
            cedula = Request.QueryString["cedula"];
            DataTable dtDocumentos = credenciales.GetDocumentosColaboradorXNumSolicitudCliente(numsolicitudempresa, idsolcol);
            tablePaginationDocumentos.DataSource = dtDocumentos;
            tablePaginationDocumentos.DataBind();
            alerta.Attributes["class"] = string.Empty;
            alerta.Attributes["class"] = "msg-info";
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