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

namespace CSLSite
{
    public partial class revisasolicitudpermisoprovisional : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        private String xmlDocumentos;
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosrevisasolicitudpermisoprovisional"]; }
            set { Session["dtDocumentosrevisasolicitudpermisoprovisional"] = value; }
        }
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudrevisasolicitudpermisoprovisional"]; }
            set { Session["numsolicitudrevisasolicitudpermisoprovisional"] = value; }
        }
        public string codigousuario
        {
            get { return (string)Session["codigousuariorevisasolicitudpermisoprovisional"]; }
            set { Session["codigousuariorevisasolicitudpermisoprovisional"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["rucempresarevisasolicitudpermisoprovisional"]; }
            set { Session["rucempresarevisasolicitudpermisoprovisional"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailrevisasolicitudpermisoprovisional"]; }
            set { Session["useremailrevisasolicitudpermisoprovisional"] = value; }
        }
        public string mensajefac
        {
            get { return (string)Session["mensajefacrevisasolicitudpermisoprovisional"]; }
            set { Session["mensajefacrevisasolicitudpermisoprovisional"] = value; }
        }
        public string mensajeok
        {
            get { return (string)Session["mensajeokrevisasolicitudpermisoprovisional"]; }
            set { Session["mensajeokrevisasolicitudpermisoprovisional"] = value; }
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
                        btsalvar.ToolTip = "Crea el permiso provisional.";
                        mensajefac = "finalización";
                        mensajeok = "finalizada";
                    }
                }
                else
                {
                    if (!credenciales.GetSolicitudEstado(numsolicitudempresa))
                    {
                        botonera.Visible = false;
                        salir.Visible = true;
                        factura.Visible = false;
                    }
                    else
                    {
                        alertafu.InnerHtml = "Adjunte el archivo en formato PDF";
                        factura.Visible = true;
                    }
                    mensajefac = "facturación";
                    mensajeok = "facturada";
                }

                var DescripcionTipoSolicitud = credenciales.GetDescripcionTipoSolicitudXNumSolicitud(numsolicitudempresa);
                for (int i = 0; i <= DescripcionTipoSolicitud.Rows.Count - 1; i++)
                {
                    txttipcli.Text = DescripcionTipoSolicitud.Rows[i][0].ToString();
                }
                this.alerta.InnerHtml = "Confirme que los documentos del colaborador(es) sean los correctos.";
                Session["dtDocumentosrevisasolicitudcolaborador"] = new DataTable();
                var tablixVehiculo = credenciales.GetSolicitudColaboradorPermisoProvisional(numsolicitudempresa);
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
            }
            catch (Exception ex)
            {
                Session["dtDocumentosrevisasolicitudcolaborador"] = new DataTable();
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
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
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechaing;
                if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaing))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio{0}\\n", txtfecing.Text));
                    txtfecing.Focus();
                    return;
                }
                DateTime fechacad;
                if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacad))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio{0}\\n", txtfecsal.Text));
                    txtfecsal.Focus();
                    return;
                }
                TimeSpan tsDias = fechacad - fechaing;
                int diferenciaEnDias = tsDias.Days;
                if (diferenciaEnDias < 0)
                {
                    this.Alerta("La Fecha de Ingreso: " + txtfecing.Text + "\\nNO deber ser mayor a la\\nFecha de Caducidad: " + txtfecsal.Text);
                    return;
                }
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString[var];
                }
                Boolean banderafac = false;
                List<string> listCedulas = new List<string>();
                DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (chkRevisado.Checked == false && !string.IsNullOrEmpty(tcomentario.Text))
                    {
                        this.Alerta("Marque la casilla del Comentario de rechazo. \\n Cedula: " + lblcipas.Text);
                        return;
                    }
                    //if (chkRevisado.Checked == true && string.IsNullOrEmpty(tcomentario.Text))
                    //{
                    //    this.Alerta("Escriba el Comentario de la Cedula: *" + lblcipas.Text);
                    //    return;
                    //}
                    else if (chkRevisado.Checked == true)
                    {
                        listCedulas.Add(lblcipas.Text);
                    }
                    if (chkRevisado.Checked == false && (dtDocSol != null || dtDocSol.Rows.Count > 0))
                    {
                        var result = from myRow in dtDocSol.AsEnumerable()
                                     where myRow.Field<string>("Cedula") == lblcipas.Text
                                     select myRow;
                        DataTable dt = result.AsDataView().ToTable();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dt.Rows[item.ItemIndex][10]) == true)
                            {
                                this.Alerta("La Cedula: *" + lblcipas.Text + "*\\nTiene documentos rechazados.\\n" + "Revise la información antes de continuar con la " + mensajefac + " de la solicitud.\\n Si quiere continuar de click en Rechazar.");
                                return;
                            }
                        }
                    }
                }
                DataTable dtColaboradores = credenciales.GetSolicitudColaboradorPermisoProvisional(numsolicitudempresa);
                var resultAprobados = from myRow in dtColaboradores.AsEnumerable()
                                      where !listCedulas.Contains(myRow.Field<string>("CIPAS"))
                                      select myRow;
                DataTable dtAprobados = resultAprobados.AsDataView().ToTable();
                if (dtAprobados.Rows.Count == 0)
                {
                    this.Alerta("Tiene un unico colaborador rechazado.\\n" + "Revise la información antes de continuar con la " + mensajefac + " de la solicitud.\\n Si quiere continuar de click en Rechazar.");
                    return;
                }
                var coloboradores = dtColaboradores;//credenciales.GetSolicitudColaboradorPermisoProvisional(numsolicitudempresa);
                coloboradores.Columns.Add("Comentario");
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (chkRevisado.Checked == true)
                    {
                        coloboradores.Rows[item.ItemIndex]["Comentario"] = tcomentario.Text;
                    }
                }
                DataTable dtCol = new DataTable();
                var resultListaAprobados = from myRow in coloboradores.AsEnumerable()
                                           where listCedulas.Contains(myRow.Field<string>("CIPAS"))
                                           select myRow;
                dtCol = resultListaAprobados.AsDataView().ToTable();
                dtCol.AcceptChanges();
                dtCol.TableName = "Colaboradores";
                StringWriter sw = new StringWriter();
                dtCol.WriteXml(sw);
                String xmlColaboradores = sw.ToString();
                String errorvehiculo = string.Empty;
                Int32 registros_actualizados_correcto = 0;
                Int32 registros_actualizados_incorrecto = 0;
                String error_consulta = string.Empty;
                String error = string.Empty;
                var empresa = credenciales.GetEmpresaColaborador(numsolicitudempresa).Rows[0]["RAZONSOCIAL"].ToString();
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                    var number = log_csl.save_log<Exception>(ex, "revisasolicitudpermisoprovisional", "btsalvar_Click", "1", Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    return;}           
                DataSet dsErrorAC_R_PERSONA_PEATON = new DataSet();
                DataTable dtAC_R_PERSONA_PEATON = new DataTable();
                DataSet dsAC_R_PERSONA_PEATON = new DataSet();
                dtAC_R_PERSONA_PEATON.Columns.Add("CEDULA");
                dtAC_R_PERSONA_PEATON.Columns.Add("APELLIDOS");
                dtAC_R_PERSONA_PEATON.Columns.Add("NOMBRES");
                dtAC_R_PERSONA_PEATON.Columns.Add("EMPRESA");
                dtAC_R_PERSONA_PEATON.Columns.Add("CARGO");
                DataSet dsErrorAC_R_PERSONA = new DataSet();
                DataTable dtAC_R_PERSONA = new DataTable();
                DataSet dsAC_R_PERSONA = new DataSet();
                dtAC_R_PERSONA.Columns.Add("CEDULA");
                dtAC_R_PERSONA.Columns.Add("APELLIDOS");
                dtAC_R_PERSONA.Columns.Add("NOMBRES");
                dtAC_R_PERSONA.Columns.Add("EMPRESA");
                dtAC_R_PERSONA.Columns.Add("AREA");
                dtAC_R_PERSONA.Columns.Add("DPTO");
                dtAC_R_PERSONA.Columns.Add("CARGO");
                dtAC_R_PERSONA.Columns.Add("EXPIRACION");
                dtAC_R_PERSONA.Columns.Add("TIPO_SANGRE");
                dtAC_R_PERSONA.Columns.Add("TIPO_LICENCIA");
                dtAC_R_PERSONA.Columns.Add("FECHA_INGRESO");
                dtAC_R_PERSONA.Columns.Add("FECHA_CADUCIDAD");
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    if (!chkRevisado.Checked)
                    {
                        var results = from myRow in dtColaboradores.AsEnumerable()
                                      where myRow.Field<string>("CIPAS") == lblcipas.Text
                                      select myRow;
                        DataTable dtresults = results.AsDataView().ToTable();
                        dtAC_R_PERSONA_PEATON.Rows.Add(dtresults.Rows[0]["CIPAS"], dtresults.Rows[0]["APELLIDOS"], dtresults.Rows[0]["NOMBRES"], empresa, dtresults.Rows[0]["CARGO"]);
                        //dtAC_R_PERSONA.Rows.Add(dtresults.Rows[0]["CIPAS"], dtresults.Rows[0]["APELLIDOS"], dtresults.Rows[0]["NOMBRES"], empresa, dtresults.Rows[0]["AREA"], "PPP PERMISO PEATONAL PROVISIONAL", dtresults.Rows[0]["CARGO"], DateTime.Now.ToString("yyyy-MM-dd"), dtresults.Rows[0]["TIPOSANGRE"], "", fechaing.ToString("yyyy-MM-dd"), fechacad.ToString("yyyy-MM-dd"));
                    }
                }
                //dsAC_R_PERSONA.Tables.Add(dtAC_R_PERSONA);
                dsAC_R_PERSONA_PEATON.Tables.Add(dtAC_R_PERSONA_PEATON);
                ////Nomina, Sistema Azul
                //dsErrorAC_R_PERSONA = onlyControl.AC_R_PERSONA_NOMINA(dsAC_R_PERSONA, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                //if (OnlyControlError(dsErrorAC_R_PERSONA, registros_actualizados_incorrecto, error_consulta, "AC_R_PERSONA_NOMINA"))
                //{
                //    return;
                //}
                //dsErrorAC_R_PERSONA = onlyControl.AC_R_PERSONA_NOMINA(dsAC_R_PERSONA, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                //if (OnlyControlError(dsErrorAC_R_PERSONA, registros_actualizados_incorrecto, error_consulta, "AC_R_PERSONA_NOMINA"))
                //{
                //    return;
                //}
                //Nomina Sistema Celeste
                dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_PEATON(dsAC_R_PERSONA_PEATON, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_PERSONA_PEATON, registros_actualizados_incorrecto, error_consulta, "AC_R_PERSONA_PEATON"))
                {
                    return;
                }       
                DataTable dtAC_R_PERMISO_TEMPORAL = new DataTable();
                DataSet dsAC_R_PERMISO_TEMPORAL = new DataSet();
                DataSet dsErrorAC_R_PERMISO_TEMPORAL = new DataSet();
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("ID_PERMISO");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("EMPRESA");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("AREA");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("ID_SOLICITANTE");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("CEDULA_SOLICITANTE");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("SOLICITANTE");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("ACTIVIDAD");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("FECHA_INICIO");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("FECHA_FIN");
                var replegal = txtusuariosolper.Text;
                var arrglo = replegal.Split(';');
                var cedulareplegal = arrglo[0];
                var nombresreplegal = arrglo[1];
                dtAC_R_PERMISO_TEMPORAL.Rows.Add("", empresa, txtarea.Text, "", cedulareplegal, nombresreplegal, txtactper.Text, fechaing.ToString("yyyy-MM-dd") + " 00:00", fechacad.ToString("yyyy-MM-dd") + " 23:59");
                dsAC_R_PERMISO_TEMPORAL.Tables.Add(dtAC_R_PERMISO_TEMPORAL);
                dsErrorAC_R_PERMISO_TEMPORAL = onlyControl.AC_R_PERMISO_TEMPORAL(dsAC_R_PERMISO_TEMPORAL, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_PERMISO_TEMPORAL, registros_actualizados_incorrecto, error_consulta, "AC_R_PERSONA_NOMINA"))
                {
                    return;
                }   
                DataTable dtAC_R_ASIGNA_PERSONA = new DataTable();
                DataSet dsAC_R_ASIGNA_PERSONA = new DataSet();
                DataSet dsErrorAC_R_ASIGNA_PERSONA = new DataSet();
                dtAC_R_ASIGNA_PERSONA.Columns.Add("ID_PERMISO");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("CEDULA");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("ID_PERSONA");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("PERSONA");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("USUARIO");
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    Label lblNombres = item.FindControl("lblNombres") as Label;
                    if (!chkRevisado.Checked)
                    {
                        dtAC_R_ASIGNA_PERSONA.Rows.Add(dsErrorAC_R_PERMISO_TEMPORAL.Tables[0].Rows[0]["ID_PERMISO"], lblcipas.Text, "", lblNombres.Text, codigousuario);

                    }
                }
                dsAC_R_ASIGNA_PERSONA.Tables.Add(dtAC_R_ASIGNA_PERSONA);
                dsErrorAC_R_ASIGNA_PERSONA = onlyControl.AC_R_ASIGNA_PERSONA(dsAC_R_ASIGNA_PERSONA, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_ASIGNA_PERSONA, registros_actualizados_incorrecto, error_consulta, "AC_R_ASIGNA_PERSONA"))
                {
                    return;
                }
                DataTable dtPermiso = new DataTable();
                dtPermiso.Columns.Add("ID");
                dtPermiso.Columns.Add("F_INGRESO");
                dtPermiso.Columns.Add("F_SALIDA");
                dtPermiso.Columns.Add("HORARIO");
                dtPermiso.Columns.Add("TIPO_CONTROL");
                //var dsPersonasOnlyControl = onlyControl.AC_C_PERSONA(empresa, string.Empty, 1, ref error_consulta);
                //if (!string.IsNullOrEmpty(error_consulta))
                //{
                //    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", error_consulta));
                //    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                //    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                //    return;
                //}
                for (int i = 0; i < dtAC_R_ASIGNA_PERSONA.Rows.Count; i++)
                {
                    //var resultPersonasOnlyControl = from myRow in dsPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0].AsEnumerable()
                    //                                where myRow.Field<string>("CEDULA") == dtAC_R_ASIGNA_PERSONA.Rows[i]["CEDULA"].ToString()
                    //                                select myRow;
                    //DataTable dt = resultPersonasOnlyControl.AsDataView().ToTable();
                    //if (resultPersonasOnlyControl.AsDataView().ToTable().Rows.Count == 0)
                    //{
                    //    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", "No se encontro el colaborador: " + dtAC_R_ASIGNA_PERSONA.Rows[i]["CEDULA"].ToString()));
                    //    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                    //    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    //    return;
                    //}
                    //                    var id = resultPersonasOnlyControl.AsDataView().ToTable().Rows[0]["ID"];
                    var id = onlyControl.GetIdAc_NominaPeaton(dtAC_R_ASIGNA_PERSONA.Rows[i]["CEDULA"].ToString());
                    string setoc = onlyControl.SetPersonaPeatonProvisional(id);
                    dtPermiso.Rows.Add(id, fechaing.ToString("yyyy-MM-dd"), fechacad.ToString("yyyy-MM-dd"), ddlTurnoOnlyControl.SelectedValue, "2");
                }
                DataSet dsPermiso = new DataSet();
                DataSet dsErrorAC_R_PERMISO = new DataSet();
                dsPermiso.Tables.Add(dtPermiso);
                dsErrorAC_R_PERMISO = onlyControl.AC_R_PERMISO(dsPermiso, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_PERMISO, registros_actualizados_incorrecto, error_consulta, "AC_R_PERMISO"))
                {
                    if (dsErrorAC_R_PERMISO.Tables[0].Rows[0]["ERROR"].ToString().Substring(0, 15) != "Error Duplicado")
                    {
                        return;   
                    }
                } 
                string mensaje = null;
                //string nombreempresa = CslHelper.getShiperName(rucempresa);

                DataTable dtCola = new DataTable();
                var resultListaApr = from myRow in dtDocSol.AsEnumerable()
                                     where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true
                                     select myRow;
                dtCola = resultListaApr.AsDataView().ToTable();
                dtCola.AcceptChanges();
                dtCola.TableName = "Colaboradores";
                StringWriter swC = new StringWriter();
                dtCola.WriteXml(swC);
                xmlDocumentos = swC.ToString();

                StringWriter swAcceso = new StringWriter();
                dsErrorAC_R_ASIGNA_PERSONA.Tables[0].WriteXml(swAcceso);
                String xmlAcceso = swAcceso.ToString();
                if (!credenciales.ApruebaSolicitudColaboradorPaseProvisional(
                    numsolicitudempresa,
                    rucempresa,
                    //nombreempresa,
                    //useremail,
                    xmlColaboradores,
                    xmlDocumentos,
                    xmlAcceso,
                    Page.User.Identity.Name.ToUpper(),
                    banderafac,
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    //ConsultaInfoSolicitud();
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud " + mensajeok + " exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString[var];
                }
                DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;
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
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    var result = from myRow in dtDocSol.AsEnumerable()
                                 where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true && myRow.Field<string>("Cedula") == lblcipas.Text
                                 select myRow;
                    DataTable dt = result.AsDataView().ToTable();
                    if (dt.Rows.Count == 0)
                    {
                        this.Alerta("La Cedula: *" + lblcipas.Text + "*\\nNO Tiene documentos rechazados.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud.");
                        return;
                    }
                }

                var resultVehiculo = from myRow in dtDocSol.AsEnumerable()
                                     where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true
                                     select myRow;
                DataTable dtColaboradores = resultVehiculo.AsDataView().ToTable();
                dtColaboradores.AcceptChanges();
                dtColaboradores.TableName = "Colaboradores";
                StringWriter sw = new StringWriter();
                dtColaboradores.WriteXml(sw);
                String xmlColaboradores = sw.ToString();
                string nombreempresa = CslHelper.getShiperName(rucempresa);
                string mensaje = null;
                if (!credenciales.RechazaSolicitudColaboradorPaseProvisional(
                    numsolicitudempresa,
                    rucempresa,
                    nombreempresa,
                    useremail,
                    xmlColaboradores,
                    Page.User.Identity.Name.ToUpper(),
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
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
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
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
        private void ExportFileUpload()
        {
            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");
            if (fuAdjuntarFactura.HasFile)
            {
                string rutafile = Server.MapPath(fuAdjuntarFactura.FileName);
                string extensionfile = Path.GetExtension(fuAdjuntarFactura.PostedFile.FileName);
                string directorio = Path.GetDirectoryName(rutafile);
                string nombrearchivo = Path.GetFileNameWithoutExtension(fuAdjuntarFactura.FileName);
                if (File.Exists(rutafile))
                {
                    File.Delete(rutafile);
                }
                fuAdjuntarFactura.SaveAs(rutafile);
                string[] files = Directory.GetFiles(directorio, "*" + nombrearchivo + extensionfile);
                foreach (string s in files)
                {
                    FileInfo fi = null;
                    try
                    {
                        fi = new FileInfo(s);
                        ExportFiles(fi.Directory.ToString(), fi.Name, numsolicitudempresa, nombrearchivo, extensionfile);
                        if (File.Exists(rutafile))
                        {
                            File.Delete(rutafile);
                        }
                    }
                    catch (FileNotFoundException ex)
                    {
                        this.Alerta(ex.Message);
                        return;
                    }
                }
                if (File.Exists(rutafile))
                {
                    File.Delete(rutafile);
                }
                //fsupload.SaveAs(savePath += fsupload.FileName);
                //savePath = @"C:\TemporalFileSystemCsaCGSA\";
                //this.Alerta("Your file was saved as " + fileName);
            }
            else
            {
                //this.Alerta("Seleccione los documentos requeridos.");
                //fsupload.Focus();
                //return;
                //return;
            }
            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            StringWriter sw = new StringWriter();
            dtDocumentos.WriteXml(sw);
            xmlDocumentos = sw.ToString();
            //System.IO.File.Delete(savePath);
        }
        private void ExportFiles(string path, string filename, string sidsolicitud, string nombrearchivo, string extension)
        {
            path = path + "\\";
            if (File.Exists(path + filename))
            {
                FileStream fileStream;
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                nombrearchivo = nombrearchivo + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssff") + extension;
                dtDocumentos.Rows.Add(sidsolicitud, dateServer + nombrearchivo);
                getFile.UploadFile(credenciales.ReadBinaryFile(path, filename, out fileStream), rutaServer, nombrearchivo);
                fileStream.Close();
            }
        }
        private bool OnlyControlError(DataSet dsError, int registros_actualizados_incorrecto, string error_consulta, string metodo)
        {
            if (dsError.Tables[0].Rows.Count != 0)
            {
                if (!string.IsNullOrEmpty(dsError.Tables[0].Rows[0]["ERROR"].ToString()))
                {
                    if (registros_actualizados_incorrecto > 0)
                    {
                        if (string.IsNullOrEmpty(error_consulta))
                        {
                            var dserror = dsError.DefaultViewManager.DataSet.Tables;
                            var dterror = dserror[0];
                            var error = string.Empty;
                            for (int i = 0; i < dterror.Rows.Count; i++)
                            {
                                error = error + " \\n *" +
                                       "Error: " + dterror.Rows[0]["ERROR"].ToString() + " \\n ";
                            }
                            var ex = new ApplicationException(string.Format("Error al usar el Metodo" + metodo + ":{0}", error));
                            var number = log_csl.save_log<Exception>(ex, "revisasolicitudpermisoprovisional", "OnlyControlError", registros_actualizados_incorrecto.ToString(), Request.UserHostAddress);
                            this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                            return true;
                        }
                        var ex2 = new ApplicationException(string.Format("Error al usar el Metodo" + metodo + ":{0}", error_consulta));
                        var number2 = log_csl.save_log<Exception>(ex2, "revisasolicitudpermisoprovisional", "OnlyControlError", registros_actualizados_incorrecto.ToString(), Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                        return true;
                    }
                    else if (!string.IsNullOrEmpty(error_consulta))
                    {
                        var ex = new ApplicationException(string.Format("Error al usar el Metodo" + metodo + ":{0}", error_consulta));
                        var number = log_csl.save_log<Exception>(ex, "revisasolicitudpermisoprovisional", "OnlyControlError", registros_actualizados_incorrecto.ToString(), Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                        return true;
                    }
                }
            }
            return false;
        }
    }
}