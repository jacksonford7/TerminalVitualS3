using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BillionEntidades;
using BreakBulk;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Xml.Linq;
using ClsAppCgsa;
using iTextSharp.text.pdf;
using iTextSharp.text;
using CLSiteUnitLogic.Cls_Container;
using CLSiteUnitLogic.Cls_CargaSuelta;
using CLSiteUnitLogic;
using N4Ws.Entidad;
using System.Globalization;

namespace CSLSite.unit
{
    public partial class UNIT_Consulta_Carga : System.Web.UI.Page
    {
        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;

        private Cls_Bil_Invoice_Cabecera objFactura = new Cls_Bil_Invoice_Cabecera();
        private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        #region "variables"
        DataSet dsContenedor;
        DateTime dtFechaHoy;
        DateTime dtFechaFin;
        int numeroDias = 0;

        private Cls_Bil_Proforma_Cabecera objProforma = new Cls_Bil_Proforma_Cabecera();
        private Cls_Bil_Proforma_Detalle objDetalleProforma = new Cls_Bil_Proforma_Detalle();
        private Cls_Bil_Proforma_Servicios objServicios = new Cls_Bil_Proforma_Servicios();

        private Cls_Bil_Invoice_Detalle objDetalleFactura = new Cls_Bil_Invoice_Detalle();
        private Cls_Bil_Invoice_Servicios objServiciosFactura = new Cls_Bil_Invoice_Servicios();
        private Cls_Bil_Invoice_Validaciones objValidacion = new Cls_Bil_Invoice_Validaciones();

        private Cls_Bil_Detalle objDetalle = new Cls_Bil_Detalle();
        private Cls_Bil_Invoice_Actualiza_Pase objActualiza_Pase = new Cls_Bil_Invoice_Actualiza_Pase();
        private List<Cls_Bil_AsumeFactura> List_Asume { set; get; }
        private List<Cls_Bil_Contenedor_DiasLibres> List_Contenedor { set; get; }
        //private List<Cls_Bil_Turnos> List_Turnos { set; get; }
        private List<Cls_Bil_Cas_Manual> List_Autorizacion { set; get; }
        private Cls_Bil_Log_Appcgsa objLogAppCgsa = new Cls_Bil_Log_Appcgsa();
        //private List<N4.Importacion.container> ContainersReefer { set; get; }
        private MantenimientoPaqueteCliente obj = new MantenimientoPaqueteCliente();
        private Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();

        private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();

        private P2D_Proforma_Cabecera objProformaCab = new P2D_Proforma_Cabecera();
        private P2D_Proforma_Detalle objProformaDet = new P2D_Proforma_Detalle();
        private P2D_Tarifario objTarifa = new P2D_Tarifario();
        private P2D_Tarja_Cfs objTarja = new P2D_Tarja_Cfs();
        private Cls_Bil_PasePuertaCFS_SubItems objPaseCFSTarja = new Cls_Bil_PasePuertaCFS_SubItems();
        private cfs_procesa_subsecuencias objGrabaSubSecuencias = new cfs_procesa_subsecuencias();

        private Int64 Gkey = 0;


        private static string TextoServicio = string.Empty;


        private string cMensajes;
        private string sddlvalor = string.Empty;
        private bool tieneServicio = false;

        private string snumero_carga;

        private static string TextoLeyenda = string.Empty;
        private int NDiasLibreas = 0;
        private static string TextoProforma = string.Empty;


        #region "Clases"
        //private static Int64? lm = -3;
        //private string OError;

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;


        //private Int64 Gkey = 0;
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

        //private decimal NEstadoCuenta = 0;

        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;


        private string gkeyBuscado = string.Empty;

        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
        private bool SinDesconsolidar = false;
        private bool SinAutorizacion = false;
        private bool Bloqueos = false;
        #endregion

        #endregion "variables"

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

        public bool UnidadDesconectada { get; private set; }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        private void Actualiza_Paneles()
        {
            UPCARGA.Update();
            UPDETALLE.Update();
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {

            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            OcultarLoading("1");

            this.Actualiza_Paneles();
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

            objCabecera = new Cls_Bil_Cabecera();
            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
        }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    Page.SslOn();
                    sddlvalor = "Ctrn";
                }

                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);

                    return;
                }

                this.banmsg.Visible = IsPostBack;

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                }
#if !DEBUG
                this.IsAllowAccess();
