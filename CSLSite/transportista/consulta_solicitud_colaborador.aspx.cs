using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Data;
using System.IO;
using csl_log;
using System.Web.Services;
using System.Web.UI.HtmlControls;

namespace CSLSite
{
    public partial class consulta_solicitud_colaborador : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        public string rucempresa
        {
            get { return (string)Session["rucempresaconsultacomprobantedepago"]; }
            set { Session["rucempresaconsultacomprobantedepago"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailconsultacomprobantedepago"]; }
            set { Session["useremailconsultacomprobantedepago"] = value; }
        }
        private DataTable dtDetSolicitud
        {
            get
            {
                return (DataTable)Session["dtDetSolicitud"];
            }
            set
            {
                Session["dtDetSolicitud"] = value;
            }

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
                //rucempresa = user.ruc;
                //foreach (string var in Request.QueryString)
                //{
                //    rucempresa = Request.QueryString["ruc"].ToString();
                //}
                //useremail = user.email;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    sinresultado.Visible = false;
                    xfinderpagado.Visible = false;
                    divseccion2.Visible = false;
                    divseccion3.Visible = false;
                    botonera.Visible = false;
                    var dtDiasAccesoTemp = aso_transportistas.GetDiasPermisoAccesoTemporal();
                    if (dtDiasAccesoTemp.Rows.Count == 0)
                    {
                        //this.alertapagado.InnerHtml = "Error al buscar parametros de dias referente al permiso de acceso temporal.";
                        this.Alerta("Error al buscar parametros de dias referente al permiso de acceso temporal.");
                        return;
                    }
                    ConsultaInformacion();
                    txtfecing.Text = Convert.ToDateTime(dtDiasAccesoTemp.Rows[0]["FECHA_INICIAL"].ToString()).ToString("dd/MM/yyyy");
                    txtfecsal.Text = Convert.ToDateTime(dtDiasAccesoTemp.Rows[0]["FECHA_FINAL"].ToString()).ToString("dd/MM/yyyy");
                    txtnumfactura.Text = "0";
                    this.alertapagado.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "consulta_solicitud_colaborador", "Page_Load", "1", Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                }
                //txtfecing.Enabled = false;
                //txtfecsal.Enabled = false;
            }
        }
        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
        private void ConsultaInformacion()
        {
            if (HttpContext.Current.Request.Cookies["token"] == null)
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            }
            txtsolicitud.Text = Request.QueryString["numsolicitud"].ToString();

            if (string.IsNullOrEmpty(txtsolicitud.Text))
            {
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                gvComprobantes.DataSource = null;
                gvComprobantes.DataBind();
                this.sinresultado.InnerHtml = string.Format("Ingrese al menos un criterio de consulta.{0}", "");
                sinresultado.Visible = true;
                return;
            }
            /*
            var tablix = credenciales.GetTipoDeDocumentosColaborador(rucempresa, txtsolicitud.Text);
            gvComprobantes.DataSource = tablix;
            gvComprobantes.DataBind();
            if (tablix.Rows.Count == 1)
            {
                if (tablix.Rows[0]["CODESTADO"].ToString() == "1")
                {
                    botonera.Visible = false;
                    divseccion2.Visible = false;
                    divseccion3.Visible = false;
                    //div2.Visible = true;
                }
                else
                {
                    botonera.Visible = true;
                    divseccion2.Visible = true;
                    divseccion3.Visible = true;
                    ConsultaPermiso();
                }
            }
            else
            {
                if (tablix.Rows.Count > 0)
                {
                    botonera.Visible = true;
                    divseccion2.Visible = true;
                    divseccion3.Visible = true;
                    ConsultaPermiso();
                    string error_consulta = string.Empty;
                    //var areaOnlyControl = credenciales.GetAreaOnlyControl(rucempresa, out error_consulta);
                    //if (!string.IsNullOrEmpty(error_consulta))
                    //{
                    //    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                    //}
                    error_consulta = string.Empty;
                    var dptoOnlyControl = credenciales.GetDptoOnlyControl(rucempresa, out error_consulta);
                    if (!string.IsNullOrEmpty(error_consulta))
                    {
                        Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                    }
                    error_consulta = string.Empty;
                    var cargoOnlyControl = credenciales.GetCargoOnlyControl(rucempresa, out error_consulta);
                    if (!string.IsNullOrEmpty(error_consulta))
                    {
                        Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                    }
                    var turnoOnlyControl = credenciales.GetConsultaTurno();
                    var areaOnlyControl = credenciales.GetConsultaArea();
                    populateDropDownList(ddlAreaOnlyControl, areaOnlyControl, "* Elija *", "AREA_ID", "AREA_NOM", false);
                    populateDropDownList(ddlDepartamentoOnlyControl, dptoOnlyControl, "* Elija *", "DEP_ID", "DEP_NOM", true);
                    populateDropDownList(ddlCargoOnlyControl, cargoOnlyControl, "* Elija *", "CALI_ID", "CALI_NOM", false);
                    populateDropDownList(ddlTurnoOnlyControl, turnoOnlyControl, "* Elija *", "TUR_ID", "TUR_D", false);
                    ddlAreaOnlyControl.SelectedItem.Text = "* Elija *";
                    ddlDepartamentoOnlyControl.SelectedItem.Text = "* Elija *";
                    ddlCargoOnlyControl.SelectedItem.Text = "* Elija *";
                    ddlTurnoOnlyControl.SelectedItem.Text = "* Elija *";
                    var tiempocre = credenciales.GetTiempoCaducidadCredencialPermanente();
                    txtfecing.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtfecsal.Text = DateTime.Now.AddDays(tiempocre).ToString("dd/MM/yyyy");
                    //txtnumfactura.Focus();
                    //                    gvComprobantes.Focus();
                }
                else
                {
                    botonera.Visible = false;
                    divseccion2.Visible = false;
                    divseccion3.Visible = false;
                }
            }
            */

            botonera.Visible = true;
            divseccion2.Visible = true;
            divseccion3.Visible = true;
            ConsultaPermiso();
            string error_consulta = string.Empty;
            //var areaOnlyControl = credenciales.GetAreaOnlyControl(rucempresa, out error_consulta);
            //if (!string.IsNullOrEmpty(error_consulta))
            //{
            //    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
            //}
            error_consulta = string.Empty;
            var dptoOnlyControl = credenciales.GetDptoOnlyControl(rucempresa, out error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
            }
            error_consulta = string.Empty;
            var cargoOnlyControl = credenciales.GetCargoOnlyControl(rucempresa, out error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
            }
            var turnoOnlyControl = aso_transportistas.GetConsultaTurno();
            var areaOnlyControl = aso_transportistas.GetConsultaArea();
            populateDropDownList(ddlAreaOnlyControl, areaOnlyControl, "* Elija *", "AREA_ID", "AREA_NOM", false);
            populateDropDownList(ddlDepartamentoOnlyControl, dptoOnlyControl, "* Elija *", "DEP_ID", "DEP_NOM", true);
            populateDropDownList(ddlCargoOnlyControl, cargoOnlyControl, "* Elija *", "CALI_ID", "CALI_NOM", false);
            populateDropDownList(ddlTurnoOnlyControl, turnoOnlyControl, "* Elija *", "TUR_ID", "TUR_D", false);
            //ddlAreaOnlyControl.SelectedItem.Text = "* Elija *";
            ddlDepartamentoOnlyControl.SelectedItem.Text = "* Elija *";
            ddlCargoOnlyControl.SelectedItem.Text = "* Elija *";
            //ddlTurnoOnlyControl.SelectedItem.Text = "* Elija *";
            var tiempocre = credenciales.GetTiempoCaducidadCredencialPermanente();
            txtfecing.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtfecsal.Text = DateTime.Now.AddDays(tiempocre).ToString("dd/MM/yyyy");
            //txtnumfactura.Focus();
            //                    gvComprobantes.Focus();

            //txtfecing.Text = "";
            //txtfecsal.Text = "";
            txtnumfactura.Text = "";
            //divcabecera.Visible = true;
            xfinderpagado.Visible = true;
            sinresultado.Visible = false;
            dtDetSolicitud = credenciales.GetSolicitudColaboradorCliente(txtsolicitud.Text, rucempresa);
            dtDetSolicitud.Columns.Add("NOMINA_ID");
            dtDetSolicitud.Columns.Add("VAL_HUELLA");
            dtDetSolicitud.Columns.Add("VAL_FOTO");
            dtDetSolicitud.Columns.Add("VAL_CNN");
            dtDetSolicitud.Columns.Add("TIPOD");

