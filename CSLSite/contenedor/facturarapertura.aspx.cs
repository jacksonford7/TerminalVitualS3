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
using System.Text.RegularExpressions;
using CSLSite;
using PasePuerta;

namespace CSLSite
{


    public partial class facturarapertura : System.Web.UI.Page
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
        private Cls_Bil_DetalleClientes objDetalleClientes = new Cls_Bil_DetalleClientes();

        private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        private Cls_Bil_Detalle objDetalle = new Cls_Bil_Detalle();
        private Cls_Bil_DetalleClientes objClientes = new Cls_Bil_DetalleClientes();

        private Cls_Bil_Invoice_Actualiza_Pase objActualiza_Pase = new Cls_Bil_Invoice_Actualiza_Pase();
        private List<Cls_Bil_AsumeFactura> List_Asume { set; get; }
        private List<Cls_Bil_Contenedor_DiasLibres> List_Contenedor { set; get; }
        private List<Cls_Bil_Turnos> List_Turnos { set; get; }



        private List<N4.Importacion.container> ContainersReefer { set; get; }

        private List<Aduana.Importacion.ecu_validacion_cntr_cfs_ff> ContaineFF { set; get; }
        private List<Aduana.Importacion.ecu_validacion_cntr_cfs_ff> GkeyContainerFF { set; get; }

        private Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();
        private Cls_Bil_Log_Appcgsa objLogAppCgsa = new Cls_Bil_Log_Appcgsa();
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
        private int NDiasZarpe = 0;
        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        private bool sap_valida = false;
        private bool tieneBloqueo = false;
        private decimal SaldoPendiente = 0;
        private decimal ValorVencido = 0;
        private decimal ValorPendiente = 0;
        private decimal Cupo = 0;
        private Int64 DiasCredito = 0;
        bool Bloqueo_Cliente = false;
        bool Liberado_Cliente = false;
        private string gkeyBuscado = string.Empty;

        //turnos
        private DateTime FechaInicial;
        private DateTime FechaFinal;
        //private DateTime FechaActualSalida;
        private DateTime FechaMenosHoras;
        private TimeSpan HorasDiferencia;
        private int TotalHoras = 0;

        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
        private bool UnidadDesconectada = false;
        private bool ContenedorReefer = false;
        private static string TextoLeyenda = string.Empty;

