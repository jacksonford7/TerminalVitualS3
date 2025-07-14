using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.Web.Script.Services;
using CSLSite.N4Object;
using CSLSite.XmlTool;
using ConectorN4;
using Newtonsoft.Json;
using csl_log;
using CSLSite.app_start;


namespace CSLSite
{
    public partial class solicitud : System.Web.UI.Page
    {
        //AntiXRCFG
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
            if (user != null)
            {
                this.textbox1.Value = user.email != null ? user.email : string.Empty;
                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
                var t = CslHelper.getShiperName(user.ruc);
                this.nomline.InnerText = t;
                this.agencia.Value = t;
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
                populateDrop(dpservicio, atraqueHelper.get_services());
                if (dpservicio.Items.Count > 0)
                {
                    if (dpservicio.Items.FindByValue("55") != null)
                    {
                        dpservicio.Items.FindByValue("55").Selected = true;
                    }
                }

            }
        }
        private void populateDrop(DropDownList dp, List<item> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "codigo";
            dp.DataTextField = "valor";
            dp.DataBind();
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)] 
        public static string ValidateJSON(jSolicitud objeto)
        {
            try
            {
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];

                if (!HttpContext.Current.Request.IsAuthenticated)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "ValidateJSON", "No autenticado", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../csl/login'", "Actualmente no se encuentra autenticado, sera redireccionado a la pagina de login");
                }
                //validar que la sesión este activa y viva
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "container", "ValidateJSON", "Sesión no existe", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../csl/login'", "Su sesión ha expirado, sera redireccionado a la página de login");
                }
               
                //Validacion 3 -> Si su token existe, es válido, y no ha expirado
                if (token == null || !csl_log.log_csl.validateToken(token.Value))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "container", "ValidateJSON", "No token", "Sin tokenID");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../csl/menu'", "Su formulario ha expirado, por favor reingrese de nuevo desde el menú");
                }

                var jmsg = new jMessage();
                usuario sUser = null;
                var mensaje = string.Empty;
                jmsg.data = string.Empty;
                jmsg.fluir = false;
                //todo ver si esta autenticada
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                //Validacion 2 -> Obtener la sesión y covetirla a objectsesion
                var user = new ObjectSesion();
                user.clase = "solicitud"; 
                user.metodo = "ValidateJSON";
                user.transaccion = "Solicitud Atraque"; 
                user.usuario = sUser.loginname;
                token = HttpContext.Current.Request.Cookies["token"];
                user.token = token.Value;
                objeto.userid = sUser.id.ToString();
               //Validacion 4 --  limpiar todo el objeto
                DataTransformHelper.CleanProperties<jSolicitud>(objeto);
                objeto.autor = sUser.loginname;
                objeto.uline = sUser.ruc;
                if (string.IsNullOrEmpty(objeto.mail1))
                {
                    objeto.mail1 = sUser.email;
                }
                
                //Validación 5 -> Que todas las reglas básicas de negocio se cumplan
                  jmsg.resultado = jSolicitud.ValidateSolicitudData(objeto, out mensaje);
                  if (!jmsg.resultado)
                  {
                      jmsg.mensaje = mensaje;
                      return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                  }

                  if (!objeto.add(out mensaje))
                  {
                      jmsg.mensaje = mensaje;
                      jmsg.resultado = false;
                      return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                  }
                  //le pongo el id de solicitud
                  user.transaccion = objeto.id;

                  //transporte a N4
                  if (!objeto.TransaportToN4(user, out mensaje))
                  {
                      jmsg.mensaje = mensaje;
                      jmsg.resultado = false;
                      return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                  }

                  //paso toda la transacción, ahora se debe encriptar
                  jmsg.data = objeto.id;
                  jmsg.mensaje = QuerySegura.EncryptQueryString(objeto.id);
                  //este ya retorna el valor con el mensaje->Validacion,Insert,Exception

                  try
                  {
                      //aca debo enviar el mail.... SI FALLA NO IMPORTA
                      var sbs = new System.Text.StringBuilder();
                      sbs.AppendFormat("<div><span>Estimado/a: {0} {1}<span><br/>", sUser.nombres, sUser.apellidos);
                      sbs.AppendFormat("<span>El Sistema de Solicitud de Servicios de Contecon Guayaquil, le informa que se acaba de registrar exitosamente la solicitud de atraque con referencia {0}.</span><br/><hr/>", objeto.id);
                      sbs.Append("<span><strong>Información sobre la solicitud:</strong></span><br/>");
                      sbs.AppendFormat("<span><strong>Nombre de la Agencia Naviera:</strong> {0}</span><br/>", objeto.agencia);
                      sbs.AppendFormat("<span><strong>Nombre de la Nave:</strong> {0}</span><br/>", objeto.nombre);
                      sbs.AppendFormat("<span><strong>IMO de la Nave:</strong> {0}</span><br/>", objeto.imo);
                      sbs.AppendFormat("<span><strong>Ruta de servicio:</strong> {0}</span><br/>", objeto.nservicio);
                      sbs.AppendFormat("<span><strong>Viaje Entrante:</strong> {0}</span><br/>", objeto.vIn);
                      sbs.AppendFormat("<span><strong>Viaje Saliente:</strong> {0}</span><br/>", objeto.vOut);
                      sbs.AppendFormat("<span><strong>Manifiesto de Importación:</strong> {0}</span><br/>", objeto.imrn);
                      sbs.AppendFormat("<span><strong>Manifiesto de Exportación:</strong> {0}</span><br/>", objeto.emrn);
                      sbs.Append("<p><strong>Fechas de operación estimadas:</strong><p>");
                      sbs.AppendFormat("<span><strong>Fecha estimada de arribo a boya Data/Posorja:</strong> {0}</span><br/>", objeto.eta);
                      sbs.AppendFormat("<span><strong>Fecha estimada de atraque en muelle CGSA:</strong> {0}</span><br/>", objeto.etb);
                      sbs.AppendFormat("<span><strong>Fecha estimada de zarpe:</strong> {0}</span><br/>", objeto.ets);
                      sbs.AppendFormat("<span><strong>Horas uso de muelle:</strong> {0}</span><br/>", objeto.uso);
                      if (objeto.lines != null && objeto.lines.Count > 0)
                      {
                          sbs.Append("<p><strong>Lineas asociadas al servicio y solicitud:</strong><p>");
                          sbs.Append("<table border='1' cellpadding='1' cellspacing='1' >");
                          sbs.Append("<tr><th>Línea</th><th>Viaje entrante</th><th>Viaje saliente</th></tr>");
                          foreach (var l in objeto.lines)
                          {
                              sbs.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", l.lnombre, l.viajeIn, l.viajeOut);
                          }
                          sbs.Append("</table>");
                      }
                      sbs.AppendFormat("<span><strong>Fecha de creación:</strong> {0}</span><br/>", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                      sbs.Append("</div>");
                      var mails = objeto.mail1;
                      mails = string.Concat(mails, ";", objeto.mail2);
                      mails = string.Concat(mails, ";", objeto.mail3);
                      mails = string.Concat(mails, ";", objeto.mail4);
                      mails = string.Concat(mails, ";", objeto.mail5);
                      atraqueHelper.insertarAviso(mails, null, string.Format("Registro de Solicitud de Atraque {0}", objeto.id), sbs.ToString(), sUser.loginname, 0, 1, "CSL_SOL");

                  }
                  finally 
                  {
                      
                      
                  }

                 //FINAL
                  return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
            }
            catch (Exception ex)
            {
                var t = string.Format("Estimado usuario\\nOcurrió un problema durante su solicitud\\n{0}\\nPor favor intente lo siguiente salir y volver a entrar al sistema\\nSi esto no funciona envienos un correo con el mensaje de error y lo atenderemos de inmediato.\\nMuchas gracias", ex.Message.Replace(":", "").Replace("'", "").Replace("/", ""));
                return "{ \"resultado\":false, \"fluir\":false, \"mensaje\":\"" + t + "\" }";
            }
        }
        ////metodo generalizado para controlar el error de esta clase.
        public static string ControlError(string mensaje)
        {
            //paselo a la pantalla una vez controlado y guardado.
            return " mensaje:" + mensaje + ", resultado:false ";
        }
   }
}