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
using PasePuerta;
using System.Data;
using Salesforces;
using CSLSite;

namespace CSLSite
{
   

    public partial class contenedorimportacionadicional : System.Web.UI.Page
    {


        #region "Clases"

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
        private List<Cls_Bil_Container_Gkey> List_Gkey { set; get; }
        private List<Cls_Bil_Turnos> List_Turnos { set; get; }

        private List<N4.Importacion.container> ContainersReefer { set; get; }

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
        private Int64 DiasCredito = 0;
        bool Bloqueo_Cliente = false;
        bool Liberado_Cliente = false;
        private string gkeyBuscado = string.Empty;
        private decimal Cupo = 0;

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

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
            UPCARGA.Update();
            UPDATOSCLIENTE.Update();
            UPFECHA.Update();
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
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('"+valor+"');", true);
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
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCabecera = new Cls_Bil_Cabecera();
            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
        }

        private void Enviar_Caso_Salesforce(string pUsuario, string pruc, string pModulo, string pNovedad, string pErrores, string pValor1, string pValor2, string pValor3, out string Mensaje)
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

                if (!string.IsNullOrEmpty(pValor1)) { detalle_carga.Detalles.Add(new DetalleCarga("BL", pValor1)); }
                if (!string.IsNullOrEmpty(pValor2)) { detalle_carga.Detalles.Add(new DetalleCarga("Cliente", pValor2)); }
                if (!string.IsNullOrEmpty(pValor3)) { detalle_carga.Detalles.Add(new DetalleCarga("Agente", pValor3)); }

                //asi puedes poner los campos que desees o se necesiten sobre la carga

                tk.Contenido = detalle_carga.ToString();

