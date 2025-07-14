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

namespace CSLSite
{
    public partial class cons_req_serv : System.Web.UI.Page
    {
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
                    if (string.IsNullOrEmpty(this.contain.Text))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;Ingrese al menos un parametro de busqueda.{0}</strong>", this.contain.Text);
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = true;
                        return;
                    }
                    //ejecutar ambos query

                    tablePagination.DataSource = null;
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los filtros para el reporte.";
                    var tablix = repNavieras.GetReqService(this.contain.Text);

                    tablix.Columns.Add("QREQUEST");

                    foreach (System.Data.DataRow x in tablix.Rows)
                    {
                        x["QREQUEST"] = securetext(x["SEC"] + ";" + x["REQUEST"]);
                    }

                    tablePagination.DataSource = tablix;
                    tablePagination.DataBind();

                    Session["reportServ"] = tablix;
                    xfinder.Visible = true;

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

        public static string securetext(string req)
        {
            if (req == null || req.Length <= 2)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(req);
        }

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