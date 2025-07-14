using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using BillionEntidades;
using N4;
using N4Ws;
using N4Ws.Entidad;
using System.Xml.Linq;
using System.Xml;
using ControlPagos.Importacion;
using Salesforces;
using System.Data;
using System.Net;
using CSLSite;
using System.Text.RegularExpressions;
using CasManual;
using PasePuerta;
using ClsAppCgsa;

namespace CSLSite
{


    public partial class facturafreightforwarderp2d : System.Web.UI.Page
    {


        #region "Clases"
        private static Int64? lm = -3;
        private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        private Cls_Bil_Proforma_Cabecera objProforma = new Cls_Bil_Proforma_Cabecera();
        private Cls_Bil_Proforma_Detalle objDetalleProforma = new Cls_Bil_Proforma_Detalle();
        private Cls_Bil_Proforma_Servicios objServicios = new Cls_Bil_Proforma_Servicios();

        private Cls_Bil_Invoice_Cabecera objFactura = new Cls_Bil_Invoice_Cabecera();
        private Cls_Bil_Invoice_Detalle objDetalleFactura = new Cls_Bil_Invoice_Detalle();
        private Cls_Bil_Invoice_Servicios objServiciosFactura = new Cls_Bil_Invoice_Servicios();
        private Cls_Bil_Invoice_Validaciones objValidacion = new Cls_Bil_Invoice_Validaciones();

        private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        private Cls_Bil_Detalle objDetalle = new Cls_Bil_Detalle();
        private Cls_Bil_Invoice_Actualiza_Pase objActualiza_Pase = new Cls_Bil_Invoice_Actualiza_Pase();
        private List<Cls_Bil_AsumeFactura> List_Asume { set; get; }
        private List<Cls_Bil_Contenedor_DiasLibres> List_Contenedor { set; get; }
        //private List<Cls_Bil_Turnos> List_Turnos { set; get; }
        private List<Cls_Bil_Cas_Manual> List_Autorizacion { set; get; }
        private Cls_Bil_Log_Appcgsa objLogAppCgsa = new Cls_Bil_Log_Appcgsa();
        //private List<N4.Importacion.container> ContainersReefer { set; get; }
        private MantenimientoPaqueteCliente obj = new MantenimientoPaqueteCliente();
        private Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();

        private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();

        private P2D_Proforma_Cabecera objProformaCab = new P2D_Proforma_Cabecera();
        private P2D_Proforma_Detalle objProformaDet = new P2D_Proforma_Detalle();
        private P2D_Tarifario objTarifa = new P2D_Tarifario();
        private P2D_Tarja_Cfs objTarja = new P2D_Tarja_Cfs();
        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;

        private Int64 Gkey = 0;
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Cliente_CodigoSap = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;
        private int Fila = 1;
        private string TipoServicio = string.Empty;
        private DateTime FechaFactura;
        private string HoraHasta = "00:00";
        private string LoginName = string.Empty;
        private int NDiasLibreas = 0;
        private decimal NEstadoCuenta = 0;
        //private int NDiasZarpe = 0;
        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        private bool sap_valida = false;
        private bool tieneBloqueo = false;
        private decimal SaldoPendiente = 0;
        private decimal ValorVencido = 0;
        private decimal ValorPendiente = 0;
        private Int64 DiasCredito = 0;
        private decimal Cupo = 0;
        bool Bloqueo_Cliente = false;
        bool Liberado_Cliente = false;
        private string gkeyBuscado = string.Empty;

        //turnos
        //private DateTime FechaInicial;
        //private DateTime FechaFinal;
        //private DateTime FechaActualSalida;
        //private DateTime FechaMenosHoras;
        //private TimeSpan HorasDiferencia;
        //private int TotalHoras = 0;

        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
        private bool SinDesconsolidar = false;
        private bool SinAutorizacion = false;
        private bool Bloqueos = false;

        private static string TextoLeyenda = string.Empty;

        private static string TextoProforma = string.Empty;
        private static string TextoServicio = string.Empty;

        #endregion

        #region "Propiedades"

        private Int64? nSesion
        {
            get
            {
                return (Int64)Session["nSesion"];
            }
            set
            {
                Session["nSesion"] = value;
            }

        }



        #endregion

        #region "Metodos"

        private Boolean valida_email(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
            UPCARGA.Update();
            UPDATOSCLIENTE.Update();
          
            UPBOTONES.Update();

        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
        }

        private void Limpia_Datos_cliente()
        {
            this.TXTCLIENTE.Text = string.Empty;
            this.TXTASUMEFACTURA.Text = string.Empty;
        }

        private void Limpia_Asume_Factura()
        {
            this.TXTASUMEFACTURA.Text = string.Empty;
        }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)//cabecera
            {
                this.banmsg_Pase.Visible = false;
                this.banmsg_det.Visible = false;
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("1");
            }
            if (Tipo == 2)//detalle
            {
                this.banmsg_Pase.Visible = false;
                this.banmsg_det.Visible = true;
                this.banmsg.Visible = false;
                this.banmsg_det.InnerHtml = Mensaje;
                OcultarLoading("2");
            }

            if (Tipo == 3)//alerta
            {
                this.banmsg_det.Visible = false;
                this.banmsg.Visible = false;
                this.banmsg_Pase.Visible = true;
                this.banmsg_Pase.InnerHtml = Mensaje;
                OcultarLoading("2");
            }

