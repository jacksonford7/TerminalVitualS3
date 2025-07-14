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


    public partial class confirmarcajasagencia : System.Web.UI.Page
    {


        #region "Clases"
        private static Int64? lm = -3;
        private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
       

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

     
        private P2D_Tarifario objTarifa = new P2D_Tarifario();
        private P2D_Tarja_Cfs objTarja = new P2D_Tarja_Cfs();


        private BTS_Prev_Cabecera objCabeceraBTS = new BTS_Prev_Cabecera();
        private Cls_Prev_Detalle_Bodega objDetalleBodega = new Cls_Prev_Detalle_Bodega();
        private Cls_Prev_Detalle_Muelle objDetalleMuelle = new Cls_Prev_Detalle_Muelle();

        private BTS_Prev_Cabecera objFacturaBTS = new BTS_Prev_Cabecera();
        private Cls_Prev_Detalle_Bodega objDetalleFacturaBodega = new Cls_Prev_Detalle_Bodega();
        private Cls_Prev_Detalle_Muelle objDetalleFacturaMuelle = new Cls_Prev_Detalle_Muelle();
        private Cls_Prev_Detalle_Servicios objDetalleFacturaServicios = new Cls_Prev_Detalle_Servicios();
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
          
            UPCARGA.Update();
           
            
            UPBOTONES.Update();

        }

        private void Actualiza_Panele_Detalle()
        {
           
            UPMUELLE.Update();
        }

        private void Limpia_Datos_cliente()
        {
          //  this.TXTCLIENTE.Text = string.Empty;
           
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
            Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabecera;
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
                detalle_carga.Titulo = "Modulo de Facturación Bodegas BTS"; //opcional
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

                    objCabeceraBTS = new BTS_Prev_Cabecera();
                    Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;

                   


                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", ex.Message));
            }
        }


        #endregion




        #region "Eventos de la grilla muelle"
        protected void tableMuelle_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void tableMuelle_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
        }


        protected void TxtNewCantidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox COMENTARIO = (TextBox)sender;
                RepeaterItem item = (RepeaterItem)COMENTARIO.NamingContainer;

                Label SECUENCIA = (Label)item.FindControl("LblFila");

                objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;

              
                var Detalle = objCabeceraBTS.Detalle_muelle.FirstOrDefault(f => f.Fila == int.Parse(SECUENCIA.Text));
                if (Detalle != null)
                {

                    int cajas_confirmar;

                    if (!int.TryParse(COMENTARIO.Text.Trim(), out cajas_confirmar))
                    {
                        cajas_confirmar = 0;
                    }

                    Detalle.cajas_confirmar = cajas_confirmar;


                }

                tableMuelle.DataSource = objCabeceraBTS.Detalle_muelle;
                tableMuelle.DataBind();

                var TotalMuelles = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas_confirmar);
                var TotalPaletizado = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas_paletizado);

                objCabeceraBTS.TOTAL_CAJAS_MUELLE = TotalMuelles + TotalPaletizado;

                this.LabelMuelle.InnerText = string.Format("DETALLE DE MUELLE - Total Cajas: {0}, Granel: {1}, Paletizado: {2}", objCabeceraBTS.TOTAL_CAJAS_MUELLE, TotalMuelles, TotalPaletizado);

                //objCabeceraBTS.TOTAL_CAJAS_MUELLE = TotalMuelles;

                //this.LabelMuelle.InnerText = string.Format("DETALLE DE MUELLE - Total Cajas: {0}", objCabeceraBTS.TOTAL_CAJAS_MUELLE);


                Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;

               

                this.Actualiza_Panele_Detalle();



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }


        }

        protected void TxtPaletizado_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox TxtPaletizado = (TextBox)sender;
                RepeaterItem item = (RepeaterItem)TxtPaletizado.NamingContainer;

                Label SECUENCIA = (Label)item.FindControl("LblFila");

                objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;


                var Detalle = objCabeceraBTS.Detalle_muelle.FirstOrDefault(f => f.Fila == int.Parse(SECUENCIA.Text));
                if (Detalle != null)
                {

                    int cajas_paletizado;

                    if (!int.TryParse(TxtPaletizado.Text.Trim(), out cajas_paletizado))
                    {
                        cajas_paletizado = 0;
                    }

                    Detalle.cajas_paletizado = cajas_paletizado;


                }

                tableMuelle.DataSource = objCabeceraBTS.Detalle_muelle;
                tableMuelle.DataBind();

                var TotalMuelles = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas_confirmar);
                var TotalPaletizado = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas_paletizado);

                objCabeceraBTS.TOTAL_CAJAS_MUELLE = TotalMuelles + TotalPaletizado;

                this.LabelMuelle.InnerText = string.Format("DETALLE DE MUELLE - Total Cajas: {0}, Granel: {1}, Paletizado: {2}", objCabeceraBTS.TOTAL_CAJAS_MUELLE, TotalMuelles, TotalPaletizado);


                Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;



                this.Actualiza_Panele_Detalle();



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

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
                    



                    /*detalle de muelles*/
                    List<BTS_Detalle_Muelle> Listmuelle = BTS_Detalle_Muelle.Carga_DetalleMuelle_Confirmar(this.TXTMRN.Text.Trim(), out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener detalle de muelles....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                    /*ultima factura en caso de tener*/
                    var LinqMuelles = (from Tbl in Listmuelle.Where(Tbl => Tbl.cajas != 0)
                                        select new
                                        {
                                            Fila = Tbl.Fila,
                                            idNave = Tbl.idNave,
                                            codLine = Tbl.codLine,
                                            nave = Tbl.nave,
                                            aisv_codig_clte = Tbl.aisv_codig_clte,
                                            aisv_nom_expor = Tbl.aisv_nom_expor,
                                            aisv_estado = Tbl.aisv_estado,
                                            cajas = Tbl.cajas,
                                            cajas_confirmar = Tbl.cajas_confirmar,
                                            ruc = Tbl.ruc,
                                            Exportador = Tbl.Exportador,
                                            idExportador = Tbl.idExportador,
                                            cajas_paletizado = Tbl.cajas_paletizado,
                                        }).Distinct();




                    //agrego todos los contenedores a la clase cabecera
                    objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;


                    objCabeceraBTS.FECHA = DateTime.Now;
                    objCabeceraBTS.TIPO_CARGA = "BRBK";
                    objCabeceraBTS.REFERENCIA = this.TXTMRN.Text.Trim() ;
                    objCabeceraBTS.IV_USUARIO_CREA = ClsUsuario.loginname;
                    objCabeceraBTS.SESION = this.hf_BrowserWindowName.Value;


                    objCabeceraBTS.Detalle_bodega.Clear();
                    objCabeceraBTS.Detalle_muelle.Clear();

                    //muelle
                    int Secuencia = 1;

                    if (LinqMuelles != null)
                    {
                        foreach (var Det in LinqMuelles)
                        {
                            objDetalleMuelle = new Cls_Prev_Detalle_Muelle();

                            objDetalleMuelle.Fila = Det.Fila;
                            objDetalleMuelle.idNave = Det.idNave;
                            objDetalleMuelle.codLine = Det.codLine;
                            objDetalleMuelle.nave = Det.nave;
                            objDetalleMuelle.aisv_codig_clte = Det.aisv_codig_clte;
                            objDetalleMuelle.aisv_nom_expor = Det.aisv_nom_expor;
                            objDetalleMuelle.aisv_estado = Det.aisv_estado;
                            objDetalleMuelle.cajas = Det.cajas;
                            objDetalleMuelle.cajas_confirmar = Det.cajas_confirmar;
                            objDetalleMuelle.Exportador = Det.Exportador;
                            objDetalleMuelle.ruc = Det.ruc;
                            objDetalleMuelle.idExportador = Det.idExportador;
                            objDetalleMuelle.cajas_paletizado = Det.cajas_paletizado;

                            objCabeceraBTS.Detalle_muelle.Add(objDetalleMuelle);
                            Secuencia++;
                        }
                    }

                    tableMuelle.DataSource = objCabeceraBTS.Detalle_muelle;
                    tableMuelle.DataBind();

                    var TotalMuelles = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas);

                    var TotalGranel = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas_confirmar);
                    var TotalPaletizado = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas_paletizado);

                    objCabeceraBTS.TOTAL_CAJAS_MUELLE = TotalMuelles;

                    this.LabelMuelle.InnerText = string.Format("DETALLE DE MUELLE - Total Cajas: {0}, Granel: {1}, Paletizado: {2}", objCabeceraBTS.TOTAL_CAJAS_MUELLE, TotalGranel, TotalPaletizado);

                   // this.LabelMuelle.InnerText = string.Format("DETALLE DE MUELLE - Total Cajas: {0}", objCabeceraBTS.TOTAL_CAJAS_MUELLE);


                    Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;

                    this.BtnFacturar.Attributes.Remove("disabled");

                   

                    this.Actualiza_Panele_Detalle();

                  


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

                Response.Redirect("~/btsagencia/confirmarcajasagencia.aspx", false);



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

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

                CultureInfo enUS = new CultureInfo("en-US");
              

                try
                {

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar la Referencia</b>"));
                        this.TXTMRN.Focus();
                        return;
                    }

                    
                    //instancia sesion
                    objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;
                    if (objCabeceraBTS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder actualziar las cajas </b>"));
                        return;
                    }
                    if (objCabeceraBTS.Detalle_muelle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de cajas de muelle, para poder actualizar</b>"));
                        return;
                    }

                   

                    objCabeceraBTS.GLOSA = "";

                    //actualizo el objeto
                    Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;


                    Fila = 1;
                   
                    /***********************************************************************************************************************************************
                    *2) proceso para grabar factura
                    **********************************************************************************************************************************************/
                    /*************************************************************************************************************************************/
                    /*proceso para almacenar datos*/
                    objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;

                    System.Xml.Linq.XDocument XMLMuelle = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                         new System.Xml.Linq.XElement("MUELLE", from p in objCabeceraBTS.Detalle_muelle.AsEnumerable().AsParallel()
                                                                        select new System.Xml.Linq.XElement("DETALLE",
                                                                             new System.Xml.Linq.XAttribute("Fila", p.Fila),
                                                                             new System.Xml.Linq.XAttribute("idNave", p.idNave == null ? "" : p.idNave.ToString().Trim()),
                                                                             new System.Xml.Linq.XAttribute("Nave", p.nave == null ? "" : p.nave.ToString().Trim()),
                                                                             new System.Xml.Linq.XAttribute("codLine", p.codLine == null ? "" : p.codLine.ToString().Trim()),
                                                                             new System.Xml.Linq.XAttribute("idExportador", p.idExportador),
                                                                             new System.Xml.Linq.XAttribute("cajas", p.cajas),
                                                                             new System.Xml.Linq.XAttribute("Exportador", p.Exportador == null ? "" : p.Exportador.ToString().Trim()),
                                                                             new System.Xml.Linq.XAttribute("ruc", p.ruc == null ? "" : p.ruc.ToString().Trim()),
                                                                             new System.Xml.Linq.XAttribute("cajas_confirmar", p.cajas_confirmar),
                                                                             new System.Xml.Linq.XAttribute("cajas_paletizado", p.cajas_paletizado),
                                                                             new System.Xml.Linq.XAttribute("flag", "I"))));



                    //actualiza sesion
                    objCabeceraBTS.xmlCantidades = XMLMuelle.ToString();
                    objCabeceraBTS.IV_USUARIO_CREA = ClsUsuario.loginname;
                    var nProceso = objCabeceraBTS.SaveTransaction_Muelle(out cMensajes);

                    /*fin de nuevo proceso de grabado*/
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {

                        this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo actualizar las cantidades de cajas de muelle.{0}</b>", cMensajes));

                        return;
                    }
                    else
                    {
                        this.Ocultar_Mensaje();

                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se actualizaron: {0}, cajas de muelles con éxito</b>", objCabeceraBTS.Detalle_muelle.Count()));
                    }
                    Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "Confirmar Cajas Muelle BTS", false, null, null, ex.StackTrace, ex);
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