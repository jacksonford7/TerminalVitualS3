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
using System.IO;

namespace CSLSite
{
    public partial class solicitudUsuariosReestiba : System.Web.UI.Page
    {
        string idEstadoIngresado = "IN";
        //string tipoGrupo = ConfigurationManager.AppSettings["Reestiba"];
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
                //alerta.Visible = false;
                sinresultado.Visible = false;
                populateDrop(dptipotrafico, CslHelperServicios.getDetalleCatalogo(ConfigurationManager.AppSettings["TipoTrafico"]));
                if (dptipotrafico.Items.Count > 0)
                {
                    if (dptipotrafico.Items.FindByValue("000") != null)
                    {
                        dptipotrafico.Items.FindByValue("000").Selected = true;
                    }
                    dptipotrafico.SelectedValue = "0";
                }
                populateDrop(dptiposervicios, CslHelperServicios.getServicios());
                if (dptiposervicios.Items.Count > 0)
                {
                    if (dptiposervicios.Items.FindByValue("000") != null)
                    {
                        dptiposervicios.Items.FindByValue("000").Selected = true;
                    }
                    dptiposervicios.SelectedValue = "RES"; //Seleccionar por defecto Reestiba
                    dptiposervicios.Enabled = false;
                }


            }
        }
        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }
        protected void btgenerarServer_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    //obtengo la sesión del usuario logeado
                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //Validación 1 -> Primero se valida se ha escogido el contenedor uno y dos.
                    sinresultado.Visible = false;
                    string contenedorUno = contenedorNombreUno.Value.Trim();//contenedor1.Text.Trim();
                    int idCodContainerU = Int32.Parse(contenedor1HF.Value == "" ? "0" : contenedor1HF.Value);
                    string contenedorDos = contenedorNombreDos.Value.Trim();//contenedor2.Text.Trim();
                    int idCodContainerD = Int32.Parse(contenedor2HF.Value == "" ? "0" : contenedor2HF.Value);

                    string FCL_grp = dbconfig.GrupoUnidad(idCodContainerU, "M");
                    string MTY_grp = dbconfig.GrupoUnidad(idCodContainerD, "O");

                    var cfgs = dbconfig.GetActiveConfig(null, null, "grupos");
                    var cfg = cfgs.FirstOrDefault();

                    string[] lsg = null;
                    if (cfg != null)
                    {
                        lsg = cfg.config_value.Split(',');
                    }

                    if (lsg != null && lsg.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(FCL_grp))
                        {
                            if (lsg.Where(c => c.Contains(FCL_grp)).Count() > 0)
                            {
                                imagen.InnerText = string.Empty;
                                Utility.mostrarMensaje(this.Page, string.Format("Su solicitud no puede ser procesada debido a que el contenedor {0}, tiene un servicio activo.", contenedorUno));
                                return;
                            }
                        }
                        if (!string.IsNullOrEmpty(MTY_grp))
                        {
                            if (lsg.Where(c => c.Contains(MTY_grp)).Count() > 0)
                            {
                                imagen.InnerText = string.Empty;
                                Utility.mostrarMensaje(this.Page, string.Format("Su solicitud no puede ser procesada debido a que el contenedor {0}, tiene un servicio activo.", contenedorUno));
                                return;

                            }
                        }

                    }



#if DEBUG

                  //  Utility.mostrarMensaje(this.Page,"VALIDACIONES OK" );  
                   // return;
