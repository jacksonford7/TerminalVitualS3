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

namespace CSLSite
{
    public partial class registroFacialManual : System.Web.UI.Page
    {
        private string v_empresaSeleccionada = string.Empty;
        DataTable dtColaboradores;
        private string ChoferSelect = string.Empty;
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();

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
            if (!IsPostBack)
            {
                dtColaboradores = new DataTable();
                string error_consulta = string.Empty;
                
                dtColaboradores.Columns.Add("id");
                DataRow dr = dtColaboradores.NewRow();
                dr["id"] = "1";
                dtColaboradores.Rows.Add(dr);
                gvColaboradores.DataSource = dtColaboradores;
                gvColaboradores.DataBind();

                Session["ListaColaboradoresRFManual"] = dtColaboradores;
            }
        }

        public static DataTable ObtenerCompanias(string empresaStr)
        {
            string error_consulta = string.Empty;
            DataTable dt = new DataTable();
            var onlyControl = new OnlyControl.OnlyControlService();
            dt = onlyControl.AC_C_EMPRESA("%"+ empresaStr+"%", 1, ref error_consulta).Tables[0];// onlyControl.AC_C_PERSONA(empresaStr, "%", 1, ref error_consulta).Tables[0];
            return dt;
        }

        public static DataTable ObtenerColaboradores(string empresaStr)
        {
            string error_consulta = string.Empty;
            DataTable dt = new DataTable();
            var onlyControl = new OnlyControl.OnlyControlService();
            dt = onlyControl.AC_C_PERSONA(empresaStr, "%", 1, ref error_consulta).Tables[0];
            return dt;
        }

