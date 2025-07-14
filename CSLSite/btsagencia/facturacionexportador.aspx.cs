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


    public partial class facturacionexportador : System.Web.UI.Page
    {


        #region "Clases"
        private static Int64? lm = -3;
        private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
       

        //private Cls_Bil_Invoice_Cabecera objFactura = new Cls_Bil_Invoice_Cabecera();
        //private Cls_Bil_Invoice_Detalle objDetalleFactura = new Cls_Bil_Invoice_Detalle();
        //private Cls_Bil_Invoice_Servicios objServiciosFactura = new Cls_Bil_Invoice_Servicios();
        private Cls_Bil_Invoice_Validaciones objValidacion = new Cls_Bil_Invoice_Validaciones();

        //private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        //private Cls_Bil_Detalle objDetalle = new Cls_Bil_Detalle();
        //private Cls_Bil_Invoice_Actualiza_Pase objActualiza_Pase = new Cls_Bil_Invoice_Actualiza_Pase();
        private List<Cls_Bil_AsumeFactura> List_Asume { set; get; }
        //private List<Cls_Bil_Contenedor_DiasLibres> List_Contenedor { set; get; }
      
        

      //  private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();

     
       // private P2D_Tarifario objTarifa = new P2D_Tarifario();
       // private P2D_Tarja_Cfs objTarja = new P2D_Tarja_Cfs();


        private BTS_Prev_Cabecera objCabeceraBTS = new BTS_Prev_Cabecera();
        private Cls_Prev_Detalle_Bodega objDetalleBodega = new Cls_Prev_Detalle_Bodega();
        private Cls_Prev_Detalle_Muelle objDetalleMuelle = new Cls_Prev_Detalle_Muelle();

        private BTS_Prev_Cabecera objFacturaBTS = new BTS_Prev_Cabecera();
        private Cls_Prev_Detalle_Bodega objDetalleFacturaBodega = new Cls_Prev_Detalle_Bodega();
        private Cls_Prev_Detalle_Muelle objDetalleFacturaMuelle = new Cls_Prev_Detalle_Muelle();
        private Cls_Prev_Detalle_Servicios objDetalleFacturaServicios = new Cls_Prev_Detalle_Servicios();


        private BTS_OTROS_Cabecera_Exportadores objCabeceraBTS_OTROS = new BTS_OTROS_Cabecera_Exportadores();
        private BTS_OTROS_Detalle_Exportadores objDetalleExportador = new BTS_OTROS_Detalle_Exportadores();
        private BTS_OTROS_Detalle_ExportadoresUnico objDetalleExportadorUnico = new BTS_OTROS_Detalle_ExportadoresUnico();
        private BTS_OTROS_Detalle_Rubros objDetalleExportadorRubros = new BTS_OTROS_Detalle_Rubros();
        private BTS_Exportador objExportador = new BTS_Exportador();
        private BTS_OTROS_Detalle_Rubros_Factura objDetalleExportadorRubrosFactura = new BTS_OTROS_Detalle_Rubros_Factura();
        private BTS_OTROS_Procesa_Factura objProcesaFactura = new BTS_OTROS_Procesa_Factura();
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
            this.UPDETALLE_RUBROS.Update();
            
            UPBOTONES.Update();
            this.UPBOTONESBUSCADOR.Update();
            this.UPBOTONES_RUBROS.Update();

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

                this.banmsg_rubros.Visible = false;
                this.banmsg_rubros.InnerHtml = string.Empty;

                OcultarLoading("1");
            }
            if (Tipo == 2)//detalle
            {
                this.banmsg_Pase.Visible = false;
                this.banmsg_det.Visible = true;
                this.banmsg.Visible = false;
                this.banmsg_det.InnerHtml = Mensaje;
                this.banmsg_rubros.Visible = false;
                this.banmsg_rubros.InnerHtml = string.Empty;
                OcultarLoading("2");
            }

            if (Tipo == 3)//alerta
            {
                this.banmsg_det.Visible = false;
                this.banmsg.Visible = false;
                this.banmsg_Pase.Visible = true;
                this.banmsg_Pase.InnerHtml = Mensaje;
                this.banmsg_rubros.Visible = false;
                this.banmsg_rubros.InnerHtml = string.Empty;
                OcultarLoading("2");
            }

            if (Tipo == 4)//ambos
            {
                this.banmsg_Pase.Visible = false;
                this.banmsg_det.Visible = true;
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                this.banmsg_det.InnerHtml = Mensaje;
                this.banmsg_rubros.Visible = false;
                this.banmsg_rubros.InnerHtml = string.Empty;
                OcultarLoading("1");
            }

            if (Tipo == 5)//rubros
            {
                this.banmsg_Pase.Visible = false;
                this.banmsg_det.Visible = false;
                this.banmsg.Visible = false;
                this.banmsg.InnerHtml = string.Empty;
                this.banmsg_det.InnerHtml = string.Empty;

                this.banmsg_rubros.Visible = true;
                this.banmsg_rubros.InnerHtml = Mensaje;

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
            this.banmsg_rubros.Visible = false;
            this.banmsg_rubros.InnerText = string.Empty;

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
            objCabeceraBTS_OTROS = new BTS_OTROS_Cabecera_Exportadores();
            Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS_OTROS;
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

                Server.HtmlEncode(this.TXTMRN.Text.Trim());
              
              

                if (!Page.IsPostBack)
                {

                    /*secuencial de sesion*/
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    this.Crear_Sesion();


                    objFacturaBTS = new BTS_Prev_Cabecera();
                    Session["InvoiceBTS" + this.hf_BrowserWindowName.Value] = objFacturaBTS;

                    objCabeceraBTS_OTROS = new BTS_OTROS_Cabecera_Exportadores();
                    Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS_OTROS;

                    


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

                    //ver exportador
                    if (e.CommandName == "Ver")
                    {
                       
                        string v_mensaje = string.Empty;

                        this.ID_FILA.Value = t.ToString();
                        this.TxtFila.Text = t.ToString();

                        var lookup = BTS_Exportador.Buscador_Exportador("",  out v_mensaje);

                        if (lookup != null && lookup.Count > 0)
                        {
                            this.tablePaginationBuscador.DataSource = lookup;
                            this.tablePaginationBuscador.DataBind();

                            banmsg_buscador.InnerText = string.Empty;
                            banmsg_buscador.Visible = false;
                            UPBUSCADOR.Update();
                        }

                    }

                    //ver detalle de eventos
                    if (e.CommandName == "Detalle")
                    {

                        string v_mensaje = string.Empty;

                        this.ID_FILA.Value = t.ToString();
                        this.TxtFila.Text = t.ToString();

                        
                        objCabeceraBTS_OTROS = Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] as BTS_OTROS_Cabecera_Exportadores;

                        //existe 
                        var Detalle = objCabeceraBTS_OTROS.Detalle_Exportador.FirstOrDefault(f => f.Fila == (string.IsNullOrEmpty(this.TxtFila.Text) ? 0 : int.Parse(this.TxtFila.Text)));
                        if (Detalle != null)
                        {
                            var rubros = BTS_OTROS_Detalle_Rubros.Carga_DetalleRubros_Eventos(Detalle.referencia, Detalle.idExportador, out v_mensaje);

                            if (!string.IsNullOrEmpty(v_mensaje))
                            {
                                this.Mostrar_Mensaje(5, string.Format("<b>Error! No se pudo encontrar información de rubros del del exportador: {0} </b>", v_mensaje));
                                return;
                            }


                            tablePaginationRubros.DataSource = rubros;
                            tablePaginationRubros.DataBind();

                            this.banmsg_rubros.Visible = false;
                            this.banmsg_rubros.InnerHtml = string.Empty;

                            this.Actualiza_Paneles();

                        }
                        else
                        {
                            tablePaginationRubros.DataSource = null;
                            tablePaginationRubros.DataBind();

                            this.Actualiza_Paneles();

                            this.Mostrar_Mensaje(5, string.Format("<b>Error! No se pudo encontrar información de rubros del del exportador: {0} </b>", t.ToString()));
                            return;
                        }

                  

                    }
                    //generar factura
                    if (e.CommandName == "Grabar")
                    {
                        string v_mensaje = string.Empty;
                        Decimal Subtotal = 0;
                        Decimal Iva = 0;
                        Decimal Total = 0;

                        this.ID_FILA.Value = t.ToString();
                        this.TxtFila.Text = t.ToString();

                        objCabeceraBTS_OTROS = Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] as BTS_OTROS_Cabecera_Exportadores;

                        if (objCabeceraBTS_OTROS == null)
                        {
                            this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la factura </b>"));
                            return;
                        }

                        var Detalle = objCabeceraBTS_OTROS.Detalle_Exportador.FirstOrDefault(f => f.Fila == (string.IsNullOrEmpty(this.TxtFila.Text) ? 0 : int.Parse(this.TxtFila.Text)));
                        if (Detalle != null)
                        {

                            /***********************************************************************************************************************************************
                            *cargar conceptos para generar draf de factura
                            **********************************************************************************************************************************************/
                            var rubros = BTS_OTROS_Detalle_Rubros.Carga_DetalleRubros_Eventos_Agrupar(Detalle.referencia, Detalle.idExportador, out v_mensaje);
                            if (!String.IsNullOrEmpty(v_mensaje))
                            {

                                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener rubros de facturación de exportador....{0}</b>", cMensajes));
                                this.Actualiza_Paneles();
                                return;
                            }
                            if (rubros.Count == 0)
                            {
                                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen rubros asociados al exportador para poder facturar.....</b>"));
                                this.Actualiza_Paneles();
                                return;
                            }

                            var list = new List<Tuple<string, string>>();

                            foreach (var item in rubros)
                            {
                                list.Add(Tuple.Create(item.codigoN4, string.Format("{0},{1},{2}", item.value_tarifa_N4, item.cantidad.ToString().Replace(".00",""), item.comentario)));
                            }

                            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                            objCabeceraBTS_OTROS.ID_CLIENTE = Detalle.ruc;
                            objCabeceraBTS_OTROS.DESC_CLIENTE = Detalle.exportador;
                            objCabeceraBTS_OTROS.ID_FACTURADO = Detalle.ruc_asume;
                            objCabeceraBTS_OTROS.DESC_FACTURADO = Detalle.exportador_asume;
                            objCabeceraBTS_OTROS.FECHA = DateTime.Now.Date;
                            objCabeceraBTS_OTROS.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objCabeceraBTS_OTROS.TIPO_CARGA = "BRBK";
                            objCabeceraBTS_OTROS.REFERENCIA = Detalle.idNave;
                            objCabeceraBTS_OTROS.TOTAL_CAJA = Detalle.cantidad;


                            objCabeceraBTS_OTROS.Detalle_Rubros.Clear();
                            objCabeceraBTS_OTROS.Detalle_Exportador_Unico.Clear();
                            objCabeceraBTS_OTROS.Detalle_Rubros_Factura.Clear();


                            foreach (var Det in objCabeceraBTS_OTROS.Detalle_Exportador.Where(f => f.Fila == (string.IsNullOrEmpty(this.TxtFila.Text) ? 0 : int.Parse(this.TxtFila.Text))))
                            {
                                objDetalleExportadorUnico = new BTS_OTROS_Detalle_ExportadoresUnico();
                                objDetalleExportadorUnico.ID = Det.ID;
                                objDetalleExportadorUnico.Fila = Det.Fila;
                                objDetalleExportadorUnico.idNave = Det.idNave;
                                objDetalleExportadorUnico.codLine = Det.codLine;
                                objDetalleExportadorUnico.ruc = Det.ruc;
                                objDetalleExportadorUnico.exportador = Det.exportador;
                                objDetalleExportadorUnico.idStowageCab = Det.idStowageCab;
                                objDetalleExportadorUnico.idExportador = Det.idExportador;
                                objDetalleExportadorUnico.cantidad = Det.cantidad;
                                objDetalleExportadorUnico.ruc_asume = Det.ruc_asume;
                                objDetalleExportadorUnico.exportador_asume = Det.exportador_asume;
                                objDetalleExportadorUnico.idExportador_asume = Det.idExportador_asume;

                                objCabeceraBTS_OTROS.Detalle_Exportador_Unico.Add(objDetalleExportadorUnico);

                            }

                            Fila = 1;
                            foreach (var Det in rubros)
                            {
                                objDetalleExportadorRubros = new BTS_OTROS_Detalle_Rubros();
                                objDetalleExportadorRubros.Fila = Fila;
                                objDetalleExportadorRubros.idNave = Det.idNave;
                                objDetalleExportadorRubros.ruc = Det.ruc;
                                objDetalleExportadorRubros.exportador = Det.exportador;
                                objDetalleExportadorRubros.idStowageCab = Det.idStowageCab;
                                objDetalleExportadorRubros.idExportador = Det.idExportador;
                                objDetalleExportadorRubros.cantidad = Det.cantidad;
                                objDetalleExportadorRubros.ruc_asume = Det.ruc_asume;
                                objDetalleExportadorRubros.codigo = Det.codigo;
                                objDetalleExportadorRubros.codigoN4 = Det.codigoN4;
                                objDetalleExportadorRubros.nombre = Det.nombre;
                                objDetalleExportadorRubros.comentario = Det.comentario;
                                objDetalleExportadorRubros.value_tarifa_N4 = Det.value_tarifa_N4;
                                objDetalleExportadorRubros.invoicetype = Det.invoicetype;
                               

                                objDetalleExportadorRubros.IV_USUARIO_CREA = LoginName;
                                objDetalleExportadorRubros.IV_FECHA_CREA = DateTime.Now;


                                Fila++;
                                objCabeceraBTS_OTROS.Detalle_Rubros.Add(objDetalleExportadorRubros);
                            }

                            /*ultima factura en caso de tener*/
                            var LinqInvoiceType = (from Tbl in rubros.Where(Tbl => Tbl.cantidad != 0)
                                               select new
                                               {
                                                   INVOICETYPE = Tbl.invoicetype,
                                                   CODIGO_TARIJA_N4 = Tbl.codigoN4,
                                                   VALUE_TARIJA_N4 = Tbl.value_tarifa_N4,
                                                   CONCEPTO = Tbl.comentario,
                                                   CANTIDAD = Tbl.cantidad,
                                               }).FirstOrDefault();


                            /***********************************************************************************************************************************************
                            *datos del cliente N4, días de crédito 
                            **********************************************************************************************************************************************/
                            var Cliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, objCabeceraBTS_OTROS.ID_FACTURADO);
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

                                    objCabeceraBTS_OTROS.DIR_FACTURADO = (ListaCliente.CLNT_ADRESS == null ? "" : ListaCliente.CLNT_ADRESS);
                                    objCabeceraBTS_OTROS.EMAIL_FACTURADO = (ListaCliente.CLNT_EMAIL == null ? "" : ListaCliente.CLNT_EMAIL);
                                    objCabeceraBTS_OTROS.CIUDAD_FACTURADO = string.Empty;
                                    objCabeceraBTS_OTROS.DIAS_CREDITO = (ListaCliente.DIAS_CREDITO == null ? 0 : ListaCliente.DIAS_CREDITO.Value);
                                }
                                else
                                {
                                    this.Limpia_Asume_Factura();
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabeceraBTS_OTROS.ID_FACTURADO));

                                    return;
                                }
                            }
                            else
                            {
                                this.Limpia_Asume_Factura();
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabeceraBTS_OTROS.ID_FACTURADO, objCabeceraBTS_OTROS.DESC_FACTURADO));

                                return;

                            }


                            /***********************************************************************************************************************************************
                            *4) Consulta de Servicios a facturar N4 
                            **********************************************************************************************************************************************/
                            string request = string.Empty;
                            string response = string.Empty;
                            Respuesta.ResultadoOperacion<bool> resp;
                            resp = ServicioSCA.CargarServicioBTS(LinqInvoiceType.INVOICETYPE, objCabeceraBTS_OTROS.ID_FACTURADO, objCabeceraBTS_OTROS.REFERENCIA, list, Page.User.Identity.Name.ToUpper(), out request, out response);
                            if (resp.Exitoso)
                            {
                                var v_result = resp.MensajeInformacion.Split(',');
                                decimal v_monto = 0;
                                v_monto = decimal.Parse(v_result[1].ToString());
                                string Draf = v_result[0];

                                objCabeceraBTS_OTROS.DRAF = Draf;


                                /*detalle de muelles*/
                                List<BTS_OTROS_Detalle_Rubros_Factura> ListServicios = BTS_OTROS_Detalle_Rubros_Factura.Carga_Servicios_Draf(Draf, out cMensajes);
                                if (!String.IsNullOrEmpty(cMensajes))
                                {

                                    this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener servicios de facturación del draf....{0}/ Error:{1}</b>", Draf, cMensajes));
                                    this.Actualiza_Panele_Detalle();
                                    return;
                                }

                                foreach (var Det in ListServicios)
                                {

                                    objDetalleExportadorRubrosFactura = new BTS_OTROS_Detalle_Rubros_Factura();
                                    objDetalleExportadorRubrosFactura.Fila = Fila;
                                    objDetalleExportadorRubrosFactura.final_nbr = Det.final_nbr;
                                    objDetalleExportadorRubrosFactura.draft_nbr = Det.draft_nbr;
                                    objDetalleExportadorRubrosFactura.payee_customer_name = Det.payee_customer_name;
                                    objDetalleExportadorRubrosFactura.currency_id = Det.currency_id;
                                    objDetalleExportadorRubrosFactura.finalized_date = Det.finalized_date;
                                    objDetalleExportadorRubrosFactura.gkey = Det.gkey;
                                    objDetalleExportadorRubrosFactura.event_type_id = Det.event_type_id;
                                    objDetalleExportadorRubrosFactura.event_id = Det.event_id;
                                    objDetalleExportadorRubrosFactura.quantity_billed = Det.quantity_billed;
                                    objDetalleExportadorRubrosFactura.quantity = Det.quantity;
                                    objDetalleExportadorRubrosFactura.rate_billed = Det.rate_billed;
                                    objDetalleExportadorRubrosFactura.amount = Det.amount;
                                    objDetalleExportadorRubrosFactura.description = Det.description;
                                    objDetalleExportadorRubrosFactura.created = Det.created;
                                    objDetalleExportadorRubrosFactura.tax = Det.tax;
                                    objDetalleExportadorRubrosFactura.amount_taxt = Det.amount_taxt;
                                    objDetalleExportadorRubrosFactura.notes = Det.notes;

                                    objDetalleExportadorRubrosFactura.IV_USUARIO_CREA = LoginName;
                                    objDetalleExportadorRubrosFactura.IV_FECHA_CREA = DateTime.Now;


                                    Fila++;
                                    objCabeceraBTS_OTROS.Detalle_Rubros_Factura.Add(objDetalleExportadorRubrosFactura);

                                }


                                Subtotal = Decimal.Round(ListServicios.Sum(p => p.amount), 2);
                                Iva = Decimal.Round(ListServicios.Sum(p => p.amount_taxt), 2);
                                Total = Subtotal + Iva;

                                objCabeceraBTS_OTROS.SUBTOTAL = Subtotal;
                                objCabeceraBTS_OTROS.IVA = Iva;
                                objCabeceraBTS_OTROS.TOTAL = Total;


                                /*proceso finalizar draft de factura*/
                                List<String> Lista = new List<String>();
                                Lista.Add(Draf);

                                var BillingFin = new N4Ws.Entidad.billing();
                                MergeInvoiceRequest Fin = new MergeInvoiceRequest();
                                Fin.finalizeDate = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm");// FechaPaidThruDay;
                                Fin.drftInvoiceNbrs = Lista;
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

                                    objCabeceraBTS_OTROS.NUMERO_FACTURA = FacturaFinal;
                                    //actualizo numero de factura
                                    Detalle.numero_factura = FacturaFinal;

                                    this.Mostrar_Mensaje(4, string.Format("<b>Informativo! Se genero la siguiente factura # {0} con éxito</b>", FacturaFinal));
                                }
                                else
                                {
                                    this.Mostrar_Mensaje(4, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Problemas al finalizar el DRAFT # {0}", Draf));
                                    return;
                                }

                            }//fin genera factura
                            else
                            {
                                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! No se logró realizar la facturación en N4...{0} </b>", resp.MensajeProblema));
                                return;

                            }



                            Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS_OTROS;

                            objCabeceraBTS_OTROS = Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] as BTS_OTROS_Cabecera_Exportadores;

                            tablePagination.DataSource = objCabeceraBTS_OTROS.Detalle_Exportador;
                            tablePagination.DataBind();

                            this.Actualiza_Panele_Detalle();

                            /*nuevo proceso de grabado*/
                            System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new System.Xml.Linq.XElement("FACT_CABECERA",
                                                                    new System.Xml.Linq.XElement("CABECERA",
                                                                    new System.Xml.Linq.XAttribute("GLOSA", objCabeceraBTS_OTROS.GLOSA == null ? "" : objCabeceraBTS_OTROS.GLOSA),
                                                                    new System.Xml.Linq.XAttribute("FECHA", objCabeceraBTS_OTROS.FECHA == null ? DateTime.Parse("1900/01/01") : objCabeceraBTS_OTROS.FECHA),
                                                                    new System.Xml.Linq.XAttribute("ID_CLIENTE", objCabeceraBTS_OTROS.ID_CLIENTE == null ? "" : objCabeceraBTS_OTROS.ID_CLIENTE),
                                                                    new System.Xml.Linq.XAttribute("DESC_CLIENTE", objCabeceraBTS_OTROS.DESC_CLIENTE == null ? "" : objCabeceraBTS_OTROS.DESC_CLIENTE),
                                                                    new System.Xml.Linq.XAttribute("ID_FACTURADO", objCabeceraBTS_OTROS.ID_FACTURADO == null ? "" : objCabeceraBTS_OTROS.ID_FACTURADO),
                                                                    new System.Xml.Linq.XAttribute("DESC_FACTURADO", objCabeceraBTS_OTROS.DESC_FACTURADO == null ? "" : objCabeceraBTS_OTROS.DESC_FACTURADO),
                                                                    new System.Xml.Linq.XAttribute("SUBTOTAL", objCabeceraBTS_OTROS.SUBTOTAL),
                                                                    new System.Xml.Linq.XAttribute("IVA", objCabeceraBTS_OTROS.IVA),
                                                                    new System.Xml.Linq.XAttribute("TOTAL", objCabeceraBTS_OTROS.TOTAL),
                                                                    new System.Xml.Linq.XAttribute("DRAF", objCabeceraBTS_OTROS.DRAF == null ? "" : objCabeceraBTS_OTROS.DRAF),
                                                                    new System.Xml.Linq.XAttribute("REFERENCIA", objCabeceraBTS_OTROS.REFERENCIA == null ? "" : objCabeceraBTS_OTROS.REFERENCIA),
                                                                    new System.Xml.Linq.XAttribute("DIR_FACTURADO", objCabeceraBTS_OTROS.DIR_FACTURADO == null ? "" : objCabeceraBTS_OTROS.DIR_FACTURADO),
                                                                    new System.Xml.Linq.XAttribute("EMAIL_FACTURADO", objCabeceraBTS_OTROS.EMAIL_FACTURADO == null ? "" : objCabeceraBTS_OTROS.EMAIL_FACTURADO),
                                                                    new System.Xml.Linq.XAttribute("CIUDAD_FACTURADO", objCabeceraBTS_OTROS.CIUDAD_FACTURADO == null ? "" : objCabeceraBTS_OTROS.CIUDAD_FACTURADO),
                                                                    new System.Xml.Linq.XAttribute("DIAS_CREDITO", objCabeceraBTS_OTROS.DIAS_CREDITO),
                                                                    new System.Xml.Linq.XAttribute("USUARIO_CREA", objCabeceraBTS_OTROS.IV_USUARIO_CREA == null ? "" : objCabeceraBTS_OTROS.IV_USUARIO_CREA),
                                                                    new System.Xml.Linq.XAttribute("CANTIDAD", objCabeceraBTS_OTROS.TOTAL_CAJA),
                                                                    new System.Xml.Linq.XAttribute("FACTURA", objCabeceraBTS_OTROS.NUMERO_FACTURA),
                                                                    new System.Xml.Linq.XAttribute("RUC_USUARIO", objCabeceraBTS_OTROS.RUC_USUARIO == null ? "" : objCabeceraBTS_OTROS.RUC_USUARIO),
                                                                    new System.Xml.Linq.XAttribute("DESC_USUARIO", objCabeceraBTS_OTROS.DESC_USUARIO == null ? "" : objCabeceraBTS_OTROS.DESC_USUARIO),
                                                                    new System.Xml.Linq.XAttribute("flag", "I"))));


                            System.Xml.Linq.XDocument XMLServicios = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                           new System.Xml.Linq.XElement("FACT_SERVICIOS", from p in objCabeceraBTS_OTROS.Detalle_Rubros_Factura.AsEnumerable().AsParallel()
                                                                          select new System.Xml.Linq.XElement("DETALLE",
                                                                               new System.Xml.Linq.XAttribute("draft_nbr", p.draft_nbr),
                                                                               new System.Xml.Linq.XAttribute("payee_customer_id", p.payee_customer_id == null ? "" : p.payee_customer_id.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("payee_customer_name", p.payee_customer_name == null ? "" : p.payee_customer_name.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("currency_id", p.currency_id == null ? "" : p.currency_id.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("finalized_date", p.finalized_date == null ? DateTime.Parse("1900/01/01") : p.finalized_date),
                                                                               new System.Xml.Linq.XAttribute("gkey", p.gkey),
                                                                               new System.Xml.Linq.XAttribute("event_type_id", p.event_type_id == null ? "" : p.event_type_id.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("event_id", p.event_id == null ? "" : p.event_id.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("quantity_billed", p.quantity_billed),
                                                                               new System.Xml.Linq.XAttribute("quantity", p.quantity),
                                                                               new System.Xml.Linq.XAttribute("rate_billed", p.rate_billed),
                                                                               new System.Xml.Linq.XAttribute("amount", p.amount),
                                                                               new System.Xml.Linq.XAttribute("description", p.description == null ? "" : p.description.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("tax", p.tax),
                                                                               new System.Xml.Linq.XAttribute("amount_taxt", p.amount_taxt),
                                                                               new System.Xml.Linq.XAttribute("notes", p.notes == null ? "" : p.notes.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("flag", "I"))));

                            System.Xml.Linq.XDocument XMLExportador = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                             new System.Xml.Linq.XElement("FACT_EXPORTADOR", from p in objCabeceraBTS_OTROS.Detalle_Exportador_Unico.AsEnumerable().AsParallel()
                                                                         select new System.Xml.Linq.XElement("DETALLE",
                                                                            new System.Xml.Linq.XAttribute("idNave", p.idNave == null ? "" : p.idNave.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("codLine", p.codLine == null ? "" : p.codLine.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("linea", p.linea == null ? "" : p.linea.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("ruc", p.ruc == null ? "" : p.ruc.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("exportador", p.exportador == null ? "" : p.exportador.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("idStowageCab", p.idStowageCab),
                                                                            new System.Xml.Linq.XAttribute("idExportador", p.idExportador),
                                                                            new System.Xml.Linq.XAttribute("cantidad", p.cantidad),
                                                                            new System.Xml.Linq.XAttribute("ruc_asume", p.ruc_asume == null ? "" : p.ruc_asume.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("exportador_asume", p.exportador_asume == null ? "" : p.exportador_asume.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("idExportador_asume", p.idExportador_asume),
                                                                            new System.Xml.Linq.XAttribute("flag", "I"))));

                            System.Xml.Linq.XDocument XMLRubros = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new System.Xml.Linq.XElement("FACT_RUBROS", from p in objCabeceraBTS_OTROS.Detalle_Rubros.AsEnumerable().AsParallel()
                                                                            select new System.Xml.Linq.XElement("DETALLE",
                                                                                new System.Xml.Linq.XAttribute("idNave", p.idNave == null ? "" : p.idNave.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("ruc", p.ruc == null ? "" : p.ruc.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("exportador", p.exportador == null ? "" : p.exportador.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("idStowageCab", p.idStowageCab),
                                                                                new System.Xml.Linq.XAttribute("idExportador", p.idExportador),
                                                                                new System.Xml.Linq.XAttribute("cantidad", p.cantidad),
                                                                                new System.Xml.Linq.XAttribute("ruc_asume", p.ruc_asume == null ? "" : p.ruc_asume.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("codigo", p.codigo == null ? "" : p.codigo.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("codigoN4", p.codigo == null ? "" : p.codigoN4.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("nombre", p.codigo == null ? "" : p.nombre.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("comentario", p.comentario == null ? "" : p.comentario.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("value_tarifa_N4", p.value_tarifa_N4 == null ? "" : p.value_tarifa_N4.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("invoicetype", p.invoicetype == null ? "" : p.invoicetype.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("flag", "I"))));

                            cMensajes = string.Empty;
                            string cMensajeActualizados = string.Empty;

                            objProcesaFactura.xmlCabecera = XMLCabecera.ToString();
                            objProcesaFactura.xmlExportador = XMLExportador.ToString();
                            objProcesaFactura.xmlRubros = XMLRubros.ToString();
                            objProcesaFactura.xmlServicios = XMLServicios.ToString();

                            var nProceso = objProcesaFactura.SaveTransaction_BTS_Exportador_Eventos(out cMensajes);
                            /*fin de nuevo proceso de grabado*/
                            if (!nProceso.HasValue || nProceso.Value <= 0)
                            {

                                this.Mostrar_Mensaje(4, string.Format("<b>Error! No se pudo grabar datos de la factura del exportador seleccionado..{0}</b>", cMensajes));


                                /*************************************************************************************************************************************
                                * crear caso salesforce
                                * **********************************************************************************************************************************/

                                this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación BODEGA BTS", "Error al grabar", cMensajes, objCabeceraBTS_OTROS.REFERENCIA, objCabeceraBTS_OTROS.DESC_FACTURADO, objCabeceraBTS_OTROS.NUMERO_FACTURA, out MensajeCasos);

                                return;
                            }
                            else
                            {
                                

                                this.Mostrar_Mensaje(4, string.Format("<b>Informativo! Se genero la siguiente factura # {0} con éxito</b>", objCabeceraBTS_OTROS.NUMERO_FACTURA));
                            }
                        }
                        else
                        {
                           
                            this.Mostrar_Mensaje(4, string.Format("<b>Error! No se pudo encontrar información del exportador para generar la factura: {0} </b>", t.ToString()));
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


        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label LblFactura = e.Item.FindControl("LblFactura") as Label;
                Button Btn = e.Item.FindControl("BtnFacturarExp") as Button;
                Button BtnPrint = e.Item.FindControl("BtnImprimir") as Button;

                if (!string.IsNullOrEmpty(LblFactura.Text))
                {

                    Btn.Attributes["disabled"] = "disabled";
                }
                if (string.IsNullOrEmpty(LblFactura.Text))
                {

                    BtnPrint.Attributes["disabled"] = "disabled";
                }
            }
        }




        #endregion

        #region "Eventos buscador exportador"
        protected void find_Click(object sender, EventArgs e)
        {

            try
            {

                string v_mensaje = string.Empty;

                var lookup = BTS_Exportador.Buscador_Exportador(txtfinder.Text, out v_mensaje);

                if (lookup != null && lookup.Count > 0)
                {
                    this.tablePaginationBuscador.DataSource = lookup;
                    this.tablePaginationBuscador.DataBind();

                    banmsg_det.InnerText = string.Empty;
                    banmsg_det.Visible = false;
                    UPBUSCADOR.Update();
                }
            }
            catch (Exception ex)
            {

                this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

            }

        }

        protected void tablePaginationBuscador_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                        int ID_EXPORTADOR = int.Parse(t.ToString());
                        string v_mensaje = string.Empty;

                      
                        objExportador = new BTS_Exportador();
                        objExportador.id = int.Parse(t);

                        if (objExportador.PopulateMyData(out v_mensaje))
                        {

                            //agregar novedad
                            objCabeceraBTS_OTROS = Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] as BTS_OTROS_Cabecera_Exportadores;

                            //existe pase a remover
                            var Detalle = objCabeceraBTS_OTROS.Detalle_Exportador.FirstOrDefault(f => f.Fila == (string.IsNullOrEmpty(this.TxtFila.Text) ? 0 : int.Parse(this.TxtFila.Text)));
                            if (Detalle != null)
                            {
                                Detalle.ruc_asume = objExportador.ruc;
                                Detalle.exportador_asume = objExportador.nombre;
                                Detalle.idExportador_asume = objExportador.id;

                                tablePagination.DataSource = objCabeceraBTS_OTROS.Detalle_Exportador;
                                tablePagination.DataBind();

                                Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS_OTROS;


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
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    this.BtnFacturar.Attributes.Add("disabled", "disabled");

                    OcultarLoading("2");
                    bool Ocultar_Mensaje = true;
                   
                   
                    string Msg = string.Empty;

                    this.LabelTotal.InnerText = string.Format("BODEGA FRIA/SECA");

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
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar la Referencia</b>"));
                        this.TXTMRN.Focus();
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

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdAgenteCodigo = string.Empty;
                    
 

                    /*detalle de exportadores*/
                    List<BTS_OTROS_Detalle_Exportadores> ListExportadores = BTS_OTROS_Detalle_Exportadores.Carga_CabecersExportadores_Eventos(this.TXTMRN.Text.Trim() , out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener detalle de exportadores....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                    /*ultima factura en caso de tener*/
                    var LinqExportadores = (from Tbl in ListExportadores.Where(Tbl => Tbl.cantidad != 0)
                                             select new
                                             {
                                                 Fila = Tbl.Fila,
                                                 idNave = Tbl.idNave ,
                                                 idStowageCab = Tbl.idStowageCab,
                                                 ruc = Tbl.ruc,
                                                 exportador = Tbl.exportador,
                                                 linea = Tbl.linea,
                                                 cantidad = Tbl.cantidad,
                                                 idExportador = Tbl.idExportador,
                                                 codLine = Tbl.codLine,
                                                 ruc_asume = Tbl.ruc,
                                                 exportador_asume = Tbl.exportador,
                                                 numero_factura = Tbl.numero_factura
                                             }).Distinct();

                    if (LinqExportadores != null)
                    {


                        //agrego todos los contenedores a la clase cabecera
                        objCabeceraBTS_OTROS = Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] as BTS_OTROS_Cabecera_Exportadores;


                        objCabeceraBTS_OTROS.FECHA = DateTime.Now;
                        objCabeceraBTS_OTROS.TIPO_CARGA = "BRBK";
                        objCabeceraBTS_OTROS.REFERENCIA = this.TXTMRN.Text.Trim() ;
                        objCabeceraBTS_OTROS.IV_USUARIO_CREA = ClsUsuario.loginname;
                        objCabeceraBTS_OTROS.SESION = this.hf_BrowserWindowName.Value;
                        objCabeceraBTS_OTROS.Detalle_Exportador.Clear();
                        

                        Int16 Secuencia = 1;
                        foreach (var Det in LinqExportadores)
                        {
                            objDetalleExportador = new BTS_OTROS_Detalle_Exportadores();

                            objDetalleExportador.Fila = Det.Fila;
                            objDetalleExportador.idNave = Det.idNave;
                            objDetalleExportador.codLine = Det.codLine;
                            objDetalleExportador.ruc = Det.ruc;
                            objDetalleExportador.exportador = Det.exportador;
                            objDetalleExportador.idStowageCab = Det.idStowageCab;
                            objDetalleExportador.cantidad = Det.cantidad;
                            objDetalleExportador.idExportador = Det.idExportador;
                            objDetalleExportador.linea = Det.linea;
                            objDetalleExportador.ruc_asume = Det.ruc;
                            objDetalleExportador.exportador_asume = Det.exportador;
                            objDetalleExportador.numero_factura = Det.numero_factura;
                            objDetalleExportador.referencia = Det.idNave;

                            objCabeceraBTS_OTROS.Detalle_Exportador.Add(objDetalleExportador);

                            Secuencia++;
                        }

                        tablePagination.DataSource = objCabeceraBTS_OTROS.Detalle_Exportador;
                        tablePagination.DataBind();

                        var TotalCajas = objCabeceraBTS_OTROS.Detalle_Exportador.Sum(x => x.cantidad);
                        objCabeceraBTS_OTROS.TOTAL_CAJA = TotalCajas;

                        this.LabelTotal.InnerText = string.Format("DETALLE DE EXPORTADORES - Total Cajas: {0}", objCabeceraBTS_OTROS.TOTAL_CAJA);

                       

                        Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS_OTROS;

                        this.BtnFacturar.Attributes.Remove("disabled");

                      

                        this.Actualiza_Panele_Detalle();

                        

                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        this.BtnFacturar.Attributes.Add("disabled", "disabled");//


                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de exportadores pendiente de facturar con el número de referencia {0} y cliente ingresado..</b>", this.TXTMRN.Text.Trim()));

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

        //proceso de generar facturas
        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

                CultureInfo enUS = new CultureInfo("en-US");
                NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
                string v_mensaje = string.Empty;
                string facturas = string.Empty;

                try
                {

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar la Referencia</b>"));
                        this.TXTMRN.Focus();
                        return;
                    }


                    objCabeceraBTS_OTROS = Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] as BTS_OTROS_Cabecera_Exportadores;

                    if (objCabeceraBTS_OTROS == null)
                    {
                        this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la factura </b>"));
                        return;
                    }
                    else
                    {
                        LoginName = ClsUsuario.loginname;

                        //recorro detalle de exportadores
                        foreach (var Detalle in objCabeceraBTS_OTROS.Detalle_Exportador.Where(p => string.IsNullOrEmpty(p.numero_factura)))
                        {

                           /***********************************************************************************************************************************************
                           *cargar conceptos para generar draf de factura
                           **********************************************************************************************************************************************/
                            var rubros = BTS_OTROS_Detalle_Rubros.Carga_DetalleRubros_Eventos_Agrupar(Detalle.referencia, Detalle.idExportador, out v_mensaje);
                            if (!String.IsNullOrEmpty(v_mensaje))
                            {

                                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener rubros de facturación de exportador....{0}</b>", cMensajes));
                                this.Actualiza_Paneles();
                                return;
                            }
                            if (rubros.Count == 0)
                            {
                                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen rubros asociados al exportador para poder facturar.....</b>"));
                                this.Actualiza_Paneles();
                                return;
                            }

                            Decimal Subtotal = 0;
                            Decimal Iva = 0;
                            Decimal Total = 0;

                            var list = new List<Tuple<string, string>>();

                            foreach (var item in rubros)
                            {
                                list.Add(Tuple.Create(item.codigoN4, string.Format("{0},{1},{2}", item.value_tarifa_N4, item.cantidad.ToString().Replace(".00", ""), item.comentario)));
                            }

                            objCabeceraBTS_OTROS.ID_CLIENTE = Detalle.ruc;
                            objCabeceraBTS_OTROS.DESC_CLIENTE = Detalle.exportador;
                            objCabeceraBTS_OTROS.ID_FACTURADO = Detalle.ruc_asume;
                            objCabeceraBTS_OTROS.DESC_FACTURADO = Detalle.exportador_asume;
                            objCabeceraBTS_OTROS.FECHA = DateTime.Now.Date;
                            objCabeceraBTS_OTROS.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objCabeceraBTS_OTROS.TIPO_CARGA = "BRBK";
                            objCabeceraBTS_OTROS.REFERENCIA = Detalle.idNave;
                            objCabeceraBTS_OTROS.TOTAL_CAJA = Detalle.cantidad;

                            objCabeceraBTS_OTROS.Detalle_Rubros.Clear();
                            objCabeceraBTS_OTROS.Detalle_Exportador_Unico.Clear();
                            objCabeceraBTS_OTROS.Detalle_Rubros_Factura.Clear();

                            objDetalleExportadorUnico = new BTS_OTROS_Detalle_ExportadoresUnico();
                            objDetalleExportadorUnico.ID = Detalle.ID;
                            objDetalleExportadorUnico.Fila = Detalle.Fila;
                            objDetalleExportadorUnico.idNave = Detalle.idNave;
                            objDetalleExportadorUnico.codLine = Detalle.codLine;
                            objDetalleExportadorUnico.ruc = Detalle.ruc;
                            objDetalleExportadorUnico.exportador = Detalle.exportador;
                            objDetalleExportadorUnico.idStowageCab = Detalle.idStowageCab;
                            objDetalleExportadorUnico.idExportador = Detalle.idExportador;
                            objDetalleExportadorUnico.cantidad = Detalle.cantidad;
                            objDetalleExportadorUnico.ruc_asume = Detalle.ruc_asume;
                            objDetalleExportadorUnico.exportador_asume = Detalle.exportador_asume;
                            objDetalleExportadorUnico.idExportador_asume = Detalle.idExportador_asume;
                            objCabeceraBTS_OTROS.Detalle_Exportador_Unico.Add(objDetalleExportadorUnico);


                            Fila = 1;
                            foreach (var Det in rubros)
                            {
                                objDetalleExportadorRubros = new BTS_OTROS_Detalle_Rubros();
                                objDetalleExportadorRubros.Fila = Fila;
                                objDetalleExportadorRubros.idNave = Det.idNave;
                                objDetalleExportadorRubros.ruc = Det.ruc;
                                objDetalleExportadorRubros.exportador = Det.exportador;
                                objDetalleExportadorRubros.idStowageCab = Det.idStowageCab;
                                objDetalleExportadorRubros.idExportador = Det.idExportador;
                                objDetalleExportadorRubros.cantidad = Det.cantidad;
                                objDetalleExportadorRubros.ruc_asume = Det.ruc_asume;
                                objDetalleExportadorRubros.codigo = Det.codigo;
                                objDetalleExportadorRubros.codigoN4 = Det.codigoN4;
                                objDetalleExportadorRubros.nombre = Det.nombre;
                                objDetalleExportadorRubros.comentario = Det.comentario;
                                objDetalleExportadorRubros.value_tarifa_N4 = Det.value_tarifa_N4;
                                objDetalleExportadorRubros.invoicetype = Det.invoicetype;


                                objDetalleExportadorRubros.IV_USUARIO_CREA = LoginName;
                                objDetalleExportadorRubros.IV_FECHA_CREA = DateTime.Now;


                                Fila++;
                                objCabeceraBTS_OTROS.Detalle_Rubros.Add(objDetalleExportadorRubros);
                            }

                            /*ultima factura en caso de tener*/
                            var LinqInvoiceType = (from Tbl in rubros.Where(Tbl => Tbl.cantidad != 0)
                                                   select new
                                                   {
                                                       INVOICETYPE = Tbl.invoicetype,
                                                       CODIGO_TARIJA_N4 = Tbl.codigoN4,
                                                       VALUE_TARIJA_N4 = Tbl.value_tarifa_N4,
                                                       CONCEPTO = Tbl.comentario,
                                                       CANTIDAD = Tbl.cantidad,
                                                   }).FirstOrDefault();


                            /***********************************************************************************************************************************************
                           *datos del cliente N4, días de crédito 
                           **********************************************************************************************************************************************/
                            var Cliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, objCabeceraBTS_OTROS.ID_FACTURADO);
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

                                    objCabeceraBTS_OTROS.DIR_FACTURADO = (ListaCliente.CLNT_ADRESS == null ? "" : ListaCliente.CLNT_ADRESS);
                                    objCabeceraBTS_OTROS.EMAIL_FACTURADO = (ListaCliente.CLNT_EMAIL == null ? "" : ListaCliente.CLNT_EMAIL);
                                    objCabeceraBTS_OTROS.CIUDAD_FACTURADO = string.Empty;
                                    objCabeceraBTS_OTROS.DIAS_CREDITO = (ListaCliente.DIAS_CREDITO == null ? 0 : ListaCliente.DIAS_CREDITO.Value);
                                }
                                else
                                {
                                    this.Limpia_Asume_Factura();
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabeceraBTS_OTROS.ID_FACTURADO));

                                    return;
                                }
                            }
                            else
                            {
                                this.Limpia_Asume_Factura();
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabeceraBTS_OTROS.ID_FACTURADO, objCabeceraBTS_OTROS.DESC_FACTURADO));

                                return;

                            }



                            /***********************************************************************************************************************************************
                           *4) Consulta de Servicios a facturar N4 
                           **********************************************************************************************************************************************/
                            string request = string.Empty;
                            string response = string.Empty;
                            Respuesta.ResultadoOperacion<bool> resp;
                            resp = ServicioSCA.CargarServicioBTS(LinqInvoiceType.INVOICETYPE, objCabeceraBTS_OTROS.ID_FACTURADO, objCabeceraBTS_OTROS.REFERENCIA, list, Page.User.Identity.Name.ToUpper(), out request, out response);
                            if (resp.Exitoso)
                            {
                                var v_result = resp.MensajeInformacion.Split(',');
                                decimal v_monto = 0;
                                v_monto = decimal.Parse(v_result[1].ToString());
                                string Draf = v_result[0];

                                objCabeceraBTS_OTROS.DRAF = Draf;


                                /*detalle de muelles*/
                                List<BTS_OTROS_Detalle_Rubros_Factura> ListServicios = BTS_OTROS_Detalle_Rubros_Factura.Carga_Servicios_Draf(Draf, out cMensajes);
                                if (!String.IsNullOrEmpty(cMensajes))
                                {

                                    this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener servicios de facturación del draf....{0}/ Error:{1}</b>", Draf, cMensajes));
                                    this.Actualiza_Panele_Detalle();
                                    return;
                                }

                                foreach (var Det in ListServicios)
                                {

                                    objDetalleExportadorRubrosFactura = new BTS_OTROS_Detalle_Rubros_Factura();
                                    objDetalleExportadorRubrosFactura.Fila = Fila;
                                    objDetalleExportadorRubrosFactura.final_nbr = Det.final_nbr;
                                    objDetalleExportadorRubrosFactura.draft_nbr = Det.draft_nbr;
                                    objDetalleExportadorRubrosFactura.payee_customer_name = Det.payee_customer_name;
                                    objDetalleExportadorRubrosFactura.currency_id = Det.currency_id;
                                    objDetalleExportadorRubrosFactura.finalized_date = Det.finalized_date;
                                    objDetalleExportadorRubrosFactura.gkey = Det.gkey;
                                    objDetalleExportadorRubrosFactura.event_type_id = Det.event_type_id;
                                    objDetalleExportadorRubrosFactura.event_id = Det.event_id;
                                    objDetalleExportadorRubrosFactura.quantity_billed = Det.quantity_billed;
                                    objDetalleExportadorRubrosFactura.quantity = Det.quantity;
                                    objDetalleExportadorRubrosFactura.rate_billed = Det.rate_billed;
                                    objDetalleExportadorRubrosFactura.amount = Det.amount;
                                    objDetalleExportadorRubrosFactura.description = Det.description;
                                    objDetalleExportadorRubrosFactura.created = Det.created;
                                    objDetalleExportadorRubrosFactura.tax = Det.tax;
                                    objDetalleExportadorRubrosFactura.amount_taxt = Det.amount_taxt;
                                    objDetalleExportadorRubrosFactura.notes = Det.notes;

                                    objDetalleExportadorRubrosFactura.IV_USUARIO_CREA = LoginName;
                                    objDetalleExportadorRubrosFactura.IV_FECHA_CREA = DateTime.Now;


                                    Fila++;
                                    objCabeceraBTS_OTROS.Detalle_Rubros_Factura.Add(objDetalleExportadorRubrosFactura);

                                }


                                Subtotal = Decimal.Round(ListServicios.Sum(p => p.amount), 2);
                                Iva = Decimal.Round(ListServicios.Sum(p => p.amount_taxt), 2);
                                Total = Subtotal + Iva;

                                objCabeceraBTS_OTROS.SUBTOTAL = Subtotal;
                                objCabeceraBTS_OTROS.IVA = Iva;
                                objCabeceraBTS_OTROS.TOTAL = Total;


                                /*proceso finalizar draft de factura*/
                                List<String> Lista = new List<String>();
                                Lista.Add(Draf);

                                var BillingFin = new N4Ws.Entidad.billing();
                                MergeInvoiceRequest Fin = new MergeInvoiceRequest();
                                Fin.finalizeDate = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm");// FechaPaidThruDay;
                                Fin.drftInvoiceNbrs = Lista;
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

                                    objCabeceraBTS_OTROS.NUMERO_FACTURA = FacturaFinal;
                                    //actualizo numero de factura
                                    Detalle.numero_factura = FacturaFinal;

                                    facturas = facturas + string.Format("{0}, ", FacturaFinal);

                                    this.Mostrar_Mensaje(4, string.Format("<b>Informativo! Se genero la siguiente factura # {0} con éxito</b>", FacturaFinal));
                                }
                                else
                                {
                                    this.Mostrar_Mensaje(4, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Problemas al finalizar el DRAFT # {0}", Draf));
                                    return;
                                }

                            }//fin genera factura
                            else
                            {
                                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! No se logró realizar la facturación en N4...{0} </b>", resp.MensajeProblema));
                                return;

                            }

                            Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS_OTROS;

                            objCabeceraBTS_OTROS = Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] as BTS_OTROS_Cabecera_Exportadores;


                            /*nuevo proceso de grabado*/
                            System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new System.Xml.Linq.XElement("FACT_CABECERA",
                                                                    new System.Xml.Linq.XElement("CABECERA",
                                                                    new System.Xml.Linq.XAttribute("GLOSA", objCabeceraBTS_OTROS.GLOSA == null ? "" : objCabeceraBTS_OTROS.GLOSA),
                                                                    new System.Xml.Linq.XAttribute("FECHA", objCabeceraBTS_OTROS.FECHA == null ? DateTime.Parse("1900/01/01") : objCabeceraBTS_OTROS.FECHA),
                                                                    new System.Xml.Linq.XAttribute("ID_CLIENTE", objCabeceraBTS_OTROS.ID_CLIENTE == null ? "" : objCabeceraBTS_OTROS.ID_CLIENTE),
                                                                    new System.Xml.Linq.XAttribute("DESC_CLIENTE", objCabeceraBTS_OTROS.DESC_CLIENTE == null ? "" : objCabeceraBTS_OTROS.DESC_CLIENTE),
                                                                    new System.Xml.Linq.XAttribute("ID_FACTURADO", objCabeceraBTS_OTROS.ID_FACTURADO == null ? "" : objCabeceraBTS_OTROS.ID_FACTURADO),
                                                                    new System.Xml.Linq.XAttribute("DESC_FACTURADO", objCabeceraBTS_OTROS.DESC_FACTURADO == null ? "" : objCabeceraBTS_OTROS.DESC_FACTURADO),
                                                                    new System.Xml.Linq.XAttribute("SUBTOTAL", objCabeceraBTS_OTROS.SUBTOTAL),
                                                                    new System.Xml.Linq.XAttribute("IVA", objCabeceraBTS_OTROS.IVA),
                                                                    new System.Xml.Linq.XAttribute("TOTAL", objCabeceraBTS_OTROS.TOTAL),
                                                                    new System.Xml.Linq.XAttribute("DRAF", objCabeceraBTS_OTROS.DRAF == null ? "" : objCabeceraBTS_OTROS.DRAF),
                                                                    new System.Xml.Linq.XAttribute("REFERENCIA", objCabeceraBTS_OTROS.REFERENCIA == null ? "" : objCabeceraBTS_OTROS.REFERENCIA),
                                                                    new System.Xml.Linq.XAttribute("DIR_FACTURADO", objCabeceraBTS_OTROS.DIR_FACTURADO == null ? "" : objCabeceraBTS_OTROS.DIR_FACTURADO),
                                                                    new System.Xml.Linq.XAttribute("EMAIL_FACTURADO", objCabeceraBTS_OTROS.EMAIL_FACTURADO == null ? "" : objCabeceraBTS_OTROS.EMAIL_FACTURADO),
                                                                    new System.Xml.Linq.XAttribute("CIUDAD_FACTURADO", objCabeceraBTS_OTROS.CIUDAD_FACTURADO == null ? "" : objCabeceraBTS_OTROS.CIUDAD_FACTURADO),
                                                                    new System.Xml.Linq.XAttribute("DIAS_CREDITO", objCabeceraBTS_OTROS.DIAS_CREDITO),
                                                                    new System.Xml.Linq.XAttribute("USUARIO_CREA", objCabeceraBTS_OTROS.IV_USUARIO_CREA == null ? "" : objCabeceraBTS_OTROS.IV_USUARIO_CREA),
                                                                    new System.Xml.Linq.XAttribute("CANTIDAD", objCabeceraBTS_OTROS.TOTAL_CAJA),
                                                                    new System.Xml.Linq.XAttribute("FACTURA", objCabeceraBTS_OTROS.NUMERO_FACTURA),
                                                                    new System.Xml.Linq.XAttribute("RUC_USUARIO", objCabeceraBTS_OTROS.RUC_USUARIO == null ? "" : objCabeceraBTS_OTROS.RUC_USUARIO),
                                                                    new System.Xml.Linq.XAttribute("DESC_USUARIO", objCabeceraBTS_OTROS.DESC_USUARIO == null ? "" : objCabeceraBTS_OTROS.DESC_USUARIO),
                                                                    new System.Xml.Linq.XAttribute("flag", "I"))));


                            System.Xml.Linq.XDocument XMLServicios = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                           new System.Xml.Linq.XElement("FACT_SERVICIOS", from p in objCabeceraBTS_OTROS.Detalle_Rubros_Factura.AsEnumerable().AsParallel()
                                                                          select new System.Xml.Linq.XElement("DETALLE",
                                                                               new System.Xml.Linq.XAttribute("draft_nbr", p.draft_nbr),
                                                                               new System.Xml.Linq.XAttribute("payee_customer_id", p.payee_customer_id == null ? "" : p.payee_customer_id.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("payee_customer_name", p.payee_customer_name == null ? "" : p.payee_customer_name.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("currency_id", p.currency_id == null ? "" : p.currency_id.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("finalized_date", p.finalized_date == null ? DateTime.Parse("1900/01/01") : p.finalized_date),
                                                                               new System.Xml.Linq.XAttribute("gkey", p.gkey),
                                                                               new System.Xml.Linq.XAttribute("event_type_id", p.event_type_id == null ? "" : p.event_type_id.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("event_id", p.event_id == null ? "" : p.event_id.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("quantity_billed", p.quantity_billed),
                                                                               new System.Xml.Linq.XAttribute("quantity", p.quantity),
                                                                               new System.Xml.Linq.XAttribute("rate_billed", p.rate_billed),
                                                                               new System.Xml.Linq.XAttribute("amount", p.amount),
                                                                               new System.Xml.Linq.XAttribute("description", p.description == null ? "" : p.description.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("tax", p.tax),
                                                                               new System.Xml.Linq.XAttribute("amount_taxt", p.amount_taxt),
                                                                               new System.Xml.Linq.XAttribute("notes", p.notes == null ? "" : p.notes.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("flag", "I"))));

                            System.Xml.Linq.XDocument XMLExportador = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                             new System.Xml.Linq.XElement("FACT_EXPORTADOR", from p in objCabeceraBTS_OTROS.Detalle_Exportador_Unico.AsEnumerable().AsParallel()
                                                                             select new System.Xml.Linq.XElement("DETALLE",
                                                                                new System.Xml.Linq.XAttribute("idNave", p.idNave == null ? "" : p.idNave.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("codLine", p.codLine == null ? "" : p.codLine.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("linea", p.linea == null ? "" : p.linea.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("ruc", p.ruc == null ? "" : p.ruc.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("exportador", p.exportador == null ? "" : p.exportador.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("idStowageCab", p.idStowageCab),
                                                                                new System.Xml.Linq.XAttribute("idExportador", p.idExportador),
                                                                                new System.Xml.Linq.XAttribute("cantidad", p.cantidad),
                                                                                new System.Xml.Linq.XAttribute("ruc_asume", p.ruc_asume == null ? "" : p.ruc_asume.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("exportador_asume", p.exportador_asume == null ? "" : p.exportador_asume.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("idExportador_asume", p.idExportador_asume),
                                                                                new System.Xml.Linq.XAttribute("flag", "I"))));

                            System.Xml.Linq.XDocument XMLRubros = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new System.Xml.Linq.XElement("FACT_RUBROS", from p in objCabeceraBTS_OTROS.Detalle_Rubros.AsEnumerable().AsParallel()
                                                                        select new System.Xml.Linq.XElement("DETALLE",
                                                                            new System.Xml.Linq.XAttribute("idNave", p.idNave == null ? "" : p.idNave.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("ruc", p.ruc == null ? "" : p.ruc.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("exportador", p.exportador == null ? "" : p.exportador.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("idStowageCab", p.idStowageCab),
                                                                            new System.Xml.Linq.XAttribute("idExportador", p.idExportador),
                                                                            new System.Xml.Linq.XAttribute("cantidad", p.cantidad),
                                                                            new System.Xml.Linq.XAttribute("ruc_asume", p.ruc_asume == null ? "" : p.ruc_asume.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("codigo", p.codigo == null ? "" : p.codigo.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("codigoN4", p.codigo == null ? "" : p.codigoN4.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("nombre", p.codigo == null ? "" : p.nombre.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("comentario", p.comentario == null ? "" : p.comentario.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("value_tarifa_N4", p.value_tarifa_N4 == null ? "" : p.value_tarifa_N4.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("invoicetype", p.invoicetype == null ? "" : p.invoicetype.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("flag", "I"))));

                            cMensajes = string.Empty;
                            string cMensajeActualizados = string.Empty;

                            objProcesaFactura = new BTS_OTROS_Procesa_Factura();
                            objProcesaFactura.xmlCabecera = XMLCabecera.ToString();
                            objProcesaFactura.xmlExportador = XMLExportador.ToString();
                            objProcesaFactura.xmlRubros = XMLRubros.ToString();
                            objProcesaFactura.xmlServicios = XMLServicios.ToString();

                            var nProceso = objProcesaFactura.SaveTransaction_BTS_Exportador_Eventos(out cMensajes);
                            /*fin de nuevo proceso de grabado*/
                            if (!nProceso.HasValue || nProceso.Value <= 0)
                            {

                                this.Mostrar_Mensaje(4, string.Format("<b>Error! No se pudo grabar datos de la factura del exportador seleccionado..{0}</b>", cMensajes));


                                /*************************************************************************************************************************************
                                * crear caso salesforce
                                * **********************************************************************************************************************************/

                                this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación BODEGA BTS", "Error al grabar", cMensajes, objCabeceraBTS_OTROS.REFERENCIA, objCabeceraBTS_OTROS.DESC_FACTURADO, objCabeceraBTS_OTROS.NUMERO_FACTURA, out MensajeCasos);

                                return;
                            }
                            else
                            {


                                this.Mostrar_Mensaje(4, string.Format("<b>Informativo! Se genero la siguiente factura # {0} con éxito</b>", objCabeceraBTS_OTROS.NUMERO_FACTURA));
                            }



                        }//fin detalle de exportadores

                        if (!string.IsNullOrEmpty(facturas))
                        {
                            this.Mostrar_Mensaje(4, string.Format("<b>Informativo! Se genero la siguiente factura # {0} con éxito</b>", facturas));
                        }
                        else
                        {
                            this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen facturas pendientes para procesar. </b>"));
                            return;
                        }


                        tablePagination.DataSource = objCabeceraBTS_OTROS.Detalle_Exportador;
                        tablePagination.DataBind();
                        this.Actualiza_Panele_Detalle();

                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "Factura BTS Exportador", false, null, null, ex.StackTrace, ex);
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