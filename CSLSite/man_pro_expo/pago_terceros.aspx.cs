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

namespace CSLSite
{
    public partial class pago_terceros : System.Web.UI.Page
    {
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

        SI_Customer_Statement_NAVIS_CGSA.Ws_Sap_EstadoDeCuenta WsSapEstadoDeCuenta = new SI_Customer_Statement_NAVIS_CGSA.Ws_Sap_EstadoDeCuenta();
        //AntiXRCFG.
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
                    dtListaAsumeCliente = new DataTable();
                    dtListaAsumeCliente.Columns.Add("REFERENCIA");
                    dtListaAsumeCliente.Columns.Add("NAVE");
                    dtListaAsumeCliente.Columns.Add("BOOKING");
                    dtListaAsumeCliente.Columns.Add("CODIGOSAP");
                    dtListaAsumeCliente.Columns.Add("RUC");
                    dtListaAsumeCliente.Columns.Add("RAZSOCIAL");
                    dtListaAsumeCliente.Columns.Add("EMAIL");
                    dtListaAsumeCliente.Columns.Add("TIPO");
                    RepeaterAsume.DataSource = dtListaAsumeCliente;
                    RepeaterAsume.DataBind();

                    dtListaBooking = new DataTable();
                    dtListaBooking.Columns.Add("BOOKING");
                    RepeaterBooking.DataSource = dtListaBooking;
                    RepeaterBooking.DataBind();

                    var r = man_pro_expo.GetCatalagoReferenciasPagoTerceros();
                    this.tablePaginationReferencias.DataSource = r;
                    this.tablePaginationReferencias.DataBind();
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "pago_terceros", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
            }
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var dtrazsocialcliasume = man_pro_expo.GetCatalagoClientesPagoTerceros(this.agencia.Value);
                var desrazsocialcliasume = string.Empty;
                if (dtrazsocialcliasume.Rows.Count > 0)
                {
                    desrazsocialcliasume = dtrazsocialcliasume.Rows[0]["CLNT_NAME"].ToString();
                }
                if (dtListaAsumeCliente.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + "Aun no Asume el Pago de ningún Cliente." + "');getGifOcultaBuscar();", true);
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

