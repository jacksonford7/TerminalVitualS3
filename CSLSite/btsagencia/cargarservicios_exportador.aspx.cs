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
//using PasePuerta;
//using ClsAppCgsa;

using System.Web.Services;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CSLSite
{


    public partial class cargarservicios_exportador : System.Web.UI.Page
    {


        #region "Clases"
        private static Int64? lm = -3;
        private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
       

     
      
        private BTS_Exportador objExportador = new BTS_Exportador();
       

        //eventos
        private BTS_Cabecera_Eventos objCabeceraEvents = new BTS_Cabecera_Eventos();
        private BTS_Detalle_Eventos objDetalleEvents = new BTS_Detalle_Eventos();
        private BTS_Cabecera_Eventos objRegistrarEvents = new BTS_Cabecera_Eventos();
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
         
            
            UPBOTONES.Update();
            this.UPBOTONESBUSCADOR.Update();
           

            UPDET_REFERENCIA.Update();
            this.UPPIE_REFERENCIA.Update();
            this.UPTIT_REFERENCIA.Update();

        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
            UPCARGA.Update();
        }

        private void Limpia_Datos()
        {
            this.CboEvento.SelectedIndex = -1;
            this.TxtIdExportador.Text = string.Empty;
            this.TxtDescExportador.Text = string.Empty;
            this.TxtCantidad.Text = string.Empty;
            this.Txtcomentario.Text = string.Empty;
          

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
               
                this.banmsg_det.Visible = false;
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;

             

                OcultarLoading("1");
            }
            if (Tipo == 2)//detalle
            {
               
                this.banmsg_det.Visible = true;
                this.banmsg.Visible = false;
                this.banmsg_det.InnerHtml = Mensaje;
                
                OcultarLoading("2");
            }

            if (Tipo == 3)//alerta
            {
                this.banmsg_det.Visible = false;
                this.banmsg.Visible = false;
            
               
                OcultarLoading("2");
            }

            if (Tipo == 4)//ambos
            {
               
                this.banmsg_det.Visible = true;
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                this.banmsg_det.InnerHtml = Mensaje;
             
                OcultarLoading("1");
            }

            if (Tipo == 5)//rubros
            {
               
                this.banmsg_det.Visible = false;
                this.banmsg.Visible = false;
                this.banmsg.InnerHtml = string.Empty;
                this.banmsg_det.InnerHtml = string.Empty;

              
                OcultarLoading("1");
            }

            if (Tipo == 6)//referencia
            {
             
                this.banmsg_det.Visible = false;
                this.banmsg.Visible = false;
              
                this.banmsg.InnerHtml = string.Empty;
                this.banmsg_det.InnerHtml = string.Empty;
               

                this.banmsg_buscador_referencia.Visible = true;
                this.banmsg_buscador_referencia.InnerHtml = Mensaje;

                OcultarLoading("1");
            }

            if (Tipo == 7)//exportador
            {
               
                this.banmsg_det.Visible = false;
                this.banmsg.Visible = false;
                this.banmsg.InnerHtml = string.Empty;
                this.banmsg_det.InnerHtml = string.Empty;

                this.banmsg_buscador.Visible = true;
                this.banmsg_buscador.InnerHtml = Mensaje;

                OcultarLoading("1");
            }

            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
           
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
          
           
            this.banmsg_buscador_referencia.Visible = false;
            this.banmsg_buscador_referencia.InnerText = string.Empty;

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
            objCabeceraEvents = new BTS_Cabecera_Eventos();
            Session["Events" + this.hf_BrowserWindowName.Value] = objCabeceraEvents;

          

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
                tk.Categoria = "EXPO"; //solo puede ser: Impo,Expo,Otros
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
                

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;
                   
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


                    //objFacturaBTS = new BTS_Prev_Cabecera();
                    //Session["InvoiceBTS" + this.hf_BrowserWindowName.Value] = objFacturaBTS;

                    //objCabeceraBTS_OTROS = new BTS_OTROS_Cabecera_Exportadores();
                    //Session["TransaccionBTS_OTROS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS_OTROS;

                    //eventos
                    objCabeceraEvents = new BTS_Cabecera_Eventos();
                    Session["Events" + this.hf_BrowserWindowName.Value] = objCabeceraEvents;


                    this.Cargar_Referencias();

                    this.Cargar_Tarifas();

                    this.Cargar_Exportador();

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
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Delete")
                    {
                        objCabeceraEvents = Session["Events" + this.hf_BrowserWindowName.Value] as BTS_Cabecera_Eventos;

                        if (objCabeceraEvents == null)
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder eliminar los eventos</b>"));
                            return;
                        }
                        else
                        {
                            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                            Int64 SECUENCIA = 0;

                            if (!Int64.TryParse(t, out SECUENCIA))
                            {
                                SECUENCIA = 0;
                            }

                            objRegistrarEvents = new BTS_Cabecera_Eventos();
                            objRegistrarEvents.id = SECUENCIA;
                            objRegistrarEvents.usuario_mod = ClsUsuario.loginname;

                            objRegistrarEvents.Delete(out cMensajes);
                            if (cMensajes != string.Empty)
                            {

                                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b> {0}", cMensajes));
                                return;
                            }
                            else
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Se elimino el evento # {0} con éxito.", SECUENCIA));

                            }

                            this.Listar_Eventos();

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
                Button BtnQuitar = e.Item.FindControl("BtnQuitar") as Button;

                if (!string.IsNullOrEmpty(LblFactura.Text))
                {

                    BtnQuitar.Attributes["disabled"] = "disabled";
                }
               
            }
        }

        #endregion

        #region "Cargar Tarifas"
        protected void Cargar_Tarifas()
        {

            try
            {
                string v_mensaje = string.Empty;

                var lookup = BTS_Tarifas.Carga_Tarifas( out v_mensaje);

                if (lookup != null && lookup.Count > 0)
                {

                    this.CboEvento.DataSource = lookup;
                    this.CboEvento.DataTextField = "TARIFA";
                    this.CboEvento.DataValueField = "ID_SERVICIO";
                    this.CboEvento.DataBind();
                  
                }
                else
                {
                    this.CboEvento.DataSource = null;
                    this.CboEvento.DataBind();
                }
            }
            catch (Exception ex)
            {

                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

            }

        }

        #endregion

        #region "Eventos buscador referencias"

        protected void Cargar_Referencias()
        {

            try
            {
                string v_mensaje = string.Empty;

                var lookup = BTS_Referencias.Buscador_Referencia("", out v_mensaje);

                if (lookup != null && lookup.Count > 0)
                {
                    this.tablePaginationReferencias.DataSource = lookup;
                    this.tablePaginationReferencias.DataBind();

                    banmsg_buscador_referencia.InnerText = string.Empty;
                    banmsg_buscador_referencia.Visible = false;
                    UPDET_REFERENCIA.Update();
                }
            }
            catch (Exception ex)
            {

                this.Mostrar_Mensaje(6, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

            }

        }

        protected void BtnFiltrarRef_Click(object sender, EventArgs e)
        {

            try
            {

                string v_mensaje = string.Empty;

                var lookup = BTS_Referencias.Buscador_Referencia(TxtFiltraReferencia.Text, out v_mensaje);

                if (lookup != null && lookup.Count > 0)
                {
                    this.tablePaginationReferencias.DataSource = lookup;
                    this.tablePaginationReferencias.DataBind();

                    banmsg_buscador_referencia.InnerText = string.Empty;
                    banmsg_buscador_referencia.Visible = false;
                    UPDET_REFERENCIA.Update();
                }
            }
            catch (Exception ex)
            {

                this.Mostrar_Mensaje(6, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

            }

        }

        protected void tablePaginationReferencias_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                        this.Mostrar_Mensaje(6, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(6, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Ver")
                    {

                        string ID_REFERENCIA = t.ToString();
                        string v_mensaje = string.Empty;

                        this.TXTMRN.Text = ID_REFERENCIA;
                        this.UPCARGA.Update();

                        this.Listar_Eventos();


                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

                }
            }
        }


        #endregion

        #region "Eventos buscador Exportador"
        protected void Cargar_Exportador()
        {

            try
            {
                string v_mensaje = string.Empty;

                var lookup = BTS_Exportador.Cargar_Exportador("", out v_mensaje);

                if (lookup != null && lookup.Count > 0)
                {
                    this.tablePaginationBuscador.DataSource = lookup;
                    this.tablePaginationBuscador.DataBind();

                    banmsg_buscador.InnerText = string.Empty;
                    banmsg_buscador.Visible = false;
                    UPBUSCADOR.Update();
                }
            }
            catch (Exception ex)
            {

                this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

            }

        }

        protected void find_Click(object sender, EventArgs e)
        {

            try
            {

                string v_mensaje = string.Empty;

                var lookup = BTS_Exportador.Cargar_Exportador(txtfinder.Text, out v_mensaje);

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
                        this.Mostrar_Mensaje(7, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(7, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
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

                            this.TxtIdExportador.Text = t;
                            this.TxtDescExportador.Text = objExportador.nombre;
                            
                        }
                        else
                        {
                            this.TxtIdExportador.Text = string.Empty;
                            this.TxtDescExportador.Text = string.Empty;

                            this.Mostrar_Mensaje(4, string.Format("<b>Error! No se pudo encontrar información del exportador para actualizar: {0} </b>", t.ToString()));
                            return;
                        }

                        this.UPCARGA.Update();

                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(7, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

                }
            }
        }
        #endregion

        #region "Eventos cargar detalle de eventos por referencia"

        protected void Listar_Eventos()
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    
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

                  
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();


                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdAgenteCodigo = string.Empty;



                    /*detalle de exportadores*/
                    List<BTS_Detalle_Eventos> ListExportadores = BTS_Detalle_Eventos.Listado_Eventos(this.TXTMRN.Text.Trim(), out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener detalle de exportadores....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                   
                    if (ListExportadores != null)
                    {


                        //agrego todos los contenedores a la clase cabecera
                        objCabeceraEvents = Session["Events" + this.hf_BrowserWindowName.Value] as BTS_Cabecera_Eventos;
                        objCabeceraEvents.Detalle_Eventos.Clear();

                        Int16 Secuencia = 1;

                        foreach (var Det in ListExportadores)
                        {
                            objDetalleEvents = new BTS_Detalle_Eventos();

                            objDetalleEvents.Fila = Det.Fila;
                            objDetalleEvents.id = Det.id;
                            objDetalleEvents.referencia = Det.referencia;
                            objDetalleEvents.fecha = Det.fecha;
                            objDetalleEvents.id_servicio = Det.id_servicio;
                            objDetalleEvents.VALUE_TARIJA_N4 = Det.VALUE_TARIJA_N4;
                            objDetalleEvents.DESCRIP_TARIFA_N4 = Det.DESCRIP_TARIFA_N4;
                            objDetalleEvents.tarifas = Det.tarifas;
                            objDetalleEvents.valor = Det.valor;
                            objDetalleEvents.id_exportador = Det.id_exportador;
                            objDetalleEvents.ruc = Det.ruc;
                            objDetalleEvents.desc_exportador = Det.desc_exportador;
                            objDetalleEvents.linea = Det.linea;
                            objDetalleEvents.cajas = Det.cajas;
                            objDetalleEvents.comentario = Det.comentario;
                            objDetalleEvents.usuario_reg = Det.usuario_reg;
                            objDetalleEvents.numero_factura = Det.numero_factura;
                            objCabeceraEvents.Detalle_Eventos.Add(objDetalleEvents);

                            Secuencia++;
                        }

                        tablePagination.DataSource = objCabeceraEvents.Detalle_Eventos;
                        tablePagination.DataBind();

                        var TotalCajas = objCabeceraEvents.Detalle_Eventos.Sum(x => x.cajas);
                      
                        this.LabelTotal.InnerText = string.Format("DETALLE DE EVENTOS EXPORTADORES - Total Cajas: {0}", TotalCajas);

                        Session["Events" + this.hf_BrowserWindowName.Value] = objCabeceraEvents;

                     

                        this.Actualiza_Panele_Detalle();



                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

 
             
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



        #endregion



        #region "Evento Botones"



       

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect("~/btsagencia/cargarservicios_exportador.aspx", false);



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

      

        //proceso de grabar eventos
        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

              
                string v_mensaje = string.Empty;
                string facturas = string.Empty;

                try
                {

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

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

                    if (CboEvento.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor seleccionar el evento a registrar</b>"));
                        this.CboEvento.Focus();
                        return;
                    }

                    if (this.CboEvento.Items.Count == 0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el evento a registrar</b>"));
                        this.CboEvento.Focus();
                        return;
                    }


                    if (string.IsNullOrEmpty(this.TxtIdExportador.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor seleccionar el exportador</b>"));
                        this.TxtIdExportador.Focus();
                        return;
                    }

                    int IdExportador = 0;
                    if (!int.TryParse(this.TxtIdExportador.Text,  out IdExportador))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor seleccionar el exportador</b>"));
                        this.TxtIdExportador.Focus();
                        return;
                    }

                    int Cajas = 0;
                    if (!int.TryParse(this.TxtCantidad.Text, out Cajas))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar la cantidad de cajas a registrar</b>"));
                        this.TxtCantidad.Focus();
                        return;
                    }


                    objCabeceraEvents = Session["Events" + this.hf_BrowserWindowName.Value] as BTS_Cabecera_Eventos;

                    if (objCabeceraEvents == null)
                    {
                        this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder registrar los eventos</b>"));
                        return;
                    }
                    else
                    {
                        LoginName = ClsUsuario.loginname;

                        objCabeceraEvents.referencia = TXTMRN.Text.Trim();
                        objCabeceraEvents.id_servicio = Int64.Parse(this.CboEvento.SelectedValue.ToString());
                        objCabeceraEvents.id_exportador = IdExportador;
                        objCabeceraEvents.desc_exportador = this.TxtDescExportador.Text.Trim();
                        objCabeceraEvents.cajas = Cajas;
                        objCabeceraEvents.comentario = this.Txtcomentario.Text;
                        objCabeceraEvents.usuario_reg = ClsUsuario.loginname;

                        cMensajes = string.Empty;
                        string cMensajeActualizados = string.Empty;

                        objRegistrarEvents = new BTS_Cabecera_Eventos();
                        objRegistrarEvents.referencia = TXTMRN.Text.Trim();
                        objRegistrarEvents.id_servicio = Int64.Parse(this.CboEvento.SelectedValue.ToString());
                        objRegistrarEvents.id_exportador = IdExportador;
                        objRegistrarEvents.desc_exportador = this.TxtDescExportador.Text.Trim();
                        objRegistrarEvents.cajas = Cajas;
                        objRegistrarEvents.comentario = this.Txtcomentario.Text;
                        objRegistrarEvents.usuario_reg = ClsUsuario.loginname;

                        var nProceso = objRegistrarEvents.SaveTransaction_Eventos(out cMensajes);
                        /*fin de nuevo proceso de grabado*/
                        if (!nProceso.HasValue || nProceso.Value <= 0)
                        {

                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo grabar datos del evento..{0}</b>", cMensajes));


                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            * **********************************************************************************************************************************/

                            this.Enviar_Caso_Salesforce(ClsUsuario.loginname, ClsUsuario.ruc, "Registrar Eventos de Exportador (BTS)", "Error al grabar", cMensajes, objCabeceraEvents.referencia, objCabeceraEvents.desc_exportador, objCabeceraEvents.comentario, out MensajeCasos);

                            return;
                        }
                        else
                        {

                            this.Limpia_Datos();
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Se registro el evento # {0} al exportador {1} con éxito</b>", nProceso, objCabeceraEvents.desc_exportador));

                        }


                        this.Actualiza_Panele_Detalle();

                        this.Listar_Eventos();

                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Eventos BTS Exportador", false, null, null, ex.StackTrace, ex);
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


        #region "Exportar a Excel"

        public static byte[] CreateExcelBytesFromStoredProcedure(string pReferencia)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BILLION"].ConnectionString;

            DataTable DtEventos = new DataTable();
           

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("BTS_LISTADO_EVENTOS_REGISTRADOS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@REFERENCIA", pReferencia);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DtEventos.Load(reader);
                    }
                }
            }

            Color colorPlaneado = ColorTranslator.FromHtml("#59B653");
            Color colorReservado = ColorTranslator.FromHtml("#5AABD9");
            Color colorDisponible = ColorTranslator.FromHtml("#D9DB63");

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");


                worksheet.Cells["A5:A5"].Value = string.Format("REFERENCIA: {0}" , pReferencia);
                worksheet.Cells["A5:A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:A5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A5:A5"].Style.Fill.BackgroundColor.SetColor(colorPlaneado);
                worksheet.Cells["A5:A5"].Style.Font.Color.SetColor(System.Drawing.Color.White);


                int r = 6;
                int c = 1;
                foreach (DataColumn column in DtEventos.Columns)  //printing column headings
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


                for (eachRow = 0; eachRow < DtEventos.Rows.Count;)
                {
                    new_col = 1;
                    col = 0;
                    foreach (DataColumn column in DtEventos.Columns)
                    {

                        worksheet.Cells[r, new_col].Value = DtEventos.Rows[fila][col];
                        col++;
                        new_col++;
                    }

                    eachRow++;
                    r++;
                    fila++;

                }

                r = r + 2;


                worksheet.Cells.AutoFitColumns();
                return excelPackage.GetAsByteArray();
            }

        }

        [WebMethod]
        public static void ExportarExcel(string pReferencia)
        {
            try
            {
                byte[] excelBytes = CreateExcelBytesFromStoredProcedure(pReferencia);

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
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.StatusDescription = "Error al exportar el archivo Excel";


            }
        }
        #endregion





    }
}