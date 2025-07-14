using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Web.Script.Services;
using ConectorN4;
using Logger;
using System.Text;
using System.Xml;


namespace CSLSite
{
    public partial class solicitudCorreccionExportacion : System.Web.UI.Page
    {
       // string idEstadoIngresado = "EP";
        string tipoGrupo = ConfigurationManager.AppSettings["Reestiba"];
        public static usuario sUser = null;
        public static UsuarioSeguridad user = null;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        
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
                //this.agencia.Value = user.ruc;
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
                alerta.Visible = false;
                sinresultado.Visible = false;
                populateDrop(ddlTipoTrafico, CslHelperServicios.getDetalleCatalogo(ConfigurationManager.AppSettings["TipoTrafico"]));
                populateDrop(ddlTipoCorreccion, CslHelperServicios.getDetalleCatalogo(ConfigurationManager.AppSettings["TipoCorreccion"]));

                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                hdfUsuario.Value = sUser.id.ToString();
                Seguridad s = new Seguridad();
                user = s.consultarUsuarioPorId(sUser.id);
                if (user != null)
                {
                    txtMail.Text = user.usuarioCorreo.Trim();
                    txtTelefono.Text = user.telefonoEmpresa.Trim();

                }
                if (ddlTipoTrafico.Items.Count > 0)
                {
                    if (ddlTipoTrafico.Items.FindByValue("000") != null)
                    {
                        ddlTipoTrafico.Items.FindByValue("000").Selected = true;
                    }
                    ddlTipoTrafico.SelectedValue = "EXPRT";
                    ddlTipoTrafico.Enabled = false;
                }

                if (ddlTipoCorreccion.Items.Count > 0)
                {
                    if (ddlTipoCorreccion.Items.FindByValue("0") != null)
                    {
                        ddlTipoCorreccion.Items.FindByValue("0").Selected = true;
                    }
                }
                /*populateDrop(dptipoestados, CslHelperServicios.getDetalleCatalogo(ConfigurationManager.AppSettings["TipoEstado"]));
                if (dptipoestados.Items.Count > 0)
                {
                    if (dptipoestados.Items.FindByValue("8") != null)
                    {
                        dptipoestados.Items.FindByValue("8").Selected = true;
                    }
                    idEstadoIngresado = int.Parse(dptipoestados.SelectedValue);
                }*/
                populateDrop(ddlTipoServicios, CslHelperServicios.getServicios());
                if (ddlTipoServicios.Items.Count > 0)
                {
                    if (ddlTipoServicios.Items.FindByValue("000") != null)
                    {
                        ddlTipoServicios.Items.FindByValue("000").Selected = true;
                    }
                    ddlTipoServicios.SelectedValue = "CIE";
                    ddlTipoServicios.Enabled = false;
                }

