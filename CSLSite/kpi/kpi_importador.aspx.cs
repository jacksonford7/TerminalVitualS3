using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using N4Ws.Entidad;
using BillionEntidades;
using N4;
using CSLSite;
using System.Web.Script.Services;
using System.Web.Services;

using System.Globalization;
using System.Drawing;

namespace CSLSite
{
    public partial class kpi_importador : System.Web.UI.Page
    {
        usuario ClsUsuario;
        private string cMensaje = string.Empty;
        //private DateTime fechadesde = new DateTime();
        //private DateTime fechahasta = new DateTime();

        public string fdesde
        {
            get { return (string)Session["fdesde"]; }
            set { Session["fdesde"] = value; }
        }

        public string fhasta
        {
            get { return (string)Session["fhasta"]; }
            set { Session["fhasta"] = value; }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string GraficoData(int id, string ruc, DateTime desde, DateTime hasta)
        {
            return services.DataChart.GetJSONData(id, ruc, desde, hasta);
        }

        private void OcultarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
        }
        private void Actualiza_Paneles()
        {
         
            //UPCARGA.Update();

        }
        private void Mostrar_Mensaje(string Mensaje)
        {


            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            OcultarLoading();


            this.Actualiza_Paneles();
        }
        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading();

        }

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

            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }

            this.banmsg.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;

            }

      

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    this.sdesde.Text = Server.HtmlEncode(this.sdesde.Text);
                    this.shasta.Text = Server.HtmlEncode(this.shasta.Text);

                    string IdAgente = string.Empty;
                    ClsUsuario = Page.Tracker();
                    if (ClsUsuario != null)
                    {
                       //this.sruc.Value = "0990005087001";
                       this.sruc.Value = ClsUsuario.ruc;

                        //informacion del cliente
                        List<Cls_STC_LeerRuc> LeerInformacion = Cls_STC_LeerRuc.Info_Cliente(ClsUsuario.ruc, out cMensaje);
                        if (!String.IsNullOrEmpty(cMensaje))
                        {
                            this.banmsg.InnerHtml = string.Format("<i class='fa fa-warning'></i><b> Informativo! Error en </br> {0} ", cMensaje);
                            return;

                        }
                        foreach (var Det in LeerInformacion)
                        {
                            this.opcion_principal.InnerText = Det.CLNT_NAME.ToUpper();

                        }

                    }

                    DateTime Fecha_Inicial = DateTime.Today.AddDays(-365);

                   /* string desde = "01/" + DateTime.Today.Month.ToString("D2") + "/" + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha desde"));
                        return;
                    }
                    
                    this.sdesde.Text = fdesde.ToString("dd/MM/yyyy");*/

                    this.sdesde.Text = Fecha_Inicial.ToString("dd/MM/yyyy");

                    this.shasta.Text = DateTime.Today.ToString("dd/MM/yyyy");



                }





            }
            catch (Exception ex)
            {

             }
        }
        //protected void BtnBuscar_Click(object sender, EventArgs e)
        //{
        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {

        //            if (HttpContext.Current.Request.Cookies["token"] == null)
        //            {
        //                System.Web.Security.FormsAuthentication.SignOut();
        //                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //                Session.Clear();
        //                OcultarLoading();
        //                return;
        //            }

        //            if (string.IsNullOrEmpty(this.sdesde.Text))
        //            {
        //                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha inicial"));
        //                this.sdesde.Focus();
        //                return;
        //            }
        //            if (string.IsNullOrEmpty(this.shasta.Text))
        //            {
        //                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha final"));
        //                this.shasta.Focus();
        //                return;
        //            }

        //            CultureInfo enUS = new CultureInfo("en-US");

        //            if (!string.IsNullOrEmpty(sdesde.Text))
        //            {
        //                if (!DateTime.TryParseExact(sdesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechadesde))
        //                {
        //                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha inicial debe ser: Mes/dia/Anio {0}", sdesde.Text));
        //                    this.sdesde.Focus();
        //                    return;
        //                }
        //            }
        //            if (!string.IsNullOrEmpty(shasta.Text))
        //            {
        //                if (!DateTime.TryParseExact(shasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
        //                {
        //                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha final debe ser: Mes/dia/Anio {0}", shasta.Text));
        //                    this.shasta.Focus();
        //                    return;

        //                }
        //            }

        //            TimeSpan tsDias = fechahasta - fechadesde;
        //            int diferenciaEnDias = tsDias.Days;
        //            if (diferenciaEnDias < 0)
        //            {
        //                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>La Fecha de Ingreso: {0} NO deber ser mayor a la Fecha final: {1}", sdesde.Text, shasta.Text));
        //                return;
        //            }
        //            if (diferenciaEnDias > 365)
        //            {
        //                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar las facturas de hasta un periodo o año."));
        //                return;
        //            }
        //            if (fechadesde.Year != fechahasta.Year)
        //            {
        //                this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar las facturas de hasta un periodo o año."));
        //                return;
        //            }

        //            this.fdesde= this.sdesde.Text;
        //           this.fhasta = this.shasta.Text;

        //            this.Actualiza_Paneles();
        //            this.Ocultar_Mensaje();

                   
        //            Page.Response.Redirect(Page.Request.Url.ToString(), false);
        //            Context.ApplicationInstance.CompleteRequest();

        //        }
        //        catch (Exception ex)
        //        {
        //            this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));

        //        }
        //    }



        //}
        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

       

    }
}