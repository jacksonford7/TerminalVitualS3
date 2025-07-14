using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Web.Services;
using csl_log;
using Microsoft.Reporting.WebForms;
using ClsAutorizaciones;


using System.Text;
using System.Xml.Linq;
using System.Xml;



namespace CSLSite.autorizaciones
{
    public partial class listado_orden_retiro_lookup : System.Web.UI.Page
    {
        public static string v_mensaje = string.Empty;
        private string id = string.Empty;

        public string rucempresa
        {
            get { return (string)Session["rucempresa"]; }
            set { Session["rucempresa"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["usuarioemail"]; }
            set { Session["usuarioemail"] = value; }
        }

     


        #region "Metodos"

        private void OcultarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
        }


      
      

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        #region "Eventos Formulario"

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

            id = QuerySegura.DecryptQueryString(Request.QueryString["id"]);
            if (Request.QueryString["id"] == null || string.IsNullOrEmpty(id))
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, id de orden de retirono valido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                var number = log_csl.save_log<Exception>(ex, "listado_orden_retiro_lookup", "Page_Init", id, Request.UserHostAddress);
                this.AbortResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
            }

            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "autorizaciones", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
               // rucempresa = user.ruc;
                useremail = user.email;
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
                    var user = Page.Tracker();
                    if (user != null && !string.IsNullOrEmpty(user.ruc))
                    {
                        //this.LblLineaNaviera.Text = rucempresa;

                        /*Recuperar toda la referencia*/
                        Int64 pid = 0;

                        id = id.Trim().Replace("\0", string.Empty);

                        if (!Int64.TryParse(id, out pid))
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, id no es numerico", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "listado_orden_retiro_lookup", "Page_Load", id == null ? "id no es numerico" : id, User.Identity.Name);
                            this.AbortResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                            return;
                        }

                        string XML = ReporteVacio.Xml_Contenedores(pid, out v_mensaje);

                        List<ReporteVacio> Listado_Unidades = ReporteVacio.Estados_Contenedores(XML , out v_mensaje);
                        if (Listado_Unidades != null)
                        {
                            XDocument XMLContenedores = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                               new XElement("CONTENEDORES", from p in Listado_Unidades.AsEnumerable().AsParallel()
                                                            select new XElement("CONTENEDOR",
                                                        new XAttribute("CNTR_CONSECUTIVO", p.CNTR_CONSECUTIVO == 0 ? "0" : p.CNTR_CONSECUTIVO.ToString()),
                                                        new XAttribute("CNTR_YARD_STATUS", p.CNTR_YARD_STATUS == null ? "" : p.CNTR_YARD_STATUS.ToString()),
                                                         new XAttribute("CNTR_VEPR_REFERENCE", p.CNTR_VEPR_REFERENCE == null ? "" : p.CNTR_VEPR_REFERENCE.ToString()),
                                                          new XAttribute("CNTR_CLNT_CUSTOMER_LINE", p.CNTR_CLNT_CUSTOMER_LINE == null ? "" : p.CNTR_CLNT_CUSTOMER_LINE.ToString()),
                                                           new XAttribute("NAVE", p.NAVE == null ? "" : p.NAVE.ToString()),
                                                            new XAttribute("CHOFER", p.CHOFER == null ? "" : p.CHOFER.ToString()),
                                                             new XAttribute("CAMION", p.CAMION == null ? "" : p.CAMION.ToString())
                                                       )));

                            string pXmlContenedor = (XMLContenedores == null ? "<CONTENEDORES><CONTENEDOR CNTR_CONSECUTIVO='0' CNTR_YARD_STATUS='IN' CNTR_VEPR_REFERENCE='E' CNTR_CLNT_CUSTOMER_LINE='MSK' NAVE='X' CHOFER='X' CAMION='X'/></CONTENEDORES>" : XMLContenedores.ToString().Trim());

                            Session["XmlContenedor"] = pXmlContenedor;
                            Session["ID"] = pid.ToString();
                            
                            string cUsuario = string.Format("Usuario: {0} {1} ", user.nombres, user.apellidos);
                            ReportParameter ReportParameter_filtro_ = new ReportParameter("ReportParameter_filtro", cUsuario);

                          
                            this.vehiculo.Visible = true;
                            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });
                            ReportViewer1.SizeToReportContent = true;
                            ReportViewer1.ShowToolBar = true;
                            ReportViewer1.ShowPrintButton = true;
                            ReportViewer1.LocalReport.Refresh();
                            

                        }

                    }
                    else
                    {
                        //this.LblLineaNaviera.Text = string.Empty;
                    }


                    sinresultado.Visible = false;

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Page_Load", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }

        #endregion

        #region "Eventos Controles Vehiculo"

   
        
        #endregion

      


      


        }
}