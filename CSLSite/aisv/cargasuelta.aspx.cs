using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.Web.Script.Services;
using ConectorN4;
using BillionEntidades;
using System.Globalization;

namespace CSLSite
{
    public partial class cargasuelta : System.Web.UI.Page
    {
        private string _Id_Opcion_Servicio = string.Empty;
        private Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();
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
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "cargasuelta", "Page_Init", "No autenticado", "No disponible");
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
            if (user != null)
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

                    this.CboDestino.SelectedIndex = 0;
                    //this.cmbMarca.Attributes["disabled"] = "disabled";
                    //bool activa = true;
                    //var cfgs = HttpContext.Current.Session["parametros"] as List<dbconfig>;
                    //var cf = cfgs.Where(f => f.config_name.Contains("valida_aisv_cfs")).FirstOrDefault();
                    //if (cf == null || string.IsNullOrEmpty(cf.config_value) || cf.config_value.Contains("0"))
                    //{
                    //    activa = false;
                    //}
                    //if (activa)
                    //{
                    //    //visible
                    //    this.ocultar1.Visible = true;
                    //    this.ocultar2.Visible = true;
                    //    this.ocultar3.Visible = true;
                    //}
                    //else
                    //{
                    //    this.ocultar1.Visible = false;
                    //    this.ocultar2.Visible =false;
                    //    this.ocultar3.Visible = false;
                    //}

                    /************************
                    * servicio CGSApp Agente
                    *************************
                     /********************************************************************
                     * si esta activa la validacion se muestra el check al usuario Agente
                     **********************************************************************/
                    usuario ClsUsuario;
                    string cMensajes;

