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
using csl_log;
using System.Text;

namespace CSLSite
{
    public partial class revisasolicitudpermisoOPC : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();

        public DataSet dsListaPermisos
        {
            get { return (DataSet)Session["dsListaPermisosOPC"]; }
            set { Session["dsListaPermisosOPC"] = value; }
        }

        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudrevisasolicitudpermisoOPC"]; }
            set { Session["numsolicitudrevisasolicitudpermisoOPC"] = value; }
        }
        public string codigousuario
        {
            get { return (string)Session["codigousuariorevisasolicitudpermisoOPC"]; }
            set { Session["codigousuariorevisasolicitudpermisoOPC"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["rucempresarevisasolicitudpermisoOPC"]; }
            set { Session["rucempresarevisasolicitudpermisoOPC"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailrevisasolicitudpermisoOPC"]; }
            set { Session["useremailrevisasolicitudpermisoOPC"] = value; }
        }
        public string mensajefac
        {
            get { return (string)Session["mensajefacrevisasolicitudpermisoOPC"]; }
            set { Session["mensajefacrevisasolicitudpermisoOPC"] = value; }
        }
        public string mensajeok
        {
            get { return (string)Session["mensajeokrevisasolicitudpermisoOPC"]; }
            set { Session["mensajeokrevisasolicitudpermisoOPC"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
           // this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }

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
                //CARGA COMBO DE PERMISOS 
                dsListaPermisos = serviciosCredenciales.ConsultaPermiso();
                dsListaPermisos.Tables[0].Rows.Add("0", " * Elija * ", "0");

                cmbPermiso.DataSource = dsListaPermisos;
                cmbPermiso.DataValueField = "ID_PERMISO";
                cmbPermiso.DataTextField = "DESCRIPCION";
                cmbPermiso.DataBind();
                cmbPermiso.SelectedValue = "0";
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
                if (!credenciales.GetTipoSolicitud(numsolicitudempresa))
                {
                    if (!credenciales.GetSolicitudEstado(numsolicitudempresa))
                    {
                        botonera.Visible = false;
                        salir.Visible = true;
                        factura.Visible = false;
                    }
                    else
                    {
                        factura.Visible = false;
                        btsalvar.Text = "Crear Permiso";
                        btsalvar.ToolTip = "Crea el permiso OPC.";
                        mensajefac = "finalización";
                        mensajeok = "finalizada";
                    }
                }
       

                var DescripcionTipoSolicitud = credenciales.GetDescripcionTipoSolicitudXNumSolicitud(numsolicitudempresa);
                for (int i = 0; i <= DescripcionTipoSolicitud.Rows.Count - 1; i++)
                {
                    txttipcli.Text = DescripcionTipoSolicitud.Rows[i][0].ToString();
                }

                var tablixVehiculo = credenciales.GetSolicitudColaboradores(numsolicitudempresa);

                //SE CARGAN LOS COLABORADORES RECHAZADOS EN LA CONSULTA RECIENTE
                RepeaterItemCollection myItemCollection = tablePagination.Items;
                RepeaterItem[] myItemArray = new RepeaterItem[myItemCollection.Count];
                myItemCollection.CopyTo(myItemArray, 0);

                tablePagination.DataSource = tablixVehiculo;
                tablePagination.DataBind();

                if (tablixVehiculo.Rows.Count > 0)
                {
                    txtusuariosolper.Text = tablixVehiculo.Rows[0]["NOMBRESREPLEGAL"].ToString();
                    txtarea.Text = tablixVehiculo.Rows[0]["AREA"].ToString();
                    txtactper.Text = tablixVehiculo.Rows[0]["ACTIVIDAD"].ToString();
                    txtfecing.Text = Convert.ToDateTime(tablixVehiculo.Rows[0]["FECHAINGRESO"]).ToString("dd/MM/yyyy");
                    txtfecsal.Text = Convert.ToDateTime(tablixVehiculo.Rows[0]["FECHASALIDA"]).ToString("dd/MM/yyyy");
                    txttipcli.Text = CslHelper.getShiperName(rucempresa);
                    codigousuario = tablixVehiculo.Rows[0]["CODIGOUSUARIO"].ToString();
                }
                var turnoOnlyControl = credenciales.GetConsultaTurno();
                populateDropDownList(ddlTurnoOnlyControl, turnoOnlyControl, "* Elija *", "TUR_ID", "TUR_D", false);
                ddlTurnoOnlyControl.SelectedItem.Text = "* Elija *";
                string error_consulta = "";
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
                var areaOnlyControl = onlyControl.AC_C_AREA("%", 1, ref error_consulta); //credenciales.GetConsultaArea();
                populateDropDownList(ddlAreaOnlyControl, areaOnlyControl.Tables[0], "* Elija *", "AREA_ID", "AREA_NOM", false);
                populateDropDownList(ddlDepartamentoOnlyControl, dptoOnlyControl, "* Elija *", "DEP_ID", "DEP_NOM", true);
                populateDropDownList(ddlCargoOnlyControl, cargoOnlyControl, "* Elija *", "CALI_ID", "CALI_NOM", false);
                ddlAreaOnlyControl.SelectedItem.Text = "* Elija *";
                ddlDepartamentoOnlyControl.SelectedItem.Text = "* Elija *";
                ddlCargoOnlyControl.SelectedItem.Text = "* Elija *";

                xfinderpagado.Visible = true;
                
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
            UPdetalle.Update();
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

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
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

                if (e.CommandName == "Rechazar")
                {
                    var item = e.Item;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;

                    if (salir.Visible)
                    {
                        this.Alerta("Acción no permitida, La solicitud ya ha sido procesada");
                        return;
                    }

                    if (string.IsNullOrEmpty(tcomentario.Text))
                    {
                        this.Alerta("Se debe agregar un comentario para rechazo especifico");
                        return;
                    }

                    var v_argumentos = e.CommandArgument.ToString().Split(',');
                    string v_idSol = v_argumentos[0].ToString();
                    string v_idSolcol= v_argumentos[1].ToString();
                    string mensaje;
                    if (!credenciales.RechazarColaboradorEspecifico(
                        v_idSol,
                        v_idSolcol,
                        tcomentario.Text,
                        Page.User.Identity.Name,
                        out mensaje))
                    {
                        this.Alerta(mensaje);
                    }
                    else
                    {
                        this.Alerta("Colaborador rechazado exitosamente.");
                    }
                    ConsultaInfoSolicitud();
                    UPdetalle.Update();
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "RepeaterBooking_ItemCommand()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
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

        protected void cmbPermiso_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRow permisoSeleccioando;

            try
            {
                string filtro = cmbPermiso.SelectedValue.ToString();
                permisoSeleccioando = dsListaPermisos?.Tables[0].Select(" id_permiso = '" + filtro + "'").FirstOrDefault();
                txtIdPermiso.Value = permisoSeleccioando["COD_PERMISO"].ToString();
            }
            catch
            {
                return;
            }
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            if (!xfinderpagado.Visible)
            {
                this.Alerta("No se puede crear el permiso, Existen Fotos Pendientes de verificar.");
                return;
            }

            foreach (string var in Request.QueryString)
            {
                numsolicitudempresa = Request.QueryString["numsolicitud"];
            }
            TextBox txtsolicitud =  new TextBox();
            txtsolicitud.Text = numsolicitudempresa;

            try
            {
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
               
                var GetTipoCredencial = credenciales.GetTipoCredencial(txtsolicitud.Text);
               
                if (string.IsNullOrEmpty(GetTipoCredencial))
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Transacción a cambiado de estado, favor verificar.');if(r==true){window.close()}else{window.close()};</script>");
                    return;
                }

                AddPagoConfirmado(txtsolicitud.Text);
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
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString["numsolicitud"];
                }
                TextBox txtsolicitud = new TextBox();
                txtsolicitud.Text = numsolicitudempresa;

                string nombreempresa = CslHelper.getShiperName(rucempresa);
                string mensaje = null;
                if (!credenciales.RechazaSolicitudColaboradorOPC(
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
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud rechazada exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
        private void AddPagoConfirmado(string numSoli)
        {
            string mensaje = null;
            String xmlDocumentos = string.Empty;
            TextBox txtsolicitud = new TextBox();
            txtsolicitud.Text = numSoli;

            //<JGUSQUI RF-2022-08-18>
            //###############################################################################################################
            //UNA VEZ CREADO EL REGISTRO EN OC Y ACTUALIZADO EL ESTADO DE LA SOLICITUD SE PROCEDE A DAR LOS PERMISOS EN OC
            //###############################################################################################################
            bool validasolicitud = true;
            string v_mensaje = string.Empty;
            RegistraPermisoPermanenteOnlyControl_New(txtsolicitud.Text, out validasolicitud, out v_mensaje);
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
            
            v_mensaje = v_mensaje + " " + v_retornaMsj;
            //</JGUSQUI RF-2022-08-18>

            if (!credenciales.AddActivaciondePermisosOPC(
                        txtsolicitud.Text.Trim(),
                        CslHelper.getShiperName(rucempresa),
                        rucempresa,
                        useremail,
                        Page.User.Identity.Name,
                        "",//txtnumfactura.Text.Trim(),
                        txtfecing.Text,
                        txtfecsal.Text,
                        ddlAreaOnlyControl.SelectedItem.Text,
                        ddlDepartamentoOnlyControl.SelectedItem.Text,
                        ddlTurnoOnlyControl.SelectedItem.Text,
                        cmbPermiso.SelectedValue.ToString() + " - " + cmbPermiso.SelectedItem.ToString(),
                        int.Parse(txtIdPermiso.Value),
                        out mensaje))
            {
                this.Alerta(mensaje);
            }
            else
            {
                Response.Write("<script language='JavaScript'>var r=alert('Solicitud finalizada exitosamente. \\n " + v_mensaje + " ');if(r==true){window.close()}else{window.close()};</script>");
            }
        }
        private void RegistraPermisoPermanenteOnlyControl_New(string numSolicitud,out bool validacolaboradores, out string resultadoStr)
        {
            validacolaboradores = false;
            resultadoStr = string.Empty;
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

            var empresa = credenciales.GetEmpresaColaborador(numSolicitud).Rows[0]["RAZONSOCIAL"].ToString();
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
            var dtPermisosDeAcceso = credenciales.GetSolicitudColaboradorOnlyControl(numSolicitud);

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
                dr["COD_PERMISO"] = txtIdPermiso.Value;// 64
                dr["ID_PERMISO"] = cmbPermiso.SelectedValue;// "H"
                dr["AREAS"] = ddlAreaOnlyControl.SelectedValue;// "916" '"873, 874"
                dtPermiso.Rows.Add(dr);
            }

            DataSet dsPermiso = new DataSet();
            DataSet dsErrorAC_R_PERMISO = new DataSet();
            dsPermiso.Tables.Add(dtPermiso);
            //dsErrorAC_R_PERMISO = onlyControl.AC_R_PERMISO(dsPermiso, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            dsErrorAC_R_PERMISO = serviciosCredenciales.CrearPermiso(dsPermiso, out error_consulta, out registros_actualizados_correcto, out registros_actualizados_incorrecto);
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
    }
}