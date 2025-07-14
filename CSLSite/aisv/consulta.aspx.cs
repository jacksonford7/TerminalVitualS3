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
using VBSEntidades;
using VBSEntidades.ClaseEntidades;
using System.Web.Script.Serialization;
using VBSEntidades.Calendario;
using System.Web.Services;
using Newtonsoft.Json;
using BillionEntidades;

namespace CSLSite
{
    public partial class consulta : System.Web.UI.Page
    {
        private string _Id_Opcion_Servicio = string.Empty;
        private List<Cls_Bil_Turnos> List_Turnos { set; get; }
        private static Int64? lm = -3;
        private string OError;

        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            this.IsAllowAccess();
            Page.Tracker();
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();

                //para que puedar dar click en el titulo de la pantalla y regresar al menu principal de la zona
                //_Id_Opcion_Servicio = Request.QueryString["opcion"];
                //this.opcion_principal.InnerHtml = string.Format("<a href=\"../cuenta/subopciones.aspx?opcion={0}\">{1}</a>", _Id_Opcion_Servicio, "AISV"); ;

            }
            this.booking.Text = Server.HtmlEncode(this.booking.Text);
            this.aisvn.Text = Server.HtmlEncode(this.aisvn.Text);
            this.cntrn.Text = Server.HtmlEncode(this.cntrn.Text);
            this.docnum.Text = Server.HtmlEncode(this.docnum.Text);
            this.desded.Text = Server.HtmlEncode(this.desded.Text);
            this.desded.Text = Server.HtmlEncode(this.hastad.Text);


