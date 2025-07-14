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


    public partial class p2d_facturafreightforwarder : System.Web.UI.Page
    {


        #region "Clases"
        private static Int64? lm = -3;
        private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private P2D_MULTI_Facturacion objCargasPendientes = new P2D_MULTI_Facturacion();
        private cfs_cargas_pendientes_detalle objDet = new cfs_cargas_pendientes_detalle();

        usuario ClsUsuario;
      

        private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        private Cls_Bil_Detalle objDetalle = new Cls_Bil_Detalle();
      
        private List<Cls_Bil_AsumeFactura> List_Asume { set; get; }
        private List<Cls_Bil_Contenedor_DiasLibres> List_Contenedor { set; get; }
       
        private List<Cls_Bil_Cas_Manual> List_Autorizacion { set; get; }
        private Cls_Bil_Log_Appcgsa objLogAppCgsa = new Cls_Bil_Log_Appcgsa();
       
        private MantenimientoPaqueteCliente obj = new MantenimientoPaqueteCliente();
        private Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();

        private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();

      
        private P2D_Tarifario objTarifa = new P2D_Tarifario();
        private P2D_Tarja_Cfs objTarja = new P2D_Tarja_Cfs();
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
        //private int Fila = 1;
        private string TipoServicio = string.Empty;
       
        private string HoraHasta = "00:00";
        private string LoginName = string.Empty;
        private int NDiasLibreas = 0;
     
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
       
        private string gkeyBuscado = string.Empty;

    

        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
      

        private static string TextoLeyenda = string.Empty;

        private static string TextoProforma = string.Empty;
        private static string TextoServicio = string.Empty;

        private string c_Subtotal = string.Empty;
        private string c_Iva = string.Empty;
        private string c_Total = string.Empty;

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

        private String Id_Factura_Generada
        {
            get
            {
                return (String)Session["Id_Factura_Generada"];
            }
            set
            {
                Session["Id_Factura_Generada"] = value;
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
            this.UPBOTONESFACTURA.Update();
            this.UPCARGA2.Update();
            UPBOTONES.Update();

        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
           // UPCARGAS.Update();
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

            this.BtnFacturar.Attributes.Add("disabled", "disabled");
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



        private void Carga_CboCiudad()
        {
            try
            {
                List<P2D_Ciudad> Listado = P2D_Ciudad.CboCiudad(out cMensajes);

                this.CboCiudad.DataSource = Listado;
                this.CboCiudad.DataTextField = "DESC_CIUDAD";
                this.CboCiudad.DataValueField = "ID_CIUDAD";
                this.CboCiudad.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboCiudad", "Hubo un error al cargar ciudades", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }

        private void Carga_CboZonas()
        {
            try
            {
                List<P2D_Zona> Listado = P2D_Zona.CboZona(out cMensajes);

                this.CboZonas.DataSource = Listado;
                this.CboZonas.DataTextField = "ZONA";
                this.CboZonas.DataValueField = "ID_ZONA";
                this.CboZonas.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboZonas", "Hubo un error al cargar zonas", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }


        private void OcultarLoadingFactura()
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
            UPCARGA2.Update();
        }

        private void Mostrar_Mensaje_Factura(string Mensaje)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "closeModal();", true);

            this.banmsg_det2.Visible = true;
            this.banmsg_det2.InnerHtml = Mensaje;

            this.banmsg_cab.Visible = true;
            this.banmsg_cab.InnerHtml = Mensaje;

            OcultarLoading();

            this.Actualiza_Paneles();
        }

        private void Mostrar_Mensaje_Proceso(string Mensaje)
        {

            this.banmsg_det2.Visible = true;
            this.banmsg_cab.Visible = true;
            this.banmsg_det.InnerHtml = Mensaje;
            this.banmsg_cab.InnerHtml = Mensaje;
            OcultarLoading();
            this.Actualiza_Paneles();
        }

        private void OcultarLoading()
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader_factura();", true);
            UPCARGA2.Update();
        }

        private void MostrarLoader()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mostrarloader_factura();", true);
            UPCARGA2.Update();
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

                this.banmsg_cab.Visible = IsPostBack;
                this.banmsg_det2.Visible = IsPostBack;

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;
                    this.banmsg_Pase.InnerText = string.Empty;

                    this.banmsg_cab.InnerText = string.Empty;
                    this.banmsg_det2.InnerText = string.Empty;
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

                       

                        this.CboAsumeFactura.DataSource = List_Asume;
                        this.CboAsumeFactura.DataTextField = "mostrar";
                        this.CboAsumeFactura.DataValueField = "ruc";
                        this.CboAsumeFactura.DataBind();
                        this.CboAsumeFactura.SelectedValue = this.hf_idasume.Value;
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
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
               

                if (!Page.IsPostBack)
                {

                    this.Carga_CboCiudad();
                    this.Carga_CboZonas();

                    this.BtnConfirmar.Attributes["disabled"] = "disabled";
                    this.BtnVisualizar.Attributes["disabled"] = "disabled";

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

                    //objFactura = new Cls_Bil_Invoice_Cabecera();
                    //Session["InvoiceCFS" + this.hf_BrowserWindowName.Value] = objFactura;

                  
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
                        
                      
                        string row_pase = DataBinder.Eval(e.Row.DataItem, "NUMERO_PASE_N4").ToString();
                        string row_inout = DataBinder.Eval(e.Row.DataItem, "IN_OUT").ToString();

                        //certificado
                    

                        if (row_inout.Equals("OUT"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Peru;
                            //this.BtnFacturar.Attributes.Add("disabled", "disabled");
                        }

                        if (!string.IsNullOrEmpty(row_pase))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Green;

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

        #region "Evento Marcar Cargar para facturar"

        protected void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
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


                CheckBox chkPaseTarja = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPaseTarja.NamingContainer;
                Label Lblnumero_carga = (Label)item.FindControl("numero_carga");

                objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;

                var Detalle = objCabecera.Detalle_CargasPendientes.FirstOrDefault(f => f.numero_carga.Equals(Lblnumero_carga.Text));
                if (Detalle != null)
                {
                    OcultarLoading("2");

                    Detalle.visto = chkPaseTarja.Checked;

                    //valida sila carga ya esta facturada
                    List<P2D_MULTI_Pendiente> ListExiste = P2D_MULTI_Pendiente.Valida_TieneFactura(Detalle.mrn, Detalle.msn, Detalle.hsn, out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error al validar tiene factura....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }
                    if (ListExiste != null)
                    {
                        if (ListExiste.Count != 0)
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! El número de carga {0} ingresado ya se encuetra facturado, no podrá agregar el mismo...</b>",
                                 string.Format("{0}-{1}-{2}", Detalle.mrn, Detalle.msn, Detalle.hsn)));
                            Detalle.visto = false;

                            this.grillacargas.DataSource = objCabecera.Detalle_CargasPendientes;
                            grillacargas.DataBind();

                            Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;

                            return;
                        }
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //INFORMACION DEL CONTENEDOR
                    var Contenedor = new N4.Importacion.container_cfs();
                    var ListaContenedores = Contenedor.CargaPorBL(ClsUsuario.loginname, Detalle.mrn, Detalle.msn, Detalle.hsn);//resultado de entidad contenedor y cfs
                    if (ListaContenedores.Exitoso)
                    {
                        this.BtnFacturar.Attributes.Remove("disabled");

                        //contenedores con carga cfs
                        var LinqPartidadN4 = (from Tbl in ListaContenedores.Resultado.Where(Tbl => Tbl.CNTR_CANTIDAD != 0)
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
                                                  CNTR_PESO = (Tbl.CNTR_PESO == null ? 0 : Tbl.CNTR_PESO),
                                                  CNTR_OPERACION = (Tbl.CNTR_OPERACION == null ? string.Empty : Tbl.CNTR_OPERACION),
                                                  CNTR_DESCRIPCION = (Tbl.CNTR_DESCRIPCION == null ? string.Empty : Tbl.CNTR_DESCRIPCION),
                                                  CNTR_EXPORTADOR = (Tbl.CNTR_EXPORTADOR == null ? string.Empty : Tbl.CNTR_EXPORTADOR),
                                                  CNTR_AGENCIA = (Tbl.CNTR_AGENCIA == null ? string.Empty : Tbl.CNTR_AGENCIA),
                                                  CARGA = string.Format("{0}-{1}-{2}", Tbl.CNTR_MRN, Tbl.CNTR_MSN, Tbl.CNTR_HSN),
                                                  CNTR_REEFER_CONT = (Tbl.CNTR_REEFER_CONT == null ? string.Empty : Tbl.CNTR_REEFER_CONT),
                                                  ID_UNIDAD = (Tbl.ID_UNIDAD == null ? 0 : Tbl.ID_UNIDAD),
                                              }).OrderBy(x => x.CNTR_CANTIDAD).ThenBy(x => x.CNTR_CONTAINER);


                        var LinqQuery = (from Tbl in LinqPartidadN4
                                         select new
                                         {
                                             CONTENEDOR = Tbl.CNTR_CONTAINER,
                                             REFERENCIA = (Tbl.CNTR_VEPR_REFERENCE == null) ? string.Empty : Tbl.CNTR_VEPR_REFERENCE,
                                             TRAFICO = (Tbl.CNTR_TYPE == null) ? string.Empty : Tbl.CNTR_TYPE,
                                             TAMANO = (Tbl.CNTR_TYSZ_SIZE == null) ? string.Empty : Tbl.CNTR_TYSZ_SIZE,
                                             TIPO = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
                                             FECHA_CAS = System.DateTime.Now,
                                             AUTORIZADO = true,
                                             BLOQUEOS = (Tbl.CNTR_HOLD == false) ? string.Empty : "SI",
                                             IN_OUT = (Tbl.CNTR_YARD_STATUS == null) ? string.Empty : Tbl.CNTR_YARD_STATUS,
                                             TIPO_CONTENEDOR = (Tbl.CNTR_TYSZ_TYPE == null) ? string.Empty : Tbl.CNTR_TYSZ_TYPE,//REEFER
                                             CONECTADO = ((Tbl.CNTR_TYSZ_TYPE == "RF") ? ((Tbl.CNTR_REEFER_CONT == "N") ? "NO CONECTADO" : "CONECTADO") : string.Empty),
                                             LINEA = (Tbl.CNTR_CLNT_CUSTOMER_LINE == null) ? string.Empty : Tbl.CNTR_CLNT_CUSTOMER_LINE,
                                             DOCUMENTO = (string.IsNullOrEmpty(Tbl.CNTR_DOCUMENT) ? ((Detalle.declaracion == null) ? string.Empty : Detalle.declaracion) : Tbl.CNTR_DOCUMENT),
                                             IMDT = (Detalle.imdt == null) ? string.Empty : Detalle.imdt,
                                             BL = (Detalle.numero_carga == null) ? string.Empty : Detalle.numero_carga,
                                             FULL_VACIO = (Tbl.CNTR_FULL_EMPTY_CODE == null) ? string.Empty : Tbl.CNTR_FULL_EMPTY_CODE,
                                             GKEY = Tbl.CNTR_CONSECUTIVO,
                                             AISV = (Tbl.CNTR_AISV == null) ? string.Empty : Tbl.CNTR_AISV,
                                             DECLARACION = (Detalle.declaracion == null) ? string.Empty : Detalle.declaracion,
                                             BLOQUEADO = Tbl.CNTR_HOLD,
                                             FECHA_ULTIMA = (DateTime?)null,
                                             NUMERO_FACTURA = string.Empty,
                                             ID_FACTURA = 0,
                                             VIAJE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                             NAVE = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                             FECHA_ARRIBO = Tbl.CNTR_VEPR_ACTUAL_ARRIVAL,
                                             CNTR_DD = Tbl.CNTR_DD,
                                             FECHA_HASTA = (DateTime?)null,
                                             ESTADO_RIDT = (Detalle.estado_ridt == null) ? string.Empty : Detalle.estado_ridt,
                                             CNTR_DESCARGA = (Tbl.CNTR_DESCARGA == null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                             MODULO = string.Empty,
                                             CNTR_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
                                             CANTIDAD = Tbl.CNTR_CANTIDAD,
                                             PESO = Tbl.CNTR_PESO,
                                             OPERACION = Tbl.CNTR_OPERACION,
                                             DESCRIPCION = Tbl.CNTR_DESCRIPCION,
                                             EXPORTADOR = Tbl.CNTR_EXPORTADOR,
                                             AGENCIA = Tbl.CNTR_AGENCIA,
                                             CARGA = Tbl.CARGA,
                                             ID_UNIDAD = Tbl.ID_UNIDAD,
                                             CERTIFICADO = false,
                                             TIENE_CERTIFICADO = "NO",
                                             TIPO_CLIENTE = (Detalle.tipo_cliente == null) ? string.Empty : Detalle.tipo_cliente,
                                         }).OrderBy(x => x.IN_OUT).ThenBy(x => x.CONTENEDOR);

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
                            objCabecera.NUMERO_CARGA = string.Format("{0}-{1}-{2}", Detalle.mrn, Detalle.msn, Detalle.hsn);
                            objCabecera.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objCabecera.SESION = this.hf_BrowserWindowName.Value;
                            objCabecera.HORA_HASTA = "00:00";
                            objCabecera.FECHA_HASTA = DateTime.Now;

                            string LLAVE = string.Format("{0}-{1}-{2}", Detalle.mrn, Detalle.msn, Detalle.hsn);
                            var Cargas = objCabecera.Detalle.FirstOrDefault(f => f.NUMERO_CARGA.Equals(LLAVE));
                            if (Cargas == null)
                            {
                                Int16 Secuencia = Int16.Parse((objCabecera.Detalle.Count + 1).ToString());

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
                                    objDetalle.P2D_ID_CLIENTE = this.hf_idcliente.Value;
                                    objDetalle.P2D_DESC_CLIENTE = this.hf_desccliente.Value;
                                    objDetalle.P2D_ID_FACTURADO = this.hf_idasume.Value;
                                    objDetalle.P2D_DESC_FACTURADO = this.hf_descasume.Value;
                                    objDetalle.P2D_ID_AGENTE = this.hf_rucagente.Value;
                                    objDetalle.P2D_DESC_AGENTE = this.hf_descagente.Value;
                                    objDetalle.P2D_DIRECCION = this.Txtdireccion.Text.Trim();

                                    objDetalle.VISTO = false;
                                    objDetalle.ID = Det.ID_FACTURA;
                                    objDetalle.SECUENCIA = Secuencia;
                                    objDetalle.GKEY = Det.GKEY;
                                    objDetalle.NUMERO_CARGA = string.Format("{0}-{1}-{2}", Detalle.mrn, Detalle.msn, Detalle.hsn);
                                    objDetalle.MRN = Detalle.mrn;
                                    objDetalle.MSN = Detalle.msn;
                                    objDetalle.HSN = Detalle.hsn;
                                    objDetalle.CONTENEDOR = Det.CONTENEDOR;
                                    objDetalle.TRAFICO = Det.TRAFICO;
                                    objDetalle.DOCUMENTO = Det.DOCUMENTO;
                                    objDetalle.DES_BLOQUEO = Det.BLOQUEOS;
                                    objDetalle.CONECTADO = Det.CONECTADO;
                                    objDetalle.REFERENCIA = Det.REFERENCIA;
                                    objDetalle.TAMANO = Det.TAMANO;
                                    objDetalle.TIPO = "CFS";
                                    objDetalle.CAS = Det.FECHA_CAS;
                                    objDetalle.AUTORIZADO = (Det.AUTORIZADO == true ? "SI" : "NO");
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
                                    objDetalle.CANTIDAD = decimal.Parse(Det.CANTIDAD.Value.ToString());
                                    objDetalle.PESO = decimal.Parse(Det.PESO.ToString());
                                    objDetalle.OPERACION = Det.OPERACION;
                                    objDetalle.DESCRIPCION = Det.DESCRIPCION;
                                    objDetalle.EXPORTADOR = Det.EXPORTADOR;
                                    objDetalle.AGENCIA = Det.AGENCIA;
                                    //nuevo
                                    objDetalle.ID_UNIDAD = Det.ID_UNIDAD;



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

                                    }


                                    objDetalle.IDPLAN = "0";
                                    objDetalle.TURNO = "* Seleccione *";


                                    objDetalle.CERTIFICADO = Det.CERTIFICADO;
                                    objDetalle.TIENE_CERTIFICADO = Det.TIENE_CERTIFICADO;
                                    objDetalle.VISTO = true;

                                    objCabecera.Detalle.Add(objDetalle);
                                    Secuencia++;
                                }

                            }
                            else
                            {
                                //total de bultos
                                var TotalBultos2 = objCabecera.Detalle.Sum(x => x.CANTIDAD);
                                objCabecera.TOTAL_BULTOS = TotalBultos2;

                                //agrega a la grilla
                                tablePagination.DataSource = objCabecera.Detalle;
                                tablePagination.DataBind();

                                this.LabelTotal.InnerText = string.Format("DETALLE DE CARGA SUELTA (CFS) - Total Bultos: {0}", objCabecera.TOTAL_BULTOS);

                                Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;
                                this.Actualiza_Panele_Detalle();

                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! el número de la carga ingresado {0}, ya se encuentra registrado..</b>", LLAVE));
                                //this.TXTHSN.Focus();
                                this.Actualiza_Paneles();
                                return;
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

                            this.Ocultar_Mensaje();

                        }

                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de carga suelta pendiente de facturar con el número de la carga seleccionado..{0}</b>", ListaContenedores.MensajeProblema));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }
                }

               

                this.grillacargas.DataSource = objCabecera.Detalle_CargasPendientes;
                grillacargas.DataBind();

                Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;

                this.UPBOTONES.Update();



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }
        #endregion

        #region "Evento Botones"

        protected void BtnquitarReg_Click(object sender, EventArgs e)
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

                    GridViewRow row = ((GridViewRow)((Button)sender).NamingContainer);
                    String NUMERO_CARGA = tablePagination.DataKeys[row.RowIndex].Value.ToString();

                    objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    var Detalle = objCabecera.Detalle.FirstOrDefault(f => f.NUMERO_CARGA.Equals(NUMERO_CARGA.Trim()));
                    if (Detalle != null)
                    {
                        objCabecera.Detalle.Remove(objCabecera.Detalle.Where(p => p.NUMERO_CARGA == NUMERO_CARGA).FirstOrDefault());

                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información de la carga: {0}, para quitar del detalle </b>", NUMERO_CARGA.ToString()));
                        return;
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

                    

                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                   

                    OcultarLoading("2");
                    bool Ocultar_Mensaje = true;


                    string Msg = string.Empty;


                    this.LabelTotal.InnerText = string.Format("DETALLE DE CARGAS AGREGADAS ");

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
                   

                    if (this.CboCiudad.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor seleccionar la ciudad de destino</b>"));
                        this.CboCiudad.Focus();
                        return;
                    }

                    if (this.CboZonas.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor seleccionar la zona de destino</b>"));
                        this.CboZonas.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.Txtdireccion.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar la dirección de destino</b>"));
                        this.Txtdireccion.Focus();
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



                    /*para almacenar clientes que asumen factura*/
                    List_Asume = new List<Cls_Bil_AsumeFactura>();
                    List_Asume.Clear();
                    List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = string.Empty, nombre = string.Empty, mostrar = string.Format("{0}", "Seleccionar...") });

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                   // ClsUsuario.ruc = "0992424311001";

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


                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i> <b> Error! Freight Forwarder [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 1,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", ClsUsuario.ruc, ClsUsuario.nombres));

                            Ocultar_Mensaje = false;
                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("Se presentaron los siguientes problemas: No exsite información del Freight Forwarder, no registrado: {0}", ClsUsuario.ruc);

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Carga Suelta", "No existe información del Freight Forwarder, no registrado.", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), ""),
                                ClsUsuario.nombres, ClsUsuario.ruc, out MensajeCasos, false);

                            /*************************************************************************************************************************************
                            * fin caso salesforce
                            * **********************************************************************************************************************************/
                            return;
                        }


                    }

                   
                    //busca contenedores por ruc de usuario
                    if (IdAgenteCodigo.Equals("CGSA"))
                    {
                        IdAgenteCodigo = string.Empty;
                    }


                    var Validacion = new Aduana.Importacion.ecu_validacion_cntr_cfs();
                    var EcuaContenedores = Validacion.CargaPorManifiestoImpoP2D_FF(ClsUsuario.loginname, ClsUsuario.ruc, IdAgenteCodigo, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), true);//ecuapas contenedores
                    if (EcuaContenedores.Exitoso)
                    {
                        //DATOS DEL AGENTE PARA BUSCAR INFORMACION
                        var LinqAgente = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                          select new
                                          {
                                              ID_AGENTE = Tbl.agente_id,
                                              ID_CLIENTE = Tbl.importador_id,
                                              DESC_CLIENTE = (Tbl.importador == null ? string.Empty : Tbl.importador),
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
                                    this.hf_idcliente.Value = ListaAgente.CLNT_CUSTOMER;
                                    this.hf_idasume.Value = ListaAgente.CLNT_CUSTOMER;


                                    this.TXTAGENCIA.Text = string.Format("{0} - {1}", ListaAgente.CLNT_CUSTOMER.Trim(), ListaAgente.CLNT_NAME.Trim());
                                    this.hf_rucagente.Value = ListaAgente.CLNT_CUSTOMER.Trim();
                                    this.hf_descagente.Value = ListaAgente.CLNT_NAME.Trim();

                                    this.TXTCLIENTE.Text = string.Format("{0} - {1}", ListaAgente.CLNT_CUSTOMER.Trim(), ListaAgente.CLNT_NAME.Trim());
                                    this.TXTASUMEFACTURA.Text = ListaAgente.CLNT_CUSTOMER.Trim() + " - " + ListaAgente.CLNT_NAME.Trim();
                                    this.hf_desccliente.Value = ListaAgente.CLNT_NAME.Trim();
                                    this.hf_descasume.Value = ListaAgente.CLNT_NAME.Trim();

                                   
                                   //agrega importador
                                    List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = ListaAgente.CLNT_CUSTOMER.Trim(), nombre = ListaAgente.CLNT_NAME.Trim(), mostrar = string.Format("{0} - {1}", ListaAgente.CLNT_CUSTOMER.Trim(), ListaAgente.CLNT_NAME.Trim()) });

                                    
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

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "No existe información del cliente, no registrado.", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), ""),
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


                        if (List_Asume.Count >= 2)
                        {
                            this.CboAsumeFactura.SelectedIndex = 1;
                        }


                        //informacion ecuapass     
                        var LinqPartidasEcua = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                            select new
                                            {
                                                mrn = Tbl.mrn,
                                                msn = Tbl.msn,
                                                hsn = Tbl.hsn,
                                                IMDT = (Tbl.imdt_id == null) ? "" : Tbl.imdt_id,
                                                GKEY = (Tbl.gkey == null ? 0 : Tbl.gkey),
                                                cntr = (Tbl.cntr == null) ? "" : Tbl.cntr,
                                                BL = (Tbl.documento_bl == null) ? "" : Tbl.documento_bl,
                                                DECLARACION = (Tbl.declaracion == null) ? "" : Tbl.declaracion,
                                                ESTADO_RIDT = (Tbl.ridt_estado == null ? "" : Tbl.ridt_estado),
                                                CARGA = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                                TIPO_CLIENTE = Tbl.tipo_cliente,
                                                importador_id = (Tbl.importador_id == null) ? "" : Tbl.importador_id,
                                                importador_name = (Tbl.importador == null) ? "" : Tbl.importador,
                                                numero_carga = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                                descripcion = (Tbl.descripcion == null) ? "" : Tbl.descripcion,
                                                total_partida = (Tbl.total_partida == null ? 0 : Tbl.total_partida),
                                                uuid = (Tbl.uuid == null ? 0 : Tbl.uuid),
                                            }).Distinct();

                        //agrego todos los contenedores a la clase cabecera
                        objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                        objCabecera.Detalle_CargasPendientes.Clear();

                        this.grillacargas.DataSource = null;
                        this.grillacargas.DataBind();

                        int i = 0;
                        foreach (var Det in LinqPartidasEcua)
                        {
                            objDet = new cfs_cargas_pendientes_detalle();
                            objDet.mrn = Det.mrn;
                            objDet.msn = Det.msn;
                            objDet.hsn = Det.hsn;
                            objDet.cntr = Det.cntr;
                            objDet.importador_id = Det.importador_id;
                            objDet.importador_name = Det.importador_name;
                            objDet.descripcion = Det.descripcion;
                            objDet.total_partida = Det.total_partida.Value;
                            objDet.visto = false;
                            objDet.numero_carga = string.Format("{0}-{1}-{2}", Det.mrn, Det.msn, Det.hsn);
                            objDet.imdt = Det.IMDT;
                            objDet.declaracion = Det.DECLARACION;
                            objDet.estado_ridt = Det.ESTADO_RIDT;
                            objDet.tipo_cliente = Det.TIPO_CLIENTE;
                            objCabecera.Detalle_CargasPendientes.Add(objDet);
                            i++;
                        }

                        if (i > 0)
                        {
                            Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;

                            this.grillacargas.DataSource = objCabecera.Detalle_CargasPendientes;
                            this.grillacargas.DataBind();
                        }
                        else
                        {
                            this.grillacargas.DataSource = null;
                            this.grillacargas.DataBind();

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de carga suelta pendiente de facturar con el número de la carga ingresada..{0}</b>", EcuaContenedores.MensajeProblema));
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




        //revisando 1
        protected void BtnAbregar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    //this.BtnFacturar.Attributes.Add("disabled", "disabled");

                    OcultarLoading("2");
                    bool Ocultar_Mensaje = true;
                  
                  
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
                    //if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    //{
                    //    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga HSN</b>"));
                    //    this.TXTHSN.Focus();
                    //    return;
                    //}

                    if (this.CboCiudad.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor seleccionar la ciudad de destino</b>"));
                        this.CboCiudad.Focus();
                        return;
                    }

                    if (this.CboZonas.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor seleccionar la zona de destino</b>"));
                        this.CboZonas.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.Txtdireccion.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar la dirección de destino</b>"));
                        this.Txtdireccion.Focus();
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
                   // ClsUsuario.ruc = "0992424311001";

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
                           

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i> <b> Error! Freight Forwarder [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 1,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", ClsUsuario.ruc, ClsUsuario.nombres));

                            Ocultar_Mensaje = false;
                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("Se presentaron los siguientes problemas: No exsite información del Freight Forwarder, no registrado: {0}", ClsUsuario.ruc);

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación Carga Suelta", "No existe información del Freight Forwarder, no registrado.", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), ""),
                                ClsUsuario.nombres, ClsUsuario.ruc, out MensajeCasos, false);

                            /*************************************************************************************************************************************
                            * fin caso salesforce
                            * **********************************************************************************************************************************/
                            return;
                        }
                        

                    }

                    //valida sila carga ya esta facturada
                    List<P2D_MULTI_Pendiente> ListExiste = P2D_MULTI_Pendiente.Valida_TieneFactura(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), "", out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error al validar tiene factura....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }
                    if (ListExiste != null)
                    {
                        if (ListExiste.Count != 0)
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! El número de carga {0} ingresado ya se encuetra facturado, no podrá agregar el mismo...</b>", 
                                 string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), "")));
                           // this.TXTHSN.Focus();
                            return;
                        }
                    }
                        //fin

                        //busca contenedores por ruc de usuario
                        if (IdAgenteCodigo.Equals("CGSA"))
                    {
                        IdAgenteCodigo = string.Empty;
                    }
                   

                    var Validacion = new Aduana.Importacion.ecu_validacion_cntr_cfs();
                    var EcuaContenedores = Validacion.CargaPorManifiestoImpoFF(ClsUsuario.loginname, ClsUsuario.ruc, IdAgenteCodigo, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), "", true);//ecuapas contenedores
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

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "No existe información del cliente, no registrado.", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), ""),
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


                        if (List_Asume.Count >= 2)
                        {
                            this.CboAsumeFactura.SelectedIndex = 1;
                        }


                        //INFORMACION DEL CONTENEDOR
                        var Contenedor = new N4.Importacion.container_cfs();
                        var ListaContenedores = Contenedor.CargaPorBL(ClsUsuario.loginname, this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), "");//resultado de entidad contenedor y cfs
                        if (ListaContenedores.Exitoso)
                        {

                            this.BtnFacturar.Attributes.Remove("disabled");

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

                           


                            /*left join de contenedores*/
                            var LinqQuery = (from Tbl in LinqPartidadN4
                                             join EcuaPartidas in LinqPartidas on Tbl.CARGA equals EcuaPartidas.CARGA into TmpFinal
                                             //join Factura in LinqUltimaFactura on Tbl.CARGA equals Factura.FT_NUMERO_CARGA into TmpFactura
                                             from Final in TmpFinal.DefaultIfEmpty()
                                             //from FinalFT in TmpFactura.DefaultIfEmpty()
                                             select new
                                             {
                                                    CONTENEDOR = Tbl.CNTR_CONTAINER,
                                                    REFERENCIA = (Tbl.CNTR_VEPR_REFERENCE == null) ? string.Empty : Tbl.CNTR_VEPR_REFERENCE,
                                                    TRAFICO = (Tbl.CNTR_TYPE == null) ? string.Empty : Tbl.CNTR_TYPE,
                                                    TAMANO = (Tbl.CNTR_TYSZ_SIZE == null) ? string.Empty : Tbl.CNTR_TYSZ_SIZE,
                                                    TIPO = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
                                                    FECHA_CAS = System.DateTime.Now,
                                                    AUTORIZADO = true ,
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
                                                    FECHA_ULTIMA = (DateTime?)null,
                                                    NUMERO_FACTURA = string.Empty,
                                                    ID_FACTURA = 0 ,
                                                    VIAJE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                                    NAVE = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                                    FECHA_ARRIBO = Tbl.CNTR_VEPR_ACTUAL_ARRIVAL,
                                                    CNTR_DD = Tbl.CNTR_DD,
                                                    FECHA_HASTA =  (DateTime?)null,
                                                    ESTADO_RIDT = (Final.ESTADO_RIDT == null) ? string.Empty : Final.ESTADO_RIDT,
                                                    CNTR_DESCARGA = (Tbl.CNTR_DESCARGA==null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                                    MODULO = string.Empty ,
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
                                objCabecera.NUMERO_CARGA = this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" +"";
                                objCabecera.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objCabecera.SESION = this.hf_BrowserWindowName.Value;
                                objCabecera.HORA_HASTA = "00:00";
                                objCabecera.FECHA_HASTA = DateTime.Now;

                                //objCabecera.Detalle.Clear();
                                //valido si existe la carga, para no agregarla
                                string LLAVE = string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim().ToUpper(), this.TXTMSN.Text.Trim().ToUpper(), ""); ;
                                var Detalle = objCabecera.Detalle.FirstOrDefault(f => f.NUMERO_CARGA.Equals(LLAVE));
                                if (Detalle == null)
                                {
                                    Int16 Secuencia = Int16.Parse((objCabecera.Detalle.Count + 1).ToString());

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
                                        objDetalle.P2D_ID_CLIENTE = this.hf_idcliente.Value;
                                        objDetalle.P2D_DESC_CLIENTE = this.hf_desccliente.Value;
                                        objDetalle.P2D_ID_FACTURADO = this.hf_idasume.Value;
                                        objDetalle.P2D_DESC_FACTURADO = this.hf_descasume.Value;
                                        objDetalle.P2D_ID_AGENTE = this.hf_rucagente.Value;
                                        objDetalle.P2D_DESC_AGENTE = this.hf_descagente.Value;
                                        objDetalle.P2D_DIRECCION = this.Txtdireccion.Text.Trim();

                                        objDetalle.VISTO = false;
                                        objDetalle.ID = Det.ID_FACTURA;
                                        objDetalle.SECUENCIA = Secuencia;
                                        objDetalle.GKEY = Det.GKEY;
                                        objDetalle.NUMERO_CARGA = string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim().ToUpper(), this.TXTMSN.Text.Trim().ToUpper(), "");
                                        objDetalle.MRN = this.TXTMRN.Text.Trim();
                                        objDetalle.MSN = this.TXTMSN.Text.Trim();
                                        //objDetalle.HSN = this.TXTHSN.Text.Trim();
                                        objDetalle.CONTENEDOR = Det.CONTENEDOR;
                                        objDetalle.TRAFICO = Det.TRAFICO;
                                        objDetalle.DOCUMENTO = Det.DOCUMENTO;
                                        objDetalle.DES_BLOQUEO = Det.BLOQUEOS;
                                        objDetalle.CONECTADO = Det.CONECTADO;
                                        objDetalle.REFERENCIA = Det.REFERENCIA;
                                        objDetalle.TAMANO = Det.TAMANO;
                                        objDetalle.TIPO = "CFS";
                                        objDetalle.CAS = Det.FECHA_CAS;
                                        objDetalle.AUTORIZADO = (Det.AUTORIZADO == true ? "SI" : "NO");
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
                                        objDetalle.CANTIDAD = decimal.Parse(Det.CANTIDAD.Value.ToString());
                                        objDetalle.PESO = decimal.Parse(Det.PESO.ToString());
                                        objDetalle.OPERACION = Det.OPERACION;
                                        objDetalle.DESCRIPCION = Det.DESCRIPCION;
                                        objDetalle.EXPORTADOR = Det.EXPORTADOR;
                                        objDetalle.AGENCIA = Det.AGENCIA;
                                        //nuevo
                                        objDetalle.ID_UNIDAD = Det.ID_UNIDAD;

                                        

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
                                           
                                        }


                                        objDetalle.IDPLAN = "0";
                                        objDetalle.TURNO = "* Seleccione *";


                                        objDetalle.CERTIFICADO = Det.CERTIFICADO;
                                        objDetalle.TIENE_CERTIFICADO = Det.TIENE_CERTIFICADO;
                                        objDetalle.VISTO = true;

                                        objCabecera.Detalle.Add(objDetalle);
                                        Secuencia++;
                                    }

                                    //this.TXTMRN.Text = string.Empty;
                                    //this.TXTMSN.Text = string.Empty;
                                    //this.TXTHSN.Text = "";
                                    //this.TXTHSN.Focus();
                                }
                                else
                                {
                                    //total de bultos
                                    var TotalBultos2 = objCabecera.Detalle.Sum(x => x.CANTIDAD);
                                    objCabecera.TOTAL_BULTOS = TotalBultos2;

                                    //agrega a la grilla
                                    tablePagination.DataSource = objCabecera.Detalle;
                                    tablePagination.DataBind();

                                    this.LabelTotal.InnerText = string.Format("DETALLE DE CARGA SUELTA (CFS) - Total Bultos: {0}", objCabecera.TOTAL_BULTOS);

                                    Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;
                                    this.Actualiza_Panele_Detalle();

                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! el número de la carga ingresado {0}, ya se encuentra registrado..</b>", LLAVE));
                                    //this.TXTHSN.Focus();
                                    this.Actualiza_Paneles();
                                    return;
                                }


                                this.Txtdireccion.Text = string.Empty;
                                this.Txtdireccion.Focus();

                                //total de bultos
                                var TotalBultos = objCabecera.Detalle.Sum(x => x.CANTIDAD);
                                objCabecera.TOTAL_BULTOS = TotalBultos;

                                //agrega a la grilla
                                tablePagination.DataSource = objCabecera.Detalle;
                                tablePagination.DataBind();

                                this.LabelTotal.InnerText = string.Format("DETALLE DE CARGA SUELTA (CFS) - Total Bultos: {0}", objCabecera.TOTAL_BULTOS);

                                Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;
                                this.Actualiza_Panele_Detalle();

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

                Response.Redirect("~/p2d/p2d_facturafreightforwarder.aspx", false);



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
           
        }

        protected void BtnConfirmar_Click(object sender, EventArgs e)
        {
            this.Mostrar_Mensaje_Proceso(string.Format("<b>GENERANDO FACTURA DE MULTIDESPACHO TRANSPORTE! </b>....POR FAVOR ESPERE, SE ESTA GENERANDO LA FACTURA......."));

            this.ImgFactura.Attributes["class"] = "ver";
            this.UPBOTONES.Update();


            if (Response.IsClientConnected)
            {
                try
                {
                    CultureInfo enUS = new CultureInfo("en-US");



                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }


                    this.BtnConfirmar.Attributes["disabled"] = "disabled";
                    this.Actualiza_Paneles();

                    string IdAgenteCodigo = string.Empty;
                    string ID_AGENTE = string.Empty;
                    string DESC_AGENTE = string.Empty;
                    string CODIGO_AGENTE = string.Empty;
                    string NOMBRES_AGENTE = string.Empty;

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    CODIGO_AGENTE = ClsUsuario.ruc.Trim();

                    //valida que se seleccione la persona a facturar
                    if (this.CboAsumeFactura.SelectedIndex == -1)
                    {
                        this.ImgFactura.Attributes["class"] = "nover";
                        this.CboAsumeFactura.Focus();
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona que asumirá la factura. </b>"));
                        return;
                    }

                    string ID_FACTURADO = this.CboAsumeFactura.SelectedValue;

                    var Cliente = N4.Entidades.Cliente.ObtenerCliente(LoginName, ID_FACTURADO);
                    if (Cliente.Exitoso)
                    {
                        var ListaCliente = Cliente.Resultado;
                        if (ListaCliente != null)
                        {
                            DESC_AGENTE = ListaCliente.CLNT_NAME;
                            ID_AGENTE = ListaCliente.CLNT_CUSTOMER.Trim();
                        }
                        else
                        {
                            this.ImgFactura.Attributes["class"] = "nover";
                            this.CboAsumeFactura.Focus();
                            this.UPBOTONES.Update();
                            this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", ID_FACTURADO));
                            return;
                        }

                    }
                    else
                    {
                        this.ImgFactura.Attributes["class"] = "nover";
                        this.CboAsumeFactura.Focus();
                        this.UPBOTONES.Update();

                        var ExisteAsume = CboAsumeFactura.Items.FindByValue(ID_FACTURADO);
                        if (ExisteAsume != null)
                        {
                            DESC_AGENTE = ExisteAsume.Text.Split('-').ToList()[1].Trim();

                        }

                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", ID_FACTURADO, DESC_AGENTE));
                        return;
                    }


                    //instancia sesion
                    objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    if (objCabecera == null)
                    {
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la factura </b>"));
                        return;
                    }
                    if (objCabecera.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de carga suelta, para poder generar la factura </b>"));
                        return;
                    }

                    var LinqCargas = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                      select p.NUMERO_CARGA).ToList();


                    if (LinqCargas.Count == 0)
                    {
                        this.ImgFactura.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas a facturar. </b>"));
                        return;
                    }

                    if (LinqCargas.Count <= 1)
                    {
                        this.ImgFactura.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas a facturar. </b>"));
                        return;
                    }

                    var Cantidades = objCabecera.Detalle.Count;

                    var oServicioMulti = P2D_MULTI_buscar_tarifas.GetServicio(Int64.Parse(this.CboCiudad.SelectedValue.ToString()), Cantidades, "MULTI");
                    var oServicioTrans = P2D_MULTI_buscar_tarifas.GetServicio(Int64.Parse(this.CboCiudad.SelectedValue.ToString()), Cantidades, "TRANS");

                    if (oServicioMulti == null)
                    {

                        // this.BtnGrabar.Attributes.Remove("disabled");
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe tarifa de MULTIDESPACHO creada para poder facturar</b>"));
                        return;
                    }

                    if (oServicioTrans == null)
                    {

                        // this.BtnGrabar.Attributes.Remove("disabled");
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe tarifa de TRANSPORTE creada para poder facturar</b>"));
                        return;
                    }

                    string request = string.Empty;
                    string response = string.Empty;
                    string v_Error = "";
                    decimal v_monto = 0;
                    string Numero_Factura = string.Empty;
                    string numeros_cargas = string.Join(", ", LinqCargas);

                    string tarifa1 = string.Format("{0},{1},{2}", oServicioMulti.CODIGO_TARIJA_N4.Trim(), oServicioMulti.FACTOR.ToString().Replace(".00", ""), oServicioMulti.DESCRIPCION.Trim());
                    string tarifa2 = string.Format("{0},{1},{2}", oServicioTrans.CODIGO_TARIJA_N4.ToString().Trim(), oServicioTrans.FACTOR.ToString().Replace(".00", ""), oServicioTrans.DESCRIPCION.Trim());

                    Respuesta.ResultadoOperacion<bool> resp;
                    resp = ServicioSCA.CargarServicioMultiDespachoTransporte(oServicioMulti.INVOICETYPE, ID_AGENTE, tarifa1, tarifa2, numeros_cargas, Page.User.Identity.Name.ToUpper(), out request, out response);

                    //concepto de multidespacho
                    var oTrace = cfs_buscar_tarifas.SaveTrace(ID_AGENTE, oServicioMulti.TIPO, oServicioMulti.INVOICETYPE, oServicioMulti.ID_TARIFA, oServicioMulti.CODIGO_TARIJA_N4, request, response, resp.Exitoso, Page.User.Identity.Name, out v_Error);
                    if (!string.IsNullOrEmpty(v_Error))
                    {
                        this.ImgFactura.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", v_Error));
                        return;

                    }

                    //concepto de transporte
                    var oTraceTrans = cfs_buscar_tarifas.SaveTrace(ID_AGENTE, oServicioTrans.TIPO, oServicioTrans.INVOICETYPE, oServicioTrans.ID_TARIFA, oServicioTrans.CODIGO_TARIJA_N4, request, response, resp.Exitoso, Page.User.Identity.Name, out v_Error);
                    if (!string.IsNullOrEmpty(v_Error))
                    {
                        this.ImgFactura.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", v_Error));
                        return;

                    }

                    if (resp.Exitoso)
                    {

                        var v_result = resp.MensajeInformacion.Split(',');

                        v_monto = decimal.Parse(v_result[1].ToString());

                        Numero_Factura = "00" + v_result[0];
                        string Establecimiento = Numero_Factura.Substring(0, 3);
                        string PuntoEmision = Numero_Factura.Substring(3, 3);
                        string Original = Numero_Factura.Substring(6, 9);
                        string FacturaFinal = string.Format("{0}-{1}-{2}", Establecimiento, PuntoEmision, Original);


                        decimal _Subtotal = (oServicioMulti.VALOR + oServicioTrans.VALOR);
                        decimal _Iva = (oServicioMulti.VALOR_IVA + oServicioTrans.VALOR_IVA);
                        decimal _Total = (oServicioMulti.VALOR_TOTAL + oServicioTrans.VALOR_TOTAL);

                        /*nuevo proceso de grabado*/
                        System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new System.Xml.Linq.XElement("FACT_CABECERA",
                                                                new System.Xml.Linq.XElement("CABECERA",
                                                                new System.Xml.Linq.XAttribute("IV_FECHA", System.DateTime.Now),
                                                                new System.Xml.Linq.XAttribute("IV_TIPO_CARGA", objCabecera.TIPO_CARGA == null ? "" : objCabecera.TIPO_CARGA),
                                                                new System.Xml.Linq.XAttribute("IV_ID_AGENTE", objCabecera.ID_AGENTE == null ? "" : objCabecera.ID_AGENTE),
                                                                new System.Xml.Linq.XAttribute("IV_DESC_AGENTE", objCabecera.DESC_AGENTE == null ? "" : objCabecera.DESC_AGENTE),
                                                                new System.Xml.Linq.XAttribute("IV_ID_CLIENTE", objCabecera.ID_CLIENTE == null ? "" : objCabecera.ID_CLIENTE),
                                                                new System.Xml.Linq.XAttribute("IV_DESC_CLIENTE", objCabecera.DESC_CLIENTE == null ? "" : objCabecera.DESC_CLIENTE),
                                                                new System.Xml.Linq.XAttribute("IV_ID_FACTURADO", objCabecera.ID_FACTURADO == null ? "" : objCabecera.ID_FACTURADO),
                                                                new System.Xml.Linq.XAttribute("IV_DESC_FACTURADO", objCabecera.DESC_FACTURADO == null ? "" : objCabecera.DESC_FACTURADO),
                                                                new System.Xml.Linq.XAttribute("IV_TOTAL_BULTOS", objCabecera.TOTAL_BULTOS),
                                                                new System.Xml.Linq.XAttribute("IV_SUBTOTAL", _Subtotal),
                                                                new System.Xml.Linq.XAttribute("IV_IVA", _Iva),
                                                                new System.Xml.Linq.XAttribute("IV_TOTAL", _Total),
                                                                new System.Xml.Linq.XAttribute("IV_FACTURA", FacturaFinal),
                                                                new System.Xml.Linq.XAttribute("IV_NUMERO_CARGA", numeros_cargas),
                                                                new System.Xml.Linq.XAttribute("IV_USUARIO_CREA", Page.User.Identity.Name.ToUpper()),
                                                                new System.Xml.Linq.XAttribute("IV_TOTAL_N4", v_monto),
                                                                new System.Xml.Linq.XAttribute("flag", "I"))));


                        System.Xml.Linq.XDocument XMLServicios = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new System.Xml.Linq.XElement("FACT_SERVICIOS",
                                                                    new System.Xml.Linq.XElement("DETALLE",
                                                                    new System.Xml.Linq.XAttribute("IV_ID_TARIFA", oServicioMulti.ID_TARIFA),
                                                                    new System.Xml.Linq.XAttribute("IV_TIPO", oServicioMulti.TIPO),
                                                                    new System.Xml.Linq.XAttribute("IV_ID_TIPO", oServicioMulti.ID_TIPO),
                                                                    new System.Xml.Linq.XAttribute("IV_CANTIDAD", LinqCargas.Count),
                                                                    new System.Xml.Linq.XAttribute("IV_FACTOR", oServicioMulti.FACTOR),
                                                                    new System.Xml.Linq.XAttribute("IV_VALOR", oServicioMulti.VALOR),
                                                                    new System.Xml.Linq.XAttribute("IV_INVOICETYPE", oServicioMulti.INVOICETYPE),
                                                                    new System.Xml.Linq.XAttribute("IV_CODIGO_TARIJA_N4", oServicioMulti.CODIGO_TARIJA_N4),
                                                                    new System.Xml.Linq.XAttribute("IV_ID_CIUDAD", oServicioMulti.ID_CIUDAD),
                                                                    new System.Xml.Linq.XAttribute("IV_ID_ZONA", oServicioMulti.ID_ZONA),
                                                                    new System.Xml.Linq.XAttribute("IV_USUARIO_CREA", Page.User.Identity.Name.ToUpper()),
                                                                    new System.Xml.Linq.XAttribute("flag", "I")),
                                                                    new System.Xml.Linq.XElement("DETALLE",
                                                                    new System.Xml.Linq.XAttribute("IV_ID_TARIFA", oServicioTrans.ID_TARIFA),
                                                                    new System.Xml.Linq.XAttribute("IV_TIPO", oServicioTrans.TIPO),
                                                                    new System.Xml.Linq.XAttribute("IV_ID_TIPO", oServicioTrans.ID_TIPO),
                                                                    new System.Xml.Linq.XAttribute("IV_CANTIDAD", LinqCargas.Count),
                                                                    new System.Xml.Linq.XAttribute("IV_FACTOR", oServicioTrans.FACTOR),
                                                                    new System.Xml.Linq.XAttribute("IV_VALOR", oServicioTrans.VALOR),
                                                                    new System.Xml.Linq.XAttribute("IV_INVOICETYPE", oServicioTrans.INVOICETYPE),
                                                                    new System.Xml.Linq.XAttribute("IV_CODIGO_TARIJA_N4", oServicioTrans.CODIGO_TARIJA_N4),
                                                                    new System.Xml.Linq.XAttribute("IV_ID_CIUDAD", oServicioTrans.ID_CIUDAD),
                                                                    new System.Xml.Linq.XAttribute("IV_ID_ZONA", oServicioTrans.ID_ZONA),
                                                                    new System.Xml.Linq.XAttribute("IV_USUARIO_CREA", Page.User.Identity.Name.ToUpper()),
                                                                    new System.Xml.Linq.XAttribute("flag", "I"))
                                                                    ));

                        
                        System.Xml.Linq.XDocument XMLDetalle = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                               new System.Xml.Linq.XElement("FACT_DETALLE", from p in objCabecera.Detalle.AsEnumerable().AsParallel()
                                                                           select new System.Xml.Linq.XElement("DETALLE",
                                                                               new System.Xml.Linq.XAttribute("IV_NUMERO_CARGA", p.NUMERO_CARGA),
                                                                               new System.Xml.Linq.XAttribute("IV_MRN", p.MRN),
                                                                               new System.Xml.Linq.XAttribute("IV_MSN", p.MSN),
                                                                               new System.Xml.Linq.XAttribute("IV_HSN", p.HSN),
                                                                               new System.Xml.Linq.XAttribute("IV_CONTENEDOR", string.IsNullOrEmpty(p.CONTENEDOR) ? "" : p.CONTENEDOR),
                                                                               new System.Xml.Linq.XAttribute("IV_TAMANO", string.IsNullOrEmpty(p.TAMANO) ? "" : p.TAMANO ),
                                                                               new System.Xml.Linq.XAttribute("IV_IMDT", string.IsNullOrEmpty(p.IMDT) ? "" : p.IMDT),
                                                                               new System.Xml.Linq.XAttribute("IV_CANTIDAD", p.CANTIDAD),
                                                                               new System.Xml.Linq.XAttribute("IV_ID_AGENTE", objCabecera.ID_AGENTE == null ? "" : objCabecera.ID_AGENTE),
                                                                               new System.Xml.Linq.XAttribute("IV_DESC_AGENTE", objCabecera.DESC_AGENTE == null ? "" : objCabecera.DESC_AGENTE),
                                                                               new System.Xml.Linq.XAttribute("IV_ID_CLIENTE", objCabecera.ID_CLIENTE == null ? "" : objCabecera.ID_CLIENTE),
                                                                               new System.Xml.Linq.XAttribute("IV_DESC_CLIENTE", objCabecera.DESC_CLIENTE == null ? "" : objCabecera.DESC_CLIENTE),
                                                                               new System.Xml.Linq.XAttribute("IV_ID_FACTURADO", objCabecera.ID_FACTURADO == null ? "" : objCabecera.ID_FACTURADO),
                                                                               new System.Xml.Linq.XAttribute("IV_DESC_FACTURADO", objCabecera.DESC_FACTURADO == null ? "" : objCabecera.DESC_FACTURADO),
                                                                               new System.Xml.Linq.XAttribute("IV_ID_UNIDAD", p.ID_UNIDAD.HasValue ? p.ID_UNIDAD.Value : 0),
                                                                               new System.Xml.Linq.XAttribute("IV_DESCRIPCION", string.IsNullOrEmpty(p.DESCRIPCION) ? "" : p.DESCRIPCION),
                                                                               new System.Xml.Linq.XAttribute("IV_DIRECCION", string.IsNullOrEmpty(p.P2D_DIRECCION) ? "" : p.P2D_DIRECCION),
                                                                               new System.Xml.Linq.XAttribute("IV_ID_CIUDAD", oServicioMulti.ID_CIUDAD),
                                                                               new System.Xml.Linq.XAttribute("IV_ID_ZONA", oServicioMulti.ID_ZONA),
                                                                               new System.Xml.Linq.XAttribute("flag", "I"))));

                        objCargasPendientes = new P2D_MULTI_Facturacion();
                        objCargasPendientes.xmlCabecera = XMLCabecera.ToString();
                        objCargasPendientes.xmlDetalle = XMLDetalle.ToString();
                        objCargasPendientes.xmlServicios = XMLServicios.ToString();

                        var nProceso = objCargasPendientes.SaveTransaction_MultiDespacho(out cMensajes);
                        /*fin de nuevo proceso de grabado*/
                        if (!nProceso.HasValue || nProceso.Value <= 0)
                        {
                            this.ImgFactura.Attributes["class"] = "nover";
                            this.UPBOTONES.Update();
                            this.Id_Factura_Generada = string.Empty;
                            this.hf_idfacturagenerada.Value = string.Empty;
                            this.Mostrar_Mensaje_Factura(string.Format("<b>Error! No se pudo grabar datos de la factura de MultiDespacho..{0}</b>", cMensajes));

                            return;
                        }
                        else
                        {

                            objCabecera.Detalle.Clear();
                            objCabecera.Detalle_CargasPendientes.Clear();

                            Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;


                            //total de bultos
                            var TotalBultos2 = objCabecera.Detalle.Sum(x => x.CANTIDAD);
                            objCabecera.TOTAL_BULTOS = TotalBultos2;

                            //agrega a la grilla
                            tablePagination.DataSource = objCabecera.Detalle;
                            tablePagination.DataBind();

                            //agrega a la grilla
                            this.grillacargas.DataSource = objCabecera.Detalle_CargasPendientes;
                            grillacargas.DataBind();


                            this.LabelTotal.InnerText = string.Format("DETALLE DE CARGA SUELTA (CFS) - Total Bultos: {0}", objCabecera.TOTAL_BULTOS);

                           
                            this.Actualiza_Panele_Detalle();


                            this.Id_Factura_Generada = nProceso.Value.ToString();

                            this.hf_idfacturagenerada.Value = nProceso.Value.ToString();

                            this.BtnVisualizar.Attributes.Remove("disabled");

                            this.OcultarLoading();

                            this.Mostrar_Mensaje_Factura(string.Format("<b>Se procedió a generar la factura de carga suelta [MULTIDESPACHOS/TRANSPORTE] # {0}  {1} ", FacturaFinal, ",Puede proceder a imprimir su comprobante."));

                            this.ImgFactura.Attributes["class"] = "nover";

                            this.BtnConfirmar.Attributes["disabled"] = "disabled";

                            this.Actualiza_Paneles();

                           
                        }

                    }
                    else
                    {
                        this.ImgFactura.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Error! No se logró realizar la facturación en N4...{0} </b>", resp.MensajeProblema));
                        return;

                    }

                }
                catch (Exception ex)
                {
                    this.ImgFactura.Attributes["class"] = "nover";
                    this.UPBOTONES.Update();
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnConfirmar_Click), "BtnConfirmar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }

        }

        protected void BtnVisualizar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;


                if (objCabecera == null)
                {
                    this.Mostrar_Mensaje_Factura(string.Format("<b>Informativo! </b>Aún no ha generado la factura de MultiDespacho/Transporte, para poder imprimirla"));
                    return;
                }

                if (string.IsNullOrEmpty(this.hf_idfacturagenerada.Value))
                {
                    this.Mostrar_Mensaje_Factura(string.Format("<b>Informativo! </b>Aún no ha generado la factura de MultiDespacho/Transporte, para poder imprimirla"));
                    return;
                }

                //IMPRIMIR FACTURA -FORMATO HTML
                string cId = securetext(this.hf_idfacturagenerada.Value);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('../p2d/p2d_facturafreightforwarder_preview.aspx?id_comprobante=" + cId + "');", true);

            }
        }

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

                CultureInfo enUS = new CultureInfo("en-US");
                //NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;

                pnlLoading.Visible = false;

                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }

                    StringBuilder tab = new StringBuilder();

                 
                  
                    this.BtnConfirmar.Attributes["disabled"] = "disabled";
                    this.BtnVisualizar.Attributes["disabled"] = "disabled";

                    this.banmsg_cab.Visible = false;
                    this.banmsg_det2.Visible = false;

                    this.lbl_agente.InnerText = string.Empty;
                    this.lbl_facturado.InnerText = string.Empty;

                    this.lbl_observacion.InnerText = string.Empty;
                    this.lbl_carga.InnerText = string.Empty;

                    this.fecha_factura.InnerText = string.Empty;
                    fecha_hasta.InnerText = string.Empty;

                    this.total.InnerText = string.Empty;


                    tab.Append("<table class='table table-bordered invoice'>");
                    tab.Append("<thead>" +
                          "<tr>" +
                          "<th style = 'width:60px' class='text-center'>CODIGO</th>" +
                          "<th class='text-left'>DESCRIPCION</th>" +
                          "<th style = 'width:60px' class='text-center'>CANTIDAD</th>" +
                          "<th style = 'width:140px' class='text-right'>V.UNITARIO</th>" +
                          "<th style = 'width:90px' class='text-right'>V.TOTAL</th>" +
                          "</tr>" +
                        "<thead>");
                    tab.Append("<tbody>");

                    c_Subtotal = string.Empty;
                    c_Iva = string.Empty;
                    c_Total = string.Empty;
                    // total.InnerText = string.Empty;

                    tab.Append("<tr><td colspan = '3' rowspan = '4'>" +
                    "<h4>Términos y condiciones</h4>" +
                    "<p>Este documento no tiene validez legal alguna. <br/>Debo y pagaré incondicionalmente a la orden de Contecon Guayaquil S.A. en el lugar y fecha que se me reconvenga, " + "" +
                    " el valor  total expresado  en este documento, más los  impuestos legales respectivos en Dólares de los Estados Unidos de América, por los bienes y/o servicios que he recibido a mi entera satisfacción.</p>" +
                    "<td class='text-right'><strong>Subtotal</strong></td>" +
                    "<td class='text-right'><strong>" + c_Subtotal + "</strong></td></td></tr>");

                    tab.Append("<tr><td class='text-right no-border'><strong>Iva 12%</strong></td>" +
                           "<td class='text-right'><strong>" + c_Iva + "</strong></td></tr>");

                    tab.Append("<tr><td class='text-right no-border'>" +
                           "<div class='well well-small rojo'><strong>Total</strong></div>" +
                           "</td>" +
                           "<td class='text-right'><strong>" + c_Total + "</strong></td></tr>");

                    tab.Append("<tbody>");
                    tab.Append("</table>");

                    this.detalle.InnerHtml = tab.ToString();

                    this.Actualiza_Paneles();


                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                   
                    if (this.CboAsumeFactura.Items.Count == 0)
                    {
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona que va asumir la carga</b>"));
                        //this.TXTHSN.Focus();
                        return;
                    }

                    //valida que se seleccione la persona a facturar
                    if (this.CboAsumeFactura.SelectedIndex == 0)
                    {
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona que asumirá la factura.</b>"));
                        this.CboAsumeFactura.Focus();
                        return;
                    }

                   
                    HoraHasta = "00:00";
                   

                    //instancia sesion
                    objCabecera = Session["TransaccionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    if (objCabecera == null)
                    {
                        this.Mostrar_Mensaje_Factura (string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la factura </b>"));
                        return;
                    }
                    if (objCabecera.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de carga suelta, para poder generar la factura </b>"));
                        return;
                    }


                    //valida que seleccione las cargas a facturar
                    var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == false)
                                                select p.CONTENEDOR).ToList();

                    if (LinqValidaContenedor.Count != 0)
                    {
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe agregar la fecha de salida de la carga a Facturar: {0} </b>", objCabecera.NUMERO_CARGA));
                        return;
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
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo obtener la información de la persona que asumirá la factura.</b>"));
                        return;
                    }


                   

                    lbl_agente.InnerText = String.Format("AGENTE ADUANERO: [{0}] - {1}", objCabecera.ID_AGENTE, objCabecera.DESC_AGENTE);
                    lbl_facturado.InnerText = String.Format("FACTURADO A: [{0}] - {1}", objCabecera.ID_FACTURADO, objCabecera.DESC_FACTURADO);

                    this.banmsg_cab.InnerText = string.Empty;
                    this.banmsg_det2.InnerText = string.Empty;

                    if (objCabecera.Detalle.Count <= 1)
                    {
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe agregar  más de una carga para facturar. </b>"));
                        return;
                    }

                    var Cantidades = objCabecera.Detalle.Count;

                    var oServicioMulti = P2D_MULTI_buscar_tarifas.GetServicio(Int64.Parse(this.CboCiudad.SelectedValue.ToString()), Cantidades,"MULTI" );
                    var oServicioTrans = P2D_MULTI_buscar_tarifas.GetServicio(Int64.Parse(this.CboCiudad.SelectedValue.ToString()), Cantidades, "TRANS");

                    if (oServicioMulti == null)
                    {

                        // this.BtnGrabar.Attributes.Remove("disabled");
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe tarifa de MULTIDESPACHO creada para poder facturar</b>"));
                        return;
                    }

                    if (oServicioTrans == null)
                    {

                        // this.BtnGrabar.Attributes.Remove("disabled");
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe tarifa de TRANSPORTE creada para poder facturar</b>"));
                        return;
                    }

                    var LinqCargas = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                      select p.NUMERO_CARGA).ToList();

                    string numeros_cargas = string.Join(", ", LinqCargas);

                    lbl_observacion.InnerText = String.Format("OBSERVACIONES: {0}", "");
                    lbl_carga.InnerText = String.Format("NUMERO DE CARGA: {0}", numeros_cargas);


                    fecha_factura.InnerText = String.Format("{0}", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    fecha_hasta.InnerText = String.Format("{0}", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    fecha_hasta.Visible = false;

                    tab.Clear();

                    decimal _Subtotal = (oServicioMulti.VALOR + oServicioTrans.VALOR);
                    decimal _Iva = (oServicioMulti.VALOR_IVA + oServicioTrans.VALOR_IVA);
                    decimal _Total = (oServicioMulti.VALOR_TOTAL + oServicioTrans.VALOR_TOTAL);

                    c_Subtotal = oServicioMulti.VALOR == 0 ? "..." : string.Format("{0:c}", _Subtotal);
                    c_Iva = oServicioMulti.VALOR_IVA == 0 ? "..." : string.Format("{0:c}", _Iva);
                    c_Total = oServicioMulti.VALOR_TOTAL == 0 ? "..." : string.Format("{0:c}", _Total);
                    total.InnerText = String.Format("{0} USD", c_Total);

                    tab.Append("<table class='table table-bordered invoice'>");
                    tab.Append("<thead>" +
                          "<tr>" +
                          "<th class='text-center'>CODIGO</th>" +
                          "<th class='text-left'>DESCRIPCION</th>" +
                          "<th class='text-center'>CANTIDAD</th>" +
                          "<th class='text-right'>V.UNITARIO</th>" +
                          "<th class='text-right'>V.TOTAL</th>" +
                          "</tr>" +
                        "<thead>");
                    tab.Append("<tbody>");

                    tab.AppendFormat("<tr>" +
                          "<td class='text-center'>{0}</td>" +
                          "<td>{1}</td>" +
                          "<td class='text-center'>{2}</td>" +
                          "<td class='text-right'>{3}</td>" +
                          "<td class='text-right'>{4}</td>" +
                          "</tr>",
                          String.IsNullOrEmpty(oServicioMulti.TIPO) ? "..." : oServicioMulti.TIPO,
                           String.IsNullOrEmpty(oServicioMulti.DESCRIPCION) ? "..." : oServicioMulti.DESCRIPCION.Trim(),
                          string.Format("{0:N2}", 1),
                          oServicioMulti.VALOR == 0 ? "..." : string.Format("{0:c}", oServicioMulti.VALOR),
                          oServicioMulti.VALOR == 0 ? "..." : string.Format("{0:c}", oServicioMulti.VALOR)
                          );

                    tab.AppendFormat("<tr>" +
                        "<td class='text-center'>{0}</td>" +
                        "<td>{1}</td>" +
                        "<td class='text-center'>{2}</td>" +
                        "<td class='text-right'>{3}</td>" +
                        "<td class='text-right'>{4}</td>" +
                        "</tr>",
                        String.IsNullOrEmpty(oServicioTrans.TIPO) ? "..." : oServicioTrans.TIPO,
                         String.IsNullOrEmpty(oServicioTrans.DESCRIPCION) ? "..." : oServicioTrans.DESCRIPCION.Trim(),
                        string.Format("{0:N2}", 1),
                        oServicioTrans.VALOR == 0 ? "..." : string.Format("{0:c}", oServicioTrans.VALOR),
                        oServicioTrans.VALOR == 0 ? "..." : string.Format("{0:c}", oServicioTrans.VALOR)
                        );

                    tab.Append("<tr><td colspan = '3' rowspan = '4'>" +
                    "<h4>Términos y condiciones</h4>" +
                    "<p>Este documento no tiene validez legal alguna. <br/>Debo y pagaré incondicionalmente a la orden de Contecon Guayaquil S.A. en el lugar y fecha que se me reconvenga, " + "" +
                    " el valor  total expresado  en este documento, más los  impuestos legales respectivos en Dólares de los Estados Unidos de América, por los bienes y/o servicios que he recibido a mi entera satisfacción.</p>" +
                    "<td class='text-right'><strong>Subtotal</strong></td>" +
                    "<td class='text-right'><strong>" + c_Subtotal + "</strong></td></td></tr>");

                    tab.Append("<tr><td class='text-right no-border'><strong>Iva 12%</strong></td>" +
                           "<td class='text-right'><strong>" + c_Iva + "</strong></td></tr>");

                    tab.Append("<tr><td class='text-right no-border'>" +
                           "<div class='well well-small rojo'><strong>Total</strong></div>" +
                           "</td>" +
                           "<td class='text-right'><strong>" + c_Total + "</strong></td></tr>");

                    tab.Append("<tbody>");
                    tab.Append("</table>");

                    this.detalle.InnerHtml = tab.ToString();


                    this.BtnConfirmar.Attributes["class"] = "btn btn-primary";
                    this.BtnConfirmar.Attributes.Remove("disabled");



                    this.banmsg_cab.Visible = false;
                    this.banmsg_det2.Visible = false;


                    this.Actualiza_Paneles();

                    objCabecera.SUBTOTAL = _Subtotal;
                    objCabecera.IVA = _Iva;
                    objCabecera.TOTAL = _Total;

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


       

        




    }
}