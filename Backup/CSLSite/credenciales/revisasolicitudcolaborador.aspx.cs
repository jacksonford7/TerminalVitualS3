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
    public partial class revisasolicitudcolaborador : System.Web.UI.Page
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
            get { return (string)Session["rucempresarevisasolicitudcolaborador"]; }
            set { Session["rucempresarevisasolicitudcolaborador"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailrevisasolicitudcolaborador"]; }
            set { Session["useremailrevisasolicitudcolaborador"] = value; }
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
                foreach (string var in Request.QueryString)
                {
                    rucempresa = Request.QueryString["ruc"];
                }
                //rucempresa = rucempresa;
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
                var tablixVehiculo = credenciales.GetSolicitudColaborador(numsolicitudempresa);
                tablePagination.DataSource = tablixVehiculo;
                tablePagination.DataBind();
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (chkRevisado.Checked)
                    {
                        //chkRevisado.Enabled = false;
                        tcomentario.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Session["dtDocumentosrevisasolicitudcolaborador"] = new DataTable();
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean banderafac = false;
                if (factura.Visible)
                {
                    if (!fuAdjuntarFactura.HasFile)
                    {
                        this.Alerta("Adjunte la factura por favor.");
                        return;
                    }
                    banderafac = true;
                }
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString["numsolicitud"];
                }
                List<string> listCedulas = new List<string>();
                DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (!chkRevisado.Checked)
                    {
                        var nomina = credenciales.GetConsultaNomina(rucempresa, lblcipas.Text);
                        if (nomina.Rows[0]["MENSAJE"].ToString() == "NUEVO" || nomina.Rows[0]["MENSAJE"].ToString() == "SEMPRESA" || nomina.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                        {
                            if (nomina.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                            {
                                //Emisión
                                foreach (string var in Request.QueryString)
                                {
                                    this.Alerta("La Solicitud pertenece a la Empresa:\\n *" + Request.QueryString["razonsocial"].ToString() + "\\nSin embargo el Colaborador:\\n *" + lblcipas.Text + " - " + nomina.Rows[0]["NOMBRES"].ToString() + "\\nEsta registrado en la Empresa:\\n *" + nomina.Rows[0]["EMPE_NOM"].ToString());
                                    return;
                                }
                            }
                        }
                    }
                    //if (chkRevisado.Checked == false && !string.IsNullOrEmpty(tcomentario.Text))
                    //{
                    //    this.Alerta("Marque la casilla del Comentario de rechazo. \\n Cedula: " + lblcipas.Text);
                    //    return;
                    //}
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
                
                DataTable dtColaboradores = credenciales.GetSolicitudColaborador(numsolicitudempresa);
                var resultAprobados = from myRow in dtColaboradores.AsEnumerable()
                                      where !listCedulas.Contains(myRow.Field<string>("CIPAS"))
                                      select myRow;
                DataTable dtAprobados = resultAprobados.AsDataView().ToTable();
                if (dtAprobados.Rows.Count == 0)
                {
                    this.Alerta("Tiene un unico colaborador rechazado.\\n" + "Revise la información antes de continuar con la " + mensajefac + " de la solicitud.\\n Si quiere continuar de click en Rechazar.");
                    return;
                }
                var coloboradores = credenciales.GetSolicitudColaborador(numsolicitudempresa);
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
                if (banderafac)
                {
                    ExportFileUpload();
                }
                else
                {
                    //Boolean validasolicitud = false;
                    //RegistraColaboradoresOnlyControl(out validasolicitud);
                    //if (validasolicitud)
                    //{
                    //    return;
                    //}
                }
                sw = new StringWriter();
                dtDocSol.TableName = "Documentos";
                dtDocSol.WriteXml(sw);
                String xmlDocumentosRechazados = sw.ToString();
                string mensaje = null;
                //string nombreempresa = CslHelper.getShiperName(rucempresa);
                if (!credenciales.ApruebaSolicitudColaborador(
                    numsolicitudempresa,
                    rucempresa,
                    //nombreempresa,
                    //useremail,
                    xmlColaboradores,
                    xmlDocumentos,
                    xmlDocumentosRechazados,
                    Page.User.Identity.Name.ToUpper(),
                    banderafac,
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    //this.Alerta("Solicitud aprobada exitosamente, en unos minutos recibirá una notificación via mail.");
                    //botonera.Visible = false;
                    //factura.Visible = false;
                    //salir.Visible = true;
                    //if (!string.IsNullOrEmpty(error))
                    //{
                    //    Response.Write("<script language='JavaScript'>var r=alert('Solicitud " + mensajeok + " exitosamente, en unos minutos recibirá una notificación via mail. \\n" + " *Sin embargo " + error + "');if(r==true){window.close()}else{window.close()};</script>");
                    //}
                    //else
                    //{
                    //ConsultaInfoSolicitud();
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud " + mensajeok + " exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                    //}
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
                if (!credenciales.RechazaSolicitudColaborador(
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
    }
}