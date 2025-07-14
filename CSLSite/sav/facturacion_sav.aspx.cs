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


    public partial class facturacion_sav : System.Web.UI.Page
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
            UPDETALLEERROR.Update();
            UPBOTONES.Update();
            this.UPBOTONESBUSCADOR.Update();
           

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
                detalle_carga.Titulo = "Modulo de Facturación Bodegas BTS EXPORTADOR"; //opcional
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
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener depósitos....{0}</b>", ex.Message));
                this.Actualiza_Panele_Detalle();
                return;

            }
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


                    this.LlenaComboDepositos();

                    this.Cargar_Exportadores();


                    this.Cargar_PasesPendientes();

                    this.BtnErrores.Attributes["disabled"] = "disabled";

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
                  
                    this.BtnFacturar.Attributes.Add("disabled", "disabled");

                       
                    bool Ocultar_Mensaje = false;

                    string Msg = string.Empty;

                    this.LabelTotal.InnerText = string.Format("DETALLE DE PASE DE PUERTAS");

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
                        this.Mostrar_Mensaje(1,string.Format("<b>Informativo! </b>La Fecha de Ingreso: {0} \nNO deber ser mayor a la \nFecha final: {1}", TxtFechaDesde.Text, TxtFechaDesde.Text));
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


                    Int64 ID_DEPORT;

                    if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                    {
                        ID_DEPORT = 0;
                    }

                    /*detalle de exportadores*/
                    List<SAV_Detalle_Factura> ListPases = SAV_Detalle_Factura.Detalle_Pases_Repcontver(fechadesde, fechahasta, ID_DEPORT, out cMensajes);
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

                                ////objDetalleSAV.asume_ruc_facturar = Det.ruc_facturar;
                                ////objDetalleSAV.asume_cliente_facturar = Det.cliente_facturar;
                                objDetalleSAV.visto = true;
                                objDetalleSAV.invoice_type = Det.invoice_type;
                                objCabecerSAV.Detalle_Pases.Add(objDetalleSAV);

                                Secuencia++;
                            }

                            tablePagination.DataSource = objCabecerSAV.Detalle_Pases;
                            tablePagination.DataBind();

                            var TotalPases = objCabecerSAV.Detalle_Pases.Sum(x => x.cantidad);
                            objCabecerSAV.cantidad = TotalPases;

                            this.LabelTotal.InnerText = string.Format("TOTAL DE PASES PENDIENTES DE FACTURAR: {0}", objCabecerSAV.cantidad);



                            Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                            this.BtnFacturar.Attributes.Remove("disabled");



                            this.Actualiza_Panele_Detalle();


                            Ocultar_Mensaje = true;
                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();

                            this.BtnFacturar.Attributes.Add("disabled", "disabled");//


                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de Devolución de Contenedores pendientes de facturar....</b>"
                                ));

                            this.Actualiza_Panele_Detalle();
                            return;
                        }

   
                       
                    }
                    else
                    {

                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        this.BtnFacturar.Attributes.Add("disabled", "disabled");//


                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de Devolución de Contenedores pendientes de facturar....</b>"
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

            this.BtnErrores.Attributes["disabled"] = "disabled";
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

        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
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
                        OcultarLoading("1");
                        return;
                    }

                    bool ChkEstado = this.ChkTodos.Checked;

                    objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;
                    if (objCabecerSAV == null)
                    {
                        return;
                    }

                    if (txtFiltro.Text != string.Empty)
                    {
                        foreach (var Det in objCabecerSAV.Detalle_Pases.Where(p => (p.asume_ruc_facturar == null ? string.Empty : p.asume_ruc_facturar).ToUpper().Contains(txtFiltro.Text.ToUpper()) ||
                                             (p.asume_cliente_facturar == null ? string.Empty : p.asume_cliente_facturar).ToUpper().Contains(txtFiltro.Text.ToUpper()) ||
                                              (p.unidad_id == null ? string.Empty : p.unidad_id).ToUpper().Contains(txtFiltro.Text.ToUpper())))
                        {
                            int id = Det.id;

                            var Detalle = objCabecerSAV.Detalle_Pases.FirstOrDefault(f => f.id.Equals(id));
                            if (Detalle != null)
                            {

                                Detalle.visto = ChkEstado;

                            }
                        }

                    }
                    else
                    {
                        foreach (var Det in objCabecerSAV.Detalle_Pases)
                        {
                            int id = Det.id;

                            var Detalle = objCabecerSAV.Detalle_Pases.FirstOrDefault(f => f.id.Equals(id));
                            if (Detalle != null)
                            {

                                Detalle.visto = ChkEstado;

                            }
                        }
                    }

                      

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


                    //tablePagination.DataSource = objCabecerSAV.Detalle_Pases;
                    //tablePagination.DataBind();

                    Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                   
                    this.Actualiza_Paneles();

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));
                }
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

        protected void chkPase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                Label Lblfila = (Label)item.FindControl("Lblfila");

                int ID_PASE = int.Parse(Lblfila.Text);
                objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;
                var Detalle = objCabecerSAV.Detalle_Pases.FirstOrDefault(f => f.fila.Equals(ID_PASE));
                if (Detalle != null)
                {
                    Detalle.visto = chkPase.Checked;

                }

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

             

                Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                this.Actualiza_Paneles();


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }

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
                       
                        string v_mensaje = string.Empty;

                        this.ID_FILA.Value = t.ToString();
                        this.TxtFila.Text = t.ToString();

                        ActualizaFila = int.Parse(t);

                        var lookup = SAV_Clientes.Buscador_Clientes(null, out v_mensaje);

                        if (lookup != null && lookup.Count > 0)
                        {
                            this.PaginationBuscador.DataSource = lookup;
                            this.PaginationBuscador.DataBind();

                            banmsg_buscador.InnerText = string.Empty;
                            banmsg_buscador.Visible = false;

                            UPBUSCADOR.Update();
                          
                        }

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
                //Button Btn = e.Item.FindControl("BtnFacturarExp") as Button;
                Button BtnPrint = e.Item.FindControl("BtnImprimir") as Button;

                //if (!string.IsNullOrEmpty(LblFactura.Text))
                //{

                //    Btn.Attributes["disabled"] = "disabled";
                //}
                if (string.IsNullOrEmpty(LblFactura.Text))
                {

                    BtnPrint.Attributes["disabled"] = "disabled";
                }
            }
        }

        #endregion

        #region "Eventos buscador"

        protected void find_Click(object sender, EventArgs e)
        {

            try
            {

                string v_mensaje = string.Empty;

                var lookup = SAV_Clientes.Buscador_Clientes(txtfinder.Text, out v_mensaje);

                if (lookup != null && lookup.Count > 0)
                {
                    this.PaginationBuscador.DataSource = lookup;
                    this.PaginationBuscador.DataBind();

                    banmsg_det.InnerText = string.Empty;
                    banmsg_det.Visible = false;
                    UPBUSCADOR.Update();
                }
                else
                {
                    this.PaginationBuscador.DataSource = null;
                    this.PaginationBuscador.DataBind();

                    banmsg_det.InnerText = "No existen registro que mostrar con los criterios ingresados!!";
                    banmsg_det.Visible = true;
                    UPBUSCADOR.Update();
                }
            }
            catch (Exception ex)
            {

                this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

            }

        }

        protected void PaginationBuscador_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                        Int64 ID_EXPORTADOR = int.Parse(t.ToString());
                        string v_mensaje = string.Empty;




                        objClienteSAV = new SAV_Clientes();
                        objClienteSAV.gkey = ID_EXPORTADOR;

                        if (objClienteSAV.PopulateMyData(out v_mensaje))
                        {

                         

                            //agregar novedad
                            objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;

                            //existe pase a remover
                            var Detalle = objCabecerSAV.Detalle_Pases.FirstOrDefault(f => f.fila == ActualizaFila);
                            if (Detalle != null)
                            {
                                Detalle.asume_ruc_facturar = objClienteSAV.CLNT_CUSTOMER;
                                Detalle.asume_cliente_facturar = objClienteSAV.CLNT_NAME;
                                Detalle.id_facturar = objClienteSAV.gkey;


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



                                //tablePagination.DataSource = objCabecerSAV.Detalle_Pases;
                                //tablePagination.DataBind();

                                Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;


                                this.Actualiza_Paneles();

                            }
                            else
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información del exportador para actualizar: {0} </b>", t.ToString()));
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información del exportador para actualizar: {0} </b>", t.ToString()));
                            return;
                        }

                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

                }
            }
        }
        #endregion



        #region "Evento Botones"



        //revisando 1
        //protected void BtnBuscar_Click(object sender, EventArgs e)
        //{

        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {
        //            this.BtnFacturar.Attributes.Add("disabled", "disabled");

        //            OcultarLoading("2");
        //            bool Ocultar_Mensaje = true;
                   
                   
        //            string Msg = string.Empty;

        //            this.LabelTotal.InnerText = string.Format("BODEGA FRIA/SECA");

        //            if (HttpContext.Current.Request.Cookies["token"] == null)
        //            {
        //                System.Web.Security.FormsAuthentication.SignOut();
        //                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //                Session.Clear();
        //                OcultarLoading("1");
        //                return;
        //            }

                 
                   
        //            tablePagination.DataSource = null;
        //            tablePagination.DataBind();


        //            /*saco los dias libre como parametros generales*/
        //            List<Cls_Bil_Configuraciones> Parametros = Cls_Bil_Configuraciones.Parametros( out cMensajes);
        //            if (!String.IsNullOrEmpty(cMensajes))
        //            {
        //                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en procesos de días libres.....{0}</b>", cMensajes));
        //                this.Actualiza_Panele_Detalle();
        //                return;
        //            }

        //            //busca contenedores por ruc de usuario
        //            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
        //            string IdAgenteCodigo = string.Empty;
                    
 

        //            /*detalle de exportadores*/
        //            List<BTS_Detalle_Bodegas> ListExportadores = BTS_Detalle_Bodegas.Carga_DetalleBodegas_Exportadores("" , out cMensajes);
        //            if (!String.IsNullOrEmpty(cMensajes))
        //            {

        //                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener detalle de bodega-exportadores....{0}</b>", cMensajes));
        //                this.Actualiza_Panele_Detalle();
        //                return;
        //            }

        //            /*detalle de exportadores*/
        //            List<BTS_Detalle_Muelle> MuelleExportadores = BTS_Detalle_Muelle.Carga_DetalleMuelle_Exportadores("", out cMensajes);
        //            if (!String.IsNullOrEmpty(cMensajes))
        //            {

        //                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener detalle de muelle-exportadores....{0}</b>", cMensajes));
        //                this.Actualiza_Panele_Detalle();
        //                return;
        //            }


        //            var LinqExportadores = (from Tbl in ListExportadores.Where(Tbl => Tbl.cajas != 0)
        //                                     select new
        //                                     {
        //                                         referencia = Tbl.referencia,
        //                                         linea = Tbl.linea ,
        //                                         ruc = Tbl.ruc,
        //                                         exportador = Tbl.Exportador,
        //                                         booking = Tbl.booking,
        //                                         cantidad = Tbl.cajas
        //                                     }).Distinct();

        //            var LinqMuelle = (from Tbl in MuelleExportadores.Where(Tbl => Tbl.cajas != 0)
        //                                    select new
        //                                    {
        //                                        referencia = Tbl.referencia,
        //                                        linea = Tbl.linea,
        //                                        ruc = Tbl.ruc,
        //                                        exportador = Tbl.Exportador,
        //                                        booking = "",
        //                                        cantidad = Tbl.cajas
        //                                    }).Distinct();



        //            if (LinqExportadores != null)
        //            {


        //                //agrego todos los contenedores a la clase cabecera
        //                objCabeceraBTS_OTROS = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as BTS_OTROS_Cabecera_Exportadores;


        //                objCabeceraBTS_OTROS.FECHA = DateTime.Now;
        //                objCabeceraBTS_OTROS.TIPO_CARGA = "BRBK";
        //                objCabeceraBTS_OTROS.REFERENCIA = "" ;
        //                objCabeceraBTS_OTROS.IV_USUARIO_CREA = ClsUsuario.loginname;
        //                objCabeceraBTS_OTROS.SESION = this.hf_BrowserWindowName.Value;
        //                objCabeceraBTS_OTROS.Detalle_Exportador.Clear();
                        

        //                Int16 Secuencia = 1;
        //                foreach (var Det in LinqExportadores)
        //                {
        //                    objDetalleExportador = new BTS_OTROS_Detalle_Exportadores();

        //                    objDetalleExportador.Fila = Secuencia;
        //                    objDetalleExportador.ruc = Det.ruc;
        //                    objDetalleExportador.exportador = Det.exportador;
        //                    objDetalleExportador.cantidad = Det.cantidad;
        //                    objDetalleExportador.linea = Det.linea;
        //                    objDetalleExportador.ruc_asume = Det.ruc;
        //                    objDetalleExportador.exportador_asume = Det.exportador;
        //                    objDetalleExportador.numero_factura = "";
        //                    objDetalleExportador.booking = Det.booking;
        //                    objDetalleExportador.referencia = Det.referencia;
        //                    objCabeceraBTS_OTROS.Detalle_Exportador.Add(objDetalleExportador);

        //                    Secuencia++;
        //                }

                       

        //                foreach (var Det in LinqMuelle)
        //                {
        //                    var existe = objCabeceraBTS_OTROS.Detalle_Exportador.Where(p => p.ruc.Equals(Det.ruc) && p.referencia.Equals(Det.referencia) && p.linea.ToUpper().Equals(Det.linea.ToUpper())).FirstOrDefault();
        //                    if (existe != null)
        //                    {
        //                        existe.cantidad = existe.cantidad + Det.cantidad;
        //                    }
        //                    else
        //                    {
        //                        objDetalleExportador = new BTS_OTROS_Detalle_Exportadores();
        //                        objDetalleExportador.Fila = Secuencia;
        //                        objDetalleExportador.ruc = Det.ruc;
        //                        objDetalleExportador.exportador = Det.exportador;
        //                        objDetalleExportador.cantidad = Det.cantidad;
        //                        objDetalleExportador.linea = Det.linea;
        //                        objDetalleExportador.ruc_asume = Det.ruc;
        //                        objDetalleExportador.exportador_asume = Det.exportador;
        //                        objDetalleExportador.numero_factura = "";
        //                        objDetalleExportador.booking = Det.booking;
        //                        objDetalleExportador.referencia = Det.referencia;
        //                        objCabeceraBTS_OTROS.Detalle_Exportador.Add(objDetalleExportador);

        //                        Secuencia++;
        //                    }

                           
        //                }

        //               tablePagination.DataSource = objCabeceraBTS_OTROS.Detalle_Exportador;
        //                tablePagination.DataBind();

        //                var TotalCajas = objCabeceraBTS_OTROS.Detalle_Exportador.Sum(x => x.cantidad);
        //                objCabeceraBTS_OTROS.TOTAL_CAJA = TotalCajas;

        //                this.LabelTotal.InnerText = string.Format("DETALLE DE EXPORTADORES - Total Cajas: {0}", objCabeceraBTS_OTROS.TOTAL_CAJA);

                       

        //                Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabeceraBTS_OTROS;

        //                this.BtnFacturar.Attributes.Remove("disabled");

                      

        //                this.Actualiza_Panele_Detalle();

                        

        //            }
        //            else
        //            {
        //                tablePagination.DataSource = null;
        //                tablePagination.DataBind();

        //                this.BtnFacturar.Attributes.Add("disabled", "disabled");//


        //                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de exportadores pendiente de facturar con el número de referencia {0} y cliente ingresado..</b>", ""));

        //                this.Actualiza_Panele_Detalle();
        //                return;

        //            }



        //            if (Ocultar_Mensaje)
        //            {
        //                this.Ocultar_Mensaje();
        //            }
                   
        //        }
        //        catch (Exception ex)
        //        {
        //            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

        //        }
        //    }

        //}

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect("~/sav/facturacion_sav.aspx", false);



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

        //proceso de generar facturas individual
        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

                CultureInfo enUS = new CultureInfo("en-US");
           
                string v_mensaje = string.Empty;
                string facturas = string.Empty;
                int Errores = 1;
                int nProcesados = 1;
                Int64 Gkey = 0;
                List<String> Lista = new List<String>();
                Decimal Subtotal = 0;
                Decimal Iva = 0;
                Decimal Total = 0;

                try
                {

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    Int64 ID_DEPOSITO;

                    if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPOSITO))
                    {
                        ID_DEPOSITO = 0;
                    }

                    //facturación sav
                    if (ID_DEPOSITO == 4)
                    {
                        objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;

                        if (objCabecerSAV == null)
                        {
                            this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la factura individual por cada contenedor</b>"));
                            return;
                        }
                        else
                        {
                            LoginName = ClsUsuario.loginname;

                            objCabecerSAV.Detalle_Errores.Clear();
                            objCabecerSAV.Detalle_Ok.Clear();

                            foreach (var Detalle in objCabecerSAV.Detalle_Pases.Where(p => string.IsNullOrEmpty(p.numero_factura) && p.visto && !string.IsNullOrEmpty(p.asume_ruc_facturar)))
                            {

                                Lista.Clear();

                                objCabecerSAV.ID_CLIENTE = Detalle.ruc_cliente;
                                objCabecerSAV.DESC_CLIENTE = Detalle.name_cliente;
                                objCabecerSAV.ID_FACTURADO = Detalle.asume_ruc_facturar;
                                objCabecerSAV.DESC_FACTURADO = Detalle.asume_cliente_facturar;
                                objCabecerSAV.FECHA = DateTime.Now.Date;
                                objCabecerSAV.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objCabecerSAV.TIPO_CARGA = "CNTR";
                                objCabecerSAV.REFERENCIA = Detalle.unidad_referencia;
                                objCabecerSAV.TOTAL_CONTENEDOR = 1;
                                objCabecerSAV.LINEA = Detalle.unidad_linea;
                                objCabecerSAV.BOOKING = Detalle.unidad_booking;
                                objCabecerSAV.CONTENEDORES = Detalle.unidad_id;
                                objCabecerSAV.INVOICE_TYPE = Detalle.invoice_type;
                                objCabecerSAV.RUC_USUARIO = ClsUsuario.ruc;
                                objCabecerSAV.DESC_USUARIO = ClsUsuario.nombres;
                                objCabecerSAV.ID_DEPOSITO = ID_DEPOSITO;
                                /***********************************************************************************************************************************************
                                *datos del cliente N4, días de crédito 
                                ************************************************************************************************************************************************/
                                var Cliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, objCabecerSAV.ID_FACTURADO);
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

                                        objCabecerSAV.DIR_FACTURADO = (ListaCliente.CLNT_ADRESS == null ? "" : ListaCliente.CLNT_ADRESS);
                                        objCabecerSAV.EMAIL_FACTURADO = (ListaCliente.CLNT_EMAIL == null ? "" : ListaCliente.CLNT_EMAIL);
                                        objCabecerSAV.CIUDAD_FACTURADO = string.Empty;
                                        objCabecerSAV.DIAS_CREDITO = (ListaCliente.DIAS_CREDITO == null ? 0 : ListaCliente.DIAS_CREDITO.Value);
                                    }
                                    else
                                    {

                                        objErroresSAV = new SAV_Detalle_Error();
                                        objErroresSAV.contenedor = Detalle.unidad_id;
                                        objErroresSAV.error = string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabecerSAV.ID_FACTURADO);
                                        objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                        objErroresSAV.fila = Errores;
                                        Errores++;
                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabecerSAV.ID_FACTURADO));

                                        return;
                                    }
                                }
                                else
                                {

                                    objErroresSAV = new SAV_Detalle_Error();
                                    objErroresSAV.contenedor = Detalle.unidad_id;
                                    objErroresSAV.error = string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabecerSAV.ID_FACTURADO, objCabecerSAV.DESC_FACTURADO);
                                    objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                    objErroresSAV.fila = Errores;
                                    Errores++;
                                    objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabecerSAV.ID_FACTURADO, objCabecerSAV.DESC_FACTURADO));

                                    return;

                                }

                                //agrego detalle del pase a grabar
                                objCabecerSAV.Detalle_Factura.Clear();
                                objCabecerSAV.Detalle_Servicios.Clear();

                                objDetallePaseSAV = new SAV_Detalle_Pases();
                                objDetallePaseSAV.turno_id = Detalle.turno_id;
                                objDetallePaseSAV.turno_fecha = Detalle.turno_fecha;
                                objDetallePaseSAV.turno_hora = Detalle.turno_hora;
                                objDetallePaseSAV.unidad_id = Detalle.unidad_id;
                                objDetallePaseSAV.unidad_tamano = Detalle.unidad_tamano;
                                objDetallePaseSAV.unidad_linea = Detalle.unidad_linea;
                                objDetallePaseSAV.unidad_booking = Detalle.unidad_booking;
                                objDetallePaseSAV.unidad_referencia = Detalle.unidad_referencia;
                                objDetallePaseSAV.unidad_estatus = Detalle.unidad_estatus;
                                objDetallePaseSAV.unidad_key = Detalle.unidad_key;
                                objDetallePaseSAV.cantidad = 1;
                                objDetallePaseSAV.chofer_licencia = Detalle.chofer_licencia;
                                objDetallePaseSAV.chofer_nombre = Detalle.chofer_nombre;
                                objDetallePaseSAV.vehiculo_placa = Detalle.vehiculo_placa;
                                objDetallePaseSAV.vehiculo_desc = Detalle.vehiculo_desc;
                                objDetallePaseSAV.creado_usuario = Detalle.creado_usuario;
                                objDetallePaseSAV.creado_fecha = Detalle.creado_fecha;
                                objDetallePaseSAV.n4_unit_key = Detalle.n4_unit_key;
                                objDetallePaseSAV.n4_message = Detalle.n4_message;
                                objDetallePaseSAV.active = Detalle.active;
                                objDetallePaseSAV.id = Detalle.id;
                                objDetallePaseSAV.documento_id = Detalle.documento_id;
                                objDetallePaseSAV.estado_pago = Detalle.estado_pago;
                                objDetallePaseSAV.estado = Detalle.estado;
                                objDetallePaseSAV.ruc_cliente = Detalle.ruc_cliente;
                                objDetallePaseSAV.name_cliente = Detalle.name_cliente;
                                objDetallePaseSAV.ruc_asume = Detalle.ruc_asume;
                                objDetallePaseSAV.name_asume = Detalle.name_asume;
                                objDetallePaseSAV.ruc_facturar = Detalle.ruc_facturar;
                                objDetallePaseSAV.cliente_facturar = Detalle.cliente_facturar;
                                objDetallePaseSAV.fila = Detalle.fila;
                                objDetallePaseSAV.asume_ruc_facturar = Detalle.asume_ruc_facturar;
                                objDetallePaseSAV.asume_cliente_facturar = Detalle.asume_cliente_facturar;
                                objDetallePaseSAV.id_facturar = Detalle.id_facturar;
                                objDetallePaseSAV.invoice_type = Detalle.invoice_type;

                                objCabecerSAV.Detalle_Factura.Add(objDetallePaseSAV);


                                /***********************************************************************************************************************************************
                                * Consulta de Servicios a facturar N4 - por cada contenedor
                                **********************************************************************************************************************************************/
                                Subtotal = 0;
                                Iva = 0;
                                Total = 0;
                                Fila = 1;

                                var Validacion = new Aduana.Importacion.ecu_validacion_cntr();
                                var Contenedor = new N4.Importacion.container();
                                var Billing = new N4Ws.Entidad.billing();
                                var Ws = new N4Ws.Entidad.InvoiceRequest();

                                /*saco el invoice type*/
                                string pInvoiceType = string.Empty;
                                pInvoiceType = objCabecerSAV.INVOICE_TYPE == null ? "2DA_MAN_LIN_NAV_CNTRS" : objCabecerSAV.INVOICE_TYPE;

                                Ws.action = N4Ws.Entidad.Action.DRAFT;
                                Ws.requester = N4Ws.Entidad.Requester.QUERY_CHARGES;

                                Ws.InvoiceTypeId = pInvoiceType;
                                Ws.payeeCustomerId = Cliente_Ruc;
                                Ws.payeeCustomerBizRole = Cliente_Rol;

                                var Direccion = new N4Ws.Entidad.address();
                                Direccion.addressLine1 = string.Empty;
                                Direccion.city = "GUAYAQUIL";

                                var Parametro = new N4Ws.Entidad.invoiceParameter();
                                Parametro.EquipmentId = Detalle.unidad_id;
                                Parametro.PaidThruDay = objCabecerSAV.FECHA.Value.ToString("yyyy-MM-dd HH:mm");
                                Parametro.bexuBookingNbr = Detalle.unidad_booking;

                                Ws.invoiceParameters.Add(Parametro);

                                Ws.billToParty.Add(Direccion);
                                //Ws.bexuBlNbr = Numero_Carga;
                                Billing.Request = Ws;

                                var Resultado = Servicios.N4ServicioBasico(Billing, LoginName);
                                if (Resultado != null)
                                {
                                    if (Resultado.status_id.Equals("OK"))
                                    {
                                        var xBilling = Resultado;

                                        FechaPaidThruDay = null;
                                        CargabexuBlNbr = null;
                                        Fila = 1;

                                        draftNumber = xBilling.response.billInvoice.draftNumber;

                                        objCabecerSAV.DRAF = draftNumber;

                                        if (!Int64.TryParse(draftNumber, out draftNumberFinal))
                                        {

                                            objErroresSAV = new SAV_Detalle_Error();
                                            objErroresSAV.contenedor = Detalle.unidad_id;
                                            objErroresSAV.error = string.Format("<b>Error! </b>No se puede convertir en campo numérico el draft # : {0}", draftNumber);
                                            objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                            objErroresSAV.fila = Errores;
                                            Errores++;
                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                            this.Mostrar_Mensaje(4, string.Format("<b>Error! </b>No se puede convertir en campo numérico el draft # : {0}", draftNumber));
                                            return;
                                        }

                                        Lista.Add(draftNumber.Trim());

                                        if (!Int64.TryParse(xBilling.response.billInvoice.gkey, out Gkey))
                                        {
                                            objErroresSAV = new SAV_Detalle_Error();
                                            objErroresSAV.contenedor = Detalle.unidad_id;
                                            objErroresSAV.error = string.Format("<i class='fa fa-warning'></i><b> Error! No se puede convertir en campo numerico el gkey: {0}</b>", xBilling.response.billInvoice.gkey);
                                            objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                            objErroresSAV.fila = Errores;
                                            Errores++;
                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                            this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! No se puede convertir en campo numerico el gkey: {0}</b>", xBilling.response.billInvoice.gkey));
                                            return;
                                        }


                                        TipoServicio = xBilling.response.billInvoice.type;

                                        FechaPaidThruDay = (from bexuPaidThruDay in xBilling.response.billInvoice.invoiceParameters.Where(p => p.Metafield == "bexuPaidThruDay")
                                                            select new
                                                            {
                                                                fecha = bexuPaidThruDay.Value.ToString()
                                                            }
                                                   ).FirstOrDefault().fecha;

                                        var pCargabexuBlNbr = xBilling.response.billInvoice.invoiceParameters.Where(p => p.Metafield == "bexuBookingNbr").FirstOrDefault();

                                        if (pCargabexuBlNbr != null)
                                        {
                                            CargabexuBlNbr = (from bexuBlNbr in xBilling.response.billInvoice.invoiceParameters.Where(p => p.Metafield == "bexuBookingNbr")
                                                              select new
                                                              {
                                                                  carga = bexuBlNbr.Value == null ? "" : bexuBlNbr.Value
                                                              }).FirstOrDefault().carga;
                                        }
                                        else
                                        {
                                            CargabexuBlNbr = Detalle.unidad_booking;
                                        }



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
                                            objServiciosSAV = new SAV_Detalle_Servicios();
                                            objServiciosSAV.ID = 0;
                                            objServiciosSAV.LINEA = Fila;
                                            objServiciosSAV.ID_SERVICIO = Det.CODIGO;
                                            objServiciosSAV.DESC_SERVICIO = Det.SERVICIO;
                                            objServiciosSAV.CARGA = Det.CARGA;
                                            objServiciosSAV.FECHA = DateTime.Parse(Det.FECHA.ToString());
                                            objServiciosSAV.TIPO_SERVICIO = TipoServicio;
                                            objServiciosSAV.CANTIDAD = Det.CANTIDAD;
                                            objServiciosSAV.PRECIO = Det.PRECIO;
                                            objServiciosSAV.SUBTOTAL = Det.TOTAL;
                                            objServiciosSAV.IVA = Det.IVA;
                                            objServiciosSAV.IV_USUARIO_CREA = LoginName;
                                            objServiciosSAV.IV_FECHA_CREA = DateTime.Now;
                                            objServiciosSAV.DRAFT = draftNumber;
                                            Fila++;
                                            objCabecerSAV.Detalle_Servicios.Add(objServiciosSAV);

                                        }

                                        Iva = Iva + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTaxes != null ? xBilling.response.billInvoice.totalTaxes : "0", enUS), 2);
                                        Total = Total + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTotal != null ? xBilling.response.billInvoice.totalTotal : "0", enUS), 2);

                                        var LinqSubtotal = (from Servicios in objCabecerSAV.Detalle_Servicios.AsEnumerable()
                                                            select Servicios.SUBTOTAL
                                                      ).Sum();

                                        Subtotal = LinqSubtotal;


                                        objCabecerSAV.SUBTOTAL = Subtotal;
                                        objCabecerSAV.IVA = Iva;
                                        objCabecerSAV.TOTAL = Total;

                                        /**********************************************************************************************************************************/
                                        /*proceso finalizar draft de factura*/
                                        var BillingFin = new N4Ws.Entidad.billing();
                                        MergeInvoiceRequest Fin = new MergeInvoiceRequest();
                                        Fin.finalizeDate = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm");// FechaPaidThruDay;
                                        Fin.drftInvoiceNbrs = Lista;
                                        Fin.invoiceTypeId = pInvoiceType;
                                        BillingFin.MergeInvoiceRequest = Fin;

                                        var Finalizar = Servicios.N4ServicioBasicoMergeAndFinalizeTransaction(BillingFin, ClsUsuario.loginname.Trim());
                                        if (Finalizar != null)
                                        {
                                            if (Finalizar.status_id.Equals("OK"))
                                            {
                                                var Factura = Finalizar;
                                                NumeroFactura = Factura.response.billInvoice.finalNumber;

                                                //draftNumber

                                                objCabecerSAV.IVA = Decimal.Round(Decimal.Parse(Factura.response.billInvoice.totalTaxes != null ? Factura.response.billInvoice.totalTaxes : "0", enUS), 2); ;
                                                objCabecerSAV.TOTAL = Decimal.Round(Decimal.Parse(Factura.response.billInvoice.totalTotal != null ? Factura.response.billInvoice.totalTotal : "0", enUS), 2);
                                                objCabecerSAV.SUBTOTAL = Decimal.Round(Decimal.Parse(Factura.response.billInvoice.totalCharges != null ? Factura.response.billInvoice.totalCharges : "0", enUS), 2);

                                                NumeroFactura = "00" + NumeroFactura;
                                                string Establecimiento = NumeroFactura.Substring(0, 3);
                                                string PuntoEmision = NumeroFactura.Substring(3, 3);
                                                string Original = NumeroFactura.Substring(6, 9);
                                                string FacturaFinal = string.Format("{0}-{1}-{2}", Establecimiento, PuntoEmision, Original);

                                                objCabecerSAV.NUMERO_FACTURA = FacturaFinal;

                                                //AGREGO AL LOG DE PROCESADOS
                                                objProcesadosSAV = new SAV_Detalle_Ok();
                                                objProcesadosSAV.contenedor = Detalle.unidad_id;
                                                objProcesadosSAV.mensaje = string.Format("Factura generada # {0} ==> OK", FacturaFinal);
                                                objProcesadosSAV.cliente = Detalle.asume_cliente_facturar;
                                                objProcesadosSAV.fila = nProcesados;
                                                objCabecerSAV.Detalle_Ok.Add(objProcesadosSAV);
                                                nProcesados++;

                                                //PROCESO DE GRABAR

                                                Detalle.numero_factura = FacturaFinal;

                                                Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                                                /*nuevo proceso de grabado*/
                                                System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                new System.Xml.Linq.XElement("FACT_CABECERA",
                                                                                         new System.Xml.Linq.XElement("CABECERA",
                                                                                        new System.Xml.Linq.XAttribute("GLOSA", objCabecerSAV.GLOSA == null ? "" : objCabecerSAV.GLOSA),
                                                                                        new System.Xml.Linq.XAttribute("FECHA", objCabecerSAV.FECHA == null ? DateTime.Parse("1900/01/01") : objCabecerSAV.FECHA),
                                                                                        new System.Xml.Linq.XAttribute("ID_CLIENTE", objCabecerSAV.ID_CLIENTE == null ? "" : objCabecerSAV.ID_CLIENTE),
                                                                                        new System.Xml.Linq.XAttribute("DESC_CLIENTE", objCabecerSAV.DESC_CLIENTE == null ? "" : objCabecerSAV.DESC_CLIENTE),
                                                                                        new System.Xml.Linq.XAttribute("ID_FACTURADO", objCabecerSAV.ID_FACTURADO == null ? "" : objCabecerSAV.ID_FACTURADO),
                                                                                        new System.Xml.Linq.XAttribute("DESC_FACTURADO", objCabecerSAV.DESC_FACTURADO == null ? "" : objCabecerSAV.DESC_FACTURADO),
                                                                                        new System.Xml.Linq.XAttribute("SUBTOTAL", objCabecerSAV.SUBTOTAL),
                                                                                        new System.Xml.Linq.XAttribute("IVA", objCabecerSAV.IVA),
                                                                                        new System.Xml.Linq.XAttribute("TOTAL", objCabecerSAV.TOTAL),
                                                                                        new System.Xml.Linq.XAttribute("DRAF", objCabecerSAV.DRAF == null ? "" : objCabecerSAV.DRAF),
                                                                                        new System.Xml.Linq.XAttribute("REFERENCIA", objCabecerSAV.REFERENCIA == null ? "" : objCabecerSAV.REFERENCIA),
                                                                                        new System.Xml.Linq.XAttribute("DIR_FACTURADO", objCabecerSAV.DIR_FACTURADO == null ? "" : objCabecerSAV.DIR_FACTURADO),
                                                                                        new System.Xml.Linq.XAttribute("EMAIL_FACTURADO", objCabecerSAV.EMAIL_FACTURADO == null ? "" : objCabecerSAV.EMAIL_FACTURADO),
                                                                                        new System.Xml.Linq.XAttribute("CIUDAD_FACTURADO", objCabecerSAV.CIUDAD_FACTURADO == null ? "" : objCabecerSAV.CIUDAD_FACTURADO),
                                                                                        new System.Xml.Linq.XAttribute("DIAS_CREDITO", objCabecerSAV.DIAS_CREDITO),
                                                                                        new System.Xml.Linq.XAttribute("USUARIO_CREA", objCabecerSAV.IV_USUARIO_CREA == null ? "" : objCabecerSAV.IV_USUARIO_CREA),
                                                                                        new System.Xml.Linq.XAttribute("TOTAL_CONTENEDORES", objCabecerSAV.TOTAL_CONTENEDOR),
                                                                                        new System.Xml.Linq.XAttribute("CONTENEDORES", objCabecerSAV.CONTENEDORES == null ? "" : objCabecerSAV.CONTENEDORES),
                                                                                        new System.Xml.Linq.XAttribute("RUC_USUARIO", objCabecerSAV.RUC_USUARIO == null ? "" : objCabecerSAV.RUC_USUARIO),
                                                                                        new System.Xml.Linq.XAttribute("DESC_USUARIO", objCabecerSAV.DESC_USUARIO == null ? "" : objCabecerSAV.DESC_USUARIO),
                                                                                        new System.Xml.Linq.XAttribute("NUMERO_FACTURA", objCabecerSAV.NUMERO_FACTURA == null ? "" : objCabecerSAV.NUMERO_FACTURA),
                                                                                        new System.Xml.Linq.XAttribute("LINEA", objCabecerSAV.LINEA == null ? "" : objCabecerSAV.LINEA),
                                                                                        new System.Xml.Linq.XAttribute("BOOKING", objCabecerSAV.BOOKING == null ? "" : objCabecerSAV.BOOKING),
                                                                                        new System.Xml.Linq.XAttribute("ID_DEPOSITO", objCabecerSAV.ID_DEPOSITO),
                                                                                        new System.Xml.Linq.XAttribute("flag", "I"))));


                                                System.Xml.Linq.XDocument XMLServicios = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                new System.Xml.Linq.XElement("FACT_SERVICIOS", from p in objCabecerSAV.Detalle_Servicios.AsEnumerable().AsParallel()
                                                                                               select new System.Xml.Linq.XElement("DETALLE",
                                                                                              new System.Xml.Linq.XAttribute("ID_SERVICIO", p.ID_SERVICIO == null ? "" : p.ID_SERVICIO.ToString().Trim()),
                                                                                              new System.Xml.Linq.XAttribute("DESC_SERVICIO", p.DESC_SERVICIO == null ? "" : p.DESC_SERVICIO.ToString().Trim()),
                                                                                              new System.Xml.Linq.XAttribute("CARGA", p.CARGA == null ? "" : p.CARGA.ToString().Trim()),
                                                                                              new System.Xml.Linq.XAttribute("FECHA", p.FECHA == null ? DateTime.Parse("1900/01/01") : p.FECHA),
                                                                                              new System.Xml.Linq.XAttribute("TIPO_SERVICIO", p.TIPO_SERVICIO),
                                                                                              new System.Xml.Linq.XAttribute("CANTIDAD", p.CANTIDAD),
                                                                                              new System.Xml.Linq.XAttribute("PRECIO", p.PRECIO),
                                                                                              new System.Xml.Linq.XAttribute("SUBTOTAL", p.SUBTOTAL),
                                                                                              new System.Xml.Linq.XAttribute("IVA", p.IVA),
                                                                                              new System.Xml.Linq.XAttribute("DRAFT", p.DRAFT == null ? "" : p.DRAFT.ToString().Trim()),
                                                                                              new System.Xml.Linq.XAttribute("USUARIO_CREA", p.IV_USUARIO_CREA),
                                                                                              new System.Xml.Linq.XAttribute("flag", "I"))));


                                                System.Xml.Linq.XDocument XMLPasePuerta = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                new System.Xml.Linq.XElement("FACT_PASES", from p in objCabecerSAV.Detalle_Factura.AsEnumerable().AsParallel()
                                                                                           select new System.Xml.Linq.XElement("DETALLE",
                                                                                            new System.Xml.Linq.XAttribute("turno_id", p.turno_id),
                                                                                            new System.Xml.Linq.XAttribute("turno_fecha", p.turno_fecha == null ? DateTime.Parse("1900/01/01") : p.turno_fecha),
                                                                                            new System.Xml.Linq.XAttribute("turno_hora", p.turno_hora == null ? "" : p.turno_hora.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("unidad_id", p.unidad_id == null ? "" : p.unidad_id.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("unidad_tamano", p.unidad_tamano == null ? "" : p.unidad_tamano.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("unidad_linea", p.unidad_linea == null ? "" : p.unidad_linea.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("unidad_booking", p.unidad_booking == null ? "" : p.unidad_booking.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("unidad_referencia", p.unidad_referencia == null ? "" : p.unidad_referencia.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("unidad_estatus", p.unidad_estatus == null ? "" : p.unidad_estatus.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("unidad_key", p.unidad_key),
                                                                                            new System.Xml.Linq.XAttribute("documento_id", p.documento_id == null ? "" : p.documento_id.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("estado_pago", p.estado_pago == null ? "" : p.estado_pago.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("ruc_cliente", p.ruc_cliente == null ? "" : p.ruc_cliente.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("name_cliente", p.name_cliente == null ? "" : p.name_cliente.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("asume_ruc_facturar", p.asume_ruc_facturar == null ? "" : p.asume_ruc_facturar.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("asume_cliente_facturar", p.asume_cliente_facturar == null ? "" : p.asume_cliente_facturar.ToString().Trim()),
                                                                                            new System.Xml.Linq.XAttribute("id_facturar", p.id_facturar),
                                                                                            new System.Xml.Linq.XAttribute("flag", "I"))));


                                                string cMensajeActualizados = string.Empty;

                                                objCabecerSAV.xmlCabecera = XMLCabecera.ToString();
                                                objCabecerSAV.xmlServicios = XMLServicios.ToString();
                                                objCabecerSAV.xmlPases = XMLPasePuerta.ToString();

                                                var nProceso = objCabecerSAV.SaveTransaction(out cMensajes);

                                                /*fin de nuevo proceso de grabado*/
                                                if (!nProceso.HasValue || nProceso.Value <= 0)
                                                {

                                                    objErroresSAV = new SAV_Detalle_Error();
                                                    objErroresSAV.contenedor = Detalle.unidad_id;
                                                    objErroresSAV.error = string.Format("<b>Error! No se pudo grabar datos de la factura..{0}</b>", cMensajes);
                                                    objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                                    objErroresSAV.fila = Errores;
                                                    Errores++;

                                                    objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                                    break;
                                                }
                                                else
                                                {

                                                    //actualizar numero de factura en tablas de preavisos
                                                    objActualizaSAV = new SAV_Asigna_Factura();
                                                    objActualizaSAV.id = Detalle.id;
                                                    objActualizaSAV.numero_factura = FacturaFinal;

                                                    var nActualiza = objActualizaSAV.SaveTransaction(out cMensajes);

                                                    if (!nActualiza.HasValue || nActualiza.Value <= 0)
                                                    {
                                                        objErroresSAV = new SAV_Detalle_Error();
                                                        objErroresSAV.contenedor = Detalle.unidad_id;
                                                        objErroresSAV.error = string.Format("<b>Error! No se pudo actualizar datos de la factura en el previaso # ..{0}- {1}</b>", Detalle.id, cMensajes);
                                                        objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                                        objErroresSAV.fila = Errores;
                                                        Errores++;

                                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                                    }

                                                }

                                            }
                                            else
                                            {

                                                objErroresSAV = new SAV_Detalle_Error();
                                                objErroresSAV.contenedor = Detalle.unidad_id;
                                                objErroresSAV.error = string.Format("<b>Error! </b>Lo sentimos, algo salió mal. {0}", Finalizar.messages.ToString());
                                                objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                                objErroresSAV.fila = Errores;
                                                Errores++;

                                                objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                            }
                                        }
                                        else
                                        {
                                            objErroresSAV = new SAV_Detalle_Error();
                                            objErroresSAV.contenedor = Detalle.unidad_id;
                                            objErroresSAV.error = string.Format("<b>Error! </b>Lo sentimos, algo salió mal. {0}", Finalizar.messages.ToString());
                                            objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                            objErroresSAV.fila = Errores;
                                            Errores++;

                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                        }


                                        Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                                    }
                                    else
                                    {
                                        //var Msg = Resultado.messages.Where(p => p.message_detail.Contains("ERROR")).FirstOrDefault().message_detail.ToString();

                                        var Msg = (from A in Resultado.messages.Where(p => p.message_detail.Contains("ERROR"))
                                                   select A.message_detail).FirstOrDefault();

                                        if (Msg != null)
                                        {
                                            objErroresSAV = new SAV_Detalle_Error();
                                            objErroresSAV.contenedor = Detalle.unidad_id;
                                            objErroresSAV.error = Msg;
                                            objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                            objErroresSAV.fila = Errores;
                                            Errores++;

                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                        }



                                    }
                                }
                                else
                                {
                                    var Msg = (from A in Resultado.messages.Where(p => p.message_detail.Contains("ERROR"))
                                               select A.message_detail).FirstOrDefault();

                                    if (Msg != null)
                                    {
                                        objErroresSAV = new SAV_Detalle_Error();
                                        objErroresSAV.contenedor = Detalle.unidad_id;
                                        objErroresSAV.error = Msg;
                                        objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                        objErroresSAV.fila = Errores;
                                        Errores++;

                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                    }
                                }

                            }


                            Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                            objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;

                            tablePagination.DataSource = objCabecerSAV.Detalle_Pases;
                            tablePagination.DataBind();

                            string Mensajes = string.Empty;

                            //cargar datos de errores
                            if (objCabecerSAV.Detalle_Errores.Count != 0)
                            {
                                Mensajes = string.Format("<i class='fa fa-warning'></i><b> Informativo! Se presentaron {0} errores al generar facturas, los mismo puede visualizar dando click en el botón detalle errores......</b><br/>", objCabecerSAV.Detalle_Errores.Count);

                                this.BtnErrores.Attributes.Remove("disabled");

                                PaginationErrores.DataSource = objCabecerSAV.Detalle_Errores;
                                PaginationErrores.DataBind();

                            }

                            if (objCabecerSAV.Detalle_Ok.Count != 0)
                            {
                                Mensajes = string.Format("{0} {1}", Mensajes, string.Format("<i class='fa fa-warning'></i><b> Se procesaron {0} facturas con éxito..</b>", objCabecerSAV.Detalle_Ok.Count));
                            }

                            if (string.IsNullOrEmpty(Mensajes))
                            {
                                Mensajes = string.Format("{0} {1}", Mensajes, string.Format("<i class='fa fa-warning'></i><b> No existen pases de puerta pendiente para facturar..</b>"));
                            }

                            this.Mostrar_Mensaje(4, Mensajes);

                            this.OcultarLoading("1");
                            this.OcultarLoading("2");

                            this.Actualiza_Paneles();



                        }

                    }
                    else 
                    {
                        //facturación: REPCONTVER
                        objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;

                        if (objCabecerSAV == null)
                        {
                            this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la factura individual por cada contenedor (REPCONTVER)</b>"));
                            return;
                        }
                        else
                        {

                          

                           

                            LoginName = ClsUsuario.loginname;

                            objCabecerSAV.Detalle_Errores.Clear();
                            objCabecerSAV.Detalle_Ok.Clear();

                            foreach (var Detalle in objCabecerSAV.Detalle_Pases.Where(p => string.IsNullOrEmpty(p.numero_factura) && p.visto && !string.IsNullOrEmpty(p.asume_ruc_facturar)))
                            {

                                //saco los rubros a facturar
                                var rubros = SAV_Servicios_Repcontver.Carga_DetalleRubros_Otros(Detalle.unidad_linea,out v_mensaje);
                                if (!String.IsNullOrEmpty(v_mensaje))
                                {

                                    this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener rubros de facturación de REPCONTVER....{0}</b>", cMensajes));
                                    return;
                                }

                                if (rubros.Count == 0)
                                {
                                    this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen rubros asociados a REPCONTVER para poder facturar.....</b>"));
                                    return;
                                }

                                var list = new List<Tuple<string, string>>();

                                foreach (var item in rubros)
                                {
                                    int cantidad = (int)item.CANTIDAD;

                                    list.Add(Tuple.Create(item.CODIGO_TARIJA_N4, string.Format("{0},{1},{2}", item.VALUE_TARIJA_N4, cantidad, (string.IsNullOrEmpty(Detalle.unidad_id) ? Detalle.unidad_booking  : Detalle.unidad_id))));
                                }

                                Lista.Clear();

                                objCabecerSAV.ID_CLIENTE = Detalle.ruc_cliente;
                                objCabecerSAV.DESC_CLIENTE = Detalle.name_cliente;
                                objCabecerSAV.ID_FACTURADO = Detalle.asume_ruc_facturar;
                                objCabecerSAV.DESC_FACTURADO = Detalle.asume_cliente_facturar;
                                objCabecerSAV.FECHA = DateTime.Now.Date;
                                objCabecerSAV.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objCabecerSAV.TIPO_CARGA = "CNTR";
                                objCabecerSAV.REFERENCIA = Detalle.unidad_referencia;
                                objCabecerSAV.TOTAL_CONTENEDOR = 1;
                                objCabecerSAV.LINEA = Detalle.unidad_linea;
                                objCabecerSAV.BOOKING = Detalle.unidad_booking;
                                objCabecerSAV.CONTENEDORES = Detalle.unidad_id;
                                objCabecerSAV.INVOICE_TYPE = Detalle.invoice_type;
                                objCabecerSAV.RUC_USUARIO = ClsUsuario.ruc;
                                objCabecerSAV.DESC_USUARIO = ClsUsuario.nombres;
                                objCabecerSAV.ID_DEPOSITO = ID_DEPOSITO;

                                /***********************************************************************************************************************************************
                                *datos del cliente N4, días de crédito 
                                ************************************************************************************************************************************************/
                                var Cliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, objCabecerSAV.ID_FACTURADO);
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

                                        objCabecerSAV.DIR_FACTURADO = (ListaCliente.CLNT_ADRESS == null ? "" : ListaCliente.CLNT_ADRESS);
                                        objCabecerSAV.EMAIL_FACTURADO = (ListaCliente.CLNT_EMAIL == null ? "" : ListaCliente.CLNT_EMAIL);
                                        objCabecerSAV.CIUDAD_FACTURADO = string.Empty;
                                        objCabecerSAV.DIAS_CREDITO = (ListaCliente.DIAS_CREDITO == null ? 0 : ListaCliente.DIAS_CREDITO.Value);
                                    }
                                    else
                                    {

                                        objErroresSAV = new SAV_Detalle_Error();
                                        objErroresSAV.contenedor = Detalle.unidad_id;
                                        objErroresSAV.error = string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0} ..(REPCONTVER)</b>", objCabecerSAV.ID_FACTURADO);
                                        objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                        objErroresSAV.fila = Errores;
                                        Errores++;
                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0} ..(REPCONTVER)</b>", objCabecerSAV.ID_FACTURADO));

                                        return;
                                    }
                                }
                                else
                                {

                                    objErroresSAV = new SAV_Detalle_Error();
                                    objErroresSAV.contenedor = Detalle.unidad_id;
                                    objErroresSAV.error = string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabecerSAV.ID_FACTURADO, objCabecerSAV.DESC_FACTURADO);
                                    objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                    objErroresSAV.fila = Errores;
                                    Errores++;
                                    objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabecerSAV.ID_FACTURADO, objCabecerSAV.DESC_FACTURADO));

                                    return;

                                }

                                //agrego detalle del pase a grabar
                                objCabecerSAV.Detalle_Factura.Clear();
                                objCabecerSAV.Detalle_Servicios.Clear();

                                objDetallePaseSAV = new SAV_Detalle_Pases();
                                objDetallePaseSAV.turno_id = Detalle.turno_id;
                                objDetallePaseSAV.turno_fecha = Detalle.turno_fecha;
                                objDetallePaseSAV.turno_hora = Detalle.turno_hora;
                                objDetallePaseSAV.unidad_id = Detalle.unidad_id;
                                objDetallePaseSAV.unidad_tamano = Detalle.unidad_tamano;
                                objDetallePaseSAV.unidad_linea = Detalle.unidad_linea;
                                objDetallePaseSAV.unidad_booking = Detalle.unidad_booking;
                                objDetallePaseSAV.unidad_referencia = Detalle.unidad_referencia;
                                objDetallePaseSAV.unidad_estatus = Detalle.unidad_estatus;
                                objDetallePaseSAV.unidad_key = Detalle.unidad_key;
                                objDetallePaseSAV.cantidad = 1;
                                objDetallePaseSAV.chofer_licencia = Detalle.chofer_licencia;
                                objDetallePaseSAV.chofer_nombre = Detalle.chofer_nombre;
                                objDetallePaseSAV.vehiculo_placa = Detalle.vehiculo_placa;
                                objDetallePaseSAV.vehiculo_desc = Detalle.vehiculo_desc;
                                objDetallePaseSAV.creado_usuario = Detalle.creado_usuario;
                                objDetallePaseSAV.creado_fecha = Detalle.creado_fecha;
                                objDetallePaseSAV.n4_unit_key = Detalle.n4_unit_key;
                                objDetallePaseSAV.n4_message = Detalle.n4_message;
                                objDetallePaseSAV.active = Detalle.active;
                                objDetallePaseSAV.id = Detalle.id;
                                objDetallePaseSAV.documento_id = Detalle.documento_id;
                                objDetallePaseSAV.estado_pago = Detalle.estado_pago;
                                objDetallePaseSAV.estado = Detalle.estado;
                                objDetallePaseSAV.ruc_cliente = Detalle.ruc_cliente;
                                objDetallePaseSAV.name_cliente = Detalle.name_cliente;
                                objDetallePaseSAV.ruc_asume = Detalle.ruc_asume;
                                objDetallePaseSAV.name_asume = Detalle.name_asume;
                                objDetallePaseSAV.ruc_facturar = Detalle.ruc_facturar;
                                objDetallePaseSAV.cliente_facturar = Detalle.cliente_facturar;
                                objDetallePaseSAV.fila = Detalle.fila;
                                objDetallePaseSAV.asume_ruc_facturar = Detalle.asume_ruc_facturar;
                                objDetallePaseSAV.asume_cliente_facturar = Detalle.asume_cliente_facturar;
                                objDetallePaseSAV.id_facturar = Detalle.id_facturar;
                                objDetallePaseSAV.invoice_type = Detalle.invoice_type;

                                objCabecerSAV.Detalle_Factura.Add(objDetallePaseSAV);


                                /***********************************************************************************************************************************************
                                * Consulta de Servicios a facturar N4 - por cada pase de puerta
                                **********************************************************************************************************************************************/
                                Subtotal = 0;
                                Iva = 0;
                                Total = 0;

                                /*ultima factura en caso de tener*/
                                var LinqInvoiceType = (from Tbl in rubros.Where(Tbl => Tbl.CANTIDAD != 0)
                                                       select new
                                                       {
                                                           INVOICETYPE = Tbl.INVOICETYPE,
                                                           CODIGO_TARIJA_N4 = Tbl.CODIGO_TARIJA_N4,
                                                           VALUE_TARIJA_N4 = Tbl.VALUE_TARIJA_N4,
                                                       }).FirstOrDefault();

                                string request = string.Empty;
                                string response = string.Empty;
                                Respuesta.ResultadoOperacion<bool> resp;
                                resp = ServicioSCA.CargarServicioBTS(LinqInvoiceType.INVOICETYPE, Cliente_Ruc, string.Format("{0}/{1}", Detalle.unidad_id, Detalle.unidad_booking), list, Page.User.Identity.Name.ToUpper(), out request, out response);
                                if (resp.Exitoso)
                                {

                                    var v_result = resp.MensajeInformacion.Split(',');
                                    decimal v_monto = 0;
                                    v_monto = decimal.Parse(v_result[1].ToString());
                                    string Draf = v_result[0];


                                    objCabecerSAV.DRAF = Draf;

                                    /*detalle de muelles*/
                                    List<BTS_OTROS_Detalle_Rubros_Factura> ListServicios = BTS_OTROS_Detalle_Rubros_Factura.Carga_Servicios_Draf(Draf, out cMensajes);
                                    if (!String.IsNullOrEmpty(cMensajes))
                                    {

                                        objErroresSAV = new SAV_Detalle_Error();
                                        objErroresSAV.contenedor = Detalle.unidad_id;
                                        objErroresSAV.error = string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener servicios de facturación del draf....{0}/ Error:{1}..(REPCONTVER)</b>", Draf, cMensajes);
                                        objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                        objErroresSAV.fila = Errores;
                                        Errores++;
                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                        this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener servicios de facturación del draf....{0}/ Error:{1}..(REPCONTVER)</b>", Draf, cMensajes));
                                        this.Actualiza_Panele_Detalle();
                                        return;
                                    }

                                    foreach (var Det in ListServicios)
                                    {
                                        objServiciosSAV = new SAV_Detalle_Servicios();
                                        objServiciosSAV.ID = 0;
                                        objServiciosSAV.LINEA = Fila;
                                        objServiciosSAV.ID_SERVICIO = Draf;
                                        objServiciosSAV.DESC_SERVICIO = Det.description;
                                        objServiciosSAV.CARGA = string.Format("{0}/{1}", Detalle.unidad_id, Detalle.unidad_booking);
                                        objServiciosSAV.FECHA = Det.created;
                                        objServiciosSAV.TIPO_SERVICIO = LinqInvoiceType.INVOICETYPE;
                                        objServiciosSAV.CANTIDAD = Det.quantity;
                                        objServiciosSAV.PRECIO = Det.amount;
                                        objServiciosSAV.SUBTOTAL = Det.amount;
                                        objServiciosSAV.IVA = Det.amount_taxt;
                                        objServiciosSAV.IV_USUARIO_CREA = LoginName;
                                        objServiciosSAV.IV_FECHA_CREA = DateTime.Now;
                                        objServiciosSAV.DRAFT = Draf;
                                        Fila++;
                                        objCabecerSAV.Detalle_Servicios.Add(objServiciosSAV);

                                    }

                                    Subtotal = Decimal.Round(ListServicios.Sum(p => p.amount), 2);
                                    Iva = Decimal.Round(ListServicios.Sum(p => p.amount_taxt), 2);
                                    Total = Subtotal + Iva;

                                    objCabecerSAV.SUBTOTAL = Subtotal;
                                    objCabecerSAV.IVA = Iva;
                                    objCabecerSAV.TOTAL = Total;


                                    /*proceso finalizar draft de factura*/
                                    List<String> ListaDraft = new List<String>();
                                    ListaDraft.Add(Draf);

                                    var BillingFin = new N4Ws.Entidad.billing();
                                    MergeInvoiceRequest Fin = new MergeInvoiceRequest();
                                    Fin.finalizeDate = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm");// FechaPaidThruDay;
                                    Fin.drftInvoiceNbrs = ListaDraft;
                                    Fin.invoiceTypeId = LinqInvoiceType.INVOICETYPE;
                                    BillingFin.MergeInvoiceRequest = Fin;

                                    var Finalizar = Servicios.N4ServicioBasicoMergeAndFinalizeTransaction(BillingFin, ClsUsuario.loginname.Trim());
                                    if (Finalizar != null)
                                    {

                                        var Factura = Finalizar;
                                        string NumeroFactura = Factura.response.billInvoice.finalNumber;
                                        NumeroFactura = "00" + NumeroFactura;
                                        string Establecimiento = NumeroFactura.Substring(0, 3);
                                        string PuntoEmision = NumeroFactura.Substring(3, 3);
                                        string Original = NumeroFactura.Substring(6, 9);
                                        string FacturaFinal = string.Format("{0}-{1}-{2}", Establecimiento, PuntoEmision, Original);

                                        objCabecerSAV.NUMERO_FACTURA = FacturaFinal;

                                        //AGREGO AL LOG DE PROCESADOS
                                        objProcesadosSAV = new SAV_Detalle_Ok();
                                        objProcesadosSAV.contenedor = Detalle.unidad_id;
                                        objProcesadosSAV.mensaje = string.Format("Factura generada # {0} ==> OK", FacturaFinal);
                                        objProcesadosSAV.cliente = Detalle.asume_cliente_facturar;
                                        objProcesadosSAV.fila = nProcesados;
                                        objCabecerSAV.Detalle_Ok.Add(objProcesadosSAV);
                                        nProcesados++;

                                        //actualizo numero de factura
                                        Detalle.numero_factura = FacturaFinal;

                                        Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                                        /*nuevo proceso de grabado*/
                                        System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                        new System.Xml.Linq.XElement("FACT_CABECERA",
                                                                                 new System.Xml.Linq.XElement("CABECERA",
                                                                                new System.Xml.Linq.XAttribute("GLOSA", objCabecerSAV.GLOSA == null ? "" : objCabecerSAV.GLOSA),
                                                                                new System.Xml.Linq.XAttribute("FECHA", objCabecerSAV.FECHA == null ? DateTime.Parse("1900/01/01") : objCabecerSAV.FECHA),
                                                                                new System.Xml.Linq.XAttribute("ID_CLIENTE", objCabecerSAV.ID_CLIENTE == null ? "" : objCabecerSAV.ID_CLIENTE),
                                                                                new System.Xml.Linq.XAttribute("DESC_CLIENTE", objCabecerSAV.DESC_CLIENTE == null ? "" : objCabecerSAV.DESC_CLIENTE),
                                                                                new System.Xml.Linq.XAttribute("ID_FACTURADO", objCabecerSAV.ID_FACTURADO == null ? "" : objCabecerSAV.ID_FACTURADO),
                                                                                new System.Xml.Linq.XAttribute("DESC_FACTURADO", objCabecerSAV.DESC_FACTURADO == null ? "" : objCabecerSAV.DESC_FACTURADO),
                                                                                new System.Xml.Linq.XAttribute("SUBTOTAL", objCabecerSAV.SUBTOTAL),
                                                                                new System.Xml.Linq.XAttribute("IVA", objCabecerSAV.IVA),
                                                                                new System.Xml.Linq.XAttribute("TOTAL", objCabecerSAV.TOTAL),
                                                                                new System.Xml.Linq.XAttribute("DRAF", objCabecerSAV.DRAF == null ? "" : objCabecerSAV.DRAF),
                                                                                new System.Xml.Linq.XAttribute("REFERENCIA", objCabecerSAV.REFERENCIA == null ? "" : objCabecerSAV.REFERENCIA),
                                                                                new System.Xml.Linq.XAttribute("DIR_FACTURADO", objCabecerSAV.DIR_FACTURADO == null ? "" : objCabecerSAV.DIR_FACTURADO),
                                                                                new System.Xml.Linq.XAttribute("EMAIL_FACTURADO", objCabecerSAV.EMAIL_FACTURADO == null ? "" : objCabecerSAV.EMAIL_FACTURADO),
                                                                                new System.Xml.Linq.XAttribute("CIUDAD_FACTURADO", objCabecerSAV.CIUDAD_FACTURADO == null ? "" : objCabecerSAV.CIUDAD_FACTURADO),
                                                                                new System.Xml.Linq.XAttribute("DIAS_CREDITO", objCabecerSAV.DIAS_CREDITO),
                                                                                new System.Xml.Linq.XAttribute("USUARIO_CREA", objCabecerSAV.IV_USUARIO_CREA == null ? "" : objCabecerSAV.IV_USUARIO_CREA),
                                                                                new System.Xml.Linq.XAttribute("TOTAL_CONTENEDORES", objCabecerSAV.TOTAL_CONTENEDOR),
                                                                                new System.Xml.Linq.XAttribute("CONTENEDORES", objCabecerSAV.CONTENEDORES == null ? "" : objCabecerSAV.CONTENEDORES),
                                                                                new System.Xml.Linq.XAttribute("RUC_USUARIO", objCabecerSAV.RUC_USUARIO == null ? "" : objCabecerSAV.RUC_USUARIO),
                                                                                new System.Xml.Linq.XAttribute("DESC_USUARIO", objCabecerSAV.DESC_USUARIO == null ? "" : objCabecerSAV.DESC_USUARIO),
                                                                                new System.Xml.Linq.XAttribute("NUMERO_FACTURA", objCabecerSAV.NUMERO_FACTURA == null ? "" : objCabecerSAV.NUMERO_FACTURA),
                                                                                new System.Xml.Linq.XAttribute("LINEA", objCabecerSAV.LINEA == null ? "" : objCabecerSAV.LINEA),
                                                                                new System.Xml.Linq.XAttribute("BOOKING", objCabecerSAV.BOOKING == null ? "" : objCabecerSAV.BOOKING),
                                                                                new System.Xml.Linq.XAttribute("ID_DEPOSITO", objCabecerSAV.ID_DEPOSITO),
                                                                                new System.Xml.Linq.XAttribute("flag", "I"))));


                                        System.Xml.Linq.XDocument XMLServicios = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                               new System.Xml.Linq.XElement("FACT_SERVICIOS", from p in objCabecerSAV.Detalle_Servicios.AsEnumerable().AsParallel()
                                                                                              select new System.Xml.Linq.XElement("DETALLE",
                                                                                             new System.Xml.Linq.XAttribute("ID_SERVICIO", p.ID_SERVICIO == null ? "" : p.ID_SERVICIO.ToString().Trim()),
                                                                                             new System.Xml.Linq.XAttribute("DESC_SERVICIO", p.DESC_SERVICIO == null ? "" : p.DESC_SERVICIO.ToString().Trim()),
                                                                                             new System.Xml.Linq.XAttribute("CARGA", p.CARGA == null ? "" : p.CARGA.ToString().Trim()),
                                                                                             new System.Xml.Linq.XAttribute("FECHA", p.FECHA == null ? DateTime.Parse("1900/01/01") : p.FECHA),
                                                                                             new System.Xml.Linq.XAttribute("TIPO_SERVICIO", p.TIPO_SERVICIO),
                                                                                             new System.Xml.Linq.XAttribute("CANTIDAD", p.CANTIDAD),
                                                                                             new System.Xml.Linq.XAttribute("PRECIO", p.PRECIO),
                                                                                             new System.Xml.Linq.XAttribute("SUBTOTAL", p.SUBTOTAL),
                                                                                             new System.Xml.Linq.XAttribute("IVA", p.IVA),
                                                                                             new System.Xml.Linq.XAttribute("DRAFT", p.DRAFT == null ? "" : p.DRAFT.ToString().Trim()),
                                                                                             new System.Xml.Linq.XAttribute("USUARIO_CREA", p.IV_USUARIO_CREA),
                                                                                             new System.Xml.Linq.XAttribute("flag", "I"))));

                                        System.Xml.Linq.XDocument XMLPasePuerta = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                              new System.Xml.Linq.XElement("FACT_PASES", from p in objCabecerSAV.Detalle_Factura.AsEnumerable().AsParallel()
                                                                                         select new System.Xml.Linq.XElement("DETALLE",
                                                                                          new System.Xml.Linq.XAttribute("turno_id", p.turno_id),
                                                                                          new System.Xml.Linq.XAttribute("turno_fecha", p.turno_fecha == null ? DateTime.Parse("1900/01/01") : p.turno_fecha),
                                                                                          new System.Xml.Linq.XAttribute("turno_hora", p.turno_hora == null ? "" : p.turno_hora.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_id", p.unidad_id == null ? "" : p.unidad_id.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_tamano", p.unidad_tamano == null ? "" : p.unidad_tamano.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_linea", p.unidad_linea == null ? "" : p.unidad_linea.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_booking", p.unidad_booking == null ? "" : p.unidad_booking.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_referencia", p.unidad_referencia == null ? "" : p.unidad_referencia.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_estatus", p.unidad_estatus == null ? "" : p.unidad_estatus.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_key", p.unidad_key),
                                                                                          new System.Xml.Linq.XAttribute("documento_id", p.documento_id == null ? "" : p.documento_id.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("estado_pago", p.estado_pago == null ? "" : p.estado_pago.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("ruc_cliente", p.ruc_cliente == null ? "" : p.ruc_cliente.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("name_cliente", p.name_cliente == null ? "" : p.name_cliente.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("asume_ruc_facturar", p.asume_ruc_facturar == null ? "" : p.asume_ruc_facturar.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("asume_cliente_facturar", p.asume_cliente_facturar == null ? "" : p.asume_cliente_facturar.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("id_facturar", p.id_facturar),
                                                                                          new System.Xml.Linq.XAttribute("flag", "I"))));

                                        string cMensajeActualizados = string.Empty;

                                        objCabecerSAV.xmlCabecera = XMLCabecera.ToString();
                                        objCabecerSAV.xmlServicios = XMLServicios.ToString();
                                        objCabecerSAV.xmlPases = XMLPasePuerta.ToString();

                                        var nProceso = objCabecerSAV.SaveTransaction(out cMensajes);

                                        /*fin de nuevo proceso de grabado*/
                                        if (!nProceso.HasValue || nProceso.Value <= 0)
                                        {

                                            objErroresSAV = new SAV_Detalle_Error();
                                            objErroresSAV.contenedor = Detalle.unidad_id;
                                            objErroresSAV.error = string.Format("<b>Error! No se pudo grabar datos de la factura..{0}</b>", cMensajes);
                                            objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                            objErroresSAV.fila = Errores;
                                            Errores++;

                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                            break;
                                        }
                                        else
                                        {

                                            //actualizar numero de factura en tablas de preavisos
                                            objActualizaSAV = new SAV_Asigna_Factura();
                                            objActualizaSAV.id = Detalle.id;
                                            objActualizaSAV.numero_factura = FacturaFinal;

                                            var nActualiza = objActualizaSAV.SaveTransaction(out cMensajes);

                                            if (!nActualiza.HasValue || nActualiza.Value <= 0)
                                            {
                                                objErroresSAV = new SAV_Detalle_Error();
                                                objErroresSAV.contenedor = Detalle.unidad_id;
                                                objErroresSAV.error = string.Format("<b>Error! No se pudo actualizar datos de la factura en el previaso # ..{0}- {1}</b>", Detalle.id, cMensajes);
                                                objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                                objErroresSAV.fila = Errores;
                                                Errores++;

                                                objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                            }

                                        }


                                        this.Mostrar_Mensaje(4, string.Format("<b>Informativo! Se genero la siguiente factura # {0} con éxito</b>", FacturaFinal));
                                    }
                                    else
                                    {
                                        objErroresSAV = new SAV_Detalle_Error();
                                        objErroresSAV.contenedor = Detalle.unidad_id;
                                        objErroresSAV.error = string.Format("<b>Error! </b>Lo sentimos, algo salió mal. {0}", Finalizar.messages.ToString());
                                        objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                        objErroresSAV.fila = Errores;
                                        Errores++;
                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                        this.Mostrar_Mensaje(4, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Problemas al finalizar el DRAFT # {0}", Draf));
                                        return;
                                    }


                                    Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;
                                }
                                else 
                                {
                                    objErroresSAV = new SAV_Detalle_Error();
                                    objErroresSAV.contenedor = Detalle.unidad_id;
                                    objErroresSAV.error = string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Problemas al cargar servicios del pase.. {0}/{1}", Detalle.unidad_id, resp.MensajeProblema);
                                    objErroresSAV.cliente = Detalle.asume_cliente_facturar;
                                    objErroresSAV.fila = Errores;
                                    Errores++;

                                    objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                }

                                Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

 
                            }


                            Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                            objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;

                            tablePagination.DataSource = objCabecerSAV.Detalle_Pases;
                            tablePagination.DataBind();

                            string Mensajes = string.Empty;

                            //cargar datos de errores
                            if (objCabecerSAV.Detalle_Errores.Count != 0)
                            {
                                Mensajes = string.Format("<i class='fa fa-warning'></i><b> Informativo! Se presentaron {0} errores al generar facturas, los mismo puede visualizar dando click en el botón detalle errores......(REPCONTVER)</b><br/>", objCabecerSAV.Detalle_Errores.Count);

                                this.BtnErrores.Attributes.Remove("disabled");

                                PaginationErrores.DataSource = objCabecerSAV.Detalle_Errores;
                                PaginationErrores.DataBind();

                            }

                            if (objCabecerSAV.Detalle_Ok.Count != 0)
                            {
                                Mensajes = string.Format("{0} {1}", Mensajes, string.Format("<i class='fa fa-warning'></i><b> Se procesaron {0} facturas con éxito..(REPCONTVER)</b>", objCabecerSAV.Detalle_Ok.Count));
                            }

                            if (string.IsNullOrEmpty(Mensajes))
                            {
                                Mensajes = string.Format("{0} {1}", Mensajes, string.Format("<i class='fa fa-warning'></i><b> No existen pases de puerta pendiente para facturar..(REPCONTVER)</b>"));
                            }

                            this.Mostrar_Mensaje(4, Mensajes);

                            this.OcultarLoading("1");
                            this.OcultarLoading("2");

                            this.Actualiza_Paneles();



                        }
                    }



                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "Factura SAV Devolución de Contenedores", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));


                }

            }

        }


        //proceso de generar facturas agrupadas
        protected void BtnFacturaAgrupada_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                CultureInfo enUS = new CultureInfo("en-US");

                string v_mensaje = string.Empty;
                string facturas = string.Empty;
                int Errores = 1;
                int nProcesados = 1;
                Int64 Gkey = 0;
                string Contenedores = string.Empty;
                List<String> Lista = new List<String>();

                try
                {
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    Int64 ID_DEPOSITO;

                    if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPOSITO))
                    {
                        ID_DEPOSITO = 0;
                    }

                    objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;

                    if (objCabecerSAV == null)
                    {
                        this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la factura individual por cada contenedor</b>"));
                        return;
                    }
                    else
                    {
                        LoginName = ClsUsuario.loginname;

                        var LinqListPasePuerta = (from TblPase in objCabecerSAV.Detalle_Pases.Where(p => string.IsNullOrEmpty(p.numero_factura) && p.visto && !string.IsNullOrEmpty(p.asume_ruc_facturar))
                                                  select new
                                                  {
                                                      asume_ruc_facturar = TblPase.asume_ruc_facturar.Trim(),
                                                      asume_cliente_facturar = TblPase.asume_cliente_facturar.Trim(),
                                                      visto = TblPase.visto,
                                                  }).Distinct();

                        objCabecerSAV.Detalle_Errores.Clear();
                        objCabecerSAV.Detalle_Ok.Clear();

                        //facturación SAV
                        if (ID_DEPOSITO == 4)
                        {

                            //recorrido de resumen de ruc
                            foreach (var Resumen in LinqListPasePuerta)
                            {
                                Lista.Clear();

                                Contenedores = string.Empty;

                                var LinqListContenedor = (from p in objCabecerSAV.Detalle_Pases.Where(x => string.IsNullOrEmpty(x.numero_factura)
                                                                     && x.visto && !string.IsNullOrEmpty(x.asume_ruc_facturar) && x.asume_ruc_facturar.Trim() == Resumen.asume_ruc_facturar)
                                                          select p.unidad_id).ToList();

                                Contenedores = string.Join(",", LinqListContenedor);

                                int nContenedor = 1;

                                //agrego detalle del pase a grabar
                                objCabecerSAV.Detalle_Factura.Clear();
                                objCabecerSAV.Detalle_Servicios.Clear();

                                foreach (var Detalle in objCabecerSAV.Detalle_Pases.Where(p => string.IsNullOrEmpty(p.numero_factura)
                                    && p.visto && !string.IsNullOrEmpty(p.asume_ruc_facturar) && p.asume_ruc_facturar.Trim() == Resumen.asume_ruc_facturar))
                                {

                                    objCabecerSAV.ID_CLIENTE = Detalle.ruc_cliente;
                                    objCabecerSAV.DESC_CLIENTE = Detalle.name_cliente;
                                    objCabecerSAV.ID_FACTURADO = Detalle.asume_ruc_facturar;
                                    objCabecerSAV.DESC_FACTURADO = Detalle.asume_cliente_facturar;
                                    objCabecerSAV.FECHA = DateTime.Now.Date;
                                    objCabecerSAV.IV_USUARIO_CREA = ClsUsuario.loginname;
                                    objCabecerSAV.TIPO_CARGA = "CNTR";
                                    objCabecerSAV.REFERENCIA = Detalle.unidad_referencia;
                                    objCabecerSAV.TOTAL_CONTENEDOR = nContenedor;//cantidad de contenedores
                                    objCabecerSAV.LINEA = Detalle.unidad_linea;
                                    objCabecerSAV.BOOKING = Detalle.unidad_booking;
                                    objCabecerSAV.CONTENEDORES = Contenedores;
                                    objCabecerSAV.INVOICE_TYPE = Detalle.invoice_type;
                                    objCabecerSAV.RUC_USUARIO = ClsUsuario.ruc;
                                    objCabecerSAV.DESC_USUARIO = ClsUsuario.nombres;

                                    objDetallePaseSAV = new SAV_Detalle_Pases();
                                    objDetallePaseSAV.turno_id = Detalle.turno_id;
                                    objDetallePaseSAV.turno_fecha = Detalle.turno_fecha;
                                    objDetallePaseSAV.turno_hora = Detalle.turno_hora;
                                    objDetallePaseSAV.unidad_id = Detalle.unidad_id;
                                    objDetallePaseSAV.unidad_tamano = Detalle.unidad_tamano;
                                    objDetallePaseSAV.unidad_linea = Detalle.unidad_linea;
                                    objDetallePaseSAV.unidad_booking = Detalle.unidad_booking;
                                    objDetallePaseSAV.unidad_referencia = Detalle.unidad_referencia;
                                    objDetallePaseSAV.unidad_estatus = Detalle.unidad_estatus;
                                    objDetallePaseSAV.unidad_key = Detalle.unidad_key;
                                    objDetallePaseSAV.cantidad = 1;
                                    objDetallePaseSAV.chofer_licencia = Detalle.chofer_licencia;
                                    objDetallePaseSAV.chofer_nombre = Detalle.chofer_nombre;
                                    objDetallePaseSAV.vehiculo_placa = Detalle.vehiculo_placa;
                                    objDetallePaseSAV.vehiculo_desc = Detalle.vehiculo_desc;
                                    objDetallePaseSAV.creado_usuario = Detalle.creado_usuario;
                                    objDetallePaseSAV.creado_fecha = Detalle.creado_fecha;
                                    objDetallePaseSAV.n4_unit_key = Detalle.n4_unit_key;
                                    objDetallePaseSAV.n4_message = Detalle.n4_message;
                                    objDetallePaseSAV.active = Detalle.active;
                                    objDetallePaseSAV.id = Detalle.id;
                                    objDetallePaseSAV.documento_id = Detalle.documento_id;
                                    objDetallePaseSAV.estado_pago = Detalle.estado_pago;
                                    objDetallePaseSAV.estado = Detalle.estado;
                                    objDetallePaseSAV.ruc_cliente = Detalle.ruc_cliente;
                                    objDetallePaseSAV.name_cliente = Detalle.name_cliente;
                                    objDetallePaseSAV.ruc_asume = Detalle.ruc_asume;
                                    objDetallePaseSAV.name_asume = Detalle.name_asume;
                                    objDetallePaseSAV.ruc_facturar = Detalle.ruc_facturar;
                                    objDetallePaseSAV.cliente_facturar = Detalle.cliente_facturar;
                                    objDetallePaseSAV.fila = Detalle.fila;
                                    objDetallePaseSAV.asume_ruc_facturar = Detalle.asume_ruc_facturar;
                                    objDetallePaseSAV.asume_cliente_facturar = Detalle.asume_cliente_facturar;
                                    objDetallePaseSAV.id_facturar = Detalle.id_facturar;
                                    objDetallePaseSAV.invoice_type = Detalle.invoice_type;

                                    objCabecerSAV.Detalle_Factura.Add(objDetallePaseSAV);

                                    nContenedor++;
                                }


                                /***********************************************************************************************************************************************
                                *datos del cliente N4, días de crédito 
                                ************************************************************************************************************************************************/
                                var Cliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, objCabecerSAV.ID_FACTURADO);
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

                                        objCabecerSAV.DIR_FACTURADO = (ListaCliente.CLNT_ADRESS == null ? "" : ListaCliente.CLNT_ADRESS);
                                        objCabecerSAV.EMAIL_FACTURADO = (ListaCliente.CLNT_EMAIL == null ? "" : ListaCliente.CLNT_EMAIL);
                                        objCabecerSAV.CIUDAD_FACTURADO = string.Empty;
                                        objCabecerSAV.DIAS_CREDITO = (ListaCliente.DIAS_CREDITO == null ? 0 : ListaCliente.DIAS_CREDITO.Value);
                                    }
                                    else
                                    {

                                        objErroresSAV = new SAV_Detalle_Error();
                                        objErroresSAV.contenedor = Contenedores;
                                        objErroresSAV.error = string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabecerSAV.ID_FACTURADO);
                                        objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                        objErroresSAV.fila = Errores;
                                        Errores++;
                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabecerSAV.ID_FACTURADO));

                                        return;
                                    }
                                }
                                else
                                {
                                    objErroresSAV = new SAV_Detalle_Error();
                                    objErroresSAV.contenedor = Contenedores;
                                    objErroresSAV.error = string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabecerSAV.ID_FACTURADO, objCabecerSAV.DESC_FACTURADO);
                                    objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                    objErroresSAV.fila = Errores;
                                    Errores++;
                                    objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabecerSAV.ID_FACTURADO, objCabecerSAV.DESC_FACTURADO));

                                    return;
                                }


                                /***********************************************************************************************************************************************
                                * Consulta de Servicios a facturar N4 - por cada contenedor
                                **********************************************************************************************************************************************/
                                Decimal Subtotal = 0;
                                Decimal Iva = 0;
                                Decimal Total = 0;
                                Fila = 1;

                                var Validacion = new Aduana.Importacion.ecu_validacion_cntr();
                                var Contenedor = new N4.Importacion.container();
                                var Billing = new N4Ws.Entidad.billing();
                                var Ws = new N4Ws.Entidad.InvoiceRequest();

                                /*saco el invoice type*/
                                string pInvoiceType = string.Empty;
                                pInvoiceType = objCabecerSAV.INVOICE_TYPE == null ? "2DA_MAN_LIN_NAV_CNTRS" : objCabecerSAV.INVOICE_TYPE;

                                Ws.action = N4Ws.Entidad.Action.DRAFT;
                                Ws.requester = N4Ws.Entidad.Requester.QUERY_CHARGES;

                                Ws.InvoiceTypeId = pInvoiceType;
                                Ws.payeeCustomerId = Cliente_Ruc;
                                Ws.payeeCustomerBizRole = Cliente_Rol;

                                var Direccion = new N4Ws.Entidad.address();
                                Direccion.addressLine1 = string.Empty;
                                Direccion.city = "GUAYAQUIL";

                                var Parametro = new N4Ws.Entidad.invoiceParameter();
                                Parametro.EquipmentId = Contenedores;
                                Parametro.PaidThruDay = objCabecerSAV.FECHA.Value.ToString("yyyy-MM-dd HH:mm");
                                //Parametro.bexuBookingNbr = "";
                                Ws.invoiceParameters.Add(Parametro);

                                Ws.billToParty.Add(Direccion);
                                Billing.Request = Ws;

                                var Resultado = Servicios.N4ServicioBasico(Billing, LoginName);
                                if (Resultado != null)
                                {
                                    if (Resultado.status_id.Equals("OK"))
                                    {
                                        var xBilling = Resultado;

                                        FechaPaidThruDay = null;
                                        CargabexuBlNbr = null;

                                        Fila = 1;

                                        draftNumber = xBilling.response.billInvoice.draftNumber;

                                        objCabecerSAV.DRAF = draftNumber;

                                        if (!Int64.TryParse(draftNumber, out draftNumberFinal))
                                        {

                                            objErroresSAV = new SAV_Detalle_Error();
                                            objErroresSAV.contenedor = Contenedores;
                                            objErroresSAV.error = string.Format("<b>Error! </b>No se puede convertir en campo numérico el draft # : {0}", draftNumber);
                                            objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                            objErroresSAV.fila = Errores;
                                            Errores++;
                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                            this.Mostrar_Mensaje(4, string.Format("<b>Error! </b>No se puede convertir en campo numérico el draft # : {0}", draftNumber));
                                            return;
                                        }

                                        Lista.Add(draftNumber.Trim());

                                        if (!Int64.TryParse(xBilling.response.billInvoice.gkey, out Gkey))
                                        {
                                            objErroresSAV = new SAV_Detalle_Error();
                                            objErroresSAV.contenedor = Contenedores;
                                            objErroresSAV.error = string.Format("<i class='fa fa-warning'></i><b> Error! No se puede convertir en campo numerico el gkey: {0}</b>", xBilling.response.billInvoice.gkey);
                                            objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                            objErroresSAV.fila = Errores;
                                            Errores++;
                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                            this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! No se puede convertir en campo numerico el gkey: {0}</b>", xBilling.response.billInvoice.gkey));
                                            return;
                                        }


                                        TipoServicio = xBilling.response.billInvoice.type;

                                        FechaPaidThruDay = (from bexuPaidThruDay in xBilling.response.billInvoice.invoiceParameters.Where(p => p.Metafield == "bexuPaidThruDay")
                                                            select new
                                                            {
                                                                fecha = bexuPaidThruDay.Value.ToString()
                                                            }
                                                   ).FirstOrDefault().fecha;

                                        var pCargabexuBlNbr = xBilling.response.billInvoice.invoiceParameters.Where(p => p.Metafield == "bexuBookingNbr").FirstOrDefault();

                                        if (pCargabexuBlNbr != null)
                                        {
                                            CargabexuBlNbr = (from bexuBlNbr in xBilling.response.billInvoice.invoiceParameters.Where(p => p.Metafield == "bexuBookingNbr")
                                                              select new
                                                              {
                                                                  carga = bexuBlNbr.Value == null ? "" : bexuBlNbr.Value
                                                              }).FirstOrDefault().carga;
                                        }
                                        else
                                        {
                                            CargabexuBlNbr = Cliente_Ruc;
                                        }



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
                                            objServiciosSAV = new SAV_Detalle_Servicios();
                                            objServiciosSAV.ID = 0;
                                            objServiciosSAV.LINEA = Fila;
                                            objServiciosSAV.ID_SERVICIO = Det.CODIGO;
                                            objServiciosSAV.DESC_SERVICIO = Det.SERVICIO;
                                            objServiciosSAV.CARGA = Det.CARGA;
                                            objServiciosSAV.FECHA = DateTime.Parse(Det.FECHA.ToString());
                                            objServiciosSAV.TIPO_SERVICIO = TipoServicio;
                                            objServiciosSAV.CANTIDAD = Det.CANTIDAD;
                                            objServiciosSAV.PRECIO = Det.PRECIO;
                                            objServiciosSAV.SUBTOTAL = Det.TOTAL;
                                            objServiciosSAV.IVA = Det.IVA;
                                            objServiciosSAV.IV_USUARIO_CREA = LoginName;
                                            objServiciosSAV.IV_FECHA_CREA = DateTime.Now;
                                            objServiciosSAV.DRAFT = draftNumber;
                                            Fila++;
                                            objCabecerSAV.Detalle_Servicios.Add(objServiciosSAV);

                                        }

                                        Iva = Iva + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTaxes != null ? xBilling.response.billInvoice.totalTaxes : "0", enUS), 2);
                                        Total = Total + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTotal != null ? xBilling.response.billInvoice.totalTotal : "0", enUS), 2);

                                        var LinqSubtotal = (from Servicios in objCabecerSAV.Detalle_Servicios.AsEnumerable()
                                                            select Servicios.SUBTOTAL
                                                      ).Sum();

                                        Subtotal = LinqSubtotal;


                                        objCabecerSAV.SUBTOTAL = Subtotal;
                                        objCabecerSAV.IVA = Iva;
                                        objCabecerSAV.TOTAL = Total;

                                        /**********************************************************************************************************************************/
                                        /*proceso finalizar draft de factura*/
                                        var BillingFin = new N4Ws.Entidad.billing();
                                        MergeInvoiceRequest Fin = new MergeInvoiceRequest();
                                        Fin.finalizeDate = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm");// FechaPaidThruDay;
                                        Fin.drftInvoiceNbrs = Lista;
                                        Fin.invoiceTypeId = pInvoiceType;
                                        BillingFin.MergeInvoiceRequest = Fin;

                                        var Finalizar = Servicios.N4ServicioBasicoMergeAndFinalizeTransaction(BillingFin, ClsUsuario.loginname.Trim());
                                        if (Finalizar != null)
                                        {
                                            if (Finalizar.status_id.Equals("OK"))
                                            {
                                                var Factura = Finalizar;
                                                NumeroFactura = Factura.response.billInvoice.finalNumber;

                                                //draftNumber

                                                objCabecerSAV.IVA = Decimal.Round(Decimal.Parse(Factura.response.billInvoice.totalTaxes != null ? Factura.response.billInvoice.totalTaxes : "0", enUS), 2); ;
                                                objCabecerSAV.TOTAL = Decimal.Round(Decimal.Parse(Factura.response.billInvoice.totalTotal != null ? Factura.response.billInvoice.totalTotal : "0", enUS), 2);
                                                objCabecerSAV.SUBTOTAL = Decimal.Round(Decimal.Parse(Factura.response.billInvoice.totalCharges != null ? Factura.response.billInvoice.totalCharges : "0", enUS), 2);

                                                NumeroFactura = "00" + NumeroFactura;
                                                string Establecimiento = NumeroFactura.Substring(0, 3);
                                                string PuntoEmision = NumeroFactura.Substring(3, 3);
                                                string Original = NumeroFactura.Substring(6, 9);
                                                string FacturaFinal = string.Format("{0}-{1}-{2}", Establecimiento, PuntoEmision, Original);

                                                objCabecerSAV.NUMERO_FACTURA = FacturaFinal;

                                                //AGREGO AL LOG DE PROCESADOS
                                                objProcesadosSAV = new SAV_Detalle_Ok();
                                                objProcesadosSAV.contenedor = Contenedores;
                                                objProcesadosSAV.mensaje = string.Format("Factura generada # {0} ==> OK", FacturaFinal);
                                                objProcesadosSAV.cliente = Resumen.asume_cliente_facturar;
                                                objProcesadosSAV.fila = nProcesados;
                                                objCabecerSAV.Detalle_Ok.Add(objProcesadosSAV);
                                                nProcesados++;

                                                //ACTUALIZAR DATOS ACTUALES
                                                foreach (var Detalle in objCabecerSAV.Detalle_Pases.Where(p => string.IsNullOrEmpty(p.numero_factura)
                                                            && p.visto && !string.IsNullOrEmpty(p.asume_ruc_facturar) && p.asume_ruc_facturar.Trim() == Resumen.asume_ruc_facturar))
                                                {
                                                    Detalle.numero_factura = FacturaFinal;

                                                    Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                                                }
                                                //FIN

                                                /*nuevo proceso de grabado*/
                                                System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                new System.Xml.Linq.XElement("FACT_CABECERA",
                                                                                         new System.Xml.Linq.XElement("CABECERA",
                                                                                        new System.Xml.Linq.XAttribute("GLOSA", objCabecerSAV.GLOSA == null ? "" : objCabecerSAV.GLOSA),
                                                                                        new System.Xml.Linq.XAttribute("FECHA", objCabecerSAV.FECHA == null ? DateTime.Parse("1900/01/01") : objCabecerSAV.FECHA),
                                                                                        new System.Xml.Linq.XAttribute("ID_CLIENTE", objCabecerSAV.ID_CLIENTE == null ? "" : objCabecerSAV.ID_CLIENTE),
                                                                                        new System.Xml.Linq.XAttribute("DESC_CLIENTE", objCabecerSAV.DESC_CLIENTE == null ? "" : objCabecerSAV.DESC_CLIENTE),
                                                                                        new System.Xml.Linq.XAttribute("ID_FACTURADO", objCabecerSAV.ID_FACTURADO == null ? "" : objCabecerSAV.ID_FACTURADO),
                                                                                        new System.Xml.Linq.XAttribute("DESC_FACTURADO", objCabecerSAV.DESC_FACTURADO == null ? "" : objCabecerSAV.DESC_FACTURADO),
                                                                                        new System.Xml.Linq.XAttribute("SUBTOTAL", objCabecerSAV.SUBTOTAL),
                                                                                        new System.Xml.Linq.XAttribute("IVA", objCabecerSAV.IVA),
                                                                                        new System.Xml.Linq.XAttribute("TOTAL", objCabecerSAV.TOTAL),
                                                                                        new System.Xml.Linq.XAttribute("DRAF", objCabecerSAV.DRAF == null ? "" : objCabecerSAV.DRAF),
                                                                                        new System.Xml.Linq.XAttribute("REFERENCIA", objCabecerSAV.REFERENCIA == null ? "" : objCabecerSAV.REFERENCIA),
                                                                                        new System.Xml.Linq.XAttribute("DIR_FACTURADO", objCabecerSAV.DIR_FACTURADO == null ? "" : objCabecerSAV.DIR_FACTURADO),
                                                                                        new System.Xml.Linq.XAttribute("EMAIL_FACTURADO", objCabecerSAV.EMAIL_FACTURADO == null ? "" : objCabecerSAV.EMAIL_FACTURADO),
                                                                                        new System.Xml.Linq.XAttribute("CIUDAD_FACTURADO", objCabecerSAV.CIUDAD_FACTURADO == null ? "" : objCabecerSAV.CIUDAD_FACTURADO),
                                                                                        new System.Xml.Linq.XAttribute("DIAS_CREDITO", objCabecerSAV.DIAS_CREDITO),
                                                                                        new System.Xml.Linq.XAttribute("USUARIO_CREA", objCabecerSAV.IV_USUARIO_CREA == null ? "" : objCabecerSAV.IV_USUARIO_CREA),
                                                                                        new System.Xml.Linq.XAttribute("TOTAL_CONTENEDORES", objCabecerSAV.TOTAL_CONTENEDOR),
                                                                                        new System.Xml.Linq.XAttribute("CONTENEDORES", objCabecerSAV.CONTENEDORES == null ? "" : objCabecerSAV.CONTENEDORES),
                                                                                        new System.Xml.Linq.XAttribute("RUC_USUARIO", objCabecerSAV.RUC_USUARIO == null ? "" : objCabecerSAV.RUC_USUARIO),
                                                                                        new System.Xml.Linq.XAttribute("DESC_USUARIO", objCabecerSAV.DESC_USUARIO == null ? "" : objCabecerSAV.DESC_USUARIO),
                                                                                        new System.Xml.Linq.XAttribute("NUMERO_FACTURA", objCabecerSAV.NUMERO_FACTURA == null ? "" : objCabecerSAV.NUMERO_FACTURA),
                                                                                        new System.Xml.Linq.XAttribute("LINEA", objCabecerSAV.LINEA == null ? "" : objCabecerSAV.LINEA),
                                                                                        new System.Xml.Linq.XAttribute("BOOKING", objCabecerSAV.BOOKING == null ? "" : objCabecerSAV.BOOKING),
                                                                                        new System.Xml.Linq.XAttribute("flag", "I"))));


                                                System.Xml.Linq.XDocument XMLServicios = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                new System.Xml.Linq.XElement("FACT_SERVICIOS", from p in objCabecerSAV.Detalle_Servicios.AsEnumerable().AsParallel()
                                                                                               select new System.Xml.Linq.XElement("DETALLE",
                                                                                                  new System.Xml.Linq.XAttribute("ID_SERVICIO", p.ID_SERVICIO == null ? "" : p.ID_SERVICIO.ToString().Trim()),
                                                                                                  new System.Xml.Linq.XAttribute("DESC_SERVICIO", p.DESC_SERVICIO == null ? "" : p.DESC_SERVICIO.ToString().Trim()),
                                                                                                  new System.Xml.Linq.XAttribute("CARGA", p.CARGA == null ? "" : p.CARGA.ToString().Trim()),
                                                                                                  new System.Xml.Linq.XAttribute("FECHA", p.FECHA == null ? DateTime.Parse("1900/01/01") : p.FECHA),
                                                                                                  new System.Xml.Linq.XAttribute("TIPO_SERVICIO", p.TIPO_SERVICIO),
                                                                                                  new System.Xml.Linq.XAttribute("CANTIDAD", p.CANTIDAD),
                                                                                                  new System.Xml.Linq.XAttribute("PRECIO", p.PRECIO),
                                                                                                  new System.Xml.Linq.XAttribute("SUBTOTAL", p.SUBTOTAL),
                                                                                                  new System.Xml.Linq.XAttribute("IVA", p.IVA),
                                                                                                  new System.Xml.Linq.XAttribute("DRAFT", p.DRAFT == null ? "" : p.DRAFT.ToString().Trim()),
                                                                                                  new System.Xml.Linq.XAttribute("USUARIO_CREA", p.IV_USUARIO_CREA),
                                                                                                  new System.Xml.Linq.XAttribute("flag", "I"))));


                                                System.Xml.Linq.XDocument XMLPasePuerta = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                new System.Xml.Linq.XElement("FACT_PASES", from p in objCabecerSAV.Detalle_Factura.AsEnumerable().AsParallel()
                                                                                           select new System.Xml.Linq.XElement("DETALLE",
                                                                                                new System.Xml.Linq.XAttribute("turno_id", p.turno_id),
                                                                                                new System.Xml.Linq.XAttribute("turno_fecha", p.turno_fecha == null ? DateTime.Parse("1900/01/01") : p.turno_fecha),
                                                                                                new System.Xml.Linq.XAttribute("turno_hora", p.turno_hora == null ? "" : p.turno_hora.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("unidad_id", p.unidad_id == null ? "" : p.unidad_id.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("unidad_tamano", p.unidad_tamano == null ? "" : p.unidad_tamano.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("unidad_linea", p.unidad_linea == null ? "" : p.unidad_linea.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("unidad_booking", p.unidad_booking == null ? "" : p.unidad_booking.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("unidad_referencia", p.unidad_referencia == null ? "" : p.unidad_referencia.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("unidad_estatus", p.unidad_estatus == null ? "" : p.unidad_estatus.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("unidad_key", p.unidad_key),
                                                                                                new System.Xml.Linq.XAttribute("documento_id", p.documento_id == null ? "" : p.documento_id.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("estado_pago", p.estado_pago == null ? "" : p.estado_pago.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("ruc_cliente", p.ruc_cliente == null ? "" : p.ruc_cliente.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("name_cliente", p.name_cliente == null ? "" : p.name_cliente.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("asume_ruc_facturar", p.asume_ruc_facturar == null ? "" : p.asume_ruc_facturar.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("asume_cliente_facturar", p.asume_cliente_facturar == null ? "" : p.asume_cliente_facturar.ToString().Trim()),
                                                                                                new System.Xml.Linq.XAttribute("id_facturar", p.id_facturar),
                                                                                                new System.Xml.Linq.XAttribute("flag", "I"))));


                                                string cMensajeActualizados = string.Empty;

                                                objCabecerSAV.xmlCabecera = XMLCabecera.ToString();
                                                objCabecerSAV.xmlServicios = XMLServicios.ToString();
                                                objCabecerSAV.xmlPases = XMLPasePuerta.ToString();

                                                var nProceso = objCabecerSAV.SaveTransaction(out cMensajes);

                                                /*fin de nuevo proceso de grabado*/
                                                if (!nProceso.HasValue || nProceso.Value <= 0)
                                                {

                                                    objErroresSAV = new SAV_Detalle_Error();
                                                    objErroresSAV.contenedor = Contenedores;
                                                    objErroresSAV.error = string.Format("<b>Error! No se pudo grabar datos de la factura..{0}</b>", cMensajes);
                                                    objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                                    objErroresSAV.fila = Errores;
                                                    Errores++;

                                                    objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                                    break;
                                                }
                                                else
                                                {

                                                    //actualizar numero de factura en tablas de preavisos
                                                    foreach (var Detalle in objCabecerSAV.Detalle_Pases.Where(p => p.visto && !string.IsNullOrEmpty(p.asume_ruc_facturar) && p.asume_ruc_facturar.Trim() == Resumen.asume_ruc_facturar))
                                                    {
                                                        objActualizaSAV = new SAV_Asigna_Factura();
                                                        objActualizaSAV.id = Detalle.id;
                                                        objActualizaSAV.numero_factura = FacturaFinal;

                                                        var nActualiza = objActualizaSAV.SaveTransaction(out cMensajes);

                                                        if (!nActualiza.HasValue || nActualiza.Value <= 0)
                                                        {
                                                            objErroresSAV = new SAV_Detalle_Error();
                                                            objErroresSAV.contenedor = Contenedores;
                                                            objErroresSAV.error = string.Format("<b>Error! No se pudo actualizar datos de la factura en el previaso # ..{0}- {1}</b>", Detalle.id, cMensajes);
                                                            objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                                            objErroresSAV.fila = Errores;
                                                            Errores++;

                                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                                        }

                                                    }

                                                }

                                            }
                                            else
                                            {
                                                objErroresSAV = new SAV_Detalle_Error();
                                                objErroresSAV.contenedor = Contenedores;
                                                objErroresSAV.error = string.Format("<b>Error! </b>Lo sentimos, algo salió mal. {0}", Finalizar.messages.ToString());
                                                objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                                objErroresSAV.fila = Errores;
                                                Errores++;

                                                objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                            }
                                        }
                                        else
                                        {
                                            objErroresSAV = new SAV_Detalle_Error();
                                            objErroresSAV.contenedor = Contenedores;
                                            objErroresSAV.error = string.Format("<b>Error! </b>Lo sentimos, algo salió mal. {0}", Finalizar.messages.ToString());
                                            objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                            objErroresSAV.fila = Errores;
                                            Errores++;

                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                        }

                                        Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                                    }
                                    else
                                    {
                                        var Msg = (from A in Resultado.messages.Where(p => p.message_detail.Contains("ERROR"))
                                                   select A.message_detail).FirstOrDefault();

                                        if (Msg != null)
                                        {
                                            objErroresSAV = new SAV_Detalle_Error();
                                            objErroresSAV.contenedor = Contenedores;
                                            objErroresSAV.error = Msg;
                                            objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                            objErroresSAV.fila = Errores;
                                            Errores++;

                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                        }

                                    }

                                }
                                else
                                {
                                    var Msg = (from A in Resultado.messages.Where(p => p.message_detail.Contains("ERROR"))
                                               select A.message_detail).FirstOrDefault();

                                    if (Msg != null)
                                    {
                                        objErroresSAV = new SAV_Detalle_Error();
                                        objErroresSAV.contenedor = Contenedores;
                                        objErroresSAV.error = Msg;
                                        objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                        objErroresSAV.fila = Errores;
                                        Errores++;

                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                    }
                                }

                            }//fin recorrido por ruc

                        }
                        else 
                        {
                            //facturación: REPCONTVER
                            //recorrido de resumen de ruc

                            var LinqListPasePuerta2 = (from TblPase in objCabecerSAV.Detalle_Pases.Where(p => string.IsNullOrEmpty(p.numero_factura) && p.visto && !string.IsNullOrEmpty(p.asume_ruc_facturar))
                                                      select new
                                                      {
                                                          asume_ruc_facturar = TblPase.asume_ruc_facturar.Trim(),
                                                          asume_cliente_facturar = TblPase.asume_cliente_facturar.Trim(),
                                                          visto = TblPase.visto,
                                                          linea = TblPase.unidad_linea
                                                      }).Distinct();

                            foreach (var Resumen in LinqListPasePuerta2)
                            {
                                Lista.Clear();

                                Contenedores = string.Empty;

                                var LinqListContenedor = (from p in objCabecerSAV.Detalle_Pases.Where(x => string.IsNullOrEmpty(x.numero_factura)
                                                                     && x.visto && !string.IsNullOrEmpty(x.asume_ruc_facturar) && x.asume_ruc_facturar.Trim() == Resumen.asume_ruc_facturar && x.unidad_linea == Resumen.linea)
                                                          select p.unidad_id).ToList();

                                Contenedores = string.Join(",", LinqListContenedor);

                                int nContenedor = 1;

                               

                                //agrego detalle del pase a grabar
                                objCabecerSAV.Detalle_Factura.Clear();
                                objCabecerSAV.Detalle_Servicios.Clear();

                                foreach (var Detalle in objCabecerSAV.Detalle_Pases.Where(p => string.IsNullOrEmpty(p.numero_factura)
                                    && p.visto && !string.IsNullOrEmpty(p.asume_ruc_facturar) && p.asume_ruc_facturar.Trim() == Resumen.asume_ruc_facturar && p.unidad_linea == Resumen.linea))
                                {



                                    objCabecerSAV.ID_CLIENTE = Detalle.ruc_cliente;
                                    objCabecerSAV.DESC_CLIENTE = Detalle.name_cliente;
                                    objCabecerSAV.ID_FACTURADO = Detalle.asume_ruc_facturar;
                                    objCabecerSAV.DESC_FACTURADO = Detalle.asume_cliente_facturar;
                                    objCabecerSAV.FECHA = DateTime.Now.Date;
                                    objCabecerSAV.IV_USUARIO_CREA = ClsUsuario.loginname;
                                    objCabecerSAV.TIPO_CARGA = "CNTR";
                                    objCabecerSAV.REFERENCIA = Detalle.unidad_referencia;
                                    objCabecerSAV.TOTAL_CONTENEDOR = nContenedor;//cantidad de contenedores
                                    objCabecerSAV.LINEA = Detalle.unidad_linea;
                                    objCabecerSAV.BOOKING = Detalle.unidad_booking;
                                    objCabecerSAV.CONTENEDORES = Contenedores;
                                    objCabecerSAV.INVOICE_TYPE = Detalle.invoice_type;
                                    objCabecerSAV.RUC_USUARIO = ClsUsuario.ruc;
                                    objCabecerSAV.DESC_USUARIO = ClsUsuario.nombres;

                                    objDetallePaseSAV = new SAV_Detalle_Pases();
                                    objDetallePaseSAV.turno_id = Detalle.turno_id;
                                    objDetallePaseSAV.turno_fecha = Detalle.turno_fecha;
                                    objDetallePaseSAV.turno_hora = Detalle.turno_hora;
                                    objDetallePaseSAV.unidad_id = Detalle.unidad_id;
                                    objDetallePaseSAV.unidad_tamano = Detalle.unidad_tamano;
                                    objDetallePaseSAV.unidad_linea = Detalle.unidad_linea;
                                    objDetallePaseSAV.unidad_booking = Detalle.unidad_booking;
                                    objDetallePaseSAV.unidad_referencia = Detalle.unidad_referencia;
                                    objDetallePaseSAV.unidad_estatus = Detalle.unidad_estatus;
                                    objDetallePaseSAV.unidad_key = Detalle.unidad_key;
                                    objDetallePaseSAV.cantidad = 1;
                                    objDetallePaseSAV.chofer_licencia = Detalle.chofer_licencia;
                                    objDetallePaseSAV.chofer_nombre = Detalle.chofer_nombre;
                                    objDetallePaseSAV.vehiculo_placa = Detalle.vehiculo_placa;
                                    objDetallePaseSAV.vehiculo_desc = Detalle.vehiculo_desc;
                                    objDetallePaseSAV.creado_usuario = Detalle.creado_usuario;
                                    objDetallePaseSAV.creado_fecha = Detalle.creado_fecha;
                                    objDetallePaseSAV.n4_unit_key = Detalle.n4_unit_key;
                                    objDetallePaseSAV.n4_message = Detalle.n4_message;
                                    objDetallePaseSAV.active = Detalle.active;
                                    objDetallePaseSAV.id = Detalle.id;
                                    objDetallePaseSAV.documento_id = Detalle.documento_id;
                                    objDetallePaseSAV.estado_pago = Detalle.estado_pago;
                                    objDetallePaseSAV.estado = Detalle.estado;
                                    objDetallePaseSAV.ruc_cliente = Detalle.ruc_cliente;
                                    objDetallePaseSAV.name_cliente = Detalle.name_cliente;
                                    objDetallePaseSAV.ruc_asume = Detalle.ruc_asume;
                                    objDetallePaseSAV.name_asume = Detalle.name_asume;
                                    objDetallePaseSAV.ruc_facturar = Detalle.ruc_facturar;
                                    objDetallePaseSAV.cliente_facturar = Detalle.cliente_facturar;
                                    objDetallePaseSAV.fila = Detalle.fila;
                                    objDetallePaseSAV.asume_ruc_facturar = Detalle.asume_ruc_facturar;
                                    objDetallePaseSAV.asume_cliente_facturar = Detalle.asume_cliente_facturar;
                                    objDetallePaseSAV.id_facturar = Detalle.id_facturar;
                                    objDetallePaseSAV.invoice_type = Detalle.invoice_type;

                                    objCabecerSAV.Detalle_Factura.Add(objDetallePaseSAV);

                                    nContenedor++;
                                }


                                /***********************************************************************************************************************************************
                                *datos del cliente N4, días de crédito 
                                ************************************************************************************************************************************************/
                                var Cliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, objCabecerSAV.ID_FACTURADO);
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

                                        objCabecerSAV.DIR_FACTURADO = (ListaCliente.CLNT_ADRESS == null ? "" : ListaCliente.CLNT_ADRESS);
                                        objCabecerSAV.EMAIL_FACTURADO = (ListaCliente.CLNT_EMAIL == null ? "" : ListaCliente.CLNT_EMAIL);
                                        objCabecerSAV.CIUDAD_FACTURADO = string.Empty;
                                        objCabecerSAV.DIAS_CREDITO = (ListaCliente.DIAS_CREDITO == null ? 0 : ListaCliente.DIAS_CREDITO.Value);
                                    }
                                    else
                                    {

                                        objErroresSAV = new SAV_Detalle_Error();
                                        objErroresSAV.contenedor = Contenedores;
                                        objErroresSAV.error = string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabecerSAV.ID_FACTURADO);
                                        objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                        objErroresSAV.fila = Errores;
                                        Errores++;
                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabecerSAV.ID_FACTURADO));

                                        return;
                                    }
                                }
                                else
                                {
                                    objErroresSAV = new SAV_Detalle_Error();
                                    objErroresSAV.contenedor = Contenedores;
                                    objErroresSAV.error = string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabecerSAV.ID_FACTURADO, objCabecerSAV.DESC_FACTURADO);
                                    objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                    objErroresSAV.fila = Errores;
                                    Errores++;
                                    objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabecerSAV.ID_FACTURADO, objCabecerSAV.DESC_FACTURADO));

                                    return;
                                }




                                /***********************************************************************************************************************************************
                                * Consulta de Servicios a facturar N4 - por cada contenedor
                                **********************************************************************************************************************************************/

                                //saco los rubros a facturar
                                var rubros = SAV_Servicios_Repcontver.Carga_DetalleRubros_Otros(Resumen.linea,out v_mensaje);
                                if (!String.IsNullOrEmpty(v_mensaje))
                                {

                                    this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener rubros de facturación de REPCONTVER....{0}</b>", cMensajes));
                                    return;
                                }

                                if (rubros.Count == 0)
                                {
                                    this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen rubros asociados a REPCONTVER para poder facturar.....</b>"));
                                    return;
                                }

                                var list = new List<Tuple<string, string>>();

                                foreach (var item in rubros)
                                {

                                    list.Add(Tuple.Create(item.CODIGO_TARIJA_N4, string.Format("{0},{1},{2}", item.VALUE_TARIJA_N4, nContenedor.ToString(), Contenedores)));
                                }

                                Lista.Clear();


                                /*ultima factura en caso de tener*/
                                var LinqInvoiceType = (from Tbl in rubros.Where(Tbl => Tbl.CANTIDAD != 0)
                                                       select new
                                                       {
                                                           INVOICETYPE = Tbl.INVOICETYPE,
                                                           CODIGO_TARIJA_N4 = Tbl.CODIGO_TARIJA_N4,
                                                           VALUE_TARIJA_N4 = Tbl.VALUE_TARIJA_N4,
                                                       }).FirstOrDefault();


                                Decimal Subtotal = 0;
                                Decimal Iva = 0;
                                Decimal Total = 0;
                                Fila = 1;


                                string request = string.Empty;
                                string response = string.Empty;
                                Respuesta.ResultadoOperacion<bool> resp;
                                resp = ServicioSCA.CargarServicioBTS(LinqInvoiceType.INVOICETYPE, Cliente_Ruc, string.Format("{0}", nContenedor), list, Page.User.Identity.Name.ToUpper(), out request, out response);
                                if (resp.Exitoso)
                                {
                                    var v_result = resp.MensajeInformacion.Split(',');
                                    decimal v_monto = 0;
                                    v_monto = decimal.Parse(v_result[1].ToString());
                                    string Draf = v_result[0];
                                    
                                    objCabecerSAV.DRAF = Draf;

                                    /*detalle de muelles*/
                                    List<BTS_OTROS_Detalle_Rubros_Factura> ListServicios = BTS_OTROS_Detalle_Rubros_Factura.Carga_Servicios_Draf(Draf, out cMensajes);
                                    if (!String.IsNullOrEmpty(cMensajes))
                                    {

                                        objErroresSAV = new SAV_Detalle_Error();
                                        objErroresSAV.contenedor = Contenedores;
                                        objErroresSAV.error = string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener servicios de facturación del draf....{0}/ Error:{1}..(REPCONTVER)</b>", Draf, cMensajes);
                                        objErroresSAV.cliente = Resumen.asume_cliente_facturar; ;
                                        objErroresSAV.fila = Errores;
                                        Errores++;
                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                        this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener servicios de facturación del draf....{0}/ Error:{1}..(REPCONTVER)</b>", Draf, cMensajes));
                                        this.Actualiza_Panele_Detalle();
                                        return;
                                    }

                                    foreach (var Det in ListServicios)
                                    {
                                        objServiciosSAV = new SAV_Detalle_Servicios();
                                        objServiciosSAV.ID = 0;
                                        objServiciosSAV.LINEA = Fila;
                                        objServiciosSAV.ID_SERVICIO = Draf;
                                        objServiciosSAV.DESC_SERVICIO = Det.description;
                                        objServiciosSAV.CARGA = string.Format("{0}", Det.event_id);
                                        objServiciosSAV.FECHA = Det.created;
                                        objServiciosSAV.TIPO_SERVICIO = LinqInvoiceType.INVOICETYPE;
                                        objServiciosSAV.CANTIDAD = Det.quantity;
                                        objServiciosSAV.PRECIO = Det.amount;
                                        objServiciosSAV.SUBTOTAL = Det.amount;
                                        objServiciosSAV.IVA = Det.amount_taxt;
                                        objServiciosSAV.IV_USUARIO_CREA = LoginName;
                                        objServiciosSAV.IV_FECHA_CREA = DateTime.Now;
                                        objServiciosSAV.DRAFT = Draf;
                                        Fila++;
                                        objCabecerSAV.Detalle_Servicios.Add(objServiciosSAV);

                                    }


                                    Subtotal = Decimal.Round(ListServicios.Sum(p => p.amount), 2);
                                    Iva = Decimal.Round(ListServicios.Sum(p => p.amount_taxt), 2);
                                    Total = Subtotal + Iva;

                                    objCabecerSAV.SUBTOTAL = Subtotal;
                                    objCabecerSAV.IVA = Iva;
                                    objCabecerSAV.TOTAL = Total;

                                    /*proceso finalizar draft de factura*/
                                    List<String> ListaDraft = new List<String>();
                                    ListaDraft.Add(Draf);

                                    var BillingFin = new N4Ws.Entidad.billing();
                                    MergeInvoiceRequest Fin = new MergeInvoiceRequest();
                                    Fin.finalizeDate = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm");// FechaPaidThruDay;
                                    Fin.drftInvoiceNbrs = ListaDraft;
                                    Fin.invoiceTypeId = LinqInvoiceType.INVOICETYPE;
                                    BillingFin.MergeInvoiceRequest = Fin;

                                    var Finalizar = Servicios.N4ServicioBasicoMergeAndFinalizeTransaction(BillingFin, ClsUsuario.loginname.Trim());
                                    if (Finalizar != null)
                                    {

                                        var Factura = Finalizar;
                                        string NumeroFactura = Factura.response.billInvoice.finalNumber;
                                        NumeroFactura = "00" + NumeroFactura;
                                        string Establecimiento = NumeroFactura.Substring(0, 3);
                                        string PuntoEmision = NumeroFactura.Substring(3, 3);
                                        string Original = NumeroFactura.Substring(6, 9);
                                        string FacturaFinal = string.Format("{0}-{1}-{2}", Establecimiento, PuntoEmision, Original);

                                        objCabecerSAV.NUMERO_FACTURA = FacturaFinal;

                                        //AGREGO AL LOG DE PROCESADOS
                                        objProcesadosSAV = new SAV_Detalle_Ok();
                                        objProcesadosSAV.contenedor = Contenedores;
                                        objProcesadosSAV.mensaje = string.Format("Factura generada # {0} ==> OK", FacturaFinal);
                                        objProcesadosSAV.cliente = Resumen.asume_cliente_facturar;
                                        objProcesadosSAV.fila = nProcesados;
                                        objCabecerSAV.Detalle_Ok.Add(objProcesadosSAV);
                                        nProcesados++;

                                        //ACTUALIZAR DATOS ACTUALES
                                        foreach (var Detalle in objCabecerSAV.Detalle_Pases.Where(p => string.IsNullOrEmpty(p.numero_factura)
                                                    && p.visto && !string.IsNullOrEmpty(p.asume_ruc_facturar) && p.asume_ruc_facturar.Trim() == Resumen.asume_ruc_facturar))
                                        {
                                            Detalle.numero_factura = FacturaFinal;

                                            Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                                        }
                                        //FIN

                                       

                                        /*nuevo proceso de grabado*/
                                        System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                        new System.Xml.Linq.XElement("FACT_CABECERA",
                                                                                 new System.Xml.Linq.XElement("CABECERA",
                                                                                new System.Xml.Linq.XAttribute("GLOSA", objCabecerSAV.GLOSA == null ? "" : objCabecerSAV.GLOSA),
                                                                                new System.Xml.Linq.XAttribute("FECHA", objCabecerSAV.FECHA == null ? DateTime.Parse("1900/01/01") : objCabecerSAV.FECHA),
                                                                                new System.Xml.Linq.XAttribute("ID_CLIENTE", objCabecerSAV.ID_CLIENTE == null ? "" : objCabecerSAV.ID_CLIENTE),
                                                                                new System.Xml.Linq.XAttribute("DESC_CLIENTE", objCabecerSAV.DESC_CLIENTE == null ? "" : objCabecerSAV.DESC_CLIENTE),
                                                                                new System.Xml.Linq.XAttribute("ID_FACTURADO", objCabecerSAV.ID_FACTURADO == null ? "" : objCabecerSAV.ID_FACTURADO),
                                                                                new System.Xml.Linq.XAttribute("DESC_FACTURADO", objCabecerSAV.DESC_FACTURADO == null ? "" : objCabecerSAV.DESC_FACTURADO),
                                                                                new System.Xml.Linq.XAttribute("SUBTOTAL", objCabecerSAV.SUBTOTAL),
                                                                                new System.Xml.Linq.XAttribute("IVA", objCabecerSAV.IVA),
                                                                                new System.Xml.Linq.XAttribute("TOTAL", objCabecerSAV.TOTAL),
                                                                                new System.Xml.Linq.XAttribute("DRAF", objCabecerSAV.DRAF == null ? "" : objCabecerSAV.DRAF),
                                                                                new System.Xml.Linq.XAttribute("REFERENCIA", objCabecerSAV.REFERENCIA == null ? "" : objCabecerSAV.REFERENCIA),
                                                                                new System.Xml.Linq.XAttribute("DIR_FACTURADO", objCabecerSAV.DIR_FACTURADO == null ? "" : objCabecerSAV.DIR_FACTURADO),
                                                                                new System.Xml.Linq.XAttribute("EMAIL_FACTURADO", objCabecerSAV.EMAIL_FACTURADO == null ? "" : objCabecerSAV.EMAIL_FACTURADO),
                                                                                new System.Xml.Linq.XAttribute("CIUDAD_FACTURADO", objCabecerSAV.CIUDAD_FACTURADO == null ? "" : objCabecerSAV.CIUDAD_FACTURADO),
                                                                                new System.Xml.Linq.XAttribute("DIAS_CREDITO", objCabecerSAV.DIAS_CREDITO),
                                                                                new System.Xml.Linq.XAttribute("USUARIO_CREA", objCabecerSAV.IV_USUARIO_CREA == null ? "" : objCabecerSAV.IV_USUARIO_CREA),
                                                                                new System.Xml.Linq.XAttribute("TOTAL_CONTENEDORES", objCabecerSAV.TOTAL_CONTENEDOR),
                                                                                new System.Xml.Linq.XAttribute("CONTENEDORES", objCabecerSAV.CONTENEDORES == null ? "" : objCabecerSAV.CONTENEDORES),
                                                                                new System.Xml.Linq.XAttribute("RUC_USUARIO", objCabecerSAV.RUC_USUARIO == null ? "" : objCabecerSAV.RUC_USUARIO),
                                                                                new System.Xml.Linq.XAttribute("DESC_USUARIO", objCabecerSAV.DESC_USUARIO == null ? "" : objCabecerSAV.DESC_USUARIO),
                                                                                new System.Xml.Linq.XAttribute("NUMERO_FACTURA", objCabecerSAV.NUMERO_FACTURA == null ? "" : objCabecerSAV.NUMERO_FACTURA),
                                                                                new System.Xml.Linq.XAttribute("LINEA", objCabecerSAV.LINEA == null ? "" : objCabecerSAV.LINEA),
                                                                                new System.Xml.Linq.XAttribute("BOOKING", objCabecerSAV.BOOKING == null ? "" : objCabecerSAV.BOOKING),
                                                                                new System.Xml.Linq.XAttribute("ID_DEPOSITO", objCabecerSAV.ID_DEPOSITO),
                                                                                new System.Xml.Linq.XAttribute("flag", "I"))));


                                        System.Xml.Linq.XDocument XMLServicios = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                               new System.Xml.Linq.XElement("FACT_SERVICIOS", from p in objCabecerSAV.Detalle_Servicios.AsEnumerable().AsParallel()
                                                                                              select new System.Xml.Linq.XElement("DETALLE",
                                                                                             new System.Xml.Linq.XAttribute("ID_SERVICIO", p.ID_SERVICIO == null ? "" : p.ID_SERVICIO.ToString().Trim()),
                                                                                             new System.Xml.Linq.XAttribute("DESC_SERVICIO", p.DESC_SERVICIO == null ? "" : p.DESC_SERVICIO.ToString().Trim()),
                                                                                             new System.Xml.Linq.XAttribute("CARGA", p.CARGA == null ? "" : p.CARGA.ToString().Trim()),
                                                                                             new System.Xml.Linq.XAttribute("FECHA", p.FECHA == null ? DateTime.Parse("1900/01/01") : p.FECHA),
                                                                                             new System.Xml.Linq.XAttribute("TIPO_SERVICIO", p.TIPO_SERVICIO),
                                                                                             new System.Xml.Linq.XAttribute("CANTIDAD", p.CANTIDAD),
                                                                                             new System.Xml.Linq.XAttribute("PRECIO", p.PRECIO),
                                                                                             new System.Xml.Linq.XAttribute("SUBTOTAL", p.SUBTOTAL),
                                                                                             new System.Xml.Linq.XAttribute("IVA", p.IVA),
                                                                                             new System.Xml.Linq.XAttribute("DRAFT", p.DRAFT == null ? "" : p.DRAFT.ToString().Trim()),
                                                                                             new System.Xml.Linq.XAttribute("USUARIO_CREA", p.IV_USUARIO_CREA),
                                                                                             new System.Xml.Linq.XAttribute("flag", "I"))));

                                        System.Xml.Linq.XDocument XMLPasePuerta = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                              new System.Xml.Linq.XElement("FACT_PASES", from p in objCabecerSAV.Detalle_Factura.AsEnumerable().AsParallel()
                                                                                         select new System.Xml.Linq.XElement("DETALLE",
                                                                                          new System.Xml.Linq.XAttribute("turno_id", p.turno_id),
                                                                                          new System.Xml.Linq.XAttribute("turno_fecha", p.turno_fecha == null ? DateTime.Parse("1900/01/01") : p.turno_fecha),
                                                                                          new System.Xml.Linq.XAttribute("turno_hora", p.turno_hora == null ? "" : p.turno_hora.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_id", p.unidad_id == null ? "" : p.unidad_id.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_tamano", p.unidad_tamano == null ? "" : p.unidad_tamano.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_linea", p.unidad_linea == null ? "" : p.unidad_linea.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_booking", p.unidad_booking == null ? "" : p.unidad_booking.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_referencia", p.unidad_referencia == null ? "" : p.unidad_referencia.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_estatus", p.unidad_estatus == null ? "" : p.unidad_estatus.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("unidad_key", p.unidad_key),
                                                                                          new System.Xml.Linq.XAttribute("documento_id", p.documento_id == null ? "" : p.documento_id.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("estado_pago", p.estado_pago == null ? "" : p.estado_pago.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("ruc_cliente", p.ruc_cliente == null ? "" : p.ruc_cliente.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("name_cliente", p.name_cliente == null ? "" : p.name_cliente.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("asume_ruc_facturar", p.asume_ruc_facturar == null ? "" : p.asume_ruc_facturar.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("asume_cliente_facturar", p.asume_cliente_facturar == null ? "" : p.asume_cliente_facturar.ToString().Trim()),
                                                                                          new System.Xml.Linq.XAttribute("id_facturar", p.id_facturar),
                                                                                          new System.Xml.Linq.XAttribute("flag", "I"))));

                                        string cMensajeActualizados = string.Empty;

                                        objCabecerSAV.xmlCabecera = XMLCabecera.ToString();
                                        objCabecerSAV.xmlServicios = XMLServicios.ToString();
                                        objCabecerSAV.xmlPases = XMLPasePuerta.ToString();

                                        var nProceso = objCabecerSAV.SaveTransaction(out cMensajes);

                                        /*fin de nuevo proceso de grabado*/
                                        if (!nProceso.HasValue || nProceso.Value <= 0)
                                        {

                                            objErroresSAV = new SAV_Detalle_Error();
                                            objErroresSAV.contenedor = Contenedores;
                                            objErroresSAV.error = string.Format("<b>Error! No se pudo grabar datos de la factura..{0}</b>", cMensajes);
                                            objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                            objErroresSAV.fila = Errores;
                                            Errores++;

                                            objCabecerSAV.Detalle_Errores.Add(objErroresSAV);

                                            break;
                                        }
                                        else
                                        {
                                            //actualizar numero de factura en tablas de preavisos
                                            foreach (var Detalle in objCabecerSAV.Detalle_Pases.Where(p => p.visto && !string.IsNullOrEmpty(p.asume_ruc_facturar) && p.asume_ruc_facturar.Trim() == Resumen.asume_ruc_facturar))
                                            {
                                                objActualizaSAV = new SAV_Asigna_Factura();
                                                objActualizaSAV.id = Detalle.id;
                                                objActualizaSAV.numero_factura = FacturaFinal;

                                                var nActualiza = objActualizaSAV.SaveTransaction(out cMensajes);

                                                if (!nActualiza.HasValue || nActualiza.Value <= 0)
                                                {
                                                    objErroresSAV = new SAV_Detalle_Error();
                                                    objErroresSAV.contenedor = Contenedores;
                                                    objErroresSAV.error = string.Format("<b>Error! No se pudo actualizar datos de la factura en el previaso # ..{0}- {1}</b>", Detalle.id, cMensajes);
                                                    objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                                    objErroresSAV.fila = Errores;
                                                    Errores++;

                                                    objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                                }

                                            }


                                        }

                                    }
                                    else
                                    {
                                        objErroresSAV = new SAV_Detalle_Error();
                                        objErroresSAV.contenedor = Contenedores;
                                        objErroresSAV.error = string.Format("<b>Error! </b>Lo sentimos, algo salió mal. {0}", Finalizar.messages.ToString());
                                        objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                        objErroresSAV.fila = Errores;
                                        Errores++;

                                        objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                    }


                                }
                                else 
                                {
                                    objErroresSAV = new SAV_Detalle_Error();
                                    objErroresSAV.contenedor = Contenedores;
                                    objErroresSAV.error = string.Format("<b>Error! </b>Lo sentimos, algo salió mal. {0}", resp.MensajeProblema.ToString());
                                    objErroresSAV.cliente = Resumen.asume_cliente_facturar;
                                    objErroresSAV.fila = Errores;
                                    Errores++;

                                    objCabecerSAV.Detalle_Errores.Add(objErroresSAV);
                                }

                                Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;


                            }//fin recorrido por ruc
                        }



                        /***************************************************************************************************************************************
                        * fin procesos
                        ****************************************************************************************************************************************/

                        Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] = objCabecerSAV;

                        objCabecerSAV = Session["TransaccionSAV" + this.hf_BrowserWindowName.Value] as SAV_Cabecera_Factura;

                        tablePagination.DataSource = objCabecerSAV.Detalle_Pases;
                        tablePagination.DataBind();

                        string Mensajes = string.Empty;

                        //cargar datos de errores
                        if (objCabecerSAV.Detalle_Errores.Count != 0)
                        {
                            Mensajes = string.Format("<i class='fa fa-warning'></i><b> Informativo! Se presentaron {0} errores al generar facturas, los mismo puede visualizar dando click en el botón detalle errores......</b><br/>", objCabecerSAV.Detalle_Errores.Count);

                            this.BtnErrores.Attributes.Remove("disabled");

                            PaginationErrores.DataSource = objCabecerSAV.Detalle_Errores;
                            PaginationErrores.DataBind();

                        }

                        if (objCabecerSAV.Detalle_Ok.Count != 0)
                        {
                            Mensajes = string.Format("{0} {1}", Mensajes, string.Format("<i class='fa fa-warning'></i><b> Se procesaron {0} facturas con éxito..</b>", objCabecerSAV.Detalle_Ok.Count));
                        }


                        this.Mostrar_Mensaje(4, Mensajes);

                        this.OcultarLoading("1");
                        this.OcultarLoading("2");

                        this.Actualiza_Paneles();


                    }//fin si existe sesion
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "Factura SAV Devolución de Contenedores", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));


                }

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
                dataTable.Columns.Add("ruc_facturar", typeof(string));
                dataTable.Columns.Add("cliente_facturar", typeof(string));
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


        //protected void BtnExcel_Click(object sender, EventArgs e)
        //{
        //    if (Response.IsClientConnected)
        //    {





        //    }
        //}





        #endregion

        #region "deposito"
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

                    this.Cargar_PasesPendientes();


                }


            }
            catch (Exception ex)
            {
             
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