        private bool ValidaClientes = false;
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
            UPDETALLECLIENTES.Update();
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
            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
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
                detalle_carga.Tipo = TipoCarga.Contenedores; //opcional
                detalle_carga.Titulo = "Modulo de Facturación Importación"; //opcional
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
                        Mensaje = string.Format("Se ha generado una notificación a nuestra área de Tesorería para la revisión de su caso generado #  {0} ...Para mayor información: Servicio al cliente: ec.sac@contecon.com.ec lunes a domingo 7h00 a 23h00...Tesorería: lunes a viernes 7h00 a 17h30....Teléfonos (04) 6006300 - 3901700", rt.Resultado);

                    }
                    else
                    {
                        Mensaje = string.Format("favor canalizar este mensaje con nuestro personal de facturación para las respectivas revisiones del caso generado #  {0} ...Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.", rt.Resultado);

                    }
                }
                else
                {
                    if (bloqueo)
                    {
                        Mensaje = string.Format("Se ha generado una notificación a nuestra área de Tesorería  para que realicen las respectivas revisiones del problema {0} ...Para mayor información: Servicio al cliente: ec.sac@contecon.com.ec lunes a domingo 7h00 a 23h00...Tesorería: lunes a viernes 7h00 a 17h30....Teléfonos (04) 6006300 - 3901700", rt.MensajeProblema);

                    }
                    else
                    {
                        Mensaje = string.Format("favor canalizar este mensaje con nuestro personal de facturación para las respectivas revisiones del problema {0} ....Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00. ", rt.MensajeProblema);
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

                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.fac@contecon.com.ec'>ec.fac@contecon.com.ec</a> para proceder con el registro....Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", this.hf_idcliente.Value, this.hf_desccliente.Value));

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

                            objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
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

                                this.LabelTotal.InnerText = string.Format("DETALLE DE CONTENEDORES - Total Contenedores: {0}", objCabecera.Detalle.Count());
                                this.Actualiza_Paneles();
                            }
                        }
                        else
                        {
                            this.Crear_Sesion();
                        }
                    }

                    objFactura = new Cls_Bil_Invoice_Cabecera();
                    Session["Invoice" + this.hf_BrowserWindowName.Value] = objFactura;


                

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
                    objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;

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
                    
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        
                    }


                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        string row_estado = DataBinder.Eval(e.Row.DataItem, "REEFER").ToString();
                        string row_conectado = DataBinder.Eval(e.Row.DataItem, "CONECTADO").ToString();
                        string row_estado_ridt = DataBinder.Eval(e.Row.DataItem, "ESTADO_RDIT").ToString();
                        Int64 row_gkey = Int64.Parse(DataBinder.Eval(e.Row.DataItem, "GKEY").ToString());
                        DateTime? row_fechasalida = (DateTime?)DataBinder.Eval(e.Row.DataItem, "FECHA_HASTA");
                        string row_contenedor = DataBinder.Eval(e.Row.DataItem, "CONTENEDOR").ToString();
                        string row_idplan = DataBinder.Eval(e.Row.DataItem, "IDPLAN").ToString();
                        //CheckBox Chk = (CheckBox)e.Row.FindControl("CHKFA");
                        string row_pase = DataBinder.Eval(e.Row.DataItem, "NUMERO_PASE_N4").ToString();
                        string row_inout = DataBinder.Eval(e.Row.DataItem, "IN_OUT").ToString();

                        //certificado
                        string row_tienecertificado = DataBinder.Eval(e.Row.DataItem, "TIENE_CERTIFICADO").ToString();
                        //CheckBox ChkCert = (CheckBox)e.Row.FindControl("CHKCERTIFICADO");
                        //fin

                       


                        if (row_estado.Equals("RF") && row_conectado.Equals("NO CONECTADO"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.PaleVioletRed;
                            //Chk.Enabled = false;

                        }
                        if (!row_estado_ridt.Equals("A"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.PaleVioletRed;
                            //Chk.Enabled = false;

                        }

                        if (row_inout.Equals("OUT"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Peru;
                           // Chk.Enabled = false;

                        }

                        if (!string.IsNullOrEmpty(row_pase))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Green;

                        }

                        //bloquea ceritificado
                        if (row_tienecertificado.Equals("SI"))
                        {
                           // ChkCert.Enabled = false;

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

        #region "Eventos de la grilla de clientes"
        protected void tableClientes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (Response.IsClientConnected)
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

                    if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
                    {
                        TextBox TxtEMail = e.Item.FindControl("TxtEMail") as TextBox;
                        Label ruc = e.Item.FindControl("ruc") as Label;
                        Label nombres = e.Item.FindControl("nombres") as Label;
                        CheckBox chkMarcarFF = e.Item.FindControl("chkMarcarFF") as CheckBox;
                        CheckBox servicio = e.Item.FindControl("servicio") as CheckBox;
                        CheckBox servicio2 = e.Item.FindControl("ChkAppCgsa") as CheckBox;
                        CheckBox inactivo = e.Item.FindControl("inactivo") as CheckBox;

                        ValidaClientes = Convert.ToBoolean(this.hf_validar.Value);

                        if (ValidaClientes)
                        {
                            if (servicio.Checked)
                            {
                                chkMarcarFF.Checked = true;
                                TxtEMail.Attributes["disabled"] = "disabled";
                                chkMarcarFF.Enabled = false;
                            }
                            else
                            {
                                //si esta desactivado bloquea
                                if (inactivo.Checked)
                                {
                                    chkMarcarFF.Checked = false;
                                    TxtEMail.Attributes["disabled"] = "disabled";
                                    chkMarcarFF.Enabled = false;
                                }
                            }

                            objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                            var Detalle = objCabecera.Detalle_Clientes.FirstOrDefault(f => f.ruc.Equals(ruc.Text.Trim()));
                            if (Detalle != null)
                            {
                                Detalle.visto = chkMarcarFF.Checked;
                                Detalle.email = TxtEMail.Text;
                            }


                            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
                        }
                        else
                        {
                            chkMarcarFF.Checked = false;
                            TxtEMail.Attributes["disabled"] = "disabled";
                            chkMarcarFF.Enabled = false;
                        }
                        


                        this.Actualiza_Panele_Detalle();
                    }

                    

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

                }
            }
        }

        protected void chkMarcarFF_CheckedChanged(object sender, EventArgs e)
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

                    CheckBox chkMarcar = (CheckBox)sender;
                    RepeaterItem item = (RepeaterItem)chkMarcar.NamingContainer;
                    Label ruc = (Label)item.FindControl("ruc");
                    TextBox TxtEMail = (TextBox)item.FindControl("TxtEMail");

                    string pruc = ruc.Text;
                    string pemail = (TxtEMail != null ? TxtEMail.Text : "");


                    objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    var Detalle = objCabecera.Detalle_Clientes.FirstOrDefault(f => f.ruc.Equals(pruc.Trim()));
                    if (Detalle != null)
                    {
                        Detalle.visto = chkMarcar.Checked;
                        Detalle.email = pemail;

                    }

                    Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;


                    this.Actualiza_Panele_Detalle();

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }
        }
        #endregion

        #region "Evento Botones"

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    OcultarLoading("2");
                    bool Ocultar_Mensaje = true;
                    UnidadDesconectada = false;

                    


                    this.LabelTotal.InnerText = string.Format("DETALLE DE CONTENEDORES");
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

                    /****************************************************************fin de dias libres****************************************************************/

                    /*****************************************************************valida clientes******************************************************************/
                    List<Cls_Bil_Configuraciones> ListValidaClientes = Cls_Bil_Configuraciones.Get_Validacion("CLIENTE_FF", out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en configuraciones.....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                       
                    }

                    ValidaClientes = false;
                    this.hf_validar.Value = "false";
                    if (ListValidaClientes.Count != 0)
                    {
                        ValidaClientes = true;
                        this.hf_validar.Value = "true";
                    }
                    /*************************************************************fin valida clientes******************************************************************/

                    /*para almacenar clientes que asumen factura*/
                    List_Asume = new List<Cls_Bil_AsumeFactura>();
                    List_Asume.Clear();
                    List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = string.Empty, nombre = string.Empty, mostrar = string.Format("{0}", "Seleccionar...") });

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdAgenteCodigo = string.Empty;
                    var AgenteCod = N4.Entidades.Forwarder.ObtenerForwarderPorRUC(ClsUsuario.loginname, ClsUsuario.ruc);
                    if (AgenteCod.Exitoso)
                    {
                        var ListaAgente = AgenteCod.Resultado;
                        if (ListaAgente != null)
                        {
                            IdAgenteCodigo = ListaAgente.CODIGO_FW;

                        }
                    }
                    else
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i> <b> Error! Freight Forwarder [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 1,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", ClsUsuario.ruc, ClsUsuario.nombres));

                        Ocultar_Mensaje = false;
                        /*************************************************************************************************************************************
                        * crear caso salesforce
                        ***********************************************************************************************************************************/
                        MensajesErrores = string.Format("Se presentaron los siguientes problemas: No exsite información del Freight Forwarder, no registrado: {0}", ClsUsuario.ruc);

                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedores de Apertura", "No existe información del Freight Forwarder, no registrado.", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                            ClsUsuario.nombres, ClsUsuario.ruc, out MensajeCasos, false);

                        /*************************************************************************************************************************************
                        * fin caso salesforce
                        * **********************************************************************************************************************************/

                     
                        return;
                    }

                    var Validacion = new Aduana.Importacion.ecu_validacion_cntr_cfs_ff();
                    var EcuaContenedores = Validacion.CargaPorManifiestoImpo(ClsUsuario.loginname, ClsUsuario.ruc, IdAgenteCodigo, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), true);//ecuapas contenedores
                    if (EcuaContenedores.Exitoso)
                    {
                        //DATOS DEL AGENTE PARA BUSCAR INFORMACION
                        var LinqAgente = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                            select new
                                            {
                                                ID_AGENTE = Tbl.agente_id,
                                                ID_CLIENTE = Tbl.agente_id,
                                                DESC_CLIENTE = string.Empty
                                            }).FirstOrDefault();

                        this.hf_idagente.Value = LinqAgente.ID_AGENTE;
                        this.hf_idcliente.Value = LinqAgente.ID_CLIENTE;
                        this.hf_idasume.Value = LinqAgente.ID_CLIENTE;

                        this.hf_rucagente.Value = string.Empty;
                        this.hf_descagente.Value = string.Empty;
                        this.hf_descasume.Value = string.Empty;
                        this.hf_desccliente.Value = LinqAgente.DESC_CLIENTE;

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

                        this.CboAsumeFactura.DataSource = List_Asume;
                        this.CboAsumeFactura.DataTextField = "mostrar";
                        this.CboAsumeFactura.DataValueField = "ruc";
                        this.CboAsumeFactura.DataBind();

                        if (List_Asume.Count == 2)
                        {
                            this.CboAsumeFactura.SelectedIndex = 1;
                        }
                    
                        //fin asume factura


                        //LISTA RESUMIDA DE CONTENEDORES YA DEPURADA
                        var LinqResumen = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                          select new
                                          {
                                              uuid  = 0,
                                              mrn  = Tbl.mrn,
                                              msn  = Tbl.msn,
                                              hsn = Tbl.hsn,
                                              referencia = Tbl.referencia,
                                              cntr = Tbl.cntr,
                                              gkey = Tbl.gkey,
                                              tipo = Tbl.tipo,
                                              agente_id = Tbl.agente_id,
                                              agente = Tbl.agente,
                                              importador_id = Tbl.importador_id,
                                              importador = Tbl.importador,
                                              declaracion = Tbl.declaracion,
                                              ridt_estado = Tbl.ridt_estado,
                                              imdt_id = Tbl.imdt_id,
                                              total_partida = Tbl.total_partida,
                                              documento_bl = Tbl.documento_bl,
                                              smdt_id = Tbl.smdt_id,
                                              smdt_estado = Tbl.smdt_estado,

                                          }).Distinct().ToList();

                        //LISTA DE DIFERENTES GKEY
                        var LinqGkeyResumen = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                           select new
                                           {
                                               mrn = Tbl.mrn,
                                               msn = Tbl.msn,
                                               hsn = Tbl.hsn,
                                               gkey = Tbl.gkey,
                                           }).Distinct().ToList();

                        ContaineFF = new List<Aduana.Importacion.ecu_validacion_cntr_cfs_ff>();
                        ContaineFF.Clear();

                        GkeyContainerFF = new List<Aduana.Importacion.ecu_validacion_cntr_cfs_ff>();
                        GkeyContainerFF.Clear();

                        foreach (var Det in LinqResumen)
                        {
                            ContaineFF.Add(new Aduana.Importacion.ecu_validacion_cntr_cfs_ff { uuid = Det.uuid, mrn = Det.mrn, msn = Det.msn, hsn = Det.hsn, referencia = Det.referencia, cntr = Det.cntr, gkey = Det.gkey, tipo = Det.tipo,
                                agente_id = Det.agente_id,
                                agente = Det.agente,
                                importador_id = Det.importador_id,
                                importador = Det.importador,
                                declaracion = Det.declaracion,
                                ridt_estado = Det.ridt_estado,
                                imdt_id = Det.imdt_id,
                                total_partida = Det.total_partida,
                                documento_bl = Det.documento_bl,
                                smdt_id = Det.smdt_id,
                                smdt_estado = Det.smdt_estado
                            });
                        }

                        foreach (var Det in LinqGkeyResumen)
                        {
                            GkeyContainerFF.Add(new Aduana.Importacion.ecu_validacion_cntr_cfs_ff
                            {                              
                                mrn = Det.mrn,
                                msn = Det.msn,
                                hsn = Det.hsn,
                                gkey = Det.gkey,
                            });
                        }

                        //clientes a validar si tienen el servicio o no
                        System.Xml.Linq.XDocument XMLClientes = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                 new System.Xml.Linq.XElement("CLIENTES", from p in ContaineFF.AsEnumerable().AsParallel()
                                                                                select new System.Xml.Linq.XElement("CLIENTE",
                                                                                   new System.Xml.Linq.XAttribute("RUC", p.importador_id == null ? "" : p.importador_id.ToString().Trim()),
                                                                                   new System.Xml.Linq.XAttribute("NOMBRES", p.importador == null ? "" : p.importador.ToString().Trim()),
                                                                                   new System.Xml.Linq.XAttribute("flag", "C"))));

                        /*ultima factura*/
                        List<Cls_Bil_DetalleClientes> ListClientes = Cls_Bil_DetalleClientes.List_Clientes(XMLClientes.ToString(), out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener listado de clientes....{0}</b>", cMensajes));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                        //fin de informacion de clientes



                        //INFORMACION DEL CONTENEDOR
                        var Contenedor = new N4.Importacion.container();
                        var Gkey = Aduana.Importacion.ecu_validacion_cntr_cfs_ff.CargaToListString(GkeyContainerFF);//pasa los gkey de las partidas o manifiestos
                        var ListaContenedores = Contenedor.CargaPorKeys(ClsUsuario.loginname, Gkey.Resultado);//resultado de entidad contenedor
                        if (ListaContenedores.Exitoso)
                        {
                            //10/02/2020
                            //almaceno lista de contenedores para validacion Reefer
                            ContainersReefer = ListaContenedores.Resultado;

                            Session["ListaCotenedor" + this.hf_BrowserWindowName.Value] = ContainersReefer;
                            //**fin

                            var LinqPartidas = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                                select new
                                                {
                                                    MRN = Tbl.mrn,
                                                    MSN = Tbl.msn,
                                                    HSN = Tbl.hsn,
                                                    IMDT = (Tbl.imdt_id == null) ? "" : Tbl.imdt_id,
                                                    GKEY = Tbl.gkey,
                                                    CONTENEDOR = (Tbl.cntr == null) ? "" : Tbl.cntr,
                                                    BL = (Tbl.documento_bl == null) ? "" : Tbl.documento_bl,
                                                    DECLARACION = (Tbl.declaracion == null) ? "" : Tbl.declaracion,
                                                    ESTADO_RIDT = (Tbl.ridt_estado==null ? "" : Tbl.ridt_estado),
                                                }).Distinct();

                            /*ultima factura*/
                            List<Cls_Bil_Invoice_Ultima_Factura> ListUltimaFactura = Cls_Bil_Invoice_Ultima_Factura.List_Ultima_Factura(this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" + this.TXTHSN.Text.Trim(), out cMensajes);
                            if (!String.IsNullOrEmpty(cMensajes))
                            {
                              
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener última factura....{0}</b>", cMensajes));
                                this.Actualiza_Panele_Detalle();
                                return;
                            }
                            /*ultima factura en caso de tener*/
                            var LinqUltimaFactura = (from TblFact in ListUltimaFactura.Where(TblFact => TblFact.IV_GKEY != 0)
                                                        select new
                                                        {
                                                            FT_NUMERO_CARGA = TblFact.IV_NUMERO_CARGA,
                                                            FT_FECHA = (TblFact.IV_FECHA==null ? null : TblFact.IV_FECHA),
                                                            FT_FACTURA = TblFact.IV_FACTURA,
                                                            FT_GKEY = TblFact.IV_GKEY,
                                                            FT_FECHA_HASTA = (TblFact.IV_FECHA_HASTA==null? null : TblFact.IV_FECHA_HASTA),
                                                            FT_ID = TblFact.IV_ID,
                                                            FT_MODULO = TblFact.IV_MODULO
                                                        }).Distinct();


                           


                            /*left join de contenedores*/
                            var LinqQuery = (from Tbl in ListaContenedores.Resultado.Where(Tbl => (Tbl.CNTR_CONTAINER == null ? "" : Tbl.CNTR_CONTAINER) != string.Empty)
                                                join EcuaPartidas in LinqPartidas on Tbl.CNTR_CONSECUTIVO equals EcuaPartidas.GKEY into TmpFinal
                                                join Factura in LinqUltimaFactura on Tbl.CNTR_CONSECUTIVO equals Factura.FT_GKEY into TmpFactura
                                                from Final in TmpFinal.DefaultIfEmpty()
                                                from FinalFT in TmpFactura.DefaultIfEmpty()
                                             select new
                                                {
                                                    CONTENEDOR = Tbl.CNTR_CONTAINER,
                                                    REFERENCIA = (Tbl.CNTR_VEPR_REFERENCE == null) ? string.Empty : Tbl.CNTR_VEPR_REFERENCE,
                                                    TRAFICO = (Tbl.CNTR_TYPE == null) ? string.Empty : Tbl.CNTR_TYPE,
                                                    TAMANO = (Tbl.CNTR_TYSZ_SIZE == null) ? string.Empty : Tbl.CNTR_TYSZ_SIZE,
                                                    TIPO = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
                                                    FECHA_CAS = (DateTime?)(Tbl.FECHA_CAS.HasValue ? Tbl.FECHA_CAS : null),
                                                    BLOQUEOS = (Tbl.CNTR_HOLD == 0) ? string.Empty : "SI",
                                                    IN_OUT = (Tbl.CNTR_YARD_STATUS == null) ? string.Empty : Tbl.CNTR_YARD_STATUS,
                                                    TIPO_CONTENEDOR = (Tbl.CNTR_TYSZ_TYPE == null) ? string.Empty : Tbl.CNTR_TYSZ_TYPE,//REEFER
                                                    CONECTADO = ((Tbl.CNTR_TYSZ_TYPE == "RF") ? ((Tbl.CNTR_REEFER_CONT == "N") ? "NO CONECTADO" : "CONECTADO") : string.Empty),
                                                    LINEA = (Tbl.CNTR_CLNT_CUSTOMER_LINE == null) ? string.Empty : Tbl.CNTR_CLNT_CUSTOMER_LINE,
                                                    DOCUMENTO = (Tbl.CNTR_DOCUMENT == null) ? ((Final == null) ? string.Empty : Final.DECLARACION) : Tbl.CNTR_DOCUMENT,
                                                    IMDT = (Final == null) ? string.Empty : Final.IMDT,
                                                    BL = (Final == null) ? string.Empty : Final.BL,
                                                    FULL_VACIO = (Tbl.CNTR_FULL_EMPTY_CODE == null) ? string.Empty : Tbl.CNTR_FULL_EMPTY_CODE,
                                                    GKEY = Tbl.CNTR_CONSECUTIVO,
                                                    AISV = (Tbl.CNTR_AISV == null) ? string.Empty : Tbl.CNTR_AISV,
                                                    DECLARACION = (Final == null) ? string.Empty : Final.DECLARACION,
                                                    BLOQUEADO = (Tbl.CNTR_HOLD == 0) ? false : true,
                                                    FECHA_ULTIMA = (FinalFT == null) ? null : (DateTime?)(FinalFT.FT_FECHA.HasValue ? FinalFT.FT_FECHA : null),
                                                    NUMERO_FACTURA = (FinalFT == null) ? string.Empty : FinalFT.FT_FACTURA,
                                                    ID_FACTURA = (FinalFT == null) ? 0 : FinalFT.FT_ID,
                                                    VIAJE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                                    NAVE = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                                    FECHA_ARRIBO = (Tbl.CNTR_VEPR_ACTUAL_ARRIVAL.HasValue ? Tbl.CNTR_VEPR_ACTUAL_ARRIVAL.Value.ToString("dd/MM/yyyy") : ""),
                                                    CNTR_DD = (Tbl.CNTR_DD == 0) ? false : true,
                                                    FECHA_HASTA = (FinalFT == null) ? null : (DateTime?)(FinalFT.FT_FECHA_HASTA.HasValue ? FinalFT.FT_FECHA_HASTA : null),
                                                    ESTADO_RIDT = (Final.ESTADO_RIDT == null) ? string.Empty : Final.ESTADO_RIDT,
                                                    CNTR_DESCARGA = (Tbl.CNTR_DESCARGA==null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                                    MODULO = (FinalFT == null) ? string.Empty : FinalFT.FT_MODULO,
                                                    CNTR_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
                                                    CERTIFICADO =   false,
                                                    TIENE_CERTIFICADO = "NO",
                                             }).OrderBy(x => x.IN_OUT).ThenBy(x=> x.CONTENEDOR);

                            if (LinqQuery != null && LinqQuery.Count() > 0)
                            {

                                bool cancelado = false;
                                //agrego todos los contenedores a la clase cabecera
                                objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;

                                objCabecera.ID_CLIENTE = this.hf_idcliente.Value;
                                objCabecera.DESC_CLIENTE = this.hf_desccliente.Value;
                                objCabecera.ID_FACTURADO = this.hf_idasume.Value;
                                objCabecera.DESC_FACTURADO = this.hf_descasume.Value;
                                objCabecera.ID_UNICO_AGENTE = this.hf_idagente.Value;
                                objCabecera.ID_AGENTE = this.hf_rucagente.Value;
                                objCabecera.DESC_AGENTE = this.hf_descagente.Value;
                                objCabecera.FECHA = DateTime.Now.Date;
                                objCabecera.TIPO_CARGA = "CONT";
                                objCabecera.NUMERO_CARGA = this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" + this.TXTHSN.Text.Trim();
                                objCabecera.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objCabecera.SESION = this.hf_BrowserWindowName.Value;
                                objCabecera.HORA_HASTA = "00:00";
                                objCabecera.FECHA_HASTA = DateTime.Now.Date;

                                //campo nuevo
                                objCabecera.TOTAL_BULTOS = 0;

                                //detalle de contendores
                                objCabecera.Detalle.Clear();
                                Int16 Secuencia = 1;
                                foreach (var Det in LinqQuery)
                                {
                                    /*datos nuevos para imprimir factura*/
                                    objCabecera.BL = Det.BL;
                                    objCabecera.BUQUE = Det.NAVE;
                                    objCabecera.VIAJE = Det.VIAJE;
                                    objCabecera.FECHA_ARRIBO = Det.FECHA_ARRIBO;


                                    objDetalle = new Cls_Bil_Detalle();
                                    objDetalle.VISTO = true;
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
                                    objDetalle.TIPO = Det.TIPO;
                                    objDetalle.CAS = Det.FECHA_CAS;
                                    objDetalle.BOOKING = "";

                                    objDetalle.IMDT = Det.IMDT;
                                    objDetalle.BLOQUEO = Det.BLOQUEADO;
                                    // objDetalle.FECHA_ULTIMA = Det.FECHA_ULTIMA;
                                    objDetalle.FECHA_ULTIMA = Det.FECHA_HASTA;
                                    objDetalle.IN_OUT = Det.IN_OUT;
                                    objDetalle.FULL_VACIO = Det.FULL_VACIO;
                                    objDetalle.AISV = Det.AISV;
                                    objDetalle.REEFER = Det.TIPO_CONTENEDOR;
                                    objDetalle.IV_USUARIO_CREA = ClsUsuario.loginname.Trim();
                                    objDetalle.IV_FECHA_CREA = DateTime.Now;
                                    objDetalle.NUMERO_FACTURA = Det.NUMERO_FACTURA;
                                    objDetalle.CNTR_DD = Det.CNTR_DD;
                                    objDetalle.FECHA_HASTA = System.DateTime.Now.Date;
                                    objDetalle.ESTADO_RDIT = Det.ESTADO_RIDT.Trim();
                                    objDetalle.CNTR_DESCARGA = Det.CNTR_DESCARGA;
                                    objDetalle.MODULO = Det.MODULO;
                                    objDetalle.CNTR_DEPARTED = Det.CNTR_DEPARTED;
                                    objDetalle.LINEA = Det.LINEA;

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

                                    if (Det.TIPO_CONTENEDOR.Trim().Equals("RF") && Det.CONECTADO.Equals("NO CONECTADO"))
                                    {
                                        UnidadDesconectada = true;
                                    }

                                    objDetalle.IDPLAN = "0";
                                    objDetalle.TURNO = "* Seleccione *";

                                   

                                    //nuevos campos
                                    objDetalle.CANTIDAD = 0;
                                    objDetalle.PESO = 0;
                                    objDetalle.OPERACION = string.Empty;
                                    objDetalle.DESCRIPCION = string.Empty;
                                    objDetalle.EXPORTADOR = string.Empty;
                                    objDetalle.AGENCIA = string.Empty;

                                    objDetalle.CERTIFICADO = Det.CERTIFICADO;
                                    objDetalle.TIENE_CERTIFICADO = Det.TIENE_CERTIFICADO;

                                    objCabecera.Detalle.Add(objDetalle);
                                    Secuencia++;
                                }

                                //detalle de clientes
                                objCabecera.Detalle_Clientes.Clear();
                                foreach (var Det in ListClientes)
                                {
                                    objClientes = new Cls_Bil_DetalleClientes();
                                    objClientes.ruc = Det.ruc;
                                    objClientes.nombres = Det.nombres;
                                    objClientes.email = Det.email;
                                    objClientes.servicio = (ValidaClientes ? Det.servicio : false);
                                    objClientes.visto = (ValidaClientes ? Det.visto : false);
                                    objClientes.IV_USUARIO_CREA = ClsUsuario.loginname.Trim();
                                    objClientes.IV_FECHA_CREA = DateTime.Now;
                                    objClientes.inactivo = Det.inactivo;
                                    objCabecera.Detalle_Clientes.Add(objClientes);
                                }

                                tablePagination.DataSource = objCabecera.Detalle;
                                tablePagination.DataBind();

                                tableClientes.DataSource = objCabecera.Detalle_Clientes;
                                tableClientes.DataBind();

                                this.LabelTotal.InnerText = string.Format("DETALLE DE CONTENEDORES - Total Contenedores: {0}", objCabecera.Detalle.Count());
                                Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
                                this.Actualiza_Panele_Detalle();

                                if (cancelado)
                                {

                                    /*************************************************************************************************************************************
                                    * crear caso salesforce
                                    ***********************************************************************************************************************************/
                                    MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", "según sistema ecuapass la Respuesta de Aprobación de Salida (Importación) -  RIDT no se encuentra aprobado");

                                    this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "Unidad sin RIDT Aprobado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}",this.TXTMRN.Text.Trim(),this.TXTMSN.Text.Trim(),this.TXTHSN.Text.Trim()),
                                        objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos, false);

                                    /*************************************************************************************************************************************
                                    * fin caso salesforce
                                    * **********************************************************************************************************************************/

                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Estimado cliente, lamentamos mucho no poder atender su requerimiento, según sistema ecuapass la Respuesta de Aprobación de Salida (Importación) -  RIDT no se encuentra aprobado. Ponerse en contacto con el Senae.</b>"));
                                    return;
                                }
                               

                            }
                            else
                            {
                                tablePagination.DataSource = null;
                                tablePagination.DataBind();

                                tableClientes.DataSource = null;
                                tableClientes.DataBind();

                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información pendiente de facturar con el número de la carga ingresada..</b>"));
                                this.Actualiza_Panele_Detalle();
                                return;
                            }
                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();

                            tableClientes.DataSource = null;
                            tableClientes.DataBind();

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información pendiente de facturar con el número de la carga ingresada..{0}</b>", ListaContenedores.MensajeProblema));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información pendiente de facturar con el número de la carga ingresada..{0}</b>", EcuaContenedores.MensajeProblema));
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

                Response.Redirect("~/contenedor/facturarapertura.aspx", false);



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
            if (Response.IsClientConnected)
            {



                CultureInfo enUS = new CultureInfo("en-US");
                this.myModal.Attributes["class"] = "nover";

                try
                {


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

                   
                    //instancia sesion
                    objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    if (objCabecera == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la cotización</b>"));
                        return;
                    }
                    if (objCabecera.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de contenedores, para poder generar la cotización</b>"));
                        return;
                    }

                    bool Valida_Todos = true;
                    //verificar si tiene una factura efectuada, para en base a esto proceder a validar de forma parcial o total
                    foreach (var Det in objCabecera.Detalle)
                    {
                        //si tiene fecha de ultima factura
                        if (Det.FECHA_ULTIMA.HasValue)
                        {
                            Valida_Todos = false;
                            break;
                        }
                    }

                    //si no tiene fecha de ultima factura, se procede a validar todos los contenedores
                    if (Valida_Todos)
                    {
                        //valida que seleccione todos los contenedores para cotizar 
                        var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == false)
                                                    select p.CONTENEDOR).ToList();

                        if (LinqValidaContenedor.Count != 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar todos los contenedores a cotizar</b>"));
                            return;
                        }
                        //valida que tenga todos tengan fecha de salida
                        foreach (var Det in objCabecera.Detalle)
                        {
                            if (!Det.FECHA_HASTA.HasValue)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida de salida del contenedor {0}</b>", Det.CONTENEDOR));
                                return;
                            }
                           
                        }

                    }
                    else
                    {
                        //valida que seleccione un contenedor contenedores para cotizar 
                        var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                                    select p.CONTENEDOR).ToList();

                        if (LinqValidaContenedor.Count == 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los contenedores a cotizar</b>"));
                            return;
                        }

                        //valida que tenga todos tengan fecha de salida
                        foreach (var Det in objCabecera.Detalle.Where(x => x.VISTO == true))
                        {
                            if (!Det.FECHA_HASTA.HasValue)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida de salida del contenedor {0}</b>", Det.CONTENEDOR));
                                return;
                            }
                            
                        }
                    }



                    //listado de contenedores
                    var LinqListContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                              select p.CONTENEDOR).ToList();

                    objCabecera.GLOSA = "";
                    Contenedores = string.Join(", ", LinqListContenedor);
                    objCabecera.HORA_HASTA = HoraHasta;

                    //numero de carga
                    Numero_Carga = objCabecera.NUMERO_CARGA;
                    objCabecera.CONTENEDORES = Contenedores;
                    objCabecera.FECHA_HASTA = System.DateTime.Now.Date;
                    LoginName = objCabecera.IV_USUARIO_CREA.Trim();
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
                    *datos del cliente N4 
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
                        //this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No existe información del Cliente del ruc: {0}</b>", objCabecera.ID_FACTURADO));
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.fac@contecon.com.ec'>ec.fac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", objCabecera.ID_FACTURADO, objCabecera.DESC_FACTURADO));

                        return;

                    }
                    /***********************************************************************************************************************************************
                    *fin: datos del cliente N4 
                    **********************************************************************************************************************************************/

                    //actualizo el objeto temporal
                    Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;

                    Fila = 1;
                    Decimal Subtotal = 0;
                    Decimal Iva = 0;
                    Decimal Total = 0;
                    /***********************************************************************************************************************************************
                    *2) proceso para grabar proforma
                    **********************************************************************************************************************************************/
                    objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    /*agrego datos a la proforma*/
                    objProforma = new Cls_Bil_Proforma_Cabecera();
                    objProforma.Detalle.Clear();
                    objProforma.DetalleServicios.Clear();
                    /*cabecera de proforma*/
                    objProforma.PF_ID = objCabecera.ID;
                    objProforma.PF_GLOSA = objCabecera.GLOSA;
                    objProforma.PF_FECHA = objCabecera.FECHA;
                    objProforma.PF_TIPO_CARGA = objCabecera.TIPO_CARGA;
                    objProforma.PF_ID_AGENTE = objCabecera.ID_AGENTE;
                    objProforma.PF_DESC_AGENTE = objCabecera.DESC_AGENTE;
                    objProforma.PF_ID_CLIENTE = objCabecera.ID_CLIENTE;
                    objProforma.PF_DESC_CLIENTE = objCabecera.DESC_CLIENTE;
                    objProforma.PF_ID_FACTURADO = objCabecera.ID_FACTURADO;
                    objProforma.PF_DESC_FACTURADO = objCabecera.DESC_FACTURADO;
                    objProforma.PF_SUBTOTAL = objCabecera.SUBTOTAL;
                    objProforma.PF_IVA = objCabecera.IVA;
                    objProforma.PF_TOTAL = objCabecera.TOTAL;
                    objProforma.IV_USUARIO_CREA = objCabecera.IV_USUARIO_CREA;
                    objProforma.IV_FECHA_CREA = DateTime.Now;
                    objProforma.PF_NUMERO_CARGA = objCabecera.NUMERO_CARGA;
                    objProforma.PF_CONTENEDORES = objCabecera.CONTENEDORES;
                    objProforma.PF_FECHA_HASTA = objCabecera.FECHA_HASTA;
                    objProforma.PF_CODIGO_AGENTE = objCabecera.ID_UNICO_AGENTE;
                    objProforma.PF_SESION = objCabecera.SESION;
                    objProforma.PF_HORA_HASTA = objCabecera.HORA_HASTA;

                    //CAMPO NUEVO
                    objProforma.PF_TOTAL_BULTOS = objCabecera.TOTAL_BULTOS;

                    //string cip = Cls_Bil_IP.GetLocalIPAddress();
                    string cip = Request.UserHostAddress;
                    objProforma.PF_IP = cip;
                    /*agrego detalle de contenedores a proforma*/
                    var LinqDetalle = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                       select p).ToList();

                    foreach (var Det in LinqDetalle)
                    {
                        objDetalleProforma = new Cls_Bil_Proforma_Detalle();
                        objDetalleProforma.PF_VISTO = Det.VISTO;
                        objDetalleProforma.PF_ID = 0;
                        objDetalleProforma.PF_GKEY = Det.GKEY;
                        objDetalleProforma.PF_MRN = Det.MRN;
                        objDetalleProforma.PF_MSN = Det.MSN;
                        objDetalleProforma.PF_HSN = Det.HSN;
                        objDetalleProforma.PF_CONTENEDOR = Det.CONTENEDOR;
                        objDetalleProforma.PF_TRAFICO = Det.TRAFICO;
                        objDetalleProforma.PF_DOCUMENTO = Det.DOCUMENTO;
                        objDetalleProforma.PF_DES_BLOQUEO = Det.DES_BLOQUEO;
                        objDetalleProforma.PF_CONECTADO = Det.CONECTADO;
                        objDetalleProforma.PF_REFERENCIA = Det.REFERENCIA;
                        objDetalleProforma.PF_FECHA_HASTA = Det.FECHA_HASTA;
                        objDetalleProforma.PF_TAMANO = Det.TAMANO;
                        objDetalleProforma.PF_TIPO = Det.TIPO;
                        objDetalleProforma.PF_CAS = Det.CAS;
                        objDetalleProforma.PF_BOOKING = Det.BOOKING;
                        objDetalleProforma.PF_IMDT = Det.IMDT;
                        objDetalleProforma.PF_BLOQUEO = Det.BLOQUEO;
                        objDetalleProforma.PF_FECHA_ULTIMA = Det.FECHA_ULTIMA;
                        objDetalleProforma.PF_IN_OUT = Det.IN_OUT;
                        objDetalleProforma.PF_FULL_VACIO = Det.FULL_VACIO;
                        objDetalleProforma.PF_AISV = Det.AISV;
                        objDetalleProforma.PF_REEFER = Det.REEFER;
                        objDetalleProforma.IV_USUARIO_CREA = Det.IV_USUARIO_CREA;
                        objDetalleProforma.IV_FECHA_CREA = DateTime.Now;

                        //CAMPOS NUEVOS
                        objDetalleProforma.PF_CANTIDAD = Det.CANTIDAD;
                        objDetalleProforma.PF_PESO = Det.PESO;
                        objDetalleProforma.PF_OPERACION = Det.OPERACION;
                        objDetalleProforma.PF_DESCRIPCION = Det.DESCRIPCION;
                        objDetalleProforma.PF_EXPORTADOR = Det.EXPORTADOR;
                        objDetalleProforma.PF_AGENCIA = Det.AGENCIA;

                        objProforma.Detalle.Add(objDetalleProforma);

                    }


                    //saco los grupo de fechas para recorrer y sacar servicios por cada fecha
                    var LinqFechasContainer = (from p in objCabecera.Detalle.Where(x => x.VISTO == true).AsEnumerable()
                                               group p by new { FECHA_HASTA = (p.FECHA_HASTA == null ? p.FECHA_ULTIMA : p.FECHA_HASTA) } into Grupo
                                               select new
                                               {
                                                   FECHA_HASTA = Grupo.Key.FECHA_HASTA

                                               }).ToList();


                    //3) si existen fechas por contenedor agrupados
                    if (LinqFechasContainer.Count != 0)
                    {

                        //recorro fechas para hacer un query relacionado a la fecha de cada grupo
                        foreach (var FechaContainer in LinqFechasContainer)
                        {
                            var FechaContainer_Filtro = FechaContainer.FECHA_HASTA.Value;

                            var LinqDetcontainer = (from p in objCabecera.Detalle.Where(x => x.VISTO == true && x.FECHA_HASTA.Value == FechaContainer_Filtro)
                                                    select p.CONTENEDOR).ToList();

                            Contenedores = string.Join(",", LinqDetcontainer);//listado contenedores

                            /***********************************************************************************************************************************************
                            *4) Consulta Servicios a proformar N4 - por cada grupo de fechas
                            **********************************************************************************************************************************************/
                            var Validacion = new Aduana.Importacion.ecu_validacion_cntr();
                            var Contenedor = new N4.Importacion.container();
                            var Billing = new N4Ws.Entidad.billing();
                            var Ws = new N4Ws.Entidad.InvoiceRequest();

                            /*saco el invoice type*/
                            string pInvoiceType = string.Empty;
                            var InvoiceType = InvoiceTypeConfig.ObtenerInvoicetypes();
                            if (InvoiceType.Exitoso)
                            {
                                var LinqInvoiceType = (from p in InvoiceType.Resultado.Where(X => X.codigo.Equals("IMPODESCON"))
                                                       select new { valor = p.valor }).FirstOrDefault();

                                pInvoiceType = LinqInvoiceType.valor == null ? "2DA_MAN_DESCONSOLIDACIONES" : LinqInvoiceType.valor;
                            }
                            /*fin invoice type*/


                            Ws.action = N4Ws.Entidad.Action.INQUIRE;
                            Ws.requester = N4Ws.Entidad.Requester.QUERY_CHARGES;
                            //Ws.InvoiceTypeId = "2DA_MAN_IMPO_CNTRS";
                            Ws.InvoiceTypeId = pInvoiceType;
                            Ws.payeeCustomerId = Cliente_Ruc;
                            Ws.payeeCustomerBizRole = Cliente_Rol;

                            var Direccion = new N4Ws.Entidad.address();
                            Direccion.addressLine1 = string.Empty;
                            Direccion.city = "GUAYAQUIL";

                            var Parametro = new N4Ws.Entidad.invoiceParameter();
                            Parametro.EquipmentId = Contenedores;
                            Parametro.PaidThruDay = FechaContainer_Filtro.ToString("yyyy-MM-dd HH:mm");

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
                                        objServicios = new Cls_Bil_Proforma_Servicios();
                                        objServicios.PF_ID = 0;
                                        objServicios.PF_LINEA = Fila;
                                        objServicios.PF_ID_SERVICIO = Det.CODIGO;
                                        objServicios.PF_DESC_SERVICIO = Det.SERVICIO;
                                        objServicios.PF_CARGA = Det.CARGA;
                                        objServicios.PF_FECHA = DateTime.Parse(Det.FECHA.ToString());
                                        objServicios.PF_TIPO_SERVICIO = TipoServicio;
                                        objServicios.PF_CANTIDAD = Det.CANTIDAD;
                                        objServicios.PF_PRECIO = Det.PRECIO;
                                        objServicios.PF_SUBTOTAL = Det.TOTAL;
                                        objServicios.PF_IVA = Det.IVA;
                                        objServicios.IV_USUARIO_CREA = LoginName;
                                        objServicios.IV_FECHA_CREA = DateTime.Now;
                                        Fila++;
                                        objProforma.DetalleServicios.Add(objServicios);

                                    }

                                    Iva = Iva + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTaxes != null ? xBilling.response.billInvoice.totalTaxes : "0", enUS), 2);
                                    Total = Total + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTotal != null ? xBilling.response.billInvoice.totalTotal : "0", enUS), 2);

                                }//fin ok

                            }//fin resultado


                            /***********************************************************************************************************************************************
                            *fin: Consulta Servicios a proformar N4 
                            **********************************************************************************************************************************************/

                        }//fin recorrido de fechas
                    }
                    else
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen contenedores marcados para cotizar</b>"));
                        return;
                    }//fin existe grupo de fechas

                    //valida si existen servicios a cotizar
                    var LinqSubtotal = (from Servicios in objProforma.DetalleServicios.AsEnumerable()
                                        select Servicios.PF_SUBTOTAL
                                                   ).Sum();

                    Subtotal = LinqSubtotal;
                    objProforma.PF_SUBTOTAL = Subtotal;
                    objProforma.PF_IVA = Iva;
                    objProforma.PF_TOTAL = Total;

                    //actualiza sesion
                    objCabecera.SUBTOTAL = Subtotal;
                    objCabecera.IVA = Iva;
                    objCabecera.TOTAL = Total;

                    Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;

                    /***********************************************************************************************************************************************
                    *graba cotizacion en base de billion, para pasara la siguiente ventana
                    **********************************************************************************************************************************************/

                    if (objProforma == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No existe objeto para generar la proforma</b>"));
                        return;
                    }
                    else
                    {
                        //si no existen servicios a cotizar
                        if (objProforma.DetalleServicios.Count == 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen servicios pendientes para cotizar</b>"));
                            return;
                        }

                        var nIdRegistro = objProforma.SaveTransaction(out cMensajes);
                        if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo grabar datos de la proforma..{0}</b>", cMensajes));
                            return;
                        }

                        this.Ocultar_Mensaje();
                        string cId = securetext(nIdRegistro.Value.ToString());
                        Response.Redirect("~/contenedor/proformaapertura.aspx?id_proforma=" + cId.Trim() + "", false);

                    }

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

                }

            }
        }

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

                CultureInfo enUS = new CultureInfo("en-US");
                NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
                this.myModal.Attributes["class"] = "nover";
                ContenedorReefer = false;

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


                    /*****************************************************************valida clientes******************************************************************/
                    List<Cls_Bil_Configuraciones> ListValidaClientes = Cls_Bil_Configuraciones.Get_Validacion("CLIENTE_FF", out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en configuraciones.....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;

                    }

                    ValidaClientes = false;
                    this.hf_validar.Value = "false";
                    if (ListValidaClientes.Count != 0)
                    {
                        ValidaClientes = true;
                        this.hf_validar.Value = "true";
                    }
                    /*************************************************************fin valida clientes******************************************************************/



                    //instancia sesion
                    objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    if (objCabecera == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la factura </b>"));
                        return;
                    }
                    if (objCabecera.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de contenedores, para poder generar la factura </b>"));
                        return;
                    }


                    bool Valida_Todos = true;
                    //verificar si tiene una factura efectuada, para en base a esto proceder a validar de forma parcial o total
                    foreach (var Det in objCabecera.Detalle)
                    {
                        //si tiene fecha de ultima factura
                        if (Det.FECHA_ULTIMA.HasValue)
                        {
                            Valida_Todos = false;
                            break;
                        }
                    }
                    //si no tiene fecha de ultima factura, se procede a validar todos los contenedores
                    if (Valida_Todos)
                    {
                        //valida que seleccione todos los contenedores para cotizar 
                        var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == false)
                                                    select p.CONTENEDOR).ToList();

                        if (LinqValidaContenedor.Count != 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar todos los contenedores a Facturar </b>"));
                            return;
                        }
                        //valida que tenga todos tengan fecha de salida
                        foreach (var Det in objCabecera.Detalle)
                        {
                            if (!Det.FECHA_HASTA.HasValue)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida de salida del contenedor {0}</b>", Det.CONTENEDOR));
                                return;
                            }
                        }
                    }
                    else
                    {
                        //valida que seleccione un contenedor contenedores para cotizar 
                        var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                                    select p.CONTENEDOR).ToList();

                        if (LinqValidaContenedor.Count == 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los contenedores a Facturar</b>"));
                            return;
                        }
                        //valida que tenga todos tengan fecha de salida
                        foreach (var Det in objCabecera.Detalle.Where(x => x.VISTO == true))
                        {
                            if (!Det.FECHA_HASTA.HasValue)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida de salida del contenedor {0}</b>", Det.CONTENEDOR));
                                return;
                            }
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
                    *validacion de clientes
                    ***********************************************************************************************************************************************/
                    //valida que tenga clientes 
                    var LinqValidaClientes = (from p in objCabecera.Detalle_Clientes.Where(x => !string.IsNullOrEmpty(x.ruc))
                                                select p.ruc).ToList();

                    if (LinqValidaClientes.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen detalle de clientes...</b>"));
                        return;
                    }

                    //actualiza el mail a cada cliente
                    foreach (RepeaterItem xitem in tableClientes.Items)
                    {
                        Label ruc = xitem.FindControl("ruc") as Label;
                        TextBox TxtEMail = xitem.FindControl("TxtEMail") as TextBox;

                        string pruc = ruc.Text;
                        string pemail = (TxtEMail != null ? TxtEMail.Text : "");

                        var Detalle = objCabecera.Detalle_Clientes.FirstOrDefault(f => f.ruc.Equals(pruc.Trim()));
                        if (Detalle != null)
                        {
                            Detalle.email = pemail;
                        }

                    }


                    //recorro clientes para verificar que tengan correos
                    foreach (var Det in objCabecera.Detalle_Clientes.Where(p => p.visto == true && !p.servicio))
                    {
                        if (string.IsNullOrEmpty(Det.email) && ValidaClientes)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! debe ingresar una dirección de correo para el cliente: {0}-{1}</b>",Det.ruc, Det.nombres));
                            return;
                        }
                    }

                   
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
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", objCabecera.ID_FACTURADO, objCabecera.DESC_FACTURADO));

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

                                this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "Cliente Bloqueado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(),true),
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

                                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "Cliente Bloqueado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
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

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "Cliente Bloqueado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
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

                    /***********************************************************************************************************************************************
                    *validacion de eventos duplicados N4 - genera caso a SalesForces
                    ***********************************************************************************************************************************************/
                    Dictionary<Int64, string> Lista_Gkeys = new Dictionary<Int64, string>();
                    foreach (var Det in objCabecera.Detalle.Where(x => x.VISTO == true))
                    {
                      
                        Lista_Gkeys.Add(Det.GKEY, Det.CONTENEDOR);
                    }

                    var ServiciosDuplicados = N4.Importacion.container.ValidarEventos("IMPODESCON", Lista_Gkeys, ClsUsuario.loginname.Trim());
                    if (ServiciosDuplicados != null)
                    {
                        if (!ServiciosDuplicados.Exitoso)
                        {

                            gkeyBuscado = string.Empty;
                            gkeyBuscado = ServiciosDuplicados.MensajeProblema;

                            string CuerpoMensaje = string.Format("Se presentaron los siguientes problemas: {0}", gkeyBuscado);

                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", gkeyBuscado);

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "No pudo facturar debido a validación de servicios duplicados en el sistema", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos,false);

                            /*************************************************************************************************************************************
                            * fin caso salesforce
                            * **********************************************************************************************************************************/

                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Estimado cliente {0}, se presento un problema el cual no le permitirá avanzar con la facturación de los contenedores, {1} </b>", objCabecera.DESC_AGENTE, MensajeCasos));
                            return;

                        }
                    }

                    /***********************************************************************************************************************************************
                    *fin validacion de eventos duplicados N4
                    ***********************************************************************************************************************************************/
                    FechaFactura = System.DateTime.Now.Date;

                    objCabecera.GLOSA = "";
                    Contenedores = string.Join(", ", LinqListContenedor);
                    //numero de carga
                    Numero_Carga = objCabecera.NUMERO_CARGA;
                    objCabecera.CONTENEDORES = Contenedores;
                    objCabecera.FECHA_HASTA = FechaFactura;
                    LoginName = objCabecera.IV_USUARIO_CREA.Trim();
                    objCabecera.HORA_HASTA = HoraHasta;
                    //actualizo el objeto
                    Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;


                    Fila = 1;
                    Decimal Subtotal = 0;
                    Decimal Iva = 0;
                    Decimal Total = 0;
                    /***********************************************************************************************************************************************
                    *2) proceso para grabar factura
                    **********************************************************************************************************************************************/
                    /*************************************************************************************************************************************/
                    /*proceso para almacenar datos*/
                    objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    /*agrego datos a la factura*/
                    objFactura = Session["Invoice" + this.hf_BrowserWindowName.Value] as Cls_Bil_Invoice_Cabecera;
                    objFactura = new Cls_Bil_Invoice_Cabecera();
                    objFactura.Detalle.Clear();
                    objFactura.DetalleServicios.Clear();
                    objFactura.Detalle_Clientes.Clear();

                    /*cabecera de factura*/
                    objFactura.IV_ID = objCabecera.ID;
                    objFactura.IV_GLOSA = objCabecera.GLOSA;
                    objFactura.IV_FECHA = objCabecera.FECHA;
                    objFactura.IV_TIPO_CARGA = objCabecera.TIPO_CARGA;
                    objFactura.IV_ID_AGENTE = objCabecera.ID_AGENTE;
                    objFactura.IV_DESC_AGENTE = objCabecera.DESC_AGENTE;
                    //objFactura.IV_ID_CLIENTE = objCabecera.ID_CLIENTE;
                    objFactura.IV_ID_CLIENTE = objCabecera.ID_AGENTE;
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
                    //string cip = Cls_Bil_IP.GetLocalIPAddress();
                    string cip = Request.UserHostAddress;
                    objFactura.IV_IP = cip;
                    //campo nuevo
                    objFactura.IV_TOTAL_BULTOS = objCabecera.TOTAL_BULTOS;


                    /*agrego detalle de contenedores a proforma*/
                    var LinqDetalle = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                       select p).ToList();

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

                        //CARBONO NEUTRO
                        objDetalleFactura.IV_CERTIFICADO = Det.CERTIFICADO;
                        objDetalleFactura.IV_TIENE_CERTIFICADO = string.IsNullOrEmpty(Det.TIENE_CERTIFICADO) ? "NO" : Det.TIENE_CERTIFICADO;

                        objFactura.Detalle.Add(objDetalleFactura);

                    }

                    /* detalle de clientes*/
                    foreach (var Det in objCabecera.Detalle_Clientes)
                    {
                        objDetalleClientes = new Cls_Bil_DetalleClientes();
                        objDetalleClientes.ruc = Det.ruc;
                        objDetalleClientes.nombres = Det.nombres;
                        objDetalleClientes.email = Det.email;
                        objDetalleClientes.servicio = Det.servicio;
                        objDetalleClientes.visto = Det.visto;
                        objDetalleClientes.IV_USUARIO_CREA = Det.IV_USUARIO_CREA;
                        objDetalleClientes.IV_FECHA_CREA = DateTime.Now;
                        objFactura.Detalle_Clientes.Add(objDetalleClientes);

                    }
                    



                    /**********************************************************************************************************************************
                    *saco los grupo de fechas para recorrer y sacar servicios por cada fecha
                    **********************************************************************************************************************************/
                    var LinqFechasContainer = (from p in objCabecera.Detalle.Where(x => x.VISTO == true).AsEnumerable()
                                               group p by new { FECHA_HASTA = (p.FECHA_HASTA == null ? p.FECHA_ULTIMA : p.FECHA_HASTA) } into Grupo
                                               select new
                                               {
                                                   FECHA_HASTA = Grupo.Key.FECHA_HASTA

                                               }).ToList();

                    //3) si existen fechas por contenedor agrupados
                    if (LinqFechasContainer.Count != 0)
                    {

                        //recorro fechas para hacer un query relacionado a la fecha de cada grupo
                        foreach (var FechaContainer in LinqFechasContainer)
                        {
                            var FechaContainer_Filtro = FechaContainer.FECHA_HASTA.Value;

                            var LinqDetcontainer = (from p in objCabecera.Detalle.Where(x => x.VISTO == true && x.FECHA_HASTA.Value == FechaContainer_Filtro)
                                                    select p.CONTENEDOR).ToList();

                            Contenedores = string.Join(",", LinqDetcontainer);//listado contenedores

                            /***********************************************************************************************************************************************
                            *4) Consulta de Servicios a facturar N4 - por cada grupo de fechas
                            **********************************************************************************************************************************************/
                            var Validacion = new Aduana.Importacion.ecu_validacion_cntr();
                            var Contenedor = new N4.Importacion.container();
                            var Billing = new N4Ws.Entidad.billing();
                            var Ws = new N4Ws.Entidad.InvoiceRequest();

                            /*saco el invoice type*/
                            string pInvoiceType = string.Empty;
                            var InvoiceType = InvoiceTypeConfig.ObtenerInvoicetypes();
                            if (InvoiceType.Exitoso)
                            {
                                var LinqInvoiceType = (from p in InvoiceType.Resultado.Where(X => X.codigo.Equals("IMPODESCON"))
                                                       select new { valor = p.valor }).FirstOrDefault();

                                pInvoiceType = LinqInvoiceType.valor == null ? "2DA_MAN_DESCONSOLIDACIONES" : LinqInvoiceType.valor;
                            }
                            /*fin invoice type*/

                            Ws.action = N4Ws.Entidad.Action.INQUIRE;
                            Ws.requester = N4Ws.Entidad.Requester.QUERY_CHARGES;
                            //Ws.InvoiceTypeId = "2DA_MAN_IMPO_CNTRS";
                            Ws.InvoiceTypeId = pInvoiceType;
                            Ws.payeeCustomerId = Cliente_Ruc;
                            Ws.payeeCustomerBizRole = Cliente_Rol;

                            var Direccion = new N4Ws.Entidad.address();
                            Direccion.addressLine1 = string.Empty;
                            Direccion.city = "GUAYAQUIL";

                            var Parametro = new N4Ws.Entidad.invoiceParameter();
                            Parametro.EquipmentId = Contenedores;
                            Parametro.PaidThruDay = FechaContainer_Filtro.ToString("yyyy-MM-dd HH:mm");

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

                                        //11/02/2020
                                        //actualizar contenedores que tiene servicios para facturar.
                                        var ExisteConenedor = objFactura.Detalle.FirstOrDefault(f => f.IV_CONTENEDOR.Equals(Det.CARGA.Trim()));
                                        if (ExisteConenedor != null)
                                        {
                                            ExisteConenedor.IV_TIENE_SERVICIOS = true;
                                        }

                                    }

                                    Iva = Iva + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTaxes != null ? xBilling.response.billInvoice.totalTaxes : "0", enUS), 2);
                                    Total = Total + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTotal != null ? xBilling.response.billInvoice.totalTotal : "0", enUS), 2);

                                }//FIN OK 

                            }//FIN RESULTADO

                            /***********************************************************************************************************************************************
                            *fin: Consulta de Servicios a facturar N4 
                            **********************************************************************************************************************************************/


                        }//FIN DE RECORRIDO POR FECHAS
                    }
                    else
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen contenedores marcados para Facturar</b>"));
                        return;
                    }//fin existe grupo de fechas


                    /***********************************************************************************************************************************************
                    *validaciones de sap/clientes de credito
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
                    Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
                    Session["Invoice" + this.hf_BrowserWindowName.Value] = objFactura;

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
                        /***********************************************************************************************************************************************
                        *validacion de dias libres
                        **********************************************************************************************************************************************/
                        //si no existen servicios a facturar
                        if (objFactura.DetalleServicios.Count == 0)
                        {

                            //30-01-2020
                            //recorre todos los contenedores seleccionados para verificar fecha y que no tengan servicios
                            cMensajes = string.Empty;
                            string cMensajeActualizados = string.Empty;
                           
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen servicios pendientes para facturar.</br> {0} ", cMensajeActualizados));
                            return;

                        }

                        //17-01-2022
                        /*verificar si tiene servicio de appcgsa*/
                        List<Cls_Bil_Configuraciones> ValidaFF = Cls_Bil_Configuraciones.Get_Validacion("REGISTRA_FF", out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en configuraciones.</br> {0} ", cMensajes));
                            return;

                        }
                        else
                        {
                            bool Valida_SNA2 = false;
                            if (ValidaFF.Count != 0)
                            {
                                Valida_SNA2 = true;
                            }
                            if (Valida_SNA2)
                            {
                                var usuario_sna = System.Configuration.ConfigurationManager.AppSettings["usuario_sna"];
                                var clave_sna = System.Configuration.ConfigurationManager.AppSettings["clave_sna"];
                                string Estado_Servicio = "0";
                                string Problema_Servicio = string.Empty;
                                string email = "";
                                string celular = "";

                                //consulta si tiene el servicio
                                string Estado_Servicio2 = "0";
                                //xml a consultar
                                string XMLCna2 = string.Format("<existe><ruc>{0}</ruc></existe>", objFactura.IV_ID_CLIENTE);


                                var WSCNA = new SNA.CRMService();
                                var Resultado2 = WSCNA.Invocar(usuario_sna.ToString(), clave_sna.ToString(), XMLCna2.ToString());

                                if (Resultado2 != null)
                                {
                                    string Res = Resultado2.ToString();
                                    var XMLResult = new XDocument();
                                    try
                                    {
                                        XMLResult = XDocument.Parse(Res);
                                    }
                                    catch (Exception ex)
                                    {

                                        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar), "BtnFacturar_Click", false, null, null, ex.StackTrace, ex);
                                        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                                        return;
                                    }

                                    XElement XElemResult = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("resultado")).FirstOrDefault();
                                    if (XElemResult != null)
                                    {
                                        XElement estado;
                                        estado = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("estado")).FirstOrDefault();
                                        Estado_Servicio2 = estado.Value;
                                    }

                                  
                                }

                                if (Estado_Servicio2.Equals("0"))
                                {
                                    //graba log del servicio
                                    objLogAppCgsa.USUARIO = objFactura.IV_USUARIO_CREA;
                                    objLogAppCgsa.ID_AGENTE = objFactura.IV_ID_AGENTE;
                                    objLogAppCgsa.AGENTE = objFactura.IV_DESC_AGENTE;
                                    objLogAppCgsa.ID_IMPORTADOR = objFactura.IV_ID_CLIENTE;
                                    objLogAppCgsa.IMPORTADOR = objFactura.IV_DESC_CLIENTE;
                                    objLogAppCgsa.NUMERO_CARGA = objFactura.IV_NUMERO_CARGA;
                                    objLogAppCgsa.SERVICIO = true;

                                    string xerror;
                                    var nProcesoApp = objLogAppCgsa.SaveTransaction(out xerror);
                                    /*fin de nuevo proceso de grabado*/
                                    if (!nProcesoApp.HasValue || nProcesoApp.Value <= 0)
                                    {

                                    }


                                    //xml para grabar
                                    string XMLCna = string.Format("<cliente><ruc>{0}</ruc><nombre>{1}</nombre><activar>{2}</activar><categoria>{3}</categoria><telefono>{4}</telefono><email>{5}</email><parametros/></cliente>",
                                    objFactura.IV_ID_CLIENTE, objFactura.IV_DESC_CLIENTE.Replace("&", " "), 1, "IMPO", "99999", objFactura.IV_EMAIL_CLIENTE);

                                    var WSCNA2 = new SNA.CRMService();
                                    var ResultadoApp = WSCNA2.Invocar(usuario_sna.ToString(), clave_sna.ToString(), XMLCna.ToString());

                                    if (ResultadoApp != null)
                                    {
                                        string Res = ResultadoApp.ToString();
                                        var XMLResult = new XDocument();
                                        try
                                        {
                                            XMLResult = XDocument.Parse(Res);
                                        }
                                        catch (Exception ex)
                                        {
                                            
                                            lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "BtnFacturar_Click", false, null, null, ex.StackTrace, ex);
                                            OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                                            return;
                                        }

                                        XElement XElemResult = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("resultado")).FirstOrDefault();
                                        if (XElemResult != null)
                                        {
                                            XElement estado;
                                            estado = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("estado")).FirstOrDefault();
                                            Estado_Servicio = estado.Value;

                                            XElement problema;
                                            problema = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("problema")).FirstOrDefault();
                                            Problema_Servicio = problema?.Value;

                                            //levanta popup si no tiene el servicio
                                            if (Estado_Servicio.Equals("1"))
                                            {
                                                
                                                this.Mostrar_Mensaje(2, string.Format("<b> Felicidades!! Usted se ha suscrito al Sistema de Trazabilidad de Carga CGSApp </b>"));

                                                //sp enviar correo
                                                objMail.Ruc = objFactura.IV_ID_CLIENTE;
                                                objMail.Email = objFactura.IV_EMAIL_CLIENTE;
                                                string error;
                                                var nProceso = objMail.SaveTransactionMail(out error);
                                                /*fin de nuevo proceso de grabado*/
                                                if (!nProceso.HasValue || nProceso.Value <= 0)
                                                {

                                                }

                                            }
                                            else
                                            {
                                                mpedit.Hide();
                                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal con la suscripción del servicio de Trazabilidad de Carga  CGSApp ....{0}", Problema_Servicio));
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        this.Ocultar_Mensaje();
                        string cId = securetext(this.hf_BrowserWindowName.Value);
                        Response.Redirect("~/contenedor/visualizafacturaapertura.aspx?id=" + cId.Trim() + "", false);

                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "Facturar", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    //this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

                }

            }

        }

        //acepta el servicio
        protected void BtnProcesar_Click(object sender, EventArgs e)
        {

            try
            {

                //if (string.IsNullOrEmpty(this.TxtEmail.Text))
                //{
                //    alertcelular.InnerText = string.Empty;
                //    alertcelular.Visible = false;

                //    this.BtnProcesar.Attributes.Remove("disabled");
                //    alertmail.Visible = true;
                //    alertmail.InnerHtml = "<b>Error!</b> >Ingrese un email";
                //    this.TxtEmail.Focus();
                //    msgload.Update();
                //    return;
                //}
                //else
                //{
                //    //if (!valida_email(this.TxtEmail.Text.Trim()))
                //    //{
                //    //    alertcelular.InnerText = string.Empty;
                //    //    alertcelular.Visible = false;

                //    //    this.BtnProcesar.Attributes.Remove("disabled");

                //    //    alertmail.Visible = true;
                //    //    alertmail.InnerHtml = "<b>Error!</b> >El mail no es valido";
                //    //    this.TxtEmail.Focus();
                //    //    msgload.Update();
                //    //    return;
                //    //}
                //    //else
                //    //{
                //    //    alertmail.InnerText = string.Empty;
                //    //    alertmail.Visible = false;
                //    //    msgload.Update();
                //    //}

                //}

                //if (string.IsNullOrEmpty(this.TxtCelular.Text))
                //{
                //    this.BtnProcesar.Attributes.Remove("disabled");
                //    alertcelular.InnerHtml = "<b>Error!</b> >Ingrese # Celular";
                //    alertcelular.Visible = true;
                //    this.TxtCelular.Focus();
                //    msgload.Update();
                //    return;
                //}
                //else
                //{
                //    if (this.TxtCelular.Text.Trim().Length < 10)
                //    {
                //        this.BtnProcesar.Attributes.Remove("disabled");
                //        alertcelular.InnerHtml = "<b>Error!</b> ># Celular incompleto";
                //        alertcelular.Visible = true;
                //        this.TxtCelular.Focus();
                //        msgload.Update();
                //        return;
                //    }
                //    else
                //    {

                //    }
                //    alertcelular.InnerText = string.Empty;
                //    alertcelular.Visible = false;
                //    msgload.Update();
                //}


                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var usuario_sna = System.Configuration.ConfigurationManager.AppSettings["usuario_sna"];
                var clave_sna = System.Configuration.ConfigurationManager.AppSettings["clave_sna"];
                string Estado_Servicio = "0";
                string Problema_Servicio = string.Empty;
                string email = "";
                string celular = "";

                //xml para grabar
                string XMLCna = string.Format("<cliente><ruc>{0}</ruc><nombre>{1}</nombre><activar>{2}</activar><categoria>{3}</categoria><telefono>{4}</telefono><email>{5}</email><parametros/></cliente>",
                    ClsUsuario.ruc, string.Format("{0} {1}", ClsUsuario.nombres.Replace("&", " "), ClsUsuario.apellidos.Replace("&", " ")), 1, "IMPO", celular,email);

                var WSCNA = new SNA.CRMService();
                var Resultado = WSCNA.Invocar(usuario_sna.ToString(), clave_sna.ToString(), XMLCna.ToString());

                if (Resultado != null)
                {
                    string Res = Resultado.ToString();
                    var XMLResult = new XDocument();
                    try
                    {
                        XMLResult = XDocument.Parse(Res);
                    }
                    catch (Exception ex)
                    {
                        mpedit.Hide();
                        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnProcesar_Click), "BtnProcesar_Click", false, null, null, ex.StackTrace, ex);
                        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                        this.Mostrar_Mensaje(4,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                        return;
                    }

                    XElement XElemResult = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("resultado")).FirstOrDefault();
                    if (XElemResult != null)
                    {
                        XElement estado;
                        estado = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("estado")).FirstOrDefault();
                        Estado_Servicio = estado.Value;

                        XElement problema;
                        problema = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("problema")).FirstOrDefault();
                        Problema_Servicio = problema?.Value;

                        //levanta popup si no tiene el servicio
                        if (Estado_Servicio.Equals("1"))
                        {
                            mpedit.Hide();
                            this.panel_notificacion.Visible = false;
                            this.Mostrar_Mensaje(4,string.Format("<b> Felicidades!! Usted se ha suscrito al Sistema de Trazabilidad de Carga CGSApp </b>"));
                        }
                        else
                        {
                            mpedit.Hide();
                            this.Mostrar_Mensaje(4,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal con la suscripción del servicio de Trazabilidad de Carga  CGSApp ....{0}", Problema_Servicio));
                            return;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                mpedit.Hide();
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnProcesar_Click), "contenedoromportacion.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(4,string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }



        }

        protected void BtnInformacion_Click(object sender, EventArgs e)
        {
            mpedit.Show();
            this.BtnProcesar.Focus();
            this.Actualiza_Paneles();
        }

        protected void BtnCerrarInformacion_Click(object sender, EventArgs e)
        {
            this.panel_notificacion.Visible = false;
            //this.UPCARGA.Update();
           this. Actualiza_Paneles();
        }

        #endregion

      

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

    









        }
}