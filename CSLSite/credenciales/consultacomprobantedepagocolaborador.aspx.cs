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
using BillionEntidades;
using System.Text;

namespace CSLSite
{
    public partial class consultacomprobantedepagocolaborador : System.Web.UI.Page
    {
        private Cls_Factura_Credenciales Factura = new Cls_Factura_Credenciales();

        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();

        public DataSet dsListaPermisos
        {
            get { return (DataSet)Session["dsListaPermisos"]; }
            set { Session["dsListaPermisos"] = value; }
        }

        public bool omiteRF
        {
            get { return (bool)Session["omiteRF"]; }
            set { Session["omiteRF"] = value; }
        }

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

        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudrevisasolicitudCredenciales_"]; }
            set { Session["numsolicitudrevisasolicitudCredenciales_"] = value; }
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
                this.btnGrabarVerificacion.Attributes["disabled"] = "disabled";
                sinresultado.Visible = false;
                xfinderpagado.Visible = false;
                divseccion2.Visible = false;
                divseccion3.Visible = false;
                botonera.Visible = false;
                //CARGA COMBO DE PERMISOS 
                dsListaPermisos = serviciosCredenciales.ConsultaPermiso();
                dsListaPermisos.Tables[0].Rows.Add("0", " * Elija * ", "0");
                
                cmbPermiso.DataSource = dsListaPermisos;
                cmbPermiso.DataValueField = "ID_PERMISO";
                cmbPermiso.DataTextField = "DESCRIPCION";
                cmbPermiso.DataBind();
                cmbPermiso.SelectedValue = "0";