            var dtvalestado = aso_transportistas.GetValEstadoSolicitud(txtsolicitud.Text);
            //CryptoOC.clsCryptoOC crypto = new CryptoOC.clsCryptoOC();
            //var cnn = System.Configuration.ConfigurationManager.ConnectionStrings["onlyaccess"].ConnectionString;
            var cnn = ""; //"Provider=SQLOLEDB; Initial Catalog=OnlyAccess; Data Source=cgpre01\\sqlserver2005;User Id=ocaccess;Password=ocaccess;Connect Timeout=120";
            //var encrypt = crypto.Encripta(cnn);
            //var descrypt = crypto.Desencripta(encrypt);
            for (int i = 0; i < dtDetSolicitud.Rows.Count; i++)
            {
                var dtcoduser = aso_transportistas.GetValidaCodigoUsuario(dtDetSolicitud.Rows[i]["RUCCIPAS"].ToString(), dtDetSolicitud.Rows[i]["CIPAS"].ToString());
                if (dtcoduser.Rows.Count > 0)
                {
                    dtDetSolicitud.Rows[i]["NOMINA_ID"] = dtcoduser.Rows[0][0].ToString();
                }
                else
                {
                    dtDetSolicitud.Rows[i]["NOMINA_ID"] = "0";
                }
                dtDetSolicitud.Rows[i]["VAL_CNN"] = cnn;
                /*
                var dtvalhuellafoto = aso_transportistas.GetValHuellaFoto(txtsolicitud.Text, dtDetSolicitud.Rows[i]["CIPAS"].ToString());
                if (dtvalhuellafoto.Rows[0]["HUELLA"].ToString() == "1" && dtvalhuellafoto.Rows[0]["FOTO"].ToString() == "1")
                {
                    dtDetSolicitud.Rows[i]["TIPOD"] = dtDetSolicitud.Rows[i]["TIPO"].ToString() + " - PERMISO TEMPORAL CREADO";
                }
                else
                {
                    dtDetSolicitud.Rows[i]["TIPOD"] = dtDetSolicitud.Rows[i]["TIPO"].ToString();
                }
                */
                var dtvalhuellafoto = aso_transportistas.GetValHuellaFoto(txtsolicitud.Text, dtDetSolicitud.Rows[i]["CIPAS"].ToString());
                if (dtvalhuellafoto.Rows[0]["HUELLA"].ToString() == "1" && dtvalhuellafoto.Rows[0]["FOTO"].ToString() == "1" && dtvalhuellafoto.Rows[0]["BLOQUEO"].ToString() == "0")
                {
                    /*
                    var dtpermiso = aso_transportistas.GetFechasPermisoDeAcceso(dtDetSolicitud.Rows[i]["RUCCIPAS"].ToString(), dtDetSolicitud.Rows[i]["CIPAS"].ToString());
                    if (dtpermiso.Rows.Count > 0)
                    {
                        if (dtpermiso.Rows[0]["FECHAINGRESO"].ToString() == "ACTIVA")
                        {
                            var permiso = "Desde: " + Convert.ToDateTime(dtpermiso.Rows[0]["FECHAINGRESO"]).ToString("dd-MM-yyyy") + " - " +
                                          "Hasta: " + Convert.ToDateTime(dtpermiso.Rows[0]["FECHASALIDA"]).ToString("dd-MM-yyyy");
                            dtDetSolicitud.Rows[i]["PERMISO"] = permiso;
                            dtDetSolicitud.Rows[i]["TIPOD"] = dtDetSolicitud.Rows[i]["TIPO"].ToString() + " - PERMISO TEMPORAL CREADO";
                        }
                        else
                        {
                            dtDetSolicitud.Rows[i]["PERMISO"] = "";
                            dtDetSolicitud.Rows[i]["TIPOD"] = dtDetSolicitud.Rows[i]["TIPO"].ToString();
                        }
                    }
                    */
                    dtDetSolicitud.Rows[i]["TIPOD"] = dtDetSolicitud.Rows[i]["TIPO"].ToString() + " - PERMISO TEMPORAL CREADO";
                }
                else
                {
                    if (dtvalhuellafoto.Rows[0]["BLOQUEO"].ToString() == "1")
                    {
                        //dtDetSolicitud.Rows[i]["PERMISO"] = "BLOQUEADO";
                        dtDetSolicitud.Rows[i]["TIPOD"] = dtDetSolicitud.Rows[i]["TIPO"].ToString() + " - PERMISO TEMPORAL BLOQUEADO";
                    }
                    else
                    {
                        //dtDetSolicitud.Rows[i]["PERMISO"] = "";
                        dtDetSolicitud.Rows[i]["TIPOD"] = dtDetSolicitud.Rows[i]["TIPO"].ToString();
                    }
                }
            }
            rpDetalle.DataSource = dtDetSolicitud;
            rpDetalle.DataBind();
            foreach (RepeaterItem item in rpDetalle.Items)
            {
                CheckBox chkHuellaEstado = item.FindControl("chkHuellaEstado") as CheckBox;
                CheckBox chkFotoEstado = item.FindControl("chkFotoEstado") as CheckBox;
                var dtvalestadohuella = aso_transportistas.GetValidaHuella(dtDetSolicitud.Rows[item.ItemIndex]["RUCCIPAS"].ToString(), dtDetSolicitud.Rows[item.ItemIndex]["CIPAS"].ToString());
                var dtvalestadofoto = aso_transportistas.GetValidaFoto(dtDetSolicitud.Rows[item.ItemIndex]["RUCCIPAS"].ToString(), dtDetSolicitud.Rows[item.ItemIndex]["CIPAS"].ToString());
                if (dtvalestadohuella.Rows.Count > 0)
                {
                    if (dtvalestadohuella.Rows[0][0].ToString() == "1")
                    {
                        chkHuellaEstado.Checked = true;
                        chkHuellaEstado.Text = "HUELLA [OK]";
                    }
                    else
                    {
                        chkHuellaEstado.Checked = false;
                        chkHuellaEstado.Text = "HUELLA [NO]";
                    }
                }
                if (dtvalestadofoto.Rows.Count > 0)
                {
                    if (dtvalestadofoto.Rows[0][0].ToString() == "1")
                    {
                        chkFotoEstado.Checked = true;
                        chkFotoEstado.Text = "FOTO [OK]";
                    }
                    else
                    {
                        chkFotoEstado.Checked = false;
                        chkFotoEstado.Text = "FOTO [NO]";
                    }
                }
            }
        }
        private void populateDropDownList(DropDownList dp, DataTable origen, string mensaje, string id, string descripcion, bool val)
        {
            if (val)
            {
                origen.Rows.Add("0", "0", mensaje);
            }
            else
            {
                //origen.Rows.Add("0", mensaje);
            }
            DataView dvorigen = new DataView();
            dvorigen = origen.DefaultView;
            dvorigen.Sort = descripcion;
            dp.DataSource = dvorigen;
            dp.DataValueField = id;
            dp.DataTextField = descripcion;
            dp.DataBind();
        }
        private void ConsultaPermiso()
        {
            if (credenciales.GetTipoSolicitud(txtsolicitud.Text.Trim()))
            {
                populateDrop(dptipoevento, credenciales.getTipoEventosColaborador("ECC"));
                if (dptipoevento.Items.Count > 0)
                {
                    if (dptipoevento.Items.FindByValue("000") != null)
                    {
                        dptipoevento.Items.FindByValue("000").Selected = true;
                    }
                    dptipoevento.SelectedValue = "0";
                }
            }
            //else
            //{
            //    populateDrop(dptipoevento, credenciales.getTipoEventosColaborador("ECP"));
            //    if (dptipoevento.Items.Count > 0)
            //    {
            //        if (dptipoevento.Items.FindByValue("000") != null)
            //        {
            //            dptipoevento.Items.FindByValue("000").Selected = true;
            //        }
            //        dptipoevento.SelectedValue = "0";
            //    }
            //}
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(txtmotivorechazo.Text))
                {
                    this.Alerta("Tiene un motivo de rechazo.\\nPara continuar limpie la observación en caso rechazo.");
                    txtmotivorechazo.Focus();
                    return;
                }
                Label lblIdSolicitud = new Label(); //gvComprobantes.Rows[0].FindControl("lblIdSolicitud") as Label;
                lblIdSolicitud.Text = txtsolicitud.Text.Trim();
                if (credenciales.GetCTipoCredencial(txtsolicitud.Text.Trim()))
                {
                    if (ddlTurnoOnlyControl.SelectedItem.Text == "* Elija *")
                    {
                        this.Alerta("Elija un turno.");
                        return;
                    }
                }
                Boolean validasolicitud = false;
                var tiposoliciud = credenciales.GetTipoSolicitudXNumSolicitudAsoTrans(txtsolicitud.Text.Trim());
                if (tiposoliciud == "1") //Credencial Permanente y Temporal
                {
                    var GetTipoCredencial = credenciales.GetTipoCredencial(txtsolicitud.Text);
                    if (GetTipoCredencial == "1") //Credencial Permanente
                    {
                        RegistraColaboradoresOnlyControl(GetTipoCredencial, out validasolicitud);
                        if (!validasolicitud)
                        {
                            Response.Write("<script language='JavaScript'>var r=alert('Registro de huella, foto y permiso de acceso temporal procesado exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                            /*
                            RegistraPermisoPermanenteOnlyControl(out validasolicitud);
                            if (!validasolicitud)
                            {
                                AddPagoConfirmado();
                            }
                            */
                        }
                    }
                        /*
                    else if (GetTipoCredencial == "2") //Credencial Temporal
                    {
                        RegistraColaboradoresOnlyControl(GetTipoCredencial, out validasolicitud);
                        if (!validasolicitud)
                        {
                            AddPagoConfirmado();
                        }
                    }
                    */
                }
                //else if (tiposoliciud == "2") //Sticker Vehicular
                //{
                //    RegistraVehiculosOnlyControl(out validasolicitud);
                //    if (!validasolicitud)
                //    {
                //        AddPagoConfirmado();
                //    }
                //}
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "btsalvar_Click", "1", Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }
        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtmotivorechazo.Text))
                {
                    this.Alerta("Escriba el motivo de rechazo.");
                    txtmotivorechazo.Focus();
                    return;
                }
                //Label lblIdSolicitud = gvComprobantes.Rows[0].FindControl("lblIdSolicitud") as Label;
                //txtsolicitud.Text = lblIdSolicitud.Text;
                string nombreempresa = CslHelper.getShiperName(rucempresa);
                string mensaje = null;
                if (!credenciales.RechazaSolicitudConfirmacionDePago(
                    txtsolicitud.Text,
                    rucempresa,
                    nombreempresa,
                    useremail,
                    txtmotivorechazo.Text,
                    Page.User.Identity.Name,
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago rechazada exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                    //txtmotivorechazo.Text = "";
                    //ConsultaInformacion();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
        private void AddPagoConfirmado()
        {
            string mensaje = null;
            String xmlDocumentos = string.Empty;
            if (!aso_transportistas.AddPagoConfirmado(
                        txtsolicitud.Text.Trim(),
                        CslHelper.getShiperName(rucempresa),
                        rucempresa,
                        useremail,
                        Page.User.Identity.Name,
                        txtnumfactura.Text.Trim(),
                        txtfecing.Text,
                        txtfecsal.Text,
                        ddlAreaOnlyControl.SelectedItem.Text,
                        ddlDepartamentoOnlyControl.SelectedItem.Text,
                        ddlTurnoOnlyControl.SelectedItem.Text,
                        out mensaje))
            {
                this.Alerta(mensaje);
            }
            else
            {
                Response.Write("<script language='JavaScript'>var r=alert('Registro de huella, foto y permiso de acceso temporal procesado exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                //ConsultaInformacion();
                //Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago enviado exitosamente, en unos minutos recibirá una notificación via mail.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>");
            }
        }
        private void RegistraVehiculosOnlyControl(out bool validavehiculos)
        {
            validavehiculos = false;
            String errorvehiculo = string.Empty;
            List<string> listPlacas = new List<string>();
            DataTable dtAprobados = new DataTable();
            Int32 registros_actualizados_correcto = 0;
            Int32 registros_actualizados_incorrecto = 0;
            String error_consulta = string.Empty;
            String error = string.Empty;
            DataSet dsVehiculos = new DataSet();
            DataSet dsErrorAC_R_VEHICULOS = new DataSet();
            DataSet dsErrorAC_C_VEHICULOS = new DataSet();
            DataTable dtRegistraVehiculos = new DataTable();
            DataTable dtVehiculosAprobados = new DataTable();
            dtVehiculosAprobados.Columns.Add("PLACA");
            dtVehiculosAprobados.Columns.Add("TIPO");
            dtVehiculosAprobados.Columns.Add("MARCA");
            dtVehiculosAprobados.Columns.Add("MODELO");
            dtVehiculosAprobados.Columns.Add("EMPRESA");
            dtVehiculosAprobados.Columns.Add("COLOR");
            dtVehiculosAprobados.Columns.Add("PROPIETARIO");
            dtVehiculosAprobados.Columns.Add("CED_PROPIETARIO");
            dtVehiculosAprobados.Columns.Add("TIPO_CERTIFICADO");
            dtVehiculosAprobados.Columns.Add("CERTIFICADO");
            dtVehiculosAprobados.Columns.Add("CATEGORIA");
            dtVehiculosAprobados.Columns.Add("FECHA_POLIZA");
            dtVehiculosAprobados.Columns.Add("FECHA_MTOP");
            dtAprobados = credenciales.GetSolicitudVehiculoOnlyControl(txtsolicitud.Text.Trim());
            for (int i = 0; i < dtAprobados.Rows.Count; i++)
            {
                string fechapoliza = string.Empty;
                string fechamtop = string.Empty;
                if (dtAprobados.Rows[i]["CATEGORIA"].ToString().Trim() == "LIVIANO")
                {
                    fechapoliza = DateTime.Now.ToString("yyyy-MM-dd");
                    fechamtop =  DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    fechapoliza = Convert.ToDateTime(dtAprobados.Rows[i][11].ToString()).ToString("yyyy-MM-dd");
                    fechamtop = Convert.ToDateTime(dtAprobados.Rows[i][12].ToString()).ToString("yyyy-MM-dd");
                }
                var empresa = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraVehiculosOnlyControl", "1", Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    validavehiculos = true;
                    return;
                }
                dtVehiculosAprobados.Rows.Add(
                dtAprobados.Rows[i][7].ToString(),
                dtAprobados.Rows[i][3].ToString(),
                dtAprobados.Rows[i][4].ToString(),
                dtAprobados.Rows[i][5].ToString(),
                empresa,
                dtAprobados.Rows[i][6].ToString(),
                "", "",
                dtAprobados.Rows[i][8].ToString(),
                dtAprobados.Rows[i][9].ToString(),
                dtAprobados.Rows[i][10].ToString(),
                fechapoliza,
                fechamtop);
            }
            for (int i = 0; i < dtVehiculosAprobados.Rows.Count; i++)
            {
                dsErrorAC_C_VEHICULOS = onlyControl.AC_C_VEHICULOS(dtVehiculosAprobados.Rows[i][0].ToString(), 0, ref error_consulta) as DataSet;
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    if (string.IsNullOrEmpty(error_consulta))
                    {
                        var dserror = dsErrorAC_C_VEHICULOS.DefaultViewManager.DataSet.Tables;
                        var dterror = dserror[0];
                        var error2 = string.Empty;
                        for (int i2 = 0; i2 < dterror.Rows.Count; i2++)
                        {
                            error2 = error2 + " \\n *" +
                                   "Error: " + dterror.Rows[i2]["ERROR"].ToString() + " \\n ";
                        }
                        var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_VEHICULOS():{0}", error2));
                        var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                        validavehiculos = true;
                        return;
                    }
                    var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_VEHICULOS():{0}", error_consulta));
                    var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                    validavehiculos = true;
                    return;
                }
                DataTable dt = dsErrorAC_C_VEHICULOS.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    var resultemp = from myRow in dtVehiculosAprobados.AsEnumerable()
                                    where myRow.Field<string>("PLACA") == dtVehiculosAprobados.Rows[i][0].ToString()
                                    select myRow;
                    DataTable dttemp = resultemp.AsDataView().ToTable();
                    dttemp.Rows[0][4] = CslHelper.getShiperName(rucempresa);
                    dsVehiculos = new DataSet();
                    dsVehiculos.Tables.Add(dttemp);
                    dsErrorAC_R_VEHICULOS = onlyControl.AC_R_VEHICULOS(dsVehiculos, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                    if (registros_actualizados_incorrecto > 0)
                    {
                        if (string.IsNullOrEmpty(error_consulta))
                        {
                            var dserror = dsErrorAC_R_VEHICULOS.DefaultViewManager.DataSet.Tables;
                            var dterror = dserror[0];
                            var error2 = string.Empty;
                            for (int i2 = 0; i2 < dterror.Rows.Count; i2++)
                            {
                                error2 = error2 + " \\n *" +
                                       "Error: " + dterror.Rows[i2]["ERROR"].ToString() + " \\n ";
                            }
                            var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_VEHICULOS():{0}", error2));
                            var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                            this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                            validavehiculos = true;
                            return;
                        }
                        var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_VEHICULOS():{0}", error_consulta));
                        var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                        validavehiculos = true;
                        return;
                    }
                    else if (!string.IsNullOrEmpty(error_consulta))
                    {
                        var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_VEHICULOS():{0}", error_consulta));
                        var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                        validavehiculos = true;
                        return;
                    }
                    DataTable dt2 = new DataTable();
                    dt2 = dsErrorAC_R_VEHICULOS.Tables[0];
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i2 = 0; i2 < dt2.Rows.Count; i2++)
                        {
                            errorvehiculo = errorvehiculo + " \\n *" +
                                            "Placa: " + dt2.Rows[i2][0].ToString() + " - " +
                                            "Error: " + dt2.Rows[i2][8].ToString() + " \\n ";
                            //var t = credenciales.SaveLog(errorvehiculo, "credenciales", "onlyControl.AC_R_VEHICULOS()", DateTime.Now.ToShortDateString(), "sistema");
                        }
                        var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_VEHICULOS():{0}", errorvehiculo));
                        var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                        validavehiculos = true;
                        return;
                    }
                }
            }
            if (!string.IsNullOrEmpty(errorvehiculo))
            {
                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_VEHICULOS():{0}", errorvehiculo));
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                validavehiculos = true;
                return;
            }
        }
        private void RegistraColaboradoresOnlyControl(string tipocredencial, out bool validacolaboradores)
        {
            validacolaboradores = false;
            String errorvehiculo = string.Empty;
            DataTable dtAprobados = new DataTable();
            List<string> listCedulas = new List<string>();
            Int32 registros_actualizados_correcto = 0;
            Int32 registros_actualizados_incorrecto = 0;
            String error_consulta = string.Empty;
            String error = string.Empty;
            DataSet dsColaboradores = new DataSet();
            DataSet dsErrorAC_R_PERSONA_PEATON = new DataSet();
            DataTable dtColaboradoresAprobados = new DataTable();
            dtColaboradoresAprobados.Columns.Add("CEDULA");
            dtColaboradoresAprobados.Columns.Add("APELLIDOS");
            dtColaboradoresAprobados.Columns.Add("NOMBRES");
            dtColaboradoresAprobados.Columns.Add("EMPRESA");
            dtColaboradoresAprobados.Columns.Add("AREA");
            dtColaboradoresAprobados.Columns.Add("DPTO");
            dtColaboradoresAprobados.Columns.Add("CARGO");
            dtColaboradoresAprobados.Columns.Add("EXPIRACION");
            dtColaboradoresAprobados.Columns.Add("TIPO_SANGRE");
            dtColaboradoresAprobados.Columns.Add("TIPO_LICENCIA");
            dtColaboradoresAprobados.Columns.Add("FECHA_INGRESO");
            dtColaboradoresAprobados.Columns.Add("FECHA_CADUCIDAD");
            //var empresa = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
            //if (!string.IsNullOrEmpty(error_consulta))
            //{
            //    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
            //    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraColaboradoresOnlyControl", "1", Request.UserHostAddress);
            //    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            //    validacolaboradores = true;
            //    return;
            //}
            var empresa = aso_transportistas.GetEmpresaNomAC(dtDetSolicitud.Rows[0]["RUCCIPAS"].ToString()).Rows[0]["EMPE_NOM"].ToString(); //credenciales.GetEmpresaColaborador(txtsolicitud.Text.Trim()).Rows[0]["RAZONSOCIAL"].ToString();
            dtAprobados = credenciales.GetSolicitudColaboradorOnlyControl(txtsolicitud.Text.Trim());
            dtAprobados.Columns.Add("CREA_PERMISO");
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime fechaing;
            if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaing))
            {
                this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", txtfecing.Text));
                txtfecing.Focus();
                validacolaboradores = true;
                return;
            }
            DateTime fechacad;
            if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacad))
            {
                this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", txtfecsal.Text));
                txtfecsal.Focus();
                validacolaboradores = true;
                return;
            }

            DataTable DTRESULT = new DataTable();
            DTRESULT = (DataTable)HttpContext.Current.Session["dtDetSolicitud"];
            for (int i = 0; i < DTRESULT.Rows.Count; i++)
            {
                if (DTRESULT.Rows[i]["VAL_HUELLA"].ToString() == "1" && DTRESULT.Rows[i]["VAL_FOTO"].ToString() == "1")
                {
                    dtAprobados.Rows[i]["CREA_PERMISO"] = "1";
                }
                else
                {
                    dtAprobados.Rows[i]["CREA_PERMISO"] = "0";
                }
            }
            dtAprobados = dtAprobados.Select("CREA_PERMISO = 1").Count() == 0 ? new DataTable() : dtAprobados.Select("CREA_PERMISO = 1").CopyToDataTable();

            for (int i = 0; i < dtAprobados.Rows.Count; i++)
            {
                var errorregistro = string.Empty;
                error_consulta = string.Empty;
                DataTable dtGetCargoOnlyControl = credenciales.GetCargoOnlyControl(empresa, out error_consulta);
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                }
                String CargoOnlyControl = string.Empty;
                var resultsCargo = from myRow in dtGetCargoOnlyControl.AsEnumerable()
                                   where myRow.Field<string>("CALI_NOM") == dtAprobados.Rows[i][11].ToString()
                                   select myRow;
                if (resultsCargo.AsDataView().Count > 0)
                {
                    CargoOnlyControl = dtAprobados.Rows[i][11].ToString();
                }
                else
                {
                    errorregistro = string.Empty;
                    credenciales.GetRegistraCargoOnlyControl(dtAprobados.Rows[i][11].ToString(), out errorregistro);
                    if (!string.IsNullOrEmpty(errorregistro))
                    {
                        var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_CARGO():{0}", error_consulta));
                        var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraColaboradoresOnlyControl", "1", Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                        validacolaboradores = true;
                        return;
                    }
                    CargoOnlyControl = dtAprobados.Rows[i][11].ToString();
                }
                TimeSpan tsDias = fechacad - fechaing;
                int diferenciaEnDias = tsDias.Days;
                if (diferenciaEnDias < 0)
                {
                    Label lblTipoSolicitud = gvComprobantes.Rows[0].FindControl("lblTipoSolicitud") as Label;
                    this.Alerta("La Fecha de Ingreso: " + txtfecing.Text + "\\nNO deber ser mayor a la\\nFecha de Caducidad: " + txtfecsal.Text);
                    validacolaboradores = true;
                    return;
                }

                dtColaboradoresAprobados.Rows.Add(
                    dtAprobados.Rows[i][2].ToString(),
                    dtAprobados.Rows[i]["APELLIDOS"].ToString(),
                    dtAprobados.Rows[i]["NOMBRES"].ToString(),
                    empresa,
                    ddlAreaOnlyControl.SelectedItem.Value,
                    ddlAreaOnlyControl.SelectedItem.Value,
                    CargoOnlyControl,
                    (string.IsNullOrEmpty(dtAprobados.Rows[i][12].ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : Convert.ToDateTime(dtAprobados.Rows[i][12].ToString()).ToString("yyyy-MM-dd")), //expiracion
                    dtAprobados.Rows[i][4].ToString(),
                    dtAprobados.Rows[i][10].ToString(),
                    fechaing.ToString("yyyy-MM-dd"),
                    fechacad.ToString("yyyy-MM-dd")
                    );
            }

            if (dtAprobados.Rows.Count == 0)
            {
                this.Alerta("No se encontraron datos para procesar.");
                txtfecsal.Focus();
                validacolaboradores = true;
                return;
            }

            dsColaboradores = new DataSet();
            dsColaboradores.Tables.Add(dtColaboradoresAprobados);
            //Nomina, Sistema Azul
            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_NOMINA(dsColaboradores, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_NOMINA(dsColaboradores, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            if (registros_actualizados_incorrecto > 0)
            {
                if (string.IsNullOrEmpty(error_consulta))
                {
                    var dserror = dsErrorAC_R_PERSONA_PEATON.DefaultViewManager.DataSet.Tables;
                    var dterror = dserror[0];
                    var error2 = string.Empty;
                    for (int i2 = 0; i2 < dterror.Rows.Count; i2++)
                    {
                        error2 = error2 + " \\n *" +
                               "Error: " + dterror.Rows[i2]["ERROR"].ToString() + " \\n ";
                    }
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERSONA_NOMINA():{0}", error2));
                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraColaboradoresOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    validacolaboradores = true;
                    return;
                }
                var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERSONA_NOMINA():{0}", error_consulta));
                var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "RegistraColaboradoresOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                validacolaboradores = true;
                return;
            }
            DataTable dtCargos = dsColaboradores.Tables[0];
            string mensaje = string.Empty;
            for (int i = 0; i < dtCargos.Rows.Count; i++)
            {
                if (dtCargos.Rows[i]["CARGO"].ToString().Trim() == "CONDUCTOR")
                {
                    if (!credenciales.UpdateCargoConductor(dtCargos.Rows[i]["CEDULA"].ToString().Trim(), out mensaje))
                    {
                        this.Alerta(mensaje);
                        validacolaboradores = true;
                        return;
                    }
                    else
                    {
                        if (!aso_transportistas.AddSolicitudColaboradorFotoHuella(dtCargos.Rows[i]["CEDULA"].ToString().Trim(), txtsolicitud.Text.Trim(), Page.User.Identity.Name, out mensaje))
                        {
                            this.Alerta(mensaje);
                            validacolaboradores = true;
                            return;
                        }
                        //Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago enviado exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                    }
                }
            }
        }
        private void RegistraPermisoPermanenteOnlyControl(out bool validacolaboradores)
        {
            DataTable dtDocumentos = new DataTable();
            validacolaboradores = false;
            //if (dptipoevento.SelectedItem.ToString() == "PERMANENTE")
            //{
                //banderapermiso = true;
                Int32 registros_actualizados_correcto = 0;
                Int32 registros_actualizados_incorrecto = 0;
                String error_consulta = string.Empty;
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fecing;
                if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecing))
                {
                    //this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecing.Text));
                    //txtfecing.Focus();
                    //validacolaboradores = true;
                    //return;
                }
                DateTime facsal;
                if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out facsal))
                {
                    //this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecsal.Text));
                    //txtfecsal.Focus();
                    //validacolaboradores = true;
                    //return;
                }
                //var empresa = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
                //if (!string.IsNullOrEmpty(error_consulta))
                //{
                //    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                //    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                //    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                //    validacolaboradores = true;
                //    return;
                //}
                var empresa = credenciales.GetEmpresaColaborador(txtsolicitud.Text.Trim()).Rows[0]["RAZONSOCIAL"].ToString();
                var dsPersonasOnlyControl = onlyControl.AC_C_PERSONA(empresa, string.Empty, 1, ref error_consulta);
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", error_consulta));
                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    validacolaboradores = true;
                    return;
                }
                //var numsolicitud = gvComprobantes.Rows[0].Cells[0].Text;
                Label numsolicitud = new Label(); //gvComprobantes.Rows[0].FindControl("lblIdSolicitud") as Label;
                numsolicitud.Text = txtsolicitud.Text.Trim();
                var dtPermisosDeAcceso = credenciales.GetSolicitudColaboradorOnlyControl(numsolicitud.Text);
                dtPermisosDeAcceso.Columns.Add("CREA_PERMISO");

                DataTable dtPermiso = new DataTable();
                dtPermiso.Columns.Add("ID");
                dtPermiso.Columns.Add("F_INGRESO");
                dtPermiso.Columns.Add("F_SALIDA");
                dtPermiso.Columns.Add("HORARIO");
                dtPermiso.Columns.Add("TIPO_CONTROL");
                
                DataTable DTRESULT = new DataTable();
                DTRESULT = (DataTable)HttpContext.Current.Session["dtDetSolicitud"];
                for (int i = 0; i < DTRESULT.Rows.Count; i++)
                {
                    if (DTRESULT.Rows[i]["VAL_HUELLA"].ToString() == "1" && DTRESULT.Rows[i]["VAL_FOTO"].ToString() == "1")
                    {
                        dtPermisosDeAcceso.Rows[i]["CREA_PERMISO"] = "1";
                    }
                    else
                    {
                        dtPermisosDeAcceso.Rows[i]["CREA_PERMISO"] = "0";
                    }
                }
                dtPermisosDeAcceso = dtPermisosDeAcceso.Select("CREA_PERMISO = 1").Count() == 0 ? new DataTable() : dtPermisosDeAcceso.Select("CREA_PERMISO = 1").CopyToDataTable();
                //List<string> listPersonasCsa = new List<string>();
                //for (int i = 0; i < dtPermisosDeAcceso.Rows.Count; i++)
                //{
                //    listPersonasCsa.Add(dtPermisosDeAcceso.Rows[i]["CEDULA"].ToString());
                //}
                for (int i = 0; i < dtPermisosDeAcceso.Rows.Count; i++)
                {
                    var resultPersonasOnlyControl = from myRow in dsPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0].AsEnumerable()
                                                    where myRow.Field<string>("CEDULA") == dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString()
                                                    select myRow;
                    DataTable dt = resultPersonasOnlyControl.AsDataView().ToTable();
                    if (resultPersonasOnlyControl.AsDataView().ToTable().Rows.Count == 0)
                    {
                        var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", "No se encontro el colaborador: " + dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString()));
                        var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                        validacolaboradores = true;
                        return;
                    }
                    var id = resultPersonasOnlyControl.AsDataView().ToTable().Rows[0]["ID"];
                    //DateTime fecingreso = new DateTime();
                    //DateTime feccaducidad = new DateTime();
                    //fecingreso = Convert.ToDateTime(dtPermisosDeAcceso.Rows[i]["FECHAINGRESO"]);
                    //feccaducidad = Convert.ToDateTime(dtPermisosDeAcceso.Rows[i]["FECHACADUCIDAD"]);
                    dtPermiso.Rows.Add(id, fecing.ToString("yyyy-MM-dd"), facsal.ToString("yyyy-MM-dd"), ddlTurnoOnlyControl.SelectedValue, "2");
                    //dtPermisosDeAcceso.Rows.Add(id, fecing.ToString("yyyy-MM-dd"), facsal.ToString("yyyy-MM-dd"));
                }
                //dtPermisosDeAcceso.Rows[i]["ID"] == resultPersonasOnlyControl.AsDataView().Table.Rows[i]["ID"].ToString();
                //var resultPersonasOnlyControl = from myRow in dtPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0].AsEnumerable()
                //                                where listPersonasCsa.Contains(myRow.Field<string>("CEDULA"))
                //                                select myRow;
                //DataTable dtPermiso = new DataTable();
                //dtPermiso.Columns.Add("ID");
                //dtPermiso.Columns.Add("F_INGRESO");
                //dtPermiso.Columns.Add("F_SALIDA");
                //for (int i = 0; i < resultPersonasOnlyControl.AsDataView().ToTable().Rows.Count; i++)
                //{
                //    dtPermiso.Rows.Add(resultPersonasOnlyControl.AsDataView().ToTable().Rows[i][0], fecing.ToString("yyyy-MM-dd"), facsal.ToString("yyyy-MM-dd"));
                //}
                DataSet dsPermiso = new DataSet();
                DataSet dsErrorAC_R_PERMISO = new DataSet();
                dsPermiso.Tables.Add(dtPermiso);
                dsErrorAC_R_PERMISO = onlyControl.AC_R_PERMISO(dsPermiso, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (dsErrorAC_R_PERMISO.Tables[0].Rows.Count != 0)
                {
                    //if (!string.IsNullOrEmpty(dsErrorAC_R_PERMISO.Tables[0].Rows[0]["ERROR"].ToString()))
                    //{
                        if (registros_actualizados_incorrecto > 0)
                        {
                            if (string.IsNullOrEmpty(error_consulta))
                            {
                                var dserror = dsErrorAC_R_PERMISO.DefaultViewManager.DataSet.Tables;
                                var dterror = dserror[0];
                                var error = string.Empty;
                                for (int i = 0; i < dterror.Rows.Count; i++)
                                {
                                    error = error + " \\n *" +
                                           "Error: " + dterror.Rows[0]["ERROR"].ToString() + " \\n ";
                                }
                                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERMISO():{0}", error));
                                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                                validacolaboradores = true;
                                return;
                            }
                            var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERMISO():{0}", "{" + error_consulta + "}"));
                            var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                            this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                            validacolaboradores = true;
                            return;
                        }
                        //else if (!string.IsNullOrEmpty(error_consulta))
                        //{
                        //    var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERMISO():{0}", error_consulta));
                        //    var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                        //    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                        //    validacolaboradores = true;
                        //    return;
                        //}
                   // }
                }
                //Boolean validasolicitud = false;
                //ActualizaFechasIngresCaducidadColaboradorOnlyControl(empresa, dtPermisosDeAcceso, out validasolicitud);
                //if (!validasolicitud)
                //{
                    
                //}
            //}
        }
        private void ActualizaFechasIngresCaducidadColaboradorOnlyControl(string empresa, DataTable dt, out bool validacolaboradores)
        {
            validacolaboradores = false;
            Int32 registros_actualizados_correcto = 0;
            Int32 registros_actualizados_incorrecto = 0;
            String error_consulta = string.Empty;
            DataTable dtColaborador = new DataTable();
            dtColaborador.Columns.Add("CEDULA");
            dtColaborador.Columns.Add("APELLIDOS");
            dtColaborador.Columns.Add("NOMBRES");
            dtColaborador.Columns.Add("EMPRESA");
            dtColaborador.Columns.Add("AREA");
            dtColaborador.Columns.Add("DPTO");
            dtColaborador.Columns.Add("CARGO");
            dtColaborador.Columns.Add("EXPIRACION");
            dtColaborador.Columns.Add("TIPO_SANGRE");
            dtColaborador.Columns.Add("TIPO_LICENCIA");
            dtColaborador.Columns.Add("FECHA_INGRESO");
            dtColaborador.Columns.Add("FECHA_CADUCIDAD");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime fecingreso = new DateTime();
                DateTime feccaducidad = new DateTime();
                fecingreso = Convert.ToDateTime(dt.Rows[i]["FECHAINGRESO"]);
                feccaducidad = Convert.ToDateTime(dt.Rows[i]["FECHACADUCIDAD"]);
                dtColaborador.Rows.Add(dt.Rows[i]["CEDULA"], "", "", empresa, "", "", "", "", "", "", fecingreso.ToString("yyyy-MM-dd"), feccaducidad.ToString("yyyy-MM-dd"));
            }
            DataSet dsColaborador = new DataSet();
            DataSet dsErrorAC_R_PERMISO_NOMINA = new DataSet();
            dsColaborador.Tables.Add(dtColaborador);
            dsErrorAC_R_PERMISO_NOMINA = onlyControl.AC_R_PERSONA_NOMINA(dsColaborador, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            if (registros_actualizados_incorrecto > 0)
            {
                if (string.IsNullOrEmpty(error_consulta))
                {
                    var dserror = dsErrorAC_R_PERMISO_NOMINA.DefaultViewManager.DataSet.Tables;
                    var dterror = dserror[0];
                    var error = string.Empty;
                    for (int i = 0; i < dterror.Rows.Count; i++)
                    {
                        error = error + " \\n *" +
                               "Error: " + dterror.Rows[0]["ERROR"].ToString() + " \\n ";
                    }
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERSONA_NOMINA():{0}", error));
                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "ActualizaFechasIngresCaducidadColaboradorOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    validacolaboradores = true;
                    return;
                }
                var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERSONA_NOMINA():{0}", error_consulta));
                var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "ActualizaFechasIngresCaducidadColaboradorOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                validacolaboradores = true;
                return;
            }
            else if (!string.IsNullOrEmpty(error_consulta))
            {
                var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERSONA_NOMINA():{0}", error_consulta));
                var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "ActualizaFechasIngresCaducidadColaboradorOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                validacolaboradores = true;
                return;
            }
        }
        protected void dptipoevento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dptipoevento.SelectedItem.ToString() == "PERMANENTE")
            {
                txtfecing.Enabled = true;
                txtfecsal.Enabled = true;
                txtfecing.BackColor = System.Drawing.Color.White;
                txtfecsal.BackColor = System.Drawing.Color.White;
            }
            else
            {
                txtfecing.Enabled = false;
                txtfecsal.Enabled = false;
                txtfecing.BackColor = System.Drawing.Color.Silver;
                txtfecsal.BackColor = System.Drawing.Color.Silver;
            }
        }
        protected void btHuella_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the reference of the clicked button.
                Button button = (sender as Button);
                //Get the command argument
                string commandArgument = button.CommandArgument;
                //Get the Repeater Item reference
                RepeaterItem item = button.NamingContainer as RepeaterItem;
                //Get the repeater item index
                int index = item.ItemIndex;

                string value = commandArgument.Trim();
                Char delimiter = ',';
                List<string> substringhuella = value.Split(delimiter).ToList();
                var idnomina = substringhuella[0].Trim();
                var conductor = substringhuella[1].Trim() + " - " + substringhuella[2].Trim();

                Boolean validasolicitud = false;
                var tiposoliciud = credenciales.GetTipoSolicitudXNumSolicitudAsoTrans(txtsolicitud.Text.Trim());
                if (tiposoliciud == "1") //Credencial Permanente y Temporal
                {
                    var GetTipoCredencial = credenciales.GetTipoCredencial(txtsolicitud.Text);
                    if (GetTipoCredencial == "1") //Credencial Permanente
                    {
                        for (int i = 0; i < dtDetSolicitud.Rows.Count; i++)
                        {
                            if (dtDetSolicitud.Rows[i]["CIPAS"].ToString() == substringhuella[1].Trim() && dtDetSolicitud.Rows[i]["NOMINA_ID"].ToString() == "0")
                            {
                                RegistraColaboradoresOnlyControl(GetTipoCredencial, out validasolicitud);
                                if (!validasolicitud)
                                {
                                    var dtcoduser = aso_transportistas.GetValidaCodigoUsuario(dtDetSolicitud.Rows[i]["RUCCIPAS"].ToString(), dtDetSolicitud.Rows[i]["CIPAS"].ToString());
                                    if (dtcoduser.Rows.Count > 0)
                                    {
                                        dtDetSolicitud.Rows[i]["NOMINA_ID"] = dtcoduser.Rows[0][0].ToString();
                                    }
                                    else
                                    {
                                        dtDetSolicitud.Rows[i]["NOMINA_ID"] = "0";
                                    }

                                    string idUser = idnomina.Trim();

                                    string cnn = "PROVIDER=MSDASQL;dsn=ONLY_CONTROL;uid=ocaccess;pwd=ocaccess;";

                                    string Script = "CallHuella('" + idUser + "', '" + cnn + "'); validaHuella('" + idUser + "', '" + txtsolicitud.Text.Trim() + "');";

                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", Script, true);
                                }
                            }
                            else
                            {
                                string idUser = idnomina.Trim();

                                string cnn = "PROVIDER=MSDASQL;dsn=ONLY_CONTROL;uid=ocaccess;pwd=ocaccess;";

                                string Script = "CallHuella('" + idUser + "', '" + cnn + "'); validaHuella('" + idUser + "', '" + txtsolicitud.Text.Trim() + "');";

                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", Script, true);
                            }
                        }
                    }
                }
                else
                {
                    this.Alerta("No se puede capturar la Huella del conductor:\\n" + conductor + "\\nNumero de Solicitud: " + txtsolicitud.Text.Trim() + ", no es Tipo Permanente.");
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "consulta_solicitud_colaborador", "btHuella_Click", "1", Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }
        protected void btFoto_Click(object sender, EventArgs e)
        {
            try
            {
                //Get the reference of the clicked button.
                Button button = (sender as Button);
                //Get the command argument
                string commandArgument = button.CommandArgument;
                //Get the Repeater Item reference
                RepeaterItem item = button.NamingContainer as RepeaterItem;
                //Get the repeater item index
                int index = item.ItemIndex;

                string value = commandArgument.Trim();
                Char delimiter = ',';
                List<string> substringfoto = value.Split(delimiter).ToList();
                var idnomina = substringfoto[0].Trim();
                var conductor = substringfoto[1].Trim() + " - " + substringfoto[2].Trim();

                var tiposoliciud = credenciales.GetTipoSolicitudXNumSolicitudAsoTrans(txtsolicitud.Text.Trim());
                if (tiposoliciud == "1") //Credencial Permanente y Temporal
                {
                    for (int i = 0; i < dtDetSolicitud.Rows.Count; i++)
                    {
                        if (dtDetSolicitud.Rows[i]["CIPAS"].ToString() == substringfoto[1].Trim() && dtDetSolicitud.Rows[i]["NOMINA_ID"].ToString() != "0")
                        {

                            string idUser = idnomina.Trim();

                            string cnn = "PROVIDER=MSDASQL;dsn=ONLY_CONTROL;uid=ocaccess;pwd=ocaccess;";

                            string Script = "CallCamara('" + idUser + "', '" + cnn + "'); validaFoto('" + idUser + "');";

                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", Script, true);
                        }
                        else
                        {
                            this.Alerta("No se puede capturar la Foto del conductor:\\n" + conductor + "\\nHasta que capture la Huella.");
                        }
                    }
                }
                else
                {
                    this.Alerta("No se puede capturar la Foto del conductor:\\n" + conductor + "\\nNumero de Solicitud: " + txtsolicitud.Text.Trim() + ", no es Tipo Permanente.");
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "consulta_solicitud_colaborador", "btFoto_Click", "1", Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }
        [WebMethod(EnableSession = true)]
        public static string IsAvailableHuella(string user)
        {
            try
            {
                string mensajeerror = "0";
                DataTable DTRESULT = new DataTable();
                DTRESULT = (DataTable)HttpContext.Current.Session["dtDetSolicitud"];
                for (int i = 0; i < DTRESULT.Rows.Count; i++)
                {
                    if (DTRESULT.Rows[i]["CIPAS"].ToString() == user)
                    {
                        DTRESULT.Rows[i]["VAL_HUELLA"] = "1";
                        mensajeerror = "1";
                    }
                    else
                    {
                        DTRESULT.Rows[i]["VAL_HUELLA"] = "0";
                        //mensajeerror = "0";
                    }
                }
                return mensajeerror;
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "consulta_solicitud_colaborador", "IsAvailableHuella", "1", DateTime.Now.ToString());
                var mensajeerror = (string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                return mensajeerror;
            }
            //HttpContext.Current.Session["dtDetSolicitud"] = DTRESULT;
            //var dtresutado = (DataTable)HttpContext.Current.Session["dtDetSolicitud"];
        }
        [WebMethod(EnableSession = true)]
        public static string IsAvailableFoto(string user)
        {
            try
            {
                string mensajeerror = "0";
                DataTable DTRESULT = new DataTable();
                DTRESULT = (DataTable)HttpContext.Current.Session["dtDetSolicitud"];
                for (int i = 0; i < DTRESULT.Rows.Count; i++)
                {
                    if (DTRESULT.Rows[i]["CIPAS"].ToString() == user)
                    {
                        DTRESULT.Rows[i]["VAL_FOTO"] = "1";
                        mensajeerror = "1";
                    }
                    else
                    {
                        DTRESULT.Rows[i]["VAL_FOTO"] = "0";
                        //mensajeerror = "0";
                    }
                }
                return mensajeerror;
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "consulta_solicitud_colaborador", "IsAvailableHuella", "1", DateTime.Now.ToString());
                var mensajeerror = (string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                return mensajeerror;
            }
            //HttpContext.Current.Session["dtDetSolicitud"] = DTRESULT;
            //var dtresutado = (DataTable)HttpContext.Current.Session["dtDetSolicitud"];
        }
        [WebMethod(EnableSession = true)]
        public static string IsAvailableRAC(string user, string numsolicitud, string fechaingreso, string fechasalida, string ddlAreaOnlyControl, string cedula, string conductor, string ruc)
        {
            string mensajeerror = string.Empty;
            bool validacolaboradores = false;

            var dtvalhuellafoto = aso_transportistas.GetValHuellaFoto(numsolicitud, cedula);
            if (dtvalhuellafoto.Rows.Count <= 0)
            {
                mensajeerror = "Hubo un problema al consultar la huella y foto del conductor: \n" + conductor;
                validacolaboradores = true;
                return mensajeerror;
            }
            if (dtvalhuellafoto.Rows[0]["HUELLA"].ToString() == "1" && dtvalhuellafoto.Rows[0]["FOTO"].ToString() == "1")
            {
                mensajeerror = "El conductor: \n" + conductor + "\nYa tiene regsitrada una huella.";
                validacolaboradores = true;
                return mensajeerror;
            }

            String errorvehiculo = string.Empty;
            DataTable dtAprobados = new DataTable();
            List<string> listCedulas = new List<string>();
            Int32 registros_actualizados_correcto = 0;
            Int32 registros_actualizados_incorrecto = 0;
            String error_consulta = string.Empty;
            String error = string.Empty;
            DataSet dsColaboradores = new DataSet();
            DataSet dsErrorAC_R_PERSONA_PEATON = new DataSet();
            DataTable dtColaboradoresAprobados = new DataTable();
            DataTable DTRESULT = new DataTable();
            DTRESULT = (DataTable)HttpContext.Current.Session["dtDetSolicitud"];
            Boolean validasolicitud = false;
            var tiposoliciud = credenciales.GetTipoSolicitudXNumSolicitudAsoTrans(numsolicitud.Trim());
            if (tiposoliciud == "1") //Credencial Permanente y Temporal
            {
                var GetTipoCredencial = credenciales.GetTipoCredencial(numsolicitud);
                if (GetTipoCredencial == "1") //Credencial Permanente
                {
                    for (int i0 = 0; i0 < DTRESULT.Rows.Count; i0++)
                    {
                        if (DTRESULT.Rows[i0]["CIPAS"].ToString() == cedula && DTRESULT.Rows[i0]["NOMINA_ID"].ToString() == "0")
                        {
                            dtColaboradoresAprobados.Columns.Add("CEDULA");
                            dtColaboradoresAprobados.Columns.Add("APELLIDOS");
                            dtColaboradoresAprobados.Columns.Add("NOMBRES");
                            dtColaboradoresAprobados.Columns.Add("EMPRESA");
                            dtColaboradoresAprobados.Columns.Add("AREA");
                            dtColaboradoresAprobados.Columns.Add("DPTO");
                            dtColaboradoresAprobados.Columns.Add("CARGO");
                            dtColaboradoresAprobados.Columns.Add("EXPIRACION");
                            dtColaboradoresAprobados.Columns.Add("TIPO_SANGRE");
                            dtColaboradoresAprobados.Columns.Add("TIPO_LICENCIA");
                            dtColaboradoresAprobados.Columns.Add("FECHA_INGRESO");
                            dtColaboradoresAprobados.Columns.Add("FECHA_CADUCIDAD");

                            var empresa = aso_transportistas.GetEmpresaNomAC(ruc).Rows[0]["EMPE_NOM"].ToString(); //credenciales.GetEmpresaColaborador(numsolicitud.Trim()).Rows[0]["RAZONSOCIAL"].ToString();
                            dtAprobados = credenciales.GetSolicitudColaboradorOnlyControl(numsolicitud.Trim());
                            CultureInfo enUS = new CultureInfo("en-US");
                            DateTime fechaing;
                            if (!DateTime.TryParseExact(fechaingreso, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaing))
                            {
                                mensajeerror = string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}>", fechaingreso);
                                validacolaboradores = true;
                                return mensajeerror;
                            }
                            DateTime fechacad;
                            if (!DateTime.TryParseExact(fechasalida, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacad))
                            {
                                mensajeerror = string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}", fechasalida);
                                validacolaboradores = true;
                                return mensajeerror;
                            }

                            for (int i = 0; i < dtAprobados.Rows.Count; i++)
                            {
                                var errorregistro = string.Empty;
                                error_consulta = string.Empty;
                                DataTable dtGetCargoOnlyControl = credenciales.GetCargoOnlyControl(empresa, out error_consulta);
                                if (!string.IsNullOrEmpty(error_consulta))
                                {
                                    mensajeerror = error_consulta;
                                    return mensajeerror;
                                }
                                String CargoOnlyControl = string.Empty;
                                var resultsCargo = from myRow in dtGetCargoOnlyControl.AsEnumerable()
                                                   where myRow.Field<string>("CALI_NOM") == dtAprobados.Rows[i][11].ToString()
                                                   select myRow;
                                if (resultsCargo.AsDataView().Count > 0)
                                {
                                    CargoOnlyControl = dtAprobados.Rows[i][11].ToString();
                                }
                                else
                                {
                                    errorregistro = string.Empty;
                                    credenciales.GetRegistraCargoOnlyControl(dtAprobados.Rows[i][11].ToString(), out errorregistro);
                                    if (!string.IsNullOrEmpty(errorregistro))
                                    {
                                        var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_CARGO():{0}", error_consulta));
                                        var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraColaboradoresOnlyControl", "1", "Num Solicitud: " + numsolicitud);
                                        mensajeerror = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString());
                                        validacolaboradores = true;
                                        return mensajeerror;
                                    }
                                    CargoOnlyControl = dtAprobados.Rows[i][11].ToString();
                                }
                                TimeSpan tsDias = fechacad - fechaing;
                                int diferenciaEnDias = tsDias.Days;
                                if (diferenciaEnDias < 0)
                                {
                                    //Label lblTipoSolicitud = gvComprobantes.Rows[0].FindControl("lblTipoSolicitud") as Label;
                                    mensajeerror = "La Fecha de Ingreso: " + fechaingreso + "\nNO deber ser mayor a la\nFecha de Caducidad: " + fechasalida;
                                    validacolaboradores = true;
                                    return mensajeerror;
                                }

                                int diferenciaEnAños = fechacad.Year - fechaing.Year;
                                diferenciaEnAños = fechacad.Year - fechaing.Year;
                                TimeSpan tsTiempo = fechacad - fechaing;
                                int tiempoCaducidad = tsTiempo.Days;

                                dtColaboradoresAprobados.Rows.Add(
                                    dtAprobados.Rows[i][2].ToString(),
                                    dtAprobados.Rows[i]["APELLIDOS"].ToString(),
                                    dtAprobados.Rows[i]["NOMBRES"].ToString(),
                                    empresa,
                                    ddlAreaOnlyControl,
                                    ddlAreaOnlyControl,
                                    CargoOnlyControl,
                                    (string.IsNullOrEmpty(dtAprobados.Rows[i][12].ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : Convert.ToDateTime(dtAprobados.Rows[i][12].ToString()).ToString("yyyy-MM-dd")), //expiracion
                                    dtAprobados.Rows[i][4].ToString(),
                                    dtAprobados.Rows[i][10].ToString(),
                                    fechaing.AddDays(-1).ToString("yyyy-MM-dd"),
                                    fechaing.AddDays(-1).ToString("yyyy-MM-dd")
                                    );
                            }
                            dsColaboradores = new DataSet();
                            dsColaboradores.Tables.Add(dtColaboradoresAprobados);
                            //Nomina, Sistema Azul
                            OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
                            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_NOMINA(dsColaboradores, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_NOMINA(dsColaboradores, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                            if (registros_actualizados_incorrecto > 0)
                            {
                                if (string.IsNullOrEmpty(error_consulta))
                                {
                                    var dserror = dsErrorAC_R_PERSONA_PEATON.DefaultViewManager.DataSet.Tables;
                                    var dterror = dserror[0];
                                    var error2 = string.Empty;
                                    for (int i2 = 0; i2 < dterror.Rows.Count; i2++)
                                    {
                                        error2 = error2 + " \n *" +
                                               "Error: " + dterror.Rows[i2]["ERROR"].ToString() + " \n ";
                                    }
                                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERSONA_NOMINA():{0}", error2));
                                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraColaboradoresOnlyControl", registros_actualizados_correcto.ToString(), "Num Solicitud: " + numsolicitud);
                                    mensajeerror = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString());
                                    validacolaboradores = true;
                                    return mensajeerror;
                                }
                                var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERSONA_NOMINA():{0}", error_consulta));
                                var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "RegistraColaboradoresOnlyControl", registros_actualizados_correcto.ToString(), "Num Solicitud: " + numsolicitud);
                                mensajeerror = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString());
                                validacolaboradores = true;
                                return mensajeerror;
                            }
                            DataTable dtCargos = dsColaboradores.Tables[0];
                            string mensaje = string.Empty;
                            for (int i2 = 0; i2 < dtCargos.Rows.Count; i2++)
                            {
                                if (dtCargos.Rows[i2]["CARGO"].ToString().Trim() == "CONDUCTOR")
                                {
                                    if (!credenciales.UpdateCargoConductor(dtCargos.Rows[i2]["CEDULA"].ToString().Trim(), out mensaje))
                                    {
                                        //this.Alerta(mensaje);
                                    }
                                    else
                                    {
                                        //Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago enviado exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                                    }
                                }
                            }       
                            var dtDetSolicitudAso = credenciales.GetSolicitudColaboradorCliente(numsolicitud, ruc);
                            dtDetSolicitudAso.Columns.Add("NOMINA_ID");
                            dtDetSolicitudAso.Columns.Add("VAL_HUELLA");
                            dtDetSolicitudAso.Columns.Add("VAL_FOTO");
                            string codigouser = string.Empty;
                            for (int i = 0; i < dtDetSolicitudAso.Rows.Count; i++)
                            {
                                var dtcoduser = aso_transportistas.GetValidaCodigoUsuario(dtDetSolicitudAso.Rows[i]["RUCCIPAS"].ToString(), dtDetSolicitudAso.Rows[i]["CIPAS"].ToString());
                                if (dtcoduser.Rows.Count > 0)
                                {
                                    codigouser = dtcoduser.Rows[0][0].ToString();
                                    DTRESULT.Rows[i]["NOMINA_ID"] = codigouser;
                                }
                            }
                            mensajeerror = "0" + "," + codigouser;
                        }
                        else
                        {
                            mensajeerror = "0";
                        }
                    }
                }
            }
            else
            {
                mensajeerror = "No se puede capturar la Huella del conductor:\n" + conductor + "\nNumero de Solicitud: " + numsolicitud.Trim() + ", no es Tipo Permanente.";
                validacolaboradores = true;
            }
            return mensajeerror;
        }
        [WebMethod(EnableSession = true)]
        public static string IsAvailableFotoRAC(string user, string numsolicitud, string fechaingreso, string fechasalida, string ddlAreaOnlyControl, string cedula, string conductor)
        {
            string mensajeerror = "0";
            bool validacolaboradores = false;

            var dtvalhuellafoto = aso_transportistas.GetValHuellaFoto(numsolicitud, cedula);
            if (dtvalhuellafoto.Rows.Count <= 0)
            {
                mensajeerror = "Hubo un problema al consultar la huella y foto del conductor: \n" + conductor;
                validacolaboradores = true;
                return mensajeerror;
            }
            if (dtvalhuellafoto.Rows[0]["HUELLA"].ToString() == "1" && dtvalhuellafoto.Rows[0]["FOTO"].ToString() == "1")
            {
                mensajeerror = "El conductor: \n" + conductor + "\nYa tiene regsitrada una foto.";
                validacolaboradores = true;
                return mensajeerror;
            }

            DataTable DTRESULT = new DataTable();
            DTRESULT = (DataTable)HttpContext.Current.Session["dtDetSolicitud"];
            Boolean validasolicitud = false;
            var tiposoliciud = credenciales.GetTipoSolicitudXNumSolicitudAsoTrans(numsolicitud.Trim());
            if (tiposoliciud == "1") //Credencial Permanente y Temporal
            {
                var GetTipoCredencial = credenciales.GetTipoCredencial(numsolicitud);
                if (GetTipoCredencial == "1") //Credencial Permanente
                {
                    for (int i0 = 0; i0 < DTRESULT.Rows.Count; i0++)
                    {
                        if (DTRESULT.Rows[i0]["CIPAS"].ToString() == cedula && DTRESULT.Rows[i0]["NOMINA_ID"].ToString() == "0")
                        {

                            mensajeerror = "No se puede capturar la Foto del conductor:\n" + conductor + "\nHasta que capture la Huella."; 
                        }
                        //else
                        //{
                        //    mensajeerror = "0";
                        //}
                    }
                }
            }
            else
            {
                mensajeerror = "No se puede capturar la Foto del conductor:\n" + conductor + "\nNumero de Solicitud: " + numsolicitud.Trim() + ", no es Tipo Permanente.";
                validacolaboradores = true;
            }
            return mensajeerror;
        }
    }
}