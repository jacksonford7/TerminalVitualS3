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
using CSLSite.app_start;
//using ZalCuenta;
using System.Net;
using BillionEntidades;
using ClsAisvSav;

namespace CSLSite.zal
{
    public partial class proforma_zal : System.Web.UI.Page
    {


        private Servicios objServicios = new Servicios();
        private Activar_Turnos objParametro = new Activar_Turnos();
        private Cls_Bil_Sav_Lineas objLineas = new Cls_Bil_Sav_Lineas();

        #region "Variables de Sesiones"
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

        private String s_linea_proforma
        {
            get
            {
                return (String)Session["s_linea_proforma"];
            }
            set
            {
                Session["s_linea_proforma"] = value;
            }

        }

        public DataTable dtTurnosZAL
        {
            get { return (DataTable)Session["dtTurnosZAL"]; }
            set { Session["dtTurnosZAL"] = value; }
        }

        public String emailCliente
        {
            get { return (String)Session["emailClienteZAL"]; }
            set { Session["emailClienteZAL"] = value; }
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

        public String Activar
        {
            get { return (String)Session["Activar"]; }
            set { Session["Activar"] = value; }
        }

        #endregion

        private static int cont=0;
        private static int pos=0;

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
                    //fecsalida.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    lbltotturnos.Text = "";
                    IniDsChofer("");
                    IniDsPlaca("");

                    /*para clientes de contado*/
                    this.xfinder.Visible = false;
                    this.sinresultado.Visible = false;

                    objParametro.Obtener_Parametro(out string error);
                    this.Activar = objParametro.config_value;

                    LlenaComboDepositos();
                    Session["ReferenciaZAL"] = null;

                   
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "proforma_ZAL", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
            }
        }

        #region "Carga datos de chofer y Placa"

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

        #region "Buscar y Selecciona turnos"

        public static DataTable N4_P_Cons_Turnos_Disponibles_ZAL(string fecha, string lineapro, long v_deposito)
        {
            var d = new DataTable();
            using (var c = ConexionN4Middleware())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "N4_P_Cons_Turnos_Disponibles_ZAL";
                coman.Parameters.AddWithValue("@FECHA", fecha);
                coman.Parameters.AddWithValue("@LINEA", lineapro);
                coman.Parameters.AddWithValue("@deposito",v_deposito);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "pasePuerta", "GetInfoEmails", "N4_P_Cons_Turnos_Disponibles_ZAL", DateTime.Now.ToShortDateString());
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

        protected void cblturnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Pregunta si sigue conectado.
                if (Response.IsClientConnected)
                {
                    int ind = cblturnos.SelectedIndex;
                    if (string.IsNullOrEmpty(txtqty.Text))
                    {
                        cblturnos.Items[ind].Selected = false;
                        this.Alerta("Ingrese la cantidad solicitada.");
                        txtqty.Focus();
                        return;
                    }

                    int top = int.Parse(txtqty.Text);
                    if (top == 0)
                    {
                        cblturnos.Items[ind].Selected = false;
                        this.Alerta("La cantidad solicitada debe ser mayor a cero.");
                        txtqty.Focus();
                        return;
                    }

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
                                    cblturnos.Items[ind].Selected = false;
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
                        cblturnos.Items[ind].Selected = false;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        txtplaca.Focus();
                        return;
                    }

                    bool val = false;

                    if (ind >= 0)
                        val = cblturnos.Items[cblturnos.SelectedIndex].Selected;

                    for (var a = 0; a < cblturnos.Items.Count; a++)
                    {
                        if (dtTurnosZAL.Rows[a]["CHECK"].ToString() == "")
                            dtTurnosZAL.Rows[a]["CHECK"] = "False";

                        if (dtTurnosZAL.Rows[a]["CHECK"].ToString() != cblturnos.Items[a].Selected.ToString())
                        {
                            ind = a;
                            val = cblturnos.Items[a].Selected;
                            a = cblturnos.Items.Count;
                            dtTurnosZAL.Rows[ind]["CHECK"] = val;
                            //numbook.InnerText = "";
                        }
                    }

                    if (cblturnos.Items.Count - 1 < top)
                    {
                        cblturnos.Items[ind].Selected = false;
                        this.Alerta("La cantidad solicitada: " + top + ", supera los turnos disponibles en la fecha indicada: " + fecsalida.Text);
                        txtqty.Focus();
                        return;
                        //mensaje cantidad solicitada supera los turnos disponibles en la fecha indicada
                    }
                    else
                    {
                        if (ind == 0)
                        {

                        }
                        else
                        {
                            DataTable dtTurns = new DataTable();
                            dtTurns = dtTurnosZAL;

                            DataTable resultadotrue = new DataTable();
                            DataView viewtrue = new DataView();
                            var resulttrue = from myRow in dtTurns.AsEnumerable()
                                             where myRow.Field<string>("CHOFER") == txtchofer.Text && myRow.Field<string>("PLACA") == txtplaca.Text && myRow.Field<string>("CHECK") == "True"
                                             select myRow;
                            viewtrue = resulttrue.AsDataView();
                            resultadotrue = viewtrue.ToTable();
                            if (resultadotrue != null)
                            {
                                if (resultadotrue.Rows.Count > 0)
                                {
                                    for (int i = 0; i <= resultadotrue.Rows.Count - 1; i++)
                                    {
                                        var turno = resultadotrue.Rows[i]["Inicio"].ToString();
                                        var inicio = dtTurnosZAL.Rows[ind]["Inicio"].ToString();
                                        if (turno == inicio)
                                        {
                                            dtTurnosZAL.Rows[ind]["CHECK"] = false;
                                            cblturnos.Items[ind].Selected = false;
                                            var msgerror = "Ya asigno el Chofer: " + txtchofer.Text + " y la\\nPlaca: " + txtplaca.Text + ", al Turno: " + turno + "\\nSelecione otro (Turno), (Placa y Chofer).";
                                            this.Alerta(msgerror);
                                            return;
                                        }
                                    }
                                }
                            }

                            //var chofer = txtchofer.Text.ToUpper().Trim();//dtTurnosZAL.Rows[ind]["CHOFER"].ToString();
                            //var placa = txtplaca.Text.ToUpper().Trim();//dtTurnosZAL.Rows[ind]["PLACA"].ToString();                       

                            dtTurnosZAL.Rows[ind]["CHECK"] = val;
                            if (val)
                            {
                                dtTurnosZAL.Rows[ind]["CHOFER"] = txtchofer.Text;
                                dtTurnosZAL.Rows[ind]["PLACA"] = txtplaca.Text;
                                cblturnos.Items[ind].Text = cblturnos.Items[ind].Text + " - CHOFER: " + txtchofer.Text.ToUpper().Trim() + " - PLACA: " + txtplaca.Text.ToUpper().Trim();
                            }
                            else
                            {
                                dtTurnosZAL.Rows[ind]["CHOFER"] = "";
                                dtTurnosZAL.Rows[ind]["PLACA"] = "";
                                cblturnos.Items[ind].Text = cblturnos.Items[ind].Text.Substring(0, 5);
                            }
                        }
                    }
                    dtTurnosZAL.AcceptChanges();

                    var dtPPWeb = (from p in dtTurnosZAL.AsEnumerable()
                                   where p.Field<string>("CHECK") == "True"
                                   select p).AsDataView().ToTable();

                    tableDetTurnos.DataSource = dtPPWeb;
                    tableDetTurnos.DataBind();


                    /*proformas, agreado 27-08-2019
                   jalvarado
                   */
                    if (this.Activar == "SI")/*si esta activa la validacion de cobros de turnos zal*/
                    {
                        var usn = new usuario();
                        HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                        usn = this.getUserBySesion();
                        if (usn == null)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "proforma", "btnconsultarturnos_Click", "No usuario", Request.UserHostAddress);
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

