using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Text;
using System.IO;
using CSLSite.unitService;
using System.Text.RegularExpressions;
using csl_log;
 using ClsAisvSav;
using System.Globalization;
using ClsProformas;
using CSLSite.app_start;
using CSLSite.sav;
using System.Data;
using System.Net.Http;
using BillionEntidades;

namespace CSLSite
{
    public partial class aisv_sav : System.Web.UI.Page
    {
        #region "Nuevas Referencias"
        private ServiciosSav objServicios = new ServiciosSav();
        public static string v_mensaje = string.Empty;
        private SaldoProforma objProforma = new SaldoProforma();
        private Cls_Bil_Sav_Lineas objLineas = new Cls_Bil_Sav_Lineas();


        #endregion

        private string numerobl
        {
            get
            {
                return (string)Session["numerobl"];
            }
            set
            {
                Session["numerobl"] = value;
            }

        }

        private string tipocontenedor
        {
            get
            {
                return (string)Session["tipocontenedor"];
            }
            set
            {
                Session["tipocontenedor"] = value;
            }

        }


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
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "vacios", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
           this.IsTokenAlive();
#if !DEBUG
            this.IsAllowAccess();
#endif
           Page.Tracker();
           if (!IsPostBack)
           {
                LlenaComboDepositos();
                this.LlenaComboLineas();
                this.IsCompatibleBrowser();
               Page.SslOn();
           }
        }

