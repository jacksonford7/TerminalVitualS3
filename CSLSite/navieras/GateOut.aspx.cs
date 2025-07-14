using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CSLSite.N4Object;
using ConectorN4;
using System.Globalization;
using System.Web.Script.Services;
using System.Configuration;

namespace CSLSite
{
    public partial class gate_out : System.Web.UI.Page
    {

        //AntiXRCFG.
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

            if (user != null /*&& !string.IsNullOrEmpty(user.nombregrupo)*/)
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
                this.agencia.Value = user.ruc;
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
               sinresultado.Visible = false;
            }
           
        }
        protected void btbuscar_Click(object sender, EventArgs e)
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
                    sinresultado.Visible = false;
                    //alerta.Visible = false;
                    //xfinder.Visible = true;
                    if (string.IsNullOrEmpty(this.tbooking.Text) &&
                        string.IsNullOrEmpty(this.contain.Text) &&
                        string.IsNullOrEmpty(this.tfechaini.Text))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;Ingrese nave, contenedor o fecha para la busqueda.{0}</strong>", this.tbooking.Text);
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = true;
                        return;
                    }
                    /*
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime fechaini;
                    DateTime fechafin;
                    if (!string.IsNullOrEmpty(tbooking.Text))
                    {
                        this.tfechaini.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        this.tfechafin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    if (!string.IsNullOrEmpty(tfechaini.Text) && !string.IsNullOrEmpty(tfechafin.Text))
                    {
                        
                    }
                    if (!DateTime.TryParseExact(this.tfechaini.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaini))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;El formato de la Fecha Desde, debe ser dia/Mes/Anio {0}</strong>", this.tfechaini.Text);
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = true;
                        return;
                    }
                    if (!DateTime.TryParseExact(this.tfechafin.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechafin))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;El formato de la Fecha Hasta, debe ser dia/Mes/Anio {0}</strong>", this.tfechafin.Text);
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = true;
                        return;
                    }*/
                    //ejecutar ambos query

                    tablePagination.DataSource = null;
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los filtros para el reporte.";
                    var tablix = repNavieras.GetRptGateOut(this.agencia.Value, this.tfechaini.Text, this.tfechafin.Text, this.tbooking.Text, this.contain.Text);
                    tablePagination.DataSource = tablix;
                    tablePagination.DataBind();
                    Session["reportGateOut"] = tablix;
                    xfinder.Visible = true;
                    //Session["DetalleReserva"] = chkDetalle.Checked;
                    if (string.IsNullOrEmpty(tbooking.Text))
                    {
                        tbooking.Text = "0";
                    }
                    var sid = QuerySegura.EncryptQueryString(string.Format("{0};{1};{2};{3};{4}", this.agencia.Value, this.tfechaini.Text, this.tfechafin.Text, tbooking.Text, contain.Text));
                    this.aprint.HRef = string.Format("RptGateOut.aspx?sid={0}", sid);
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)]
        public static string ValidateJSON(turnoConsolidacion objeto)
        {
            try
            {
                if (!HttpContext.Current.Request.IsAuthenticated)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "turnos", "ValidateJSON", "No autenticado", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Actualmente no se encuentra autenticado, sera redireccionado a la pagina de login");
                }
                //validar que la sesión este activa y viva
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "turnos", "ValidateJSON", "Sesión no existe", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Su sesión ha expirado, sera redireccionado a la pagina de login");
                }
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                //Validacion 3 -> Si su token existe, es válido, y no ha expirado
                if (token == null || !csl_log.log_csl.validateToken(token.Value))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "turnos", "ValidateJSON", "Sin Valor Token", "No tokenID");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../cuenta/menu.aspx'", "Su formulario ha expirado, por favor reingrese de nuevo desde el menú");
                }

                var jmsg = new jMessage();
                var cantmaxbkg = turnoConsolidacion.GetCantMaxBkg(objeto.linea, objeto.booking);
                var sumcantreserva = turnoConsolidacion.GetSumCantReserva(objeto.booking);
                int validacantidad = 0;
                if (objeto.detalles.Count > 0)
                {
                    foreach (var v in objeto.detalles)
                    {
                        if (!string.IsNullOrEmpty(v.reserva))
                        {
                            if (v.reserva != "0")
                            {
                                validacantidad = validacantidad + Convert.ToInt32(v.reserva);
                            }
                        }
                    }
                }
                if (sumcantreserva == cantmaxbkg)
                {
                    jmsg.resultado = false;
                    string msgerror = "Booking no dispone de contenedores.";

                    jmsg.mensaje = msgerror;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                sumcantreserva = sumcantreserva + validacantidad;
                if (sumcantreserva > cantmaxbkg)
                {
                    jmsg.resultado = false;
                    string msgerror = "La cantidad a reservar supera el maximo del Booking." + System.Environment.NewLine +
                                      "*Cantidad total del Booking: " + cantmaxbkg.ToString() + System.Environment.NewLine +
                                      "*Minimo a reservar: " + (cantmaxbkg - turnoConsolidacion.GetSumCantReserva(objeto.booking)).ToString();

                    jmsg.mensaje = msgerror;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                //Validacion 2 -> Obtener la sesión y covetirla a objectsesion
                var user = new ObjectSesion();

                var exportador = turnoConsolidacion.GetExportador(sUser.id);
                if (exportador == "0" || string.IsNullOrEmpty(exportador))
                {
                    exportador = objeto.linea;
                }
                var nombre_empresa = turnoConsolidacion.GetNombreEmpresa(objeto.booking);
                if (nombre_empresa == "0" || string.IsNullOrEmpty(nombre_empresa))
                {
                    nombre_empresa = objeto.linea;
                }

                user.clase = "turnos"; user.metodo = "ValidateJSON";
                user.transaccion = "asignar_turno"; user.usuario = sUser.loginname;
                string mensaje = string.Empty;
                jmsg.data = string.Empty;
                jmsg.fluir = false;
                //aqui usuario;
                objeto.usuario = sUser.loginname; ;
                //depurar los valores
                DataTransformHelper.CleanProperties<turnoConsolidacion>(objeto);
                //revalidar la información
                jmsg.resultado = turnoConsolidacion.validar(objeto, out mensaje);
                if (!jmsg.resultado)
                {
                    jmsg.mensaje = mensaje;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
                //guardar--->
                if (!objeto.add(out mensaje))
                {
                    jmsg.resultado = false;
                    jmsg.mensaje = mensaje;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                string mail = string.Empty;
                string destinatarios = turnoConsolidacion.GetMails();
                mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/>Este es un mensaje del Centro de Servicios en Linea de Contecon S.A, para comunicarle lo siguiente.<br/>A continuacion el detalle de su reserva:<br/>", sUser.nombres, sUser.apellidos));
                mail = string.Concat(mail, string.Format("<strong>Línea: </strong>{0}<br/><strong>Exportador: </strong>{1}<br/><strong>Booking: </strong>{2}<br/><strong>Fecha Consolidación: </strong>{3}<br/>", exportador, nombre_empresa, objeto.booking, objeto.fecha_pro));
                if (objeto.detalles.Count > 0)
                {
                    mail = string.Concat(mail, "<table rules='all' border='10'><tr><th align='center'>Desde</th><th align='center'>Hasta</th><th align='center'>Reservado</th></tr>");
                    foreach (var l in objeto.detalles)
                    {
                        if (!string.IsNullOrEmpty(l.reserva))
                        {
                            if (l.reserva != "0")
                            {
                                validacantidad = validacantidad + Convert.ToInt32(l.reserva);
                                mail = string.Concat(mail, string.Format("<tr><td align='center'>{0}</td><td align='center'>{1}</td><td align='center'>{2}</td></tr>", l.desde, l.hasta, l.reserva));   
                            }
                        }
                    }
                    mail = string.Concat(mail, "</table>");
                }
                else
                {
                    mail = string.Concat(mail, "Hubo un problema y no se encontraron detalles que mostrar, comuniquese con CGSA");
                }
                //var car = new CSLSite.unitService.mailserviceSoapClient();
                //car.sendmail(mail, string.Format("{0};" + destinatarios, objeto.mail), objeto.usuario, token.Value);
                string error = string.Empty;
                //el mail del usuario logueado
                var user_email = sUser.email;
               // destinatarios = user_email + ";" + objeto.mail + ";" + destinatarios;

                var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
                destinatarios = string.Format("{0};{1};{2};{3};CGSA-Consolidaciones@cgsa.com.ec", user_email, objeto.mail, destinatarios, correoBackUp);

                turnoConsolidacion.addMail(out error, destinatarios, "Se genero una reserva de turnos para consolidación, *Booking " + objeto.booking, mail, objeto.mail, objeto.usuario, objeto.idlinea, objeto.linea);
                if (!string.IsNullOrEmpty(error))
                {
                    jmsg.resultado = false;
                    jmsg.mensaje = error;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                return CslHelper.JsonNewResponse(true, true, "window.location='../cuenta/menu.aspx'", "Proceso exitoso en unos minutos recibirá una notificación via mail.");
            }
            catch (Exception ex)
            {
                var t = string.Format("Estimado usuario\\nOcurrió un problema durante su solicitud\\n{0}\\nPor favor intente lo siguiente salir y volver a entrar al sistema\\nSi esto no funciona envienos un correo con el mensaje de error y lo atenderemos de inmediato.\\nMuchas gracias", ex.Message.Replace(":", "").Replace("'", "").Replace("/", ""));
                return "{ \"resultado\":false, \"fluir\":false, \"mensaje\":\"" + t + "\" }";
            }
        }
    }
}