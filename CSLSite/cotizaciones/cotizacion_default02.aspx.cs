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

namespace CSLSite
{


    public partial class cotizacion_default02 : System.Web.UI.Page
    {


        #region "Clases"
       
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

     
        private Cls_Bil_Proforma_Cabecera objProforma = new Cls_Bil_Proforma_Cabecera();
        private Cls_Bil_Proforma_Detalle objDetalleProforma = new Cls_Bil_Proforma_Detalle();
        private Cls_Bil_Proforma_Servicios objServicios = new Cls_Bil_Proforma_Servicios();

        

        private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        private Cls_Bil_Detalle objDetalle = new Cls_Bil_Detalle();
       
        private List<Cls_Bil_AsumeFactura> List_Asume { set; get; }
        private List<Cls_Bil_Contenedor_DiasLibres> List_Contenedor { set; get; }

        private List<Cls_Bil_Cas_Manual> List_Autorizacion { set; get; }


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
        private int Fila = 1;
        private string TipoServicio = string.Empty;
        private DateTime FechaFactura;
        private string HoraHasta = "00:00";
        private string LoginName = string.Empty;
        private int NDiasLibreas = 0;
       
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
       
        private string gkeyBuscado = string.Empty;

        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
        private bool SinDesconsolidar = false;
        private bool SinAutorizacion = false;
        private bool Bloqueos = false;
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
            UPAGREGAR.Update();
            UPBOTONES.Update();

        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
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
            
