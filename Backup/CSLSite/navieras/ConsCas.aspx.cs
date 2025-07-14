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

namespace CSLSite
{
    public partial class cons_cas : System.Web.UI.Page
    {
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
               sinresultado.Visible = false;
            }
           
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }
                    sinresultado.Visible = false;
                    //alerta.Visible = false;
                    //xfinder.Visible = true;
                    if (string.IsNullOrEmpty(this.contain.Text) &&
                        string.IsNullOrEmpty(this.bl.Text))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;Ingrese al menos un parametro de busqueda.{0}</strong>", this.contain.Text);
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = true;
                        return;
                    }
                    //ejecutar ambos query

                    tablePagination.DataSource = null;
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los filtros para el reporte.";
                    var tablix = repNavieras.GetCasDate(this.contain.Text, this.bl.Text);
                    tablePagination.DataSource = tablix;
                    tablePagination.DataBind();
                    Session["reportCas"] = tablix;
                    xfinder.Visible = true;
                    //Session["DetalleReserva"] = chkDetalle.Checked;
                    //if (string.IsNullOrEmpty(tbooking.Text))
                    //{
                    //    tbooking.Text = "0";
                    //}
                    //var sid = QuerySegura.EncryptQueryString(string.Format("{0};{1};{2};{3};{4}", this.tbooking.Text, this.agencia.Value, this.contain.Text, this.tfechaini.Text, this.tfechafin.Text));
                    //this.aprint.HRef = string.Format("RptGateIn.aspx?sid={0}", sid);
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
    }
}