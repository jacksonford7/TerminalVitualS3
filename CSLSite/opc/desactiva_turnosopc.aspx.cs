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
using System.Text;
using ControlOPC.Entidades;
using System.Data;
using System.Globalization;

namespace CSLSite
{
    public partial class desactiva_turnosopc : System.Web.UI.Page
    {
        //
        private Vessel_Visit objVesselV = new Vessel_Visit();
        private Vessel_Crane objVesselCrane = new Vessel_Crane();
        private Crane_Turn objVesselTurno = new Crane_Turn();

        #region "Propiedades"

        public static string v_mensaje = string.Empty;

        private DataTable pDetalleGruas
        {
            get
            {
                return (DataTable)Session["DtDetalleGruas"];
            }
            set
            {
                Session["DtDetalleGruas"] = value;
            }

        }

        private DataTable pDetalleturnos
        {
            get
            {
                return (DataTable)Session["DtDetalleTurnos"];
            }
            set
            {
                Session["DtDetalleTurnos"] = value;
            }

        }

        #endregion

        #region "Metodos"

    
        private void MessageBox(String msg, Page page)
        {
            StringBuilder s = new StringBuilder();
            msg = msg.Replace("'", " ");
            msg = msg.Replace(System.Environment.NewLine, " ");
            msg = msg.Replace("/", "");
            s.Append(" alertify.alert('").Append(msg).Append("');");
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), s.ToString(), true);

        }

        private void Limpiar()
        {
            //Caja de Texto
            TxtReferencia.Text = null;
            TxtFechaDesde.Text = "";
          

            LblNombre.Text = null;
            LblViaje.Text = null;
            LblETA.Text = null;
            LblETD.Text = null;
            LblHoras.Text = null;
            LblVoyageIn.Text = null;
            LblVoyageOut.Text = null;

            CboGruas.DataSource = null;
            CboGruas.DataBind();

            Session["VesselV"] = null;

            TableTurnos.DataSource = null;
            TableTurnos.DataBind();

        }

        private void CargaTurnos(string _pReferencia)
        {

            try
            {

                List<Crane_Turn> Llenacombo = Crane_Turn.GetListturnReferenceCombo(_pReferencia, out v_mensaje);
                if (v_mensaje != string.Empty)
                {
                    this.MessageBox(v_mensaje, this);
                    CboGruas.DataSource = null;
                    CboGruas.DataBind();
                    return;
                }
                else
                {
                    if (Llenacombo != null && Llenacombo.Count > 0)
                    {
                        this.CboGruas.DataSource = Llenacombo;
                        CboGruas.DataBind();
                    }
                    else {
                        if (this.CboGruas.Items.Count > 0)
                        {
                            this.CboGruas.Items.Clear();
                        } 
                        CboGruas.DataSource = null;
                        CboGruas.DataBind();
                    }
                    
                }

            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }

        }

        protected string jsarguments(object idGrua, object NumberTurno)
        {
            return string.Format("{0};{1}", idGrua != null ? idGrua.ToString().Trim() : "0", NumberTurno != null ? NumberTurno.ToString().Trim() : "0");
        }

      
        protected void RemoverTurno_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "RemoverTurno_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "RemoverTurno_ItemCommand", "CommandArgument is NULL", user.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandName == "RefButton")
                    {
                        var xpars = e.CommandArgument.ToString();
                        if (xpars.Length <= 0)
                        {

                            var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "RemoverTurno_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                            this.MessageBox(cMensaje2.ToString(), this);
                            return;
                        }
                        Label lbl = (Label)e.Item.FindControl("LblStatus");
                        if (lbl.Text == "INACTIVO")
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alert('no puede inactivar un turno dado de baja')", true);
                            return;
                        }
                        Int64 cId = Int64.Parse(xpars.ToString());
                        this.CboGruas.SelectedValue = cId.ToString();

                        List<Crane_Turn> Lista_Turno = Crane_Turn.GetTurn(cId, out v_mensaje);
                        if (v_mensaje != string.Empty)
                        {
                            this.MessageBox(v_mensaje, this); 
                            return;
                        }

                        var xList = Lista_Turno.FirstOrDefault();
                        if (xList != null)
                        {
                            this.TxtFechaDesde.Text = xList.turn_time_end.HasValue ? xList.turn_time_end.Value.ToString("dd/MM/yyyy HH:mm") : "...";  
                        }
                    }
                   

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la eliminación de turnos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al eliminar turno", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }

            }
        }

        protected void Opciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                // Option rowOption = (Option)e.Item.DataItem;
                Label lbl = (Label)e.Item.FindControl("LblStatus");
                if (lbl.Text == "INACTIVO")
                {
                    lbl.ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("Lblopcname")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("Lblend")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("LblStart")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lblturno_nomber")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("LblIdOT")).ForeColor = System.Drawing.Color.Red;
                }
               
            }

           
        }

        #endregion


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

            //this.IsAllowAccess();

            var user = Page.Tracker();
            if (user != null)
            {
               // this.textbox1.Value = user.email != null ? user.email : string.Empty;
                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user.nombres, user.apellidos);
            }
            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {

                var sp = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
                string t = null;
                if (!string.IsNullOrEmpty(sp))
                {
                    t = CslHelper.getShiperName(sp);
                }
                //this.nomexpo.InnerText = t != null ? t : string.Format("{0} {1}", user.nombres, user.apellidos);
                //this.numexpo.InnerText = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
                //this.numexport.Value = string.IsNullOrEmpty(user.codigoempresa) ? user.ruc : user.codigoempresa;
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
                Limpiar();

                var oref = Request.QueryString["ID"] as string;
                if (!string.IsNullOrEmpty(oref))
                {

                    this.BtnBuscar.Enabled = false;
                    this.TxtReferencia.Enabled = false;
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
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "container", "ValidateJSON", "No token", "Sin tokenID");
                    return CslHelper.JsonNewResponse(false, true, "window.location='../cuenta/menu.aspx'", "Su formulario ha expirado, por favor reingrese de nuevo desde el menú");
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
                jmsg.resultado = jAisvContainer.ValidateAisvData(objeto, sUser.ruc, sUser.bloqueo_cartera, out mensaje);
                if (!jmsg.resultado)
                {
                    jmsg.mensaje = mensaje;
                    if (objeto.hasprof == "N")
                    {
                        jmsg.fluir = true;
                        jmsg.data = "window.open('../servicios/proforma','Proformas','width=1000,height=800,scrollbars=yes');";
                        jmsg.mensaje = string.Format("{0}\n Va a ser direccionado a la página de proformas", jmsg.mensaje);
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(jmsg);
                }
                //  System.Diagnostics.Trace.Write(string.Format("----->Fin de Validaciones:{0}",DateTime.Now));
                //transporte a N4



                objeto.nomexpo = CslHelper.getShiperName(objeto.idexport);

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


        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                //bool lTieneFecha = true;
                string cMensaje = "";
                string cReferencia = "";
                if (String.IsNullOrEmpty(this.TxtReferencia.Text) != false)
                {

                    ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alert('Ingrese la referencia de la nave a buscar')", true);
                    return ;
   
                }

                cReferencia = this.TxtReferencia.Text.Trim();

                objVesselV = new Vessel_Visit();

                List<Vessel> Lista = Vessel.ListaVessel(cReferencia);
                var xList = Lista.FirstOrDefault();

                if (xList != null)
                {
                   
                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objVesselV.ETA = xList.ETA;
                    objVesselV.ETD = xList.ETD;
                    objVesselV.NAME = xList.NAME;
                    objVesselV.VOYAGE_IN = xList.VOYAGE_IN;
                    objVesselV.VOYAGE_OUT = xList.VOYAGE_OUT;
                    objVesselV.ATA = xList.ATA;
                    objVesselV.ATD = xList.ATD;
                    objVesselV.Create_user = sUser.loginname;
                    objVesselV.Mod_user = String.Empty;
                    objVesselV.REFERENCE = xList.REFERENCE;
                    objVesselV.GKEY = xList.GKEY;
                    objVesselV.END_WORK = xList.END_WORK;
                    objVesselV.START_WORK = xList.START_WORK;

                    //salvar objeto
                    Session["VesselV"] = objVesselV;

                    this.LblNombre.Text = objVesselV.NAME;
                    //this.LblETA.Text = objVesselV.ETA.ToString();
                    //this.LblETD.Text = objVesselV.ETD.ToString();
                    this.LblETA.Text = objVesselV.ETA.HasValue ? objVesselV.ETA.Value.ToString("dd/MM/yyyy HH:mm") : "...";
                    this.LblETD.Text = objVesselV.ETD.HasValue ? objVesselV.ETD.Value.ToString("dd/MM/yyyy HH:mm") : "...";
                    this.LblViaje.Text = xList.VOYAGE.ToString();
                    this.LblVoyageIn.Text = objVesselV.VOYAGE_IN;
                    this.LblVoyageOut.Text = objVesselV.VOYAGE_OUT;

                    if (xList.ETD != null && xList.ETA != null)
                    {
                        var horas = (xList.ETD.Value - xList.ETA.Value).TotalHours;
                        this.LblHoras.Text = horas.ToString();
                    }
                    else { this.LblHoras.Text = "0"; }


                    //genera turnos
                    List<Crane_Turn> ListaTurnos = Crane_Turn.GetListTurnReference(cReferencia, out cMensaje);
                    if (cMensaje != string.Empty)
                    {
                        this.MessageBox(cMensaje, this);
                        TableTurnos.DataSource = null;
                        TableTurnos.DataBind();
                        return;
                    }
                    else
                    {

                        if (ListaTurnos != null && ListaTurnos.Count > 0)
                        {
                            foreach (Crane_Turn ListaNew in ListaTurnos)
                            {
                                objVesselTurno = new Crane_Turn();
                                objVesselTurno.id = ListaNew.id;
                                objVesselTurno.vessel_visit_id = ListaNew.vessel_visit_id;
                                objVesselTurno.vessel_crane_id = ListaNew.vessel_crane_id;
                                objVesselTurno.turno_number = ListaNew.turno_number;
                                objVesselTurno.task_id = ListaNew.task_id;
                                objVesselTurno.crane_id = ListaNew.crane_id;
                                objVesselTurno.crane_name = ListaNew.crane_name;

                                objVesselTurno.turn_time_start = ListaNew.turn_time_start;
                                objVesselTurno.turn_time_end = ListaNew.turn_time_end;
                                objVesselTurno.turn_time_meet = ListaNew.turn_time_start;

                                objVesselTurno.opc_id = ListaNew.opc_id;
                                objVesselTurno.opc_name = ListaNew.opc_name;
                                objVesselTurno.vlock = ListaNew.vlock;
                                objVesselTurno.vunlock = ListaNew.vunlock;
                                objVesselTurno.status = ListaNew.status;
                                objVesselTurno.t_status = ListaNew.t_status;

                                objVesselTurno.Create_user = sUser.loginname;
                                objVesselTurno.Mod_user = string.Empty;
                                objVesselV.Turns.Add(objVesselTurno);
                            }

                            //salvar objeto
                            Session["VesselV"] = objVesselV;

                            TableTurnos.DataSource = objVesselV.Turns;
                            TableTurnos.DataBind();

                            CargaTurnos(cReferencia);

                            //foreach (RepeaterItem item2 in TableTurnos.Items)
                            //{
                            //   Label lbl = (Label)item2.FindControl("LblStatus");
                            //   if (lbl.Text == "INACTIVO")
                            //        {
                            //            lbl.Attributes.Add("style", "ForeColor:Red;");
                            //        }
                            //}

                        }
                        else
                        {
                            TableTurnos.DataSource = null;
                            TableTurnos.DataBind();
                            CargaTurnos(cReferencia);

                            ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alert('No existe información de turnos para mostrar')", true);
                            return;
                           
                        }
                        
                    }
                    
                }
                else
                {

                    TableTurnos.DataSource = objVesselV.Turns;
                    TableTurnos.DataBind();

                    CargaTurnos(cReferencia);

                    this.LblNombre.Text = null;
                    this.LblETA.Text = null;
                    this.LblETD.Text = null;
                    this.LblViaje.Text = null;
                    this.LblVoyageIn.Text = null;
                    this.LblVoyageOut.Text = null;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alert('No existe información de nave con los criterios de búsqueda ingresados')", true);
                    return;
                   
                }
             

            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }

           
        }

        protected void CboGruas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

 
        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

            try
            {
                //recuperar objeto
                objVesselV = Session["VesselV"] as Vessel_Visit;

                DateTime dfechaDesde;

                if (objVesselV == null)
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La sesión no existe"), "container", "ValidateJSON", "Sesión no existe", "No disponible");
                }
                else
                {
                    //si existen elementos ddel combo
                    if (this.CboGruas.SelectedIndex == -1)
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alertify.alert('Debe seleccionar el turno a dar de baja')", true);
                        return;
                      
                    }

                    if (String.IsNullOrEmpty(this.TxtFechaDesde.Text) != false)
                    {
                        this.TxtFechaDesde.Focus();
                        ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alertify.alert('Ingrese una Fecha de finalización del turno valida')", true);
                        return;
                      
                    }
                    if (objVesselV.Turns.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alertify.alert('Aún no ha agregado el detalle del turno a finalizar')", true);
                        return;
                      
                    }

                    Int64 nId = Int64.Parse(this.CboGruas.SelectedValue);
                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    dfechaDesde = DateTime.Parse(this.TxtFechaDesde.Text);

                    objVesselTurno.id = nId;
                    objVesselTurno.Mod_user = sUser.loginname;
                   
                    objVesselTurno.turn_time_meet = dfechaDesde;

                    List<Crane_Turn> Lista_Turno = Crane_Turn.GetTurn(nId, out v_mensaje);
                    if (v_mensaje != string.Empty)
                    {
                        this.MessageBox(v_mensaje, this);
                        return;
                    }

                    var xList = Lista_Turno.FirstOrDefault();
                    if (xList != null)
                    {
                        objVesselTurno.turn_time_end = xList.turn_time_end;   
                    }

                    objVesselTurno.LiberarTurnoCuadrilla(out v_mensaje);
                    if (v_mensaje != string.Empty)
                    {
                        //this.MessageBox(v_mensaje.ToString(), this);
                        string mensaje = @"alert('" + v_mensaje.ToString() + "')";

                        ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", mensaje, true);

                    }
                    else
                    {
                        Session["VesselV"] = null;
                       
                        this.TxtFechaDesde.Text = null;
                        ScriptManager.RegisterStartupScript(this, typeof(string), "mensaje", "alertify.alert('Se Libero el turno con éxito')", true);
                        BtnBuscar_Click(sender, e);

                 
                    }

                }

            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
      
        }

    

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.Limpiar();

        }
    }
}