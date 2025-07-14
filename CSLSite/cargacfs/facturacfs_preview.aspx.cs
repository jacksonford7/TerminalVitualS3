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
using Salesforces;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSLSite
{
    public partial class facturacfs_preview : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Proforma_Consulta objProforma = new Cls_Bil_Proforma_Consulta();

        private Cls_Bil_Invoice_Cabecera objFactura = new Cls_Bil_Invoice_Cabecera();
        private Cls_Bil_Invoice_Detalle objDetalleFactura = new Cls_Bil_Invoice_Detalle();
        private Cls_Bil_Invoice_Servicios objServiciosFactura = new Cls_Bil_Invoice_Servicios();
        private Cls_Bil_Invoice_Actualiza_Pase objActualiza_Pase = new Cls_Bil_Invoice_Actualiza_Pase();

        private Cls_Bil_Invoice_Procesar objProcesar = new Cls_Bil_Invoice_Procesar();

        #endregion

        #region "Variables"
        private Int64 id_factura = 0;
        private string c_id_factura = string.Empty;
        private string c_Subtotal = string.Empty;
        private string c_Iva = string.Empty;
        private string c_Total = string.Empty;
        private int n_fila = 1;
        string cMensaje = string.Empty;
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string cMensajes;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;
        private int Fila = 1;
        private string TipoServicio = string.Empty;
        private string draftNumber = string.Empty;
        private Int64 draftNumberFinal = 0;
        private string NumeroFactura = string.Empty;
        private string Contenedores = string.Empty;
        private bool SinServicios = false;
        private string MensajeCasos = string.Empty;
        private static Int64? lm = -3;
        private string OError;
        #endregion

        #region "Propiedades"
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

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
        }

        private void OcultarLoading()
        {
          
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
            UPDETALLE.Update();
        }

        private void MostrarLoader()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mostrarloader();", true);
            UPDETALLE.Update();
        }

        private void Mostrar_Mensaje(string Mensaje)
        {

            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading();
            this.Actualiza_Paneles();
        }

        private void Mostrar_Mensaje2(string Mensaje)
        {

            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            MostrarLoader(); 
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            OcultarLoading();
            this.Actualiza_Paneles();
           

        }

        private void Poblar_Servicios()
        {
            StringBuilder tab = new StringBuilder();
            objFactura = Session["InvoiceCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Invoice_Cabecera;
            if (objFactura == null)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para facturar"));
                return;
            }
            else
            {
                //grabar traza de factura
                var nIdRegistro_Traza = objFactura.SaveTransaction_Traza_cfs(out cMensajes);
                if (!nIdRegistro_Traza.HasValue || nIdRegistro_Traza.Value <= 0)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo grabar traza de la factura de carga suelta..{0}</b>", cMensajes));
                }
                
                CultureInfo enUS = new CultureInfo("en-US");

                //si existen servicios
                if (objFactura.DetalleServicios.Count != 0)
                {

                    this.BtnFacturar.Attributes["class"] = "btn btn-primary";

                    agente.InnerText = String.Format("AGENTE ADUANERO: [{0}] - {1}", objFactura.IV_ID_AGENTE, objFactura.IV_DESC_AGENTE);
                    cliente.InnerText = String.Format("CONSIGNATARIO: [{0}] - {1}", objFactura.IV_ID_CLIENTE, objFactura.IV_DESC_CLIENTE);
                    facturado.InnerText = String.Format("FACTURADO A: [{0}] - {1}", objFactura.IV_ID_FACTURADO, objFactura.IV_DESC_FACTURADO);
                    observacion.InnerText = String.Format("OBSERVACIONES: {0}", objFactura.IV_GLOSA);
                    carga.InnerText = String.Format("NUMERO DE CARGA: {0}", objFactura.IV_NUMERO_CARGA);
                  

                    fecha.InnerText = String.Format("{0}", objFactura.IV_FECHA_CREA.HasValue ? objFactura.IV_FECHA_CREA.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                    fecha_hasta.InnerText = String.Format("{0}", objFactura.IV_FECHA_HASTA.HasValue ? objFactura.IV_FECHA_HASTA.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                    fecha_hasta.Visible = false;

                    c_Subtotal = objFactura.IV_SUBTOTAL == 0 ? "..." : string.Format("{0:c}", objFactura.IV_SUBTOTAL);
                    c_Iva = objFactura.IV_IVA == 0 ? "..." : string.Format("{0:c}", objFactura.IV_IVA);
                    c_Total = objFactura.IV_TOTAL == 0 ? "..." : string.Format("{0:c}", objFactura.IV_TOTAL);
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


                    var LinqServiciosAgrupados = (from p in objFactura.DetalleServicios.AsEnumerable()
                                                  group p by new { CODIGO = p.IV_ID_SERVICIO, SERVICIO = p.IV_DESC_SERVICIO, PRECIO = p.IV_PRECIO } into Grupo
                                                  select new
                                                  {
                                                      SUBTOTAL = Grupo.Sum(g => Decimal.Round(g.IV_SUBTOTAL, 2)),
                                                      SERVICIO = Grupo.Key.SERVICIO.ToString().Trim(),
                                                      CODIGO = Grupo.Key.CODIGO.ToString().Trim(),
                                                      CANTIDAD = Grupo.Sum(g => Decimal.Round(g.IV_CANTIDAD, 2)),
                                                      PRECIO = Decimal.Round(Grupo.Key.PRECIO, 2),
                                                      IVA = Grupo.Sum(g => Decimal.Round(g.IV_IVA, 2))
                                                  }).ToList();


                    foreach (var pf in LinqServiciosAgrupados)
                    {

                        tab.AppendFormat("<tr>" +
                            "<td class='text-center'>{0}</td>" +
                            "<td>{1}</td>" +
                            "<td class='text-center'>{2}</td>" +
                            "<td class='text-right'>{3}</td>" +
                            "<td class='text-right'>{4}</td>" +
                            "</tr>",
                            String.IsNullOrEmpty(pf.CODIGO) ? "..." : pf.CODIGO,
                            String.IsNullOrEmpty(pf.SERVICIO) ? "..." : pf.SERVICIO,
                            pf.CANTIDAD == 0 ? "..." : string.Format("{0:N2}", pf.CANTIDAD),
                            pf.PRECIO == 0 ? "..." : string.Format("{0:c}", pf.PRECIO),
                            pf.SUBTOTAL == 0 ? "..." : string.Format("{0:c}", pf.SUBTOTAL)
                            );


                        n_fila++;
                    }
                }
                else
                {
                    agente.InnerText = string.Empty;
                    cliente.InnerText = string.Empty;
                    facturado.InnerText = string.Empty;
                    observacion.InnerText = string.Empty;
                    carga.InnerText = string.Empty;
                  
                    fecha.InnerText = string.Empty;
                    fecha_hasta.InnerText = string.Empty;

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
                    total.InnerText = string.Empty;


                    this.BtnFacturar.Attributes["disabled"] = "disabled";
                }

                tab.Append("<tr><td colspan = '3' rowspan = '4'>" +
                      "<h4>Términos y condiciones</h4>" +
                      "<p>Este documento no tiene validez legal alguna. <br/>Debo y pagaré incondicionalmente a la orden de Contecon Guayaquil S.A. en el lugar y fecha que se me reconvenga, " + "" +
                      " el valor  total expresado  en este documento, más los  impuestos legales respectivos en Dólares de los Estados Unidos de América, por los bienes y/o servicios que he recibido a mi entera satisfacción.</p>" +
                      "<td class='text-right'><strong>Subtotal</strong></td>" +
                      "<td class='text-right'><strong>" + c_Subtotal + "</strong></td></td></tr>");

                tab.Append("<tr><td class='text-right no-border'><strong>Iva 15%</strong></td>" +
                       "<td class='text-right'><strong>" + c_Iva + "</strong></td></tr>");

                tab.Append("<tr><td class='text-right no-border'>" +
                       "<div class='well well-small rojo'><strong>Total</strong></div>" +
                       "</td>" +
                       "<td class='text-right'><strong>" + c_Total + "</strong></td></tr>");

                tab.Append("<tbody>");
                tab.Append("</table>");

                this.detalle.InnerHtml = tab.ToString();

                this.Actualiza_Paneles();
            }
        }




        private void Enviar_Caso_Salesforce(string pUsuario, string pruc, string pModulo, string pNovedad, string pErrores, out string Mensaje)
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

                detalle_carga.Detalles.Add(new DetalleCarga("Errores:", pErrores));


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

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }


       

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);
               
                return;
            }

            this.banmsg.Visible = IsPostBack;
            this.banmsg_det.Visible = IsPostBack;

        
            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
                this.banmsg_det.InnerText = string.Empty;
            }

            try
            {

                c_id_factura = QuerySegura.DecryptQueryString(Request.QueryString["id"]);
                if (Request.QueryString["id"] == null || string.IsNullOrEmpty(c_id_factura))
                {
                    this.Mostrar_Mensaje( string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para facturar carga suelta"));
                    Response.Redirect("~/facturacioncfs.aspx", false);
                    return;
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje( string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...{0}", ex.Message));
      
            }

            

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                if (!IsPostBack)
                {
                    try
                    {
                       
                        ClsUsuario = Page.Tracker();
                        if (ClsUsuario == null)
                        {
                            return;
                        }

                        c_id_factura = c_id_factura.Trim().Replace("\0", string.Empty);

                        if (!Int64.TryParse(c_id_factura, out id_factura))
                        {
                            //campo no es numerico
                            this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error al convertir {0}", c_id_factura));
                            return;
                        }
                        this.hf_BrowserWindowName.Value = id_factura.ToString();

                        this.Poblar_Servicios();
                        this.BtnVisualizar.Attributes["disabled"] = "disabled";
                    }
                    catch (Exception ex)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...{0}", ex.Message));
                    }
                }

            }
        }

     



        protected void BtnVisualizar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                objFactura = Session["InvoiceCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Invoice_Cabecera;
                if (objFactura == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Aún no ha generado la factura de carga suelta, para poder imprimirla"));
                    return;
                }

                if (string.IsNullOrEmpty(this.hf_idfacturagenerada.Value))
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Aún no ha generado la factura de carga suelta, para poder imprimirla"));
                    return;
                }

                //IMPRIMIR FACTURA -FORMATO HTML
                string cId = securetext(this.hf_idfacturagenerada.Value);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('../reportes/factura_preview.aspx?id_comprobante=" + cId + "');", true);

            }


        }

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {

          
            this.Mostrar_Mensaje(string.Format("<b>GENERANDO FACTURA CARGA SUELTA! </b>....POR FAVOR ESPERE, SE ESTA GENERANDO LA FACTURA......."));

            if (Response.IsClientConnected)
            {

                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    return;
                }

                CultureInfo enUS = new CultureInfo("en-US");

                try
                {
                    this.BtnFacturar.Attributes["disabled"] = "disabled";
                    this.Actualiza_Paneles();

                   

                    List<String> Lista = new List<String>();
                    Decimal Subtotal = 0;
                    Decimal Iva = 0;
                    Decimal Total = 0;
                    string pInvoiceType = string.Empty;

                    objFactura = Session["InvoiceCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Invoice_Cabecera;
                    if (objFactura == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para facturar carga suelta"));
                        return;
                    }
                    else
                    {

                        if (objFactura.DetalleServicios.Count == 0)
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe servicios pendientes para facturar carga suelta: {0}", objFactura.IV_NUMERO_CARGA));
                            return;
                        }



                        /***********************************************************************************************************************************************
                        *datos del cliente N4 
                        **********************************************************************************************************************************************/
                        var Cliente = N4.Entidades.Cliente.ObtenerCliente(objFactura.IV_USUARIO_CREA.Trim(), objFactura.IV_ID_FACTURADO);
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
                                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos de cliente para facturar carga suelta: {0}", objFactura.IV_NUMERO_CARGA));
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos de cliente para facturar carga suelta: {0}", objFactura.IV_NUMERO_CARGA));
                            return;
                        }

                        /*saco el invoice type*/
                        var InvoiceType = InvoiceTypeConfig.ObtenerInvoicetypes();
                        if (InvoiceType.Exitoso)
                        {
                            var LinqInvoiceType = (from p in InvoiceType.Resultado.Where(X => X.codigo.Equals("IMPOCFS"))
                                                   select new { valor = p.valor }).FirstOrDefault();

                            pInvoiceType = LinqInvoiceType.valor == null ? "2DA_MAN_CFS_IMPO" : LinqInvoiceType.valor;
                        }
                        /*fin invoice type*/

                        /***********************************************************************************************************************************************
                        *proceso facturacion
                        **********************************************************************************************************************************************/
                        var FechaContainer_Filtro = (from p in objFactura.Detalle.Where(x => x.IV_FECHA_HASTA.HasValue)
                                                     select new { FECHA_HASTA = p.IV_FECHA_HASTA.Value }).FirstOrDefault();

                        Fila = 1;
                        
                        objFactura.DetalleServicios.Clear(); 

                        var LinqDetcontainer = (from p in objFactura.Detalle.Where(x => x.IV_FECHA_HASTA.HasValue)
                                                select p.IV_CONTENEDOR).ToList();

                        Contenedores = string.Join(",", LinqDetcontainer);//listado contenedores

                        /***********************************************************************************************************************************************
                        *Servicios a facturar N4 
                        **********************************************************************************************************************************************/
                        var Validacion = new Aduana.Importacion.ecu_validacion_cntr_cfs();
                        var Contenedor = new N4.Importacion.container_cfs();
                        var Billing = new N4Ws.Entidad.billing();

                        var Ws = new N4Ws.Entidad.InvoiceRequest();

                        Ws.action = N4Ws.Entidad.Action.DRAFT;
                        Ws.requester = N4Ws.Entidad.Requester.QUERY_CHARGES;
                        Ws.InvoiceTypeId = pInvoiceType;
                        Ws.payeeCustomerId = objFactura.IV_ID_FACTURADO;
                        Ws.payeeCustomerBizRole = Cliente_Rol;

                        var Direccion = new N4Ws.Entidad.address();
                        Direccion.addressLine1 = string.Empty;
                        Direccion.city = "GUAYAQUIL";

                        var Parametro = new N4Ws.Entidad.invoiceParameter();
                        Parametro.bexuPaidThruDay = FechaContainer_Filtro.FECHA_HASTA.ToString("yyyy-MM-dd HH:mm");
                        Parametro.bexuBlNbr = objFactura.IV_NUMERO_CARGA;

                        Ws.invoiceParameters.Add(Parametro);
                        Ws.billToParty.Add(Direccion);
                        Ws.bexuBlNbr = objFactura.IV_NUMERO_CARGA;
                        Billing.Request = Ws;

                        //resultado query billing, de una consulta especifica
                        var Resultado = Servicios.N4ServicioBasico(Billing, objFactura.IV_USUARIO_CREA.Trim());
                        if (Resultado != null)
                        {
                            //servicios ok
                            /***************************************************************************************************************************************/
                            /*proceso para descomponer servicios*/
                            if (Resultado.status_id.Equals("OK"))
                            {
                                var xBilling = Resultado;

                                FechaPaidThruDay = null;
                                CargabexuBlNbr = null;


                                draftNumber = xBilling.response.billInvoice.draftNumber;

                                if (!Int64.TryParse(draftNumber, out draftNumberFinal))
                                {

                                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>No se puede convertir en campo numérico el draft # : {0}", draftNumber));
                                    return;
                                }
                                //agrego lista para finalizar draft
                                Lista.Add(draftNumber.Trim());

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
                                    objServiciosFactura.IV_USUARIO_CREA = objFactura.IV_USUARIO_CREA.Trim();
                                    objServiciosFactura.IV_FECHA_CREA = DateTime.Now;
                                    objServiciosFactura.IV_DRAFT = draftNumber;
                                    Fila++;
                                    objFactura.DetalleServicios.Add(objServiciosFactura);

                                }

                                Iva = Iva + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTaxes != null ? xBilling.response.billInvoice.totalTaxes : "0", enUS), 2);
                                Total = Total + Decimal.Round(Decimal.Parse(xBilling.response.billInvoice.totalTotal != null ? xBilling.response.billInvoice.totalTotal : "0", enUS), 2);



                            }//fin ok

                        }//fin resultado

                        Session["InvoiceCFS" + this.hf_BrowserWindowName.Value] = objFactura;
                        //recupero
                        objFactura = Session["InvoiceCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Invoice_Cabecera;
                        if (objFactura == null)
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para facturar carga suelta.."));
                            return;
                        }

                        System.Xml.Linq.XDocument XMLContenedor = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new System.Xml.Linq.XElement("FACT_DETALLE", from p in objFactura.Detalle.AsEnumerable().AsParallel()
                                                                        select new System.Xml.Linq.XElement("DETALLE",
                                                                        new System.Xml.Linq.XAttribute("IV_MRN", p.IV_MRN == null ? "-" : p.IV_MRN.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_MSN", p.IV_MSN == null ? "-" : p.IV_MSN.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_HSN", p.IV_HSN == null ? "-" : p.IV_HSN.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_CONTENEDOR", p.IV_CONTENEDOR == null ? "-" : p.IV_CONTENEDOR.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_GKEY", p.IV_GKEY == 0 ? 0 : p.IV_GKEY),
                                                                        new System.Xml.Linq.XAttribute("IV_REFERENCIA", p.IV_REFERENCIA == null ? "" : p.IV_REFERENCIA.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_TRAFICO", p.IV_TRAFICO == null ? "" : p.IV_TRAFICO.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_TAMANO", p.IV_TAMANO == null ? "" : p.IV_TAMANO.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_TIPO", p.IV_TIPO == null ? "" : p.IV_TIPO.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_CAS", p.IV_CAS == null ? DateTime.Parse("1900/01/01") : p.IV_CAS),
                                                                        new System.Xml.Linq.XAttribute("IV_BOOKING", p.IV_BOOKING == null ? "" : p.IV_BOOKING.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_IMDT", p.IV_IMDT == null ? "" : p.IV_IMDT.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_BLOQUEO", p.IV_BLOQUEO),
                                                                        new System.Xml.Linq.XAttribute("IV_FECHA_ULTIMA", p.IV_FECHA_ULTIMA == null ? DateTime.Parse("1900/01/01") : p.IV_FECHA_ULTIMA),
                                                                        new System.Xml.Linq.XAttribute("IV_IN_OUT", p.IV_IN_OUT == null ? "" : p.IV_IN_OUT.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_FULL_VACIO", p.IV_FULL_VACIO == null ? "" : p.IV_FULL_VACIO.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_AISV", p.IV_AISV == null ? "" : p.IV_AISV.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_REEFER", p.IV_REEFER == null ? "" : p.IV_REEFER.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_DOCUMENTO", p.IV_DOCUMENTO == null ? "" : p.IV_DOCUMENTO.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_FECHA_HASTA", p.IV_FECHA_HASTA == null ? DateTime.Parse("1900/01/01") : p.IV_FECHA_HASTA),
                                                                        new System.Xml.Linq.XAttribute("IV_USUARIO_CREA", p.IV_USUARIO_CREA),
                                                                        new System.Xml.Linq.XAttribute("IV_CNTR_DD", p.IV_CNTR_DD),
                                                                        new System.Xml.Linq.XAttribute("IV_CANTIDAD", p.IV_CANTIDAD),
                                                                        new System.Xml.Linq.XAttribute("IV_PESO", p.IV_PESO),
                                                                        new System.Xml.Linq.XAttribute("IV_OPERACION", p.IV_OPERACION == null ? "" : p.IV_OPERACION.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_DESCRIPCION", p.IV_DESCRIPCION == null ? "" : p.IV_DESCRIPCION.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_EXPORTADOR", p.IV_EXPORTADOR == null ? "" : p.IV_EXPORTADOR.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("IV_AGENCIA", p.IV_AGENCIA == null ? "" : p.IV_AGENCIA.ToString().Trim()),
                                                                        new System.Xml.Linq.XAttribute("ID_UNIDAD", p.ID_UNIDAD == null ? 0 : p.ID_UNIDAD),
                                                                         new System.Xml.Linq.XAttribute("IV_CERTIFICADO", (p.IV_TIENE_CERTIFICADO.Equals("SI") && !p.IV_CERTIFICADO ? "1" : p.IV_CERTIFICADO.ToString())),
                                                                        new System.Xml.Linq.XAttribute("flag", "I"))));


                        System.Xml.Linq.XDocument XMLServicios = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                new System.Xml.Linq.XElement("FACT_SERVICIOS", from p in objFactura.DetalleServicios.AsEnumerable().AsParallel()
                                                                                select new System.Xml.Linq.XElement("DETALLE",
                                                                                new System.Xml.Linq.XAttribute("IV_ID_SERVICIO", p.IV_ID_SERVICIO == null ? "" : p.IV_ID_SERVICIO.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("IV_DESC_SERVICIO", p.IV_DESC_SERVICIO == null ? "" : p.IV_DESC_SERVICIO.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("IV_CARGA", p.IV_CARGA == null ? "" : p.IV_CARGA.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("IV_FECHA", p.IV_FECHA == null ? DateTime.Parse("1900/01/01") : p.IV_FECHA),
                                                                                new System.Xml.Linq.XAttribute("IV_TIPO_SERVICIO", p.IV_TIPO_SERVICIO),
                                                                                new System.Xml.Linq.XAttribute("IV_CANTIDAD", p.IV_CANTIDAD),
                                                                                new System.Xml.Linq.XAttribute("IV_PRECIO", p.IV_PRECIO),
                                                                                new System.Xml.Linq.XAttribute("IV_SUBTOTAL", p.IV_SUBTOTAL),
                                                                                new System.Xml.Linq.XAttribute("IV_IVA", p.IV_IVA),
                                                                                new System.Xml.Linq.XAttribute("IV_DRAFT", p.IV_DRAFT == null ? "" : p.IV_DRAFT.ToString().Trim()),
                                                                                new System.Xml.Linq.XAttribute("IV_USUARIO_CREA", p.IV_USUARIO_CREA),
                                                                                new System.Xml.Linq.XAttribute("flag", "I"))));
                        //si no existen servicios a cotizar
                        if (objFactura.DetalleServicios.Count == 0)
                        {
                            SinServicios = true;
                        }


                        cMensajes = string.Empty;
                        string cMensajeActualizados = string.Empty;
                        int nRecorrido = 1;
                        if (SinServicios)
                        {
                            foreach (var Det in objFactura.Detalle.Where(p => p.IV_FECHA_HASTA.HasValue))
                            {
                                if (nRecorrido == 1)
                                {
                                    //si la fecha de salida es menor a la fecha tope con los dias libres incluidos, y si no es la primera facturacion
                                    if ((Det.IV_FECHA_HASTA.Value <= Det.IV_FECHA_TOPE_DLIBRE.Value) && Det.IV_FECHA_ULTIMA.HasValue)
                                    {
                                        /*grabar transaccion de factura*/
                                        objActualiza_Pase = new Cls_Bil_Invoice_Actualiza_Pase();
                                        objActualiza_Pase.IV_ID = Det.IV_ID;
                                        objActualiza_Pase.IV_MRN = Det.IV_MRN;
                                        objActualiza_Pase.IV_MSN = Det.IV_MSN;
                                        objActualiza_Pase.IV_HSN = Det.IV_HSN;
                                        objActualiza_Pase.IV_FECHA_ULTIMA = Det.IV_FECHA_ULTIMA;
                                        objActualiza_Pase.IV_FECHA_HASTA = Det.IV_FECHA_HASTA;
                                        objActualiza_Pase.IV_MODULO = Det.IV_MODULO;
                                        objActualiza_Pase.IV_USUARIO_CREA = objFactura.IV_USUARIO_CREA.Trim();

                                        var nIdRegistro = objActualiza_Pase.SaveTransaction_Update_cfs(out cMensajes);
                                        if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                                        {

                                            this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo actualizar datos días libres, para la carga suelta {0}, {1}</b>", objFactura.IV_NUMERO_CARGA, cMensajes));
                                            return;
                                        }
                                        else
                                        {
                                            cMensajeActualizados = cMensajeActualizados + string.Format("Se procedió con la actualización de la fecha de salida de la carga {0}, el mismo cuenta con días libres y podrá generar pases de puerta hasta: {1} <br/>", objFactura.IV_NUMERO_CARGA, Det.IV_FECHA_HASTA.Value.ToString("dd/MM/yyyy"));
                                        }
                                    }
                                }
                                   
                                nRecorrido++;

                            }
                            
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen servicios pendientes para facturar carga suelta.</br> {0} ", cMensajeActualizados));
                            return;
                            
                        }
                            
                        
                        var LinqSubtotal = (from Servicios in objFactura.DetalleServicios.AsEnumerable()
                                            select Servicios.IV_SUBTOTAL
                                                ).Sum();
                        Subtotal = LinqSubtotal;

                        objFactura.IV_SUBTOTAL = Subtotal;
                        objFactura.IV_IVA = Iva;
                        objFactura.IV_TOTAL = Total;
                        objFactura.IV_DRAFT = draftNumber;

                        //revisar validacion
                        /**********************************************************************************************************************************
                        * valida si existe factura
                        * 27-04-2020
                        * *********************************************************************************************************************************/                          
                        List<Cls_Bil_Configuraciones> ValidaFactura = Cls_Bil_Configuraciones.Get_Validacion("VALIDA_FACTURA", out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {
                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en configuraciones.</br> {0} ", cMensajes));
                            return;

                        }
                        else
                        {
                            bool Valida_Factura = false;
                            if (ValidaFactura.Count != 0)
                            {
                                Valida_Factura = true;
                            }

                            if (Valida_Factura)
                            {
                                XDocument XMLContenedores = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                new XElement("CONTENEDORES", from p in objFactura.Detalle.AsEnumerable().AsParallel()
                                                                select new XElement("CONTENEDOR",
                                                        new XAttribute("IV_CONTENEDOR", p.IV_CONTENEDOR == null ? "" : p.IV_CONTENEDOR.ToString()))));

                                /*ultima factura*/
                                List<Cls_Bil_Invoice_Duplicados> Existe_Invoice = Cls_Bil_Invoice_Duplicados.Existe_Factura(objFactura.IV_ID_FACTURADO, XMLContenedores.ToString(), out cMensajes);
                                if (String.IsNullOrEmpty(cMensajes))
                                {
                                    string AuxNumeroFactura = string.Empty;
                                    decimal AuxSubtotalFactura = 0;
                                    string AuxFacturaFinal = string.Empty;

                                    foreach (var Det in Existe_Invoice)
                                    {
                                        AuxNumeroFactura = string.Format("00{0}",Det.INV_NUMERO_FACTURA.Trim());
                                        string AuxEstablecimiento = AuxNumeroFactura.Substring(0, 3);
                                        string AuxPuntoEmision = AuxNumeroFactura.Substring(3, 3);
                                        string AuxOriginal = AuxNumeroFactura.Substring(6, 9);
                                            AuxFacturaFinal = string.Format("{0}-{1}-{2}", AuxEstablecimiento, AuxPuntoEmision, AuxOriginal);
                                            AuxSubtotalFactura = Det.INV_TOTAL;

                                        if (AuxSubtotalFactura == objFactura.IV_SUBTOTAL)
                                        {
                                            this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! No se puede generar la factura, Ya existe una procesada: {0} - para el número de carga: {1}...para el cliente: {2}, con el monto:$ {3}</br> {0} ", AuxFacturaFinal, objFactura.IV_NUMERO_CARGA, objFactura.IV_DESC_FACTURADO, objFactura.IV_TOTAL));
                                            return;

                                        }

                                    }

                                        
                                }

                            }
                        }
                            
                        /**********************************************************************************************************************************/
                        /*proceso finalizar draft de factura*/
                        var BillingFin = new N4Ws.Entidad.billing();
                        MergeInvoiceRequest Fin = new MergeInvoiceRequest();
                        Fin.finalizeDate = DateTime.Now.Date.ToString("yyyy-MM-dd HH:mm");// FechaPaidThruDay;
                        Fin.drftInvoiceNbrs = Lista;
                        Fin.invoiceTypeId = pInvoiceType;

                        BillingFin.MergeInvoiceRequest = Fin;

                        var Finalizar = Servicios.N4ServicioBasicoMergeAndFinalizeTransaction(BillingFin, objFactura.IV_USUARIO_CREA.Trim());
                        if (Finalizar != null)
                        {
                            if (Finalizar.status_id.Equals("OK"))
                            {
                                var Factura = Finalizar;
                                NumeroFactura = Factura.response.billInvoice.finalNumber;

                                //draftNumber
                                objFactura.IV_FACTURA = NumeroFactura;
                                objFactura.IV_IVA = Decimal.Round(Decimal.Parse(Factura.response.billInvoice.totalTaxes != null ? Factura.response.billInvoice.totalTaxes : "0", enUS), 2); ;
                                objFactura.IV_TOTAL = Decimal.Round(Decimal.Parse(Factura.response.billInvoice.totalTotal != null ? Factura.response.billInvoice.totalTotal : "0", enUS), 2);
                                objFactura.IV_SUBTOTAL = Decimal.Round(Decimal.Parse(Factura.response.billInvoice.totalCharges != null ? Factura.response.billInvoice.totalCharges : "0", enUS), 2);

                                NumeroFactura = "00" + NumeroFactura;
                                string Establecimiento = NumeroFactura.Substring(0, 3);
                                string PuntoEmision = NumeroFactura.Substring(3, 3);
                                string Original = NumeroFactura.Substring(6, 9);
                                string FacturaFinal = string.Format("{0}-{1}-{2}", Establecimiento, PuntoEmision, Original);


                                /*nuevo proceso de grabado*/
                                System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                new System.Xml.Linq.XElement("FACT_CABECERA",
                                                                        new System.Xml.Linq.XElement("CABECERA",
                                                                        new System.Xml.Linq.XAttribute("IV_FECHA", objFactura.IV_FECHA == null ? DateTime.Parse("1900/01/01") : objFactura.IV_FECHA),
                                                                        new System.Xml.Linq.XAttribute("IV_TIPO_CARGA", objFactura.IV_TIPO_CARGA == null ? "" : objFactura.IV_TIPO_CARGA),
                                                                        new System.Xml.Linq.XAttribute("IV_ID_AGENTE", objFactura.IV_ID_AGENTE == null ? "" : objFactura.IV_ID_AGENTE),
                                                                        new System.Xml.Linq.XAttribute("IV_DESC_AGENTE", objFactura.IV_DESC_AGENTE == null ? "" : objFactura.IV_DESC_AGENTE),
                                                                        new System.Xml.Linq.XAttribute("IV_ID_CLIENTE", objFactura.IV_ID_CLIENTE == null ? "" : objFactura.IV_ID_CLIENTE),
                                                                        new System.Xml.Linq.XAttribute("IV_DESC_CLIENTE", objFactura.IV_DESC_CLIENTE == null ? "" : objFactura.IV_DESC_CLIENTE),
                                                                        new System.Xml.Linq.XAttribute("IV_ID_FACTURADO", objFactura.IV_ID_FACTURADO == null ? "" : objFactura.IV_ID_FACTURADO),
                                                                        new System.Xml.Linq.XAttribute("IV_DESC_FACTURADO", objFactura.IV_DESC_FACTURADO == null ? "" : objFactura.IV_DESC_FACTURADO),
                                                                        new System.Xml.Linq.XAttribute("IV_SUBTOTAL", objFactura.IV_SUBTOTAL),
                                                                        new System.Xml.Linq.XAttribute("IV_IVA", objFactura.IV_IVA),
                                                                        new System.Xml.Linq.XAttribute("IV_TOTAL", objFactura.IV_TOTAL),
                                                                        new System.Xml.Linq.XAttribute("IV_FACTURA", objFactura.IV_FACTURA == null ? "" : objFactura.IV_FACTURA),
                                                                        new System.Xml.Linq.XAttribute("IV_NUMERO_CARGA", objFactura.IV_NUMERO_CARGA == null ? "" : objFactura.IV_NUMERO_CARGA),
                                                                        new System.Xml.Linq.XAttribute("IV_FECHA_HASTA", objFactura.IV_FECHA_HASTA == null ? DateTime.Parse("1900/01/01") : objFactura.IV_FECHA_HASTA),
                                                                        new System.Xml.Linq.XAttribute("IV_BL", objFactura.IV_BL == null ? "" : objFactura.IV_BL),
                                                                        new System.Xml.Linq.XAttribute("IV_BUQUE", objFactura.IV_BUQUE == null ? "" : objFactura.IV_BUQUE),
                                                                        new System.Xml.Linq.XAttribute("IV_VIAJE", objFactura.IV_VIAJE == null ? "" : objFactura.IV_VIAJE),
                                                                        new System.Xml.Linq.XAttribute("IV_FECHA_ARRIBO", objFactura.IV_FECHA_ARRIBO == null ? "" : objFactura.IV_FECHA_ARRIBO),
                                                                        new System.Xml.Linq.XAttribute("IV_DIR_FACTURADO", objFactura.IV_DIR_FACTURADO == null ? "" : objFactura.IV_DIR_FACTURADO),
                                                                        new System.Xml.Linq.XAttribute("IV_EMAIL_FACTURADO", objFactura.IV_EMAIL_FACTURADO == null ? "" : objFactura.IV_EMAIL_FACTURADO),
                                                                        new System.Xml.Linq.XAttribute("IV_CIUDAD_FACTURADO", objFactura.IV_CIUDAD_FACTURADO == null ? "" : objFactura.IV_CIUDAD_FACTURADO),
                                                                        new System.Xml.Linq.XAttribute("IV_DIAS_CREDITO", objFactura.IV_DIAS_CREDITO),
                                                                        new System.Xml.Linq.XAttribute("IV_USUARIO_CREA", objFactura.IV_USUARIO_CREA == null ? "" : objFactura.IV_USUARIO_CREA),
                                                                        new System.Xml.Linq.XAttribute("IV_HORA_HASTA", objFactura.IV_HORA_HASTA == null ? "" : objFactura.IV_HORA_HASTA),
                                                                        new System.Xml.Linq.XAttribute("IV_IP", objFactura.IV_IP == null ? "" : objFactura.IV_IP),
                                                                        new System.Xml.Linq.XAttribute("IV_TOTAL_BULTOS", objFactura.IV_TOTAL_BULTOS),
                                                                        new System.Xml.Linq.XAttribute("IV_P2D", objFactura.IV_P2D),
                                                                        new System.Xml.Linq.XAttribute("IV_RUC_USUARIO", objFactura.IV_RUC_USUARIO == null ? "" : objFactura.IV_RUC_USUARIO),
                                                                        new System.Xml.Linq.XAttribute("IV_DESC_USUARIO", objFactura.IV_DESC_USUARIO == null ? "" : objFactura.IV_DESC_USUARIO),
                                                                        new System.Xml.Linq.XAttribute("flag", "I"))));


                                this.BtnFacturar.Attributes["disabled"] = "disabled";
                                // this.ImgCarga.Attributes["visible"] = "false";

                                this.Mostrar_Mensaje(string.Format("<b>Se procedió a generar la factura de carga suelta # {0}  {1} ...{2} ", FacturaFinal, ",Puede proceder a imprimir su comprobante.", cMensajeActualizados));

                                /*nuevo proceso de grabado*/

                                objProcesar.xmlCabecera = XMLCabecera.ToString();
                                objProcesar.xmlContenedor = XMLContenedor.ToString();
                                objProcesar.xmlServicios = XMLServicios.ToString();

                                var nProceso = objProcesar.SaveTransaction_cfs_new(out cMensajes);
                                /*fin de nuevo proceso de grabado*/
                                if (!nProceso.HasValue || nProceso.Value <= 0)
                                {
                                    this.Id_Factura_Generada = string.Empty;
                                    this.hf_idfacturagenerada.Value = string.Empty;
                                    this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo grabar datos de la factura de carga suelta..{0}</b>", cMensajes));

                                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                                    /*************************************************************************************************************************************
                                    * crear caso salesforce
                                    * **********************************************************************************************************************************/

                                    this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Error al grabar", cMensajes, out MensajeCasos);
                                        
                                    return;
                                }
                                else
                                {
                                    this.Poblar_Servicios();
                                    objFactura.Detalle.Clear();
                                    objFactura.DetalleServicios.Clear();
                                    Session["InvoiceCFS" + this.hf_BrowserWindowName.Value] = objFactura;
                                    this.Id_Factura_Generada = nProceso.Value.ToString();
                                    this.hf_idfacturagenerada.Value = nProceso.Value.ToString();

                                    this.BtnVisualizar.Attributes.Remove("disabled");
                                       
                                    this.OcultarLoading();
                                    this.Actualiza_Paneles();
                                }

                                  
                            }
                            else
                            {

                                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                                /*************************************************************************************************************************************
                                * crear caso salesforce
                                * **********************************************************************************************************************************/

                                this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Error al grabar", Finalizar.messages.ToString(), out MensajeCasos);

                                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. {0}", Finalizar.messages.ToString()));
                                return;
                            }
                        }
                        else
                        {
                            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            * **********************************************************************************************************************************/

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación CFS", "Error al grabar", string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Problemas al finalizar el DRAFT # {0}", draftNumber), out MensajeCasos);

                            this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Problemas al finalizar el DRAFT # {0}", draftNumber));
                            return;
                        }

                    }

                }
                catch (Exception ex)
                {
                   
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "Facturar CFS", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", OError));

                }
            }

        }


    

    }
}