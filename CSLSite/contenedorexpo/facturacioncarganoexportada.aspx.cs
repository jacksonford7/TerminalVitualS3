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
using System.Web.Script.Services;

using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using CSLSite;

namespace CSLSite.contenedorexpo
{


    public partial class facturacioncarganoexportada : System.Web.UI.Page
    {


        #region "Clases"
        private static Int64? lm = -3;
        private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
       
        private Cls_Bil_Invoice_Cabecera objFactura = new Cls_Bil_Invoice_Cabecera();
        private Cls_Bil_Invoice_Detalle objDetalleFactura = new Cls_Bil_Invoice_Detalle();
        private Cls_Bil_Invoice_Servicios objServiciosFactura = new Cls_Bil_Invoice_Servicios();
       
        private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        private Cls_Bil_Detalle objDetalle = new Cls_Bil_Detalle();
      
        private List<Cls_Bil_AsumeFactura> List_Asume { set; get; }
        private List<Cls_Bil_Contenedor_DiasLibres> List_Contenedor { set; get; }
      

        private List<Cls_Bil_Invoice_Type> List_Invoice_Type { set; get; }
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
        private string EmpresaSelect = string.Empty;

        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
       
        private Int64 DiasCredito = 0;
       
        private string gkeyBuscado = string.Empty;

        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
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
           
            UPFECHA.Update();
            UPBOTONES.Update();
           // UPINVOICETYPE.Update();
           // UPEXPORTADOR.Update();

        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
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
            Session["Booking" + this.hf_BrowserWindowName.Value] = objCabecera;
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
                tk.Categoria = "EXPO"; //solo puede ser: Impo,Expo,Otros
                tk.Usuario = pUsuario.Trim(); //login
                tk.Ruc = pruc.Trim(); //login ruc
                tk.PalabraClave = "CasoEXPO"; //Opcional es una palabra clave para agrupar
                tk.Copias = "desarrollo@cgsa.com.ec";//opcional es para enviar copia de respaldo
                tk.Aplicacion = "Billion"; //obligatorio
                tk.Modulo = pModulo;//opcional

                var detalle_carga = new SaleforcesContenido();
                detalle_carga.Categoria = TipoCategoria.Importacion; //opcional
                detalle_carga.Tipo = TipoCarga.Contenedores; //opcional
                detalle_carga.Titulo = "Modulo de Facturación Exportaciones"; //opcional
                detalle_carga.Novedad = pNovedad; //mensaje del modulo o error

                detalle_carga.Detalles.Add(new DetalleCarga("Errores:", MensajesErrores));

                if (!string.IsNullOrEmpty(pValor1)) { detalle_carga.Detalles.Add(new DetalleCarga("BOOKING", pValor1));  }
                if (!string.IsNullOrEmpty(pValor2)) { detalle_carga.Detalles.Add(new DetalleCarga("OTROS", pValor2)); }
                if (!string.IsNullOrEmpty(pValor3)) { detalle_carga.Detalles.Add(new DetalleCarga("OTROS", pValor3)); }

                //asi puedes poner los campos que desees o se necesiten sobre la carga

                tk.Contenido = detalle_carga.ToString();

                var rt = tk.NuevoCaso();
                if (rt.Exitoso)
                {
                    Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del caso generado #  {0} ", rt.Resultado);
                }
                else
                {
                    Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del problema {0} ", rt.MensajeProblema);
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
                Server.HtmlEncode(this.Txtempresa.Text.Trim());

                if (!Page.IsPostBack)
                {

                    /*secuencial de sesion*/
                    //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    this.Crear_Sesion();
                   
                    objFactura = new Cls_Bil_Invoice_Cabecera();
                    Session["InvoiceBooking" + this.hf_BrowserWindowName.Value] = objFactura;

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", ex.Message));
            }
        }


        #endregion
      
       #region "Eventos de la grilla"

        protected void CHKFA_CheckedChanged(object sender, EventArgs e)
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

                    CultureInfo enUS = new CultureInfo("en-US");

                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    CheckBox CHKFA = (CheckBox)row.FindControl("CHKFA");