        [System.Web.Services.WebMethod]
        public static string[] GetEmpresas(string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                string error_consulta = string.Empty;
                int v_max = 0;
                DataTable dt = ObtenerCompanias(prefix);
                
                foreach (DataRow dr in dt.Select("EMPE_NOM LIKE '%" + prefix+"%'") )
                {
                    v_max = v_max + 1;
                    StringResultado.Add(string.Format("{0}+{1}", dr["EMPE_NOM"], dr["EMPE_RUC"] + " " +  dr["EMPE_NOM"]));
                    if (v_max > 100)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        [System.Web.Services.WebMethod]
        public static string[] GetColaborador(string prefix, string empresa)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(empresa))
                {
                    throw new Exception("Debe seleccionar la empresa");
                }
                string error_consulta = string.Empty;

                DataTable dt = ObtenerColaboradores(empresa);

                foreach (DataRow dr in dt.Select("APELLIDOS LIKE '%" + prefix + "%'"))
                {
                    StringResultado.Add(string.Format("{0}+{1}", dr["ID"], dr["ID"] + " - " + dr["CEDULA"] + " - " + dr["APELLIDOS"] + " " + dr["NOMBRES"]));
                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            this.Ocultar_Mensaje();
            string IdEmpresa = string.Empty;
            string DesEmpresa = string.Empty;
            string IdChofer = string.Empty;
            string CedChofer = string.Empty;
            string DesChofer = string.Empty;

            if (!string.IsNullOrEmpty(TxtChofer.Text))
            {
                ChoferSelect = this.TxtChofer.Text.Trim();
                v_empresaSeleccionada = this.TxtEmpresa.Text.Trim();
               
                if (ChoferSelect.Split('-').ToList().Count > 1)
                {
                    IdChofer = ChoferSelect.Split('-').ToList()[0].Trim();
                    CedChofer = ChoferSelect.Split('-').ToList()[1].Trim();
                    DesChofer = ChoferSelect.Split('-').ToList()[2].Trim();

                    dtColaboradores = Session["ListaColaboradoresRFManual"] as DataTable;
                    for (int i = 0; i <= dtColaboradores.Rows.Count - 1; i++)
                    {
                        FileUpload fsupload = (FileUpload)gvColaboradores.Rows[i].FindControl("fsupload");// item.FindControl("fsupload") as FileUpload;
                        FileUpload fsupload1 = (FileUpload)gvColaboradores.Rows[i].FindControl("fsupload1");
                        FileUpload fsupload2 = (FileUpload)gvColaboradores.Rows[i].FindControl("fsupload2");

                        if (string.IsNullOrEmpty(fsupload.FileName) || string.IsNullOrEmpty(fsupload1.FileName) || string.IsNullOrEmpty(fsupload2.FileName))
                        {
                            this.Alerta("Verifique que todas las fotografías seleccionadas estén cargadas: \\n intente nuevamente. ");
                            this.Mostrar_Mensaje(2, "<b>Informativo! Verifique que todas las fotografías seleccionadas estén cargadas. <br/>Intente nuevamente. </b>");
                            return;
                        }
                    }

                    DataTable dtImagenes = New_ExportFileUpload(CedChofer);

                    if (dtImagenes != null)
                    {
                        //#####################################################################
                        // SE PROCEDE A REALIZAR EL REGISTRO FACIAL EN LA BASE DE ONLY CONTROL
                        //#####################################################################
                        string v_retornaMsj = string.Empty;
                        RegistroFacialOnlyControl_New(IdChofer, dtImagenes, out v_retornaMsj);
                        this.Alerta(v_retornaMsj);
                        IdTxtempresa.Value = string.Empty;
                        TxtEmpresa.Text = string.Empty;
                        IdTxtChofer.Value= string.Empty;
                        TxtChofer.Text = string.Empty;
                    }
                    else
                    {
                        this.Alerta("Intenete nuevamente por favor.");
                        return;
                    }
                }
                else
                {
                    this.Mostrar_Mensaje(2, "<b>Informativo! Debe seleccionar un colaborador valido para agregar la información </b>");
                    return;
                }
            }
        }

        private void RegistroFacialOnlyControl_New(string codigo_nomina,DataTable dtFotos, out string resultadoStr)
        {
            resultadoStr = string.Empty;
            //###################################################
            //  GRABA REGISTRO FACIAL EN LA BASE DE ONLY CONTROL
            //###################################################
            string img1 = null; string img2 = null; string img3 = null; string imgt1 = string.Empty; string imgt2 = string.Empty; string imgt3 = string.Empty;
            for (int x = 0; x < dtFotos.Rows.Count; x++)
            {
                string v_imageStr = dtFotos.Rows[x]["foto"].ToString();
                if (x == 0) { img1 = v_imageStr; imgt1 = dtFotos.Rows[x]["template"].ToString(); }
                if (x == 1) { img2 = v_imageStr; imgt2 = dtFotos.Rows[x]["template"].ToString(); }
                if (x == 2) { img3 = v_imageStr; imgt3 = dtFotos.Rows[x]["template"].ToString(); }
            }

            RespuestaSwNeuro obRfResult = serviciosCredenciales.ActualizaFace(IdTxtChofer.Value, img1, img2, img3, imgt1, imgt2, imgt3);

            if (obRfResult != null)
            {
                if (obRfResult.codigo != "1")
                {
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.ActualizaFace():{0}", obRfResult.mensaje));
                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepagocolaborador", "RegistroFacialOnlyControl_New", "1", Request.UserHostAddress);
                    //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                }

                resultadoStr = resultadoStr + string.Format("\\n RF SW OC: {1} de Imagenes del ID {0} ", IdTxtChofer.Value, obRfResult.mensaje);
            }
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.Actualiza_Paneles();
        }

        private void Actualiza_Paneles()
        {
            //UPCARGA.Update();
            //UpdatePanelpb.Update();
           // upresult2.Update();
        }

        private DataTable New_ExportFileUpload(string cedula)
        {
            string v_msjNotificacion;
            DataTable dtDocumentosFacial = new DataTable();
            dtDocumentosFacial.Columns.Add("idSolicitud");
            dtDocumentosFacial.Columns.Add("idSolcol");
            dtDocumentosFacial.Columns.Add("identificacion");
            dtDocumentosFacial.Columns.Add("secuencia");
            dtDocumentosFacial.Columns.Add("documento");
            dtDocumentosFacial.Columns.Add("extension");
            dtDocumentosFacial.Columns.Add("ruta");
            dtDocumentosFacial.Columns.Add("usuarioCrea");
            dtDocumentosFacial.Columns.Add("template");
            dtDocumentosFacial.Columns.Add("foto");

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
                        this.Alerta(" Licencia NEURO Inactiva - SW OC NEURO: " + _exit.mensaje + "  \\n No se realizo la subida de las fotos: \\n intente nuevamente. ");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                        v_msjNotificacion = "Licencia NEURO Inactiva - SW OC NEURO: " + _exit.mensaje + "  <br/> No se realizo la subida de las fotos: <br/>Intente nuevamente.";
                        this.Mostrar_Mensaje(2, v_msjNotificacion);

                        return null;
                    }
                }
                else
                {
                    credenciales.AddTraceRegistroFacial(0, 0, 0, "LICENCIA NEURO", "0 - VALIDA LICENCIA", "NO SE OBTUVO RESPUESTA DEL SW NEURO", false, v_usuario, out mError);
                    this.Alerta("Error al consumir SW OC NEURO: No se obtuvo respuesta");

                    v_msjNotificacion = "Error al consumir SW OC NEURO: No se obtuvo respuesta";
                    this.Mostrar_Mensaje(2, v_msjNotificacion);
                    
                    return null;
                }

                /////////////////////////////////////////////////////////
                //VALIDA Y PROCESA CADA IMAGEN ELEGIDA POR EL USUARIO
                /////////////////////////////////////////////////////////
                List<CSLSite.cliente.ListaImagenesRF> LstImagenes = new List<CSLSite.cliente.ListaImagenesRF>();
                List<CSLSite.cliente.ListaImagenesRF> LstImagenesFinal = new List<CSLSite.cliente.ListaImagenesRF>();

                byte[] v_imageBase64OC = null;
                string v_imageBase64StrOC = string.Empty;

                foreach (GridViewRow item in gvColaboradores.Rows)
                {
                    FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                    FileUpload fsupload1 = item.FindControl("fsupload1") as FileUpload;
                    FileUpload fsupload2 = item.FindControl("fsupload2") as FileUpload;

                    for (int i = 1; i <= 3; i++)
                    {
                        ///////////////////////////////////////////////////////////////////////////
                        //CONSUMO DE SW ONLYCONTROL PARA EL PROCESAMIENTO DE IMAGENES - IMAGETOICAO
                        ///////////////////////////////////////////////////////////////////////////
                        byte[] v_imageBase64 = null;

                        if (i == 1) { v_imageBase64 = fsupload.FileBytes; v_imageBase64OC = fsupload.FileBytes; v_imageBase64StrOC = Convert.ToBase64String(v_imageBase64OC); }
                        if (i == 2) { v_imageBase64 = fsupload1.FileBytes; }
                        if (i == 3) { v_imageBase64 = fsupload2.FileBytes; }

                        string v_imageBase64Str = Convert.ToBase64String(v_imageBase64);
                        var imageToIcao = swNeuro.imageToIcao(v_imageBase64Str);

                        if (!string.IsNullOrEmpty(imageToIcao))
                        {
                            _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(imageToIcao);

                            if (_exit.codigo != "1")
                            {
                                credenciales.AddTraceRegistroFacial(0, long.Parse(item.RowIndex.ToString()), i, "1 - IMAGETOICAO", "Imagen " + i.ToString() + " " + TxtChofer.Text, _exit.mensaje, false, v_usuario, out mError);
                                this.Alerta(string.Format("ERROR AL PROCESAR IMAGEN {0} de ID {1} {2} {3} - IMAGETOICAO - SW OC NEURO: {4} ", i.ToString(), item.RowIndex.ToString(), TxtChofer.Text, _exit.mensaje, "  \\n No se realizo la subida de las fotos: \\n intente nuevamente. "));
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                                v_msjNotificacion = string.Format("ERROR AL PROCESAR IMAGEN {0} de ID {1} {2} {3} - IMAGETOICAO - SW OC NEURO: {4} ", i.ToString(), item.RowIndex.ToString(), TxtChofer.Text, _exit.mensaje, "  <br/> No se realizo la subida de las fotos: <br/>Intente nuevamente. ");
                                this.Mostrar_Mensaje(2, v_msjNotificacion);
                                return null;
                            }
                            else
                            {
                                credenciales.AddTraceRegistroFacial(0, long.Parse(item.RowIndex.ToString()), i, "1 - IMAGETOICAO", "Imagen " + i.ToString() + " " + TxtChofer.Text, _exit.mensaje, true, v_usuario, out mError);
                                /////////////////////////////////////////////////////////////////////
                                //SE OBTIENE LA IMAGEN PROCESADA PARA GUARDARLA EN UNA LISTA TEMPORAL
                                ////////////////////////////////////////////////////////////////////
                                byte[] bytesResultImagenProcesada = Convert.FromBase64String(_exit.dato);
                                string stringResultImagenProcesada = _exit.dato;

                                /////////////////////////////////////////////////////////////////////////////////
                                //CONSUMO DE SW ONLYCONTROL PARA EL PROCESAMIENTO DE IMAGENES - IMAGETOTEMPLATE
                                ////////////////////////////////////////////////////////////////////////////////

                                var imageToTemplate = swNeuro.imageToTemplate(stringResultImagenProcesada);

                                if (!string.IsNullOrEmpty(imageToTemplate))
                                {
                                    _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(imageToTemplate);

                                    if (_exit.codigo != "1")
                                    {
                                        credenciales.AddTraceRegistroFacial(0, long.Parse(item.RowIndex.ToString()), i, "2 - IMAGETOTEMPLATE", "Imagen " + i.ToString() + " " + TxtChofer.Text, _exit.mensaje, false, v_usuario, out mError);
                                        this.Alerta(string.Format("ERROR AL PROCESAR IMAGEN {0} de ID {1} {2} {3} - IMAGETOTEMPLATE - SW OC NEURO: {4} ", i.ToString(), item.RowIndex.ToString(), TxtChofer.Text, _exit.mensaje, "  \\n No se realizo la subida de las fotos: \\n intente nuevamente. "));
                                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                                        v_msjNotificacion = string.Format("ERROR AL PROCESAR IMAGEN {0} de ID {1} {2} {3} - IMAGETOTEMPLATE - SW OC NEURO: {4} ", i.ToString(), item.RowIndex.ToString(), TxtChofer.Text, _exit.mensaje, "  <br/> No se realizo la subida de las fotos: <br/>Intente nuevamente. ");
                                        this.Mostrar_Mensaje(2, v_msjNotificacion);
                                        return null;
                                    }
                                    else
                                    {
                                        credenciales.AddTraceRegistroFacial(0, long.Parse(item.RowIndex.ToString()), i, "2 - IMAGETOTEMPLATE", "Imagen " + i.ToString() + " " + TxtChofer.Text, _exit.mensaje, true, v_usuario, out mError);
                                        ///////////////////////////////////////////////////////////////////////
                                        //SE OBTIENE LA IMAGEN TEMPLATE Y SE LA GUARDA EN LA LISTA EN MEMORIA
                                        //////////////////////////////////////////////////////////////////////
                                        byte[] v_imageBase64Template = Convert.FromBase64String(_exit.dato);
                                        string v_imageStrTemplate = _exit.dato;

                                        if (i == 1)
                                        {
                                            bytesResultImagenProcesada = v_imageBase64OC;
                                            stringResultImagenProcesada = v_imageBase64StrOC;
                                        }

                                        LstImagenes.Add(new CSLSite.cliente.ListaImagenesRF
                                        {
                                            idsolicitud = 0,
                                            idSolcol = 0,
                                            identificacion = cedula,
                                            secuencia = i,
                                            nombre = fsupload.FileName,
                                            ruta = Server.MapPath(fsupload.FileName),
                                            imagenProcesadaB64 = bytesResultImagenProcesada,
                                            imagenProcesadaStr = stringResultImagenProcesada,
                                            TemplateB64 = v_imageBase64Template,
                                            TemplateStr = v_imageStrTemplate,
                                            doc = "Imagen " + i.ToString(),
                                            ext = ".jpg"
                                        });
                                    }
                                }
                                else
                                {
                                    credenciales.AddTraceRegistroFacial(0, long.Parse(item.RowIndex.ToString()), i, "2 - IMAGETOTEMPLATE", "Imagen " + i.ToString() + " " + TxtChofer.Text, "NO SE OBTUVO RESPUESTA DEL SW NEURO", false, v_usuario, out mError);
                                    this.Alerta("Error al consumir SW IMAGETOTEMPLATE OC NEURO: No se obtuvo respuesta");

                                    v_msjNotificacion = "Error al consumir SW IMAGETOTEMPLATE OC NEURO: No se obtuvo respuesta";
                                    this.Mostrar_Mensaje(2, v_msjNotificacion);
                                    return null;
                                }
                            }
                        }
                        else
                        {
                            credenciales.AddTraceRegistroFacial(0, long.Parse(item.RowIndex.ToString()), i, "1 - IMAGETOICAO", "Imagen " + i.ToString() + " " + TxtChofer.Text, "NO SE OBTUVO RESPUESTA DEL SW NEURO", false, v_usuario, out mError);
                            this.Alerta("Error al consumir SW IMAGETOICAO OC NEURO: No se obtuvo respuesta");

                            v_msjNotificacion = "Error al consumir SW IMAGETOTEMPLATE OC NEURO: No se obtuvo respuesta";
                            this.Mostrar_Mensaje(2, v_msjNotificacion);
                            return null;
                        }
                    }

                    /////////////////////////////////////////////////
                    ///COMPARA LAS IMAGENES QUE SE SUBIRÁN AL LA DB
                    /////////////////////////////////////////////////
                    int v_contador = 0;
                    byte[] v_template = null;
                    byte[] v_image = null;
                    string v_templateStr = string.Empty;
                    string v_imageStr = string.Empty;


                
                    foreach (var lstImagen in LstImagenes)
                    {
                        v_contador += 1;
                        if (v_contador == 1)
                        {
                            v_template = lstImagen.TemplateB64;
                            v_image = lstImagen.imagenProcesadaB64;
                            v_templateStr = lstImagen.TemplateStr;
                            v_imageStr = lstImagen.imagenProcesadaStr;
                            continue;
                        }
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //CONSUMO DE SW ONLYCONTROL PARA EL PROCESAMIENTO DE IMAGENES - VERIFYTEMPLATE - VERIFYIMAGE
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        var templateVerify = swNeuro.VerifyTemplate(lstImagen.TemplateStr, v_templateStr);
                        //var imageVerify = swNeuro.VerifyImage(Convert.ToBase64String(lstImagen.imagenProcesadaB64), Convert.ToBase64String(v_image));
                        v_template = lstImagen.TemplateB64;
                        v_image = lstImagen.imagenProcesadaB64;
                        v_templateStr = lstImagen.TemplateStr;
                        v_imageStr = lstImagen.imagenProcesadaStr;

                        if (!string.IsNullOrEmpty(templateVerify))
                        {
                            _exit = JsonConvert.DeserializeObject<RespuestaSwNeuro>(templateVerify);

                            if (_exit.codigo != "1")
                            {
                                credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "3 - TEMPLATEVERIFY", lstImagen.nombre + " " + TxtChofer.Text, _exit.mensaje, false, v_usuario, out mError);
                                this.Alerta(string.Format("ERROR AL COMPARAR TEMPLATES DE IMAGEN {0} DEL COLABORADOR {1} - VERIFYTEMPLATE - SW OC NEURO: {2} {3}", lstImagen.secuencia, lstImagen.identificacion, _exit.mensaje, "  \\n No se realizo la subida de las fotos: \\n intente nuevamente. "));
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                                v_msjNotificacion = string.Format("ERROR AL COMPARAR TEMPLATES DE IMAGEN {0} DEL COLABORADOR {1} - VERIFYTEMPLATE - SW OC NEURO: {2} {3}", lstImagen.secuencia, lstImagen.identificacion, _exit.mensaje, "  <br/> No se realizo la subida de las fotos: <br/>Intente nuevamente. ");
                                this.Mostrar_Mensaje(2, v_msjNotificacion);
                                return null;
                            }
                            else
                            {
                                credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "3 - TEMPLATEVERIFY", lstImagen.nombre + " " + TxtChofer.Text, _exit.mensaje, true, v_usuario, out mError);
                            }
                        }
                        else
                        {
                            credenciales.AddTraceRegistroFacial(lstImagen.idsolicitud, lstImagen.idSolcol, lstImagen.secuencia, "3 - TEMPLATEVERIFY", lstImagen.nombre + " " + TxtChofer.Text, "NO SE OBTUVO RESPUESTA DEL SW NEURO", false, v_usuario, out mError);
                            this.Alerta("Error al consumir SW VERIFYTEMPLATE OC NEURO: No se obtuvo respuesta");

                            v_msjNotificacion = "Error al consumir SW VERIFYTEMPLATE OC NEURO: No se obtuvo respuesta";
                            this.Mostrar_Mensaje(2, v_msjNotificacion);
                            return null;
                        }
                    }

