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
using BillionReglasNegocio;
using CSLSite;

namespace CSLSite.contenedorexpo
{
    public partial class facturacionbookingvisualiza : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
       // private Cls_Bil_Proforma_Consulta objProforma = new Cls_Bil_Proforma_Consulta();

        private Cls_Bil_Invoice_Cabecera objFactura = new Cls_Bil_Invoice_Cabecera();
        private Cls_Bil_Invoice_Detalle objDetalleFactura = new Cls_Bil_Invoice_Detalle();
        private Cls_Bil_Invoice_Servicios objServiciosFactura = new Cls_Bil_Invoice_Servicios();
     

        private Cls_Bil_Invoice_Procesar objProcesar = new Cls_Bil_Invoice_Procesar();

        private List<Cls_Bill_CabeceraExpo> objCabecera = new List<Cls_Bill_CabeceraExpo>();
        private Cls_Bill_Container_Expo objDetalle = new Cls_Bill_Container_Expo();

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
       // private int Fila = 1;
        private string TipoServicio = string.Empty;
        private string draftNumber = string.Empty;
       // private Int64 draftNumberFinal = 0;
        private string NumeroFactura = string.Empty;
        private string Contenedores = string.Empty;
     //   private bool SinServicios = false;

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
           
        }

        private void MostrarLoader()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mostrarloader();", true);
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
            objFactura = Session["InvoiceBooking" + this.hf_BrowserWindowName.Value] as Cls_Bil_Invoice_Cabecera;
            if (objFactura == null)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para facturar"));
                return;
            }
            else
            {
                
                
                CultureInfo enUS = new CultureInfo("en-US");

                //si existen servicios
                if (objFactura.DetalleServicios.Count != 0)
                {

                    this.BtnFacturar.Attributes["class"] = "btn btn-theme07";

                   
                    facturado.InnerText = String.Format("FACTURADO A: [{0}] - {1}", objFactura.IV_ID_FACTURADO, objFactura.IV_DESC_FACTURADO);
                    observacion.InnerText = String.Format("OBSERVACIONES: {0}", objFactura.IV_GLOSA);
                    carga.InnerText = String.Format("NUMERO DE BOOKING: {0}", objFactura.IV_NUMERO_CARGA);
                    contenedor.InnerHtml = String.Format("CONTENEDORES: {0}", objFactura.IV_CONTENEDORES);

                    fecha.InnerText = String.Format("{0}", objFactura.IV_FECHA_CREA.HasValue ? objFactura.IV_FECHA_CREA.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                    

                    c_Subtotal = objFactura.IV_SUBTOTAL == 0 ? "..." : string.Format("{0:c}", objFactura.IV_SUBTOTAL);
                    c_Iva = objFactura.IV_IVA == 0 ? "..." : string.Format("{0:c}", objFactura.IV_IVA);
                    c_Total = objFactura.IV_TOTAL == 0 ? "..." : string.Format("{0:c}", objFactura.IV_TOTAL);
                    total.InnerText = String.Format("{0} USD", c_Total);


                    tab.Append("<table class='table'>");
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
                    facturado.InnerText = string.Empty;
                    observacion.InnerText = string.Empty;
                    carga.InnerText = string.Empty;
                    contenedor.InnerText = string.Empty;

                    fecha.InnerText = string.Empty;
                   

                    tab.Append("<table class='table'>");
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
                    this.Mostrar_Mensaje( string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para facturar"));
                    Response.Redirect("~/facturacionbooking.aspx", false);
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

                    }
                    catch (Exception ex)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...{0}", ex.Message));
                    }
                }

            }
        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.Id_Factura_Generada = string.Empty;
            Response.Redirect("~/contenedorexpo/facturacionbooking.aspx", false);
           
           
        }

        

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
     
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
                    this.MostrarLoader();

                  
                    objFactura = Session["InvoiceBooking" + this.hf_BrowserWindowName.Value] as Cls_Bil_Invoice_Cabecera;
                    if (objFactura == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para facturar"));
                        return;
                    }
                    else
                    {

                        if (objFactura.DetalleServicios.Count == 0)
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe servicios pendientes para facturar"));
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
                                Cliente_Direccion = ListaCliente.CLNT_ADRESS==null ? "" : ListaCliente.CLNT_ADRESS;
                                Cliente_Ciudad = ListaCliente.CLNT_CITY == null ? "" : ListaCliente.CLNT_CITY;
                            }
                            else
                            {
                                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos de cliente/exportador para facturar"));
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos de cliente/exportador para facturar"));
                            return;
                        }

                       
                        
                        //Fila = 1;

                        var VALOR = (from Tbl in objFactura.Detalle
                                                   select new
                                                   { CNTR_VEPR_REFERENCE = (Tbl.IV_REFERENCIA == null ? string.Empty : Tbl.IV_REFERENCIA),
                                                       IV_CNTR_DEPARTED = (Tbl.IV_CNTR_DEPARTED == null ? null : Tbl.IV_CNTR_DEPARTED),
                                                       IV_CNTR_DESCARGA = (Tbl.IV_CNTR_DESCARGA == null ? null : Tbl.IV_CNTR_DESCARGA),
                                                       IV_CNTR_LINEA = (Tbl.IV_LINEA == null ? string.Empty : Tbl.IV_LINEA),
                                                   }).FirstOrDefault();

                        objCabecera = new List<Cls_Bill_CabeceraExpo>();

                        Cls_Bill_CabeceraExpo objCab = new Cls_Bill_CabeceraExpo();

                        List<ClientesCombo> listaClientes = new List<ClientesCombo>();
                        ClientesCombo v_Clientes;
                        v_Clientes = new ClientesCombo();
                        v_Clientes.Orden = 1;
                        v_Clientes.Tipo = "AISV";
                        v_Clientes.Ruc = Cliente_Ruc;
                        v_Clientes.Cliente = objFactura.IV_DESC_FACTURADO;
                        v_Clientes.Booking = objFactura.IV_NUMERO_CARGA;

                        N4.Entidades.Cliente DatosAdicionales = new N4.Entidades.Cliente();
                        DatosAdicionales.CLNT_FAX_INVC = "";
                        DatosAdicionales.CLNT_CITY = Cliente_Ciudad;
                        DatosAdicionales.CLNT_ADRESS = Cliente_Direccion;
                        DatosAdicionales.DIAS_CREDITO = objFactura.IV_DIAS_CREDITO;

                        v_Clientes.DatoCliente = DatosAdicionales;

                        /*v_Clientes.DatoCliente.CLNT_FAX_INVC = "";
                        v_Clientes.DatoCliente.CLNT_CITY = Cliente_Ciudad;
                        v_Clientes.DatoCliente.CLNT_ADRESS = Cliente_Direccion;
                        v_Clientes.DatoCliente.DIAS_CREDITO = objFactura.IV_DIAS_CREDITO;*/
                        listaClientes.Add(v_Clientes);

                        objCab.CNTR_BKNG_BOOKING = objFactura.IV_NUMERO_CARGA;
                        objCab.CNTR_VEPR_REFERENCE = VALOR.CNTR_VEPR_REFERENCE;
                        objCab.CNTR_CLIENT_ID = objFactura.IV_ID_FACTURADO;
                        objCab.CNTR_CLIENT = objFactura.IV_DESC_FACTURADO;
                        objCab.CNTR_CLIENTE = listaClientes.FirstOrDefault();
                        objCab.CNTR_CLIENTES = listaClientes.ToList();
                        objCab.VISTO = true;
                        objCab.CNTR_CREDITO = (objFactura.IV_DIAS_CREDITO > 0 ? true : false); 
                        objCab.CNTR_CONTADO = !objCab.CNTR_CREDITO;

                        objCab.CNTR_CONTAINERS = "";
                        objCab.CNTR_FECHA = DateTime.Now;
                        objCab.CNTR_ESTADO = "N";
                        objCab.CNTR_VEPR_VSSL_NAME = objFactura.IV_BUQUE;
                        objCab.CNTR_VEPR_VOYAGE = objFactura.IV_VIAJE;// cab.VIAJE;
                        objCab.CNTR_VEPR_ACTUAL_ARRIVAL = VALOR.IV_CNTR_DESCARGA;//cab.LLEGADA;
                        objCab.CNTR_VEPR_ACTUAL_DEPARTED = VALOR.IV_CNTR_DEPARTED;//cab.SALIDA;
                        objCab.CNTR_USUARIO_CREA = objFactura.IV_USUARIO_CREA.Trim();

                        objCab.CNTR_CLNT_CUSTOMER_LINE = VALOR.IV_CNTR_LINEA;
                        
                        objCab.CNTR_PROCESADO = false;

                        objCab.CNTR_INVOICE_TYPE = objFactura.INVOICE_TYPE;
                        objCab.CNTR_INVOICE_TYPE_NAME = objCab.CNTR_CREDITO ? "EXPORCREFULL" : "EXPOCONFULL";
                        objCab.CNTR_CONTAINERS = objFactura.IV_CONTENEDORES;

                        //crea detalle
                        System.Data.DataSet v_ds = new System.Data.DataSet("CONTENEDORES");
                        System.Data.DataTable v_dt = new System.Data.DataTable("CONTAINERS");
                        v_dt.Columns.Add("ID", typeof(Int64));
                        v_dt.Columns.Add("CONTAINER", typeof(String));

                        cMensajes = string.Empty;
                        string cMensajeActualizados = string.Empty;
                        foreach (var Det in objFactura.Detalle)
                        {
                            System.Data.DataRow v_dr = v_dt.NewRow();
                            v_dr["ID"] = Det.IV_GKEY;
                            v_dr["CONTAINER"] = Det.IV_CONTENEDOR;
                            v_dt.Rows.Add(v_dr);

                            objCab.CNTR_CONTENEDOR20 += Det.IV_TAMANO == "20" ? 1 : 0;
                            objCab.CNTR_CONTENEDOR40 += Det.IV_TAMANO == "40" ? 1 : 0;

                            objDetalle = new Cls_Bill_Container_Expo();
                            objDetalle.VISTO = false;
                            objDetalle.CNTR_CONSECUTIVO = Det.IV_GKEY;
                            objDetalle.CNTR_CONTAINER = Det.IV_CONTENEDOR;
                            objDetalle.CNTR_TYPE = Det.IV_TRAFICO;
                            objDetalle.CNTR_TYSZ_SIZE = Det.IV_TAMANO;
                            objDetalle.CNTR_TYSZ_ISO = Det.CNTR_TYSZ_ISO;
                            objDetalle.CNTR_TYSZ_TYPE = Det.IV_REEFER;
                            objDetalle.CNTR_FULL_EMPTY_CODE = Det.IV_FULL_VACIO;
                            objDetalle.CNTR_YARD_STATUS = Det.IV_IN_OUT;
                            objDetalle.CNTR_TEMPERATURE =Det.CNTR_TEMPERATURE;  
                            objDetalle.CNTR_TYPE_DOCUMENT = Det.CNTR_TYPE_DOCUMENT;
                            objDetalle.CNTR_DOCUMENT = Det.IV_DOCUMENTO;
                            objDetalle.CNTR_VEPR_REFERENCE = Det.IV_REFERENCIA;
                            objDetalle.CNTR_CLNT_CUSTOMER_LINE = Det.IV_LINEA;
                            objDetalle.CNTR_LCL_FCL = Det.CNTR_LCL_FCL;
                            objDetalle.CNTR_CATY_CARGO_TYPE = Det.CNTR_CATY_CARGO_TYPE;
                            objDetalle.CNTR_FREIGHT_KIND = Det.CNTR_FREIGHT_KIND;
                            objDetalle.CNTR_DD = Det.IV_CNTR_DD ==true ? 1 : 0;
                            objDetalle.CNTR_BKNG_BOOKING = Det.CNTR_BKNG_BOOKING;
                            objDetalle.FECHA_CAS = Det.IV_CAS;
                            objDetalle.CNTR_AISV = Det.IV_AISV;
                            objDetalle.CNTR_HOLD = Det.IV_BLOQUEO==true ? 1 : 0;
                            objDetalle.CNTR_REEFER_CONT = Det.IV_REEFER;
                            objDetalle.CNTR_VEPR_VSSL_NAME = Det.CNTR_VEPR_VSSL_NAME;
                            objDetalle.CNTR_VEPR_VOYAGE = Det.CNTR_VEPR_VOYAGE;
                            objDetalle.CNTR_VEPR_ACTUAL_ARRIVAL = Det.IV_CNTR_DESCARGA;
                            objDetalle.CNTR_VEPR_ACTUAL_DEPARTED = Det.IV_CNTR_DEPARTED;
                            objDetalle.IV_USUARIO_CREA = Det.IV_USUARIO_CREA;
                            objDetalle.CNTR_PROFORMA = Det.CNTR_PROFORMA;
                            objCab.Detalle.Add(objDetalle);

                        }

                        v_ds.Tables.Add(v_dt);
                        objCab.CNTR_CONTAINERSXML = v_ds.GetXml();
                        objCabecera.Add(objCab);

                        RN_Bill_InvoiceContainerExpo _obj = new RN_Bill_InvoiceContainerExpo();
                        string _error = string.Empty;
                        _error = _obj.grabarEntidad(objCabecera.Where(p => p.VISTO == true).ToList());

                        if (_error != string.Empty)
                        {
                            this.Id_Factura_Generada = string.Empty;
                            this.hf_idfacturagenerada.Value = string.Empty;
                            this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo grabar datos de la pre-factura..{0}</b>", _error));
                            return;

                        }
                        else
                        {
                            this.BtnFacturar.Attributes["disabled"] = "disabled";
                            Mostrar_Mensaje("<b>Informativo! </b> Transacción generada exitosamente");

                            objFactura.Detalle.Clear();
                            objFactura.DetalleServicios.Clear();
                            Session["InvoiceBooking" + this.hf_BrowserWindowName.Value] = objFactura;
                            this.Id_Factura_Generada = String.Empty;
                            this.hf_idfacturagenerada.Value = String.Empty;


                            this.OcultarLoading();
                            this.Actualiza_Paneles();

                            
                        }



                    }

                }
                catch (Exception ex)
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "Facturar", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", OError));   

                }
            }

        }


        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            
            

        }

    }
}