            objSesion.USUARIO_CREA = "CGSA";
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCabecera = new Cls_Bil_Cabecera();
            Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;
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
                        Mensaje = string.Format("se envió una notificación a nuestro personal de Tesorería  para que realicen las respectivas revisiones del caso generado #  {0} ...Casilla de atención: tesoreia@cgsa.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 3,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.", rt.Resultado);

                    }
                    else
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del caso generado #  {0} ...Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.", rt.Resultado);

                    }
                }
                else
                {
                    if (bloqueo)
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de Tesorería  para que realicen las respectivas revisiones del problema {0} ...Casilla de atención: tesoreia@cgsa.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 3,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.", rt.MensajeProblema);

                    }
                    else
                    {
                        Mensaje = string.Format("se envió una notificación a nuestro personal de facturación para que realicen las respectivas revisiones del problema {0} ....Casilla de atención: ec.fac@contecon.com.ec,  Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00. ", rt.MensajeProblema);
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
               

              

                this.banmsg.Visible = IsPostBack;
                this.banmsg_det.Visible = IsPostBack;
                this.banmsg_Pase.Visible = IsPostBack;

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;
                    this.banmsg_Pase.InnerText = string.Empty;
                }

               
               
               
                    if (!Page.IsPostBack)
                    {
                        /*para almacenar clientes que asumen factura*/
                        List_Asume = new List<Cls_Bil_AsumeFactura>();
                        List_Asume.Clear();
                        List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = string.Empty, nombre = string.Empty, mostrar = string.Format("{0}", "Seleccionar...") });

                        this.TXTMRN.Text = string.Empty;
                        this.TXTMSN.Text = string.Empty;
                        if (string.IsNullOrEmpty(this.TXTHSN.Text))
                        { this.TXTHSN.Text = string.Format("{0}", "0000"); }
                        this.TXTCLIENTE.Text = string.Empty;
                        this.TXTASUMEFACTURA.Text = string.Empty;
                        this.TXTAGENCIA.Text = string.Empty;
                        this.CboAsumeFactura.Items.Clear();

                        this.CboAsumeFactura.DataSource = List_Asume;
                        this.CboAsumeFactura.DataTextField = "mostrar";
                        this.CboAsumeFactura.DataValueField = "ruc";
                        this.CboAsumeFactura.DataBind();
                        this.Actualiza_Paneles();

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
               

                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
                Server.HtmlEncode(this.TXTHSN.Text.Trim());

                if (!Page.IsPostBack)
                {
                    /*secuencial de sesion*/
                   
                    this.Crear_Sesion();
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
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

        protected void tablePagination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                


                    //objCabecera = Session["Transaccion"] as Cls_Bil_Cabecera;
                    objCabecera = Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;

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
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }
        }

        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {

           


                try
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        
                        string row_estado_ridt = DataBinder.Eval(e.Row.DataItem, "ESTADO_RDIT").ToString();
                        string row_autorizado = DataBinder.Eval(e.Row.DataItem, "AUTORIZADO").ToString();
                        DateTime? row_fechasalida = (DateTime?)DataBinder.Eval(e.Row.DataItem, "FECHA_HASTA");
                        string row_contenedor = DataBinder.Eval(e.Row.DataItem, "CONTENEDOR").ToString();
                        string row_bloqueo = DataBinder.Eval(e.Row.DataItem, "DES_BLOQUEO").ToString();

                        CheckBox Chk = (CheckBox)e.Row.FindControl("CHKFA");
                        string row_pase = DataBinder.Eval(e.Row.DataItem, "NUMERO_PASE_N4").ToString();
                        string row_inout = DataBinder.Eval(e.Row.DataItem, "IN_OUT").ToString();


                        if (!row_estado_ridt.Equals("A"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.PaleVioletRed;
                            //this.BtnFacturar.Attributes.Add("disabled", "disabled");
                        }

                        if (row_inout.Equals("OUT"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Peru;
                            this.BtnCotizar.Attributes.Add("disabled", "disabled");
                        }

                        if (!string.IsNullOrEmpty(row_pase))
                        {
                            e.Row.ForeColor = System.Drawing.Color.Green;

                        }

                        if (row_autorizado.Equals("NO"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.OrangeRed;
                          
                            //this.BtnFacturar.Attributes.Add("disabled", "disabled");
                            //this.BtnFacturar.Attributes.Remove("disabled");
                        }

                        if (row_bloqueo.Equals("SI"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.IndianRed;  
                            //this.BtnFacturar.Attributes.Add("disabled", "disabled");
                            //this.BtnFacturar.Attributes.Remove("disabled");
                        }

                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

                }
            
        }
        #endregion

        #region "Evento Botones"
        //revisando 1
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

           
                try
                {
                    string ruc_contecon = System.Configuration.ConfigurationManager.AppSettings["ruc_cgsa"];

                    this.BtnCotizar.Attributes.Remove("disabled");
                    OcultarLoading("2");
                    bool Ocultar_Mensaje = true;
                    bool cancelado = false;
                  

                    this.LabelTotal.InnerText = string.Format("DETALLE DE CARGA SUELTA (CFS) ");
                   

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
                  
                    var LinqDiasLibres = (from Dias in Parametros.Where(Dias => Dias.NOMBRE.Equals("DIASLIBRES"))
                                             select new
                                             {
                                                 VALOR = Dias.VALOR==null ? string.Empty : Dias.VALOR
                                             }).FirstOrDefault();

                    if (LinqDiasLibres != null)
                    {
                        NDiasLibreas = int.Parse(LinqDiasLibres.VALOR);
                    }
                    else { NDiasLibreas = 0; }

                    /***************************fin de dias libres***************************/


                   /*para almacenar clientes que asumen factura*/
                    List_Asume = new List<Cls_Bil_AsumeFactura>();
                    List_Asume.Clear();
                    List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = string.Empty, nombre = string.Empty, mostrar = string.Format("{0}", "Seleccionar...") });

                //busca contenedores por ruc de usuario
                    
                    string IdAgenteCodigo = string.Empty;
                    var AgenteCod = N4.Entidades.Agente.ObtenerAgentePorRuc("CGSA", ruc_contecon);
                    if (AgenteCod.Exitoso)
                    {
                        var ListaAgente = AgenteCod.Resultado;
                        if (ListaAgente != null)
                        {
                            IdAgenteCodigo = ListaAgente.codigo;
                        }
                    }

                    var Validacion = new Aduana.Importacion.ecu_validacion_cntr_cfs();
                    var EcuaContenedores = Validacion.CargaPorManifiestoImpoPro("CGSA", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), ruc_contecon);//ecuapas contenedores
                    if (EcuaContenedores.Exitoso)
                    {
                        //DATOS DEL AGENTE PARA BUSCAR INFORMACION
                        var LinqAgente = (from Tbl in EcuaContenedores.Resultado.Where(Tbl => Tbl.gkey != null)
                                            select new
                                            {
                                                ID_AGENTE = Tbl.agente_id,
                                                ID_CLIENTE = Tbl.importador_id,
                                                DESC_CLIENTE = (Tbl.importador==null ? string.Empty : Tbl.importador)
                                            }).FirstOrDefault();

                        this.hf_idagente.Value = LinqAgente.ID_AGENTE;
                        this.hf_idcliente.Value = LinqAgente.ID_CLIENTE;
                        this.hf_idasume.Value = LinqAgente.ID_CLIENTE;

                        this.hf_rucagente.Value = string.Empty;
                        this.hf_descagente.Value = string.Empty;
                        this.hf_descasume.Value = string.Empty;
                        this.hf_desccliente.Value = LinqAgente.DESC_CLIENTE;

                        //LLENADO DE CAMPOS DE PANTALLA DEL AGENTE
                        var Agente = N4.Entidades.Agente.ObtenerAgente("CGSA", this.hf_idagente.Value);
                        if (Agente.Exitoso)
                        {
                            var ListaAgente = Agente.Resultado;
                            if (ListaAgente != null)
                            {
                                this.TXTAGENCIA.Text = string.Format("{0} - {1}", ListaAgente.ruc.Trim(), ListaAgente.nombres.Trim());
                                this.hf_rucagente.Value = ListaAgente.ruc.Trim();
                                this.hf_descagente.Value = ListaAgente.nombres.Trim();

                               
                            }
                            else
                            {
                                this.TXTAGENCIA.Text = string.Empty;
                               // this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del agente</b>"));
                              //  return;
                            }
                        }
                        else
                        {
                            this.TXTAGENCIA.Text = string.Empty;
                            //this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del agente, {0}</b>", Agente.MensajeProblema));
                            //return;
                        }
                        //LLENADO DE CAMPOS DE PANTALLA DEL CLIENTE
                        var Cliente = N4.Entidades.Cliente.ObtenerCliente("CGSA", this.hf_idcliente.Value);
                        if (Cliente.Exitoso)
                        {
                            var ListaCliente = Cliente.Resultado;
                            if (ListaCliente != null)
                            {
                                this.TXTCLIENTE.Text = string.Format("{0} - {1}", ListaCliente.CLNT_CUSTOMER.Trim(), ListaCliente.CLNT_NAME.Trim());
                                this.TXTASUMEFACTURA.Text = ListaCliente.CLNT_CUSTOMER.Trim() + " - " + ListaCliente.CLNT_NAME.Trim();
                                this.hf_desccliente.Value = ListaCliente.CLNT_NAME.Trim();
                                this.hf_descasume.Value = ListaCliente.CLNT_NAME.Trim();
                                //agrega importador
                               // List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = ListaCliente.CLNT_CUSTOMER.Trim(), nombre = ListaCliente.CLNT_NAME.Trim(), mostrar = string.Format("{0} - {1}", ListaCliente.CLNT_CUSTOMER.Trim(), ListaCliente.CLNT_NAME.Trim()) });
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
                            //List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = this.hf_idcliente.Value, nombre = this.hf_desccliente.Value, mostrar = string.Format("{0} - {1}", this.hf_idcliente.Value, this.hf_desccliente.Value) });
                            Ocultar_Mensaje = false;
                           
                            //this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.fac@contecon.com.ec'>ec.fac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", this.hf_idcliente.Value, this.hf_desccliente.Value));


                            /*************************************************************************************************************************************
                            * crear caso salesforce
                            ***********************************************************************************************************************************/
                            MensajesErrores = string.Format("Se presentaron los siguientes problemas: No existe información del cliente, no registrado: {0}", this.hf_idcliente.Value);

                            this.Enviar_Caso_Salesforce("CGSA", ruc_contecon, "Facturación CFS", "No existe información del cliente, no registrado.", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()),
                                this.hf_desccliente.Value, this.hf_rucagente.Value, out MensajeCasos, false);

                            /*************************************************************************************************************************************
                            * fin caso salesforce
                            **************************************************************************************************************************************/

                        }

                        /*verifica si la carga tiene mas personas que van asumir la carga*/
                        var Resultado = PagoAsignado.ListaAsignacionPartida("CGSA", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());
                        if (Resultado != null)
                        {
                            if (Resultado.Exitoso)
                            {
                                var LinqQuery = from Tbl in Resultado.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.ruc))
                                                select new
                                                {
                                                    ruc = Tbl.ruc,
                                                    nombre = Tbl.nombre,
                                                    mostrar = string.Format("{0} - {1}", Tbl.ruc, Tbl.nombre)
                                                };
                                foreach (var Items in LinqQuery)
                                {
                                   // List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = Items.ruc.Trim(), nombre = Items.nombre.Trim(), mostrar = Items.mostrar });
                                }

                            }
                            
                        }

                        
                        List_Asume.Add(new Cls_Bil_AsumeFactura { ruc = ruc_contecon, nombre = "CGSA", mostrar = string.Format("{0} - {1}", ruc_contecon, "CGSA") });

                        this.CboAsumeFactura.DataSource = List_Asume;
                        this.CboAsumeFactura.DataTextField = "mostrar";
                        this.CboAsumeFactura.DataValueField = "ruc";
                        this.CboAsumeFactura.DataBind();
                        this.CboAsumeFactura.SelectedIndex = 1;
                    //fin asume factura


                    //INFORMACION DEL CONTENEDOR
                    var Contenedor = new N4.Importacion.container_cfs();
                        var ListaContenedores = Contenedor.CargaPorBL("CGSA", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());//resultado de entidad contenedor y cfs
                        if (ListaContenedores.Exitoso)
                        {
                            

                            //valida si tiene ubicacion la carga, si esta desconsolidada.(verdadero=es carga cfs, false=no esta desconsolidada)
                            var LinqTarja = Aduana.Importacion.ecu_validacion_cntr_cfs.EsCargaCFS(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());
                            if (LinqTarja.Value)
                            {
                                SinDesconsolidar = true;
                            }
                            else { SinDesconsolidar = false; }

                            
                            List_Autorizacion = new List<Cls_Bil_Cas_Manual>();
                            List_Autorizacion.Clear();
                           

                            //autorizacion de salida de la carga cfs
                            var Autorizacion = CasBBK.ListaCasPartida("CGSA", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim());
                            if (Autorizacion.Exitoso)
                            {
                                var LinqAut = (from Tbl in Autorizacion.Resultado.Where(Tbl => Tbl.activo)
                                                        select new
                                                        {
                                                            CARGA = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn),
                                                            FECHA_AUTORIZACION = Tbl.fecha_registro,
                                                            AUTORIZADO = true,
                                                            USUARIO_AUTORIZA = (Tbl.usuario_libera == null ? string.Empty : Tbl.usuario_libera),
                                                            CONSIGNATARIO = (Tbl.consignatario_manifiesto == null ? string.Empty : Tbl.consignatario_manifiesto)
                                                        }).FirstOrDefault();

                                List_Autorizacion.Add(new Cls_Bil_Cas_Manual { CARGA = LinqAut.CARGA, FECHA_AUTORIZACION = LinqAut.FECHA_AUTORIZACION, AUTORIZADO = LinqAut.AUTORIZADO, USUARIO_AUTORIZA = LinqAut.USUARIO_AUTORIZA, CONSIGNATARIO = LinqAut.CONSIGNATARIO });

                                SinAutorizacion = true;
                            }
                            else
                            {
                                List_Autorizacion.Add(new Cls_Bil_Cas_Manual { CARGA = string.Format("{0}-{1}-{2}", this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim()), FECHA_AUTORIZACION = (DateTime?)null, AUTORIZADO = false, USUARIO_AUTORIZA=string.Empty, CONSIGNATARIO=string.Empty });
                                SinAutorizacion = false;
                            }

                            var LinqAutorizacion = (from Tbl in List_Autorizacion
                                                    select new
                                                    {
                                                        CARGA = Tbl.CARGA,
                                                        FECHA_AUTORIZACION = Tbl.FECHA_AUTORIZACION,
                                                        AUTORIZADO = Tbl.AUTORIZADO,
                                                        USUARIO_AUTORIZA = Tbl.USUARIO_AUTORIZA,
                                                        CONSIGNATARIO = Tbl.CONSIGNATARIO 
                                                    });


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
                                                    CARGA = string.Format("{0}-{1}-{2}", Tbl.mrn, Tbl.msn, Tbl.hsn)
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

                            /*ultima factura*/
                            List<Cls_Bil_Invoice_Ultima_Factura> ListUltimaFactura = Cls_Bil_Invoice_Ultima_Factura.List_Ultima_Factura_cfs(this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" + this.TXTHSN.Text.Trim(), out cMensajes);
                            if (!String.IsNullOrEmpty(cMensajes))
                            {
                              
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible.error en obtener última factura cfs....{0}</b>", cMensajes));
                                this.Actualiza_Panele_Detalle();
                                return;
                            }
                            /*ultima factura en caso de tener*/
                            var LinqUltimaFactura = (from TblFact in ListUltimaFactura.Where(TblFact => !string.IsNullOrEmpty(TblFact.IV_FACTURA))
                                                        select new
                                                        {
                                                            FT_NUMERO_CARGA = TblFact.IV_NUMERO_CARGA,
                                                            FT_FECHA = (TblFact.IV_FECHA==null ? null : TblFact.IV_FECHA),
                                                            FT_FACTURA = TblFact.IV_FACTURA,
                                                            FT_FECHA_HASTA = (TblFact.IV_FECHA_HASTA==null? null : TblFact.IV_FECHA_HASTA),
                                                            FT_ID = TblFact.IV_ID,
                                                            FT_MODULO = TblFact.IV_MODULO
                                                        }).Distinct();

                            /*left join de contenedores*/
                            var LinqQuery = (from Tbl in LinqPartidadN4
                                                join EcuaPartidas in LinqPartidas on Tbl.CARGA equals EcuaPartidas.CARGA into TmpFinal
                                                join Factura in LinqUltimaFactura on Tbl.CARGA equals Factura.FT_NUMERO_CARGA into TmpFactura
                                                join AutCas in LinqAutorizacion on Tbl.CARGA equals AutCas.CARGA into TmpAutorizacion
                                                from Final in TmpFinal.DefaultIfEmpty()
                                                from FinalFT in TmpFactura.DefaultIfEmpty()
                                                from FinalAut in TmpAutorizacion.DefaultIfEmpty()
                                             select new
                                                {
                                                    CONTENEDOR = Tbl.CNTR_CONTAINER,
                                                    REFERENCIA = (Tbl.CNTR_VEPR_REFERENCE == null) ? string.Empty : Tbl.CNTR_VEPR_REFERENCE,
                                                    TRAFICO = (Tbl.CNTR_TYPE == null) ? string.Empty : Tbl.CNTR_TYPE,
                                                    TAMANO = (Tbl.CNTR_TYSZ_SIZE == null) ? string.Empty : Tbl.CNTR_TYSZ_SIZE,
                                                    TIPO = (Tbl.CNTR_CATY_CARGO_TYPE == null) ? string.Empty : Tbl.CNTR_CATY_CARGO_TYPE,
                                                    FECHA_CAS = (DateTime?)(FinalAut == null ? null : FinalAut.FECHA_AUTORIZACION),
                                                    AUTORIZADO = (bool?)(FinalAut == null ? false : FinalAut.AUTORIZADO),
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
                                                    FECHA_ULTIMA = (FinalFT == null) ? null : (DateTime?)(FinalFT.FT_FECHA.HasValue ? FinalFT.FT_FECHA : null),
                                                    NUMERO_FACTURA = (FinalFT == null) ? string.Empty : FinalFT.FT_FACTURA,
                                                    ID_FACTURA = (FinalFT == null) ? 0 : FinalFT.FT_ID,
                                                    VIAJE = (Tbl.CNTR_VEPR_VOYAGE == null) ? "" : Tbl.CNTR_VEPR_VOYAGE,
                                                    NAVE = (Tbl.CNTR_VEPR_VSSL_NAME == null) ? "" : Tbl.CNTR_VEPR_VSSL_NAME,
                                                    FECHA_ARRIBO = Tbl.CNTR_VEPR_ACTUAL_ARRIVAL,
                                                    CNTR_DD = Tbl.CNTR_DD,
                                                    FECHA_HASTA = (FinalFT == null) ? null : (DateTime?)(FinalFT.FT_FECHA_HASTA.HasValue ? FinalFT.FT_FECHA_HASTA : null),
                                                    ESTADO_RIDT = (Final.ESTADO_RIDT == null) ? string.Empty : Final.ESTADO_RIDT,
                                                    CNTR_DESCARGA = (Tbl.CNTR_DESCARGA==null ? null : (DateTime?)Tbl.CNTR_DESCARGA),
                                                    MODULO = (FinalFT == null) ? string.Empty : FinalFT.FT_MODULO,
                                                    CNTR_DEPARTED = (Tbl.CNTR_VEPR_ACTUAL_DEPARTED == null ? null : (DateTime?)Tbl.CNTR_VEPR_ACTUAL_DEPARTED),
                                                    CANTIDAD = Tbl.CNTR_CANTIDAD,
                                                    PESO = Tbl.CNTR_PESO ,
                                                    OPERACION = Tbl.CNTR_OPERACION ,
                                                    DESCRIPCION = Tbl.CNTR_DESCRIPCION,
                                                    EXPORTADOR = Tbl.CNTR_EXPORTADOR ,
                                                    AGENCIA = Tbl.CNTR_AGENCIA,
                                                    CARGA = Tbl.CARGA,
                                                    ID_UNIDAD = Tbl.ID_UNIDAD 
                                             }).OrderBy(x => x.IN_OUT).ThenBy(x=> x.CONTENEDOR);

                            if (LinqQuery != null && LinqQuery.Count() > 0)
                            {

                                
                                //agrego todos los contenedores a la clase cabecera
                                objCabecera = Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;

                                objCabecera.ID_CLIENTE = this.hf_idcliente.Value;
                                objCabecera.DESC_CLIENTE = this.hf_desccliente.Value;
                                objCabecera.ID_FACTURADO = this.hf_idasume.Value;
                                objCabecera.DESC_FACTURADO = this.hf_descasume.Value;
                                objCabecera.ID_UNICO_AGENTE = this.hf_idagente.Value;
                                objCabecera.ID_AGENTE = this.hf_rucagente.Value;
                                objCabecera.DESC_AGENTE = this.hf_descagente.Value;
                                objCabecera.FECHA = DateTime.Now;
                                objCabecera.TIPO_CARGA = "CFS";
                                objCabecera.NUMERO_CARGA = this.TXTMRN.Text.Trim() + "-" + this.TXTMSN.Text.Trim() + "-" + this.TXTHSN.Text.Trim();
                                objCabecera.IV_USUARIO_CREA = "CGSA";
                                objCabecera.SESION = this.hf_BrowserWindowName.Value;
                                objCabecera.HORA_HASTA = "00:00";

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
                                    objDetalle.ID = Det.ID_FACTURA;
                                    objDetalle.SECUENCIA = Secuencia;
                                    objDetalle.GKEY = Det.GKEY;
                                    objDetalle.MRN = this.TXTMRN.Text.Trim();
                                    objDetalle.MSN = this.TXTMSN.Text.Trim();
                                    objDetalle.HSN = this.TXTHSN.Text.Trim();
                                    objDetalle.CONTENEDOR = Det.CONTENEDOR;
                                    objDetalle.TRAFICO = Det.TRAFICO;
                                    objDetalle.DOCUMENTO = Det.DOCUMENTO;
                                    objDetalle.DES_BLOQUEO = Det.BLOQUEOS;
                                    objDetalle.CONECTADO = Det.CONECTADO;
                                    objDetalle.REFERENCIA = Det.REFERENCIA;
                                    objDetalle.TAMANO = Det.TAMANO;
                                    objDetalle.TIPO = "CFS";
                                    objDetalle.CAS = Det.FECHA_CAS;
                                    objDetalle.AUTORIZADO = (Det.AUTORIZADO ==true ? "SI" : "NO");
                                    objDetalle.BOOKING = "";

                                    objDetalle.IMDT = Det.IMDT;
                                    objDetalle.BLOQUEO = Det.BLOQUEADO;
                                    // objDetalle.FECHA_ULTIMA = Det.FECHA_ULTIMA;
                                    objDetalle.FECHA_ULTIMA = Det.FECHA_HASTA;
                                    objDetalle.IN_OUT = Det.IN_OUT;
                                    objDetalle.FULL_VACIO = Det.FULL_VACIO;
                                    objDetalle.AISV = Det.AISV;
                                    objDetalle.REEFER = Det.TIPO_CONTENEDOR;
                                    objDetalle.IV_USUARIO_CREA = "CGSA";
                                    objDetalle.IV_FECHA_CREA = DateTime.Now;
                                    objDetalle.NUMERO_FACTURA = Det.NUMERO_FACTURA;
                                    objDetalle.CNTR_DD = Det.CNTR_DD;
                                    objDetalle.FECHA_HASTA = Det.FECHA_HASTA;
                                    objDetalle.ESTADO_RDIT = Det.ESTADO_RIDT.Trim();
                                    objDetalle.CNTR_DESCARGA = Det.CNTR_DESCARGA;
                                    objDetalle.MODULO = Det.MODULO;
                                    objDetalle.CNTR_DEPARTED = Det.CNTR_DEPARTED;
                                    objDetalle.LINEA = Det.LINEA;

                                    //nuevos campos
                                    objDetalle.CANTIDAD =  decimal.Parse(Det.CANTIDAD.Value.ToString());
                                    objDetalle.PESO = decimal.Parse(Det.PESO.ToString());
                                    objDetalle.OPERACION = Det.OPERACION;
                                    objDetalle.DESCRIPCION = Det.DESCRIPCION;
                                    objDetalle.EXPORTADOR = Det.EXPORTADOR;
                                    objDetalle.AGENCIA = Det.AGENCIA;
                                    //nuevo
                                    objDetalle.ID_UNIDAD = Det.ID_UNIDAD;

                                    if (!objDetalle.ESTADO_RDIT.Equals("A"))
                                    {
                                        cancelado = true;
                                    }

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
                                        Bloqueos = true;
                                    }

                                    
                                    objDetalle.IDPLAN = "0";
                                    objDetalle.TURNO = "* Seleccione *";

                                 

                                    objCabecera.Detalle.Add(objDetalle);
                                    Secuencia++;
                                }

                                //total de bultos
                                var TotalBultos = objCabecera.Detalle.Sum(x => x.CANTIDAD);
                                objCabecera.TOTAL_BULTOS = TotalBultos;

                                //agrega a la grilla
                                tablePagination.DataSource = objCabecera.Detalle;
                                tablePagination.DataBind();

                                this.LabelTotal.InnerText = string.Format("DETALLE DE CARGA SUELTA (CFS) - Total Bultos: {0}", objCabecera.TOTAL_BULTOS);

                                Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;
                                this.Actualiza_Panele_Detalle();

                                if (cancelado)
                                {

                                    /*************************************************************************************************************************************
                                    * crear caso salesforce
                                    ***********************************************************************************************************************************/
                                    MensajesErrores = string.Format("Se presentaron los siguientes problemas: {0}", "según sistema ecuapass la Respuesta de Aprobación de Salida (Importación) -  RIDT no se encuentra aprobado");

                                    this.Enviar_Caso_Salesforce("CGSA", ruc_contecon, "Facturación CFS", "Unidad sin RIDT Aprobado", MensajesErrores.Trim(), string.Format("{0}-{1}-{2}",this.TXTMRN.Text.Trim(),this.TXTMSN.Text.Trim(),this.TXTHSN.Text.Trim()),
                                        objCabecera.DESC_FACTURADO, objCabecera.DESC_AGENTE, out MensajeCasos, false);

                                    /*************************************************************************************************************************************
                                    * fin caso salesforce
                                    * **********************************************************************************************************************************/

                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Estimado cliente, lamentamos mucho no poder atender su requerimiento, según sistema ecuapass la Respuesta de Aprobación de Salida (Importación) -  RIDT no se encuentra aprobado. Ponerse en contacto con el Senae.</b>"));
                                    //return;
                                }
                                
                            }
                            else
                            {
                                tablePagination.DataSource = null;
                                tablePagination.DataBind();

                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de carga suelta pendiente de proformar con el número de la carga ingresada..</b>"));
                                this.Actualiza_Panele_Detalle();
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de carga suelta pendiente de proformar con el número de la carga ingresada..{0}</b>", ListaContenedores.MensajeProblema));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de carga suelta pendiente de proformar con el número de la carga ingresada..{0}</b>", EcuaContenedores.MensajeProblema));
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

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect("~/cotizaciones/cotizacion_default02.aspx", false);



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.myModal.Attributes["class"] = "nover";
            this.Actualiza_Paneles();

        }

        protected void BtnCotizar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {



                CultureInfo enUS = new CultureInfo("en-US");
                this.myModal.Attributes["class"] = "nover";

                try
                {


                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga MRN</b>"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga MSN</b>"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el número de la carga HSN</b>"));
                        this.TXTHSN.Focus();
                        return;
                    }
                    if (this.CboAsumeFactura.Items.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona que va asumir la carga</b>"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    if (this.Agregar())
                    {
                        HoraHasta = "00:00";


                        //valida que se seleccione la persona a facturar
                        if (this.CboAsumeFactura.SelectedIndex == 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la persona que asumirá la factura.</b>"));
                            this.CboAsumeFactura.Focus();
                            return;
                        }

                        //fecha hasta para sacar los servicios
                        Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                        if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la cotización</b>"));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                        //instancia sesion
                        objCabecera = Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                        if (objCabecera == null)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder generar la cotización de carga suelta CFS</b>"));
                            return;
                        }
                        if (objCabecera.Detalle.Count == 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de carga suelta CFS, para poder generar la cotización</b>"));
                            return;
                        }

                        //valida que seleccione todos los contenedores para cotizar 
                        var LinqValidaContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == false)
                                                    select p.CONTENEDOR).ToList();

                        if (LinqValidaContenedor.Count != 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe agregar la fecha de salida de la carga a cotizar: {0}</b>", objCabecera.NUMERO_CARGA));
                            return;
                        }
                        //valida que tenga todos tengan fecha de salida
                        foreach (var Det in objCabecera.Detalle)
                        {
                            if (!Det.FECHA_HASTA.HasValue)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida de salida de la carga {0}</b>", objCabecera.NUMERO_CARGA));
                                return;
                            }
                            if (Det.FECHA_HASTA.Value.Date < System.DateTime.Today.Date)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! La fecha de salida de la carga: {0}, no puede ser menor que la fecha actual..</b>", objCabecera.NUMERO_CARGA));
                                return;
                            }
                        }

                        //listado de contenedores
                        var LinqListContenedor = (from p in objCabecera.Detalle.Where(x => x.VISTO == true)
                                                  select p.CONTENEDOR).ToList();

                        objCabecera.GLOSA = this.Txtcomentario.Text.Trim();
                        Contenedores = string.Join(", ", LinqListContenedor);
                        objCabecera.HORA_HASTA = HoraHasta;

                        //numero de carga
                        Numero_Carga = objCabecera.NUMERO_CARGA;
                        objCabecera.CONTENEDORES = Contenedores;
                        objCabecera.FECHA_HASTA = FechaFactura;
                        LoginName = objCabecera.IV_USUARIO_CREA.Trim();
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
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo obtener la información de la persona que asumirá la proforma.</b>"));
                            return;
                        }

                        /***********************************************************************************************************************************************
                        *datos del cliente N4 
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
                            }
                            else
                            {
                                this.Limpia_Asume_Factura();
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo cargar datos del Cliente con el ruc: {0}</b>", objCabecera.ID_FACTURADO));

                                return;
                            }
                        }
                        else
                        {
                            this.Limpia_Asume_Factura();
                            // this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i> <b> Error! Cliente [{0}-{1}] no se encuentra registrado en nuestra base de datos, por favor enviar la información RUC/CI, nombre, dirección, ciudad/Provincia y correo electrónico, al mail: <a href='mailto: ec.fac@contecon.com.ec'>ec.fac@contecon.com.ec</a> para proceder con el registro...Teléfonos (04) 6006300 – 3901700 opción 2,  Horario lunes a viernes 8h30 a 19h00 sábados y domingos 8h00 a 15h00.</b>", objCabecera.ID_FACTURADO, objCabecera.DESC_FACTURADO));
                            return;
                        }
                        /***********************************************************************************************************************************************
                        *fin: datos del cliente N4 
                        **********************************************************************************************************************************************/

                        //actualizo el objeto temporal
                        Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;

                        Fila = 1;
                        Decimal Subtotal = 0;
                        Decimal Iva = 0;
                        Decimal Total = 0;
                        /***********************************************************************************************************************************************
                        *2) proceso para grabar proforma
                        **********************************************************************************************************************************************/
                        objCabecera = Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
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

                        //string cip = Cls_Bil_IP.GetLocalIPAddress();
                        string cip = Request.UserHostAddress;
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

                        Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;

                        /***********************************************************************************************************************************************
                        *graba cotizacion en base de billion, para pasara la siguiente ventana
                        **********************************************************************************************************************************************/

                        if (objProforma == null)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No existe objeto para generar la proforma de carga suelta</b>"));
                            return;
                        }
                        else
                        {
                            //si no existen servicios a cotizar
                            if (objProforma.DetalleServicios.Count == 0)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existen servicios pendientes para cotizar</b>"));
                                return;
                            }

                            var nIdRegistro = objProforma.SaveTransaction(out cMensajes);
                            if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo grabar datos de la proforma de carga suelta..{0}</b>", cMensajes));
                                return;
                            }

                            this.Ocultar_Mensaje();
                            string cId = securetext(nIdRegistro.Value.ToString());
                            Response.Redirect("~/cotizaciones/cotizacion_default02_preview.aspx?id_proforma=" + cId.Trim() + "", false);

                        }

                    }

                  

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

                }

            }
        }


        private bool Agregar()
        {
            bool resultado = true;
            try
            {
                CultureInfo enUS = new CultureInfo("en-US");
               

                objCabecera = Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                if (objCabecera == null)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta</b>"));
                    resultado = false;
                    return resultado;
                }

                foreach (var Det in objCabecera.Detalle)
                {
                    var Detalle = objCabecera.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(Det.CONTENEDOR.Trim()));
                    if (Detalle != null)
                    {

                        if (Detalle.IN_OUT.Equals("OUT"))
                        {

                        }
                        else
                        {

                            HoraHasta = "00:00";
                            Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                            if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                            {
                                Detalle.VISTO = false;
                                Detalle.FECHA_HASTA = null;
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la cotización o factura</b>"));
                                this.TxtFechaHasta.Focus();
                                resultado = false;
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
                                    resultado = false;
                                    break;
                                }
                                else
                                {
                                    Detalle.VISTO = true;
                                    Detalle.FECHA_HASTA = FechaFactura;
                                    resultado = true;
                                }

                            }

                        }

                    }
                }


                tablePagination.DataSource = objCabecera.Detalle;
                tablePagination.DataBind();

                Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;

                this.Actualiza_Panele_Detalle();

                return resultado;

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", ex.Message));
                resultado = false;
                return resultado;
            }
        }
       

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {               
                CultureInfo enUS = new CultureInfo("en-US");

                objCabecera = Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
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
                        
                        if (Detalle.IN_OUT.Equals("OUT") )
                        {

                        }
                        else
                        {
                                  
                            HoraHasta = "00:00";
                            Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                            if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                            {
                                Detalle.VISTO = false;
                                Detalle.FECHA_HASTA = null;
                                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida para la cotización o factura</b>"));
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
                                    Detalle.VISTO = true;
                                    Detalle.FECHA_HASTA = FechaFactura;
                                }

                            }

                        }

                    }
                }


                tablePagination.DataSource = objCabecera.Detalle;
                tablePagination.DataBind();

                Session["CotizacionCFS" + this.hf_BrowserWindowName.Value] = objCabecera;

                this.Actualiza_Panele_Detalle();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", ex.Message));

            }
        }

        #endregion

        #region "Eventos Check"
      
        #endregion

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //ViewStateUserKey = Session.SessionID;

        }

     
      
      

      
   
       


        }
}