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

using System.Web.Services;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using Newtonsoft.Json;

namespace CSLSite
{


    public partial class facturacion_sav_cliente_listado : System.Web.UI.Page
    {


        #region "Clases"
        private static Int64? lm = -3;
        private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
       

      
        private Cls_Bil_Invoice_Validaciones objValidacion = new Cls_Bil_Invoice_Validaciones();

       
        private List<Cls_Bil_AsumeFactura> List_Asume { set; get; }
      
      

        private SAV_Cabecera_Factura objCabecerSAV = new SAV_Cabecera_Factura();
        private SAV_Detalle_Factura objDetalleSAV = new SAV_Detalle_Factura();
        private SAV_Clientes objClienteSAV = new SAV_Clientes();
        private SAV_Detalle_Pases objDetallePaseSAV = new SAV_Detalle_Pases();
        private SAV_Detalle_Error objErroresSAV = new SAV_Detalle_Error();
        private SAV_Detalle_Servicios objServiciosSAV = new SAV_Detalle_Servicios();
        private SAV_Detalle_Ok objProcesadosSAV = new SAV_Detalle_Ok();
        private SAV_Asigna_Factura objActualizaSAV = new SAV_Asigna_Factura();
        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;

      
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
     
        private string LoginName = string.Empty;
      
       
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
      
        private Int64 DiasCredito = 0;
       
       
        private string gkeyBuscado = string.Empty;

      
        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
      

        private static string TextoLeyenda = string.Empty;

        private static string TextoProforma = string.Empty;
        private static string TextoServicio = string.Empty;

        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();
        private string draftNumber = string.Empty;
        private Int64 draftNumberFinal = 0;
        private string NumeroFactura = string.Empty;

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

