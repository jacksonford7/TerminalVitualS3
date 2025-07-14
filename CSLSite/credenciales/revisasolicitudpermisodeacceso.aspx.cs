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
    public partial class revisasolicitudpermisodeacceso : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        private String xmlDocumentos;
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosrevisasolicitudcolaborador"]; }
            set { Session["dtDocumentosrevisasolicitudcolaborador"] = value; }
        }
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudrevisasolicitudcolaborador"]; }
            set { Session["numsolicitudrevisasolicitudcolaborador"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["rucempresarevisasolicitudpermisodeacceso"]; }
            set { Session["rucempresarevisasolicitudpermisodeacceso"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailrevisasolicitudpermisodeacceso"]; }
            set { Session["useremailrevisasolicitudpermisodeacceso"] = value; }
        }
        public string mensajefac
        {
            get { return (string)Session["mensajefacrevisasolicitudcolaborador"]; }
            set { Session["mensajefacrevisasolicitudcolaborador"] = value; }
        }
        public string mensajeok
        {
            get { return (string)Session["mensajeokrevisasolicitudcolaborador"]; }
            set { Session["mensajeokrevisasolicitudcolaborador"] = value; }
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
                foreach (string var in Request.QueryString)
                {
                    rucempresa = Request.QueryString["ruc"];
                }
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
                    numsolicitudempresa = Request.QueryString["numsolicitud"];
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
                        btsalvar.Text = "Finalizar";
                        btsalvar.ToolTip = "Finaliza la solicitud.";
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
                        //alertafu.InnerHtml = "Adjunte el archivo en formato PDF";
                        //factura.Visible = true;
                        factura.Visible = false;
                        //btsalvar.Text = "Finalizar";
                        //btsalvar.ToolTip = "Finaliza la solicitud.";
                        mensajefac = "finalización";
                        mensajeok = "finalizada";
                    }
                }
                var DescripcionTipoSolicitud = credenciales.GetDescripcionTipoSolicitudXNumSolicitud(numsolicitudempresa);
                for (int i = 0; i <= DescripcionTipoSolicitud.Rows.Count - 1; i++)
                {
                    txttipcli.Text = DescripcionTipoSolicitud.Rows[i][0].ToString();
                }
                this.alerta.InnerHtml = "Confirme que los documentos del colaborador(es) sean los correctos.";
                //Session["dtDocumentosrevisasolicitudcolaborador"] = new DataTable();
                var tablixVehiculo = credenciales.GetSolicitudPermisosDeAcceso(numsolicitudempresa);
                tablePagination.DataSource = tablixVehiculo;
                tablePagination.DataBind();
                string error_consulta = string.Empty;
                //var areaOnlyControl = credenciales.GetAreaOnlyControl(rucempresa, out error_consulta);
                //if (!string.IsNullOrEmpty(error_consulta))
                //{
                //    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                //}
                //var areaOnlyControl = credenciales.GetConsultaArea();
                //error_consulta = string.Empty;
                //var dptoOnlyControl = credenciales.GetDptoOnlyControl(rucempresa, out error_consulta);

                //if (!string.IsNullOrEmpty(error_consulta))
                //{
                //    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                //}
                //error_consulta = string.Empty;
                //var cargoOnlyControl = credenciales.GetCargoOnlyControl(rucempresa, out error_consulta);
                //if (!string.IsNullOrEmpty(error_consulta))
                //{
                //    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                //}
                var turnoOnlyControl = credenciales.GetConsultaTurno();
                turnoOnlyControl.Rows.Add("0", "* Elija *");
                DropDownList ddlAreaOnlyControl = new DropDownList();
                DropDownList ddlDepartamentoOnlyControl = new DropDownList();
                DropDownList ddlCargoOnlyControl = new DropDownList();
                DropDownList ddlPermiso = new DropDownList();
                DropDownList ddlturnoOnlyControl = new DropDownList();
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    //ddlAreaOnlyControl = item.FindControl("ddlAreaOnlyControl") as DropDownList;
                    //ddlDepartamentoOnlyControl = item.FindControl("ddlDepartamentoOnlyControl") as DropDownList;
                    //ddlCargoOnlyControl = item.FindControl("ddlCargoOnlyControl") as DropDownList;
                    //ddlPermiso = item.FindControl("ddlPermiso") as DropDownList;
                    ddlturnoOnlyControl = item.FindControl("ddlTurnoOnlyControl") as DropDownList;
                    //populateDropDownList(ddlAreaOnlyControl, areaOnlyControl, "* Elija *", "AREA_ID", "AREA_NOM", false);
                    //populateDropDownList(ddlDepartamentoOnlyControl, dptoOnlyControl, "* Elija *", "DEP_ID", "DEP_NOM", true);
                    //populateDropDownList(ddlCargoOnlyControl, cargoOnlyControl, "* Elija *", "CALI_ID", "CALI_NOM", false);
                    populateDropDownList(ddlturnoOnlyControl, turnoOnlyControl, "* Elija *", "TUR_ID", "TUR_D", false);
                    ddlturnoOnlyControl.SelectedValue = "0";
                    //populateDropPermiso(ddlPermiso, credenciales.getTipoEventosColaborador("ECC"));
                }
                //ddlAreaOnlyControl.SelectedItem.Text = "* Elija *";
                //ddlDepartamentoOnlyControl.SelectedItem.Text = "* Elija *";
                //ddlCargoOnlyControl.SelectedItem.Text = "* Elija *";
                //ddlPermiso.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                Session["dtDocumentosrevisasolicitudcolaborador"] = new DataTable();
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
        private void populateDropPermiso(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }
        private void populateDropDownList(DropDownList dp, DataTable origen, string mensaje, string id, string descripcion, bool val)
        {
            if (val)
            {
                //origen.Rows.Add("0", "0", mensaje);
            }
            else
            {
                //origen.Rows.Add("0", mensaje);
            }
            dp.DataSource = origen;
            dp.DataValueField = id;
            dp.DataTextField = descripcion;
            dp.DataBind();
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString["numsolicitud"];
                }

                Boolean banderafac = false;
                List<string> listCedulas = new List<string>();
                Int32 registros_actualizados_correcto = 0;
                Int32 registros_actualizados_incorrecto = 0;
                String error_consulta = string.Empty;
                DataTable dtPermiso = new DataTable();
                dtPermiso.Columns.Add("ID");
                dtPermiso.Columns.Add("F_INGRESO");
                dtPermiso.Columns.Add("F_SALIDA");
                dtPermiso.Columns.Add("HORARIO");
                dtPermiso.Columns.Add("TIPO_CONTROL");

                var empresa = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    return;
                }
                var dsPersonasOnlyControl = onlyControl.AC_C_PERSONA(empresa, string.Empty, 1, ref error_consulta);
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", error_consulta));
                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    return;
                }
                var coloboradores = credenciales.GetSolicitudPermisosDeAcceso(numsolicitudempresa);
                coloboradores.Columns.Add("Permiso");
                coloboradores.Columns.Add("TipoPermiso");
                coloboradores.Columns.Add("FechaIngresoOC");
                coloboradores.Columns.Add("FechaCaducidadOC");
                coloboradores.Columns.Add("TurnoOC");
                coloboradores.Columns.Add("AreaOC");
                coloboradores.Columns.Add("DptoOC");
                coloboradores.Columns.Add("CargoOC");
                coloboradores.Columns.Add("Rechazado");
                coloboradores.Columns.Add("Comentario");
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkPermiso = item.FindControl("chkPermiso") as CheckBox;
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    TextBox txtfecing = item.FindControl("txtfecing") as TextBox;
                    TextBox txtfecsal = item.FindControl("txtfecsal") as TextBox;
                    Label lblfecing = item.FindControl("lblfecing") as Label;
                    Label lblfecsal = item.FindControl("lblfeccad") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    CultureInfo enUS = new CultureInfo("en-US");
                    DropDownList ddlAreaOnlyControl = item.FindControl("ddlAreaOnlyControl") as DropDownList;
                    DropDownList ddlDepartamentoOnlyControl = item.FindControl("ddlDepartamentoOnlyControl") as DropDownList;
                    DropDownList ddlCargoOnlyControl = item.FindControl("ddlCargoOnlyControl") as DropDownList;
                    DropDownList ddlPermiso = item.FindControl("ddlPermiso") as DropDownList;
                    DropDownList ddlTurnoOnlyControl = item.FindControl("ddlTurnoOnlyControl") as DropDownList;
                    if (chkRevisado.Checked)
                    {
                        coloboradores.Rows[item.ItemIndex]["Rechazado"] = chkRevisado.Checked;
                        coloboradores.Rows[item.ItemIndex]["Comentario"] = tcomentario.Text;
                    }
                    else
                    {
                        coloboradores.Rows[item.ItemIndex]["Permiso"] = chkPermiso.Checked;
                        coloboradores.Rows[item.ItemIndex]["TipoPermiso"] = ""; //ddlPermiso.SelectedItem.Value;
                        coloboradores.Rows[item.ItemIndex]["TurnoOC"] = ddlTurnoOnlyControl.SelectedItem.Text;
                        coloboradores.Rows[item.ItemIndex]["AreaOC"] = ""; //ddlAreaOnlyControl.SelectedItem.Text;
                        coloboradores.Rows[item.ItemIndex]["DptoOC"] = ""; //ddlDepartamentoOnlyControl.SelectedItem.Text;
                        coloboradores.Rows[item.ItemIndex]["CargoOC"] = ""; //ddlCargoOnlyControl.SelectedItem.Text;
                        var resultPersonasOnlyControl = from myRow in dsPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0].AsEnumerable()
                                                        where myRow.Field<string>("CEDULA") == lblcipas.Text
                                                        select myRow;
                        DataTable dt = resultPersonasOnlyControl.AsDataView().ToTable();
                        if (resultPersonasOnlyControl.AsDataView().ToTable().Rows.Count == 0)
                        {
                            var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", "No se encontro el colaborador: " + lblcipas.Text));
                            var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                            this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                            return;
                        }
                        var id = resultPersonasOnlyControl.AsDataView().ToTable().Rows[0]["ID"];
                        if (chkPermiso.Checked)
                        {
                            DateTime fecing = new DateTime();
                            if (!string.IsNullOrEmpty(txtfecing.Text))
                            {
                                if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecing))
                                {
                                    this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecing.Text));
                                    txtfecing.Focus();
                                    return;
                                }
                            }
                            DateTime facsal = new DateTime();
                            if (!string.IsNullOrEmpty(txtfecsal.Text))
                            {
                                if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out facsal))
                                {
                                    this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecsal.Text));
                                    txtfecsal.Focus();
                                    return;
                                }
                            }
                            coloboradores.Rows[item.ItemIndex]["FechaIngresoOC"] = fecing.ToString("yyyy-MM-dd");
                            coloboradores.Rows[item.ItemIndex]["FechaCaducidadOC"] = facsal.ToString("yyyy-MM-dd");
                            dtPermiso.Rows.Add(id, fecing.ToString("yyyy-MM-dd"), facsal.ToString("yyyy-MM-dd"), ddlTurnoOnlyControl.SelectedValue, "2");
                        }
                        else
                        {
                            DateTime lfecing = new DateTime();
                            var fechainicio = Convert.ToDateTime(lblfecing.Text).ToString("dd/MM/yyyy");
                            if (!string.IsNullOrEmpty(fechainicio))
                            {
                                if (!DateTime.TryParseExact(fechainicio, "dd/MM/yyyy", enUS, DateTimeStyles.None, out lfecing))
                                {
                                    this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", fechainicio));
                                    lblfecing.Focus();
                                    return;
                                }
                            }
                            DateTime lfacsal = new DateTime();
                            var fechacaducidad = Convert.ToDateTime(lblfecsal.Text).ToString("dd/MM/yyyy");
                            if (!string.IsNullOrEmpty(fechacaducidad))
                            {
                                if (!DateTime.TryParseExact(fechacaducidad, "dd/MM/yyyy", enUS, DateTimeStyles.None, out lfacsal))
                                {
                                    this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", fechacaducidad));
                                    lblfecsal.Focus();
                                    return;
                                }
                            }
                            coloboradores.Rows[item.ItemIndex]["FechaIngresoOC"] = lfecing.ToString("yyyy-MM-dd");
                            coloboradores.Rows[item.ItemIndex]["FechaCaducidadOC"] = lfacsal.ToString("yyyy-MM-dd");
                            dtPermiso.Rows.Add(id, lfecing.ToString("yyyy-MM-dd"), lfacsal.ToString("yyyy-MM-dd"), ddlTurnoOnlyControl.SelectedValue, "2");
                        }
                    }
                }
                Boolean validasolicitud = false;
                RegistraPermisoPermanenteOnlyControl(out validasolicitud, dtPermiso);
                if (!validasolicitud)
                {
                    coloboradores.AcceptChanges();
                    coloboradores.TableName = "Colaboradores";
                    StringWriter sw = new StringWriter();
                    coloboradores.WriteXml(sw);
                    String xmlColaboradores = sw.ToString();
                    New_ExportFileUpload();//23-11-2020
                    //ExportFileUpload();
                    string mensaje = null;
                    string nombreempresa = CslHelper.getShiperName(rucempresa);
                    if (!credenciales.ApruebaSolicitudPermisoDeAcceso(
                        numsolicitudempresa,
                        rucempresa,
                        nombreempresa,
                        useremail,
                        xmlColaboradores,
                        xmlDocumentos,
                        Page.User.Identity.Name.ToUpper(),
                        out mensaje))
                    {
                        this.Alerta(mensaje);
                    }
                    else
                    {
                        Response.Write("<script language='JavaScript'>var r=alert('Solicitud " + mensajeok + " exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                    }
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
                    numsolicitudempresa = Request.QueryString["numsolicitud"];
                }
                DataTable dtRechazados = new DataTable();
                dtRechazados.Columns.Add("Cedula");
                dtRechazados.Columns.Add("Nombre");
                dtRechazados.Columns.Add("Comentario");
                //DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;
                //if (dtDocSol == null)
                //{
                //    this.Alerta("NO tiene documentos rechazados.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud.");
                //    return;
                //}
                //if (dtDocSol.Rows.Count == 0)
                //{
                //    this.Alerta("NO tiene documentos rechazados.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud.");
                //    return;
                //}
                var bandera = false;
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    Label lblnombres = item.FindControl("lblnombres") as Label;
                    Label lblapellidos = item.FindControl("lblapellidos") as Label;
                    if (chkRevisado.Checked)
                    {
                        if (string.IsNullOrEmpty(tcomentario.Text))
                        {
                            this.Alerta("Escriba el motivo del rechazo.");
                            tcomentario.Focus();
                            return;
                        }
                        bandera = true;
                        dtRechazados.Rows.Add(lblcipas.Text, lblnombres.Text + " " + lblapellidos.Text, tcomentario.Text);
                    }
                    //var result = from myRow in dtDocSol.AsEnumerable()
                    //             where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true && myRow.Field<string>("Cedula") == lblcipas.Text
                    //             select myRow;
                    //DataTable dt = result.AsDataView().ToTable();
                    //if (dt.Rows.Count == 0)
                    //{
                    //    this.Alerta("La Cedula: *" + lblcipas.Text + "*\\nNO Tiene documentos rechazados.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud.");
                    //    return;
                    //}
                }
                //var resultVehiculo = from myRow in dtDocSol.AsEnumerable()
                //                     where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true
                //                     select myRow;
                //DataTable dtColaboradores = resultVehiculo.AsDataView().ToTable();
                //dtColaboradores.AcceptChanges();
                //dtColaboradores.TableName = "Colaboradores";
                if (!bandera)
                {
                    this.Alerta("No tiene ningun colaborador rechazado en la solicitud.");
                    return;
                }
                dtRechazados.TableName = "Colaboradores";
                StringWriter sw = new StringWriter();
                dtRechazados.WriteXml(sw);
                String xmlColaboradores = sw.ToString();
                string nombreempresa = CslHelper.getShiperName(rucempresa);
                string mensaje = null;
                if (!credenciales.RechazaSolicitudColaboradorPermisoDeAcceso(
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

        private void New_ExportFileUpload()
        {

            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");

            if (fuAdjuntarFactura.HasFile)
            {
                string rutafile = Server.MapPath(fuAdjuntarFactura.FileName);
                string finalname;
                var p = CSLSite.app_start.CredencialesHelper.UploadFile(Server.MapPath(fuAdjuntarFactura.FileName), fuAdjuntarFactura.PostedFile.InputStream, out finalname);
                if (!p)
                {
                    this.Alerta(finalname);
                    return;
                }
                dtDocumentos.Rows.Add(numsolicitudempresa, finalname);

            }

            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            StringWriter sw = new StringWriter();
            dtDocumentos.WriteXml(sw);
            xmlDocumentos = sw.ToString();
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
            dtColaboradoresAprobados.Columns.Add("CARGO");
            var nomeemp = CslHelper.getShiperName(rucempresa);
            dtAprobados = credenciales.GetSolicitudColaboradorOnlyControl(numsolicitudempresa);
            for (int i = 0; i < dtAprobados.Rows.Count; i++)
            {
                dtColaboradoresAprobados.Rows.Add(dtAprobados.Rows[i][2], dtAprobados.Rows[i][3], dtAprobados.Rows[i][3], nomeemp, dtAprobados.Rows[i][11]);
            }
            dsColaboradores = new DataSet();
            dsColaboradores.Tables.Add(dtColaboradoresAprobados);
            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_PEATON(dsColaboradores, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            if (registros_actualizados_incorrecto > 0)
            {
                this.Alerta(error_consulta);
                validacolaboradores = true;
                return;
            }
            else if (!string.IsNullOrEmpty(error_consulta))
            {
                this.Alerta(error_consulta);
                validacolaboradores = true;
                return;
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
                this.Alerta("Hubo un error al intentar registrar los siguientes vehiculos en el metodo AC_R_PERSONA_PEATON, revise a continuación. <br />" + errorvehiculo);
                //validacolaboradores = true;
                return;
            }
        }
        private void RegistraPermisoPermanenteOnlyControl(out bool validacolaboradores, DataTable dtPermiso/*DateTime fecing, DateTime fecsal, string cedula, string empresa*/)
        {
            DataTable dtDocumentos = new DataTable();
            validacolaboradores = false;
            Int32 registros_actualizados_correcto = 0;
            Int32 registros_actualizados_incorrecto = 0;
            String error_consulta = string.Empty;
            //var dtPermisosDeAcceso = credenciales.GetPermisoAccesoColaborador(cedula, rucempresa);
            //DataTable dtPermiso = new DataTable();
            //dtPermiso.Columns.Add("ID");
            //dtPermiso.Columns.Add("F_INGRESO");
            //dtPermiso.Columns.Add("F_SALIDA");
            //for (int i = 0; i < dtPermisosDeAcceso.Rows.Count; i++)
            //{
            //    var resultPersonasOnlyControl = from myRow in dsPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0].AsEnumerable()
            //                                    where myRow.Field<string>("CEDULA") == dtPermisosDeAcceso.Rows[i]["CEDULA"].ToString()
            //                                    select myRow;
            //    DataTable dt = resultPersonasOnlyControl.AsDataView().ToTable();
            //    if (resultPersonasOnlyControl.AsDataView().ToTable().Rows.Count == 0)
            //    {
            //        var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", "No se encontro el colaborador: " + dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString()));
            //        var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
            //        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            //        validacolaboradores = true;
            //        return;
            //    }
            //    var id = resultPersonasOnlyControl.AsDataView().ToTable().Rows[i]["ID"];
            //    dtPermiso.Rows.Add(id, fecing.ToString("yyyy-MM-dd"), fecsal.ToString("yyyy-MM-dd"));
            //}
            DataSet dsPermiso = new DataSet();
            DataSet dsErrorAC_R_PERMISO = new DataSet();
            dsPermiso.Tables.Add(dtPermiso);
            dsErrorAC_R_PERMISO = onlyControl.AC_R_PERMISO(dsPermiso, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            if (dsErrorAC_R_PERMISO.Tables[0].Rows.Count != 0)
            {
                if (dsErrorAC_R_PERMISO.Tables[0].Rows[0]["ERROR"].ToString().Substring(0, 15) != "Error Duplicado")
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
                }
            }
        }
    }
}