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

namespace CSLSite.exportables
{
    public partial class listbooking : System.Web.UI.Page
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
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "trucktime", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

            this.IsTokenAlive();
            this.IsAllowAccess();
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

                   

                }
                    

            }
            catch (Exception)
            {

                
            }

            // this.corte.InnerHtml =string.Format("Corte a la fecha:<i> <strong>{0}</strong></i>",DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            var user = this.getUserBySesion();
            if (user.loginname == null || user.loginname.Trim().Length <= 0)
             {
                 this.sinresultado.Attributes["class"] = string.Empty;
                 this.sinresultado.Attributes["class"] = "alert alert-warning";
                 this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El usuario era null!"), "inventario.llenos", "btbuscar_Click", "Error", user.loginname));
                 return;
             }
              var token = HttpContext.Current.Request.Cookies["token"];
             //Validacion 3 -> Si su token existe
             if (token == null)
             {
                 var id = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "llenos", "btbuscar_Click", token.Value, user.loginname);
                 var personalms = string.Format("Su formulario ha expirado, será redireccionado a la página de menú, su id={0} expiró..", id);
                 this.PersonalResponse(personalms, "../cuenta/menu.aspx", true);
                 return;
             }

            if (string.IsNullOrEmpty(this.tbooking.Text))
            {

                this.tbooking.Focus();
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "alert alert-warning";
                this.sinresultado.InnerText = string.Format("Por favor seleccione el booking a consultar");
                return;

            }


            CatalogosTableAdapters.Booking_informeTableAdapter ta = new CatalogosTableAdapters.Booking_informeTableAdapter();
            Catalogos.Booking_informeDataTable  tb = new Catalogos.Booking_informeDataTable();
#if DEBUG
            user.codigoempresa = "MSK";
            //  System.Threading.Thread.Sleep(7000);
#endif

            try
            {
                ta.Fill(tb, this.tbooking.Text, user.codigoempresa);

                if (tb.Rows.Count > 0)
                {
                    string fname = string.Format("trucktime{0}", DateTime.Now.ToString("ddMMyyyHHmmss"));
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "alert alert-warning";
                    Session["listadobooking"] = tb;
                    string llamada = string.Format("'descarga(\"{0}\",\"{1}\",\"{2}\");'", fname, "listadobooking", "listadobooking");
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