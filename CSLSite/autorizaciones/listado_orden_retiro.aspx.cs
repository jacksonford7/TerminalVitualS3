using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Web.Services;
using csl_log;
using Microsoft.Reporting.WebForms;
using ClsAutorizaciones;
using System.Xml.Linq;

namespace CSLSite.autorizaciones
{
    public partial class listado_orden_retiro : System.Web.UI.Page
    {
        public static string v_mensaje = string.Empty;
      
       
        public string rucempresa
        {
            get { return (string)Session["rucempresa"]; }
            set { Session["rucempresa"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["usuarioemail"]; }
            set { Session["usuarioemail"] = value; }
        }

        private DataTable DtListOrdenes
        {
            get
            {
                return (DataTable)Session["DtListOrdenes"];
            }
            set
            {
                Session["DtListOrdenes"] = value;
            }

        }


        #region "Metodos"

        private void OcultarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Carga_Ordenes(DateTime pDesde, DateTime pHasta, string LINEA_NAVIERA, string REFERENCIA)
        {
            try
            {
                DataSet dsRetorno = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(Int64));
                dt.Columns.Add("FECHA", typeof(DateTime));
                dt.Columns.Add("AUTORIZACION", typeof(String));
                dt.Columns.Add("REFERENCIA", typeof(String));
                dt.Columns.Add("USUARIO_CRE", typeof(String));
                dt.Columns.Add("FECHA_CREA", typeof(DateTime));
                dt.Columns.Add("LINEA_NAVIERA", typeof(String));
                dt.Columns.Add("TOTAL_CONTENEDORES", typeof(int));
                dt.Columns.Add("TOTAL_PENDIENTES", typeof(int));
                dt.Columns.Add("TOTAL_PROCESADOS", typeof(int));
                dt.Columns.Add("ESTADO_TRANSACCION", typeof(String));

                List<ReporteVacio> Listado = ReporteVacio.Listado_Ordenes(pDesde, pHasta, LINEA_NAVIERA, REFERENCIA, out v_mensaje);
                if (Listado != null)
                {
                    var Tmp = (from Orden in Listado.AsEnumerable()
                               select new
                               {
                                   ID = Orden.ID,
                                   FECHA = (!Orden.FECHA.HasValue ? null : Orden.FECHA),
                                   AUTORIZACION = Orden.AUTORIZACION,
                                   REFERENCIA = Orden.REFERENCIA,
                                   USUARIO_CRE = Orden.USUARIO_CRE,
                                   FECHA_CREA = (!Orden.FECHA_CREA.HasValue ? null : Orden.FECHA_CREA),
                                   LINEA_NAVIERA = Orden.LINEA_NAVIERA,
                                   TOTAL_CONTENEDORES = Orden.TOTAL_CONTENEDORES,
                                   TOTAL_PENDIENTES = Orden.TOTAL_PENDIENTES,
                                   TOTAL_PROCESADOS = Orden.TOTAL_PROCESADOS,
                                   ESTADO_TRANSACCION = Orden.ESTADO_TRANSACCION,
                                   XML_CONTENEDORES = Orden.XML_CONTENEDORES
                               }).ToList();


                    //dsRetorno.Tables.Add(MantenimientoVehiculo.LINQToDataTable(Tmp));


                    foreach (var Det in Tmp)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = Det.ID;
                        dr["FECHA"] = (!Det.FECHA.HasValue ? System.DateTime.Today : Det.FECHA);
                        dr["AUTORIZACION"] = Det.AUTORIZACION;
                        dr["REFERENCIA"] = Det.REFERENCIA;
                        dr["USUARIO_CRE"] = Det.USUARIO_CRE;
                        dr["FECHA_CREA"] = (!Det.FECHA_CREA.HasValue ? System.DateTime.Today : Det.FECHA_CREA);
                        dr["LINEA_NAVIERA"] = Det.LINEA_NAVIERA;
                        dr["TOTAL_CONTENEDORES"] = Det.TOTAL_CONTENEDORES;

                        List<ReporteVacio> Cantidades = ReporteVacio.Cantidad_Contenedores(Det.XML_CONTENEDORES, out v_mensaje);
                        if (Cantidades != null)
                        {
                            var CANT_RETIRADA = (from p in Cantidades
                                                 select new { RETIRADA = p.RETIRADA }).FirstOrDefault();

                            var CANT_POR_RETIRADA = (from p in Cantidades
                                                     select new { POR_RETIRADA = p.POR_RETIRADA }).FirstOrDefault();

                            dr["TOTAL_PENDIENTES"] = CANT_POR_RETIRADA.POR_RETIRADA ;
                            dr["TOTAL_PROCESADOS"] = CANT_RETIRADA.RETIRADA;
                        }
                        else
                        {
                            dr["TOTAL_PENDIENTES"] = "0";
                            dr["TOTAL_PROCESADOS"] = "0";
                        }
                        dr["ESTADO_TRANSACCION"] = Det.ESTADO_TRANSACCION;
                       
                        dt.Rows.Add(dr);
                    }

                    dsRetorno.Tables.Add(dt);

                    DtListOrdenes = dsRetorno.Tables[0];
                    tablePagination.DataSource = dsRetorno.Tables[0];
                    tablePagination.DataBind();
                    this.ordenes.Visible = true;

                    if (dsRetorno.Tables[0].Rows.Count <= 0)
                    {
                        this.ordenes.Visible = false;
                        this.sinresultado.InnerText = "No se encontraron resultados, revise los parámetros";
                        sinresultado.Visible = true;
                        return;
                    }


                }
                else
                {
                    DtListOrdenes = null;
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                    this.ordenes.Visible = false;
                    this.sinresultado.InnerText = "No se encontraron resultados, revise los parámetros";
                    sinresultado.Visible = true;

                }

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.InnerText = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_Ordenes", "Hubo un error al cargar listado", t.loginname));
                sinresultado.Visible = true;

            }

        }



        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        #region "Eventos Formulario"

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "autorizaciones", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
               // rucempresa = user.ruc;
                useremail = user.email;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
            this.TxtReferencia.Text = Server.HtmlEncode(this.TxtReferencia.Text);
            this.TxtLineaNaviera.Text = Server.HtmlEncode(this.TxtLineaNaviera.Text);
            this.desded.Text = Server.HtmlEncode(this.desded.Text);
            this.desded.Text = Server.HtmlEncode(this.hastad.Text);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    var user = Page.Tracker();
                    if (user != null && !string.IsNullOrEmpty(user.ruc))
                    {
                        var ruc_cgsa = System.Configuration.ConfigurationManager.AppSettings["ruc_cgsa"];
                        if (user.ruc.Trim().Equals(ruc_cgsa.Trim()))
                        {
                            this.TxtLineaNaviera.Text = string.Empty;
                            this.buscar_linea.Visible = true;
                            this.AuxLinea_Naviera.Value = string.Empty;
                            rucempresa = string.Empty;
                        }
                        else
                        {
                            this.TxtLineaNaviera.Text = user.ruc;
                            this.buscar_linea.Visible = false;
                            this.AuxLinea_Naviera.Value = user.ruc;
                            rucempresa = user.ruc;
                        }

                        this.ordenes.Visible = false;

                    }
                    else
                    {
                        this.TxtLineaNaviera.Text = string.Empty;
                        this.buscar_linea.Visible = false;
                        this.AuxLinea_Naviera.Value = string.Empty;
                        rucempresa = string.Empty;
                    }