                    String CONTENEDOR = tablePagination.DataKeys[row.RowIndex].Value.ToString();

                    objCabecera = Session["Booking" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;

                    var Detalle = objCabecera.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(CONTENEDOR.Trim()));
                    if (Detalle != null)
                    {
                        if (CHKFA.Checked)
                        {
                            Detalle.VISTO = true;

                            Fecha = string.Format("{0}", this.TxtFechaHasta.Text.Trim());
                            if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                            {
                                Detalle.VISTO = false;
                                Detalle.FECHA_HASTA = null;
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la factura</b>"));
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

                    Session["Booking" + this.hf_BrowserWindowName.Value] = objCabecera;


                    this.Actualiza_Panele_Detalle();

                }
                   
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }
        }

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

                   
                    objCabecera = Session["Booking" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;

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

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        string row_estado = DataBinder.Eval(e.Row.DataItem, "REEFER").ToString();
                        string row_conectado = DataBinder.Eval(e.Row.DataItem, "CONECTADO").ToString();
                       
                        Int64 row_gkey = Int64.Parse(DataBinder.Eval(e.Row.DataItem, "GKEY").ToString());
                     
                        string row_contenedor = DataBinder.Eval(e.Row.DataItem, "CONTENEDOR").ToString();
                      
                        CheckBox Chk = (CheckBox)e.Row.FindControl("CHKFA");
                       
                        string row_inout = DataBinder.Eval(e.Row.DataItem, "IN_OUT").ToString();
                        string row_existe = DataBinder.Eval(e.Row.DataItem, "ESTADO_RDIT").ToString();
                        string row_pase = DataBinder.Eval(e.Row.DataItem, "NUMERO_PASE_N4").ToString();

                        if (row_estado.Equals("RF") && row_conectado.Equals("NO CONECTADO"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.PaleVioletRed;
                            Chk.Enabled = false;

                        }
                      //BLOQUEO SI EXISTE
                        if (row_existe.Equals("SI"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Peru;
                            //Chk.Enabled = false;
                        }

                        if (row_inout.Equals("OUT"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Peru;
                            Chk.Enabled = false;

                        }

                        if (!string.IsNullOrEmpty(row_pase))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Green;
                            Chk.Enabled = false;
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

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    OcultarLoading("2");
                    bool Ocultar_Mensaje = true;
                    //UnidadDesconectada = false;

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
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número del Booking</b>"));
                        this.TXTMRN.Focus();
                        return;
                    }

                    //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //carga exportador

                    //  this.Cargar_Exportador(this.TXTMRN.Text.ToString().Trim(), ClsUsuario.Usuario);

                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                   
                    var Contenedor = new N4.Exportacion.container();
                    var ListaContenedores = Contenedor.CargaporBooking_NoExportada(ClsUsuario.loginname, this.TXTMRN.Text.ToString().Trim());
                    if (ListaContenedores.Exitoso)
                    {


                        /*ultima factura*/
                        List<Cls_Bil_Invoice_Ultima_Factura> ListUltimaFactura = Cls_Bil_Invoice_Ultima_Factura.List_Ultima_Factura_Expo_NoExportada(this.TXTMRN.Text.Trim(), out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener última factura....{0}</b>", cMensajes));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                        /*ultima factura en caso de tener*/
                        /*var LinqUltimaFactura = (from TblFact in ListUltimaFactura.Where(TblFact => TblFact.IV_GKEY != 0)
                                                 select new
                                                 {
                                                     FT_BOOKING = TblFact.IV_NUMERO_CARGA,
                                                     FT_GKEY = TblFact.IV_GKEY,
                                                 }).Distinct();*/

                        var LinqUltimaFactura = (from TblFact in ListUltimaFactura.Where(TblFact => TblFact.IV_GKEY != 0)
                                                 select new
                                                 {
                                                     FT_NUMERO_CARGA = TblFact.IV_NUMERO_CARGA,
                                                     FT_FECHA = (TblFact.IV_FECHA == null ? null : TblFact.IV_FECHA),
                                                     FT_FACTURA = TblFact.IV_FACTURA,
                                                     FT_GKEY = TblFact.IV_GKEY,
                                                     FT_FECHA_HASTA = (TblFact.IV_FECHA_HASTA == null ? null : TblFact.IV_FECHA_HASTA),
                                                     FT_ID = TblFact.IV_ID,
                                                     FT_MODULO = TblFact.IV_MODULO
                                                 }).Distinct();

                        /*pase puerta*/
                        List<Cls_Bil_Invoice_Pase_Puerta> ListPasePuerta = Cls_Bil_Invoice_Pase_Puerta.List_Pase_Puerta_NoExportada(this.TXTMRN.Text.Trim(), out cMensajes);
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
                                         join Factura in LinqUltimaFactura on Tbl.CNTR_CONSECUTIVO equals Factura.FT_GKEY into TmpFactura
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
                                             DOCUMENTO = (Tbl.CNTR_DOCUMENT == null) ?string.Empty :Tbl.CNTR_DOCUMENT,
                                             IMDT =string.Empty,
                                             BL = string.Empty,
                                             FULL_VACIO = (Tbl.CNTR_FULL_EMPTY_CODE == null) ? string.Empty : Tbl.CNTR_FULL_EMPTY_CODE,
                                             GKEY = Tbl.CNTR_CONSECUTIVO,
                                             AISV = (Tbl.CNTR_AISV == null) ? string.Empty : Tbl.CNTR_AISV,
                                             DECLARACION = (Tbl.CNTR_DOCUMENT == null) ? string.Empty : Tbl.CNTR_DOCUMENT,
                                             BLOQUEADO = (Tbl.CNTR_HOLD == 0) ? false : true,
                                             VIAJE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                             NAVE = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                             FECHA_ARRIBO = (Tbl.CNTR_VEPR_ACTUAL_ARRIVAL.HasValue ? Tbl.CNTR_VEPR_ACTUAL_ARRIVAL.Value.ToString("dd/MM/yyyy") : ""),
                                             CNTR_DD = (Tbl.CNTR_DD == 0) ? false : true,                                            
                                             CNTR_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
                                             FECHA_ARRIBO2 = (Tbl.CNTR_VEPR_ACTUAL_ARRIVAL == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_ARRIVAL),
                                             CNTR_TYSZ_ISO = (Tbl.CNTR_TYSZ_ISO == null) ? string.Empty : Tbl.CNTR_TYSZ_ISO,
                                             CNTR_TEMPERATURE = Tbl.CNTR_TEMPERATURE ==null ? 0 : Tbl.CNTR_TEMPERATURE,
                                             CNTR_TYPE_DOCUMENT=(Tbl.CNTR_TYPE_DOCUMENT == null) ? string.Empty : Tbl.CNTR_TYPE_DOCUMENT,
                                             CNTR_LCL_FCL = (Tbl.CNTR_LCL_FCL == null) ? string.Empty : Tbl.CNTR_LCL_FCL,
                                             CNTR_CATY_CARGO_TYPE = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
                                             CNTR_FREIGHT_KIND = (Tbl.CNTR_FREIGHT_KIND == null) ? string.Empty : Tbl.CNTR_FREIGHT_KIND,
                                             CNTR_BKNG_BOOKING = (Tbl.CNTR_BKNG_BOOKING == null) ? string.Empty : Tbl.CNTR_BKNG_BOOKING,
                                             CNTR_VEPR_VOYAGE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                             CNTR_VEPR_VSSL_NAME = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                             EXISTE_CONTENEDOR = (FinalFT == null) ? "NO": "SI",
                                             CNTR_PROFORMA = (Tbl.CNTR_PROFORMA == null) ? string.Empty : Tbl.CNTR_PROFORMA,
                                             FECHA_ULTIMA = (FinalFT == null) ? null : (DateTime?)(FinalFT.FT_FECHA.HasValue ? FinalFT.FT_FECHA : null),
                                             NUMERO_FACTURA = (FinalFT == null) ? string.Empty : FinalFT.FT_FACTURA,
                                             ID_FACTURA = (FinalFT == null) ? 0 : FinalFT.FT_ID,
                                             FECHA_HASTA = (FinalFT == null) ? null : (DateTime?)(FinalFT.FT_FECHA_HASTA.HasValue ? FinalFT.FT_FECHA_HASTA : null),
                                             MODULO = (FinalFT == null) ? string.Empty : FinalFT.FT_MODULO,
                                         }).OrderBy(x => x.IN_OUT).ThenBy(x => x.CONTENEDOR);


                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {

                           

                            //agrego todos los contenedores a la clase cabecera
                            objCabecera = Session["Booking" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                            objCabecera.FECHA = DateTime.Now;
                            objCabecera.TIPO_CARGA = "EXP";
                            objCabecera.NUMERO_CARGA = this.TXTMRN.Text.Trim() ;//booking
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
                                objDetalle.SECUENCIA = Secuencia;
                                objDetalle.GKEY = Det.GKEY;
                                objDetalle.MRN = this.TXTMRN.Text.Trim();
                                objDetalle.MSN = "0000";
                                objDetalle.HSN = "0000";
                                objDetalle.CONTENEDOR = Det.CONTENEDOR;
                                objDetalle.TRAFICO = Det.TRAFICO;
                                objDetalle.DOCUMENTO = Det.DOCUMENTO;
                                objDetalle.DES_BLOQUEO = Det.BLOQUEOS;
                                objDetalle.CONECTADO = Det.CONECTADO;
                                objDetalle.REFERENCIA = Det.REFERENCIA;
                                objDetalle.TAMANO = Det.TAMANO;
                                objDetalle.TIPO = Det.TIPO;
                                objDetalle.CAS = Det.FECHA_CAS;
                                objDetalle.BOOKING = this.TXTMRN.Text.Trim();

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
                                objDetalle.CNTR_DD = true;
                                objDetalle.FECHA_HASTA = Det.FECHA_HASTA;
                                objDetalle.CNTR_DEPARTED = Det.CNTR_DEPARTED;
                                objDetalle.FECHA_ARRIBO = Det.FECHA_ARRIBO2;
                                objDetalle.MODULO = Det.MODULO;

                                objDetalle.LINEA = Det.LINEA;



                                objDetalle.IDPLAN = "0";
                                objDetalle.TURNO = "* Seleccione *";

                                objDetalle.CNTR_TYSZ_ISO = Det.CNTR_TYSZ_ISO;
                                objDetalle.CNTR_TEMPERATURE = Det.CNTR_TEMPERATURE.Value;
                                objDetalle.CNTR_TYPE_DOCUMENT = Det.CNTR_TYPE_DOCUMENT;
                                objDetalle.CNTR_LCL_FCL = Det.CNTR_LCL_FCL;
                                objDetalle.CNTR_CATY_CARGO_TYPE = Det.CNTR_CATY_CARGO_TYPE;
                                objDetalle.CNTR_FREIGHT_KIND = Det.CNTR_FREIGHT_KIND;
                                objDetalle.CNTR_BKNG_BOOKING = Det.CNTR_BKNG_BOOKING;
                                objDetalle.CNTR_VEPR_VOYAGE = Det.CNTR_VEPR_VOYAGE;
                                objDetalle.CNTR_VEPR_VSSL_NAME = Det.CNTR_VEPR_VSSL_NAME;
                                objDetalle.CNTR_PROFORMA = Det.CNTR_PROFORMA;
                                objDetalle.ESTADO_RDIT = Det.EXISTE_CONTENEDOR;

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

                                objDetalle.CERTIFICADO = false;
                                objDetalle.TIENE_CERTIFICADO = string.Empty;


                                objCabecera.Detalle.Add(objDetalle);
                                Secuencia++;
                            }

                            tablePagination.DataSource = objCabecera.Detalle;
                            tablePagination.DataBind();
                            this.LabelTotal.InnerText = string.Format("DETALLE DE CONTENEDORES - Total Contenedores: {0}", objCabecera.Detalle.Count());
                            Session["Booking" + this.hf_BrowserWindowName.Value] = objCabecera;
                            this.Actualiza_Panele_Detalle();

                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información pendiente de facturar con el número de booking ingresado..{0}</b>", ListaContenedores.MensajeProblema));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información pendiente de facturar con el número de booking ingresado..{0}</b>", ListaContenedores.MensajeProblema));
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

                Response.Redirect("~/contenedorexpo/facturacionbooking.aspx", false);



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

        

    

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

                CultureInfo enUS = new CultureInfo("en-US");

                ContenedorReefer = false;

                try
                {

                    //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdEmpresa = string.Empty;
                    string DesEmpresa = string.Empty;

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de Booking</b>"));
                        this.TXTMRN.Focus();
                        return;
                    }


                  
                    if (string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Informativo! Debe ingresar el exportador a emitir la factura </b>"));
                        this.Txtempresa.Focus();
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
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Exportador [{0}-{1}] no se encuentra registrado en nuestra base de datos</b>", IdEmpresa, DesEmpresa));
                                this.Txtempresa.Focus();
                                return;

                            }                         
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar el exportador de la lista. </b>"));
                            this.Txtempresa.Focus();
                            return;
                        }
                    }

