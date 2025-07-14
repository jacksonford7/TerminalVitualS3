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
using ConectorN4;
using csl_log;

namespace CSLSite
{
    public partial class revisasolicitudempresa : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        private DataTable dtDocumentos = new DataTable();
        private String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudrevisasolicitudempresa"]; }
            set { Session["numsolicitudrevisasolicitudempresa"] = value; }
        }
        private string rucempresa
        {
            get { return (string)Session["rucempresarevisasolicitudempresa"]; }
            set { Session["rucempresarevisasolicitudempresa"] = value; }
        }
        private string useremail
        {
            get { return (string)Session["useremailrevisasolicitudempresa"]; }
            set { Session["useremailrevisasolicitudempresa"] = value; }
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
                sinresultado.Visible = false;
                try
                {
                    ConsultaInfoSolicitud();
                }
                catch (Exception ex)
                {
                    Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
                }
            }
        }
        private void ConsultaInfoSolicitud()
        {
            foreach (string var in Request.QueryString)
            {
                numsolicitudempresa = Request.QueryString[var];
            }
            if (!credenciales.GetSolicitudEstado(numsolicitudempresa))
            {
                botonera.Visible = false;
                //factura.Visible = false;
                salir.Visible = true;
            }            
            var tablixDocumentos = credenciales.GetDocumentosEmpresaXNumSolicitud(numsolicitudempresa);
            tablePaginationDocumentos.DataSource = tablixDocumentos;
            tablePaginationDocumentos.DataBind();
            //xfinder.Visible = true;
            alerta.Attributes["class"] = string.Empty;
            alerta.Attributes["class"] = "msg-info";

            var tablixSolicitudEmpresa = credenciales.GetSolicitudEmpresa(numsolicitudempresa);
            for (int i = 0; i <= tablixSolicitudEmpresa.Rows.Count - 1; i++)
            {
                txttipcli.Text = tablixSolicitudEmpresa.Rows[i][0].ToString();
                txtrazonsocial.Text = tablixSolicitudEmpresa.Rows[i][3].ToString();
                txtruccipas.Text = tablixSolicitudEmpresa.Rows[i][4].ToString();
                txtactividadcomercial.Text = tablixSolicitudEmpresa.Rows[i][5].ToString();
                txtdireccion.Text = tablixSolicitudEmpresa.Rows[i][6].ToString();
                txttelofi.Text = tablixSolicitudEmpresa.Rows[i][7].ToString();
                txtcontacto.Text = tablixSolicitudEmpresa.Rows[i][8].ToString();
                txttelcelcon.Text = tablixSolicitudEmpresa.Rows[i][9].ToString();
                txtmailinfocli.Text = tablixSolicitudEmpresa.Rows[i][10].ToString();
                txtcertificaciones.Text = tablixSolicitudEmpresa.Rows[i][11].ToString();
                turl.Text = tablixSolicitudEmpresa.Rows[i][12].ToString();
                txtafigremios.Text = tablixSolicitudEmpresa.Rows[i][13].ToString();
                txtrefcom.Text = tablixSolicitudEmpresa.Rows[i][14].ToString();
                txtreplegal.Text = tablixSolicitudEmpresa.Rows[i][15].ToString();
                txttelreplegal.Text = tablixSolicitudEmpresa.Rows[i][16].ToString();
                txtdirdomreplegal.Text = tablixSolicitudEmpresa.Rows[i][17].ToString();
                txtci.Text = tablixSolicitudEmpresa.Rows[i][18].ToString();
                tmailRepLegal.Text = tablixSolicitudEmpresa.Rows[i][19].ToString();
                txtmailebilling.Text = tablixSolicitudEmpresa.Rows[i][20].ToString();
                //if (tablixSolicitudEmpresa.Rows[i]["ESTADO"].ToString() == "R")
                //{
                txtmotivorechazo.Text = tablixSolicitudEmpresa.Rows[i]["OBSERVACION"].ToString();
                hfRazSocial.Value = txtrazonsocial.Text;
                //    txtmotivorechazo.ForeColor = System.Drawing.Color.Red;
                //}
                //else
                //{
                //    txtmotivorechazo.Text = "";
                //}
            }
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtmotivorechazo.Text))
                {
                    this.Alerta("Tiene una observación de rechazo.\\n" + "Revise la información antes de continuar con la aprobación de la solicitud de registro de empresa.");
                    return;
                }
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString[var];
                }
                var estadosolicitud = credenciales.GetEstadoEmpresa(txtruccipas.Text.Trim()).Rows[0]["ESTADO"].ToString();
                if (estadosolicitud == "ACTUALIZA")
                {
                    foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                    {
                        CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                        TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                        if (chkRevisado.Checked == true && string.IsNullOrEmpty(tcomentario.Text))
                        {
                            this.Alerta("Tiene documentos rechazados.\\n" + "Revise la información antes de continuar con la aprobación de la solicitud de registro de empresa.");
                            return;
                        }
                    }
                    var errorRegistraEmpresaN4Actualiza = RegistraEmpresaN4();
                    if (!string.IsNullOrEmpty(errorRegistraEmpresaN4Actualiza))
                    {
                        this.Alerta(errorRegistraEmpresaN4Actualiza);
                        return;
                    }
                    string mensaje = null;
                    string nombreempresa = CslHelper.getShiperName(rucempresa);
                    if (!credenciales.ActualizaSolicitudEmpresa(
                        numsolicitudempresa,
                        rucempresa,
                        nombreempresa,
                        useremail,
                        Page.User.Identity.Name.ToUpper(),
                        Request.UserHostAddress.ToString(),
                        out mensaje))
                    {
                        this.Alerta(mensaje);
                    }
                    else
                    {
                        Response.Write("<script language='JavaScript'>var r=alert('Solicitud finalizada la Empresa ha sido registrada exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                    }
                }
                else
                {
                    //Rela Empresa en N4
                    //var datosempresa = credenciales.GetDatosEmpresa(numsolicitudempresa, "P");
                    //string existeempresa = CslHelper.getShiperName(datosempresa.Rows[0]["RUCCIPAS"].ToString());
                    //if (existeempresa.Trim().ToUpper() == "NO REGISTRADO")
                    //{
                        var errorRegistraEmpresaN4Finaliza = RegistraEmpresaN4();
                        if (!string.IsNullOrEmpty(errorRegistraEmpresaN4Finaliza))
                        {
                            this.Alerta(errorRegistraEmpresaN4Finaliza);
                            return;
                        }
                    //}

                    Int32 registros_actualizados_correcto = 0;
                    Int32 registros_actualizados_incorrecto = 0;
                    String error_consulta = string.Empty;
                    String error = string.Empty;
                    DataSet dsErrorAC_R_EMPRESA = new DataSet();
                    DataSet dsErrorAC_C_EMPRESA = new DataSet();
                    DataSet dsEmpresa = new DataSet();
                    DataTable dtEmpresaAprobada = new DataTable();
                    dtEmpresaAprobada.Columns.Add("ID");
                    dtEmpresaAprobada.Columns.Add("EM_RUC");
                    dtEmpresaAprobada.Columns.Add("NOMBRE");
                    dtEmpresaAprobada.Columns.Add("CONTACTO");
                    dtEmpresaAprobada.Columns.Add("TELEFONO");
                    dtEmpresaAprobada.Columns.Add("DIRECCION");
                    dtEmpresaAprobada.Columns.Add("MAIL");
                    dtEmpresaAprobada.Columns.Add("REPRESENTANTE");
                    var name = txtrazonsocial.Text.Trim();
                    if (name.Length > 100)
                    {
                        name = txtrazonsocial.Text.Trim().Substring(0, 100);
                    }
                    var direc = txtdireccion.Text.Trim();
                    if (direc.Length > 70)
                    {
                        direc = txtdireccion.Text.Trim().Substring(0, 70);
                    }
                    var emailcli = txtmailinfocli.Text.Trim();
                    if (emailcli.Length > 50)
                    {
                        emailcli = txtmailinfocli.Text.Trim().Substring(0, 50);
                    }
                    var contacto = txtcontacto.Text.Trim();
                    if (contacto.Length > 30)
                    {
                        contacto = txtcontacto.Text.Trim().Substring(0, 30);
                    }

                    dtEmpresaAprobada.Rows.Add(string.Empty, txtruccipas.Text, name, contacto, txttelofi.Text, direc, emailcli, txtreplegal.Text);
                    //Consulta la Empresa en OnlyControl
                    dsErrorAC_C_EMPRESA = onlyControl.AC_C_EMPRESA(txtruccipas.Text, 0, ref error_consulta) as DataSet;
                    if (!string.IsNullOrEmpty(error_consulta))
                    {
                        var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                        var number = log_csl.save_log<Exception>(ex, "revisasolicitudempresa", "btsalvar_Click", "1", Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                        return;
                    }
                    DataTable dt = dsErrorAC_C_EMPRESA.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                    }
                    else
                    {
                        dsEmpresa.Tables.Add(dtEmpresaAprobada);
                        //Registra la Empresa en OnlyControl
                        dsErrorAC_R_EMPRESA = onlyControl.AC_R_EMPRESA(dsEmpresa, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                        if (registros_actualizados_incorrecto > 0)
                        {
                            if (string.IsNullOrEmpty(error_consulta))
                            {
                                var dserror = dsErrorAC_R_EMPRESA.DefaultViewManager.DataSet.Tables;
                                var dterror = dserror[0];
                                var error2 = string.Empty;
                                for (int i2 = 0; i2 < dterror.Rows.Count; i2++)
                                {
                                    error2 = error2 + " \\n *" +
                                           "Error: " + dterror.Rows[i2]["ERROR"].ToString() + " \\n ";
                                }
                                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_EMPRESA():{0}", error2));
                                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                                return;
                            }
                            var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_EMPRESA():{0}", error_consulta));
                            var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                            this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                            return;
                        }
                        else if (!string.IsNullOrEmpty(error_consulta))
                        {
                            var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_EMPRESA():{0}", error_consulta));
                            var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                            this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                            return;
                        }
                        DataTable dt2 = new DataTable();
                        dt2 = dsErrorAC_R_EMPRESA.Tables[0];
                        if (dt2.Rows.Count > 0)
                        {
                        }
                    }
                    foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                    {
                        CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                        TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                        if (chkRevisado.Checked == true && string.IsNullOrEmpty(tcomentario.Text))
                        {
                            this.Alerta("Tiene documentos rechazados.\\n" + "Revise la información antes de continuar con la aprobación de la solicitud de registro de empresa.");
                            return;
                        }
                    }
                    string mensaje = null;
                    string nombreempresa = CslHelper.getShiperName(rucempresa);
                    if (!credenciales.FinalizaSolicitudEmpresa(
                        numsolicitudempresa,
                        rucempresa,
                        nombreempresa,
                        useremail,
                        Page.User.Identity.Name.ToUpper(),
                        Request.UserHostAddress.ToString(),
                        out mensaje))
                    {
                        this.Alerta(mensaje);
                    }
                    else
                    {
                        Response.Write("<script language='JavaScript'>var r=alert('Solicitud finalizada la Empresa ha sido registrada exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
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
                    numsolicitudempresa = Request.QueryString[var];
                }
                //if (string.IsNullOrEmpty(txtmotivorechazo.Text))
                //{
                    List<string> listRechazados = new List<string>();
                    foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                    {
                        CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                        TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                        if (chkRevisado.Checked == false)
                        {
                            listRechazados.Add(chkRevisado.Checked.ToString());
                        }
                    }
                    var tablixDocumentos = credenciales.GetDocumentosEmpresaXNumSolicitud(numsolicitudempresa);
                    if (listRechazados.Count == tablePaginationDocumentos.Items.Count && string.IsNullOrEmpty(txtmotivorechazo.Text))
                    {
                        this.Alerta("NO tiene observación o documentos para el rechazo de la solicitud.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud de registro de empresa.");
                        return;
                    }
                    foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                    {
                        CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                        TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                        if (chkRevisado.Checked == true && string.IsNullOrEmpty(tcomentario.Text))
                        {
                            this.Alerta("Escriba el Comentario del documento rechazado.");
                            return;
                        }
                    }
                //}
                DataTable dtEmpresa = credenciales.GetDocumentosEmpresaXNumSolicitud(numsolicitudempresa);
                dtEmpresa.Columns.Add("DOCUMENTORECHAZADO");
                //dtEmpresa.Columns.Add("COMENTARIO");
                foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (chkRevisado.Checked)
                    {
                        dtEmpresa.Rows[item.ItemIndex]["DOCUMENTORECHAZADO"] = chkRevisado.Checked;
                        dtEmpresa.Rows[item.ItemIndex]["COMENTARIO"] = tcomentario.Text;
                    }
                }
                var resultAprobados = from myRow in dtEmpresa.AsEnumerable()
                                      where Convert.ToBoolean(myRow.Field<string>("DOCUMENTORECHAZADO")) == true
                                      select myRow;
                DataTable dtAprobados = resultAprobados.AsDataView().ToTable();
                dtAprobados.AcceptChanges();
                dtAprobados.TableName = "Empresa";
                StringWriter sw = new StringWriter();
                dtAprobados.WriteXml(sw);
                String xmlEmpresa = sw.ToString();
                string mensaje = null;
                string nombreempresa = CslHelper.getShiperName(rucempresa);
                var encryptnumsolicitud = QuerySegura.EncryptQueryString(numsolicitudempresa);
                //var clave =  QuerySegura.EncryptQueryString(rucempresa);
                if (!credenciales.RechazaSolicitudEmpresa(
                    encryptnumsolicitud,
                    //clave,
                    numsolicitudempresa,
                    txtmotivorechazo.Text,
                    rucempresa,
                    nombreempresa,
                    useremail,
                    xmlEmpresa,
                    Page.User.Identity.Name.ToUpper(),
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    //this.Alerta("Solicitud rechazada exitosamente, en unos minutos recibirá una notificación via mail.");
                    //ConsultaInfoSolicitud();
                    //botonera.Visible = false;
                    //factura.Visible = false;
                    //salir.Visible = true;
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
            string directorio = string.Empty;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("Placa");
            dtDocumentos.Columns.Add("IdDocEmp");
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");
            dtDocumentos.Columns.Add("NombreArchivo");
            dtDocumentos.Columns.Add("Extension");
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                TextBox txtidsolicitud = item.FindControl("txtidsolicitud") as TextBox;
                TextBox txtiddocemp = item.FindControl("txtiddocemp") as TextBox;
                if (fsupload.HasFile)
                {
                    string rutafile = Server.MapPath(fsupload.FileName);
                    string extensionfile = Path.GetExtension(fsupload.PostedFile.FileName);
                    directorio = Path.GetDirectoryName(rutafile);
                    string nombrearchivo = Path.GetFileNameWithoutExtension(fsupload.FileName);
                    //if (File.Exists(rutafile))
                    //{
                    //    File.Delete(rutafile);
                    //}
                    //if (Directory.Exists(directorio))
                    //{
                    //    Directory.Delete(directorio);
                    //}
                    System.IO.Directory.CreateDirectory(@directorio);
                    fsupload.SaveAs(rutafile);
                    string[] files = Directory.GetFiles(directorio, "*" + nombrearchivo + extensionfile);
                    foreach (string s in files)
                    {
                        FileInfo fi = null;
                        try
                        {
                            fi = new FileInfo(s);
                            ExportFiles(fi.Directory.ToString(), fi.Name, nombrearchivo, extensionfile, txtidsolicitud.Text, txtiddocemp.Text);
                        }
                        catch (FileNotFoundException ex)
                        {
                            this.Alerta(ex.Message);
                            return;
                        }
                    }
                    //if (File.Exists(rutafile))
                    //{
                    //    File.Delete(rutafile);
                    //}
                }
                else
                {
                    //this.Alerta("Seleccione los documentos requeridos.");
                    //return;
                }
            }
            //if (Directory.Exists(directorio))
            //{
            //    Directory.Delete(directorio);
            //}
            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            DataTable dtPadre = Session["dtDocumentosVehiculosSolicitud"] as DataTable;
            DataTable dtHijo = new DataTable();
            dtHijo.Columns.Add("Placa");
            dtHijo.Columns.Add("IdDocEmp");
            dtHijo.Columns.Add("IdSolicitud");
            dtHijo.Columns.Add("Ruta");
            dtHijo.Columns.Add("NombreArchivo");
            dtHijo.Columns.Add("Extension");
            for (int i = 0; i < dtDocumentos.Rows.Count; i++)
            {
                dtHijo.Rows.Add(dtDocumentos.Rows[i]["Placa"], dtDocumentos.Rows[i]["IdDocEmp"], dtDocumentos.Rows[i]["IdSolicitud"], dtDocumentos.Rows[i]["Ruta"], dtDocumentos.Rows[i]["NombreArchivo"], dtDocumentos.Rows[i]["Extension"]);
            }
            if (dtPadre != null)
            {
                for (int i = 0; i < dtPadre.Rows.Count; i++)
                {
                    foreach (DataRow x in dtPadre.Rows)
                    {
                        if ((string)x[0] == numsolicitudempresa)
                        {
                            x.Delete();
                            break;
                        }
                    }
                }
                for (int i = 0; i < dtPadre.Rows.Count; i++)
                {
                    //var results = from myRow in dtPadre.AsEnumerable()
                    //              where myRow.Field<string>("Placa") == splaca
                    //              select new
                    //              {
                    //                  Placa = myRow.Field<string>("Placa")
                    //              };
                    //foreach (var item in results)
                    //{
                    //    if (item.Placa == splaca)
                    //    {
                            
                    //    }
                    //}
                    //if (dtPadre.Rows[i]["Placa"] == splaca)
                    //{
                    //    dtPadre.Rows.RemoveAt(i);
                    //}
                    //else
                    //{
                    dtHijo.Rows.Add(dtPadre.Rows[i]["Placa"], dtPadre.Rows[i]["IdDocEmp"], dtPadre.Rows[i]["IdSolicitud"], dtPadre.Rows[i]["Ruta"], dtPadre.Rows[i]["NombreArchivo"], dtPadre.Rows[i]["Extension"]);
                    //}
                }
            }
            Session["dtDocumentosVehiculosSolicitud"] = new DataTable();
            Session["dtDocumentosVehiculosSolicitud"] = dtHijo;
            DataTable datos = Session["dtDocumentosVehiculosSolicitud"] as DataTable;
            Response.Write("<script language='JavaScript'>window.close();</script>");
        }
        private void ExportFiles(string path, string filename, string name, string extensionfile, string sidsolicitud, string siddocemp)
        {
            path = path + "\\";
            if (File.Exists(path + filename))
            {
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                var llistaDoc = new List<listaDoc>();
                var ilistaDoc = new listaDoc(Convert.ToInt32(sidsolicitud), Convert.ToInt32(siddocemp), rutaServer + filename);
                llistaDoc.Add(ilistaDoc);
                foreach (var itemlistaDoc in llistaDoc)
                {
                    var rutafile = path + filename /*+ '_' + DateTime.Now.ToString("dd-MM-yyyy")*/;
                    dtDocumentos.Rows.Add(numsolicitudempresa, itemlistaDoc.iddocemp, itemlistaDoc.idtipemp, path, name, extensionfile);
                }
            }
        }
        private string RegistraEmpresaN4()
        {
            try
            {
                var name = txtrazonsocial.Text.Trim();
                if (name.Length > 80)
                {
                    name = txtrazonsocial.Text.Trim().Substring(0, 80);
                }
                var direc = txtdireccion.Text.Trim();
                if (direc.Length > 60)
                {
                    direc = txtdireccion.Text.Trim().Substring(0, 60);
                }
                var tcontacto = txtcontacto.Text.Trim();
                if (tcontacto.Length > 80)
                {
                    tcontacto = txtcontacto.Text.Trim().Substring(0, 80);
                }
                var tk = HttpContext.Current.Request.Cookies["token"];
                ObjectSesion sesObj = new ObjectSesion();
                sesObj.clase = "revisasolicitudempresa";
                sesObj.metodo = "RegistraEmpresaN4";
                sesObj.transaccion = "RegistraLaEmpresaEnN4";
                sesObj.usuario = Page.User.Identity.Name.ToUpper();
                sesObj.token = tk.Value;
                String xmlns_argo = "http://www.navis.com/argo";
                String xmlns_xsi = "http://www.w3.org/2001/XMLSchema-instance";
                String xsi_schemaLocation = "http://www.navis.com/argo snx.xsd";
                XDocument xmlRegistraEmpresaN4 = new XDocument();
                xmlRegistraEmpresaN4 = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("argo_n4_snx",
                            new XAttribute("xmlns_n4_argo", xmlns_argo),
                            new XAttribute("xmlns_n4_xsi", xmlns_xsi),
                            new XAttribute("xsi_n4_schemaLocation", xsi_schemaLocation),
                        new XElement("trucking-company",
                            new XAttribute("id", txtruccipas.Text.Trim()),
                            new XAttribute("name", name),
                            new XAttribute("is-eq-operator", "N"),
                            new XAttribute("is-eq-owner", "N"),
                            new XAttribute("status", "OK"),
                            new XAttribute("is-exam-site-carrier", "N"),
                            new XAttribute("is-seized-cargo-carrier", "N"),
                            new XAttribute("life-cycle-state", "ACT"),
                            new XElement("contact-info",
                                new XAttribute("address-line-1", direc),
                                new XAttribute("address-line-2", direc),
                                new XAttribute("name", tcontacto),
                                new XAttribute("city", "GUAYAQUIL"),
                                new XAttribute("country", "EC"),
                                new XAttribute("state", "001"),
                                new XAttribute("telephone", txttelofi.Text),
                                new XAttribute("email", (txtmailinfocli.Text + ";" + tmailRepLegal.Text).Trim()),
                                new XAttribute("web-site", (txtmailebilling.Text).Trim())
                                ))));
                var varXmlRegistraEmpresaN4 = xmlRegistraEmpresaN4.ToString();
                varXmlRegistraEmpresaN4 = varXmlRegistraEmpresaN4.Replace("_n4_", ":");
                var msgN4 = credenciales.registraEmpresaN4(sesObj, varXmlRegistraEmpresaN4.ToString());
                if (!string.IsNullOrEmpty(msgN4))
                {
                    return msgN4;
                }
                //xmlRegistraEmpresaN4 = new XDocument();
                //xmlRegistraEmpresaN4 = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                //        new XElement("argo_n4_snx",
                //            new XAttribute("xmlns_n4_argo", xmlns_argo),
                //            new XAttribute("xmlns_n4_xsi", xmlns_xsi),
                //            new XAttribute("xsi_n4_schemaLocation", xsi_schemaLocation),
                //        new XElement("shipper-consignee",
                //            new XAttribute("id", txtruccipas.Text.Trim()),
                //            new XAttribute("name", txtrazonsocial.Text.Trim()),
                //            new XAttribute("credit-status", "OAC"),
                //            new XAttribute("is-eq-operator", "N"),
                //            new XAttribute("is-eq-owner", "N"),
                //            new XAttribute("life-cycle-state", "ACT"),
                //            new XElement("contact-info",
                //                new XAttribute("address-line-1", txtdireccion.Text.Trim()),
                //                new XAttribute("address-line-2", txtdireccion.Text.Trim()),
                //                new XAttribute("name", txtcontacto.Text.Trim()),
                //                new XAttribute("city", "GUAYAQUIL"),
                //                new XAttribute("country", "EC"),
                //                new XAttribute("state", "001"),
                //                new XAttribute("telephone", txttelofi.Text),
                //                new XAttribute("email", (txtmailinfocli.Text + ";" + tmailRepLegal.Text).Trim()),
                //                new XAttribute("web-site", (txtmailebilling.Text).Trim())
                //                ))));
                //varXmlRegistraEmpresaN4 = xmlRegistraEmpresaN4.ToString();
                //varXmlRegistraEmpresaN4 = varXmlRegistraEmpresaN4.Replace("_n4_", ":");
                //msgN4 = credenciales.registraEmpresaN4(sesObj, varXmlRegistraEmpresaN4.ToString());
                return msgN4;
            }
            catch (Exception ex)
            {
                var msgN4 = ex.Message.ToString();
                return msgN4;
            }
        }
        private class listaDoc
        {
            public int idtipemp { get; set; }
            public int iddocemp { get; set; }
            public string rutafile { get; set; }
            public listaDoc(int idtipemp, int iddocemp, string rutafile) { this.idtipemp = idtipemp; this.iddocemp = iddocemp; this.rutafile = rutafile.Trim(); }
        }
        protected void ibUpdateEmpresaOk_Click(object sender, ImageClickEventArgs e)
        {
            foreach (string var in Request.QueryString)
            {
                numsolicitudempresa = Request.QueryString[var];
            }
            string mensaje = string.Empty;
            if (!credenciales.ActualizaRazonSocialEmpresa(
                    numsolicitudempresa,
                    txtrazonsocial.Text.Trim(),
                    out mensaje))
            {
                this.Alerta(mensaje);
            }
            else
            {
                this.Alerta("Actualización exitosa.");
                return;
            }
        }
    }
}