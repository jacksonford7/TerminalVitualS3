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
using System.Xml.Linq;
using System.Data;

namespace CSLSite
{
    public partial class estadocta : System.Web.UI.Page
    {
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
         
            Page.Tracker();
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
                
        }

        public static string FormatoDate(object dato)
        { 
          if(dato==null)
          {
            return "00/00/0000";
          }
            DateTime fech;
            if (DateTime.TryParse(dato.ToString(), out fech))
            {
                return fech.ToString("dd/MM/yyyy");
            }
            return dato.ToString();
        }

        //FormatoDecimal
        public static string FormatoDecimal(object dato)
        {
            if (dato == null)
            {
                return (0).ToString("C");
            }
            decimal fech;
            NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
            CultureInfo enUS = new CultureInfo("en-US");
            if (decimal.TryParse(dato.ToString(),style,enUS, out fech))
            {
                return fech.ToString("C");
            }
            return dato.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var t = this.getUserBySesion();
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }
                    populate(t);
                }
                catch (Exception ex)
                {
                   

                }
            }

        }
        private void populate(CSLSite.usuario usero)
        {
            //CODIGO NUEVO 2017--> CONTROL DE PENDIENTES
            //obtiene el dominio completo
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            string sroot = System.Configuration.ConfigurationManager.AppSettings["SROOT"];// = //SROOT
            if (!string.IsNullOrEmpty(sroot))
            {
                baseUrl = baseUrl + "/" + sroot;
            }
            bool popup = false;
            var ec = new EstadoCuenta.Ws_Sap_EstadoDeCuentaSoapClient();
            //-->cORREGIR EL CODIGO DE BASE
            string sap_user = String.Empty;
            string sap_pass = string.Empty;
            var cfgs = Session["parametros"] as List<dbconfig>;
            var cf = cfgs.Where(f => f.config_name.Contains("sap_user")).FirstOrDefault();
            if (cf != null && !string.IsNullOrEmpty(cf.config_value))
            {
                sap_user = cf.config_value;
                cf = null;
            }
            cf = cfgs.Where(f => f.config_name.Contains("sap_pass")).FirstOrDefault();
            if (cf != null && !string.IsNullOrEmpty(cf.config_value))
            {
                sap_pass = cf.config_value;
            }
            if (string.IsNullOrEmpty(sap_pass) || string.IsNullOrEmpty(sap_user))
            {
                sap_user = "SAP_RFC_PPO";
                sap_pass = "123@pPo";
            }
            var promise = ec.SI_Customer_Statement_NAVIS_CGSA(usero.ruc, sap_user, sap_pass);
            popup = true;
            long zr = 0;
            if (promise == null)
            {
                zr = csl_log.log_csl.save_log<Exception>(new ApplicationException("Error de WEBSERVICE, OBJETO NULO"), "login", "SAP", usero.ruc, usero.loginname);
                popup = false;
                this.PersonalResponse(
                    string.Format(@"Estimado Cliente<br/>No se pudo 
                    mostrar información sobre saldos pendientes asociados a su cuenta, es posible que no 
                    tenga código asociado en nuestros sistemas financieros, para mas detalles 
                    comuníquese con nosotros y facilitenos el siguiente TICKET DE SERVICIO: EG-0{0} <br/>En unos segundos su navegador 
                    será redireccionado a la página del menú<br/>Gracias por entender.", zr), baseUrl + "/csl/menu", true);
                return;
            }
            var error = promise.Descendants("ERROR").FirstOrDefault();
            if (error != null)
            {
                zr = csl_log.log_csl.save_log<Exception>(new ApplicationException(error.Value), "login", "SAP", usero.ruc, usero.loginname);
                popup = false;
                this.PersonalResponse(
            string.Format(@"Estimado Cliente<br/>No se pudo 
                    mostrar información sobre saldos pendientes asociados a su cuenta, es posible que no 
                    tenga código asociado en nuestros sistemas financieros, para mas detalles 
                    comuníquese con nosotros y facilitenos el siguiente TICKET DE SERVICIO: CS-0{0} <br/>En unos segundos su navegador 
                    será redireccionado a la página del menú<br/>Gracias por entender.", zr), baseUrl + "/csl/menu", true);
                return;
            }
            //ARMAR LA PANTALLA
            if (popup)
            {
                //obtener datos de cabecera
                var cab = promise.Descendants("CABECERA").FirstOrDefault();
                var det = promise.Descendants("DETALLE").FirstOrDefault();
                //PREPARA EL DETALLE PARA REPORTE Y PARA PANTALLA
                var tabla = DataTransformHelper.TableFromXML("DETALLE", det.ToString());
                if (tabla != null && tabla.Rows.Count > 0)
                {
                    Session["FacturasPendientes"] = tabla;
                }
                else
                {
                    popup = false;
                }
                decimal dec;
                XElement el = cab.Element("RUC");
                if (el != null)
                {
                    this.num_ruc.InnerText = el.Value;
                }
                el = cab.Element("RAZON_SOCIAL");
                if (el != null)
                {
                    this.razon_soc.InnerText = el.Value;
                }
                el = cab.Element("SALDO");
                NumberStyles style = NumberStyles.Number | NumberStyles.AllowDecimalPoint;
                CultureInfo enUS = new CultureInfo("en-US");
                if (el != null)
                {
                    if (decimal.TryParse(el.Value, style, enUS, out dec))
                    {
                        this.saldo.InnerText = string.Format("{0:C}", dec);
                    }
                    else
                    {
                        this.saldo.InnerText = string.Format("${0}", el.Value);
                    }
                }
                el = cab.Element("PLAZO_FACTURA");
                if (el != null)
                {
                    this.plazo.InnerText = string.Format("{0} días", el.Value);
                }
                var fv = cab.Element("FACTURAS_VENCIDAS");
                var fp = cab.Element("FACTURAS_PENDIENTES");
                //elementos tendran 0
                dec = 0;
                if (fv != null && !string.IsNullOrEmpty(fv.Value))
                {
                    if (decimal.TryParse(fv.Value, out dec))
                    {
                        fac_ven.InnerText = string.Format("{0:c}", dec);
                    }
                    else
                    {
                        fac_ven.InnerText = "$0:EE";
                    }

                }
                dec = 0;
                if (fp != null && !string.IsNullOrEmpty(fp.Value))
                {
                   

                    if (decimal.TryParse(fp.Value, out dec))
                    {
                        fac_pend.InnerText = string.Format("{0:c}", dec);
                    }
                    else
                    {
                        fac_pend.InnerText = "$0:EE";
                    }

                }
                tablePagination.DataSource = tabla;
                tablePagination.DataBind();
            }
        }
    }
}