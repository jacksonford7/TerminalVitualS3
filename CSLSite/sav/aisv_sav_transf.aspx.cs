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
using BillionEntidades;

namespace CSLSite
{
    public partial class aisv_sav_transf : System.Web.UI.Page
    {
        #region "Nuevas Referencias"
        private ServiciosSav objServicios = new ServiciosSav();
        public static string v_mensaje = string.Empty;
        private SaldoProforma objProforma = new SaldoProforma();
        private Preaviso objPases = new Preaviso();
        private Cls_Bil_Sav_Lineas objLineas = new Cls_Bil_Sav_Lineas();
        private Decimal nSaldo_Anterior = 0;
        private Decimal nSaldo_Actual = 0;
        private Decimal nNuevo_Saldo = 0;
        private Decimal nTotal_Pagar = 0;
        private Decimal nSaldo_Validar = 0;
        private Decimal nValor_Pase = 0;
        private Decimal nTotal_Pase = 0;
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
               this.IsCompatibleBrowser();
               Page.SslOn();

                this.LlenaComboDepositos();
                this.LlenaComboLineas();
                
               
                var user = Page.Tracker();
                /*si es usuario de credito sale de la pantalla*/
                if (user != null)
                {
                    bool Credito = usuario.Credito(user.ruc);
                    //validaciones usuario de contado
                    if (Credito)
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario De Crédito "), "aisv_sav_transf", "Page_Init", user.ruc, user.loginname);
                        this.PersonalResponse("Su cuenta de usuario no tiene permisos para esta opción, ya que usted es un cliente de crédito., arregle esto primero con Customer Services", "../cuenta/menu.aspx", true);
                    }       
                }
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
                    dplinea.Items.Clear();

                    dplinea.DataSource = man_pro_expo.consultaLineas_Sav(ID_DEPORT);
                    dplinea.DataValueField = "ID";
                    dplinea.DataTextField = "DESCRIPCION";
                    dplinea.DataBind();
                    dplinea.Enabled = true;

                    dpturno.Items.Clear();
                    dpturno.DataSource = null;
                    dpturno.DataBind();

                    var usn = new usuario();
                    usn = this.getUserBySesion();
                    if (usn != null) 
                    {
                        this.populateSaldos(usn.ruc);
                    }
                       