                        long IdDepot = long.Parse(cmbDeposito.SelectedValue);
                        /*si es cliente de credito*/
                        if (usn.IsCredito || (s_linea_proforma == "BTS" && IdDepot == 1)) //JGUSQUI - 202305-31 BTS==CREDITO
                        {
                            this.xfinder.Visible = false;
                            this.sinresultado.Visible = false;
                        }
                        else
                        {   /*cliente de contado*/
                            this.xfinder.Visible = true;
                            this.sinresultado.Visible = false;

                            int nCantidad = 0;
                            if (!int.TryParse(dtPPWeb.Rows.Count.ToString(), out nCantidad))
                            {
                                this.sinresultado.InnerText = "Por favor comuníquese con nosotros, al parecer la cantidad de pases presenta problemas.";
                                sinresultado.Visible = true;
                                btnera.Visible = false;
                                alerta.Visible = false;
                                xfinder.Visible = false;
                                this.Alerta(this.sinresultado.InnerText);
                                return;
                            }
                            this.alerta.InnerHtml = string.Format("<strong>Estimado usuario: {0}, Usted tiene {1} turnos seleccionados, los mismo generaran los siguientes valores a pagar.</strong>", usn.loginname, nCantidad.ToString());

                            populateRepeater(nCantidad);


                        }
                        /*fin de datos de la proforma*/
                    }//fin activo parametro

                }
                    

 
            }
            catch (Exception)
            {
                cont = 0;
                //mensaje error
            }
        }

        protected void btnconsultarturnos_Click(object sender, EventArgs e)
        {
            try
            {
                //Pregunta si sigue conectado.
                if (Response.IsClientConnected)
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }

                    if (string.IsNullOrEmpty(txtchofer.Text))
                    {
                        this.Alerta("Digite el Chofer.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        txtchofer.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(fecsalida.Text))
                    {
                        this.Alerta("Ingrese la Fecha de Salida.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        fecsalida.Focus();
                        return;
                    }

                    if (cmbDeposito.SelectedValue == "-1")
                    {
                        this.Alerta("Seleccione un depósito.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        cmbDeposito.Focus();
                        return;
                    }

                    //VALIDACION BOOKING OPACIFIC
                    if (cmbDeposito.SelectedValue == "5")
                    {
                        app_start.RepContver.ServicioCISE _servicio = new app_start.RepContver.ServicioCISE();

                        if (!string.IsNullOrEmpty(_servicio.Token))
                        {
                            Session["proforma_zal_Token"] = _servicio.Token;
                        }
/*
                        Dictionary<string, string> obj = new Dictionary<string, string>();
                        obj.Add("key", System.Configuration.ConfigurationManager.AppSettings["OPACIFIC_KEY"]);
                        obj.Add("usuario_id", System.Configuration.ConfigurationManager.AppSettings["OPACIFIC_USER_ID"]);


                        DateTime fechaOPACIF = new DateTime();
                        CultureInfo enUSOPACIF = new CultureInfo("en-US");
                        if (!DateTime.TryParseExact(fecsalida.Text, "dd/MM/yyyy", enUSOPACIF, DateTimeStyles.None, out fechaOPACIF))
                        {
                            this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}", fecsalida.Text));
                            fecsalida.Focus();
                            return;
                        }

                        obj.Add("fecha", fechaOPACIF.ToString("dd/MM/yyyy"));
                        //obj.Add("fecha", DateTime.Now.ToString("dd/MM/yyyy"));
                        //obj.Add("hora", "1");
                        obj.Add("booking", nbrboo.Value.ToString().Replace("_CISE", String.Empty));
                        obj.Add("accion", System.Configuration.ConfigurationManager.AppSettings["ACCION_VALIDA"]);


                        //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // FAILS!

                        CSLSite.app_start.RepContver.Respuesta _result = _servicio.Peticion("VALIDAR_BOOKING", obj);

                        if (_result.result.estado != 1)
                        {
                            this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " - Servicio: " + _result.result.mensaje);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                            cmbDeposito.Focus();
                            return;
                        }
                        */
                    }

                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "fchange();", true);

                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime fechasal = new DateTime();
                    if (!DateTime.TryParseExact(fecsalida.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}", fecsalida.Text));
                        fecsalida.Focus();
                        return;
                    }
                    if (s_linea_proforma == null)
                    {
                        this.Alerta("Seleccione el Booking.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        return;
                    }

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
                    if (string.IsNullOrEmpty(txtqty.Text))
                    {
                        this.Alerta("Ingrese la cantidad solicitada.");
                        txtqty.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }

                    int top = int.Parse(txtqty.Text);
                    if (top == 0)
                    {
                        this.Alerta("La cantidad solicitada debe ser mayor a cero.");
                        txtqty.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }
                    if (top > Convert.ToInt64(salqty.Value))
                    {
                        this.Alerta("La Cantidad Solicitada: " + top.ToString() + ", es mayor a la Disponible: " + salqty.Value.ToString());
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        txtqty.Focus();
                        return;
                    }
                    dtTurnosZAL = new DataTable();
                    dtTurnosZAL = N4_P_Cons_Turnos_Disponibles_ZAL(fechasal.ToString("yyyy-MM-dd"), s_linea_proforma.ToString(), long.Parse(cmbDeposito.SelectedValue));
                    dtTurnosZAL.Columns.Add("CHECK");
                    dtTurnosZAL.Columns.Add("IDPZAL");
                    dtTurnosZAL.Columns.Add("CHOFER");
                    dtTurnosZAL.Columns.Add("PLACA");
                    dtTurnosZAL.Columns.Add("IDCISE");
                    cblturnos.DataSource = dtTurnosZAL;
                    cblturnos.DataValueField = "Plan";
                    cblturnos.DataTextField = "Inicio";
                    cblturnos.DataBind();
                    lbltotturnos.Text = "Tot. Turnos: " + (dtTurnosZAL.Rows.Count - 1).ToString();

                    tableDetTurnos.DataSource = new DataTable();
                    tableDetTurnos.DataBind();

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mySetValue();ocultagiffecha();", true);

                }

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "Proforma_ZAL", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        #endregion

     
        #region "Carga datos de la Proforma Concepto a Pagar, limpia datos de proforma"

        public void populateRepeater(int cantidad)
        {
            try
            {

                string vr = string.Empty;
                decimal nIva = 0;
                //cabecera del grupo
                objServicios = new Servicios();
                Session["Servicios"] = objServicios;
                objServicios = Session["Servicios"] as Servicios;

                Int64 ID_DEPORT;

                if (!Int64.TryParse(this.cmbDeposito.SelectedValue.ToString(), out ID_DEPORT))
                {
                    ID_DEPORT = 0;
                }

                Int64 ID_LINEA = 0;
                string OError;

                objLineas = new Cls_Bil_Sav_Lineas();
                objLineas.LINEA = this.xlinea.Value;
                objLineas.DEPOT = ID_DEPORT;
                if (!objLineas.PopulateMyData_Id_Linea(out OError))
                {
                    this.sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError);
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }
                else
                {
                    ID_LINEA = objLineas.ID;

                }

                var table = Servicios.List_Servicios_Repcontver(ID_DEPORT, ID_LINEA, out vr);  
                //var table = Servicios.List_Servicios(out vr);
    
                foreach (var f in table)
                {
                  
                    f.mensaje = !string.IsNullOrEmpty(f.nota) ?
                    string.Format("<a class='infotip'><span class='classic'>{0}</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>",  f.nota) : "";
                      
                    f.contenido = f.descripcion;
                    f.icont = true;

                    f.cantidad = f.tipo.Contains("T") ? cantidad : f.cantidad;
                    if (!f.aplica)
                    {
                        f.cantidad = 0;
                    }

                    //TODO CORREGIR SOLO EN PRIMER CASO
                    if (f.tipo.Contains("C"))
                    {
                        f.vtotal = (f.costo * f.cantidad);
                        f.costo = f.costo ;
                    }
                    else
                    {
                        f.vtotal = (f.costo * f.cantidad);
                    }

                    nIva = f.iva;

                    objServicios.Detalle.Add(f);
                    
                }

                Session["Servicios"] = objServicios;

                tablaNueva.DataSource = table;
                tablaNueva.DataBind();
                var subt = table.Where(a => a.aplica).ToList().Sum(b => b.vtotal);
                var iva = (subt * (nIva/100));
                var tot = (subt + iva);

                stsubtotal.InnerHtml = string.Format("<strong>{0:c}</strong>", subt );
                stiva.InnerHtml = string.Format("<strong>{0:c}</strong>", iva);
                sttal.InnerHtml = string.Format("<strong>{0:c}</strong>", tot);

                if (subt == 0)
                {
                    this.sinresultado.InnerText = string.Format("Debe seleccionar los turnos, para poder visualizar los valores a pagar");
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }

                ViewState["cantidad"] = null;
                ViewState["cantidad"] = cantidad;

                ViewState["total"] = null;
                ViewState["total"] = tot;


            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "Proforma_zal", "populateRepeater", "Hubo un ERROR en BOTON CANCELAR", t != null ? t.loginname : "NoUser"));
                sinresultado.Visible = true;
            }
        }

        protected void limpiar()
        {
            lbltotturnos.Text = "";
            this.txtchofer.Text = "";
            this.txtplaca.Text = "";
            this.txtqty.Text = "";
            this.fecsalida.Text = "";

            this.xfinder.Visible = false;
            this.sinresultado.Visible = false;

            tableDetTurnos.DataSource = new DataTable(); ;
            tableDetTurnos.DataBind();

            tablaNueva.DataSource = new DataTable(); ;
            tablaNueva.DataBind();

            cblturnos.DataSource = new DataTable(); ;
            cblturnos.DataBind();
        }

        #endregion

        #region"Proceso para generar pases y proforma"

        /*proceso de generar pases y proforma para clientes de contado*/
        protected void btnAsumirBook_Click(object sender, EventArgs e)
        {
            //<JGUSQUI 20210415> CAMBIO REALIZADO PARA ASUME TERCEROS ZAL
            bool v_AsumeTercero = false;
            string v_rucAsume = string.Empty;
            string v_nameAsume = string.Empty;
            string v_loginAsume = string.Empty;
            string v_loginCliente = string.Empty;
            string v_rucCliente = string.Empty;
            string v_nameCliente = string.Empty;
            long v_idAsume = 0;
            //</JGUSQUI 202104015>

            try
            {
                //<JGUSQUI 20210415> CAMBIO REALIZADO PARA ASUME TERCEROS ZAL
                //valida si existe pago de tercero
                var oAsume = new ControlPagos.Importacion.PagoAsignado();
                var userLogeado = this.getUserBySesion();
                oAsume.booking = nbrboo.Value;
                oAsume.ruc_asumido = userLogeado.ruc;

                var v_resultado = oAsume.ExisteAsumeTerceroZAL();
                if (v_resultado.Exitoso)
                {
                    oAsume = v_resultado.Resultado;
                    v_AsumeTercero = true;
                    v_rucAsume = v_resultado.Resultado.ruc;
                    v_nameAsume = v_resultado.Resultado.nombre;
                    v_loginAsume = v_resultado.Resultado.login_asigna;
                    v_idAsume = long.Parse(v_resultado.Resultado.id_asignacion.ToString());

                }
                //</JGUSQUI 20210415>

                //Pregunta si sigue conectado.
                if (Response.IsClientConnected)
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }

                    long IdDepot = long.Parse(cmbDeposito.SelectedValue);
                    bool Generado = false;
                    DateTime fechacliente = DateTime.Now;
                    DateTime pfechacliente = DateTime.Now;
                    DateTime fechasal = new DateTime();

                    CultureInfo enUS = new CultureInfo("es-US");
                    int Cantidad = 0;
                    Decimal Total = 0;
                    Decimal totalIva = 0;
                    Decimal ValotUnitario = 0;
                    Decimal nIva = 0;

                    string Mensaje = string.Empty;
                    string Chofer = string.Empty;
                    string Placa = string.Empty;


                    var usn = new usuario();
                    HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                    usn = this.getUserBySesion();

                    //<JGUSQUI 20210415> CAMBIO REALIZADO PARA ASUME TERCEROS ZAL
                    v_loginCliente = usn.loginname;
                    v_rucCliente = usn.ruc;
                    v_nameCliente = usn.nombres == usn.apellidos? usn.apellidos: usn.nombres + " " + usn.apellidos;
                    oAsume.login_modifica = usn.loginname;
                    //</JGUSQUI 20210415>

                    if (string.IsNullOrEmpty(txtqty.Text))
                    {
                        this.Alerta("Ingrese la cantidad solicitada.");
                        txtqty.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }

                    int top = int.Parse(txtqty.Text);
                    if (top == 0)
                    {
                        this.Alerta("La cantidad solicitada debe ser mayor a cero.");
                        txtqty.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }

                    var dtValChk = (from p in dtTurnosZAL.AsEnumerable()
                                    where p.Field<String>("CHECK") == "True" && p.Field<String>("Plan") != "0"
                                    select p).AsDataView().ToTable();

                    if (dtValChk.Rows.Count == 0)
                    {
                        this.Alerta("Seleccione el(los) turno(s).");
                        txtqty.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }

                    if (dtValChk.Rows.Count != top)
                    {
                        this.Alerta("Los turnos seleccionados deben ser igual a la cantidad solicitada.");
                        txtqty.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }


                    if (!DateTime.TryParseExact(fecsalida.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechasal))
                    {
                        this.Alerta("La fecha de salida no no tiene un formato válido");
                        fecsalida.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }

                    if (cmbDeposito.SelectedValue == "-1")
                    {
                        this.Alerta("Seleccione un depósito.");
                        cmbDeposito.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }

                    //<JGUSQUI 20210415> CAMBIO REALIZADO PARA ASUME TERCEROS ZAL
                    if (v_AsumeTercero)
                    {
                        var oUsuario = new usuario();
                        oUsuario = oUsuario.ObtenerUser(v_loginAsume, v_rucAsume);

                        usn.ruc = oUsuario.ruc;
                        usn.codigoempresa = oUsuario.codigoempresa;
                        usn.email = oUsuario.email;
                        usn.grupo = oUsuario.grupo;
                        usn.id = oUsuario.id;
                        usn.loginname = oUsuario.loginname == null ? usn.loginname : oUsuario.loginname;


                        this.agencia.Value = oUsuario.ruc;
                        this.emailCliente = oUsuario.email;
                        this.ClienteBloqueado = oUsuario.bloqueo_cartera; // si es true esta bloqueado si es false no esta bloqueado
                        this.ClienteTipo = oUsuario.IsCredito;
                    }
                    //</JGUSQUI 20210415>

                    //JAB 21-07-2021 (VALIDA FECHA DEL TURNO SEA IGUAL A LOS SELECCIONADOS
                    for (int i = 1; i < dtTurnosZAL.Rows.Count; i++)
                    {
                        if (dtTurnosZAL.Rows[i]["CHECK"].ToString() == "True")
                        {
                            string new_fecha = dtTurnosZAL.Rows[i]["FECHA"].ToString();
                            DateTime Fecha_Auxiliar;
                            if (!DateTime.TryParseExact(new_fecha, "dd/MM/yyyy", enUS, DateTimeStyles.None, out Fecha_Auxiliar))
                            {
                                this.Alerta("La fecha del turno seleccionado no tiene un formato válido");
                                fecsalida.Focus();
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                                return;
                            }

                            if (Fecha_Auxiliar != fechasal)
                            {
                                string msg = string.Format("La fecha del turno actual {0}, es diferente a la fecha del turno inicial generado:{1}", fechasal.ToString("dd/MM/yyyy"), Fecha_Auxiliar.ToString("dd/MM/yyyy"));
                                this.Alerta(msg);
                                fecsalida.Focus();
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                                return;
                            }
                        }
                    }

                    //FIN VALIDACION


                    /*valida cotizacion*/
                    if (this.Activar == "SI")/*si esta activa la validacion de cobros de turnos zal*/
                    {
                        if (usn == null)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "proforma", "btnconsultarturnos_Click", "No usuario", Request.UserHostAddress);
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

                        

                        //verifica si el uusario es de contado o credito
                        bool Credito = usuario.Credito(usn.ruc);

                        //<JGUSQUI 31-05-2023 - LINEA BTS --> SI LA LINEA ES BTS SE LO PONE COMO CREDITO  >
                        if ((s_linea_proforma == "BTS") && (IdDepot == 1))
                        {
                            Credito = true;
                        }
                        //</JGUSQUI 31-05-2023 

                        //if (!usn.IsCredito)
                        if (!Credito)
                        {

                            if (!DateTime.TryParseExact(pfechacliente.Date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacliente))
                            {
                                this.sinresultado.InnerText = "La fecha de la proforma no tiene un formato válido";
                                sinresultado.Visible = true;
                                btnera.Visible = false;
                                alerta.Visible = false;
                                xfinder.Visible = false;
                                this.Alerta(this.sinresultado.InnerText);
                                return;
                            }

                            var strQty = ViewState["cantidad"] != null ? ViewState["cantidad"].ToString() : string.Empty;

                            if (!int.TryParse(strQty, out Cantidad) || Cantidad <= 0)
                            {
                                this.sinresultado.InnerText = string.Format("Por favor primero seleccione los turnos a emitir proforma QTY:{0}", ViewState["cantidad"]);
                                sinresultado.Visible = true;
                                btnera.Visible = false;
                                alerta.Visible = false;
                                xfinder.Visible = false;
                                this.Alerta(this.sinresultado.InnerText);
                                return;
                            }

                            var strValor = ViewState["total"] != null ? ViewState["total"].ToString() : string.Empty;

                            if (!Decimal.TryParse(strValor, out Total) || Total <= 0)
                            {
                                this.sinresultado.InnerText = string.Format("Por favor primero seleccione los turnos a emitir proforma Total:{0}", ViewState["cantidad"]);
                                sinresultado.Visible = true;
                                btnera.Visible = false;
                                alerta.Visible = false;
                                xfinder.Visible = false;
                                this.Alerta(this.sinresultado.InnerText);
                                return;
                            }

                        }

                        /*fin validaciones si es contado el cliente*/
                    }

                    //<JGUSQUI 20210406> CAMBIO REALIZADO PARA ASUME TERCEROS ZAL
                    bool v_Credito = usuario.Credito(usn.ruc);

                    //<JGUSQUI 31-05-2023 - LINEA BTS --> SI LA LINEA ES BTS SE LO PONE COMO CREDITO  >
                    if ((s_linea_proforma == "BTS") && (IdDepot == 1))
                    {
                        v_Credito = true;
                    }
                    //</JGUSQUI 31-05-2023 

                    if (!v_Credito)
                    {
                        if (v_AsumeTercero)
                        {
                            xfinder.Visible = false;
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = "Ocurrió un error, El cliente que asume el pago no tiene crédito.";
                            return;
                        }
                    }
                    //</JGUSQUI 20210406>

                    /*generacion del pase*/
                    string liq = "9025";
                    for (int i = 1; i < dtTurnosZAL.Rows.Count; i++)
                    {
                        string pase = "";
                        int idpase = 0;

                        if (dtTurnosZAL.Rows[i]["CHECK"].ToString() == "True")
                        {
                            Chofer = dtTurnosZAL.Rows[i]["CHOFER"].ToString();
                            Placa = dtTurnosZAL.Rows[i]["PLACA"].ToString();

                            pase = CreatePase(nbrboo.Value, ruc.Value, ruc.Value, xreferencia.Value, liq, txtqty.Text, "", fechasal.ToString("yyyy-MM-dd"), dtTurnosZAL.Rows[i]["Plan_id"].ToString(), dtTurnosZAL.Rows[i]["Turno_Secuencia"].ToString(), dtTurnosZAL.Rows[i]["turno"].ToString(), dtTurnosZAL.Rows[i]["Inicio"].ToString(), dtTurnosZAL.Rows[i]["Fin"].ToString(), Page.User.Identity.Name, dtTurnosZAL.Rows[i]["PLACA"].ToString(), dtTurnosZAL.Rows[i]["CHOFER"].ToString(),
                                v_rucCliente,v_nameCliente, v_rucAsume,v_nameAsume,v_idAsume);

                            if (man_pro_expo.AddPaseZAL(pase, emailCliente.ToString(), IdDepot, out idpase))
                            {
                                /*verifico que sea un pase valido y no de error*/
                                if (idpase != 0)
                                {
                                    dtTurnosZAL.Rows[i]["IDPZAL"] = idpase;
                                    Generado = true;

                                    if (IdDepot == 5)
                                    {

                                        try
                                        {
                                            app_start.RepContver.ServicioCISE _servicio = new app_start.RepContver.ServicioCISE();
                                            Dictionary<string, string> obj = new Dictionary<string, string>();

                                            string v_datoChefer = Chofer;
                                            string[] v_chofer = v_datoChefer.Split('-');

                                            string v_error = string.Empty;
                                            string v_tipo_contenedor = string.Empty;

                                            var oInformacion = ExisteContenedorLinea.Consultarbooking(nbrboo.Value.ToString(), out v_error).FirstOrDefault();
                                            if (oInformacion != null) 
                                            {
                                                v_tipo_contenedor = oInformacion.id;
                                            }

                                                //obj.Add("key", System.Configuration.ConfigurationManager.AppSettings["OPACIFIC_KEY"]);
                                                //obj.Add("usuario_id", System.Configuration.ConfigurationManager.AppSettings["OPACIFIC_USER_ID"]);
                                                obj.Add("AV_ID_TIPO_ORDEN", "E");
                                            obj.Add("AN_ID_TURNO", idpase.ToString());
                                            obj.Add("AD_FECHA_TURNO", fechasal.ToString("yyyy-MM-dd") + " " + dtTurnosZAL.Rows[i]["Inicio"].ToString()); //DateTime.Now.ToString("dd/MM/yyyy")); //
                                            obj.Add("AN_ID_NAVIERA", "13");
                                            obj.Add("AV_NAVIERA_DESCRIPCION", "PIL");
                                            obj.Add("AV_BOOKING", nbrboo.Value.ToString().Replace("_RTV", ""));//nbrboo.Value.ToString().Replace("_CISE", String.Empty));
                                            obj.Add("AV_BL", nbrboo.Value.ToString().Replace("_RTV", ""));
                                            obj.Add("AV_CONTENEDOR", "");
                                            obj.Add("AV_ID_TIPO_CTNER", v_tipo_contenedor);
                                            obj.Add("AV_CLIENTE_RUC", userLogeado.ruc);
                                            obj.Add("AV_CLIENTE_NOMBRE", userLogeado.nombres);
                                            obj.Add("AV_TRANSPORTISTA_RUC", v_chofer[0].ToString().Trim());
                                            obj.Add("AV_TRANSPORTISTA_DESCRIPCION", v_chofer[1].ToString().Trim());
                                            obj.Add("AV_CHOFER_CEDULA", v_chofer[0].ToString().Trim());
                                            obj.Add("AV_CHOFER_NOMBRE", v_chofer[1].ToString().Trim());
                                            obj.Add("AV_PLACAS", Placa);
                                            obj.Add("AV_BUQUE", "");
                                            obj.Add("AV_VIAJE", "");
                                            obj.Add("AV_ESTADO", "A");

                                            //obj.Add("hora", int.Parse(dtTurnosZAL.Rows[i]["Inicio"].ToString().Substring(0, 2)).ToString());
                                            
                                            

                                            obj.Add("accion", System.Configuration.ConfigurationManager.AppSettings["ACCION_CREACITA"]);

                                            CSLSite.app_start.RepContver.Respuesta _result = _servicio.Peticion("CREAR_CITA", obj);


                                            if (_result.result.estado != 1)
                                            {
                                                Generado = false;
                                                man_pro_expo.CancelarTurnoPorLinea(long.Parse(idpase.ToString()), usn.loginname, out Mensaje);

                                                Exception ex;
                                                ex = new Exception("Error al crear cita en OPACIFIC, El pase : " + idpase.ToString() + " se ha cancelado; Respuesta de SW Opacific: " + _result.result.mensaje);
                                                var number = log_csl.save_log<Exception>(ex, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                                this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " Mensaje Servicio: " + _result.result.mensaje + " #Error: " + number);
                                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                                                break;
                                            }
                                            else
                                            {
                                                //NUEVO CAMPO
                                                //ACTUALIZO CON EL ID CISE
                                                dtTurnosZAL.Rows[i]["IDCISE"] = long.Parse(_result.result.id_turno_referencia.ToString());
                                                man_pro_expo.UpdateTurnoPorLinea(long.Parse(idpase.ToString()), long.Parse(_result.result.id_turno_referencia.ToString()), out Mensaje);
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            //17-03-2020
                                            man_pro_expo.CancelarTurnoPorLinea(idpase, "error", out Mensaje);
                                            Generado = false;
                                            Exception exc = new Exception(" Booking:" + nbrboo.Value.ToString() + " - " + ex.Message);
                                            var number = log_csl.save_log<Exception>(exc, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                            break;
                                        }

                                    }

                                }
                                else
                                {
                                    Generado = false;
                                    break;
                                }

                            }
                        }
                    }

                    dtTurnosZAL.AcceptChanges();
                    bool Procesar = false;
                    //17-04-2020
                    /*cancela todos los pases si es de bodega cise y si algun pase no tiene deposito asignado*/
                    if (IdDepot == 2)
                    {
                        /* var dtPPWebAsig = (from p in dtTurnosZAL.AsEnumerable()
                                            where (p.Field<String>("IDCISE") == null || string.IsNullOrEmpty(p.Field<String>("IDCISE"))) 
                                            && p.Field <String>("CHECK") == "True"
                                            select p).AsDataView().ToTable();*/

                        //si existen registros sin una cita, anulo todos
                        if (!Generado)
                        {
                            try
                            {
                                Exception exc = new Exception("Booking:" + nbrboo.Value.ToString() + " - INICIO CANCELACION MASIVA");
                                var number = log_csl.save_log<Exception>(exc, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);


                                //cancela cita cise
                                for (int i = 1; i < dtTurnosZAL.Rows.Count; i++)
                                {
                                    if (dtTurnosZAL.Rows[i]["CHECK"].ToString() == "True")//&& !string.IsNullOrEmpty(dtTurnosZAL.Rows[i]["IDCISE"].ToString()))
                                    {
                                        Exception exc2 = new Exception("Booking:" + nbrboo.Value.ToString() + " - RECORRIENDO FOR, SI ES TRUE");
                                        var number2 = log_csl.save_log<Exception>(exc2, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);


                                        String paseLinea = "0";// (string.IsNullOrEmpty(dtTurnosZAL.Rows[i]["IDCISE"].ToString()) ? "0" : dtTurnosZAL.Rows[i]["IDCISE"].ToString());

                                        try
                                        {
                                            paseLinea = (string.IsNullOrEmpty(dtTurnosZAL.Rows[i]["IDCISE"].ToString()) ? "0" : dtTurnosZAL.Rows[i]["IDCISE"].ToString());
                                        }
                                        catch
                                        {
                                            paseLinea = "0";
                                        }

                                        btnCancelaPase(paseLinea);

                                        String idpase = "0";
                                        try
                                        {
                                            idpase = (string.IsNullOrEmpty(dtTurnosZAL.Rows[i]["IDPZAL"].ToString()) ? "0" : dtTurnosZAL.Rows[i]["IDPZAL"].ToString());
                                        }
                                        catch
                                        {
                                            idpase = "0";
                                        }

                                        man_pro_expo.CancelarTurnoPorLinea(long.Parse(idpase.ToString()), usn.loginname, out Mensaje);

                                        Procesar = true;
                                    }
                                }



                            }
                            catch
                            {
                                Procesar = true;
                                Exception exc = new Exception(" Booking:" + nbrboo.Value.ToString() + " - ERROR NO CONTROLADO POR OPACIFIC EN SW CANCELACION");
                                var number = log_csl.save_log<Exception>(exc, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                            }

                            if (Procesar)
                            {
                                this.Alerta("Estimado cliente, se presentó un problema, los pases generados se cancelaron, por favor comunicarse con IT CGSA...");
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);

                                limpiar();
                                return;
                            }

                            /*
                            try
                            {
                                XDocument docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                                   new XElement("PASEPUERTA", from p in dtTurnosZAL.AsEnumerable().AsParallel()
                                                                              where p.Field<String>("CHECK") == "True"
                                                                              select new XElement("VBS_T_PASE_PUERTA_ZAL",
                                                                               new XAttribute("ID_PPZAL", p.Field<String>("IDPZAL") == null ? "0" : p.Field<String>("IDPZAL").ToString()),
                                                                               new XAttribute("ESTADO", "CA"),
                                                                               new XAttribute("USR_MOD", usn.loginname),
                                                                               new XAttribute("FECHA_MOD", DateTime.Now.ToString("MM/dd/yyyy HH:mm"))
                                                                               )));
                                //si esta todo ok la cancelacion
                                if (man_pro_expo.SaveCancelarPasePuertaZAL(docXML.ToString(), out Mensaje))
                                {
                                    this.Alerta("Estimado cliente, se presentó un problema, los pases generados se cancelaron, por favor comunicarse con IT CGSA...");
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);

                                    Exception exc = new Exception("Booking:" + nbrboo.Value.ToString() + " - CANCELACION MASIVA CON EXITO");
                                    var number = log_csl.save_log<Exception>(exc, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);


                                    limpiar();
                                    return;

                                }
                                else
                                {
                                    this.Alerta("Estimado cliente, se presentó un problema durante la cancelación de pases, por favor comunicarse con IT CGSA...");
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);

                                    Exception exc = new Exception("Booking:" + nbrboo.Value.ToString() + " - ERROR AL CANCELAR DE FORMA MASIVA");
                                    var number = log_csl.save_log<Exception>(exc, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);

                                    limpiar();
                                    return;
                                }
                            }
                            catch
                            {
                                Exception exc = new Exception(" Booking:" + nbrboo.Value.ToString() + " - ERROR EN XML ");
                                var number = log_csl.save_log<Exception>(exc, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                            }
                            */



                        }

                    }


                    /*fin de validacion*/

                    if (this.Activar == "SI")/*si esta activa la validacion de cobros de turnos zal*/
                    {
                        /*verifico si existe un error para cancelar los pases emitidos de la zal (nuevo)*/
                        if (!Generado)
                        {

                            var dtPPWebAsig = (from p in dtTurnosZAL.AsEnumerable()
                                               where p.Field<String>("IDPZAL") != null
                                               select p).AsDataView().ToTable();

                            if (dtPPWebAsig.Rows.Count != 0)
                            {
                                XDocument docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                                    new XElement("PASEPUERTA", from p in dtTurnosZAL.AsEnumerable().AsParallel()
                                                                               where p.Field<String>("IDPZAL") != null
                                                                               select new XElement("VBS_T_PASE_PUERTA_ZAL",
                                                                                new XAttribute("ID_PPZAL", p.Field<String>("IDPZAL") == null ? "" : p.Field<String>("IDPZAL").ToString()),
                                                                                new XAttribute("ESTADO", "CA"),
                                                                                new XAttribute("USR_MOD", usn.loginname),
                                                                                new XAttribute("FECHA_MOD", DateTime.Now.ToString("MM/dd/yyyy HH:mm"))
                                                                                )));
                                /*si esta todo ok la cancelacion*/
                                if (man_pro_expo.SaveCancelarPasePuertaZAL(docXML.ToString(), out Mensaje))
                                {
                                    this.Alerta("Estimado cliente, se presentó un problema, los pases generados se cancelaron, por favor comunicarse con IT CGSA...");
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);

                                    limpiar();
                                    return;

                                }
                                else
                                {
                                    this.Alerta("Estimado cliente, se presentó un problema durante la cancelación de pases, por favor comunicarse con IT CGSA...");
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                                    limpiar();
                                    return;
                                }

                            }

                        }
                        else/*todo se genero bien y procede a generar proforma*/
                        {

                            /*cliente de contado*/
                            //verifica si el uusario es de contado o credito
                            bool Credito = usuario.Credito(usn.ruc);

                            //<JGUSQUI 31-05-2023 - LINEA BTS --> SI LA LINEA ES BTS SE LO PONE COMO CREDITO  >
                            if ((s_linea_proforma == "BTS") && (IdDepot == 1))
                            {
                                Credito = true;
                            }
                            //</JGUSQUI 31-05-2023 
                            // if (!usn.IsCredito)
                            if (!Credito)
                            {

                                var proforma = new ProformaContado();
                                proforma.Email = usn.email;
                                proforma.FechaSalida = fechacliente;
                                proforma.IdGrupo = usn.grupo.HasValue ? usn.grupo.Value : 0;
                                proforma.IdUsuario = usn.id;
                                proforma.UsuarioIngreso = usn.loginname;

                                proforma.Referencia = xreferencia.Value;
                                proforma.Nave = nave.Value;
                                proforma.RUC = usn.ruc;
                                proforma.Token = token != null ? token.Value : "DEBUG"; ;

                                proforma.Estado = true;
                                proforma.Bokingnbr = nbrboo.Value;
                                proforma.Cantidad = Cantidad;
                                proforma.Reserva = int.Parse(resqty.Value);

                                ////Nuevos campos
                                proforma.FechaCliente = fechacliente;
                                proforma.Etd = fechacliente;
                                proforma.Reefer = true;
                                proforma.Cutoff = fechacliente;
                                proforma.Size = "";
                                proforma.chofer = Chofer;
                                proforma.placa = Placa;

                                objServicios = Session["Servicios"] as Servicios;

                                if (objServicios == null)
                                {
                                    this.sinresultado.InnerText = string.Format("Error: No se obtuvo Session[Servicios]");
                                    sinresultado.Visible = true;
                                    btnera.Visible = false;
                                    alerta.Visible = false;
                                    xfinder.Visible = false;
                                    this.Alerta(this.sinresultado.InnerText);
                                    return;
                                }

                                var secu = 1;
                                foreach (var r in objServicios.Detalle)
                                {

                                    var detalle = new ProformaContadoDetalle();
                                    detalle.BL = nbrboo.Value;
                                    detalle.Cantidad = r.cantidad;
                                    detalle.CodigoServicio = r.codigo;
                                    detalle.DescServicio = r.descripcion;
                                    detalle.FechaAlmacenaje = DateTime.Now;
                                    detalle.Item = secu;
                                    detalle.Referencia = xreferencia.Value;
                                    detalle.ValorTotal = r.vtotal;
                                    detalle.ValorUnitario = r.costo;
                                    ValotUnitario = r.costo;
                                    proforma.Detalle.Add(detalle);
                                    totalIva += r.vtotal;
                                    secu++;
                                    nIva = r.iva;

                                }
                                /*detalle de turnos*/
                                var results = from p in dtTurnosZAL.AsEnumerable().AsParallel()
                                              where p.Field<String>("IDPZAL") != null
                                              select new
                                              {
                                                  FECHA = fechasal.ToString("yyyy-MM-dd") + " " + p.Field<String>("TURNO"),
                                                  CHOFER = p.Field<String>("CHOFER"),
                                                  PLACA = p.Field<String>("PLACA")
                                              };

                                foreach (var item in results)
                                {
                                    var detalle = new DetalleTurnos();
                                    detalle.fecha = item.FECHA;
                                    detalle.chofer = item.CHOFER;
                                    detalle.placa = item.PLACA;
                                    proforma.Detalle_Turnos.Add(detalle);
                                }
                                /*fin detalle de turnos*/


                                var subt = totalIva;
                                var iva = (subt * (nIva / 100));
                                var tot = (subt + iva);
                                ValotUnitario = ValotUnitario + (ValotUnitario * (nIva / 100));

                                proforma.ReteFuente = 0;
                                proforma.ReteIVA = 0;
                                proforma.IVA = iva;
                                proforma.PctIVA = nIva;
                                proforma.PctFuente = 0;
                                proforma.IvaMas = 0;

                                proforma.SubTotal = totalIva;
                                proforma.Total = tot;


                                var msm = string.Empty;
                                string se;

                                se = proforma.GuardarContado(usn, out msm);
                                /*error durante la generacion de liquidacion*/
                                if (se == null || msm != string.Empty)
                                {
                                    string NewMensaje = msm;
                                    this.sinresultado.InnerText = string.Format(msm);
                                    sinresultado.Visible = true;
                                    btnera.Visible = false;
                                    alerta.Visible = false;
                                    xfinder.Visible = false;
                                    //this.Alerta(this.sinresultado.InnerText);

                                    /*reverso pase puerta de la zal*/
                                    var dtPPWebErr = (from p in dtTurnosZAL.AsEnumerable()
                                                      where p.Field<String>("IDPZAL") != null
                                                      select p).AsDataView().ToTable();

                                    if (dtPPWebErr.Rows.Count != 0)
                                    {
                                        XDocument docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                                            new XElement("PASEPUERTA", from p in dtTurnosZAL.AsEnumerable().AsParallel()
                                                                                       where p.Field<String>("IDPZAL") != null
                                                                                       select new XElement("VBS_T_PASE_PUERTA_ZAL",
                                                                                        new XAttribute("ID_PPZAL", p.Field<String>("IDPZAL") == null ? "0" : p.Field<String>("IDPZAL").ToString()),
                                                                                        new XAttribute("ESTADO", "CA"),
                                                                                        new XAttribute("USR_MOD", usn.loginname),
                                                                                        new XAttribute("FECHA_MOD", DateTime.Now.ToString("MM/dd/yyyy HH:mm"))
                                                                                        )));
                                        /*si esta todo ok la cancelacion*/
                                        if (man_pro_expo.SaveCancelarPasePuertaZAL(docXML.ToString(), out Mensaje))
                                        {
                                            this.Alerta(string.Format("{0} Estimado cliente, se presentó un problema, los pases generados se cancelaron, por favor comunicarse con IT CGSA...", NewMensaje.ToString()));
                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);

                                            limpiar();
                                            return;

                                        }
                                        else
                                        {
                                            this.Alerta(string.Format("{0} Estimado cliente, se presentó un problema durante la cancelación de pases, por favor comunicarse con IT CGSA...", NewMensaje.ToString()));
                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                                            limpiar();
                                            return;
                                        }

                                    }

                                    this.Alerta(NewMensaje);
                                    limpiar();
                                    return;

                                }
                                else/*no existe error en la generacion de proforma de contado*/
                                {

                                    /*actualizo numero de liquidacion en los pase de puerta*/
                                    var dtPPWebLiq = (from p in dtTurnosZAL.AsEnumerable()
                                                      where p.Field<String>("IDPZAL") != null
                                                      select p).AsDataView().ToTable();

                                    if (dtPPWebLiq.Rows.Count != 0)
                                    {
                                        XDocument docXML = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                                            new XElement("PASEPUERTA", from p in dtTurnosZAL.AsEnumerable().AsParallel()
                                                                                       where p.Field<String>("IDPZAL") != null
                                                                                       select new XElement("VBS_T_PASE_PUERTA_ZAL",
                                                                                        new XAttribute("ID_PPZAL", p.Field<String>("IDPZAL") == null ? "0" : p.Field<String>("IDPZAL").ToString()),
                                                                                        new XAttribute("VALORUNITARIO", ValotUnitario),
                                                                                          new XAttribute("IdProforma", se == null ? "0" : se.ToString()),
                                                                                        new XAttribute("LIQUIDACION", proforma.Liquidacion)
                                                                                        )));

                                        //this.Alerta(docXML.ToString().Trim());

                                        /*si se presenta error en la actualización de pases con la liquidacion*/
                                        if (!man_pro_expo.ActualizaLiquidacionPasePuertaZAL(docXML.ToString(), out Mensaje))
                                        {
                                            this.Alerta(string.Format("{0} Estimado cliente, se presentó un problema, error al actualizar # liquidación, por favor comunicarse con IT CGSA...", Mensaje));
                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);

                                            //this.sinresultado.InnerText = string.Format(docXML.ToString());
                                            //sinresultado.Visible = true;

                                        }
                                        else
                                        {

                                        }

                                    }


                                    //if (cmbDeposito.SelectedValue == "2")
                                    //{

                                    //    //CREACION DE CITA SISTEMA DE OPACIFIC
                                    //    foreach (DataRow dr_Fila in dtPPWebLiq.Rows)
                                    //    {
                                    //        try
                                    //        {

                                    //            ServicioOpacific _servicio = new ServicioOpacific();
                                    //            Dictionary<string, string> obj = new Dictionary<string, string>();

                                    //            string v_datoChefer = dr_Fila["CHOFER"].ToString();
                                    //            string[] v_chofer = v_datoChefer.Split('-');
                                    //            DateTime v_fecha = DateTime.Parse(dr_Fila["fec"].ToString());

                                    //            //obj.Add("fecha", DateTime.Now.ToString("dd/MM/yyyy"));

                                    //            obj.Add("key", System.Configuration.ConfigurationManager.AppSettings["OPACIFIC_KEY"]);
                                    //            obj.Add("usuario_id", System.Configuration.ConfigurationManager.AppSettings["OPACIFIC_USER_ID"]);
                                    //            //obj.Add("fecha", v_fecha.ToString("dd/MM/yyyy")); //DateTime.Now.ToString("dd/MM/yyyy")); //
                                    //            obj.Add("fecha", dr_Fila["fec"].ToString()); //DateTime.Now.ToString("dd/MM/yyyy")); //
                                    //            obj.Add("hora", int.Parse(dr_Fila["Inicio"].ToString().Substring(0,2)).ToString());
                                    //            obj.Add("transportista", v_chofer[1].ToString().Trim());
                                    //            obj.Add("ruc", v_chofer[0].ToString().Trim());
                                    //            obj.Add("booking", nbrboo.Value.ToString().Replace("_CISE",String.Empty));
                                    //            obj.Add("linea_naviera", "MAERSK LINE");
                                    //            obj.Add("cedula", v_chofer[0].ToString().Trim());
                                    //            obj.Add("chofer", v_chofer[1].ToString().Trim());
                                    //            obj.Add("placa", dr_Fila["PLACA"].ToString());
                                    //            obj.Add("cita_contecon", dr_Fila["IDPZAL"].ToString());
                                    //            obj.Add("accion", System.Configuration.ConfigurationManager.AppSettings["ACCION_CREACITA"]);

                                    //            Respuesta _result = _servicio.Peticion("CREAR_CITA", obj);
                                    //            if (_result.result.estado != 1)
                                    //            {
                                    //                man_pro_expo.CancelarTurnoPorLinea(long.Parse(dr_Fila["IDPZAL"].ToString()), usn.loginname, out Mensaje);

                                    //                Exception ex;
                                    //                ex = new Exception("Error al crear cita en OPACIFIC, El pase : " + dr_Fila["IDPZAL"].ToString() + " se ha cancelado; Respuesta de SW Opacific: " + _result.result.mensaje );
                                    //                var number = log_csl.save_log<Exception>(ex , "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                    //                this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " Mensaje Servicio: " + _result.result.mensaje + " #Error: " + number);
                                    //                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                                    //            }
                                    //            else
                                    //            {

                                    //                man_pro_expo.UpdateTurnoPorLinea(long.Parse(dr_Fila["IDPZAL"].ToString()), long.Parse(_result.result.mensaje.ToString()), out Mensaje);
                                    //            }
                                    //        }
                                    //        catch(Exception ex)
                                    //        {
                                    //            Exception exc = new Exception(" Booking:" + nbrboo.Value.ToString() + " - " + ex.Message);
                                    //            var number = log_csl.save_log<Exception>(exc, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                    //        }
                                    //    }


                                    //}

                                    /*se imprime proforma de contado*/
                                    var sid = QuerySegura.EncryptQueryString(se);
                                    this.Popup(string.Format("../zal/printproforma_zal.aspx?sid={0}", sid));

                                    //esto es para limpiar
                                    this.sinresultado.InnerText = string.Format("Proforma {0} generada con éxito.", proforma.Secuencia);
                                    sinresultado.Visible = true;
                                    btnera.Visible = false;
                                    alerta.Visible = false;
                                    xfinder.Visible = false;
                                    //nbrboo.Value = string.Empty;


                                }


                            }
                            else
                            {
                                /*credito proceso normal*/
                               
                            }

                            //if (cmbDeposito.SelectedValue == "2")
                            //{
                            //    var dtPPWebLiq = (from p in dtTurnosZAL.AsEnumerable()
                            //                      where p.Field<String>("IDPZAL") != null
                            //                      select p).AsDataView().ToTable();

                            //    //CREACION DE CITA SISTEMA DE OPACIFIC
                            //    foreach (DataRow dr_Fila in dtPPWebLiq.Rows)
                            //    {

                            //        long N_IDPZAL  = long.Parse(dr_Fila["IDPZAL"].ToString());

                            //        try
                            //        {

                            //            ServicioOpacific _servicio = new ServicioOpacific();
                            //            Dictionary<string, string> obj = new Dictionary<string, string>();

                            //            string v_datoChefer = dr_Fila["CHOFER"].ToString();
                            //            string[] v_chofer = v_datoChefer.Split('-');
                            //            //DateTime v_fecha = DateTime.Parse(dr_Fila["fec"].ToString());

                            //            //obj.Add("fecha", DateTime.Now.ToString("dd/MM/yyyy"));

                            //            obj.Add("key", System.Configuration.ConfigurationManager.AppSettings["OPACIFIC_KEY"]);
                            //            obj.Add("usuario_id", System.Configuration.ConfigurationManager.AppSettings["OPACIFIC_USER_ID"]);
                            //            //obj.Add("fecha", v_fecha.ToString("dd/MM/yyyy")); //DateTime.Now.ToString("dd/MM/yyyy")); //
                            //            obj.Add("fecha", dr_Fila["fec"].ToString()); //DateTime.Now.ToString("dd/MM/yyyy")); //
                            //            obj.Add("hora", int.Parse(dr_Fila["Inicio"].ToString().Substring(0, 2)).ToString());
                            //            obj.Add("transportista", v_chofer[1].ToString().Trim());
                            //            obj.Add("ruc", v_chofer[0].ToString().Trim());
                            //            obj.Add("booking", nbrboo.Value.ToString().Replace("_CISE", String.Empty));
                            //            obj.Add("linea_naviera", "MAERSK LINE");
                            //            obj.Add("cedula", v_chofer[0].ToString().Trim());
                            //            obj.Add("chofer", v_chofer[1].ToString().Trim());
                            //            obj.Add("placa", dr_Fila["PLACA"].ToString());
                            //            obj.Add("cita_contecon", dr_Fila["IDPZAL"].ToString());
                            //            obj.Add("accion", System.Configuration.ConfigurationManager.AppSettings["ACCION_CREACITA"]);

                            //            Respuesta _result = _servicio.Peticion("CREAR_CITA", obj);
                            //            if (_result.result.estado != 1)
                            //            {
                            //                man_pro_expo.CancelarTurnoPorLinea(long.Parse(dr_Fila["IDPZAL"].ToString()), usn.loginname, out Mensaje);

                            //                Exception ex;
                            //                ex = new Exception("Error al crear cita en OPACIFIC, El pase : " + dr_Fila["IDPZAL"].ToString() + " se ha cancelado; Respuesta de SW Opacific: " + _result.result.mensaje);
                            //                var number = log_csl.save_log<Exception>(ex, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                            //                this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " Mensaje Servicio: " + _result.result.mensaje + " #Error: " + number);
                            //                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                            //            }
                            //            else
                            //            {

                            //                man_pro_expo.UpdateTurnoPorLinea(long.Parse(dr_Fila["IDPZAL"].ToString()), long.Parse(_result.result.mensaje.ToString()), out Mensaje);
                            //            }
                            //        }
                            //        catch (Exception ex)
                            //        {
                            //            //17-03-2020
                            //            man_pro_expo.CancelarTurnoPorLinea(N_IDPZAL,"error", out Mensaje);

                            //            Exception exc = new Exception(" Booking:" + nbrboo.Value.ToString() + " - " + ex.Message);
                            //            var number = log_csl.save_log<Exception>(exc, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                            //        }
                            //    }

                            //}
                            //FIN NUEVOS PROCESO
                            nbrboo.Value = string.Empty;

                        }
                    }




                    /*imprime pases normal, si todo esta bien, no importa si es contado o credito*/
                    var dtPPWeb = (from p in dtTurnosZAL.AsEnumerable()
                                   where p.Field<String>("IDPZAL") != null
                                   select p).AsDataView().ToTable();

                    StringWriter xmlPPWeb = new StringWriter();
                    dtPPWeb.TableName = "PaseWeb";
                    dtPPWeb.WriteXml(xmlPPWeb);

                    Session["idpaseszal"] = xmlPPWeb.ToString();
                    lbltotturnos.Text = "";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "openPopReporte();ocultagifloader();reload();", true);




                    if (this.Activar == "SI")/*si esta activa la validacion de cobros de turnos zal*/
                    {
                        this.limpiar();

                        //this.Alerta("Para poder imprimir su pase de puerta, debe pagar la proforma generada, luego de esto podrá imprimir el mismo mediante la opción de cancelación o reimpresión de pase a puerta.");

                    }



                }


            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
            }
        }

        public static string CreatePase(string book, string ruc, string exp, string refe, string liq, string cant, string plc, string fec, string plan, string sec, string turno, string ini, string fin, string us, string placa, string chofer,
            string ruc_cliente = "",
            string name_cliente = "",
            string ruc_asume = "",
            string name_asume = "",
            long id_asignacion = 0)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<PASEPUERTA>");
            sb.Append(string.Format("<BOOKING>{0}</BOOKING>", book));//fila.FECHA != null ? ((DateTime)fila.FECHA).ToString("yyyy-MM-dd") : null));
            sb.Append(string.Format("<RUC>{0}</RUC>", ruc));
            sb.Append(string.Format("<EXPORTADOR>{0}</EXPORTADOR>", exp));
            sb.Append(string.Format("<REFERENCIA>{0}</REFERENCIA>", refe));
            sb.Append(string.Format("<LIQUIDACION>{0}</LIQUIDACION>", liq));
            sb.Append(string.Format("<FECHA_SALIDA>{0}</FECHA_SALIDA>", fec));
            sb.Append(string.Format("<CANTIDAD>{0}</CANTIDAD>", cant));
            /*
            if (plc!=null)
                sb.Append(string.Format("<PLACA>{0}</PLACA>", plc));
            else
                sb.Append("<PLACA></PLACA>");
            */
            sb.Append(string.Format("<PLAN>{0}</PLAN>", plan));
            sb.Append(string.Format("<SECUENCIA>{0}</SECUENCIA>", sec));
            sb.Append(string.Format("<TURNO>{0}</TURNO>", turno));
            sb.Append(string.Format("<INICIO>{0}</INICIO>", ini));
            sb.Append(string.Format("<FIN>{0}</FIN>", fin));
            sb.Append(string.Format("<USUARIO>{0}</USUARIO>", us));
            sb.Append(string.Format("<PLACA>{0}</PLACA>", placa));
            sb.Append(string.Format("<CHOFER>{0}</CHOFER>", chofer));
            sb.Append(string.Format("<RUC_CLIENTE>{0}</RUC_CLIENTE>", ruc_cliente));
            sb.Append(string.Format("<NAME_CLIENTE>{0}</NAME_CLIENTE>", name_cliente));
            sb.Append(string.Format("<RUC_ASUME>{0}</RUC_ASUME>", ruc_asume));
            sb.Append(string.Format("<NAME_ASUME>{0}</NAME_ASUME>", name_asume));
            sb.Append(string.Format("<ID_ASIGNACION>{0}</ID_ASIGNACION>", id_asignacion));
            sb.Append("</PASEPUERTA>");

            return sb.ToString();
        }

        protected void btnCancelaPase(string idPase)
        {
            try
            {
                app_start.RepContver.ServicioCISE _servicio = new app_start.RepContver.ServicioCISE();
                Dictionary<string, string> obj = new Dictionary<string, string>();

                obj.Add("AN_ID_TURNO_REFERENCIA", idPase.ToString());
                obj.Add("AV_ESTADO", "X");
                

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


        #endregion

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                
                //if (dtListaBooking.Rows.Count == 0)
                //{
                //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + "Aun no Agrega Bookings a la lista." + "');getGifOcultaBuscar();", true);
                //    return;
                //}

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

                //for (int i = 0; i < dtListaBooking.Rows.Count; i++)
                //{
                //    if (!man_pro_expo.AddBookAutoriza(
                //        dtListaBooking.Rows[i]["BOOKING"].ToString(),
                //        dtListaBooking.Rows[i]["REFERENCIA"].ToString(),
                //        dtListaBooking.Rows[i]["LINEA"].ToString(),
                //        Page.User.Identity.Name.ToUpper(),
                //        out mensaje))
                //    {
                //        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + mensaje + "');", true);
                //        cont += 1;
                //        mensajeu = mensaje;
                //        if (mensaje.Contains("ya se encuentra autorizado"))
                //            dtListaBooking.Rows[i]["MENSAJE"] = "Registro duplicado";
                //        else
                //            dtListaBooking.Rows[i]["MENSAJE"] = mensaje.Length > 30 ? mensaje.Substring(1, 30) : mensaje;
                //    }
                //    else
                //    {


                //        //if (!string.IsNullOrEmpty(error))
                //        //{
                //        //    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", error, true);
                //        //    return;
                //        //}
                //        add += 1;
                //        dtListaBooking.Rows[i]["MENSAJE"] = "Registrado con éxito";
                //    }
                //}

                //RepeaterBooking.DataSource = dtListaBooking;
                //RepeaterBooking.DataBind();

                if (cont == 0)
                    Response.Write("<script language='JavaScript'>var r=alert('Transacción exitosa todos los bookings fueron registrados.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>");
                else
                {
                    this.Alerta(string.Format("Algunos registros reportaron error verifique los mensajes: bookings registrados {0}, bookings con error {1}.", add,cont));
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

        protected void RepeaterBooking_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete") //breakpoint on this line
            {
                //dtListaBooking.Rows.RemoveAt(e.Item.ItemIndex);
                //dtListaBooking.AcceptChanges();
                //RepeaterBooking.DataSource = dtListaBooking;
                //RepeaterBooking.DataBind();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
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
                //06-abr-2020
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

                this.limpiar();
            }
            catch
            {

            }
        }


        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "btnRemove_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

    }
}