                    try
                    {
                        var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        ClsUsuario = ClsUsuario_;
                    }
                    catch
                    {
                        Response.Redirect("../login.aspx", false);
                        return;
                    }
                    bool Valida_Agente = false;
                    var ruc_contecon = System.Configuration.ConfigurationManager.AppSettings["ruc_cgsa"];
                    if (!ruc_contecon.ToString().Trim().Contains(ClsUsuario.ruc)) 
                    {
                        List<Cls_Bil_Configuraciones> ValidaSNA = Cls_Bil_Configuraciones.Get_Validacion("VALIDA_CGSAppBAN", out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {
                            this.Alerta(string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en configuraciones.</br> {0} ", cMensajes));
                            return;
                        }
                        else
                        {
                            if (ValidaSNA.Count != 0)
                            {
                                Valida_Agente = true;
                            }
                            if (Valida_Agente)
                            {
                                this.ServicioAG.Visible = true;
                                this.ChkAppCgsaAG.Visible = true;
                                this.ChkAppCgsaAG.Checked = true;
                                this.ChkAppCgsaAG.Attributes.Remove("disabled");
                                this.ChkAppCgsaAG.Checked = true;
                            }
                            else
                            {
                                this.ChkAppCgsaAG.Checked = false;
                                this.ChkAppCgsaAG.Visible = false;
                                this.ServicioAG.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        this.ChkAppCgsaAG.Checked = false;
                        this.ChkAppCgsaAG.Visible = false;
                        this.ServicioAG.Visible = false;
                    }
               
                    //valida si ya esta suscrito
                    if (Valida_Agente)
                    {
                        bool? IsSuscrito = Cls_SuscripcionBanano.VerificaSiExisteCliente(ClsUsuario.ruc, out cMensajes);

                        if (IsSuscrito == null)
                        {
                            this.ChkAppCgsaAG.Checked = false;
                            this.susribirse.Visible = false;
                        }
                        else
                        {
                            this.ChkAppCgsaAG.Checked = true;
                            if (IsSuscrito == true)
                            {
                                this.ChkAppCgsaAG.Attributes["disabled"] = "disabled";
                                this.susribirse.Visible = false;
                            }
                        }

                        UPCGSAAPPAG.Update();
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
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "cargasuelta", "ValidateJSON", "No autenticado", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Actualmente no se encuentra autenticado, sera redireccionado a la pagina de login");
                }
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "cargasuelta", "ValidateJSON", "Sesión no existe", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Su sesión ha expirado, sera redireccionado a la pagina de login");
                }
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                //Validacion 3 -> Si su token existe, es válido, y no ha expirado
                if (token == null || !csl_log.log_csl.validateToken(token.Value))
                {
                    string pOpcion = HttpContext.Current.Request.QueryString["opcion"];
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "cargasuelta", "ValidateJSON", "Sin Token", "No TokenID");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../cuenta/subopciones.aspx?opcion='" + pOpcion, "Su formulario ha expirado, por favor reingrese de nuevo desde el menú");
                }
                
                var jmsg = new jMessage();
                usuario sUser = null;
                //validar que la sesión este activa y viva
                 sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                //Validacion 2 -> Obtener la sesión y covetirla a objectsesion
                var user = new ObjectSesion();
                user.clase = "cargasuelta"; user.metodo = "ValidateJSON";
                user.transaccion = "AISV cargasuelta"; user.usuario = sUser.loginname;

    
                //Validacion 4 --  limpiar todo el objeto
                DataTransformHelper.CleanProperties<jAisvContainer>(objeto);

                //preparo el mensaje que será enviado al explorador
                var mensaje = string.Empty;
                 jmsg = new jMessage();
                jmsg.data = string.Empty;
                jmsg.fluir = false;

                string cMensajes;
                List<Cls_Bil_Configuraciones> ValidaCampos = Cls_Bil_Configuraciones.Get_Validacion("VALIDA_AISV", out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {
                    objeto.validacampos = false;
                }
                else
                {
                    objeto.validacampos = false;
                    if (ValidaCampos.Count != 0)
                    {
                        objeto.validacampos = true;
                    }
                }

                List<Cls_Bil_Configuraciones> ValidaBooking = Cls_Bil_Configuraciones.Get_Validacion("VALIDA_BOOKING", out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {
                    objeto.valida_saldo_booking = false;
                }
                else
                {
                    objeto.valida_saldo_booking = false;
                    if (ValidaBooking.Count != 0)
                    {
                        objeto.valida_saldo_booking = true;
                    }
                }

                List<Cls_Bil_Configuraciones> ValidaBL = Cls_Bil_Configuraciones.Get_Validacion("VALIDA_AISV_BL", out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {
                    objeto.valida_bl_banano = false;
                }
                else
                {
                    objeto.valida_bl_banano = false;
                    if (ValidaBL.Count != 0)
                    {
                        objeto.valida_bl_banano = true;
                    }
                }

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
                        jmsg.mensaje = string.Format("51. Debe seleccionar un chofer valido, asociada a la empresa de transporte {0}", objeto.trancia);
                        return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                    }
                    else
                    {
                        var ChoferTransporte = N4.Entidades.Chofer.ObtenerChofer(sUser.loginname, objeto.tdocument);
                        if (!ChoferTransporte.Exitoso)
                        {
                            jmsg.fluir = false;
                            jmsg.mensaje = string.Format("51. Debe seleccionar un chofer valido, asociada a la empresa de transporte {0}", objeto.trancia);
                            return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                        }
                    }

                    if (string.IsNullOrEmpty(objeto.tplaca))
                    {
                        jmsg.fluir = false;
                        jmsg.mensaje = string.Format("49. Debe ingresar una placa de vehículo valida, asociada a la empresa de transporte {0}", objeto.trancia);
                        return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                    }
                    else
                    {
                        var PlacaTransporte = N4.Entidades.Camion.ObtenerCamion(sUser.loginname, objeto.tplaca);
                        if (!PlacaTransporte.Exitoso)
                        {
                            jmsg.fluir = false;
                            jmsg.mensaje = string.Format("49. Debe ingresar una placa de vehículo valida, asociada a la empresa de transporte {0}", objeto.trancia);
                            return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                        }
                    }


                }
                catch
                {

                }

                //###########################
                //valida booking de acopio
                //###########################
                bool activaVBSAcopio = true;
                var vParam = HttpContext.Current.Session["parametros"] as List<dbconfig>;
                var vbsAcopio = vParam.Where(f => f.config_name.Contains("activa_vbs_acopio")).FirstOrDefault();
                if (objeto.vbs_destino == "3")
                {
                    if (vbsAcopio == null || string.IsNullOrEmpty(vbsAcopio.config_value) || vbsAcopio.config_value.Contains("0"))
                    {
                        activaVBSAcopio = false;
                    }

                    if (activaVBSAcopio)
                    {
                        if (VBSEntidades.BananoBodega.BAN_Stowage_Plan_Det.VerificaSiExisteAgente(objeto.bnumber, out mensaje).Equals(false))
                        {
                            jmsg.mensaje = string.Format("BOOKING NO ESTÁ AUTORIZADO PARA REALIZAR Consolidación/Acópio | {0}", mensaje);
                            jmsg.resultado = false;
                            return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                        }
                    }
                }

                //seteo el token
                user.token = token.Value;
                objeto.autor = sUser.loginname; 
                //nuevo uso el login que esta en la session!!
                objeto.idexport = sUser.ruc;

                //verifica si esta suscrito
                bool? IsSuscrito = false;
                try
                {
                    if (objeto.vbs_destino == "2")
                    {
                        bool Valida_Agente = false;
                        List<Cls_Bil_Configuraciones> ValidaSNA = Cls_Bil_Configuraciones.Get_Validacion("VALIDA_CGSAppBAN", out cMensajes);

                        if (String.IsNullOrEmpty(cMensajes))
                        {
                            if (ValidaSNA.Count != 0)
                            {
                                Valida_Agente = true;
                            }
                        }

                        if (Valida_Agente)
                        {
                            IsSuscrito = Cls_SuscripcionBanano.VerificaSiExisteCliente(sUser.ruc, out cMensajes);

                            //YA A SIDO SUSCRITO PERO CLIENTE HA SOLICITADO DAR DE BAJA
                            if (IsSuscrito is null)
                            {
                                IsSuscrito = false;
                            }
                            else
                            {
                                //EL CLIENTE AL GENERAR EL AISV NO QUITÓ EL CHECK DE SUSCRIPCIÓN 
                                if (IsSuscrito == false && objeto.suscritoCGSApp.ToUpper() == "TRUE")
                                {
                                    IsSuscrito = true;
                                    //############################################
                                    //SE GUARDA EL REGISTRO DE SUSCRIPCIÓN
                                    //############################################
                                    string _oError = string.Empty;

                                    usuario ClsUsuario;
                                    try
                                    {
                                        var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                                        ClsUsuario = ClsUsuario_;
                                    }
                                    catch
                                    {
                                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "cargasuelta", "ValidateJSON", "No autenticado", "No disponible");
                                        return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Actualmente no se encuentra autenticado, sera redireccionado a la pagina de login");
                                    }

                                    try
                                    {
                                        Cls_SuscripcionBanano oSuscribirCliente = new Cls_SuscripcionBanano();
                                        oSuscribirCliente.ClientId = ClsUsuario.ruc;
                                        oSuscribirCliente.Client = ClsUsuario.apellidos + " " + ClsUsuario.nombres;
                                        oSuscribirCliente.Create_user = ClsUsuario.loginname;
                                        oSuscribirCliente.file_pdf = string.Empty;
                                        oSuscribirCliente.Comment = string.Empty;
                                        oSuscribirCliente.activo = true;

                                        oSuscribirCliente.SaveCliente(out _oError);
                                        if (string.IsNullOrEmpty(_oError))
                                        {
                                            //sp enviar correo
                                            Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();
                                            objMail.Ruc = ClsUsuario.ruc;
                                            objMail.Email = ClsUsuario.email;
                                            string error;
                                            var nProceso = objMail.SaveTransactionMail(out error);
                                            //fin de nuevo proceso de grabado
                                            if (!nProceso.HasValue || nProceso.Value <= 0)
                                            {

                                            }
                                        }
                                        IsSuscrito = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        csl_log.log_csl.save_log<Exception>(ex, "cargasuelta", "ValidateJSON", ClsUsuario.ruc, "sistema");
                                    }
                                }

                            }
                        }
                    }
                }
                catch { }
                objeto.suscritoCGSApp = IsSuscrito.ToString();
                //objeto.aisv_referencia = objeto.aisv_referencia.ToString();

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
                        oby.clase = "cargasuelta";
                        oby.metodo = "jaisv.Add";
                        oby.transaccion = "Falló add";
                        oby.usuario = "sistema";
                        string mes = string.Empty;
                        jAisvContainer.cancelAdvice(oby, objeto.unumber, objeto.breferencia, out mes);
                    }
                    catch (Exception ex)
                    {
                        csl_log.log_csl.save_log<Exception>(ex, "cargasuelta", "cancelAdvice", objeto.secuencia, "sistema");
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
                else
                {
                    bool activa = true;
                    bool activaVBSBodega = true;
                    var cfgs = HttpContext.Current.Session["parametros"] as List<dbconfig>;
                    var cf = cfgs.Where(f => f.config_name.Contains("valida_aisv_cfs")).FirstOrDefault();
                    var vbsBod = cfgs.Where(f => f.config_name.Contains("activa_vbs_Bodega")).FirstOrDefault();
                    var vbsBodRuc = cfgs.Where(f => f.config_name.Contains("activa_vbs_Bodega_ruc")).FirstOrDefault();
                    if (cf == null || string.IsNullOrEmpty(cf.config_value) || cf.config_value.Contains("0"))
                    {
                        activa = false;
                    }
                    if (vbsBod == null || string.IsNullOrEmpty(vbsBod.config_value) || vbsBod.config_value.Contains("0"))
                    {
                        activaVBSBodega = false;
                    }
                    else
                    {
                        if (!(vbsBodRuc is null))
                        {
                            if (!string.IsNullOrEmpty(vbsBodRuc?.config_value.Trim()))
                            {
                                if (!vbsBodRuc.config_value.Contains("*"))
                                {
                                    if (!vbsBodRuc.config_value.Contains(objeto.idexport))
                                    {
                                        activaVBSBodega = false;
                                    }
                                }
                            }
                        }
                    }

                    if (activa)
                    {
                        int dy = 0;
                        if (int.TryParse(objeto.ubultos, out dy))
                        {

                        }
                        //Int64 idLoadingDet = 0;
                        //if (!Int64.TryParse(objeto.vbs_hora_cita, out idLoadingDet))
                        //{

                        //}
                        //graba tablas nuevas de vbs
                        if (objeto.vbs_destino == "1")
                        {
                            Cls_CFS_Turnos_Banano objActualizar = new Cls_CFS_Turnos_Banano();
                            objActualizar.idLoadingDet_remanente = objeto.vbs_hora_cita;
                            objActualizar.aisv_codigo = objeto.secuencia;
                            objActualizar.box = dy;
                            objActualizar.aisvUsuarioCrea = sUser.ruc;

                            string xerror;
                            var nProcesoCarbono = objActualizar.SaveTransaction_VBS_remanente(out xerror);
                            /*fin de nuevo proceso de grabado*/
                            if (!nProcesoCarbono.HasValue || nProcesoCarbono.Value <= 0)
                            {

                            }
                        }
                        
                    } //VBS MUELLE

                    if (activaVBSBodega)//VBS BODEGA
                    {
                        int dy = 0;
                        if (int.TryParse(objeto.ubultos, out dy))
                        {

                        }
                     
                        //graba tablas nuevas de vbs
                        if (objeto.vbs_destino == "2")
                        {
                            var oStowagePlanTurno = VBSEntidades.BananoBodega.BAN_Stowage_Plan_Turno.GetEntidad(long.Parse(objeto.vbs_hora_cita.ToString()));

                            Cls_CFS_Turnos_Banano objActualizar = new Cls_CFS_Turnos_Banano();
                            objActualizar.idStowageDet = long.Parse(oStowagePlanTurno.idStowageDet.ToString());

                            objActualizar.reservado = oStowagePlanTurno.reservado + dy;
                            objActualizar.disponible = oStowagePlanTurno.box - dy;
                            //objActualizar.aisvUsuarioCrea = sUser.ruc;

                            objActualizar.idStowagePlanTurno = long.Parse(objeto.vbs_hora_cita.ToString());
                            objActualizar.idStowageDet = long.Parse(oStowagePlanTurno.idStowageDet.ToString());
                            objActualizar.fecha = oStowagePlanTurno.fecha;
                            objActualizar.idHoraInicio = int.Parse(oStowagePlanTurno.idHoraInicio.ToString());
                            objActualizar.horaInicio = oStowagePlanTurno.horaInicio;
                            objActualizar.idHoraFin = int.Parse(oStowagePlanTurno.idHoraFin.ToString());
                            objActualizar.horaFin = oStowagePlanTurno.horaFin;
                            objActualizar.box = dy;
                            //objActualizar.comentario = string.Empty;
                            objActualizar.aisv_codigo = objeto.secuencia;
                            objActualizar.dae = objeto.adocnumero;
                            objActualizar.booking = objeto.bnumber;
                            //objActualizar.IIEAutorizada = false;
                            //objActualizar.daeAutorizada = false;
                            objActualizar.placa = objeto.tplaca;
                            objActualizar.idChofer = objeto.tdocument;
                            objActualizar.chofer = objeto.tconductor;
                            objActualizar.idCapacidadHoraBodega = long.Parse(oStowagePlanTurno.idCapacidadHoraBodega.ToString());
                            objActualizar.idCapacidadHorafecha = long.Parse(oStowagePlanTurno.idCapacidadHorafecha.ToString());
                            objActualizar.estado = "NUE";
                            objActualizar.aisvUsuarioCrea = objeto.autor;
                            objActualizar.isActive = false;
                            objActualizar.aisv_referencia = objeto.aisv_referencia;

                            string xerror;
                            var nProcesoCarbono = objActualizar.SaveTransaction_VBS_remanenteBodega(out xerror);
                            /*fin de nuevo proceso de grabado*/
                            if (!nProcesoCarbono.HasValue || nProcesoCarbono.Value <= 0)
                            {

                            }
                        }
                    }
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
        public static string Cargaturnos(string Parametros)
        {
            var mensaje = string.Empty;
            var jmsg = new jMessage();
            jmsg = new jMessage();
            jmsg.data = string.Empty;
            jmsg.fluir = false;

            string CadenaCampos;

            CadenaCampos = Parametros;
            string[] CantidadCampos = CadenaCampos.Split('|');
            string cMensajes;
            string Fecha;
            //string HoraHasta = "00:00";
            Fecha = string.Format("{0}", CantidadCampos[2]);
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime FechaActualSalida;
            List<Cls_CFS_Turnos_Banano> Listado = null;


            List<CamposDropDown> details = new List<CamposDropDown>();


            if (!DateTime.TryParseExact(Fecha, "yyyy-MM-dd HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
            {
                jmsg.mensaje = "La fecha seleccionada, no es una fecha valida";
                jmsg.resultado = false;

                return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);

            }

            int Cantidad = 0;
            if (!int.TryParse(CantidadCampos[3], out Cantidad))
            {
                Cantidad = 0;
            }

            try {
                string v_destino = string.Empty;
                string v_idStowageDet = string.Empty;
                v_destino = string.Format("{0}", CantidadCampos[4]);
                v_idStowageDet = string.Format("{0}", CantidadCampos[5]);
                if (v_destino == "2")
                {
                    Listado = Cls_CFS_Turnos_Banano.Carga_Turnos_Remanente(long.Parse(v_idStowageDet), FechaActualSalida, Cantidad, out cMensajes);
                    if (Listado != null)
                    {
                        foreach (var Det in Listado)
                        {
                            CamposDropDown Combo = new CamposDropDown();
                            Combo.Id = Det.idStowagePlanTurno.ToString();
                            Combo.Turno = string.Format("{0} - {1}  |   Stock {2}: {3}", Det.horaInicio, Det.horaFin, Det.disponible, Det.boxExtra);
                            details.Add(Combo);
                        }

                    }
                }
                else
                {
                    Listado = Cls_CFS_Turnos_Banano.Carga_Turnos_Remanente(CantidadCampos[0], CantidadCampos[1], FechaActualSalida, Cantidad, out cMensajes);

                    if (Listado != null)
                    {
                        foreach (var Det in Listado)
                        {
                            CamposDropDown Combo = new CamposDropDown();
                            Combo.Id = Det.idLoadingDet_remanente;
                            Combo.Turno = string.Format("{0} - {1}  |   Stock {2}: {3}", Det.horaInicio, Det.horaFin, Det.bodega, Det.saldo_nuevo);
                            details.Add(Combo);
                        }

                    }
                }
            } catch { }
            

            return Newtonsoft.Json.JsonConvert.SerializeObject(details);

        }


        [System.Web.Services.WebMethod]
        public static string LlenaComboMarca(string Parametros)
        {

            string data = null;
            string CadenaCampos = string.Empty;
            List<CamposDropDown> details = new List<CamposDropDown>();
            try
            {
                CadenaCampos = Parametros;
                string[] CantidadCampos = CadenaCampos.Split('|');
                int Cantidad = 0;

                var cfgs = HttpContext.Current.Session["parametros"] as List<dbconfig>;
                var vbsBod = cfgs.Where(f => f.config_name.Contains("activa_vbs_Bodega")).FirstOrDefault();
                var vbsBodRuc = cfgs.Where(f => f.config_name.Contains("activa_vbs_Bodega_ruc")).FirstOrDefault();

                if (vbsBod == null || string.IsNullOrEmpty(vbsBod.config_value) || vbsBod.config_value.Contains("0"))
                {
                    return string.Empty;
                }
                else
                {
                    if (!(vbsBodRuc is null))
                    {
                        if (!string.IsNullOrEmpty(vbsBodRuc?.config_value.Trim()))
                        {
                            if (!vbsBodRuc.config_value.Contains("*"))
                            {
                                if (!vbsBodRuc.config_value.Contains(CantidadCampos[1].ToString()))
                                {
                                    return string.Empty;
                                }
                            }
                        }
                    }
                    
                }

                //if (!int.TryParse(CantidadCampos[3], out Cantidad))
                //{
                //    Cantidad = 0;
                //}
                string oError = string.Empty;
                //var oStowageCab = VBSEntidades.BananoBodega.BAN_Stowage_Plan_Cab.GetStowagePlanCabEspecifico()
                var oDetalle = VBSEntidades.BananoBodega.BAN_Stowage_Plan_Det.ConsultarLista(CantidadCampos[0].ToString(), CantidadCampos[1].ToString(), out oError);
                //cmbMarca.DataSource = oEntidad;
                //cmbMarca.DataValueField = "ID";
                //cmbMarca.DataTextField = "nombre";
                //cmbMarca.DataBind();
                if (oDetalle != null)
                {
                    foreach (var Det in oDetalle)
                    {
                        Det.oMarca = VBSEntidades.BananoMuelle.BAN_Catalogo_Marca.GetMarca(Det.idMarca);
                        Det.oHold = VBSEntidades.BananoMuelle.BAN_Catalogo_Hold.GetHold(Det.idHold);
                        CamposDropDown Combo = new CamposDropDown();
                        Combo.Id = Det.idStowageDet.ToString();
                        Combo.Turno = string.Format("{0} - {1}  | {2} : {3}", Det.idStowageDet.ToString(), Det.oHold?.nombre, Det.oMarca?.nombre, Det.reservado);
                        details.Add(Combo);
                    }
                }
                data =  Newtonsoft.Json.JsonConvert.SerializeObject(details);
            }
            catch (Exception ex)
            {
                csl_log.log_csl.save_log<Exception>(ex, "LlenaComboMarca", "LlenaComboMarca", "-1", "sistema");
            }
            return data;
        }

        [System.Web.Services.WebMethod]
        public static string LlenarComboAISV(string Parametros)
        {
            string data = null;
            string CadenaCampos = string.Empty;
            List<CamposDropDown> details = new List<CamposDropDown>();
            try
            {
                CadenaCampos = Parametros;
                string[] CantidadCampos = CadenaCampos.Split('|');
                //int Cantidad = 0;
                /*
                var cfgs = HttpContext.Current.Session["parametros"] as List<dbconfig>;
                var vbsBod = cfgs.Where(f => f.config_name.Contains("activa_vbs_Bodega")).FirstOrDefault();
                var vbsBodRuc = cfgs.Where(f => f.config_name.Contains("activa_vbs_Bodega_ruc")).FirstOrDefault();

                if (vbsBod == null || string.IsNullOrEmpty(vbsBod.config_value) || vbsBod.config_value.Contains("0"))
                {
                    return string.Empty;
                }
                else
                {
                    if (!(vbsBodRuc is null))
                    {
                        if (!string.IsNullOrEmpty(vbsBodRuc?.config_value.Trim()))
                        {
                            if (!vbsBodRuc.config_value.Contains("*"))
                            {
                                if (!vbsBodRuc.config_value.Contains(CantidadCampos[1].ToString()))
                                {
                                    return string.Empty;
                                }
                            }
                        }
                    }

                }*/

                string oError = string.Empty;
                var oDetalle = VBSEntidades.BananoBodega.BAN_Stowage_Plan_Aisv.LlenaComboAisv(CantidadCampos[0].ToString(), CantidadCampos[2].ToString(), CantidadCampos[3].ToString(), CantidadCampos[1].ToString(), out oError);

                if (oDetalle != null)
                {
                    CamposDropDown Combo1 = new CamposDropDown();
                    Combo1.Id = "";
                    Combo1.Turno = "-- Seleccione --";
                    details.Add(Combo1);
                    foreach (var Det in oDetalle)
                    {
                        //Det.oMarca = VBSEntidades.BananoMuelle.BAN_Catalogo_Marca.GetMarca(Det.idMarca);
                        //Det.oHold = VBSEntidades.BananoMuelle.BAN_Catalogo_Hold.GetHold(Det.idHold);
                        CamposDropDown Combo = new CamposDropDown();
                        Combo.Id = Det.aisv;
                        Combo.Turno = string.Format("{0} - {1}  | {2} : {3}", Det.aisv, Det.booking, Det.horaInicio, Det.horaFin);
                        details.Add(Combo);
                    }
                    
                }
                data = Newtonsoft.Json.JsonConvert.SerializeObject(details);
            }
            catch (Exception ex)
            {
                csl_log.log_csl.save_log<Exception>(ex, "LlenaComboMarca", "LlenaComboMarca", "-1", "sistema");
            }
            return data;
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

        protected void btnSiDeseo_Click(object sender, EventArgs e)
        {
            string _oError = string.Empty;

            usuario ClsUsuario;
            try
            {
                var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                ClsUsuario = ClsUsuario_;
            }
            catch
            {
                Response.Redirect("../login.aspx", false);
                return;
            }

            try
            {
                Cls_SuscripcionBanano oSuscribirCliente = new Cls_SuscripcionBanano();
                oSuscribirCliente.ClientId = ClsUsuario.ruc;
                oSuscribirCliente.Client = ClsUsuario.apellidos + " " + ClsUsuario.nombres;
                oSuscribirCliente.Create_user = ClsUsuario.loginname;
                oSuscribirCliente.file_pdf = string.Empty;
                oSuscribirCliente.Comment = string.Empty;
                oSuscribirCliente.activo = true;
                oSuscribirCliente.SaveCliente(out _oError);
                if (string.IsNullOrEmpty(_oError))
                {
                    this.Alerta(string.Format("<b> Felicidades!! Usted se ha suscrito al Sistema de Trazabilidad de Carga CGSApp </b>"));
                    this.ChkAppCgsaAG.Attributes["disabled"] = "disabled";

                    //sp enviar correo
                    objMail.Ruc = ClsUsuario.ruc;
                    objMail.Email = ClsUsuario.email;
                    string error;
                    var nProceso = objMail.SaveTransactionMail(out error);
                    //fin de nuevo proceso de grabado
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {

                    }
                }
                else
                {
                    this.Alerta(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", _oError));
                }

                this.ChkAppCgsaAG.Checked = true;
            }
            catch (Exception ex)
            {
                csl_log.log_csl.save_log<Exception>(ex, "cargasuelta", "btnSideseo", ClsUsuario.ruc, "sistema");
            }
            UPCGSAAPPAG.Update();
        }

        protected void btnNoDeseo_Click(object sender, EventArgs e)
        {
            this.ChkAppCgsaAG.Checked = false;
            this.ChkAppCgsaAG.Attributes.Remove("disabled");
            UPCGSAAPPAG.Update();
        }
    }

    public class CamposDropDown
    {
       // public Int64 Id { get; set; }
        public string Id { get; set; }
        public string Turno { get; set; }
    }
}