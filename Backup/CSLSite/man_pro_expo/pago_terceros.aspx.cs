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
namespace CSLSite
{
    public partial class pago_terceros : System.Web.UI.Page
    {
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
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../csl/login", true);
                }
                this.agencia.Value = user.ruc;
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
                var r = man_pro_expo.GetCatalagoReferenciasPagoTerceros();
                this.tablePaginationReferencias.DataSource = r;
                this.tablePaginationReferencias.DataBind();

                var c = man_pro_expo.GetCatalagoClientesPagoTerceros();
                this.tablePaginationClientes.DataSource = c;
                this.tablePaginationClientes.DataBind();
            }
           
        }

        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
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

                var cliente_sap = WsSapEstadoDeCuenta.SI_Customer_Statement_NAVIS_CGSA("txtCliente.Text.Trim()", parametros_cre.Rows[0]["USER"].ToString(), parametros_cre.Rows[0]["PASSWORD"].ToString());

                XmlNodeList nodelist = cliente_sap.SelectNodes("/CABECERA");
                foreach (XmlNode node in nodelist) // for each <testcase> node
                {
                    var credito = node.SelectSingleNode("CREDITO").InnerText;
                    var saldo = node.SelectSingleNode("SALDO").InnerText;
                    if (Convert.ToDecimal(credito) > Convert.ToDecimal(saldo))
                    {
                        this.Alerta("El Credito: " + credito.ToString() + ", " + "es mayor al Saldo: " + saldo.ToString());
                        return;
                    }
                }
                /*
                string mensaje = null;
                if (!man_pro_expo.AddManHorasReefer(
                    xlineanav.Value,
                    txtcanthoras.Text,
                    dfechavigencia.ToString("yyyy-MM-dd"),
                    Page.User.Identity.Name.ToUpper(),
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Transacción registrada exitosamente.');if(r==true){window.location='../csl/menu';}else{window.location='../csl/menu';}</script>");
                }
                */
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudempresa", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }
    }
}