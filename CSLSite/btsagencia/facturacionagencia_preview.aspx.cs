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
    public partial class facturacionagencia_preview : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Proforma_Consulta objProforma = new Cls_Bil_Proforma_Consulta();

        private Cls_Bil_Invoice_Cabecera objFactura = new Cls_Bil_Invoice_Cabecera();
        private Cls_Bil_Invoice_Detalle objDetalleFactura = new Cls_Bil_Invoice_Detalle();
        private Cls_Bil_Invoice_Servicios objServiciosFactura = new Cls_Bil_Invoice_Servicios();
        private Cls_Bil_Invoice_Actualiza_Pase objActualiza_Pase = new Cls_Bil_Invoice_Actualiza_Pase();

        private BTS_Procesa_Draf objProcesar = new BTS_Procesa_Draf();


        private BTS_Prev_Cabecera objFacturaBTS = new BTS_Prev_Cabecera();
        private Cls_Prev_Detalle_Bodega objDetalleFacturaBodega = new Cls_Prev_Detalle_Bodega();
        private Cls_Prev_Detalle_Muelle objDetalleFacturaMuelle = new Cls_Prev_Detalle_Muelle();
        private Cls_Prev_Detalle_Servicios objDetalleFacturaServicios = new Cls_Prev_Detalle_Servicios();

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
            objFacturaBTS = Session["InvoiceBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;
            if (objFacturaBTS == null)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para facturar"));
                return;
            }
            else
            {
               
                
                CultureInfo enUS = new CultureInfo("en-US");

                //si existen servicios
                if (objFacturaBTS.Detalle_Servicios.Count != 0)
                {

                    this.BtnFacturar.Attributes["class"] = "btn btn-primary";


                    if (objFacturaBTS.TIPO_CARGA.Equals("FRIA/SECA"))
                    {
                        this.sub_opcion.InnerHtml = "PREVISUALIZACIÓN DE FACTURA DE AGENCIA (FRIA/SECA)";
                    }
                    if (objFacturaBTS.TIPO_CARGA.Equals("MUELLE"))
                    {
                        this.sub_opcion.InnerHtml = "PREVISUALIZACIÓN DE FACTURA DE AGENCIA (MUELLE)";
                    }

                    TITULOS.Update();

                    cliente.InnerText = String.Format("FACTURADO A: [{0}] - {1}", objFacturaBTS.ID_FACTURADO, objFacturaBTS.DESC_FACTURADO);
                    cajas_bodega.InnerText = String.Format("TOTAL CAJAS BODEGA FRIA/SECA: {0}", objFacturaBTS.TOTAL_CAJAS_BODEGA);
                    cajas_muelle.InnerText = String.Format("TOTAL CAJAS MUELLE: {0}", objFacturaBTS.TOTAL_CAJAS_MUELLE);
                    carga.InnerText = String.Format("NUMERO DE REFERENCIA: {0}", objFacturaBTS.REFERENCIA);
                  

                    fecha.InnerText = String.Format("{0}", objFacturaBTS.FECHA.HasValue ? objFacturaBTS.FECHA.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                    fecha_hasta.InnerText = String.Format("{0}", objFacturaBTS.FECHA.HasValue ? objFacturaBTS.FECHA.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                    fecha_hasta.Visible = false;

                    c_Subtotal = objFacturaBTS.SUBTOTAL == 0 ? "..." : string.Format("{0:c}", objFacturaBTS.SUBTOTAL);
                    c_Iva = objFacturaBTS.IVA == 0 ? "..." : string.Format("{0:c}", objFacturaBTS.IVA);
                    c_Total = objFacturaBTS.TOTAL == 0 ? "..." : string.Format("{0:c}", objFacturaBTS.TOTAL);
                    total.InnerText = String.Format("{0} USD", c_Total);


                    tab.Append("<table class='table table-bordered invoice'>");
                    tab.Append("<thead>" +
                          "<tr>" +
                          "<th class='text-center'>TIPO</th>" +
                          "<th class='text-left'>DESCRIPCION</th>" +
                          "<th class='text-center'>CANTIDAD</th>" +
                          "<th class='text-right'>V.UNITARIO</th>" +
                          "<th class='text-right'>V.TOTAL</th>" +
                          "</tr>" +
                        "<thead>");
                    tab.Append("<tbody>");




                    foreach (var pf in objFacturaBTS.Detalle_Servicios)
                    {

                        tab.AppendFormat("<tr>" +
                            "<td class='text-center'>{0}</td>" +
                            "<td>{1}</td>" +
                            "<td class='text-center'>{2}</td>" +
                            "<td class='text-right'>{3}</td>" +
                            "<td class='text-right'>{4}</td>" +
                            "</tr>",
                            String.IsNullOrEmpty(pf.notes) ? "..." : pf.notes,
                            String.IsNullOrEmpty(pf.description) ? "..." : pf.description,
                            pf.quantity.Value == 0 ? "..." : string.Format("{0:N2}", pf.quantity.Value),
                            pf.rate_billed.Value == 0 ? "..." : string.Format("{0:c}", pf.rate_billed.Value),
                            pf.amount.Value == 0 ? "..." : string.Format("{0:c}", pf.amount.Value)
                            );


                        n_fila++;
                    }
                }
                else
                {
                    
                    cliente.InnerText = string.Empty;
                    cajas_bodega.InnerText = string.Empty;
                    cajas_muelle.InnerText = string.Empty;
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
                detalle_carga.Tipo = TipoCarga.BRBK; //opcional
                detalle_carga.Titulo = "Modulo de Facturación Bodega BTS"; //opcional
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
                    this.Mostrar_Mensaje( string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para facturar bodega BTS"));
                    Response.Redirect("~/facturacionagencia.aspx", false);
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
                objFacturaBTS = Session["InvoiceBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;
                if (objFacturaBTS == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Aún no ha generado el borrador de factura de Bodega BTS, para poder imprimirla"));
                    return;
                }

                if (string.IsNullOrEmpty(this.hf_idfacturagenerada.Value))
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Aún no ha generado el borrador de factura de Bodega BTS, para poder imprimirla"));
                    return;
                }

                //IMPRIMIR FACTURA -FORMATO HTML
                string cId = securetext(this.hf_idfacturagenerada.Value);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('../btsagencia/facturaagencia_print_preview.aspx?id_comprobante=" + cId + "');", true);

            }


        }

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {

          
            this.Mostrar_Mensaje(string.Format("<b>GENERANDO BORRADOR BODEGA BTS! </b>....POR FAVOR ESPERE, SE ESTA GENERANDO EL BORRADOR DE LA FACTURA......."));

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

                   

                    string pInvoiceType = string.Empty;

                    objFacturaBTS = Session["InvoiceBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;
                    if (objFacturaBTS == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para facturar bodega BTS"));
                        return;
                    }
                    else
                    {

                        if (objFacturaBTS.Detalle_Servicios.Count == 0)
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe servicios pendientes para facturar bodega BTS: {0}", objFacturaBTS.REFERENCIA));
                            return;
                        }

                        /*nuevo proceso de grabado*/
                        System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new System.Xml.Linq.XElement("FACT_CABECERA",
                                                                new System.Xml.Linq.XElement("CABECERA",
                                                                new System.Xml.Linq.XAttribute("GLOSA", objFacturaBTS.GLOSA == null ? "" : objFacturaBTS.GLOSA),
                                                                new System.Xml.Linq.XAttribute("FECHA", objFacturaBTS.FECHA == null ? DateTime.Parse("1900/01/01") : objFacturaBTS.FECHA),
                                                                new System.Xml.Linq.XAttribute("TIPO_CARGA", objFacturaBTS.TIPO_CARGA == null ? "" : objFacturaBTS.TIPO_CARGA),
                                                                new System.Xml.Linq.XAttribute("ID_CLIENTE", objFacturaBTS.ID_CLIENTE == null ? "" : objFacturaBTS.ID_CLIENTE),
                                                                new System.Xml.Linq.XAttribute("DESC_CLIENTE", objFacturaBTS.DESC_CLIENTE == null ? "" : objFacturaBTS.DESC_CLIENTE),
                                                                new System.Xml.Linq.XAttribute("ID_FACTURADO", objFacturaBTS.ID_FACTURADO == null ? "" : objFacturaBTS.ID_FACTURADO),
                                                                new System.Xml.Linq.XAttribute("DESC_FACTURADO", objFacturaBTS.DESC_FACTURADO == null ? "" : objFacturaBTS.DESC_FACTURADO),
                                                                new System.Xml.Linq.XAttribute("SUBTOTAL", objFacturaBTS.SUBTOTAL),
                                                                new System.Xml.Linq.XAttribute("IVA", objFacturaBTS.IVA),
                                                                new System.Xml.Linq.XAttribute("TOTAL", objFacturaBTS.TOTAL),
                                                                new System.Xml.Linq.XAttribute("DRAF", objFacturaBTS.DRAF == null ? "" : objFacturaBTS.DRAF),
                                                                new System.Xml.Linq.XAttribute("REFERENCIA", objFacturaBTS.REFERENCIA == null ? "" : objFacturaBTS.REFERENCIA),
                                                                new System.Xml.Linq.XAttribute("DIR_FACTURADO", objFacturaBTS.DIR_FACTURADO == null ? "" : objFacturaBTS.DIR_FACTURADO),
                                                                new System.Xml.Linq.XAttribute("EMAIL_FACTURADO", objFacturaBTS.EMAIL_FACTURADO == null ? "" : objFacturaBTS.EMAIL_FACTURADO),
                                                                new System.Xml.Linq.XAttribute("CIUDAD_FACTURADO", objFacturaBTS.CIUDAD_FACTURADO == null ? "" : objFacturaBTS.CIUDAD_FACTURADO),
                                                                new System.Xml.Linq.XAttribute("DIAS_CREDITO", objFacturaBTS.DIAS_CREDITO),
                                                                new System.Xml.Linq.XAttribute("USUARIO_CREA", objFacturaBTS.IV_USUARIO_CREA == null ? "" : objFacturaBTS.IV_USUARIO_CREA),
                                                                new System.Xml.Linq.XAttribute("CAJAS_BODEGA", objFacturaBTS.TOTAL_CAJAS_BODEGA),
                                                                new System.Xml.Linq.XAttribute("CAJAS_MUELLE", objFacturaBTS.TOTAL_CAJAS_MUELLE),
                                                                new System.Xml.Linq.XAttribute("RUC_USUARIO", objFacturaBTS.RUC_USUARIO == null ? "" : objFacturaBTS.RUC_USUARIO),
                                                                new System.Xml.Linq.XAttribute("DESC_USUARIO", objFacturaBTS.DESC_USUARIO == null ? "" : objFacturaBTS.DESC_USUARIO),
                                                                new System.Xml.Linq.XAttribute("flag", "I"))));

                        System.Xml.Linq.XDocument XMLBodega = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                               new System.Xml.Linq.XElement("FACT_BODEGA", from p in objFacturaBTS.Detalle_bodega.AsEnumerable().AsParallel()
                                                                              select new System.Xml.Linq.XElement("DETALLE",
                                                                               new System.Xml.Linq.XAttribute("idNave", p.idNave == null ? "" : p.idNave.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("codLine", p.codLine == null ? "" : p.codLine.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("nave", p.nave == null ? "" : p.nave.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("ruc", p.ruc == null ? "" : p.ruc.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("Exportador", p.Exportador == null ? "" : p.Exportador.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("booking", p.booking == null ? "" : p.booking.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("idModalidad", p.idModalidad),
                                                                               new System.Xml.Linq.XAttribute("desc_modalidad", p.desc_modalidad == null ? "" : p.desc_modalidad.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("id_bodega", p.id_bodega),
                                                                               new System.Xml.Linq.XAttribute("desc_bodega", p.desc_bodega == null ? "" : p.desc_bodega.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("id_tipo_bodega", p.id_tipo_bodega),
                                                                               new System.Xml.Linq.XAttribute("tipo_bodega", p.desc_bodega == null ? "" : p.tipo_bodega.ToString().Trim()),
                                                                               new System.Xml.Linq.XAttribute("QTY_out", p.QTY_out),
                                                                               new System.Xml.Linq.XAttribute("flag", "I"))));

                        System.Xml.Linq.XDocument XMLMuelle = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                              new System.Xml.Linq.XElement("FACT_MUELLE", from p in objFacturaBTS.Detalle_muelle.AsEnumerable().AsParallel()
                                                                          select new System.Xml.Linq.XElement("DETALLE",
                                                                            new System.Xml.Linq.XAttribute("idNave", p.idNave == null ? "" : p.idNave.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("codLine", p.codLine == null ? "" : p.codLine.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("nave", p.nave == null ? "" : p.nave.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("aisv_codig_clte", p.aisv_codig_clte == null ? "" : p.aisv_codig_clte.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("aisv_nom_expor", p.aisv_nom_expor == null ? "" : p.aisv_nom_expor.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("aisv_estado", p.aisv_estado == null ? "" : p.aisv_estado.ToString().Trim()),
                                                                            new System.Xml.Linq.XAttribute("cajas", p.cajas),
                                                                            new System.Xml.Linq.XAttribute("flag", "I"))));

                        System.Xml.Linq.XDocument XMLServicios = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                             new System.Xml.Linq.XElement("FACT_SERVICIOS", from p in objFacturaBTS.Detalle_Servicios.AsEnumerable().AsParallel()
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

                       
                     
                      
                        cMensajes = string.Empty;
                        string cMensajeActualizados = string.Empty;
                       


                        this.BtnFacturar.Attributes["disabled"] = "disabled";

                      
                        /*nuevo proceso de grabado*/

                        objProcesar.xmlCabecera = XMLCabecera.ToString();
                        objProcesar.xmlBodega = XMLBodega.ToString();
                        objProcesar.xmlMuelle = XMLMuelle.ToString();
                        objProcesar.xmlServicios = XMLServicios.ToString();

                        var nProceso = objProcesar.SaveTransaction_BTS(out cMensajes);

                        /*fin de nuevo proceso de grabado*/
                        if (!nProceso.HasValue || nProceso.Value <= 0)
                        {
                            this.Id_Factura_Generada = string.Empty;
                            this.hf_idfacturagenerada.Value = string.Empty;
                            this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo grabar datos de la factura de bodega BTS..{0}</b>", cMensajes));

                            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            * **********************************************************************************************************************************/

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Facturación BODEGA BTS", "Error al grabar", cMensajes, out MensajeCasos);

                            return;
                        }
                        else
                        {

                            this.Poblar_Servicios();

                            objFacturaBTS.Detalle_bodega.Clear();
                            objFacturaBTS.Detalle_muelle.Clear();
                            objFacturaBTS.Detalle_Servicios.Clear();

                            Session["InvoiceBTS" + this.hf_BrowserWindowName.Value] = objFacturaBTS;

                            this.Id_Factura_Generada = nProceso.Value.ToString();
                            this.hf_idfacturagenerada.Value = nProceso.Value.ToString();

                            this.BtnVisualizar.Attributes.Remove("disabled");

                            this.OcultarLoading();
                            this.Actualiza_Paneles();
                        }


                        this.Mostrar_Mensaje(string.Format("<b>Se procedió a generar el borrador de factura de bodega BTS - Draf # {0}  {1} ...{2} ", objFacturaBTS.DRAF, ",Puede proceder a imprimir su comprobante.", cMensajeActualizados));




                    }

                }
                catch (Exception ex)
                {
                   
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "Facturar Bodega BTS", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", OError));

                }
            }

        }


    

    }
}