                    sinresultado.Visible = false;

                    string desde = string.Format("{0}/{1}/{2}","01", DateTime.Today.Month.ToString("D2"), DateTime.Today.Year.ToString());

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {

                        this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha desde";
                        sinresultado.Visible = true;
                        return;
                    }

                    this.desded.Text = fdesde.ToString("dd/MM/yyyy");
                    this.hastad.Text = DateTime.Today.ToString("dd/MM/yyyy");

                    
                    

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Page_Load", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
            else
            {
                this.TxtLineaNaviera.Text = this.AuxLinea_Naviera.Value;
                if (rucempresa != this.AuxLinea_Naviera.Value)
                {
                    this.ordenes.Visible = false;
                }
                rucempresa = this.AuxLinea_Naviera.Value;
            }
        }

        #endregion

        #region "Eventos Controles Vehiculo"

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        OcultarLoading();
                    }
                    alerta.Visible = true;

                    sinresultado.Visible = false;

                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";


                    if (string.IsNullOrEmpty(this.TxtLineaNaviera.Text))
                    {
                        OcultarLoading();
                        this.Alerta("Debe seleccionar la líneas naviera");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtLineaNaviera.Focus();
                        return;
                    }

                    var user = Page.getUserBySesion();

                    string LINEA_NAVIERA = this.TxtLineaNaviera.Text.Trim();

                    DateTime desde;
                    DateTime hasta;

                    if (this.desded.Text != string.Empty)
                    {
                        if (!DateTime.TryParseExact(desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                        {
                            OcultarLoading();
                            this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha desde";
                            sinresultado.Visible = true;
                            return;
                        }
                    }
                    else { desde = DateTime.Parse("01/01/1999"); }

                    if (this.hastad.Text != string.Empty)
                    {
                        if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                        {
                            OcultarLoading();
                            this.sinresultado.InnerText = "Ingrese una fecha valida, revise la fecha hasta";
                            sinresultado.Visible = true;
                            return;
                        }
                    }
                    else { hasta = DateTime.Parse("01/01/1999"); }

                    TimeSpan tsDias = hasta - desde;
                    int diferenciaEnDias = tsDias.Days;
                    if (diferenciaEnDias < 0)
                    {
                        OcultarLoading();
                        this.sinresultado.InnerText = string.Format("La Fecha de desde: {0} \\nNO deber ser mayor a la\\nFecha hasta: {1}",this.desded.Text, hastad.Text);
                        sinresultado.Visible = true;
                        return;

                    }
                    if (diferenciaEnDias > 31)
                    {
                        OcultarLoading();
                        this.sinresultado.InnerText = "Solo puede consultar ordenes de hasta un mes.";
                        sinresultado.Visible = true;
                        return;
              
                    }

                    this.Carga_Ordenes(desde, hasta, LINEA_NAVIERA, this.TxtReferencia.Text.Trim());

                    Session["usuarioemail"] = useremail;
                    Session["rucempresa"] = rucempresa;
                    OcultarLoading();
                   
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "BtnBuscar_Click", "Hubo un error al buscar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                    OcultarLoading();
                }
            }
        }


        #endregion

        #region "Control Grilla"
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

                    tablePagination.PageIndex = e.NewPageIndex;
                    tablePagination.DataSource = DtListOrdenes.AsDataView();
                    tablePagination.DataBind();


                }


            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "paginar", "tablePagination_PageIndexChanging", "Hubo un error al buscar", t.loginname));
                sinresultado.Visible = true;
            }
        }


        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
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

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination_RowCommand", "No se pudo obtener usuario", "anónimo"));

                        sinresultado.InnerText = cMensaje2;
                        sinresultado.Visible = true;
                        return;
                    }

                    if (e.CommandName == "Seleccionar")
                    {
                        string Id = e.CommandArgument.ToString();
                        Int64 nId = 0;

                        if (!Int64.TryParse(Id, out nId))
                        {
                            var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "tablePagination_RowCommand", "CommandArgument longitud errada: " + Id.Length.ToString(), user.loginname));
                            sinresultado.InnerText = cMensaje2;
                            sinresultado.Visible = true;
                            return;
                        }

                        usuario sUser = null;
                        sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                       

                    }

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la eliminación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "eliminar", "tablePagination_RowCommand", "Hubo un error al eliminar", t.loginname));
                    sinresultado.Visible = true;
                }


            }

        }


        #endregion






    }
}