            this.banmsg2.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {

             
                this.banmsg2.InnerText = string.Empty;
                this.TxtFechaTurno.Text = string.Empty;
                this.TxtHoraInicio.Text = string.Empty;
                this.TxtHoraFin.Text = string.Empty;
                this.TxtReferencia.Text = string.Empty;
                this.CboDestino.SelectedIndex = -1;
                this.TxtUnidades.Text = string.Empty;
                this.manualHideReferencia.Value = string.Empty;

                this.CargarTurnos();

            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                xfinder.Visible = IsPostBack;
                sinresultado.Visible = false;
                this.banmsg2.Visible = false;
            }
        }

        #region "turnos banano"
        private void Actualiza_Paneles()
        {
            UPACTUALIZAR.Update();
            UPFECHASALIDA.Update();
            UPTURNO.Update();
            UPBOTONES.Update();



        }


        private void Ocultar_Mensaje()
        {

            this.banmsg2.InnerText = string.Empty;          
            this.banmsg2.Visible = false;
          
            this.Actualiza_Paneles();
          
        }

        private void CargarTurnos()
        {
            try
            {
                string cMensajes;

                List<BTS_Horarios_Aisv> Listado = BTS_Horarios_Aisv.CboLineas(out cMensajes);


             
                this.Cbohora.DataSource = Listado;
                this.Cbohora.DataTextField = "hora";
                this.Cbohora.DataValueField = "value";
                this.Cbohora.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(CargarTurnos), "consulta.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            this.banmsg2.Visible = true;
            this.banmsg2.InnerHtml = Mensaje;
          
            this.Actualiza_Paneles();
        }

        private void Turno_Default()
        {
            //turno por defecto
            List_Turnos = new List<Cls_Bil_Turnos>();
            List_Turnos.Clear();
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan ="0", Turno = "* Seleccione *" });
            this.CboTurnos.DataSource = List_Turnos;
            this.CboTurnos.DataTextField = "Turno";
            this.CboTurnos.DataValueField = "IdPlan";
            this.CboTurnos.DataBind();
        }

        protected void Cbohora_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    CultureInfo enUS = new CultureInfo("en-US");

                    this.Ocultar_Mensaje();

                    List_Turnos = new List<Cls_Bil_Turnos>();
                    List_Turnos.Clear();

                    if (string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        this.Turno_Default();
                        this.Actualiza_Paneles();
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    string new_fecha = string.Format("{0} {1}", TxtFechaHasta.Text, this.Cbohora.SelectedValue);

                    DateTime desde;
                    if (!DateTime.TryParseExact(new_fecha, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out desde))
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la actualización del turno.. día/mes/año HH:mm</b>"));
                        this.Cbohora.Focus();
                        return;
                    }

                    if (desde.Date < System.DateTime.Now.Date)
                    {
                        this.Turno_Default();
                        this.TxtFechaHasta.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha y hora de salida: {0}, no puede ser menor que la fecha actual: {1} </b>", desde.ToString("dd/MM/yyyy HH:mm"), System.DateTime.Now.ToString("dd/MM/yyyy HH:mm")));
                        this.Cbohora.Focus();
                        return;
                    }

                    int Cantidad = 0;
                    if (!int.TryParse(this.TxtUnidades.Text, out Cantidad))
                    {
                        Cantidad = 0;
                    }

                    string cMensajes;

                    List<Cls_CFS_Turnos_Banano> Listado = Cls_CFS_Turnos_Banano.Carga_Turnos_Remanente(this.TxtReferencia.Text.ToString().Trim(), ClsUsuario.ruc.Trim(), desde, Cantidad, out cMensajes);

                    if (Listado != null)
                    {
                        List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}", "0"), Turno = "* Seleccione *" });

                        if (Listado.Count != 0)
                        {
                            foreach (var Det in Listado)
                            {
                                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Det.idLoadingDet_remanente.ToString(), Turno = string.Format("{0} - {1}  |   Stock {2}: {3}", Det.horaInicio, Det.horaFin, Det.bodega, Det.saldo_nuevo.ToString("N2")) });


                            }
                        }
                        else
                        {
                            this.CboTurnos.DataSource = List_Turnos;
                            this.CboTurnos.DataTextField = "Turno";
                            this.CboTurnos.DataValueField = "IdPlan";
                            this.CboTurnos.DataBind();

                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", new_fecha, cMensajes));
                            return;
                        }



                        this.CboTurnos.DataSource = List_Turnos;
                        this.CboTurnos.DataTextField = "Turno";
                        this.CboTurnos.DataValueField = "IdPlan";
                        this.CboTurnos.DataBind();

                    }
                    else
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", new_fecha, cMensajes));
                        return;
                    }

                    this.CboTurnos.Focus();
                    this.Actualiza_Paneles();
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();

                    banmsg2.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Cbohora_SelectedIndexChanged", "Hubo un error al buscar", t.loginname));
                    banmsg2.Visible = true;
                }
            }
        }

        protected void TxtFechaHasta_TextChanged(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    CultureInfo enUS = new CultureInfo("en-US");

                    this.Ocultar_Mensaje();

                    List_Turnos = new List<Cls_Bil_Turnos>();
                    List_Turnos.Clear();

                    if (string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        this.Turno_Default();
                        this.Actualiza_Paneles();
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    string new_fecha = string.Format("{0} {1}", TxtFechaHasta.Text, this.Cbohora.SelectedValue);

                    DateTime desde;
                    if (!DateTime.TryParseExact(new_fecha, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out desde))
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la actualización del turno.. día/mes/año HH:mm</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (desde.Date < System.DateTime.Now.Date)
                    {
                        this.Turno_Default();
                        this.TxtFechaHasta.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha y hora de salida: {0}, no puede ser menor que la fecha actual: {1} </b>", desde.ToString("dd/MM/yyyy HH:mm"), System.DateTime.Now.ToString("dd/MM/yyyy HH:mm")));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    int Cantidad = 0;
                    if (!int.TryParse(this.TxtUnidades.Text, out Cantidad))
                    {
                        Cantidad = 0;
                    }

                    string cMensajes;

                    List<Cls_CFS_Turnos_Banano> Listado = Cls_CFS_Turnos_Banano.Carga_Turnos_Remanente(this.TxtReferencia.Text.ToString().Trim(), ClsUsuario.ruc.Trim(), desde, Cantidad, out cMensajes);

                    if (Listado != null)
                    {
                        List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}", "0"), Turno = "* Seleccione *" });

                        if (Listado.Count != 0)
                        {
                            foreach (var Det in Listado)
                            {
                                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Det.idLoadingDet_remanente.ToString(), Turno = string.Format("{0} - {1}  |   Stock {2}: {3}", Det.horaInicio, Det.horaFin, Det.bodega, Det.saldo_nuevo.ToString("N2")) });


                            }
                        }
                        else
                        {
                            this.CboTurnos.DataSource = List_Turnos;
                            this.CboTurnos.DataTextField = "Turno";
                            this.CboTurnos.DataValueField = "IdPlan";
                            this.CboTurnos.DataBind();

                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", new_fecha, cMensajes));
                            return;
                        }
                        


                        this.CboTurnos.DataSource = List_Turnos;
                        this.CboTurnos.DataTextField = "Turno";
                        this.CboTurnos.DataValueField = "IdPlan";
                        this.CboTurnos.DataBind();

                    }
                    else
                    {
                        this.Turno_Default();
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", new_fecha, cMensajes));
                        return;
                    }

                    this.CboTurnos.Focus();
                    this.Actualiza_Paneles();
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();

                    banmsg2.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "TxtFechaHasta_TextChanged", "Hubo un error al buscar", t.loginname));
                    banmsg2.Visible = true;
                }
            }
        }

        protected void BtnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }
                }

                this.BtnActualizaTurno.Attributes["disabled"] = "disabled";
                this.Actualiza_Paneles();

                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechaConvertida;

                string new_fecha = string.Format("{0} {1}", TxtFechaHasta.Text, Cbohora.SelectedValue);

                if (!DateTime.TryParseExact(new_fecha, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechaConvertida))
                {
                    this.BtnActualizaTurno.Attributes.Remove("disabled");
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la actualización del turno.. día/mes/año</b>"));
                    this.TxtFechaHasta.Focus();
                }

                if (this.CboTurnos.SelectedIndex == 0)
                {
                    this.BtnActualizaTurno.Attributes.Remove("disabled");
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar un turno para poder actualizar la AISV</b>"));
                    this.CboTurnos.Focus();
                    return;
                }

                //actualizar turnos
                string cMensajes;
                int box = 0;
                Cls_CFS_Turnos_Banano ObjHorario = new Cls_CFS_Turnos_Banano();
                //ObjHorario.idLoadingDet = Int64.Parse(this.CboTurnos.SelectedValue.ToString());
                ObjHorario.idLoadingDet_remanente = this.CboTurnos.SelectedValue.ToString();
                if (!ObjHorario.PopulateMyData_Remante(out cMensajes))
                {
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! No puede consultar los horarios de turnos de la AISV {0}</b>", cMensajes));

                    return;
                }
                else
                {
                    box = ObjHorario.box;
                }

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());


                Cls_CFS_Turnos_Banano objActualizar = new Cls_CFS_Turnos_Banano();
                objActualizar.aisv_codigo = this.TxtNewAisv.Text;
                objActualizar.vbs_fecha_cita = fechaConvertida;
                objActualizar.idLoadingDet_remanente = this.CboTurnos.SelectedValue.ToString();
                objActualizar.vbs_destino = int.Parse(this.CboDestino.SelectedValue.ToString());
                objActualizar.box = box;
                objActualizar.aisvUsuarioCrea = ClsUsuario.loginname.Trim();

               
                var nProceso = objActualizar.SaveTransaction_AISV_Remanente(out cMensajes);

                if (!nProceso.HasValue || nProceso.Value <= 0)
                {

                    this.UPBOTONES.Update();

                    this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo actualizar los turnos de la AISV de carga suelta..{0}</b>", cMensajes));

                    return;
                }
                else
                {
                    int dy = 0;
                    if (int.TryParse(this.TxtUnidades.Text, out dy))
                    {

                    }

                    //graba tablas nuevas de vbs
                    Cls_CFS_Turnos_Banano objActualizarVBS = new Cls_CFS_Turnos_Banano();
                    objActualizarVBS.idLoadingDet_remanente = this.CboTurnos.SelectedValue.ToString(); ;
                    objActualizarVBS.aisv_codigo = this.TxtNewAisv.Text;
                    objActualizarVBS.box = dy;
                    objActualizarVBS.aisvUsuarioCrea = ClsUsuario.loginname.Trim();

                    string xerror;
                    var nProcesoVBS = objActualizarVBS.SaveTransaction_VBS_Modifica_remanente(out xerror);
                    /*fin de nuevo proceso de grabado*/
                    if (!nProcesoVBS.HasValue || nProcesoVBS.Value <= 0)
                    {
                        this.UPBOTONES.Update();

                        this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo actualizar los turnos/VBS de la AISV de carga suelta..{0}</b>", cMensajes));

                        return;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1,string.Format("<b>Se procedió con la actualización de turnos con éxito, de la AISV # {0}  {1} ", this.TxtNewAisv.Text, ",Puede proceder a imprimir la misma."));

                        this.Actualiza_Paneles();
                    }
                }

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAprobar_Click), "consulta.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }


        #endregion

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
                        Session.Clear();
                        return;
                    }
                    populate();

                  
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

        public static string anulado(object estado)
        {
            if (estado == null)
            {
                return "<span>sin estado!</span>";
            }
            if (estado.ToString().ToLower() == "r")
            {
                return "<span>Registrado</span>";
            }
            if (estado.ToString().ToLower() == "a")
            {
                return "<span class='red' >Anulado</span>";
            }
            if (estado.ToString().ToLower() == "i")
            {
                return "<span class='azul' >Ingresado</span>";
            }
            if (estado.ToString().ToLower() == "s")
            {
                return "<span class='naranja' >Salida</span>";
            }
            return "<span>sin estado!</span>";
        }

        public static string boton(object estado)
        {
            return estado.ToString().ToLower() == "r" ? "ver" : "xver";
        }
        public static string tipos(object tipo, object movi)
        {
            if (tipo == null || movi == null)
            {
                return "!error";
            }

            if (tipo.ToString().Trim().Length < 1 || movi.ToString().Trim().Length < 1)
            {
                return "!error";
            }

            if (movi.ToString().Trim() == "E")
            {
                if (tipo.ToString().Trim() == "C")
                {
                    return "Full";
                }
                else
                {
                    return "C. Suelta";
                }
            }
            else
            {
                return "Consolidación";
            }
        }
        public static string securetext(object number)
        {
            if (number == null || number.ToString().Length <= 2)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        public static string mostrarcontenedor(object estado)
        {
            return estado.ToString().ToLower() == "c" ? "visibility:visible" : "visibility:hidden";
        }

        public static string mostrarbanano(object estado)
        {
            return estado.ToString().ToLower() == "s" ? "visibility:visible" : "visibility:hidden";
        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                if (e.CommandName == "Anular")
                {
                    try
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            Session.Clear();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            return;
                        }

                        var user = Page.getUserBySesion();
                        if (user == null)
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                            sinresultado.Visible = true;
                            return;
                        }
                        if (e.CommandArgument == null)
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                            sinresultado.Visible = true;
                            return;
                        }

                        var xpars = e.CommandArgument.ToString().Split(';');
                        if (xpars.Length <= 0 || xpars.Length < 5)
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "tablePagination_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                            sinresultado.Visible = true;
                            return;
                        }

                        if (string.IsNullOrEmpty(xpars[5]) || xpars[5].ToLower() != "r")
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = "No se puede anular este AISV ya que su estado ha cambiado, es posible que la carga ya se encuentre patios.";
                            sinresultado.Visible = true;
                            return;
                        }

                        if (jAisvContainer.UnidadEstado(xpars[2]) > 1)
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("No puede anular el AISV {0} ya que la unidad {1}, se encuentra en patios", xpars[0], xpars[2]);
                            sinresultado.Visible = true;
                            return;
                        }
                        string vt = string.Empty;
                        //cancel advice si es unidad
                        if (!string.IsNullOrEmpty(xpars[2]))
                        {
                            if (xpars[2] == "null")
                            {
                                sinresultado.Attributes["class"] = string.Empty;
                                sinresultado.Attributes["class"] = "msg-critico";
                                sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal y no se encontró el contenedor. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El registro era de contenedor lleno pero no se encontró la unidad"), "consulta", "tablePagination_ItemCommand", "Contenedor fué nulo", user.loginname));
                                sinresultado.Visible = true;
                                return;
                            }

                            //Validacion 2 -> Obtener la sesión y covetirla a objectsesion
                            var userk = new ObjectSesion();
                            userk.clase = "consulta"; userk.metodo = "tablePagination_ItemCommand";
                            userk.transaccion = "AISV Consulta"; userk.usuario = user.loginname;

                            //aqui era el error.
                            userk.token = HttpContext.Current.Request.Cookies["token"].Value;


                            var referencia_acopio = System.Web.Configuration.WebConfigurationManager.AppSettings["acopio"];
                            if (string.IsNullOrEmpty(referencia_acopio))
                            {
                                referencia_acopio = "CGS2008001";
                            }

                            //SI ES CONTENEDOR O ES CONSOLIDACION
                            if (xpars[3].ToUpper().Contains("C") || xpars[4].ToUpper().Contains("C") || xpars[1].Contains(referencia_acopio))
                            {
                                //si hay unidad o contenedor.
                                if (!string.IsNullOrEmpty(xpars[2]))
                                {
                                    if (jAisvContainer.ConfirmacionPreaviso(xpars[2].Trim()))
                                    {
                                        if (!jAisvContainer.cancelAdvice(userk, xpars[2], xpars[1], out vt))
                                        {
                                            sinresultado.Attributes["class"] = string.Empty;
                                            sinresultado.Attributes["class"] = "msg-critico";
                                            sinresultado.InnerText = string.Format("No se pudo eliminar el registro por la siguiente causa:[{1}], algo salió mal. Código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException(vt), "CancelAdvice", "tablePagination_ItemCommand", vt, user.loginname), vt);
                                            sinresultado.Visible = true;
                                            return;
                                        }
                                    }
                                }
                            }

                        }

                        if (!jAisvContainer.delete(xpars[0], user.loginname, out vt))
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = vt;
                            sinresultado.Visible = true;
                            return;
                        }





                        //nuevo bloque si el aisv cambió por concurrencia, entonces no se puede anular
                        if (!string.IsNullOrEmpty(vt))
                        {
                            var negativo = 2;
                            if (!int.TryParse(vt, out negativo) || negativo < 0)
                            {
                                sinresultado.Attributes["class"] = string.Empty;
                                sinresultado.Attributes["class"] = "msg-critico";
                                sinresultado.InnerText = "No fúe posible anular este documento, es probable que la carga ya este ingresada o que el transporte ya este fuera de la terminal, confirme con planificación"; ;
                                sinresultado.Visible = true;
                                populate();
                                return;
                            }

                        }

                        //proceso para restaurar el saldo
                        if (!string.IsNullOrEmpty(xpars[3]))
                        {
                            if (xpars[3] == "S" && xpars[7] == "1")//--> MUELLE 
                            {

                                Int64 idLoadingDet = 0;
                                if (Int64.TryParse(xpars[6], out idLoadingDet))
                                {

                                }

                                int box = 0;
                                if (int.TryParse(xpars[8], out box))
                                {

                                }

                                //graba tablas nuevas de vbs
                                Cls_CFS_Turnos_Banano objActualizarVBS = new Cls_CFS_Turnos_Banano();
                                objActualizarVBS.idLoadingDet = idLoadingDet;
                                objActualizarVBS.aisv_codigo = xpars[0];
                                objActualizarVBS.box = box;
                                objActualizarVBS.aisvUsuarioCrea = user.loginname.Trim();

                                string xerror;
                                var nProcesoVBS = objActualizarVBS.SaveTransaction_CancelaVBS(out xerror);
                                /*fin de nuevo proceso de grabado*/
                                if (!nProcesoVBS.HasValue || nProcesoVBS.Value <= 0)
                                {
                                    this.UPBOTONES.Update();

                                    this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo cancelar los turnos/VBS de la AISV de carga suelta..{0}</b>", xerror));

                                    return;
                                }
                                else
                                {
                                   
                                    this.Actualiza_Paneles();
                                }
                            }

                            if (xpars[3] == "S" && xpars[7] == "2")//--> BODEGA
                            {

                                Int64 idStowagePlanAisv = 0;
                                if (Int64.TryParse(xpars[6], out idStowagePlanAisv))
                                {

                                }

                                int box = 0;
                                if (int.TryParse(xpars[8], out box))
                                {

                                }

                                //graba tablas nuevas de vbs
                                Cls_CFS_Turnos_Banano objActualizarVBS = new Cls_CFS_Turnos_Banano();
                                objActualizarVBS.idStowagePlanTurno = idStowagePlanAisv;
                                objActualizarVBS.aisv_codigo = xpars[0];
                                objActualizarVBS.box = box;
                                objActualizarVBS.aisvUsuarioCrea = user.loginname.Trim();

                                string xerror;
                                var nProcesoVBS = objActualizarVBS.SaveTransaction_CancelaVBSBanano(out xerror);
                                /*fin de nuevo proceso de grabado*/
                                if (!nProcesoVBS.HasValue || nProcesoVBS.Value <= 0)
                                {
                                    this.UPBOTONES.Update();

                                    this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo cancelar los turnos/VBS de la AISV de carga suelta..{0}</b>", xerror));

                                    return;
                                }
                                else
                                {

                                    this.Actualiza_Paneles();
                                }
                            }
                        }

                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        sinresultado.InnerText = string.Format("La anulación del AISV  No.{0} ha resultado exitosa.", xpars[0]);
                        sinresultado.Visible = true;
                        populate();
                    }
                    catch (Exception ex)
                    {
                        var t = this.getUserBySesion();
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la anulación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al anular", t.loginname));
                        sinresultado.Visible = true;

                    }
                }
                if (e.CommandName == "actualizar_cita")
                {
                    try
                    {
                        if (HttpContext.Current.Request.Cookies["token"] == null)
                        {
                            System.Web.Security.FormsAuthentication.SignOut();
                            Session.Clear();
                            System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                            return;
                        }
                        var user = Page.getUserBySesion();
                        if (user == null)
                        {
                          
                            banmsg2.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                            banmsg2.Visible = true;
                            return;
                        }
                        if (e.CommandArgument == null)
                        {
                           
                            banmsg2.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                            banmsg2.Visible = true;
                            return;
                        }

                        var xpars = e.CommandArgument.ToString();
                        if (xpars.Length <= 0)
                        {
                           
                            banmsg2.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "tablePagination_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                            banmsg2.Visible = true;
                            return;
                        }
                        else
                        {
                            string cMensajes;
                            Cls_CFS_Turnos_Banano ObjTurnos = new Cls_CFS_Turnos_Banano();
                            ObjTurnos.aisv_codigo = xpars;

                            if (!ObjTurnos.PopulateMyData_Aisv(out cMensajes))
                            {
                               
                                banmsg2.InnerText = string.Format("No puede consultar los turnos de la AISV {0}.", cMensajes);
                                banmsg2.Visible = true;
                                return;
                            }
                            else
                            {

                                this.TxtNewAisv.Text = xpars;

                                if (ObjTurnos.vbs_destino.HasValue)
                                {
                                    this.CboDestino.SelectedValue = ObjTurnos.vbs_destino.Value.ToString();
                                }
                                else
                                {
                                    this.CboDestino.SelectedIndex=1;
                                }

                                this.UPACTUALIZAR.Update();

                                if (ObjTurnos.vbs_fecha_cita.HasValue)
                                {
                                    this.TxtFechaTurno.Text = ObjTurnos.vbs_fecha_cita.Value.ToString("dd/MM/yyyy");
                                }

                                if (ObjTurnos.vbs_id_hora_cita.HasValue && ObjTurnos.vbs_destino == 1)
                                {
                                    Cls_CFS_Turnos_Banano ObjHorario = new Cls_CFS_Turnos_Banano();
                                    ObjHorario.idLoadingDet = ObjTurnos.vbs_id_hora_cita.Value;

                                    if (!ObjHorario.PopulateMyData(out cMensajes))
                                    {
                                        banmsg2.InnerText = string.Format("No puede consultar los horarios de turnos de la AISV {0}.", cMensajes);
                                        banmsg2.Visible = true;
                                        return;
                                    }
                                    else
                                    {
                                        this.TxtHoraInicio.Text = ObjHorario.horaInicio;
                                        this.TxtHoraFin.Text = ObjHorario.horaFin;
                                        this.TxtReferencia.Text = ObjHorario.idNave;
                                        this.TxtUnidades.Text = ObjTurnos.aisv_cant_bult.ToString();
                                        this.manualHideReferencia.Value = ObjHorario.idNave;
                                        this.TxtFechaHasta.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                                        this.Cbohora.SelectedValue = ObjHorario.horaFin;
                                        //carga turnos
                                        CultureInfo enUS = new CultureInfo("en-US");

                                        this.Ocultar_Mensaje();

                                        List_Turnos = new List<Cls_Bil_Turnos>();
                                        List_Turnos.Clear();

                                        if (string.IsNullOrEmpty(TxtFechaHasta.Text))
                                        {
                                            this.Turno_Default();
                                            this.Actualiza_Paneles();
                                            return;
                                        }

                                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                                        string new_fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text , ObjHorario.horaInicio);

                                        DateTime desde;
                                        if (!DateTime.TryParseExact(new_fecha, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out desde))
                                        {
                                            this.Turno_Default();
                                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la actualización del turno.. día/mes/año</b>"));
                                            this.TxtFechaHasta.Focus();
                                            return;
                                        }

                                        if (desde.Date < System.DateTime.Now.Date)
                                        {
                                            this.Turno_Default();
                                            this.TxtFechaHasta.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha y hora de salida: {0}, no puede ser menor que la fecha actual: {1} </b>", desde.ToString("dd/MM/yyyy HH:mm"), System.DateTime.Now.ToString("dd/MM/yyyy HH:mm")));
                                            this.TxtFechaHasta.Focus();
                                            return;
                                        }

                                        int Cantidad = 0;
                                        if (!int.TryParse(this.TxtUnidades.Text, out Cantidad))
                                        {
                                            Cantidad = 0;
                                        }

                                       
                                        List<Cls_CFS_Turnos_Banano> Listado = Cls_CFS_Turnos_Banano.Carga_Turnos_Remanente(this.TxtReferencia.Text.ToString().Trim(), ClsUsuario.ruc.Trim(), desde, Cantidad, out cMensajes);

                                        if (Listado != null)
                                        {
                                            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}", "0"), Turno = "* Seleccione *" });

                                            foreach (var Det in Listado)
                                            {
                                               // List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Det.idLoadingDet.ToString(), Turno = string.Format("{0} | {1} - {2}  |   Stock: {3}", Det.bodega, Det.horaInicio, Det.horaFin, Det.box) });

                                                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Det.idLoadingDet_remanente.ToString(), Turno = string.Format("{0} - {1}  |   Stock {2}: {3}", Det.horaInicio, Det.horaFin, Det.bodega, Det.saldo_nuevo.ToString("N2")) });

                                            }

                                            this.CboTurnos.DataSource = List_Turnos;
                                            this.CboTurnos.DataTextField = "Turno";
                                            this.CboTurnos.DataValueField = "IdPlan";
                                            this.CboTurnos.DataBind();

                                        }
                                        else
                                        {
                                            this.Turno_Default();
                                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", this.TxtFechaHasta.Text, cMensajes));
                                            return;
                                        }

                                        this.CboTurnos.Focus();
                                        this.Actualiza_Paneles();
                                    }
                                }

                                if (ObjTurnos.vbs_id_hora_cita.HasValue && ObjTurnos.vbs_destino == 2)
                                {
                                    Cls_CFS_Turnos_Banano ObjHorario = new Cls_CFS_Turnos_Banano();
                                    ObjHorario.idStowagePlanTurno = ObjTurnos.vbs_id_hora_cita.Value;

                                    if (!ObjHorario.PopulateMyDataBodega(out cMensajes))
                                    {
                                        banmsg2.InnerText = string.Format("No puede consultar los horarios de turnos de la AISV {0}.", cMensajes);
                                        banmsg2.Visible = true;
                                        return;
                                    }
                                    else
                                    {
                                        this.TxtHoraInicio.Text = ObjHorario.horaInicio;
                                        this.TxtHoraFin.Text = ObjHorario.horaFin;
                                        this.TxtReferencia.Text = ObjHorario.idNave;
                                        this.TxtUnidades.Text = ObjTurnos.aisv_cant_bult.ToString();
                                        this.manualHideReferencia.Value = ObjHorario.idNave;
                                        this.TxtFechaHasta.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

                                        //carga turnos
                                        CultureInfo enUS = new CultureInfo("en-US");

                                        this.Ocultar_Mensaje();

                                        List_Turnos = new List<Cls_Bil_Turnos>();
                                        List_Turnos.Clear();

                                        if (string.IsNullOrEmpty(TxtFechaHasta.Text))
                                        {
                                            this.Turno_Default();
                                            this.Actualiza_Paneles();
                                            return;
                                        }

                                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                                        DateTime desde;
                                        if (!DateTime.TryParseExact(TxtFechaHasta.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                                        {
                                            this.Turno_Default();
                                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la actualización del turno.. día/mes/año</b>"));
                                            this.TxtFechaHasta.Focus();
                                            return;
                                        }

                                        if (desde.Date < System.DateTime.Now.Date)
                                        {
                                            this.Turno_Default();
                                            this.TxtFechaHasta.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser menor que la fecha actual: {1} </b>", desde.ToString("dd/MM/yyyy"), System.DateTime.Now.ToString("dd/MM/yyyy")));
                                            this.TxtFechaHasta.Focus();
                                            return;
                                        }

                                        int Cantidad = 0;
                                        if (!int.TryParse(this.TxtUnidades.Text, out Cantidad))
                                        {
                                            Cantidad = 0;
                                        }


                                        List<Cls_CFS_Turnos_Banano> Listado = Cls_CFS_Turnos_Banano.Carga_Turnos_Bodega(this.TxtReferencia.Text.ToString().Trim(), ClsUsuario.ruc.Trim(), desde, Cantidad, out cMensajes);

                                        if (Listado != null)
                                        {
                                            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}", "0"), Turno = "* Seleccione *" });

                                            foreach (var Det in Listado)
                                            {
                                                // List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Det.idLoadingDet.ToString(), Turno = string.Format("{0} | {1} - {2}  |   Stock: {3}", Det.bodega, Det.horaInicio, Det.horaFin, Det.box) });

                                                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Det.idStowagePlanTurno.ToString(), Turno = string.Format("{0} - {1}  |   Stock {2}: {3}", Det.horaInicio, Det.horaFin, Det.bodega, Det.box.ToString("N2")) });

                                            }

                                            this.CboTurnos.DataSource = List_Turnos;
                                            this.CboTurnos.DataTextField = "Turno";
                                            this.CboTurnos.DataValueField = "IdPlan";
                                            this.CboTurnos.DataBind();

                                        }
                                        else
                                        {
                                            this.Turno_Default();
                                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para la fecha seleccionada fecha: {0}, mensaje: {1} </b>", this.TxtFechaHasta.Text, cMensajes));
                                            return;
                                        }

                                        this.CboTurnos.Focus();
                                        this.Actualiza_Paneles();
                                    }
                                }

                                if (this.CboDestino.SelectedIndex == 1)
                                {
                                    this.BtnActualizaTurno.Attributes["disabled"] = "disabled";
                                    this.TxtFechaHasta.Attributes["disabled"] = "disabled";
                                    this.CboTurnos.Attributes["disabled"] = "disabled";

                                    this.Actualiza_Paneles();
                                }
                                else
                                {
                                    this.BtnActualizaTurno.Attributes.Remove("disabled");
                                    this.TxtFechaHasta.Attributes.Remove("disabled");
                                    this.CboTurnos.Attributes.Remove("disabled");
                                    this.Actualiza_Paneles();
                                }

                                if (ObjTurnos.estado.Equals("A"))
                                {
                                    this.BtnActualizaTurno.Attributes["disabled"] = "disabled";
                                    this.TxtFechaHasta.Attributes["disabled"] = "disabled";
                                    this.CboTurnos.Attributes["disabled"] = "disabled";

                                    this.Actualiza_Paneles();
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        var t = this.getUserBySesion();
                      
                        banmsg2.InnerText = string.Format("Ha ocurrido un problema durante la consulta de turno, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al anular", t.loginname));
                        banmsg2.Visible = true;

                    }
                }

            }
        }

        protected string jsarguments(object aisv, object referencia, object unidad, object movimiento, object tipo, object estado)
        {
            return string.Format("{0};{1};{2};{3};{4};{5}", aisv != null ? aisv.ToString().Trim() : "0", referencia != null ? referencia.ToString().Trim() : "na", unidad != null ? unidad.ToString().Trim() : "null", movimiento != null ? movimiento.ToString().Trim() : "x", tipo != null ? tipo.ToString().Trim() : "x", estado);
        }

        protected string jsarguments_cancela(object aisv, object referencia, object unidad, object movimiento, object tipo, object estado, object vbs_id_hora_cita, object vbs_destino, object aisv_cant_bult)
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", aisv != null ? aisv.ToString().Trim() : "0", referencia != null ? referencia.ToString().Trim() : "na", unidad != null ? unidad.ToString().Trim() : "null", movimiento != null ? movimiento.ToString().Trim() : "x", tipo != null ? tipo.ToString().Trim() : "x", estado, vbs_id_hora_cita != null ? vbs_id_hora_cita.ToString().Trim() : "0",
                 vbs_destino != null ? vbs_destino.ToString().Trim() : "0", aisv_cant_bult != null ? aisv_cant_bult.ToString().Trim() : "0");
        }
        public static string set_view()
        {
            var cfgs = HttpContext.Current.Session["parametros"] as List<dbconfig>;
            var cf = cfgs.Where(f => f.config_name.Contains("val_camara")).FirstOrDefault();
            if (cf == null || string.IsNullOrEmpty(cf.config_value) || cf.config_value.Contains("0"))
            {
                return "nover";
            }
            return "";
        }


        public static string refrigeracion(object estado, object temperatura)
        {
            //0.00
            //nuevo si activan la configuracion entonces fluye
            if (estado == null)
            {
                return "<span style='color:Blue;' >No disponible</span>";
            }
            decimal de;
            if (decimal.TryParse(temperatura.ToString(), out de))
            {
                if (de == 0)
                {
                    return "<span style='color:Black;font-weight:bold;'>No requerido</span>";
                }
            }
            if (!estado.ToString().Equals("0"))
            {
                return "<span style='color:Green;font-weight:bold;'>Activo</span>";
            }

            return "<span style='color:Red;font-weight:bold;' >Inactivo</span>";
        }


        private void populate()
        {
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
            Session["resultado"] = null;
            var table = new Catalogos.Listar_AISVDataTable();
            var ta = new CatalogosTableAdapters.Listar_AISVTableAdapter();
            try
            {
                DateTime desde;
                DateTime hasta;
                if (!DateTime.TryParseExact(desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
                    sinresultado.Visible = true;
                    return;
                }
                if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
                    sinresultado.Visible = true;
                    return;
                }
                if (desde.Year != hasta.Year)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-alerta";
                    this.sinresultado.InnerText = "El rango máximo de consulta es de 1 año, gracias por entender.";
                    sinresultado.Visible = true;
                    return;
                }
                TimeSpan ts = desde - hasta;
                // Difference in days.
                if (ts.Days > 30)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-alerta";
                    this.sinresultado.InnerText = "El rango máximo de consulta es de 1 mes, gracias por entender.";
                    sinresultado.Visible = true;
                    return;
                }
                var user = Page.getUserBySesion();
                ta.ClearBeforeFill = true;
                //user.loginname = "botrosa";
                ta.Fill(table, user.loginname, this.aisvn.Text.Trim(), this.docnum.Text.Trim(), this.cntrn.Text.Trim(), this.booking.Text.Trim(), desde, hasta);
                if (table.Rows.Count <= 0)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = "No se encontraron resultados, revise la unidad, documento o # aisv";
                    sinresultado.Visible = true;
                    return;
                }

                Session["resultado"] = table;
                this.tablePagination.DataSource = table;
                this.tablePagination.DataBind();
                xfinder.Visible = true;
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "populate", "Hubo un error al buscar", t.loginname));
                sinresultado.Visible = true;
            }
            finally
            {
                ta.Dispose();
                table.Dispose();
            }
        }


        [WebMethod]
        public static string ConsultarFechaHoraAISV(string aisv)
        {
            try
            {
                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                jAisvContainer dbo = new jAisvContainer();
                var user = new usuario();

                user = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                var json = dbo.GetFechaYHoraAISV(aisv);

                // Convertir el objeto json a una cadena JSON
                string jsonStr = JsonConvert.SerializeObject(json);

                // Devolver la cadena JSON como respuesta
                return jsonStr;
            }
            catch (Exception ex)
            {
                throw new Exception("error", ex);
            }
        }




        [WebMethod]
        public static string ConsultarEventosPorDiaAISV(string start, string varTodos, string aisv)
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
                var CodigoContenedorAisvRegistro = string.Empty;
                CodigoContenedorAisvRegistro = dbo.GetCodigoIso(aisv);
                getTipoCarga = dbo.GetISO(CodigoContenedorAisvRegistro);
                if (varTodos == "TODOS")
                {
                    getTipoCarga = "ALL";
                    var consultaTurnosDetalle = objCab.GetListaTurnosPorDiaTipoContenedorALL(fechaRestadaString, getTipoCarga,100);

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

                    var consultaTurnosDetalle = objCab.GetListaTurnosPorDiaTipoContenedor(fechaRestadaString, getTipoCarga,100);

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


            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }


        [WebMethod]

        public static string ActualizarTurnoPorAISV(string idTurno, string fecha, string horallegada, string aisv)
        {
            try
            {
                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                jAisvContainer dbo = new jAisvContainer();
                var user = new usuario();

                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechaConvertida;//= fecha; // Asumiendo que es "12/9/2023" en formato local

                if (!DateTime.TryParseExact(fecha, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaConvertida))
                {
                    return null;
                }

                user = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                DateTime fechaActual = DateTime.Now;
               
                TimeSpan horallegadaTime = TimeSpan.Parse(horallegada);
                
                var idTurnoAnterior = dbo.GetIdturno(aisv);
                objCab.EditarTurnoCanceladoAISV(aisv, idTurnoAnterior, user.loginname, fechaActual);

                dbo.UpdateAisvRegistro(idTurno, fechaConvertida, horallegadaTime, aisv);
                objCab.EditarTurnoDisponibles(Convert.ToInt32(idTurno), user.loginname, fechaActual, aisv, "", "", "");



                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("error", ex);
            }


        }

       
    }
}