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
using System.IO;
using System.Text;
using System.Web.Services;
using VBSEntidades.Calendario;
using System.Globalization;
using VBSEntidades;
using VBSEntidades.ClaseEntidades;
using System.Web.Script.Serialization;

namespace CSLSite
{
    public partial class container : System.Web.UI.Page
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
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();
            if (user != null)
            {
                this.textbox1.Value = user.email != null ? user.email : string.Empty;
                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
            }
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
                //this.opcion_principal.InnerHtml = string.Format("<a href=\"../cuenta/subopciones.aspx?opcion={0}\">{1}</a>", _Id_Opcion_Servicio, "Servicios a clientes de CGSA"); 

            }

            //<JGUSQUI 20230914 - VALIDACION PARA OMITIR OPCION POR RUC>
            usuario sUser = null;
            sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            if (jAisvContainer.GetRucScannerActivo(sUser.ruc))
            {
                myDiv.Visible = false;
            }
            else { myDiv.Visible = true;}
            //</JGUSQUI 20230914>
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
                //getTiposRefrigerados 2019
                populateDrop(dprefservice, app_start.TurnoPan.RetornarTipos());
                if (dprefservice.Items.Count > 0)
                {
                    if (dprefservice.Items.FindByValue("*") != null)
                    {
                        dprefservice.Items.FindByValue("*").Selected = true;
                    }
                }

                //getTipoCarga
                populateDrop(CboTipoCarga, CslHelper.getTipoCarga());
                if (CboTipoCarga.Items.Count > 0)
                {
                    if (CboTipoCarga.Items.FindByValue("0") != null)
                    {
                        CboTipoCarga.Items.FindByValue("0").Selected = true;
                    }
                }


                //<JGUSQUI 20230914 - VALIDACION PARA OMITIR OPCION POR RUC>
                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                if (jAisvContainer.GetRucScannerActivo(sUser.ruc))
                {
                    myDiv.Visible = false;
                }
                else { myDiv.Visible = true;}
                //</JGUSQUI 20230914>
            }
        }

        //recargar el combo de tipos
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
            //NUEVO 2019
            objeto.QDae = "N";

            try
            {
                if (!HttpContext.Current.Request.IsAuthenticated)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "ValidateJSON", "No autenticado", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Actualmente no se encuentra autenticado, sera redireccionado a la pagina de login");
                }
                //validar que la sesión este activa y viva
                if (HttpContext.Current.Session["control"] == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "container", "ValidateJSON", "Sesión no existe", "No disponible");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../login.aspx'", "Su sesión ha expirado, sera redireccionado a la página de login");
                }
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                //Validacion 3 -> Si su token existe, es válido, y no ha expirado
                if (token == null || !csl_log.log_csl.validateToken(token.Value))
                {
                    string pOpcion = HttpContext.Current.Request.QueryString["opcion"];
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "container", "ValidateJSON", "No token", "Sin tokenID");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../cuenta/subopciones.aspx?opcion='" + pOpcion, "Su formulario ha expirado, por favor reingrese de nuevo desde el menú");
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


                if (!string.IsNullOrEmpty(objeto.fechCalendarLlega) && string.IsNullOrEmpty(objeto.horaCalendarLlega))
                {
                    jmsg.fluir = false;
                    jmsg.mensaje = string.Format("28.1 Se debe agregar hora de la cita ");
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }

                if (string.IsNullOrEmpty(objeto.id_tipocarga))
                {
                    jmsg.fluir = false;
                    jmsg.mensaje = string.Format("21.1 Se debe seleccionar el tipo de carga ");
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
                else
                {
                    if (objeto.id_tipocarga.Equals("0"))
                    {
                        jmsg.fluir = false;
                        jmsg.mensaje = string.Format("21.1 Se debe seleccionar el tipo de carga ");
                        return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                    }
                }
                try
                {
                    if (objeto.idTurno <= 0)
                    {
                        jmsg.fluir = false;
                        jmsg.mensaje = string.Format("28.1 Se debe seleccionar una cita ");
                        return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                    }

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
                    if (objeto.hasprof == "N")
                    {
                        jmsg.fluir = true;
                        jmsg.data = "window.open('../portal/proforma_cn.aspx','Proformas','width=1000,height=800,scrollbars=yes');";
                        jmsg.mensaje = string.Format("{0}\n Va a ser direccionado a la página de proformas", jmsg.mensaje);
                    }

                    if (objeto.QDae == "Y")
                    {
                        jmsg.fluir = true;
                        jmsg.data = "window.open('../ecuapass/consulta.aspx','Consultar DAE','width=1000,height=800,scrollbars=yes')";
                        jmsg.mensaje = string.Format("{0}\nDe clic en Aceptar para ser direccionado a la opción Consulta de la DAE.", jmsg.mensaje,objeto.adocnumero);

                        HttpContext.Current.Session["cosulta_dae"] = objeto.adocnumero;
                    }


                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
                //  System.Diagnostics.Trace.Write(string.Format("----->Fin de Validaciones:{0}",DateTime.Now));
                //transporte a N4
                objeto.nomexpo = CslHelper.getShiperName(objeto.idexport);



                VBS_Turno oTurno;
                try
                {
                    oTurno = VBS_Turno.ListTurnos(objeto.idTurno).FirstOrDefault();
                    objeto.fechCalendarLlega = oTurno.VigenciaInicial.ToString("dd/MM/yyyy");
                    //objeto.horaCalendarLlega = oTurno.Horario;

                    if (oTurno.Disponible <= 0)
                    {
                        jmsg.fluir = false;
                        jmsg.mensaje = string.Format("28.1 Debe elegir otro turno, horario ya no se encuentra disponible ");
                        return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                    }
                }
                catch {

                    oTurno = new VBS_Turno();
                }

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
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var idTurno = objeto.idTurno;
                var NombreUsuario = ClsUsuario.nombres + "-" + ClsUsuario.apellidos;

                objCab.EditarTurnoDisponibles(idTurno, NombreUsuario, DateTime.Now, aisv, objeto.tdocument, objeto.tconductor,objeto.tplaca);
                // System.Diagnostics.Trace.Write(string.Format("----->Fin de Insert:{0}", DateTime.Now));
                //paso toda la transacción, ahora se debe encriptar
                jmsg.data = aisv;
                jmsg.mensaje = QuerySegura.EncryptQueryString(aisv);
                //este ya retorna el valor con el mensaje->Validacion,Insert,Exception

                //NUEVO 2020-------------->CARBONO NEUTRO
                if (objeto.carbono != null)
                {
                    if (!objeto.carbono.Contains("X"))
                    {
                        //AQUI GRABAR EL OBJETO AISV_PARA TABLA DE CARBONO//
                        var cbr = new app_start.CarbonoNeutro();
                        cbr.aisv_booking = objeto.bnumber;
                        cbr.aisv_producto = objeto.producto;
                        cbr.ruc_exportador = sUser.ruc;
                        cbr.email_exportador = objeto.mail1;
                        cbr.email_exportador1 = objeto.mail2;
                        cbr.email_exportador2 = objeto.mail3;
                        cbr.email_exportador3 = objeto.mail4;
                        cbr.email_exportador4 = objeto.mail5;

                        cbr.nombres_exportador = objeto.nomexpo;
                        cbr.aisv_numero = aisv;
                        cbr.aisv_proforma = objeto.prosequence;
                        cbr.aisv_contenedor = objeto.unumber;
                        cbr.aisv_login = sUser.loginname;
                        cbr.aisv_referencia = objeto.breferencia;
                        cbr.cert_tipo = objeto.carbono;
                        string cd;
                        if (!cbr.Grabar(out cd))
                        {
                            csl_log.log_csl.save_log<ApplicationException>(new ApplicationException(cd), "container", "Carbono_Garbar", objeto.secuencia, "sistema");
                        }
                    }

                }

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
        public static string IsAvailableTG(string placa)
        {
            if (asignacionDae.IsTruckTag(placa))
            {
                //var validacionError = string.Format("*Datos del transporte*\nEl camión con placas {0}, a partir del 15 de octubre del 2018 debe poseer TAG para el ingreso a la terminarl, comuníquese con CGSA en horarios de oficina.", placa);
                //return false;
                return "1";
            }
            else
            {
                return "2";
            }

            /*
            if (c.Rows.Count == 0)
            {
                return "2";
            }
            else
            {
                rucbooking = c.Rows[0][0].ToString();
                if (rucuser == rucbooking)
                {
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
            */
        }

        [WebMethod]
        public static string ConsultarEventosPorDiaAISV(string start, string ISO)
        {
            try
            {
               List<EventoCalendario> eventos = new List<EventoCalendario>();

                DateTime fechaSeleccionada = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime fechaActual = DateTime.Now;

                string fechaRestadaString = fechaSeleccionada.ToString("yyyy-MM-dd");

                // Verificar si la fecha seleccionada es la fecha actual
                bool esFechaActual = fechaSeleccionada.Date == fechaActual.Date;

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                jAisvContainer dbo = new jAisvContainer();
                var getTipoCarga = string.Empty;
                TimeSpan duracion = fechaSeleccionada - fechaActual;
                var tiempoespera = 100;
                // Verificar si la duración es mayor a 3 días (72 horas)
                if (duracion.TotalHours > 72)
                {
                    // La fecha seleccionada es mayor a 3 días
                    tiempoespera = Convert.ToInt32(objCab.GetParametrosValida("Tiempo_Previo_Cutoff"));
                }

                getTipoCarga = dbo.GetISO(ISO);
                if (getTipoCarga == null || getTipoCarga == "")
                    getTipoCarga = "TODOS";

                if (getTipoCarga == "TODOS")
                {
                    getTipoCarga = "ALL";
                    var consultaTurnosDetalle = objCab.GetListaTurnosPorDiaTipoContenedorALL(fechaRestadaString, getTipoCarga,tiempoespera);

                    if (consultaTurnosDetalle.Resultado != null)
                    {
                        foreach (VBS_TurnosDetalle detalle in consultaTurnosDetalle.Resultado)
                        {
                            EventoCalendario evento = new EventoCalendario();
                            evento.title = $"{detalle.TipoCargas} - {detalle.TipoContenedor}"; // Combinar tipo_contenedor y total_turnos
                            evento.start = fechaRestadaString;
                            evento.end = fechaRestadaString;
                            if (detalle.TipoCargaId == 1)
                                evento.color = "#336BFF";
                            if (detalle.TipoCargaId == 2)
                                evento.color = "#17a2b8";
                            if (detalle.TipoCargaId == 3)
                                evento.color = "#dc3545";

                            evento.horario = detalle.Horario.ToString(@"hh\:mm");
                            evento.idDetalle = detalle.IdTurno;
                            evento.cantidad = detalle.Disponible;
                            // Si es la fecha actual, verificar si la hora ya ha pasado o no tiene suficiente anticipación
                            if (esFechaActual)
                            {
                                DateTime horaEvento = fechaSeleccionada.Date + detalle.Horario;
                                TimeSpan anticipacionMinima = new TimeSpan(0, 45, 0);
                                DateTime horaLimite = fechaActual.Add(anticipacionMinima);

                                if (horaEvento < horaLimite)
                                {
                                    // La hora ya ha pasado o no tiene suficiente anticipación, no se agrega al evento
                                    continue;
                                }
                            }

                            eventos.Add(evento);
                        }
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(eventos.ToArray());

                    // Serializar los datos de eventos paginados a JSON
                    return json;
                }
                else
                {
                    
                    var consultaTurnosDetalle = objCab.GetListaTurnosPorDiaTipoContenedor(fechaRestadaString, getTipoCarga,tiempoespera);

                    if (consultaTurnosDetalle.Resultado != null)
                    {
                        foreach (VBS_TurnosDetalle detalle in consultaTurnosDetalle.Resultado)
                        {
                            EventoCalendario evento = new EventoCalendario();
                            evento.title = $"{detalle.TipoCargas} - {detalle.TipoContenedor}"; // Combinar tipo_contenedor y total_turnos
                            evento.start = fechaRestadaString;
                            evento.end = fechaRestadaString;
                            if (detalle.TipoCargaId == 1)
                                evento.color = "#336BFF";
                            if (detalle.TipoCargaId == 2)
                                evento.color = "#17a2b8";
                            if (detalle.TipoCargaId == 3)
                                evento.color = "#dc3545";

                            evento.horario = detalle.Horario.ToString(@"hh\:mm");
                            evento.idDetalle = detalle.IdTurno;
                            evento.cantidad = detalle.Disponible;
                            // Si es la fecha actual, verificar si la hora ya ha pasado o no tiene suficiente anticipación
                            if (esFechaActual)
                            {
                                DateTime horaEvento = fechaSeleccionada.Date + detalle.Horario;
                                TimeSpan anticipacionMinima = new TimeSpan(0, 45, 0);
                                DateTime horaLimite = fechaActual.Add(anticipacionMinima);

                                if (horaEvento < horaLimite)
                                {
                                    // La hora ya ha pasado o no tiene suficiente anticipación, no se agrega al evento
                                    continue;
                                }
                            }

                            eventos.Add(evento);
                        }
                    }

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(eventos.ToArray());
                    
                    // Serializar los datos de eventos paginados a JSON
                    System.Threading.Thread.Sleep(1000);
                    return json;
                }

              
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }

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