                    //fecha hasta para sacar los servicios
                    Fecha = string.Format("{0}", this.TxtFechaHasta.Text.Trim());
                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la factura</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }


                    string pInvoiceType = string.Empty;
                    var InvoiceType = InvoiceTypeConfig.ObtenerInvoicetypes();
                    if (InvoiceType.Exitoso)
                    {
                        var LinqInvoiceType = (from p in InvoiceType.Resultado.Where(X => X.codigo.Equals("EXPOCARGANOEXP"))
                                               select new { valor = p.valor }).FirstOrDefault();

                        pInvoiceType = LinqInvoiceType.valor == null ? "2DA_MAN_EXPO_CNTRS" : LinqInvoiceType.valor;

                    }

                    if (string.IsNullOrEmpty(pInvoiceType))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe Invoice Type para poder facturar la carga no exportada</b>"));
                        return;
                    }

                    //instancia sesion
                    objCabecera = Session["Booking" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
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

                    Valida_Todos = false;
                    //si no tiene fecha de ultima factura, se procede a validar todos los contenedores
                    if (Valida_Todos)
                    {
                        //valida que seleccione todos los contenedores para cotizar 
                        var LinqValidaContenedorExp = (from p in objCabecera.Detalle.Where(x => x.VISTO == false)
                                                    select p.CONTENEDOR).ToList();

                        if (LinqValidaContenedorExp.Count != 0)
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
                        var LinqValidaContenedorExp = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                                    select p.CONTENEDOR).ToList();

                        if (LinqValidaContenedorExp.Count == 0)
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


                    //exportador
                    objCabecera.ID_FACTURADO = IdEmpresa;// this.CboExportador.SelectedValue;
                    objCabecera.INVOICE_TYPE = pInvoiceType;
                    objCabecera.DESC_FACTURADO = DesEmpresa.Trim();

                    //valida que seleccione un contenedor contenedores para cotizar 
                    var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                                select p.CONTENEDOR).ToList();

                    if (LinqValidaContenedor.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar los contenedores a Facturar</b>"));
                        return;
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
                           
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabecera.ID_FACTURADO));

                            return;
                        }
                    }
                    else
                    {

                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.fac@contecon.com.ec'>ec.fac@contecon.com.ec</a> para proceder con el registro.</b>", objCabecera.ID_FACTURADO, objCabecera.DESC_FACTURADO));
                        return;

                    }




                    /***********************************************************************************************************************************************
                    *fin: consulta datos de cliente
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

                    //10-02-2020
                    /**********************************************************************************************************************************
                    * validacion de horas reefer
                    * ********************************************************************************************************************************/
                    //if (ContenedorReefer)
                    //{
                    //    var ValidaContainersReefer = new List<N4.Importacion.container>();

                    //    foreach (var Det in objCabecera.Detalle.Where(x => x.VISTO == true && x.REEFER.Trim().Equals("RF")))
                    //    {
                    //        var ContReefer = new N4.Importacion.container();

                    //        ContReefer.CNTR_CONTAINER = Det.CONTENEDOR;
                    //        ContReefer.CNTR_CLNT_CUSTOMER_LINE = Det.LINEA;
                    //        ContReefer.CNTR_VEPR_REFERENCE = Det.REFERENCIA;
                    //        ValidaContainersReefer.Add(ContReefer);
                    //    }

                    //    var ValidaHorasReefer = N4.Importacion.container.ValidacionReeferImpo(ValidaContainersReefer, ClsUsuario.loginname.Trim());
                    //    if (!ValidaHorasReefer.Exitoso)
                    //    {
                    //        gkeyBuscado = string.Empty;
                    //        gkeyBuscado = ValidaHorasReefer.MensajeProblema;

                    //        string CuerpoMensaje = string.Format("Se presentaron los siguientes problemas: {0}", gkeyBuscado);

                    //        /*************************************************************************************************************************************
                    //        * crear caso salesforce
                    //        ***********************************************************************************************************************************/
                    //        MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", gkeyBuscado);

                    //        this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Exportaciones", "No pudo facturar debido a validación a que no tiene cargadas las horas reefer",
                    //            MensajesErrores.Trim(), string.Format("{0}", this.TXTMRN.Text.Trim()),
                    //            objCabecera.DESC_FACTURADO, "",out MensajeCasos);

                    //        /*************************************************************************************************************************************
                    //        * fin caso salesforce
                    //        * **********************************************************************************************************************************/

                    //        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Estimado cliente {0}, se presento un problema el cual no le permitirá avanzar con la facturación de los contenedores seleccionados, {1} </b>", objCabecera.DESC_AGENTE, MensajeCasos));
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        var no_pasa = ValidaHorasReefer.Resultado.Where(f => !f.Item4).Count() > 0;
                    //        if (no_pasa)
                    //        {
                    //            //aqui puedo recuperar todos aquellos que no pasan validacion
                    //            //si quieres recupera la unidad/es para garbar un log  o enviar un mensaje personalizado
                    //            var novalidos = ValidaHorasReefer.Resultado.Where(g => !g.Item4).ToList();
                    //            StringBuilder tb = new StringBuilder();
                    //            novalidos.ForEach(t =>
                    //            {
                    //                tb.AppendFormat("Unidad:{0}->{1} /", t.Item1, !string.IsNullOrEmpty(t.Item3) ? t.Item3 : t.Item2);
                    //            });

                    //            string CuerpoMensaje = string.Format("Se presentaron los siguientes problemas: {0}", tb.ToString());

                    //            /*************************************************************************************************************************************
                    //            * crear caso salesforce
                    //            ***********************************************************************************************************************************/
                    //            MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", tb.ToString());

                    //            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Exportaciones",
                    //                "No pudo facturar debido a validación a que no tiene cargadas las horas reefer",
                    //                MensajesErrores.Trim(), string.Format("{0}", this.TXTMRN.Text.Trim()),
                    //                objCabecera.DESC_FACTURADO, "", out MensajeCasos);

                    //            /*************************************************************************************************************************************
                    //            * fin caso salesforce
                    //            * **********************************************************************************************************************************/

                    //            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Estimado cliente {0}, se presento un problema el cual no le permitirá avanzar con la facturación de los contenedores seleccionados, {1} </b>", objCabecera.ID_CLIENTE, MensajeCasos));


                    //            return;
                    //        }
                    //    }

                    //}

                    /**********************************************************************************************************************************
                    * fin validacion de horas reefer
                    * ********************************************************************************************************************************/

                    //listado de contenedores
                    var LinqListContenedor2 = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                              select p.CONTENEDOR).ToList();

                    objCabecera.GLOSA = "";
                    Contenedores = string.Join(", ", LinqListContenedor2);
                    //numero de carga
                    Numero_Carga = objCabecera.NUMERO_CARGA;
                    objCabecera.CONTENEDORES = Contenedores;
                    objCabecera.FECHA_HASTA = DateTime.Now;
                    LoginName = objCabecera.IV_USUARIO_CREA.Trim();
                    objCabecera.HORA_HASTA = HoraHasta;
                    //actualizo el objeto
                    Session["Booking" + this.hf_BrowserWindowName.Value] = objCabecera;


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
                    objCabecera = Session["Booking" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    /*agrego datos a la factura*/
                    objFactura = Session["InvoiceBooking" + this.hf_BrowserWindowName.Value] as Cls_Bil_Invoice_Cabecera;
                    objFactura = new Cls_Bil_Invoice_Cabecera();
                    objFactura.Detalle.Clear();
                    objFactura.DetalleServicios.Clear();
                    /*cabecera de factura*/
                    objFactura.IV_ID = objCabecera.ID;
                    objFactura.IV_GLOSA = objCabecera.GLOSA;
                    objFactura.IV_FECHA = objCabecera.FECHA;
                    objFactura.IV_TIPO_CARGA = objCabecera.TIPO_CARGA;
                    objFactura.IV_ID_AGENTE = objCabecera.ID_FACTURADO;
                    objFactura.IV_DESC_AGENTE = objCabecera.DESC_FACTURADO;
                    objFactura.IV_ID_CLIENTE = objCabecera.ID_FACTURADO;
                    objFactura.IV_DESC_CLIENTE = objCabecera.DESC_FACTURADO;
                    objFactura.IV_ID_FACTURADO = objCabecera.ID_FACTURADO;
                    objFactura.IV_DESC_FACTURADO = objCabecera.DESC_FACTURADO;
                    objFactura.IV_SUBTOTAL = objCabecera.SUBTOTAL;
                    objFactura.IV_IVA = objCabecera.IVA;
                    objFactura.IV_TOTAL = objCabecera.TOTAL;
                    objFactura.IV_USUARIO_CREA = objCabecera.IV_USUARIO_CREA;
                    objFactura.IV_FECHA_CREA = DateTime.Now;
                    //objFactura.IV_NUMERO_CARGA = string.Format("{0}-{1}-{2}",objCabecera.NUMERO_CARGA,"0000","0000");
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
                    objFactura.INVOICE_TYPE = objCabecera.INVOICE_TYPE;

                    string cip = Request.UserHostAddress;
                    objFactura.IV_IP = cip;
                    /*agrego detalle de contenedores a proforma*/
                    var LinqDetalle = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                       select p).ToList();

                    //campo nuevo
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
                        objDetalleFactura.IV_CAS = Det.FECHA_HASTA;//Det.CAS;
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
                        objDetalleFactura.IV_LINEA = Det.LINEA;

                        //30-01-2020
                        objDetalleFactura.IV_FECHA_TOPE_DLIBRE = Det.FECHA_TOPE_DLIBRE;
                        objDetalleFactura.IV_CNTR_DESCARGA = Det.FECHA_ARRIBO;
                        objDetalleFactura.IV_MODULO = Det.MODULO;
                        objDetalleFactura.IV_CNTR_DEPARTED = Det.CNTR_DEPARTED;//FECHA ZARPE
                        objDetalleFactura.IV_TIENE_SERVICIOS = false;

                        objDetalleFactura.CNTR_TYSZ_ISO = Det.CNTR_TYSZ_ISO;
                        objDetalleFactura.CNTR_TEMPERATURE = Det.CNTR_TEMPERATURE;
                        objDetalleFactura.CNTR_TYPE_DOCUMENT = Det.CNTR_TYPE_DOCUMENT;
                        objDetalleFactura.CNTR_LCL_FCL = Det.CNTR_LCL_FCL;
                        objDetalleFactura.CNTR_CATY_CARGO_TYPE = Det.CNTR_CATY_CARGO_TYPE;
                        objDetalleFactura.CNTR_FREIGHT_KIND = Det.CNTR_FREIGHT_KIND;
                        objDetalleFactura.CNTR_BKNG_BOOKING = Det.CNTR_BKNG_BOOKING;
                        objDetalleFactura.CNTR_VEPR_VOYAGE = Det.CNTR_VEPR_VOYAGE;
                        objDetalleFactura.CNTR_VEPR_VSSL_NAME = Det.CNTR_VEPR_VSSL_NAME;
                        objDetalleFactura.CNTR_PROFORMA = Det.CNTR_PROFORMA;

                        //CAMPOS NUEVOS
                        objDetalleFactura.IV_CANTIDAD = Det.CANTIDAD;
                        objDetalleFactura.IV_PESO = Det.PESO;
                        objDetalleFactura.IV_OPERACION = Det.OPERACION;
                        objDetalleFactura.IV_DESCRIPCION = Det.DESCRIPCION;
                        objDetalleFactura.IV_EXPORTADOR = Det.EXPORTADOR;
                        objDetalleFactura.IV_AGENCIA = Det.AGENCIA;

                        //CARBONO NEUTRO
                        objDetalleFactura.IV_CERTIFICADO = false;
                        objDetalleFactura.IV_TIENE_CERTIFICADO = "NO";

                        objFactura.Detalle.Add(objDetalleFactura);

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


                            var Billing = new N4Ws.Entidad.billing();
                            var Ws = new N4Ws.Entidad.InvoiceRequest();

                            /*saco el invoice type*/
                            //string pInvoiceType = string.Empty;
                            //pInvoiceType = this.CboInvoiceType.SelectedValue;

                            Ws.action = N4Ws.Entidad.Action.INQUIRE;
                            Ws.requester = N4Ws.Entidad.Requester.QUERY_CHARGES;

                            Ws.InvoiceTypeId = pInvoiceType;
                            Ws.payeeCustomerId = Cliente_Ruc;
                            Ws.payeeCustomerBizRole = Cliente_Rol;

                            var Direccion = new N4Ws.Entidad.address();
                            Direccion.addressLine1 = string.Empty;
                            Direccion.city = "GUAYAQUIL";

                            var Parametro = new N4Ws.Entidad.invoiceParameter();
                            Parametro.EquipmentId = Contenedores;
                            Parametro.PaidThruDay = FechaContainer_Filtro.ToString("yyyy-MM-dd HH:mm");
                            Parametro.bexuBookingNbr = Numero_Carga;

                            Ws.invoiceParameters.Add(Parametro);
                            Ws.billToParty.Add(Direccion);


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

                                    CargabexuBlNbr = (from bexuBlNbr in xBilling.response.billInvoice.invoiceParameters.Where(p => p.Metafield == "bexuBookingNbr")
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
                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen contenedores marcados para Facturar</b>"));
                        return;
                    }//fin existe grupo de fechas

                  
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
                    Session["Booking" + this.hf_BrowserWindowName.Value] = objCabecera;
                    Session["InvoiceBooking" + this.hf_BrowserWindowName.Value] = objFactura;

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

                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen servicios pendientes para facturar.</br> {0} ", ""));
                            return;

                        }

                        this.Ocultar_Mensaje();
                        string cId = securetext(this.hf_BrowserWindowName.Value);
                        Response.Redirect("~/contenedorexpo/facturacioncarganoexportadavisualiza.aspx?id=" + cId.Trim() + "", false);

                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "facturacioncarganoexportada", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
         
                }

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

                objCabecera = Session["Booking" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
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
                            if (Detalle.ESTADO_RDIT.Equals("SI"))
                            {

                            }
                            else
                            {
                                if (ChkEstado)
                                {
                                    Fecha = string.Format("{0}", this.TxtFechaHasta.Text.Trim());
                                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                                    {
                                        Detalle.VISTO = false;
                                        Detalle.FECHA_HASTA = null;
                                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la factura</b>"));
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

                Session["Booking" + this.hf_BrowserWindowName.Value] = objCabecera;

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