                var rt = tk.NuevoCaso();
                if (rt.Exitoso)
                {
                    Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del caso generado #  {0}...Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 16h30 sábados y domingos 8h00 a 15h00. ", rt.Resultado);
                }
                else
                {
                    Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del problema {0}....Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 16h30 sábados y domingos 8h00 a 15h00. ", rt.MensajeProblema);
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

        #region "Ventos Page"

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

                                if (numero_carga.Split('+').ToList().Count > 3)
                                {
                                    this.TXTCLIENTE.Text = numero_carga.Split('+').ToList()[5].Trim();
                                    this.TXTASUMEFACTURA.Text = numero_carga.Split('+').ToList()[3].Trim();
                                    this.TXTAGENCIA.Text = numero_carga.Split('+').ToList()[4].Trim();

                                    this.hf_idagente.Value = this.TXTAGENCIA.Text.Trim();
                                    this.hf_idcliente.Value = this.TXTCLIENTE.Text.Trim();
                                    this.hf_idasume.Value = this.TXTASUMEFACTURA.Text.Trim();



                                    var Agente = N4.Entidades.Agente.ObtenerAgente(ClsUsuario.loginname, this.TXTAGENCIA.Text);
                                    if (Agente.Exitoso)
                                    {
                                        var ListaAgente = Agente.Resultado;
                                        if (ListaAgente != null)
                                        {
                                            this.TXTAGENCIA.Text = string.Format("{0} - {1}", ListaAgente.ruc.Trim(), ListaAgente.nombres.Trim());
                                            //agrega agente si no es importador, lo agrega
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

                                    var Cliente = N4.Entidades.Cliente.ObtenerCliente(ClsUsuario.loginname, this.TXTCLIENTE.Text);
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
                                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.fac@contecon.com.ec'>ec.fac@contecon.com.ec</a> para proceder con el registro. {0}, ,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00. </b>", string.Empty));

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

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! {0} </b>", ex.Message));
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
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen datos de la sesión actual para recuperar...puede volver a generar la consulta </b>"));
                                return;
                            }
                            else
                            {
                                CultureInfo enUS = new CultureInfo("en-US");
                                this.TxtFechaHasta.Text = objCabecera.FECHA_HASTA.Value.ToString("MM-dd-yyyy");
                                
                                this.Txtcomentario.Text = objCabecera.GLOSA;
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
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! {0} </b>", ex.Message));
            }
        }


        #endregion
      
        #region "Eventos de la grilla"

        protected void CHKFA_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CultureInfo enUS = new CultureInfo("en-US");

                GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                CheckBox CHKFA = (CheckBox)row.FindControl("CHKFA");

                String CONTENEDOR = tablePagination.DataKeys[row.RowIndex].Value.ToString();

                objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                var Detalle = objCabecera.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(CONTENEDOR.Trim()));
                if (Detalle != null)
                {
                    if (CHKFA.Checked)
                    {
                        //fecha hasta - para sacar los servicios

                        //if (string.IsNullOrEmpty(this.TxtHora.Text))
                        //{
                        //    HoraHasta = "00:00";
                        //}
                        //else
                        //{
                        //    HoraHasta = this.TxtHora.Text.Trim();
                        //}

                        //if (Detalle.REEFER.Trim().Equals("RF") && HoraHasta.Trim().Equals("00:00"))
                        //{
                        //    Detalle.VISTO = false;
                        //    Detalle.FECHA_HASTA = null;
                        //    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una hora para el contenedor Reefer {0}</b>", Detalle.CONTENEDOR));
                        //    this.TxtHora.Focus();
                        //}
                        //else
                        //{
                            Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                            if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                            {
                                Detalle.VISTO = false;
                                Detalle.FECHA_HASTA = null;
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la cotización o factura</b>"));
                                this.TxtFechaHasta.Focus();
                            }
                            else
                            {
                                if (FechaFactura.Date < System.DateTime.Today.Date)
                                {
                                    Detalle.VISTO = false;
                                    Detalle.FECHA_HASTA = null;
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha igual o superior a la fecha actual</b>"));
                                    this.TxtFechaHasta.Focus();

                                }
                                else
                                {
                                    Detalle.VISTO = CHKFA.Checked;
                                    Detalle.FECHA_HASTA = FechaFactura;
                                    this.Ocultar_Mensaje();

                                }

                            }
                        //}

                    }
                    else
                    {
                        Detalle.VISTO = false;
                        Detalle.FECHA_HASTA = null;
                        Detalle.TURNO = string.Empty;
                        Detalle.IDPLAN = "0";
                    }


                }


                tablePagination.DataSource = objCabecera.Detalle;
                tablePagination.DataBind();

                Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;


                this.Actualiza_Panele_Detalle();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }
        }

        protected void CboTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            DropDownList CboTurno = (DropDownList)row.FindControl("CboTurno");
            String row_contenedor = tablePagination.DataKeys[row.RowIndex].Value.ToString();

            objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
            var Detalle = objCabecera.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(row_contenedor.Trim()));
            if (Detalle != null)
            {
                Detalle.IDPLAN = CboTurno.SelectedValue.ToString().Trim();
                Detalle.TURNO = CboTurno.SelectedItem.Text.ToString().Trim();
            }


            tablePagination.DataSource = objCabecera.Detalle;
            tablePagination.DataBind();

            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;

            this.Actualiza_Panele_Detalle();
        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void tablePagination_PreRender(object sender, EventArgs e)
        {
            try
            {
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
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", ex.Message));

            }

        }

        protected void tablePagination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

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
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", ex.Message));

            }
        }

        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            /* if (e.Row.RowType == DataControlRowType.DataRow)
             {
                 string estado = DataBinder.Eval(e.Row.DataItem, "REEFER").ToString();
                 string conectado = DataBinder.Eval(e.Row.DataItem, "CONECTADO").ToString();
                 CheckBox Chk = (CheckBox)e.Row.FindControl("CHKFA");

                 if (estado.Equals("RF") && conectado.Equals("NO CONECTADO"))
                 {
                     e.Row.ForeColor = System.Drawing.Color.PaleVioletRed;
                     Chk.Enabled = false;

                 }              
             }*/

            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string row_estado = DataBinder.Eval(e.Row.DataItem, "REEFER").ToString();
                    string row_conectado = DataBinder.Eval(e.Row.DataItem, "CONECTADO").ToString();
                    string row_estado_ridt = DataBinder.Eval(e.Row.DataItem, "ESTADO_RDIT").ToString();
                    Int64 row_gkey = Int64.Parse(DataBinder.Eval(e.Row.DataItem, "GKEY").ToString());
                    DateTime? row_fechasalida = (DateTime?)DataBinder.Eval(e.Row.DataItem, "FECHA_HASTA");
                    string row_contenedor = DataBinder.Eval(e.Row.DataItem, "CONTENEDOR").ToString();
                    string row_idplan = DataBinder.Eval(e.Row.DataItem, "IDPLAN").ToString();
                    CheckBox Chk = (CheckBox)e.Row.FindControl("CHKFA");
                    string row_inout = DataBinder.Eval(e.Row.DataItem, "IN_OUT").ToString();
                    string row_pase = DataBinder.Eval(e.Row.DataItem, "NUMERO_PASE_N4").ToString();


                    //LLENADO DE COMBO DEL GRID
                    DataTable dt = new DataTable();
                    DropDownList CboTurno = (DropDownList)e.Row.FindControl("CboTurno");
                    CboTurno.Items.Clear();
                    dt.Columns.Add(new DataColumn("IDPLAN", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("TURNO", Type.GetType("System.String")));


                    if (row_estado.Equals("RF") && row_conectado.Equals("NO CONECTADO"))
                    {
                        e.Row.ForeColor = System.Drawing.Color.PaleVioletRed;
                        Chk.Enabled = false;

                    }
                    if (!row_estado_ridt.Equals("A"))
                    {
                        e.Row.ForeColor = System.Drawing.Color.PaleVioletRed;
                        Chk.Enabled = false;

                    }

                    if (row_inout.Equals("OUT"))
                    {
                        e.Row.ForeColor = System.Drawing.Color.Peru;
                        Chk.Enabled = false;

                    }

                    if (!string.IsNullOrEmpty(row_pase))
                    {
                        e.Row.ForeColor = System.Drawing.Color.Green;

                    }

                    dt.Rows.Add(new String[] { "0", "* Seleccione *" });
                    dt.AcceptChanges();

                    if (row_fechasalida.HasValue)
                    {
                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        var Turnos = PasePuerta.TurnoVBS.ObtenerTurnos(ClsUsuario.ruc, row_contenedor, row_gkey, row_fechasalida.Value);
                        if (Turnos.Exitoso)
                        {
                            //si es contenedor reefer
                            if (row_estado.Trim() == "RF")
                            {
                                //si es el mismo dia de la fecha tope de facturacion
                                FechaInicial = row_fechasalida.Value.Date;
                                FechaMenosHoras = row_fechasalida.Value.AddHours(-3);
                                HoraHasta = FechaMenosHoras.ToString("HH:mm");

                                FechaFinal = FechaMenosHoras;
                                HorasDiferencia = FechaFinal.Subtract(FechaInicial);
                                TotalHoras = HorasDiferencia.Hours;
                                var Horas = HorasDiferencia.TotalHours;

                                if (Horas >= 24)
                                {
                                    var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                                     select new
                                                     {
                                                         IdPlan = string.Format("{0}-{1}-{2}-{3}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm")),
                                                         Turno = Tbl.Turno
                                                     }).ToList().OrderBy(x => x.Turno);

                                    foreach (var Items in LinqQuery)
                                    {
                                        dt.Rows.Add(new String[] { Items.IdPlan, Items.Turno });
                                        dt.AcceptChanges();
                                    }
                                }
                                else
                                {
                                    if (Horas < 0)
                                    {
                                    }
                                    else
                                    {
                                        var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno) && (String.Compare(Tbl.Turno, HoraHasta) <= 0))
                                                         select new
                                                         {
                                                             IdPlan = string.Format("{0}-{1}-{2}-{3}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm")),
                                                             Turno = Tbl.Turno
                                                         }).ToList().OrderBy(x => x.Turno);

                                        foreach (var Items in LinqQuery)
                                        {
                                            dt.Rows.Add(new String[] { Items.IdPlan, Items.Turno });
                                            dt.AcceptChanges();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                                 select new
                                                 {
                                                     IdPlan = string.Format("{0}-{1}-{2}-{3}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm")),
                                                     Turno = Tbl.Turno
                                                 }).ToList().OrderBy(x => x.Turno);

                                foreach (var Items in LinqQuery)
                                {
                                    dt.Rows.Add(new String[] { Items.IdPlan, Items.Turno });
                                    dt.AcceptChanges();
                                }
                            }

                        }
                        else
                        {
                            row_idplan = "0";
                        }

                    }
                    else
                    {
                        row_idplan = "0";
                    }
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    ds.AcceptChanges();
                    CboTurno.DataSource = ds;
                    CboTurno.SelectedValue = row_idplan;
                    CboTurno.DataBind();

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
                    bool Ocultar_Mensaje = true;
                    OcultarLoading("2");
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
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga MRN </b>"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga MSN </b>"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga HSN </b>"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    tablePagination.DataSource = null;
                    tablePagination.DataBind();


                    /*saco los dias libre como parametros generales*/
                    List<Cls_Bil_Configuraciones> Parametros = Cls_Bil_Configuraciones.Parametros(out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en procesos de días libres.....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                    var LinqDiasLibres = (from Dias in Parametros.Where(Dias => Dias.NOMBRE.Equals("DIASLIBRES"))
                                          select new
                                          {
                                              VALOR = Dias.VALOR == null ? string.Empty : Dias.VALOR
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

                    var Validacion = new Aduana.Importacion.ecu_validacion_cntr();
                    var EcuaContenedores = Validacion.CargaPorManifiestoImpo(ClsUsuario.loginname, ClsUsuario.ruc, IdAgenteCodigo, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), true);//ecuapas contenedores
                    if (EcuaContenedores.Exitoso)
                    {
                        //DATOS DEL AGENTE PARA BUSCAR INFORMACION
                        var LinqAgente = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                            select new
                                            {
                                                ID_AGENTE = Tbl.agente_id,
                                                ID_CLIENTE = Tbl.importador_id,
                                                DESC_CLIENTE = (Tbl.importador == null ? string.Empty : Tbl.importador)
                                            }).FirstOrDefault();

                        this.hf_idagente.Value = LinqAgente.ID_AGENTE;
                        this.hf_idcliente.Value = LinqAgente.ID_CLIENTE;
                        this.hf_idasume.Value = LinqAgente.ID_CLIENTE;

                        this.hf_rucagente.Value = string.Empty;
                        this.hf_descagente.Value = string.Empty;
                        this.hf_descasume.Value = string.Empty;
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
                                //agrega importador
                                if (!this.hf_idcliente.Value.Trim().Equals(ClsUsuario.ruc.Trim()))
                                {
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
                                this.TXTASUMEFACTURA.Text = ListaCliente.CLNT_CUSTOMER.Trim() + " - " + ListaCliente.CLNT_NAME.Trim();
                                this.hf_desccliente.Value = ListaCliente.CLNT_NAME.Trim();
                                this.hf_descasume.Value = ListaCliente.CLNT_NAME.Trim();
                                //asume factura
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
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro.,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", this.hf_idcliente.Value, this.hf_desccliente.Value));
                            Ocultar_Mensaje = false;


                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("Se presentaron los siguientes problemas: No exsite información del cliente, no registrado: {0}", this.hf_idcliente.Value);

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "No exsite información del cliente, no registrado.", 
                                MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                this.hf_desccliente.Value, this.hf_rucagente.Value, out MensajeCasos);

                            /*************************************************************************************************************************************
                            * fin caso salesforce
                            * **********************************************************************************************************************************/

                           
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
                            else
                            {
                                //this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>No se pudo cargar los datos de las personas que asumirán las facturas, {0}", Resultado.MensajeProblema));
                                //return;
                            }
                        }

                        this.CboAsumeFactura.DataSource = List_Asume;
                        this.CboAsumeFactura.DataTextField = "mostrar";
                        this.CboAsumeFactura.DataValueField = "ruc";
                        this.CboAsumeFactura.DataBind();
                        //fin asume factura


                        //INFORMACION DEL CONTENEDOR
                        var Contenedor = new N4.Importacion.container();
                        var Gkey = Aduana.Importacion.ecu_validacion_cntr.CargaToListString(EcuaContenedores.Resultado);//pasa los gkey de las partidas o manifiestos
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
                                                    ESTADO_RIDT = (Tbl.ridt_estado == null ? "" : Tbl.ridt_estado),
                                                }).Distinct();

                            /*ultima factura*/
                            List<Cls_Bil_Invoice_Ultima_Factura> ListUltimaFactura = Cls_Bil_Invoice_Ultima_Factura.List_Ultima_Factura(this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" + this.TXTHSN.Text.Trim(), out cMensajes);
                            if (!String.IsNullOrEmpty(cMensajes))
                            {
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.....{0}</b>", cMensajes));
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




                            /*contenedores con pases expirados y con nuevos eventos para facturar*/
                            List_Gkey = new List<Cls_Bil_Container_Gkey>();
                            List_Gkey.Clear();
                            var EventosContenedores = PasePuerta.Pase_Container.ObtenerListaEditable(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), ClsUsuario.ruc, ClsUsuario.loginname,true,null);
                            if (EventosContenedores.Exitoso)
                            {
                                var LinqQueryEventos = (from p in EventosContenedores.Resultado.AsEnumerable()
                                                where !string.IsNullOrEmpty(p.Field<string>("CONTENEDOR"))
                                                select new
                                                {
                                                    
                                                    MRN = p.Field<string>("MRN") == null ? "" : p.Field<string>("MRN").Trim(),
                                                    MSN = p.Field<string>("MSN") == null ? "" : p.Field<string>("MSN").Trim(),
                                                    HSN = p.Field<string>("HSN") == null ? "" : p.Field<string>("HSN").Trim(),
                                                    CONTENEDOR = p.Field<string>("CONTENEDOR") == null ? "" : p.Field<string>("CONTENEDOR").Trim(),
                                                    ID_CARGA = p.Field<Decimal>("ID_CARGA") 
                                                }).Distinct();

                                foreach (var Det in LinqQueryEventos)
                                {
                                    List_Gkey.Add(new Cls_Bil_Container_Gkey { Gkey = Int64.Parse(Det.ID_CARGA.ToString()) });
                                }
                            }

                            var LinqNewEventos = (from Tbl in List_Gkey.ToList()
                                                  select new { GKEY = Tbl.Gkey });


                            /*fin contenedores con eventos*/

                            /*pase puerta*/
                            List<Cls_Bil_Invoice_Pase_Puerta> ListPasePuerta = Cls_Bil_Invoice_Pase_Puerta.List_Pase_Puerta(this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" + this.TXTHSN.Text.Trim(), out cMensajes);
                            if (!String.IsNullOrEmpty(cMensajes))
                            {
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.....{0}</b>", cMensajes));
                                this.Actualiza_Panele_Detalle();
                                return;
                            }
                            /*ultima factura en caso de tener*/
                            var LinqListPasePuerta = (from TblPase in ListPasePuerta.Where(TblPase => TblPase.GKEY != 0)
                                                      select new
                                                      {
                                                          ID_PPWEB = TblPase.ID_PPWEB,
                                                          ID_PASE = TblPase.ID_PASE,
                                                          GKEY = TblPase.GKEY,
                                                          CONTENEDOR = TblPase.CONTENEDOR,
                                                          NUMERO_PASE_N4 = TblPase.NUMERO_PASE_N4,
                                                      }).Distinct();


                            /*left join de contenedores*/
                            var LinqQuery = (from Tbl in ListaContenedores.Resultado.Where(Tbl => (Tbl.CNTR_CONTAINER == null ? "" : Tbl.CNTR_CONTAINER) != string.Empty)
                                                join Eventos in LinqNewEventos on Tbl.CNTR_CONSECUTIVO equals Eventos.GKEY
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
                                                    CNTR_DESCARGA = (Tbl.CNTR_DESCARGA == null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                                    MODULO = (FinalFT == null) ? string.Empty : FinalFT.FT_MODULO,
                                                    CNTR_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
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
                                objCabecera.FECHA = DateTime.Now;
                                objCabecera.TIPO_CARGA = "CONT";
                                objCabecera.NUMERO_CARGA = this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" + this.TXTHSN.Text.Trim();
                                objCabecera.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objCabecera.SESION = this.hf_BrowserWindowName.Value;
                                objCabecera.HORA_HASTA = "00:00";
                                //campo nuevo
                                objCabecera.TOTAL_BULTOS = 0;


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
                                    objDetalle.VISTO = false;
                                    objDetalle.ID = 0;
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
                                    objDetalle.FECHA_HASTA = Det.FECHA_HASTA;
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

                                    var Pase = LinqListPasePuerta.FirstOrDefault(f => f.GKEY.Equals(objDetalle.GKEY) && f.CONTENEDOR.Equals(objDetalle.CONTENEDOR));
                                    if (Pase != null)
                                    {
                                        objDetalle.NUMERO_PASE_N4 = Pase.NUMERO_PASE_N4;
                                    }

                                    //nuevos campos
                                    objDetalle.CANTIDAD = 0;
                                    objDetalle.PESO = 0;
                                    objDetalle.OPERACION = string.Empty;
                                    objDetalle.DESCRIPCION = string.Empty;
                                    objDetalle.EXPORTADOR = string.Empty;
                                    objDetalle.AGENCIA = string.Empty;

                                    objCabecera.Detalle.Add(objDetalle);
                                    Secuencia++;
                                }

                                tablePagination.DataSource = objCabecera.Detalle;
                                tablePagination.DataBind();
                                this.LabelTotal.InnerText = string.Format("DETALLE DE CONTENEDORES - Total Contenedores: {0}", objCabecera.Detalle.Count());
                                Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
                                this.Actualiza_Panele_Detalle();

                                if (cancelado)
                                {
                                    /*************************************************************************************************************************************
                                   * crear caso salesforce
                                   ***********************************************************************************************************************************/
                                    MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", "según sistema ecuapass la Respuesta de Aprobación de Salida (Importación) -  RIDT no se encuentra aprobado");

                                    this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "Unidad sin RIDT Aprobado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                        objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos);

                                    /*************************************************************************************************************************************
                                    * fin caso salesforce
                                    * **********************************************************************************************************************************/

                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Estimado cliente, lamentamos mucho no poder atender su requerimiento, según sistema ecuapass la Respuesta de Aprobación de Salida (Importación) -  RIDT no se encuentra aprobado. Ponerse en contacto con el Senae.</b>"));
                                    return;
                                }
                                else
                                {
                                    //si las unidades no estan conectadas, se envia caso a salesforce
                                    if (UnidadDesconectada)
                                    {
                                        /*************************************************************************************************************************************
                                        * crear caso salesforce
                                        ***********************************************************************************************************************************/
                                        MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", "Existen unidades Reefer sin conectar");

                                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "Unidades Reefer Desconectadas", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                            objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos);

                                        Ocultar_Mensaje = false;

                                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Estimado cliente, lamentamos mucho no poder atender su requerimiento, {0} </b>", MensajeCasos));

                                    }
                                }

                            }
                            else
                            {
                                tablePagination.DataSource = null;
                                tablePagination.DataBind();

                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información pendiente de facturar con el número de la carga ingresada..{0}</b>", ListaContenedores.MensajeProblema));
                                this.Actualiza_Panele_Detalle();
                                return;
                            }
                        }
                        else
                        {
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

                Response.Redirect("~/contenedor/contenedorimportacionadicional.aspx", false);



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.myModal.Attributes["class"] = "nover";
            this.Actualiza_Paneles();

        }

        protected void BtnCotizar_Click(object sender, EventArgs e)
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

                //if (string.IsNullOrEmpty(this.TxtHora.Text))
                //{
                //    HoraHasta = "00:00";
                //}
                //else
                //{
                //    HoraHasta = this.TxtHora.Text.Trim();
                //}

                //fecha hasta para sacar los servicios
                //Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                Fecha = string.Format("{0}", this.TxtFechaHasta.Text.Trim());
                if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la cotización</b>"));
                    this.TxtFechaHasta.Focus();
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
                //valida que seleccione todos los contenedores para cotizar 
                var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                            select p.CONTENEDOR).ToList();

                if (LinqValidaContenedor.Count == 0)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los contenedores a cotizar</b>"));
                    return;
                }

                //valida que tenga fecha de salida
                foreach (var Det in objCabecera.Detalle.Where(x => x.VISTO == true))
                {
                    if (!Det.FECHA_HASTA.HasValue)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida de salida del contenedor {0}</b>", Det.CONTENEDOR));
                        return;
                    }
                }

                //listado de contenedores
                var LinqListContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                        select p.CONTENEDOR).ToList();

                objCabecera.GLOSA = this.Txtcomentario.Text.Trim();
                Contenedores = string.Join(", ", LinqListContenedor);
                objCabecera.HORA_HASTA = HoraHasta;

                //numero de carga
                Numero_Carga = objCabecera.NUMERO_CARGA;
                objCabecera.CONTENEDORES = Contenedores;
                objCabecera.FECHA_HASTA = FechaFactura;
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
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.fac@contecon.com.ec'>ec.fac@contecon.com.ec</a> para proceder con el registro.,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", objCabecera.ID_FACTURADO, objCabecera.DESC_FACTURADO));


                    return;

                }
                /***********************************************************************************************************************************************
                *fin: datos del cliente N4 
                **********************************************************************************************************************************************/

                /***********************************************************************************************************************************************
                *1) inicio:reefer
                **********************************************************************************************************************************************/
                //saco los grupo de fechas
                var LinqFechasRF = (from p in objCabecera.Detalle.Where(x => x.VISTO == true && x.REEFER.Equals("RF")).AsEnumerable()
                                   group p by new { FECHA_HASTA = (p.FECHA_HASTA ==null ? p.FECHA_ULTIMA : p.FECHA_HASTA)} into Grupo
                                    select new
                                    {
                                        FECHA_HASTA = Grupo.Key.FECHA_HASTA

                                    }).ToList();

                //si existen fechas reefer
                if (LinqFechasRF.Count != 0)
                {
                    //recorro fechas para hacer un query relacionado a la fecha de cada grupo
                    foreach (var Fecha in LinqFechasRF)
                    {
                        var FechaReefer_Filtro = Fecha.FECHA_HASTA.Value;

                        var LinqReefer = (from p in objCabecera.Detalle.Where(x => x.VISTO == true && x.REEFER.Equals("RF") && x.FECHA_HASTA.Value == FechaReefer_Filtro)
                                          select new
                                          {
                                              GKEY = (p.GKEY == 0 ? 0 : p.GKEY),
                                              REFERENCIA = (p.REFERENCIA == null ? string.Empty : p.REFERENCIA)
                                          }).ToList().OrderBy(x => x.GKEY);

                        //si existe informacion
                        string REFERENCIA = string.Empty;
                        List<Int64> Lista = new List<Int64>();
                        foreach (var Det in LinqReefer)
                        {
                            Lista.Add(Det.GKEY);
                            REFERENCIA = Det.REFERENCIA;
                        }
                        //ejecutamos servicios reefer por cada fecha
                        N4Ws.Entidad.Servicios.ReeferImpoHour(Lista, REFERENCIA, FechaReefer_Filtro, LoginName);
                    }
                        


                }

                /***********************************************************************************************************************************************
                *fin:reefer
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
                            var LinqInvoiceType = (from p in InvoiceType.Resultado.Where(X => X.codigo.Equals("IMPOFULL"))
                                           select new { valor = p.valor }).FirstOrDefault();

                            pInvoiceType = LinqInvoiceType.valor==null ? "2DA_MAN_IMPO_CNTRS" : LinqInvoiceType.valor;
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
                    Response.Redirect("~/contenedor/proformaimportacionadicional.aspx?id_proforma=" + cId.Trim() + "", false);

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }


        }

        protected void BtnFacturar_Click(object sender, EventArgs e)
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
                //if (string.IsNullOrEmpty(this.TxtHora.Text))
                //{
                //    HoraHasta = "00:00";
                //}
                //else
                //{
                //    HoraHasta = this.TxtHora.Text.Trim();
                //}

                //valida que se seleccione la persona a facturar
                if (this.CboAsumeFactura.SelectedIndex == 0)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona que asumirá la factura.</b>"));
                    this.CboAsumeFactura.Focus();
                    return;
                }


                //fecha hasta para sacar los servicios
                //Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                Fecha = string.Format("{0}", this.TxtFechaHasta.Text.Trim());
                if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la factura</b>"));
                    this.TxtFechaHasta.Focus();
                    return;
                }

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

                //valida que seleccione algun contenedores para facturar 
                var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                            select p.CONTENEDOR).ToList();

                if (LinqValidaContenedor.Count == 0)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los contenedores a Facturar </b>"));
                    return;
                }

                //valida que tenga fecha de salida
                foreach (var Det in objCabecera.Detalle.Where(x => x.VISTO == true))
                {
                    if (!Det.FECHA_HASTA.HasValue)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida de salida del contenedor {0}</b>", Det.CONTENEDOR));
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
                *validaciones de contenedores 
                **********************************************************************************************************************************************/
                XDocument XMLContenedores = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("CONTENEDORES", from p in objCabecera.Detalle.Where(x => x.VISTO == true).AsEnumerable().AsParallel()
                                                            select new XElement("CONTENEDOR",
                                                            new XAttribute("IV_CONTENEDOR", p.CONTENEDOR == null ? "" : p.CONTENEDOR.ToString()),
                                                            new XAttribute("IV_GKEY", p.GKEY == 0 ? "0" : p.GKEY.ToString()),
                                                            new XAttribute("IV_CAS", p.CAS.HasValue ? p.CAS.Value.ToString("yyyy/MM/dd") : ""),
                                                            new XAttribute("IV_BLOQUEO", p.BLOQUEO),
                                                            new XAttribute("IV_FECHA_HASTA", p.FECHA_HASTA.Value.ToString("yyyy/MM/dd")),
                                                            new XAttribute("IV_DOCUMENTO", p.DOCUMENTO == null ? "" : p.DOCUMENTO.ToString()))));


                var Valor = objValidacion.Validacion_Contenedores(XMLContenedores.ToString());
                if (Valor != string.Empty)
                {
                    this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Informativo! {0} </b>", Valor));
                    return;
                }
                /***********************************************************************************************************************************************
                *fin: validaciones de contenedores 
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
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro.,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", objCabecera.ID_FACTURADO, objCabecera.DESC_FACTURADO));


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
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error al obtener los datos de bloqueo de cliente, por favor comunicar a CGSA...Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00. {0}</b>", cMensajes));
                            return;
                        }

                        //si esta bloqueado, no permite facturar
                        if (Bloqueo_Cliente)
                        {

                            MensajesErrores = string.Format("El cliente {0} no esta Autorizado para la Facturación, presenta un bloqueo..", objCabecera.DESC_FACTURADO);

                            /*************************************************************************************************************************************
                             * crear caso salesforce
                             * **********************************************************************************************************************************/

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "Cliente Bloqueado", MensajesErrores.Trim(), 
                                string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos);

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
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error al obtener los datos de liberación de cliente, por favor comunicar a CGSA...{0}..Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", cMensajes));
                            return;
                        }

                        //si esta liberado
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
                                        objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos);

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
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! El cliente a facturar {0} tiene valores pendientes de pago...No podrá generar la factura..Casilla de atención: tesoreria@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 3,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", objCabecera.DESC_FACTURADO));
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
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Ocurrió un error al obtener los datos de bloqueo de cliente, por favor comunicar a CGSA..{0}..Casilla de atención: tesoreria@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 3,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", cMensajes));
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
                            objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos);

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

                    //10-02-2020
                    //para validar si son contendores de tipo reefer
                    if (Det.REEFER.Trim().Equals("RF"))
                    {
                        ContenedorReefer = true;
                    }

                    Lista_Gkeys.Add(Det.GKEY, Det.CONTENEDOR);
                }

                var ServiciosDuplicados = N4.Importacion.container.ValidarEventos("IMPOFULL", Lista_Gkeys, ClsUsuario.loginname.Trim());
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

                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "No pudo facturar debido a validación de servicios duplicados en el sistema", MensajesErrores.Trim(),
                           string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                            objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos);

                        /*************************************************************************************************************************************
                        * fin caso salesforce
                        * **********************************************************************************************************************************/

                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Estimado cliente {0}, se presento un problema el cual no le permitirá avanzar con la facturación de los contenedores seleccionados, {1} </b>", objCabecera.DESC_AGENTE, MensajeCasos));
                        return;


                    }
                }

                /***********************************************************************************************************************************************
                *fin validacion de eventos duplicados N4
                ***********************************************************************************************************************************************/
                //10-02-2020
                /**********************************************************************************************************************************
                * validacion de horas reefer
                * ********************************************************************************************************************************/
                if (ContenedorReefer)
                {
                    var ValidaContainersReefer = new List<N4.Importacion.container>();

                    foreach (var Det in objCabecera.Detalle.Where(x => x.VISTO == true && x.REEFER.Trim().Equals("RF")))
                    {
                        var ContReefer = new N4.Importacion.container();

                        ContReefer.CNTR_CONTAINER = Det.CONTENEDOR;
                        ContReefer.CNTR_CLNT_CUSTOMER_LINE = Det.LINEA;
                        ContReefer.CNTR_VEPR_REFERENCE = Det.REFERENCIA;
                        ValidaContainersReefer.Add(ContReefer);
                    }

                    var ValidaHorasReefer = N4.Importacion.container.ValidacionReeferImpo(ValidaContainersReefer, ClsUsuario.loginname.Trim());
                    if (!ValidaHorasReefer.Exitoso)
                    {
                        gkeyBuscado = string.Empty;
                        gkeyBuscado = ValidaHorasReefer.MensajeProblema;

                        string CuerpoMensaje = string.Format("Se presentaron los siguientes problemas: {0}", gkeyBuscado);

                        /*************************************************************************************************************************************
                        * crear caso salesforce
                        ***********************************************************************************************************************************/
                        MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", gkeyBuscado);

                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "No pudo facturar debido a validación a que no tiene cargadas las horas reefer", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                            objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos);

                        /*************************************************************************************************************************************
                        * fin caso salesforce
                        * **********************************************************************************************************************************/

                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Estimado cliente {0}, se presento un problema el cual no le permitirá avanzar con la facturación de los contenedores seleccionados, {1} </b>", objCabecera.DESC_AGENTE, MensajeCasos));
                        return;
                    }
                    else
                    {
                        var no_pasa = ValidaHorasReefer.Resultado.Where(f => !f.Item4).Count() > 0;
                        if (no_pasa)
                        {
                            //aqui puedo recuperar todos aquellos que no pasan validacion
                            //si quieres recupera la unidad/es para garbar un log  o enviar un mensaje personalizado
                            var novalidos = ValidaHorasReefer.Resultado.Where(g => !g.Item4).ToList();
                            StringBuilder tb = new StringBuilder();
                            novalidos.ForEach(t =>
                            {
                                tb.AppendFormat("Unidad:{0}->{1} /", t.Item1, !string.IsNullOrEmpty(t.Item3) ? t.Item3 : t.Item2);
                            });

                            string CuerpoMensaje = string.Format("Se presentaron los siguientes problemas: {0}", tb.ToString());

                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", tb.ToString());

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor",
                                "No pudo facturar debido a validación a que no tiene cargadas las horas reefer",
                                MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos);

                            /*************************************************************************************************************************************
                            * fin caso salesforce
                            * **********************************************************************************************************************************/

                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Estimado cliente {0}, se presento un problema el cual no le permitirá avanzar con la facturación de los contenedores seleccionados, {1} </b>", objCabecera.DESC_AGENTE, MensajeCasos));
                            return;
                        }
                    }

                    //ContainersReefer = Session["ListaCotenedor" + this.hf_BrowserWindowName.Value] as List<N4.Importacion.container>;
                    //if (ContainersReefer != null)
                    //{
                    //    var ValidaHorasReefer = N4.Importacion.container.ValidacionReeferImpo(ContainersReefer, ClsUsuario.Usuario.Trim());
                    //    if (!ValidaHorasReefer.Exitoso)
                    //    {
                    //        gkeyBuscado = string.Empty;
                    //        gkeyBuscado = ValidaHorasReefer.MensajeProblema;

                    //        string CuerpoMensaje = string.Format("Se presentaron los siguientes problemas: {0}", gkeyBuscado);

                    //        /*************************************************************************************************************************************
                    //        * crear caso salesforce
                    //        ***********************************************************************************************************************************/
                    //        MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", gkeyBuscado);

                    //        this.Enviar_Caso_Salesforce(ClsUsuario.Usuario, ClsUsuario.Ruc, "Facturación Contenedor", "No pudo facturar debido a validación a que no tiene cargadas las horas reefer", MensajesErrores.Trim(),
                    //            string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                    //            objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos);

                    //        /*************************************************************************************************************************************
                    //        * fin caso salesforce
                    //        * **********************************************************************************************************************************/

                    //        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Estimado cliente {0}, se presento un problema el cual no le permitirá avanzar con la facturación de los contenedores seleccionados, {1} </b>", objCabecera.DESC_AGENTE, MensajeCasos));
                    //        return;
                    //    }
                    //}
                }

                /**********************************************************************************************************************************
              * fin validacion de horas reefer
              * ********************************************************************************************************************************/


                objCabecera.GLOSA = this.Txtcomentario.Text.Trim();
                Contenedores = string.Join(", ", LinqListContenedor);
                //numero de carga
                Numero_Carga = objCabecera.NUMERO_CARGA;
                objCabecera.CONTENEDORES = Contenedores;
                objCabecera.FECHA_HASTA = FechaFactura;
                LoginName = objCabecera.IV_USUARIO_CREA.Trim();
                objCabecera.HORA_HASTA = HoraHasta;
                //actualizo el objeto
                Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;


                /***********************************************************************************************************************************************
                *1) inicio:reefer
                **********************************************************************************************************************************************/
                //saco los grupo de fechas
                var LinqFechasRF = (from p in objCabecera.Detalle.Where(x => x.VISTO == true && x.REEFER.Equals("RF")).AsEnumerable()
                                    group p by new { FECHA_HASTA = (p.FECHA_HASTA == null ? p.FECHA_ULTIMA : p.FECHA_HASTA) } into Grupo
                                    select new
                                    {
                                        FECHA_HASTA = Grupo.Key.FECHA_HASTA

                                    }).ToList();

                //si existen fechas reefer
                if (LinqFechasRF.Count != 0)
                {
                    //recorro fechas para hacer un query relacionado a la fecha de cada grupo
                    foreach (var Fecha in LinqFechasRF)
                    {
                        var FechaReefer_Filtro = Fecha.FECHA_HASTA.Value;

                        var LinqReefer = (from p in objCabecera.Detalle.Where(x => x.VISTO == true && x.REEFER.Equals("RF") && x.FECHA_HASTA.Value == FechaReefer_Filtro)
                                          select new
                                          {
                                              GKEY = (p.GKEY == 0 ? 0 : p.GKEY),
                                              REFERENCIA = (p.REFERENCIA == null ? string.Empty : p.REFERENCIA)
                                          }).ToList().OrderBy(x => x.GKEY);

                        //si existe informacion
                        string REFERENCIA = string.Empty;
                        List<Int64> Lista = new List<Int64>();
                        foreach (var Det in LinqReefer)
                        {
                            Lista.Add(Det.GKEY);
                            REFERENCIA = Det.REFERENCIA;
                        }
                        //ejecutamos servicios reefer por cada fecha
                        N4Ws.Entidad.Servicios.ReeferImpoHour(Lista, REFERENCIA, FechaReefer_Filtro, LoginName);
                    }



                }

                /***********************************************************************************************************************************************
                *fin:reefer
                **********************************************************************************************************************************************/

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
                    objDetalleFactura.IV_ID = 0;
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

                    objFactura.Detalle.Add(objDetalleFactura);

                }


                //10-02-2020
                /**********************************************************************************************************************************
                * validacion de fecha de zarpe
                * ********************************************************************************************************************************/
                /*saco el valor de dias para el zarpe*/
                List<Cls_Bil_Configuraciones> ParametrosDiasZarpe = Cls_Bil_Configuraciones.Parametros(out cMensajes);
                if (!String.IsNullOrEmpty(cMensajes))
                {
                    this.myModal.Attributes["class"] = "nover";
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en procesos de días de Zarpe.....{0}</b>", cMensajes));
                    this.Actualiza_Panele_Detalle();
                    return;
                }

                var LinqValorZarpe = (from Tope in ParametrosDiasZarpe.Where(Tope => Tope.NOMBRE.Equals("DIASZARPE"))
                                      select new
                                      {
                                          VALOR = Tope.VALOR == null ? string.Empty : Tope.VALOR
                                      }).FirstOrDefault();

                if (LinqValorZarpe != null)
                {
                    NDiasZarpe = int.Parse(LinqValorZarpe.VALOR);
                }
                else { NDiasZarpe = 1; }

                foreach (var Det in LinqDetalle)
                {
                    //10-02-2020
                    if (Det.CNTR_DEPARTED.HasValue)
                    {

                    }
                    if (Det.CNTR_DESCARGA.Value > Det.CNTR_DEPARTED.Value.AddDays(NDiasZarpe))
                    {
                        string CuerpoMensaje = string.Format("Se presentaron los siguientes problemas: {0}", gkeyBuscado);

                        /*************************************************************************************************************************************
                        * crear caso salesforce
                        ***********************************************************************************************************************************/
                        MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0} tiene la fecha de descarga {1} mayor a la fecha de zarpe {2}.. ", 
                            Det.CONTENEDOR, Det.CNTR_DESCARGA.Value.ToString("dd/MM/yyyy"), Det.CNTR_DEPARTED.Value.ToString("dd/MM/yyyy"));

                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Contenedor", "No pudo facturar debido a validación de fecha de descarga mayor a fecha de zarpe",
                            MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                            objFactura.IV_DESC_FACTURADO, objFactura.IV_DESC_AGENTE, out MensajeCasos);

                        /*************************************************************************************************************************************
                        * fin caso salesforce
                        * **********************************************************************************************************************************/

                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Estimado cliente {0}, se presento un problema el cual no le permitirá avanzar con la facturación de los contenedores seleccionados, {1} </b>", objFactura.IV_DESC_AGENTE, MensajeCasos));
                        return;
                    }

                }

                /**********************************************************************************************************************************
               * fin validacion de fecha de zarpe
               * ********************************************************************************************************************************/


               /**********************************************************************************************************************************
               *saco los grupo de fechas para recorrer y sacar servicios por cada fecha
               **********************************************************************************************************************************/
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
                            var LinqInvoiceType = (from p in InvoiceType.Resultado.Where(X => X.codigo.Equals("IMPOFULL"))
                                                   select new { valor = p.valor }).FirstOrDefault();

                            pInvoiceType = LinqInvoiceType.valor == null ? "2DA_MAN_IMPO_CNTRS" : LinqInvoiceType.valor;
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

                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se puede convertir en campo numerico el gkey: {0} </b>", xBilling.response.billInvoice.gkey));
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
                                    objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos);

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
                        //recorre todos los contenedores seleccionados para verificar fecha
                        cMensajes = string.Empty;
                        string cMensajeActualizados = string.Empty;
                        foreach (var Det in objFactura.Detalle.Where(p => !p.IV_TIENE_SERVICIOS))
                        {
                            //si la fecha de salida es menor a la fecha tope con los dias libres incluidos, y si no es la primera facturacion
                            if ((Det.IV_FECHA_HASTA.Value <= Det.IV_FECHA_TOPE_DLIBRE.Value) && Det.IV_FECHA_ULTIMA.HasValue)
                            {
                                /*grabar transaccion de factura*/
                                objActualiza_Pase = new Cls_Bil_Invoice_Actualiza_Pase();
                                objActualiza_Pase.IV_ID = Det.IV_ID;
                                objActualiza_Pase.IV_GKEY = Det.IV_GKEY;
                                objActualiza_Pase.IV_MRN = Det.IV_MRN;
                                objActualiza_Pase.IV_MSN = Det.IV_MSN;
                                objActualiza_Pase.IV_HSN = Det.IV_HSN;
                                objActualiza_Pase.IV_CONTENEDOR = Det.IV_CONTENEDOR;
                                objActualiza_Pase.IV_FECHA_ULTIMA = Det.IV_FECHA_ULTIMA;
                                objActualiza_Pase.IV_FECHA_HASTA = Det.IV_FECHA_HASTA;
                                objActualiza_Pase.IV_MODULO = Det.IV_MODULO;

                                var nIdRegistro = objActualiza_Pase.SaveTransaction_Update(out cMensajes);
                                if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                                {

                                    this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo actualizar datos de la factura para el contenedor {0}, {1}</b>", Det.IV_CONTENEDOR, cMensajes));
                                    return;
                                }
                                else
                                {
                                    cMensajeActualizados = cMensajeActualizados + string.Format("Se procedió con la actualización de la fecha de salida del contendor {0}, el mismo cuenta con días libres y podrá generar pases de puerta hasta: {1} <br/>", Det.IV_CONTENEDOR, Det.IV_FECHA_HASTA.Value.ToString("dd/MM/yyyy"));
                                }
                            }
                        }

                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen servicios pendientes para facturar. {0} </b>", cMensajeActualizados));
                        return;

                    }

                    //si no existen servicios a facturar
                   /* if (objFactura.DetalleServicios.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen servicios pendientes para facturar</b>"));
                        return;
                    }*/

                    this.Ocultar_Mensaje();
                    string cId = securetext(this.hf_BrowserWindowName.Value);
                    Response.Redirect("~/contenedor/facturaimportacionadicional.aspx?id=" + cId.Trim() + "", false);

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }


        }

        #endregion

        #region "Eventos Check"
        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool ChkEstado = this.ChkTodos.Checked;
                CultureInfo enUS = new CultureInfo("en-US");

                objCabecera = Session["Transaccion" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                if (objCabecera == null)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta</b>"));
                    return;
                }

                foreach (var Det in objCabecera.Detalle)
                {
                    var Detalle = objCabecera.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(Det.CONTENEDOR.Trim()));
                    if (Detalle != null)
                    {
                        if (Detalle.REEFER.Equals("RF") && Detalle.CONECTADO.Equals("NO CONECTADO"))
                        {
                            //Detalle.VISTO = ChkEstado;
                        }
                        else
                        {
                            if (!Detalle.ESTADO_RDIT.Equals("A") || Detalle.IN_OUT.Equals("OUT"))
                            {

                            }
                            else
                            {
                                if (ChkEstado)
                                {
                                    //fecha hasta - para sacar los servicios
                                    //if (string.IsNullOrEmpty(this.TxtHora.Text))
                                    //{
                                    //    HoraHasta = "00:00";
                                    //}
                                    //else
                                    //{
                                    //    HoraHasta = this.TxtHora.Text.Trim();
                                    //}

                                    //if (Detalle.REEFER.Trim().Equals("RF") && HoraHasta.Trim().Equals("00:00"))
                                    //{
                                    //    Detalle.VISTO = false;
                                    //    Detalle.FECHA_HASTA = null;
                                    //    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una hora para el contenedor Reefer {0}</b>", Detalle.CONTENEDOR));
                                    //    this.TxtHora.Focus();
                                    //    break;
                                    //}
                                    //else
                                    //{
                                        //Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                                    Fecha = string.Format("{0}", this.TxtFechaHasta.Text.Trim());
                                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                                    {
                                        Detalle.VISTO = false;
                                        Detalle.FECHA_HASTA = null;
                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la cotización o factura</b>"));
                                        this.TxtFechaHasta.Focus();
                                        break;
                                    }
                                    else
                                    {
                                        if (FechaFactura.Date < System.DateTime.Today.Date)
                                        {
                                            Detalle.VISTO = false;
                                            Detalle.FECHA_HASTA = null;
                                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha igual o superior a la fecha actual</b>"));
                                            this.TxtFechaHasta.Focus();
                                            break;
                                        }
                                        else
                                        {
                                            Detalle.VISTO = ChkEstado;
                                            Detalle.FECHA_HASTA = FechaFactura;
                                        }

                                    }
                                    //}

                                }
                                else
                                {
                                    Detalle.VISTO = false;
                                    Detalle.FECHA_HASTA = null;
                                    Detalle.TURNO = string.Empty;
                                    Detalle.IDPLAN = "0";
                                }
                            }

                        }

                    }
                }


                tablePagination.DataSource = objCabecera.Detalle;
                tablePagination.DataBind();

                Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;

                this.Actualiza_Panele_Detalle();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", ex.Message));

            }
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