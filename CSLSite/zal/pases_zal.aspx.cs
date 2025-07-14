using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CSLSite.N4Object;
using ConectorN4;
using System.Globalization;
using System.Web.Script.Services;
using System.Configuration;
using Newtonsoft.Json;
using csl_log;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using ClsProformas;
using System.Web.UI.HtmlControls;
using CSLSite.app_start;

namespace CSLSite.zal
{
    public partial class pases_zal : System.Web.UI.Page 
    {
        #region "Variables de sesiones"

        private Activar_Turnos objParametro = new Activar_Turnos();
        public String Activar
        {
            get { return (String)Session["Activar"]; }
            set { Session["Activar"] = value; }
        }

        private DataTable p_drPlaca
        {
            get
            {
                return (DataTable)Session["drPlacaPPWeb"];
            }
            set
            {
                Session["drPlacaPPWeb"] = value;
            }

        }

        private DataTable p_drChoferFilter
        {
            get
            {
                return (DataTable)Session["drChoferFilterPPWeb"];
            }
            set
            {
                Session["drChoferFilterPPWeb"] = value;
            }

        }

        private DataTable p_drChofer
        {
            get
            {
                return (DataTable)Session["drChoferPPWeb"];
            }
            set
            {
                Session["drChoferPPWeb"] = value;
            }

        }

        public DataTable dtTurnosZAL
        {
            get { return (DataTable)Session["dtTurnosZAL"]; }
            set { Session["dtTurnosZAL"] = value; }
        }

        public DataTable dtDetpasezal
        {
            get { return (DataTable)Session["dtDetpasezal"]; }
            set { Session["dtDetpasezal"] = value; }
        }

        public DataTable dtListaAsumeCliente
        {
            get { return (DataTable)Session["dtListaAsumeCliente"]; }
            set { Session["dtListaAsumeCliente"] = value; }
        }

        public DataTable dtListaBooking
        {
            get { return (DataTable)Session["dtListaBooking"]; }
            set { Session["dtListaBooking"] = value; }
        }

        public String emailCliente
        {
            get { return (String)Session["emailCliente"]; }
            set { Session["emailCliente"] = value; }
        }

        public Boolean ClienteBloqueado
        {
            get { return (Boolean)Session["ValClienteBloqueado"]; }
            set { Session["ValClienteBloqueado"] = value; }
        }

        public Boolean ClienteTipo
        {
            get { return (Boolean)Session["ValClienteTipo"]; }
            set { Session["ValClienteTipo"] = value; }
        }

        #endregion


        private static int cont = 0;
        private static int pos = 0;
        WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA;


        #region "Conexion N4"

        private static SqlConnection ConexionN4Middleware()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
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
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
                this.agencia.Value = user.ruc;
                this.emailCliente = user.email;
                this.ClienteBloqueado = user.bloqueo_cartera; // si es true esta bloqueado si es false no esta bloqueado
                this.ClienteTipo = user.IsCredito;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    dtListaBooking = new DataTable();
                    dtListaBooking.Columns.Add("BOOKING");
                    dtListaBooking.Columns.Add("REFERENCIA");
                    dtListaBooking.Columns.Add("LINEA");
                    dtListaBooking.Columns.Add("MENSAJE");
                    RepeaterBooking.DataSource = dtListaBooking;
                    RepeaterBooking.DataBind();
                    fecsalida.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    IniDsChofer("");
                    IniDsPlaca("");


                    this.mensaje.Visible = false;
                    this.saldo.InnerText = string.Empty;

                    objParametro.Obtener_Parametro(out string error);
                    this.Activar = objParametro.config_value;

