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
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Text.RegularExpressions;

namespace CSLSite
{
    public partial class solicitudUsuariosLateArrival : System.Web.UI.Page
    {


        public string Grid
        {
            get { return (string)Session["Grid_vacios"]; }
            set { Session["Grid_vacios"] = value; }
        }

        public List<contenedoresCerrojoElectronico> GridContenedoresLateArrival
        {
            get { return (List<contenedoresCerrojoElectronico>)Session["GridContenedoresLateArrival"]; }
            set { Session["GridContenedoresLateArrival"] = value; }
        }

       // string estadoIngresado = "EP";
        string estadoIngresado = "FI";
        
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
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
            this.IsAllowAccess();
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
                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
               // alerta.Visible = false;
                sinresultado.Visible = false;
                populateDrop(dptiposervicios, CslHelperServicios.getServicios());
                if (dptiposervicios.Items.Count > 0)
                {
                    if (dptiposervicios.Items.FindByValue("000") != null)
                    {
                        dptiposervicios.Items.FindByValue("000").Selected = true;
                    }
                    dptiposervicios.SelectedValue = "LA";
                    dptiposervicios.Enabled = false;
                }

                Session["identificacionAgencia"] = sUser.codigoempresa;
                txtReferencia.Text = "";
            }
            /*else {                
                txtReferencia.Text = Session["txtReferencia"].ToString();
            }*/
        }

        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }

        protected void btnSubirArchivoLateArrival_Click(object sender, EventArgs e)
        {
            this.tablePagination.DataSource = null;
            this.tablePagination.DataBind();
            xfinder.Visible = false;
            sinresultado.Visible = false;
            sinresultado.InnerHtml = string.Empty;
            var str = string.Empty;

            //obtengo la sesión del usuario logeado
            usuario sUser = null;
            sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            if (String.IsNullOrEmpty(hddFechaCutOff.Value.Trim()))
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-alerta";
                this.sinresultado.InnerText = string.Format("La referencia no tiene CUTOFF.");
                sinresultado.Visible = true;
                return;
            }

            string date = hddFechaCutOff.Value.Trim();
            DateTime dt = Convert.ToDateTime(date);
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            if (dt.CompareTo(DateTime.Now) >= 0) {


                if (fuContenedores.PostedFile.ContentLength <= 0)
                {
                    this.sinresultado.InnerText = "Seleccione el  archivo csv antes de proceder";
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.Visible = true;
                    return;
                }
                //subir el archivo validar q sea csv, si existe reemplazarlo
                var nombrefile = fuContenedores.PostedFile.FileName;
                if (System.IO.Path.GetExtension(nombrefile).ToUpper() != ".CSV")
                {
                    this.sinresultado.InnerText = "La extensión del archivo debe ser .CSV [Microsoft Excel/OpenOffice separado por comas]";
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.Visible = true;
                    return;
                }
                if (fuContenedores.PostedFile.ContentLength > 1500000)
                {
                    this.sinresultado.InnerText = "El tamaño del archivo excede el limite [2mb]";
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.Visible = true;
                    return;
                }


                try
                {
                    //leo toda la cadena como string.
                    str = new StreamReader(fuContenedores.PostedFile.InputStream).ReadToEnd();
                    str.Replace(",", ";");
                    str = Regex.Replace(str, @"\r\n?|\n", ";");
                    str = Regex.Replace(str, @"\t|\s", string.Empty);
                    str = Regex.Replace(str, ";;", ";");
                    str = Regex.Replace(str, @"'|!|<|>|-|_|""|#|", string.Empty);

                    //nuevo normalizo el string.
                    try
                    {
                        byte[] bytes = Encoding.Default.GetBytes(str);
                        str = Encoding.UTF8.GetString(bytes);
                    }
                    catch
                    {
                        str = Regex.Replace(str, Environment.NewLine, string.Empty);
                    }
                    //intento separado por saltos
                    str = Regex.Replace(str, @"'|!|<|>|-|_|""|#|", string.Empty);
                    List<string> getList = str.Split(';').ToList<String>();
                    if (getList.Count <= 1)
                    {
                        //intento separado por comas
                        getList = str.Split(',').ToList<String>();
                    }
                    if (getList.Count > 100)
                    {
                        this.sinresultado.InnerText = string.Format("La cantidad máxima de contenedores que puede asociar es [100] por transacción, el archivo presenta: {0}, por favor corríjalo", getList.Count);
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.Visible = true;
                        return;
                    }
                    //nuevo solo leer el CSV
                    Import_To_Grid(sUser, getList);
                   Grid = tablePagination.ID.ToString();
                }
                catch (Exception ex)
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "vacios", "btup_Click", str, "N4"));
                    sinresultado.Visible = true;
                    return;
                }

            }
            else {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Para la referencia "+txtReferencia.Text.Trim()+" solo puede subir archivos hasta el cutoff de la nave ("+date+").");
                sinresultado.Visible = true;
                return;
            }       
        }


        private void Import_To_Grid(usuario sUser, List<string> unidades)
        {
            try
            {

                DataTable dt = new DataTable();
                dt.TableName = "tablaContenedores";
                dt.Columns.Add("descripcionContenedor", typeof(string));
                //popular el datatable
                foreach (var c in unidades.Distinct())
                {
                    if (!string.IsNullOrEmpty(c))
                    {
                        var dr = dt.NewRow();
                        dr["descripcionContenedor"] = c.Trim();
                        dt.Rows.Add(dr);
                    }
                }
                List<contenedoresCerrojoElectronico> tabla = CslHelperServicios.consultarGrupoExcel(dt, "1");

                //Todo aqui verficar cada contenedor con grupo.

                //exportacion contenedores
                //  var ss = unidadN4.consultaUnidadesN4("EXPRT", "M", unidades);
                //me retorna la lista de contenedores 



                this.tablePagination.DataSource = tabla;
                this.tablePagination.DataBind();
                GridContenedoresLateArrival = tabla;
                xfinder.Visible = true;
                sinresultado.Visible = false;
                return;

            }
            catch (Exception ex)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la subida, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "late_Arrival", "Import_To_Grid_cg", "", sUser.loginname));
                sinresultado.Visible = true;
                return;
            }
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
                    }
                    string TipoServicio = dptiposervicios.SelectedValue;
                    string descripcionServicio = dptiposervicios.Items.FindByValue(dptiposervicios.SelectedValue).Text;
                    string descripcionGrupo = dptiposervicios.SelectedValue.Trim();

                    string mensajeCargaLate = "";
                    string evento = CslHelperServicios.consultaEventoPorServicio(dptiposervicios.SelectedValue);
                    //obtengo la sesión del usuario logeado
                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    List<datosCabecera> datosCab = new List<datosCabecera>();
                     var procesados = 0;
                    //Validación 1 -> valida cada contenedor contra N4
                    foreach (var item in GridContenedoresLateArrival)
                    {
                            string mensajeN4 = "";
                            string lblNombreContenedor = item.descripcion.Trim();
                            string lblObservacion = item.observacion.Trim();
                            if (string.IsNullOrEmpty(lblObservacion))
                            {
                                var tk = HttpContext.Current.Request.Cookies["token"];
                                ConectorN4.ObjectSesion sesObj = new ObjectSesion();
                                sesObj.clase = "solicitudUsuariosLateArrival";
                                sesObj.metodo = "btgenerar_Click";
                                sesObj.transaccion = "generarSolicitudLateArrival";
                                sesObj.usuario = sUser.loginname;
                                sesObj.token = tk.Value;
                                string a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"></unit-identity></units><properties><property tag=\"RoutingGroupRef\" value=\"{1}\"/></properties></icu>", lblNombreContenedor, descripcionGrupo);
                                mensajeN4 = Utility.validacionN4(sesObj, a);
                                if (!String.IsNullOrEmpty(mensajeN4))
                                {
                                    this.sinresultado.Attributes["class"] = string.Empty;
                                    this.sinresultado.Attributes["class"] = "msg-critico";
                                    this.sinresultado.InnerText = string.Format(mensajeN4 + " - Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente codigo [H00-{0}] ", csl_log.log_csl.save_log<Exception>(new Exception(mensajeN4), "solicitudUsuarios", "btGenerarSolicitud", lblNombreContenedor, sUser.loginname));
                                    sinresultado.Visible = true;
                                    return;
                                }
                                procesados++;
                            }
                    }
                   
                    if (procesados <= 0)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-alerta";
                        this.sinresultado.InnerText = "No hay contenedores que se puedan procesar";
                        sinresultado.Visible = true;
                        return;
                    }
                   

                    //Validación 2 -> Si no hubo problema guardo la cabecera de la solicitud de LateArrival                    
                    try
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime fechax;
                       

                        if (!DateTime.TryParseExact(hddFechaCutOff.Value.Replace("/", "-"), "yyyy-MM-dd HH:mm", enUS, DateTimeStyles.None, out fechax))
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = string.Format("Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente código [FE00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Error conversion de fecha"), "solicitud", "solicitud_cabecera", hddFechaCutOff.Value, sUser.loginname));
                            sinresultado.Visible = true;
                            return;
                        }
                        DateTime fechaCO = fechax;
                        DateTime fechaCOHasta = fechaCO;
                        datosCab = CslHelperServicios.cabeceraSolicitud(0, TipoServicio, null, null, txtReferencia.Text.Trim(), null, sUser.loginname, estadoIngresado, sUser.grupo.ToString(), hddReferencia.Value.Trim(), fechaCO.ToString("yyyy-MM-dd HH:mm"), fechaCOHasta.AddHours(4).ToString("yyyy-MM-dd HH:mm"));
                        if (datosCab == null)
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = string.Format("Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente código [J00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Error cabecera nula"), "solicitud", "solicitud_cabecera",txtReferencia.Text, sUser.loginname));
                            sinresultado.Visible = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente código [F00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "solicitud", "solicitud_cabecera", datosCab[0].idSolicitud.Trim(), sUser.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    
                    foreach (var item in GridContenedoresLateArrival)
                    {
                       
                            string lblNombreContenedor = item.descripcion.Trim();
                            string lblObservacion = item.observacion.Trim();
                            string lblIDContenedor = item.idCodigoContenedor.Trim();
                            //Validación 3 --> Se guarda los detalles con el id de la solicitud generada.
                            if (string.IsNullOrEmpty(lblObservacion))
                            {
                                int retornoD = CslHelperServicios.detalleSolicitud(int.Parse(datosCab[0].idSolicitud), int.Parse(lblIDContenedor), sUser.loginname);
                                mensajeCargaLate = cargarLateArrival(lblNombreContenedor, sUser);

                                if (!String.IsNullOrEmpty(mensajeCargaLate))
                                {
                                    this.sinresultado.Attributes["class"] = string.Empty;
                                    this.sinresultado.Attributes["class"] = "msg-critico";
                                    this.sinresultado.InnerText = string.Format(mensajeCargaLate + " - Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente codigo [N00-{0}] ", csl_log.log_csl.save_log<Exception>(new Exception(mensajeCargaLate), "cargarLateArrival", "btGenerarSolicitud", lblNombreContenedor, sUser.loginname));
                                    sinresultado.Visible = true;
                                    return;
                                }
                            }

                    }
                    //Validación 4 --> Se envía un correo al usuario. (Se guarda en las tablas)
                    if (datosCab != null && datosCab.Count > 0)
                    {
                        var sold = datosCab[0].idSolicitud;
                        var solnum = datosCab[0].codigoSolicitud;
                        int sol = 0;
                        if (int.TryParse(sold, out sol))
                        {
                            envioCorreo(sUser, sol, descripcionServicio, solnum);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la generación de la solicitud, por favor repórtelo con este código P00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btgenerar_Click", "Hubo un error al procesar los datos", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
        public void envioCorreo(usuario sUser, int idSolicitud, string descripcionServicio, string codigoSolicitud)
        {
            string descripcionEstado = "";            
            string nombreUsuario = "";
            string correoUsuario = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string referencia = "";

            string nobok = string.Empty;
            string nocarga = string.Empty;

            List<consultaCabeceraUsuario> solicitudUsuario = CslHelperServicios.consultaSolicitudUsuario(null, 0, null, null, null, null, false, null, idSolicitud.ToString());
            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.ToString();
                fechaPropuesta = datoSolicitud.fechaPropuesta;
                comentarios = datoSolicitud.comentarios;
                referencia = datoSolicitud.referencia;
                nobok = datoSolicitud.noBooking;
                nocarga = datoSolicitud.noCarga;
                
            }

            var jmsg = new jMessage();
            string mail = string.Empty;
            string destinatarios = turnoConsolidacion.GetMails();
            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null,"0",0,"1");
            mail = Utility.late_arriva_msg(listaDetalle.Select(u => u.descripcionContenedor).ToList(), trafico, descripcionEstado, nobok, nocarga);
            string error = string.Empty;
            //el mail del usuario logueado
           
            
            var user_email = "CGSA-VslPlanners@cgsa.com.ec; ExpoNavios@cgsa.com.ec; " + correoUsuario;
            destinatarios = user_email;


            var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
            destinatarios = string.Format("{0};{1}", correoBackUp, destinatarios);

            CLSDataCentroSolicitud.addMail(out error, sUser.email, codigoSolicitud + " - Solicitud de Late Arrival.", mail, destinatarios, sUser.loginname, "", "");
            if (!string.IsNullOrEmpty(error))
            {
                alerta.Visible = true;
                alerta.InnerText = error;
                return;
            }
            else
            {
                imagenSolicitud.InnerHtml = "";
                Utility.mostrarMensajeRedireccionando(this.Page, "Su solicitud " + codigoSolicitud + " ha sido generada para el servicio de Late Arrival, revise su correo en unos minutos para mayor información.", "../csl/menudefault");
            }
        }
        public string cargarLateArrival(string descripcionContenedor, usuario sUser)
        {
            string mensajeN4 = "";
            string errorN4 = "";
            DateTime fechaCO = DateTime.Parse(hddFechaCutOff.Value.Trim());
            DateTime fechaCON = fechaCO.AddHours(double.Parse(ConfigurationManager.AppSettings["numeroHorasMaxima"]));
            var tk = HttpContext.Current.Request.Cookies["token"];
            ConectorN4.ObjectSesion sesObj = new ObjectSesion();
            sesObj.clase = " marcaLateArrival";
            sesObj.metodo = "cargarLateArrival";
            sesObj.transaccion = "marcaLateArrival";
            sesObj.usuario = sUser.loginname;
            sesObj.token = tk.Value;
            String a = string.Format("<groovy class-location=\"code-extension\" class-name=\"CGSALateArrivalWS\"><parameters><parameter id=\"UNIT\" value=\"{0}\"/><parameter id=\"VESSELVISIT\" value=\"{1}\"/><parameter id=\"CUTOFF\" value=\"{2}\"/></parameters></groovy>", descripcionContenedor, hddReferencia.Value.Trim(), fechaCON.ToString("yyyy-MM-dd HH:mm:ss"));
            mensajeN4 = Utility.validacionN4(sesObj, a);
            if ( !string.IsNullOrEmpty(mensajeN4) &&  !mensajeN4.Contains("exitosa"))
            {
                errorN4 = mensajeN4;
            }
            return errorN4;
        }

    }
}