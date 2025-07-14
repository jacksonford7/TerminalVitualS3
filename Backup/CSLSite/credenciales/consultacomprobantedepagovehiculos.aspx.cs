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

namespace CSLSite
{
    public partial class consultacomprobantedepagovehiculos : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        public string rucempresa
        {
            get { return (string)Session["rucempresaconsultacomprobantedepagoveh"]; }
            set { Session["rucempresaconsultacomprobantedepagoveh"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailconsultacomprobantedepagoveh"]; }
            set { Session["useremailconsultacomprobantedepagoveh"] = value; }
        }
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    ViewStateUserKey = Session.SessionID;
        //}
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
                sinresultado.Visible = false;
                xfinderpagado.Visible = false;
                divseccion2.Visible = false;
                botonera.Visible = false;
                ConsultaInformacion();
                this.alertapagado.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
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
            var tablix = credenciales.GetTipoDeDocumentosVehiculo(rucempresa, txtsolicitud.Text);
            gvComprobantes.DataSource = tablix;
            gvComprobantes.DataBind();
            if (tablix.Rows.Count == 1)
            {
                if (tablix.Rows[0][4].ToString() == "1")
                {
                    botonera.Visible = false;
                    divseccion2.Visible = false;
                }
                else
                {
                    botonera.Visible = false;
                    divseccion2.Visible = false;
                    ConsultaPermiso();
                }
            }
            else
            {
                if (tablix.Rows.Count > 0)
                {
                    botonera.Visible = true;
                    divseccion2.Visible = true;
                    ConsultaPermiso();
                }
                else
                {
                    botonera.Visible = false;
                    divseccion2.Visible = false;
                }
            }
            txtfecing.Text = "";
            txtfecsal.Text = "";
            txtnumfactura.Text = "";
            //divcabecera.Visible = true;
            xfinderpagado.Visible = true;
            sinresultado.Visible = false;
            //botonera.Visible = true;
            var tablixVehiculo = credenciales.GetSolicitudVehiculoCliente(txtsolicitud.Text);
            rpVehiculos.DataSource = tablixVehiculo;
            rpVehiculos.DataBind();
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
                Boolean validasolicitud = false;
                //var tiposoliciud = credenciales.GetTipoSolicitudXNumSolicitud(txtsolicitud.Text.Trim());
                //if (tiposoliciud == "3") //Permisos de Acceso
                //{
                //    RegistraPermisosDeAccesoOnlyControl(out validasolicitud);
                //    if (!validasolicitud)
                //    {
                //        AddPagoConfirmado();
                //    }
                //}
                //else if (tiposoliciud == "2") //Sticker Vehicular
                //{
                    RegistraVehiculosOnlyControl(out validasolicitud);
                    if (!validasolicitud)
                    {
                        AddPagoConfirmado();
                    }
                //}
                //else if (tiposoliciud == "1") //Credencial
                //{
                //    RegistraColaboradoresOnlyControl(out validasolicitud);
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
        private void AddPagoConfirmado()
        {
            string mensaje = null;
            String xmlDocumentos = string.Empty;
            if (!credenciales.AddPagoConfirmado(
                txtsolicitud.Text.Trim(),
                CslHelper.getShiperName(rucempresa),
                rucempresa,
                useremail,
                Page.User.Identity.Name,
                txtnumfactura.Text.Trim(),
                txtfecing.Text,
                txtfecsal.Text,
                "",
                "",
                "",
                out mensaje))
            {
                this.Alerta(mensaje);
            }
            else
            {
                //this.Alerta("Confirmación de pago enviado exitosamente.");
                //ConsultaInformacion();
                Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago enviado exitosamente.');if(r==true){window.close();}else{window.close();}</script>");
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
                    //this.Alerta("Solicitud rechazada exitosamente.");
                    Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago rechazada exitosamente.');if(r==true){window.close();}else{window.close();}</script>");
                    //txtmotivorechazo.Text = "";
                    //ConsultaInformacion();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
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
                    fechamtop = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    fechapoliza = Convert.ToDateTime(string.IsNullOrEmpty(dtAprobados.Rows[i][11].ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : dtAprobados.Rows[i][11].ToString()).ToString("yyyy-MM-dd");
                    fechamtop = Convert.ToDateTime(string.IsNullOrEmpty(dtAprobados.Rows[i][12].ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : dtAprobados.Rows[i][12].ToString()).ToString("yyyy-MM-dd");
                }
                //var empresa = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
                //if (!string.IsNullOrEmpty(error_consulta))
                //{
                //    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                //    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraVehiculosOnlyControl", "1", Request.UserHostAddress);
                //    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                //    validavehiculos = true;
                //    return;
                //}
                var empresa = credenciales.GetEmpresaVehiculo(txtsolicitud.Text.Trim()).Rows[0]["RAZONSOCIAL"].ToString();
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
                //if (dt.Rows.Count == 0)
                //{
                    var resultemp = from myRow in dtVehiculosAprobados.AsEnumerable()
                                    where myRow.Field<string>("PLACA") == dtVehiculosAprobados.Rows[i][0].ToString()
                                    select myRow;
                    DataTable dttemp = resultemp.AsDataView().ToTable();
                    //dttemp.Rows[0][4] = CslHelper.getShiperName(rucempresa);
                    dsVehiculos = new DataSet();
                    dsVehiculos.Tables.Add(dttemp);
                    string mensaje = "";
                    for (int f = 0; f < dsVehiculos.Tables[0].Rows.Count; f++)
                    {
                        if (!credenciales.AddModeloVehiculo(dsVehiculos.Tables[0].Rows[f]["MARCA"].ToString(), dsVehiculos.Tables[0].Rows[f]["MODELO"].ToString(), out mensaje))
                        {
                            var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_VEHICULOS():{0}", mensaje));
                            var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "btsalvar_Click", DateTime.Now.ToShortDateString(), Request.UserHostAddress);
                            this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                            validavehiculos = true;
                            return;
                        }
                    }
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
                //}
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
        private void RegistraColaboradoresOnlyControl(out bool validacolaboradores)
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
            var empresa = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraVehiculosOnlyControl", "1", Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                validacolaboradores = true;
                return;
            }
            dtAprobados = credenciales.GetSolicitudColaboradorOnlyControl(txtsolicitud.Text.Trim());
            for (int i = 0; i < dtAprobados.Rows.Count; i++)
            {
                dtColaboradoresAprobados.Rows.Add(dtAprobados.Rows[i][2], dtAprobados.Rows[i][3], dtAprobados.Rows[i][3], empresa, dtAprobados.Rows[i][11]);
            }
            dsColaboradores = new DataSet();
            dsColaboradores.Tables.Add(dtColaboradoresAprobados);
            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_PEATON(dsColaboradores, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            if (registros_actualizados_incorrecto > 0)
            {
                alertapagado.InnerHtml = error_consulta;
                validacolaboradores = true;
                return;
            }
            else if (!string.IsNullOrEmpty(error_consulta))
            {
                alertapagado.InnerHtml = error_consulta;
                validacolaboradores = true;
                return;
            }
            else if (string.IsNullOrEmpty(error_consulta))
            {
                var dserror = dsErrorAC_R_PERSONA_PEATON.DefaultViewManager.DataSet.Tables;
                var dterror = dserror[0];
                alertapagado.InnerHtml = dterror.Rows[0]["ERROR"].ToString();
            }
            else
            {
                alertapagado.InnerHtml = error_consulta;
            }
            DataTable dt2 = new DataTable();
            dt2 = dsErrorAC_R_PERSONA_PEATON.Tables[0];
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    if (dt2.Rows[i][5].ToString().Substring(0, 27).ToUpper() != "Error: Cedula ya registrada".ToUpper())
                    {
                        errorvehiculo = dt2.Rows[i][5].ToString();
                        var t = credenciales.SaveLog(errorvehiculo, "credenciales", "onlyControl.AC_R_PERSONA_PEATON()", DateTime.Now.ToShortDateString(), "sistema");
                        validacolaboradores = true;
                        return;
                    }
                }
            }
            if (!string.IsNullOrEmpty(errorvehiculo))
            {
                alertapagado.InnerHtml = "Hubo un error al intentar registrar los siguientes vehiculos en el metodo AC_R_PERSONA_PEATON, revise a continuación. <br />" + errorvehiculo;
                //validacolaboradores = true;
                return;
            }
        }
        private void RegistraPermisosDeAccesoOnlyControl(out bool validacolaboradores)
        {
            DataTable dtDocumentos = new DataTable();
            validacolaboradores = false;
            //if (dptipoevento.SelectedItem.ToString() == "PERMANENTE")
            //{
                //banderapermiso = true;
                Int32 registros_actualizados_correcto = 0;
                Int32 registros_actualizados_incorrecto = 0;
                String error_consulta = string.Empty;
                //CultureInfo enUS = new CultureInfo("en-US");
                //DateTime fecing;
                //if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecing))
                //{
                //    this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecing.Text));
                //    txtfecing.Focus();
                //    validacolaboradores = true;
                //    return;
                //}
                //DateTime facsal;
                //if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out facsal))
                //{
                //    this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecsal.Text));
                //    txtfecsal.Focus();
                //    validacolaboradores = true;
                //    return;
                //}
                //var empresa = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
                //if (!string.IsNullOrEmpty(error_consulta))
                //{
                //    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                //    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                //    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                //    validacolaboradores = true;
                //    return;
                //}
                var empresa = credenciales.GetEmpresaVehiculo(txtsolicitud.Text.Trim()).Rows[0]["RAZONSOCIAL"].ToString();
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
                var dtPermisosDeAcceso = credenciales.GetColaboradoresSolicitudPermisosDeAcceso(numsolicitud.Text);
                DataTable dtPermiso = new DataTable();
                dtPermiso.Columns.Add("ID");
                dtPermiso.Columns.Add("F_INGRESO");
                dtPermiso.Columns.Add("F_SALIDA");
                //List<string> listPersonasCsa = new List<string>();
                //for (int i = 0; i < dtPermisosDeAcceso.Rows.Count; i++)
                //{
                //    listPersonasCsa.Add(dtPermisosDeAcceso.Rows[i]["CEDULA"].ToString());
                //}
                for (int i = 0; i < dtPermisosDeAcceso.Rows.Count; i++)
                {
                    var resultPersonasOnlyControl = from myRow in dsPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0].AsEnumerable()
                                                    //where listPersonasCsa.Contains(myRow.Field<string>("CEDULA"))
                                                    select myRow;
                    var id = resultPersonasOnlyControl.AsDataView().ToTable().Rows[i]["ID"];
                    DateTime fecingreso = new DateTime();
                    DateTime feccaducidad = new DateTime();
                    fecingreso = Convert.ToDateTime(dtPermisosDeAcceso.Rows[i]["FECHAINGRESO"]);
                    feccaducidad = Convert.ToDateTime(dtPermisosDeAcceso.Rows[i]["FECHACADUCIDAD"]);
                    dtPermiso.Rows.Add(id, fecingreso.ToString("yyyy-MM-dd"), feccaducidad.ToString("yyyy-MM-dd"));
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
                    var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERMISO():{0}", error_consulta));
                    var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                    validacolaboradores = true;
                    return;
                }
                else if (!string.IsNullOrEmpty(error_consulta))
                {
                    var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERMISO():{0}", error_consulta));
                    var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                    validacolaboradores = true;
                    return;
                }
                Boolean validasolicitud = false;
                ActualizaFechasIngresCaducidadColaboradorOnlyControl(empresa, dtPermisosDeAcceso, out validasolicitud);
                if (!validasolicitud)
                {
                    
                }
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
    }
}