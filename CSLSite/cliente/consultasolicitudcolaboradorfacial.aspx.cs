using csl_log;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace CSLSite.cliente
{
    public partial class consultasolicitudcolaboradorfacial : System.Web.UI.Page
    {
        private String xmlDocumentos;
        private GetFile.Service getFile = new GetFile.Service();
        private DataTable dtDocumentos = new DataTable();

        public byte[] ArrayFoto;  //Almacenar Foto
        public byte[] ArrayFoto1;  //Almacenar Foto
        public byte[] ArrayFoto2;  //Almacenar Foto

        public String cedula
        {
            get { return (String)Session["cedulaconsultasolicitudcolaboradorfacial"]; }
            set { Session["cedulaconsultasolicitudcolaboradorfacial"] = value; }
        }
        public String numsolicitud
        {
            get { return (String)Session["numsolicitudconsultasolicitudcolaboradorfacial"]; }
            set { Session["numsolicitudconsultasolicitudcolaboradorfacial"] = value; }
        }
        public String idsolcol
        {
            get { return (String)Session["idsolcolconsultasolicitudcolaboradorfacial"]; }
            set { Session["idsolcolconsultasolicitudcolaboradorfacial"] = value; }
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
           
            if (HttpContext.Current.Request.Cookies["token"] == null)
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            }
            try
            {
                numsolicitud = Request.QueryString["numsolicitud"];
                idsolcol = Request.QueryString["idsolcol"];
                cedula = Request.QueryString["cedula"];
                DataTable dtResultados = credenciales.GetRegistroFacialXNumSolicitudCliente(numsolicitud, idsolcol);

                if (!(dtResultados is null))
                {
                    if (dtResultados.Rows.Count > 0)
                    {
                        xfinder.Visible = false;
                        xResult.Visible = true;

                        tableResultado.DataSource = dtResultados;
                        tableResultado.DataBind();
                        var drow = dtResultados.Select(" estado = 'R'");
                        if (drow.Count() > 0)//table.Select("Size >= 230 AND Team = 'b'");
                        {
                            xfinder.Visible = true;
                            xResult.Visible = false;
                            //btnReenviar.Visible = true;
                            btnReenviar_Click(null, null);
                        }

                        return;
                    }
                }

                xfinder.Visible = true;
                xResult.Visible = false;

                btnbuscardoc_Click(null, null);
            }
            catch (Exception)
            {
                Response.Write("<script language='JavaScript'>var r=alert('Ocurrió un problema al cargar la pagina, la URL ha cambiado.');if(r==true){window.location='http://www.cgsa.com.ec/inicio.aspx';}else{window.location='http://www.cgsa.com.ec/inicio.aspx';};</script>");
            }
 
        }

        protected void btnbuscardoc_Click(object sender, EventArgs e)
        {
            try
            {
                var tablix3 = credenciales.GetListadoFotosCredencialSubir(long.Parse(numsolicitud), long.Parse(idsolcol), cedula);
                tablePaginationDocumentos.DataSource = tablix3;
                tablePaginationDocumentos.DataBind();
                btnbuscardoc.Focus();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudempresa", "btnbuscardoc_Click()", "1", Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }

        protected  void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                string v_msjNotificacion = string.Empty;
                foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                {
                    FileUpload fsupload = item.FindControl("fsupload") as FileUpload;

                    if (string.IsNullOrEmpty(fsupload.FileName))
                    {
                        v_msjNotificacion = "Verifique que todas las fotografías seleccionadas estén cargadas. \\nIntente nuevamente. ";
                        this.Alerta(v_msjNotificacion);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                        v_msjNotificacion = "Verifique que todas las fotografías seleccionadas estén cargadas. <br/>Intente nuevamente. ";
                        msjNotificaciones.InnerHtml = v_msjNotificacion;
                        msjNotificaciones.Visible = true;
                        UPNotificacion.Update();
                        return;
                    }
                }

                string respuestaStrRF = New_ExportFileUpload();
                string mensaje = string.Empty;

                if (!string.IsNullOrEmpty(respuestaStrRF))
                {
                    this.Alerta(respuestaStrRF);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                    respuestaStrRF = respuestaStrRF.Replace("\\n", "<br/>");
                    v_msjNotificacion = "Error al exportar imagenes <br/>" + respuestaStrRF;
                    msjNotificaciones.InnerHtml = v_msjNotificacion;
                    msjNotificaciones.Visible = true;
                    UPNotificacion.Update();
                    return;
                }

                if (string.IsNullOrEmpty(xmlDocumentos))
                {
                    this.Alerta("No se realizó la subida de las fotos: \\n intente nuevamente. " );
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                    v_msjNotificacion = "No se realizó la subida de las fotos. <br>Intente nuevamente. ";
                    msjNotificaciones.InnerHtml = v_msjNotificacion;
                    msjNotificaciones.Visible = true;
                    UPNotificacion.Update();
                    return;
                }

               if (!credenciales.AddFotosRegistroFacial(xmlDocumentos,Page.User.Identity.Name.ToUpper(),out mensaje))
                {

                    this.Alerta(mensaje);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                    v_msjNotificacion = "Error al Insertar imagenes <br/>" + mensaje;
                    msjNotificaciones.InnerHtml = v_msjNotificacion;
                    msjNotificaciones.Visible = true;
                    UPNotificacion.Update();
                    return;
                }
                else
                {
                    v_msjNotificacion = "Transacción exitosa <br/>" + mensaje;
                    msjNotificaciones.InnerHtml = v_msjNotificacion;
                    msjNotificaciones.Visible = true;
                    UPNotificacion.Update();
                    Response.Write("<script language='JavaScript'>var r=alert('Registro facial enviado exitosamente.');if(r==true){window.close();}else{window.close();}</script>");
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudempresa", "btsalvar_Click()", DateTime.Now.ToShortDateString(), "sistemas");
                //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                var error = "Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} " + number.ToString();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                Response.Write("<script language='JavaScript'>var r=alert('" + error + "');if(r==true){window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}</script>");
            }
        }

        private string New_ExportFileUpload()
        {
            string respuestaStr = string.Empty;
            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("idSolicitud");
            dtDocumentos.Columns.Add("idSolcol");
            dtDocumentos.Columns.Add("identificacion");
            dtDocumentos.Columns.Add("secuencia");
            dtDocumentos.Columns.Add("documento");
            dtDocumentos.Columns.Add("extension");
            dtDocumentos.Columns.Add("ruta");
            dtDocumentos.Columns.Add("usuarioCrea");
            dtDocumentos.Columns.Add("template");
            dtDocumentos.Columns.Add("foto");

            //CONSUMO DE SW ONLYCONTROL PARA VERIFICAR LICENCIA ACTIVA DE HERRAMIENTA NEURO RF
            RespuestaSwNeuro _exit = null;
            string mError = string.Empty;
            string v_usuario = string.Empty;
            var swNeuro = new swNeuroOC.wsNeuroSoapClient();
            try
            {
                v_usuario = Page.User.Identity.Name;
                var neuroLicencia = swNeuro.licenseStatus();
                //////////////////////////////////////////
                //VALIDA LICENCIA DE HERREAMIENTA NEURO
                //////////////////////////////////////////
                if (!string.IsNullOrEmpty(neuroLicencia))
                {
                    _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(neuroLicencia);

                    if (_exit.codigo != "1")
                    {
                        credenciales.AddTraceRegistroFacial(0, 0, 0, "LICENCIA NEURO", "0 - VALIDA LICENCIA", _exit.mensaje, false, v_usuario, out mError);
                        respuestaStr = " Licencia NEURO Inactiva - SW OC NEURO: " + _exit.mensaje +"  \\n No se realizo la subida de las fotos: \\n intente nuevamente. ";
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        return respuestaStr;
                    }
                }
                else
                {
                    credenciales.AddTraceRegistroFacial(0, 0, 0, "LICENCIA NEURO", "0 - VALIDA LICENCIA", "NO SE OBTUVO RESPUESTA DEL SW NEURO", false, v_usuario, out mError);
                    respuestaStr = "Error al consumir SW OC NEURO: No se obtuvo respuesta";
                    return respuestaStr;
                }

                /////////////////////////////////////////////////////////
                //VALIDA Y PROCESA CADA IMAGEN ELEGIDA POR EL USUARIO
                /////////////////////////////////////////////////////////
                List<ListaImagenesRF> LstImagenes = new List<ListaImagenesRF>();
                byte[] v_imageBase64OC = null;
                string v_imageBase64StrOC = string.Empty;

                foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                {
                    FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                    TextBox txtidsolicitud = item.FindControl("txtidsolicitud") as TextBox;
                    TextBox txtidSolcol = item.FindControl("txtidSolcol") as TextBox;
                    TextBox txtidentificacion = item.FindControl("txtidentificacion") as TextBox;
                    TextBox txtsec = item.FindControl("txtsec") as TextBox;
                    TextBox txtdoc = item.FindControl("txtdoc") as TextBox;
                    TextBox txtext = item.FindControl("txtext") as TextBox;

                    ///////////////////////////////////////////////////////////////////////////
                    //CONSUMO DE SW ONLYCONTROL PARA EL PROCESAMIENTO DE IMAGENES - IMAGETOICAO
                    ///////////////////////////////////////////////////////////////////////////
                    byte[] v_imageBase64 = fsupload.FileBytes;//System.IO.File.ReadAllBytes(fsupload.FileName);
                    string v_imageBase64Str = Convert.ToBase64String(v_imageBase64);
                    var imageToIcao = swNeuro.imageToIcao(v_imageBase64Str);

                    if (item.ItemIndex == 0) { v_imageBase64OC = fsupload.FileBytes; v_imageBase64StrOC = Convert.ToBase64String(v_imageBase64OC); }

                    if (!string.IsNullOrEmpty(imageToIcao))
                    {
                        _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(imageToIcao);

                        if (_exit.codigo != "1")
                        {
                            credenciales.AddTraceRegistroFacial(long.Parse(txtidsolicitud.Text), long.Parse(txtidSolcol.Text), int.Parse(txtsec.Text), "1 - IMAGETOICAO", txtdoc.Text, _exit.mensaje, false, v_usuario, out mError);
                            respuestaStr = string.Format("ERROR AL PROCESAR IMAGEN {0} - IMAGETOICAO - SW OC NEURO: {1} {2}", txtsec.Text, _exit.mensaje , "  \\n No se realizo la subida de las fotos: \\n intente nuevamente. ");
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                            return respuestaStr;
                        }
                        else
                        {
                            credenciales.AddTraceRegistroFacial(long.Parse(txtidsolicitud.Text), long.Parse(txtidSolcol.Text), int.Parse(txtsec.Text), "1 - IMAGETOICAO", txtdoc.Text, _exit.mensaje, true, v_usuario, out mError);
                            /////////////////////////////////////////////////////////////////////
                            //SE OBTIENE LA IMAGEN PROCESADA PARA GUARDARLA EN UNA LISTA TEMPORAL
                            ////////////////////////////////////////////////////////////////////
                            byte[] bytesResultImagenProcesada = Convert.FromBase64String(_exit.dato);

                            /////////////////////////////////////////////////////////////////////////////////
                            //CONSUMO DE SW ONLYCONTROL PARA EL PROCESAMIENTO DE IMAGENES - IMAGETOTEMPLATE
                            ////////////////////////////////////////////////////////////////////////////////
                            
                            var imageToTemplate = swNeuro.imageToTemplate(Convert.ToBase64String(bytesResultImagenProcesada));

                            if (!string.IsNullOrEmpty(imageToTemplate))
                            {
                                _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(imageToTemplate);

                                if (_exit.codigo != "1")
                                {
                                    credenciales.AddTraceRegistroFacial(long.Parse(txtidsolicitud.Text), long.Parse(txtidSolcol.Text), int.Parse(txtsec.Text), "2 - IMAGETOTEMPLATE", txtdoc.Text, _exit.mensaje, false, v_usuario, out mError);
                                    respuestaStr = string.Format("ERROR AL PROCESAR IMAGEN {0} - IMAGETOTEMPLATE - SW OC NEURO: {1} {2}", txtsec.Text, _exit.mensaje, "  \\n No se realizo la subida de las fotos: \\n intente nuevamente. ");
                                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                                    return respuestaStr;
                                }
                                else
                                {
                                    credenciales.AddTraceRegistroFacial(long.Parse(txtidsolicitud.Text), long.Parse(txtidSolcol.Text), int.Parse(txtsec.Text), "2 - IMAGETOTEMPLATE", txtdoc.Text, _exit.mensaje, true, v_usuario, out mError);
                                    ///////////////////////////////////////////////////////////////////////
                                    //SE OBTIENE LA IMAGEN TEMPLATE Y SE LA GUARDA EN LA LISTA EN MEMORIA
                                    //////////////////////////////////////////////////////////////////////

                                    if (item.ItemIndex == 0)
                                    {
                                        bytesResultImagenProcesada = v_imageBase64OC;
                                    }

                                    byte[] v_imageBase64Template = Convert.FromBase64String(_exit.dato);
                                    LstImagenes.Add(new ListaImagenesRF
                                    {
                                        idsolicitud = long.Parse(txtidsolicitud.Text.ToString()),
                                        idSolcol = long.Parse(txtidSolcol.Text.ToString()),
                                        identificacion = txtidentificacion.Text,
                                        secuencia = int.Parse(txtsec.Text),
                                        nombre = fsupload.FileName,
                                        ruta = Server.MapPath(fsupload.FileName),
                                        imagenProcesadaB64 = bytesResultImagenProcesada,
                                        TemplateB64 = v_imageBase64Template,
                                        doc = txtdoc.Text,
                                        ext = txtext.Text
                                    });
                                }
                            }
                            else
                            {
                                credenciales.AddTraceRegistroFacial(long.Parse(txtidsolicitud.Text), long.Parse(txtidSolcol.Text), int.Parse(txtsec.Text), "2 - IMAGETOTEMPLATE", txtdoc.Text, "NO SE OBTUVO RESPUESTA DEL SW NEURO", false, v_usuario, out mError);
                                respuestaStr = "Error al consumir SW IMAGETOTEMPLATE OC NEURO: No se obtuvo respuesta";
                                return respuestaStr;
                            }
                        }
                    }
                    else
                    {
                        credenciales.AddTraceRegistroFacial(long.Parse(txtidsolicitud.Text),long.Parse(txtidSolcol.Text),int.Parse(txtsec.Text) , "1 - IMAGETOICAO", txtdoc.Text, "NO SE OBTUVO RESPUESTA DEL SW NEURO", false, v_usuario, out mError);
                        respuestaStr = "Error al consumir SW IMAGETOICAO OC NEURO: No se obtuvo respuesta";
                        return respuestaStr;
                    }
                }

                /////////////////////////////////////////////////
                ///COMPARA LAS IMAGENES QUE SE SUBIRÁN AL LA DB
                /////////////////////////////////////////////////
                int v_contador = 0;
                byte[] v_template = null;
                byte[] v_image = null;

                if (LstImagenes.Count < 3)//EN CASO DE QUE SEA UNA CORRECION DE IMAGEN RECHAZADA SE BUSCA EN LA DB EL TEMPLATE DE LAS IMAGENES VERIFICADAS O ACTIVAS
                {
                    DataTable dtResultados = credenciales.GetRegistroFacialXNumSolicitudCliente(LstImagenes.FirstOrDefault()?.idsolicitud.ToString(), LstImagenes.FirstOrDefault()?.idSolcol.ToString());
                    foreach (var lstImagen in LstImagenes)
                    {
                        if (dtResultados!= null)
                        {
                            if (dtResultados.Rows.Count > 0)
                            {
                                var drow = dtResultados.Select(" estado <> 'R'").FirstOrDefault()["template"].ToString();
                                if (!string.IsNullOrEmpty(drow))//table.Select("Size >= 230 AND Team = 'b'");
                                {
                                    //byte[] myByte = System.Text.Encoding.UTF8.GetBytes(drow);
                                    //v_template = myByte;
                                    //v_image = lstImagen.imagenProcesadaB64;

                                    /////////////////////////////////////////////////////////////////////////////////
                                    //CONSUMO DE SW ONLYCONTROL PARA EL PROCESAMIENTO DE IMAGENES - VERIFYTEMPLATE
                                    ////////////////////////////////////////////////////////////////////////////////
                                    var templateVerify = swNeuro.VerifyTemplate(Convert.ToBase64String(lstImagen.TemplateB64), drow);
                                    //var imageVerify = swNeuro.VerifyImage(Convert.ToBase64String(lstImagen.imagenProcesadaB64), Convert.ToBase64String(v_image));

                                    if (!string.IsNullOrEmpty(templateVerify))
                                    {
                                        _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(templateVerify);

                                        if (_exit.codigo != "1")
                                        {
                                            credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "3 - TEMPLATEVERIFY", lstImagen.nombre , _exit.mensaje, false, v_usuario, out mError);
                                            respuestaStr = string.Format("ERROR AL COMPARAR TEMPLATES {0} - VERIFYTEMPLATE - SW OC NEURO: {1} {2}", lstImagen.secuencia, _exit.mensaje, "  \\n No se realizo la subida de las fotos: \\n intente nuevamente. ");
                                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                                            return respuestaStr;
                                        }
                                        else
                                        {
                                            credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "3 - TEMPLATEVERIFY", lstImagen.nombre, _exit.mensaje, true, v_usuario, out mError);
                                        }
                                        /*else
                                        {
                                            _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(imageVerify);

                                            if (_exit.codigo != "0")
                                            {
                                                this.Alerta(string.Format("ERROR AL COMPARAR IMAGENES {0} - VERIFYTEMPLATE - SW OC NEURO: {1} {2}", lstImagen.secuencia, _exit.mensaje, "  \\n No se realizo la subida de las fotos: \\n intente nuevamente. "));
                                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                                                return;
                                            }
                                        }*/
                                    }
                                    else
                                    {
                                        credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "3 - TEMPLATEVERIFY", lstImagen.nombre, "NO SE OBTUVO RESPUESTA DEL SW NEURO", false, v_usuario, out mError);
                                        respuestaStr ="Error al consumir SW VERIFYTEMPLATE OC NEURO: No se obtuvo respuesta";
                                        return respuestaStr;
                                    }

                                }
                            }
                        }
                        else
                        {
                            credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "GetRegistroFacialXNumSolicitudCliente", lstImagen.nombre, "NO SE OBTUVO RESPUESTA DEL METODO GetRegistroFacialXNumSolicitudCliente", false, v_usuario, out mError);
                        }
                    }
                }
                else
                {
                    foreach (var lstImagen in LstImagenes)
                    {
                        v_contador += 1;
                        if (v_contador == 1)
                        {
                            v_template = lstImagen.TemplateB64;
                            v_image = lstImagen.imagenProcesadaB64;
                            continue;
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //CONSUMO DE SW ONLYCONTROL PARA EL PROCESAMIENTO DE IMAGENES - VERIFYTEMPLATE - VERIFYIMAGE
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        var templateVerify = swNeuro.VerifyTemplate(Convert.ToBase64String(lstImagen.TemplateB64), Convert.ToBase64String(v_template));
                        //var imageVerify = swNeuro.VerifyImage(Convert.ToBase64String(lstImagen.imagenProcesadaB64), Convert.ToBase64String(v_image));
                        v_template = lstImagen.TemplateB64;
                        v_image = lstImagen.imagenProcesadaB64;
                        if (!string.IsNullOrEmpty(templateVerify))
                        {
                            _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(templateVerify);

                            if (_exit.codigo != "1")
                            {
                                credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "3 - TEMPLATEVERIFY", lstImagen.nombre, _exit.mensaje, false, v_usuario, out mError);
                                respuestaStr = string.Format("ERROR AL COMPARAR TEMPLATES {0} - VERIFYTEMPLATE - SW OC NEURO: {1} {2}", lstImagen.secuencia, _exit.mensaje, "  \\n No se realizo la subida de las fotos: \\n intente nuevamente. ");
                                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                                return respuestaStr;
                            }
                            else
                            {
                                credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "3 - TEMPLATEVERIFY", lstImagen.nombre, _exit.mensaje, true, v_usuario, out mError);
                                /*if (!string.IsNullOrEmpty(imageVerify))
                                {
                                    _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(imageVerify);

                                    if (_exit.codigo != "0")
                                    {
                                        credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, int.Parse(lstImagen.secuencia), "VERIFYIMAGE", lstImagen.nombre, "ERROR AL COMPARAR IMAGENES", false, v_usuario, out mError);
                                        this.Alerta(string.Format("ERROR AL COMPARAR IMAGENES {0} - VERIFYTEMPLATE - SW OC NEURO: {1} {2}", lstImagen.secuencia, _exit.mensaje, "  \\n No se realizo la subida de las fotos: \\n intente nuevamente. "));
                                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, int.Parse(lstImagen.secuencia), "IMAGEVERIFY", lstImagen.nombre, "NO SE OBTUVO RESPUESTA DEL SW NEURO", false, v_usuario, out mError);
                                }*/

                            }
                        }
                        else
                        {
                            credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "3 - TEMPLATEVERIFY", lstImagen.nombre, "NO SE OBTUVO RESPUESTA DEL SW NEURO", false, v_usuario, out mError);
                            respuestaStr = "Error al consumir SW VERIFYTEMPLATE OC NEURO: No se obtuvo respuesta";
                            return respuestaStr;
                        }
                    }
                }

                foreach (var lstImagen in LstImagenes)
                {
                    string finalname = string.Empty;
                    var p = CSLSite.app_start.CredencialesHelper.UploadFileRF(lstImagen.ruta, lstImagen.imagenProcesadaB64, out finalname);
                    if (!p)
                    {
                        credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "4 - UPLOAD PHOTO", lstImagen.nombre, finalname, false, v_usuario, out mError);
                        respuestaStr = finalname;
                        return respuestaStr;
                    }
                    credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "4 - UPLOAD PHOTO", lstImagen.nombre, _exit.mensaje, true, v_usuario, out mError);
                    dtDocumentos.Rows.Add(lstImagen.idsolicitud,
                                            lstImagen.idSolcol,
                                            lstImagen.identificacion,
                                            lstImagen.secuencia,
                                            lstImagen.doc,
                                            lstImagen.ext, 
                                            finalname, 
                                            Page.User.Identity.Name,
                                            Convert.ToBase64String(lstImagen.TemplateB64),
                                            Convert.ToBase64String(lstImagen.imagenProcesadaB64));
                }
                /*
                foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                {
                    FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                    TextBox txtidsolicitud = item.FindControl("txtidsolicitud") as TextBox;
                    TextBox txtidSolcol = item.FindControl("txtidSolcol") as TextBox;
                    TextBox txtidentificacion = item.FindControl("txtidentificacion") as TextBox;
                    TextBox txtsec = item.FindControl("txtsec") as TextBox;
                    TextBox txtdoc = item.FindControl("txtdoc") as TextBox;
                    TextBox txtext = item.FindControl("txtext") as TextBox;


                    if (fsupload.HasFile)
                    {
                        string rutafile = Server.MapPath(fsupload.FileName);
                        string finalname;
                        var p = CSLSite.app_start.CredencialesHelper.UploadFile(Server.MapPath(fsupload.FileName), fsupload.PostedFile.InputStream, out finalname);
                        if (!p)
                        {
                            this.Alerta(finalname);
                            return;
                        }
                        dtDocumentos.Rows.Add(Convert.ToInt64(txtidsolicitud.Text),
                                                Convert.ToInt64(txtidSolcol.Text),
                                                txtidentificacion.Text,
                                                Convert.ToInt32(txtsec.Text),
                                                txtdoc.Text,
                                                txtext.Text, finalname, Page.User.Identity.Name);
                    }
                }
                */
            }
            catch(Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudempresa", "New_ExportFileUpload()", DateTime.Now.ToShortDateString(), "sistemas");
                //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                var error = "Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} " + number.ToString();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                xmlDocumentos = null;
                Response.Write("<script language='JavaScript'>var r=alert('" + error + "');if(r==true){window.close();}else{window.close();}</script>");
                respuestaStr = error;
                return respuestaStr;
            }
          
            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            StringWriter sw = new StringWriter();
            dtDocumentos.WriteXml(sw);
            xmlDocumentos = sw.ToString();
            return respuestaStr;
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Write("<script language='JavaScript'>window.close();</script>");
            }
            catch (Exception ex)
            {
                this.Alerta(ex.Message);
            }
        }

        protected void btnReenviar_Click(object sender, EventArgs e)
        {
            xfinder.Visible = true;
            xResult.Visible = false;

            btnbuscardoc_Click(null, null);
        }

        public byte[] ReadImage(Stream Imput)
        {
            BinaryReader reader = new BinaryReader(Imput);
            byte[] imgByte = reader.ReadBytes((int)Imput.Length);

            return imgByte;
        }
    }

    public class ListaImagenesRF
    {
        public long idsolicitud { get; set; }
        public long idSolcol { get; set; }
        public string identificacion { get; set; }
        public int secuencia { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
        public byte[] imagenProcesadaB64 { get; set; }
        public string imagenProcesadaStr { get; set; }
        public byte[] TemplateB64 { get; set; }
        public string TemplateStr { get; set; }
        public string doc { get; set; }
        public string ext { get; set; }
    }

}