        private int ActualizaFila
        {
            get
            {
                return (int)Session["ActualizaFila"];
            }
            set
            {
                Session["ActualizaFila"] = value;
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
          
            UPBOTONES.Update();
         
           

        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
        
        }

        private void Limpia_Datos_cliente()
        {
          
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

            if (Tipo == 5)//rubros
            {
                this.banmsg_Pase.Visible = false;
                this.banmsg_det.Visible = false;
                this.banmsg.Visible = false;
                this.banmsg.InnerHtml = string.Empty;
                this.banmsg_det.InnerHtml = string.Empty;

        

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
            objCabecerSAV = new SAV_Cabecera_Factura();
            Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;
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
                detalle_carga.Tipo = TipoCarga.BRBK; //opcional
                detalle_carga.Titulo = "Modulo de Facturación SAV Devolución de Contenedores"; //opcional
                detalle_carga.Novedad = pNovedad; //mensaje del modulo o error

                detalle_carga.Detalles.Add(new DetalleCarga("Errores:", MensajesErrores));

                if (!string.IsNullOrEmpty(pValor1)) { detalle_carga.Detalles.Add(new DetalleCarga("BL/REF", pValor1));  }
                if (!string.IsNullOrEmpty(pValor2)) { detalle_carga.Detalles.Add(new DetalleCarga("Cliente", pValor2)); }
                if (!string.IsNullOrEmpty(pValor3)) { detalle_carga.Detalles.Add(new DetalleCarga("Otros", pValor3)); }

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
                       

                        this.Actualiza_Paneles();
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

                this.TxtFechaDesde.Text = Server.HtmlEncode(this.TxtFechaDesde.Text);
                this.TxtFechaHasta.Text = Server.HtmlEncode(this.TxtFechaHasta.Text);

                if (!Page.IsPostBack)
                {

                    string desde = DateTime.Today.Month.ToString("D2") + "/01/" + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                        this.Mostrar_Mensaje(4,string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha desde"));
                        return;
                    }

                    this.TxtFechaDesde.Text = fdesde.ToString("MM/dd/yyyy");
                    this.TxtFechaHasta.Text = DateTime.Today.ToString("MM/dd/yyyy");

                    /*secuencial de sesion*/
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    this.Crear_Sesion();

                    objCabecerSAV = new SAV_Cabecera_Factura();
                    Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;


                    this.Cargar_Exportadores();


                    this.Cargar_PasesPendientes();

                  
                    this.UPBOTONES.Update();


                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", ex.Message));
            }
        }


        #endregion

        #region "Evento Cargar Pases a Facturar"

        private void Cargar_PasesPendientes()
        {
            try
            {

                if (Response.IsClientConnected)
                {
                  
                 
                       
                    bool Ocultar_Mensaje = false;

                    string Msg = string.Empty;

                    this.LabelTotal.InnerText = string.Format("DETALLE DE PASE DE PUERTAS FACTURADOS");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }


                    if (string.IsNullOrEmpty(this.TxtFechaDesde.Text))
                    {
                        this.Mostrar_Mensaje(1,string.Format("<b>Informativo! </b>Por favor seleccione la fecha inicial"));
                        this.TxtFechaDesde.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TxtFechaHasta.Text))
                    {
                        this.Mostrar_Mensaje(1,string.Format("<b>Informativo! </b>Por favor seleccione la fecha final"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    CultureInfo enUS = new CultureInfo("en-US");

                    if (!string.IsNullOrEmpty(TxtFechaDesde.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                        {
                            this.Mostrar_Mensaje(1,string.Format("<b>Informativo! </b>El formato de la fecha inicial debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                            this.Mostrar_Mensaje(1,string.Format("<b>Informativo! </b>El formato de la fecha final debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text));
                            this.TxtFechaHasta.Focus();
                            return;

                        }
                    }

                    TimeSpan tsDias = fechahasta - fechadesde;
                    int diferenciaEnDias = tsDias.Days;
                    if (diferenciaEnDias < 0)
                    {
                        this.Mostrar_Mensaje(1,string.Format("<b>Informativo! </b>La Fecha de Ingreso: {0} \nNO deber ser mayor a la \n Fecha final: {1}", TxtFechaDesde.Text, TxtFechaDesde.Text));
                        return;
                    }
                    if (diferenciaEnDias > 31)
                    {
                        this.Mostrar_Mensaje(1,string.Format("<b>Informativo! </b>Solo puede consultar las facturas de hasta un mes."));
                        return;
                    }


                    tablePagination.DataSource = null;
                    tablePagination.DataBind();


                     
                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdAgenteCodigo = string.Empty;



                    /*detalle de exportadores*/
                    List<SAV_Detalle_Factura> ListPases = SAV_Detalle_Factura.Detalle_Pases_Facturados_Por_Clientes(fechadesde, fechahasta, ClsUsuario.ruc, out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener detalle de pases de devolución de contenedores....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                       

                     
                    if (ListPases != null)
                    {

                        if (ListPases.Count != 0)
                        {
                            objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;
                            objCabecerSAV.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objCabecerSAV.SESION = this.hf_BrowserWindowName.Value;
                            objCabecerSAV.Detalle_Pases.Clear();


                            Int16 Secuencia = 1;
                            foreach (var Det in ListPases)
                            {
                                objDetalleSAV = new SAV_Detalle_Factura();
                                objDetalleSAV.turno_id = Det.turno_id;
                                objDetalleSAV.turno_fecha = Det.turno_fecha;
                                objDetalleSAV.turno_hora = Det.turno_hora;
                                objDetalleSAV.unidad_id = Det.unidad_id;
                                objDetalleSAV.unidad_tamano = Det.unidad_tamano;
                                objDetalleSAV.unidad_linea = Det.unidad_linea;
                                objDetalleSAV.unidad_booking = Det.unidad_booking;
                                objDetalleSAV.unidad_referencia = Det.unidad_referencia;
                                objDetalleSAV.unidad_estatus = Det.unidad_estatus;
                                objDetalleSAV.unidad_key = Det.unidad_key;
                                objDetalleSAV.cantidad = 1;
                                objDetalleSAV.chofer_licencia = Det.chofer_licencia;
                                objDetalleSAV.chofer_nombre = Det.chofer_nombre;
                                objDetalleSAV.vehiculo_placa = Det.vehiculo_placa;
                                objDetalleSAV.vehiculo_desc = Det.vehiculo_desc;
                                objDetalleSAV.creado_usuario = Det.creado_usuario;
                                objDetalleSAV.creado_fecha = Det.creado_fecha;
                                objDetalleSAV.n4_unit_key = Det.n4_unit_key;
                                objDetalleSAV.n4_message = Det.n4_message;
                                objDetalleSAV.active = Det.active;
                                objDetalleSAV.id = Det.id;
                                objDetalleSAV.documento_id = Det.documento_id;
                                objDetalleSAV.estado_pago = Det.estado_pago;
                                objDetalleSAV.estado = Det.estado;
                                objDetalleSAV.ruc_cliente = Det.ruc_cliente;
                                objDetalleSAV.name_cliente = Det.name_cliente;
                                objDetalleSAV.ruc_asume = Det.ruc_asume;
                                objDetalleSAV.name_asume = Det.name_asume;
                                objDetalleSAV.ruc_facturar = Det.ruc_facturar;
                                objDetalleSAV.cliente_facturar = Det.cliente_facturar;
                                objDetalleSAV.fila = Secuencia;
                                objDetalleSAV.numero_factura = Det.numero_factura;

                                var existe = objCabecerSAV.Detalle_Clientes.FirstOrDefault(p => p.CLNT_CUSTOMER == Det.ruc_facturar);
                                if (existe != null)
                                {
                                    objDetalleSAV.asume_ruc_facturar = existe.CLNT_CUSTOMER;
                                    objDetalleSAV.asume_cliente_facturar = existe.CLNT_NAME;
                                    objDetalleSAV.id_facturar = existe.gkey;
                                }
                                else
                                {
                                    objDetalleSAV.asume_ruc_facturar = Det.ruc_cliente;
                                    objDetalleSAV.asume_cliente_facturar = Det.name_cliente;
                                    objDetalleSAV.id_facturar = null;
                                }

                                objDetalleSAV.visto = true;
                                objDetalleSAV.invoice_type = Det.invoice_type;
                                objCabecerSAV.Detalle_Pases.Add(objDetalleSAV);

                                Secuencia++;
                            }

                            tablePagination.DataSource = objCabecerSAV.Detalle_Pases;
                            tablePagination.DataBind();

                            var TotalPases = objCabecerSAV.Detalle_Pases.Sum(x => x.cantidad);
                            objCabecerSAV.cantidad = TotalPases;

                            this.LabelTotal.InnerText = string.Format("TOTAL DE PASES FACTURADOS: {0}", objCabecerSAV.cantidad);



                            Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                         



                            this.Actualiza_Panele_Detalle();


                            Ocultar_Mensaje = true;
                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();

                          

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de Devolución de Contenedores Facturados....</b>"
                                ));

                            this.Actualiza_Panele_Detalle();
                            return;
                        }
   
                       
                    }
                    else
                    {

                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                      

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de Devolución de Contenedores Facturados....</b>"
                            ));

                        this.Actualiza_Panele_Detalle();
                        return;

                    }



                    if (Ocultar_Mensaje)
                    {
                        this.Ocultar_Mensaje();
                    }

                    
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

            }

        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            this.Cargar_PasesPendientes();

          
            UPBOTONES.Update();


        }

        protected void BtnFiltrarGrid_Click(object sender, EventArgs e)
        {
            try
            {
                objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;

               

                if (objCabecerSAV == null)
                { return; }


                List<SAV_Detalle_Factura> objCabceraOrdenada = new List<SAV_Detalle_Factura>();
                objCabceraOrdenada = objCabecerSAV.Detalle_Pases;

                if (txtFiltro.Text != string.Empty)
                {
                    objCabceraOrdenada = (from A in objCabecerSAV.Detalle_Pases.Where(p => (p.asume_ruc_facturar == null ? string.Empty : p.asume_ruc_facturar).ToUpper().Contains(txtFiltro.Text.ToUpper()) ||
                                         (p.asume_cliente_facturar == null ? string.Empty : p.asume_cliente_facturar).ToUpper().Contains(txtFiltro.Text.ToUpper()) ||
                                          (p.unidad_id == null ? string.Empty : p.unidad_id).ToUpper().Contains(txtFiltro.Text.ToUpper())
                                         )
                                          select A).ToList();

                  

                    tablePagination.DataSource = objCabceraOrdenada;
                    tablePagination.DataBind();
                }
                else
                {
                    tablePagination.DataSource = objCabecerSAV.Detalle_Pases;
                    tablePagination.DataBind();
                }

                this.Actualiza_Paneles();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
            }
        }

      
        #endregion

        #region "Cargar clientes"
        private void Cargar_Exportadores()
        {
            try
            {

                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    OcultarLoading("1");
                    return;
                }

                string v_mensaje = string.Empty;

                objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;

                var lookup = SAV_Clientes.Clientes_Todos(out v_mensaje);

                foreach (var Det in lookup)
                {
                    objClienteSAV = new SAV_Clientes();
                    objClienteSAV.CLNT_CUSTOMER = Det.CLNT_CUSTOMER;
                    objClienteSAV.CLNT_NAME = Det.CLNT_NAME;
                    objClienteSAV.CLNT_ADRESS = Det.CLNT_ADRESS;
                    objClienteSAV.CLNT_EMAIL = Det.CLNT_EMAIL;
                    objClienteSAV.CLNT_TYPE = Det.CLNT_TYPE;
                    objClienteSAV.CLNT_EBILLING = Det.CLNT_EBILLING;
                    objClienteSAV.CLNT_FAX_INVC = Det.CLNT_FAX_INVC;
                    objClienteSAV.CLNT_RFC = Det.CLNT_RFC;
                    objClienteSAV.CLNT_ACTIVE = Det.CLNT_ACTIVE;
                    objClienteSAV.CLNT_ROLE = Det.CLNT_ROLE;
                    objClienteSAV.CODIGO_SAP = Det.CODIGO_SAP;
                    objClienteSAV.gkey = Det.gkey;
                    objCabecerSAV.Detalle_Clientes.Add(objClienteSAV);
                }

                Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

            }
        }
        #endregion


        #region "Eventos de la grilla"

     

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                    //this.Ocultar_Mensaje();

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(3, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(3, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Ver")
                    {
                       
                       

                    }

                  
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

                }
            }
        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label LblFactura = e.Item.FindControl("LblFactura") as Label;

                Button BtnPrint = e.Item.FindControl("BtnImprimir") as Button;

             
                if (string.IsNullOrEmpty(LblFactura.Text))
                {

                    BtnPrint.Attributes["disabled"] = "disabled";
                }
            }
        }