            if (Tipo == 4)//ambos
            {
                this.banmsg_Pase.Visible = false;
                this.banmsg_det.Visible = true;
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                this.banmsg_det.InnerHtml = Mensaje;
                OcultarLoading("1");
            }

            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.banmsg_Pase.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.banmsg_Pase.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
            OcultarLoading("2");
        }

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCabecera = new Cls_Bil_Cabecera();
            Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;
        }


        private void Enviar_Caso_Salesforce(string pUsuario, string pruc, string pModulo, string pNovedad, string pErrores, string pValor1, string pValor2, string pValor3, out string Mensaje, bool bloqueo=false)
        {
            /*************************************************************************************************************************************
            * crear caso salesforce
            * **********************************************************************************************************************************/
            Mensaje = string.Empty;

            try
            {

                Salesforces.Ticket tk = new Ticket();

                tk.Tipo = "ERROR"; //debe ser: Error, Sugerencia, Queja, Problema, Otros
                tk.Categoria = "IMPO"; //solo puede ser: Impo,Expo,Otros
                tk.Usuario = pUsuario.Trim(); //login
                tk.Ruc = pruc.Trim(); //login ruc
                tk.PalabraClave = "CasoImpo"; //Opcional es una palabra clave para agrupar
                tk.Copias = "desarrollo@cgsa.com.ec";//opcional es para enviar copia de respaldo
                tk.Aplicacion = "Billion"; //obligatorio
                tk.Modulo = pModulo;//opcional

                var detalle_carga = new SaleforcesContenido();
                detalle_carga.Categoria = TipoCategoria.Importacion; //opcional
                detalle_carga.Tipo = TipoCarga.CFS; //opcional
                detalle_carga.Titulo = "Modulo de Facturación Importación CFS"; //opcional
                detalle_carga.Novedad = pNovedad; //mensaje del modulo o error

                detalle_carga.Detalles.Add(new DetalleCarga("Errores:", MensajesErrores));

                if (!string.IsNullOrEmpty(pValor1)) { detalle_carga.Detalles.Add(new DetalleCarga("BL", pValor1));  }
                if (!string.IsNullOrEmpty(pValor2)) { detalle_carga.Detalles.Add(new DetalleCarga("Cliente", pValor2)); }
                if (!string.IsNullOrEmpty(pValor3)) { detalle_carga.Detalles.Add(new DetalleCarga("Agente", pValor3)); }

                //asi puedes poner los campos que desees o se necesiten sobre la carga

                tk.Contenido = detalle_carga.ToString();

                var rt = tk.NuevoCaso();
                if (rt.Exitoso)
                {
                    if (bloqueo)
                    {
                        Mensaje = string.Format("Se ha generado una notificación a nuestra área de Tesorería para la revisión de su caso #  {0} ...Para mayor información: Servicio al cliente: ec.sac@contecon.com.ec lunes a domingo 7h00 a 23h00..Tesorería: lunes a viernes 8h00 a 16h30...Teléfonos (04) 6006300 - 3901700", rt.Resultado);

                    }
                    else
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del caso generado #  {0} ...Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.", rt.Resultado);

                    }
                }
                else
                {
                    if (bloqueo)
                    {
                        Mensaje = string.Format("Se ha generado una notificación a nuestra área de Tesorería para que realicen las respectivas revisiones del problema {0} ...Para mayor información: Servicio al cliente: ec.sac@contecon.com.ec lunes a domingo 7h00 a 23h00..Tesorería: lunes a viernes 8h00 a 16h30...Teléfonos (04) 6006300 - 3901700", rt.MensajeProblema);

                    }
                    else
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del problema {0} ....Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00. ", rt.MensajeProblema);
                    }
                }

            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;

            }


            /*************************************************************************************************************************************
            * fin caso salesforce
            * **********************************************************************************************************************************/

        }


        private static string leyenda_carbononeutro()
        {
            List<Cls_Bil_Configuraciones> Leyenda = Cls_Bil_Configuraciones.Parametros(out TextoLeyenda);
            if (!String.IsNullOrEmpty(TextoLeyenda))
            {
                return string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en configuraciones.</br> {0} ", TextoLeyenda);
            }

            var LinqLeyenda = (from Tope in Leyenda.Where(Tope => Tope.NOMBRE.Equals("LEYENDA_CFS"))
                               select new
                               {
                                   VALOR = Tope.VALOR == null ? string.Empty : Tope.VALOR
                               }).FirstOrDefault();

            if (LinqLeyenda != null)
            {
                return LinqLeyenda.VALOR == null ? "" : LinqLeyenda.VALOR;
            }


            return "";
        }

        private static string leyenda_proforma_p2d()
        {
            List<Cls_Bil_Configuraciones> Leyenda = Cls_Bil_Configuraciones.Parametros(out TextoLeyenda);
            if (!String.IsNullOrEmpty(TextoLeyenda))
            {
                return string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en configuraciones.</br> {0} ", TextoLeyenda);
            }

            var LinqLeyenda = (from Tope in Leyenda.Where(Tope => Tope.NOMBRE.Equals("TEXTO_P2D_FAC"))
                               select new
                               {
                                   VALOR = Tope.VALOR == null ? string.Empty : Tope.VALOR
                               }).FirstOrDefault();

            if (LinqLeyenda != null)
            {
                return LinqLeyenda.VALOR == null ? "" : LinqLeyenda.VALOR;
            }


            return "";
        }

        private static string leyenda_servicio_p2d()
        {
            List<Cls_Bil_Configuraciones> Leyenda = Cls_Bil_Configuraciones.Parametros(out TextoLeyenda);
            if (!String.IsNullOrEmpty(TextoLeyenda))
            {
                return string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en configuraciones.</br> {0} ", TextoLeyenda);
            }

            var LinqLeyenda = (from Tope in Leyenda.Where(Tope => Tope.NOMBRE.Equals("TEXTO_P2D_FACT_SERV"))
                               select new
                               {
                                   VALOR = Tope.VALOR == null ? string.Empty : Tope.VALOR
                               }).FirstOrDefault();

            if (LinqLeyenda != null)
            {
                return LinqLeyenda.VALOR == null ? "" : LinqLeyenda.VALOR;
            }


            return "";
        }

   
        #endregion

        #region "Eventos del formulario"

        #region "Eventos Page"

        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    Page.SslOn();
                }

                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);

                    return;
                }

                this.banmsg.Visible = IsPostBack;
                this.banmsg_det.Visible = IsPostBack;
                this.banmsg_Pase.Visible = IsPostBack;

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;
                    this.banmsg_Pase.InnerText = string.Empty;
                }

               
                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {
                    if (!Page.IsPostBack)
                    {
                        /*para almacenar clientes que asumen factura*/
                        List_Asume = new List<Cls_Bil_AsumeFactura>();
                        List_Asume.Clear();
                        List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = string.Empty, nombre = string.Empty, mostrar = string.Format("{0}", "Seleccionar...") });

                        numero_carga = QuerySegura.DecryptQueryString(Request.QueryString["ID_CARGA"]);
                        if (numero_carga == null || string.IsNullOrEmpty(numero_carga))
                        {
                            this.TXTMRN.Text = string.Empty;
                            this.TXTMSN.Text = string.Empty;
                            if (string.IsNullOrEmpty(this.TXTHSN.Text))
                            { this.TXTHSN.Text = string.Format("{0}", "0000"); }
                            this.TXTCLIENTE.Text = string.Empty;
                            this.TXTASUMEFACTURA.Text = string.Empty;
                            this.TXTAGENCIA.Text = string.Empty;
                            this.CboAsumeFactura.Items.Clear();

                            this.Actualiza_Paneles();
                        }
                        else
                        {
                            numero_carga = numero_carga.Trim().Replace("\0", string.Empty);
                            if (numero_carga.Split('+').ToList().Count > 0)
                            {
                                this.TXTMRN.Text = numero_carga.Split('+').ToList()[0].Trim();
                                this.TXTMSN.Text = numero_carga.Split('+').ToList()[1].Trim();
                                this.TXTHSN.Text = numero_carga.Split('+').ToList()[2].Trim();
                                this.TXTCLIENTE.Text = string.Format("{0} - {1}",numero_carga.Split('+').ToList()[5].Trim(), numero_carga.Split('+').ToList()[6].Trim());
                                this.TXTASUMEFACTURA.Text = numero_carga.Split('+').ToList()[3].Trim();
                                this.TXTAGENCIA.Text = numero_carga.Split('+').ToList()[4].Trim();

                                this.hf_idagente.Value = this.TXTAGENCIA.Text.Trim();
                                this.hf_idcliente.Value = numero_carga.Split('+').ToList()[5].Trim();
                                this.hf_idasume.Value = this.TXTASUMEFACTURA.Text.Trim();
                                this.hf_desccliente.Value = numero_carga.Split('+').ToList()[6].Trim();


                                var Agente = N4.Entidades.Agente.ObtenerAgente(ClsUsuario.loginname, this.TXTAGENCIA.Text);
                                if (Agente.Exitoso)
                                {
                                    var ListaAgente = Agente.Resultado;
                                    if (ListaAgente != null)
                                    {
                                        this.TXTAGENCIA.Text = string.Format("{0} - {1}", ListaAgente.ruc.Trim(), ListaAgente.nombres.Trim());
                                        //agrego agente si no es importador
                                        if (!this.hf_idcliente.Value.Trim().Equals(ClsUsuario.ruc.Trim()))
                                        {
                                            List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = ListaAgente.ruc.Trim(), nombre = ListaAgente.nombres.Trim(), mostrar = string.Format("{0} - {1}", ListaAgente.ruc.Trim(), ListaAgente.nombres.Trim()) });
                                        }

                                    }
                                    else
                                    {
                                        this.TXTAGENCIA.Text = string.Empty;
                                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del agente</b>"));

                                    }
                                }
                                else
                                {
                                    this.TXTAGENCIA.Text = string.Empty;
                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del agente</b>"));

                                }

                                var Cliente = N4.Entidades.Cliente.ObtenerCliente(ClsUsuario.loginname, this.hf_idcliente.Value);
                                if (Cliente.Exitoso)
                                {
                                    var ListaCliente = Cliente.Resultado;
                                    if (ListaCliente != null)
                                    {
                                        this.TXTCLIENTE.Text = string.Format("{0} - {1}", ListaCliente.CLNT_CUSTOMER.Trim(), ListaCliente.CLNT_NAME.Trim());
                                        this.TXTASUMEFACTURA.Text = string.Format("{0} - {1}", ListaCliente.CLNT_CUSTOMER.Trim(), ListaCliente.CLNT_NAME.Trim());

                                        //asume factura
                                        List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = ListaCliente.CLNT_CUSTOMER.Trim(), nombre = ListaCliente.CLNT_NAME.Trim(), mostrar = string.Format("{0} - {1}", ListaCliente.CLNT_CUSTOMER.Trim(), ListaCliente.CLNT_NAME.Trim()) });

                                    }
                                    else
                                    {
                                        this.Limpia_Datos_cliente();
                                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente</b>"));

                                    }
                                }
                                else
                                {
                                    this.Limpia_Datos_cliente();
                                    List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = this.hf_idcliente.Value, nombre = this.hf_desccliente.Value, mostrar = string.Format("{0} - {1}", this.hf_idcliente.Value, this.hf_desccliente.Value) });

                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro....Casilla de atención: ec.sac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", this.hf_idcliente.Value, this.hf_desccliente.Value));

                                }

                                /*verifica si la carga tiene mas personas que van asumir la carga*/
                                var Resultado = PagoAsignado.ListaAsignacionPartida(ClsUsuario.loginname.Trim(), this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());
                                if (Resultado != null)
                                {
                                    if (Resultado.Exitoso)
                                    {
                                        var LinqQuery = from Tbl in Resultado.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.ruc))
                                                        select new
                                                        {
                                                            ruc = Tbl.ruc,
                                                            nombre = Tbl.nombre,
                                                            mostrar = string.Format("{0} - {1}", Tbl.ruc, Tbl.nombre)
                                                        };
                                        foreach (var Items in LinqQuery)
                                        {
                                            List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = Items.ruc.Trim(), nombre = Items.nombre.Trim(), mostrar = Items.mostrar });
                                        }

                                    }
                                }

                                this.CboAsumeFactura.DataSource = List_Asume;
                                this.CboAsumeFactura.DataTextField = "mostrar";
                                this.CboAsumeFactura.DataValueField = "ruc";
                                this.CboAsumeFactura.DataBind();
                                this.CboAsumeFactura.SelectedValue = this.hf_idasume.Value;
                                this.Actualiza_Paneles();
                            }


                        }
                    }

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>{0}", ex.Message));
            }


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {

                    //banmsg.Visible = false;
                }

                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
                Server.HtmlEncode(this.TXTHSN.Text.Trim());
                

                if (!Page.IsPostBack)
                {

                  

                    //texto para leyenda de carbono neutro
                    TextoLeyenda = leyenda_carbononeutro();

                    TextoProforma = leyenda_proforma_p2d();

                    TextoServicio = leyenda_servicio_p2d();

                    /*secuencial de sesion*/
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    numero_carga = QuerySegura.DecryptQueryString(Request.QueryString["ID_CARGA"]);
                    if (numero_carga == null || string.IsNullOrEmpty(numero_carga))
                    {
                        this.Crear_Sesion();
                    }
                    else
                    {
                        /*sesion existe y se desea realizar otras acciones, cuando de cotizacion se pasa a facturar*/
                        numero_carga = numero_carga.Trim().Replace("\0", string.Empty);
                        if (numero_carga.Split('+').ToList().Count > 7)
                        {
                            this.hf_BrowserWindowName.Value = numero_carga.Split('+').ToList()[6].Trim();

                            objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                            if (objCabecera == null)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen datos de la sesión actual para recuperar...puede volver a generar la consulta</b>"));
                                return;
                            }
                            else
                            {
                                CultureInfo enUS = new CultureInfo("en-US");
                              
                                tablePagination.DataSource = objCabecera.Detalle;
                                tablePagination.DataBind();

                                var TotalBultos = objCabecera.Detalle.Sum(x => x.CANTIDAD);

                                this.LabelTotal.InnerText = string.Format("DETALLE DE CARGA SUELTA (CFS)  - Total Bultos: {0}", TotalBultos);
                                this.Actualiza_Paneles();
                            }
                        }
                        else
                        {
                            this.Crear_Sesion();
                        }
                    }

                    objFactura = new Cls_Bil_Invoice_Cabecera();
                    Session["InvoiceCFS" + this.hf_BrowserWindowName.Value] = objFactura;

                  
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", ex.Message));
            }
        }


        #endregion
      
       #region "Eventos de la grilla"

       
        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void tablePagination_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }


                    if (tablePagination.Rows.Count > 0)
                    {
                        tablePagination.UseAccessibleHeader = true;
                        // Agrega la sección THEAD y TBODY.
                        tablePagination.HeaderRow.TableSection = TableRowSection.TableHeader;

                        // Agrega el elemento TH en la fila de encabezado.               
                        // Agrega la sección TFOOT. 
                        //tablePagination.FooterRow.TableSection = TableRowSection.TableFooter;
                    }
                }

               

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

        protected void tablePagination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    //objCabecera = Session["Transaccion"] as Cls_Bil_Cabecera;
                    objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;

                    if (objCabecera == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta</b>"));
                        return;
                    }
                    else
                    {
                        tablePagination.PageIndex = e.NewPageIndex;
                        tablePagination.DataSource = objCabecera.Detalle;
                        tablePagination.DataBind();
                        this.Actualiza_Panele_Detalle();
                    }
                }
                   

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }
        }

        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (Response.IsClientConnected)
            {


                try
                {
                    //cabecera de la grilla
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                       
                    }

                    //filas de la grilla
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        
                        string row_estado_ridt = DataBinder.Eval(e.Row.DataItem, "ESTADO_RDIT").ToString();
                        string row_autorizado = DataBinder.Eval(e.Row.DataItem, "AUTORIZADO").ToString();
                        DateTime? row_fechasalida = (DateTime?)DataBinder.Eval(e.Row.DataItem, "FECHA_HASTA");
                        string row_contenedor = DataBinder.Eval(e.Row.DataItem, "CONTENEDOR").ToString();
                        string row_bloqueo = DataBinder.Eval(e.Row.DataItem, "DES_BLOQUEO").ToString();

                        CheckBox Chk = (CheckBox)e.Row.FindControl("CHKFA");
                        string row_pase = DataBinder.Eval(e.Row.DataItem, "NUMERO_PASE_N4").ToString();
                        string row_inout = DataBinder.Eval(e.Row.DataItem, "IN_OUT").ToString();

                        //certificado
                        string row_tienecertificado = DataBinder.Eval(e.Row.DataItem, "TIENE_CERTIFICADO").ToString();
                        CheckBox ChkCert = (CheckBox)e.Row.FindControl("CHKCERTIFICADO");
                        //fin

                        if (!row_estado_ridt.Equals("A"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.PaleVioletRed;
                            this.BtnFacturar.Attributes.Add("disabled", "disabled");
                        }

                        if (row_inout.Equals("OUT"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Peru;
                            this.BtnFacturar.Attributes.Add("disabled", "disabled");
                        }

                        if (!string.IsNullOrEmpty(row_pase))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Green;

                        }

                        if (row_autorizado.Equals("NO"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.OrangeRed;
                          
                            this.BtnFacturar.Attributes.Add("disabled", "disabled");
                            //this.BtnFacturar.Attributes.Remove("disabled");
                        }

                        if (row_bloqueo.Equals("SI"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.IndianRed;  
                            this.BtnFacturar.Attributes.Add("disabled", "disabled");
                            //this.BtnFacturar.Attributes.Remove("disabled");
                        }

                        //bloquea ceritificado
                        if (row_tienecertificado.Equals("SI"))
                        {
                            ChkCert.Enabled = false;

                        }

                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

                }
            }
        }

      

        #endregion

        #region "Evento Botones"

       
        

        //revisando 1
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    this.BtnFacturar.Attributes.Add("disabled", "disabled");

                    OcultarLoading("2");
                    bool Ocultar_Mensaje = true;
                    bool cancelado = false;
                  
                    string Msg = string.Empty;
                    bool Es_FF = false;

                    this.LabelTotal.InnerText = string.Format("DETALLE DE CARGA SUELTA (CFS) ");
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga MRN</b>"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga MSN</b>"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga HSN</b>"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    tablePagination.DataSource = null;
                    tablePagination.DataBind();


                    /*saco los dias libre como parametros generales*/
                    List<Cls_Bil_Configuraciones> Parametros = Cls_Bil_Configuraciones.Parametros( out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en procesos de días libres.....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }
                  
                    

                   /*para almacenar clientes que asumen factura*/
                    List_Asume = new List<Cls_Bil_AsumeFactura>();
                    List_Asume.Clear();
                    List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = string.Empty, nombre = string.Empty, mostrar = string.Format("{0}", "Seleccionar...") });

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdAgenteCodigo = string.Empty;
                    var FFCod = N4.Entidades.Forwarder.ObtenerForwarderPorRUC(ClsUsuario.loginname, ClsUsuario.ruc);
                    if (FFCod.Exitoso)
                    {
                        var ListaAgente = FFCod.Resultado;
                        if (ListaAgente != null)
                        {
                            IdAgenteCodigo = ListaAgente.CODIGO_FW;

                        }

                        if (string.IsNullOrEmpty(IdAgenteCodigo))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i> <b> Error! Freight Forwarder [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 1,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", ClsUsuario.ruc, ClsUsuario.nombres));

                            Ocultar_Mensaje = false;
                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("Se presentaron los siguientes problemas: No exsite información del Freight Forwarder, no registrado: {0}", ClsUsuario.ruc);

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Carga Suelta", "No existe información del Freight Forwarder, no registrado.", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                ClsUsuario.nombres, ClsUsuario.ruc, out MensajeCasos, false);

                            /*************************************************************************************************************************************
                            * fin caso salesforce
                            * **********************************************************************************************************************************/
                            return;
                        }
                        else
                        {
                            Es_FF = true;
                        }

                    }

                    //si no es FF, es un agente
                    if (string.IsNullOrEmpty(IdAgenteCodigo))
                    {
                        var AgenteCod = N4.Entidades.Agente.ObtenerAgentePorRuc(ClsUsuario.loginname, ClsUsuario.ruc);
                        if (AgenteCod.Exitoso)
                        {
                            var ListaAgente = AgenteCod.Resultado;
                            if (ListaAgente != null)
                            {
                                IdAgenteCodigo = ListaAgente.codigo;

                                Es_FF = false;
                            }
                        }
                    }
                    //busca contenedores por ruc de usuario
                    if (IdAgenteCodigo.Equals("CGSA"))
                    {
                        IdAgenteCodigo = string.Empty;
                    }
                   

                    var Validacion = new Aduana.Importacion.ecu_validacion_cntr_cfs();
                    var EcuaContenedores = Validacion.CargaPorManifiestoImpoFF(ClsUsuario.loginname, ClsUsuario.ruc, IdAgenteCodigo, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), true);//ecuapas contenedores
                    if (EcuaContenedores.Exitoso)
                    {
                        //DATOS DEL AGENTE PARA BUSCAR INFORMACION
                        var LinqAgente = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                            select new
                                            {
                                                ID_AGENTE = Tbl.agente_id,
                                                ID_CLIENTE = Tbl.importador_id,
                                                DESC_CLIENTE = (Tbl.importador==null ? string.Empty : Tbl.importador),
                                                TIPO_CLIENTE = Tbl.tipo_cliente
                                            }).FirstOrDefault();

                        this.hf_idagente.Value = LinqAgente.ID_AGENTE;
                        this.hf_idcliente.Value = LinqAgente.ID_CLIENTE;
                        this.hf_idasume.Value = LinqAgente.ID_CLIENTE;

                        this.hf_rucagente.Value = string.Empty;
                        this.hf_descagente.Value = string.Empty;
                        this.hf_descasume.Value = string.Empty;
                        this.hf_desccliente.Value = LinqAgente.DESC_CLIENTE;

                        //LLENADO DE CAMPOS DE PANTALLA DEL AGENTE
                        if (!LinqAgente.TIPO_CLIENTE.Equals("FF"))
                        {
                            var Agente = N4.Entidades.Agente.ObtenerAgente(ClsUsuario.loginname, this.hf_idagente.Value);
                            if (Agente.Exitoso)
                            {
                                var ListaAgente = Agente.Resultado;
                                if (ListaAgente != null)
                                {
                                    this.TXTAGENCIA.Text = string.Format("{0} - {1}", ListaAgente.ruc.Trim(), ListaAgente.nombres.Trim());
                                    this.hf_rucagente.Value = ListaAgente.ruc.Trim();
                                    this.hf_descagente.Value = ListaAgente.nombres.Trim();

                                    //si la persona que va a facturar es importador, no agrega el agente
                                    if (!this.hf_idcliente.Value.Trim().Equals(ClsUsuario.ruc.Trim()))
                                    {
                                        //agrega importador

                                        List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = ListaAgente.ruc.Trim(), nombre = ListaAgente.nombres.Trim(), mostrar = string.Format("{0} - {1}", ListaAgente.ruc.Trim(), ListaAgente.nombres.Trim()) });

                                    }
                                }
                                else
                                {
                                    this.TXTAGENCIA.Text = string.Empty;
                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del agente</b>"));
                                    return;
                                }
                            }
                            else
                            {
                                this.TXTAGENCIA.Text = string.Empty;
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del agente, {0}</b>", Agente.MensajeProblema));
                                return;
                            }
                        }
                        else
                        {
                            //LLENADO DE CAMPOS DE PANTALLA DEL AGENTE
                            var Agente = N4.Entidades.Forwarder.ObtenerForwarderPorCodigo(ClsUsuario.loginname, this.hf_idagente.Value);
                            if (Agente.Exitoso)
                            {
                                var ListaAgente = Agente.Resultado;
                                if (ListaAgente != null)
                                {
                                    this.TXTAGENCIA.Text = string.Format("{0} - {1}", ListaAgente.CLNT_CUSTOMER.Trim(), ListaAgente.CLNT_NAME.Trim());
                                    this.hf_rucagente.Value = ListaAgente.CLNT_CUSTOMER.Trim();
                                    this.hf_descagente.Value = ListaAgente.CLNT_NAME.Trim();

                                    this.TXTCLIENTE.Text = string.Format("{0} - {1}", ListaAgente.CLNT_CUSTOMER.Trim(), ListaAgente.CLNT_NAME.Trim());
                                    this.TXTASUMEFACTURA.Text = ListaAgente.CLNT_CUSTOMER.Trim() + " - " + ListaAgente.CLNT_NAME.Trim();
                                    this.hf_desccliente.Value = ListaAgente.CLNT_NAME.Trim();
                                    this.hf_descasume.Value = ListaAgente.CLNT_NAME.Trim();

                                    //si la persona que va a facturar es importador, no agrega el agente
                                    if (!this.hf_idcliente.Value.Trim().Equals(ClsUsuario.ruc.Trim()))
                                    {
                                        //agrega importador
                                        List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = ListaAgente.CLNT_CUSTOMER.Trim(), nombre = ListaAgente.CLNT_NAME.Trim(), mostrar = string.Format("{0} - {1}", ListaAgente.CLNT_CUSTOMER.Trim(), ListaAgente.CLNT_NAME.Trim()) });

                                    }
                                }
                                else
                                {
                                    this.Limpia_Datos_cliente();
                                    this.TXTAGENCIA.Text = string.Empty;
                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Freight Forwarder con el ruc:{0}</b>", ClsUsuario.ruc));
                                    return;
                                }
                            }
                            else
                            {
                                this.TXTAGENCIA.Text = string.Empty;
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Freight Forwarder, {0}</b>", Agente.MensajeProblema));
                                return;
                            }
                        }

                        //LLENADO DE CAMPOS DE PANTALLA DEL CLIENTE
                        var Cliente = N4.Entidades.Cliente.ObtenerCliente(ClsUsuario.loginname, this.hf_idcliente.Value);
                        if (Cliente.Exitoso)
                        {
                            var ListaCliente = Cliente.Resultado;
                            if (ListaCliente != null)
                            {
                                this.TXTCLIENTE.Text = string.Format("{0} - {1}", ListaCliente.CLNT_CUSTOMER.Trim(), ListaCliente.CLNT_NAME.Trim());
                                this.TXTASUMEFACTURA.Text = ListaCliente.CLNT_CUSTOMER.Trim() + " - " + ListaCliente.CLNT_NAME.Trim();
                                this.hf_desccliente.Value = ListaCliente.CLNT_NAME.Trim();
                                this.hf_descasume.Value = ListaCliente.CLNT_NAME.Trim();
                              
                            }
                            else
                            {
                                this.Limpia_Datos_cliente();
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente</b>"));
                                return;
                            }
                        }
                        else
                        {
                            this.Limpia_Datos_cliente();
                            List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = this.hf_idcliente.Value, nombre = this.hf_desccliente.Value, mostrar = string.Format("{0} - {1}", this.hf_idcliente.Value, this.hf_desccliente.Value) });
                            Ocultar_Mensaje = false;

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", this.hf_idcliente.Value, this.hf_desccliente.Value));


                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("Se presentaron los siguientes problemas: No existe información del cliente, no registrado: {0}", this.hf_idcliente.Value);

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "No existe información del cliente, no registrado.", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                this.hf_desccliente.Value, this.hf_rucagente.Value, out MensajeCasos, false);

                            /*************************************************************************************************************************************
                            * fin caso salesforce
                            **************************************************************************************************************************************/

                        }

                        this.CboAsumeFactura.DataSource = List_Asume;
                        this.CboAsumeFactura.DataTextField = "mostrar";
                        this.CboAsumeFactura.DataValueField = "ruc";
                        this.CboAsumeFactura.DataBind();
                        //fin asume factura


                        if (List_Asume.Count >= 2) { this.CboAsumeFactura.SelectedIndex = 1; }


                        //INFORMACION DEL CONTENEDOR
                        var Contenedor = new N4.Importacion.container_cfs();
                        var ListaContenedores = Contenedor.CargaPorBL(ClsUsuario.loginname, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());//resultado de entidad contenedor y cfs
                        if (ListaContenedores.Exitoso)
                        {
                            

                            //valida si tiene ubicacion la carga, si esta desconsolidada.(verdadero=es carga cfs, false=no esta desconsolidada)
                            var LinqTarja = Aduana.Importacion.ecu_validacion_cntr_cfs.EsCargaCFS(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());
                            if (LinqTarja.Value)
                            {
                                SinDesconsolidar = true;
                            }
                            else { SinDesconsolidar = false; }

                            
                            List_Autorizacion = new List<Cls_Bil_Cas_Manual>();
                            List_Autorizacion.Clear();
                           

                            //autorizacion de salida de la carga cfs
                            var Autorizacion = CasBBK.ListaCasPartida(ClsUsuario.loginname, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());
                            if (Autorizacion.Exitoso)
                            {
                                var LinqAut = (from Tbl in Autorizacion.Resultado.Where(Tbl => Tbl.activo)
                                                        select new
                                                        {
                                                            CARGA = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                                            FECHA_AUTORIZACION = Tbl.fecha_registro,
                                                            AUTORIZADO = true,
                                                            USUARIO_AUTORIZA = (Tbl.usuario_libera == null ? string.Empty : Tbl.usuario_libera),
                                                            CONSIGNATARIO = (Tbl.consignatario_manifiesto == null ? string.Empty : Tbl.consignatario_manifiesto)
                                                        }).FirstOrDefault();

                                List_Autorizacion.Add(new Cls_Bil_Cas_Manual { CARGA = LinqAut.CARGA, FECHA_AUTORIZACION = LinqAut.FECHA_AUTORIZACION, AUTORIZADO = LinqAut.AUTORIZADO, USUARIO_AUTORIZA = LinqAut.USUARIO_AUTORIZA, CONSIGNATARIO = LinqAut.CONSIGNATARIO });

                                this.BtnFacturar.Attributes.Remove("disabled");
                                SinAutorizacion = true;
                            }
                            else
                            {
                                List_Autorizacion.Add(new Cls_Bil_Cas_Manual { CARGA = string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()), FECHA_AUTORIZACION = (DateTime?)null, AUTORIZADO = false, USUARIO_AUTORIZA=string.Empty, CONSIGNATARIO=string.Empty });
                                SinAutorizacion = false;
                            }

                            var LinqAutorizacion = (from Tbl in List_Autorizacion
                                                    select new
                                                    {
                                                        CARGA = Tbl.CARGA,
                                                        FECHA_AUTORIZACION = Tbl.FECHA_AUTORIZACION,
                                                        AUTORIZADO = Tbl.AUTORIZADO,
                                                        USUARIO_AUTORIZA = Tbl.USUARIO_AUTORIZA,
                                                        CONSIGNATARIO = Tbl.CONSIGNATARIO 
                                                    });


                            //informacion ecuapass     
                            var LinqPartidas = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                                select new
                                                {
                                                    MRN = Tbl.mrn,
                                                    MSN = Tbl.msn,
                                                    HSN = Tbl.hsn,
                                                    IMDT = (Tbl.imdt_id == null) ? "" : Tbl.imdt_id,
                                                    GKEY = (Tbl.gkey==null ? 0 : Tbl.gkey),
                                                    CONTENEDOR = (Tbl.cntr == null) ? "" : Tbl.cntr,
                                                    BL = (Tbl.documento_bl == null) ? "" : Tbl.documento_bl,
                                                    DECLARACION = (Tbl.declaracion == null) ? "" : Tbl.declaracion,
                                                    ESTADO_RIDT = (Tbl.ridt_estado==null ? "" : Tbl.ridt_estado),
                                                    CARGA = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                                    TIPO_CLIENTE = Tbl.tipo_cliente
                                                }).Distinct();

                            //contenedores con carga cfs
                            var LinqPartidadN4 = (from Tbl in ListaContenedores.Resultado.Where(Tbl => Tbl.CNTR_CANTIDAD != 0 )
                                                  select new
                                                  {
                                                      CNTR_CONTAINER = (Tbl.CNTR_AISV == null ? string.Empty : Tbl.CNTR_AISV),
                                                      CNTR_VEPR_REFERENCE = (Tbl.CNTR_VEPR_REFERENCE == null) ? string.Empty : Tbl.CNTR_VEPR_REFERENCE,
                                                      CNTR_TYPE = (Tbl.CNTR_TYPE == null) ? string.Empty : Tbl.CNTR_TYPE,
                                                      CNTR_TYSZ_SIZE = (Tbl.CNTR_TYSZ_SIZE == null) ? string.Empty : Tbl.CNTR_TYSZ_SIZE,
                                                      CNTR_CATY_CARGO_TYPE = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
                                                      FECHA_CAS = (DateTime?)(Tbl.FECHA_CAS.HasValue ? Tbl.FECHA_CAS : null),
                                                      BLOQUEOS = Tbl.CNTR_HOLD,
                                                      CNTR_YARD_STATUS = (Tbl.CNTR_YARD_STATUS == null) ? string.Empty : Tbl.CNTR_YARD_STATUS,
                                                      CNTR_TYSZ_TYPE = (Tbl.CNTR_TYSZ_TYPE == null) ? string.Empty : Tbl.CNTR_TYSZ_TYPE,
                                                      CNTR_CLNT_CUSTOMER_LINE = (Tbl.CNTR_CLNT_CUSTOMER_LINE == null) ? string.Empty : Tbl.CNTR_CLNT_CUSTOMER_LINE,
                                                      CNTR_DOCUMENT = (Tbl.CNTR_DOCUMENT == null ? string.Empty : Tbl.CNTR_DOCUMENT),
                                                      CNTR_FULL_EMPTY_CODE = (Tbl.CNTR_FULL_EMPTY_CODE == null) ? string.Empty : Tbl.CNTR_FULL_EMPTY_CODE,
                                                      CNTR_CONSECUTIVO = Tbl.CNTR_CONSECUTIVO,
                                                      CNTR_AISV = (Tbl.CNTR_AISV == null) ? string.Empty : Tbl.CNTR_AISV,
                                                      CNTR_HOLD = (Tbl.CNTR_HOLD == 0) ? false : true,
                                                      CNTR_VEPR_VOYAGE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                                      CNTR_VEPR_VSSL_NAME = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                                      CNTR_VEPR_ACTUAL_ARRIVAL = (Tbl.CNTR_VEPR_ACTUAL_ARRIVAL.HasValue ? Tbl.CNTR_VEPR_ACTUAL_ARRIVAL.Value.ToString("dd/MM/yyyy") : ""),
                                                      CNTR_DD = (Tbl.CNTR_DD == null) ? false : Tbl.CNTR_DD.Value,
                                                      CNTR_DESCARGA = (Tbl.CNTR_DESCARGA == null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                                      CNTR_VEPR_ACTUAL_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
                                                      CNTR_CANTIDAD = (Tbl.CNTR_CANTIDAD == null ? 0 : Tbl.CNTR_CANTIDAD),
                                                      CNTR_PESO = (Tbl.CNTR_PESO==null ? 0 : Tbl.CNTR_PESO),
                                                      CNTR_OPERACION = (Tbl.CNTR_OPERACION == null ? string.Empty : Tbl.CNTR_OPERACION),
                                                      CNTR_DESCRIPCION = (Tbl.CNTR_DESCRIPCION == null ? string.Empty : Tbl.CNTR_DESCRIPCION),
                                                      CNTR_EXPORTADOR = (Tbl.CNTR_EXPORTADOR == null ? string.Empty : Tbl.CNTR_EXPORTADOR),
                                                      CNTR_AGENCIA = (Tbl.CNTR_AGENCIA == null ? string.Empty : Tbl.CNTR_AGENCIA),
                                                      CARGA = string.Format("{0}-{1}-{2}", Tbl.CNTR_MRN, Tbl.CNTR_MSN, Tbl.CNTR_HSN),
                                                      CNTR_REEFER_CONT = (Tbl.CNTR_REEFER_CONT == null ? string.Empty : Tbl.CNTR_REEFER_CONT),
                                                      ID_UNIDAD = (Tbl.ID_UNIDAD == null ? 0 : Tbl.ID_UNIDAD),
                                                  }).OrderBy(x => x.CNTR_CANTIDAD).ThenBy(x => x.CNTR_CONTAINER);

                            /*ultima factura*/
                            List<Cls_Bil_Invoice_Ultima_Factura> ListUltimaFactura = Cls_Bil_Invoice_Ultima_Factura.List_Ultima_Factura_cfs(this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" + this.TXTHSN.Text.Trim(), out cMensajes);
                            if (!String.IsNullOrEmpty(cMensajes))
                            {
                              
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener última factura cfs....{0}</b>", cMensajes));
                                this.Actualiza_Panele_Detalle();
                                return;
                            }
                            /*ultima factura en caso de tener*/
                            var LinqUltimaFactura = (from TblFact in ListUltimaFactura.Where(TblFact => !string.IsNullOrEmpty(TblFact.IV_FACTURA))
                                                        select new
                                                        {
                                                            FT_NUMERO_CARGA = TblFact.IV_NUMERO_CARGA,
                                                            FT_FECHA = (TblFact.IV_FECHA==null ? null : TblFact.IV_FECHA),
                                                            FT_FACTURA = TblFact.IV_FACTURA,
                                                            FT_FECHA_HASTA = (TblFact.IV_FECHA_HASTA==null? null : TblFact.IV_FECHA_HASTA),
                                                            FT_ID = TblFact.IV_ID,
                                                            FT_MODULO = TblFact.IV_MODULO
                                                        }).Distinct();



                            /*left join de contenedores*/
                            var LinqQuery = (from Tbl in LinqPartidadN4
                                                join EcuaPartidas in LinqPartidas on Tbl.CARGA equals EcuaPartidas.CARGA into TmpFinal
                                                join Factura in LinqUltimaFactura on Tbl.CARGA equals Factura.FT_NUMERO_CARGA into TmpFactura
                                                join AutCas in LinqAutorizacion on Tbl.CARGA equals AutCas.CARGA into TmpAutorizacion
                                             from Final in TmpFinal.DefaultIfEmpty()
                                             from FinalFT in TmpFactura.DefaultIfEmpty()
                                             from FinalAut in TmpAutorizacion.DefaultIfEmpty()
                                             select new
                                                {
                                                    CONTENEDOR = Tbl.CNTR_CONTAINER,
                                                    REFERENCIA = (Tbl.CNTR_VEPR_REFERENCE == null) ? string.Empty : Tbl.CNTR_VEPR_REFERENCE,
                                                    TRAFICO = (Tbl.CNTR_TYPE == null) ? string.Empty : Tbl.CNTR_TYPE,
                                                    TAMANO = (Tbl.CNTR_TYSZ_SIZE == null) ? string.Empty : Tbl.CNTR_TYSZ_SIZE,
                                                    TIPO = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
                                                    FECHA_CAS = (DateTime?)(FinalAut == null ? null : FinalAut.FECHA_AUTORIZACION),
                                                    AUTORIZADO = (bool?)(FinalAut == null ? false : FinalAut.AUTORIZADO),
                                                    BLOQUEOS = (Tbl.CNTR_HOLD == false) ? string.Empty : "SI",
                                                    IN_OUT = (Tbl.CNTR_YARD_STATUS == null) ? string.Empty : Tbl.CNTR_YARD_STATUS,
                                                    TIPO_CONTENEDOR = (Tbl.CNTR_TYSZ_TYPE == null) ? string.Empty : Tbl.CNTR_TYSZ_TYPE,//REEFER
                                                    CONECTADO = ((Tbl.CNTR_TYSZ_TYPE == "RF") ? ((Tbl.CNTR_REEFER_CONT == "N") ? "NO CONECTADO" : "CONECTADO") : string.Empty),
                                                    LINEA = (Tbl.CNTR_CLNT_CUSTOMER_LINE == null) ? string.Empty : Tbl.CNTR_CLNT_CUSTOMER_LINE,
                                                    DOCUMENTO = (string.IsNullOrEmpty(Tbl.CNTR_DOCUMENT) ? ((Final == null) ? string.Empty : Final.DECLARACION) : Tbl.CNTR_DOCUMENT),
                                                    IMDT = (Final == null) ? string.Empty : Final.IMDT,
                                                    BL = (Final == null) ? string.Empty : Final.BL,
                                                    FULL_VACIO = (Tbl.CNTR_FULL_EMPTY_CODE == null) ? string.Empty : Tbl.CNTR_FULL_EMPTY_CODE,
                                                    GKEY = Tbl.CNTR_CONSECUTIVO,
                                                    AISV = (Tbl.CNTR_AISV == null) ? string.Empty : Tbl.CNTR_AISV,
                                                    DECLARACION = (Final == null) ? string.Empty : Final.DECLARACION,
                                                    BLOQUEADO = Tbl.CNTR_HOLD,
                                                    FECHA_ULTIMA = (FinalFT == null) ? null : (DateTime?)(FinalFT.FT_FECHA.HasValue ? FinalFT.FT_FECHA : null),
                                                    NUMERO_FACTURA = (FinalFT == null) ? string.Empty : FinalFT.FT_FACTURA,
                                                    ID_FACTURA = (FinalFT == null) ? 0 : FinalFT.FT_ID,
                                                    VIAJE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                                    NAVE = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                                    FECHA_ARRIBO = Tbl.CNTR_VEPR_ACTUAL_ARRIVAL,
                                                    CNTR_DD = Tbl.CNTR_DD,
                                                    FECHA_HASTA = (FinalFT == null) ? null : (DateTime?)(FinalFT.FT_FECHA_HASTA.HasValue ? FinalFT.FT_FECHA_HASTA : null),
                                                    ESTADO_RIDT = (Final.ESTADO_RIDT == null) ? string.Empty : Final.ESTADO_RIDT,
                                                    CNTR_DESCARGA = (Tbl.CNTR_DESCARGA==null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                                    MODULO = (FinalFT == null) ? string.Empty : FinalFT.FT_MODULO,
                                                    CNTR_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
                                                    CANTIDAD = Tbl.CNTR_CANTIDAD,
                                                    PESO = Tbl.CNTR_PESO ,
                                                    OPERACION = Tbl.CNTR_OPERACION ,
                                                    DESCRIPCION = Tbl.CNTR_DESCRIPCION,
                                                    EXPORTADOR = Tbl.CNTR_EXPORTADOR ,
                                                    AGENCIA = Tbl.CNTR_AGENCIA,
                                                    CARGA = Tbl.CARGA,
                                                    ID_UNIDAD = Tbl.ID_UNIDAD ,
                                                    CERTIFICADO =  false ,
                                                    TIENE_CERTIFICADO =  "NO",
                                                    TIPO_CLIENTE = (Final == null) ? string.Empty : Final.TIPO_CLIENTE,
                                             }).OrderBy(x => x.IN_OUT).ThenBy(x=> x.CONTENEDOR);

                            if (LinqQuery != null && LinqQuery.Count() > 0)
                            {

                                
                                //agrego todos los contenedores a la clase cabecera
                                objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;

                                objCabecera.ID_CLIENTE = this.hf_idcliente.Value;
                                objCabecera.DESC_CLIENTE = this.hf_desccliente.Value;
                                objCabecera.ID_FACTURADO = this.hf_idasume.Value;
                                objCabecera.DESC_FACTURADO = this.hf_descasume.Value;
                                objCabecera.ID_UNICO_AGENTE = this.hf_idagente.Value;
                                objCabecera.ID_AGENTE = this.hf_rucagente.Value;
                                objCabecera.DESC_AGENTE = this.hf_descagente.Value;
                                objCabecera.FECHA = DateTime.Now;
                                objCabecera.TIPO_CARGA = "CFS";
                                objCabecera.NUMERO_CARGA = this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" + this.TXTHSN.Text.Trim();
                                objCabecera.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objCabecera.SESION = this.hf_BrowserWindowName.Value;
                                objCabecera.HORA_HASTA = "00:00";
                                objCabecera.FECHA_HASTA = DateTime.Now;
                                
                                objCabecera.Detalle.Clear();

                                Int16 Secuencia = 1;
                                foreach (var Det in LinqQuery)
                                {
                                    /*datos nuevos para imprimir factura*/
                                    objCabecera.BL = Det.BL;
                                    objCabecera.BUQUE = Det.NAVE;
                                    objCabecera.VIAJE = Det.VIAJE;
                                    objCabecera.FECHA_ARRIBO = Det.FECHA_ARRIBO;
                                    objCabecera.TIPO_CLIENTE = Det.TIPO_CLIENTE;
                                    objCabecera.P2D = true;
                                    objCabecera.RUC_USUARIO = this.hf_rucagente.Value;
                                    objCabecera.DESC_USUARIO = this.hf_descagente.Value;

                                    objDetalle = new Cls_Bil_Detalle();
                                    objDetalle.VISTO = false;
                                    objDetalle.ID = Det.ID_FACTURA;
                                    objDetalle.SECUENCIA = Secuencia;
                                    objDetalle.GKEY = Det.GKEY;
                                    objDetalle.MRN = this.TXTMRN.Text.Trim();
                                    objDetalle.MSN = this.TXTMSN.Text.Trim();
                                    objDetalle.HSN = this.TXTHSN.Text.Trim();
                                    objDetalle.CONTENEDOR = Det.CONTENEDOR;
                                    objDetalle.TRAFICO = Det.TRAFICO;
                                    objDetalle.DOCUMENTO = Det.DOCUMENTO;
                                    objDetalle.DES_BLOQUEO = Det.BLOQUEOS;
                                    objDetalle.CONECTADO = Det.CONECTADO;
                                    objDetalle.REFERENCIA = Det.REFERENCIA;
                                    objDetalle.TAMANO = Det.TAMANO;
                                    objDetalle.TIPO = "CFS";
                                    objDetalle.CAS = Det.FECHA_CAS;
                                    objDetalle.AUTORIZADO = (Det.AUTORIZADO ==true ? "SI" : "NO");
                                    objDetalle.BOOKING = "";

                                    objDetalle.IMDT = Det.IMDT;
                                    objDetalle.BLOQUEO = Det.BLOQUEADO;
                                  
                                    objDetalle.FECHA_ULTIMA = Det.FECHA_HASTA;
                                    objDetalle.IN_OUT = Det.IN_OUT;
                                    objDetalle.FULL_VACIO = Det.FULL_VACIO;
                                    objDetalle.AISV = Det.AISV;
                                    objDetalle.REEFER = Det.TIPO_CONTENEDOR;
                                    objDetalle.IV_USUARIO_CREA = ClsUsuario.loginname.Trim();
                                    objDetalle.IV_FECHA_CREA = DateTime.Now;
                                    objDetalle.NUMERO_FACTURA = Det.NUMERO_FACTURA;
                                    objDetalle.CNTR_DD = Det.CNTR_DD;
                                    objDetalle.FECHA_HASTA = DateTime.Now; //Det.FECHA_HASTA;
                                    objDetalle.ESTADO_RDIT = Det.ESTADO_RIDT.Trim();
                                    objDetalle.CNTR_DESCARGA = Det.CNTR_DESCARGA;
                                    objDetalle.MODULO = Det.MODULO;
                                    objDetalle.CNTR_DEPARTED = Det.CNTR_DEPARTED;
                                    objDetalle.LINEA = Det.LINEA;

                                    //nuevos campos
                                    objDetalle.CANTIDAD =  decimal.Parse(Det.CANTIDAD.Value.ToString());
                                    objDetalle.PESO = decimal.Parse(Det.PESO.ToString());
                                    objDetalle.OPERACION = Det.OPERACION;
                                    objDetalle.DESCRIPCION = Det.DESCRIPCION;
                                    objDetalle.EXPORTADOR = Det.EXPORTADOR;
                                    objDetalle.AGENCIA = Det.AGENCIA;
                                    //nuevo
                                    objDetalle.ID_UNIDAD = Det.ID_UNIDAD;

                                    if (!objDetalle.ESTADO_RDIT.Equals("A"))
                                    {
                                        cancelado = true;
                                    }

                                    if (NDiasLibreas != 0)
                                    {
                                        objDetalle.FECHA_TOPE_DLIBRE = objDetalle.CNTR_DESCARGA.Value.AddDays(NDiasLibreas);
                                    }
                                    else
                                    {
                                        objDetalle.FECHA_TOPE_DLIBRE = objDetalle.CNTR_DESCARGA;
                                    }

                                    if (Det.BLOQUEOS.Equals("SI"))
                                    {
                                        Bloqueos = true;
                                    }

                                    
                                    objDetalle.IDPLAN = "0";
                                    objDetalle.TURNO = "* Seleccione *";

                                    
                                    objDetalle.CERTIFICADO = Det.CERTIFICADO;
                                    objDetalle.TIENE_CERTIFICADO = Det.TIENE_CERTIFICADO;
                                    objDetalle.VISTO = true;

                                    objCabecera.Detalle.Add(objDetalle);
                                    Secuencia++;
                                }

                                //total de bultos
                                var TotalBultos = objCabecera.Detalle.Sum(x => x.CANTIDAD);
                                objCabecera.TOTAL_BULTOS = TotalBultos;

                                //agrega a la grilla
                                tablePagination.DataSource = objCabecera.Detalle;
                                tablePagination.DataBind();

                                this.LabelTotal.InnerText = string.Format("DETALLE DE CARGA SUELTA (CFS) - Total Bultos: {0}", objCabecera.TOTAL_BULTOS);

                                Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;
                                this.Actualiza_Panele_Detalle();

                                if (cancelado)
                                {

                                    /*************************************************************************************************************************************
                                    * crear caso salesforce
                                    ***********************************************************************************************************************************/
                                    MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", "según sistema ecuapass la Respuesta de Aprobación de Salida (Importación) -  RIDT no se encuentra aprobado");

                                    this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Unidad sin RIDT Aprobado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}",this.TXTMRN.Text.Trim(),this.TXTMSN.Text.Trim(),this.TXTHSN.Text.Trim()),
                                        objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos, false);

                                    /*************************************************************************************************************************************
                                    * fin caso salesforce
                                    * **********************************************************************************************************************************/

                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Estimado cliente, lamentamos mucho no poder atender su requerimiento, según sistema ecuapass la Respuesta de Aprobación de Salida (Importación) -  RIDT no se encuentra aprobado. Ponerse en contacto con el Senae.</b>"));
                                    return;
                                }
                                else
                                {
                                    //carga no esta autorizada paea salir
                                    if (!SinAutorizacion)
                                    {
                                        /*************************************************************************************************************************************
                                        * crear caso salesforce
                                        ***********************************************************************************************************************************/
                                        MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", "La carga CFS no tiene autorización de salida CAS");

                                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "La carga no tiene autorización de salida CAS", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                            objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos, false);

                                        Ocultar_Mensaje = false;
                                        this.BtnFacturar.Attributes.Add("disabled", "disabled");//comentar pruebas
                                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Estimado cliente, no podrá avanzar con la facturación, la carga no cuenta con la aprobación CAS de autorización de salida... {0} </b>", MensajeCasos));

                                    }
                                    else {

                                        //carga presenta bloqueos
                                        if (Bloqueos)
                                        {
                                            /*************************************************************************************************************************************
                                            * crear caso salesforce
                                            ***********************************************************************************************************************************/
                                            MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", "La carga presenta bloqueos");

                                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "La carga presenta bloqueos", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                                objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos, false);

                                            Ocultar_Mensaje = false;

                                            this.BtnFacturar.Attributes.Add("disabled", "disabled");//comentar pruebas

                                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Estimado cliente, la carga presenta un bloqueo, no podrá facturar la misma...{0} </b>", MensajeCasos));
                                        }
                                        else
                                        {
                                            //carga no esta desconsolidada
                                            if (!SinDesconsolidar)
                                            {
                                                /*************************************************************************************************************************************
                                                * crear caso salesforce
                                                ***********************************************************************************************************************************/
                                                MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", "La carga no tiene ubicación");

                                                this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "La carga no tiene ubicación", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                                    objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos, false);

                                                Ocultar_Mensaje = false;

                                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Estimado cliente, este mensaje no es impedimento para que pueda realizar su factura.. {0} </b>", MensajeCasos));
                                            }

                                        }
                                        
                                    }                                   
                                }
                            }
                            else
                            {
                                tablePagination.DataSource = null;
                                tablePagination.DataBind();

                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de carga suelta pendiente de facturar con el número de la carga ingresada..</b>"));
                                this.Actualiza_Panele_Detalle();
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de carga suelta pendiente de facturar con el número de la carga ingresada..{0}</b>", ListaContenedores.MensajeProblema));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de carga suelta pendiente de facturar con el número de la carga ingresada..{0}</b>", EcuaContenedores.MensajeProblema));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                    if (Ocultar_Mensaje)
                    {
                        this.Ocultar_Mensaje();
                    }
                   
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

                }
            }

        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect("~/cargacfs/facturacioncfs.aspx", false);



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.myModal.Attributes["class"] = "nover";
            this.Actualiza_Paneles();

        }

        protected void BtnCotizar_Click(object sender, EventArgs e)
        {
           
        }

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

                CultureInfo enUS = new CultureInfo("en-US");
                NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
                this.myModal.Attributes["class"] = "nover";
              

                try
                {

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga MRN</b>"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga MSN</b>"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga HSN</b>"));
                        this.TXTHSN.Focus();

                        return;
                    }
                    if (this.CboAsumeFactura.Items.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona que va asumir la carga</b>"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    //valida que se seleccione la persona a facturar
                    if (this.CboAsumeFactura.SelectedIndex == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona que asumirá la factura.</b>"));
                        this.CboAsumeFactura.Focus();
                        return;
                    }

                   
                    HoraHasta = "00:00";
                   

                    //instancia sesion
                    objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    if (objCabecera == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la factura </b>"));
                        return;
                    }
                    if (objCabecera.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de carga suelta, para poder generar la factura </b>"));
                        return;
                    }


                    //valida que seleccione las cargas a facturar
                    var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == false)
                                                select p.CONTENEDOR).ToList();

                    if (LinqValidaContenedor.Count != 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe agregar la fecha de salida de la carga a Facturar: {0} </b>", objCabecera.NUMERO_CARGA));
                        return;
                    }
                    //valida que tenga todos tengan fecha de salida
                    foreach (var Det in objCabecera.Detalle)
                    {
                        if (!Det.FECHA_HASTA.HasValue)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida de salida de la carga {0}</b>", objCabecera.NUMERO_CARGA));
                            return;
                        }

                        if (Det.FECHA_HASTA.Value.Date < System.DateTime.Today.Date)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! La fecha de salida de la carga: {0}, no puede ser menor que la fecha actual..</b>", objCabecera.NUMERO_CARGA));
                            return;
                        }
                    }
                    
                    
                    //listado de contenedores
                    var LinqListContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                              select p.CONTENEDOR).ToList();

                    //asume factura
                    objCabecera.ID_FACTURADO = this.CboAsumeFactura.SelectedValue;
                    var ExisteAsume = CboAsumeFactura.Items.FindByValue(objCabecera.ID_FACTURADO.Trim());
                    if (ExisteAsume != null)
                    {
                        objCabecera.DESC_FACTURADO = ExisteAsume.Text.Split('-').ToList()[1].Trim();
                        this.hf_idasume.Value = objCabecera.ID_FACTURADO;
                        this.hf_descasume.Value = objCabecera.DESC_FACTURADO;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo obtener la información de la persona que asumirá la factura.</b>"));
                        return;
                    }

                    /***********************************************************************************************************************************************
                    *validaciones de carga suelta, si tiene pases 
                    **********************************************************************************************************************************************/
                    XDocument XMLContenedores = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                new XElement("CARGACFS", from p in objCabecera.Detalle.Where(x => x.VISTO == true).AsEnumerable().AsParallel()
                                                             select new XElement("CARGAS",
                                                         new XAttribute("IV_MRN", p.MRN == null ? "" : p.MRN.ToString()),
                                                         new XAttribute("IV_MSN", p.MSN == null ? "" : p.MSN.ToString()),
                                                         new XAttribute("IV_HSN", p.HSN == null ? "" : p.HSN.ToString()),
                                                         new XAttribute("IV_GKEY", p.GKEY == 0 ? "0" : p.GKEY.ToString()),
                                                         new XAttribute("IV_CAS", p.CAS.HasValue ? p.CAS.Value.ToString("yyyy/MM/dd") : ""),
                                                         new XAttribute("IV_BLOQUEO", p.BLOQUEO),
                                                         new XAttribute("IV_FECHA_HASTA", p.FECHA_HASTA.Value.ToString("yyyy/MM/dd")),
                                                         new XAttribute("IV_DOCUMENTO", p.DOCUMENTO == null ? "" : p.DOCUMENTO.ToString()))));


                    var Valor = objValidacion.Validacion_Carga_Suelta_FF(XMLContenedores.ToString());
                    if (Valor != string.Empty)
                    {
                        this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} </b>", Valor));
                        return;
                    }
                    /***********************************************************************************************************************************************
                    *fin: validaciones de carga suelta 
                    **********************************************************************************************************************************************/
                    /***********************************************************************************************************************************************
                    *datos del cliente N4, días de crédito 
                    **********************************************************************************************************************************************/
                    var Cliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, objCabecera.ID_FACTURADO);
                    if (Cliente.Exitoso)
                    {
                        var ListaCliente = Cliente.Resultado;
                        if (ListaCliente != null)
                        {
                            Cliente_Ruc = ListaCliente.CLNT_CUSTOMER.Trim();
                            Cliente_Rol = ListaCliente.CLNT_ROLE.Trim();
                            Cliente_Direccion = ListaCliente.CLNT_ADRESS.Trim();
                            Cliente_Ciudad = ListaCliente.CLNT_EMAIL;
                            DiasCredito = (ListaCliente.DIAS_CREDITO == null ? 0 : ListaCliente.DIAS_CREDITO.Value);
                            Cliente_CodigoSap = ListaCliente.CODIGO_SAP;
                            objCabecera.DIR_FACTURADO = (ListaCliente.CLNT_ADRESS == null ? "" : ListaCliente.CLNT_ADRESS);
                            objCabecera.EMAIL_FACTURADO = (ListaCliente.CLNT_EMAIL == null ? "" : ListaCliente.CLNT_EMAIL);
                            objCabecera.CIUDAD_FACTURADO = string.Empty;
                            objCabecera.DIAS_CREDITO = (ListaCliente.DIAS_CREDITO == null ? 0 : ListaCliente.DIAS_CREDITO.Value);
                        }
                        else
                        {
                            this.Limpia_Asume_Factura();
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabecera.ID_FACTURADO));

                            return;
                        }
                    }
                    else
                    {
                        this.Limpia_Asume_Factura();
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabecera.ID_FACTURADO, objCabecera.DESC_FACTURADO));

                        return;

                    }


                    /***********************************************************************************************************************************************
                    *fin: consulta datos de cliente
                    **********************************************************************************************************************************************/
                    /***********************************************************************************************************************************************
                    *validaciones de sap 
                    **********************************************************************************************************************************************/
                    //si tiene dias de credito, no se valida, ya que es sólo para clientes de contado
                    if (DiasCredito == 0)
                    {
                        List<Cls_Bil_Parametros_Sap> Parametros = Cls_Bil_Parametros_Sap.Parametros(out cMensajes);
                        if (Parametros != null)
                        {
                            var User = Parametros.Where(f => !string.IsNullOrEmpty(f.USER)).FirstOrDefault();
                            if (User != null)
                            {
                                sap_usuario = User.USER;
                                sap_clave = User.PASSWORD;
                                sap_valida = User.VALIDACION;
                            }
                            if (string.IsNullOrEmpty(sap_usuario) || string.IsNullOrEmpty(sap_clave))
                            {
                                sap_usuario = "sap";
                                sap_clave = "sap";
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error al obtener los parámetros del sistema SAP, por favor comunicar a CGSA... {0}</b>", cMensajes));
                            return;
                        }
                        if (sap_valida)
                        {
                            //bloqueo de tesoreria
                            var Bloqueo = PasePuerta.ClienteFacturacion.BloqueoTesoreria(LoginName, objCabecera.ID_FACTURADO);
                            if (Bloqueo.Exitoso)
                            {
                                Bloqueo_Cliente = Bloqueo.Resultado;
                            }
                            else
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error al obtener los datos de bloqueo de cliente, por favor comunicar a CGSA... {0}</b>", cMensajes));
                                return;
                            }

                            //si esta bloqueado, no permite facturar
                            if (Bloqueo_Cliente)
                            {
                                MensajesErrores = string.Format("El cliente {0} no esta Autorizado para la Facturación, presenta un bloqueo..", objCabecera.DESC_FACTURADO);


                                /*************************************************************************************************************************************
                                 * crear caso salesforce
                                 * **********************************************************************************************************************************/

                                this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Cliente Bloqueado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                    objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos,true);

                                /*************************************************************************************************************************************
                                * fin caso salesforce
                                * **********************************************************************************************************************************/

                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} {1} </b>", MensajesErrores, MensajeCasos));
                                return;
                            }

                            //bloqueo de tesoreria
                            var Liberado = PasePuerta.ClienteFacturacion.ClienteLiberado(LoginName, objCabecera.ID_FACTURADO);
                            if (Liberado.Exitoso)
                            {
                                Liberado_Cliente = Liberado.Resultado;
                            }
                            else
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error al obtener los datos de liberación de cliente, por favor comunicar a CGSA... {0}</b>", cMensajes));
                                return;
                            }

                            //si esta liberado
                            if (Liberado_Cliente)
                            {

                            }
                            else
                            {
                                var WsEstadoDeCuenta = new EstadoCuenta.Ws_Sap_EstadoDeCuentaSoapClient();
                                var RespEstadoCta = WsEstadoDeCuenta.SI_Customer_Statement_NAVIS_CGSA(objCabecera.ID_FACTURADO, sap_usuario, sap_clave);
                                if (RespEstadoCta == null)
                                {
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error en el WEBSERVICE SAP - OBJETO NULO, por favor comunicar a CGSA...</b>"));
                                    return;
                                }
                                var Nodoerror = RespEstadoCta.Descendants("ERROR").FirstOrDefault();
                                if (Nodoerror != null)
                                {
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error en la validación de SAP: {0}, por favor comunicar a CGSA...</b>", Nodoerror.Value));
                                    return;
                                }
                                else
                                {

                                    var NodoCab = RespEstadoCta.Descendants("CABECERA").FirstOrDefault();
                                    //verificar si  tiene bloqueo el cliente
                                    var Tagbloq = NodoCab.Element("BLOQUEO");
                                    if (Tagbloq != null && Tagbloq.Value.Contains("1"))
                                    {
                                        tieneBloqueo = true;
                                    }
                                    //cliente esta bloqueado
                                    if (tieneBloqueo)
                                    {
                                        MensajesErrores = string.Format("El cliente: {0} se encuentra con bloqueos...no podrá  generar la factura, por favor comunicarse con CGSA..", objCabecera.DESC_FACTURADO);

                                        /*************************************************************************************************************************************
                                        * crear caso salesforce
                                        * **********************************************************************************************************************************/

                                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Cliente Bloqueado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                            objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos,true);

                                        /*************************************************************************************************************************************
                                        * fin caso salesforce
                                        * **********************************************************************************************************************************/

                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} {1} </b>", MensajesErrores, MensajeCasos));
                                        return;
                                    }

                                    var Tagsaldo = NodoCab.Element("SALDO");
                                    if (Tagsaldo == null || string.IsNullOrEmpty(Tagsaldo.Value) || !decimal.TryParse(Tagsaldo.Value, style, enUS, out SaldoPendiente))
                                    {
                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error, el saldo fué nulo cuando se consulto el servicio SAP, por favor comunicar a CGSA...</b>"));
                                        return;
                                    }

                                    /*saco el valor permitido para facturacion $100, de parametros generales*/
                                    List<Cls_Bil_Configuraciones> ParametrosEstadocta = Cls_Bil_Configuraciones.Parametros(out cMensajes);
                                    if (!String.IsNullOrEmpty(cMensajes))
                                    {
                                        this.myModal.Attributes["class"] = "nover";
                                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en procesos de estado de cuenta, valor tope.....{0}</b>", cMensajes));
                                        this.Actualiza_Panele_Detalle();
                                        return;
                                    }

                                    var LinqValorTope = (from Tope in ParametrosEstadocta.Where(Tope => Tope.NOMBRE.Equals("ESTADOCUENTA"))
                                                         select new
                                                         {
                                                             VALOR = Tope.VALOR == null ? string.Empty : Tope.VALOR
                                                         }).FirstOrDefault();

                                    if (LinqValorTope != null)
                                    {
                                        NEstadoCuenta = decimal.Parse(LinqValorTope.VALOR);
                                    }
                                    else { NEstadoCuenta = 0; }

                                    /***************************fin valor permitido***************************/

                                    if (SaldoPendiente != 0 && SaldoPendiente > NEstadoCuenta)
                                    {

                                        this.fac_cliente.InnerText = objCabecera.DESC_FACTURADO;
                                        this.monto_fac.InnerText = SaldoPendiente.ToString("c");
                                        var TagValorVencido = NodoCab.Element("FACTURAS_VENCIDAS");
                                        var TagValorPendiente = NodoCab.Element("FACTURAS_PENDIENTES");


                                        //elementos tendran 0
                                        if (TagValorVencido != null && !string.IsNullOrEmpty(TagValorVencido.Value))
                                        {
                                            if (decimal.TryParse(TagValorVencido.Value, out ValorVencido))
                                            {
                                                fac_ven.InnerText = string.Format("{0:c}", ValorVencido);
                                            }
                                            else
                                            {
                                                fac_ven.InnerText = string.Format("{0:c}", 0); ;
                                            }
                                        }

                                        if (TagValorPendiente != null && !string.IsNullOrEmpty(TagValorPendiente.Value))
                                        {
                                            if (decimal.TryParse(TagValorPendiente.Value, out ValorPendiente))
                                            {
                                                fac_pend.InnerText = string.Format("{0:c}", ValorPendiente);
                                            }
                                            else
                                            {
                                                fac_pend.InnerText = string.Format("{0:c}", 0);
                                            }
                                        }

                                        this.myModal.Attributes["class"] = "modal-dialog";
                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! El cliente a facturar {0} tiene valores pendientes de pago...No podrá generar la factura</b>", objCabecera.DESC_FACTURADO));
                                        return;
                                    }
                                    else
                                    {
                                        this.myModal.Attributes["class"] = "nover";
                                    }
                                }
                            }


                        }
                    }
                    else
                    {

                        //bloqueo de tesoreria
                        var Bloqueo = PasePuerta.ClienteFacturacion.BloqueoTesoreria(LoginName, objCabecera.ID_FACTURADO);
                        if (Bloqueo.Exitoso)
                        {
                            Bloqueo_Cliente = Bloqueo.Resultado;
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error al obtener los datos de bloqueo de cliente, por favor comunicar a CGSA... {0}</b>", cMensajes));
                            return;
                        }

                        //si esta bloqueado, no permite facturar
                        if (Bloqueo_Cliente)
                        {

                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("El cliente: {0} no esta Autorizado para la Facturación, presenta un bloqueo", objCabecera.DESC_FACTURADO);

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Cliente Bloqueado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos,true);

                            /*************************************************************************************************************************************
                            * fin caso salesforce
                            * **********************************************************************************************************************************/

                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo!  {0} {1} </b>", MensajesErrores, MensajeCasos));
                            return;
                        }
                    }
                    /***********************************************************************************************************************************************
                    *fin: validaciones de sap 
                    **********************************************************************************************************************************************/


                    objCabecera.GLOSA = "";
                    Contenedores = string.Join(", ", LinqListContenedor);
                    //numero de carga
                    Numero_Carga = objCabecera.NUMERO_CARGA;
                    objCabecera.CONTENEDORES = Contenedores;
                    //objCabecera.FECHA_HASTA = FechaFactura;
                    LoginName = objCabecera.IV_USUARIO_CREA.Trim();
                    objCabecera.HORA_HASTA = HoraHasta;
                    //actualizo el objeto
                    Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;


                    Fila = 1;
                    Decimal Subtotal = 0;
                    Decimal Iva = 0;
                    Decimal Total = 0;
                    /***********************************************************************************************************************************************
                    *2) proceso para grabar factura
                    **********************************************************************************************************************************************/
                    /*************************************************************************************************************************************/
                    /*proceso para almacenar datos*/
                    objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    /*agrego datos a la factura*/
                    objFactura = Session["InvoiceCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Invoice_Cabecera;
                    objFactura = new Cls_Bil_Invoice_Cabecera();
                    objFactura.Detalle.Clear();
                    objFactura.DetalleServicios.Clear();
                    /*cabecera de factura*/
                    objFactura.IV_ID = objCabecera.ID;
                    objFactura.IV_GLOSA = objCabecera.GLOSA;
                    objFactura.IV_FECHA = objCabecera.FECHA;
                    objFactura.IV_TIPO_CARGA = objCabecera.TIPO_CARGA;
                    objFactura.IV_ID_AGENTE = objCabecera.ID_AGENTE;
                    objFactura.IV_DESC_AGENTE = objCabecera.DESC_AGENTE;
                    objFactura.IV_ID_CLIENTE = objCabecera.ID_CLIENTE;
                    objFactura.IV_DESC_CLIENTE = objCabecera.DESC_CLIENTE;
                    objFactura.IV_ID_FACTURADO = objCabecera.ID_FACTURADO;
                    objFactura.IV_DESC_FACTURADO = objCabecera.DESC_FACTURADO;
                    objFactura.IV_SUBTOTAL = objCabecera.SUBTOTAL;
                    objFactura.IV_IVA = objCabecera.IVA;
                    objFactura.IV_TOTAL = objCabecera.TOTAL;
                    objFactura.IV_USUARIO_CREA = objCabecera.IV_USUARIO_CREA;
                    objFactura.IV_FECHA_CREA = DateTime.Now;
                    objFactura.IV_NUMERO_CARGA = objCabecera.NUMERO_CARGA;
                    objFactura.IV_CONTENEDORES = objCabecera.CONTENEDORES;
                    objFactura.IV_FECHA_HASTA = objCabecera.FECHA_HASTA;

                    objFactura.IV_BL = objCabecera.BL;
                    objFactura.IV_BUQUE = objCabecera.BUQUE;
                    objFactura.IV_VIAJE = objCabecera.VIAJE;
                    objFactura.IV_FECHA_ARRIBO = objCabecera.FECHA_ARRIBO;
                    objFactura.IV_DIR_FACTURADO = objCabecera.DIR_FACTURADO;
                    objFactura.IV_EMAIL_FACTURADO = objCabecera.EMAIL_FACTURADO;
                    objFactura.IV_CIUDAD_FACTURADO = objCabecera.CIUDAD_FACTURADO;
                    objFactura.IV_DIAS_CREDITO = objCabecera.DIAS_CREDITO;
                    objFactura.IV_HORA_HASTA = objCabecera.HORA_HASTA;
                    objFactura.IV_TIPO_CLIENTE = objCabecera.TIPO_CLIENTE;
                    objFactura.IV_P2D = objCabecera.P2D;
                    objFactura.IV_RUC_USUARIO = objCabecera.RUC_USUARIO;
                    objFactura.IV_DESC_USUARIO = objCabecera.DESC_USUARIO;

                    string cip = Request.UserHostAddress;
                    objFactura.IV_IP = cip;
                    /*agrego detalle de contenedores a proforma*/
                    var LinqDetalle = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                       select p).ToList();

                    objFactura.IV_TOTAL_BULTOS = objCabecera.TOTAL_BULTOS;

                    /*detalle de factura*/
                    foreach (var Det in LinqDetalle)
                    {
                        objDetalleFactura = new Cls_Bil_Invoice_Detalle();
                        objDetalleFactura.IV_VISTO = Det.VISTO;
                        objDetalleFactura.IV_ID = Det.ID;
                        objDetalleFactura.IV_GKEY = Det.GKEY;
                        objDetalleFactura.IV_MRN = Det.MRN;
                        objDetalleFactura.IV_MSN = Det.MSN;
                        objDetalleFactura.IV_HSN = Det.HSN;
                        objDetalleFactura.IV_CONTENEDOR = Det.CONTENEDOR;
                        objDetalleFactura.IV_TRAFICO = Det.TRAFICO;
                        objDetalleFactura.IV_DOCUMENTO = Det.DOCUMENTO;
                        objDetalleFactura.IV_DES_BLOQUEO = Det.DES_BLOQUEO;
                        objDetalleFactura.IV_CONECTADO = Det.CONECTADO;
                        objDetalleFactura.IV_REFERENCIA = Det.REFERENCIA;
                        objDetalleFactura.IV_TAMANO = Det.TAMANO;
                        objDetalleFactura.IV_TIPO = Det.TIPO;
                        objDetalleFactura.IV_CAS = Det.CAS;
                        objDetalleFactura.IV_BOOKING = Det.BOOKING;

                        objDetalleFactura.IV_IMDT = Det.IMDT;
                        objDetalleFactura.IV_BLOQUEO = Det.BLOQUEO;
                        objDetalleFactura.IV_FECHA_ULTIMA = Det.FECHA_ULTIMA;
                        objDetalleFactura.IV_FECHA_HASTA = Det.FECHA_HASTA;
                        objDetalleFactura.IV_IN_OUT = Det.IN_OUT;
                        objDetalleFactura.IV_FULL_VACIO = Det.FULL_VACIO;
                        objDetalleFactura.IV_AISV = Det.AISV;
                        objDetalleFactura.IV_REEFER = Det.REEFER;
                        objDetalleFactura.IV_USUARIO_CREA = Det.IV_USUARIO_CREA;
                        objDetalleFactura.IV_FECHA_CREA = DateTime.Now;
                        objDetalleFactura.IV_CNTR_DD = Det.CNTR_DD;

                        //30-01-2020
                        objDetalleFactura.IV_FECHA_TOPE_DLIBRE = Det.FECHA_TOPE_DLIBRE;
                        objDetalleFactura.IV_CNTR_DESCARGA = Det.CNTR_DESCARGA;
                        objDetalleFactura.IV_MODULO = Det.MODULO;
                        objDetalleFactura.IV_CNTR_DEPARTED = Det.CNTR_DEPARTED;//FECHA ZARPE
                        objDetalleFactura.IV_TIENE_SERVICIOS = false;
                        

                        //CAMPOS NUEVOS
                        objDetalleFactura.IV_CANTIDAD = Det.CANTIDAD;
                        objDetalleFactura.IV_PESO = Det.PESO;
                        objDetalleFactura.IV_OPERACION = Det.OPERACION;
                        objDetalleFactura.IV_DESCRIPCION = Det.DESCRIPCION;
                        objDetalleFactura.IV_EXPORTADOR = Det.EXPORTADOR;
                        objDetalleFactura.IV_AGENCIA = Det.AGENCIA;
                        objDetalleFactura.ID_UNIDAD = Det.ID_UNIDAD;

                        //CARBONO NEUTRO
                        objDetalleFactura.IV_CERTIFICADO = Det.CERTIFICADO;
                        objDetalleFactura.IV_TIENE_CERTIFICADO = string.IsNullOrEmpty(Det.TIENE_CERTIFICADO) ? "NO" : Det.TIENE_CERTIFICADO;

                        objFactura.Detalle.Add(objDetalleFactura);

                    }

                  
                    /**********************************************************************************************************************************
                    * carga servicio de transporte.
                    **********************************************************************************************************************************/
                    //si tiene una cotizacion activa, pregunto si tiene ya cargado el servicio

                    XDocument XMLUnidadesValidar = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                           new XElement("CONTENEDORES", from p in objCabecera.Detalle.Where(x => x.VISTO == true).AsParallel()
                                                        select new XElement("CONTENEDOR",
                                                 new XAttribute("gkey", p.ID_UNIDAD == 0 ? "0" : p.ID_UNIDAD.ToString()),
                                                 new XAttribute("contenedor", p.CONTENEDOR == null ? "" : p.CONTENEDOR.Trim())
                                                 )));

                    //valida si tiene ya cargado el servicio de transporte, ya no muestra el mensaje de que tiene cotizaciones.
                    List<P2D_Valida_Proforma> ListTransp = P2D_Valida_Proforma.Validacion_Servicio_Transporte_Cfs(XMLUnidadesValidar.ToString(), out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error en obtener datos de servicio de transporte....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                    /*listado de unidades sin el servicio*/
                    var LinqTransp = (from TblFact in ListTransp.Where(TblFact => TblFact.gkey != 0 && TblFact.servicio == 0)
                                      select new
                                      {
                                          gkey = TblFact.gkey,
                                          contenedor = TblFact.contenedor,
                                          servicio = TblFact.servicio
                                      }).Distinct().FirstOrDefault();

                    if (LinqTransp != null)
                    {
                        objTarja = new P2D_Tarja_Cfs();
                        objTarja.mrn = this.TXTMRN.Text.Trim();
                        objTarja.msn = this.TXTMSN.Text.Trim();
                        objTarja.hsn = this.TXTHSN.Text.Trim();
                        objTarja.ruc = string.IsNullOrEmpty(ClsUsuario.ruc) ? "" : ClsUsuario.ruc.Trim();

                        if (!objTarja.PopulateMyData(out cMensajes))
                        {
                           
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Datos del peso de la carga!: {0}</b>", cMensajes));
                            return;
                        }

                        //busco si existe en el tarifario
                        objTarifa = new P2D_Tarifario();
                        objTarifa.M3 = objTarja.m3;
                        objTarifa.PESO = objTarja.pesokg;

                        if (!objTarifa.PopulateMyData(out cMensajes))
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo!: {0}</b>", cMensajes));
                            return;
                        }

                        //marcar servicio
                        var ResultadoEvt = P2D_Servicio_Transporte.Marcar_Servicio_Transporte_Cfs(LoginName, LinqTransp.gkey, objTarifa.VALOR, 0);
                        if (ResultadoEvt.Exitoso)
                        {

                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar servicios de transporte: {0}, Existen los siguientes problemas: {1} </b>", ResultadoEvt.MensajeInformacion, ResultadoEvt.MensajeProblema));
                            return;
                        }

                    }
                    else
                    {
                       // this.Mostrar_Mensaje(2, string.Format("<b>Informativo! El número de carga {0} ya tiene cargado el servicios de transporte Port To Door </b>", objFactura.IV_NUMERO_CARGA));
                        //return;

                    }
                    /**********************************************************************************************************************************
                    * fin carga servicio de transporte
                    **********************************************************************************************************************************/



                    var FechaContainer_Filtro = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                                    select new { FECHA_HASTA = p.FECHA_HASTA.Value }).FirstOrDefault();

                    var LinqDetcontainer = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                            select p.CONTENEDOR).ToList();

                    Contenedores = string.Join(",", LinqDetcontainer);//listado contenedores

                    /***********************************************************************************************************************************************
                    *4) Consulta de Servicios a facturar N4 
                    **********************************************************************************************************************************************/
                    var Validacion = new Aduana.Importacion.ecu_validacion_cntr_cfs();
                    var Contenedor = new N4.Importacion.container_cfs();
                    var Billing = new N4Ws.Entidad.billing();
                    var Ws = new N4Ws.Entidad.InvoiceRequest();

                    /*saco el invoice type*/
                    string pInvoiceType = string.Empty;
                    var InvoiceType = InvoiceTypeConfig.ObtenerInvoicetypes();
                    if (InvoiceType.Exitoso)
                    {
                        var LinqInvoiceType = (from p in InvoiceType.Resultado.Where(X => X.codigo.Equals("IMPOP2D"))
                                                select new { valor = p.valor }).FirstOrDefault();

                        pInvoiceType = LinqInvoiceType.valor == null ? "2DA_MAN_CFS_BBK_ONE" : LinqInvoiceType.valor;
                    }
                    /*fin invoice type*/

                    Ws.action = N4Ws.Entidad.Action.INQUIRE;
                    Ws.requester = N4Ws.Entidad.Requester.QUERY_CHARGES;
                    Ws.InvoiceTypeId = pInvoiceType;
                    Ws.payeeCustomerId = Cliente_Ruc;
                    Ws.payeeCustomerBizRole = Cliente_Rol;

                    var Direccion = new N4Ws.Entidad.address();
                    Direccion.addressLine1 = string.Empty;
                    Direccion.city = "GUAYAQUIL";

                    var Parametro = new N4Ws.Entidad.invoiceParameter();
                    Parametro.bexuPaidThruDay = FechaContainer_Filtro.FECHA_HASTA.ToString("yyyy-MM-dd HH:mm");
                    Parametro.bexuBlNbr = Numero_Carga;

                    Ws.invoiceParameters.Add(Parametro);
                    Ws.billToParty.Add(Direccion);
                    Ws.bexuBlNbr = Numero_Carga;
                    Billing.Request = Ws;

                    //resultado query billing, de una consulta especifica de fecha
                    var Resultado = Servicios.N4ServicioBasico(Billing, LoginName);
                    if (Resultado != null)
                    {
                        //servicios ok
                        if (Resultado.status_id.Equals("OK"))
                        {
                            var xBilling = Resultado;

                            FechaPaidThruDay = null;
                            CargabexuBlNbr = null;
                            Fila = 1;

                            if (!Int64.TryParse(xBilling.response.billInvoice.gkey, out Gkey))
                            {

                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se puede convertir en campo numerico el gkey: {0}</b>", xBilling.response.billInvoice.gkey));
                                return;
                            }


                            TipoServicio = xBilling.response.billInvoice.type;

                            FechaPaidThruDay = (from bexuPaidThruDay in xBilling.response.billInvoice.invoiceParameters.Where(p => p.Metafield == "bexuPaidThruDay")
                                                select new
                                                {
                                                    fecha = bexuPaidThruDay.Value.ToString()
                                                }
                                        ).FirstOrDefault().fecha;

                            CargabexuBlNbr = (from bexuBlNbr in xBilling.response.billInvoice.invoiceParameters.Where(p => p.Metafield == "bexuBlNbr")
                                                select new
                                                {
                                                    carga = bexuBlNbr.Value == null ? null : bexuBlNbr.Value.ToString()
                                                }).FirstOrDefault().carga;


                            var LinqServicios = (from invoice in xBilling.response.billInvoice.invoiceCharges
                                                    select new
                                                    {
                                                        TOTAL = invoice.totalCharged,
                                                        SERVICIO = invoice.description,
                                                        CARGA = String.IsNullOrEmpty(CargabexuBlNbr) != true ? CargabexuBlNbr : invoice.chargeEntityId,
                                                        CODIGO = invoice.chargeGlCode,
                                                        CANTIDAD = invoice.quantityBilled,
                                                        PRECIO = invoice.rateBilled,
                                                        IVA = invoice.totalTaxes,
                                                        FECHA = FechaPaidThruDay,
                                                        TIPO = TipoServicio
                                                    }
                                            ).ToList();

                            var LinqServiciosAgrupados = (from p in LinqServicios.AsEnumerable()
                                                            group p by new { CARGA = p.CARGA, CODIGO = p.CODIGO, SERVICIO = p.SERVICIO, PRECIO = p.PRECIO, FECHA = p.FECHA, INVOICETYPE = p.TIPO } into Grupo
                                                            select new
                                                            {
                                                                TOTAL = Grupo.Sum(g => Decimal.Round(Decimal.Parse(g.TOTAL != null ? g.TOTAL : "0", enUS), 2)),
                                                                SERVICIO = Grupo.Key.SERVICIO.ToString().Trim(),
                                                                CARGA = Grupo.Key.CARGA,
                                                                CODIGO = Grupo.Key.CODIGO.ToString().Trim(),
                                                                CANTIDAD = Grupo.Sum(g => Decimal.Round(Decimal.Parse(g.CANTIDAD != null ? g.CANTIDAD : "0", enUS), 2)),
                                                                PRECIO = Decimal.Round(Decimal.Parse(Grupo.Key.PRECIO != null ? Grupo.Key.PRECIO : "0", enUS), 2),
                                                                IVA = Grupo.Sum(g => Decimal.Round(Decimal.Parse(g.IVA != null ? g.IVA : "0", enUS), 2)),
                                                                FECHA = Grupo.Key.FECHA,
                                                                INVOICETYPE = Grupo.Key.INVOICETYPE,

                                                            }).ToList();

                            foreach (var Det in LinqServiciosAgrupados)
                            {
                                objServiciosFactura = new Cls_Bil_Invoice_Servicios();
                                objServiciosFactura.IV_ID = 0;
                                objServiciosFactura.IV_LINEA = Fila;
                                objServiciosFactura.IV_ID_SERVICIO = Det.CODIGO;
                                objServiciosFactura.IV_DESC_SERVICIO = Det.SERVICIO;
                                objServiciosFactura.IV_CARGA = Det.CARGA;
                                objServiciosFactura.IV_FECHA = DateTime.Parse(Det.FECHA.ToString());
                                objServiciosFactura.IV_TIPO_SERVICIO = TipoServicio;
                                objServiciosFactura.IV_CANTIDAD = Det.CANTIDAD;
                                objServiciosFactura.IV_PRECIO = Det.PRECIO;
                                objServiciosFactura.IV_SUBTOTAL = Det.TOTAL;
                                objServiciosFactura.IV_IVA = Det.IVA;
                                objServiciosFactura.IV_USUARIO_CREA = LoginName;
                                objServiciosFactura.IV_FECHA_CREA = DateTime.Now;
                                Fila++;
                                objFactura.DetalleServicios.Add(objServiciosFactura);

                            }

                            Iva = Iva + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTaxes != null ? xBilling.response.billInvoice.totalTaxes : "0", enUS), 2);
                            Total = Total + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTotal != null ? xBilling.response.billInvoice.totalTotal : "0", enUS), 2);

                        }//FIN OK 

                    }//FIN RESULTADO

                    /***********************************************************************************************************************************************
                    *fin: Consulta de Servicios a facturar N4 
                    **********************************************************************************************************************************************/
                    /***********************************************************************************************************************************************
                    * validaciones de sap/clientes de credito
                    **********************************************************************************************************************************************/
                    //si tiene dias de credito, no se valida, ya que es sólo para clientes de credito
                    if (DiasCredito != 0)
                    {
                        List<Cls_Bil_Parametros_Sap> Parametros = Cls_Bil_Parametros_Sap.Parametros(out cMensajes);
                        if (Parametros != null)
                        {
                            var User = Parametros.Where(f => !string.IsNullOrEmpty(f.USER)).FirstOrDefault();
                            if (User != null)
                            {
                                sap_usuario = User.USER;
                                sap_clave = User.PASSWORD;
                                sap_valida = User.VALIDACION;
                            }
                            if (string.IsNullOrEmpty(sap_usuario) || string.IsNullOrEmpty(sap_clave))
                            {
                                sap_usuario = "sap";
                                sap_clave = "sap";
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error al obtener los parámetros del sistema SAP, por favor comunicar a CGSA... {0}</b>", cMensajes));
                            return;
                        }

                        if (sap_valida)
                        {
                            /*saco el valor permitido para facturacion $100, de parametros generales*/
                            List<Cls_Bil_Configuraciones> ParametrosEstadocta = Cls_Bil_Configuraciones.Get_Validacion("VALIDASALDO", out cMensajes);
                            if (!String.IsNullOrEmpty(cMensajes))
                            {
                                this.myModal.Attributes["class"] = "nover";
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en procesos de estado de cuenta, valor cupo.....{0}</b>", cMensajes));
                                this.Actualiza_Panele_Detalle();
                                return;
                            }

                            var LinqValorTope = (from Tope in ParametrosEstadocta.Where(Tope => Tope.NOMBRE.Equals("VALIDASALDO"))
                                                 select new
                                                 {
                                                     VALOR = 1
                                                 }).FirstOrDefault();

                            bool ValidaCupo = false;
                            //si valida control de cupo de credito clientes de CREDITO
                            if (LinqValorTope != null)
                            {
                                ValidaCupo = true;
                            }
                            else { ValidaCupo = false; }

                            if (ValidaCupo)
                            {

                                var WsEstadoDeCuenta = new CSLSite.EstadoCuenta.Ws_Sap_EstadoDeCuentaSoapClient();
                                var RespEstadoCta = WsEstadoDeCuenta.SI_Customer_Statement_NAVIS_CGSA(objCabecera.ID_FACTURADO, sap_usuario, sap_clave);
                                if (RespEstadoCta == null)
                                {
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error en el WEBSERVICE SAP - OBJETO NULO, por favor comunicar a CGSA...</b>"));
                                    return;
                                }
                                var Nodoerror = RespEstadoCta.Descendants("ERROR").FirstOrDefault();
                                if (Nodoerror != null)
                                {
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error en la validación de SAP: {0}, por favor comunicar a CGSA...</b>", Nodoerror.Value));
                                    return;
                                }

                                var NodoCab = RespEstadoCta.Descendants("CABECERA").FirstOrDefault();
                                //verificar si  tiene bloqueo el cliente
                                var Tagbloq = NodoCab.Element("BLOQUEO");
                                if (Tagbloq != null && Tagbloq.Value.Contains("1"))
                                {
                                    tieneBloqueo = true;
                                }
                                //cliente esta bloqueado
                                if (tieneBloqueo)
                                {
                                    MensajesErrores = string.Format("El cliente: {0} se encuentra con bloqueos...no podrá  generar la factura, por favor comunicarse con CGSA..", objCabecera.DESC_FACTURADO);

                                    /*************************************************************************************************************************************
                                    * crear caso salesforce
                                    * **********************************************************************************************************************************/

                                    this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "Cliente Bloqueado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                        objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos, true);

                                    /*************************************************************************************************************************************
                                    * fin caso salesforce
                                    * **********************************************************************************************************************************/

                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} {1} </b>", MensajesErrores, MensajeCasos));
                                    return;
                                }

                                var Tagsaldo = NodoCab.Element("SALDO");
                                var Tagcupo = NodoCab.Element("CREDITO");//cupo de credito
                                var TagValorVencido = NodoCab.Element("FACTURAS_VENCIDAS");
                                var TagValorPendiente = NodoCab.Element("FACTURAS_PENDIENTES");

                                if (Tagsaldo == null || string.IsNullOrEmpty(Tagsaldo.Value) || !decimal.TryParse(Tagsaldo.Value, style, enUS, out SaldoPendiente))
                                {
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error, el saldo fué nulo cuando se consulto el servicio SAP, por favor comunicar a CGSA...</b>"));
                                    return;
                                }
                                if (Tagcupo == null || string.IsNullOrEmpty(Tagcupo.Value) || !decimal.TryParse(Tagcupo.Value, style, enUS, out Cupo))
                                {
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error, el cupo fué nulo cuando se consulto el servicio SAP, por favor comunicar a CGSA...</b>"));
                                    return;
                                }

                                //elementos tendran 0
                                if (TagValorVencido != null && !string.IsNullOrEmpty(TagValorVencido.Value))
                                {
                                    if (decimal.TryParse(TagValorVencido.Value, out ValorVencido))
                                    {
                                    }
                                }
                                else { ValorVencido = 0; }

                                if (TagValorPendiente != null && !string.IsNullOrEmpty(TagValorPendiente.Value))
                                {
                                    if (decimal.TryParse(TagValorPendiente.Value, out ValorPendiente))
                                    {
                                    }
                                }
                                else { ValorPendiente = 0; }



                                if (SaldoPendiente > Cupo)
                                {
                                    this.fac_cliente.InnerText = objCabecera.DESC_FACTURADO;
                                    this.monto_fac.InnerText = SaldoPendiente.ToString("c");

                                    fac_cupo.InnerText = string.Format("{0:c}", Cupo);

                                    fac_ven.InnerText = string.Format("{0:c}", ValorVencido);
                                    fac_pend.InnerText = string.Format("{0:c}", ValorPendiente);

                                    this.myModal.Attributes["class"] = "modal-dialog";
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! El cliente a facturar {0} tiene valores pendientes de pago...No podrá generar la factura</b>", objCabecera.DESC_FACTURADO));
                                    return;
                                }
                                else
                                {
                                    decimal nTotalCredito = (Cupo - SaldoPendiente);
                                    decimal nValorFacturar = Total;
                                    if (nValorFacturar > nTotalCredito)
                                    {
                                        this.fac_cliente.InnerText = objCabecera.DESC_FACTURADO;
                                        this.monto_fac.InnerText = SaldoPendiente.ToString("c");

                                        fac_cupo.InnerText = string.Format("{0:c}", Cupo);

                                        fac_ven.InnerText = string.Format("{0:c}", ValorVencido);
                                        fac_pend.InnerText = string.Format("{0:c}", ValorPendiente);

                                        this.myModal.Attributes["class"] = "modal-dialog";
                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! El cliente a facturar {0} tiene valores pendientes de pago...No podrá generar la factura, puede emitir una factura por: {1}</b>", objCabecera.DESC_FACTURADO, string.Format("{0:c}", nTotalCredito)));
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                this.myModal.Attributes["class"] = "nover";
                            }

                        }
                    }
                    /***********************************************************************************************************************************************
                    *fin: validaciones de sap/clientes de credito
                    **********************************************************************************************************************************************/




                    var LinqSubtotal = (from Servicios in objFactura.DetalleServicios.AsEnumerable()
                                        select Servicios.IV_SUBTOTAL
                                                   ).Sum();

                    Subtotal = LinqSubtotal;
                    objFactura.IV_SUBTOTAL = Subtotal;
                    objFactura.IV_IVA = Iva;
                    objFactura.IV_TOTAL = Total;

                    //actualiza sesion
                    objCabecera.SUBTOTAL = Subtotal;
                    objCabecera.IVA = Iva;
                    objCabecera.TOTAL = Total;

                    Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;
                    Session["InvoiceCFS" + this.hf_BrowserWindowName.Value] = objFactura;

                    /***********************************************************************************************************************************************
                    *pasar a la siguiente ventana
                    **********************************************************************************************************************************************/
                    if (objFactura == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No existe objeto para generar la visualización de la factura</b>"));
                        return;
                    }
                    else
                    {
                        if (Subtotal == 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen servicios pendientes para facturar carga suelta, servicio Port To Door.</br> {0} ", ""));
                            return;
                        }

                            this.Ocultar_Mensaje();
                        string cId = securetext(this.hf_BrowserWindowName.Value);
                        Response.Redirect("~/cargacfs/facturafreightforwarderp2d_preview.aspx?id=" + cId.Trim() + "", false);

                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "Factura CFS", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));


                }

            }

        }

   
       

     

        #endregion

        #region "Eventos Check"
      
        #endregion

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }


       

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {

        }





    }
}