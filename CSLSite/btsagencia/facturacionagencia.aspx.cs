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

namespace CSLSite
{


    public partial class facturacionagencia : System.Web.UI.Page
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

      //  private Int64 Gkey = 0;
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
        //private int NDiasLibreas = 0;
        //private decimal NEstadoCuenta = 0;
        //private int NDiasZarpe = 0;
        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        private bool sap_valida = false;
        private bool tieneBloqueo = false;
        private decimal SaldoPendiente = 0;
        //private decimal ValorVencido = 0;
        //private decimal ValorPendiente = 0;
        private Int64 DiasCredito = 0;
        private decimal Cupo = 0;
        bool Bloqueo_Cliente = false;
        bool Liberado_Cliente = false;
        private string gkeyBuscado = string.Empty;

      
        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
        //private bool SinDesconsolidar = false;
        //private bool SinAutorizacion = false;
        //private bool Bloqueos = false;

        private static string TextoLeyenda = string.Empty;

        private static string TextoProforma = string.Empty;
        private static string TextoServicio = string.Empty;
        private string EmpresaSelect = string.Empty;
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
            UPMUELLE.Update();
        }

        private void Limpia_Datos_cliente()
        {
          //  this.TXTCLIENTE.Text = string.Empty;
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

      

        private void CboCargaLineas()
        {
            try
            {
                List<BTS_Lineas> Listado = BTS_Lineas.CboLineas(out cMensajes);


                /*para almacenar clientes que asumen factura*/
                List_Asume = new List<Cls_Bil_AsumeFactura>();
                List_Asume.Clear();
                List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = string.Empty, nombre = string.Empty, mostrar = string.Format("{0}", "Seleccionar...") });

                foreach(var Det in Listado)
                {
                    List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = Det.id.ToString().Trim(), nombre = Det.nombre.Trim(), mostrar = string.Format("{0} - {1}", Det.codLine.Trim(), Det.nombre.Trim()) });
                }
                this.CboAsumeFactura.DataSource = List_Asume;
                this.CboAsumeFactura.DataTextField = "mostrar";
                this.CboAsumeFactura.DataValueField = "ruc";
                this.CboAsumeFactura.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "CboCargaLineas", "Hubo un error al cargar ciudades", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }

        [System.Web.Services.WebMethod]
        public static string[] GetEmpresas(string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Exportador = N4.Entidades.Cliente.ObtenerClientes(ClsUsuario.loginname, prefix);
                if (Exportador.Exitoso)
                {
                    var LinqQuery = (from Tbl in Exportador.Resultado.Where(Tbl => Tbl.CLNT_CUSTOMER != null)
                                     select new
                                     {
                                         EMPRESA = string.Format("{0} - {1}", Tbl.CLNT_CUSTOMER.Trim(), Tbl.CLNT_NAME.Trim()),
                                         RUC = Tbl.CLNT_CUSTOMER.Trim(),
                                         NOMBRE = Tbl.CLNT_NAME.Trim(),
                                         ID = Tbl.CLNT_CUSTOMER.Trim()
                                     });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }

                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
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
                        this.CboCargaLineas();

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
                    //InvoiceBTS
                    objCabeceraBTS = new BTS_Prev_Cabecera();
                    Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;

                    this.TxtFechaHasta.Text = System.DateTime.Now.ToString("MM/dd/yyyy");


                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", ex.Message));
            }
        }


        #endregion
      
       #region "Eventos de la grilla bodega"

       
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


                    objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;

                    if (objCabeceraBTS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta</b>"));
                        return;
                    }
                    else
                    {
                        tablePagination.PageIndex = e.NewPageIndex;
                        tablePagination.DataSource = objCabeceraBTS.Detalle_bodega;
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
                        
                       

                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

                }
            }
        }



        #endregion


        #region "Eventos de la grilla muelle"
        protected void tableMuelle_PreRender(object sender, EventArgs e)
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


                    if (tableMuelle.Rows.Count > 0)
                    {
                        tableMuelle.UseAccessibleHeader = true;
                        // Agrega la sección THEAD y TBODY.
                        tableMuelle.HeaderRow.TableSection = TableRowSection.TableHeader;

                    }
                }



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

        protected void tableMuelle_PageIndexChanging(object sender, GridViewPageEventArgs e)
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


                    objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;

                    if (objCabeceraBTS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta</b>"));
                        return;
                    }
                    else
                    {
                        tableMuelle.PageIndex = e.NewPageIndex;
                        tableMuelle.DataSource = objCabeceraBTS.Detalle_muelle;
                        tableMuelle.DataBind();

                        this.Actualiza_Panele_Detalle();
                    }
                }


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }
        }

        protected void tableMuelle_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    string IdEmpresa = string.Empty;
                    string DesEmpresa = string.Empty;

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

                    if (this.CboAsumeFactura.SelectedIndex == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el cliente a facturar</b>"));
                        this.CboAsumeFactura.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Informativo! Debe ingresar el cliente a emitir la factura </b>"));
                        this.Txtempresa.Focus();
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
                    

                    string IdFacturado = this.CboAsumeFactura.SelectedValue.ToString();
                    var ExisteAsume = CboAsumeFactura.Items.FindByValue(IdFacturado);

                    if (ExisteAsume != null)
                    {
                       
                        this.hf_idasume.Value = ExisteAsume.Text.Split('-').ToList()[0].Trim();
                        this.hf_descasume.Value = ExisteAsume.Text.Split('-').ToList()[1].Trim();
                    }
                    else
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo obtener la información del cliente a facturar.</b>"));
                        return;
                    }

                    //valida que exista exportador
                    if (!string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        EmpresaSelect = this.Txtempresa.Text.Trim();
                        if (EmpresaSelect.Split('-').ToList().Count > 1)
                        {

                            IdEmpresa = EmpresaSelect.Split('-').ToList()[0].Trim();
                            DesEmpresa = EmpresaSelect.Split('-').ToList()[1].Trim();

                            var ExisteCliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, IdEmpresa);
                            if (!ExisteCliente.Exitoso)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos</b>", IdEmpresa, DesEmpresa));
                                this.Txtempresa.Focus();
                                return;

                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar el cliente de la lista. </b>"));
                            this.Txtempresa.Focus();
                            return;
                        }
                    }

                    //LLENADO DE CAMPOS DE PANTALLA DEL CLIENTE
                    var Cliente = N4.Entidades.Cliente.ObtenerCliente(ClsUsuario.loginname, IdEmpresa);
                    if (Cliente.Exitoso)
                    {
                        var ListaCliente = Cliente.Resultado;
                        if (ListaCliente != null)
                        {

                            this.TXTASUMEFACTURA.Text = ListaCliente.CLNT_CUSTOMER.Trim() + " - " + ListaCliente.CLNT_NAME.Trim();

                            this.hf_descasume.Value = ListaCliente.CLNT_NAME.Trim();

                        }
                        else
                        {

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente</b>"));
                            return;
                        }
                    }
                    else
                    {

                        Ocultar_Mensaje = false;

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", this.hf_idasume.Value, this.hf_descasume.Value));


                        /*************************************************************************************************************************************
                        * crear caso salesforce
                        ***********************************************************************************************************************************/
                        MensajesErrores = string.Format("Se presentaron los siguientes problemas: No existe información del cliente, no registrado: {0}", this.hf_descasume.Value);

                        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Bodega BTS", "No existe información del cliente, no registrado.", MensajesErrores.Trim(), string.Format("{0}", this.TXTMRN.Text),
                            this.hf_descasume.Value, this.hf_idasume.Value, out MensajeCasos, false);

                        /*************************************************************************************************************************************
                        * fin caso salesforce
                        **************************************************************************************************************************************/

                    }


                    //bodega fria/seca
                    if (this.CboTipoFactura.SelectedValue.Equals("1"))
                    {
                        /*detalle de bodegas*/
                        List<BTS_Detalle_Bodegas> ListBodegas = BTS_Detalle_Bodegas.Carga_DetalleBodegas(this.TXTMRN.Text.Trim(), this.hf_idasume.Value, out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener detalle de bodegas....{0}</b>", cMensajes));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                        /*ultima factura en caso de tener*/
                        var LinqBodegas = (from Tbl in ListBodegas.Where(Tbl => Tbl.QTY_out != 0)
                                           select new
                                           {
                                               Fila = Tbl.Fila,
                                               idNave = Tbl.idNave,
                                               codLine = Tbl.codLine,
                                               nave = Tbl.nave,
                                               ruc = Tbl.ruc,
                                               Exportador = Tbl.Exportador,
                                               booking = Tbl.booking,
                                               idModalidad = Tbl.idModalidad,
                                               desc_modalidad = Tbl.desc_modalidad,
                                               id_bodega = Tbl.id_bodega,
                                               desc_bodega = Tbl.desc_bodega,
                                               tipo_bodega = Tbl.tipo_bodega,
                                               id_tipo_bodega = Tbl.id_tipo_bodega,
                                               QTY_out = Tbl.QTY_out,
                                           }).Distinct();

                        if (LinqBodegas != null)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;


                            objCabeceraBTS.FECHA = DateTime.Now;
                            objCabeceraBTS.TIPO_CARGA = "FRIA/SECA";
                            objCabeceraBTS.ID_CLIENTE = this.hf_idasume.Value;
                            objCabeceraBTS.DESC_CLIENTE = this.hf_descasume.Value;
                            objCabeceraBTS.ID_FACTURADO = IdEmpresa;
                            objCabeceraBTS.DESC_FACTURADO = DesEmpresa.Trim();
                            objCabeceraBTS.REFERENCIA = this.TXTMRN.Text.Trim();
                            objCabeceraBTS.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objCabeceraBTS.SESION = this.hf_BrowserWindowName.Value;


                            objCabeceraBTS.Detalle_bodega.Clear();
                            objCabeceraBTS.Detalle_muelle.Clear();

                            Int16 Secuencia = 1;
                            foreach (var Det in LinqBodegas)
                            {
                                objDetalleBodega = new Cls_Prev_Detalle_Bodega();

                                objDetalleBodega.Fila = Det.Fila;
                                objDetalleBodega.idNave = Det.idNave;
                                objDetalleBodega.codLine = Det.codLine;
                                objDetalleBodega.nave = Det.nave;
                                objDetalleBodega.ruc = Det.ruc;
                                objDetalleBodega.Exportador = Det.Exportador;
                                objDetalleBodega.booking = Det.booking;
                                objDetalleBodega.idModalidad = Det.idModalidad;
                                objDetalleBodega.desc_modalidad = Det.desc_modalidad;
                                objDetalleBodega.id_bodega = Det.id_bodega;
                                objDetalleBodega.desc_bodega = Det.desc_bodega;
                                objDetalleBodega.tipo_bodega = Det.tipo_bodega;
                                objDetalleBodega.id_tipo_bodega = Det.id_tipo_bodega;
                                objDetalleBodega.QTY_out = Det.QTY_out;
                                objCabeceraBTS.Detalle_bodega.Add(objDetalleBodega);
                                Secuencia++;
                            }

                            tablePagination.DataSource = objCabeceraBTS.Detalle_bodega;
                            tablePagination.DataBind();

                            var TotalCajas = objCabeceraBTS.Detalle_bodega.Sum(x => x.QTY_out);
                            objCabeceraBTS.TOTAL_CAJAS_BODEGA = TotalCajas;

                            this.LabelTotal.InnerText = string.Format("DETALLE DE BODEGA FRIA/SECA - Total Cajas: {0}", objCabeceraBTS.TOTAL_CAJAS_BODEGA);


                            objCabeceraBTS.TOTAL_CAJAS_MUELLE = 0;

                            this.LabelMuelle.InnerText = string.Empty;

                            tableMuelle.DataSource = objCabeceraBTS.Detalle_muelle;
                            tableMuelle.DataBind();


                            Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;

                            this.BtnFacturar.Attributes.Remove("disabled");

                            this.CboAsumeFactura.Attributes.Add("disabled", "disabled");

                            this.Actualiza_Panele_Detalle();

                            this.UPDATOSCLIENTE.Update();

                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();

                            this.BtnFacturar.Attributes.Add("disabled", "disabled");//

                            this.CboAsumeFactura.Attributes.Remove("disabled");

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de bodega FRIA/SECA pendiente de facturar con el número de referencia {0} y cliente ingresado..{1}</b>", this.TXTMRN.Text.Trim(), this.hf_descasume.Value));

                            this.UPDATOSCLIENTE.Update();

                            this.Actualiza_Panele_Detalle();
                            return;

                        }

                    }

                    //bodega muelle
                    if (this.CboTipoFactura.SelectedValue.Equals("2"))
                    {

                        /*detalle de muelles*/
                        List<BTS_Detalle_Muelle> Listmuelle = BTS_Detalle_Muelle.Carga_DetalleMuelle(this.TXTMRN.Text.Trim(), this.hf_idasume.Value, out cMensajes);
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
                                               cajas_paletizado = Tbl.cajas_paletizado
                                           }).Distinct();



                        if (LinqMuelles != null)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;


                            objCabeceraBTS.FECHA = DateTime.Now;
                            objCabeceraBTS.TIPO_CARGA = "MUELLE";
                            objCabeceraBTS.ID_CLIENTE = this.hf_idasume.Value;
                            objCabeceraBTS.DESC_CLIENTE = this.hf_descasume.Value;
                            objCabeceraBTS.ID_FACTURADO = IdEmpresa;
                            objCabeceraBTS.DESC_FACTURADO = DesEmpresa.Trim();
                            objCabeceraBTS.REFERENCIA = this.TXTMRN.Text.Trim();
                            objCabeceraBTS.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objCabeceraBTS.SESION = this.hf_BrowserWindowName.Value;


                            objCabeceraBTS.Detalle_bodega.Clear();
                            objCabeceraBTS.Detalle_muelle.Clear();

                            Int16 Secuencia = 1;
                           

                            tablePagination.DataSource = objCabeceraBTS.Detalle_bodega;
                            tablePagination.DataBind();

                            var TotalCajas = objCabeceraBTS.Detalle_bodega.Sum(x => x.QTY_out);
                            objCabeceraBTS.TOTAL_CAJAS_BODEGA = TotalCajas;

                            this.LabelTotal.InnerText = string.Empty;

                            //muelle
                            Secuencia = 1;

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
                                    objDetalleMuelle.cajas_paletizado = Det.cajas_paletizado;
                                    objCabeceraBTS.Detalle_muelle.Add(objDetalleMuelle);
                                    Secuencia++;
                                }
                            }

                            tableMuelle.DataSource = objCabeceraBTS.Detalle_muelle;
                            tableMuelle.DataBind();

                            var TotalMuelles = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas);
                            var TotalPaletizado = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas_paletizado);
                            objCabeceraBTS.TOTAL_CAJAS_MUELLE = TotalMuelles + TotalPaletizado;

                            this.LabelMuelle.InnerText = string.Format("DETALLE DE MUELLE - Total Cajas: {0}, Total Granel: {1}, Total Paletizado: {2}", objCabeceraBTS.TOTAL_CAJAS_MUELLE, TotalMuelles, TotalPaletizado);


                            Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;

                            this.BtnFacturar.Attributes.Remove("disabled");

                            this.CboAsumeFactura.Attributes.Add("disabled", "disabled");

                            this.Actualiza_Panele_Detalle();

                            this.UPDATOSCLIENTE.Update();

                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();

                            this.BtnFacturar.Attributes.Add("disabled", "disabled");//

                            this.CboAsumeFactura.Attributes.Remove("disabled");

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de bodega MUELLE pendiente de facturar con el número de referencia {0} y cliente ingresado..{1}</b>", this.TXTMRN.Text.Trim(), this.hf_descasume.Value));

                            this.UPDATOSCLIENTE.Update();

                            this.Actualiza_Panele_Detalle();
                            return;

                        }




                    }



                    //if (LinqBodegas != null)
                    //{

                       



                    //    //agrego todos los contenedores a la clase cabecera
                    //    objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;


                    //    objCabeceraBTS.FECHA = DateTime.Now;
                    //    objCabeceraBTS.TIPO_CARGA = "BRBK";
                    //    objCabeceraBTS.ID_CLIENTE = this.hf_idasume.Value;
                    //    objCabeceraBTS.DESC_CLIENTE = this.hf_descasume.Value;
                    //    objCabeceraBTS.ID_FACTURADO = IdEmpresa;
                    //    objCabeceraBTS.DESC_FACTURADO = DesEmpresa.Trim();
                    //    objCabeceraBTS.REFERENCIA = this.TXTMRN.Text.Trim() ;
                    //    objCabeceraBTS.IV_USUARIO_CREA = ClsUsuario.loginname;
                    //    objCabeceraBTS.SESION = this.hf_BrowserWindowName.Value;


                    //    objCabeceraBTS.Detalle_bodega.Clear();
                    //    objCabeceraBTS.Detalle_muelle.Clear();

                    //    Int16 Secuencia = 1;
                    //    foreach (var Det in LinqBodegas)
                    //    {
                    //        objDetalleBodega = new Cls_Prev_Detalle_Bodega();

                    //        objDetalleBodega.Fila = Det.Fila;
                    //        objDetalleBodega.idNave = Det.idNave;
                    //        objDetalleBodega.codLine = Det.codLine;
                    //        objDetalleBodega.nave = Det.nave;
                    //        objDetalleBodega.ruc = Det.ruc;
                    //        objDetalleBodega.Exportador = Det.Exportador;
                    //        objDetalleBodega.booking = Det.booking;
                    //        objDetalleBodega.idModalidad = Det.idModalidad;
                    //        objDetalleBodega.desc_modalidad = Det.desc_modalidad;
                    //        objDetalleBodega.id_bodega = Det.id_bodega;
                    //        objDetalleBodega.desc_bodega = Det.desc_bodega;
                    //        objDetalleBodega.tipo_bodega = Det.tipo_bodega;
                    //        objDetalleBodega.id_tipo_bodega = Det.id_tipo_bodega;
                    //        objDetalleBodega.QTY_out = Det.QTY_out;
                    //        objCabeceraBTS.Detalle_bodega.Add(objDetalleBodega);
                    //        Secuencia++;
                    //    }

                    //    tablePagination.DataSource = objCabeceraBTS.Detalle_bodega;
                    //    tablePagination.DataBind();

                    //    var TotalCajas = objCabeceraBTS.Detalle_bodega.Sum(x => x.QTY_out);
                    //    objCabeceraBTS.TOTAL_CAJAS_BODEGA = TotalCajas;

                    //    this.LabelTotal.InnerText = string.Format("DETALLE DE BODEGA FRIA/SECA - Total Cajas: {0}", objCabeceraBTS.TOTAL_CAJAS_BODEGA);

                    //    //muelle
                    //    Secuencia = 1;

                    //    if (LinqMuelles != null)
                    //    {
                    //        foreach (var Det in LinqMuelles)
                    //        {
                    //            objDetalleMuelle = new Cls_Prev_Detalle_Muelle();

                    //            objDetalleMuelle.Fila = Det.Fila;
                    //            objDetalleMuelle.idNave = Det.idNave;
                    //            objDetalleMuelle.codLine = Det.codLine;
                    //            objDetalleMuelle.nave = Det.nave;
                    //            objDetalleMuelle.aisv_codig_clte = Det.aisv_codig_clte;
                    //            objDetalleMuelle.aisv_nom_expor = Det.aisv_nom_expor;
                    //            objDetalleMuelle.aisv_estado = Det.aisv_estado;
                    //            objDetalleMuelle.cajas = Det.cajas;
                    //            objCabeceraBTS.Detalle_muelle.Add(objDetalleMuelle);
                    //            Secuencia++;
                    //        }
                    //    }

                    //    tableMuelle.DataSource = objCabeceraBTS.Detalle_muelle;
                    //    tableMuelle.DataBind();

                    //    var TotalMuelles = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas);
                    //    objCabeceraBTS.TOTAL_CAJAS_MUELLE = TotalMuelles;

                    //    this.LabelMuelle.InnerText = string.Format("DETALLE DE MUELLE - Total Cajas: {0}", objCabeceraBTS.TOTAL_CAJAS_MUELLE);


                    //    Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;

                    //    this.BtnFacturar.Attributes.Remove("disabled");

                    //    this.CboAsumeFactura.Attributes.Add("disabled", "disabled");

                    //    this.Actualiza_Panele_Detalle();

                    //    this.UPDATOSCLIENTE.Update();

                    //}
                    //else
                    //{
                    //    tablePagination.DataSource = null;
                    //    tablePagination.DataBind();

                    //    this.BtnFacturar.Attributes.Add("disabled", "disabled");//

                    //    this.CboAsumeFactura.Attributes.Remove("disabled");

                    //    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de bodega FRIA/SECA pendiente de facturar con el número de referencia {0} y cliente ingresado..{1}</b>", this.TXTMRN.Text.Trim(), this.hf_descasume.Value));

                    //    this.UPDATOSCLIENTE.Update();

                    //    this.Actualiza_Panele_Detalle();
                    //    return;

                    //}





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

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

                CultureInfo enUS = new CultureInfo("en-US");
                NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;

                string IdEmpresa = string.Empty;
                string DesEmpresa = string.Empty;
                string INVOICETYPE = string.Empty;
                try
                {

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar la Referencia</b>"));
                        this.TXTMRN.Focus();
                        return;
                    }

                    if (this.CboAsumeFactura.SelectedIndex == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el cliente a facturar</b>"));
                        this.CboAsumeFactura.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Informativo! Debe ingresar el cliente a emitir la factura </b>"));
                        this.Txtempresa.Focus();
                        return;
                    }

                    HoraHasta = "00:00";
                    
                    //fecha hasta para sacar los servicios
                    Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la factura</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    //valida que exista exportador
                    if (!string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        EmpresaSelect = this.Txtempresa.Text.Trim();
                        if (EmpresaSelect.Split('-').ToList().Count > 1)
                        {

                            IdEmpresa = EmpresaSelect.Split('-').ToList()[0].Trim();
                            DesEmpresa = EmpresaSelect.Split('-').ToList()[1].Trim();

                            var ExisteCliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, IdEmpresa);
                            if (!ExisteCliente.Exitoso)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos</b>", IdEmpresa, DesEmpresa));
                                this.Txtempresa.Focus();
                                return;

                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar el cliente de la lista. </b>"));
                            this.Txtempresa.Focus();
                            return;
                        }
                    }


                    //instancia sesion
                    objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;
                    if (objCabeceraBTS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la factura </b>"));
                        return;
                    }
                    

                    var list = new List<Tuple<string, string>>();

                    /***********************************************************************************************************************************************
                    *cargar conceptos para generar draf de factura
                    **********************************************************************************************************************************************/
                    /*detalle de bodegas fria/seca*/
                    if (this.CboTipoFactura.SelectedValue.Equals("1"))
                    {

                        if (objCabeceraBTS.Detalle_bodega.Count == 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de bodega fria/seca, para poder generar la factura </b>"));
                            return;
                        }

                        List<BTS_Rubros> ListBodegas = BTS_Rubros.Carga_RubrosBodegas(this.TXTMRN.Text.Trim(), this.hf_idasume.Value, out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener rubros de facturación de bodegas....{0}</b>", cMensajes));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                        if (ListBodegas.Count == 0)
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen rubros asociados a las bodegas para poder facturar.....</b>"));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                        foreach (var item in ListBodegas)
                        {
                            list.Add(Tuple.Create(item.CODIGO_TARIJA_N4, string.Format("{0},{1},{2}", item.VALUE_TARIJA_N4, item.CANTIDAD, item.CONCEPTO)));
                        }

                        /*ultima factura en caso de tener*/
                        var LinqBodegas = (from Tbl in ListBodegas.Where(Tbl => Tbl.CANTIDAD != 0)
                                           select new
                                           {
                                               INVOICETYPE = Tbl.INVOICETYPE,
                                               CODIGO_TARIJA_N4 = Tbl.CODIGO_TARIJA_N4,
                                               VALUE_TARIJA_N4 = Tbl.VALUE_TARIJA_N4,
                                               CONCEPTO = Tbl.CONCEPTO,
                                               CANTIDAD = Tbl.CANTIDAD,
                                           }).FirstOrDefault();

                        INVOICETYPE = LinqBodegas.INVOICETYPE;

                    }


                    /*detalle de bodegas muelle*/
                    if (this.CboTipoFactura.SelectedValue.Equals("2"))
                    {

                        if (objCabeceraBTS.Detalle_muelle.Count == 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de bodega Muelle, para poder generar la factura </b>"));
                            return;
                        }

                        /*detalle de muelles*/
                        List<BTS_Rubros> Listmuelle = BTS_Rubros.Carga_RubrosMuelle(this.TXTMRN.Text.Trim(), this.hf_idasume.Value, out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener rubros de facturación de muelles....{0}</b>", cMensajes));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                        if (Listmuelle.Count == 0)
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen rubros asociados a las bodegas de Muelle para poder facturar.....</b>"));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                        foreach (var item in Listmuelle)
                        {
                            list.Add(Tuple.Create(item.CODIGO_TARIJA_N4, string.Format("{0},{1},{2}", item.VALUE_TARIJA_N4, item.CANTIDAD, item.CONCEPTO)));
                        }

                        /*ultima factura en caso de tener*/
                        var LinqMuelles = (from Tbl in Listmuelle.Where(Tbl => Tbl.CANTIDAD != 0)
                                           select new
                                           {
                                               INVOICETYPE = Tbl.INVOICETYPE,
                                               CODIGO_TARIJA_N4 = Tbl.CODIGO_TARIJA_N4,
                                               VALUE_TARIJA_N4 = Tbl.VALUE_TARIJA_N4,
                                               CONCEPTO = Tbl.CONCEPTO,
                                               CANTIDAD = Tbl.CANTIDAD,
                                           }).FirstOrDefault();

                        INVOICETYPE = LinqMuelles.INVOICETYPE;

                    }

   

                    /***********************************************************************************************************************************************
                    *fin
                    **********************************************************************************************************************************************/

                    /***********************************************************************************************************************************************
                    *datos del cliente N4, días de crédito 
                    **********************************************************************************************************************************************/
                    objCabeceraBTS.ID_FACTURADO = IdEmpresa;
                    objCabeceraBTS.DESC_FACTURADO = DesEmpresa.Trim();

                    var Cliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, objCabeceraBTS.ID_FACTURADO);
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

                            objCabeceraBTS.DIR_FACTURADO = (ListaCliente.CLNT_ADRESS == null ? "" : ListaCliente.CLNT_ADRESS);
                            objCabeceraBTS.EMAIL_FACTURADO = (ListaCliente.CLNT_EMAIL == null ? "" : ListaCliente.CLNT_EMAIL);
                            objCabeceraBTS.CIUDAD_FACTURADO = string.Empty;
                            objCabeceraBTS.DIAS_CREDITO = (ListaCliente.DIAS_CREDITO == null ? 0 : ListaCliente.DIAS_CREDITO.Value);
                        }
                        else
                        {
                            this.Limpia_Asume_Factura();
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabeceraBTS.ID_FACTURADO));

                            return;
                        }
                    }
                    else
                    {
                        this.Limpia_Asume_Factura();
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", objCabeceraBTS.ID_FACTURADO, objCabeceraBTS.DESC_FACTURADO));

                        return;

                    }


                    /***********************************************************************************************************************************************
                    *fin: consulta datos de cliente
                    **********************************************************************************************************************************************/
                   


                    objCabeceraBTS.GLOSA = "";


                    //actualizo el objeto
                    Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;


                    Fila = 1;
                    Decimal Subtotal = 0;
                    Decimal Iva = 0;
                    Decimal Total = 0;
                    /***********************************************************************************************************************************************
                    *2) proceso para grabar factura
                    **********************************************************************************************************************************************/
                    /*************************************************************************************************************************************/
                    /*proceso para almacenar datos*/
                    objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;

                    /*agrego datos a la factura*/
                    objFacturaBTS = Session["InvoiceBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;

                    //InvoiceBTS
                    //objFacturaBTS = new BTS_Prev_Cabecera();
                    objFacturaBTS.Detalle_bodega.Clear();
                    objFacturaBTS.Detalle_muelle.Clear();
                    objFacturaBTS.Detalle_Servicios.Clear();
                    
                    /*cabecera de factura*/
                    objFacturaBTS.ID = objCabeceraBTS.ID;
                    objFacturaBTS.GLOSA = objCabeceraBTS.GLOSA;
                    objFacturaBTS.FECHA = objCabeceraBTS.FECHA;
                    objFacturaBTS.TIPO_CARGA = objCabeceraBTS.TIPO_CARGA;
                    objFacturaBTS.ID_CLIENTE = objCabeceraBTS.ID_CLIENTE;
                    objFacturaBTS.DESC_CLIENTE = objCabeceraBTS.DESC_CLIENTE;
                    objFacturaBTS.ID_FACTURADO = objCabeceraBTS.ID_FACTURADO;
                    objFacturaBTS.DESC_FACTURADO = objCabeceraBTS.DESC_FACTURADO;
                    objFacturaBTS.REFERENCIA = objCabeceraBTS.REFERENCIA;
                    objFacturaBTS.SUBTOTAL = objCabeceraBTS.SUBTOTAL;
                    objFacturaBTS.IVA = objCabeceraBTS.IVA;
                    objFacturaBTS.TOTAL = objCabeceraBTS.TOTAL;
                    objFacturaBTS.IV_USUARIO_CREA = objCabeceraBTS.IV_USUARIO_CREA;
                    objFacturaBTS.IV_FECHA_CREA = DateTime.Now;
                    objFacturaBTS.DIR_FACTURADO = objCabeceraBTS.DIR_FACTURADO;
                    objFacturaBTS.EMAIL_FACTURADO = objCabeceraBTS.EMAIL_FACTURADO;
                    objFacturaBTS.CIUDAD_FACTURADO = objCabeceraBTS.CIUDAD_FACTURADO;


                    objFacturaBTS.DIAS_CREDITO = objCabeceraBTS.DIAS_CREDITO;
                    objFacturaBTS.TOTAL_CAJAS_BODEGA = objCabeceraBTS.TOTAL_CAJAS_BODEGA;
                    objFacturaBTS.TOTAL_CAJAS_MUELLE = objCabeceraBTS.TOTAL_CAJAS_MUELLE;



                    /*agrego detalle de BODEGA*/
                    var LinqDetalle = (from p in objCabeceraBTS.Detalle_bodega.Where(x => x.QTY_out != 0)
                                       select p).ToList();

                   

                    /*detalle de factura BODEGA*/
                    foreach (var Det in LinqDetalle)
                    {
                        objDetalleFacturaBodega = new Cls_Prev_Detalle_Bodega();
                        objDetalleFacturaBodega.ID = Det.ID;
                        objDetalleFacturaBodega.Fila = Det.Fila;
                        objDetalleFacturaBodega.idNave = Det.idNave;
                        objDetalleFacturaBodega.codLine = Det.codLine;
                        objDetalleFacturaBodega.nave = Det.nave;
                        objDetalleFacturaBodega.ruc = Det.ruc;
                        objDetalleFacturaBodega.Exportador = Det.Exportador;
                        objDetalleFacturaBodega.booking = Det.booking;
                        objDetalleFacturaBodega.idModalidad = Det.idModalidad;
                        objDetalleFacturaBodega.desc_modalidad = Det.desc_modalidad;
                        objDetalleFacturaBodega.id_bodega = Det.id_bodega;
                        objDetalleFacturaBodega.desc_bodega = Det.desc_bodega;
                        objDetalleFacturaBodega.tipo_bodega = Det.tipo_bodega;
                        objDetalleFacturaBodega.id_tipo_bodega = Det.id_tipo_bodega;
                        objDetalleFacturaBodega.QTY_out = Det.QTY_out;

                        objFacturaBTS.Detalle_bodega.Add(objDetalleFacturaBodega);

                    }

                    /*agrego detalle de MUELLE*/
                    var LinqDetalleMuelle = (from p in objCabeceraBTS.Detalle_muelle.Where(x => x.cajas != 0)
                                       select p).ToList();



                    /*detalle de factura BODEGA*/
                    foreach (var Det in LinqDetalleMuelle)
                    {
                        objDetalleFacturaMuelle = new Cls_Prev_Detalle_Muelle();
                        objDetalleFacturaMuelle.ID = Det.ID;
                        objDetalleFacturaMuelle.Fila = Det.Fila;
                        objDetalleFacturaMuelle.idNave = Det.idNave;
                        objDetalleFacturaMuelle.codLine = Det.codLine;
                        objDetalleFacturaMuelle.nave = Det.nave;
                        objDetalleFacturaMuelle.aisv_codig_clte = Det.aisv_codig_clte;
                        objDetalleFacturaMuelle.aisv_nom_expor = Det.aisv_nom_expor;
                        objDetalleFacturaMuelle.aisv_estado = Det.aisv_estado;
                        objDetalleFacturaMuelle.cajas = Det.cajas;
                        

                        objFacturaBTS.Detalle_muelle.Add(objDetalleFacturaMuelle);

                    }


                    /***********************************************************************************************************************************************
                    *4) Consulta de Servicios a facturar N4 
                    **********************************************************************************************************************************************/
                    string request = string.Empty;
                    string response = string.Empty;
                    Respuesta.ResultadoOperacion<bool> resp;
                    resp = ServicioSCA.CargarServicioBTS(INVOICETYPE, objFacturaBTS.ID_FACTURADO, objFacturaBTS.REFERENCIA, list,  Page.User.Identity.Name.ToUpper(), out request, out response);
                    if (resp.Exitoso)
                    {
                        var v_result = resp.MensajeInformacion.Split(',');
                        decimal v_monto = 0;
                        v_monto = decimal.Parse(v_result[1].ToString());
                        string Draf = v_result[0];

                        objFacturaBTS.DRAF = Draf;

                        /*detalle de muelles*/
                        List<Cls_Prev_Detalle_Servicios> ListServicios = Cls_Prev_Detalle_Servicios.Carga_Servicios_Draf(Draf, out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener servicios de facturación del draf....{0}/ Error:{1}</b>", Draf, cMensajes));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                        if (ListServicios != null)
                        {
                            foreach (var Det in ListServicios)
                            {

                                objDetalleFacturaServicios = new Cls_Prev_Detalle_Servicios();
                                objDetalleFacturaServicios.Fila = Fila;
                                objDetalleFacturaServicios.final_nbr = Det.final_nbr;
                                objDetalleFacturaServicios.draft_nbr = Det.draft_nbr;
                                objDetalleFacturaServicios.payee_customer_name = Det.payee_customer_name;
                                objDetalleFacturaServicios.currency_id = Det.currency_id;
                                objDetalleFacturaServicios.finalized_date = Det.finalized_date;
                                objDetalleFacturaServicios.gkey = Det.gkey;
                                objDetalleFacturaServicios.event_type_id = Det.event_type_id;
                                objDetalleFacturaServicios.event_id = Det.event_id;
                                objDetalleFacturaServicios.quantity_billed = Det.quantity_billed;
                                objDetalleFacturaServicios.quantity = Det.quantity;
                                objDetalleFacturaServicios.rate_billed = Det.rate_billed;
                                objDetalleFacturaServicios.amount = Det.amount;
                                objDetalleFacturaServicios.description = Det.description;
                                objDetalleFacturaServicios.created = Det.created;
                                objDetalleFacturaServicios.tax = Det.tax;
                                objDetalleFacturaServicios.amount_taxt = Det.amount_taxt;
                                objDetalleFacturaServicios.notes = Det.notes;

                                objDetalleFacturaServicios.IV_USUARIO_CREA = LoginName;
                                objDetalleFacturaServicios.IV_FECHA_CREA = DateTime.Now;


                                Fila++;
                                objFacturaBTS.Detalle_Servicios.Add(objDetalleFacturaServicios);

                            }


                            Subtotal = Decimal.Round(ListServicios.Sum(p => p.amount.Value), 2);
                            Iva = Decimal.Round(ListServicios.Sum(p => p.amount_taxt.Value), 2);
                            Total = Subtotal + Iva;

                          
                        }


                    }
                    else
                    {

                      
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se logró realizar la facturación en N4...{0} </b>", resp.MensajeProblema));
                        return;
                    }

                    //Subtotal = LinqSubtotal;
                    objFacturaBTS.SUBTOTAL = Subtotal;
                    objFacturaBTS.IVA = Iva;
                    objFacturaBTS.TOTAL = Total;

                    //actualiza sesion
                    objCabecera.SUBTOTAL = Subtotal;
                    objCabecera.IVA = Iva;
                    objCabecera.TOTAL = Total;

                    Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;
                    Session["InvoiceBTS" + this.hf_BrowserWindowName.Value] = objFacturaBTS;

                    /***********************************************************************************************************************************************
                    *pasar a la siguiente ventana
                    **********************************************************************************************************************************************/
                    if (objFacturaBTS == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No existe objeto para generar la visualización de la factura</b>"));
                        return;
                    }
                    else
                    {
                        
                        

                        this.Ocultar_Mensaje();
                        string cId = securetext(this.hf_BrowserWindowName.Value);
                        Response.Redirect("~/btsagencia/facturacionagencia_preview.aspx?id=" + cId.Trim() + "", false);

                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "Factura BTS", false, null, null, ex.StackTrace, ex);
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



        public static byte[] CreateExcelBytesFromStoredProcedure(string pReferencia, string pLinea)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BILLION"].ConnectionString;

            DataTable DtBodega = new DataTable();
            DataTable DtMuelle = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("BTS_DETALLE_BODEGAS", connection))
                {
                  
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@REFERENCIA", pReferencia);
                    command.Parameters.AddWithValue("@LINEA", pLinea);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DtBodega.Load(reader);
                    }


                }

            }

            Color colorPlaneado = ColorTranslator.FromHtml("#59B653");
            Color colorReservado = ColorTranslator.FromHtml("#5AABD9");
            Color colorDisponible = ColorTranslator.FromHtml("#D9DB63");

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");


                worksheet.Cells["A5:A5"].Value = "BODEGA:";
                worksheet.Cells["A5:A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:A5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A5:A5"].Style.Fill.BackgroundColor.SetColor(colorPlaneado);
                worksheet.Cells["A5:A5"].Style.Font.Color.SetColor(System.Drawing.Color.White);


                int r = 6;
                int c = 1;
                foreach (DataColumn column in DtBodega.Columns)  //printing column headings
                {


                    worksheet.Cells[r, c].Value = column.ColumnName;
                    worksheet.Cells[r, c].Style.Font.Bold = true;
                    worksheet.Cells[r, c].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells[r, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[r, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[r, c].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                    c = c + 1;
                }

                r = 7;
                c = 1;
                int fila = 0;
                int eachRow = 0;
                int col = 0;
                int new_col = c;
               

                for (eachRow = 0; eachRow < DtBodega.Rows.Count;)
                {
                    new_col = 1;
                    col = 0;
                    foreach (DataColumn column in DtBodega.Columns)
                    {

                        worksheet.Cells[r, new_col].Value = DtBodega.Rows[fila][col];
                        col++;
                        new_col++;
                    }

                    eachRow++;
                    r++;
                    fila++;

                }

                r = r + 2;

                worksheet.Cells["A"+ r.ToString() + ":A" + r.ToString()].Value = "MUELLE:";
                worksheet.Cells["A" + r.ToString() + ":A" + r.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + r.ToString() + ":A" + r.ToString()].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A" + r.ToString() + ":A" + r.ToString()].Style.Fill.BackgroundColor.SetColor(colorReservado);
                worksheet.Cells["A" + r.ToString() + ":A" + r.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.White);

              

                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["catalogo"].ConnectionString;

               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("BTS_DETALLE_MUELLE", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@REFERENCIA", pReferencia);
                        command.Parameters.AddWithValue("@LINEA", pLinea);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DtMuelle.Load(reader);
                        }


                    }

                }


                c = 1;
                 r = r + 1;
                foreach (DataColumn column in DtMuelle.Columns)  //printing column headings
                {


                    worksheet.Cells[r, c].Value = column.ColumnName;
                    worksheet.Cells[r, c].Style.Font.Bold = true;
                    worksheet.Cells[r, c].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells[r, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[r, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[r, c].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                    c = c + 1;
                }

                fila = 0;
                r = r + 1;

                for (eachRow = 0; eachRow < DtMuelle.Rows.Count;)
                {
                    new_col = 1;
                    col = 0;
                    foreach (DataColumn column in DtMuelle.Columns)
                    {

                        worksheet.Cells[r, new_col].Value = DtMuelle.Rows[fila][col];
                        col++;
                        new_col++;
                    }

                    eachRow++;
                    r++;
                    fila++;

                }

                worksheet.Cells.AutoFitColumns();
                return excelPackage.GetAsByteArray();
            }

        }

        [WebMethod]
        public static void ExportarExcel(string pReferencia, string pLinea)
        {
            try
            {
                byte[] excelBytes = CreateExcelBytesFromStoredProcedure(pReferencia, pLinea);

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

    }
}