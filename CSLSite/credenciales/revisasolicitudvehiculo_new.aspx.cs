﻿using System;
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
    public partial class revisasolicitudvehiculo_new : System.Web.UI.Page
    {
        private static Int64? lm = -3;
        private string OError;

        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        private String xmlDocumentos;
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosrevisasolicitudvehiculo"]; }
            set { Session["dtDocumentosrevisasolicitudvehiculo"] = value; }
        }
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudrevisasolicitudvehiculo"]; }
            set { Session["numsolicitudrevisasolicitudvehiculo"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["rucrevisasolicitudvehiculo"]; }
            set { Session["rucrevisasolicitudvehiculo"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailrevisasolicitudvehiculo"]; }
            set { Session["useremailrevisasolicitudvehiculo"] = value; }
        }
        public string mensajefac
        {
            get { return (string)Session["mensajefacrevisasolicitudvehiculo"]; }
            set { Session["mensajefacrevisasolicitudvehiculo"] = value; }
        }
        public string mensajeok
        {
            get { return (string)Session["mensajeokrevisasolicitudvehiculo"]; }
            set { Session["mensajeokrevisasolicitudvehiculo"] = value; }
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
                //rucempresa = user.ruc;
                foreach (string var in Request.QueryString)
                {
                    rucempresa = Request.QueryString["ruc"].ToString();
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
            factura.Visible = false;
            txttipcli.Text = Session["txttipVehiculo.Text"] as string;
        }
        private void ConsultaInfoSolicitud()
        {
            try
            {
                //foreach (string var in Request.QueryString)
                //{
                numsolicitudempresa = Request.QueryString["numsolicitud"];
                //}
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
                        factura.Visible = false;
                        salir.Visible = true;
                    }
                    else
                    {
                        //factura.Visible = true;
                        //alertafu.InnerHtml = "Adjunte el archivo en formato PDF";
                    }
                    mensajefac = "facturación";
                    mensajeok = "facturada";
                }
                Session["dtDocumentosrevisasolicitudvehiculo"] = new DataTable();
                var DescripcionTipoSolicitud = credenciales.GetDescripcionTipoSolicitudXNumSolicitud(numsolicitudempresa);
                for (int i = 0; i <= DescripcionTipoSolicitud.Rows.Count - 1; i++)
                {
                    Session["txttipVehiculo.Text"] = DescripcionTipoSolicitud.Rows[i][0].ToString(); ;
                    txttipcli.Text = DescripcionTipoSolicitud.Rows[i][0].ToString();
                }
                this.alerta.InnerHtml = "Confirme que los documentos del vehiculo(s) sean los correctos.";
                var tablixVehiculo = credenciales.GetSolicitudVehiculo_New(numsolicitudempresa);
                tablePagination.DataSource = tablixVehiculo;
                tablePagination.DataBind();
            }
            catch (Exception ex)
            {
                Session["dtDocumentosrevisasolicitudvehiculo"] = new DataTable();
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
           // HiddenField1.Value = ObtenerTarifa();
        }

       
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean banderafac = true;
               

                var oServ = serviciosCredenciales.GetServicio(long.Parse(numsolicitudempresa));
               
                if (oServ?.nombre == "ATV") //ACTUALIZACION DOCUMENTOS TRANSPORTISTAS (VEH)
                {
                    banderafac = false;
                }

              

                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString["numsolicitud"];
                }

                string Numero_Factura = this.TxtNumdocumento.Text.Trim();


                List<string> listPlacas = new List<string>();
                DataTable dtDocSol = Session["dtDocumentosrevisasolicitudvehiculo"] as DataTable;
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblplaca = item.FindControl("lblplaca") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (!chkRevisado.Checked)
                    {
                        var nomina = credenciales.GetConsultaVehiculo(rucempresa, lblplaca.Text);
                        if (nomina.Rows[0]["MENSAJE"].ToString() == "NUEVO" || nomina.Rows[0]["MENSAJE"].ToString() == "SEMPRESA" || nomina.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                        {
                            if (nomina.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                            {
                                //Emisión
                                foreach (string var in Request.QueryString)
                                {
                                    this.Alerta("La Solicitud pertenece a la Empresa:\\n *" + Request.QueryString["razonsocial"].ToString() + "\\nSin embargo la Placa:\\n *" + lblplaca.Text + "\\nEsta registrado en la Empresa:\\n *" + nomina.Rows[0]["EMPE_NOM"].ToString());
                                    return;
                                }
                            }
                        }
                    }
                   
                    else if (chkRevisado.Checked == true)
                    {
                        listPlacas.Add(lblplaca.Text);
                    }
                    if (chkRevisado.Checked == false && dtDocSol != null)
                    {
                        var result = from myRow in dtDocSol.AsEnumerable()
                                     where myRow.Field<string>("Placa") == lblplaca.Text
                                     select myRow;
                        DataTable dt = result.AsDataView().ToTable();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dt.Rows[item.ItemIndex][10]) == true)
                            {
                                this.Alerta("La Placa: *" + lblplaca.Text + "*\\nTiene documentos rechazados.\\n" + "Revise la información antes de continuar con la " + mensajefac + " de la solicitud.\\n Si quiere continuar de click en Rechazar.");
                                return;
                            }
                        }
                    }
                }

                DataTable dtVehiculos = credenciales.GetSolicitudVehiculo_New(numsolicitudempresa);
                var resultAprobados = from myRow in dtVehiculos.AsEnumerable()
                                      select myRow;

                DataTable dtAprobados = resultAprobados.AsDataView().ToTable();

                //if (dtAprobados.Rows.Count == 0)
                //{
                //    this.Alerta("Tiene un unico vehiculo rechazado.\\n" + "Revise la información antes de continuar con la " + mensajefac + " de la solicitud.\\n Si quiere continuar de click en Rechazar.");
                //    return;
                //}

                var vehiculos = credenciales.GetSolicitudVehiculo_New(numsolicitudempresa);
                vehiculos.Columns.Add("Comentario");
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblplaca = item.FindControl("lblplaca") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (chkRevisado.Checked == true)
                    {
                        vehiculos.Rows[item.ItemIndex]["Comentario"] = tcomentario.Text;
                    }
                }

                DataTable dtVeh = new DataTable();
                var resultListaAprobados = from myRow in vehiculos.AsEnumerable()
                                           //where listPlacas.Contains(myRow.Field<string>("Placa"))
                                           select myRow;

                dtVeh = resultListaAprobados.AsDataView().ToTable();
                dtVeh.AcceptChanges();
                dtVeh.TableName = "Vehiculos";
                StringWriter sw = new StringWriter();
                dtVeh.WriteXml(sw);


                DataTable dtCola = new DataTable();
                var resultListaApr = from myRow in dtDocSol.AsEnumerable()
                                     //where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true
                                     select myRow;
                dtCola = resultListaApr.AsDataView().ToTable();
                dtCola.AcceptChanges();
                dtCola.TableName = "Colaboradores";
                StringWriter swC = new StringWriter();
                dtCola.WriteXml(swC);
                String xmlDocumentosR = swC.ToString();

                String xmlVehiculos = sw.ToString();
                

                Boolean validasolicitud = false;
                string mensaje = null;


                if (!credenciales.ActualizarSolicitudVehiculo_Fechas(
                         numsolicitudempresa,
                         out mensaje))
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btsalvar_Click), "btsalvar_Click", false, null, null, mensaje, null);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    Response.Write("<script language='JavaScript'>alert('" + mensaje + " - " + OError + "');</script>");
                }
                else
                {

                    //Response.Write("<script language='JavaScript'>var r=alert('Solicitud " + mensajeok + " exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                }


                RegistraVehiculosOnlyControl(out validasolicitud, listPlacas);

                if (validasolicitud)
                {
                    return;
                }

               

                if (!credenciales.ApruebaSolicitudVehiculo_Transportista(
                         numsolicitudempresa,
                         rucempresa,
                         Numero_Factura,
                         0,

                         Page.User.Identity.Name.ToUpper(),
                         xmlVehiculos,
                         xmlDocumentos,
                         xmlDocumentosR,
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
                if (!credenciales.RechazaSolicitudVehiculo_Transportista(
                    numsolicitudempresa,
                    rucempresa,
                    nombreempresa,
                    useremail,
                    xmlVehiculos,
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

        private void RegistraVehiculosOnlyControl(out bool validavehiculos, List<string> Placas)
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

            DataTable dtVehiculos = credenciales.GetSolicitudVehiculoOnlyControl(numsolicitudempresa);
            DataTable dtVeh = new DataTable();
            var resultListaAprobados = from myRow in dtVehiculos.AsEnumerable()
                                       where !Placas.Contains(myRow.Field<string>("PLACA"))
                                       select myRow;
            dtAprobados = resultListaAprobados.AsDataView().ToTable();
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
                    fechapoliza = Convert.ToDateTime(dtAprobados.Rows[i][11].ToString()).ToString("yyyy-MM-dd");
                    fechamtop = Convert.ToDateTime(dtAprobados.Rows[i][12].ToString()).ToString("yyyy-MM-dd");
                }
                var empresa = credenciales.GetEmpresaVehiculo(numsolicitudempresa).Rows[0]["RAZONSOCIAL"].ToString();
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
           
            dsVehiculos = new DataSet();
            dsVehiculos.Tables.Add(dtVehiculosAprobados);

            string mensaje = "";

            for (int i = 0; i < dsVehiculos.Tables[0].Rows.Count; i++)
            {
                if (!credenciales.AddModeloVehiculo(dsVehiculos.Tables[0].Rows[i]["MARCA"].ToString(), dsVehiculos.Tables[0].Rows[i]["MODELO"].ToString(), out mensaje))
                {
                    this.Alerta(mensaje);
                    validavehiculos = true;
                    return;
                }
            }

            //dsErrorAC_R_VEHICULOS = onlyControl.AC_R_VEHICULOS(dsVehiculos, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            //if (OnlyControlError(dsErrorAC_R_VEHICULOS, registros_actualizados_incorrecto, error_consulta, "AC_R_VEHICULOS"))
            //{
            //    validavehiculos = true;
            //    return;
            //}

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

                var resultemp = from myRow in dtVehiculosAprobados.AsEnumerable()
                                where myRow.Field<string>("PLACA") == dtVehiculosAprobados.Rows[i][0].ToString()
                                select myRow;
                DataTable dttemp = resultemp.AsDataView().ToTable();

                dsVehiculos = new DataSet();
                dsVehiculos.Tables.Add(dttemp);
                mensaje = "";

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

            }

            if (!string.IsNullOrEmpty(errorvehiculo))
            {
                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_VEHICULOS():{0}", errorvehiculo));
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "btsalvar_Click", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                validavehiculos = true;
                return;
            }

           // mensaje = "";

            //for (int i = 0; i < dsVehiculos.Tables[0].Rows.Count; i++)
            //{

            //    if (!credenciales.UpdateUCREVehiculo(dsVehiculos.Tables[0].Rows[i]["PLACA"].ToString(), dsVehiculos.Tables[0].Rows[i]["MARCA"].ToString(), dsVehiculos.Tables[0].Rows[i]["MODELO"].ToString(), out mensaje))
            //    {
            //        this.Alerta(mensaje);
            //        validavehiculos = true;
            //        return;
            //    }
            //}
           
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