        #region "DEPOSITO"
        protected void cmbDeposito_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbDeposito.SelectedIndex != -1)
                {
                    Int64 ID_DEPORT;

                    if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                    {
                        ID_DEPORT = 0;
                    }

                    dplinea.DataSource = man_pro_expo.consultaLineas_Sav(ID_DEPORT);
                    dplinea.DataValueField = "ID";
                    dplinea.DataTextField = "DESCRIPCION";
                    dplinea.DataBind();
                    dplinea.Enabled = true;

                    dpturno.Items.Clear();
                    dpturno.DataSource = null;
                    dpturno.DataBind();

                    this.populateRepeater(1);

                }


            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "aisv_sav.aspx", "cmbDeposito_SelectedIndexChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

              
            }
        }
        #endregion

        #region "LINEA"
        protected void dplinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (dplinea.SelectedIndex != -1)
                {
                  
                    this.populateRepeater(1);

                }


            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "aisv_sav.aspx", "cmbDeposito_SelectedIndexChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);


            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // this.sinresultado.Visible = IsPostBack;
            if (!IsPostBack)
            {
                this.sinresultado.Attributes["class"] = string.Empty;

                var usn = new usuario();
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                if (token == null)
                {
                    this.Alerta("Estimado Cliente,Su sesión ha expirado, sera redireccionado a la pagina de login", true);
                    System.Web.Security.FormsAuthentication.SignOut();
                    Session.Clear();
                    return;
                }

                usn = this.getUserBySesion();
                if (usn != null)
                {
                    bool Credito = usuario.Credito(usn.ruc);
                    if (Credito)
                    {
                        this.xfinder.Visible = false;
                    }
                    else
                    {
                        this.xfinder.Visible = true;
                        populateRepeater(1);
                    }
                }
                else
                {
                    this.xfinder.Visible = false;

                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "aisv_sav", "Page_Load", "No usuario", Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
                   

            }
            this.txtidentidad.InnerText = this.dv_licencia.Value;
            this.txtconductor.InnerText = this.dv_nombre.Value;

        }

        protected void btbuscar_Click(object sender, EventArgs e)
        {
            //<JGUSQUI 20210406> CAMBIO REALIZADO PARA ASUME TERCEROS SAV
            bool v_AsumeTercero = false;
            string v_rucAsume = string.Empty;
            string v_nameAsume = string.Empty;
            string v_loginAsume = string.Empty;
            string v_loginCliente = string.Empty;
            string v_rucCliente = string.Empty;
            string v_nameCliente = string.Empty;
            long v_idAsume = 0;
            //</JGUSQUI 20210406>

            txtcontenedor.Text = txtcontenedor.Text.ToUpper();
            txtplaca.Text = txtplaca.Text.ToUpper();

            DateTime fecha = new DateTime();
            CultureInfo enUS= new CultureInfo("en-US");
            if (!DateTime.TryParseExact(fecsalida.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}", fecsalida.Text));
                fecsalida.Focus();
                return;
            }

            try
            {
                //<JGUSQUI 20210406> CAMBIO REALIZADO PARA ASUME TERCEROS SAV
                //valida si existe pago de tercero
                var oAsume = new ControlPagos.Importacion.PagoAsignado();
                var userLogeado = this.getUserBySesion();
                oAsume.container = txtcontenedor.Text.ToUpper();
                oAsume.ruc_asumido = userLogeado.ruc;
                
                var v_resultado = oAsume.ExisteAsumeTercero();
                if (v_resultado.Exitoso)
                {
                    oAsume = v_resultado.Resultado;
                    v_AsumeTercero = true;
                    v_rucAsume = v_resultado.Resultado.ruc;
                    v_nameAsume = v_resultado.Resultado.nombre;
                    v_loginAsume = v_resultado.Resultado.login_asigna;
                    v_idAsume = long.Parse(v_resultado.Resultado.id_asignacion.ToString());
                }
                //</JGUSQUI 20210406>
                
                
                if (Response.IsClientConnected)
                {
                    //variables proforma
                    //CultureInfo enUS = new CultureInfo("es-US");
                    int Cantidad = 0;
                    Decimal Total = 0;
                    Decimal totalIva = 0;
                    Decimal ValotUnitario = 0;
                    Decimal nIva = 0;
                    string IdProforma = string.Empty;
                    string IdLiquidacion = string.Empty;
                    bool Genera_Profroma = false;
                    string SecuenciaProforma = string.Empty;
                    string MensajeProforma = string.Empty;

                    var user = this.getUserBySesion();
                    //<JGUSQUI 20210406> CAMBIO REALIZADO PARA ASUME TERCEROS SAV
                    v_loginCliente = user.loginname;
                    v_rucCliente = user.ruc;
                    //v_nameCliente = user.nombres + " " + user.apellidos;
                    v_nameCliente = user.nombres == user.apellidos ? user.apellidos : user.nombres + " " + user.apellidos;
                    oAsume.login_modifica = user.loginname;
                    //</JGUSQUI 20210406>

                    if (user.loginname == null || user.loginname.Trim().Length <= 0)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El usuario era null!"), "aisv_sav", "btbuscar_Click","SAV", user.loginname));
                        return;
                    }

                    var token = HttpContext.Current.Request.Cookies["token"];
                    //Validacion 3 -> Si su token existe
                    if (token == null)
                    {
                        var id = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "aisv_sav", "btbuscar_Click", token.Value, user.loginname);
                        var personalms = string.Format("Su formulario ha expirado, será redireccionado a la página de menú, su id={0} expiró..", id);
                        this.PersonalResponse(personalms, "../cuenta/menu.aspx", true);
                        return;
                    }

                    //<JGUSQUI 20210406> CAMBIO REALIZADO PARA ASUME TERCEROS SAV
                    if (v_AsumeTercero)
                    {
                        var oUsuario = new usuario();
                        oUsuario = oUsuario.ObtenerUser(v_loginAsume, v_rucAsume);

                        user.ruc = oUsuario.ruc;
                        user.codigoempresa = oUsuario.codigoempresa;
                        user.email = oUsuario.email;
                        user.grupo = oUsuario.grupo;
                        user.id = oUsuario.id;
                        user.loginname = oUsuario.loginname == null? user.loginname: oUsuario.loginname;
                    }
                    //</JGUSQUI 20210406>

                    //Si todo esta listo envío el trabajo al servicio de preavisos y el resto es de el...
                    //se debe grabar abajo se debe imprimir.
                    var mailes = string.Empty;
                    if (txtMailAdicional.Value != null && txtMailAdicional.Value.Trim().Length > 0)
                    {
                        mailes = string.Format("{0};{1}", user.email, txtMailAdicional.Value);
                    }
                    else
                    {
                        mailes = user.email;
                    }

                    //1 turno
                    if (string.IsNullOrEmpty(dpturno.SelectedValue) || dpturno.SelectedValue.Equals("0"))
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = "Por favor seleccione el horario.";
                        return;

                    }
                    if (string.IsNullOrEmpty(txtcontenedor.Text))
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = "Por favor escriba el numero del contenedor";
                        return;

                    }
                    if (string.IsNullOrEmpty(dptamano.SelectedValue) || dptamano.SelectedValue.Equals("0"))
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = "Por favor seleccione el tamaño del contenedor";
                        return;

                    }
                    if (string.IsNullOrEmpty(dplinea.SelectedValue) || dplinea.SelectedValue.Equals("0"))
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = "Por favor seleccione el operador del contenedor";
                        return;

                    }

                    if (string.IsNullOrEmpty(dv_licencia.Value))
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = "Por favor seleccione el conductor";
                        return;

                    }
                    if (string.IsNullOrEmpty(txtplaca.Text))
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = "Por favor escriba el numero de placa";
                        return;

                    }

                    //valida si es MSC
                    string Operadora = this.dplinea.SelectedValue.ToString();
                    bool Valida_WS = false;
                    string _CODIGOADUANA = string.Empty;

                    Int64 ID_DEPORT;

                    if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                    {
                        ID_DEPORT = 0;
                    }

                    string cMensajes;
                    List<Cls_Bil_Sav_Lineas> LineaOperadora = Cls_Bil_Sav_Lineas.Consulta_Linea_Sav(Operadora, ID_DEPORT, out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                        this.Alerta(string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener Línea Operadora....{0}", cMensajes));
                        dplinea.Focus();
                        return;
                    }

                    if (LineaOperadora.Count == 0)
                    {

                        this.Alerta(string.Format("No existe información con la línea operadora seleccionada: {0} ..", Operadora));
                        dplinea.Focus();
                        return;
                    }

                    var LinqLinea = LineaOperadora.FirstOrDefault();

                    if (LinqLinea.VALIDA_WS)
                    {
                        Valida_WS = true;
                    }
                    else
                    {

                        List<Cls_Bil_CabeceraMsc> FechaDespacho = Cls_Bil_CabeceraMsc.Despacho_Contenedor_Linea(this.txtcontenedor.Text.Trim(), Operadora, out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {
                            this.Alerta(string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener contenedor de despacho....{0}", cMensajes));
                            fecsalida.Focus();
                            return;
                        }

                        if (FechaDespacho.Count == 0)
                        {
                            dpturno.Items.Clear();

                            this.Alerta(string.Format("No existe información del contenedor: {0} y Línea Operadora..", this.txtcontenedor.Text.Trim(), Operadora));
                            fecsalida.Focus();
                            return;
                        }


                    }

                    if (string.IsNullOrEmpty(LinqLinea.CODIGOADUANA))
                    {
                        _CODIGOADUANA = "";
                    }
                    else
                    {
                        _CODIGOADUANA = LinqLinea.CODIGOADUANA.Trim();
                    }

                    //if (Operadora == "MSC")
                    //{
                    //    string cMensajes;
                    //    List<Cls_Bil_Configuraciones> ValidaSMC = Cls_Bil_Configuraciones.Get_Validacion("VALIDA_MSC", out cMensajes);
                    //    if (!String.IsNullOrEmpty(cMensajes))
                    //    {
                    //        this.sinresultado.Attributes["class"] = string.Empty;
                    //        this.sinresultado.Attributes["class"] = "msg-critico";
                    //        this.sinresultado.InnerText = string.Format("Informativo! Error en configuraciones. {0}", cMensajes);
                    //        return;
                    //    }

                    //    bool Valida_SMC = false;
                    //    if (ValidaSMC.Count != 0)
                    //    {
                    //        Valida_SMC = true;
                    //    }

                    //    if (Valida_SMC)
                    //    {
                    //        List<Cls_Bil_CabeceraMsc> FechaDespacho = Cls_Bil_CabeceraMsc.Despacho_Contenedor(this.txtcontenedor.Text.Trim(), out cMensajes);
                    //        if (!String.IsNullOrEmpty(cMensajes))
                    //        {
                    //            this.sinresultado.Attributes["class"] = string.Empty;
                    //            this.sinresultado.Attributes["class"] = "msg-critico";
                    //            this.sinresultado.InnerText = string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener contenedor de despacho....{0}", cMensajes);
                    //            return;
                    //        }

                    //        if (FechaDespacho.Count == 0)
                    //        {
                    //            dpturno.Items.Clear();

                    //            this.sinresultado.Attributes["class"] = string.Empty;
                    //            this.sinresultado.Attributes["class"] = "msg-critico";
                    //            this.sinresultado.InnerText = string.Format("No existe información del contenedor: {0} ingresado.", this.txtcontenedor.Text.Trim());
                    //            return;

                    //        }

                    //    }
                    //}

                    //VALIDAR DATOS----->
                    //CHOFER------->AISV
                    bool choferlock = jAisvContainer.IsDriverLock(this.dv_licencia.Value);
                    if (choferlock)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("El conductor con licencia {0} esta inhabilitado para ingresar a la terminal",this.dv_licencia.Value);
                        return;
                    }

                    //PLACA-------->AISV
                    bool camionlock = jAisvContainer.IsTruckLock(this.txtplaca.Text);
                    if (camionlock)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("El camion con placas {0} esta inhabilitado para ingresar a la terminal", this.txtplaca.Text);
                        return;
                    }

                    this.bandera.Value = "1";
                    //CONTENEDOR DE RETORNO--->
                    //ESTA MARCADO SI/NO --PUMA
                    //FECHA DE CAS NO ES SUPERIOR A HOY SI/NO --PUMA 
                    string mens = string.Empty;

                    /*var ru = jAisvContainer.UnidadRetornable(txtcontenedor.Text.Trim(), out mens);
                    if (ru == null)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = mens;
                        return;
                    }
                    if (string.IsNullOrEmpty(ru.categoria))
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = mens;
                        return;
                    }

                    if (!ru.categoria.ToLower().Contains("imprt"))
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Unidad {0} no cuenta con autorizacion de retorno [CAT]",txtcontenedor.Text);
                        return;
                    }

                    if (!ru.cas.HasValue)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Unidad {0} no cuenta con autorizacion de retorno [CAS]", txtcontenedor.Text);
                        return;
                    }
                    if (ru.cas.Value < DateTime.Now.Date)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Ha expirado la autorizacion para unidad {0}", txtcontenedor.Text);
                        return;
                    }
                    */

                    //if (string.IsNullOrEmpty(txtDAE.Text))
                    //{
                    //    this.sinresultado.Attributes["class"] = string.Empty;
                    //    this.sinresultado.Attributes["class"] = "msg-critico";
                    //    this.sinresultado.InnerText = "Por favor escriba el numero de documento";
                    //    return;
                    //}

                    //VALIDA DAE
                    bool valida = true;// ecuapass_dae.ValidarDAE(txtDAE.Text);
                    if (!valida)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = "Por favor verifique que el número de DAE ingresado sea correcto e intente nuevamente";
                        return;
                    }

                    //GENERAR SECUENCIA DE AISV
                    string sq = jAisvContainer.SiguienteAISV();
                    if (string.IsNullOrEmpty(sq))
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = "Ocurrio una excepcion durante la creacion de la secuencia";
                        return;
                    }


                    //validacion cuando es cliente de contado/proforma
                    bool Credito = usuario.Credito(user.ruc);

                    //<JGUSQUI 20210406> CAMBIO REALIZADO PARA ASUME TERCEROS SAV
                    if (!Credito)
                    {
                        if (v_AsumeTercero)
                        {
                            xfinder.Visible = false;
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = "Ocurrió un error, El cliente que asume el pago no tiene crédito.";
                            return;
                        }
                    }
                    //</JGUSQUI 20210406>

                    if (!Credito)
                    {
                        var strQty = ViewState["cantidad"] != null ? ViewState["cantidad"].ToString() : string.Empty;

                        if (!int.TryParse(strQty, out Cantidad) || Cantidad <= 0)
                        {
                            xfinder.Visible = false;
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = "Ocurrió un error, no se pudo obtener la cantidad para el calculo de los valores de la proforma.";
                            return;
                        }

                        var strValor = ViewState["total"] != null ? ViewState["total"].ToString() : string.Empty;

                        if (!Decimal.TryParse(strValor, out Total) || Total <= 0)
                        {
                            xfinder.Visible = false;
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = "Ocurrió un error, no se pudo obtener el total de la proforma.";
                            return;
                        }

                        objServicios = Session["ServiciosSav"] as ServiciosSav;

                        if (objServicios == null)
                        {
                            xfinder.Visible = false;
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = "Ocurrió un error, no se pudo obtener Session[ServiciosSav]";
                            return;

                        }

                    }


                    //la fecha del turno es la de la cas

                    //la hora del turno es del combo 1-2
                    //Parte para el servicio web de Farbem

                    /*************************************************************************************************************************************************************
                    * 02-06-2025 REPCONTVER (grabar turno de forma directa) ZAL/REPCONTVER
                    *************************************************************************************************************************************************************/
                    DataTable v_dataTurno = new DataTable();
                    TurnoSav turnos = new TurnoSav();
                    if (int.Parse(cmbDeposito.SelectedValue.ToString()) == 5)
                    {
                        string pError;

                        var sc = new TurnosSAV.ServicioPreavisosClient();
                        var tok = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);

                        if (validarTurno(fecha, Valida_WS, _CODIGOADUANA))
                        {
                            v_dataTurno = turnos.ConsultarHorariosDisponibles(fecha, int.Parse(cmbDeposito.SelectedValue.ToString()));
                        }
                        else
                        {
                            dpturno.Items.Clear();
                            return;
                        }

                        if (v_dataTurno.Select(" ID = " + dpturno.SelectedValue.ToString()).Count() == 0)
                        {
                            this.Alerta(string.Format("Turno Caducado"));
                            dpturno.Items.Clear();
                            return;
                        }


                        var paseRev = new SAV_GenerarPase();


                        bool? check = paseRev.ExisteTurnoEnCGSA(sq, out pError);
                        if (!check.HasValue)
                        {
                            this.Alerta(string.Format("Turno ya existe, por favor intente en unos minutos o seleccione otro turno"));
                            dpturno.Items.Clear();
                            return;
                        }

                        bool? check2 = paseRev.ExisteConteendorTurnoEnCGSA(txtcontenedor.Text.ToUpper(), out pError);
                        if (!check2.HasValue)
                        {
                            this.Alerta(string.Format("Turno ya existe para la unidad {0}",txtcontenedor.Text.ToUpper()));
                            dpturno.Items.Clear();
                            return;
                        }

                        paseRev.turno_id = sq;
                        paseRev.turno_fecha = fecha;

                        string valorCombo = dpturno.SelectedItem.Text;
                        string[] comboTurno = valorCombo.Split('-');
                        paseRev.turno_hora = comboTurno[1];

                        paseRev.unidad_id = txtcontenedor.Text.ToUpper();
                        paseRev.unidad_tamano = dptamano.SelectedValue;
                        paseRev.unidad_linea = dplinea.SelectedValue;
                        paseRev.unidad_booking = "";
                        paseRev.unidad_dae = this.txtDAE.Text;
                        paseRev.unidad_referencia = "";
                        paseRev.unidad_key = 0;

                        paseRev.vehiculo_placa = this.txtplaca.Text;
                        paseRev.chofer_licencia = dv_licencia.Value;
                        paseRev.chofer_nombre = dv_nombre.Value;

                        paseRev.deposito_id = int.Parse(cmbDeposito.SelectedValue.ToString());
                        paseRev.deposito_nombre = cmbDeposito.SelectedItem.Text;
                        paseRev.turno_numero = int.Parse(dpturno.SelectedValue.ToString());

                     
                        paseRev.ruc_asume = v_rucAsume;
                        paseRev.ruc_cliente = v_rucCliente;
                        paseRev.name_asume = v_nameAsume;
                        paseRev.name_cliente = v_nameCliente;
                        paseRev.id_asignacion = v_idAsume;

                        paseRev.creado_usuario = user.loginname; //usuario logueado

                        

                        var rpm = paseRev.SaveTransaction(out pError);
                        if (!rpm.HasValue)
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = pError;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                            return;
                        }

                        int Id = int.Parse(rpm.Value.ToString());//id transaccion

                        //generacion de proforma, clientes de contado
                        if (!Credito)
                        {

                            //carga datos de preaviso-para proforma
                            List<Preaviso> ListPreaviso = Preaviso.Cargar_Preaviso(Id, out v_mensaje);
                            if (ListPreaviso != null && string.IsNullOrEmpty(v_mensaje))
                            {
                                foreach (var Det in ListPreaviso)
                                {
                                    var proforma = new ProformaContado();
                                    proforma.Email = user.email;
                                    proforma.FechaSalida = DateTime.Now;
                                    proforma.IdGrupo = user.grupo.HasValue ? user.grupo.Value : 0;
                                    proforma.IdUsuario = user.id;
                                    proforma.UsuarioIngreso = user.loginname;

                                    proforma.Referencia = Det.unidad_referencia;
                                    proforma.Nave = txtcontenedor.Text.ToUpper();
                                    proforma.RUC = user.ruc;
                                    proforma.Token = token != null ? token.Value : "DEBUG"; ;

                                    proforma.Estado = true;
                                    proforma.Bokingnbr = Det.unidad_booking;
                                    proforma.Cantidad = Cantidad;
                                    proforma.Reserva = Cantidad;

                                    proforma.FechaCliente = DateTime.Now;
                                    proforma.Etd = DateTime.Now;
                                    proforma.Reefer = true;
                                    proforma.Cutoff = DateTime.Now;
                                    proforma.Size = dptamano.SelectedValue;
                                    proforma.chofer = dv_nombre.Value;
                                    proforma.placa = this.txtplaca.Text;

                                    var secu = 1;
                                    foreach (var r in objServicios.Detalle)
                                    {

                                        var detalle = new ProformaContadoDetalle();
                                        detalle.BL = Det.unidad_booking;
                                        detalle.Cantidad = r.cantidad;
                                        detalle.CodigoServicio = r.codigo;
                                        detalle.DescServicio = r.descripcion;
                                        detalle.FechaAlmacenaje = DateTime.Now;
                                        detalle.Item = secu;
                                        detalle.Referencia = Det.unidad_referencia;
                                        detalle.ValorTotal = r.vtotal;
                                        detalle.ValorUnitario = r.costo;
                                        detalle.Contenedor = txtcontenedor.Text.ToUpper();
                                        ValotUnitario = r.costo;
                                        proforma.Detalle.Add(detalle);
                                        totalIva += r.vtotal;
                                        secu++;
                                        nIva = r.iva;

                                    }

                                    var subt = totalIva;
                                    var iva = (subt * (nIva / 100));
                                    var tot = (subt + iva);
                                    ValotUnitario = ValotUnitario + (ValotUnitario * (nIva / 100));

                                    proforma.ReteFuente = 0;
                                    proforma.ReteIVA = 0;
                                    proforma.IVA = iva;
                                    proforma.PctIVA = nIva;
                                    proforma.PctFuente = 0;
                                    proforma.IvaMas = 0;

                                    proforma.SubTotal = totalIva;
                                    proforma.Total = tot;

                                    var msm = string.Empty;
                                    string se;

                                    se = proforma.GuardarContado(user, out msm);
                                    /*error durante la generacion de liquidacion*/
                                    if (se == null || msm != string.Empty)
                                    {
                                        string NewMensaje = msm;
                                        this.sinresultado.Attributes["class"] = string.Empty;
                                        this.sinresultado.Attributes["class"] = "msg-critico";
                                        this.sinresultado.InnerText = NewMensaje;
                                        return;
                                    }
                                    else
                                    {
                                        IdProforma = se.ToString();
                                        IdLiquidacion = proforma.Liquidacion;
                                        SecuenciaProforma = proforma.Secuencia;

                                        objProforma = new SaldoProforma();
                                        objProforma.id_preaviso = Id;
                                        objProforma.ruc = user.ruc;
                                        objProforma.liquidacion = IdLiquidacion;
                                        objProforma.idproforma = Int64.Parse(IdProforma);
                                        objProforma.total = tot;
                                        objProforma.valorcruzado = 0;
                                        objProforma.valorsaldo = tot;
                                        objProforma.usuario = user.loginname;
                                        objProforma.Save(out v_mensaje);
                                        if (v_mensaje != string.Empty)
                                        {
                                            this.sinresultado.Attributes["class"] = string.Empty;
                                            this.sinresultado.Attributes["class"] = "msg-critico";
                                            this.sinresultado.InnerText = string.Format("Error al grabar saldo de proforma pre-aviso: {0}", v_mensaje);
                                            return;
                                        }

                                        Genera_Profroma = true;
                                        var actualiza = sc.actualizar_turno(tok, Id, IdProforma, "N", DateTime.Now);
                                        if (actualiza <= 0)
                                        {
                                            this.sinresultado.Attributes["class"] = string.Empty;
                                            this.sinresultado.Attributes["class"] = "msg-critico";
                                            this.sinresultado.InnerText = string.Format("Error al actualizar pre-aviso: {0},  {1}", Id, "Falla al actualizar");
                                            //return;                                    
                                        }

                                    }//fin graba proforma

                                }//fin recorre preaviso

                            }//fin existe preaviso

                        }//fin proforma contado
                        else
                        {   //credito
                            var actualiza = sc.actualizar_turno(tok, Id, "", "Y", DateTime.Now);
                            if (actualiza <= 0)
                            {
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = string.Format("Error al actualizar pre-aviso: {0},  {1}", Id, "Falla al actualizar");
                                //return;                                    
                            }

                        }

                        if (Id != 0)
                        {
                            //dtTurnosZAL.Rows[i]["IDPZAL"] = idpase;
                            //Generado = true;
                            string v_error = string.Empty;
                            if (ID_DEPORT == 5)
                            {
                                try
                                {
                                    var oInformacion = ExisteContenedorLinea.ConsultarListadoAISVGenerados(txtcontenedor.Text.ToUpper(), Operadora, out v_error).FirstOrDefault();

                                    if (oInformacion != null)
                                    {
                                        app_start.RepContver.ServicioCISE _servicio = new app_start.RepContver.ServicioCISE();
                                        Dictionary<string, string> obj = new Dictionary<string, string>();

                                        obj.Add("AV_ID_TIPO_ORDEN", "I");
                                        obj.Add("AN_ID_TURNO", Id.ToString());
                                        obj.Add("AD_FECHA_TURNO", fecha.ToString("yyyy-MM-dd") + comboTurno[1]); //DateTime.Now.ToString("dd/MM/yyyy")); //
                                        obj.Add("AN_ID_NAVIERA", "13");
                                        obj.Add("AV_NAVIERA_DESCRIPCION", Operadora);
                                       // obj.Add("AV_BOOKING", string.Empty);//nbrboo.Value.ToString());//nbrboo.Value.ToString().Replace("_CISE", String.Empty));
                                        //obj.Add("AV_BL", oInformacion?.bl?.ToString().Trim());//nbrboo.Value.ToString());
                                        obj.Add("AV_CONTENEDOR", txtcontenedor.Text.ToUpper());
                                        //obj.Add("AV_ID_TIPO_CTNER", oInformacion?.tipo?.ToString().Trim());

                                        if (Operadora.Equals("PIL"))
                                        {
                                            obj.Add("AV_BL", this.numerobl);//nbrboo.Value.ToString());
                                            obj.Add("AV_ID_TIPO_CTNER", this.tipocontenedor);
                                            obj.Add("AV_BOOKING", this.numerobl);
                                        }
                                        else
                                        {
                                            obj.Add("AV_BL", oInformacion?.bl?.ToString().Trim());//nbrboo.Value.ToString());
                                            obj.Add("AV_ID_TIPO_CTNER", oInformacion?.tipo?.ToString().Trim());
                                            obj.Add("AV_BOOKING", string.Empty);
                                        }

                                        obj.Add("AV_CLIENTE_RUC", userLogeado.ruc);
                                        obj.Add("AV_CLIENTE_NOMBRE", userLogeado.nombres);
                                        obj.Add("AV_TRANSPORTISTA_RUC", dv_licencia.Value);
                                        obj.Add("AV_TRANSPORTISTA_DESCRIPCION", dv_nombre.Value);
                                        obj.Add("AV_CHOFER_CEDULA", dv_licencia.Value);
                                        obj.Add("AV_CHOFER_NOMBRE", dv_nombre.Value);
                                        obj.Add("AV_PLACAS", this.txtplaca.Text);
                                        obj.Add("AV_BUQUE", oInformacion?.nave?.ToString().Trim());
                                        obj.Add("AV_VIAJE", oInformacion?.viaje?.ToString().Trim());
                                        obj.Add("AV_ESTADO", "A");

                                        obj.Add("accion", System.Configuration.ConfigurationManager.AppSettings["ACCION_CREACITA"]);

                                        CSLSite.app_start.RepContver.Respuesta _result = _servicio.Peticion("CREAR_CITA", obj);


                                        if (_result.result.estado != 1)
                                        {
                                            Genera_Profroma = false;
                                            //man_pro_expo.CancelarTurnoPorLinea(long.Parse(Id.ToString()), usr.loginname, out v_error);
                                            sc = new TurnosSAV.ServicioPreavisosClient();


                                            var can = paseRev.cancelar_turno(sq, txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV", out pError);
                                            //tok = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");

                                            //var tok = sc.obtener_token("farbem", "f@rB3m_2019");
                                            // var vt = sc.cancelar_turno_preavisado_simple(tok, sq, txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV");

                                            //AQUI LA ANULACION DEL AISV Y TURNO
                                            if (!can.HasValue)
                                            {
                                                sinresultado.Attributes["class"] = string.Empty;
                                                sinresultado.Attributes["class"] = "msg-critico";
                                                sinresultado.InnerText = pError;
                                                sinresultado.Visible = true;
                                                //populate();
                                                return;
                                            }
                                            sinresultado.Attributes["class"] = string.Empty;
                                            sinresultado.Attributes["class"] = "msg-info";
                                            sinresultado.InnerText = string.Format("La anulación del AISV  No.{0} ha resultado exitosa.", sq);
                                            sinresultado.Visible = true;


                                            Exception ex;
                                            ex = new Exception("Error al crear cita en REVCONVERT, El pase : " + Id.ToString() + " se ha cancelado; Respuesta de SW RevConvert: " + _result.result.mensaje);
                                            var number = log_csl.save_log<Exception>(ex, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                            this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " Mensaje Servicio: " + _result.result.mensaje + " #Error: " + number);
                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                                        }
                                        else
                                        {
                                            //NUEVO CAMPO
                                            //ACTUALIZO CON EL ID CISE
                                            var idPaseReconvert = long.Parse(_result.result.id_turno_referencia.ToString());
                                            ExisteContenedorLinea.Save_Update(Id.ToString(), _result.result.id_turno_referencia.ToString(), out v_error);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Genera_Profroma = false;
                                    //man_pro_expo.CancelarTurnoPorLinea(long.Parse(Id.ToString()), usr.loginname, out v_error);
                                    var sc1 = new TurnosSAV.ServicioPreavisosClient();

                                    var tok1 = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");

                                    //var tok = sc.obtener_token("farbem", "f@rB3m_2019");
                                    //var vt = sc.cancelar_turno_preavisado_simple(tok, Id.ToString(), txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV");

                                    var can = paseRev.cancelar_turno(sq, txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV", out pError);

                                    Genera_Profroma = false;
                                    Exception exc = new Exception(" Booking:" + Id.ToString() + " - " + ex.Message);
                                    var number = log_csl.save_log<Exception>(exc, "aisv_sav", "btbuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                }

                            }

                        }

                        //<JGUSQUI 20250530>INTEGRACION CON REVCONVERT
                        //if (Genera_Profroma)
                        //{
                        //    if (Id != 0)
                        //    {
                        //        //dtTurnosZAL.Rows[i]["IDPZAL"] = idpase;
                        //        //Generado = true;
                        //        string v_error = string.Empty;
                        //        if (ID_DEPORT == 5)
                        //        {
                        //            try
                        //            {
                        //                var oInformacion = ExisteContenedorLinea.ConsultarListadoAISVGenerados(txtcontenedor.Text.ToUpper(), Operadora, out v_error).FirstOrDefault();

                        //                if (oInformacion != null)
                        //                {
                        //                    app_start.RepContver.ServicioCISE _servicio = new app_start.RepContver.ServicioCISE();
                        //                    Dictionary<string, string> obj = new Dictionary<string, string>();

                        //                    obj.Add("AV_ID_TIPO_ORDEN", "I");
                        //                    obj.Add("AN_ID_TURNO", Id.ToString());
                        //                    obj.Add("AD_FECHA_TURNO", fecha.ToString("yyyy-MM-dd") + comboTurno[1]); //DateTime.Now.ToString("dd/MM/yyyy")); //
                        //                    obj.Add("AN_ID_NAVIERA", "13");
                        //                    obj.Add("AV_NAVIERA_DESCRIPCION", Operadora);
                        //                    obj.Add("AV_BOOKING", string.Empty);//nbrboo.Value.ToString());//nbrboo.Value.ToString().Replace("_CISE", String.Empty));
                        //                    obj.Add("AV_BL", oInformacion?.bl?.ToString().Trim());//nbrboo.Value.ToString());
                        //                    obj.Add("AV_CONTENEDOR", txtcontenedor.Text.ToUpper());
                        //                    obj.Add("AV_ID_TIPO_CTNER", oInformacion?.tipo?.ToString().Trim());
                        //                    obj.Add("AV_CLIENTE_RUC", userLogeado.ruc);
                        //                    obj.Add("AV_CLIENTE_NOMBRE", userLogeado.nombres);
                        //                    obj.Add("AV_TRANSPORTISTA_RUC", dv_licencia.Value);
                        //                    obj.Add("AV_TRANSPORTISTA_DESCRIPCION", dv_nombre.Value);
                        //                    obj.Add("AV_CHOFER_CEDULA", dv_licencia.Value);
                        //                    obj.Add("AV_CHOFER_NOMBRE", dv_nombre.Value);
                        //                    obj.Add("AV_PLACAS", this.txtplaca.Text);
                        //                    obj.Add("AV_BUQUE", oInformacion?.nave?.ToString().Trim());
                        //                    obj.Add("AV_VIAJE", oInformacion?.viaje?.ToString().Trim());
                        //                    obj.Add("AV_ESTADO", "A");

                        //                    obj.Add("accion", System.Configuration.ConfigurationManager.AppSettings["ACCION_CREACITA"]);

                        //                    CSLSite.app_start.RepContver.Respuesta _result = _servicio.Peticion("CREAR_CITA", obj);


                        //                    if (_result.result.estado != 1)
                        //                    {
                        //                        Genera_Profroma = false;
                        //                        //man_pro_expo.CancelarTurnoPorLinea(long.Parse(Id.ToString()), usr.loginname, out v_error);
                        //                        sc = new TurnosSAV.ServicioPreavisosClient();


                        //                        var can = paseRev.cancelar_turno(sq, txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV", out pError);
                        //                        //tok = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");

                        //                        //var tok = sc.obtener_token("farbem", "f@rB3m_2019");
                        //                        // var vt = sc.cancelar_turno_preavisado_simple(tok, sq, txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV");

                        //                        //AQUI LA ANULACION DEL AISV Y TURNO
                        //                        if (!can.HasValue)
                        //                        {
                        //                            sinresultado.Attributes["class"] = string.Empty;
                        //                            sinresultado.Attributes["class"] = "msg-critico";
                        //                            sinresultado.InnerText = pError;
                        //                            sinresultado.Visible = true;
                        //                            //populate();
                        //                            return;
                        //                        }
                        //                        sinresultado.Attributes["class"] = string.Empty;
                        //                        sinresultado.Attributes["class"] = "msg-info";
                        //                        sinresultado.InnerText = string.Format("La anulación del AISV  No.{0} ha resultado exitosa.", sq);
                        //                        sinresultado.Visible = true;


                        //                        Exception ex;
                        //                        ex = new Exception("Error al crear cita en REVCONVERT, El pase : " + Id.ToString() + " se ha cancelado; Respuesta de SW RevConvert: " + _result.result.mensaje);
                        //                        var number = log_csl.save_log<Exception>(ex, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                        //                        this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " Mensaje Servicio: " + _result.result.mensaje + " #Error: " + number);
                        //                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        //                    }
                        //                    else
                        //                    {
                        //                        //NUEVO CAMPO
                        //                        //ACTUALIZO CON EL ID CISE
                        //                        var idPaseReconvert = long.Parse(_result.result.id_turno_referencia.ToString());
                        //                        ExisteContenedorLinea.Save_Update(Id.ToString(), _result.result.id_turno_referencia.ToString(), out v_error);
                        //                    }
                        //                }
                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                Genera_Profroma = false;
                        //                //man_pro_expo.CancelarTurnoPorLinea(long.Parse(Id.ToString()), usr.loginname, out v_error);
                        //                var sc1 = new TurnosSAV.ServicioPreavisosClient();

                        //                var tok1 = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");

                        //                //var tok = sc.obtener_token("farbem", "f@rB3m_2019");
                        //                //var vt = sc.cancelar_turno_preavisado_simple(tok, Id.ToString(), txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV");

                        //                var can = paseRev.cancelar_turno(sq, txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV", out pError);

                        //                Genera_Profroma = false;
                        //                Exception exc = new Exception(" Booking:" + Id.ToString() + " - " + ex.Message);
                        //                var number = log_csl.save_log<Exception>(exc, "aisv_sav", "btbuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                        //            }

                        //        }

                        //    }
                        //}


                    }
                    else 
                    {
                        var sc = new TurnosSAV.ServicioPreavisosClient();

                        var tok = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");
                        var tur = sc.nuevo_turno_sin_unidad(sq, dpturno.Text, fecha);
                        var uni = new TurnosSAV.unidad();
                        var cho = new TurnosSAV.chofer();
                        var vh = new TurnosSAV.vehiculo();

                        vh.placa = this.txtplaca.Text;
                        cho.licencia = dv_licencia.Value;
                        cho.nombres = dv_nombre.Value;

                        uni.id = txtcontenedor.Text.ToUpper();
                        uni.linea = dplinea.SelectedValue;
                        uni.tamano = dptamano.SelectedValue;
                        uni.dae = this.txtDAE.Text;

                        //<JGUSQUI 20210406> CAMBIO REALIZADO PARA ASUME TERCEROS SAV
                        uni.ruc_asume = v_rucAsume;
                        uni.ruc_cliente = v_rucCliente;
                        uni.name_asume = v_nameAsume;
                        uni.name_cliente = v_nameCliente;
                        uni.id_asignacion = v_idAsume;
                        //</JGUSQUI 20210406>
                        tur.deposito.id = int.Parse(cmbDeposito.SelectedValue.ToString());
                        tur.deposito.nombre = cmbDeposito.SelectedItem.Text;
                        tur.turno_numero = int.Parse(dpturno.SelectedValue.ToString());
                        tur.fecha = fecha;

                        string valorCombo = dpturno.SelectedItem.Text;
                        string[] comboTurno = valorCombo.Split('-');
                        tur.horario = comboTurno[1];


                        tur.unidad = uni; //unidad creada con linea / tamano
                        tur.creado_por = user.loginname; //usuario logueado
                        tur.chofer = cho; // chofer seleccionado
                        tur.vehiculo = vh; //placa 
                        tur.mail = mailes; // emails

                        //valida que el turno seleccionado aun sigue vigente
                        //TurnoSav turnos = new TurnoSav();
                        //DataTable v_dataTurno =  new DataTable();

                        if (validarTurno(fecha, Valida_WS, _CODIGOADUANA))
                        {
                            v_dataTurno = turnos.ConsultarHorariosDisponibles(fecha, int.Parse(cmbDeposito.SelectedValue.ToString()));
                        }
                        else
                        {
                            dpturno.Items.Clear();
                            return;
                        }

                        if (v_dataTurno.Select(" ID = " + dpturno.SelectedValue.ToString()).Count() == 0)
                        {
                            this.Alerta(string.Format("Turno Caducado"));
                            dpturno.Items.Clear();
                            return;
                        }

                        var rpm = sc.preavisar_turno(tur, tok, user.loginname);
                        if (rpm.resultado != 1)
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = rpm.mensaje_principal;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                            return;
                        }

                        int Id = int.Parse(rpm.que_hacer);//id transaccion

                        //generacion de proforma, clientes de contado
                        if (!Credito)
                        {

                            //carga datos de preaviso-para proforma
                            List<Preaviso> ListPreaviso = Preaviso.Cargar_Preaviso(Id, out v_mensaje);
                            if (ListPreaviso != null && string.IsNullOrEmpty(v_mensaje))
                            {
                                foreach (var Det in ListPreaviso)
                                {
                                    var proforma = new ProformaContado();
                                    proforma.Email = user.email;
                                    proforma.FechaSalida = DateTime.Now;
                                    proforma.IdGrupo = user.grupo.HasValue ? user.grupo.Value : 0;
                                    proforma.IdUsuario = user.id;
                                    proforma.UsuarioIngreso = user.loginname;

                                    proforma.Referencia = Det.unidad_referencia;
                                    proforma.Nave = txtcontenedor.Text.ToUpper();
                                    proforma.RUC = user.ruc;
                                    proforma.Token = token != null ? token.Value : "DEBUG"; ;

                                    proforma.Estado = true;
                                    proforma.Bokingnbr = Det.unidad_booking;
                                    proforma.Cantidad = Cantidad;
                                    proforma.Reserva = Cantidad;

                                    proforma.FechaCliente = DateTime.Now;
                                    proforma.Etd = DateTime.Now;
                                    proforma.Reefer = true;
                                    proforma.Cutoff = DateTime.Now;
                                    proforma.Size = dptamano.SelectedValue;
                                    proforma.chofer = dv_nombre.Value;
                                    proforma.placa = this.txtplaca.Text;

                                    var secu = 1;
                                    foreach (var r in objServicios.Detalle)
                                    {

                                        var detalle = new ProformaContadoDetalle();
                                        detalle.BL = Det.unidad_booking;
                                        detalle.Cantidad = r.cantidad;
                                        detalle.CodigoServicio = r.codigo;
                                        detalle.DescServicio = r.descripcion;
                                        detalle.FechaAlmacenaje = DateTime.Now;
                                        detalle.Item = secu;
                                        detalle.Referencia = Det.unidad_referencia;
                                        detalle.ValorTotal = r.vtotal;
                                        detalle.ValorUnitario = r.costo;
                                        detalle.Contenedor = txtcontenedor.Text.ToUpper();
                                        ValotUnitario = r.costo;
                                        proforma.Detalle.Add(detalle);
                                        totalIva += r.vtotal;
                                        secu++;
                                        nIva = r.iva;

                                    }

                                    var subt = totalIva;
                                    var iva = (subt * (nIva / 100));
                                    var tot = (subt + iva);
                                    ValotUnitario = ValotUnitario + (ValotUnitario * (nIva / 100));

                                    proforma.ReteFuente = 0;
                                    proforma.ReteIVA = 0;
                                    proforma.IVA = iva;
                                    proforma.PctIVA = nIva;
                                    proforma.PctFuente = 0;
                                    proforma.IvaMas = 0;

                                    proforma.SubTotal = totalIva;
                                    proforma.Total = tot;

                                    var msm = string.Empty;
                                    string se;

                                    se = proforma.GuardarContado(user, out msm);
                                    /*error durante la generacion de liquidacion*/
                                    if (se == null || msm != string.Empty)
                                    {
                                        string NewMensaje = msm;
                                        this.sinresultado.Attributes["class"] = string.Empty;
                                        this.sinresultado.Attributes["class"] = "msg-critico";
                                        this.sinresultado.InnerText = NewMensaje;
                                        return;
                                    }
                                    else
                                    {
                                        IdProforma = se.ToString();
                                        IdLiquidacion = proforma.Liquidacion;
                                        SecuenciaProforma = proforma.Secuencia;

                                        objProforma = new SaldoProforma();
                                        objProforma.id_preaviso = Id;
                                        objProforma.ruc = user.ruc;
                                        objProforma.liquidacion = IdLiquidacion;
                                        objProforma.idproforma = Int64.Parse(IdProforma);
                                        objProforma.total = tot;
                                        objProforma.valorcruzado = 0;
                                        objProforma.valorsaldo = tot;
                                        objProforma.usuario = user.loginname;
                                        objProforma.Save(out v_mensaje);
                                        if (v_mensaje != string.Empty)
                                        {
                                            this.sinresultado.Attributes["class"] = string.Empty;
                                            this.sinresultado.Attributes["class"] = "msg-critico";
                                            this.sinresultado.InnerText = string.Format("Error al grabar saldo de proforma pre-aviso: {0}", v_mensaje);
                                            return;
                                        }

                                        Genera_Profroma = true;
                                        var actualiza = sc.actualizar_turno(tok, Id, IdProforma, "N", DateTime.Now);
                                        if (actualiza <= 0)
                                        {
                                            this.sinresultado.Attributes["class"] = string.Empty;
                                            this.sinresultado.Attributes["class"] = "msg-critico";
                                            this.sinresultado.InnerText = string.Format("Error al actualizar pre-aviso: {0},  {1}", Id, "Falla al actualizar");
                                            //return;                                    
                                        }

                                    }//fin graba proforma

                                }//fin recorre preaviso

                            }//fin existe preaviso

                        }//fin proforma contado
                        else
                        {   //credito
                            var actualiza = sc.actualizar_turno(tok, Id, "", "Y", DateTime.Now);
                            if (actualiza <= 0)
                            {
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = string.Format("Error al actualizar pre-aviso: {0},  {1}", Id, "Falla al actualizar");
                                //return;                                    
                            }

                            //<JGUSQUI 20210406> CAMBIO REALIZADO PARA ASUME TERCEROS SAV
                            if (v_AsumeTercero)
                            {
                                oAsume.consecutivo = oAsume.obtener_preaviso_id_tos(oAsume.container).Resultado.ToString();
                                oAsume.BorrarAsignacionSAV();
                            }
                            //</JGUSQUI 20210406> 
                        }

                        //<JGUSQUI 20250530>INTEGRACION CON REVCONVERT
                        if (Genera_Profroma)
                        {
                            if (Id != 0)
                            {
                                //dtTurnosZAL.Rows[i]["IDPZAL"] = idpase;
                                //Generado = true;
                                string v_error = string.Empty;
                                if (ID_DEPORT == 5)
                                {
                                    try
                                    {
                                        var oInformacion = ExisteContenedorLinea.ConsultarListadoAISVGenerados(txtcontenedor.Text.ToUpper(), Operadora, out v_error).FirstOrDefault();

                                        if (oInformacion != null)
                                        {
                                            app_start.RepContver.ServicioCISE _servicio = new app_start.RepContver.ServicioCISE();
                                            Dictionary<string, string> obj = new Dictionary<string, string>();

                                            obj.Add("AV_ID_TIPO_ORDEN", "I");
                                            obj.Add("AN_ID_TURNO", Id.ToString());
                                            obj.Add("AD_FECHA_TURNO", tur.fecha.ToString("yyyy-MM-dd") + tur.horario.ToString()); //DateTime.Now.ToString("dd/MM/yyyy")); //
                                            obj.Add("AN_ID_NAVIERA", "13");
                                            obj.Add("AV_NAVIERA_DESCRIPCION", Operadora);
                                            obj.Add("AV_BOOKING", string.Empty);//nbrboo.Value.ToString());//nbrboo.Value.ToString().Replace("_CISE", String.Empty));

                                            if (Operadora.Equals("PIL"))
                                            {
                                                obj.Add("AV_BL", this.numerobl);//nbrboo.Value.ToString());
                                                obj.Add("AV_ID_TIPO_CTNER", this.tipocontenedor);
                                            }
                                            else 
                                            {
                                                obj.Add("AV_BL", oInformacion?.bl?.ToString().Trim());//nbrboo.Value.ToString());
                                                obj.Add("AV_ID_TIPO_CTNER", oInformacion?.tipo?.ToString().Trim());
                                            }
                                            
                                            obj.Add("AV_CONTENEDOR", txtcontenedor.Text.ToUpper());
                                           
                                            obj.Add("AV_CLIENTE_RUC", userLogeado.ruc);
                                            obj.Add("AV_CLIENTE_NOMBRE", userLogeado.nombres);
                                            obj.Add("AV_TRANSPORTISTA_RUC", dv_licencia.Value);
                                            obj.Add("AV_TRANSPORTISTA_DESCRIPCION", dv_nombre.Value);
                                            obj.Add("AV_CHOFER_CEDULA", dv_licencia.Value);
                                            obj.Add("AV_CHOFER_NOMBRE", dv_nombre.Value);
                                            obj.Add("AV_PLACAS", this.txtplaca.Text);
                                            obj.Add("AV_BUQUE", oInformacion?.nave?.ToString().Trim());
                                            obj.Add("AV_VIAJE", oInformacion?.viaje?.ToString().Trim());
                                            obj.Add("AV_ESTADO", "A");

                                            obj.Add("accion", System.Configuration.ConfigurationManager.AppSettings["ACCION_CREACITA"]);

                                            CSLSite.app_start.RepContver.Respuesta _result = _servicio.Peticion("CREAR_CITA", obj);


                                            if (_result.result.estado != 1)
                                            {
                                                Genera_Profroma = false;
                                                //man_pro_expo.CancelarTurnoPorLinea(long.Parse(Id.ToString()), usr.loginname, out v_error);
                                                sc = new TurnosSAV.ServicioPreavisosClient();

                                                tok = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");

                                                //var tok = sc.obtener_token("farbem", "f@rB3m_2019");
                                                var vt = sc.cancelar_turno_preavisado_simple(tok, sq, txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV");
                                                //AQUI LA ANULACION DEL AISV Y TURNO
                                                if (vt.resultado != 1)
                                                {
                                                    sinresultado.Attributes["class"] = string.Empty;
                                                    sinresultado.Attributes["class"] = "msg-critico";
                                                    sinresultado.InnerText = vt.mensaje_principal;
                                                    sinresultado.Visible = true;
                                                    //populate();
                                                    return;
                                                }
                                                sinresultado.Attributes["class"] = string.Empty;
                                                sinresultado.Attributes["class"] = "msg-info";
                                                sinresultado.InnerText = string.Format("La anulación del AISV  No.{0} ha resultado exitosa.", sq);
                                                sinresultado.Visible = true;


                                                Exception ex;
                                                ex = new Exception("Error al crear cita en REVCONVERT, El pase : " + Id.ToString() + " se ha cancelado; Respuesta de SW RevConvert: " + _result.result.mensaje);
                                                var number = log_csl.save_log<Exception>(ex, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                                this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " Mensaje Servicio: " + _result.result.mensaje + " #Error: " + number);
                                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                                            }
                                            else
                                            {
                                                //NUEVO CAMPO
                                                //ACTUALIZO CON EL ID CISE
                                                var idPaseReconvert = long.Parse(_result.result.id_turno_referencia.ToString());
                                                ExisteContenedorLinea.Save_Update(Id.ToString(), _result.result.id_turno_referencia.ToString(), out v_error);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Genera_Profroma = false;
                                        //man_pro_expo.CancelarTurnoPorLinea(long.Parse(Id.ToString()), usr.loginname, out v_error);
                                        var sc1 = new TurnosSAV.ServicioPreavisosClient();

                                        var tok1 = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");

                                        //var tok = sc.obtener_token("farbem", "f@rB3m_2019");
                                        var vt = sc.cancelar_turno_preavisado_simple(tok, Id.ToString(), txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV");

                                        Genera_Profroma = false;
                                        Exception exc = new Exception(" Booking:" + Id.ToString() + " - " + ex.Message);
                                        var number = log_csl.save_log<Exception>(exc, "aisv_sav", "btbuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                    }

                                }

                            }
                        }


                    }


                    //si genero la proforma, imprime
                    if (Genera_Profroma)
                    {
                        MensajeProforma=string.Format("Proforma No. {0}.", SecuenciaProforma);
                        var sid = QuerySegura.EncryptQueryString(IdProforma);
                        this.Popup(string.Format("../sav/printproforma_sav.aspx?sid={0}", sid));
                    }

                    //msg-info
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = string.Format("Se ha generado existosamente el Doc. No. {0}, {1}", sq, MensajeProforma);

                    //abrir una pantalla de impresion//
                    //Lanzar la impresion

                    var qs = QuerySegura.EncryptQueryString(sq);
                    string script = string.Format("window.open('../sav/printaisvsav.aspx?sid={0}',null,'height = 750,width = 750,status = yes,toolbar = no,menubar = no,location = no');", qs);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "openwindow", script, true);
                    // se debe enviar el mail
                    //Response.Redirect("../cuenta/menu.aspx", false);
                    //Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "aisv_sav", "btbuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
            }

            
        }


        #region "Nuevos Metodos"
        public void populateRepeater(int cantidad)
        {
            try
            {

                string vr = string.Empty;
                decimal nIva = 0;
                ViewState["cantidad"] = null;
                ViewState["total"] = null;

                //cabecera del grupo
                objServicios = new ServiciosSav();
                Session["ServiciosSav"] = objServicios;
                objServicios = Session["ServiciosSav"] as ServiciosSav;

                Int64 ID_DEPORT;

                if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                {
                    ID_DEPORT = 0;
                }

                Int64 ID_LINEA = 0;

                /***************************************************************************************
                * saco el id de la línea naviera
                ***************************************************************************************/
                //obtiene linea en base al código aduanero
                string OError;

                if (this.dplinea.SelectedValue.ToString().Equals("0"))
                {
                    ID_LINEA = 0;
                }
                else 
                {
                    objLineas = new Cls_Bil_Sav_Lineas();
                    objLineas.LINEA = this.dplinea.SelectedValue.ToString();
                    objLineas.DEPOT = ID_DEPORT;
                    if (!objLineas.PopulateMyData_Id_Linea(out OError))
                    {
                        this.sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError);
                        xfinder.Visible = true;
                        return;
                    }
                    else
                    {
                        ID_LINEA = objLineas.ID;

                    }
                }
                
                /***************************************************************************************/

                var table = ServiciosSav.List_Servicios_Recontvere(ID_DEPORT, ID_LINEA, out vr);

                foreach (var f in table)
                {

                    f.mensaje = !string.IsNullOrEmpty(f.nota) ?
                    string.Format("<a class='infotip'><span class='classic'>{0}</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>", f.nota) : "";

                    f.contenido = f.descripcion;
                    f.icont = true;

                    f.cantidad = f.tipo.Contains("T") ? cantidad : f.cantidad;
                    if (!f.aplica)
                    {
                        f.cantidad = 0;
                    }

                    //TODO CORREGIR SOLO EN PRIMER CASO
                    if (f.tipo.Contains("C"))
                    {
                        f.vtotal = (f.costo * f.cantidad);
                        f.costo = f.costo;
                    }
                    else
                    {
                        f.vtotal = (f.costo * f.cantidad);
                    }

                    nIva = f.iva;

                    objServicios.Detalle.Add(f);

                }

                Session["ServiciosSav"] = objServicios;

                tablaNueva.DataSource = table;
                tablaNueva.DataBind();
                var subt = table.Where(a => a.aplica).ToList().Sum(b => b.vtotal);
                var iva = (subt * (nIva / 100));
                var tot = (subt + iva);

                stsubtotal.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);
                stiva.InnerHtml = string.Format("<strong>{0:c}</strong>", iva);
                sttal.InnerHtml = string.Format("<strong>{0:c}</strong>", tot);

                if (subt == 0)
                {
                    this.sinresultado.InnerText = string.Format("No existen conceptos para realizar el cálculo de la Proforma.");
                    xfinder.Visible = false;
                    return;
                }
                else 
                {
                    xfinder.Visible = true; ;
                }

                ViewState["cantidad"] = cantidad;
                this.sinresultado.InnerText = string.Empty;
                ViewState["total"] = tot;


            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "aisv_sav", "populateRepeater", "Hubo un ERROR en cargar concepto", t != null ? t.loginname : "NoUser"));
                sinresultado.Visible = true;
            }
        }

        #endregion

        protected void fecsalida_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btnconsultarturnos_Click(object sender, EventArgs e)
        {
            DateTime fecha= new DateTime();
            CultureInfo enUS = new CultureInfo("en-US");
            if (!DateTime.TryParseExact(fecsalida.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}", fecsalida.Text));
                fecsalida.Focus();
                return;
            }
            bool Valida_WS = false;
            string _CODIGOADUANA = string.Empty;

            string Operadora = this.dplinea.SelectedValue.ToString();
            Int64 ID_DEPOT = Int64.Parse(this.cmbDeposito.SelectedValue.ToString());

            string cMensajes;
            List<Cls_Bil_Sav_Lineas> LineaOperadora = Cls_Bil_Sav_Lineas.Consulta_Linea_Sav(Operadora, ID_DEPOT,out cMensajes);
            if (!String.IsNullOrEmpty(cMensajes))
            {
                this.Alerta(string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener Línea Operadora....{0}", cMensajes));
                dplinea.Focus();
                return;
            }

            if (LineaOperadora.Count == 0)
            {
                
                this.Alerta(string.Format("No existe información con la línea operadora seleccionada: {0} ..", Operadora));
                dplinea.Focus();
                return;
            }

            var LinqLinea = LineaOperadora.FirstOrDefault();

            if (LinqLinea.VALIDA_WS)
            {
                Valida_WS = true;
            }
            else
            {

                List<Cls_Bil_CabeceraMsc> FechaDespacho = Cls_Bil_CabeceraMsc.Despacho_Contenedor_Linea(this.txtcontenedor.Text.Trim(), Operadora, out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {
                    this.Alerta(string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener contenedor de despacho....{0}", cMensajes));
                    fecsalida.Focus();
                    return;
                }

                if (FechaDespacho.Count == 0)
                {
                    dpturno.Items.Clear();

                    this.Alerta(string.Format("No existe información del contenedor: {0} y Línea Operadora..", this.txtcontenedor.Text.Trim(), Operadora));
                    fecsalida.Focus();
                    return;
                }
                else
                {
                    var Linq = (from Tbl in FechaDespacho
                                select new
                                {
                                    fecha_despacho = Tbl.FECHA_DESPACHO

                                }).FirstOrDefault();

                    if (fecha > Linq.fecha_despacho)
                    {

                        this.Alerta(string.Format("La fecha de salida: {0} no puede ser mayor que la fecha de despacho programada: {1}", fecha.ToString("dd/MM/yyyy"), Linq.fecha_despacho.ToString("dd/MM/yyyy")));
                        fecsalida.Focus();
                        return;
                    }

                }
            }

            if (string.IsNullOrEmpty(LinqLinea.CODIGOADUANA))
            {
                _CODIGOADUANA = "";
            }
            else 
            {
                _CODIGOADUANA = LinqLinea.CODIGOADUANA.Trim();
            }

            //if (Operadora == "MSC")
            //{
               
            //    List<Cls_Bil_Configuraciones> ValidaSMC = Cls_Bil_Configuraciones.Get_Validacion("VALIDA_MSC", out cMensajes);
            //    if (!String.IsNullOrEmpty(cMensajes))
            //    {
            //        this.Alerta(string.Format("Informativo! Error en configuraciones. {0}", cMensajes));
            //        fecsalida.Focus();
            //        return;
            //    }

            //    bool Valida_SMC = false;
            //    if (ValidaSMC.Count != 0)
            //    {
            //        Valida_SMC = true;
            //    }

            //    if (Valida_SMC)
            //    {

            //        List<Cls_Bil_CabeceraMsc> FechaDespacho = Cls_Bil_CabeceraMsc.Despacho_Contenedor(this.txtcontenedor.Text.Trim(), out cMensajes);
            //        if (!String.IsNullOrEmpty(cMensajes))
            //        {
            //            this.Alerta(string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener contenedor de despacho....{0}", cMensajes));
            //            fecsalida.Focus();
            //            return;
            //        }

            //        if (FechaDespacho.Count == 0)
            //        {
            //            dpturno.Items.Clear();

            //            this.Alerta(string.Format("No existe información del contenedor: {0} ingresado..", this.txtcontenedor.Text.Trim()));
            //            fecsalida.Focus();
            //            return;
            //        }

            //        //////else
            //        //////{
            //        //////    var Linq = (from Tbl in FechaDespacho
            //        //////                select new
            //        //////                {
            //        //////                    fecha_despacho = Tbl.FECHA_DESPACHO

            //        //////                }).FirstOrDefault();

            //        //////    if (fecha > Linq.fecha_despacho)
            //        //////    {

            //        //////        this.Alerta(string.Format("La fecha de salida: {0} no puede ser mayor que la fecha de despacho programada: {1}", fecha.ToString("dd/MM/yyyy"), Linq.fecha_despacho.ToString("dd/MM/yyyy")));
            //        //////        fecsalida.Focus();
            //        //////        return;
            //        //////    }

            //        //////}


            //    }
            //}
                

          

            if (validarTurno(fecha, Valida_WS, _CODIGOADUANA))
            {
                TurnoSav turnos = new TurnoSav();

                //se consulta los turnos habilitados 
                dpturno.DataSource = turnos.ConsultarHorariosDisponibles(fecha, int.Parse(cmbDeposito.SelectedValue.ToString())); //ds.Tables[0].DefaultView;
                dpturno.DataValueField = "ID";
                dpturno.DataTextField = "DESCRIPCION";
                dpturno.DataBind();
                dpturno.Enabled = true;

                if (dpturno.Items.Count == 0)
                {
                    this.Alerta(string.Format(" No existe turnos vigentes para la fecha {0}", fecha));
                }
            }
        }

        public void LlenaComboLineas()
        {
            try
            { 
                //dplinea.DataSource = man_pro_expo.consultaLineas();
                dplinea.DataSource = man_pro_expo.consultaLineas_Sav(Int64.Parse(this.cmbDeposito.SelectedValue.ToLower()));
                dplinea.DataValueField = "ID";
                dplinea.DataTextField = "DESCRIPCION";
                dplinea.DataBind();
                dplinea.Enabled = true;
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "aisv_sav.aspx", "LlenaComboLineas()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        public void LlenaComboDepositos()
        {
            try
            {
                cmbDeposito.DataSource = man_pro_expo.consultaDepositosFiltrado("1"); //ds.Tables[0].DefaultView;
                cmbDeposito.DataValueField = "ID";
                cmbDeposito.DataTextField = "DESCRIPCION";
                cmbDeposito.DataBind();
                cmbDeposito.Enabled = true;
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "aisv_sav.aspx", "LlenaComboDepositos()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        public bool validarTurno(DateTime fecha, bool Valida_WS, string CODIGOADUANA)
        {
            bool v_respuesta = false;
            try
            {
                if (txtcontenedor.Text == string.Empty)
                {
                    this.Alerta(string.Format("Ingrese un contenedor valido"));
                    return v_respuesta;
                }

                this.tipocontenedor = string.Empty;
                this.numerobl = string.Empty;

                //Validación de la CAS 
                var usn = new usuario();
                usn = this.getUserBySesion();

                if (string.IsNullOrEmpty(cmbDeposito.SelectedValue) || cmbDeposito.SelectedValue.Equals("0"))
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = "Por favor seleccione el Deposito";
                    return v_respuesta;

                }

                if (string.IsNullOrEmpty(dplinea.SelectedValue) || dplinea.SelectedValue.Equals("0"))
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = "Por favor seleccione el operador del contenedor";
                    return v_respuesta;

                }

                //<20230517>
                //BLOQUE DE CODIGO COMENTADOO POR SOLICITUD DE JOSE LUIS RODRIGUEZ
                //</20230517>
                switch (dplinea.SelectedValue) 
                {

                    case "HSD":

                        v_respuesta = true;

                        if (Valida_WS)
                        {

                            ServicioHamburg _servicioh = new ServicioHamburg();
                            _servicioh.Usuario = usn.loginname;

                            //RespuestaCast ResultToken = _servicio.Peticion("TOKEN", string.Empty, string.Empty);

                            //if (ResultToken.isCorrect)
                            //{
                            RespuestaHamburgCast<MessageHS> _resulta = _servicioh.Peticion("VALIDA_CAST", txtcontenedor.Text);

                            if (_resulta.Success)
                            {
                                CultureInfo enUS = new CultureInfo("en-US");
                                DateTime fechaCast;

                                string swFechaCast = _resulta.Message.DateExpiry.ToString("dd/MM/yyyy HH:mm");

                                //CutOff*
                                if (DateTime.TryParseExact(swFechaCast, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechaCast))
                                {
                                    if (fechaCast < fecha)
                                    {
                                        v_respuesta = false;
                                        dpturno.Items.Clear();
                                        this.Alerta(string.Format(" La fecha de ingreso {0} es mayor a la fecha maxima permitida {1}", fecha, fechaCast));
                                    }
                                    else
                                    {
                                        v_respuesta = true;
                                    }
                                }
                                else
                                {
                                    v_respuesta = false;
                                    this.sinresultado.Attributes["class"] = string.Empty;
                                    this.sinresultado.InnerText = "La fecha Cast SW HamburgSUD - tiene un formato de fecha no admitido.";
                                    this.sinresultado.Attributes["class"] = "msg-critico";
                                }
                            }
                            else
                            {
                                v_respuesta = false;
                                dpturno.Items.Clear();
                                this.Alerta(string.Format("Respuesta SW HamburgSUD: {0}", _resulta.strResultado));
                            }



                        }


                        break;

                    case "PIL":

                        v_respuesta = true;

                        if (Valida_WS)
                        {
                            ServicioEikon _servicio = new ServicioEikon();
                            _servicio.Usuario = usn.loginname;

                            var DEPOT = this.cmbDeposito.Items.FindByValue(cmbDeposito.SelectedValue.ToString().Trim());

                            RespuestaCastEikon ResultToken = _servicio.Peticion("TOKEN", string.Empty, string.Empty, DEPOT.Text);

                            if (ResultToken.isCorrect)
                            {
                                RespuestaCastEikon _result = _servicio.Peticion("VALIDA_CAST", txtcontenedor.Text, ResultToken.strToken.accessToken, DEPOT.Text);

                                if (_result.isCorrect)
                                {
                                    CultureInfo enUS = new CultureInfo("en-US");
                                    DateTime fechaCast;

                                    string swFechaCast = _result.ecasfechamaxsalida.ToString("dd/MM/yyyy HH:mm");
                                    string ecaswarehousedevolucion = _result.ecaswarehousedevolucion;
                                    this.tipocontenedor = _result.tipocontenedor;
                                    this.numerobl = _result.numerobl;

                                    if (!string.IsNullOrEmpty(ecaswarehousedevolucion))
                                    {
                                        switch (DEPOT.ToString()) 
                                        {
                                            case "SAV-DER":

                                                if (!ecaswarehousedevolucion.Trim().Equals("9025")) 
                                                {
                                                    cmbDeposito.SelectedValue = "5";

                                                    this.populateRepeater(1);

                                                    //v_respuesta = false;
                                                    //dpturno.Items.Clear();
                                                    //this.Alerta(string.Format("El contenedor: {0} no habilitado para el depósito :{1}", txtcontenedor.Text, DEPOT.ToString()));

                                                }

                                                break;

                                            case "ZAL/REPCONTVER":

                                                if (!ecaswarehousedevolucion.Trim().Equals("1246"))
                                                {
                                                    cmbDeposito.SelectedValue = "4";

                                                    this.populateRepeater(1);

                                                    //v_respuesta = false;
                                                    //dpturno.Items.Clear();
                                                    //this.Alerta(string.Format("El contenedor: {0} no habilitado para el depósito :{1}", txtcontenedor.Text, DEPOT.ToString()));
                                                }


                                                break;

                                        }
                                        

                                    }
                                    else 
                                    {
                                        v_respuesta = false;
                                        dpturno.Items.Clear();
                                        this.Alerta(string.Format("Servicios de PIL no estan devolviendo el datos de: ecaswarehousedevolucion {0}", ecaswarehousedevolucion));
                                    }


                                    if (!v_respuesta)
                                    {
                                        dpturno.Items.Clear();

                                    }
                                    else 
                                    {
                                        //CutOff*
                                        if (DateTime.TryParseExact(swFechaCast, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechaCast))
                                        {
                                            if (fechaCast < fecha)
                                            {
                                                v_respuesta = false;
                                                dpturno.Items.Clear();
                                                this.Alerta(string.Format(" La fecha de ingreso {0} es mayor a la fecha maxima permitida {1}", fecha, fechaCast));
                                            }
                                            else
                                            {
                                                v_respuesta = true;
                                            }
                                        }
                                        else
                                        {
                                            v_respuesta = false;
                                            this.sinresultado.Attributes["class"] = string.Empty;
                                            this.sinresultado.InnerText = "La fecha Cast SW EIKON - tiene un formato de fecha no admitido.";
                                            this.sinresultado.Attributes["class"] = "msg-critico";
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    v_respuesta = false;
                                    dpturno.Items.Clear();
                                    this.Alerta(string.Format("Respuesta SW EIKON: {0}" , _result.strResultado));
                                }
                            }
                            else
                            {
                                v_respuesta = false;
                                dpturno.Items.Clear();
                                this.Alerta(string.Format("Respuesta SW EIKON: {0}", ResultToken.strResultado));
                            }
                        }

                        break;


                    case "COS":

                        v_respuesta = true;

                        if (Valida_WS)
                        {
                            ServicioCosco _servicio = new ServicioCosco();
                            _servicio.Usuario = usn.loginname;

                            RespuestaCosco ResultToken = _servicio.Peticion("TOKEN", string.Empty, string.Empty);

                            if (ResultToken.isCorrect)
                            {
                                RespuestaCosco _result = _servicio.Peticion("VALIDA_CAST", txtcontenedor.Text, ResultToken.strToken.token);

                                if (_result.isCorrect)
                                {
                                    CultureInfo enUS = new CultureInfo("en-US");
                                    DateTime fechaCast;

                                    string swFechaCast = _result.DateExpiry.ToString("dd/MM/yyyy HH:mm");

                                    //CutOff*
                                    if (DateTime.TryParseExact(swFechaCast, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechaCast))
                                    {
                                        if (fechaCast < fecha)
                                        {
                                            v_respuesta = false;
                                            dpturno.Items.Clear();
                                            this.Alerta(string.Format(" La fecha de ingreso {0} es mayor a la fecha maxima permitida {1}", fecha, fechaCast));
                                        }
                                        else
                                        {
                                            v_respuesta = true;
                                        }
                                    }
                                    else
                                    {
                                        v_respuesta = false;
                                        this.sinresultado.Attributes["class"] = string.Empty;
                                        this.sinresultado.InnerText = "La fecha Cast SW COSCO - tiene un formato de fecha no admitido.";
                                        this.sinresultado.Attributes["class"] = "msg-critico";
                                    }
                                }
                                else
                                {
                                    v_respuesta = false;
                                    dpturno.Items.Clear();
                                    string cmensaje = "El contenedor ingresado no tiene fecha de CAS asignado, o la unidad no existe.";
                                    this.Alerta(string.Format("Respuesta SW COSCO: {0}" , cmensaje));
                                }
                            }
                            else
                            {
                                v_respuesta = false;
                                dpturno.Items.Clear();
                                this.Alerta(string.Format("Respuesta SW COSCO: {0}" , ResultToken.strResultado));
                            }
                        }


                        break;

                    default:

                        v_respuesta = true;

                        if (Valida_WS)
                        {
                            ServicioMarGlobal _servicio = new ServicioMarGlobal();
                            _servicio.Usuario = usn.loginname;

                            RespuestaCast ResultToken = _servicio.Peticion("TOKEN", string.Empty, string.Empty);

                            if (ResultToken.isCorrect)
                            {
                                RespuestaCast _result = _servicio.Peticion("VALIDA_CAST", txtcontenedor.Text, ResultToken.strResultado);

                                if (_result.isCorrect)
                                {
                                    CultureInfo enUS = new CultureInfo("en-US");
                                    DateTime fechaCast;

                                    string swFechaCast = _result.DateExpiry.ToString("dd/MM/yyyy HH:mm");

                                    //CutOff*
                                    if (DateTime.TryParseExact(swFechaCast, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechaCast))
                                    {
                                        if (fechaCast < fecha)
                                        {
                                            v_respuesta = false;
                                            dpturno.Items.Clear();
                                            this.Alerta(string.Format(" La fecha de ingreso {0} es mayor a la fecha maxima permitida {1}", fecha, fechaCast));
                                        }
                                        else
                                        {
                                            //valida línea naviera sea la misma seleccionada
                                            string _CustomsLineCode = string.IsNullOrEmpty(_result.CustomsLineCode) ? "" : _result.CustomsLineCode.Trim();
                                            string _LineaWS = string.Empty;

                                            if (!_CustomsLineCode.Equals(CODIGOADUANA))
                                            {
                                                v_respuesta = false;
                                                dpturno.Items.Clear();

                                                //obtiene linea en base al código aduanero
                                                string OError;
                                                objLineas = new Cls_Bil_Sav_Lineas();
                                                objLineas.CODIGOADUANA = _CustomsLineCode;

                                                if (!objLineas.PopulateMyData(out OError))
                                                {
                                                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                                                }
                                                else
                                                {
                                                    _LineaWS = objLineas.DESCRIPCION;
                                                    this.Alerta(string.Format("El contenedor ingresado: {0} no pertenece a la línea seleccionada: {1}, debe seleccionar la línea: {2}", _result.Container, dplinea.SelectedValue, _LineaWS));
                                                }

                                            }
                                            else 
                                            {
                                                v_respuesta = true;
                                            }
                                            
                                        }
                                    }
                                    else
                                    {
                                        v_respuesta = false;
                                        this.sinresultado.Attributes["class"] = string.Empty;
                                        this.sinresultado.InnerText = "La fecha Cast SW MarGlobal - tiene un formato de fecha no admitido.";
                                        this.sinresultado.Attributes["class"] = "msg-critico";
                                    }
                                }
                                else
                                {
                                    v_respuesta = false;
                                    dpturno.Items.Clear();
                                    this.Alerta(string.Format("Respuesta SW MarGlobal: {0}" , _result.strResultado));
                                }
                            }
                            else
                            {
                                v_respuesta = false;
                                dpturno.Items.Clear();
                                this.Alerta(string.Format("Respuesta SW MarGlobal: {0}" , ResultToken.strResultado));
                            }
                        }

                        break;

                }


                //if (dplinea.SelectedValue != "HSD")
                //{
                //    v_respuesta = true;

                //    if (Valida_WS)
                //    {
                //        ServicioMarGlobal _servicio = new ServicioMarGlobal();
                //        _servicio.Usuario = usn.loginname;

                //        RespuestaCast ResultToken = _servicio.Peticion("TOKEN", string.Empty, string.Empty);

                //        if (ResultToken.isCorrect)
                //        {
                //            RespuestaCast _result = _servicio.Peticion("VALIDA_CAST", txtcontenedor.Text, ResultToken.strResultado);

                //            if (_result.isCorrect)
                //            {
                //                CultureInfo enUS = new CultureInfo("en-US");
                //                DateTime fechaCast;

                //                string swFechaCast = _result.DateExpiry.ToString("dd/MM/yyyy HH:mm");

                //                //CutOff*
                //                if (DateTime.TryParseExact(swFechaCast, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechaCast))
                //                {
                //                    if (fechaCast < fecha)
                //                    {
                //                        v_respuesta = false;
                //                        dpturno.Items.Clear();
                //                        this.Alerta(string.Format(" La fecha de ingreso {0} es mayor a la fecha maxima permitida {1}", fecha, fechaCast));
                //                    }
                //                    else
                //                    {
                //                        v_respuesta = true;
                //                    }
                //                }
                //                else
                //                {
                //                    v_respuesta = false;
                //                    this.sinresultado.Attributes["class"] = string.Empty;
                //                    this.sinresultado.InnerText = "La fecha Cast SW MarGlobal - tiene un formato de fecha no admitido.";
                //                    this.sinresultado.Attributes["class"] = "msg-critico";
                //                }
                //            }
                //            else
                //            {
                //                v_respuesta = false;
                //                dpturno.Items.Clear();
                //                this.Alerta(string.Format("Respuesta SW MarGlobal: " + _result.strResultado));
                //            }
                //        }
                //        else
                //        {
                //            v_respuesta = false;
                //            dpturno.Items.Clear();
                //            this.Alerta(string.Format("Respuesta SW MarGlobal: " + ResultToken.strResultado));
                //        }
                //    }

                       
                //}


                //if (dplinea.SelectedValue == "HSD") 
                //{
                //    v_respuesta = true;

                //    if (Valida_WS)
                //    {

                //        ServicioHamburg _servicioh = new ServicioHamburg();
                //        _servicioh.Usuario = usn.loginname;

                //        //RespuestaCast ResultToken = _servicio.Peticion("TOKEN", string.Empty, string.Empty);

                //        //if (ResultToken.isCorrect)
                //        //{
                //        RespuestaHamburgCast<MessageHS> _resulta = _servicioh.Peticion("VALIDA_CAST", txtcontenedor.Text);

                //        if (_resulta.Success)
                //        {
                //            CultureInfo enUS = new CultureInfo("en-US");
                //            DateTime fechaCast;

                //            string swFechaCast = _resulta.Message.DateExpiry.ToString("dd/MM/yyyy HH:mm");

                //            //CutOff*
                //            if (DateTime.TryParseExact(swFechaCast, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fechaCast))
                //            {
                //                if (fechaCast < fecha)
                //                {
                //                    v_respuesta = false;
                //                    dpturno.Items.Clear();
                //                    this.Alerta(string.Format(" La fecha de ingreso {0} es mayor a la fecha maxima permitida {1}", fecha, fechaCast));
                //                }
                //                else
                //                {
                //                    v_respuesta = true;
                //                }
                //            }
                //            else
                //            {
                //                v_respuesta = false;
                //                this.sinresultado.Attributes["class"] = string.Empty;
                //                this.sinresultado.InnerText = "La fecha Cast SW HamburgSUD - tiene un formato de fecha no admitido.";
                //                this.sinresultado.Attributes["class"] = "msg-critico";
                //            }
                //        }
                //        else
                //        {
                //            v_respuesta = false;
                //            dpturno.Items.Clear();
                //            this.Alerta(string.Format("Respuesta SW HamburgSUD: " + _resulta.strResultado));
                //        }



                //    }
                //}


              

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "aisv_sav.aspx", "validarTurno()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
            return v_respuesta;
        }
    }
}