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
using System.Text.RegularExpressions;
using CSLSite;
using PasePuerta;

namespace CSLSite
{


    public partial class p2d_cotizador : System.Web.UI.Page
    {


        #region "Clases"
        private static Int64? lm = -3;
        private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        
        private P2D_Proforma_Cabecera objProformaCab = new P2D_Proforma_Cabecera();
        private P2D_Proforma_Detalle objProformaDet = new P2D_Proforma_Detalle();
        private P2D_Tarifario objTarifa = new P2D_Tarifario();
        private P2D_Tarja_Cfs objTarja = new P2D_Tarja_Cfs();

        private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;

       
        private int Fila = 1;

        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;

        private int Cantidad_Bultos = 0;

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
            UPRESULTADO.Update();
            UPCALCULAR.Update();
            this.UPAPILABLE.Update();
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
                this.banmsg2.Visible = true;
                this.banmsg.Visible = false;
              
                OcultarLoading("2");
            }

            if (Tipo == 4)//ambos
            {
               
                this.banmsg2.Visible = true;
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                this.banmsg2.InnerHtml = Mensaje;
                OcultarLoading("1");
            }

            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg2.InnerText = string.Empty;

            this.banmsg_det.InnerText = string.Empty;
          
            this.banmsg.Visible = false;
            this.banmsg2.Visible = false;

