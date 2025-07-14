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
    public partial class mant_pago_terceros : System.Web.UI.Page
    {
        public DataTable dtListaAsumeCliente
        {
            get { return (DataTable)Session["dtMantListaAsumeCliente"]; }
            set { Session["dtMantListaAsumeCliente"] = value; }
        }

        public String emailCliente
        {
            get { return (String)Session["MantemailCliente"]; }
            set { Session["MantemailCliente"] = value; }
        }

        public String userLogin
        {
            get { return (String)Session["MantPagoTercerosUsuaurioLogin"]; }
            set { Session["MantemailCliente"] = value; }
        }

        public Boolean ClienteBloqueado
        {
            get { return (Boolean)Session["ValClienteBloqueado"]; }
            set { Session["ValClienteBloqueado"] = value; }
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
                Session["MantPagoTercerosUsuaurioLogin"] = user.loginname;
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
                    Session["dtMantListaAsumeCliente"] = new DataTable();
                    dtListaAsumeCliente.Columns.Add("RUCCLI");
                    dtListaAsumeCliente.Columns.Add("RAZSOCIALCLI");
                    dtListaAsumeCliente.Columns.Add("RUCASUME");
                    dtListaAsumeCliente.Columns.Add("RAZSOCIALASUME");
                    RepeaterAsume.DataSource = dtListaAsumeCliente;
                    RepeaterAsume.DataBind();
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "mant_pago_terceros", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
            }
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                var dtrazsocialcliasume = man_pro_expo.GetCatalagoClientesPagoTerceros(this.agencia.Value);
                var desrazsocialcliasume = string.Empty;
                if (dtrazsocialcliasume.Rows.Count > 0)
                {
                    desrazsocialcliasume = dtrazsocialcliasume.Rows[0]["CLNT_NAME"].ToString();
                }
                */
                if (dtListaAsumeCliente.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + "Aun no Asume el Pago de ningún Cliente." + "');getGifOcultaBuscar();", true);
                    return;
                }
                /*
                string mail = string.Empty;
                string error = string.Empty;
                string correoBackUp = string.Empty;
                string mail_em = string.Empty;
                string sUser_email = string.Empty;
                string destinatarios = string.Empty;
                string mensajeerror = string.Empty;

                string mailContenedores = string.Empty;                
                */
                dtListaAsumeCliente.TableName = "PagoTerceros";
                StringWriter sw = new StringWriter();
                dtListaAsumeCliente.WriteXml(sw);
                String xmlPagoTerceros = sw.ToString();
                string mensaje = null;
                if (!man_pro_expo.AddMantPagoTerceros(
                    xmlPagoTerceros,
                    Page.User.Identity.Name.ToUpper(),
                    out mensaje))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaEnviar('¡ " + mensaje + "');", true);
                }
                else
                {
                    /*
                    for (int i = 0; i < dtListaAsumeCliente.Rows.Count; i++)
                    {
                        string mailContenedoresXCli = string.Empty;
                        var c = man_pro_expo.GetCntrXBook(dtListaAsumeCliente.Rows[i]["RUC"].ToString(), dtListaAsumeCliente.Rows[i]["REFERENCIA"].ToString());
                        if (c.Rows.Count == 0)
                        {
                            mailContenedores = string.Concat(mailContenedores,
                            string.Format(
                                "<strong>Cliente: </strong>{0}<strong>     Referencia: </strong>{1}<strong>     Nave: </strong>{2}<strong>     Unidad: </strong>{3}<strong>     Booking: </strong>{4}<strong>     DAE: </strong>{5}<br/>",
                                dtListaAsumeCliente.Rows[i]["RUC"].ToString() + " - " + dtListaAsumeCliente.Rows[i]["RAZSOCIAL"].ToString(), dtListaAsumeCliente.Rows[i]["REFERENCIA"].ToString(), dtListaAsumeCliente.Rows[i]["NAVE"].ToString(), "", "", ""));
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

                        mail = string.Concat(mail, string.Format("Estimado/a, " + dtListaAsumeCliente.Rows[i]["RAZSOCIAL"].ToString() + " {0} {1}:<br/><br/>Este es un mensaje del Sistema de Solicitud de Servicios de Contecon Guayaquil S.A, para comunicarle lo siguiente:<br/><br/>", "", ""));
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

                    mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/><br/>Este es un mensaje del Sistema de Solicitud de Servicios de Contecon Guayaquil S.A, para comunicarle lo siguiente:<br/><br/>", "", ""));
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
                    */

                    Response.Write("<script language='JavaScript'>var r=alert('Transacción registrada exitosamente.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>");
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "mant_pago_terceros", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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

        protected void btnAsumir_Click(object sender, EventArgs e)
        {
            try
            {
                if (xruca.Value == xruc.Value)
                {
                    this.Alerta("! No puede asumir el pago de usted mismo");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    return;
                }
                var query = from myRow in dtListaAsumeCliente.AsEnumerable()
                            where myRow.Field<string>("RUCCLI") == xruca.Value && myRow.Field<string>("RUCASUME") == xruc.Value
                            select myRow;
                DataTable tbresult = query.AsDataView().ToTable();
                if (tbresult.Rows.Count == 1)
                {
                    this.Alerta("El Cliente: " + xruca.Value + " - "+ xrazsoca.Value + "\\nya Asumio el Pago de: " + xruc.Value + " - " + xrazonsocial.Value);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    return;
                }
                
                dtListaAsumeCliente.Rows.Add(xruca.Value, xrazsoca.Value, xruc.Value, xrazonsocial.Value);
                RepeaterAsume.DataSource = dtListaAsumeCliente;
                RepeaterAsume.DataBind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                this.tablePaginationClientes.DataSource = null;
                this.tablePaginationClientes.DataBind();
                txtrucbuscar.Focus();
                //this.tablePagination.DataSource = null;
                //this.tablePagination.DataBind();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "mant_pago_terceros", "btnAsumir_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "mant_pago_terceros", "btnRemove_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var c = man_pro_expo.GetCatalagoClientesPagoTerceros(txtrucbuscar.Text.Trim());
                this.tablePaginationClientes.DataSource = c;
                this.tablePaginationClientes.DataBind();
                if (c.Rows.Count == 0)
                {
                    this.Alerta("No se encontraron datos con el RUC: " + txtrucbuscar.Text.Trim());
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    txtrucbuscar.Focus();
                    return;
                }
                //txtrucbuscar.Text = "";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "mant_pago_terceros", "btnBuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            }
        }

        protected void btBuscarCli_Click(object sender, EventArgs e)
        {
            try
            {
                var c = man_pro_expo.GetCatalagoClientesPagoTerceros(txtrucbuscarcli.Text.Trim());
                this.tablePagination.DataSource = c;
                this.tablePagination.DataBind();
                if (c.Rows.Count == 0)
                {
                    this.Alerta("No se encontraron datos con el RUC: " + txtrucbuscarcli.Text.Trim());
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    txtrucbuscarcli.Focus();
                    return;
                }
                //txtrucbuscarcli.Text = "";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "mant_pago_terceros", "btnBuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
            }
        }
    }
}