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
    public partial class turnos : System.Web.UI.Page
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

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
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
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime fecha;

            this.xfecha.Value = xfecha.Value;

            if (!DateTime.TryParseExact(this.xfecha.Value, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                tablePagination.DataSource = null;
                tablePagination.DataBind();
                this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", this.xfecha.Value);
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = true;
                return;
            }
            //ejecutar ambos query

            /*
            if (fecha <= DateTime.Now.Date)
            {
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                tablePagination.DataSource = null;
                tablePagination.DataBind();
                this.sinresultado.InnerHtml = "Toda reserva debe de realizarse con 48 horas de anticipaciòn a la fecha de consulta.";
                sinresultado.Visible = true;
                btnera.Visible = false;
                alerta.Visible = false;
                xfinder.Visible = true;
                return;
            }
            */

            tablePagination.DataSource = null;

            var t = turno.GetLimite(fecha, this.agencia.Value, nbrboo.Value.ToString());
            var  menerr = string.Empty;
             if (t != null)
             {
                 btnera.Visible = true;
                 btnera.Visible = true;
                 alerta.Visible = true;
                 var g = t.Item1;
                 sinresultado.Visible = false;
                 if (t.Item2 > 0)
                 {
                     this.alerta.InnerHtml = string.Format("CAPACIDAD DISPONIBLE: <strong>&nbsp;LINEA {0} |{1} UNIDADES </strong>", this.agencia.Value, t.Item2.ToString("##"));
                     var tablix = turno.GetHorarios(fecha, out menerr);
                     if (!string.IsNullOrEmpty(menerr))
                     {
                         tablePagination.DataSource = null;
                         tablePagination.DataBind();
                         sinresultado.Attributes["class"] = string.Empty;
                         sinresultado.Attributes["class"] = "msg-critico";
                         this.sinresultado.InnerText = menerr;
                         sinresultado.Visible = true;
                         btnera.Visible = false;
                         alerta.Visible = false;
                         this.sinresultado.InnerHtml = menerr;
                         return;
                     }
                     tablePagination.DataSource = tablix;
                     tablePagination.DataBind();
                 }
                 else
                 {
                     sinresultado.Attributes["class"] = string.Empty;
                     sinresultado.Attributes["class"] = "msg-critico";
                     this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;LA LÍNEA {0} | NO CUENTA CON DISPONIBILIDAD PARA EL {1}</strong>", this.agencia.Value, this.xfecha.Value);
                     sinresultado.Visible = true;
                     btnera.Visible = false;
                     alerta.Visible = false;
                 }
                 this.idlin.Value = t.Item1.ToString();
                 this.diponible.Value = t.Item2.ToString();
                 diponible.Value = t.Item2.ToString();
                 //cantbkg.Value = t.Item3.ToString();
                 xfinder.Visible = true;
             }
             else
             {
                 sinresultado.Attributes["class"] = string.Empty;
                 sinresultado.Attributes["class"] = "msg-critico";
                 tablePagination.DataSource = null;
                 tablePagination.DataBind();
                 this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;NO EXISTE UNA PROGRAMACIÓN PARA LA FECHA {0}</strong>", this.xfecha.Value);
                 sinresultado.Visible = true;
                 btnera.Visible = false;
                 alerta.Visible = false;
                 xfinder.Visible = true;
             }
        }
        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)]
        public static string ValidateJSON(turno objeto)
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
                var cantmaxbkg = turno.GetCantMaxBkg(objeto.linea, objeto.booking);
                var sumcantreserva = turno.GetSumCantReserva(objeto.booking);
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
                                      "*Minimo a reservar: " + (cantmaxbkg - turno.GetSumCantReserva(objeto.booking)).ToString();

                    jmsg.mensaje = msgerror;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                //Validacion 2 -> Obtener la sesión y covetirla a objectsesion
                var user = new ObjectSesion();

                var exportador = turno.GetExportador(sUser.id);
                if (exportador == "0" || string.IsNullOrEmpty(exportador))
                {
                    exportador = objeto.linea;
                }
                var nombre_empresa = turno.GetNombreEmpresa(objeto.booking);
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
                DataTransformHelper.CleanProperties<turno>(objeto);
                //revalidar la información
                jmsg.resultado = turno.validar(objeto, out mensaje);
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
                string destinatarios = turno.GetMails();
                mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/>Este es un mensaje del Sistema de Terminal Virtual de Contecon S.A, para comunicarle lo siguiente.<br/>A continuación el detalle de su reserva:<br/>", sUser.nombres, sUser.apellidos));
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
                destinatarios = string.Format("{0};{1};CGSA-Consolidaciones@cgsa.com.ec", objeto.mail, correoBackUp);


                turno.addMail(out error, user_email, "Se generó una reserva de turnos para consolidación, *Booking " + objeto.booking, mail, destinatarios, objeto.usuario, objeto.idlinea, objeto.linea);
                if (!string.IsNullOrEmpty(error))
                {
                    jmsg.resultado = false;
                    jmsg.mensaje = error;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                return CslHelper.JsonNewResponse(true, true, "window.location='../cuenta/menu.aspx'", "Proceso exitoso en unos minutos recibirá el mail");
            }
            catch (Exception ex)
            {
                var t = string.Format("Estimado usuario\\nOcurrió un problema durante su solicitud\\n{0}\\nPor favor intente lo siguiente salir y volver a entrar al sistema\\nSi esto no funciona envienos un correo con el mensaje de error y lo atenderemos de inmediato.\\nMuchas gracias", ex.Message.Replace(":", "").Replace("'", "").Replace("/", ""));
                return "{ \"resultado\":false, \"fluir\":false, \"mensaje\":\"" + t + "\" }";
            }
        }
    }
}