                txtAISV.Enabled = false;
                txtAISV.Text = "...";
                pnlAisv.Visible = false;
                pnlDaeActual.Visible = false;
                pnlDaeNueva.Visible = true;

            }
        }

        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }

        protected void ddlTipoCorreccion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoCorreccion.SelectedValue == ConfigurationManager.AppSettings["tipoCorreccionDaeExceso"].ToString())
            {
                txtAISV.Enabled = true;
                txtAISV.Text = "";
                pnlAisv.Visible = true;
            }
            else
            {
                if (ddlTipoCorreccion.SelectedValue == ConfigurationManager.AppSettings["tipoCorreccionDaeAisv"].ToString())
                {
                    txtAISV.Enabled = false;
                    txtAISV.Text = "...";
                    pnlAisv.Visible = false;
                    pnlDaeActual.Visible = false;
                    pnlDaeNueva.Visible = true;

                }
                else
                {
                    if (ddlTipoCorreccion.SelectedValue == ConfigurationManager.AppSettings["tipoCorreccionIntercambio"].ToString())
                    {
                        txtAISV.Enabled = false;
                        txtAISV.Text = "...";
                        pnlDaeNueva.Visible = true;
                        pnlDaeActual.Visible = true;
                        pnlAisv.Visible = false;
                    }
                    else
                    {
                        txtAISV.Enabled = false;
                        txtAISV.Text = "...";
                        pnlAisv.Visible = false;
                        pnlDaeActual.Visible = false;
                        pnlDaeNueva.Visible = true;
                    }

                }

            }

            contenedor1.Text = "...";
            contenedor1HF.Value = "";
            bookingContenedor.Text = "";
            bookingContenedorHF.Value = "";
            txtNumeroCarga.Text = "";
            txtPeso.Text = "";
            txtSello1.Text = "";
            txtSello2.Text = "";
            txtSello3.Text = "";
            txtSello4.Text = "";
            txtActualDae.Text = "";
            txtNuevoDae.Text = "";
        }
        
        public bool validacion()
        {
            bool validacion = false;
            this.alerta.Visible = false;
            this.alerta.InnerText = "";
            if (ddlTipoCorreccion.SelectedValue != "0")
            {
                if (ddlTipoCorreccion.SelectedValue == ConfigurationManager.AppSettings["tipoCorreccionDaeExceso"])
                {
                    if (string.IsNullOrEmpty(txtAISV.Text.Trim()) || string.IsNullOrEmpty(contenedor1HF.Value.ToString()) || 
                        string.IsNullOrEmpty(txtNuevoDae.Text.Trim().ToString()) ||  string.IsNullOrEmpty(txtMail.Text.Trim()))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "Por favor llene los campos obligatorios.";
                        return true;
                    }

                    if (txtNuevoDae.Text.Trim().Length != int.Parse(ConfigurationManager.AppSettings["longitudDae"].ToString()))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "Por favor, la longitud de la DAE debe ser de " + ConfigurationManager.AppSettings["longitudDae"].ToString();
                        return true;
                    }
                }
                else
                {
                    if (ddlTipoCorreccion.SelectedValue == ConfigurationManager.AppSettings["tipoCorreccionDaeAisv"])
                    {
                        if (string.IsNullOrEmpty(contenedor1HF.Value.ToString()) || string.IsNullOrEmpty(txtNuevoDae.Text.Trim().ToString()) || string.IsNullOrEmpty(txtMail.Text.Trim()))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Por favor llene los campos obligatorios.";
                            return true;
                        }

                        if (txtNuevoDae.Text.Trim().Length != int.Parse(ConfigurationManager.AppSettings["longitudDae"].ToString()))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Por favor, la longitud de la DAE debe ser de " + ConfigurationManager.AppSettings["longitudDae"].ToString();
                            return true;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtActualDae.Text.Trim()) || string.IsNullOrEmpty(contenedor1HF.Value.ToString()) || string.IsNullOrEmpty(txtNuevoDae.Text.Trim().ToString()) || string.IsNullOrEmpty(txtMail.Text.Trim()))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Por favor llene los campos obligatorios.";
                            return true;
                        }

                        if (txtNuevoDae.Text.Trim().Length != int.Parse(ConfigurationManager.AppSettings["longitudDae"].ToString()))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Por favor, la longitud de la DAE debe ser de " + ConfigurationManager.AppSettings["longitudDae"].ToString() + " dígitos.";
                            return true;
                        }
                    }

                }
            }
            else
            {
                alerta.Visible = true;
                alerta.InnerText = "Por favor seleccione un tipo de corrección.";
                return true;
            }

            if (hdfCorreoUsuario.Value.ToString() == "0")
            {
                alerta.Visible = true;
                alerta.InnerText = "Por favor verifique el formato de los correos que esta ingresando.";
                return true;
            }

            return validacion;

        }

        protected void btgenerar_Click(object sender, EventArgs e)
        {
            int retorno = 0;
            if (Response.IsClientConnected)
            {

                //NUEVA VALIDACION---->BLOQUEO PAN--->2020
                if (!string.IsNullOrEmpty(txtNumeroCarga.Text))
                {
                    //TOMAR EL NUMERO
                    Int64 kk;
                    if (Int64.TryParse(txtNumeroCarga.Text.Trim(), out kk))
                    {
                        //llamar a la funcion
                        if (jAisvContainer.UnidadBloqueadaPAN(kk))
                        {
                            Utility.mostrarMensaje(this.Page, String.Format("La unidad {0} tiene un bloqueo activo solicitado por las Autoridades que no permite continuar con su solicitud. Cualquier consulta adicional dirigir un correo a la casilla ec.sac@contecon.com.ec", contenedor1.Text));
                            alerta.Visible = true;
                            alerta.InnerHtml = String.Format("La unidad {0} tiene un bloqueo activo solicitado por las Autoridades que no permite continuar con su solicitud. <br/>Cualquier consulta adicional dirigir un correo a la casilla <a href='mailto:ec.sac@contecon.com.ec?subject=Bloqueo Autoridades'>ec.sac@contecon.com.ec</a>", contenedor1.Text);
                            return;
                        }
                    }
                }


              

                //--------------------------------------------------------------------
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    string TipoServicio = ddlTipoServicios.SelectedValue;
                    string descripcionServicio = ddlTipoServicios.Items.FindByValue(ddlTipoServicios.SelectedValue).Text;
                    string mensajeErrorN4 = string.Empty;

                    //obtengo la sesión del usuario logeado
                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //Validar la DAE 2020
                    string de = string.Empty;
                    if (!jAisvContainer.RevisionDAE(txtNuevoDae.Text, sUser.ruc, out de))
                    {
                        Utility.mostrarMensaje(this.Page, de);
                        alerta.Visible = true;
                        alerta.InnerHtml = String.Format("La DAE {1},{0} <br/>Cualquier consulta adicional dirigir un correo a la casilla <a href='mailto:ec.sac@contecon.com.ec?subject=DaeNoRegistra'>ec.sac@contecon.com.ec</a>", de, txtNuevoDae.Text);
                        return;
                    }

                    bool validacionCampos = validacion();

                    if (validacionCampos)
                    {
                        return;
                    }

                    //Validación 2 -> Si no hubo problema guardo la cabecera de la solicitud de ingreso de exportacion                    

                    string codigoSecuencia = string.Empty;
                    string codigoSecuenciaCII = string.Empty;
                    List<datosCabecera> datosCab = new List<datosCabecera>();
                    try
                    {
                        //
                        string xmlRespuesta = string.Empty;
                        if (ddlTipoCorreccion.SelectedValue == ConfigurationManager.AppSettings["tipoCorreccionIntercambio"].ToString())
                        {
                            //INTERCAMBIO DE DAE
                            IIE iie = CslHelperServicios.validarIIE(txtNumeroCarga.Text);
                            if (iie.estado != null)
                            {
                                if (iie.estado.Trim() == "A")
                                {
                                    codigoSecuencia = CslHelperServicios.generarCII(txtNumeroCarga.Text, contenedor1.Text, txtPeso.Text, txtNuevoDae.Text, txtAISV.Text, txtActualDae.Text, sUser.loginname, txtSello1.Text, txtSello2.Text, txtSello3.Text, txtSello4.Text);

                                }
                                else if (iie.estado.Trim() == "R")
                                {
                                    Utility.mostrarMensaje(this.Page, "El IIE generado previamente para dicho contenedor ha sido rechazado. No se puede procesar su solicitud");
                                    return;
                                }
                                else if (iie.estado.Trim() == "N")
                                {
                                    Utility.mostrarMensaje(this.Page, "El IIE generado previamente para dicho contenedor aún se encuentra en la cola de trabajo. Intente procesar nuevamente la solicitud en unos minutos.");
                                    return;
                                }
                                else if (iie.estado.Trim() == "S")
                                {
                                    Utility.mostrarMensaje(this.Page, "El IIE generado previamente para dicho contenedor ya ha sido enviado. Intente procesar nuevamente la solicitud en unos minutos.");
                                    return;
                                }
                                else
                                {
                                    Utility.mostrarMensaje(this.Page, "Ocurrió un error al enviar el IIE generado previamente para dicho contenedor.");
                                    return;
                                }

                            }
                            else
                            {
                                Utility.mostrarMensaje(this.Page, "No se encuentra ningún  IIE generado previamente para dicho contenedor.");
                                return;
                            }

                        }
                        else if (ddlTipoCorreccion.SelectedValue == ConfigurationManager.AppSettings["tipoCorreccionDaeExceso"].ToString())
                        {
                            //DAE EN EXECESO
                            
                            DateTime fechaActual = DateTime.Now;
                            CultureInfo ci = CultureInfo.InvariantCulture;
                            string fechaFormato = fechaActual.ToString("yyyy-mm-ddThh:mm:ss", ci).Trim() + 'Z';

                            //nuevo crear IIE
                            var trasnm = string.Empty;
                            Int64 gkey = 0;
                            if (!Int64.TryParse(txtNumeroCarga.Text.Trim(), out gkey))
                            {
                                Utility.mostrarMensaje(this.Page, "No se pudo realizar el CII de eliminación debido al numero de carga, por favor vuelva a generar la solicitud en unos minutos.");
                                return;
                            }
                            if (!CLSDataCentroSolicitud.crearIIE(gkey, bookingContenedor.Text, sUser.loginname, txtNuevoDae.Text, out trasnm))
                            {
                                Utility.mostrarMensaje(this.Page, trasnm);
                                return;

                            }
                            codigoSecuencia = trasnm;
                        }
                        else
                        {
                            IIE iie = CslHelperServicios.validarIIE(txtNumeroCarga.Text);
                            if (iie.estado != null)
                            {
                                if (iie.estado.Trim() == "A")
                                {
                                    codigoSecuenciaCII = CslHelperServicios.generarCII(txtNumeroCarga.Text, contenedor1.Text, txtPeso.Text, 
                                        txtNuevoDae.Text, txtAISV.Text, txtActualDae.Text, sUser.loginname, txtSello1.Text, txtSello2.Text, txtSello3.Text, txtSello4.Text);
                                }
                                else if (iie.estado.Trim() == "R")
                                {
                                    Utility.mostrarMensaje(this.Page, "El IIE generado previamente para dicho contenedor ha sido rechazado. No se puede procesar su solicitud");
                                    return;
                                }
                                else if (iie.estado.Trim() == "N")
                                {
                                    Utility.mostrarMensaje(this.Page, "El IIE generado previamente para dicho contenedor aún se encuentra en la cola de trabajo. Intente procesar nuevamente la solicitud en unos minutos.");
                                    return;
                                }
                                else if (iie.estado.Trim() == "S")
                                {
                                    Utility.mostrarMensaje(this.Page, "El IIE generado previamente para dicho contenedor ya ha sido enviado. Intente procesar nuevamente la solicitud en unos minutos.");
                                    return;
                                }
                                else
                                {
                                    Utility.mostrarMensaje(this.Page, "Ocurrió un error al enviar el IIE generado previamente para dicho contenedor.");
                                    return;
                                }
                            }
                            else
                            {
                                Utility.mostrarMensaje(this.Page, "No se encuentra ningún  IIE generado previamente para dicho contenedor.");
                                return;
                            }

                            if (codigoSecuenciaCII != "0")
                            {
                                DateTime fechaActual = DateTime.Now;
                                CultureInfo ci = CultureInfo.InvariantCulture;
                                string fechaFormato = fechaActual.ToString("yyyy-mm-ddThh:mm:ss", ci).Trim() + 'Z';
                                decimal d =0;
                                if (!decimal.TryParse(txtPeso.Text.Replace(".",","), out d))
                                {
                                    d = -1001;
                                }
                                d = d * 1000;
                                var s = String.Format("{0:0}", d);

                                //NUEVO CREAR IIE
                                var trasnm = string.Empty;
                                Int64 gkey =0;
                                if (!Int64.TryParse(txtNumeroCarga.Text.Trim(), out gkey))
                                {
                                    Utility.mostrarMensaje(this.Page, "No se pudo realizar el CII de eliminación, debido al numero de carga, por favor vuelva a generar la solicitud en unos minutos.");
                                    codigoSecuencia = "0";
                                    return;
                                }


                            if (!CLSDataCentroSolicitud.crearIIE(gkey, bookingContenedor.Text, sUser.loginname, txtNuevoDae.Text, out trasnm))
                            {
                                Utility.mostrarMensaje(this.Page, trasnm);
                                return;

                            }
                            codigoSecuencia = trasnm;

                            }
                            else
                            {
                                Utility.mostrarMensaje(this.Page, "No se pudo realizar el CII de eliminación, por favor vuelva a generar la solicitud en unos minutos.");
                                return;
                            }
                        }
                        if (codigoSecuencia != "0")
                        {

                            datosCab = CslHelperServicios.cabeceraSolicitud(0, TipoServicio, ddlTipoTrafico.SelectedValue.ToString(), null, bookingContenedorHF.Value.ToString(), null, sUser.loginname, ConfigurationManager.AppSettings["estadoProceso"].ToString(), sUser.grupo.ToString());
                        }
                        else
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = string.Format("Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(new Exception("Error en el proceso de generar el IIE/CII"), "solicitud", "solicitud_cabecera", retorno.ToString().Trim(), sUser.loginname));
                            sinresultado.Visible = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "solicitud", "solicitud_cabecera", retorno.ToString().Trim(), sUser.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    //Validación 3 --> Se guarda los detalles con el id de la solicitud generada.
                    int retornoD = CslHelperServicios.detalleSolicitudExportacionDae(int.Parse(datosCab[0].idSolicitud.Trim()), int.Parse(contenedor1HF.Value.ToString()), txtTelefono.Text.ToString(), txtMail.Text.ToString(), txtAISV.Text.ToString(), txtActualDae.Text.ToString(), txtNuevoDae.Text.ToString(), ddlTipoCorreccion.SelectedValue, sUser.loginname, codigoSecuencia, contenedor1.Text);
                    
                    var eventos = CslHelperServicios.consultaEventoPorTipoServicio_multiple(ddlTipoServicios.SelectedValue.Trim(), 
                        ddlTipoCorreccion.SelectedValue.Trim());
                    if (retornoD != 0)
                    {

                        //Validación 4 --> Se envía un correo al usuario. (Se guarda en las tablas)
                        var jmsg = new jMessage();
                        string mail = string.Empty;
                        string mail2 = string.Empty;

                        if (ddlTipoCorreccion.SelectedValue == ConfigurationManager.AppSettings["tipoCorreccionDaeExceso"].ToString())
                        {
                            mail = Utility.correccion_iie_msg_ingreso(contenedor1.Text, txtAISV.Text, 
                                ddlTipoCorreccion.SelectedItem.Text,
                                txtActualDae.Text, txtNuevoDae.Text, 
                                codigoSecuenciaCII, codigoSecuencia);
                        }

                        else
                        {
                            mail = Utility.correccion_iie_msg_ingreso(contenedor1.Text, txtAISV.Text, 
                                ddlTipoCorreccion.SelectedItem.Text, txtActualDae.Text, txtNuevoDae.Text, 
                                codigoSecuenciaCII, codigoSecuencia);
                        }
                       
                        string error = string.Empty;
                                              
                                               
                        /*AQUI EL NUEVO SCRIPT DE CORREOS*/
                        //mail_iie

                        var cfgs = dbconfig.GetActiveConfig(null, null, null);
                  
                        var correoBackUp = cfgs.Where(a => a.config_name.Contains("correoBackUpI")).FirstOrDefault();
                        var copia = cfgs.Where(a => a.config_name.Contains("mail_iie")).FirstOrDefault();
                        var destinatarios = string.Format("{0};{1};{2}",correoBackUp!=null?correoBackUp.config_value:"no_cfg",copia!=null?copia.config_value:"no_cfg",txtMail.Text );

                        CLSDataCentroSolicitud.addMail(out error, 
                            sUser.email, datosCab[0].codigoSolicitud.Trim() + " - Solicitud de Corrección de Ingreso de Exportación.",
                            mail, destinatarios, sUser.loginname, "", "");
                     

                        if (!string.IsNullOrEmpty(error))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = error;
                            return;
                        }
                        else
                        {

                            var tk = HttpContext.Current.Request.Cookies["token"];
                            ConectorN4.ObjectSesion sesObj = new ObjectSesion();
                            sesObj.clase = "solicitudCorreccionExportacion" + descripcionServicio;
                            sesObj.metodo = "btgenerar_Click";
                            sesObj.transaccion = "generarSolicitud" + descripcionServicio;
                            sesObj.usuario = sUser.loginname;
                            sesObj.token = tk.Value;

                            //eijo el paar el resto  el 1er evento que salga en la lista
                            var evento = eventos != null && eventos.Count > 0 ? eventos[0] : "error_de_sistema";
                            if (ddlTipoCorreccion.SelectedValue == ConfigurationManager.AppSettings["tipoCorreccionDaeExceso"].ToString())
                            {
                               //CORRECCION IIE DAE NUEVO
                                StringBuilder newa = new StringBuilder();
                                newa.Append("<icu><units>");
                                newa.Append(string.Format("<unit-identity id=\"{0}\" type=\"{1}\">", contenedor1.Text.Trim(), "CONTAINERIZED"));
                                newa.Append("</unit-identity></units>");
                                newa.AppendFormat("<properties><property tag=\"UnitRemark\" value=\"" + "Corrección DAE (Sin ingreso a Ecuapass)" + "\"/><property tag=\"UnitFlexString01\" value=\"{0}\"/><property tag=\"UnitFlexString05\" value=\"{1}\"/></properties>", txtNuevoDae.Text.Trim(), codigoSecuencia);
                                newa.Append(string.Format("<event id=\"{1}\" note=\"" + "Nuevo Ingreso de Exportación" + "\" time-event-applied=\"{0}\" user-id=\"{2}\"/>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), evento, "MID_CSL"));
                                newa.Append("</icu>");

                                string errorEvento = Utility.validacionN4(sesObj, newa.ToString());
                                if (string.IsNullOrEmpty(errorEvento))
                                {
                                    Utility.mostrarMensajeRedireccionando(this.Page, "Se genero el número " + datosCab[0].codigoSolicitud.Trim() + " y su número de entrega Ecuapass es " + codigoSecuencia + "  para su solicitud del servicio de " + descripcionServicio + ", revise su correo en unos minutos para mayor información.", "../cuenta/menu.aspx");
                                }
                                else
                                {
                                    Utility.mostrarMensajeRedireccionando(this.Page, "Ocurrió un error al momento de generar el evento de facturación", "../cuenta/menu.aspx");
                                }
                            }
                            else if (ddlTipoCorreccion.SelectedValue == ConfigurationManager.AppSettings["tipoCorreccionIntercambio"].ToString())
                            {
                                StringBuilder newa = new StringBuilder();
                                newa.Append("<icu><units>");
                                newa.Append(string.Format("<unit-identity id=\"{0}\" type=\"{1}\">", contenedor1.Text.Trim(), "CONTAINERIZED"));
                                newa.Append("</unit-identity></units>");
                                newa.AppendFormat("<properties><property tag=\"UnitRemark\" value=\"" + "SERVICIO DE CORRECCIÓN DE DAE - INGRESO" + "\"/><property tag=\"UnitFlexString01\" value=\"{0}\"/><property tag=\"UnitFlexString05\" value=\"{1}\"/></properties>", txtNuevoDae.Text.Trim(), codigoSecuencia);
                                newa.Append(string.Format("<event id=\"{1}\" note=\"" + "Nuevo Ingreso de Exportación" + "\" time-event-applied=\"{0}\" user-id=\"{2}\"/>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), evento, "MID_SEGURIDAD"));
                                newa.Append("</icu>");
                                
                                string errorEvento = Utility.validacionN4(sesObj, newa.ToString());
                                if (string.IsNullOrEmpty(errorEvento))
                                {
                                    Utility.mostrarMensajeRedireccionando(this.Page, "Se genero el número " + datosCab[0].codigoSolicitud.Trim() + " y su número de entrega Ecuapass es " + codigoSecuencia + " (CII) / " + codigoSecuencia + " (CII)  para su solicitud del servicio de " + descripcionServicio + ", revise su correo en unos minutos para mayor información.", "../cuenta/menu.aspx");
                                }
                                else
                                {
                                    Utility.mostrarMensajeRedireccionando(this.Page, "Ocurrió un error al momento de generar el evento de facturación", "../cuenta/menu.aspx");
                                }
                            }
                            else
                            {
                                StringBuilder newa = new StringBuilder();
                                newa.Append("<icu><units>");
                                newa.Append(string.Format("<unit-identity id=\"{0}\" type=\"{1}\">", contenedor1.Text.Trim(), "CONTAINERIZED"));
                                newa.Append("</unit-identity></units>");
                                newa.AppendFormat("<properties><property tag=\"UnitRemark\" value=\"" + "Corrección DAE(Cambio de DAE en Contenedor), user:{2}" + "\"/><property tag=\"UnitFlexString01\" value=\"{0}\"/><property tag=\"UnitFlexString05\" value=\"{1}\"/></properties>", txtNuevoDae.Text.Trim(), codigoSecuencia,sUser.loginname);
                                newa.Append(string.Format("<event id=\"{1}\" note=\"" + "Corrección DAE(Cambio de DAE en Contenedor)" + "\" time-event-applied=\"{0}\" user-id=\"{2}\"/>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), eventos[0], "MID_CSL"));
                                newa.Append("</icu>");
                                string errorEvento = Utility.validacionN4(sesObj, newa.ToString());
                                if (eventos.Count > 1)
                                {
                                    newa.Clear();
                                    newa.Append("<icu><units>");
                                    newa.Append(string.Format("<unit-identity id=\"{0}\" type=\"{1}\">", contenedor1.Text.Trim(), "CONTAINERIZED"));
                                    newa.Append("</unit-identity></units>");
                                    newa.AppendFormat("<properties><property tag=\"UnitRemark\" value=\"" + "Corrección DAE(Cambio de DAE en Contenedor) + Nuevo Ingreso, user:{0}" + "\"/></properties>", sUser.loginname);
                                    newa.Append(string.Format("<event id=\"{1}\" note=\"" + "Nuevo ingreso de exportación por CSL, user:{3}" + "\" time-event-applied=\"{0}\" user-id=\"{2}\"/>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), eventos[1], "MID_CSL", sUser.loginname));
                                    newa.Append("</icu>");
                                    errorEvento = Utility.validacionN4(sesObj, newa.ToString());
                                }
                                if (string.IsNullOrEmpty(errorEvento))
                                    Utility.mostrarMensajeRedireccionando(this.Page, "Se genero el número " + datosCab[0].codigoSolicitud.Trim() + ", su número de entrega de eliminación ECUAPASS es " + codigoSecuenciaCII + " y su número de entrega de nuevo ingreso ECUAPASS es " + codigoSecuencia + "  para su solicitud del servicio de " + descripcionServicio + ", revise su correo en unos minutos para mayor información.", "../cuenta/menu.aspx");
                                else
                                    Utility.mostrarMensajeRedireccionando(this.Page, "Ocurrió un error al momento de generar el evento de facturación", "../cuenta/menu.aspx");
                            }
                        }
                    }
                    else
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la generación de la solicitud, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(new Exception("Error al momento de generar la solicitud"), "Genera IIE", "btgenerar_Click", "Hubo un error al buscar", sUser.loginname));
                        sinresultado.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la generación de la solicitud, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btgenerar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }

    }
}