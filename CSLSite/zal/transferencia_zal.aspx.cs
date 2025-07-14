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
using BillionEntidades;

namespace CSLSite.zal
{
    public partial class transferencia_zal : System.Web.UI.Page
    {


        private Servicios objServicios = new Servicios();
        private Pases_Saldos objPases = new Pases_Saldos();
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

        //public DataTable dtConceptos
        //{
        //    get { return (DataTable)Session["dtConceptos"]; }
        //    set { Session["dtConceptos"] = value; }
        //}



        #endregion


        private static int cont = 0;
        private static int pos = 0;

        private Decimal nSaldo_Anterior = 0;
        private Decimal nSaldo_Actual = 0;
        private Decimal nNuevo_Saldo = 0;
        private Decimal nTotal_Pagar = 0;
        private Decimal nSaldo_Validar = 0;
        private Decimal nSubtotal_Pagar = 0;
        private Decimal nValor_Pase = 0;
        private Decimal nTotal_Pase = 0;

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
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "~/login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "~/login.aspx", true);
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

                /*si es usuario de credito sale de la pantalla*/
                if (user != null)
                {

                    objParametro.Obtener_Parametro(out string error);
                    this.Activar = objParametro.config_value;

                    //validaciones usuario de contado
                    if (user.IsCredito)
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario De Crédito "), "Transferencia_Zal", "Page_Init", user.ruc, user.loginname);
                        this.PersonalResponse("Su cuenta de usuario no tiene permisos para esta opción, ya que usted es un cliente de crédito., arregle esto primero con Customer Services", "../cuenta/menu.aspx", true);
                    }
                    //validaciones usuario de contado
                    if (this.Activar == "NO")
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario De Crédito "), "Transferencia_Zal", "Page_Init", user.ruc, user.loginname);
                        this.PersonalResponse("Opción no disponible., arregle esto primero con Customer Services", "../cuenta/menu.aspx", true);
                    }
                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    lbltotturnos.Text = "";
                    IniDsChofer("");
                    IniDsPlaca("");

                    /*para clientes de contado*/
                    this.xfinder.Visible = false;
                    this.sinresultado.Visible = false;
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
            WPASEPUERTA = new WFCPASEPUERTA.WFCPASEPUERTAClient();
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
                coman.Parameters.AddWithValue("@deposito", v_deposito);
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

                /*proformas, agreado 27-08-2019
                jalvarado
                */
                bool error = false;

