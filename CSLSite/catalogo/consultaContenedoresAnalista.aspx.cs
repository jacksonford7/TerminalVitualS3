using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ConectorN4;
using System.Text;

namespace CSLSite
{
    public partial class consultaContenedoresAnalista : System.Web.UI.Page
    {
        public string descripcionServicioG = "";
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                bloquearControles();
                sinresultado.Visible = false;
                string[] parametros = { "idSolicitud" };
                Page.ReviewQS(parametros, "No se puede buscar un booking sin haber seleccionado una solicitud.");
                usuario sUser = null;
                if (!IsPostBack)
                {

                    populateDrop(dpestados, CslHelperServicios.getDetalleCatalogo(ConfigurationManager.AppSettings["TipoEstado"]));
                    if (dpestados.Items.Count > 0)
                    {
                        if (dpestados.Items.FindByValue("000") != null)
                        {
                            dpestados.Items.FindByValue("000").Selected = true;
                        }
                        dpestados.SelectedValue = "0";
                    }
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var u = this.getUserBySesion();

                    try
                    {
                        List<consultaCabeceraAnalista> table = CslHelperServicios.consultaSolicitudAnalista(null, "", "", "", "", "", false, "", int.Parse(Request.QueryString["idSolicitud"].ToString()));
                        foreach (var item in table)
                        {
                            dpestados.SelectedValue = item.idEstado as string;

                            lblNumBooking.Text = string.IsNullOrEmpty(item.noBooking) ? "Ninguno" : item.noBooking.Trim();
                            lblNumCarga.Text = string.IsNullOrEmpty(item.noCarga) ? "Ninguno" : item.noCarga.Trim();
                            txtobservacion.Text = string.IsNullOrEmpty(item.observacion) ? "" : item.observacion.Trim();
                            lblTipoTrafico.Text = string.IsNullOrEmpty(item.trafico) ? "Ninguno" : item.trafico.Trim();
                            descripcionServicioG = item.servicio;
                            lblCodigoSolicitud.Text = string.IsNullOrEmpty(item.numSolicitud) ? "" : item.numSolicitud.Trim();
                        }

                        Session["descripcionServicioG"] = descripcionServicioG;
                    }
                    catch (Exception ex)
                    {
                        xfinder.Visible = false;
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lineas", "find_Click", dpestados.SelectedValue.Trim(), u != null ? u.loginname : "userNull"));
                        sinresultado.Visible = true;
                        return;
                    }

                    try
                    {
                        //Se busca el detalle según el id de la solicitud.
                        List<consultaDetalleUsuario> detalleSolicitud = new List<consultaDetalleUsuario>();
                        if (descripcionServicioG.Trim() == "Late Arrival")
                        {
                            detalleSolicitud = CslHelperServicios.consultaSolicitudDetalleUsuario(int.Parse(Request.QueryString["idSolicitud"].ToString()), "", "0", 0, "1");
                        }
                        else
                        {
                            detalleSolicitud = CslHelperServicios.consultaSolicitudDetalleUsuario(int.Parse(Request.QueryString["idSolicitud"].ToString()), "");
                        }

                        if (Response.IsClientConnected)
                        {
                            //ta.ClearBeforeFill = true;
                            //ta.Fill(table, "REFERENCIA", txtfinder.Text.Trim(), txtfinder.Text.Trim());
                            if (detalleSolicitud.Count > 0)
                            {
                                this.tablePagination.DataSource = detalleSolicitud;
                                this.tablePagination.DataBind();
                                xfinder.Visible = true;
                                sinresultado.Visible = false;
                                return;
                            }
                            xfinder.Visible = false;
                            sinresultado.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        xfinder.Visible = false;
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "solicitud", "detalle_solicitud", Request.QueryString["idSolicitud"].ToString(), u != null ? u.loginname : "userNull"));
                        sinresultado.Visible = true;
                    }
                }
            }
        }
        public void bloquearControles()
        {

            //En caso de que el estado de la solicitud sea rechazado o finalizado, el analista no puedo realizar ningún cambio.
            if (dpestados.SelectedValue == "RE" || dpestados.SelectedValue == "FI")
            {
                dpestados.Enabled = false;
                save.Enabled = false;
                txtobservacion.Enabled = false;
            }
        }
        protected void save_Click(object sender, EventArgs e)
        {
            int retorno = 0;
            usuario sUser = null;
            sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            var u = this.getUserBySesion();
            string mensajeN4 = "";
            string valorServicio = "";
            string descripcionContenedor = "";
            descripcionServicioG = Session["descripcionServicioG"].ToString();
            switch (descripcionServicioG.Trim())
            {
                case "Verificación de Sellos":
                    valorServicio = "SEL";
                    break;
                case "Repesaje":
                    valorServicio = "BAS";
                    break;
                case "Reestiba":
                    valorServicio = "RES";
                    break;
            }

            string evento = CslHelperServicios.consultaEventoPorServicio(valorServicio);
            string mensajeContenedor = "";
            string actualizacionS = "";
            string servicio = "0";
            string a = "";
            try
            {
                if (!String.IsNullOrEmpty(txtobservacion.Text.Trim()))
                {
                    //Se actualiza el estado y observación de la solicitud.
                    retorno = CslHelperServicios.actualizacionSolicitudAnalista(int.Parse(Request.QueryString["idSolicitud"].ToString()), dpestados.SelectedValue.Trim(), txtobservacion.Text.Trim(), sUser.loginname);

                    var tk = HttpContext.Current.Request.Cookies["token"];
                    ConectorN4.ObjectSesion sesObj = new ObjectSesion();
                    sesObj.clase = "solicitudUsuarios" + descripcionServicioG;
                    sesObj.metodo = "btgenerar_Click";
                    sesObj.transaccion = "generarSolicitud" + descripcionServicioG;
                    sesObj.usuario = sUser.loginname;
                    sesObj.token = tk.Value;

                    var servicioAplicado = false;

                    foreach (RepeaterItem item in tablePagination.Items)
                    {



                        //En caso de ser servicio de Repesaje o Verificación de Sellos, y se finaliza el servicio, se carga el evento.
                        DropDownList ddlServicio = (DropDownList)item.FindControl("servicioddl");
                        Label idDetalleSolicitud = (Label)item.FindControl("idDetalleSolicitud");

                        if (ddlServicio.Visible)
                        {
                            if (ddlServicio.SelectedValue.Contains("SI"))
                            {
                                servicio = "1";
                                List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(0, null, idDetalleSolicitud.Text.ToString());
                                foreach (var itemD in listaDetalle)
                                {
                                    mensajeContenedor = invocacionEvento(itemD.descripcionContenedor, evento, descripcionServicioG);
                                    //mensajeN4 = quitarGrupo(itemD.descripcionContenedor, "");
                                    a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"></unit-identity></units><properties><property tag=\"RoutingGroupRef\" value=\"{1}\"/></properties></icu>", itemD.descripcionContenedor, "");
                                    mensajeN4 = Utility.validacionN4(sesObj, a);

                                    if (!String.IsNullOrEmpty(mensajeContenedor))
                                    {
                                        this.sinresultado.Attributes["class"] = string.Empty;
                                        this.sinresultado.Attributes["class"] = "msg-critico";
                                        this.sinresultado.InnerText = string.Format(mensajeContenedor + "Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(new Exception(mensajeContenedor), "consultaAnalistas", "btGenerarSolicitud", itemD.descripcionContenedor, sUser.loginname));
                                        sinresultado.Visible = true;
                                        return;
                                    }
                                }
                            }

                            actualizacionS = CslHelperServicios.actualizacionSolicitudOperario(idDetalleSolicitud.Text.ToString(), null, null, sUser.loginname, null, servicio);
                        }
                        else if (dpestados.SelectedValue.Trim() == "FI")
                        {
                            //Se quita el grupo de los procesos, enviado vacio.
                            List<consultaDetalleUsuario> listaDetalleD = CslHelperServicios.consultaSolicitudDetalleUsuario(int.Parse(Request.QueryString["idSolicitud"].ToString()), null);
                            foreach (var itemD in listaDetalleD)
                            {
                                //mensajeN4 = quitarGrupo(itemD.descripcionContenedor, "");
                                a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"></unit-identity></units><properties><property tag=\"RoutingGroupRef\" value=\"{1}\"/></properties></icu>", itemD.descripcionContenedor, "");
                                mensajeN4 = Utility.validacionN4(sesObj, a);

                                if (!string.IsNullOrEmpty(itemD.descripcionContenedor))
                                {
                                    descripcionContenedor = itemD.descripcionContenedor;
                                }
                            }
                        }
                        else if (descripcionServicioG == "Reestiba")
                        {
                            //Cuando el servicio de Reestiba se cambia a estado EN PROCESO, (significa que está aprobado), 
                            if (dpestados.SelectedValue.Trim() == "EP")
                            {
                                //Se quita el grupo de los procesos, enviado vacio.
                                if (!servicioAplicado)
                                {
                                    List<consultaDetalleUsuario> listaDetalleD = CslHelperServicios.consultaSolicitudDetalleUsuario(int.Parse(Request.QueryString["idSolicitud"].ToString()), null);
                                    foreach (var itemD in listaDetalleD)
                                    {
                                        mensajeContenedor = invocacionEventoGrupo(itemD.descripcionContenedor, evento, descripcionServicioG, valorServicio, sesObj);
                                        if (!string.IsNullOrEmpty(itemD.descripcionContenedor))
                                        {
                                            descripcionContenedor = itemD.descripcionContenedor;
                                        }
                                    }
                                    servicioAplicado = true;
                                }
                            }
                        }
                    }


                    if (!String.IsNullOrEmpty(mensajeN4))
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        Exception errorN4 = new Exception(mensajeN4);
                        this.sinresultado.InnerText = string.Format(mensajeN4 + "Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(new Exception(mensajeN4), "solicitudUsuarios", "btGenerarSolicitud", descripcionContenedor, sUser.loginname));
                        //this.sinresultado.InnerText = mensajeN4;
                        sinresultado.Visible = true;
                        return;
                    }

                    //Se envia el correo al usuario notificandole el cambio de su solicitud.
                    envioCorreo(int.Parse(Request.QueryString["idSolicitud"].ToString()), sUser);
                }
                else
                {
                    //xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("La observación es de caracter obligatoria.");
                    sinresultado.Visible = true;
                }
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lineas", "find_Click", dpestados.SelectedValue.Trim(), u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
            finally
            {

            }
        }

        public void envioCorreo(int idSolicitud, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";


            List<consultaCabeceraUsuario> solicitudUsuario = CslHelperServicios.consultaSolicitudUsuario(null, 0, null, null, null, null, false, null, idSolicitud.ToString());
            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();

            }

            var jmsg = new jMessage();


            var soliunica = solicitudUsuario.FirstOrDefault();




            var cfgs = dbconfig.GetActiveConfig(null, null, null);
      
            var correoBackUp = cfgs.Where(a => a.config_name.Contains("correoBackUp")).FirstOrDefault();
            CSLSite.dbconfig cfg = null;
            string mail = string.Empty;
            string destinatarios = string.Empty;
            var user_email = correoUsuario;


            switch (descripcionServicio)
            {
                case "Reestiba":
                    cfg = cfgs.Where(v => v.config_name.Contains("mail_restiba")).FirstOrDefault();
                    destinatarios = string.Format("{0};{1}", correoBackUp.config_value, cfg != null ? cfg.config_value : "ec.sac@contecon.com.ec");
                    if (descripcionEstado.Trim().Contains("En Proceso"))
                    {
                        mail = getBodyReestiba(solicitudUsuario, sUser);
                    }
                    else if (descripcionEstado.Contains("Finalizada"))
                    {
                        mail = getBodyReestiba(solicitudUsuario, sUser);
                    }
                    else
                    {
                        mail = getBodyReestibaNoAceptada(solicitudUsuario, sUser);
                        destinatarios = correoBackUp != null ? correoBackUp.config_value : "ec.sac@contecon.com.ec";
                    }
                    break;
                case "Repesaje":
                    cfg = cfgs.Where(v => v.config_name.Contains("mail_repesaje")).FirstOrDefault();
                    destinatarios = string.Format("{0};{1}", correoBackUp.config_value, cfg != null ? cfg.config_value : "ec.sac@contecon.com.ec");
                    mail = getBodyRepesaje(solicitudUsuario, sUser);
                    break;
                case "Verificación de Sellos":
                    mail = getBodyVerificacion(solicitudUsuario, sUser);
                    cfg = cfgs.Where(v => v.config_name.Contains("mail_sellos")).FirstOrDefault();
                    destinatarios = string.Format("{0};{1}", correoBackUp.config_value, cfg != null ? cfg.config_value : "ec.sac@contecon.com.ec");
                    break;
                case "Late Arrival":
                    mail = getBodyLateArrival(solicitudUsuario, sUser);
                    cfg = cfgs.Where(v => v.config_name.Contains("mail_late")).FirstOrDefault();
                    destinatarios = string.Format("{0};{1}", correoBackUp.config_value, cfg != null ? cfg.config_value : "ec.sac@contecon.com.ec");
                    break;
                case "Etiquetado - Desetiquetado Unidades":
                    mail = getBodyEtiquetadoDes(solicitudUsuario, sUser);
                    cfg = cfgs.Where(j => j.config_name.Contains("mail_imo")).FirstOrDefault();
                    destinatarios = string.Format("{0};{1}", correoBackUp.config_value, cfg != null ? cfg.config_value : "ec.sac@contecon.com.ec");
                    break;
                case "Revisión Técnica Refrigerada":
                    mail = getBodyRevisionTecnica(solicitudUsuario, sUser);
                    cfg = cfgs.Where(j => j.config_name.Contains("mail_rtr")).FirstOrDefault();
                    destinatarios = string.Format("{0};{1}", correoBackUp.config_value,  cfg != null ? cfg.config_value : "ec.sac@contecon.com.ec");
                    break;
                default:
                    mail = getBodyGeneral(solicitudUsuario, sUser);
                    cfg = cfgs.Where(j => j.config_name.Contains("mail_iie")).FirstOrDefault();
                    destinatarios = string.Format("{0};{1}", correoBackUp.config_value,  cfg != null ? cfg.config_value : "ec.sac@contecon.com.ec");
                    break;
            }

            string mensaje = string.Empty;
            string clase = "";
            CLSDataCentroSolicitud.addMail(out mensaje, user_email,
                codigoSolicitud + " - Solicitud de " + descripcionServicio, mail, destinatarios, sUser.loginname, "", "");

            if (!string.IsNullOrEmpty(mensaje))
            {
                clase = "msg-critico";
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = clase;
                this.sinresultado.InnerText = mensaje;
                sinresultado.Visible = true;
            }
            else
            {

                bloquearControles();
                Session["descripcionServicioG"] = null;
                Utility.mostrarMensaje(this.Page, "Datos actualizados con éxito. Se le notificará al usuario por correo el cambio de estado de la solicitud.", true);
            }

        }

        public static string getClass(object valor, object fk, object inactive)
        {
            if (fk == null || fk.ToString().Trim().Length <= 0 || fk.ToString().Trim().ToUpper().Contains("BBK"))
            {
                return "point";
            }
            if (fk.ToString().Trim().ToUpper().Contains("LCL") && inactive != null)
            {
                return "point";
            }
            if (valor == null)
            {
                return "point rowdis";
            }
            int i = 0;
            if (!int.TryParse(valor.ToString(), out i))
            {
                return "point rowdis";
            }
            if (i > 0)
            {
                return "point";
            }
            return "point rowdis";
        }

        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }

        public string getBodyReestiba(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string box = string.Empty;

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                box = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
            }

            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            string contenedorUno = "";
            string contenedorDos = "";

            //Añade los n contenedores al correo.
            foreach (var item in listaDetalle)
            {
                if (item.noFila == "1")
                {
                    contenedorUno = item.descripcionContenedor.Trim();
                }

                if (item.noFila == "2")
                {
                    contenedorDos = item.descripcionContenedor.Trim();
                }
            }

            if (descripcionEstado.Contains("Finalizada"))
            {
                mail = Utility.restiba_fin(contenedorUno, contenedorDos, trafico, box, productoEmbalaje, fechaPropuesta, comentarios, observacion);
            }
            if (descripcionEstado.Contains("En Proceso"))
            {
                mail = Utility.restiba_msg_proceso(contenedorUno, contenedorDos, trafico, box, productoEmbalaje, fechaPropuesta, comentarios, observacion);
            }

            return mail;
        }

        public string getBodyReestibaNoAceptada(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
            }

            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            string contenedorUno = "";
            string contenedorDos = "";

            //Añade los n contenedores al correo.
            foreach (var item in listaDetalle)
            {
                if (item.noFila == "1")
                {
                    contenedorUno = item.descripcionContenedor.Trim();
                }

                if (item.noFila == "2")
                {
                    contenedorDos = item.descripcionContenedor.Trim();
                }
            }

            var jmsg = new jMessage();
            string destinatarios = turnoConsolidacion.GetMails();

            mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/><br/>De acuerdo a su solicitud, le informamos que ha registrado la Solicitud de Reestiba de mercancías de la unidad detallada a continuación: <br/><br/>", sUser.nombres, sUser.apellidos));
            mail = string.Concat(mail, string.Format("<strong>Solicitud de reestiba SENAE: </strong>{0}<br/><strong>Tráfico: </strong>{1}<br/> <strong>Contenedor Lleno: </strong>{2}<br/><strong>Contenedor Vacío: </strong>{3}<br/><strong>Producto y embalaje: </strong>{4}<br/><strong>Fecha y hora propuesta: </strong>{5}<br/><strong>Comentarios: </strong>{6}<br/><strong>Estado: </strong>{7}<br/><br/>", "019013162015RE000001P", trafico, contenedorUno, contenedorDos, productoEmbalaje, fechaPropuesta, comentarios, descripcionEstado));

            if (trafico == "IMPRT")
            {
                mail = string.Concat(mail, "<br/><br/>Se le notificará por este medio, la fecha y hora de la programación de la Reestiba. Es responsabilidad del importador gestionar con el Servicio Nacional de Aduana del Ecuador (SENAE) que el funcionario asignado se encuentre presente en la fecha y hora planificada para esta operación.");
            }
            else
            {
                mail = string.Concat(mail, "<br/><br/>Se le notificará por este medio, la fecha y hora de la programación de la Reestiba. Es responsabilidad del exportador presentar la comunicación sobre la reestiba solicitada a la Policía Nacional Antinarcóticos (PNA), quienes dispondrán en el documento si la operación será efectuada con un delegado de esta autoridad, el mismo que deberá ser presentado en la fecha y hora planificada para esta operación.");
            }

            mail = string.Concat(mail, "<br/><br/>Es un placer servirle, <br/> Atentamente, <br/> <b>Terminal Virtual</b> <br/> Contecon Guayaquil S.A. CGSA <br/> An ICTSI Group Company <br/><br/> <b>El contenido de este mensaje es informativo, por favor no responda este correo.</b>");

            return mail;
        }

        public string getBodyRepesaje(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string numCarga = "";
            string numBooking = "";

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                numCarga = String.IsNullOrEmpty(datoSolicitud.noCarga) ? null : datoSolicitud.noCarga.Trim();
                numBooking = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
            }
            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            mail = Utility.repesaje_msg_proceso(listaDetalle.Select(u => u.descripcionContenedor).ToList(), trafico, descripcionEstado, numBooking, numCarga, observacion);

            return mail;
        }

        public string getBodyVerificacion(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string numCarga = "";
            string numBooking = "";

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                numCarga = String.IsNullOrEmpty(datoSolicitud.noCarga) ? null : datoSolicitud.noCarga.Trim();
                numBooking = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
            }
            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            mail = Utility.verificacion_msg_proceso(listaDetalle.Select(u => u.descripcionContenedor).ToList(), trafico, descripcionEstado, numBooking, numCarga, observacion);
            return mail;
        }

        public string getBodyGeneral(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string numCarga = "";
            string numBooking = "";

            string codiser = string.Empty;

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                numCarga = String.IsNullOrEmpty(datoSolicitud.noCarga) ? null : datoSolicitud.noCarga.Trim();
                numBooking = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
                codiser = String.IsNullOrEmpty(datoSolicitud.codigoServicio) ? null : datoSolicitud.codigoServicio.Trim();
            }

            mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/><br/> De acuerdo a su solicitud, le informamos que se ha modificado el servicio " + descripcionServicio + " con la(s) unidad(es) detalladas a continuación: <br/><br/>", sUser.nombres, sUser.apellidos));

            if (!String.IsNullOrEmpty(observacion))
            {
                mail = string.Concat(mail, string.Format("<p style='border:1px solid orange;'>{0}</p>", observacion));
            }


            mail = string.Concat(mail, "<strong>INFORMACIÓN DE LA CARGA:</strong><br/><br/>");


            mail = string.Concat(mail, string.Format("<strong>Tráfico: </strong>{0}<br/>", trafico));





            if (!String.IsNullOrEmpty(numCarga))
            {
                mail = string.Concat(mail, string.Format("<strong>Número de Carga: </strong>{0}<br/>", numCarga));
            }

            if (!String.IsNullOrEmpty(numBooking))
            {
                mail = string.Concat(mail, string.Format("<strong>Número de Booking: </strong>{0}<br/>", numBooking));
            }

            mail = string.Concat(mail, string.Format("<strong>Estado:</strong>{0}<br/>", descripcionEstado));

            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            mail = string.Concat(mail, "<br/><strong>CONTENEDOR(ES):</strong><br/><br/>");

            //Añade los n contenedores al correo.
            foreach (var item in listaDetalle)
            {
                mail = string.Concat(mail, string.Format("{0}<br/>", item.descripcionContenedor));
            }

            mail = string.Concat(mail, "<br/><br/>Es un placer servirle, <br/><br/> Atentamente, <br/> <b>Terminal Virtual</b> <br/> Contecon Guayaquil S.A. CGSA <br/> An ICTSI Group Company <br/><br/> <b>El contenido de este mensaje es informativo, por favor no responda este correo.</b>");

            //Nuevo para exportacion =
            var cntr = listaDetalle.FirstOrDefault();
            if (cntr != null)
            {
                //CI-ORRECION
                mail = Utility.correccion_iie_msg_fin(cntr.descripcionContenedor, null, descripcionServicio, null, null, null, null, observacion);
            }

            if (codiser.Contains("CRJ"))
            {
                mail = string.Empty;
                mail = Utility.cerrojo_msg(listaDetalle.Select(u => u.descripcionContenedor).ToList(), "P", observacion);
            }
            return mail;
        }

        public string getBodyLateArrival(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string numCarga = "";
            string numBooking = "";

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                numCarga = String.IsNullOrEmpty(datoSolicitud.noCarga) ? null : datoSolicitud.noCarga.Trim();
                numBooking = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
            }
            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null, "0", 0, "1");
            mail = Utility.late_arriva_proc(listaDetalle.Select(u => u.descripcionContenedor).ToList(), trafico, descripcionEstado, numBooking, numCarga);
            return mail;
        }
        public string getBodyEtiquetadoDes(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string numCarga = "";
            string numBooking = "";

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                numCarga = String.IsNullOrEmpty(datoSolicitud.noCarga) ? null : datoSolicitud.noCarga.Trim();
                numBooking = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
            }

            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            mail = Utility.etiqueta_msg_proceso(listaDetalle.Select(u => u.descripcionContenedor).ToList(), trafico, descripcionEstado, numBooking, numCarga);

            return mail;
        }
        public string getBodyRevisionTecnica(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string numCarga = "";
            string numBooking = "";

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                numCarga = String.IsNullOrEmpty(datoSolicitud.noCarga) ? null : datoSolicitud.noCarga.Trim();
                numBooking = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
            }
            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            mail = Utility.tecnica_msg_proceso(listaDetalle.Select(u => u.descripcionContenedor).ToList(), trafico, descripcionEstado, numBooking, numCarga);
            return mail;
        }



        public string invocacionEvento(string descripcionContenedor, string evento, string descripcionServicio)
        {
            wsN4 g = new wsN4();

            string me = string.Empty;
            string errorN4 = string.Empty;

            StringBuilder newa = new StringBuilder();
            newa.Append("<icu><units>");
            newa.Append(string.Format("<unit-identity id=\"{0}\" type=\"{1}\">", descripcionContenedor, "CONTAINERIZED"));
            newa.Append("</unit-identity></units>");
            newa.Append(string.Format("<properties><property tag=\"UnitRemark\" value=\"{0}\"/></properties>", "SERVICIO " + descripcionServicio));
            newa.Append(string.Format("<event id=\"{1}\" note=\"{3}\" time-event-applied=\"{0}\" user-id=\"{2}\"/>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), evento, "MID_CSL", "SERVICIO " + descripcionServicio));
            newa.Append("</icu>");

            var i = g.CallBasicService("ICT/ECU/GYE/CGSA", newa.ToString(), ref me);
            g.Dispose();
            /*I ES LA RESPUESTA QUE DEVUELVE EL N4 Y me ES LA DESCRIPCION DEL MENSAJE DE ERROR*/
            if (i > 0)
            {
                errorN4 = me;
            }

            return errorN4;
        }
        public string invocacionEventoGrupo(string descripcionContenedor, string evento, string descripcionServicio, string grupo, ObjectSesion cliente)
        {
            string errorN4 = string.Empty;
            StringBuilder newa = new StringBuilder();
            newa.Append("<icu><units>");
            newa.Append(string.Format("<unit-identity id=\"{0}\" type=\"{1}\">", descripcionContenedor, "CONTAINERIZED"));
            newa.Append("</unit-identity></units>");
            newa.Append(string.Format("<properties><property tag=\"UnitRemark\" value=\"{0}\"/><property tag=\"RoutingGroupRef\" value=\"{1}\"/></properties>", string.Format("Servicio {0}, user:{1}", descripcionServicio, cliente.usuario), grupo));
            newa.Append(string.Format("<event id=\"{1}\" note=\"{3}\" time-event-applied=\"{0}\" user-id=\"{2}\"/>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), evento, "MID_CSL", "SERVICIO " + descripcionServicio));
            newa.Append("</icu>");
            errorN4 = Utility.validacionN4(cliente, newa.ToString());
            return errorN4;
        }
        protected void tablePagination_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            descripcionServicioG = Session["descripcionServicioG"].ToString();
            if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
            {
                if (descripcionServicioG == "Repesaje" || descripcionServicioG == "Verificación de Sellos")
                {
                    DropDownList servicioddl = ((DropDownList)e.Item.FindControl("servicioddl"));
                    servicioddl.Visible = true;
                }

                if (descripcionServicioG != "Verificación de Sellos")
                {
                    var colTD = e.Item.FindControl("tipoVerificacionTD");
                    colTD.Visible = false;
                }
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    if (descripcionServicioG != "Verificación de Sellos")
                    {
                        var colTH = e.Item.FindControl("tipoVerificacionTH");
                        colTH.Visible = false;
                    }
                }
            }
        }

    }
}