using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;

namespace CSLSite
{
    public partial class consultaSolicitudUsuario : System.Web.UI.Page
    {
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
                //this.agencia.Value = user.ruc;
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
                populateDrop(dpestados, CslHelperServicios.getDetalleCatalogo(ConfigurationManager.AppSettings["TipoEstado"]));
                if (dpestados.Items.Count > 0)
                {
                    if (dpestados.Items.FindByValue("000") != null)
                    {
                        dpestados.Items.FindByValue("000").Selected = true;
                    }
                    dpestados.SelectedValue = "0";
                }
                populateDrop(dptiposervicios, CslHelperServicios.getServicios());
                if (dptiposervicios.Items.Count > 0)
                {
                    if (dptiposervicios.Items.FindByValue("000") != null)
                    {
                        dptiposervicios.Items.FindByValue("000").Selected = true;
                    }
                    dptiposervicios.SelectedValue = "0";
                }

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                usuariologin.Value = sUser.loginname.Trim();
                desded.Text = DateTime.Now.ToString("dd/MM/yyyy");
                hastad.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
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

                     string numSolicitud = String.IsNullOrEmpty(txtNumSolicitud.Text.Trim()) ? "0" : txtNumSolicitud.Text.Trim();
                    int idContenedor = 0;
                    if (!string.IsNullOrEmpty(contenedor1.Text.Trim())) {
                        idContenedor = String.IsNullOrEmpty(idContenedorU.Value.Trim()) ? 0 : int.Parse(idContenedorU.Value.Trim());
                    }
                     
                    


                    DateTime desde;
                    DateTime hasta;
                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    if (!DateTime.TryParseExact(desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde (formato incorrecto dd/MM/yyyy)";
                        sinresultado.Visible = true;
                        return;
                    }

                    if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta (formato incorrecto dd/MM/yyyy)";
                        sinresultado.Visible = true;
                        return;
                    }


                    string  fDesde = desde.ToString("yyyy-MM-dd");
                    string fHasta= hasta.ToString("yyyy-MM-dd");



                    bool cTodos = false;

                    long nume = 0;

                    if (!long.TryParse(numSolicitud, out nume))
                    {
                        var nvs = numSolicitud.Split('-');
                        var el = nvs.LastOrDefault();
                        if (el != null)
                        {
                            if (long.TryParse(el, out nume))
                            {
                                numSolicitud = nume.ToString();
                            }
                        }
                    }
                    else
                    {
                        numSolicitud = nume.ToString();
                    }

                    List<consultaCabeceraUsuario> table = CslHelperServicios.consultaSolicitudUsuario(null, idContenedor, dptiposervicios.SelectedValue.ToString(), fDesde, fHasta, dpestados.SelectedValue.ToString(), cTodos, usuariologin.Value.Trim(), numSolicitud);//.catalogosDataTable();
                    
                    var u = this.getUserBySesion();
                    
                    if (Response.IsClientConnected)
                    {   
                        if (table.Count > 0)
                        {
                            this.tablePagination.DataSource = table;
                            this.tablePagination.DataBind();
                            xfinder.Visible = true;
                            sinresultado.Visible = false;
                            alerta.Visible = false;
                            return;
                        }
                        else {
                            xfinder.Visible = false;
                            sinresultado.Visible = false;
                            alerta.Visible = true;
                            tablePagination.DataSource = null;
                            this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
                        }                        
                    }
                    
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

        //public void btconfirmar_Click(object sender, EventArgs e)
        //{

        //}
    }
}