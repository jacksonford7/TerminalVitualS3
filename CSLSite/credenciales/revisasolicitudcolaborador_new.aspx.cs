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

using CSLSite;
using BillionEntidades;

namespace CSLSite
{

    public partial class revisasolicitudcolaborador_new : System.Web.UI.Page
    {
        private static Int64? lm = -3;
        private string OError;
        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        private String xmlDocumentos;
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosrevisasolicitudcolaboradorPermanente"]; }
            set { Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] = value; }
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
                Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] = new DataTable();
                
                ConsultaInfoSolicitud();
            }
            factura.Visible = false;
            txttipcli.Text = Session["txttipcli.Text"] as string;
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
                       
                    }
                    else
                    {
                       
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
                     
                    }
                    else
                    {
                        alertafu.InnerHtml = "Adjunte el archivo en formato PDF";
                       
                    }
                    mensajefac = "finalización";
                    mensajeok = "finalizada";
                }

                var DescripcionTipoSolicitud = credenciales.GetDescripcionTipoSolicitudXNumSolicitud(numsolicitudempresa);

                for (int i = 0; i <= DescripcionTipoSolicitud.Rows.Count - 1; i++)
                {
                    Session["txttipcli.Text"] = DescripcionTipoSolicitud.Rows[i][0].ToString(); ;
                    txttipcli.Text = DescripcionTipoSolicitud.Rows[i][0].ToString();
                }
                //this.alerta.InnerHtml = "Confirme que los documentos del colaborador(es) sean los correctos.";
                Mostrar_Mensaje(1, "Confirme que los documentos del colaborador(es) sean los correctos.");
                Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] = new DataTable();
                var tablixVehiculo = credenciales.GetSolicitudColaborador_New(numsolicitudempresa);

                tablixVehiculo.Columns.Add("TIPOD");
                tablixVehiculo.Columns.Add("PERMISO");
                for (int i = 0; i < tablixVehiculo.Rows.Count; i++)
                {
                    var dtvalhuellafoto = aso_transportistas.GetValHuellaFoto(numsolicitudempresa, tablixVehiculo.Rows[i]["CIPAS"].ToString());
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

                    //CheckBox chkHuellaEstado = item.FindControl("chkHuellaEstado") as CheckBox;
                    //CheckBox chkFotoEstado = item.FindControl("chkFotoEstado") as CheckBox;
                    //var dtvalestadohuella = aso_transportistas.GetValidaHuella(tablixVehiculo.Rows[item.ItemIndex]["RUCCIPAS"].ToString(), tablixVehiculo.Rows[item.ItemIndex]["CIPAS"].ToString());
                    //var dtvalestadofoto = aso_transportistas.GetValidaFoto(tablixVehiculo.Rows[item.ItemIndex]["RUCCIPAS"].ToString(), tablixVehiculo.Rows[item.ItemIndex]["CIPAS"].ToString());
                    //if (dtvalestadohuella.Rows.Count > 0)
                    //{
                    //    if (dtvalestadohuella.Rows[0][0].ToString() == "1")
                    //    {
                    //        chkHuellaEstado.Checked = true;
                    //        chkHuellaEstado.Text = "HUELLA [OK]";
                    //    }
                    //    else
                    //    {
                    //        chkHuellaEstado.Checked = false;
                    //        chkHuellaEstado.Text = "HUELLA [NO]";
                    //    }
                    //}
                    //if (dtvalestadofoto.Rows.Count > 0)
                    //{
                    //    if (dtvalestadofoto.Rows[0][0].ToString() == "1")
                    //    {
                    //        chkFotoEstado.Checked = true;
                    //        chkFotoEstado.Text = "FOTO [OK]";
                    //    }
                    //    else
                    //    {
                    //        chkFotoEstado.Checked = false;
                    //        chkFotoEstado.Text = "FOTO [NO]";
                    //    }
                    //}
                }

             
                
            }
            catch (Exception ex)
            {
                Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] = new DataTable();
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }


       

    
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            
            try
            {
                Boolean banderafac = false;
                

                banderafac = false;
             
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString["numsolicitud"];
                }

                string Numero_Factura = this.TxtNumdocumento.Text.Trim();

                List<string> listCedulas = new List<string>();
                DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] as DataTable;

                foreach (DataRow item in dtDocSol.Rows)
                {
                    if (bool.Parse( item["DocumentoRechazado"].ToString()) == true)
                    {
                        foreach (RepeaterItem fila in tablePagination.Items)
                        {
                            Label lblcipasColab = fila.FindControl("lblcipas") as Label;
                            CheckBox chkRevisadoColab = fila.FindControl("chkRevisado") as CheckBox;

                            if (item["Cedula"].ToString() == lblcipasColab.Text)
                            {
                                chkRevisadoColab.Checked = true;
                                UPdetalle.Update();
                            }
                        }
                    }

                }


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

                DataTable dtColaboradores = credenciales.GetSolicitudColaborador_New(numsolicitudempresa);
                var resultAprobados = from myRow in dtColaboradores.AsEnumerable()
                                      //where !listCedulas.Contains(myRow.Field<string>("CIPAS"))
                                      select myRow;
                DataTable dtAprobados = resultAprobados.AsDataView().ToTable();

                //if (dtAprobados.Rows.Count == 0)
                //{
                //    this.Alerta("Tiene un unico colaborador rechazado.\\n" + "Revise la información antes de continuar con la " + mensajefac + " de la solicitud.\\n Si quiere continuar de click en Rechazar.");
                //    return;
                //}

                //****************************************************
                //ACTUALIZA ESTADO DE COLABORADOR RECHAZADO
                //****************************************************
                string v_mensaje = string.Empty;

                //foreach (RepeaterItem item in tablePagination.Items)
                //{
                //    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                //    Label txtNumeroSolicitudColab = item.FindControl("txtNumeroSolicitudColab") as Label;
                //    Label txtNumeroSolicitudColColab = item.FindControl("txtNumeroSolicitudColColab") as Label;
                //    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                //    if (chkRevisado.Checked)
                //    {
                //        credenciales.ActualizarEstadoColaborador(long.Parse(txtNumeroSolicitudColab.Text), long.Parse(txtNumeroSolicitudColColab.Text), "R", tcomentario.Text, Page.User.Identity.Name.ToUpper(), out v_mensaje);
                //    }
                //}

                var coloboradores = credenciales.GetSolicitudColaborador_New(numsolicitudempresa);

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
                           // where listCedulas.Contains(myRow.Field<string>("CIPAS"))
                            select myRow;

                dtCol = resultListaAprobados.AsDataView().ToTable();
                dtCol.AcceptChanges();
                dtCol.TableName = "Colaboradores";
                StringWriter sw = new StringWriter();
                dtCol.WriteXml(sw);

                String xmlColaboradores = sw.ToString();
               
                sw = new StringWriter();
                dtDocSol.TableName = "Documentos";
                dtDocSol.WriteXml(sw);
                String xmlDocumentosRechazados = sw.ToString();
                string mensaje = null;

                Boolean validasolicitud = false;

                if (!credenciales.ActualizarSolicitudColaborador_Fechas(
                         numsolicitudempresa,
                         out mensaje))
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btsalvar_Click), "btsalvar_Click", false, null, null, mensaje, null);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    Response.Write("<script language='JavaScript'>alert('" + mensaje + " - " + OError + "');</script>");
                }
               

                RegistraColaboradoresOnlyControl(out validasolicitud);

                if (!credenciales.ApruebaSolicitudColaborador_Transportista(
                        numsolicitudempresa,
                        rucempresa,
                        Numero_Factura,
                        0,
                        xmlColaboradores,
                        xmlDocumentos,
                        xmlDocumentosRechazados,
                        Page.User.Identity.Name.ToUpper(),
                        banderafac,
                        out mensaje))
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btsalvar_Click), "btsalvar_Click", false, null, null, mensaje, null);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    Response.Write("<script language='JavaScript'>alert('" + mensaje + " - " + OError + "');</script>");
                }
                else
                {

                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud " + mensajeok + " exitosamente.');if(r==true){window.close()}else{window.close()};</script>");

                }


              
            }
            catch (Exception ex)
            {
                //Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btsalvar_Click), "btsalvar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                Response.Write("<script language='JavaScript'>alert('" + OError + "');</script>");
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
                DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] as DataTable;
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
                if (!credenciales.RechazaSolicitudColaborador_New(
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
            string cMensajes;
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

            var empresa = credenciales.GetEmpresaColaborador(numsolicitudempresa).Rows[0]["RAZONSOCIAL"].ToString();
            dtAprobados = credenciales.GetSolicitudColaboradorOnlyControl(numsolicitudempresa);

            var nomeemp = CslHelper.getShiperName(rucempresa);

            List<Cls_TRANSP_Colaborador_Only> ListColaborador = Cls_TRANSP_Colaborador_Only.Datos_Colaborador(dtAprobados.Rows[0][2].ToString(), out cMensajes);

            if (!String.IsNullOrEmpty(cMensajes))
            {
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", cMensajes));            
                return;
            }

            DateTime fechaing;
            DateTime fechacad;
            CultureInfo enUS = new CultureInfo("en-US");

            fechaing = DateTime.Now;
            fechacad = fechaing.AddYears(2);

            var Only = ListColaborador.FirstOrDefault();

            for (int i = 0; i < dtAprobados.Rows.Count; i++)
            {
                dtColaboradoresAprobados.Rows.Add(dtAprobados.Rows[i][2],
                     dtAprobados.Rows[i]["APELLIDOS"].ToString(),
                    dtAprobados.Rows[i]["NOMBRES"].ToString(),
                    empresa,
                    Only.NOMINA_AREA1,
                    Only.NOMINA_DEP1,
                    Only.NOMINA_CAL1,
                    (string.IsNullOrEmpty(dtAprobados.Rows[i][12].ToString()) ? DateTime.Now.ToString("yyyy-MM-dd") : Convert.ToDateTime(dtAprobados.Rows[i][12].ToString()).ToString("yyyy-MM-dd")), //expiracion
                    dtAprobados.Rows[i][4].ToString(),
                    Only.NOMINA_EMPE,
                    Only.NOMINA_FING.HasValue ? Only.NOMINA_FING.Value.ToString("yyyy-MM-dd") : fechaing.ToString("yyyy-MM-dd"),
                    Only.NOMINA_FCARD.HasValue ? Only.NOMINA_FCARD.Value.ToString("yyyy-MM-dd") : fechacad.ToString("yyyy-MM-dd")
                    );
            }

            dsColaboradores = new DataSet();
            dsColaboradores.Tables.Add(dtColaboradoresAprobados);

            int registros_actualizados_correcto1 = 0;
            int registros_actualizados_incorrecto1 = 0;
            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_NOMINA(dsColaboradores, ref registros_actualizados_correcto1, ref registros_actualizados_incorrecto1, ref error_consulta) as DataSet;
            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_NOMINA(dsColaboradores, ref registros_actualizados_correcto1, ref registros_actualizados_incorrecto1, ref error_consulta) as DataSet;


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
                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraColaboradoresOnlyControl", registros_actualizados_correcto1.ToString(), Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    validacolaboradores = true;
                    return;
                }
                var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERSONA_NOMINA():{0}", error_consulta));
                var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "RegistraColaboradoresOnlyControl", registros_actualizados_correcto1.ToString(), Request.UserHostAddress);
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

            //if (registros_actualizados_incorrecto > 0)
            //{
            //    this.Alerta(error_consulta);
            //    validacolaboradores = true;
            //    return;
            //}
            //else if (!string.IsNullOrEmpty(error_consulta))
            //{
            //    this.Alerta(error_consulta);
            //    validacolaboradores = true;
            //    return;
            //}

            //DataTable dt2 = new DataTable();
            //dt2 = dsErrorAC_R_PERSONA_PEATON.Tables[0];
            //if (dt2.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt2.Rows.Count; i++)
            //    {
            //        if (dt2.Rows[i][5].ToString().Substring(0, 27).ToUpper() != "Error: Cedula ya registrada".ToUpper())
            //        {
            //            errorvehiculo = dt2.Rows[i][5].ToString();
            //            var t = credenciales.SaveLog(errorvehiculo, "credenciales", "onlyControl.AC_R_PERSONA_PEATON()", DateTime.Now.ToShortDateString(), "sistema");
            //            validacolaboradores = true;
            //            return;
            //        }
            //    }
            //}
            //if (!string.IsNullOrEmpty(errorvehiculo))
            //{
            //    this.Alerta("Hubo un error al intentar registrar los siguientes colaboradores en el metodo AC_R_PERSONA_PEATON, revise a continuación. <br />" + errorvehiculo);
            //    //validacolaboradores = true;
            //    return;
            //}
        }

      

      

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)
            {
                this.alerta.Attributes["class"] = string.Empty;
                this.alerta.Attributes["class"] = "alert alert-info";
            }
            else
            {
                this.alerta.Attributes["class"] = string.Empty;
                this.alerta.Attributes["class"] = "alert alert-danger";

            }
            //class="alert alert-danger"
            this.alerta.Visible = true;
            this.alerta.InnerHtml = Mensaje;
            this.UPdetalle.Update();
        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
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

                Mostrar_Mensaje(1, "Confirme que los documentos del colaborador(es) sean los correctos.");

                if (e.CommandName == "Rechazar")
                {
                    var item = e.Item;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;

                    if (salir.Visible)
                    {
                        this.Alerta("Acción no permitida, La solicitud ya ha sido procesada");
                        Mostrar_Mensaje(2, "Acción no permitida, La solicitud ya ha sido procesada.");
                        return;
                    }

                    if (string.IsNullOrEmpty(tcomentario.Text))
                    {
                        
                        Mostrar_Mensaje(2, "Se debe agregar un comentario para rechazo especifico.");
                        return;
                    }
                    int v_accion = 0;
                    var v_argumentos = e.CommandArgument.ToString().Split(',');
                    string v_idSol = v_argumentos[0].ToString();
                    string v_idSolcol = v_argumentos[1].ToString();
                    string mensaje;
                    if (!credenciales.RechazarColaboradorEspecifico(
                        v_idSol,
                        v_idSolcol,
                        tcomentario.Text,
                        Page.User.Identity.Name,
                        out mensaje))
                    {
                        //this.Alerta(mensaje);
                        v_accion = 1;
                    }
                    else
                    {
                        //this.Alerta("Colaborador rechazado exitosamente.");
                        v_accion = 2;
                    }
                    ConsultaInfoSolicitud();

                    if (v_accion == 1)
                    {
                        Mostrar_Mensaje(2, mensaje);
                    }
                    else
                    {
                        Mostrar_Mensaje(1, "Colaborador rechazado exitosamente.");
                    }
                    UPdetalle.Update();
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "RepeaterBooking_ItemCommand()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                Mostrar_Mensaje(2, string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }
    }
}