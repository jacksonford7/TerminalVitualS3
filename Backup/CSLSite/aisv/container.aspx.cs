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


namespace CSLSite
{
    public partial class container : System.Web.UI.Page
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
            var user =  Page.Tracker();
            if (user != null)
            {
                this.textbox1.Value = user.email != null ? user.email : string.Empty;
                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
            }
            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa);
                this.nomexpo.InnerText =  t!=null? t:string.Format("{0} {1}",user.nombres , user.apellidos);
                this.numexpo.InnerText = string.IsNullOrEmpty( user.codigoempresa) ? user.ruc :user.codigoempresa;
                this.numexport.Value = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
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
                    populateDrop(dprovincia, CslHelper.getProvincias());
                    if (dprovincia.Items.Count > 0)
                    {
                        if (dprovincia.Items.FindByValue("00") != null)
                        {
                            dprovincia.Items.FindByValue("00").Selected = true;
                        }
                    }
                
                     populateDrop(dpins, CslHelper.getInstitucion());
                    if (dpins.Items.Count > 0)
                    {
                        if (dpins.Items.FindByValue("0") != null)
                        {
                            dpins.Items.FindByValue("0").Selected = true;
                        }
                    }

                    populateDrop(dprefrigera, CslHelper.getRefrigeracion());
                    if (dprefrigera.Items.Count > 0)
                    {
                        if (dprefrigera.Items.FindByValue("0") != null)
                        {
                            dprefrigera.Items.FindByValue("0").Selected = true;
                        }
                    }
                    populateDrop(dpventila, CslHelper.getVentilacion());
                    if (dpventila.Items.Count > 0)
                    {
                        if (dpventila.Items.FindByValue("0") != null)
                        {
                            dpventila.Items.FindByValue("0").Selected = true;
                        }
                    }
                    populateDrop(dphumedad, CslHelper.getHumedad());
                    if (dphumedad.Items.Count > 0)
                    {
                        if (dphumedad.Items.FindByValue("0") != null)
                        {
                            dphumedad.Items.FindByValue("0").Selected = true;
                        }
                    }
                    //getDepositos
                    populateDrop(dporigen, CslHelper.getDepositos());
                    if (dporigen.Items.Count > 0)
                    {
                        if (dporigen.Items.FindByValue("0") != null)
                        {
                            dporigen.Items.FindByValue("0").Selected = true;
                        }
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
        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)] 
        public static string ValidateJSON(jAisvContainer objeto)
        {
             objeto.hasprof = "Y";
            try
            {
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
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                //Validacion 3 -> Si su token existe, es válido, y no ha expirado
                if (token == null || !csl_log.log_csl.validateToken(token.Value))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "container", "ValidateJSON", "No token", "Sin tokenID");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../csl/menu'", "Su formulario ha expirado, por favor reingrese de nuevo desde el menú");
                }
                var jmsg = new jMessage();
                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                //Validacion 2 -> Obtener la sesión y covetirla a objectsesion
                var user = new ObjectSesion();
                user.clase = "container"; user.metodo = "ValidateJSON";
                user.transaccion = "AISV Container"; user.usuario = sUser.loginname;
                token = HttpContext.Current.Request.Cookies["token"];

               
                

                //Validacion 4 --  limpiar todo el objeto
                DataTransformHelper.CleanProperties<jAisvContainer>(objeto);
                //preparo el mensaje que será enviado al explorador
                var mensaje = string.Empty;
                jmsg.data = string.Empty;
                jmsg.fluir = false;

                //seteo el token
                user.token = token.Value;
                objeto.autor = sUser.loginname; 
                //nuevo uso el login que esta en la session!!
                objeto.idexport = sUser.ruc;

                //objeto.nomexpo = 

                //Validación 5 -> Que todas las reglas básicas de negocio se cumplan
                jmsg.resultado = jAisvContainer.ValidateAisvData(objeto, out mensaje);
                if (!jmsg.resultado)
                {
                    jmsg.mensaje = mensaje;
                    if (objeto.hasprof == "N")
                    {
                        jmsg.fluir = true;
                        jmsg.data = "window.open('../servicios/proforma','Proformas','width=1000,height=800,scrollbars=yes');";
                        jmsg.mensaje = string.Format( "{0}\n Va a ser direccionado a la página de proformas",jmsg.mensaje);
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
                //  System.Diagnostics.Trace.Write(string.Format("----->Fin de Validaciones:{0}",DateTime.Now));
                //transporte a N4



               objeto.nomexpo =   CslHelper.getShiperName(objeto.idexport);

                if (!objeto.TransaportToN4(user, out mensaje))
                {
                    jmsg.mensaje = mensaje;
                    jmsg.resultado = false;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
                //  System.Diagnostics.Trace.Write(string.Format("----->Fin de transporte:{0}", DateTime.Now));
                //última capa es el insert a la tabla
                var aisv = string.Empty;
                
                if (!objeto.add(out mensaje))
                {
                    jmsg.mensaje = mensaje;
                    jmsg.resultado = false;
                    //nuevo si fallo la inserción cancele el advice.
                    try
                    {
                        ObjectSesion oby = new ObjectSesion();
                        oby.clase = "container";
                        oby.metodo = "jaisv.Add";
                        oby.transaccion = "Falló add";
                        oby.usuario = "sistema";
                        string mes = string.Empty;
                        jAisvContainer.cancelAdvice(oby, objeto.unumber, objeto.breferencia, out mes);
                    }
                    catch (Exception ex)
                    {
                        csl_log.log_csl.save_log<Exception>(ex, "container", "cancelAdvice", objeto.secuencia, "sistema");
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
                //confirmación final a N4
                aisv = objeto.secuencia;
                // System.Diagnostics.Trace.Write(string.Format("----->Fin de Insert:{0}", DateTime.Now));
                //paso toda la transacción, ahora se debe encriptar
                jmsg.data = aisv;
                jmsg.mensaje = QuerySegura.EncryptQueryString(aisv);
                //este ya retorna el valor con el mensaje->Validacion,Insert,Exception
                return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
            }
            catch (Exception ex)
            {
                var t = string.Format("Estimado usuario\\nOcurrió un problema durante su solicitud\\n{0}\\nPor favor intente lo siguiente salir y volver a entrar al sistema\\nSi esto no funciona envienos un correo con el mensaje de error y lo atenderemos de inmediato.\\nMuchas gracias", ex.Message.Replace(":", "").Replace("'", "").Replace("/", ""));
                return "{ \"resultado\":false, \"fluir\":false, \"mensaje\":\"" + t + "\" }";
            }
        }
        //metodo generalizado para controlar el error de esta clase.
        public static string ControlError(string mensaje)
        { 
          //paselo a la pantalla una vez controlado y guardado.
           return " mensaje:" + mensaje + ", resultado:false ";
        }
   }
}