            this.banmsg_det.Visible = false;
     
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
            objProformaCab = new P2D_Proforma_Cabecera();
            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objProformaCab;
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
                detalle_carga.Tipo = TipoCarga.Contenedores; //opcional
                detalle_carga.Titulo = "Modulo de Facturación Importación"; //opcional
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
                        Mensaje = string.Format("Se ha generado una notificación a nuestra área de Tesorería para la revisión de su caso generado #  {0} ...Para mayor información: Servicio al cliente: ec.sac@contecon.com.ec lunes a domingo 7h00 a 23h00...Tesorería: lunes a viernes 7h00 a 17h30....Teléfonos (04) 6006300 - 3901700", rt.Resultado);

                    }
                    else
                    {
                        Mensaje = string.Format("favor canalizar este mensaje con nuestro personal de facturación para las respectivas revisiones del caso generado #  {0} ...Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00.", rt.Resultado);

                    }
                }
                else
                {
                    if (bloqueo)
                    {
                        Mensaje = string.Format("Se ha generado una notificación a nuestra área de Tesorería  para que realicen las respectivas revisiones del problema {0} ...Para mayor información: Servicio al cliente: ec.sac@contecon.com.ec lunes a domingo 7h00 a 23h00...Tesorería: lunes a viernes 7h00 a 17h30....Teléfonos (04) 6006300 - 3901700", rt.MensajeProblema);

                    }
                    else
                    {
                        Mensaje = string.Format("favor canalizar este mensaje con nuestro personal de facturación para las respectivas revisiones del problema {0} ....Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h00 a 16h30 sábados y domingos 8h00 a 15h00. ", rt.MensajeProblema);
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
                string Error=  string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboCiudad", "Hubo un error al cargar ciudades", t.loginname));
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
                this.banmsg2.Visible = IsPostBack;
                this.banmsg_det.Visible = IsPostBack;
               

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg2.InnerText = string.Empty;

                    this.banmsg_det.InnerText = string.Empty;
                  
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
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
                Server.HtmlEncode(this.TXTHSN.Text.Trim());
                Server.HtmlEncode(this.Txtdireccion.Text.Trim());
                Server.HtmlEncode(this.TxtBultos.Text.Trim());

                Server.HtmlEncode(this.TxtTotalPeso.Text.Trim());
                Server.HtmlEncode(this.TxtTotalVolumen.Text.Trim());
                Server.HtmlEncode(this.TxtTotalPagar.Text.Trim());

                if (!Page.IsPostBack)
                {

                    /*secuencial de sesion*/
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    this.Crear_Sesion();

                    this.Carga_CboCiudad();
                    this.Carga_CboZonas();

                    this.TXTHSN.Text = "0000";

                    List<Cls_Bil_Configuraciones> Leyenda = Cls_Bil_Configuraciones.Parametros(out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                       
                    }
                    else
                    {
                        var LinqLeyenda = (from Tope in Leyenda.Where(Tope => Tope.NOMBRE.Equals("TEXTO_P2D_PRO"))
                                           select new
                                           {
                                               VALOR = Tope.VALOR == null ? string.Empty : Tope.VALOR
                                           }).FirstOrDefault();

                        if (LinqLeyenda != null)
                        {
                            this.mensaje_proforma.InnerHtml= LinqLeyenda.VALOR == null ? "" : LinqLeyenda.VALOR;
                        }
                    }
                    


                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", ex.Message));
            }
        }


        #endregion

        #region "Evento Botones"

        protected void BtnApilable_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {

                    if (this.BtnApilable.Text.Equals("CARGA NO APILABLE"))
                    {
                        this.BtnApilable.Text = "CARGA APILABLE";

                        this.BtnApilable.Attributes["class"] = "btn btn-outline-primary mr-4";

                    }
                    else
                    {
                        this.BtnApilable.Attributes["class"] = "btn btn-primary mr-4";

                        this.BtnApilable.Text = "CARGA NO APILABLE";
                    }

                    this.UPAPILABLE.Update();

                    this.BtnBuscarCarga_Click(sender, e);


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnApilable_Click), "BtnApilable_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
                    return;

                }
            }
        }
        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {

                  

                    this.BtnBuscarCarga_Click(sender, e);

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(ChkTodos_CheckedChanged), "ChkTodos_CheckedChanged", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));
                    return;

                }
            }

        }

        protected void BtnBuscarCarga_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    OcultarLoading("2");

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
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga HSN</b>"));
                        this.TXTHSN.Focus();
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

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objTarja = new P2D_Tarja_Cfs();
                    objTarja.mrn = this.TXTMRN.Text.Trim();
                    objTarja.msn = this.TXTMSN.Text.Trim();
                    objTarja.hsn = this.TXTHSN.Text.Trim();
                    objTarja.ruc = string.IsNullOrEmpty(ClsUsuario.ruc) ? "" : ClsUsuario.ruc.Trim();

                    if (this.BtnApilable.Text.Equals("CARGA NO APILABLE"))
                    {
                        
                        objTarja.apilable = false;
                        this.div_volumen.Visible = true;
                        this.div_volumen.Attributes["class"] = "form-group col-md-2";
                        this.UPVOLUMEN.Update();
                    }
                    else
                    {

                        objTarja.apilable = true;
                        this.div_volumen.Visible = false;
                        this.div_volumen.Attributes["class"] = "form-group col-md-2";
                        this.UPVOLUMEN.Update();
                    }

                    //objTarja.apilable = this.ChkTodos.Checked;

                    if (!objTarja.PopulateMyData(out cMensajes))
                    {
                        this.TxtBultos.Text = string.Empty;
                        this.TxtTotalPeso.Text = string.Empty;
                        this.TxtTotalVolumen.Text = string.Empty;
                        this.TxtTotalPagar.Text = string.Empty;

                        this.hf_ID_TARIFA.Value = string.Empty;
                        this.hf_SECUENCIA.Value = string.Empty;

                        this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo!: {0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                    //valido que no tenga una cotizacion activa
                    objValida = new P2D_Valida_Proforma();
                    objValida.MRN = this.TXTMRN.Text.Trim();
                    objValida.MSN = this.TXTMSN.Text.Trim();
                    objValida.HSN = this.TXTHSN.Text.Trim();
                    objValida.RUC = string.IsNullOrEmpty(ClsUsuario.ruc) ? "" : ClsUsuario.ruc.Trim();
                    if (objValida.PopulateMyData(out cMensajes))
                    {
                        this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo!: {0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }
                    //fin validacion

                    objProformaCab = Session["Transaccion" + this.hf_BrowserWindowName.Value] as P2D_Proforma_Cabecera;
                    objProformaCab.FECHA = DateTime.Now;
                    objProformaCab.RUC_USUARIO = ClsUsuario.ruc;
                    objProformaCab.NOMBRES = ClsUsuario.nombres;
                    objProformaCab.APELLIDOS = ClsUsuario.apellidos;

                    objProformaCab.MRN = this.TXTMRN.Text.Trim();
                    objProformaCab.MSN = this.TXTMSN.Text.Trim();
                    objProformaCab.HSN = this.TXTHSN.Text.Trim();
                    objProformaCab.CANT_BULTOS = int.Parse(objTarja.cantidad.ToString());
                    objProformaCab.USUARIO_CREA = ClsUsuario.loginname;
                    objProformaCab.ID_CIUDAD = Int64.Parse(this.CboCiudad.SelectedValue);
                    objProformaCab.ID_ZONA = Int64.Parse(this.CboZonas.SelectedValue);
                    objProformaCab.DIR_ENTREGA = this.Txtdireccion.Text.Trim();
                    objProformaCab.TOTAL_M3 = objTarja.m3;
                    objProformaCab.TOTAL_TN = objTarja.pesokg;
                    objProformaCab.APILABLE = objTarja.apilable;
                    objProformaCab.EXPRESS = this.ChkExpress.Checked;

                    this.TxtContenedor.Text = objTarja.contenedor;
                    this.TxtBultos.Text = objTarja.cantidad.ToString("N2");
                    this.TxtTotalPeso.Text = objTarja.pesokg.ToString("N2");
                    this.TxtTotalVolumen.Text = objTarja.m3.ToString("N2");

                    objProformaCab.Detalle.Clear();

                    decimal valor = decimal.Parse("0.00");

                    //crea detalle para ser ingresado por pantalla
                    objProformaDet = new P2D_Proforma_Detalle();
                    objProformaDet.SECUENCIA = 1;
                    objProformaDet.PESO = objTarja.pesokg;
                    objProformaDet.LARGO = valor;
                    objProformaDet.ALTO = valor;
                    objProformaDet.ANCHO = valor;
                    objProformaDet.M3 = objTarja.m3;
                    objProformaDet.ESTADO = true;
                    objProformaDet.M3_TEXTO = objTarja.m3.ToString("N2"); 
                    objProformaDet.USUARIO_CREA = ClsUsuario.loginname;
                    objProformaCab.Detalle.Add(objProformaDet);


                    //busco si existe en el tarifario
                    objTarifa = new P2D_Tarifario();
                    objTarifa.M3 = objTarja.m3;
                    objTarifa.PESO = objTarja.pesokg;
                    objTarifa.EXPRESS = this.ChkExpress.Checked;

                    if (!objTarifa.PopulateMyData(out cMensajes))
                    {

                        this.TxtTotalPagar.Text = string.Empty;

                        this.hf_ID_TARIFA.Value = string.Empty;
                        this.hf_SECUENCIA.Value = string.Empty;

                        this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo!: {0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }
                    else
                    {
                        this.TxtTotalPagar.Text = string.Format("{0:c}", objTarifa.TOTAL_PAGAR);
                        this.hf_ID_TARIFA.Value = objTarifa.ID_TARIFA.ToString();
                        this.hf_SECUENCIA.Value = objTarifa.SECUENCIA.ToString();

                        objProformaCab.ID_TARIFA = objTarifa.ID_TARIFA;
                        objProformaCab.ID_TARIFA_SECUEN = objTarifa.SECUENCIA;
                        objProformaCab.TOTAL_PAGAR = objTarifa.TOTAL_PAGAR;
                        objProformaCab.CANT_CALCULAR = objTarifa.VALOR;
                    }

                    Session["Transaccion" + this.hf_BrowserWindowName.Value] = objProformaCab;

                    this.BtnCalcular.Attributes.Remove("disabled");

                    this.Actualiza_Panele_Detalle();

                    this.Ocultar_Mensaje();



                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

                }
            }
        }


      

        protected void BtnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                decimal latitude = 0;
                decimal longitude = 0;

                objProformaCab = Session["Transaccion" + this.hf_BrowserWindowName.Value] as P2D_Proforma_Cabecera;
                if (objProformaCab == null)
                {
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información para generar la proforma</b>"));
                    this.BtnCalcular.Focus();
                    return;
                }
                if (objProformaCab.Detalle.Count == 0)
                {
                    this.Mostrar_Mensaje(1,string.Format("<b>Informativo! </b>Debe generar el detalle de volumen de la carga"));
                    return;
                }

                objProformaCab.ID_CIUDAD = Int64.Parse(this.CboCiudad.SelectedValue);
                objProformaCab.ID_ZONA = Int64.Parse(this.CboZonas.SelectedValue);
                objProformaCab.DIR_ENTREGA = this.Txtdireccion.Text.Trim();
                objProformaCab.EXPRESS = this.ChkExpress.Checked;


                if (!decimal.TryParse(this.TxtLatitud.Text, out latitude))
                {
                    latitude = 0;
                }
                objProformaCab.LATITUD = latitude;

                if (!decimal.TryParse(this.TxtLongitud.Text, out longitude))
                {
                    longitude = 0;
                }

                objProformaCab.LONGITUD = longitude;
                objProformaCab.UBICACION = this.TxtUbicacionEntrega.Text.Trim().ToUpper();

                //proceso de grabar proforma
                /*nuevo proceso de grabado*/
                System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                new System.Xml.Linq.XElement("COT_CABECERA",
                                                        new System.Xml.Linq.XElement("CABECERA",
                                                        new System.Xml.Linq.XAttribute("RUC_USUARIO", objProformaCab.RUC_USUARIO == null ? "" : objProformaCab.RUC_USUARIO),
                                                        new System.Xml.Linq.XAttribute("NOMBRES", objProformaCab.NOMBRES == null ? "" : objProformaCab.NOMBRES),
                                                        new System.Xml.Linq.XAttribute("APELLIDOS", objProformaCab.APELLIDOS == null ? "" : objProformaCab.APELLIDOS),
                                                        new System.Xml.Linq.XAttribute("MRN", objProformaCab.MRN == null ? "" : objProformaCab.MRN),
                                                        new System.Xml.Linq.XAttribute("MSN", objProformaCab.MSN == null ? "" : objProformaCab.MSN),
                                                        new System.Xml.Linq.XAttribute("HSN", objProformaCab.HSN == null ? "" : objProformaCab.HSN),
                                                        new System.Xml.Linq.XAttribute("CANT_BULTOS", objProformaCab.CANT_BULTOS),
                                                        new System.Xml.Linq.XAttribute("ID_CIUDAD", objProformaCab.ID_CIUDAD == null ? null: objProformaCab.ID_CIUDAD),
                                                        new System.Xml.Linq.XAttribute("ID_ZONA", objProformaCab.ID_ZONA == null ? null : objProformaCab.ID_ZONA),
                                                        new System.Xml.Linq.XAttribute("ID_TARIFA", objProformaCab.ID_TARIFA),
                                                        new System.Xml.Linq.XAttribute("ID_TARIFA_SECUEN", objProformaCab.ID_TARIFA_SECUEN),
                                                        new System.Xml.Linq.XAttribute("DIR_ENTREGA", objProformaCab.DIR_ENTREGA == null ? "" : objProformaCab.DIR_ENTREGA),
                                                        new System.Xml.Linq.XAttribute("TOTAL_M3", objProformaCab.TOTAL_M3),
                                                        new System.Xml.Linq.XAttribute("TOTAL_TN", objProformaCab.TOTAL_TN),
                                                        new System.Xml.Linq.XAttribute("CANT_CALCULAR", objProformaCab.CANT_CALCULAR),
                                                        new System.Xml.Linq.XAttribute("TOTAL_PAGAR", objProformaCab.TOTAL_PAGAR),
                                                        new System.Xml.Linq.XAttribute("USUARIO_CREA", objProformaCab.USUARIO_CREA),
                                                        new System.Xml.Linq.XAttribute("LATITUD", objProformaCab.LATITUD == 0 ? 0 : objProformaCab.LATITUD),
                                                        new System.Xml.Linq.XAttribute("LONGITUD", objProformaCab.LONGITUD == 0 ? 0 : objProformaCab.LONGITUD),
                                                        new System.Xml.Linq.XAttribute("UBICACION", objProformaCab.UBICACION == null ? "" : objProformaCab.UBICACION),
                                                        new System.Xml.Linq.XAttribute("APILABLE", objProformaCab.APILABLE ),
                                                         new System.Xml.Linq.XAttribute("EXPRESS", objProformaCab.EXPRESS),
                                                        new System.Xml.Linq.XAttribute("flag", "I"))));

                    
                System.Xml.Linq.XDocument XMLDetalle = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                new System.Xml.Linq.XElement("COT_DETALLE", from p in objProformaCab.Detalle.AsEnumerable().AsParallel()
                                                                            select new System.Xml.Linq.XElement("DETALLE",
                                                                                new System.Xml.Linq.XAttribute("SECUENCIA", p.SECUENCIA),
                                                                                new System.Xml.Linq.XAttribute("PESO", p.PESO),
                                                                                new System.Xml.Linq.XAttribute("LARGO", p.LARGO ),
                                                                                new System.Xml.Linq.XAttribute("ALTO", p.ALTO ),
                                                                                new System.Xml.Linq.XAttribute("ANCHO", p.ANCHO),
                                                                                new System.Xml.Linq.XAttribute("M3", p.M3),
                                                                                new System.Xml.Linq.XAttribute("USUARIO_CREA", p.USUARIO_CREA),
                                                                                new System.Xml.Linq.XAttribute("flag", "I"))));

                objProformaCab.xmlCabecera = XMLCabecera.ToString();
                objProformaCab.xmlDetalle = XMLDetalle.ToString();
                   
                var nProceso = objProformaCab.SaveTransaction(out cMensajes);
                if (!nProceso.HasValue || nProceso.Value <= 0)
                {
                    this.hf_PROFORMA.Value = string.Empty;
                    this.Mostrar_Mensaje(3,string.Format("<b>Error! No se pudo grabar datos de la cotización..{0}</b>", cMensajes));
                }
                else
                {
                    this.BtnImprimir.Attributes.Remove("disabled");

                    this.hf_PROFORMA.Value = nProceso.Value.ToString();

                    this.Mostrar_Mensaje(4, string.Format("<b>Infomrativo! Se genero la proforma No. {0} con éxito, ya puede imprimir la misma</b>", nProceso.Value.ToString()));
                }
                

                this.Actualiza_Panele_Detalle();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

        protected void BtnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                objProformaCab = new P2D_Proforma_Cabecera();
                Session["Transaccion" + this.hf_BrowserWindowName.Value] = objProformaCab;

                this.BtnCalcular.Attributes["disabled"] = "disabled";
                this.BtnImprimir.Attributes["disabled"] = "disabled";
                

                this.TXTMRN.Text = string.Empty;
                this.TXTMSN.Text = string.Empty;
                this.TXTHSN.Text ="0000";
                this.TxtBultos.Text = string.Empty;
                this.TxtContenedor.Text = string.Empty;
                this.TxtTotalVolumen.Text = string.Empty;
                this.TxtTotalPagar.Text = string.Empty;

                this.ChkExpress.Checked = false;

                this.BtnApilable.Attributes["class"] = "btn btn-primary mr-4";

                this.BtnApilable.Text = "CARGA NO APILABLE";

                this.hf_ID_TARIFA.Value = string.Empty;
                this.hf_SECUENCIA.Value = string.Empty;

                this.TxtTotalPeso.Text = string.Empty;
                this.TxtTotalVolumen.Text = string.Empty;
                this.TxtTotalPagar.Text = string.Empty;

                this.hf_ID_TARIFA.Value = string.Empty;
                this.hf_SECUENCIA.Value = string.Empty;
                this.hf_PROFORMA.Value = string.Empty;

                this.Actualiza_Panele_Detalle();

                this.Ocultar_Mensaje();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));
            }

        }


        protected void BtnImprimir_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
               
                objProformaCab = Session["Transaccion" + this.hf_BrowserWindowName.Value] as P2D_Proforma_Cabecera;
                if (objProformaCab == null)
                {
                    this.Mostrar_Mensaje(3,string.Format("<b>Informativo! </b>Aún no ha generado la cotización, para poder imprimirla"));
                    return;
                }

                if (string.IsNullOrEmpty(this.hf_PROFORMA.Value))
                {
                    this.Mostrar_Mensaje(3,string.Format("<b>Informativo! </b>Aún no ha generado la cotización, para poder imprimirla"));
                    return;
                }

                //IMPRIMIR FACTURA -FORMATO HTML
                string cId = securetext(this.hf_PROFORMA.Value);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "popOpen('../reportes/p2d_proforma_preview.aspx?id_comprobante=" + cId + "');", true);

            }


        }




        //acepta el servicio


        #endregion

        #region "Eventos Check"

        #endregion

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }


        protected void ChkExpress_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.ChkExpress.Checked)
                {
                    this.CboZonas.Attributes["disabled"] = "disabled";


                }
                else
                {
                    this.CboZonas.Attributes.Remove("disabled");
                }



                this.UPCARGA.Update();


                if (Response.IsClientConnected)
                {
                    try
                    {
                        OcultarLoading("2");

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
                        if (string.IsNullOrEmpty(this.TXTHSN.Text))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga HSN</b>"));
                            this.TXTHSN.Focus();
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

                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        objTarja = new P2D_Tarja_Cfs();
                        objTarja.mrn = this.TXTMRN.Text.Trim();
                        objTarja.msn = this.TXTMSN.Text.Trim();
                        objTarja.hsn = this.TXTHSN.Text.Trim();
                        objTarja.ruc = string.IsNullOrEmpty(ClsUsuario.ruc) ? "" : ClsUsuario.ruc.Trim();

                        if (this.BtnApilable.Text.Equals("CARGA NO APILABLE"))
                        {

                            objTarja.apilable = false;
                        }
                        else
                        {

                            objTarja.apilable = true;

                        }

                        //objTarja.apilable = this.ChkTodos.Checked;

                        if (!objTarja.PopulateMyData(out cMensajes))
                        {
                            this.TxtBultos.Text = string.Empty;
                            this.TxtTotalPeso.Text = string.Empty;
                            this.TxtTotalVolumen.Text = string.Empty;
                            this.TxtTotalPagar.Text = string.Empty;

                            this.hf_ID_TARIFA.Value = string.Empty;
                            this.hf_SECUENCIA.Value = string.Empty;

                            this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo!: {0}</b>", cMensajes));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                        //valido que no tenga una cotizacion activa
                        objValida = new P2D_Valida_Proforma();
                        objValida.MRN = this.TXTMRN.Text.Trim();
                        objValida.MSN = this.TXTMSN.Text.Trim();
                        objValida.HSN = this.TXTHSN.Text.Trim();
                        objValida.RUC = string.IsNullOrEmpty(ClsUsuario.ruc) ? "" : ClsUsuario.ruc.Trim();
                        if (objValida.PopulateMyData(out cMensajes))
                        {
                            this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo!: {0}</b>", cMensajes));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }
                        //fin validacion

                        objProformaCab = Session["Transaccion" + this.hf_BrowserWindowName.Value] as P2D_Proforma_Cabecera;
                        objProformaCab.FECHA = DateTime.Now;
                        objProformaCab.RUC_USUARIO = ClsUsuario.ruc;
                        objProformaCab.NOMBRES = ClsUsuario.nombres;
                        objProformaCab.APELLIDOS = ClsUsuario.apellidos;

                        objProformaCab.MRN = this.TXTMRN.Text.Trim();
                        objProformaCab.MSN = this.TXTMSN.Text.Trim();
                        objProformaCab.HSN = this.TXTHSN.Text.Trim();
                        objProformaCab.CANT_BULTOS = int.Parse(objTarja.cantidad.ToString());
                        objProformaCab.USUARIO_CREA = ClsUsuario.loginname;
                        objProformaCab.ID_CIUDAD = Int64.Parse(this.CboCiudad.SelectedValue);
                        objProformaCab.ID_ZONA = Int64.Parse(this.CboZonas.SelectedValue);
                        objProformaCab.DIR_ENTREGA = this.Txtdireccion.Text.Trim();
                        objProformaCab.TOTAL_M3 = objTarja.m3;
                        objProformaCab.TOTAL_TN = objTarja.pesokg;
                        objProformaCab.APILABLE = objTarja.apilable;
                        objProformaCab.EXPRESS = this.ChkExpress.Checked;

                        this.TxtContenedor.Text = objTarja.contenedor;
                        this.TxtBultos.Text = objTarja.cantidad.ToString("N2");
                        this.TxtTotalPeso.Text = objTarja.pesokg.ToString("N2");
                        this.TxtTotalVolumen.Text = objTarja.m3.ToString("N2");

                        objProformaCab.Detalle.Clear();

                        decimal valor = decimal.Parse("0.00");

                        //crea detalle para ser ingresado por pantalla
                        objProformaDet = new P2D_Proforma_Detalle();
                        objProformaDet.SECUENCIA = 1;
                        objProformaDet.PESO = objTarja.pesokg;
                        objProformaDet.LARGO = valor;
                        objProformaDet.ALTO = valor;
                        objProformaDet.ANCHO = valor;
                        objProformaDet.M3 = objTarja.m3;
                        objProformaDet.ESTADO = true;
                        objProformaDet.M3_TEXTO = objTarja.m3.ToString("N2");
                        objProformaDet.USUARIO_CREA = ClsUsuario.loginname;
                        objProformaCab.Detalle.Add(objProformaDet);


                        //busco si existe en el tarifario
                        objTarifa = new P2D_Tarifario();
                        objTarifa.M3 = objTarja.m3;
                        objTarifa.PESO = objTarja.pesokg;
                        objTarifa.EXPRESS = this.ChkExpress.Checked;

                        if (!objTarifa.PopulateMyData(out cMensajes))
                        {

                            this.TxtTotalPagar.Text = string.Empty;

                            this.hf_ID_TARIFA.Value = string.Empty;
                            this.hf_SECUENCIA.Value = string.Empty;

                            this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Informativo!: {0}</b>", cMensajes));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }
                        else
                        {
                            this.TxtTotalPagar.Text = string.Format("{0:c}", objTarifa.TOTAL_PAGAR);
                            this.hf_ID_TARIFA.Value = objTarifa.ID_TARIFA.ToString();
                            this.hf_SECUENCIA.Value = objTarifa.SECUENCIA.ToString();

                            objProformaCab.ID_TARIFA = objTarifa.ID_TARIFA;
                            objProformaCab.ID_TARIFA_SECUEN = objTarifa.SECUENCIA;
                            objProformaCab.TOTAL_PAGAR = objTarifa.TOTAL_PAGAR;
                            objProformaCab.CANT_CALCULAR = objTarifa.VALOR;
                        }

                        Session["Transaccion" + this.hf_BrowserWindowName.Value] = objProformaCab;

                        this.BtnCalcular.Attributes.Remove("disabled");

                        this.Actualiza_Panele_Detalle();

                        this.Ocultar_Mensaje();



                    }
                    catch (Exception ex)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

                    }
                }


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", ex.Message));

            }
        }








    }
}