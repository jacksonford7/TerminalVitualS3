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

namespace CSLSite
{
    public partial class datossolicitudvehiculo : System.Web.UI.Page
    {
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicituddatossolicitudvehiculo"]; }
            set { Session["numsolicituddatossolicitudvehiculo"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["rucempresadatossolicitudvehiculo"]; }
            set { Session["rucempresadatossolicitudvehiculo"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremaildatossolicitudvehiculo"]; }
            set { Session["useremaildatossolicitudvehiculo"] = value; }
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
                //foreach (string var in Request.QueryString)
                //{
                numsolicitudempresa = Request.QueryString["numsolicitud"];
                var placa = Request.QueryString["placa"];
                //}
                //if (!credenciales.GetSolicitudEstado(numsolicitudempresa))
                //{
                //    botonera.Visible = false;
                //    factura.Visible = false;
                //    salir.Visible = true;
                //}
                //if (!credenciales.GetTipoSolicitud(numsolicitudempresa))
                //{
                //    factura.Visible = false;
                //    btsalvar.Text = "Finalizar";
                //    btsalvar.ToolTip = "Finaliza la solicitud.";
                //}
                this.alerta.InnerHtml = "Confirme que los documentos del vehiculo(s) sean los correctos.";
                var tablixVehiculo = credenciales.GetSolicitudVehiculoDetalle(numsolicitudempresa, placa);
                tablePagination.DataSource = tablixVehiculo;
                tablePagination.DataBind();
            }
            catch (Exception ex)
            {
                Session["dtDocumentosrevisasolicitudvehiculo"] = new DataTable();
                this.Alerta(ex.Message);
            }
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                this.Alerta(ex.Message);
            }
        }
        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDocSol = Session["dtDocumentosrevisasolicitudvehiculo"] as DataTable;
                if (dtDocSol == null)
                {
                    this.Alerta("NO tiene documentos rechazados.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud.");
                    return;
                }
                if (dtDocSol.Rows.Count == 0)
                {
                    this.Alerta("NO tiene documentos rechazados.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud.");
                    return;
                }
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblplaca = item.FindControl("lblplaca") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    var result = from myRow in dtDocSol.AsEnumerable()
                                 where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true && myRow.Field<string>("Placa") == lblplaca.Text
                                 select myRow;
                    DataTable dt = result.AsDataView().ToTable();
                    if (dt.Rows.Count == 0)
                    {
                        this.Alerta("La Placa: *" + lblplaca.Text + "*\\nNoTiene documentos rechazados.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud.");
                        return;
                    }
                }
                var resultVehiculo = from myRow in dtDocSol.AsEnumerable()
                             where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true
                             select myRow;
                DataTable dtVehiculos = resultVehiculo.AsDataView().ToTable();
                dtVehiculos.AcceptChanges();
                dtVehiculos.TableName = "Vehiculos";
                StringWriter sw = new StringWriter();
                dtVehiculos.WriteXml(sw);
                String xmlVehiculos = sw.ToString();
                string nombreempresa = CslHelper.getShiperName(rucempresa);
                string mensaje = null;
                if (!credenciales.RechazaSolicitudVehiculo(
                    numsolicitudempresa,
                    rucempresa,
                    nombreempresa,
                    useremail,
                    xmlVehiculos,
                    Page.User.Identity.Name,
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    //this.Alerta("Solicitud rechazada exitosamente, en unos minutos recibirá una notificación via mail.");
                    //botonera.Visible = false;
                    //factura.Visible = false;
                    //salir.Visible = true;
                    //Response.Write("<script language='JavaScript'>window.close();</script>");
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud rechazada exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                }
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