        #endregion

        #region "Eventos buscador"

        
       
        #endregion



        #region "Evento Botones"



      

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect("~/btsagencia/facturacionagencia.aspx", false);



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {
           
            this.Actualiza_Paneles();

        }

        protected void BtnCotizar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {



               

            }
        }

    


        #endregion

        #region "Eventos Exportar"

        public static DataTable ConvertRepeaterToDataTable(string repeater)
        {
            // Crear el DataTable con las columnas necesarias
            DataTable dataTable = new DataTable();

            var dataList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(repeater);

            if (dataList.Count > 0)
            {
                // Agregar columnas según los campos en el Repeater (ajusta los nombres según tu Repeater)
                dataTable.Columns.Add("fila", typeof(int));
                dataTable.Columns.Add("numero_factura", typeof(string));
                dataTable.Columns.Add("turno_fecha", typeof(string));
                dataTable.Columns.Add("turno_hora", typeof(string));
                dataTable.Columns.Add("asume_ruc_facturar", typeof(string));
                dataTable.Columns.Add("asume_cliente_facturar", typeof(string));
                dataTable.Columns.Add("contenedor", typeof(string));
                dataTable.Columns.Add("booking", typeof(string));
                dataTable.Columns.Add("linea", typeof(string));
                dataTable.Columns.Add("estado_pago", typeof(string));

                // Agregar filas al DataTable
                foreach (var dict in dataList)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var kvp in dict)
                    {
                        row[kvp.Key] = kvp.Value ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(row);
                }
            }
            
         

         
            return dataTable;
        }


        public static byte[] CreateExcelBytesFromDataTable(string fechaDesde, string fechaHasta, string repeater)
        {
            CultureInfo enUS = new CultureInfo("en-US");

            DataTable dataTable = new DataTable();

            dataTable = ConvertRepeaterToDataTable(repeater);

            Color colorPlaneado = ColorTranslator.FromHtml("#59B653");
            Color colorReservado = ColorTranslator.FromHtml("#5AABD9");
            Color colorDisponible = ColorTranslator.FromHtml("#D9DB63");

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");


                worksheet.Cells["B1:B1"].Value = "Fecha Desde:";
                worksheet.Cells["B1:B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["B1:B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["B1:B1"].Style.Fill.BackgroundColor.SetColor(colorPlaneado);
                worksheet.Cells["B1:B1"].Style.Font.Color.SetColor(System.Drawing.Color.White);


                worksheet.Cells["C1:C1"].Value = fechaDesde;
                worksheet.Cells["C1:C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                worksheet.Cells["H1:H1"].Value = "Fecha Hasta:";
                worksheet.Cells["H1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["H1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["H1:H1"].Style.Fill.BackgroundColor.SetColor(colorReservado);
                worksheet.Cells["H1:H1"].Style.Font.Color.SetColor(System.Drawing.Color.White);

                worksheet.Cells["I1:I1"].Value = fechaHasta;
                worksheet.Cells["I1:I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //Asignar los valores de la cabecera
                int c = 1;
                int r = 2;
               
                foreach (DataColumn column in dataTable.Columns)  //printing column headings
                {


                    worksheet.Cells[r, c].Value = column.ColumnName;
                    worksheet.Cells[r, c].Style.Font.Bold = true;
                    worksheet.Cells[r, c].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells[r, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[r, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[r, c].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                    c = c + 1;
                }




                r = 3;
                c = 1;

                int fila = 0;
                int eachRow = 0;
                int col = 0;
                int new_col = c;

                var query = from p in dataTable.AsEnumerable().Where(y => y.Field<int>("fila") != 0)
                            select p;

                //se convierte para recorrer
                DataTable resultado = query.CopyToDataTable<DataRow>();

                if (resultado.Rows.Count > 0)
                {
                    for (eachRow = 0; eachRow < resultado.Rows.Count;)
                    {
                        col = 1;
                        new_col = c;
                        foreach (DataColumn column in resultado.Columns)
                        {

                            worksheet.Cells[r, new_col].Value = resultado.Rows[fila][col - 1];
                            col++;
                            new_col++;
                        }

                        eachRow++;
                        r++;
                        fila++;

                    }

                }


                // Ajustar el ancho de las columnas automáticamente
                worksheet.Cells.AutoFitColumns();

                // Convertir el archivo Excel a bytes
                return excelPackage.GetAsByteArray();

            }


      
        }

        [WebMethod]
        public static void ExportarExcel(string fechaDesde, string fechaHasta, string repeater)
        {
            try
            {
                byte[] excelBytes = CreateExcelBytesFromDataTable(fechaDesde, fechaHasta, repeater);

                string dateTimeFormat = "yyyyMMddHHmmssfff";
                string formattedDateTime = DateTime.Now.ToString(dateTimeFormat);

                string fileName = "Reporte_" + formattedDateTime + ".xlsx";
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

                HttpContext.Current.Response.BinaryWrite(excelBytes);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {

                // Define el código de estado HTTP y la descripción del error.
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.StatusDescription = "Error al exportar el archivo Excel";


            }
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