                var usn = new usuario();
                usn = this.getUserBySesion();
                if (usn == null)
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "proforma", "btnconsultarturnos_Click", "No usuario", Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }


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
                            dtTurnosZAL.Rows[ind]["PASE_CRUZAR"] = "";
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

               
                   
                /*cliente de contado*/
                this.xfinder.Visible = true;
                this.sinresultado.Visible = false;

                int nCantidad = 0;
                if (!int.TryParse(dtPPWeb.Rows.Count.ToString(), out nCantidad))
                {                   
                    error = true;
                    this.sinresultado.InnerText = "Por favor comuníquese con nosotros, al parecer la cantidad de pases presenta problemas.";
                    sinresultado.Visible = true;
                    btnera.Visible = true;
                    alerta.Visible = true;
                    xfinder.Visible = true;
                    this.Alerta(this.sinresultado.InnerText);
                   
                }
                //this.alerta.InnerHtml = string.Format("<strong>Estimado usuario: {0}, Usted tiene {1} turnos seleccionados, los mismo generaran los siguientes valores a pagar.</strong>", usn.loginname, nCantidad.ToString());

                /*valida cantidad de pases no sea mayores al total de pases a transferir*/
                objPases = Session["DetallePases"] as Pases_Saldos;
                if (objPases == null && !error)
                {
                    error = true;
                    this.sinresultado.InnerText = string.Format("Error: No se obtuvo Session[DetallePases]");
                    sinresultado.Visible = true;
                    btnera.Visible = true;
                    alerta.Visible = true;
                    xfinder.Visible = true;
                    this.Alerta(this.sinresultado.InnerText);
                    
                }
                if (nCantidad > objPases.Detalle.Count && !error)
                {
                    error = true;
                    this.sinresultado.InnerText = string.Format("turnos a emitir {0} no pueden superar el total de pases a transferir: {1}", nCantidad, objPases.Detalle.Count);
                    sinresultado.Visible = true;
                    btnera.Visible = true;
                    alerta.Visible = true;
                    xfinder.Visible = true;
                    this.Alerta(this.sinresultado.InnerText);
                    
                }

                if (error)
                {
                    cblturnos.Items[ind].Selected = false;
                    dtTurnosZAL.Rows[ind]["CHECK"] = false;
                    dtTurnosZAL.Rows[ind]["CHOFER"] = "";
                    dtTurnosZAL.Rows[ind]["PLACA"] = "";
                    dtTurnosZAL.Rows[ind]["PASE_CRUZAR"] = "";
                    cblturnos.Items[ind].Text = cblturnos.Items[ind].Text.Substring(0, 5);

                    dtTurnosZAL.AcceptChanges();

                    var dtPPWeb2 = (from p in dtTurnosZAL.AsEnumerable()
                                    where p.Field<string>("CHECK") == "True"
                                    select p).AsDataView().ToTable();

                    tableDetTurnos.DataSource = dtPPWeb2;
                    tableDetTurnos.DataBind();

                    if (!int.TryParse(dtPPWeb2.Rows.Count.ToString(), out nCantidad)) { }
                }

                this.alerta.InnerHtml = string.Format("<strong>Estimado usuario: {0}, Usted tiene {1} turnos seleccionados, los mismo generaran los siguientes valores a pagar.</strong>", usn.loginname, nCantidad.ToString());


                /*calcula totales*/
                populateRepeater(nCantidad);
    
                /*fin de datos de la proforma*/


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
                if (Response.IsClientConnected)
                {
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
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "fchange();", true);


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

                        //obj.Add("booking", nbrboo.Value.ToString().Trim().Replace("_CISE", ""));
                        obj.Add("booking", nbrboo.Value.ToString().Trim().Replace("_RTV", ""));
                        obj.Add("accion", System.Configuration.ConfigurationManager.AppSettings["ACCION_VALIDA"]);

                        CSLSite.app_start.RepContver.Respuesta _result = _servicio.Peticion("VALIDAR_BOOKING", obj);

                        if (_result.result.estado != 1)
                        {
                            this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " - Servicio: " + _result.result.mensaje);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                            cmbDeposito.Focus();
                            return;
                        }

                    }

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
                    int top = 0;
                    string strQty = txtqty.Text;
                    if (!int.TryParse(strQty, out top))
                    {
                        this.Alerta(string.Format("Error: La cantidad solicitada debe ser mayor a cero.: {0}", strQty));
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }

                    if (top == 0)
                    {
                        this.Alerta("La cantidad solicitada debe ser mayor a cero.");
                        txtqty.Focus();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }

                    int nsaldo = 0;
                    string strQty2 = salqty.Value;
                    if (!int.TryParse(strQty2, out nsaldo))
                    {
                        this.Alerta(string.Format("Error: El saldo debe ser mayor a cero: {0}", strQty2));
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                        return;
                    }

                    //int top = int.Parse(txtqty.Text);
                    //if (top == 0)
                    //{
                    //    this.Alerta("La cantidad solicitada debe ser mayor a cero.");
                    //    txtqty.Focus();
                    //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                    //    return;
                    //}
                    if (top > nsaldo)
                    {
                        this.Alerta("La Cantidad Solicitada: " + top.ToString() + ", es mayor a la Disponible: " + nsaldo.ToString());
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        txtqty.Focus();
                        return;
                    }

                    limpiar();

                    dtTurnosZAL = new DataTable();
                    //dtTurnosZAL = N4_P_Cons_Turnos_Disponibles_ZAL(fechasal.ToString("yyyy-MM-dd"), s_linea_proforma.ToString());
                    dtTurnosZAL = N4_P_Cons_Turnos_Disponibles_ZAL(fechasal.ToString("yyyy-MM-dd"), s_linea_proforma.ToString(), long.Parse(cmbDeposito.SelectedValue));
                    dtTurnosZAL.Columns.Add("CHECK");
                    dtTurnosZAL.Columns.Add("IDPZAL");
                    dtTurnosZAL.Columns.Add("CHOFER");
                    dtTurnosZAL.Columns.Add("PLACA");
                    dtTurnosZAL.Columns.Add("PASE_CRUZAR");
                    cblturnos.DataSource = dtTurnosZAL;
                    cblturnos.DataValueField = "Plan";
                    cblturnos.DataTextField = "Inicio";
                    cblturnos.DataBind();
                    lbltotturnos.Text = "Tot. Turnos: " + (dtTurnosZAL.Rows.Count - 1).ToString();

                    tableDetTurnos.DataSource = new DataTable();
                    tableDetTurnos.DataBind();


                    /*valida usuario para mosytrar detalle de pases con saldos*/
                    var usn = new usuario();
                    usn = this.getUserBySesion();

                    if (usn == null)
                    {
                        var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                        var number = log_csl.save_log<Exception>(ex, "proforma", "btnconsultarturnos_Click", "No usuario", Request.UserHostAddress);
                        this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                        return;
                    }



                    /*CARGA DETALLE DE PASES A FAVOR DEL CLIENTE DE CONTADO*/
                    List<Pases_Saldos> ListPases = Pases_Saldos.Detalle_Pases(usn.ruc, long.Parse(cmbDeposito.SelectedValue));
                    if (ListPases != null)
                    {
                        tablePagination.DataSource = ListPases;
                        tablePagination.DataBind();
                        xfinder2.Visible = true;

                        var subt = ListPases.Where(a => a.id_pase != 0).ToList().Sum(b => b.saldo_pase);
                        tot_saldo.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);

                        ViewState["saldo_anterior"] = null;
                        ViewState["saldo_anterior"] = subt;

                        /*almacena el detalle de pases*/
                        objPases = new Pases_Saldos();
                        Session["DetallePases"] = objPases;
                        objPases = Session["DetallePases"] as Pases_Saldos;

                        foreach (var Lista2 in ListPases)
                        {
                            objPases.Detalle.Add(Lista2);
                        }

                        Session["DetallePases"] = objPases;
                    }
                    else
                    {

                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        xfinder2.Visible = false;
                        var subt = 0.00;
                        tot_saldo.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);


                        ViewState["saldo_anterior"] = null;
                        ViewState["saldo_anterior"] = subt;

                        objPases = new Pases_Saldos();
                        Session["DetallePases"] = objPases;
                        objPases = Session["DetallePases"] as Pases_Saldos;
                    }


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

        protected void fecsalida_TextChanged(object sender, EventArgs e)
        {

        }


        #region "Carga datos de la Proforma Concepto a Pagar, limpia datos de proforma"

        /*carga conceptos y calcula valores a pagar*/
        public void populateRepeater(int cantidad)
        {
            try
            {

                string vr = string.Empty;
                int i = 1;
                nTotal_Pase = 0;

                /*saco los tipo de precios, pueden existir hasta dos tipos diferentes*/
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    Label LblValorPase = item.FindControl("lbl_saldo_pase") as Label;
                    var x = LblValorPase.Text;

                    if (!Decimal.TryParse(x, out nValor_Pase))
                    {
                        nValor_Pase = 0;
                        this.sinresultado.InnerText = "Por favor comuníquese con nosotros, al parecer el precio del pases presenta problemas.";
                        sinresultado.Visible = true;
                        btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = false;
                        this.Alerta(this.sinresultado.InnerText);
                        return;
                    }

                    nTotal_Pase = nTotal_Pase + nValor_Pase;

                    if (cantidad == i) { break; }
                    i = i + 1;

                    
                }

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
                    f.mensaje = string.Empty;
                    f.contenido = f.descripcion;
                    f.icont = true;
                    f.cantidad = cantidad;
                    f.vtotal = nTotal_Pase;
                    f.costo = f.costo;
                    objServicios.Detalle.Add(f);
                }

                Session["Servicios"] = objServicios;

                tablaNueva.DataSource = table;
                tablaNueva.DataBind();
                var subt = table.Where(a => a.aplica).ToList().Sum(b => b.vtotal);

                sttsubtotal.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);
                sttal.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);

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
                ViewState["subtotal"] = null;
                ViewState["saldo_actual"] = null;
                ViewState["total"] = null;
                ViewState["cantidad"] = cantidad;

            
                var strSaldo_Anterior = ViewState["saldo_anterior"] != null ? ViewState["saldo_anterior"].ToString() : string.Empty;

                if (!Decimal.TryParse(strSaldo_Anterior, out nSaldo_Anterior) || nSaldo_Anterior <= 0)
                {
                    this.sinresultado.InnerText = string.Format("No existe un saldo anterior para poder transferir los valores: {0}", nSaldo_Anterior);
                    sinresultado.Visible = true;
                    btnera.Visible = false;
                    alerta.Visible = false;
                    xfinder.Visible = false;
                    this.Alerta(this.sinresultado.InnerText);
                    return;
                }

                nSaldo_Actual = (subt - nSaldo_Anterior);
                if (nSaldo_Actual <= 0)
                {
                    nNuevo_Saldo = (nSaldo_Anterior - subt);
                    nTotal_Pagar = 0;
                }
                else
                {
                    nNuevo_Saldo = 0;
                    nTotal_Pagar = (subt - nSaldo_Anterior);
                }

                sttsaldo_anterior.InnerHtml = string.Format("<strong>{0:c}</strong>", nSaldo_Anterior);
                sttsubtotal.InnerHtml = string.Format("<strong>{0:c}</strong>", subt);
                sttsaldo_actual.InnerHtml = string.Format("<strong>{0:c}</strong>", nNuevo_Saldo);
                sttal.InnerHtml = string.Format("<strong>{0:c}</strong>", nTotal_Pagar);

                ViewState["subtotal"] = subt;
                ViewState["saldo_anterior"] = nSaldo_Anterior;
                ViewState["saldo_actual"] = nNuevo_Saldo;
                ViewState["total"] = nTotal_Pagar;

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
            this.tot_saldo.InnerText = "0.00";
            nSaldo_Anterior = 0;
            nSaldo_Actual = 0;
            nNuevo_Saldo = 0;
            nTotal_Pagar = 0;
            nSaldo_Validar = 0;
            nSubtotal_Pagar = 0;

            this.xfinder.Visible = false;
            this.sinresultado.Visible = false;

            tableDetTurnos.DataSource = new DataTable(); ;
            tableDetTurnos.DataBind();

            tablaNueva.DataSource = new DataTable(); ;
            tablaNueva.DataBind();

            cblturnos.DataSource = new DataTable(); ;
            cblturnos.DataBind();

            this.tablePagination.DataSource = new DataTable(); ;
            tablePagination.DataBind();

        }

        #endregion

        #region"Proceso para generar pases y proforma"

        /*proceso de generar pases y proforma para clientes de contado*/
        protected void btnAsumirBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {

                    long IdDepot = long.Parse(cmbDeposito.SelectedValue);
                    bool Generado = false;
                    DateTime fechacliente = DateTime.Now;
                    DateTime pfechacliente = DateTime.Now;
                    DateTime fechasal = new DateTime();

                    CultureInfo enUS = new CultureInfo("es-US");
                    int Cantidad = 0;
                    Decimal Total = 0;
                    Decimal ValotUnitario = 0;
                    string Mensaje_Error = string.Empty;

                    int AC = 1;
                    string Mensaje = string.Empty;


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


                    if (!DateTime.TryParseExact(pfechacliente.Date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacliente))
                    {
                        this.sinresultado.InnerText = "La fecha de la proforma no tiene un formato válido";
                        sinresultado.Visible = true;
                        this.Alerta(this.sinresultado.InnerText);
                        return;
                    }

                    var strQty = ViewState["cantidad"] != null ? ViewState["cantidad"].ToString() : string.Empty;

                    if (!int.TryParse(strQty, out Cantidad) || Cantidad <= 0)
                    {
                        this.sinresultado.InnerText = string.Format("Por favor primero seleccione los turnos a transferir valores QTY: {0}", Cantidad);
                        sinresultado.Visible = true;
                        this.Alerta(this.sinresultado.InnerText);
                        return;
                    }

                    var strSaldoAnterior = ViewState["saldo_anterior"] != null ? ViewState["saldo_anterior"].ToString() : string.Empty;
                    if (!Decimal.TryParse(strSaldoAnterior, out nSaldo_Anterior))
                    {
                        this.sinresultado.InnerText = string.Format("El saldo Anterior no puede ser cero: {0} ", nSaldo_Anterior);
                        sinresultado.Visible = true;
                        this.Alerta(this.sinresultado.InnerText);
                        return;
                    }

                    /*saco nuevamente el saldo actual - para validar*/
                    List<Saldos> Lista = Saldos.Get_Saldo(usn.ruc, IdDepot);
                    var xList = Lista.FirstOrDefault();
                    if (xList != null)
                    {
                        nSaldo_Validar = xList.saldo_final;/*saldo actual*/

                        this.sinresultado.Visible = true;
                        this.sinresultado.InnerText = xList.leyenda;

                        if (nSaldo_Validar < nSaldo_Anterior)
                        {
                            this.sinresultado.InnerText = string.Format("No tiene saldo disponible para realizar la transferencia de valores, debe volver a generar los turnos saldo actual: {0}", nSaldo_Validar);
                            sinresultado.Visible = true;
                            this.Alerta(this.sinresultado.InnerText);
                            return;
                        }
                    }


                    var strValor = ViewState["total"] != null ? ViewState["total"].ToString() : string.Empty;

                    if (!Decimal.TryParse(strValor, out Total))
                    {
                        this.sinresultado.InnerText = string.Format("Por favor primero seleccione los turnos a transferir valores, Total no es valido: {0}", Total);
                        sinresultado.Visible = true;
                        this.Alerta(this.sinresultado.InnerText);
                        return;
                    }



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



                    objPases = Session["DetallePases"] as Pases_Saldos;

                    /*asignacion de pases a cruzar valores*/
                    for (int i = 1; i < dtTurnosZAL.Rows.Count; i++)
                    {
                        if (dtTurnosZAL.Rows[i]["CHECK"].ToString() == "True")
                        {
                            var results = from p in objPases.Detalle.AsEnumerable()
                                          where p.fila == AC
                                          select new
                                          {
                                              PASE = p.id_pase.ToString()
                                          };

                            foreach (var item in results)
                            {
                                dtTurnosZAL.Rows[i]["PASE_CRUZAR"] = item.PASE.ToString();
                            }


                            AC = AC + 1;
                        }
                    }

                    dtTurnosZAL.AcceptChanges();
                    /*fin de relacionar pases con saldos con los nuevos a generar*/

                    /*generacion del pase a puerta zal*/
                    string liq = "9025";
                    for (int i = 1; i < dtTurnosZAL.Rows.Count; i++)
                    {
                        string pase = "";
                        int idpase = 0;


                        if (dtTurnosZAL.Rows[i]["CHECK"].ToString() == "True")
                        {

                            if (dtTurnosZAL.Rows[i]["PASE_CRUZAR"] == null)
                            {
                                Mensaje = string.Format("Error..No existe un número de pase relacionado al turno: {0}", dtTurnosZAL.Rows[i]["turno"].ToString());
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                                Generado = false;
                                break;

                            }

                            try
                            {
                                pase = CreatePase(nbrboo.Value, ruc.Value, ruc.Value, xreferencia.Value, liq, txtqty.Text, "", fechasal.ToString("yyyy-MM-dd"), dtTurnosZAL.Rows[i]["Plan_id"].ToString(), dtTurnosZAL.Rows[i]["Turno_Secuencia"].ToString(), dtTurnosZAL.Rows[i]["turno"].ToString(), dtTurnosZAL.Rows[i]["Inicio"].ToString(), dtTurnosZAL.Rows[i]["Fin"].ToString(), Page.User.Identity.Name, dtTurnosZAL.Rows[i]["PLACA"].ToString(), dtTurnosZAL.Rows[i]["CHOFER"].ToString(), dtTurnosZAL.Rows[i]["PASE_CRUZAR"].ToString());
                                Generado = true;
                            }
                            catch (Exception ex)
                            {
                                string cmsg = ex.Message.ToString();
                                Mensaje = cmsg;
                                Generado = false;
                                break;
                            }

                            if (Generado)
                            {
                                if (man_pro_expo.AddPaseZAL_Transferencia(pase, emailCliente.ToString(), out idpase, out Mensaje_Error, IdDepot))
                                {
                                    /*verifico que sea un pase valido y no de error*/
                                    if (Mensaje_Error != string.Empty)
                                    {
                                        Mensaje = Mensaje_Error;
                                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                                        Generado = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (idpase != 0)
                                        {
                                            dtTurnosZAL.Rows[i]["IDPZAL"] = idpase;
                                            Generado = true;
                                        }
                                        else
                                        {
                                            Generado = false;
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
                                                                            new XAttribute("PASE_CRUZAR", p.Field<String>("PASE_CRUZAR") == null ? "0" : p.Field<String>("PASE_CRUZAR").ToString()),
                                                                            new XAttribute("FECHA_MOD", DateTime.Now.ToString("MM/dd/yyyy HH:mm"))
                                                                            )));
                            /*si esta todo ok la cancelacion*/
                            string ms1 = string.Empty;
                            if (man_pro_expo.SaveCancelarPasePuertaZAL_Transferencia(docXML.ToString(), out ms1))
                            {
                                this.Alerta(string.Format("Estimado cliente, se presentó un problema, los pases generados se cancelaron, por favor comunicarse con IT CGSA...{0}", Mensaje));
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);

                                limpiar();
                                return;

                            }
                            else
                            {
                                this.Alerta(string.Format("Estimado cliente, se presentó un problema durante la cancelación de pases, por favor comunicarse con IT CGSA....{0}", Mensaje));
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                                limpiar();
                                return;
                            }

                        }
                        else
                        {
                            this.Alerta(string.Format("Estimado cliente, se presentó un problema durante la emisión de pases, por favor comunicarse con IT CGSA....{0}", Mensaje_Error));
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
                            limpiar();
                            return;
                        }

                    }
                    else/*todo se genero bien y procede a generar proforma*/
                    {
                        if (cmbDeposito.SelectedValue == "5")
                        {
                            var dtPPWebLiq = (from p in dtTurnosZAL.AsEnumerable()
                                              where p.Field<String>("IDPZAL") != null
                                              select p).AsDataView().ToTable();

                            //CREACION DE CITA SISTEMA DE OPACIFIC
                            foreach (DataRow dr_Fila in dtPPWebLiq.Rows)
                            {
                                try
                                {

                                    app_start.RepContver.ServicioCISE _servicio = new app_start.RepContver.ServicioCISE();
                                    Dictionary<string, string> obj = new Dictionary<string, string>();

                                    string v_datoChefer = dr_Fila["CHOFER"].ToString();
                                    string[] v_chofer = v_datoChefer.Split('-');
                                    //DateTime v_fecha = DateTime.Parse(dr_Fila["fec"].ToString());

                                    obj.Add("key", System.Configuration.ConfigurationManager.AppSettings["OPACIFIC_KEY"]);
                                    obj.Add("usuario_id", System.Configuration.ConfigurationManager.AppSettings["OPACIFIC_USER_ID"]);
                                    //obj.Add("fecha", v_fecha.ToString("dd/MM/yyyy"));// DateTime.Now.ToString("dd/MM/yyyy"));
                                    obj.Add("fecha", dr_Fila["fec"].ToString());// DateTime.Now.ToString("dd/MM/yyyy"));
                                    obj.Add("hora", int.Parse(dr_Fila["Inicio"].ToString().Substring(0, 2)).ToString());
                                    obj.Add("transportista", v_chofer[1].ToString().Trim());
                                    obj.Add("ruc", v_chofer[0].ToString().Trim());
                                    obj.Add("booking", nbrboo.Value.ToString().Trim().Replace("_CISE", ""));
                                    obj.Add("linea_naviera", "MAERSK LINE");
                                    obj.Add("cedula", v_chofer[0].ToString().Trim());
                                    obj.Add("chofer", v_chofer[1].ToString().Trim());
                                    obj.Add("placa", dr_Fila["PLACA"].ToString());
                                    obj.Add("cita_contecon", dr_Fila["IDPZAL"].ToString());
                                    obj.Add("accion", System.Configuration.ConfigurationManager.AppSettings["ACCION_CREACITA"]);

                                    CSLSite.app_start.RepContver.Respuesta _result = _servicio.Peticion("CREAR_CITA", obj);
                                    if (_result.result.estado != 1)
                                    {
                                        man_pro_expo.CancelarTurnoPorLinea(long.Parse(dr_Fila["IDPZAL"].ToString()), usn.loginname, out Mensaje);

                                        Exception ex;
                                        ex = new Exception("Error al crear cita en OPACIFIC, El pase : " + dr_Fila["IDPZAL"].ToString() + " se ha cancelado; Respuesta de SW Opacific: " + _result.result.mensaje);
                                        var number = log_csl.save_log<Exception>(ex, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                        this.Alerta(System.Configuration.ConfigurationManager.AppSettings["msjValidaBooking"] + " Mensaje Servicio: " + _result.result.mensaje + " #Error: " + number);
                                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                                    }
                                    else
                                    {
                                        man_pro_expo.UpdateTurnoPorLinea(long.Parse(dr_Fila["IDPZAL"].ToString()), long.Parse(_result.result.mensaje.ToString()), out Mensaje);
                                        //if (!man_pro_expo.UpdateTurnoPorLinea(long.Parse(dr_Fila["IDPZAL"].ToString()), long.Parse(_result.result.mensaje.ToString()), out Mensaje))
                                        //{
                                        //    // se cancela la cita de opacific
                                        //    obj.Add("cita_id", _result.result.mensaje.ToString());
                                        //    Respuesta _results = _servicio.Peticion("CANCELA_CITA", obj);
                                        //}
                                    }
                                    this.Alerta("e-Pass generado Exitosamente... ");
                                }
                                catch (Exception ex)
                                {
                                    Exception exc = new Exception(" Booking:" + nbrboo.Value.ToString() + " - " + ex.Message);
                                    var number = log_csl.save_log<Exception>(exc, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                                }
                            }
                        }
                        else
                        {
                            this.Alerta("e-Pass generado Exitosamente... ");
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagifloader();", true);
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


                    this.limpiar();

                }


            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "proforma_ZAL", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
            }
        }


        public static string CreatePase(string book, string ruc, string exp, string refe, string liq, string cant, string plc, string fec, string plan, string sec, string turno, string ini, string fin, string us, string placa, string chofer, string pase_cruzar)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<PASEPUERTA>");
            sb.Append(string.Format("<BOOKING>{0}</BOOKING>", book));
            sb.Append(string.Format("<RUC>{0}</RUC>", ruc));
            sb.Append(string.Format("<EXPORTADOR>{0}</EXPORTADOR>", exp));
            sb.Append(string.Format("<REFERENCIA>{0}</REFERENCIA>", refe));
            sb.Append(string.Format("<LIQUIDACION>{0}</LIQUIDACION>", liq));
            sb.Append(string.Format("<FECHA_SALIDA>{0}</FECHA_SALIDA>", fec));
            sb.Append(string.Format("<CANTIDAD>{0}</CANTIDAD>", cant));
           
            sb.Append(string.Format("<PLAN>{0}</PLAN>", plan));
            sb.Append(string.Format("<SECUENCIA>{0}</SECUENCIA>", sec));
            sb.Append(string.Format("<TURNO>{0}</TURNO>", turno));
            sb.Append(string.Format("<INICIO>{0}</INICIO>", ini));
            sb.Append(string.Format("<FIN>{0}</FIN>", fin));
            sb.Append(string.Format("<USUARIO>{0}</USUARIO>", us));
            sb.Append(string.Format("<PLACA>{0}</PLACA>", placa));
            sb.Append(string.Format("<CHOFER>{0}</CHOFER>", chofer));
            sb.Append(string.Format("<PASE_CRUZAR>{0}</PASE_CRUZAR>", pase_cruzar));
            sb.Append("</PASEPUERTA>");

            return sb.ToString();
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

                this.limpiar();
            }
            catch
            {

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
    }
}