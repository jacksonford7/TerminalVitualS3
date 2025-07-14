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

using System.Web.Services;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CSLSite
{


    public partial class reportecajas_agencia : System.Web.UI.Page
    {


        #region "Clases"
        private static Int64? lm = -3;
        private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
       

        private Cls_Bil_Invoice_Cabecera objFactura = new Cls_Bil_Invoice_Cabecera();
        private Cls_Bil_Invoice_Detalle objDetalleFactura = new Cls_Bil_Invoice_Detalle();
        private Cls_Bil_Invoice_Servicios objServiciosFactura = new Cls_Bil_Invoice_Servicios();
        private Cls_Bil_Invoice_Validaciones objValidacion = new Cls_Bil_Invoice_Validaciones();

        private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        private Cls_Bil_Detalle objDetalle = new Cls_Bil_Detalle();
        private Cls_Bil_Invoice_Actualiza_Pase objActualiza_Pase = new Cls_Bil_Invoice_Actualiza_Pase();
        private List<Cls_Bil_AsumeFactura> List_Asume { set; get; }
        private List<Cls_Bil_Contenedor_DiasLibres> List_Contenedor { set; get; }
      
        private List<Cls_Bil_Cas_Manual> List_Autorizacion { set; get; }
        private Cls_Bil_Log_Appcgsa objLogAppCgsa = new Cls_Bil_Log_Appcgsa();

        private MantenimientoPaqueteCliente obj = new MantenimientoPaqueteCliente();
        private Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();

        private P2D_Valida_Proforma objValida = new P2D_Valida_Proforma();

     
        private P2D_Tarifario objTarifa = new P2D_Tarifario();
        private P2D_Tarja_Cfs objTarja = new P2D_Tarja_Cfs();


        private BTS_Prev_Cabecera objCabeceraBTS = new BTS_Prev_Cabecera();
        private Cls_Prev_Detalle_Bodega objDetalleBodega = new Cls_Prev_Detalle_Bodega();
        private Cls_Prev_Detalle_Muelle objDetalleMuelle = new Cls_Prev_Detalle_Muelle();

        private BTS_Prev_Cabecera objFacturaBTS = new BTS_Prev_Cabecera();
        private Cls_Prev_Detalle_Bodega objDetalleFacturaBodega = new Cls_Prev_Detalle_Bodega();
        private Cls_Prev_Detalle_Muelle objDetalleFacturaMuelle = new Cls_Prev_Detalle_Muelle();
        private Cls_Prev_Detalle_Servicios objDetalleFacturaServicios = new Cls_Prev_Detalle_Servicios();
        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;

      //  private Int64 Gkey = 0;
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
      
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        private bool sap_valida = false;
        private bool tieneBloqueo = false;
        private decimal SaldoPendiente = 0;
      
        private Int64 DiasCredito = 0;
        private decimal Cupo = 0;
        bool Bloqueo_Cliente = false;
        bool Liberado_Cliente = false;
        private string gkeyBuscado = string.Empty;

      
        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;
      

        private static string TextoLeyenda = string.Empty;

        private static string TextoProforma = string.Empty;
        private static string TextoServicio = string.Empty;
        private string EmpresaSelect = string.Empty;
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

        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
            UPMUELLE.Update();
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
            Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabecera;
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


                    objFacturaBTS = new BTS_Prev_Cabecera();
                    Session["InvoiceBTS" + this.hf_BrowserWindowName.Value] = objFacturaBTS;

                    objCabeceraBTS = new BTS_Prev_Cabecera();
                    Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;

                   

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", ex.Message));
            }
        }


        #endregion
      
       #region "Eventos de la grilla bodega"

       
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
                        
                       

                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

                }
            }
        }



        #endregion


        #region "Eventos de la grilla muelle"
        protected void tableMuelle_PreRender(object sender, EventArgs e)
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


                    if (tableMuelle.Rows.Count > 0)
                    {
                        tableMuelle.UseAccessibleHeader = true;
                        // Agrega la sección THEAD y TBODY.
                        tableMuelle.HeaderRow.TableSection = TableRowSection.TableHeader;

                    }
                }



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

        protected void tableMuelle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           
        }

        protected void tableMuelle_RowDataBound(object sender, GridViewRowEventArgs e)
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



        //revisando 1
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    

                    OcultarLoading("2");
                    bool Ocultar_Mensaje = true;
                    string IdEmpresa = string.Empty;
                    string DesEmpresa = string.Empty;

                    string Msg = string.Empty;

                    this.LabelTotal.InnerText = string.Format("BODEGA FRIA/SECA");

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

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string IdAgenteCodigo = string.Empty;
                    

                   


                    /*detalle de bodegas*/
                    List<BTS_Detalle_Bodegas> ListBodegas = BTS_Detalle_Bodegas.Carga_DetalleBodegas_Referencia(this.TXTMRN.Text.Trim() , out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener detalle de bodegas....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                    /*ultima factura en caso de tener*/
                    var LinqBodegas = (from Tbl in ListBodegas.Where(Tbl => Tbl.cajas != 0)
                                             select new
                                             {
                                                 Fila = Tbl.Fila,
                                                 idNave = Tbl.idNave ,
                                                 codLine = Tbl.codLine,
                                                 nave = Tbl.nave,
                                                 ruc = Tbl.ruc,
                                                 Exportador = Tbl.Exportador,
                                                 booking = Tbl.booking,
                                                 idModalidad = Tbl.idModalidad,
                                                 desc_modalidad = Tbl.desc_modalidad,
                                                 id_bodega = Tbl.id_bodega,
                                                 desc_bodega = Tbl.desc_bodega,
                                                 tipo_bodega = Tbl.tipo_bodega,
                                                 id_tipo_bodega = Tbl.id_tipo_bodega,
                                                 QTY_out = Tbl.QTY_out,
                                                 REFERENCIA = Tbl.referencia,
                                                 LINEA = Tbl.linea,
                                                 CAJAS = Tbl.cajas
                                             }).Distinct();

                    if (LinqBodegas != null)
                    {

                        /*detalle de muelles*/
                        List<BTS_Detalle_Muelle> Listmuelle = BTS_Detalle_Muelle.Carga_DetalleMuelle_Referencia(this.TXTMRN.Text.Trim(),out cMensajes);
                        if (!String.IsNullOrEmpty(cMensajes))
                        {

                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible...error en obtener detalle de muelles....{0}</b>", cMensajes));
                            this.Actualiza_Panele_Detalle();
                            return;
                        }

                        /*ultima factura en caso de tener*/
                        var LinqMuelles = (from Tbl in Listmuelle.Where(Tbl => Tbl.cajas != 0)
                                           select new
                                           {
                                               Fila = Tbl.Fila,
                                               idNave = Tbl.idNave,
                                               codLine = Tbl.codLine,
                                               nave = Tbl.nave,
                                               aisv_codig_clte = Tbl.aisv_codig_clte,
                                               aisv_nom_expor = Tbl.aisv_nom_expor,
                                               aisv_estado = Tbl.aisv_estado,
                                               cajas = Tbl.cajas,
                                               referencia = Tbl.referencia,
                                               linea = Tbl.linea,
                                               ruc = Tbl.ruc,
                                               exportador = Tbl.Exportador,
                                               cajas_paletizado = Tbl.cajas_paletizado,
                                           }).Distinct();




                        //agrego todos los contenedores a la clase cabecera
                        objCabeceraBTS = Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] as BTS_Prev_Cabecera;

                        objCabeceraBTS.Detalle_bodega.Clear();
                        objCabeceraBTS.Detalle_muelle.Clear();

                        Int16 Secuencia = 1;
                        foreach (var Det in LinqBodegas)
                        {
                            objDetalleBodega = new Cls_Prev_Detalle_Bodega();

                            objDetalleBodega.Fila = Det.Fila;
                            objDetalleBodega.idNave = Det.idNave;
                            objDetalleBodega.codLine = Det.codLine;
                            objDetalleBodega.nave = Det.nave;
                            objDetalleBodega.ruc = Det.ruc;
                            objDetalleBodega.Exportador = Det.Exportador;
                            objDetalleBodega.booking = Det.booking;
                            objDetalleBodega.idModalidad = Det.idModalidad;
                            objDetalleBodega.desc_modalidad = Det.desc_modalidad;
                            objDetalleBodega.id_bodega = Det.id_bodega;
                            objDetalleBodega.desc_bodega = Det.desc_bodega;
                            objDetalleBodega.tipo_bodega = Det.tipo_bodega;
                            objDetalleBodega.id_tipo_bodega = Det.id_tipo_bodega;
                            objDetalleBodega.QTY_out = Det.QTY_out;

                            objDetalleBodega.referencia = Det.REFERENCIA;
                            objDetalleBodega.linea = Det.LINEA;
                            objDetalleBodega.cajas = Det.CAJAS;

                            objCabeceraBTS.Detalle_bodega.Add(objDetalleBodega);
                            Secuencia++;
                        }

                        tablePagination.DataSource = objCabeceraBTS.Detalle_bodega;
                        tablePagination.DataBind();

                        var TotalCajas = objCabeceraBTS.Detalle_bodega.Sum(x => x.cajas);
                        objCabeceraBTS.TOTAL_CAJAS_BODEGA = TotalCajas;

                        this.LabelTotal.InnerText = string.Format("DETALLE DE BODEGA FRIA/SECA - Total Cajas: {0}", objCabeceraBTS.TOTAL_CAJAS_BODEGA);

                        //muelle
                        Secuencia = 1;

                        if (LinqMuelles != null)
                        {
                            foreach (var Det in LinqMuelles)
                            {
                                objDetalleMuelle = new Cls_Prev_Detalle_Muelle();

                                objDetalleMuelle.Fila = Det.Fila;
                                objDetalleMuelle.idNave = Det.idNave;
                                objDetalleMuelle.codLine = Det.codLine;
                                objDetalleMuelle.nave = Det.nave;
                                objDetalleMuelle.aisv_codig_clte = Det.aisv_codig_clte;
                                objDetalleMuelle.aisv_nom_expor = Det.aisv_nom_expor;
                                objDetalleMuelle.aisv_estado = Det.aisv_estado;
                                objDetalleMuelle.cajas = Det.cajas;
                                objDetalleMuelle.linea = Det.linea;
                                objDetalleMuelle.referencia = Det.referencia;
                                objDetalleMuelle.ruc = Det.ruc;
                                objDetalleMuelle.Exportador = Det.exportador;
                                objDetalleMuelle.cajas_paletizado = Det.cajas_paletizado;
                                objCabeceraBTS.Detalle_muelle.Add(objDetalleMuelle);
                                Secuencia++;
                            }
                        }

                        tableMuelle.DataSource = objCabeceraBTS.Detalle_muelle;
                        tableMuelle.DataBind();

                        var TotalMuelles = objCabeceraBTS.Detalle_muelle.Sum(x => x.cajas);
                        objCabeceraBTS.TOTAL_CAJAS_MUELLE = TotalMuelles;

                        this.LabelMuelle.InnerText = string.Format("DETALLE DE MUELLE - Total Cajas: {0}", objCabeceraBTS.TOTAL_CAJAS_MUELLE);


                        Session["TransaccionBTS" + this.hf_BrowserWindowName.Value] = objCabeceraBTS;

                      

                        this.Actualiza_Panele_Detalle();

                       

                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                      

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe información de bodega FRIA/SECA  con el número de referencia {0} </b>", this.TXTMRN.Text.Trim()));

                       

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

   

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }


       
        protected void BtnProcesar_Click(object sender, EventArgs e)
        {

        }



        public static byte[] CreateExcelBytesFromStoredProcedure(string pReferencia)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BILLION"].ConnectionString;

            DataTable DtBodega = new DataTable();
            DataTable DtMuelle = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("BTS_REPORTE_DETALLE_BODEGAS", connection))
                {
                  
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@REFERENCIA", pReferencia);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DtBodega.Load(reader);
                    }


                }

            }

            Color colorPlaneado = ColorTranslator.FromHtml("#59B653");
            Color colorReservado = ColorTranslator.FromHtml("#5AABD9");
            Color colorDisponible = ColorTranslator.FromHtml("#D9DB63");

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");


                worksheet.Cells["A5:A5"].Value = "BODEGA:";
                worksheet.Cells["A5:A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A5:A5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A5:A5"].Style.Fill.BackgroundColor.SetColor(colorPlaneado);
                worksheet.Cells["A5:A5"].Style.Font.Color.SetColor(System.Drawing.Color.White);


                int r = 6;
                int c = 1;
                foreach (DataColumn column in DtBodega.Columns)  //printing column headings
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
               

                for (eachRow = 0; eachRow < DtBodega.Rows.Count;)
                {
                    new_col = 1;
                    col = 0;
                    foreach (DataColumn column in DtBodega.Columns)
                    {

                        worksheet.Cells[r, new_col].Value = DtBodega.Rows[fila][col];
                        col++;
                        new_col++;
                    }

                    eachRow++;
                    r++;
                    fila++;

                }

                r = r + 2;

                worksheet.Cells["A"+ r.ToString() + ":A" + r.ToString()].Value = "MUELLE:";
                worksheet.Cells["A" + r.ToString() + ":A" + r.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A" + r.ToString() + ":A" + r.ToString()].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A" + r.ToString() + ":A" + r.ToString()].Style.Fill.BackgroundColor.SetColor(colorReservado);
                worksheet.Cells["A" + r.ToString() + ":A" + r.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.White);

              

                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["catalogo"].ConnectionString;

               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("BTS_REPORTE_DETALLE_MUELLE", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@REFERENCIA", pReferencia);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DtMuelle.Load(reader);
                        }


                    }

                }


                c = 1;
                 r = r + 1;
                foreach (DataColumn column in DtMuelle.Columns)  //printing column headings
                {


                    worksheet.Cells[r, c].Value = column.ColumnName;
                    worksheet.Cells[r, c].Style.Font.Bold = true;
                    worksheet.Cells[r, c].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells[r, c].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[r, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[r, c].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                    c = c + 1;
                }

                fila = 0;
                r = r + 1;

                for (eachRow = 0; eachRow < DtMuelle.Rows.Count;)
                {
                    new_col = 1;
                    col = 0;
                    foreach (DataColumn column in DtMuelle.Columns)
                    {

                        worksheet.Cells[r, new_col].Value = DtMuelle.Rows[fila][col];
                        col++;
                        new_col++;
                    }

                    eachRow++;
                    r++;
                    fila++;

                }

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
                // Define el código de estado HTTP y la descripción del error.
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.StatusDescription = "Error al exportar el archivo Excel";


            }
        }

    }
}