#endif
                    //*----fin del nuevo---///
                    if (string.IsNullOrEmpty(txtTipoProductoEmbalaje.Text))
                    {
                        imagen.InnerText = string.Empty;
                        Utility.mostrarMensaje(this.Page, "La descripción/embalaje del producto es un campo obligatorio");
                        return;

                    }
                    if (string.IsNullOrEmpty(contenedorUno) || contenedorUno.Contains("..."))
                    {
                        imagen.InnerText = string.Empty;
                        Utility.mostrarMensaje(this.Page, "Debe de ingresar los campos obligatorios.");
                        return;
                    }

                    if (idCodContainerU == 0)
                    {
                        imagen.InnerText = string.Empty;
                        Utility.mostrarMensaje(this.Page, "Debe de ingresar los campos obligatorios.");
                        return;
                    }

                    if (string.IsNullOrEmpty(contenedorDos) || contenedorDos.Contains("..."))
                    {
                        imagen.InnerText = string.Empty;
                        Utility.mostrarMensaje(this.Page, "Debe de ingresar los campos obligatorios.");
                        return;
                    }

                    if (idCodContainerD == 0)
                    {
                        imagen.InnerText = string.Empty;
                        Utility.mostrarMensaje(this.Page, "Debe de ingresar los campos obligatorios.");
                        return;
                    }

                    if (string.IsNullOrEmpty(txtNumDocAduana.Text.Trim()))
                    {
                        imagen.InnerText = string.Empty;
                        Utility.mostrarMensaje(this.Page, "Debe de ingresar los campos obligatorios.");
                        return;
                    }



                    if (txtNumDocAduana.Text.Trim().Length != 21)
                    {
                        imagen.InnerText = string.Empty;
                        Utility.mostrarMensaje(this.Page, "El numero solicitud de reestiba debe contener 21 caracteres ");
                        return;
                    }


                    //Verifica el archivo para subir.
                    /*
                    if ((archivoAduana.PostedFile != null) && (archivoAduana.PostedFile.ContentLength > 0))
                    {
                        string filePath = archivoAduana.PostedFile.FileName; // Obtiene la ruta del archivo
                        string filename1 = Path.GetFileName(filePath);       // Obtiene el nombre del archivo
                        string ext = Path.GetExtension(filename1);           // Obtiene la extensión, (tipo), de archivo

                        if (ext.Trim() != ".pdf")
                        {
                            imagen.InnerText = "";
                            Utility.mostrarMensaje(this.Page, "El archivo debe ser PDF.");
                            return;
                        }
                    }
                    else
                    {
                        imagen.InnerText = "";
                        Utility.mostrarMensaje(this.Page, "Porfavor, seleccione un archivo.");
                        return;
                    }
                    */
                    List<datosCabecera> datosCab = new List<datosCabecera>();


                    string TipoServicio = dptiposervicios.SelectedValue;
                    string TipoTrafico = dptipotrafico.SelectedValue;
                    string booking = bookingContenedor.Text.Trim();

                    string fechaPropuesta = String.IsNullOrEmpty(txtFechaPropuesta.ToString()) ? null : txtFechaPropuesta.Text.Trim() + " " +
                    DateTime.Now.ToShortTimeString();

                    int retorno = 0;

                    //Validación 2 --> Subir archivo de aduana.

                    /*
                    var ps = System.Configuration.ConfigurationManager.AppSettings["pesoarchivo"];
                    if (string.IsNullOrEmpty(ps))
                    {
                        ps = "2000000";
                    }
                   
                    
                    long ti = 0;
                    if (!long.TryParse(ps, out ti))
                    {

                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Ha ocurrido un problema al subir el archivo, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Conversion de tipo fallida"), "archivo", "btSubirArchivo_Click", "Hubo un error al subir el archivo.", sUser.loginname));
                        sinresultado.Visible = true;
                        return;
                    }
                   
                    
                    //archivo aduana
                    if (archivoAduana.PostedFile.ContentLength > ti)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Ha ocurrido un problema al subir el archivo, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Archivo Excede su tamaño nominal"), "archivo", "btSubirArchivo_Click", string.Format("Limite:{0}", ps), sUser.loginname));
                        sinresultado.Visible = true;
                        return;
                    }
                       
                    */

                    /*
                        var archivo = new app_start.oFile();
                        var rutaAlmacen = System.Configuration.ConfigurationManager.AppSettings["rutaArchivoAduana"];
                        if (string.IsNullOrEmpty(rutaAlmacen))
                        {
                            rutaAlmacen = Server.MapPath(rutaAlmacen);
                        }
                        rutaAlmacen = string.Concat(rutaAlmacen, "\\", System.IO.Path.GetFileName(archivoAduana.PostedFile.FileName));
                        rutaAlmacen = Utility.ComprobarFolderAndFile(rutaAlmacen);
                        if (rutaAlmacen == "-1")
                        {

                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("Ha ocurrido un problema al subir el archivo, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Archivo Excede su tamaño nominal"), "archivo", "btSubirArchivo_Click", string.Format("Error en permisos de escritura {0}", rutaAlmacen), sUser.loginname));
                            sinresultado.Visible = true;
                            return;
                        }
                        if (rutaAlmacen == "-2")
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("Ha ocurrido un problema al subir el archivo, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Archivo Excede su tamaño nominal"), "archivo", "btSubirArchivo_Click", string.Format("Error al comprobar un archivo {0}", rutaAlmacen), sUser.loginname));
                            sinresultado.Visible = true;
                            return;
                        }
                        archivoAduana.PostedFile.SaveAs(rutaAlmacen);
                        archivo.rutafisica = rutaAlmacen;
                        archivo.rutavirtual = rutaAlmacen;
                        archivo.creador = sUser.loginname;
                        archivo.nombre = Path.GetFileName(rutaAlmacen);
                        var token = HttpContext.Current.Request.Cookies["token"];
                        archivo.token = token != null ? token.Value : null;
                        if (!archivo.guardar())
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("Ha ocurrido un problema al grabar el archivo, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Archivo Excede su tamaño nominal"), "archivo", "btSubirArchivo_Click", string.Format("Error al garbar datos del archivo {0}", rutaAlmacen), sUser.loginname));
                            sinresultado.Visible = true;
                            return;
                        }
                        string fn = archivo.id.ToString();

                    */
                    string fn = string.Empty;
                    //Validación 3 --> guardo la cabecera de la solicitud de reestiba                    
                    try
                    {
                        datosCab = CslHelperServicios.cabeceraSolicitudReestiba(0,
                            TipoServicio, TipoTrafico, sUser.loginname, idEstadoIngresado,
                            sUser.grupo.ToString(), txtNumDocAduana.Text.Trim(),
                            fechaPropuesta, txtTipoProductoEmbalaje.Text.ToString().Trim(), txtComentario.Text.ToString().Trim(), fn);
                        retorno = int.Parse(datosCab[0].idSolicitud);
                    }
                    catch (Exception ex)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "reestiba", "reestiba_cabecera", contenedor1.Text.Trim(), sUser.loginname));
                        sinresultado.Visible = true;
                        imagen.InnerText = "";
                        return;
                    }

                    //VALIDACIÓN 4 --> Se guarda la el detalle con el id de la solicitud.
                    try
                    {
                        int retornoD = CslHelperServicios.detalleSolicitudReestiba(int.Parse(datosCab[0].idSolicitud.Trim()), idCodContainerU, idCodContainerD, sUser.loginname);
                    }
                    catch (Exception ex)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "reestiba", "reestiba_detalle", contenedor1.Text.Trim(), sUser.loginname));
                        sinresultado.Visible = true;
                        imagen.InnerText = "";
                        return;
                    }


                    string numpantalla = txtNumDocAduana.Text.Trim();

                    //Validación 5 --> Se envía un correo al usuario. (Se guarda en las tablas)
                    envioCorreo(sUser, retorno, contenedorUno, contenedorDos, numpantalla, datosCab[0].codigoSolicitud.Trim());


                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btgenerar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                    imagen.InnerText = "";
                }
            }
        }

        public void envioCorreo(usuario sUser, int idSolicitud, string contenedorUno, string contenedorDos, string booking, string codigoSolicitud)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string boknum = string.Empty;

            List<consultaCabeceraUsuario> solicitudUsuario = CslHelperServicios.consultaSolicitudUsuario(null, 0, null, null, null, null, false, null, idSolicitud.ToString());
            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.ToString();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.ToString();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.ToString();

            }

            var jmsg = new jMessage();
            string mail = string.Empty;
            // string destinatarios = turnoConsolidacion.GetMails();

            var destinatarios = string.Empty;
            mail = Utility.restiba_msg_ingreso(contenedorUno, contenedorDos, trafico, booking, productoEmbalaje, fechaPropuesta, comentarios);
            string error = string.Empty;
            //el mail del usuario logueado


            var cfgs = dbconfig.GetActiveConfig(null, null, null);
            var mail_destino = cfgs.Where(a => a.config_name.Contains("mail_restiba")).FirstOrDefault();

            //cambio
            var correoBackUp = cfgs.Where(a => a.config_name.Contains("correoBackUp")).FirstOrDefault();
            destinatarios = string.Format("{0};{1};{2}", correoBackUp != null ? correoBackUp.config_value : "no_cfg", correoUsuario, mail_destino != null ? mail_destino.config_value : "no_cfg");



            CLSDataCentroSolicitud.addMail(out error, sUser.email, codigoSolicitud + " - Solicitud de Reestiba.", mail, destinatarios, sUser.loginname, "", "");

            if (!string.IsNullOrEmpty(error))
            {
                alerta.Visible = true;
                alerta.InnerText = error;
                return;
            }
            else
            {
                imagen.InnerHtml = "";
                Utility.mostrarMensajeRedireccionando(this.Page, "Su solicitud " + codigoSolicitud + " ha sido generada para el servicio de Reestiba, revise su correo en unos minutos para mayor información.", "../csl/menudefault");
            }
        }

        public string invocacionEvento(string descripcionContenedor, string evento)
        {
            wsN4 g = new wsN4();

            string me = string.Empty;
            string errorN4 = string.Empty;

            StringBuilder newa = new StringBuilder();
            newa.Append("<icu><units>");
            newa.Append(string.Format("<unit-identity id=\"{0}\" type=\"{1}\">", descripcionContenedor, "CONTAINERIZED"));
            newa.Append("</unit-identity></units>");
            newa.Append("<properties><property tag=\"UnitRemark\" value=\"SERVICIO REESTIBA DE CARGA\"/></properties>");
            newa.Append(string.Format("<event id=\"{1}\" note=\"SERVICIO REESTIBA DE CARGA\" time-event-applied=\"{0}\" user-id=\"{2}\"/>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), evento, "MID_SEGURIDAD"));
            newa.Append("</icu>");

            var i = g.CallBasicService("ICT/ECU/GYE/CGSA", newa.ToString(), ref me);

            /*I ES LA RESPUESTA QUE DEVUELVE EL N4 Y me ES LA DESCRIPCION DEL MENSAJE DE ERROR*/
            if (i > 0)
            {
                errorN4 = me;
            }

            return errorN4;
        }

        protected void btSubirArchivo_Click(object sender, EventArgs e)
        {
            var t = this.getUserBySesion();
            var ps = System.Configuration.ConfigurationManager.AppSettings["pesoarchivo"];
            if (string.IsNullOrEmpty(ps))
            {
                ps = "2000000";
            }
            long ti = 0;
            if (!long.TryParse(ps, out ti))
            {

                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema al subir el archivo, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Conversion de tipo fallida"), "archivo", "btSubirArchivo_Click", "Hubo un error al subir el archivo.", t.loginname));
                sinresultado.Visible = true;
                return;
            }
            if (archivoAduana.PostedFile.ContentLength > ti)
            {
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema al subir el archivo, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Archivo Excede su tamaño nominal"), "archivo", "btSubirArchivo_Click", string.Format("Limite:{0}", ps), t.loginname));
                sinresultado.Visible = true;
                return;
            }
            var archivo = new app_start.oFile();
            if (archivoAduana.PostedFile != null && archivoAduana.PostedFile.ContentLength > 0)
            {

                string ext = Path.GetExtension(archivoAduana.PostedFile.FileName);
                if (!ext.Contains("pdf"))
                {
                    Utility.mostrarMensaje(this.Page, "El archivo debe ser PDF.");
                }


                var rutaAlmacen = System.Configuration.ConfigurationManager.AppSettings["rutaArchivoAduana"];
                if (string.IsNullOrEmpty(rutaAlmacen))
                {
                    rutaAlmacen = Server.MapPath(rutaAlmacen);
                }
                rutaAlmacen = string.Concat(rutaAlmacen, "\\", System.IO.Path.GetFileName(archivoAduana.PostedFile.FileName));
                rutaAlmacen = Utility.ComprobarFolderAndFile(rutaAlmacen);
                if (rutaAlmacen == "-1")
                {

                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema al subir el archivo, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Archivo Excede su tamaño nominal"), "archivo", "btSubirArchivo_Click", string.Format("Error en permisos de escritura {0}", rutaAlmacen), t.loginname));
                    sinresultado.Visible = true;
                    return;
                }
                if (rutaAlmacen == "-2")
                {
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema al subir el archivo, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Archivo Excede su tamaño nominal"), "archivo", "btSubirArchivo_Click", string.Format("Error al comprobar un archivo {0}", rutaAlmacen), t.loginname));
                    sinresultado.Visible = true;
                    return;
                }
                archivoAduana.PostedFile.SaveAs(rutaAlmacen);
                archivo.rutafisica = rutaAlmacen;
                archivo.rutavirtual = rutaAlmacen;
                archivo.creador = t.loginname;
                archivo.nombre = Path.GetFileNameWithoutExtension(rutaAlmacen);
                archivo.token = "";

                if (archivo.guardar())
                {

                }
            }
            else
            {
                Utility.mostrarMensaje(this.Page, "Porfavor, seleccione un archivo.");
            }

        }



    }
}