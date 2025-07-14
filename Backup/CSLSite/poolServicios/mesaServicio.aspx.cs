using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Data;

namespace CSLSite
{
    public partial class mesaServicio : System.Web.UI.Page
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
                    dpestados.SelectedValue = "IN";
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
                cargarServicios(null, null, null, null, null, dpestados.SelectedValue.ToString(), false, sUser.loginname);
                cargarGrupos();
            }
        }
        private void cargarGrupos()
        {
            List<Grupo> grupos = new List<Grupo>();            
            grupos = CslHelperServicios.consultarGrupo("", "A");
            dptipousuario.Items.Clear();
            dptipousuario.Items.Add(new ListItem("* Seleccione el tipo usuario *", "0"));
            dptipousuario.DataSource = grupos.OrderBy(x => x.descripcion);
            dptipousuario.DataValueField = "codigo";
            dptipousuario.DataTextField = "descripcion";
            dptipousuario.SelectedValue = "0";
            dptipousuario.DataBind();
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

                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    string numSolicitud = String.IsNullOrEmpty(txtNumSolicitud.Text.Trim()) ? "0" : txtNumSolicitud.Text.Trim();
                    //int idContenedor = String.IsNullOrEmpty(idContenedorU.Value.Trim()) ? 0 : int.Parse(idContenedorU.Value.Trim());
                    string fDesde = String.IsNullOrEmpty(desded.Text.Trim()) ? null : desded.Text.Trim();
                    string fHasta = String.IsNullOrEmpty(hastad.Text.Trim()) ? null : hastad.Text.Trim();
                    bool cTodos = chkTodos.Checked;

                    cargarServicios(numSolicitud, dptiposervicios.SelectedValue.ToString(), fDesde, fHasta, dptipousuario.SelectedValue.ToString(), dpestados.SelectedValue.ToString(), cTodos, sUser.loginname);
                                     
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
        public void cargarServicios(string numSolicitud, string tipoServicios, string fDesde, string fHasta, string tipoUsuario, string estados, bool cTodos, string userName) 
        {
           
            //nuevo cargar reporte dinamico.
            Session["Analista_REP"] = null;
            var par = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(numSolicitud))
            {
                par.Add("sol_num", numSolicitud);
            }
            if (!string.IsNullOrEmpty(estados))
            {
                par.Add("estado", estados);
            }
            if (!string.IsNullOrEmpty(tipoServicios))
            {
                par.Add("ser_code", tipoServicios);
            }
           
            //conversion de las fechas//
            DateTime desde;
            DateTime hasta;

            CultureInfo enUS = new CultureInfo("en-US");
            if (!string.IsNullOrEmpty(fDesde))
            {
                if (DateTime.TryParseExact(fDesde.Replace("-", "/"), "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                {
                    par.Add("desde", desde.ToString("yyyy-MM-dd"));
                }
            }
            if (!string.IsNullOrEmpty(fHasta))
            {

                if (DateTime.TryParseExact(fHasta.Replace("-", "/"), "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                {
                    par.Add("hasta", hasta.ToString("yyyy-MM-dd"));
                }
            }
            Session["Analista_REP"] = CSLSite.app_start.oFile.DinamycReport("portalSca", "pc_reporte_analista", true, par);

            int numSecuencial = 1;
            List<consultaCabeceraAnalista> table =
                CslHelperServicios.consultaSolicitudAnalista(numSolicitud, tipoServicios, fDesde, fHasta, tipoUsuario, estados, cTodos, userName, 0, "");//.catalogosDataTable();
            
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {new DataColumn("No", typeof(string)), 
                                new DataColumn("FechadeCreacion", typeof(string)), 
                                new DataColumn("CodigoSolicitud", typeof(string)),
                                new DataColumn("Servicio", typeof(string)),
                                new DataColumn("NoCarga", typeof(string)),
                                new DataColumn("Booking", typeof(string)),
                                new DataColumn("EstadodeServicio", typeof(string)),
                                new DataColumn("TipodeTrafico", typeof(string)),
                                new DataColumn("UsuarioCreacion", typeof(string)),
                                 new DataColumn("Exportador", typeof(string))
            });

            if (table.Count > 0)
            {
                foreach (consultaCabeceraAnalista item in table)
                {
                    
                    dt.Rows.Add(numSecuencial.ToString(), item.fechaSolicitud, item.numSolicitud, item.servicio, item.noCarga, 
                        item.noBooking, item.estado, item.trafico, item.usuario,item.exportador);
                    numSecuencial += 1;
                }
            }

            Session["tablaAnalistas"] = dt;

            var u = this.getUserBySesion();

            if (Response.IsClientConnected)
            {
                if (table.Count > 0)
                {
                    this.tbPaginationGeneral.DataSource = table;
                    this.tbPaginationGeneral.DataBind();
                    xfinder.Visible = true;
                    sinresultado.Visible = false;
                    alerta.Visible = false;
                    return;
                }
                else
                {
                    xfinder.Visible = false;
                    sinresultado.Visible = false;
                    alerta.Visible = true;
                    tbPaginationGeneral.DataSource = null;
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
                }
            }  
        }
    }
}