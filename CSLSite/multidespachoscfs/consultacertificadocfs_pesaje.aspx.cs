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


    public partial class consultacertificadocfs_pesaje : System.Web.UI.Page
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
       
        private List<Cls_Bil_Cas_Manual> List_Autorizacion { set; get; }
        private Cls_Bil_Log_Appcgsa objLogAppCgsa = new Cls_Bil_Log_Appcgsa();
 
        private MantenimientoPaqueteCliente obj = new MantenimientoPaqueteCliente();
        private Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();

        private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();

        private P2D_Proforma_Cabecera objProformaCab = new P2D_Proforma_Cabecera();
        private P2D_Proforma_Detalle objProformaDet = new P2D_Proforma_Detalle();
        private P2D_Tarifario objTarifa = new P2D_Tarifario();
        private P2D_Tarja_Cfs objTarja = new P2D_Tarja_Cfs();
        private Cls_Bil_PasePuertaCFS_SubItems objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
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

        private void mensaje()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mensaje('');", true);
        }

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
           
            UPCARGA.Update();
            UPDATOSCLIENTE.Update();
           
            UPBOTONES.Update();
          

        }

        private void Actualiza_Panele_Detalle()
        {
            
            UPTARJA.Update();
        }

        private void Limpia_Datos_cliente()
        {
            this.TXTCLIENTE.Text = string.Empty;
           
        }

        private void Limpia_Asume_Factura()
        {
          
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
#if !DEBUG
                this.IsAllowAccess();
#endif
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
                          
                            this.TXTAGENCIA.Text = string.Empty;
                          

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
                               
                                this.TXTAGENCIA.Text = numero_carga.Split('+').ToList()[4].Trim();

                                this.hf_idagente.Value = this.TXTAGENCIA.Text.Trim();
                                this.hf_idcliente.Value = numero_carga.Split('+').ToList()[5].Trim();
                               
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

                               
                                this.Actualiza_Paneles();
                            }


                        }
                    }

                }
                else
                {
                    ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
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

                    this.BtnVer.Attributes.Add("disabled", "disabled");
                    this.BtnDescargar.Attributes.Add("disabled", "disabled");

                    this.mensaje_proforma.Visible = false;

                  
                    //texto para leyenda de carbono neutro
                 

                  
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

      

       
     
     

#endregion

#region "Evento Botones"

    
   



        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    objTarja = new P2D_Tarja_Cfs();
                    objTarja.mrn = this.TXTMRN.Text.Trim();
                    objTarja.msn = this.TXTMSN.Text.Trim();
                    objTarja.hsn = this.TXTHSN.Text.Trim();
                    objTarja.ruc = string.IsNullOrEmpty(ClsUsuario.ruc) ? "" : ClsUsuario.ruc.Trim();
                    //objTarja.apilable = this.ChkTodos.Checked;
                    if (!objTarja.PopulateMyData(out cMensajes))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo!: {0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                    //busco si existe en el tarifario
                    objTarifa = new P2D_Tarifario();
                    objTarifa.M3 = objTarja.m3;
                    objTarifa.PESO = objTarja.pesokg;

                    if (!objTarifa.PopulateMyData(out cMensajes))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo!: {0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                    string Msg = string.IsNullOrEmpty(TextoServicio) ? "" : string.Format(TextoServicio, objTarja.pesokg.ToString("N2"), objTarja.m3.ToString("N2"), objTarifa.TOTAL_PAGAR);

                    
                    

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ChkTodos_CheckedChanged), "ChkTodos_CheckedChanged", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
                    return;

                }
            }

        }

      

   

        //revisando 1
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    this.BtnVer.Attributes.Add("disabled", "disabled");
                    this.BtnDescargar.Attributes.Add("disabled", "disabled");

                    OcultarLoading("2");
                    bool Ocultar_Mensaje = true;
                    bool cancelado = false;
                    
                    string Msg = string.Empty;

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

                 

                    /*saco los dias libre como parametros generales*/
                    List<Cls_Bil_Configuraciones> Parametros = Cls_Bil_Configuraciones.Parametros( out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en procesos de días libres.....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }
                  
                    var LinqDiasLibres = (from Dias in Parametros.Where(Dias => Dias.NOMBRE.Equals("DIASLIBRES"))
                                             select new
                                             {
                                                 VALOR = Dias.VALOR==null ? string.Empty : Dias.VALOR
                                             }).FirstOrDefault();

                    if (LinqDiasLibres != null)
                    {
                        NDiasLibreas = int.Parse(LinqDiasLibres.VALOR);
                    }
                    else { NDiasLibreas = 0; }

                    /***************************fin de dias libres***************************/


                   /*para almacenar clientes que asumen factura*/
                    List_Asume = new List<Cls_Bil_AsumeFactura>();
                    List_Asume.Clear();
                    List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = string.Empty, nombre = string.Empty, mostrar = string.Format("{0}", "Seleccionar...") });

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdAgenteCodigo = string.Empty;
                    var AgenteCod = N4.Entidades.Agente.ObtenerAgentePorRuc(ClsUsuario.loginname, ClsUsuario.ruc);
                    if (AgenteCod.Exitoso)
                    {
                        var ListaAgente = AgenteCod.Resultado;
                        if (ListaAgente != null)
                        {
                            IdAgenteCodigo = ListaAgente.codigo;
                        }
                    }

                    var Validacion = new Aduana.Importacion.ecu_validacion_cntr_cfs();
                    var EcuaContenedores = Validacion.CargaPorManifiestoImpo(ClsUsuario.loginname, ClsUsuario.ruc, IdAgenteCodigo, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), true);//ecuapas contenedores
                    if (EcuaContenedores.Exitoso)
                    {
                        //DATOS DEL AGENTE PARA BUSCAR INFORMACION
                        var LinqAgente = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                            select new
                                            {
                                                ID_AGENTE = Tbl.agente_id,
                                                ID_CLIENTE = Tbl.importador_id,
                                                DESC_CLIENTE = (Tbl.importador==null ? string.Empty : Tbl.importador)
                                            }).FirstOrDefault();

                        this.hf_idagente.Value = LinqAgente.ID_AGENTE;
                        this.hf_idcliente.Value = LinqAgente.ID_CLIENTE;
                       

                        this.hf_rucagente.Value = string.Empty;
                        this.hf_descagente.Value = string.Empty;
                       
                        this.hf_desccliente.Value = LinqAgente.DESC_CLIENTE;

                        //LLENADO DE CAMPOS DE PANTALLA DEL AGENTE
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

                        //LLENADO DE CAMPOS DE PANTALLA DEL CLIENTE
                        var Cliente = N4.Entidades.Cliente.ObtenerCliente(ClsUsuario.loginname, this.hf_idcliente.Value);
                        if (Cliente.Exitoso)
                        {
                            var ListaCliente = Cliente.Resultado;
                            if (ListaCliente != null)
                            {
                                this.TXTCLIENTE.Text = string.Format("{0} - {1}", ListaCliente.CLNT_CUSTOMER.Trim(), ListaCliente.CLNT_NAME.Trim());
                             
                                this.hf_desccliente.Value = ListaCliente.CLNT_NAME.Trim();
                           
                                //agrega importador
                                List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = ListaCliente.CLNT_CUSTOMER.Trim(), nombre = ListaCliente.CLNT_NAME.Trim(), mostrar = string.Format("{0} - {1}", ListaCliente.CLNT_CUSTOMER.Trim(), ListaCliente.CLNT_NAME.Trim()) });
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
                                                    CARGA = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn)
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


                            /*left join de contenedores*/
                            var LinqQuery = (from Tbl in LinqPartidadN4
                                                join EcuaPartidas in LinqPartidas on Tbl.CARGA equals EcuaPartidas.CARGA into TmpFinal
                                             from Final in TmpFinal.DefaultIfEmpty()
                                             select new
                                                {
                                                    CONTENEDOR = Tbl.CNTR_CONTAINER,
                                                    REFERENCIA = (Tbl.CNTR_VEPR_REFERENCE == null) ? string.Empty : Tbl.CNTR_VEPR_REFERENCE,
                                                    TRAFICO = (Tbl.CNTR_TYPE == null) ? string.Empty : Tbl.CNTR_TYPE,
                                                    TAMANO = (Tbl.CNTR_TYSZ_SIZE == null) ? string.Empty : Tbl.CNTR_TYSZ_SIZE,
                                                    TIPO = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
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
                                                    VIAJE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                                    NAVE = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                                    FECHA_ARRIBO = Tbl.CNTR_VEPR_ACTUAL_ARRIVAL,
                                                    CNTR_DD = Tbl.CNTR_DD,
                                                    ESTADO_RIDT = (Final.ESTADO_RIDT == null) ? string.Empty : Final.ESTADO_RIDT,
                                                    CNTR_DESCARGA = (Tbl.CNTR_DESCARGA==null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                                    CNTR_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
                                                    CANTIDAD = Tbl.CNTR_CANTIDAD,
                                                    PESO = Tbl.CNTR_PESO ,
                                                    OPERACION = Tbl.CNTR_OPERACION ,
                                                    DESCRIPCION = Tbl.CNTR_DESCRIPCION,
                                                    EXPORTADOR = Tbl.CNTR_EXPORTADOR ,
                                                    AGENCIA = Tbl.CNTR_AGENCIA,
                                                    CARGA = Tbl.CARGA,
                                                    ID_UNIDAD = Tbl.ID_UNIDAD ,
                                             }).OrderBy(x => x.IN_OUT).ThenBy(x=> x.CONTENEDOR);

                            if (LinqQuery != null && LinqQuery.Count() > 0)
                            {

                                
                                //agrego todos los contenedores a la clase cabecera
                                objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;

                                objCabecera.ID_CLIENTE = this.hf_idcliente.Value;
                                objCabecera.DESC_CLIENTE = this.hf_desccliente.Value;
                                objCabecera.ID_FACTURADO = this.hf_idcliente.Value;
                                objCabecera.DESC_FACTURADO = this.hf_desccliente.Value;
                                objCabecera.ID_UNICO_AGENTE = this.hf_idagente.Value;
                                objCabecera.ID_AGENTE = this.hf_rucagente.Value;
                                objCabecera.DESC_AGENTE = this.hf_descagente.Value;
                                objCabecera.FECHA = DateTime.Now;
                                objCabecera.TIPO_CARGA = "CFS";
                                objCabecera.NUMERO_CARGA = this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" + this.TXTHSN.Text.Trim();
                                objCabecera.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objCabecera.SESION = this.hf_BrowserWindowName.Value;
                                objCabecera.HORA_HASTA = "00:00";

                                objCabecera.Detalle.Clear();
                                objCabecera.DetalleSubItem.Clear();

                                Int16 Secuencia = 1;

                                string _MRN = string.Empty;
                                string _MSN = string.Empty;
                                string _HSN = string.Empty;
                                Int64 _GKEY = 0;

                                foreach (var Det in LinqQuery)
                                {
                                    /*datos nuevos para imprimir factura*/
                                    objCabecera.BL = Det.BL;
                                    objCabecera.BUQUE = Det.NAVE;
                                    objCabecera.VIAJE = Det.VIAJE;
                                    objCabecera.FECHA_ARRIBO = Det.FECHA_ARRIBO;
                                    

                                    objDetalle = new Cls_Bil_Detalle();
                                    objDetalle.VISTO = false;
                                    objDetalle.ID = 0;
                                    objDetalle.SECUENCIA = Secuencia;
                                    objDetalle.GKEY = Det.GKEY;
                                    objDetalle.MRN = this.TXTMRN.Text.Trim();
                                    objDetalle.MSN = this.TXTMSN.Text.Trim();
                                    objDetalle.HSN = this.TXTHSN.Text.Trim();

                                     _MRN = objDetalle.MRN;
                                    _MSN = objDetalle.MSN;
                                     _HSN = objDetalle.HSN;
                                     _GKEY = objDetalle.GKEY;

                                    objDetalle.CONTENEDOR = Det.CONTENEDOR;
                                    objDetalle.TRAFICO = Det.TRAFICO;
                                    objDetalle.DOCUMENTO = Det.DOCUMENTO;
                                    objDetalle.DES_BLOQUEO = Det.BLOQUEOS;
                                    objDetalle.CONECTADO = Det.CONECTADO;
                                    objDetalle.REFERENCIA = Det.REFERENCIA;
                                    objDetalle.TAMANO = Det.TAMANO;
                                    objDetalle.TIPO = "CFS";
                                    objDetalle.CAS = DateTime.Now;
                                    objDetalle.AUTORIZADO =  "SI" ;
                                    objDetalle.BOOKING = "";

                                    objDetalle.IMDT = Det.IMDT;
                                    objDetalle.BLOQUEO = Det.BLOQUEADO;
     
                                    objDetalle.FECHA_ULTIMA = DateTime.Now;
                                    objDetalle.IN_OUT = Det.IN_OUT;
                                    objDetalle.FULL_VACIO = Det.FULL_VACIO;
                                    objDetalle.AISV = Det.AISV;
                                    objDetalle.REEFER = Det.TIPO_CONTENEDOR;
                                    objDetalle.IV_USUARIO_CREA = ClsUsuario.loginname.Trim();
                                    objDetalle.IV_FECHA_CREA = DateTime.Now;
            
                                    objDetalle.CNTR_DD = Det.CNTR_DD;
                                    objDetalle.FECHA_HASTA = DateTime.Now;
                                    objDetalle.ESTADO_RDIT = Det.ESTADO_RIDT.Trim();
                                    objDetalle.CNTR_DESCARGA = Det.CNTR_DESCARGA;
                                    objDetalle.MODULO = "V";
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

                                  

                                    objCabecera.Detalle.Add(objDetalle);
                                    Secuencia++;

                                    if (Secuencia == 1)
                                    {
                                       
                                    }
                                }

                           
                                //consulta detalle de subitems
                                var Tarja = PasePuerta.Pase_WebCFS.ObtenerTarjaCFS_Ver_Certificado(_MRN, _MSN, _HSN);
                                if (Tarja.Exitoso)
                                {
                                    var LinqDetTarja = (from Tbl in Tarja.Resultado.Where(Tbl => Tbl.CONSECUTIVO != 0)
                                                     select new
                                                     {
                                                         CONSECUTIVO = Tbl.CONSECUTIVO,
                                                         CARGA = objCabecera.NUMERO_CARGA,
                                                         CANTIDAD = Tbl.CANTIDAD,
                                                         MRN = _MRN,
                                                         MSN = _MSN,
                                                         HSN = _HSN,
                                                         P2D_ALTO = Tbl.P2D_ALTO == null ? 0 : Tbl.P2D_ALTO.Value,
                                                         P2D_ANCHO = Tbl.P2D_ANCHO == null ? 0 : Tbl.P2D_ANCHO.Value,
                                                         P2D_LARGO = Tbl.P2D_LARGO == null ? 0 : Tbl.P2D_LARGO.Value,
                                                         PESO = Tbl.PESO == null ? 0 : Tbl.PESO.Value,
                                                         P2D_VOLUMEN = Tbl.P2D_VOLUMEN == null ? 0 : Tbl.P2D_VOLUMEN.Value,
                                                         IMO = string.IsNullOrEmpty(Tbl.IMO) ? "NO APLICA" : Tbl.IMO.Trim(),
                                                         NUMERO_CERTIFICADO = string.IsNullOrEmpty(Tbl.NUMERO_CERTIFICADO) ? "NO APLICA" : Tbl.NUMERO_CERTIFICADO.Trim()
                                                     }).ToList().OrderBy(x => x.CONSECUTIVO);

                                    List<Int64> Lista = new List<Int64>();

                                    foreach (var Det in LinqDetTarja)
                                    {

                                        objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
                                        objPaseCFSTarja.CARGA = Det.CARGA;
                                        objPaseCFSTarja.MRN = Det.MRN;
                                        objPaseCFSTarja.MSN = Det.MSN;
                                        objPaseCFSTarja.HSN = Det.HSN;
                                        objPaseCFSTarja.CONSECUTIVO = Det.CONSECUTIVO;
                                        objPaseCFSTarja.CANTIDAD = Det.CANTIDAD.Value;
                                        objPaseCFSTarja.CIATRANS = string.Empty;
                                        objPaseCFSTarja.CHOFER = string.Empty;
                                        objPaseCFSTarja.ID_CIATRANS = string.Empty;
                                        objPaseCFSTarja.ID_CHOFER = string.Empty;
                                        objPaseCFSTarja.PLACA = string.Empty;
                                        objPaseCFSTarja.VISTO = false;
                                        objPaseCFSTarja.TRANSPORTISTA_DESC = string.Empty;
                                        objPaseCFSTarja.CHOFER_DESC = string.Empty;
                                        objPaseCFSTarja.ESTADO_PAGO = "NO";
                                        objPaseCFSTarja.MARCADO_SUBITEMS = string.Empty;

                                        objPaseCFSTarja.P2D_ALTO = Det.P2D_ALTO;
                                        objPaseCFSTarja.P2D_ANCHO = Det.P2D_ANCHO;
                                        objPaseCFSTarja.P2D_LARGO = Det.P2D_LARGO;
                                        objPaseCFSTarja.PESO = Det.PESO;
                                        objPaseCFSTarja.P2D_VOLUMEN = Det.P2D_VOLUMEN;
                                        objPaseCFSTarja.IMO = Det.IMO;
                                        objPaseCFSTarja.NUMERO_CERTIFICADO = Det.NUMERO_CERTIFICADO;

                                        Lista.Add(Det.CONSECUTIVO.Value);

                                        objCabecera.DetalleSubItem.Add(objPaseCFSTarja);


                                    }


                                }

                               

                                //total de bultos
                                var TotalBultos = objCabecera.Detalle.Sum(x => x.CANTIDAD);
                                objCabecera.TOTAL_BULTOS = TotalBultos;

                               

                                tablePagination_Tarja.DataSource = objCabecera.DetalleSubItem;
                                tablePagination_Tarja.DataBind();

                                this.LabelTotal.InnerText = string.Format("DETALLE DE SUB. ÍTEMS: {0}", objCabecera.TOTAL_BULTOS);

                                Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;

                                this.BtnVer.Attributes.Remove("disabled");
                                this.BtnDescargar.Attributes.Remove("disabled");

                                this.Actualiza_Panele_Detalle();

                                
                            }
                            else
                            {
                              
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

        

   
        protected void BtnVer_Click(object sender, EventArgs e)
        {
        

        }

        protected void BtnDescargar_Click(object sender, EventArgs e)
        {


        }




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