                dtListaAsumeCliente.TableName = "PagoTerceros";
                StringWriter sw = new StringWriter();
                dtListaAsumeCliente.WriteXml(sw);
                String xmlPagoTerceros = sw.ToString();
                string mensaje = null;
                if (!man_pro_expo.AddPagoTerceros(
                    this.agencia.Value,
                    xmlPagoTerceros,
                    Page.User.Identity.Name.ToUpper(),
                    out mensaje))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + mensaje + "');", true);
                }
                else
                {
                    for (int i = 0; i < dtListaAsumeCliente.Rows.Count; i++)
                    {
                        string mailContenedoresXCli = string.Empty;
                        var c = man_pro_expo.GetCntrXBook(dtListaAsumeCliente.Rows[i]["RUC"].ToString(), dtListaAsumeCliente.Rows[i]["REFERENCIA"].ToString());
                        if (c.Rows.Count == 0)
                        {
                            mailContenedores = string.Concat(mailContenedores,
                            string.Format(
                                "<strong>Cliente: </strong>{0}<strong>     Referencia: </strong>{1}<strong>     Nave: </strong>{2}<strong>     Unidad: </strong>{3}<strong>     Booking: </strong>{4}<strong>     DAE: </strong>{5}<br/>",
                                dtListaAsumeCliente.Rows[i]["RUC"].ToString() + " - " + dtListaAsumeCliente.Rows[i]["RAZSOCIAL"].ToString(), dtListaAsumeCliente.Rows[i]["REFERENCIA"].ToString(), dtListaAsumeCliente.Rows[i]["NAVE"].ToString(), "", dtListaAsumeCliente.Rows[i]["BOOKING"].ToString(), ""));
                        }
                        else
                        {
                            for (int f = 0; f < c.Rows.Count; f++)
                            {
                                mailContenedores = string.Concat(mailContenedores,
                                string.Format(
                                    "<strong>Cliente: </strong>{0}<strong>     Referencia: </strong>{1}<strong>     Nave: </strong>{2}<strong>     Unidad: </strong>{3}<strong>     Booking: </strong>{4}<strong>     DAE: </strong>{5}<br/>",
                                    c.Rows[f]["ruc_exportador"].ToString() + " - " + dtListaAsumeCliente.Rows[i]["RAZSOCIAL"].ToString(), c.Rows[f]["breferencia"].ToString(), dtListaAsumeCliente.Rows[i]["NAVE"].ToString(), c.Rows[f]["CONTENEDOR"].ToString(), c.Rows[f]["BOOKING"].ToString(), c.Rows[f]["DAE"].ToString()));

                                mailContenedoresXCli = string.Concat(mailContenedoresXCli,
                                string.Format(
                                    "<strong>Cliente: </strong>{0}<strong>     Referencia: </strong>{1}<strong>     Nave: </strong>{2}<strong>     Unidad: </strong>{3}<strong>     Booking: </strong>{4}<strong>     DAE: </strong>{5}<br/>",
                                    c.Rows[f]["ruc_exportador"].ToString() + " - " + dtListaAsumeCliente.Rows[i]["RAZSOCIAL"].ToString(), c.Rows[f]["breferencia"].ToString(), dtListaAsumeCliente.Rows[i]["NAVE"].ToString(), c.Rows[f]["CONTENEDOR"].ToString(), c.Rows[f]["BOOKING"].ToString(), c.Rows[f]["DAE"].ToString()));

                            }
                        }

                        mail = string.Empty;

                        correoBackUp = "contecon.it@gmail.com";
                        mail_em = "rreyes@cgsa.com.ec";
                        sUser_email = dtListaAsumeCliente.Rows[i]["EMAIL"].ToString();

                        mail = string.Concat(mail, string.Format("Estimado/a, " + dtListaAsumeCliente.Rows[i]["RAZSOCIAL"].ToString() + " {0} {1}:<br/><br/>Este es un mensaje del Sistema de Terminal Virtual de Contecon Guayaquil S.A, para comunicarle lo siguiente:<br/><br/>", "", ""));
                        mail = string.Concat(mail, "A continuación, se detalla información de su carga que es asumida por, " + this.agencia.Value + " - " + desrazsocialcliasume + "<br/><br/> ");
                        mail = string.Concat(mail, string.Format("<strong>Referencia: </strong>{0}<br/><strong>Nave: </strong>{1}<br/>", dtListaAsumeCliente.Rows[i]["REFERENCIA"].ToString(), dtListaAsumeCliente.Rows[i]["NAVE"].ToString()));
                        if (!string.IsNullOrEmpty(mailContenedoresXCli))
                        {
                            mail = string.Concat(mail, string.Format("<br/><br/><strong>Detalle: </strong><br/>"));
                            mail = mail + mailContenedoresXCli;
                        }

                        error = string.Empty;
                        destinatarios = string.Format("{0};{1}", correoBackUp != null ? correoBackUp : "no_cfg", mail_em != null ? mail_em : "no_cfg");
                        CLSDataCentroSolicitud.addMail(out error, sUser_email, "Lista de Pago a Terceros " + "" + "", mail, destinatarios, Page.User.Identity.Name, "", "");

                        if (!string.IsNullOrEmpty(error))
                        {
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", error, true);
                            return;
                        }
                    }

                    mail = string.Empty;

                    correoBackUp = "contecon.it@gmail.com";
                    mail_em = "rreyes@cgsa.com.ec";
                    sUser_email = emailCliente;

                    mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/><br/>Este es un mensaje del Sistema de Terminal Virtual de Contecon Guayaquil S.A, para comunicarle lo siguiente:<br/><br/>", "", ""));
                    mail = string.Concat(mail, string.Format("<br/><strong>Lista de Cliente(s) Asumidos para el pago total: </strong><br/>"));
                    mail = mail + mailContenedores;
                    mail = string.Concat(mail, "<br/><br/> Agradecemos considerar que los valores asumidos, son basados en los valores totales de las cargas en mención. No se asumen valores parciales.");
                    error = string.Empty;
                    destinatarios = string.Format("{0};{1}", correoBackUp != null ? correoBackUp : "no_cfg", mail_em != null ? mail_em : "no_cfg");
                    CLSDataCentroSolicitud.addMail(out error, sUser_email, "Lista de Pago a Terceros " + "" + "", mail, destinatarios, Page.User.Identity.Name, "", "");

                    if (!string.IsNullOrEmpty(error))
                    {
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", error, true);
                        return;
                    }

                    Response.Write("<script language='JavaScript'>var r=alert('Transacción registrada exitosamente.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>");
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "pago_terceros", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + ex.Message + "');", true);
            }
        }

        protected void RepeaterAsume_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete") //breakpoint on this line
            {
                //Label lbReferencia = (Label)e.Item.FindControl("lbReferencia");
                //Label lbCodigoSap = (Label)e.Item.FindControl("lbCodigoSap");

                dtListaAsumeCliente.Rows.RemoveAt(e.Item.ItemIndex);
                dtListaAsumeCliente.AcceptChanges();
                RepeaterAsume.DataSource = dtListaAsumeCliente;
                RepeaterAsume.DataBind();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void RepeaterBooking_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete") //breakpoint on this line
            {
                dtListaBooking.Rows.RemoveAt(e.Item.ItemIndex);
                dtListaBooking.AcceptChanges();
                RepeaterBooking.DataSource = dtListaBooking;
                RepeaterBooking.DataBind();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
            }
        }

        protected void btnAsumir_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClienteBloqueado)
                {
                    this.Alerta("! No puede asumir el pago de un Tercero debido a que presenta valores vencidos, debe contactarse con: tesoreria@cgsa.com.ec");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();getGifOcultaBuscar();", true);
                    return;
                }

                String str_err = string.Empty;
                var parametros_cre = man_pro_expo.getPoblarCredenciales(out str_err);
                if (!string.IsNullOrEmpty(str_err))
                {
                    this.Alerta(str_err);
                    return;
                }
                if (parametros_cre.Rows.Count == 0)
                {
                    this.Alerta("NO SE ENCONTRARON CREDENCIALES DE AUTENTICACIÓN EN LA TABLA GEN_C_PARAMETROS");
                    return;
                }

                var tipo_cliente = ClienteTipo == true ? "1" : "0"; //(0 - CONTADO) -> (>0 - CREDITO)

                string Valida = parametros_cre.Rows[0]["VALIDACION"].ToString();
                if (Valida.Equals("1"))
                {
                    var ec = new EstadoCuenta.Ws_Sap_EstadoDeCuentaSoapClient();
                    var cliente_sap = ec.SI_Customer_Statement_NAVIS_CGSA(xruc.Value, parametros_cre.Rows[0]["USER"].ToString(), parametros_cre.Rows[0]["PASSWORD"].ToString());
                    var cab = cliente_sap.Descendants("CABECERA").FirstOrDefault();

                    var credito = cab.Element("CREDITO").Value;
                    var saldo = cab.Element("SALDO").Value;
                    var bloqueo = cab.Element("BLOQUEO").Value;
                    //var tipo_cliente = ClienteTipo == true ? "1" : "0"; //(0 - CONTADO) -> (>0 - CREDITO)

                    if (bloqueo == "1")
                    {
                        var smensaje = "El Cliente presenta valores vencidos, debe contactarse con: tesoreria@cgsa.com.ec";
                        this.Alerta(smensaje);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();getGifOcultaBuscar();", true);
                        return;
                    }
                }
                


                EnumerableRowCollection<DataRow> query;
                for (int i = 0; i < dtListaBooking.Rows.Count; i++)
                {
                    query = from myRow in dtListaAsumeCliente.AsEnumerable()
                            where myRow.Field<string>("CODIGOSAP") == xcodigosap.Value && myRow.Field<string>("REFERENCIA") == xreferencia.Value.ToString() && myRow.Field<string>("NAVE") == xnave.Value.ToString() && myRow.Field<string>("BOOKING") == dtListaBooking.Rows[i]["BOOKING"].ToString()
                                select myRow;
                    DataTable tbresult = query.AsDataView().ToTable();

                    if (tbresult.Rows.Count == 0)
                    {
                        //this.Alerta("Ya se Asumio la Referencia: " + xreferencia.Value + " y Nave: " + xnave.Value + "\\nCon el booking: " + dtListaBooking.Rows[i]["BOOKING"] + " y Cliente: " + xrazonsocial.Value);
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();getGifOcultaBuscar();", true);
                        //return;
                        dtListaAsumeCliente.Rows.Add(xreferencia.Value, xnave.Value, dtListaBooking.Rows[i]["BOOKING"], xcodigosap.Value, xruc.Value, xrazonsocial.Value, xemail.Value, tipo_cliente);
                    }
                    //dtListaBooking.Rows.Add(nbrboo.Value);
                    //else
                    //    dtListaAsumeCliente.Rows.Add(xreferencia.Value, xnave.Value, nbrboo.Value, xcodigosap.Value, xruc.Value, xrazonsocial.Value, xemail.Value, tipo_cliente);
                }
                
                RepeaterAsume.DataSource = dtListaAsumeCliente;
                RepeaterAsume.DataBind();
                
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                
                this.tablePaginationClientes.DataSource = null;
                this.tablePaginationClientes.DataBind();
                dtListaBooking.Clear();
                RepeaterBooking.DataSource = dtListaBooking;
                RepeaterBooking.DataBind();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "pago_terceros", "btnAsumir_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void btnAsumirBook_Click(object sender, EventArgs e)
        {
            try
            {
                dtListaBooking.Rows.Add(nbrboo.Value);
                RepeaterBooking.DataSource = dtListaBooking;
                RepeaterBooking.DataBind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getBookOculta();", true);
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "pago_terceros", "btnAsumirBook_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
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
                var number = log_csl.save_log<Exception>(ex, "pago_terceros", "btnRemove_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var c = man_pro_expo.GetCatalagoClientesPagoTerceros(txtrucbuscar.Text.Trim());
                if (c.Rows.Count == 0)
                {
                    this.Alerta("No se encontraron datos con el RUC: " + txtrucbuscar.Text.Trim());
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    txtrucbuscar.Focus();
                    return;
                }

                var t = man_pro_expo.GetValidaPagoTerceros(this.agencia.Value, txtrucbuscar.Text.Trim());
                if (t.Rows.Count == 0)
                {
                    this.Alerta("No esta autorizado para asumir el pago del Cliente:\\n" + txtrucbuscar.Text.Trim() + " - " + c.Rows[0]["CLNT_NAME"]);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    return;
                }
                else if (t.Rows[0]["ESTADO"].ToString() == "1")
                {
                    this.tablePaginationClientes.DataSource = c;
                    this.tablePaginationClientes.DataBind();
                }
                txtrucbuscar.Text = "";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "pago_terceros", "btnBuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            }
        }
    }
}