                    this.populateRepeater(1);

                   

                }


            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "aisv_sav_transf.aspx", "cmbDeposito_SelectedIndexChanged()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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
                    var usn = new usuario();
                    usn = this.getUserBySesion();
                    if (usn != null)
                    {
                        this.populateSaldos(usn.ruc);
                    }

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

                    cmbDeposito_SelectedIndexChanged(sender, e);

                    bool Credito = usuario.Credito(usn.ruc);
                    if (Credito)
                    {
                        this.xfinder.Visible = false;
                        this.xfinder2.Visible = false;
                    }
                    else
                    {
                        this.xfinder.Visible = true;
                        this.xfinder2.Visible = true;

                        this.populateSaldos(usn.ruc);
                        this.populateRepeater(1);
                       

                    }


                    

                }
                else
                {
                    this.xfinder.Visible = false;

                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "aisv_sav_transf", "Page_Load", "No usuario", Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }

                //LlenaComboDepositos();
                //this.LlenaComboLineas();

            }
            this.txtidentidad.InnerText = this.dv_licencia.Value;
            this.txtconductor.InnerText = this.dv_nombre.Value;

        }

        protected void btbuscar_Click(object sender, EventArgs e)
        {
            DateTime fecha = new DateTime();
            CultureInfo enUS = new CultureInfo("en-US");
            if (!DateTime.TryParseExact(fecsalida.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}", fecsalida.Text));
                fecsalida.Focus();
                return;
            }

            try
            {
                if (Response.IsClientConnected)
                {
                    //variables proforma
                    //CultureInfo enUS = new CultureInfo("es-US");
                    int Cantidad = 0;
                    Decimal Total = 0;
                    Decimal TotalSaldo = 0;
                    
                    string IdProforma = string.Empty;
                    string IdLiquidacion = string.Empty;
                    string SecuenciaProforma = string.Empty;
                   
                    int AC = 1;
                    Int64 idPreaviso = 0; 

                    var user = this.getUserBySesion();
                    if (user.loginname == null || user.loginname.Trim().Length <= 0)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El usuario era null!"), "aisv_sav", "btbuscar_Click", "SAV", user.loginname));
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

                    
                    //Si todo esta listo envío el trabajo al servicio de preavisos y el resto es de el...
                    //se debe grabar abajo se debe imprimir.
                    var mailes = string.Empty;
                    if ( txtMailAdicional.Value != null && txtMailAdicional.Value.Trim().Length > 0)
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
                    string cMensajes;

                    Int64 ID_DEPORT;

                    if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                    {
                        ID_DEPORT = 0;
                    }

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

                    //////bool Valida_WS = false;
                    //////if (Operadora == "MSC")
                    //////{
                    //////    string cMensajes;
                    //////    List<Cls_Bil_Configuraciones> ValidaSMC = Cls_Bil_Configuraciones.Get_Validacion("VALIDA_MSC", out cMensajes);
                    //////    if (!String.IsNullOrEmpty(cMensajes))
                    //////    {
                    //////        this.sinresultado.Attributes["class"] = string.Empty;
                    //////        this.sinresultado.Attributes["class"] = "msg-critico";
                    //////        this.sinresultado.InnerText = string.Format("Informativo! Error en configuraciones. {0}", cMensajes);
                    //////        return;
                    //////    }

                    //////    bool Valida_SMC = false;
                    //////    if (ValidaSMC.Count != 0)
                    //////    {
                    //////        Valida_SMC = true;
                    //////    }

                    //////    if (Valida_SMC)
                    //////    {
                    //////        List<Cls_Bil_CabeceraMsc> FechaDespacho = Cls_Bil_CabeceraMsc.Despacho_Contenedor(this.txtcontenedor.Text.Trim(), out cMensajes);
                    //////        if (!String.IsNullOrEmpty(cMensajes))
                    //////        {
                    //////            this.sinresultado.Attributes["class"] = string.Empty;
                    //////            this.sinresultado.Attributes["class"] = "msg-critico";
                    //////            this.sinresultado.InnerText = string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener contenedor de  despacho....{0}", cMensajes);
                    //////            return;
                    //////        }

                    //////        if (FechaDespacho.Count == 0)
                    //////        {
                    //////            dpturno.Items.Clear();

                    //////            this.sinresultado.Attributes["class"] = string.Empty;
                    //////            this.sinresultado.Attributes["class"] = "msg-critico";
                    //////            this.sinresultado.InnerText = string.Format("No existe información del contenedor: {0} ingresado...", this.txtcontenedor.Text.Trim());
                    //////            return;

                    //////        }

                    //////    }
                    //////}
                    //fin



                    //if (string.IsNullOrEmpty(this.bk_iso.Value) || string.IsNullOrEmpty(this.bk_linea.Value) || string.IsNullOrEmpty(this.bk_num.Value) )
                    //{
                    //    this.sinresultado.Attributes["class"] = string.Empty;
                    //    this.sinresultado.Attributes["class"] = "msg-critico";
                    //    this.sinresultado.InnerText = "Por favor seleccione los datos del booking";
                    //    return;
                    //}
                    //VALIDAR DATOS----->
                    //CHOFER------->AISV
                    bool choferlock = jAisvContainer.IsDriverLock(this.dv_licencia.Value);
                    if (choferlock)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("El conductor con licencia {0} esta inhabilitado para ingresar a la terminal", this.dv_licencia.Value);
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
                    //var ru = jAisvContainer.UnidadRetornable(txtcontenedor.Text.Trim(), out mens);
                   /* if (ru == null)
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
                        this.sinresultado.InnerText = string.Format("Unidad {0} no cuenta con autorizacion de retorno [CAT]", txtcontenedor.Text);
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
                    }*/

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
                    if (Credito)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = "Clientes de crédito no pueden realizar transferencias.";
                        return;
                    }
                    else
                    {

                        var strSaldoAnterior = ViewState["saldo_anterior"] != null ? ViewState["saldo_anterior"].ToString() : string.Empty;
                        if (!Decimal.TryParse(strSaldoAnterior, out nSaldo_Anterior))
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = string.Format("El saldo Anterior no puede ser cero: {0} ", nSaldo_Anterior);
                            sinresultado.Visible = true;
                            return;
                        }

                        var strQty = ViewState["cantidad"] != null ? ViewState["cantidad"].ToString() : string.Empty;

                        if (!int.TryParse(strQty, out Cantidad) || Cantidad <= 0)
                        {
                            xfinder.Visible = false;
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = "No existen valores pendiente para poder realizar la transferencia";
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

                        /*saco nuevamente el saldo actual - para validar*/
                        List<SaldoProforma> Lista = SaldoProforma.Get_Saldo_Repcontver(user.ruc, ID_DEPORT);

                        if (Lista != null) 
                        {
                            var xList = Lista.FirstOrDefault();
                            if (xList != null)
                            {
                                nSaldo_Validar = xList.saldo_final;/*saldo actual*/

                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.Visible = true;
                                this.sinresultado.InnerText = xList.leyenda;

                                if (nSaldo_Validar < nSaldo_Anterior)
                                {
                                    this.sinresultado.InnerText = string.Format("No tiene saldo disponible para realizar la transferencia de valores, debe volver a generar los turnos, saldo actual: {0}", nSaldo_Validar);
                                    sinresultado.Visible = true;
                                    return;
                                }
                            }
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

                        objPases = Session["DetallePasesSav"] as Preaviso;
                        if (objPases == null)
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = "Ocurrió un error, no se pudo obtener Session[DetallePasesSav]";
                            return;

                        }

                        var results = from p in objPases.Detalle.AsEnumerable()
                                      where p.fila == AC
                                      select new
                                      {
                                          PASE = p.id
                                      };

                        foreach (var item in results)
                        {
                            idPreaviso = item.PASE;
                        }


                        //actualizo el saldo
                        var cMensaje = SaldoProforma.Actualiza_Saldo(idPreaviso);
                        if (cMensaje != string.Empty)
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = string.Format("Error al actualizar saldo: {0}", cMensaje);
                            return;
                        }

                        List<SaldoProforma> ListSaldo = SaldoProforma.Lista_Saldo(idPreaviso, out v_mensaje);
                        if (ListSaldo != null && string.IsNullOrEmpty(v_mensaje))
                        {
                            foreach (var Det in ListSaldo)
                            {
                                IdProforma = Det.idproforma.ToString().Trim();
                                IdLiquidacion = Det.liquidacion;
                                SecuenciaProforma = IdProforma;
                                TotalSaldo = Det.total;
                            }
                        }
                        else
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = v_mensaje;

                            //reversa saldo
                            objProforma = new SaldoProforma();
                            objProforma.Reversar(idPreaviso,out v_mensaje);
                            if (v_mensaje != string.Empty)
                            {
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = v_mensaje;

                            }
                            return;
                        }

                        //valida que el turno seleccionado aun sigue vigente
                        TurnoSav turnos = new TurnoSav();
                        DataTable v_dataTurno = new DataTable();

                        if (int.Parse(cmbDeposito.SelectedValue.ToString()) == 5)
                        {
                            string pError;
                            var sc = new TurnosSAV.ServicioPreavisosClient();
                            var tok = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");


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
                                this.Alerta(string.Format("Turno ya existe para la unidad {0}", txtcontenedor.Text.ToUpper()));
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

                            paseRev.creado_usuario = user.loginname; //usuario logueado


                            var rpm = paseRev.SaveTransaction(out pError);
                            if (!rpm.HasValue)
                            {
                                //reversa saldo
                                objProforma = new SaldoProforma();
                                objProforma.Reversar(idPreaviso, out v_mensaje);
                                if (v_mensaje != string.Empty)
                                {
                                    this.sinresultado.Attributes["class"] = string.Empty;
                                    this.sinresultado.Attributes["class"] = "msg-critico";
                                    this.sinresultado.InnerText = v_mensaje;

                                }

                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = pError;
                                return;
                            }

                            int Id = int.Parse(rpm.Value.ToString());//id transaccion

                            objProforma = new SaldoProforma();
                            objProforma.id_preaviso = Id;
                            objProforma.ruc = user.ruc;
                            objProforma.liquidacion = IdLiquidacion;
                            objProforma.idproforma = Int64.Parse(IdProforma);
                            objProforma.total = TotalSaldo;
                            objProforma.valorcruzado = 0;
                            objProforma.valorsaldo = TotalSaldo;
                            objProforma.usuario = user.loginname;
                            objProforma.Save(out v_mensaje);

                            if (v_mensaje != string.Empty)
                            {
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = string.Format("Error al grabar saldo de proforma pre-aviso: {0}", v_mensaje);
                                return;
                            }


                            var actualiza = sc.actualizar_turno(tok, Id, IdProforma, "Y", DateTime.Now);
                            if (actualiza <= 0)
                            {
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = string.Format("Error al actualizar pre-aviso: {0},  {1}", Id, "Falla al actualizar");
                                //return;                                    
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
                                            obj.Add("AV_BOOKING", string.Empty);//nbrboo.Value.ToString());//nbrboo.Value.ToString().Replace("_CISE", String.Empty));
                                            //obj.Add("AV_BL", oInformacion?.bl?.ToString().Trim());//nbrboo.Value.ToString());
                                            obj.Add("AV_CONTENEDOR", txtcontenedor.Text.ToUpper());
                                            //obj.Add("AV_ID_TIPO_CTNER", oInformacion?.tipo?.ToString().Trim());

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

                                            obj.Add("AV_CLIENTE_RUC", user.ruc);
                                            obj.Add("AV_CLIENTE_NOMBRE", user.nombres);
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
                                               
                                                sc = new TurnosSAV.ServicioPreavisosClient();


                                                var can = paseRev.cancelar_turno(sq, txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV", out pError);
                                               
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
                                       
                                       
                                        var sc1 = new TurnosSAV.ServicioPreavisosClient();

                                        var tok1 = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");

                                     
                                        var can = paseRev.cancelar_turno(sq, txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV", out pError);

                                        
                                        Exception exc = new Exception(" Booking:" + Id.ToString() + " - " + ex.Message);
                                        var number = log_csl.save_log<Exception>(exc, "aisv_sav", "btbuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                    }

                                }

                            }

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

                            uni.id = txtcontenedor.Text;
                            uni.linea = dplinea.SelectedValue;
                            uni.tamano = dptamano.SelectedValue;

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

                                //reversa saldo
                                objProforma = new SaldoProforma();
                                objProforma.Reversar(idPreaviso, out v_mensaje);
                                if (v_mensaje != string.Empty)
                                {
                                    this.sinresultado.Attributes["class"] = string.Empty;
                                    this.sinresultado.Attributes["class"] = "msg-critico";
                                    this.sinresultado.InnerText = v_mensaje;

                                }

                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = rpm.mensaje_principal;
                                return;
                            }

                            int Id = int.Parse(rpm.que_hacer);//id transaccion
                                                              //carga datos de preaviso-para proforma

                            objProforma = new SaldoProforma();
                            objProforma.id_preaviso = Id;
                            objProforma.ruc = user.ruc;
                            objProforma.liquidacion = IdLiquidacion;
                            objProforma.idproforma = Int64.Parse(IdProforma);
                            objProforma.total = TotalSaldo;
                            objProforma.valorcruzado = 0;
                            objProforma.valorsaldo = TotalSaldo;
                            objProforma.usuario = user.loginname;
                            objProforma.Save(out v_mensaje);
                            if (v_mensaje != string.Empty)
                            {
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = string.Format("Error al grabar saldo de proforma pre-aviso: {0}", v_mensaje);
                                return;
                            }


                            var actualiza = sc.actualizar_turno(tok, Id, IdProforma, "Y", DateTime.Now);
                            if (actualiza <= 0)
                            {
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = string.Format("Error al actualizar pre-aviso: {0},  {1}", Id, "Falla al actualizar");
                                //return;                                    
                            }

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
                                       // obj.Add("AV_BOOKING", string.Empty);//nbrboo.Value.ToString());//nbrboo.Value.ToString().Replace("_CISE", String.Empty));

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

                                       // obj.Add("AV_BL", oInformacion?.bl?.ToString().Trim());//nbrboo.Value.ToString());
                                        obj.Add("AV_CONTENEDOR", txtcontenedor.Text.ToUpper());
                                        //obj.Add("AV_ID_TIPO_CTNER", oInformacion?.tipo?.ToString().Trim());
                                        obj.Add("AV_CLIENTE_RUC", user.ruc);
                                        obj.Add("AV_CLIENTE_NOMBRE", user.nombres);
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
                                    
                                    //man_pro_expo.CancelarTurnoPorLinea(long.Parse(Id.ToString()), usr.loginname, out v_error);
                                    var sc1 = new TurnosSAV.ServicioPreavisosClient();

                                    var tok1 = sc.obtener_token(System.Configuration.ConfigurationManager.AppSettings["TOKEN_KEY"], System.Configuration.ConfigurationManager.AppSettings["TOKEN_PSW"]);//("farbem", "f@rB3m_2019");

                                    //var tok = sc.obtener_token("farbem", "f@rB3m_2019");
                                    var vt = sc.cancelar_turno_preavisado_simple(tok, Id.ToString(), txtcontenedor.Text.ToUpper(), user.loginname, "ANULACION POR AISV");


                                    Exception exc = new Exception(" Booking:" + Id.ToString() + " - " + ex.Message);
                                    var number = log_csl.save_log<Exception>(exc, "aisv_sav", "btbuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                }

                            }

                        }
                                              
                    }


                    this.populateSaldos(user.ruc);
                    this.populateRepeater(1);
                    //msg-info
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = string.Format("Se ha generado existosamente el Doc. No. {0}", sq);

                    //abrir una pantalla de impresion//
                    //Lanzar la impresion

                    var qs = QuerySegura.EncryptQueryString(sq);
                    string script = string.Format("window.open('../sav/printaisvsav.aspx?sid={0}',null,'height = 750,width = 850,status = yes,toolbar = no,menubar = no,location = no');", qs);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "openwindow", script, true);

                  


                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "aisv_sav_transf", "btbuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
            }

            
        }

        protected void btnconsultarturnos_Click(object sender, EventArgs e)
        {
            DateTime fecha = new DateTime();
            CultureInfo enUS = new CultureInfo("en-US");
            if (!DateTime.TryParseExact(fecsalida.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}", fecsalida.Text));
                fecsalida.Focus();
                return;
            }

            string Operadora = this.dplinea.SelectedValue.ToString();
            Int64 ID_DEPOT = Int64.Parse(this.cmbDeposito.SelectedValue.ToString());

            bool Valida_WS = false;
            string _CODIGOADUANA = string.Empty;

            string cMensajes;
            List<Cls_Bil_Sav_Lineas> LineaOperadora = Cls_Bil_Sav_Lineas.Consulta_Linea_Sav(Operadora, ID_DEPOT, out cMensajes);
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


            //////if (Operadora == "MSC")
            //////{
            //////    string cMensajes;
            //////    List<Cls_Bil_Configuraciones> ValidaSMC = Cls_Bil_Configuraciones.Get_Validacion("VALIDA_MSC", out cMensajes);
            //////    if (!String.IsNullOrEmpty(cMensajes))
            //////    {
            //////        this.Alerta(string.Format("Informativo! Error en configuraciones. {0}", cMensajes));
            //////        fecsalida.Focus();
            //////        return;
            //////    }

            //////    bool Valida_SMC = false;
            //////    if (ValidaSMC.Count != 0)
            //////    {
            //////        Valida_SMC = true;
            //////    }

            //////    if (Valida_SMC)
            //////    {

            //////        List<Cls_Bil_CabeceraMsc> FechaDespacho = Cls_Bil_CabeceraMsc.Despacho_Contenedor(this.txtcontenedor.Text.Trim(), out cMensajes);
            //////        if (!String.IsNullOrEmpty(cMensajes))
            //////        {
            //////            this.Alerta(string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener contenedor de despacho....{0}", cMensajes));
            //////            fecsalida.Focus();
            //////            return;
            //////        }

            //////        if (FechaDespacho.Count == 0)
            //////        {
            //////            dpturno.Items.Clear();

            //////            this.Alerta(string.Format("No existe información de despacho con el contenedor: {0} ingresado..", this.txtcontenedor.Text.Trim()));
            //////            fecsalida.Focus();
            //////            return;
            //////        }

            //////    }
            //////}

            if (validarTurno(fecha, Valida_WS, _CODIGOADUANA))
            {
                TurnoSav turnos = new TurnoSav();

                //se consulta los turnos habilitados 
                dpturno.DataSource = turnos.ConsultarHorariosDisponibles(fecha, int.Parse(cmbDeposito.SelectedValue.ToString())); //ds.Tables[0].DefaultView;
                dpturno.DataValueField = "ID";
                dpturno.DataTextField = "DESCRIPCION";
                dpturno.DataBind();
                dpturno.Enabled = true;
            }
            else { return; }

            if (dpturno.Items.Count== 0)
            {
                this.Alerta(string.Format(" No existe turnos vigentes para la fecha {0}", fecha));
            }
        }

        #region "Nuevos Metodos"

        public void LlenaComboLineas()
        {
            try
            {
                dplinea.DataSource = man_pro_expo.consultaLineas();
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

        public void populateRepeater(int cantidad)
        {
            try
            {

                string vr = string.Empty;
                int i = 1;
                nTotal_Pase = 0;

                /*saco los tipo de precios, pueden existir hasta dos tipos diferentes*/
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    Label LblValorPase = item.FindControl("lbl_saldo_pase") as Label;
                    var x = LblValorPase.Text;

                    if (!Decimal.TryParse(x, out nValor_Pase))
                    {
                        nValor_Pase = 0;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = "Por favor comuníquese con nosotros, al parecer el precio del pases presenta problemas.";
                        sinresultado.Visible = true;
                        return;
                    }

                    nTotal_Pase = nTotal_Pase + nValor_Pase;

                    if (cantidad == i) { break; }
                    i = i + 1;


                }

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

                //var table = ServiciosSav.List_Servicios(out vr);
                var table = ServiciosSav.List_Servicios_Recontvere(ID_DEPORT, ID_LINEA,out vr);

                foreach (var f in table)
                {

                    f.mensaje = string.Empty;
                    f.contenido = f.descripcion;
                    f.icont = true;
                    f.cantidad = cantidad;
                    f.vtotal = nTotal_Pase;
                    f.costo = f.costo;
                    objServicios.Detalle.Add(f);

                }

                Session["ServiciosSav"] = objServicios;

                tablaNueva.DataSource = table;
                tablaNueva.DataBind();

                var subt = table.Where(a => a.aplica).ToList().Sum(b => b.vtotal);
                sttsubtotal.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);
                sttal.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);


                if (subt == 0)
                {
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("No existen valores para poder realizar la transferencia.");
                    sinresultado.Visible = true;
                    xfinder.Visible = false;
                    return;
                }
                else 
                {
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Empty;
                    sinresultado.Visible = false;
                    xfinder.Visible = true;
                }

                ViewState["cantidad"] = null;
                ViewState["subtotal"] = null;
                ViewState["saldo_actual"] = null;
                ViewState["total"] = null;
                ViewState["cantidad"] = cantidad;

                var strSaldo_Anterior = ViewState["saldo_anterior"] != null ? ViewState["saldo_anterior"].ToString() : string.Empty;

                if (!Decimal.TryParse(strSaldo_Anterior, out nSaldo_Anterior) || nSaldo_Anterior <= 0)
                {
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("No existe un saldo anterior para poder transferir los valores: {0}", nSaldo_Anterior);
                    sinresultado.Visible = true;
                    return;
                }

                nSaldo_Actual = (subt - nSaldo_Anterior);
                if (nSaldo_Actual <= 0)
                {
                    nNuevo_Saldo = (nSaldo_Anterior - subt);
                    nTotal_Pagar = 0;
                }
                else
                {
                    nNuevo_Saldo = 0;
                    nTotal_Pagar = (subt - nSaldo_Anterior);
                }

                sttsaldo_anterior.InnerHtml = string.Format("<strong>{0:c}</strong>", nSaldo_Anterior);
                sttsubtotal.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);
                sttsaldo_actual.InnerHtml = string.Format("<strong>{0:c}</strong>", nNuevo_Saldo);
                sttal.InnerHtml = string.Format("<strong>{0:c}</strong>", nTotal_Pagar);

                ViewState["subtotal"] = subt;
                ViewState["saldo_anterior"] = nSaldo_Anterior;
                ViewState["saldo_actual"] = nNuevo_Saldo;
                ViewState["total"] = subt;

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

        public void populateSaldos(string ruc)
        {
            Int64 ID_DEPORT;

            if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
            {
                ID_DEPORT = 0;
            }

            /*CARGA DETALLE DE PASES A FAVOR DEL CLIENTE DE CONTADO*/
            List<Preaviso> ListPases = Preaviso.Detalle_Pases_Saldo_Repcontver(ruc, ID_DEPORT, out v_mensaje);
            if (ListPases != null)
            {
                tablePagination.DataSource = ListPases;
                tablePagination.DataBind();
                xfinder2.Visible = true;

                var subt = ListPases.Where(a => a.id != 0).ToList().Sum(b => b.saldo_pase);
                tot_saldo.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);

                ViewState["saldo_anterior"] = null;
                ViewState["saldo_anterior"] = subt;

                /*almacena el detalle de pases*/
                objPases = new Preaviso();
                Session["DetallePasesSav"] = objPases;
                objPases = Session["DetallePasesSav"] as Preaviso;

                foreach (var Lista2 in ListPases)
                {
                    objPases.Detalle.Add(Lista2);
                }

                Session["DetallePasesSav"] = objPases;
            }
            else
            {

                tablePagination.DataSource = null;
                tablePagination.DataBind();
                xfinder2.Visible = false;
                var subt = 0.00;
                tot_saldo.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);


                ViewState["saldo_anterior"] = null;
                ViewState["saldo_anterior"] = subt;

                objPases = new Preaviso();
                Session["DetallePasesSav"] = objPases;
                objPases = Session["DetallePasesSav"] as Preaviso;
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
                var number = log_csl.save_log<Exception>(ex, "aisv_sav_transf", "LlenaComboDepositos()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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
                                this.Alerta(string.Format("Respuesta SW HamburgSUD: " + _resulta.strResultado));
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

                                                    //var usn = new usuario();
                                                   // usn = this.getUserBySesion();
                                                    if (usn != null)
                                                    {
                                                        this.populateSaldos(usn.ruc);
                                                    }

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

                                                   // var usn = new usuario();
                                                    //usn = this.getUserBySesion();
                                                    if (usn != null)
                                                    {
                                                        this.populateSaldos(usn.ruc);
                                                    }

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
                                    this.Alerta(string.Format("Respuesta SW EIKON: " + _result.strResultado));
                                }
                            }
                            else
                            {
                                v_respuesta = false;
                                dpturno.Items.Clear();
                                this.Alerta(string.Format("Respuesta SW EIKON: " + ResultToken.strResultado));
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
                                    this.Alerta(string.Format("Respuesta SW COSCO: {0}", cmensaje));

                                    //v_respuesta = false;
                                    //dpturno.Items.Clear();
                                    //this.Alerta(string.Format("Respuesta SW COSCO: " + _result.strResultado.ToString()));
                                }
                            }
                            else
                            {
                                v_respuesta = false;
                                dpturno.Items.Clear();
                                this.Alerta(string.Format("Respuesta SW COSCO: " + ResultToken.strResultado));
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
                                    this.Alerta(string.Format("Respuesta SW MarGlobal: " + _result.strResultado));
                                }
                            }
                            else
                            {
                                v_respuesta = false;
                                dpturno.Items.Clear();
                                this.Alerta(string.Format("Respuesta SW MarGlobal: " + ResultToken.strResultado));
                            }
                        }
                        break;
                }

              

               
                
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "aisv_sav_transf", "validarTurno()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
            return v_respuesta;
        }
        #endregion

    }
}