                ConsultaInformacion();
                this.alertapagado.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
                //txtfecing.Enabled = false;
                //txtfecsal.Enabled = false;
                omiteRF = false;
                this.UPFotos.Update();
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
            txtsolicitud.Text = Request.QueryString["sid1"].ToString();
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
            var tablix = credenciales.GetTipoDeDocumentosColaborador(rucempresa, txtsolicitud.Text);
            gvComprobantes.DataSource = tablix;
            gvComprobantes.DataBind();
            if (tablix.Rows.Count == 1)
            {
                if (tablix.Rows[0]["CODESTADO"].ToString() == "1")
                {
                    botonera.Visible = false;
                    salir.Visible = true;
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
                    var areaOnlyControl = onlyControl.AC_C_AREA("%", 1, ref error_consulta); //credenciales.GetConsultaArea();
                    populateDropDownList(ddlAreaOnlyControl, areaOnlyControl.Tables[0], "* Elija *", "AREA_ID", "AREA_NOM", false);
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
                    salir.Visible = true;
                    divseccion2.Visible = false;
                    divseccion3.Visible = false;
                }
            }
            //txtfecing.Text = "";
            //txtfecsal.Text = "";
            txtnumfactura.Text = "";
            //divcabecera.Visible = true;        
            xfinderpagado.Visible = true;
            sinresultado.Visible = false;
            var tablixVehiculo = credenciales.GetSolicitudColaboradorCliente_New(txtsolicitud.Text, rucempresa);
            tablixVehiculo.Columns.Add("TIPOD");
            tablixVehiculo.Columns.Add("PERMISO");
            for (int i = 0; i < tablixVehiculo.Rows.Count; i++)
            {
                var dtvalhuellafoto = aso_transportistas.GetValHuellaFoto(txtsolicitud.Text, tablixVehiculo.Rows[i]["CIPAS"].ToString());
                if (dtvalhuellafoto.Rows[0]["HUELLA"].ToString() == "1" && dtvalhuellafoto.Rows[0]["FOTO"].ToString() == "1" && dtvalhuellafoto.Rows[0]["BLOQUEO"].ToString() == "0")
                {
                    var dtpermiso = aso_transportistas.GetFechasPermisoDeAcceso(tablixVehiculo.Rows[i]["RUCCIPAS"].ToString(), tablixVehiculo.Rows[i]["CIPAS"].ToString());
                    if (dtpermiso.Rows.Count > 0)
                    {
                        if (dtpermiso.Rows[0]["ESTADO"].ToString() == "ACTIVA")
                        {
                            var permiso = "Desde: " + Convert.ToDateTime(dtpermiso.Rows[0]["FECHAINGRESO"]).ToString("dd-MM-yyyy") + " - " +
                                          "Hasta: " + Convert.ToDateTime(dtpermiso.Rows[0]["FECHASALIDA"]).ToString("dd-MM-yyyy");
                            tablixVehiculo.Rows[i]["PERMISO"] = permiso;
                            tablixVehiculo.Rows[i]["TIPOD"] = tablixVehiculo.Rows[i]["TIPO"].ToString() + " - PERMISO TEMPORAL CREADO";
                        }
                        else
                        {
                            tablixVehiculo.Rows[i]["PERMISO"] = "";
                            tablixVehiculo.Rows[i]["TIPOD"] = tablixVehiculo.Rows[i]["TIPO"].ToString();
                        }
                    }
                }
                else
                {
                    if (dtvalhuellafoto.Rows[0]["BLOQUEO"].ToString() == "1")
                    {
                        tablixVehiculo.Rows[i]["PERMISO"] = "BLOQUEADO";
                        tablixVehiculo.Rows[i]["TIPOD"] = tablixVehiculo.Rows[i]["TIPO"].ToString();
                    }
                    else
                    {
                        tablixVehiculo.Rows[i]["PERMISO"] = "";
                        tablixVehiculo.Rows[i]["TIPOD"] = tablixVehiculo.Rows[i]["TIPO"].ToString();
                    }
                }
            }
            rpDetalle.DataSource = tablixVehiculo;
            rpDetalle.DataBind();
            foreach (RepeaterItem item in rpDetalle.Items)
            {
                CheckBox chkHuellaEstado = item.FindControl("chkHuellaEstado") as CheckBox;
                CheckBox chkFotoEstado = item.FindControl("chkFotoEstado") as CheckBox;
                var dtvalestadohuella = aso_transportistas.GetValidaHuella(tablixVehiculo.Rows[item.ItemIndex]["RUCCIPAS"].ToString(), tablixVehiculo.Rows[item.ItemIndex]["CIPAS"].ToString());
                var dtvalestadofoto = aso_transportistas.GetValidaFoto(tablixVehiculo.Rows[item.ItemIndex]["RUCCIPAS"].ToString(), tablixVehiculo.Rows[item.ItemIndex]["CIPAS"].ToString());
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

            if (tablixVehiculo.Select(" ESTADO = 'V'").Count() < tablixVehiculo.Rows.Count)
            {
                xfinderpagado.Visible = false;
            }

            //CARGA DATOS DE LA FACTURA
            Int64 IDSOLICITUD = 0;
            string OError;
            if (!Int64.TryParse(txtsolicitud.Text, out IDSOLICITUD))
            {
                IDSOLICITUD = 0;
            }

            Factura = new Cls_Factura_Credenciales();
            Factura.IDSOLICITUD = IDSOLICITUD;
            if (Factura.PopulateMyData(out OError))
            {
                this.txtnumfactura.Text = Factura.NUMERO_FACTURA.Trim();
            }
            else
            {
                this.txtnumfactura.Text = string.Empty;

                //sinresultado.Attributes["class"] = string.Empty;
                //sinresultado.Attributes["class"] = "msg-critico";
                //sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo: {0}", OError);
                //sinresultado.Visible = true;
            }

            //if (salir.Visible)
            //{
            //    xfinderpagado.Visible = true;
            //}
            //botonera.Visible = true;
            UPPAGADO.Update();
        }
        private void populateDropDownList(DropDownList dp, DataTable origen, string mensaje, string id, string descripcion, bool val)
        {
            if (val)
            {
                origen.Rows.Add("0", "0", mensaje);
            }
            else
            {
                origen.Rows.Add("0", mensaje);
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
            if (!xfinderpagado.Visible)
            {
                //Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago enviado exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                this.Alerta("No se puede crear el permiso, Existen Fotos Pendientes de verificar.");
                return;
            }
            
            try
            {
                if (!string.IsNullOrEmpty(txtmotivorechazo.Text))
                {
                    this.Alerta("Tiene un motivo de rechazo.\\nPara continuar limpie la observación en caso rechazo.");
                    txtmotivorechazo.Focus();
                    return;
                }
                Label lblIdSolicitud = gvComprobantes.Rows[0].FindControl("lblIdSolicitud") as Label;
                if (credenciales.GetCTipoCredencial(txtsolicitud.Text.Trim()))
                {
                    if (ddlTurnoOnlyControl.SelectedItem.Text == "* Elija *")
                    {
                        this.Alerta("Elija un turno.");
                        return;
                    }
                }

                if (cmbPermiso.SelectedValue == "0")
                {
                    this.Alerta("Elija el Permiso");
                    return;
                }

                Boolean validasolicitud = false;
                var tiposoliciud = credenciales.GetTipoSolicitudXNumSolicitud(txtsolicitud.Text.Trim());
                if (tiposoliciud == "1") //Credencial Permanente y Temporal
                {           
                    var GetTipoCredencial = credenciales.GetTipoCredencial(txtsolicitud.Text);
                    if (GetTipoCredencial == "1") //Credencial Permanente
                    {
                        RegistraColaboradoresOnlyControl(GetTipoCredencial, out validasolicitud);
                        if (!validasolicitud)
                        {
                            //RegistraPermisoPermanenteOnlyControl(out validasolicitud);
                            //if (!validasolicitud)
                            //{
                            AddPagoConfirmado();
                            //}
                        }
                    }
                    else if (GetTipoCredencial == "2") //Credencial Temporal
                    {
                        RegistraColaboradoresOnlyControl(GetTipoCredencial, out validasolicitud);
                        if (!validasolicitud)
                        {
                            AddPagoConfirmado();
                        }   
                    }
                }
                if (string.IsNullOrEmpty(tiposoliciud))
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Transacción a cambiado de estado, favor verificar.');if(r==true){window.close()}else{window.close()};</script>");
                    return;
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
                Label lblIdSolicitud = gvComprobantes.Rows[0].FindControl("lblIdSolicitud") as Label;
                txtsolicitud.Text = lblIdSolicitud.Text;
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


            //<JGUSQUI RF-2022-08-18>
            //###############################################################################################################
            //UNA VEZ CREADO EL REGISTRO EN OC Y ACTUALIZADO EL ESTADO DE LA SOLICITUD SE PROCEDE A DAR LOS PERMISOS EN OC
            //###############################################################################################################
            bool validasolicitud = true;
            string v_mensaje = string.Empty;
            RegistraPermisoPermanenteOnlyControl_New(out validasolicitud, out v_mensaje);
            if (!validasolicitud)
            {
                v_mensaje = v_mensaje + "Permisos creados con éxito \\n";
            }
            else
            {
                v_mensaje = v_mensaje + "\\nNo se logró crear los permisos en OC, favor verificar \\n";
                this.Alerta(v_mensaje);
                Response.Write("<script language='JavaScript'>var r=alert('Error al crear permiso. \\n " + v_mensaje + " ');if(r==true){window.close()}else{window.close()};</script>");
                return;
            }

            //#####################################################################
            // SE PROCEDE A REALIZAR EL REGISTRO FACIAL EN LA BASE DE ONLY CONTROL
            //#####################################################################
            string v_retornaMsj = string.Empty;

            if (omiteRF==false)
            {
                RegistroFacialOnlyControl_New(out v_retornaMsj);
            }
            v_mensaje = v_mensaje + " " + v_retornaMsj;
            //</JGUSQUI RF-2022-08-18>

            if (!credenciales.AddPagoConfirmado_New(
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
                        cmbPermiso.SelectedValue.ToString() +" - "+ cmbPermiso.SelectedItem.ToString(),
                        int.Parse(txtIdPermiso.Value),
                        out mensaje))
            {
                this.Alerta(mensaje);
            }
            else
            {
                Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago enviado exitosamente. \\n " + v_mensaje + " ');if(r==true){window.close()}else{window.close()};</script>");
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
            var empresa = credenciales.GetEmpresaColaborador(txtsolicitud.Text.Trim()).Rows[0]["RAZONSOCIAL"].ToString();
            dtAprobados = credenciales.GetSolicitudColaboradorOnlyControl(txtsolicitud.Text.Trim());
            dtAprobados.Columns.Add("BLOQUEADO");

            for (int i = 0; i < dtAprobados.Rows.Count; i++)
            {
                var dtvalhuellafoto = aso_transportistas.GetValHuellaFoto(txtsolicitud.Text, dtAprobados.Rows[i]["CIPAS"].ToString());
                if (dtvalhuellafoto.Rows[0]["HUELLA"].ToString() == "1" && dtvalhuellafoto.Rows[0]["FOTO"].ToString() == "1" && dtvalhuellafoto.Rows[0]["BLOQUEO"].ToString() == "1")
                {
                    dtAprobados.Rows[i]["BLOQUEADO"] = "1";
                }
                else
                {
                    dtAprobados.Rows[i]["BLOQUEADO"] = "0";
                }
            }

            dtAprobados = dtAprobados.Select("BLOQUEADO = '" + "0" + "'").Count() == 0 ? dtAprobados : dtAprobados.Select("BLOQUEADO = '" + "0" + "'").CopyToDataTable();

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
                //if (tipocredencial == "1")//Credencial Permanente
                //{
                    int diferenciaEnAños = fechacad.Year - fechaing.Year;
                    diferenciaEnAños = fechacad.Year - fechaing.Year;
                    TimeSpan tsTiempo = fechacad - fechaing;
                    int tiempoCaducidad = tsTiempo.Days;
                    if (tiempoCaducidad > credenciales.GetTiempoCaducidadCredencialPermanente())
                    {
                        Label lblTipoSolicitud = gvComprobantes.Rows[0].FindControl("lblTipoSolicitud") as Label;
                        this.Alerta("El Tiempo de Caducidad es mayor al permitido para una\\n" + lblTipoSolicitud.Text);
                        validacolaboradores = true;
                        return;
                    }
                    else if (tiempoCaducidad < credenciales.GetTiempoCaducidadCredencialPermanente())
                    {
                        Label lblTipoSolicitud = gvComprobantes.Rows[0].FindControl("lblTipoSolicitud") as Label;
                        this.Alerta("El Tiempo de Caducidad es menor al permitido para una\\n" + lblTipoSolicitud.Text);
                        validacolaboradores = true;
                        return;
                    }
                //}
                dtColaboradoresAprobados.Rows.Add(
                    dtAprobados.Rows[i][2].ToString(),
                    dtAprobados.Rows[i]["APELLIDOS"].ToString(),
                    dtAprobados.Rows[i]["NOMBRES"].ToString(), 
                    empresa,
                    ddlAreaOnlyControl.SelectedItem,
                    ddlAreaOnlyControl.SelectedItem,
                    CargoOnlyControl,
                    (string.IsNullOrEmpty(dtAprobados.Rows[i][12].ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : Convert.ToDateTime(dtAprobados.Rows[i][12].ToString()).ToString("yyyy-MM-dd")), //expiracion
                    dtAprobados.Rows[i][4].ToString(),
                    dtAprobados.Rows[i][10].ToString(),
                    fechaing.ToString("yyyy-MM-dd"),
                    fechacad.ToString("yyyy-MM-dd")
                    );
            }
            dsColaboradores = new DataSet();
            dsColaboradores.Tables.Add(dtColaboradoresAprobados);
            //Nomina, Sistema Azul
             int registros_actualizados_correcto1 = 0;
             int registros_actualizados_incorrecto1 = 0;
            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_NOMINA(dsColaboradores, ref registros_actualizados_correcto1, ref registros_actualizados_incorrecto1, ref error_consulta) as DataSet;
            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_NOMINA(dsColaboradores, ref registros_actualizados_correcto1, ref registros_actualizados_incorrecto1, ref error_consulta) as DataSet;
            //for (int i = 0; i < dsErrorAC_R_PERSONA_PEATON.Tables[0].Rows.Count; i++)
            //{
            //    if (dsErrorAC_R_PERSONA_PEATON.Tables[i].Rows[0]["EMPRESA"].ToString() != "")
            //    {
            //        var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERSONA_NOMINA():{0}", "Empresa No Existe"));
            //        var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepagocolaborador", "RegistraColaboradoresOnlyControl", DateTime.Now.ToShortDateString(), Request.UserHostAddress);
            //        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            //        validacolaboradores = true;
            //        return;
            //    }   
            //}
            if (registros_actualizados_incorrecto1 > 0)
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
                        //this.Alerta(mensaje);
                    }
                    else
                    {
                        //Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago enviado exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                    }
                }
            }
        }

        private void RegistraPermisoPermanenteOnlyControl_New(out bool validacolaboradores, out string resultadoStr)
        {
            DataTable dtDocumentos = new DataTable();
            validacolaboradores = false;
            resultadoStr = string.Empty;
            //if (dptipoevento.SelectedItem.ToString() == "PERMANENTE")
            //{
            //banderapermiso = true;
            int registros_actualizados_correcto = 0;
            int registros_actualizados_incorrecto = 0;
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
           
            var empresa = credenciales.GetEmpresaColaborador(txtsolicitud.Text.Trim()).Rows[0]["RAZONSOCIAL"].ToString();
            var dsPersonasOnlyControl = onlyControl.AC_C_PERSONA(empresa, string.Empty, 1, ref error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", error_consulta));
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                resultadoStr = string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", error_consulta);
                validacolaboradores = true;
                return;
            }
            //var numsolicitud = gvComprobantes.Rows[0].Cells[0].Text;
            Label numsolicitud = gvComprobantes.Rows[0].FindControl("lblIdSolicitud") as Label;
            var dtPermisosDeAcceso = credenciales.GetSolicitudColaboradorOnlyControl(numsolicitud.Text);

            DataTable dtPermiso = new DataTable();
            DataRow dr;
            dtPermiso.Columns.Add(new DataColumn("ID", typeof(String)));
            dtPermiso.Columns.Add(new DataColumn("F_HORARIO", typeof(Int32)));
            dtPermiso.Columns.Add(new DataColumn("F_INGRESO", typeof(DateTime)));
            dtPermiso.Columns.Add(new DataColumn("F_SALIDA", typeof(DateTime)));
            dtPermiso.Columns.Add(new DataColumn("TIPO_CONTROL", typeof(Int32))); //1 LIBRE (NO CADUCA O NO VALIDA FECHA) - 2 CONTROLADO
            dtPermiso.Columns.Add(new DataColumn("COD_PERMISO", typeof(Int32))); // NEW - EJ: 65
            dtPermiso.Columns.Add(new DataColumn("ID_PERMISO", typeof(String))); // NEW - EJ: FA
            dtPermiso.Columns.Add(new DataColumn("AREAS", typeof(String))); // NEW
                                 
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
                    //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    resultadoStr = resultadoStr + string.Format("\\n RF SW OC: {1}{2} de Imagenes del ID {0} :", dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString(), "EMPLEADO NO PERTENECE A LA EMPRESA ", empresa);
                    validacolaboradores = true;
                    return;
                }
                var id = resultPersonasOnlyControl.AsDataView().ToTable().Rows[0]["ID"];

                //dtPermiso.Rows.Add(id, fecing.ToString("yyyy-MM-dd"), facsal.ToString("yyyy-MM-dd"), ddlTurnoOnlyControl.SelectedValue, "2");
                dr = dtPermiso.NewRow();
                dr["ID"] = id;//"999999";
                dr["F_HORARIO"] = ddlTurnoOnlyControl.SelectedValue;// 7
                dr["F_INGRESO"] = fecing.ToString("yyyy-MM-dd");// Now.Date
                dr["F_SALIDA"] = facsal.ToString("yyyy-MM-dd");// DateAdd("d", 1, Now.Date)
                dr["TIPO_CONTROL"] = 2;
                dr["COD_PERMISO"] = txtIdPermiso.Value ;// 64
                dr["ID_PERMISO"] = cmbPermiso.SelectedValue;// "H"
                dr["AREAS"] = ddlAreaOnlyControl.SelectedValue;// "916" '"873, 874"
                dtPermiso.Rows.Add(dr);
            }
         