                    LstImagenesFinal.AddRange(LstImagenes);
                    LstImagenes.Clear();
                }

                foreach (var lstImagen in LstImagenesFinal)
                {
                    dtDocumentosFacial.Rows.Add(lstImagen.idsolicitud,
                                            lstImagen.idSolcol,
                                            lstImagen.identificacion,
                                            lstImagen.secuencia,
                                            lstImagen.doc,
                                            lstImagen.ext,
                                            "",
                                            Page.User.Identity.Name,
                                            lstImagen.TemplateStr,
                                            lstImagen.imagenProcesadaStr);
                }

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaboradorprovisional", "New_ExportFileUpload()", DateTime.Now.ToShortDateString(), "sistemas");
                //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                var error = "Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} " + number.ToString();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                //Response.Write("<script language='JavaScript'>var r=alert('" + error + "');if(r==true){window.close();}else{window.close();}</script>");

                v_msjNotificacion = error;
                this.Mostrar_Mensaje(2, v_msjNotificacion);
                return null;
            }

            dtDocumentosFacial.AcceptChanges();
            dtDocumentosFacial.TableName = "DocumentosRF";
            return dtDocumentosFacial;
        }

        protected void gvColaboradores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Actualiza_Paneles();
        }

        protected void btnVerRegistroFacial_Click(object sender, EventArgs e)
        {
            this.Ocultar_Mensaje();
            string IdEmpresa = string.Empty;
            string DesEmpresa = string.Empty;
            string IdChofer = string.Empty;
            string CedChofer = string.Empty;
            string DesChofer = string.Empty;

            if (!string.IsNullOrEmpty(TxtChofer.Text))
            {
                ChoferSelect = this.TxtChofer.Text.Trim();
                v_empresaSeleccionada = this.TxtEmpresa.Text.Trim();

                if (ChoferSelect.Split('-').ToList().Count > 1)
                {
                    xfinderDes.Visible = true;
                    sinresultadoDespacho.Visible = false;
                    IdChofer = ChoferSelect.Split('-').ToList()[0].Trim();
                    CedChofer = ChoferSelect.Split('-').ToList()[1].Trim();
                    DesChofer = ChoferSelect.Split('-').ToList()[2].Trim();

                    var onlyControl = new OnlyControl.OnlyControlService();
                    OnlyControl.DataTemplates clsinterfaz = new OnlyControl.DataTemplates();
                    string result = onlyControl.ConsultaFace(IdChofer, ref clsinterfaz);

                    ImgFoto1.Src = "data:image/jpg;base64," + clsinterfaz.Image1;
                    ImgFoto2.Src = "data:image/jpg;base64," + clsinterfaz.Image2;
                    ImgFoto3.Src = "data:image/jpg;base64," + clsinterfaz.Image3;
                }
                else
                {
                    xfinderDes.Visible = false;
                    sinresultadoDespacho.Visible = true;

                    this.Mostrar_Mensaje(2, "<b>Informativo! Debe seleccionar un colaborador valido para agregar la información </b>");
                    this.TxtChofer.Focus();
                    return;
                }
            }
            else
            {
                xfinderDes.Visible = false;
                sinresultadoDespacho.Visible = true;

                this.Mostrar_Mensaje(2, "<b>Informativo! Debe seleccionar un colaborador valido para agregar la información </b>");
                this.TxtChofer.Focus();
                return;
            }
            UPFotos.Update();
        }
    }
}