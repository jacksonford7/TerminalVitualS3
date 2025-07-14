using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.Web.Script.Services;
using ConectorN4;


namespace CSLSite
{
    public partial class cargaconsolidar : System.Web.UI.Page
    {
        private string _Id_Opcion_Servicio = string.Empty;

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
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "cargaconsolidar", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();
            if (user != null)
            {
                this.textbox1.Value = user.email != null ? user.email : string.Empty;
                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
            }
            //if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            if (user != null )
            {
                var sp = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
                string t = null;
                if (!string.IsNullOrEmpty(sp))
                {
                    t = CslHelper.getShiperName(sp);
                }
                this.nomexpo.InnerText = t != null ? t : string.Format("{0} {1}", user.nombres, user.apellidos);
                this.numexpo.InnerText = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
                this.numexport.Value = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
                //para que puedar dar click en el titulo de la pantalla y regresar al menu principal de la zona
                //_Id_Opcion_Servicio = Request.QueryString["opcion"];
                //this.opcion_principal.InnerHtml = string.Format("<a href=\"../cuenta/subopciones.aspx?opcion={0}\">{1}</a>", _Id_Opcion_Servicio, "Autorización de Ingreso y Salida de Vehículos"); ;

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
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
                    populateDrop(dphumedad, CslHelper.getHumedad());
                    if (dphumedad.Items.Count > 0)
                    {
                        if (dphumedad.Items.FindByValue("0") != null)
                        {
                            dphumedad.Items.FindByValue("0").Selected = true;
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

                    populateDrop(dpembala, CslHelper.getEmbalajes());
                    if (dpembala.Items.Count > 0)
                    {
                        if (dpembala.Items.FindByValue("0") != null)
                        {
                            dpembala.Items.FindByValue("0").Selected = true;
                        }
                    }

                    populateDrop(dpimo, CslHelper.getImos());
                    if (dpimo.Items.Count > 0)
                    {
                        if (dpimo.Items.FindByValue("0") != null)
                        {
                            dpimo.Items.FindByValue("0").Selected = true;
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
            try
            {
                if (!HttpContext.Current.Request.IsAuthenticated)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "cargaconsolidar", "ValidateJSON", "No autenticado", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Actualmente no se encuentra autenticado, sera redireccionado a la pagina de login");
                }
                //validar que la sesión este activa y viva
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "cargaconsolidar", "ValidateJSON", "Sesión no existe", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Su sesión ha expirado, sera redireccionado a la pagina de login");
                }
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                //Validacion 3 -> Si su token existe, es válido, y no ha expirado
                if (token == null || !csl_log.log_csl.validateToken(token.Value))
                {
                    string pOpcion = HttpContext.Current.Request.QueryString["opcion"];
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "cargaconsolidar", "ValidateJSON", "Sin Valor Token", "No tokenID");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../cuenta/subopciones.aspx?opcion='" + pOpcion, "Su formulario ha expirado, por favor reingrese de nuevo desde el menú");
                }
                
                var jmsg = new jMessage();
                usuario sUser = null;
                 sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                //Validacion 2 -> Obtener la sesión y covetirla a objectsesion
                var user = new ObjectSesion();
                user.clase = "cargaconsolidar"; user.metodo = "ValidateJSON";
                user.transaccion = "AISV cargaconsolidar"; user.usuario = sUser.loginname;


                //Validacion 4 --  limpiar todo el objeto
                DataTransformHelper.CleanProperties<jAisvContainer>(objeto);

                //preparo el mensaje que será enviado al explorador
                var mensaje = string.Empty;
                 jmsg = new jMessage();
                jmsg.data = string.Empty;
                jmsg.fluir = false;

                  //seteo el token
                user.token = token.Value;
                objeto.autor = sUser.loginname; 
                //nuevo uso el login que esta en la session!!
                objeto.idexport = sUser.ruc;

                //###########################
                //valida transportista
                //###########################
                try
                {
                    

                    //valida empresa, chofer, placa
                    if (string.IsNullOrEmpty(objeto.tranruc))
                    {
                        jmsg.fluir = false;
                        jmsg.mensaje = string.Format("56. Debe seleccionar una compañía de transporte valida para agregar la información");
                        return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                    }
                    else
                    {
                        var EmpresaTransporte = N4.Entidades.CompaniaTransporte.ObtenerCompania(sUser.loginname, objeto.tranruc);
                        if (!EmpresaTransporte.Exitoso)
                        {
                            jmsg.fluir = false;
                            jmsg.mensaje = string.Format("56. Debe seleccionar una compañía de transporte valida para agregar la información");
                            return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                        }
                    }

                    if (string.IsNullOrEmpty(objeto.tdocument))
                    {
                        jmsg.fluir = false;
                        jmsg.mensaje = string.Format("50. Debe seleccionar un chofer valido, asociada a la empresa de transporte {0}", objeto.trancia);
                        return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                    }
                    else
                    {
                        var ChoferTransporte = N4.Entidades.Chofer.ObtenerChofer(sUser.loginname, objeto.tdocument);
                        if (!ChoferTransporte.Exitoso)
                        {
                            jmsg.fluir = false;
                            jmsg.mensaje = string.Format("50. Debe seleccionar un chofer valido, asociada a la empresa de transporte {0}", objeto.trancia);
                            return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                        }
                    }

                    if (string.IsNullOrEmpty(objeto.tplaca))
                    {
                        jmsg.fluir = false;
                        jmsg.mensaje = string.Format("51. Debe ingresar una placa de vehículo valida, asociada a la empresa de transporte {0}", objeto.trancia);
                        return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                    }
                    else
                    {
                        var PlacaTransporte = N4.Entidades.Camion.ObtenerCamion(sUser.loginname, objeto.tplaca);
                        if (!PlacaTransporte.Exitoso)
                        {
                            jmsg.fluir = false;
                            jmsg.mensaje = string.Format("51. Debe ingresar una placa de vehículo valida, asociada a la empresa de transporte {0}", objeto.trancia);
                            return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                        }
                    }


                }
                catch
                {

                }



                //Validación 5 -> Que todas las reglas básicas de negocio se cumplan
                jmsg.resultado = jAisvContainer.ValidateAisvData(objeto, sUser.ruc, sUser.bloqueo_cartera, out mensaje);
                if (!jmsg.resultado)
                {
                    jmsg.mensaje = mensaje;

                    if (objeto.QDae == "Y")
                    {
                        jmsg.fluir = true;
                        jmsg.data = "window.open('../ecuapass/consulta.aspx','Consultar DAE','width=1000,height=800,scrollbars=yes')";
                        jmsg.mensaje = string.Format("{0}\nDe clic en Aceptar para ser direccionado a la opción Consulta de la DAE.", jmsg.mensaje, objeto.adocnumero);

                        HttpContext.Current.Session["cosulta_dae"] = objeto.adocnumero;
                    }

                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                objeto.nomexpo = CslHelper.getShiperName(objeto.idexport);

                //transporte a N4
                if (!objeto.TransaportToN4(user, out mensaje))
                {
                    jmsg.mensaje = mensaje;
                    jmsg.resultado = false;
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
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
                        oby.clase = "cargaconsolidar";
                        oby.metodo = "jaisv.Add";
                        oby.transaccion = "Falló add";
                        oby.usuario = "sistema";
                        string mes = string.Empty;
                        jAisvContainer.cancelAdvice(oby, objeto.unumber, objeto.breferencia, out mes);
                    }
                    catch (Exception ex)
                    {
                        csl_log.log_csl.save_log<Exception>(ex, "cargaconsolidar", "cancelAdvice", objeto.secuencia, "sistema");
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
                //confirmación final a N4
                aisv = objeto.secuencia;

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


        [System.Web.Services.WebMethod]
        public static string[] GetPlaca(string idempresa, string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Camion = N4.Entidades.Camion.ObtenerCamiones(ClsUsuario.loginname, prefix, idempresa);
                if (Camion.Exitoso)
                {
                    var LinqQuery = (from Tbl in Camion.Resultado.Where(Tbl => Tbl.numero != null)
                                     select new
                                     {
                                         EMPRESA = string.Format("{0}", Tbl.numero.Trim()),
                                         RUC = Tbl.numero.Trim(),
                                         NOMBRE = Tbl.numero.Trim(),
                                         ID = Tbl.numero.Trim()
                                     });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }

                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }
    }
}