            DataSet dsPermiso = new DataSet();
            DataSet dsErrorAC_R_PERMISO = new DataSet();
            dsPermiso.Tables.Add(dtPermiso);
            //dsErrorAC_R_PERMISO = onlyControl.AC_R_PERMISO(dsPermiso, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            dsErrorAC_R_PERMISO = serviciosCredenciales.CrearPermiso(dsPermiso,out error_consulta, out registros_actualizados_correcto, out registros_actualizados_incorrecto);
            if (dsErrorAC_R_PERMISO.Tables[0].Rows.Count != 0)
            {
              
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
            }
        }

        private void RegistroFacialOnlyControl_New(out string resultadoStr)
        {
            DataTable dtDocumentos = new DataTable();
            resultadoStr = "";
            String error_consulta = string.Empty;
            CultureInfo enUS = new CultureInfo("en-US");
           
            var empresa = credenciales.GetEmpresaColaborador(txtsolicitud.Text.Trim()).Rows[0]["RAZONSOCIAL"].ToString();
            var dsPersonasOnlyControl = onlyControl.AC_C_PERSONA(empresa, string.Empty, 1, ref error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", error_consulta));
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepagocolaborador", "RegistroFacialOnlyControl_New", "1", Request.UserHostAddress);
                resultadoStr = string.Format("Lo sentimos, algo salió mal y no se logró realizar el registro facial. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString());
                return;
            }
           
            Label numsolicitud = gvComprobantes.Rows[0].FindControl("lblIdSolicitud") as Label;
            var dtPermisosDeAcceso = credenciales.GetSolicitudColaboradorOnlyControl(numsolicitud.Text);

            for (int i = 0; i < dtPermisosDeAcceso.Rows.Count; i++)
            {
                var resultPersonasOnlyControl = from myRow in dsPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0].AsEnumerable()
                                                where myRow.Field<string>("CEDULA") == dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString()
                                                select myRow;
                DataTable dt = resultPersonasOnlyControl.AsDataView().ToTable();
                if (resultPersonasOnlyControl.AsDataView().ToTable().Rows.Count == 0)
                {
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", "No se encontro el colaborador: " + dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString()));
                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepagocolaborador", "RegistroFacialOnlyControl_New", "1", Request.UserHostAddress);
                    //resultadoStr = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString());
                    resultadoStr = resultadoStr + string.Format("\\n RF SW OC: {1}{2} de Imagenes del ID {0} :", dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString(), "EMPLEADO NO PERTENECE A LA EMPRESA ", empresa);
                    continue;
                }
                string id = resultPersonasOnlyControl.AsDataView().ToTable().Rows[0]["ID"].ToString();
                DataTable dtFotos = credenciales.GetFotosSolicitudColaboradorOnlyControl(numsolicitud.Text, dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString());

                //###################################################
                //  GRABA REGISTRO FACIAL EN LA BASE DE ONLY CONTROL
                //###################################################
                //string img1 = null; string img2 = null; string img3 = null; string imgt1 = string.Empty; string imgt2 = string.Empty; string imgt3 = string.Empty;
                //for (int x = 0; x < dtFotos.Rows.Count; x++)
                //{
                //    byte[] v_image = (byte[])dtFotos.Rows[x]["RF"];
                //    string v_imageStr = Convert.ToBase64String(v_image);
                //    if (x == 0) { img1 = v_imageStr; imgt1 = dtFotos.Rows[x]["template"].ToString(); }
                //    if (x == 1) { img2 = v_imageStr; imgt2 = dtFotos.Rows[x]["template"].ToString(); }
                //    if (x == 2) { img3 = v_imageStr; imgt3 = dtFotos.Rows[x]["template"].ToString(); }
                //}
                //string strResult = string.Empty;
                //RespuestaSwNeuro obRfResult = serviciosCredenciales.ActualizaFace(id, img1, img2, img3, imgt1, imgt2, imgt3);
                //credenciales.ActualizarCodigoNominaRegistroFacial(long.Parse(dtPermisosDeAcceso.Rows[i]["NUMSOLICITUD"].ToString()), long.Parse(dtPermisosDeAcceso.Rows[i]["IDSOLCOL"].ToString()), id, out strResult);

                string img1 = null; string img2 = null; string img3 = null; string imgt1 = string.Empty; string imgt2 = string.Empty; string imgt3 = string.Empty;
                for (int x = 0; x < dtFotos.Rows.Count; x++)
                {
                    /*byte[] bData;
                    BinaryReader br = new BinaryReader(System.IO.File.OpenRead(dtFotos.Rows[x]["rutaLocal"].ToString()));
                    bData = br.ReadBytes((int)br.BaseStream.Length);
                    string v_imageStr = Convert.ToBase64String(bData);*/
                    string v_imageStr = dtFotos.Rows[x]["RF"].ToString();
                    if (x == 0) { img1 = v_imageStr; imgt1 = dtFotos.Rows[x]["template"].ToString(); }
                    if (x == 1) { img2 = v_imageStr; imgt2 = dtFotos.Rows[x]["template"].ToString(); }
                    if (x == 2) { img3 = v_imageStr; imgt3 = dtFotos.Rows[x]["template"].ToString(); }
                }
                string strResult;

                RespuestaSwNeuro obRfResult = serviciosCredenciales.ActualizaFace(id, img1, img2, img3, imgt1, imgt2, imgt3);
                credenciales.ActualizarCodigoNominaRegistroFacial(long.Parse(dtPermisosDeAcceso.Rows[i]["NUMSOLICITUD"].ToString()), long.Parse(dtPermisosDeAcceso.Rows[i]["IDSOLCOL"].ToString()), id, out strResult);


                if (obRfResult != null)
                {
                    if (obRfResult.codigo != "1")
                    {
                        var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.ActualizaFace():{0}", obRfResult.mensaje));
                        var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepagocolaborador", "RegistroFacialOnlyControl_New", "1", Request.UserHostAddress);
                        //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    }

                    resultadoStr = resultadoStr + string.Format("\\n RF SW OC: {1} de Imagenes del ID {0} ", dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString(), obRfResult.mensaje);
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
                Label numsolicitud = gvComprobantes.Rows[0].FindControl("lblIdSolicitud") as Label;
                var dtPermisosDeAcceso = credenciales.GetSolicitudColaboradorOnlyControl(numsolicitud.Text);
                DataTable dtPermiso = new DataTable();
                dtPermiso.Columns.Add("ID");
                dtPermiso.Columns.Add("F_INGRESO");
                dtPermiso.Columns.Add("F_SALIDA");
                dtPermiso.Columns.Add("HORARIO");
                dtPermiso.Columns.Add("TIPO_CONTROL");
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

        private string ConsultarFotosDespacho(string imagen)
        {
            if (string.IsNullOrEmpty(imagen))
            {
                //sinresultadoDespacho.Visible = true;
                return string.Empty;
            }

            

            string v_divImagenes = string.Empty;
           
            v_divImagenes += @"<div class='carousel-item active'>
                                    <img src = '" + imagen + @"' class='d-block w-100' style='height:540px; width:360px; overflow:auto' alt='...'/>
                                    <div class='carousel-caption d-none d-md-block'>
                                        <!-- <h5>Second slide label</h5>
                                        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                    </div>
                                </div> ";
           

            StringBuilder tab = new StringBuilder();

            string v_cuerpo = string.Empty;
            v_cuerpo = v_cuerpo + @"<div class='mb-5'>
                                    <div id='carouselExampleCaptions{X1}' class='carousel slide' data-ride='carousel'>
                                    <ol class='carousel-indicators'>
                                        <li data-target='#carouselExampleCaptions{X1}' data-slide-to='0' class='active'></li>
                                      
                                    </ol>
                                    <div class='carousel-inner'>
                                           
                                        " + v_divImagenes + @"
                                    </div>
                                    <a class='carousel-control-prev' href='#carouselExampleCaptions{X1}' role='button' data-slide='prev'>
                                        <span class='carousel-control-prev-icon' aria-hidden='true'></span>
                                        <span class='sr-only'>Previous</span>
                                    </a>
                                    <a class='carousel-control-next' href='#carouselExampleCaptions{X1}' role='button' data-slide='next'>
                                        <span class='carousel-control-next-icon' aria-hidden='true'></span>
                                        <span class='sr-only'>Next</span>
                                    </a>
                                </div>
                            </div>
                            ";
            v_cuerpo = v_cuerpo.Replace("{X1}", imagen.ToString());

            string v_html = string.Empty;
            v_html = v_html + v_cuerpo;

            tab.Append(v_html);
            return tab.ToString();
        }

        protected void rpDetalle_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            msjErrores.InnerHtml = "";
            msjErrores.Visible = false;
            UPFotos.Update();
            try
            {
                /*valida cotizacion*/
                var usn = new usuario();
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                usn = this.getUserBySesion();

                if (usn == null)
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "pases_zal", "RepeaterBooking_ItemCommand", "No usuario", Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
                if (token == null)
                {
                    this.Alerta("Estimado Cliente,Su sesión ha expirado, sera redireccionado a la pagina de login", true);
                    System.Web.Security.FormsAuthentication.SignOut();
                    Session.Clear();
                    return;
                }

               

                if (e.CommandName == "Fotos")
                {
                    var v_argumentos = e.CommandArgument.ToString().Split(',');
                    lblidSolicitud.Value = v_argumentos[0].ToString();
                    lblidSolcol.Value  = v_argumentos[1].ToString();
                    lblEstado.Value = v_argumentos[2].ToString();

                    if (lblEstado.Value == "A")
                    {
                        xfinderDet.Visible = true;
                        this.btnGrabarVerificacion.Attributes.Remove("disabled");
                    }
                    else
                    {
                        xfinderDet.Visible = false;
                        this.btnGrabarVerificacion.Attributes["disabled"] = "disabled";
                    }

                    customSwitch.Checked = true;
                    customSwitch2.Checked = true;
                    customSwitch3.Checked = true;
                    txtREchazoFotoMotivo.Text = string.Empty;
                    UPFotos.Update();

                    if (Response.IsClientConnected)
                    {
                        try
                        {
                            if (HttpContext.Current.Request.Cookies["token"] == null)
                            {
                                System.Web.Security.FormsAuthentication.SignOut();
                                Session.Clear();
                                Response.Redirect("../login.aspx", false);
                                //OcultarLoading("1");
                                return;
                            }

                            if (lblidSolicitud.Value== "" || lblidSolcol.Value == "" )
                            {
                                sinresultadoDespacho.Visible = true;
                                xfinderDes.Visible = false;
                                UPFotos.Update();
                                //this.Alerta("Debe ingresar el número de carga");
                                return;
                            }

                            var oTarjaDet = credenciales.GetRegistroFacialXNumSolicitudCliente(lblidSolicitud.Value , lblidSolcol.Value);

                            if (oTarjaDet == null)
                            {
                                sinresultadoDespacho.Visible = true;
                                xfinderDes.Visible = false;
                                UPFotos.Update();
                                //this.Alerta(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se encontró resultados con el número de solicitud ingresado"));
                                return;
                            }

                            if (oTarjaDet.Rows.Count <= 0)
                            {
                                sinresultadoDespacho.Visible = true;
                                xfinderDes.Visible = false;
                                UPFotos.Update();
                                return;
                            }

                            StringBuilder tab = new StringBuilder();

                            string v_cuerpo = string.Empty;
                            string v_detalle = string.Empty;
                            string v_html = @"  <div class='bs-example'>
                                        <div class='accordion' id='accordionExample'>";

                            foreach (var Det in oTarjaDet.Rows)
                            {
                                DataRow drFila = (DataRow)Det;

                                //HABILITA SWITCH DE RECHAZO/AUTORIZADO
                                if (drFila["secuencia"].ToString() == "1") { if (drFila["estado"].ToString() != "A") { this.customSwitch.Attributes["disabled"] = "disabled"; } else { this.customSwitch.Attributes.Remove("disabled"); } }
                                if (drFila["secuencia"].ToString() == "2") { if (drFila["estado"].ToString() != "A") { this.customSwitch2.Attributes["disabled"] = "disabled"; } else { this.customSwitch2.Attributes.Remove("disabled"); } } 
                                if (drFila["secuencia"].ToString() == "3") { if (drFila["estado"].ToString() != "A") { this.customSwitch3.Attributes["disabled"] = "disabled"; } else { this.customSwitch3.Attributes.Remove("disabled"); } } 

                                v_detalle = string.Empty;
                                
                                //v_detalle = v_detalle + "<p>" + ConsultarFotosDespacho(drFila["ruta"].ToString())  + "</p>";
                                //v_detalle = v_detalle + "<p>" + "<center><a href='" + drFila["ruta"].ToString() + "'  class='topopup' target='_blank'><i class='fa fa-search'></i> Ver Tamaño Completo</a></center>" + "</p>";



                                 v_detalle = v_detalle + "<p><span class='text-muted'>Solicitud :</span> " + drFila["idSolicitud"].ToString() + " - " + drFila["idSolcol"].ToString() + " - " + drFila["secuencia"].ToString() +
                                " <br/> <span class='text-muted'>Documento :</span> " + drFila["documento"].ToString() +
                                " <br/> <span class='text-muted'>Fecha Registro :</span> " + ((DateTime)drFila["fechaCreacion"]).ToString("dd/MM/yyyy hh:mm") +
                                " <br/> <span class='text-muted'>Observación :</span> " + drFila["comentarios"].ToString().ToUpper();
                                

                                v_detalle = v_detalle + "";
                                //string barra = @"  &nbsp;  &nbsp;";

                                v_cuerpo = v_cuerpo + @"
                                                <div class='card'>
                                                    <div class='card-header' id='heading{X1}'>
                                                        <h6 class='mb-0'>
                                                            <a class='collapsed card-link' data-toggle='collapse' data-target='#collapse{X1}'>
                                                                <i class='fa fa-plus'></i> "
                                                                            + string.Format("<span class='text-muted'>Código :</span>   {0} &nbsp;  &nbsp; &nbsp;  &nbsp;" +
                                                                                            "<span class='text-muted'>Identificación :</span>   {1} &nbsp;  &nbsp; &nbsp;  &nbsp;" +
                                                                                            "<span class='text-muted'>Estado :</span>  <b style='color:{3}';>{2}</b>  &nbsp;  &nbsp; &nbsp;  &nbsp;" +
                                                                                            "<a href='{4} '  class='topopup' target='_blank'><i class='fa fa-search'></i> Ver Imagen</a>"
                                                                                            //"<span class='text-muted'>NOTAS : </span>   {3}  "
                                                                                            , drFila["idSolicitud"].ToString().ToUpper() + "-" + drFila["idSolcol"].ToString() + "-" + drFila["secuencia"].ToString()
                                                                                            , drFila["identificacion"].ToString().ToUpper()
                                                                                            , drFila["EstadoDesc"].ToString()
                                                                                            //, drFila["comentarios"].ToString().ToUpper()
                                                                                            , drFila["Estado"].ToString() != "V"? drFila["Estado"].ToString() == "A" ? "#F27C00" : "#FF0000" : "#339D28"
                                                                                            , drFila["ruta"].ToString())
                                                                    + @"</a>
                                                        </h6>
                                                    </div>
                                                    <div id = 'collapse{X1}' class='collapse' aria-labelledby='heading{X1}' data-parent='#accordionExample'>
                                                        <div class='card-body'>
                                                            " + v_detalle + @"
                                                        </div>
                                                    </div>
                                                </div>
                                                ";

                                v_cuerpo = v_cuerpo.Replace("{X1}", drFila["idSolicitud"].ToString()+ drFila["idSolcol"].ToString()+ drFila["secuencia"].ToString());
                            }

                            v_html = v_html + v_cuerpo;
                            v_html = v_html + @" </div>
                        </div>";
                            tab.Append(v_html);
                            this.htmlDespachos.InnerHtml = tab.ToString();
                            xfinderDes.Visible = true;
                            sinresultadoDespacho.Visible = false;
                            //this.Actualiza_Panele_Detalle();
                            //this.Ocultar_Mensaje();
                            UPFotos.Update();
                        }
                        catch (Exception ex)
                        {
                            this.Alerta(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));

                        }
                    }
                }
                UPFotos.Update();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "RepeaterBooking_ItemCommand()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void btnGrabarVerificacion_Click(object sender, EventArgs e)
        {
            try
            {
                msjErrores.Visible = false;
                if ((!customSwitch.Checked) || (!customSwitch2.Checked) || (!customSwitch3.Checked))
                {
                    if (string.IsNullOrEmpty(txtREchazoFotoMotivo.Text))
                    {
                        //msjErrores.Attributes["class"] = string.Empty;
                        //msjErrores.Attributes["class"] = "msg-critico";
                        msjErrores.InnerText = "Escriba el motivo de rechazo.";
                        msjErrores.Visible = true;
                        UPFotos.Update();
                        return;
                    }
                }

                string mensajeMostrar = null;
                string mensaje = null;
                string v_estado = string.Empty;
                bool v_cheked = false;
                string v_motivo = string.Empty;
                for (int i = 1; i < 4; i++)
                {
                    if (i == 1) { v_estado = customSwitch.Checked? "V":"R"; v_cheked = customSwitch.Disabled; }
                    if (i == 2) { v_estado = customSwitch2.Checked ? "V" : "R"; v_cheked = customSwitch2.Disabled; }
                    if (i == 3) { v_estado = customSwitch3.Checked ? "V" : "R"; v_cheked = customSwitch3.Disabled; }
                    if (v_estado == "R") { v_motivo = txtREchazoFotoMotivo.Text; } else { v_motivo = string.Empty; }

                    if (!v_cheked)
                    {
                        if (!credenciales.ActualizarEstadoFotosRegistroFacial(long.Parse(lblidSolicitud.Value), long.Parse(lblidSolcol.Value), i, v_estado, v_motivo, Page.User.Identity.Name, out mensaje))
                        {
                            mensajeMostrar += string.Format("| Imagen {0}: {1} <br/>", i, mensaje);
                        }
                        else
                        {
                            mensajeMostrar += string.Format("| Imagen {0}: {1} <br/>", i, "Transacción exitosa");
                            //Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago rechazada exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                        }
                    }
                }
                this.btnGrabarVerificacion.Attributes["disabled"] = "disabled";
                //msjErrores.Attributes["class"] = string.Empty;
                //msjErrores.Attributes["class"] = "msg-critico";
                msjErrores.InnerHtml = mensajeMostrar;
                msjErrores.Visible = true;
                ConsultaInformacion();
                UPFotos.Update();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "btsalvar_Click", "1", Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected void cmbPermiso_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRow permisoSeleccioando;
            try
            {
                string filtro  = cmbPermiso.SelectedValue.ToString();
                permisoSeleccioando = dsListaPermisos?.Tables[0].Select(" id_permiso = '" + filtro + "'" ) .FirstOrDefault();
                txtIdPermiso.Value = permisoSeleccioando["COD_PERMISO"].ToString();
            }
            catch
            {
                return;
            }
            UPPAGADO.Update();
        }

        protected void btnOmitirRF_Click(object sender, EventArgs e)
        {
            omiteRF = true;
            //this.btnCreaPermisosRF.Attributes["disabled"] = "disabled";
            this.btnCreaPermisosRF.Visible = false;
            this.xfinderpagado.Visible = true;
            UPPAGADO.Update();
            UPCAB.Update();
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Write("<script language='JavaScript'>window.close();</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
    }
}