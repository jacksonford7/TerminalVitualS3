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
using ClsAutorizaciones;
using System.Xml;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace CSLSite.autorizaciones
{
    public partial class cargar_archivo_contenedores : System.Web.UI.Page
    {
        public static string v_mensaje = string.Empty;
      

        private Contenedor objConteendor = new Contenedor();
        private Contenedor_Detalle objDetConteendor = new Contenedor_Detalle();

        //public string rucempresa
        //{
        //    get { return (string)Session["rucempresa"]; }
        //    set { Session["rucempresa"] = value; }
        //}
        public string useremail
        {
            get { return (string)Session["usuarioemail"]; }
            set { Session["usuarioemail"] = value; }
        }
        private string xmlDocumentos
        {
            get
            {
                return (string)Session["xmlDocumentos"];
            }
            set
            {
                Session["xmlDocumentos"] = value;
            }

        }
        private string RutaArchivo
        {
            get
            {
                return (string)Session["RutaArchivo"];
            }
            set
            {
                Session["RutaArchivo"] = value;
            }

        }
        private string ExtensionArchivo
        {
            get
            {
                return (string)Session["ExtensionArchivo"];
            }
            set
            {
                Session["ExtensionArchivo"] = value;
            }

        }

        private DataTable DtContenedores
        {
            get
            {
                return (DataTable)Session["DtContenedores"];
            }
            set
            {
                Session["DtContenedores"] = value;
            }

        }

        private string ContenidoArchivo
        {
            get
            {
                return (string)Session["ContenidoArchivo"];
            }
            set
            {
                Session["ContenidoArchivo"] = value;
            }

        }


        #region "Metodos"

        private void OcultarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
        }

        private void MostrarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mostrarloader();", true);
        }

        private void Limpiar()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "clearTextBox();", true);
        }


        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
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
            //this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "autorizaciones", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
                //rucempresa = user.ruc;
                useremail = user.email;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }

            this.TxtCantidad.Text = Server.HtmlEncode(this.TxtCantidad.Text);
            this.TxtLineaNaviera.Text = Server.HtmlEncode(this.TxtLineaNaviera.Text);
            this.TxtNumeroOrden.Text = Server.HtmlEncode(this.TxtNumeroOrden.Text);
            this.TxtFechaProceso.Text = Server.HtmlEncode(this.TxtFechaProceso.Text);
            this.Txtautorizacion.Text = Server.HtmlEncode(this.Txtautorizacion.Text);
            this.TxtReferencia.Text = Server.HtmlEncode(this.TxtReferencia.Text);
            this.TxtFechaProceso.Text = System.DateTime.Today.ToString("dd/MM/yyyy");

           
            if (Session["FileUpload1"] == null && fsuploadarchivo.HasFile)
            {
                Session["FileUpload1"] = fsuploadarchivo;
                LblRuta.Text = fsuploadarchivo.FileName;
                this.ContenidoArchivo = new StreamReader(fsuploadarchivo.PostedFile.InputStream).ReadToEnd();
            }
           
            else if (Session["FileUpload1"] != null && (!fsuploadarchivo.HasFile))
            {
                fsuploadarchivo = (AjaxControlToolkit.AsyncFileUpload)Session["FileUpload1"];
                LblRuta.Text = fsuploadarchivo.FileName;

            }
           
            else if (fsuploadarchivo.HasFile)
            {
                Session["FileUpload1"] = fsuploadarchivo;
                LblRuta.Text = fsuploadarchivo.FileName;
                this.ContenidoArchivo = new StreamReader(fsuploadarchivo.PostedFile.InputStream).ReadToEnd();
            }

   
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
                        //this.LblLineaNaviera.Text = user.ruc;
                        this.TxtFechaProceso.Text = System.DateTime.Today.ToString("dd/MM/yyyy");
                        xmlDocumentos = null;
                        lblTotContenedores.Text = string.Empty;
                        this.BtnGrabar.Attributes["disabled"] = "disabled";
                        this.LblRuta.Text=string.Empty;
                        this.lblTotContenedores.Text = string.Empty;

                        var ruc_cgsa = System.Configuration.ConfigurationManager.AppSettings["ruc_cgsa"];
                        if (user.ruc.Trim().Equals(ruc_cgsa.Trim()))
                        {
                            this.TxtLineaNaviera.Text = string.Empty;
                            this.buscar_linea.Visible = true;
                        }
                        else
                        {
                            this.TxtLineaNaviera.Text = user.ruc;
                            this.buscar_linea.Visible = false;
                            //rucempresa = user.ruc;
                        }
                        
                    }
                    else
                    {
                        this.TxtLineaNaviera.Text = string.Empty;
                        this.buscar_linea.Visible = false;
                    }


                    sinresultado.Visible = false;

               

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }

        #endregion

        #region "Eventos Controles "

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                   
                    this.BtnGrabar.Attributes.Add("disabled", "disabled");

                    MostrarLoading();

                    string fileName = string.Empty;
                    string conStr = string.Empty;
                    string lineerror = string.Empty;
                    string referror = string.Empty;

                    int i = 0;
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }

                    if (string.IsNullOrEmpty(this.TxtLineaNaviera.Text))
                    {
                        this.Alerta("Debe especificar la líneas naviera");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtLineaNaviera.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.Txtautorizacion.Text))
                    {
                        this.Alerta("Debe especificar la autorización de salida");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.Txtautorizacion.Focus();   
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtCantidad.Text))
                    {
                        this.Alerta("Debe especificar la  cantidad de contenedores autorizados");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtCantidad.Focus();
                        return;
                    }

                    Int64 CantidadAutorizados = 0;

                    if (!Int64.TryParse(this.TxtCantidad.Text, out CantidadAutorizados))
                    {
                        this.Alerta("Debe especificar la  cantidad de contenedores autorizados");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtCantidad.Focus();
                        return;
                    }

                    this.TxtReferencia.Text = this.Referencia.Value.Trim();

                    if (string.IsNullOrEmpty(TxtReferencia.Text))
                    {

                        this.BtnGrabar.Attributes.Remove("disabled");
                        this.Alerta("Debe especificar la nave");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtReferencia.Focus();

                        return;
                    }
                  
                    if (fsuploadarchivo.PostedFile == null)
                    {
                        this.Alerta("Debe seleccionar el archivo CSV a validar..");
                        return;
                    }



                    //subir el archivo validar q sea csv, si existe reemplazarlo
                    var nombrefile = fsuploadarchivo.PostedFile.FileName;
                    if (System.IO.Path.GetExtension(nombrefile).ToUpper() != ".CSV")
                    {
                        this.Alerta("La extensión del archivo debe ser .CSV");
                        return;
                    }
                    if (fsuploadarchivo.PostedFile.ContentLength > 1500000)
                    {
                        this.Alerta("El tamaño del archivo excede el limite [2mb]");
                        return;
                    }

                    LblRuta.Text = fsuploadarchivo.FileName;
                    fileName = LblRuta.Text;

 
                    //leo toda la cadena como string.
                    //var str = new StreamReader(fsuploadarchivo.PostedFile.InputStream).ReadToEnd();
                    var str = this.ContenidoArchivo;
                    str.Replace(",", ";");
                    str = Regex.Replace(str, @"\r\n?|\n", ";");
                    str = Regex.Replace(str, @"\t|\s", string.Empty);
                    str = Regex.Replace(str, ";;", ";");
                    str = Regex.Replace(str, @"'|!|<|>|-|_|""|#|", string.Empty);

                    //nuevo normalizo el string.
                    try
                    {
                        byte[] bytes = Encoding.Default.GetBytes(str);
                        str = Encoding.UTF8.GetString(bytes);
                    }
                    catch
                    {
                        str = Regex.Replace(str, Environment.NewLine, string.Empty);
                    }

                    //intento separado por saltos
                    str = Regex.Replace(str, @"'|!|<|>|-|_|""|#|", string.Empty);
                    List<string> getList = str.Split(';').ToList<String>();
                    if (getList.Count <= 1)
                    {
                        //intento separado por comas
                        getList = str.Split(',').ToList<String>();
                    }

                    //valida en base a la cantidad digitada de contenedores autorizados
                    Int64 nTotalUnidades = getList.Count - 1;
                    Int64 ntotal = 0;
                    if (nTotalUnidades > CantidadAutorizados)
                    {
                        this.Alerta(string.Format("Estimado Cliente, la cantidad de contenedores ingresados: {0} excede en el número de contenedores autorizados {1} en el NAA. Por favor verifique la información.", nTotalUnidades, CantidadAutorizados));
                        return;
                    }
                    //valida si los contenedores ya fueron ingresados en una transacción
                    List<CantidadUnidades> TotalUnidades = CantidadUnidades.CANTIDAD_UNIDADES(this.TxtReferencia.Text.Trim(),this.Txtautorizacion.Text.Trim(),this.TxtLineaNaviera.Text.Trim(), out v_mensaje);
                    if (!string.IsNullOrEmpty(v_mensaje))
                    {
                        sinresultado.Visible = true;
                        var t = this.getUserBySesion();
                        sinresultado.InnerText = string.Format("Ha ocurrido un problema al validar contenedores, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(new ApplicationException(v_mensaje), "Cargar", "BtnAgregar_Click", "Hubo un error al cargar", t.loginname));
                    }

                    foreach (var Det in TotalUnidades)
                    {
                        ntotal = Det.TOTAL;
                    }
                    if ((nTotalUnidades + ntotal) > CantidadAutorizados)
                    {
                        this.Alerta(string.Format("Estimado Cliente, la cantidad de contenedores ingresados: ({0} + {1} unidades procesada ), excede en el número de contenedores autorizados {2} en el NAA. Por favor verifique la información.", nTotalUnidades, ntotal,CantidadAutorizados));
                        return;
                    }


                    //fin validacion unidades

                    if (getList.Count > 3000)
                    {
                        this.Alerta(string.Format("Estimado Cliente, La cantidad máxima de contenedores que puede procesar es [3000] por transacción, el archivo presenta: {0}, por favor corríjalo", getList.Count));
                        return;
                    }

                    DataTable dt = new DataTable();
                    
                    dt.Columns.Add("F1", typeof(String));//CONTENEDOR
                    dt.Columns.Add("F2", typeof(String));//AUTORIZACION
                    dt.Columns.Add("F3", typeof(String));//LINEA
                    dt.Columns.Add("F4", typeof(String));//NAVE

                   

                    StringBuilder tab2 = new StringBuilder();
                    tab2.Append("<UNIT>");

                    foreach (var a in getList)
                    {
                        if (!string.IsNullOrEmpty(a))
                        {
                            DataRow dr = dt.NewRow();
                            dr["F1"] = Server.HtmlEncode(a.Replace("?", string.Empty));
                            dt.Rows.Add(dr);
                            tab2.Append(string.Format("<VALOR  ID_GKEY='{0}' />", Server.HtmlEncode(a.Replace("?", string.Empty))));

                        }
                    }
               
                    tab2.Append("</UNIT>");

                   
                    string XMLValidar_Gkey = tab2.ToString();

                    if (dt.Rows.Count == 0)
                    {
                        this.Alerta("No se encontro informacion, revise el archivo.");
                        return;
                    }

                    //retorna gkey, para saber si estan repetidos
                    List<ValidaContenedor> Retorna_Gkey = ValidaContenedor.Retorna_Gkey(this.TxtReferencia.Text.Trim(), XMLValidar_Gkey.ToString(), out v_mensaje);
                    if (!string.IsNullOrEmpty(v_mensaje))
                    {
                        sinresultado.Visible = true;
                        var t = this.getUserBySesion();
                        sinresultado.InnerText = string.Format("Ha ocurrido un problema al validar contenedores gkey, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(new ApplicationException(v_mensaje), "Cargar", "BtnAgregar_Click", "Hubo un error al cargar", t.loginname));
                    }
                    //arma nuevo xml para validar unidades por gkey
                    StringBuilder tab = new StringBuilder();
                    tab.Append("<CONTENEDORES>");
                    foreach (var Det in Retorna_Gkey)
                    {
                        tab.Append(string.Format("<CONTENEDOR  UNIDAD='{0}'  REFERENCIA='{1}' LINEA='{2}' AUTORIZACION='{3}' />", Det.CNTR_CONSECUTIVO, this.TxtReferencia.Text.Trim(), this.TxtLineaNaviera.Text.Trim(), this.Txtautorizacion.Text.Trim()));

                    }
                    tab.Append("</CONTENEDORES>");
                    string XMLValidar = tab.ToString();

                    //valida si los contenedores ya fueron ingresados en una transacción
                    List<Contenedor> Valida_Unidad = Contenedor.VALIDA_CONTENEDORES(XMLValidar.ToString(), out v_mensaje);
                    if (!string.IsNullOrEmpty(v_mensaje))
                    {
                        sinresultado.Visible = true;
                        var t = this.getUserBySesion();
                        sinresultado.InnerText = string.Format("Ha ocurrido un problema al validar contenedores, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(new ApplicationException(v_mensaje), "Cargar", "BtnAgregar_Click", "Hubo un error al cargar", t.loginname));
                    }
                    lineerror = string.Empty;
                    int nFila = 0;
                    foreach (var Det in Valida_Unidad)
                    {
                        if (nFila == 0)
                        {
                            lineerror = string.Format("Estimado Cliente, el NAA ingresado {0} se encuentra utilizado en la orden de retiro # {1}, en la referencia de nave {2}. Por favor verifique e ingrese el NAA correspondiente. {3} {4}",this.Txtautorizacion.Text, Det.ID, Det.REFERENCIA, System.Environment.NewLine, System.Environment.NewLine);
                        }
                        lineerror = lineerror + string.Format("La unidad: {0}, ya se encuentra en la orden de servicio #: {1}, tiene estado: {2}. Autorización: {3} {4}", Det.CONTENEDOR, Det.ID, Det.PROCESO, Det.AUTORIZACION,System.Environment.NewLine);
                        nFila++;
                    }

                    this.text_cont.Value = string.Empty;
                    if (!string.IsNullOrEmpty(lineerror))
                    {
                        this.text_cont.Value =  lineerror + System.Environment.NewLine;

                    }
                    
                    if (!string.IsNullOrEmpty(text_cont.Value.Trim()))
                    {
                        lblTotContenedores.Text = string.Empty;
                        this.BtnGrabar.Attributes.Add("disabled", "disabled");

                        this.div_error_n4.Visible = true;
                        this.mensaje_errores.Visible = true;
                        this.mensaje_proceso.Visible = false;


                        //this.div_error_n4.Visible = true;
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        return;
                    }
                    else
                    {
                        this.div_error_n4.Visible = false;
                    }
                    
                    //fin de validacion


                    foreach (DataRow row in dt.Rows)
                    {
                        dt.Rows[i]["F2"] = this.Txtautorizacion.Text.Trim();
                        dt.Rows[i]["F3"] = this.TxtLineaNaviera.Text.Trim();
                        dt.Rows[i]["F4"] = this.TxtReferencia.Text.Trim();
                        i++;
                    }

                    dt.Columns[0].ColumnName = "Contenedor";
                    dt.Columns[1].ColumnName = "NumAutSenae";
                    dt.Columns[2].ColumnName = "LineaNaviera";
                    dt.Columns[3].ColumnName = "Referencia";

                    StringWriter swContenedores = new StringWriter();
                    dt.AcceptChanges();
                    dt.TableName = "CNTR";
                    dt.WriteXml(swContenedores);
                    string XmlContenedores = swContenedores.ToString();

                    DataTable dtLisCntr = new DataTable();

                    List<Contenedor> Listado = Contenedor.CONSULTA_CONTENEDORES(swContenedores.ToString(), out v_mensaje);
                    if (!string.IsNullOrEmpty(v_mensaje))
                    {
                        sinresultado.Visible = true;
                        var t = this.getUserBySesion();
                        sinresultado.InnerText = string.Format("Ha ocurrido un problema al validar contenedores, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(new ApplicationException(v_mensaje), "Cargar", "BtnAgregar_Click", "Hubo un error al cargar", t.loginname));
                    }

                    var result_noexsiten = (from p in Listado.AsEnumerable()
                                         where (p.CNTR_CONSECUTIVO == 0)
                                         select new
                                         {
                                             CNTR_CONSECUTIVO = p.CNTR_CONSECUTIVO == 0 ? 0 : p.CNTR_CONSECUTIVO,
                                             CNTR_CONTAINER = p.CNTR_CONTAINER == null ? string.Empty : p.CNTR_CONTAINER,
                                             CNTR_VEPR_REFERENCE = p.CNTR_VEPR_REFERENCE == null ? string.Empty : p.CNTR_VEPR_REFERENCE,
                                             CNTR_GROUP = p.CNTR_GROUP == null ? string.Empty : p.CNTR_GROUP,
                                             CNTR_LINE = p.CNTR_LINE == null ? string.Empty : p.CNTR_LINE,
                                             CNTR_VALIDACION_REF = p.CNTR_VALIDACION_REF,
                                             CNTR_VALIDACION_LINE = p.CNTR_VALIDACION_LINE,
                                             CNTR_NUMAUTSENAE = p.CNTR_NUMAUTSENAE == null ? string.Empty : p.CNTR_NUMAUTSENAE,
                                             CNTR_VAL_N4 = p.CNTR_VAL_N4,
                                             CNTR_ERROR_N4 = p.CNTR_ERROR_N4 == null ? string.Empty : p.CNTR_ERROR_N4,
                                             REF_FINAL = p.REF_FINAL == null ? string.Empty : p.REF_FINAL,
                                             BOOKING = p.BOOKING == null ? string.Empty : p.BOOKING
                                         });


                    var resultreftrue = (from p in Listado.AsEnumerable()
                                         where (p.CNTR_VALIDACION_REF == true && p.CNTR_CONSECUTIVO != 0)
                                         select new
                                         {
                                             CNTR_CONSECUTIVO = p.CNTR_CONSECUTIVO == 0 ? 0 : p.CNTR_CONSECUTIVO,
                                             CNTR_CONTAINER = p.CNTR_CONTAINER == null ? string.Empty : p.CNTR_CONTAINER,
                                             CNTR_VEPR_REFERENCE = p.CNTR_VEPR_REFERENCE == null ? string.Empty : p.CNTR_VEPR_REFERENCE,
                                             CNTR_GROUP = p.CNTR_GROUP == null ? string.Empty : p.CNTR_GROUP,
                                             CNTR_LINE = p.CNTR_LINE == null ? string.Empty : p.CNTR_LINE,
                                             CNTR_VALIDACION_REF = p.CNTR_VALIDACION_REF,
                                             CNTR_VALIDACION_LINE = p.CNTR_VALIDACION_LINE,
                                             CNTR_NUMAUTSENAE = p.CNTR_NUMAUTSENAE == null ? string.Empty : p.CNTR_NUMAUTSENAE,
                                             CNTR_VAL_N4 = p.CNTR_VAL_N4,
                                             CNTR_ERROR_N4 = p.CNTR_ERROR_N4 == null ? string.Empty : p.CNTR_ERROR_N4,
                                             REF_FINAL = p.REF_FINAL == null ? string.Empty : p.REF_FINAL,
                                             BOOKING = p.BOOKING == null ? string.Empty : p.BOOKING
                                         });


                    var resultlinefalse = (from p in Listado.AsEnumerable()
                                           where (p.CNTR_VALIDACION_LINE == false && p.CNTR_CONSECUTIVO != 0)
                                           select new
                                           {
                                               CNTR_CONSECUTIVO = p.CNTR_CONSECUTIVO == 0 ? 0 : p.CNTR_CONSECUTIVO,
                                               CNTR_CONTAINER = p.CNTR_CONTAINER == null ? string.Empty : p.CNTR_CONTAINER,
                                               CNTR_VEPR_REFERENCE = p.CNTR_VEPR_REFERENCE == null ? string.Empty : p.CNTR_VEPR_REFERENCE,
                                               CNTR_GROUP = p.CNTR_GROUP == null ? string.Empty : p.CNTR_GROUP,
                                               CNTR_LINE = p.CNTR_LINE == null ? string.Empty : p.CNTR_LINE,
                                               CNTR_VALIDACION_REF = p.CNTR_VALIDACION_REF,
                                               CNTR_VALIDACION_LINE = p.CNTR_VALIDACION_LINE,
                                               CNTR_NUMAUTSENAE = p.CNTR_NUMAUTSENAE == null ? string.Empty : p.CNTR_NUMAUTSENAE,
                                               CNTR_VAL_N4 = p.CNTR_VAL_N4,
                                               CNTR_ERROR_N4 = p.CNTR_ERROR_N4 == null ? string.Empty : p.CNTR_ERROR_N4,
                                               REF_FINAL = p.REF_FINAL == null ? string.Empty : p.REF_FINAL,
                                               BOOKING = p.BOOKING == null ? string.Empty : p.BOOKING
                                           });

                    foreach (var Det in result_noexsiten)
                    {
                        lineerror = lineerror + string.Format("Contenedor: {0}, no existe o fue retirado.. {1}", Det.CNTR_CONTAINER, System.Environment.NewLine);
                    }

                    foreach (var Det in resultlinefalse)
                    {
                        lineerror = lineerror + string.Format("Linea: {0}, no pertenece a la Referencia: {1} {2}", Det.CNTR_LINE, Det.CNTR_VEPR_REFERENCE, System.Environment.NewLine);
                    }

                    var resultreffalse = (from p in Listado.AsEnumerable()
                                          where (p.CNTR_VALIDACION_REF == false && p.CNTR_CONSECUTIVO != 0)
                                          select new
                                          {
                                              CNTR_CONSECUTIVO = p.CNTR_CONSECUTIVO == 0 ? 0 : p.CNTR_CONSECUTIVO,
                                              CNTR_CONTAINER = p.CNTR_CONTAINER == null ? string.Empty : p.CNTR_CONTAINER,
                                              CNTR_VEPR_REFERENCE = p.CNTR_VEPR_REFERENCE == null ? string.Empty : p.CNTR_VEPR_REFERENCE,
                                              CNTR_GROUP = p.CNTR_GROUP == null ? string.Empty : p.CNTR_GROUP,
                                              CNTR_LINE = p.CNTR_LINE == null ? string.Empty : p.CNTR_LINE,
                                              CNTR_VALIDACION_REF = p.CNTR_VALIDACION_REF,
                                              CNTR_VALIDACION_LINE = p.CNTR_VALIDACION_LINE,
                                              CNTR_NUMAUTSENAE = p.CNTR_NUMAUTSENAE == null ? string.Empty : p.CNTR_NUMAUTSENAE,
                                              CNTR_VAL_N4 = p.CNTR_VAL_N4,
                                              CNTR_ERROR_N4 = p.CNTR_ERROR_N4 == null ? string.Empty : p.CNTR_ERROR_N4,
                                              REF_FINAL = p.REF_FINAL == null ? string.Empty : p.REF_FINAL,
                                              BOOKING = p.BOOKING == null ? string.Empty : p.BOOKING
                                          });

                    foreach (var Det in resultreffalse)
                    {
                        referror = referror + string.Format("Contenedor: {0}, no pertenece a la Referencia: {1} {2}", Det.CNTR_CONTAINER, Det.CNTR_VEPR_REFERENCE, System.Environment.NewLine);
                    }

                    this.text_cont.Value = string.Empty;
                    if (string.IsNullOrEmpty(lineerror))
                    {
                        this.text_cont.Value = referror;
                       
                    }
                    else if (string.IsNullOrEmpty(referror))
                    {
                        this.text_cont.Value = lineerror;
                    }
                    else
                    {
                        this.text_cont.Value = lineerror + System.Environment.NewLine + referror;
                       
                    }

                    if (!string.IsNullOrEmpty(text_cont.Value.Trim()))
                    {
                        this.div_error_n4.Visible = true;
                        this.mensaje_errores.Visible = true;
                        this.mensaje_proceso.Visible = false;

                        lblTotContenedores.Text = string.Empty;
                        this.BtnGrabar.Attributes.Add("disabled", "disabled");

                       
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        return;

                    }
                    else {
                        this.div_error_n4.Visible = false;
                        this.mensaje_errores.Visible = false;
                        this.mensaje_proceso.Visible = true;
                    }

                    DataSet dsRetorno = new DataSet();
                    dsRetorno.Tables.Add(MantenimientoVehiculo.LINQToDataTable(resultreftrue));

                    DtContenedores = dsRetorno.Tables[0];


                    tablePagination.DataSource = dsRetorno.Tables[0];
                    tablePagination.DataBind();

                    lblTotContenedores.Text = dsRetorno.Tables[0].Rows.Count.ToString();

                    if (tablePagination.HeaderRow == null)
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        sinresultado.Visible = true;
                        sinresultado.InnerText = string.Format("Los contenedores del archivo: {0}, no se encontraron en la base de datos, revise el archivo.", fileName);

                        return;
                     }

                    if (dsRetorno.Tables[0].Rows.Count != 0) {
                        this.BtnGrabar.Attributes.Remove("disabled");
                        this.TxtCantidad.Enabled = false;
                    }
                   

                }
                catch (Exception ex)
                {
                    sinresultado.Visible = true;
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema al cargar datos de excel, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "BtnAgregar_Click", "Hubo un error al cargar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
        
                }

            }


        }

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
                    tablePagination.DataSource = DtContenedores.AsDataView();
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

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    this.BtnAgregar.Attributes.Add("disabled", "disabled");

                    string fileName = string.Empty;
                    string conStr = string.Empty;
                    string lineerror = string.Empty;
                    string referror = string.Empty;

                    //int i = 0;
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }

                    if (DtContenedores.Rows.Count == 0)
                    {
                      
                        this.Alerta("No se encontro detalle de contenedores para grabar");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        return;
                    }

                    int CantidadAutorizados = 0;

                    if (!int.TryParse(this.TxtCantidad.Text, out CantidadAutorizados))
                    {
                        this.Alerta("Debe especificar la  cantidad de contenedores autorizados");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtCantidad.Focus();
                        return;
                    }


                    fileName = this.LblRuta.Text;
                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    string Autorizacion = DtContenedores.Rows[0]["CNTR_NUMAUTSENAE"].ToString();
                    string Referencia = DtContenedores.Rows[0]["CNTR_VEPR_REFERENCE"].ToString();

                    objConteendor = new Contenedor();
                    objConteendor.AUTORIZACION = Autorizacion;
                    objConteendor.REFERENCIA = Referencia;
                    objConteendor.ARCHIVO = fileName;
                    objConteendor.ESTADO = true;
                    objConteendor.USUARIO_CRE = sUser.loginname;
                    objConteendor.LINEA_NAVIERA = this.TxtLineaNaviera.Text.Trim();
                    objConteendor.MAIL = sUser.email;
                    objConteendor.CANTIDAD_AUTORIZADA = CantidadAutorizados;
                    for (int i = 0; i <= DtContenedores.Rows.Count - 1; i++)
                    {
                        objDetConteendor = new Contenedor_Detalle();
                        objDetConteendor.CONTENEDOR = DtContenedores.Rows[i]["CNTR_CONTAINER"].ToString();
                        objDetConteendor.GKEY = Int64.Parse(DtContenedores.Rows[i]["CNTR_CONSECUTIVO"].ToString());
                        objDetConteendor.ESTADO = true;
                        objDetConteendor.GRUPO = DtContenedores.Rows[i]["CNTR_GROUP"].ToString();
                        objDetConteendor.REF_FINAL = DtContenedores.Rows[i]["REF_FINAL"].ToString();

                        objConteendor.Detalle.Add(objDetConteendor);

                        
                    }

                    var nProceso = objConteendor.SaveTransaction(out v_mensaje);
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {
                        this.Alerta("Error...no se pudo generar la transacción");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        return;

                    }
                    else
                    {
                       
                        this.BtnAgregar.Attributes.Add("disabled", "disabled");
                        this.BtnGrabar.Attributes.Add("disabled", "disabled");
                        this.TxtNumeroOrden.Text = nProceso.Value.ToString().Trim();
                       
                        this.Alerta(string.Format("Se procedio a generar la orden de retiro # {0}. Usted recibirá un mail con la confirmación del estado (Pendientes/Procesados) de la orden de retiro ingresada.", nProceso.Value.ToString().Trim()));
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                       

                    }
                    
                }
                catch (Exception ex)
                {
                    sinresultado.Visible = true;
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema al grabar, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "BtnGrabar_Click", "Hubo un error al grabar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                   
                }


            }

        }
        #endregion







    }
}