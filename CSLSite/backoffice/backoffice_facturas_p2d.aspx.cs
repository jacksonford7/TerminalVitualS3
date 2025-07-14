using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Text;
using System.IO;
using CSLSite.unitService;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Globalization;

namespace CSLSite
{
    public partial class backoffice_facturas_p2d : System.Web.UI.Page
    {
        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();

        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "p2d", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

           this.IsTokenAlive();
          // this.IsAllowAccess();
           Page.Tracker();

           if (!IsPostBack)
           {
               this.IsCompatibleBrowser();
               Page.SslOn();
           }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
          this.sinresultado.Visible = IsPostBack;
           

            try
            {

                if (!Page.IsPostBack)
                {

                    this.TxtFechaDesde.Text = Server.HtmlEncode(this.TxtFechaDesde.Text);
                    this.TxtFechaHasta.Text = Server.HtmlEncode(this.TxtFechaHasta.Text);

                    string desde = DateTime.Today.Month.ToString("D2") + "/01/" + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                       
                        return;
                    }

                    this.TxtFechaDesde.Text = fdesde.ToString("MM/dd/yyyy");
                    this.TxtFechaHasta.Text = DateTime.Today.ToString("MM/dd/yyyy");



                }
                    

            }
            catch (Exception)
            {

                
            }

           
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            var user = this.getUserBySesion();
            if (user.loginname == null || user.loginname.Trim().Length <= 0)
             {
                 this.sinresultado.Attributes["class"] = string.Empty;
                 this.sinresultado.Attributes["class"] = "alert alert-warning";
                 this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El usuario era null!"), "p2d", "btbuscar_Click", "Error", user.loginname));
                 return;
             }
              var token = HttpContext.Current.Request.Cookies["token"];
             //Validacion 3 -> Si su token existe
             if (token == null)
             {
                 var id = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "backoffice_facturas_p2d", "btbuscar_Click", token.Value, user.loginname);
                 var personalms = string.Format("Su formulario ha expirado, será redireccionado a la página de menú, su id={0} expiró..", id);
                 this.PersonalResponse(personalms, "../cuenta/menu.aspx", true);
                 return;
             }

            if (string.IsNullOrEmpty(this.TxtFechaDesde.Text))
            {

                this.TxtFechaDesde.Focus();
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "alert alert-warning";
                this.sinresultado.InnerText = string.Format("Por favor seleccione la fecha inicial");
                return;
             
            }

            if (string.IsNullOrEmpty(this.TxtFechaHasta.Text))
            {
                this.TxtFechaHasta.Focus();
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "alert alert-warning";
                this.sinresultado.InnerText = string.Format("Por favor seleccione la fecha final");
                return;
            
            }

            CultureInfo enUS = new CultureInfo("en-US");

            if (!string.IsNullOrEmpty(TxtFechaDesde.Text))
            {
                if (!DateTime.TryParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "alert alert-warning";
                    this.sinresultado.InnerText = string.Format("Informativo!     El formato de la fecha inicial debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text);

                  
                    this.TxtFechaDesde.Focus();
                    return;
                }
            }

            if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
            {
                if (!DateTime.TryParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "alert alert-warning";
                    this.sinresultado.InnerText = string.Format("Informativo!     El formato de la fecha final debe ser: dia/Mes/Anio {0}", TxtFechaHasta.Text);

                   
                    this.TxtFechaHasta.Focus();
                    return;
                }
            }

            TimeSpan tsDias = fechahasta - fechadesde;
            int diferenciaEnDias = tsDias.Days;
            if (diferenciaEnDias < 0)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "alert alert-warning";
                this.sinresultado.InnerText = string.Format("Informativo!    La Fecha de Ingreso: {0} No deber ser mayor a la Fecha final: {1}", TxtFechaDesde.Text, TxtFechaDesde.Text);

               
                return;
            }
            if (diferenciaEnDias > 31)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "alert alert-warning";

                this.sinresultado.InnerText = string.Format("Sólo puede consultar informacion de hasta un 1 mes.");
              
                return;
            }



            CatalogosTableAdapters.p2d_Listado_Factura_cfsTableAdapter  ta = new CatalogosTableAdapters.p2d_Listado_Factura_cfsTableAdapter();
            Catalogos.p2d_Listado_Factura_cfsDataTable   tb = new Catalogos.p2d_Listado_Factura_cfsDataTable();


            try
            {
                ta.Fill(tb, fechadesde, fechahasta);

                if (tb.Rows.Count > 0)
                {
                    string fname = string.Format("facturas_agente_port_to_door{0}", DateTime.Now.ToString("ddMMyyyHHmmss"));
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "alert alert-warning";
                    Session["facturas_agente_port_to_door"] = tb;
                    string llamada = string.Format("'descarga(\"{0}\",\"{1}\",\"{2}\");'", fname, "facturas_agente_port_to_door", "facturas_agente_port_to_door");
                    sinresultado.InnerHtml = string.Format("Se ha generado el archivo {0}.xlsx, con {1} filas<br/><a class='btn btn-link' href='#' onclick={2} >Clic Aquí para descargarlo</a>", fname, tb.Rows.Count, llamada);
                    sinresultado.Visible = true;
                }
                else
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = "Hubo un problema y no se encontraron registros que mostrar ";
                }
            }
            catch 
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = "Hubo un problema tratando de acceder a los datos ";
            }

 
        }

    }
}