#endif

                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {
                    if (!Page.IsPostBack)
                    {
                        this.TXTMRN.Text = string.Empty;
                        this.TXTMSN.Text = string.Empty;
                        this.TXTHSN.Text = string.Format("{0}", "0000");
                        this.Actualiza_Paneles();
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
            if (Response.IsClientConnected)
            {
                //banmsg.Visible = false;
            }

            Server.HtmlEncode(this.TXTMRN.Text.Trim());
            Server.HtmlEncode(this.TXTMSN.Text.Trim());
            Server.HtmlEncode(this.TXTHSN.Text.Trim());

            if (!Page.IsPostBack)
            {
                try
                {
                    numeroDias = 30;
                    dtFechaHoy = DateTime.Now;
                    dsContenedor = new DataSet("dsContenedor");

                    dtFechaFin = dtFechaHoy.AddDays(numeroDias);

                    this.fecdesde.Text = dtFechaHoy.ToShortDateString();
                    this.fechasta.Text = dtFechaFin.ToShortDateString();

                    dtFechaFin = dtFechaHoy.AddDays(numeroDias);
                    this.Crear_Sesion();

                    bool esRetorno = Session["RetornoDesdeDetalle"] != null && (bool)Session["RetornoDesdeDetalle"];

                    if (esRetorno && Session["UltimoGridMostrado"] != null)
                    {
                        string modo = Session["UltimoGridMostrado"].ToString();


                        ddlOpciones.SelectedValue = modo;
                        AplicarVisibilidad(modo);


                        if (modo == "Book" && Session["DatosContenedorBooking"] is List<Cls_Container> listaBooking)
                        {
                            var resultadoFormateado = listaBooking.Select(r => new
                            {
                                r.CNTR_CONSECUTIVO,
                                r.CNTR_CONTAINER,
                                r.CNTR_TYPE,
                                WEIGHT = r.CNTR_GROSS_WEIGHT,
                                r.CNTR_TYSZ_ISO,
                                r.CNTR_TYSZ_TYPE,
                                r.CNTR_FULL_EMPTY_CODE,
                                r.CNTR_YARD_STATUS,
                                TemperaturaTexto = r.CNTR_TEMPERATURE == 0 ? "Sin temperatura" : r.CNTR_TEMPERATURE.ToString(),
                                r.CNTR_TYPE_DOCUMENT,
                                r.CNTR_DOCUMENT,
                                r.CNTR_CLNT_CUSTOMER_LINE,
                                r.CNTR_LCL_FCL,
                                r.CNTR_CATY_CARGO_TYPE,
                                r.CNTR_FREIGHT_KIND,
                                r.CNTR_DD,
                                r.CNTR_BKNG_BOOKING,
                                r.FECHA_CAS,
                                r.CNTR_AISV,
                                r.CNTR_REEFER_CONT,
                                r.CNTR_VEPR_VSSL_NAME,
                                r.CNTR_VEPR_VOYAGE,
                                r.CNTR_VEPR_ACTUAL_ARRIVAL,
                                r.CNTR_VEPR_ACTUAL_DEPARTED,
                                r.CNTR_VEPR_ESTIMADO_ARRIVAL,
                                r.CNTR_VEPR_ESTIMADO_DEPARTED,
                                r.CNTR_GROSS_WEIGHT,
                                r.CNTR_CITY_UNLOADED,
                                r.CNTR_CITY_ARRIVE,
                                r.CNTR_CITY_LOADED,
                                r.CNTR_PERMANENCIA
                            }).ToList();

                            gdvBooking.DataSource = resultadoFormateado;
                            gdvBooking.DataBind();

                            divFiltroGdvBooking.Visible = true;
                        }
                        else if (modo == "Cntr" && Session["DatosContenedorCntr"] != null)
                        {
                            var listaCntr = Session["DatosContenedorCntr"] as List<ContenedorVista>;
                            if (listaCntr != null)
                            {
                                gbContainer.DataSource = listaCntr;
                                gbContainer.DataBind();

                                divFiltroGbContainer.Visible = true;
                            }
                        }

                        Session.Remove("RetornoDesdeDetalle");
                    }
                    else
                    {
                        Session.Remove("UltimoGridMostrado");
                        Session.Remove("DatosContenedorBooking");
                        Session.Remove("DatosContenedorCntr");

                        AplicarVisibilidad();
                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", ex.Message));
                }
            }
        }


        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showLoader", "mostrarLoaderSwal();", true);


                string modo = ddlOpciones.SelectedValue;

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                switch (modo)
                {
                    case "Book":
                        if (!validacionesBooking())
                        {
                            return;
                        }
                        consultarPorBooking();
             
                        break;
                    case "BL":
                        //   BuscarPorBL();
                        break;
                    case "Cntr":
                        if (!validacionesCNTR())
                        {
                            return;
                        }
                        consultarCNTR();

                        break;
                    case "NC":

                        if (!validacionesCargaSuelta())
                        {
                            return;
                        }
                      
                        consultaCargaSuelta();
                
                        break;
                }

                this.Actualiza_Paneles();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                MostrarError("Error en la consulta: " + ex.Message);
            }
        }
        public void consultaCargaSuelta() 
        {

            string mensajeError = string.Empty;
            var logicaCarga = new Cls_CargaSuelta();
            var resultado = logicaCarga.ConsultarCargaSuelta(
                this.TXTMRN.Text.Trim(),
                this.TXTMSN.Text.Trim(),
                this.TXTHSN.Text.Trim(),
                ClsUsuario.loginname,
                ClsUsuario.ruc,
                out mensajeError
            );

            


            if (resultado.Cabecera != null || resultado.TieneErrores == false)
            {

                divFiltroGdvCargaSuelta.Visible = true;

                MostrarResumen(gdvCargaSuelta, lblResumenCargaSuelta, 1);

                var LinqQuery2 = resultado.Cabecera.Detalle
                     .Where(Tbl => !string.IsNullOrEmpty(Tbl.CONTENEDOR))
                     .Select(Tbl => new CargaSueltaVista
                     {
                         LINEA = Tbl.LINEA,
                         NAVE = resultado.Cabecera.BUQUE,
                         DAI = resultado.Cabecera.NUMERO_CARGA,
                         BULTOS = Convert.ToInt32(Tbl.CANTIDAD),
                         ESTADO = Tbl.ESTADO_RDIT,
                         FECHAINGRESO = Tbl.FECHA_ARRIBO,
                         FECHADESCONSOLIDA = Tbl.FECHA_TOPE_DLIBRE,
                         FECHADESPACHO = Tbl.FECHA_ULTIMA
                     })
                     .OrderBy(x => x.ESTADO)
                     .ToList();

                this.gdvCargaSuelta.DataSource = LinqQuery2;
                this.gdvCargaSuelta.DataBind();
                Session["DatosContenedorCargaSuelta"] = LinqQuery2;
                Session["DatosContenedorCargaSuelta2"] = resultado.Cabecera;
                Session["UltimoGridMostrado"] = "NC";
            }
            else
            {
                MostrarError("No se encontraron registros con los datos ingresados.");
            }
        }
        public void consultarCNTR()
        {

            DateTime fechaHasta = DateTime.Now;
            DateTime fechaDesde = fechaHasta.AddMonths(-3);

            fecdesde.Text = fechaDesde.ToString("dd/MM/yyyy");
            fechasta.Text = fechaHasta.ToString("dd/MM/yyyy");

            DateTime dfecha_desde, dfecha_hasta;
            string formato = "dd/MM/yyyy HH:mm:ss";

            bool desdeOk = DateTime.TryParseExact($"{fecdesde.Text} 00:00:00", formato, null, System.Globalization.DateTimeStyles.None, out dfecha_desde);
            bool hastaOk = DateTime.TryParseExact($"{fechasta.Text} 23:59:59", formato, null, System.Globalization.DateTimeStyles.None, out dfecha_hasta);

            if (!desdeOk) dfecha_desde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day, 0, 0, 0);
            if (!hastaOk) dfecha_hasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);

            snumero_carga = !string.IsNullOrWhiteSpace(TXTMRN.Text)
                ? $"{TXTMRN.Text}-{TXTMSN.Text}-{TXTHSN.Text}"
                : null;


            if (HttpContext.Current.Request.Cookies["token"] == null)
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                Session.Clear();
                return;
            }

            this.gbContainer.DataSource = null;
            this.gbContainer.DataBind();

            string IdAgenteCodigo = string.Empty;

            var AgenteCod = N4.Entidades.Agente.ObtenerAgentePorRuc(ClsUsuario.loginname, ClsUsuario.ruc);
            if (AgenteCod.Exitoso && AgenteCod.Resultado != null)
            {
                IdAgenteCodigo = AgenteCod.Resultado.codigo;
            }

            var Validacion = new Aduana.Importacion.ecu_validacion_cntr();
            var EcuaContenedores = Validacion.CargaPorManifiestoImpoCntr(
                ClsUsuario.loginname, ClsUsuario.ruc, IdAgenteCodigo,
                this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(),
                this.txtcontainer.Text.Trim(), true
            );

            if (EcuaContenedores.Exitoso)
            {
                var Contenedor = new N4.Importacion.container();
                var Gkey = Aduana.Importacion.ecu_validacion_cntr.CargaToListString(EcuaContenedores.Resultado);


                tieneServicio = true;
                var ListaContenedores = Contenedor.CargaPorKeysDerco(
                    ClsUsuario.loginname, Gkey.Resultado, dfecha_desde, dfecha_hasta,
                    this.txtcontainer.Text, snumero_carga, null
                );

                if (ListaContenedores.Exitoso && ListaContenedores.Resultado.Any())
                {
                    var LinqQuery = ListaContenedores.Resultado
                                     .Where(Tbl => !string.IsNullOrEmpty(Tbl.CNTR_CONTAINER))
                                     .Select(Tbl => new ContenedorVista
                                     {
                                         CONTENEDOR = Tbl.CNTR_CONTAINER ?? string.Empty,
                                         CAS = tieneServicio ? Tbl.FECHA_CAS : null,
                                         TIPO_IZO = tieneServicio ? Tbl.CNTR_TYSZ_ISO ?? string.Empty : string.Empty,
                                         Category = tieneServicio ? Tbl.CNTR_TYPE ?? string.Empty : string.Empty,
                                         TIPO_STATE = tieneServicio ? Tbl.CNTR_YARD_STATUS ?? string.Empty : string.Empty,
                                         LINE_OP = tieneServicio ? Tbl.CNTR_CLNT_CUSTOMER_LINE ?? string.Empty : string.Empty,
                                         IB_ACTUAL_VISIT = tieneServicio ? Tbl.CNTR_VEPR_VSSL_NAME ?? string.Empty : string.Empty,
                                         NDOCUMENTO = tieneServicio ? Tbl.IB_CARRIER ?? string.Empty : string.Empty,
                                         FRGHT_KIND = tieneServicio ? Tbl.CNTR_FREIGHT_KIND ?? string.Empty : string.Empty
                                     })
                                     .OrderBy(x =>
                                         x.TIPO_STATE == "EN PATIO" ? 0 :
                                         x.TIPO_STATE == "A BORDO" ? 1 :
                                         x.TIPO_STATE == "DESPACHADO" ? 2 : 3
                                     )
                                     .ToList();



                    this.gbContainer.DataSource = LinqQuery;
                    this.gbContainer.DataBind();
                    divFiltroGbContainer.Visible = true;

                    MostrarResumen(gbContainer, lblResumenCntr, LinqQuery.Count);
                    var LinqQuery2 = ListaContenedores.Resultado
                        .Where(Tbl => !string.IsNullOrEmpty(Tbl.CNTR_CONTAINER))
                        .Select(Tbl => new
                        {
                            GKEY = Tbl.CNTR_CONSECUTIVO,
                            CONTENEDOR = Tbl.CNTR_CONTAINER,
                            CONTENEDORSIZE = Tbl.CNTR_TYSZ_SIZE,
                            VESSEL = Tbl.CNTR_VEPR_VSSL_NAME,
                            ESTADO = Tbl.CNTR_YARD_STATUS,
                            CONSIGNATARIO = Tbl.IB_ACTUAL_VISIT,
                            EXPORTADOR = EcuaContenedores.Resultado.FirstOrDefault(X => X.cntr == Tbl.CNTR_CONTAINER)?.importador ?? string.Empty,
                            FECHACAS = Convert.ToString(Tbl.FECHA_CAS),
                            FECHASALIDA = Convert.ToString(Tbl.CNTR_DESCARGA),
                            FECHADESDE = Convert.ToString(this.fecdesde),
                            FECHAHASTA = Convert.ToString(this.fechasta),
                            MRN = TXTMRN.Text,
                            WEIGHT = "Ver detalle",
                            GROSS = Tbl.CNTR_GROSS_WEIGHT,
                            TARA = Tbl.PESO_MANIFIESTO_TARA,
                            IMDT = Tbl.PESO_IMDT,
                            NODOC = Tbl.CNTR_DOCUMENT,
                            NUMERO_CARGA = snumero_carga,
                            CATEGORY = Tbl.CNTR_TYPE ?? string.Empty,
                            IB_CARRIER = Tbl.IB_CARRIER ?? string.Empty,
                            IB_ACTUAL_VISIT = Tbl.IB_ACTUAL_VISIT ?? string.Empty,
                            VSTATE = Tbl.CNTR_TYSZ_TYPE ?? string.Empty,
                            TIME_IN = Tbl.TIME_IN,
                        })
                        .OrderBy(x => x.VSTATE)
                        .ToList();
                    Session["DatosContenedorCntr"] = LinqQuery;
                    Session["DatosContenedorCntr2"] = LinqQuery2;
                    Session["UltimoGridMostrado"] = "Cntr";



                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);

                    MostrarError("No existen datos con la información ingresada.");
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);

                MostrarError("No existen datos con la información ingresada.");
                return;
            }
        }

        protected void gbContainer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string baseUrl = ResolveUrl("~/unit/UNIT_Consulta_Contenedor.aspx");

                string idContenedor = DataBinder.Eval(e.Row.DataItem, "CONTENEDOR").ToString();
                string encodedId = HttpUtility.UrlEncode(idContenedor);
                string navigateUrl = $"{baseUrl}?id={encodedId}";

                HyperLink lnkContenedor = (HyperLink)e.Row.FindControl("lnkContenedor");
                if (lnkContenedor != null)
                {
                    lnkContenedor.NavigateUrl = navigateUrl;
                }

                Label lblEstado = (Label)e.Row.FindControl("lblVSTATE");
                if (lblEstado != null)
                {
                    string estado = lblEstado.Text.Trim().ToUpper();

                    switch (estado)
                    {
                        case "DESPACHADO":
                            lblEstado.ForeColor = System.Drawing.Color.Gray;
                            break;
                        case "EN PATIO":
                            lblEstado.ForeColor = System.Drawing.Color.Red;
                            break;
                        case "A BORDO":
                            lblEstado.ForeColor = System.Drawing.Color.Green;
                            break;
                        case "EMBARCADO":
                            lblEstado.ForeColor = System.Drawing.Color.Gray;
                            break;
                    }

                    lblEstado.BackColor = System.Drawing.Color.Transparent;
                }
            }
        }

        public bool validacionesCNTR()
        {
            bool mrnIngresado = !string.IsNullOrWhiteSpace(this.TXTMRN.Text);
            bool msnIngresado = !string.IsNullOrWhiteSpace(this.TXTMSN.Text);
            bool cntrIngresado = !string.IsNullOrWhiteSpace(this.txtcontainer.Text);

            if (!(mrnIngresado && msnIngresado) && !cntrIngresado)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                MostrarError("Debe ingresar CNTR (si no ha ingresado MRN y MSN).");
                return false;
            }

            return true;
        }

        public bool validacionesBooking()
        {
            string booking = TXTBooking.Text.Trim();
            string contenedor = txtcontainer.Text.Trim();

            bool bookingVacio = string.IsNullOrWhiteSpace(booking);
            bool contenedorVacio = string.IsNullOrWhiteSpace(contenedor);

            if (bookingVacio && contenedorVacio)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                MostrarError("Debe ingresar al menos BOOKING o CONTENEDOR.");
                return false;
            }

            return true;
        }
        public bool validacionesCargaSuelta()
        {
            bool mrnIngresado = !string.IsNullOrWhiteSpace(this.TXTMRN.Text);
            bool msnIngresado = !string.IsNullOrWhiteSpace(this.TXTMSN.Text);
            bool hsnIngresado = !string.IsNullOrWhiteSpace(this.TXTHSN.Text);

            if (!mrnIngresado || !msnIngresado || !hsnIngresado)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                MostrarError("Debe ingresar MRN, MSN y HSN. Todos son obligatorios.");
                return false;
            }

            return true;
        }



        public void consultarPorBooking()
        {
            try
            {

                var result = Cls_Container.CargaPorBooking(this.TXTBooking.Text, this.txtcontainer.Text.ToString());


                if (result != null && result.Count > 0)
                {

                    Session["DatosContenedorBooking"] = result;
                    var resultadoFormateado = result.Select(r => new
                    {
                        r.CNTR_CONSECUTIVO,
                        r.CNTR_CONTAINER,
                        r.CNTR_TYPE,
                        WEIGHT = r.CNTR_GROSS_WEIGHT,
                        r.CNTR_TYSZ_ISO,
                        r.CNTR_TYSZ_TYPE,
                        r.CNTR_FULL_EMPTY_CODE,
                        r.CNTR_YARD_STATUS,
                        TemperaturaTexto = r.CNTR_TEMPERATURE == 0 ? "Sin temperatura" : r.CNTR_TEMPERATURE.ToString(),
                        r.CNTR_TYPE_DOCUMENT,
                        r.CNTR_DOCUMENT,
                        r.CNTR_CLNT_CUSTOMER_LINE,
                        r.CNTR_LCL_FCL,
                        r.CNTR_CATY_CARGO_TYPE,
                        r.CNTR_FREIGHT_KIND,
                        r.CNTR_DD,
                        r.CNTR_BKNG_BOOKING,
                        r.FECHA_CAS,
                        r.CNTR_AISV,
                        r.CNTR_REEFER_CONT,
                        r.CNTR_VEPR_VSSL_NAME,
                        r.CNTR_VEPR_VOYAGE,
                        r.CNTR_VEPR_ACTUAL_ARRIVAL,
                        r.CNTR_VEPR_ACTUAL_DEPARTED,
                        r.CNTR_VEPR_ESTIMADO_ARRIVAL,
                        r.CNTR_VEPR_ESTIMADO_DEPARTED,
                        r.CNTR_GROSS_WEIGHT,
                        r.CNTR_CITY_UNLOADED,
                        r.CNTR_CITY_ARRIVE,
                        r.CNTR_CITY_LOADED,
                        r.CNTR_PERMANENCIA
                    }).ToList();


                    this.gdvBooking.DataSource = resultadoFormateado;
                    this.gdvBooking.DataBind();
                    divFiltroGdvBooking.Visible = true;
                    Session["UltimoGridMostrado"] = "Book";
                    MostrarResumen(gdvBooking, lblResumenBooking, resultadoFormateado.Count);
                }
                else
                {
                    MostrarError("No hay datos de contenedores.");
                }

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.ToString();
                throw;
            }
        }
        protected void GridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView grid = (GridView)sender;
            string sortExpression = e.SortExpression;
            string direction = GetSortDirection(sortExpression);

            if (grid.ID == "gdvBooking")
            {
                var data = Session["DatosContenedorBooking"] as List<Cls_Container>;
                if (data != null)
                {
                    var formatted = data.Select(r => new
                    {
                        r.CNTR_CONTAINER,
                        r.CNTR_TYPE,
                        r.CNTR_YARD_STATUS,
                        r.CNTR_CLNT_CUSTOMER_LINE,
                        r.CNTR_BKNG_BOOKING,
                        r.CNTR_AISV,
                        TemperaturaTexto = r.CNTR_TEMPERATURE == 0 ? "Sin temperatura" : r.CNTR_TEMPERATURE.ToString(),
                        r.CNTR_LCL_FCL,
                        WEIGHT = r.CNTR_GROSS_WEIGHT
                    }).ToList();

                    var propInfo = formatted.First().GetType().GetProperty(e.SortExpression);
                    var sorted = direction == "ASC"
                        ? formatted.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                        : formatted.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

                    gdvBooking.DataSource = sorted;
                    gdvBooking.DataBind();
                    MostrarResumen(gdvBooking, lblResumenBooking, sorted.Count);
                }
            }
            else if (grid.ID == "gbContainer")
            {
                var data = Session["DatosContenedorCntr"] as List<ContenedorVista>;
                if (data != null)
                {
                    var propInfo = typeof(ContenedorVista).GetProperty(sortExpression);
                    if (propInfo != null)
                    {
                        var sorted = direction == "ASC"
                            ? data.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                            : data.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

                        grid.DataSource = sorted;
                        grid.DataBind();
                        MostrarResumen(grid, lblResumenCntr, sorted.Count);
                    }
                }
            }
        }

        protected void GridView_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            string sortExpression = ViewState["SortExpression"] as string;
            string sortDirection = ViewState["SortDirection"] as string;

            for (int i = 0; i < grid.Columns.Count; i++)
            {
                string currentSort = grid.Columns[i].SortExpression;

                if (string.IsNullOrEmpty(currentSort)) continue;

                string headerText = grid.Columns[i].HeaderText;
                headerText = headerText.Replace(" ▲", "").Replace(" ▼", "");

                if (currentSort == sortExpression)
                {
                    string arrow = sortDirection == "ASC" ? " ▲" : " ▼";
                    headerText += arrow;
                }

                grid.Columns[i].HeaderText = headerText;
            }
        }



        private string GetSortDirection(string column)
        {
            string sortDirection = "ASC";
            string lastDirection = ViewState["SortDirection"] as string;
            string lastColumn = ViewState["SortExpression"] as string;

            if (lastColumn == column && lastDirection == "ASC")
                sortDirection = "DESC";

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }


        protected void ddlOpciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarVisibilidad();
            gdvCargaSuelta.DataSource = null;
            gdvCargaSuelta.DataBind();
            divFiltroGdvCargaSuelta.Visible = false;

            gdvBooking.DataSource = null;
            gdvBooking.DataBind();
            divFiltroGdvBooking.Visible = false;

            gbContainer.DataSource = null;
            gbContainer.DataBind();
            divFiltroGbContainer.Visible = false;
        }

        protected void gdvBooking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string baseUrl = ResolveUrl("~/unit/UNIT_Consulta_Booking.aspx");
                string idContenedor = DataBinder.Eval(e.Row.DataItem, "CNTR_CONTAINER").ToString();

                if (Session["DatosContenedorBooking"] != null)
                {
                    var lista = Session["DatosContenedorBooking"] as List<Cls_Container>;
                    if (lista != null)
                    {
                        var contenedorSeleccionado = lista.FirstOrDefault(x => x.CNTR_CONTAINER == idContenedor);
                        if (contenedorSeleccionado != null)
                        {
                            Session["ContenedorSeleccionado"] = contenedorSeleccionado;
                        }
                    }
                }

                string encodedId = HttpUtility.UrlEncode(idContenedor);
                string navigateUrl = $"{baseUrl}?id={encodedId}";

                HyperLink lnkContenedor = (HyperLink)e.Row.FindControl("lnkCntrBooking");
                if (lnkContenedor != null)
                {
                    lnkContenedor.NavigateUrl = navigateUrl;
                }


                Label lblEstado = (Label)e.Row.FindControl("lblEstadoBooking");
                if (lblEstado != null)
                {
                    string estado = lblEstado.Text.Trim().ToUpper();

                    switch (estado)
                    {
                        case "DESPACHADO":
                            lblEstado.ForeColor = System.Drawing.Color.Gray;
                            break;
                        case "EN PATIO":
                            lblEstado.ForeColor = System.Drawing.Color.Red;
                            break;
                        case "A BORDO":
                            lblEstado.ForeColor = System.Drawing.Color.Green;
                            break;
                    }

                    lblEstado.BackColor = System.Drawing.Color.Transparent;
                }


            }
        }


        protected void gdvBooking_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvBooking.PageIndex = e.NewPageIndex;

            var listaBooking = Session["DatosContenedorBooking"] as List<Cls_Container>;
            if (listaBooking != null)
            {
                var resultadoFormateado = listaBooking.Select(r => new
                {
                    r.CNTR_CONSECUTIVO,
                    r.CNTR_CONTAINER,
                    r.CNTR_TYPE,
                    WEIGHT = r.CNTR_GROSS_WEIGHT,
                    r.CNTR_TYSZ_ISO,
                    r.CNTR_TYSZ_TYPE,
                    r.CNTR_FULL_EMPTY_CODE,
                    r.CNTR_YARD_STATUS,
                    TemperaturaTexto = r.CNTR_TEMPERATURE == 0 ? "Sin temperatura" : r.CNTR_TEMPERATURE.ToString(),
                    r.CNTR_TYPE_DOCUMENT,
                    r.CNTR_DOCUMENT,
                    r.CNTR_CLNT_CUSTOMER_LINE,
                    r.CNTR_LCL_FCL,
                    r.CNTR_CATY_CARGO_TYPE,
                    r.CNTR_FREIGHT_KIND,
                    r.CNTR_DD,
                    r.CNTR_BKNG_BOOKING,
                    r.FECHA_CAS,
                    r.CNTR_AISV,
                    r.CNTR_REEFER_CONT,
                    r.CNTR_VEPR_VSSL_NAME,
                    r.CNTR_VEPR_VOYAGE,
                    r.CNTR_VEPR_ACTUAL_ARRIVAL,
                    r.CNTR_VEPR_ACTUAL_DEPARTED,
                    r.CNTR_VEPR_ESTIMADO_ARRIVAL,
                    r.CNTR_VEPR_ESTIMADO_DEPARTED,
                    r.CNTR_GROSS_WEIGHT,
                    r.CNTR_CITY_UNLOADED,
                    r.CNTR_CITY_ARRIVE,
                    r.CNTR_CITY_LOADED
                }).ToList();

                gdvBooking.DataSource = resultadoFormateado;
                gdvBooking.DataBind();

                MostrarResumen(gdvBooking, lblResumenBooking, resultadoFormateado.Count);
            }
        }
        private void MostrarResumen(GridView grid, Label lbl, int total)
        {
            int inicio = (grid.PageIndex * grid.PageSize) + 1;
            int fin = Math.Min(inicio + grid.PageSize - 1, total);

            if (total == 0)
                lbl.Text = "No hay registros para mostrar.";
            else
                lbl.Text = $"Mostrando registros del {inicio} al {fin} de un total de {total} registros";
        }

        private void AplicarVisibilidad(string modo = null)
        {
            limpiarDivs();

            string selectedValue = modo ?? ddlOpciones.SelectedValue;

            bool isBooking = selectedValue == "Book";
            bool isCargaSuelta = selectedValue == "NC"; 

            divTituloConsulta.InnerText = isBooking ? "CONSULTA DE CARGA EXPORTACIÓN" : "CONSULTA DE CARGA IMPORTACIÓN";

            if (isCargaSuelta)
            {
                divMRN.Visible = true;
                divMSN.Visible = true;
                divHSN.Visible = true;

                divFecIngreso.Visible = false;
                divFecHasta.Visible = false;
                divBooking.Visible = false;
                dvGBooking.Visible = false;
                dvGbContainer.Visible = false;
                divContainer.Visible = false;
                dvGCargaSuelta.Visible = true;
                gdvCargaSuelta.Visible = true;
            }
            else
            {
                divMRN.Visible = divMSN.Visible = divHSN.Visible = divFecIngreso.Visible = divFecHasta.Visible = !isBooking;
                divBooking.Visible = isBooking;
                dvGBooking.Visible = isBooking;
                dvGbContainer.Visible = !isBooking;
                divContainer.Visible = true;
            }

            divBooking.Attributes["class"] = "form-group col-md-2";
            divContainer.Attributes["class"] = "form-group col-md-2";

            txtcontainer.Text = string.Empty;
            divFiltroGbContainer.Visible = gbContainer.Rows.Count > 0;
            divFiltroGdvBooking.Visible = gdvBooking.Rows.Count > 0;
            divFiltroGdvCargaSuelta.Visible = gdvCargaSuelta.Rows.Count > 0;

            if (isBooking)
            {
                divBtnBuscar.Attributes["class"] = "form-group col-md-2";
                divContainer.Parent.Controls.AddAt(divContainer.Parent.Controls.IndexOf(divContainer) + 1, divBtnBuscar);
            }
            else
            {
                var secondRow = UPDETALLE.FindControl("divSecondRow") as Control;
                if (secondRow != null)
                {
                    secondRow.Controls.Add(divBtnBuscar);
                }
            }

            UPDETALLE.Update();
        }

        protected void gdvCargaSuelta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string baseUrl = ResolveUrl("~/unit/UNIT_Consulta_CargaSuelta.aspx");
                string idDai = DataBinder.Eval(e.Row.DataItem, "DAI").ToString();

                if (Session["DatosContenedorCargaSuelta2"] != null)
                {
                    var lista = Session["DatosContenedorCargaSuelta2"] as ResultadoCargaSuelta;
                    if (lista != null)
                    
                    {
                            Session["ContenedorSeleccionadoCargaSuelta"] = lista;
                        
                    }
                }

                string encodedId = HttpUtility.UrlEncode(idDai);
                string navigateUrl = $"{baseUrl}?id={encodedId}";

                HyperLink lnkContenedor = (HyperLink)e.Row.FindControl("lnkDai");
                if (lnkContenedor != null)
                {
                    lnkContenedor.NavigateUrl = navigateUrl;
                }


                //Label lblEstado = (Label)e.Row.FindControl("lblEstadoBooking");
                //if (lblEstado != null)
                //{
                //    string estado = lblEstado.Text.Trim().ToUpper();

                //    switch (estado)
                //    {
                //        case "DESPACHADO":
                //            lblEstado.ForeColor = System.Drawing.Color.Gray;
                //            break;
                //        case "EN PATIO":
                //            lblEstado.ForeColor = System.Drawing.Color.Red;
                //            break;
                //        case "A BORDO":
                //            lblEstado.ForeColor = System.Drawing.Color.Green;
                //            break;
                //    }

                //    lblEstado.BackColor = System.Drawing.Color.Transparent;
                //}


            }
        }
        public void limpiarDivs()
        {
            gbContainer.DataSource = null;
            gbContainer.DataBind();
            gdvBooking.DataSource = null;
            gdvBooking.DataBind();
            this.TXTMRN.Text = null;
            this.TXTMSN.Text = null;
            this.TXTHSN.Text = null;
            this.TXTBooking.Text = null;
            this.txtcontainer.Text = null;

        }

        public void MostrarError(string mensaje)
        {
            string script = $"mostrarError('{mensaje.Replace("'", "\\'")}');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mostrarError", script, true);

        }
        protected void gbContainer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gbContainer.PageIndex = e.NewPageIndex;

            var listaCntr = Session["DatosContenedorCntr"] as List<ContenedorVista>;

            if (listaCntr != null)
            {
                gbContainer.DataSource = listaCntr;
                gbContainer.DataBind();
            }
        }
        private void ActualizarResumen(GridView grid, Label lblResumen)
        {
            int pageSize = grid.PageSize;
            int currentPage = grid.PageIndex;
            int totalRows = grid.DataSource is ICollection collection ? collection.Count : grid.Rows.Count;

            int startRow = (currentPage * pageSize) + 1;
            int endRow = Math.Min(startRow + pageSize - 1, totalRows);

            lblResumen.Text = $"Mostrando registros del {startRow} al {endRow} de un total de {totalRows} registros";
        }
        protected void btnExportarGbContainer_Click(object sender, EventArgs e)
        {
            ExportarGridToExcel(gbContainer, "Contenedor_Import");
        }

        protected void btnExportarGdvBooking_Click(object sender, EventArgs e)
        {
            ExportarGridToExcel(gdvBooking, "Booking_Export");
        }
        protected void GridView_SortingCntr(object sender, GridViewSortEventArgs e)
        {
            var data = Session["DatosContenedorCntr"] as List<ContenedorVista>;
            if (data != null)
            {
                string sortExpression = e.SortExpression;
                string direction = GetSortDirection(sortExpression);

                var propInfo = typeof(ContenedorVista).GetProperty(sortExpression);
                if (propInfo != null)
                {
                    var sorted = direction == "ASC"
                        ? data.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                        : data.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

                    gbContainer.DataSource = sorted;
                    gbContainer.DataBind();
                }
            }
        }
        protected void ddlPageSizeBooking_SelectedIndexChanged(object sender, EventArgs e)
        {
            gdvBooking.PageSize = int.Parse(ddlPageSizeBooking.SelectedValue);
            gdvBooking_PageIndexChanging(gdvBooking, new GridViewPageEventArgs(0));

            ActualizarResumen(gdvBooking, lblResumenBooking);
        }

        protected void ddlPageSizeCntr_SelectedIndexChanged(object sender, EventArgs e)
        {
            gbContainer.PageSize = int.Parse(ddlPageSizeCntr.SelectedValue);
            gbContainer_PageIndexChanging(gbContainer, new GridViewPageEventArgs(0));

            ActualizarResumen(gbContainer, lblResumenCntr);
        }
        protected void gdvCargaSuelta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvCargaSuelta.PageIndex = e.NewPageIndex;

            var listaCarga = Session["DatosContenedorCargaSuelta"] as List<CargaSueltaVista>;
            if (listaCarga != null )
            {
                gdvCargaSuelta.DataSource = listaCarga;
                gdvCargaSuelta.DataBind();
                MostrarResumen(gdvCargaSuelta, lblResumenCargaSuelta, listaCarga.Count);
            }
        }
        protected void btnExportarGdvCargaSuelta_Click(object sender, EventArgs e)
        {
            var listaCarga = Session["DatosContenedorCargaSuelta"] as List<CargaSueltaVista>;
            if (listaCarga == null || listaCarga == null || listaCarga.Count == 0)
                return;

            var exportGrid = new GridView
            {
                AutoGenerateColumns = false,
                EnableViewState = false,
                CssClass = "textmode"
            };

            exportGrid.Columns.Add(new BoundField { DataField = "LINEA", HeaderText = "*LÍNEA" });
            exportGrid.Columns.Add(new BoundField { DataField = "NAVE", HeaderText = "NAVE" });
            exportGrid.Columns.Add(new BoundField { DataField = "DAI", HeaderText = "DAI" });
            exportGrid.Columns.Add(new BoundField { DataField = "BULTOS", HeaderText = "# BULTOS O ITEMS" });
            exportGrid.Columns.Add(new BoundField { DataField = "ESTADO", HeaderText = "ESTADO" });
            exportGrid.Columns.Add(new BoundField { DataField = "FECHAINGRESO", HeaderText = "Fecha de Ingreso", DataFormatString = "{0:dd/MM/yyyy}" });
            exportGrid.Columns.Add(new BoundField { DataField = "FECHADESCONSOLIDA", HeaderText = "Fecha de Desconsolidación", DataFormatString = "{0:dd/MM/yyyy}" });
            exportGrid.Columns.Add(new BoundField { DataField = "FECHADESPACHO", HeaderText = "Fecha de Despacho", DataFormatString = "{0:dd/MM/yyyy}" });

            exportGrid.DataSource = listaCarga;
            exportGrid.DataBind();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", $"attachment;filename=CargaSuelta_Export_{DateTime.Now:yyyyMMddHHmmss}.xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            using (StringWriter sw = new StringWriter())
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                HttpContext.Current.Response.Write("<html><head><style>.textmode { mso-number-format:'\\@'; }</style></head><body>");
                exportGrid.RenderControl(hw);
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.Write("</body></html>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        protected void ddlPageSizeCargaSuelta_SelectedIndexChanged(object sender, EventArgs e)
        {
            gdvCargaSuelta.PageSize = int.Parse(ddlPageSizeCargaSuelta.SelectedValue);
            gdvCargaSuelta_PageIndexChanging(gdvCargaSuelta, new GridViewPageEventArgs(0));

            var lista = Session["DatosContenedorCargaSuelta"] as List<CargaSueltaVista>;
            if (lista != null || lista.Count >0)
            {
                MostrarResumen(gdvCargaSuelta, lblResumenCargaSuelta, lista.Count);
            }
        }

        protected void GridView_SortingCargaSuelta(object sender, GridViewSortEventArgs e)
        {
            var data = Session["DatosContenedorCargaSuelta"] as List<CargaSueltaVista>;
            if (data != null || data.Count >0)
            {
                string sortExpression = e.SortExpression;
                string direction = GetSortDirection(sortExpression);

                var propInfo = typeof(CargaSueltaVista).GetProperty(sortExpression);
                if (propInfo != null)
                {
                    var sorted = direction == "ASC"
                        ? data.OrderBy(x => propInfo.GetValue(x, null)).ToList()
                        : data.OrderByDescending(x => propInfo.GetValue(x, null)).ToList();

                    gdvCargaSuelta.DataSource = sorted;
                    gdvCargaSuelta.DataBind();
                    MostrarResumen(gdvCargaSuelta, lblResumenCargaSuelta, sorted.Count);
                }
            }
        }

        private void ExportarGridToExcel(GridView originalGrid, string fileName)
        {
            var exportGrid = new GridView
            {
                AutoGenerateColumns = false,
                EnableViewState = false,
                HeaderStyle = { CssClass = "textmode" },
                RowStyle = { CssClass = "textmode" },
                CssClass = "table table-bordered"
            };

            if (originalGrid.ID == "gbContainer")
            {
                var lista = Session["DatosContenedorCntr"] as List<ContenedorVista>;
                exportGrid.Columns.Add(new BoundField { DataField = "CAS", HeaderText = "Vigencia De Cas" });
                exportGrid.Columns.Add(new BoundField { DataField = "CONTENEDOR", HeaderText = "Contenedor" });
                exportGrid.Columns.Add(new BoundField { DataField = "Category", HeaderText = "Categoría" });
                exportGrid.Columns.Add(new BoundField { DataField = "TIPO_STATE", HeaderText = "Estado" });
                exportGrid.Columns.Add(new BoundField { DataField = "LINE_OP", HeaderText = "Naviera" });
                exportGrid.Columns.Add(new BoundField { DataField = "IB_ACTUAL_VISIT", HeaderText = "Referencia Nave" });
                exportGrid.Columns.Add(new BoundField { DataField = "NDOCUMENTO", HeaderText = "N. Autorización de Aduana" });
                exportGrid.Columns.Add(new BoundField { DataField = "FRGHT_KIND", HeaderText = "Tipo De Carga" });

                exportGrid.DataSource = lista;
            }
            else if (originalGrid.ID == "gdvBooking")
            {
                var listaBooking = Session["DatosContenedorBooking"] as List<Cls_Container>;
                if (listaBooking != null)
                {
                    var resultadoFormateado = listaBooking.Select(r => new
                    {
                        CNTR_CONTAINER = "\t" + r.CNTR_CONTAINER,
                        CNTR_TYPE = r.CNTR_TYPE,
                        CNTR_YARD_STATUS = r.CNTR_YARD_STATUS,
                        CNTR_CLNT_CUSTOMER_LINE = r.CNTR_CLNT_CUSTOMER_LINE,
                        CNTR_BKNG_BOOKING = "\t" + r.CNTR_BKNG_BOOKING,
                        CNTR_AISV = "\t" + r.CNTR_AISV,
                        TemperaturaTexto = r.CNTR_TEMPERATURE == 0 ? "Sin temperatura" : r.CNTR_TEMPERATURE.ToString(),
                        WEIGHT = r.CNTR_GROSS_WEIGHT,
                        CNTR_LCL_FCL = r.CNTR_LCL_FCL
                    }).ToList();

                    exportGrid.DataSource = resultadoFormateado;
                }
            }

            exportGrid.DataBind();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", $"attachment;filename={fileName}_{DateTime.Now:yyyyMMddHHmmss}.xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            using (StringWriter sw = new StringWriter())
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                HttpContext.Current.Response.Write(style);
                exportGrid.RenderControl(hw);
                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Necesario para permitir RenderControl fuera del flujo normal.
        }
   

        protected void btnExportarGbContainerXML_Click(object sender, EventArgs e)
        {
            LogicXml.ExportarXML("Cntr");
        }
        protected void btnExportarGbBookingXML_Click(object sender, EventArgs e)
        {
            LogicXml.ExportarXML("Book");
        }
        protected void btnExportarGdvCargaSueltaXML_Click(object sender, EventArgs e)
        {
            LogicXml.ExportarXML("CargaSuelta");
        }

        protected void btnExportarGbContainerPDF_Click(object sender, EventArgs e)
        {
            LogicPdfs.ExportarPDF("Cntr");
        }
        protected void btnExportarGdvBookingPDF_Click(object sender, EventArgs e)
        {
            LogicPdfs.ExportarPDF("Book");
        }
     

        protected void btnExportarGdvCargaSueltaPDF_Click(object sender, EventArgs e)
        {
            LogicPdfs.ExportarPDF("CargaSuelta");
        }


        protected void btnGenerarProforma_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {



                CultureInfo enUS = new CultureInfo("en-US");
                //     this.myModal.Attributes["class"] = "nover";

                try
                {



                    //if (this.CboAsumeFactura.Items.Count == 0)
                    //{
                    //    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona que va asumir la carga</b>"));
                    //    this.TXTHSN.Focus();
                    //    return;
                    //}


                    HoraHasta = "00:00";


                    //valida que se seleccione la persona a facturar
                    //if (this.CboAsumeFactura.SelectedIndex == 0)
                    //{
                    //    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona que asumirá la factura.</b>"));
                    //    this.CboAsumeFactura.Focus();
                    //    return;
                    //}


                    //instancia sesion
                    objCabecera = Session["DatosContenedorCargaSuelta2"] as Cls_Bil_Cabecera;
                  

                    if (objCabecera.Detalle.Count == 0)
                    {
                        MostrarError(" Informativo! No existe detalle de carga suelta CFS, para poder generar la cotización");
                        return;

                    }

                    //valida que seleccione todos los contenedores para cotizar 
                    var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == false)
                                                select p.CONTENEDOR).ToList();

                    //if (LinqValidaContenedor.Count != 0)
                    //{
                    //    MostrarError(" Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..");
                    //    return;
                    //}
                    //valida que tenga todos tengan fecha de salida
                 

                    //listado de contenedores
                    var LinqListContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                              select p.CONTENEDOR).ToList();

                 //   objCabecera.GLOSA = this.Txtcomentario.Text.Trim();
                    Contenedores = string.Join(", ", LinqListContenedor);
                    objCabecera.HORA_HASTA = HoraHasta;

                    //numero de carga
                    Numero_Carga = objCabecera.NUMERO_CARGA;
                    objCabecera.CONTENEDORES = Contenedores;
                    objCabecera.FECHA_HASTA = FechaFactura;
                    LoginName = objCabecera.IV_USUARIO_CREA.Trim();
               //     objCabecera.ID_FACTURADO = "1720680196001";

                    //     var ExisteAsume = CboAsumeFactura.Items.FindByValue(objCabecera.ID_FACTURADO.Trim());
                    //     if (ExisteAsume != null)
                    //     {
                    //         objCabecera.DESC_FACTURADO = ExisteAsume.Text.Split('-').ToList()[1].Trim();
                    //   //      this.hf_idasume.Value = objCabecera.ID_FACTURADO;
                    // //        this.hf_descasume.Value = objCabecera.DESC_FACTURADO;
                    //     }
                    //     else
                    //     {
                    ////         this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo obtener la información de la persona que asumirá la factura.</b>"));
                    //         return;
                    //     }

                    /***********************************************************************************************************************************************
                    *datos del cliente N4 
                    **********************************************************************************************************************************************/
                    //var Cliente = N4.Entidades.Cliente.ObtenerCliente(ClsUsuario.loginname, objCabecera.ID_FACTURADO);
                    //if (Cliente.Exitoso)
                    //{
                    //    var ListaCliente = Cliente.Resultado;
                    //    if (ListaCliente != null)
                    //    {
                    //        Cliente_Ruc = ListaCliente.CLNT_CUSTOMER.Trim();
                    //        Cliente_Rol = ListaCliente.CLNT_ROLE.Trim();
                    //        Cliente_Direccion = ListaCliente.CLNT_ADRESS.Trim();
                    //        Cliente_Ciudad = ListaCliente.CLNT_EMAIL;
                    //    }
                    //    else
                    //    {
                    //        MostrarError(" Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..");
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    MostrarError(" Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..");
                    //    return;
                    //}
                    /***********************************************************************************************************************************************
                    *fin: datos del cliente N4 
                    **********************************************************************************************************************************************/

                    //actualizo el objeto temporal

                    Fila = 1;
                    Decimal Subtotal = 0;
                    Decimal Iva = 0;
                    Decimal Total = 0;
                    /***********************************************************************************************************************************************
                    *2) proceso para grabar proforma
                    **********************************************************************************************************************************************/
                    objCabecera = Session["DatosContenedorCargaSuelta2"] as Cls_Bil_Cabecera;
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

                    string cip = Cls_Bil_IP.GetLocalIPAddress();
                     cip = Request.UserHostAddress;
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


                    /***********************************************************************************************************************************************
                    *4) Consulta Servicios a proformar N4 - por cada grupo de fechas
                    **********************************************************************************************************************************************/
                    var Validacion = new Aduana.Importacion.ecu_validacion_cntr_cfs();
                    var Contenedor = new N4.Importacion.container_cfs();
                    var Billing = new N4Ws.Entidad.billing();
                    var Ws = new N4Ws.Entidad.InvoiceRequest();

                    /*saco el invoice type*/
                    string pInvoiceType = string.Empty;
                    var InvoiceType = InvoiceTypeConfig.ObtenerInvoicetypes();
                    if (InvoiceType.Exitoso)
                    {
                        var LinqInvoiceType = (from p in InvoiceType.Resultado.Where(X => X.codigo.Equals("IMPOCFS"))
                                               select new { valor = p.valor }).FirstOrDefault();

                        pInvoiceType = LinqInvoiceType.valor == null ? "2DA_MAN_CFS_IMPO" : LinqInvoiceType.valor;
                    }
                    /*fin invoice type*/
                    /*datos de contecon*/
                    var ruc_contecon = System.Configuration.ConfigurationManager.AppSettings["ruc_cgsa"];
                    var rol_contecon = System.Configuration.ConfigurationManager.AppSettings["rol_contecon"];

                    string pRuc = string.Empty;
                    string pRol = string.Empty;
                    if (string.IsNullOrEmpty(ruc_contecon))
                    {
                        pRuc = Cliente_Ruc;
                        pRol = Cliente_Rol;
                    }
                    else
                    {
                        pRuc = ruc_contecon.ToString().Trim();
                        pRol = rol_contecon.ToString().Trim();
                    }

                    Ws.action = N4Ws.Entidad.Action.INQUIRE;
                    Ws.requester = N4Ws.Entidad.Requester.QUERY_CHARGES;

                    Ws.InvoiceTypeId = pInvoiceType;
                    Ws.payeeCustomerId = Cliente_Ruc;
                    Ws.payeeCustomerBizRole = Cliente_Rol;

                    var Direccion = new N4Ws.Entidad.address();
                    Direccion.addressLine1 = string.Empty;
                    Direccion.city = "GUAYAQUIL";

                    var Parametro = new N4Ws.Entidad.invoiceParameter();
                    Parametro.bexuPaidThruDay = FechaFactura.ToString("yyyy-MM-dd HH:mm");
                    Parametro.bexuBlNbr = Numero_Carga;
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

                    Session["DatosContenedorCargaSuelta2"] = objCabecera;

                    /***********************************************************************************************************************************************
                    *graba cotizacion en base de billion, para pasara la siguiente ventana
                    **********************************************************************************************************************************************/

                    if (objProforma == null)
                    {
                        MostrarError(" Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..");
                        return;
                    }
                    else
                    {
                        //si no existen servicios a cotizar 
                        if (objProforma.DetalleServicios.Count == 0)
                        {
                            MostrarError(" Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..");
                            return;
                        }

                        var nIdRegistro = objProforma.SaveTransaction(out cMensajes);
                        if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                        {
                            MostrarError(" Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..");
                            return;
                        }

                         // this.Ocultar_Mensaje();
                        string cId = securetext(nIdRegistro.Value.ToString());
                        Response.Redirect("~/cargacfs/cotizacioncfs_preview.aspx?id_proforma=" + cId.Trim() + "", false);

                    }

                }
                catch (Exception ex)
                {
                      MostrarError(" Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..");

                }

            }
            
        }


        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

    }
}


#endregion