                    LlenaComboDepositos();
                    Session["ReferenciaZAL"] = null;
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "pago_terceros", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
            }
        }

        #region "Carga placa chofer"
        private void IniDsPlaca(string empresa)
        {
            p_drPlaca = pasePuerta.GetPlacainfozal(empresa);
        }

        private void IniDsChofer(string empresa)
        {
            DataSet dsRetorno = new DataSet();
            //WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
            var dtRetorno = pasePuerta.GetChoferinfozal(empresa);
            if (dtRetorno.Rows.Count > 0)
            {
                DataView myDataView = dtRetorno.DefaultView;
                myDataView.Sort = "CHOFER ASC";

                p_drChofer = myDataView.ToTable();
                p_drChoferFilter = p_drChofer;
            }
        }

        [System.Web.Services.WebMethod]
        public static string[] GetChoferList(string prefix)
        {

            WFCPASEPUERTA.WFCPASEPUERTAClient WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            DataSet dsRetorno = new DataSet();
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();

            DataTable DTRESULT = new DataTable();
            DTRESULT = (DataTable)HttpContext.Current.Session["drChoferFilterPPWeb"];//drChoferPPWeb"];

            if (DTRESULT != null)
            {
                var list = /*(from currentStat in DTRESULT.AsEnumerable()
                        where currentStat.Field<String>("CHOFER") != null && currentStat.Field<String>("CHOFER").Contains(prefixText.ToUpper())
                        select currentStat.Field<String>("IDCHOFER") + " - " + currentStat.Field<String>("CHOFER")).ToList().Take(5);*/

                (from currentStat in DTRESULT.Select("CHOFER like '%" + prefix + "%'").AsEnumerable()
                 select currentStat.Field<String>("CHOFER")).ToList().Take(5);

                string[] prefixTextArray = list.ToArray<string>();
                return prefixTextArray;
            }
            else
            {
                ArrayList myAL = new ArrayList();
                // Add stuff to the ArrayList.
                string[] myArr = (String[])myAL.ToArray(typeof(string));
                string[] prefixTextArray2 = myArr.ToArray<string>();
                return prefixTextArray2;
            }
            //Return Selected Products
        }
        #endregion

        #region "Disponibilidad de turnos"
        public static DataTable N4_P_Cons_Turnos_Disponibles_ZAL(string fecha)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "N4_P_Cons_Turnos_Disponibles_ZAL";
                coman.Parameters.AddWithValue("@FECHA", fecha);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoEmails", "SP_GET_INFO_EMAILS_PPWEB", DateTime.Now.ToShortDateString());
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d;
        }
        #endregion

        protected void fecsalida_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                if (!DateTime.TryParseExact(fecsalida.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                {
                    this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", fecsalida.Text));
                    fecsalida.Focus();
                    return;
                }
                dtTurnosZAL = new DataTable();
                dtTurnosZAL = N4_P_Cons_Turnos_Disponibles_ZAL(fechasal.ToString("yyyy-MM-dd"));
                dtTurnosZAL.Columns.Add("CHECK");


            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "pago_terceros", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {

                if (dtListaBooking.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + "Aun no Agrega Bookings a la lista." + "');getGifOcultaBuscar();", true);
                    return;
                }

                string mail = string.Empty;
                string error = string.Empty;
                string correoBackUp = string.Empty;
                string mail_em = string.Empty;
                string sUser_email = string.Empty;
                string destinatarios = string.Empty;
                string mensajeerror = string.Empty;

                string mailContenedores = string.Empty;
                int cont = 0;
                int add = 0;
                string mensaje = null;
                string mensajeu = null;

                for (int i = 0; i < dtListaBooking.Rows.Count; i++)
                {
                    if (!man_pro_expo.AddBookAutoriza(
                        dtListaBooking.Rows[i]["BOOKING"].ToString(),
                        dtListaBooking.Rows[i]["REFERENCIA"].ToString(),
                        dtListaBooking.Rows[i]["LINEA"].ToString(),
                        Page.User.Identity.Name.ToUpper(),
                        out mensaje))
                    {
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + mensaje + "');", true);
                        cont += 1;
                        mensajeu = mensaje;
                        if (mensaje.Contains("ya se encuentra autorizado"))
                            dtListaBooking.Rows[i]["MENSAJE"] = "Registro duplicado";
                        else
                            dtListaBooking.Rows[i]["MENSAJE"] = mensaje.Length > 30 ? mensaje.Substring(1, 30) : mensaje;
                    }
                    else
                    {


                        //if (!string.IsNullOrEmpty(error))
                        //{
                        //    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", error, true);
                        //    return;
                        //}
                        add += 1;
                        dtListaBooking.Rows[i]["MENSAJE"] = "Registrado con éxito";
                    }
                }

                RepeaterBooking.DataSource = dtListaBooking;
                RepeaterBooking.DataBind();

                if (cont == 0)
                    Response.Write("<script language='JavaScript'>var r=alert('Transacción exitosa todos los bookings fueron registrados.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>");
                else
                {
                    this.Alerta(string.Format("Algunos registros reportaron error verifique los mensajes: bookings registrados {0}, bookings con error {1}.", add, cont));
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + mensajeu + "');", true);
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "booking_atoriza", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + ex.Message + "');", true);
            }
        }

        #region "Grilla Repeater Detalla de pases a cancelar o modificar"

        protected void RepeaterBooking_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            /*valida cotizacion*/
            var usn = new usuario();
            usn = this.getUserBySesion();

            if (usn == null)
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                var number = log_csl.save_log<Exception>(ex, "proforma", "btnAsumirBook_Click", "No usuario", Request.UserHostAddress);
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
            }

            if (this.Activar == "SI")/*si esta activa la validacion de cobros de turnos zal*/
            {
                if (usn.IsCredito)  //validaciones usuario de credito
                {
                    if (e.Item.ItemType == ListItemType.Header)
                    {
                        Label lbl_tit_liquidacion = (Label)e.Item.FindControl("lbl_tit_liquidacion");
                        lbl_tit_liquidacion.Visible = false;

                        Label lbl_tit_estado = (Label)e.Item.FindControl("lbl_tit_estado");
                        lbl_tit_estado.Visible = false;

                        Label lbl_tit_imprimir = (Label)e.Item.FindControl("lbl_tit_imprimir");
                        lbl_tit_imprimir.Visible = false;

                    }
                    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
                    {
                        Label lbl_liquidacion = (Label)e.Item.FindControl("lbl_liquidacion");
                        lbl_liquidacion.Visible = false;

                        Label lbl_estado = (Label)e.Item.FindControl("lbl_estado");
                        lbl_estado.Visible = false;

                        ImageButton btnImprimirProforma = (ImageButton)e.Item.FindControl("btnImprimirProforma");
                        btnImprimirProforma.Visible = false;
                    }

                }

               // long IdDepot = long.Parse(cmbDeposito.SelectedValue);

                long IdDepot = 0;
                if (this.cmbDeposito.SelectedIndex != -1)
                {
                    IdDepot = long.Parse(cmbDeposito.SelectedValue);
                }

                if (IdDepot == 5)//OPACIFIC
                {
                    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
                    {
                        TextBox TxtChofer = (TextBox)e.Item.FindControl("txtchofer");
                        TextBox TxtPlaca = (TextBox)e.Item.FindControl("txtplaca");
                        TxtChofer.Enabled = true;
                        TxtPlaca.Enabled = true;
                    }
                }
                else
                {
                    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
                    {
                        TextBox TxtChofer = (TextBox)e.Item.FindControl("txtchofer");
                        TextBox TxtPlaca = (TextBox)e.Item.FindControl("txtplaca");
                        TxtChofer.Enabled = true;
                        TxtPlaca.Enabled = true;
                    }
                }
   
            }
            else {

                if (e.Item.ItemType == ListItemType.Header)
                {
                    Label lbl_tit_liquidacion = (Label)e.Item.FindControl("lbl_tit_liquidacion");
                    lbl_tit_liquidacion.Visible = false;

                    Label lbl_tit_estado = (Label)e.Item.FindControl("lbl_tit_estado");
                    lbl_tit_estado.Visible = false;

                    Label lbl_tit_imprimir = (Label)e.Item.FindControl("lbl_tit_imprimir");
                    lbl_tit_imprimir.Visible = false;

                }
                if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
                {
                    Label lbl_liquidacion = (Label)e.Item.FindControl("lbl_liquidacion");
                    lbl_liquidacion.Visible = false;

                    Label lbl_estado = (Label)e.Item.FindControl("lbl_estado");
                    lbl_estado.Visible = false;

                    ImageButton btnImprimirProforma = (ImageButton)e.Item.FindControl("btnImprimirProforma");
                    btnImprimirProforma.Visible = false;
                }


                long IdDepot = 0;
                if (this.cmbDeposito.SelectedIndex != -1) {
                     IdDepot = long.Parse(cmbDeposito.SelectedValue);
                }
                if (IdDepot == 2)//OPACIFIC
                {
                    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
                    {
                        TextBox TxtChofer = (TextBox)e.Item.FindControl("txtchofer");
                        TextBox TxtPlaca = (TextBox)e.Item.FindControl("txtplaca");
                        TxtChofer.Enabled = true;
                        TxtPlaca.Enabled = true;
                    }
                }
                else
                {
                    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
                    {
                        TextBox TxtChofer = (TextBox)e.Item.FindControl("txtchofer");
                        TextBox TxtPlaca = (TextBox)e.Item.FindControl("txtplaca");
                        TxtChofer.Enabled = true;
                        TxtPlaca.Enabled = true;
                    }
                }

            }




        }

        protected void RepeaterBooking_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                /*valida cotizacion*/
                var usn = new usuario();
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                usn = this.getUserBySesion();

                if (usn == null)
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "pases_zal", "RepeaterBooking_ItemCommand", "No usuario", Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
                if (token == null)
                {
                    this.Alerta("Estimado Cliente,Su sesión ha expirado, sera redireccionado a la pagina de login", true);
                    System.Web.Security.FormsAuthentication.SignOut();
                    Session.Clear();
                    return;
                }


                if (e.CommandName.Contains("Delete")) //breakpoint on this line
                {
                    String value = dtDetpasezal.Rows[e.Item.ItemIndex]["IDPZAL"].ToString();
                    String paseLinea = dtDetpasezal.Rows[e.Item.ItemIndex]["ID_PASE_LINEA"].ToString();
                    string mensaje = null;

                    if (!man_pro_expo.AddCancelPaseZAL(value, Page.User.Identity.Name, out mensaje))
                    {
                        this.Alerta(mensaje);
                        return;
                    }
                    else
                    {
                        this.Alerta("Pase: " + value + ", cancelado exitosamente.");

                        
                        long IdDepot = long.Parse(cmbDeposito.SelectedValue);
                        if (cmbDeposito.SelectedValue == "-1")
                        {
                                
                        }

                        if (IdDepot == 1)//ZAL
                        {
                               
                        }

                        if (IdDepot == 5)//OPACIFIC
                        {
                            btnCancelaPase(paseLinea);
                        }

                        if (IdDepot == 3)//ZAL
                        {

                        }


                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime fechasal = new DateTime();
                        if (!DateTime.TryParseExact(fecsalida.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                        {
                            this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", fecsalida.Text));
                            fecsalida.Focus();
                            return;
                        }

                        if (this.Activar == "SI")/*si esta activa la validacion de cobros de turnos zal*/
                        {
                            //validaciones usuario de contado
                            if (!usn.IsCredito)
                            {

                                /*muestra mensaje de saldo a favor*/
                                List<Saldos> Lista = Saldos.Get_Saldo(usn.ruc, IdDepot);
                                var xList = Lista.FirstOrDefault();
                                if (xList != null)
                                {
                                    decimal nSaldo = xList.saldo_final;
                                    if (nSaldo != 0)
                                    {
                                        this.mensaje.Visible = true;
                                        this.saldo.InnerText = xList.leyenda;
                                    }
                                    else
                                    {
                                        this.mensaje.Visible = false;
                                        this.saldo.InnerText = string.Empty;
                                    }
                                }

                                var detpasezal = man_pro_expo.GetDetallePaseZALContado(nbrboo.Value, xreferencia.Value, fechasal.ToString("yyyy-MM-dd"), xlinea.Value, IdDepot);
                                if (detpasezal.Rows.Count == 0 || detpasezal == null)
                                {
                                    RepeaterBooking.DataSource = null;
                                    RepeaterBooking.DataBind();
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                                    return;
                                }

                                dtDetpasezal = new DataTable();
                                dtDetpasezal = detpasezal;
                                RepeaterBooking.DataSource = detpasezal;
                                RepeaterBooking.DataBind();


                            }
                            else
                            {   /*credito*/
                                var detpasezal = man_pro_expo.GetDetallePaseZAL(nbrboo.Value, xreferencia.Value, fechasal.ToString("yyyy-MM-dd"), xlinea.Value, IdDepot);
                                if (detpasezal.Rows.Count == 0 || detpasezal == null)
                                {
                                    RepeaterBooking.DataSource = null;
                                    RepeaterBooking.DataBind();
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                                    return;
                                }

                                dtDetpasezal = new DataTable();
                                dtDetpasezal = detpasezal;
                                RepeaterBooking.DataSource = detpasezal;
                                RepeaterBooking.DataBind();

                            }
                        }
                        else
                        {
                            var detpasezal = man_pro_expo.GetDetallePaseZAL(nbrboo.Value, xreferencia.Value, fechasal.ToString("yyyy-MM-dd"), xlinea.Value, IdDepot);
                            if (detpasezal.Rows.Count == 0 || detpasezal == null)
                            {
                                RepeaterBooking.DataSource = null;
                                RepeaterBooking.DataBind();
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                                return;
                            }

                            dtDetpasezal = new DataTable();
                            dtDetpasezal = detpasezal;
                            RepeaterBooking.DataSource = detpasezal;
                            RepeaterBooking.DataBind();
                        }
                                                

                    }

                }
                else if (e.CommandName == "Imprimir") //imprimir pase a puerta
                {

                   // string cEstado = dtDetpasezal.Rows[e.Item.ItemIndex]["ESTADO_PAGO"].ToString();
                    string cEstado = "PAGADO";
                    if (cEstado.Equals("PENDIENTE DE PAGO"))
                    {
                        this.Alerta(string.Format("LA LIQUIDACIÓN DEBE ESTAR PAGADA, PARA PODER VISUALIZAR EL PASE PUERTA"));
                        return;
                       
                    }
                    else {
                        StringWriter xmlPPWeb = new StringWriter();
                        Int64 value = Convert.ToInt64(dtDetpasezal.Rows[e.Item.ItemIndex]["IDPZAL"].ToString());
                        var query = from myRow in dtDetpasezal.AsEnumerable()
                                    where myRow.Field<Int64>("IDPZAL") == value
                                    select myRow;
                        DataTable tbresult = new DataTable();
                        tbresult = query.AsDataView().ToTable();
                        tbresult.TableName = "PaseWeb";
                        tbresult.WriteXml(xmlPPWeb);

                        Session["idpaseszal"] = xmlPPWeb.ToString();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "openPopReporte();", true);
                    }

                   
                }
                else if (e.CommandName == "Imprimir_proforma") //imprime proforma o liquidacion
                {
                    string cEstado = dtDetpasezal.Rows[e.Item.ItemIndex]["ESTADO_PAGO"].ToString();
                    if (cEstado.Equals("PAGADO"))
                    {
                        this.Alerta(string.Format("LIQUIDACIÓN YA FUE PAGADA, NO PODRÁ VISUALIZARLA"));
                        return;
                    }

                    Int64 value = Convert.ToInt64(dtDetpasezal.Rows[e.Item.ItemIndex]["IdProforma"].ToString());

                    /*se imprime proforma de contado*/
                    var sid = QuerySegura.EncryptQueryString(value.ToString());
                    this.Popup(string.Format("../zal/printproforma_zal.aspx?sid={0}", sid));

                }
                else if (e.CommandName == "Update")
                {

                    TextBox txtchofer = (Object)e.Item.FindControl("txtchofer") as TextBox;
                    TextBox txtplaca = (Object)e.Item.FindControl("txtplaca") as TextBox;
                    Label lblFechaSalida = (Object)e.Item.FindControl("lblFechaSalida") as Label;
                    Label lblTurno = (Object)e.Item.FindControl("lblTurno") as Label;

                    DataTable DTRESULT2 = new DataTable();
                    DTRESULT2 = (DataTable)HttpContext.Current.Session["drChoferFilterPPWeb"];
                    if (DTRESULT2 != null)
                    {
                        if (DTRESULT2.Rows.Count > 0)
                        {
                            var valcho = (from currentStat in DTRESULT2.Select("CHOFER = '" + txtchofer.Text.Trim() + "'").AsEnumerable()
                                          select currentStat.Field<String>("CHOFER")).ToList().Take(5);
                            string[] aChof = valcho.ToArray<string>();
                            if (aChof.Count() > 0)
                            {
                                txtchofer.Text = aChof[0].ToString();
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(txtchofer.Text))
                                {
                                    this.Alerta("El Chofer: " + txtchofer.Text + " no es valido.");
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                                    txtchofer.Focus();
                                    return;
                                }
                            }
                        }
                    }

                    var wPlaca = (from row in p_drPlaca.AsEnumerable()
                                  where row.Field<String>("PLACA") != null && row.Field<String>("PLACA").Trim().Equals(txtplaca.Text.ToString().Trim().ToUpper())
                                  select row.Field<String>("PLACA")).Count();

                    if ((int)wPlaca <= 0)
                    {
                        this.Alerta("La Placa: " + txtplaca.Text + ", no es valida.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        txtplaca.Focus();
                        return;
                    }
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime fechasal = new DateTime();
                    if (!DateTime.TryParseExact(lblFechaSalida.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        //this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", lblFechaSalida.Text));
                        //return;
                    }
                    var v_turno = pasePuerta.GetValTurno(fechasal.ToString("yyyy-MM-dd"), lblTurno.Text.Substring(3, 6).Trim());
                    if (v_turno.Rows[0]["V_TURNO"].ToString() == "1")
                    {
                        var v_msg = v_turno.Rows[0]["MENSAJE"].ToString().Substring(0, 130);
                        this.Alerta(v_msg);
                        return;
                    }

                    String value = dtDetpasezal.Rows[e.Item.ItemIndex]["IDPZAL"].ToString();
                    string mensaje = null;

                    if (!man_pro_expo.AddUpdatePaseZAL(value, Page.User.Identity.Name, txtchofer.Text.ToUpper().Trim(), txtplaca.Text.ToUpper().Trim(), out mensaje))
                    {
                        this.Alerta(mensaje);
                        return;
                    }
                    else
                    {

                        string cMensajeCise = string.Empty;

                        long IdDepot = long.Parse(cmbDeposito.SelectedValue);

                        if (IdDepot == 5)//CISE
                        {
                            try
                            {
                                String paseCGSA = dtDetpasezal.Rows[e.Item.ItemIndex]["IDPZAL"].ToString();
                                String paseLinea = dtDetpasezal.Rows[e.Item.ItemIndex]["ID_PASE_LINEA"].ToString();
                                String fechaTurno = dtDetpasezal.Rows[e.Item.ItemIndex]["FECHA_TURNO"].ToString();
                                String booking = dtDetpasezal.Rows[e.Item.ItemIndex]["BOOKING"].ToString();
                                String ChoferSelect = txtchofer.Text.ToUpper().Trim();
                                String cChofer = string.Empty;
                                String cNombreChofer = string.Empty;
                                if (ChoferSelect.Split('-').ToList().Count > 1)
                                {

                                    cChofer = ChoferSelect.Split('-').ToList()[0].Trim();
                                    cNombreChofer = ChoferSelect.Split('-').ToList()[1].Trim();
                                }
                                else
                                {
                                    cChofer = string.Empty;
                                    cNombreChofer = string.Empty;
                                }

                                app_start.RepContver.ServicioCISE _servicio = new app_start.RepContver.ServicioCISE();
                                Dictionary<string, string> obj = new Dictionary<string, string>();

                                usuario ClsUsuario; 
                                try
                                {
                                    var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                                    ClsUsuario = ClsUsuario_;
                                }
                                catch
                                {
                                    Response.Redirect("../login.aspx", false);
                                    return;
                                }

                                obj.Add("AN_ID_TURNO_REFERENCIA", paseLinea);
                                obj.Add("AV_ID_TIPO_ORDEN", "E");
                                obj.Add("AN_ID_TURNO", paseCGSA);
                                obj.Add("AD_FECHA_TURNO", fechaTurno);
                                obj.Add("AN_ID_NAVIERA", "13");
                                obj.Add("AV_NAVIERA_DESCRIPCION", "PIL");
                                obj.Add("AV_BOOKING", booking);
                                obj.Add("AV_BL", booking);
                                obj.Add("AV_ID_TIPO_CTNER", "");
                                obj.Add("AV_CONTENEDOR", "");
                                obj.Add("AV_CLIENTE_RUC", ClsUsuario.ruc);
                                obj.Add("AV_CLIENTE_NOMBRE", ClsUsuario.nombres);
                                obj.Add("AV_TRANSPORTISTA_RUC", cChofer);
                                obj.Add("AV_TRANSPORTISTA_DESCRIPCION", cNombreChofer);
                                obj.Add("AV_CHOFER_CEDULA", cChofer);
                                obj.Add("AV_CHOFER_NOMBRE", cNombreChofer);
                                obj.Add("AV_PLACAS", txtplaca.Text.ToUpper().Trim());                               
                                obj.Add("AV_BUQUE", "");
                                obj.Add("AV_VIAJE", "");

                                //obj.Add("cita_id", paseLinea.ToString());
                                //obj.Add("cita_contecon", paseCGSA.ToString());

                                CSLSite.app_start.RepContver.Respuesta _result = _servicio.Peticion("ACTUALIZA_CITA", obj);
                                if (_result.result.estado != 1)
                                {
                                    Exception ex;
                                    ex = new Exception("Error al actualizar cita en OPACIFIC con Servicio Web; Cita: " + paseCGSA.ToString());
                                    var number = log_csl.save_log<Exception>(ex, "pases_zal", "RepeaterBooking_ItemCommand()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                    this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " Mensaje Servicio: " + _result.result.mensaje + " #Error: " + number);
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                                    //return;
                                }
                                cMensajeCise = _result.result.mensaje;
                            }
                            catch (Exception ex)
                            {
                                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "btnCancelaPase()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                            }
                        }

                        this.Alerta("Pase: " + value + ", actualizado exitosamente..." + cMensajeCise);

                        if (this.Activar == "SI")/*si esta activa la validacion de cobros de turnos zal*/
                        {
                            //validaciones usuario de contado
                            if (!usn.IsCredito)
                            {
                                /*muestra mensaje de saldo a favor*/
                                List<Saldos> Lista = Saldos.Get_Saldo(usn.ruc, IdDepot);
                                var xList = Lista.FirstOrDefault();
                                if (xList != null)
                                {
                                    decimal nSaldo = xList.saldo_final;
                                    if (nSaldo != 0)
                                    {
                                        this.mensaje.Visible = true;
                                        this.saldo.InnerText = xList.leyenda;
                                    }
                                    else
                                    {
                                        this.mensaje.Visible = false;
                                        this.saldo.InnerText = string.Empty;
                                    }
                                }

                                var detpasezal = man_pro_expo.GetDetallePaseZALContado(nbrboo.Value, xreferencia.Value, fechasal.ToString("yyyy-MM-dd"), xlinea.Value, IdDepot);
                                if (detpasezal.Rows.Count == 0 || detpasezal == null)
                                {
                                    RepeaterBooking.DataSource = null;
                                    RepeaterBooking.DataBind();
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                                    return;
                                }

                            }
                            else/*credito*/
                            {
                                var detpasezal = man_pro_expo.GetDetallePaseZAL(nbrboo.Value, xreferencia.Value, fechasal.ToString("yyyy-MM-dd"), xlinea.Value, IdDepot);
                                if (detpasezal.Rows.Count == 0 || detpasezal == null)
                                {
                                    RepeaterBooking.DataSource = null;
                                    RepeaterBooking.DataBind();
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                                    return;
                                }

                                dtDetpasezal = new DataTable();
                                dtDetpasezal = detpasezal;
                                RepeaterBooking.DataSource = detpasezal;
                                RepeaterBooking.DataBind();
                            }

                        }
                        else
                        {
                            var detpasezal = man_pro_expo.GetDetallePaseZAL(nbrboo.Value, xreferencia.Value, fechasal.ToString("yyyy-MM-dd"), xlinea.Value, IdDepot);
                            if (detpasezal.Rows.Count == 0 || detpasezal == null)
                            {
                                RepeaterBooking.DataSource = null;
                                RepeaterBooking.DataBind();
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                                return;
                            }

                            dtDetpasezal = new DataTable();
                            dtDetpasezal = detpasezal;
                            RepeaterBooking.DataSource = detpasezal;
                            RepeaterBooking.DataBind();
                        }

                        if (this.Activar == "SI")/*si esta activa la validacion de cobros de turnos zal*/
                        {
                            //string cEstado = dtDetpasezal.Rows[e.Item.ItemIndex]["ESTADO_PAGO"].ToString();
                            string cEstado = "PAGADO";
                            if (cEstado.Equals("PENDIENTE DE PAGO"))
                            {
                                this.Alerta(string.Format("LA LIQUIDACIÓN DEBE ESTAR PAGADA, PARA PODER VISUALIZAR EL PASE PUERTA"));
                                return;
                            }
                            else
                            {
                                StringWriter xmlPPWeb = new StringWriter();
                                Int64 value_ = Convert.ToInt64(dtDetpasezal.Rows[e.Item.ItemIndex]["IDPZAL"].ToString());
                                var query = from myRow in dtDetpasezal.AsEnumerable()
                                            where myRow.Field<Int64>("IDPZAL") == value_
                                            select myRow;
                                DataTable tbresult = new DataTable();
                                tbresult = query.AsDataView().ToTable();
                                tbresult.TableName = "PaseWeb";
                                tbresult.WriteXml(xmlPPWeb);

                                Session["idpaseszal"] = xmlPPWeb.ToString();
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "openPopReporte();", true);
                            }

                        }
                        else
                        {
                            StringWriter xmlPPWeb = new StringWriter();
                            Int64 value_ = Convert.ToInt64(dtDetpasezal.Rows[e.Item.ItemIndex]["IDPZAL"].ToString());
                            var query = from myRow in dtDetpasezal.AsEnumerable()
                                        where myRow.Field<Int64>("IDPZAL") == value_
                                        select myRow;
                            DataTable tbresult = new DataTable();
                            tbresult = query.AsDataView().ToTable();
                            tbresult.TableName = "PaseWeb";
                            tbresult.WriteXml(xmlPPWeb);

                            Session["idpaseszal"] = xmlPPWeb.ToString();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "openPopReporte();", true);
                        }
                       

                    }
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "RepeaterBooking_ItemCommand()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        #endregion


        #region "Busca pases a modificar"

        protected void btnAsumirBook_Click(object sender, EventArgs e)
        {
            try
            {
                long IdDepot = long.Parse(cmbDeposito.SelectedValue);
                this.mensaje.Visible = false;
                this.saldo.InnerText = string.Empty;

                if (string.IsNullOrEmpty(nbrboo.Value))
                {
                    this.Alerta("Fata el Booking.");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                    return;
                }
                if (string.IsNullOrEmpty(xreferencia.Value))
                {
                    this.Alerta("Falta la Referencia.");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                    return;
                }
                if (string.IsNullOrEmpty(xlinea.Value))
                {
                    this.Alerta("Falta la Linea.");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                    return;
                }
                if (string.IsNullOrEmpty(fecsalida.Text))
                {
                    this.Alerta("Falta la Fecha de Salida.");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                    return;
                }

                if (cmbDeposito.SelectedValue == "-1")
                {
                    this.Alerta("Seleccione un depósito.");
                    cmbDeposito.Focus();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                    return;
                }

                //if (IdDepot == 1)//ZAL
                //{
                //    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                //}

                //if (IdDepot == 2)//OPACIFIC
                //{
                //    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPOPA"];
                //}


                /*valida cotizacion*/
                var usn = new usuario();
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                usn = this.getUserBySesion();

                if (usn == null)
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "proforma", "btnAsumirBook_Click", "No usuario", Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
                if (token == null)
                {
                    this.Alerta("Estimado Cliente,Su sesión ha expirado, sera redireccionado a la pagina de login", true);
                    System.Web.Security.FormsAuthentication.SignOut();
                    Session.Clear();
                    return;
                }

                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechasal = new DateTime();
                if (!DateTime.TryParseExact(fecsalida.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio, {0}", fecsalida.Text));
                    fecsalida.Focus();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                    return;
                }

                var query = from myRow in dtListaBooking.AsEnumerable()
                            where myRow.Field<string>("REFERENCIA") == xreferencia.Value.ToString() && myRow.Field<string>("BOOKING") == nbrboo.Value.ToString()
                            select myRow;
                DataTable tbresult = query.AsDataView().ToTable();

                if (tbresult.Rows.Count == 0)
                {

                    if (this.Activar == "SI")/*si esta activa la validacion de cobros de turnos zal*/
                    {

                        //validaciones usuario de contado
                        if (!usn.IsCredito)
                        {
                            /*muestra mensaje de saldo a favor*/
                            List<Saldos> Lista = Saldos.Get_Saldo(usn.ruc, IdDepot);
                            var xList = Lista.FirstOrDefault();
                            if (xList != null)
                            {
                                decimal nSaldo = xList.saldo_final;
                                if (nSaldo != 0)
                                {
                                    this.mensaje.Visible = true;
                                    this.saldo.InnerText = xList.leyenda;
                                }
                                else
                                {
                                    this.mensaje.Visible = false;
                                    this.saldo.InnerText = string.Empty;
                                }
                            }
                          

                            var detpasezal = man_pro_expo.GetDetallePaseZALContado_Ruc(nbrboo.Value, xreferencia.Value, fechasal.ToString("yyyy-MM-dd"), xlinea.Value, IdDepot, usn.ruc);
                            if (detpasezal.Rows.Count == 0 || detpasezal == null)
                            {

                                this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                                RepeaterBooking.DataSource = null;
                                RepeaterBooking.DataBind();
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                                return;
                            }

                            dtDetpasezal = new DataTable();
                            dtDetpasezal = detpasezal;
                            RepeaterBooking.DataSource = detpasezal;
                            RepeaterBooking.DataBind();

                        }
                        else
                        {
                            var detpasezal = man_pro_expo.GetDetallePaseZAL_Ruc(nbrboo.Value, xreferencia.Value, fechasal.ToString("yyyy-MM-dd"), xlinea.Value, IdDepot, usn.ruc);
                            if (detpasezal.Rows.Count == 0 || detpasezal == null)
                            {
                                this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                                RepeaterBooking.DataSource = null;
                                RepeaterBooking.DataBind();
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                                return;
                            }

                            dtDetpasezal = new DataTable();

                            dtDetpasezal = detpasezal;
                            RepeaterBooking.DataSource = detpasezal;
                            RepeaterBooking.DataBind();
                        }
                    }
                    else
                    {
                        var detpasezal = man_pro_expo.GetDetallePaseZAL_Ruc(nbrboo.Value, xreferencia.Value, fechasal.ToString("yyyy-MM-dd"), xlinea.Value, IdDepot, usn.ruc);
                        if (detpasezal.Rows.Count == 0 || detpasezal == null)
                        {
                            this.Alerta("No se encontraron datos, revise los criterios de consulta.");
                            RepeaterBooking.DataSource = null;
                            RepeaterBooking.DataBind();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                            return;
                        }

                        dtDetpasezal = new DataTable();

                        dtDetpasezal = detpasezal;
                        RepeaterBooking.DataSource = detpasezal;
                        RepeaterBooking.DataBind();

                    }                


                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
            }
        }
        #endregion

        protected void btnCancelaPase(string idPase)
        {
            try
            {
                app_start.RepContver.ServicioCISE _servicio = new app_start.RepContver.ServicioCISE();
                Dictionary<string, string> obj = new Dictionary<string, string>();

                obj.Add("AV_ESTADO", "X");
                obj.Add("AN_ID_TURNO_REFERENCIA", idPase.ToString());

                CSLSite.app_start.RepContver.Respuesta _result = _servicio.Peticion("CANCELA_CITA", obj);
                if (_result.result.estado != 1)
                {
                    Exception ex;
                    ex = new Exception("Error al cancelarr cita en OPACIFIC con Servicio Web; Cita: " + idPase.ToString());
                    var number = log_csl.save_log<Exception>(ex, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " Mensaje Servicio: " + _result.result.mensaje + " #Error: " + number);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "btnCancelaPase()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void btnReImprimirPase(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "btnReImprimirPase()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }
      
        public void LlenaComboDepositos()
        {
            try
            {
                cmbDeposito.DataSource = man_pro_expo.consultaDepositos(); //ds.Tables[0].DefaultView;
                cmbDeposito.DataValueField = "ID";
                cmbDeposito.DataTextField = "DESCRIPCION";
                cmbDeposito.DataBind();
                cmbDeposito.Enabled = true;
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "pases_zal", "LlenaComboDepositos()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void cmbDeposito_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long IdDepot = long.Parse(cmbDeposito.SelectedValue);
                if (cmbDeposito.SelectedValue == "-1")
                {
                    this.Alerta("Seleccione un depósito.");
                    cmbDeposito.Focus();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                    return;
                }

                if (IdDepot == 1)//ZAL
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }

                if (IdDepot == 2)//OPACIFIC
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPOPA"];
                }

                if (IdDepot == 3)//ZAL
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }


                if (IdDepot == 4)//SAV-DER
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }


                if (IdDepot == 5)//REPCONTVER
                {
                    Session["ReferenciaZAL"] = System.Configuration.ConfigurationManager.AppSettings["REFDEPZAL"];
                }

            }
            catch
            {

            }
        }
    }
}