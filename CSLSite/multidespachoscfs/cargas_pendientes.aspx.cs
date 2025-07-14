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
using SqlConexion;
using CasManual;


using System.Reflection;
using System.ComponentModel;

namespace CSLSite
{


    public partial class cargas_pendientes : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;
      
      
        private string cMensajes;

        private cfs_cargas_pendientes objCab = new cfs_cargas_pendientes();
        private cfs_cargas_pendientes_detalle objDet = new cfs_cargas_pendientes_detalle();
        private cfs_cargas_pendientes objCargasPendientes = new cfs_cargas_pendientes();

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
       
        
        private string LoginName = string.Empty;

        private string c_Subtotal = string.Empty;
        private string c_Iva = string.Empty;
        private string c_Total = string.Empty;

        /*variables control de credito*/

        private static Int64? lm = -3;
        private string OError;

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


        private void Actualiza_Paneles()
        {
           
            this.UPCARGA.Update();
            this.UPBOTONESFACTURA.Update();
            this.UPTITULO.Update();

        }


        private void Cargas_Pendientes()
        {
            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                objCab = Session["CargasPendientes" + this.hf_BrowserWindowName.Value] as cfs_cargas_pendientes;

                var Tabla = cfs_cargas_pendientes.cargas_pendientes(ClsUsuario.ruc ,out cMensajes);
                if (Tabla == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de cargas pendientes. {0}", cMensajes));
                    return;
                }
                if (Tabla.Count <= 0)
                {
                    grilla.DataSource = null;
                    grilla.DataBind();

                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de cargas pendientes."));
                    return;
                }

                objCab.Detalle.Clear();

                int i = 0;
                foreach (var Det in Tabla)
                {
                    objDet = new cfs_cargas_pendientes_detalle();
                    objDet.mrn = Det.mrn;
                    objDet.msn = Det.msn;
                    objDet.hsn = Det.hsn;
                    objDet.cntr = Det.cntr;
                    objDet.importador_id = Det.importador_id;
                    objDet.importador_name = Det.importador_name;
                    objDet.descarga = Det.descarga;
                    objDet.descripcion = Det.descripcion;
                    objDet.total_partida = Det.total_partida;
                    objDet.volumen =Det.volumen;
                    objDet.peso = Det.peso;
                    objDet.visto = false;
                    objDet.numero_carga = string.Format("{0}-{1}-{2}", Det.mrn, Det.msn, Det.hsn);
                    objCab.Detalle.Add(objDet);
                    i++;
                }

                if (i > 0)
                {
                    grilla.DataSource = objCab.Detalle;
                    grilla.DataBind();
                }
                else
                {
                    grilla.DataSource = null;
                    grilla.DataBind();

                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de cargas pendientes."));
                    return;
                }

             

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Cargas_Pendientes), "Cargas_Pendientes", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));



            }
        }


        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('"+valor+"');", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Mostrar_Mensaje(string Mensaje)
        {
          
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
              
            
           
            this.Actualiza_Paneles();
        }

        private void Mostrar_Mensaje_Factura(string Mensaje)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "closeModal();", true);

            this.banmsg_det.Visible = true;
            this.banmsg_det.InnerHtml = Mensaje;

            this.banmsg_cab.Visible = true;
            this.banmsg_cab.InnerHtml = Mensaje;

            OcultarLoading();

            this.Actualiza_Paneles();
        }

        private void Mostrar_Mensaje_Proceso(string Mensaje)
        {

            this.banmsg_det.Visible = true;
            this.banmsg_cab.Visible = true;
            this.banmsg_det.InnerHtml = Mensaje;
            this.banmsg_cab.InnerHtml = Mensaje;
            OcultarLoading();
            this.Actualiza_Paneles();
        }

        private void OcultarLoading()
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
            UPCARGA.Update();
        }

        private void MostrarLoader()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mostrarloader();", true);
            UPCARGA.Update();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;          
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();

        }

      
        

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCab = new cfs_cargas_pendientes();           
            Session["CargasPendientes" + this.hf_BrowserWindowName.Value] = objCab;
            
        }


        private void CboTransportistas(string ID_AGENTE)
        {
            try
            {
                DataSet dsRetorno = new DataSet();

                List<cfs_agente_transportista> Listado = cfs_agente_transportista.CboTransportista(ID_AGENTE, out cMensajes);

                this.CboAsumeFactura.DataSource = Listado;
                this.CboAsumeFactura.DataTextField = "DESC_TRANSPORTE";
                this.CboAsumeFactura.DataValueField = "RUC_TRANSPORTE";
                this.CboAsumeFactura.DataBind();


            }
            catch (Exception ex)
            {
              
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(CboTransportistas), "CboTransportistas", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));


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
            this.banmsg_cab.Visible = IsPostBack;
            this.banmsg_det.Visible = IsPostBack;


            if (!Page.IsPostBack)
            {

                this.banmsg.InnerText = string.Empty;
                this.banmsg_cab.InnerText = string.Empty;
                this.banmsg_det.InnerText = string.Empty;
                this.TxtTotalBultos.Text = string.Empty;
                this.TxtTotalPeso.Text = string.Empty;
                this.TxtTotalVolumen.Text = string.Empty;

                ClsUsuario = Page.Tracker();
                
            }

        }

       

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                if (!Page.IsPostBack)
                {     
                   this.Crear_Sesion();

                    this.Cargas_Pendientes();

                    this.BtnConfirmar.Attributes["disabled"] = "disabled";
                    this.BtnVisualizar.Attributes["disabled"] = "disabled";

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    this.CboTransportistas(ClsUsuario.ruc);
                }
               

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

        #region "Evento Marcar Cargar para facturar"

        protected void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPaseTarja = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPaseTarja.NamingContainer;
                Label Lblnumero_carga = (Label)item.FindControl("numero_carga");

                objCab = Session["CargasPendientes" + this.hf_BrowserWindowName.Value] as cfs_cargas_pendientes;
                var Detalle = objCab.Detalle.FirstOrDefault(f => f.numero_carga.Equals(Lblnumero_carga.Text));
                if (Detalle != null)
                {
                    Detalle.visto = chkPaseTarja.Checked;


                }

                var Bultos = objCab.Detalle.Where(x=> x.visto==true).Sum(p => p.total_partida);
                var Volumen = objCab.Detalle.Where(x => x.visto == true).Sum(p => p.volumen);
                var Peso = objCab.Detalle.Where(x => x.visto == true).Sum(p => p.peso);

                this.TxtTotalBultos.Text = Bultos.ToString();
                this.TxtTotalVolumen.Text = Volumen.ToString();
                this.TxtTotalPeso.Text = Peso.ToString();

                grilla.DataSource = objCab.Detalle;
                grilla.DataBind();

                Session["CargasPendientes" + this.hf_BrowserWindowName.Value] = objCab;

                this.UPBOTONES.Update();



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }
        #endregion

        #region "Eventos para generar vista previa de facturas"

        protected void BtnFacturar_Click(object sender, EventArgs e)
        {
            try
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

                    StringBuilder tab = new StringBuilder();

                    this.BtnConfirmar.Attributes["disabled"] = "disabled";
                    this.BtnVisualizar.Attributes["disabled"] = "disabled";
                   

                    this.lbl_agente.InnerText = string.Empty;
                    this.lbl_facturado.InnerText = string.Empty;
                   
                    this.lbl_observacion.InnerText = string.Empty;
                    this.lbl_carga.InnerText = string.Empty;

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
                    string IdAgenteCodigo = string.Empty;
                    string ID_AGENTE = string.Empty;
                    string DESC_AGENTE = string.Empty;

                    //valida que se seleccione la persona a facturar
                    if (this.CboAsumeFactura.SelectedIndex == -1)
                    {
                       
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
                           
                            this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", ID_FACTURADO));
                            return;
                        }

                    }
                    else
                    {
                     
                        var ExisteAsume = CboAsumeFactura.Items.FindByValue(ID_FACTURADO);
                        if (ExisteAsume != null)
                        {
                            DESC_AGENTE = ExisteAsume.Text.Split('-').ToList()[1].Trim();

                        }

                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.</b>", ID_FACTURADO, DESC_AGENTE));
                        return;
                    }


                    /*
                    var AgenteCod = N4.Entidades.Agente.ObtenerAgentePorRuc(ClsUsuario.loginname, ClsUsuario.ruc);
                    if (AgenteCod.Exitoso)
                    {
                        var ListaAgente = AgenteCod.Resultado;
                        if (ListaAgente != null)
                        {
                            IdAgenteCodigo = ListaAgente.codigo;
                            ID_AGENTE = ListaAgente.ruc;
                            DESC_AGENTE = ListaAgente.nombres;
                        }
                    }
                    */
                    lbl_agente.InnerText = String.Format("AGENTE ADUANERO: [{0}] - {1}", ID_AGENTE, DESC_AGENTE);
                    lbl_facturado.InnerText = String.Format("FACTURADO A: [{0}] - {1}", ID_AGENTE, DESC_AGENTE);

                    this.banmsg_cab.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;

                    objCab = Session["CargasPendientes" + this.hf_BrowserWindowName.Value] as cfs_cargas_pendientes;

                    if (objCab == null)
                    {

                        //this.BtnGrabar.Attributes.Remove("disabled");
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas a facturar.. </b>"));
                        return;
                    }

                    var LinqCargas = (from p in objCab.Detalle.Where(x => x.visto == true)
                                              select p.numero_carga).ToList();

                    if (LinqCargas.Count == 0)
                    {
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas a facturar. </b>"));
                        return;
                    }

                    if (LinqCargas.Count <= 1)
                    {
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas a facturar. </b>"));
                        return;
                    }

                    if (LinqCargas.Count > 5)
                    {
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Sólo puede facturar hasta 5 números de cargas </b>"));
                        return;
                    }

                    var oServicio = cfs_buscar_tarifas.GetServicio(LinqCargas.Count);

                    if (oServicio == null)
                    {

                       // this.BtnGrabar.Attributes.Remove("disabled");
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe tarifa creada para poder facturar</b>"));
                        return;
                    }

                    string numeros_cargas = string.Join(", ", LinqCargas);
                    
                    lbl_observacion.InnerText = String.Format("OBSERVACIONES: {0}", "");
                    lbl_carga.InnerText = String.Format("NUMERO DE CARGA: {0}", numeros_cargas);

                    fecha.InnerText = String.Format("{0}", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    fecha_hasta.InnerText = String.Format("{0}", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    fecha_hasta.Visible = false;

                    tab.Clear();

                    c_Subtotal = oServicio.VALOR == 0 ? "..." : string.Format("{0:c}", oServicio.VALOR);
                    c_Iva = oServicio.VALOR_IVA == 0 ? "..." : string.Format("{0:c}", oServicio.VALOR_IVA);
                    c_Total = oServicio.VALOR_TOTAL == 0 ? "..." : string.Format("{0:c}", oServicio.VALOR_TOTAL);
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
                           String.IsNullOrEmpty(oServicio.TIPO) ? "..." : oServicio.TIPO,
                            String.IsNullOrEmpty(oServicio.DESCRIPCION) ? "..." : oServicio.DESCRIPCION.Trim(),
                           string.Format("{0:N2}", 1),
                           oServicio.VALOR == 0 ? "..." : string.Format("{0:c}", oServicio.VALOR),
                           oServicio.VALOR == 0 ? "..." : string.Format("{0:c}", oServicio.VALOR)
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
                    this.banmsg_det.Visible = false;


                    this.Actualiza_Paneles();

                }


            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnFacturar_Click), "cargas_pendientes.aspx", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        #endregion


        protected void BtnConfirmar_Click(object sender, EventArgs e)
        {

            this.Mostrar_Mensaje_Proceso(string.Format("<b>GENERANDO FACTURA DE MULTIDESPACHO! </b>....POR FAVOR ESPERE, SE ESTA GENERANDO LA FACTURA......."));

            this.ImgCarga.Attributes["class"] = "ver";
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

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdAgenteCodigo = string.Empty;
                    string ID_AGENTE = string.Empty;
                    string DESC_AGENTE = string.Empty;
                    string CODIGO_AGENTE = string.Empty;
                    string NOMBRES_AGENTE = string.Empty;
                    CODIGO_AGENTE = ClsUsuario.ruc.Trim();
                    //valida que se seleccione la persona a facturar
                    if (this.CboAsumeFactura.SelectedIndex == -1)
                    {
                        this.ImgCarga.Attributes["class"] = "nover";
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
                            this.ImgCarga.Attributes["class"] = "nover";
                            this.CboAsumeFactura.Focus();
                            this.UPBOTONES.Update();
                            this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", ID_FACTURADO));
                            return;
                        }

                    }
                    else
                    {
                        this.ImgCarga.Attributes["class"] = "nover";
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
                   

                    
                    var AgenteCod = N4.Entidades.Agente.ObtenerAgentePorRuc(ClsUsuario.loginname, ClsUsuario.ruc);
                    if (AgenteCod.Exitoso)
                    {
                        var ListaAgente = AgenteCod.Resultado;
                        if (ListaAgente != null)
                        {
                            IdAgenteCodigo = ListaAgente.codigo;
                            CODIGO_AGENTE = ListaAgente.ruc;
                            NOMBRES_AGENTE = ListaAgente.nombres;
                        }
                    }

                    objCab = Session["CargasPendientes" + this.hf_BrowserWindowName.Value] as cfs_cargas_pendientes;

                    if (objCab == null)
                    {
                        this.ImgCarga.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();

                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas a facturar.. </b>"));
                        return;
                    }

                    var LinqCargas = (from p in objCab.Detalle.Where(x => x.visto == true)
                                      select p.numero_carga).ToList();

                    if (LinqCargas.Count == 0)
                    {
                        this.ImgCarga.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas a facturar. </b>"));
                        return;
                    }

                    if (LinqCargas.Count <= 1)
                    {
                        this.ImgCarga.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar las cargas a facturar. </b>"));
                        return;
                    }

                    if (LinqCargas.Count > 5)
                    {
                        this.ImgCarga.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! Sólo puede facturar hasta 5 números de cargas </b>"));
                        return;
                    }

                    var oServicio = cfs_buscar_tarifas.GetServicio(LinqCargas.Count);

                    if (oServicio == null)
                    {
                        this.ImgCarga.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();

                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe tarifa creada para poder facturar</b>"));
                        return;
                    }

                    string request = string.Empty;
                    string response = string.Empty;
                    string v_Error = "";
                    decimal v_monto = 0;
                    string Numero_Factura = string.Empty;
                    string numeros_cargas = string.Join(", ", LinqCargas);

                    Respuesta.ResultadoOperacion<bool> resp;
                    resp = ServicioSCA.CargarServicioMultiDespacho(oServicio.INVOICETYPE, ID_AGENTE, oServicio.CODIGO_TARIJA_N4, oServicio.FACTOR.ToString().Replace(".00",""), Page.User.Identity.Name.ToUpper(), out request, out response);

                    var oTrace = cfs_buscar_tarifas.SaveTrace(ID_AGENTE, oServicio.TIPO, oServicio.INVOICETYPE, oServicio.ID, oServicio.CODIGO_TARIJA_N4, request, response, resp.Exitoso, Page.User.Identity.Name, out v_Error);

                    if (!string.IsNullOrEmpty(v_Error))
                    {
                        this.ImgCarga.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", v_Error));
                        return;
                      
                    }

                    if (resp.Exitoso)
                    {
                        var v_result = resp.MensajeInformacion.Split(',');
                        //Numero_Factura = "902500" + v_result[0];
                        v_monto = decimal.Parse(v_result[1].ToString());

                        Numero_Factura = "00" + v_result[0]; 
                        string Establecimiento = Numero_Factura.Substring(0, 3);
                        string PuntoEmision = Numero_Factura.Substring(3, 3);
                        string Original = Numero_Factura.Substring(6, 9);
                        string FacturaFinal = string.Format("{0}-{1}-{2}", Establecimiento, PuntoEmision, Original);

                        System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                               new System.Xml.Linq.XElement("FACT_CABECERA",
                                                                       new System.Xml.Linq.XElement("CABECERA",
                                                                       new System.Xml.Linq.XAttribute("IV_FECHA", System.DateTime.Now),
                                                                       new System.Xml.Linq.XAttribute("IV_TIPO_CARGA", "CFS"),
                                                                       new System.Xml.Linq.XAttribute("IV_ID_AGENTE", CODIGO_AGENTE == null ? "" : CODIGO_AGENTE),
                                                                       new System.Xml.Linq.XAttribute("IV_DESC_AGENTE", NOMBRES_AGENTE == null ? "" : NOMBRES_AGENTE), 
                                                                       new System.Xml.Linq.XAttribute("IV_ID_FACTURADO", ID_AGENTE == null ? "" : ID_AGENTE),
                                                                       new System.Xml.Linq.XAttribute("IV_DESC_FACTURADO", DESC_AGENTE == null ? "" : DESC_AGENTE),
                                                                       new System.Xml.Linq.XAttribute("IV_SUBTOTAL", oServicio.VALOR),
                                                                       new System.Xml.Linq.XAttribute("IV_IVA", oServicio.VALOR_IVA),
                                                                       new System.Xml.Linq.XAttribute("IV_TOTAL", oServicio.VALOR_TOTAL),
                                                                       new System.Xml.Linq.XAttribute("IV_FACTURA", FacturaFinal),
                                                                       new System.Xml.Linq.XAttribute("IV_NUMERO_CARGA", numeros_cargas),
                                                                       new System.Xml.Linq.XAttribute("IV_USUARIO_CREA", Page.User.Identity.Name.ToUpper()),
                                                                       new System.Xml.Linq.XAttribute("flag", "I"))));

                        System.Xml.Linq.XDocument XMLDetalle = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                              new System.Xml.Linq.XElement("FACT_DETALLE",
                                                                      new System.Xml.Linq.XElement("DETALLE",
                                                                      new System.Xml.Linq.XAttribute("IV_ID_TARIFA", oServicio.ID),
                                                                      new System.Xml.Linq.XAttribute("IV_TIPO", oServicio.TIPO),
                                                                      new System.Xml.Linq.XAttribute("IV_CANTIDAD", LinqCargas.Count),
                                                                      new System.Xml.Linq.XAttribute("IV_FACTOR", oServicio.FACTOR),
                                                                      new System.Xml.Linq.XAttribute("IV_VALOR", v_monto),
                                                                      new System.Xml.Linq.XAttribute("IV_INVOICETYPE", oServicio.INVOICETYPE),
                                                                      new System.Xml.Linq.XAttribute("IV_CODIGO_TARIJA_N4", oServicio.CODIGO_TARIJA_N4),
                                                                      new System.Xml.Linq.XAttribute("IV_USUARIO_CREA", Page.User.Identity.Name.ToUpper()),
                                                                      new System.Xml.Linq.XAttribute("flag", "I"))));

                        objCargasPendientes = new cfs_cargas_pendientes();
                        objCargasPendientes.xmlCabecera = XMLCabecera.ToString();
                        objCargasPendientes.xmlDetalle = XMLDetalle.ToString();

                        var nProceso = objCargasPendientes.SaveTransaction_MultiDespacho(out cMensajes);
                        /*fin de nuevo proceso de grabado*/
                        if (!nProceso.HasValue || nProceso.Value <= 0)
                        {
                            this.ImgCarga.Attributes["class"] = "nover";
                            this.UPBOTONES.Update();
                            this.Id_Factura_Generada = string.Empty;
                            this.hf_idfacturagenerada.Value = string.Empty;
                            this.Mostrar_Mensaje_Factura(string.Format("<b>Error! No se pudo grabar datos de la factura de MultiDespacho..{0}</b>", cMensajes));

                            return;
                        }
                        else
                        {
      
                            objCab.Detalle.Clear();

                            Session["CargasPendientes" + this.hf_BrowserWindowName.Value] = objCab;

                            this.Id_Factura_Generada = nProceso.Value.ToString();

                            this.hf_idfacturagenerada.Value = nProceso.Value.ToString();

                            this.BtnVisualizar.Attributes.Remove("disabled");

                            this.OcultarLoading();

                            this.Mostrar_Mensaje_Factura(string.Format("<b>Se procedió a generar la factura de carga suelta [MULTIDESPACHOS] # {0}  {1} ", FacturaFinal, ",Puede proceder a imprimir su comprobante."));


                            this.ImgCarga.Attributes["class"] = "nover";

                           

                            this.BtnConfirmar.Attributes["disabled"] = "disabled";
                           

                            this.Actualiza_Paneles();

                            // this.BtnVisualizar.Attributes["disabled"] = "disabled";
                            this.Cargas_Pendientes();
                        }

                    }
                    else
                    {
                        this.ImgCarga.Attributes["class"] = "nover";
                        this.UPBOTONES.Update();
                        this.Mostrar_Mensaje_Factura(string.Format("<i class='fa fa-warning'></i><b> Error! No se logró realizar la facturación en N4...{0} </b>", resp.MensajeProblema));
                        return;

                     
                    }

                }
                catch (Exception ex)
                {
                    this.ImgCarga.Attributes["class"] = "nover";
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
                objCab = Session["CargasPendientes" + this.hf_BrowserWindowName.Value] as cfs_cargas_pendientes;
              
                if (objCab == null)
                {
                    this.Mostrar_Mensaje_Factura(string.Format("<b>Informativo! </b>Aún no ha generado la factura de MultiDespacho, para poder imprimirla"));
                    return;
                }

                if (string.IsNullOrEmpty(this.hf_idfacturagenerada.Value))
                {
                    this.Mostrar_Mensaje_Factura(string.Format("<b>Informativo! </b>Aún no ha generado la factura de MultiDespacho, para poder imprimirla"));
                    return;
                }

                //IMPRIMIR FACTURA -FORMATO HTML
                string cId = securetext(this.hf_idfacturagenerada.Value);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('../multidespachoscfs/factura_multi_preview.aspx?id_comprobante=" + cId + "');", true);

            }


        }

        protected void grilla_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
          

        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
          

        }

    
        protected void BtnNuevo_Click(object sender, EventArgs e)
        {

            try
            {
                //this.BtnGrabar.Attributes.Remove("disabled");

           

                //this.TxtFechaDesde.Text = string.Empty;
                //this.TxtFechaHasta.Text = string.Empty;
                //this.CboTipoProducto.SelectedIndex = -1;
                //this.CboBodega.SelectedIndex = -1;
                //this.TxtCapacidad.Text = string.Empty;
                //this.TxtFrecuencia.Text = string.Empty;
                //this.Txtcomentario.Text = string.Empty;

                //this.banmsg.InnerText = string.Empty;
                //this.banmsg.Visible = false;

                //objCab = Session["Turnos" + this.hf_BrowserWindowName.Value] as brbk_turnos_cab;
                //objCab.Detalle.Clear();

                //tablePagination.DataSource = null;
                //tablePagination.DataBind();

                //Session["Turnos" + this.hf_BrowserWindowName.Value] = objCab;

                //this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnNuevo_Click), "brbk_generar_turnos", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }

        }

    
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable table = new DataTable();

            try
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
               
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
                return table;

            }
            catch (Exception)
            {
                throw;
            }

        }



   

    }
}