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
using System.Xml.Linq;

namespace CSLSite.autorizaciones
{
    public partial class listado_veh_chof_autorizados : System.Web.UI.Page
    {
        public static string v_mensaje = string.Empty;
      
       
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


        private void Carga_Empresas(string LINEA_NAVIERA)
        {
            try
            {
                DataSet dsRetorno = new DataSet();

                List<MantenimientoEmpresa> Listado = MantenimientoEmpresa.ComboBox_Empresas(LINEA_NAVIERA, out v_mensaje);

                this.CboEmpresas.DataSource = Listado;
                this.CboEmpresas.DataTextField = "RAZON_SOCIAL";
                this.CboEmpresas.DataValueField = "ID_EMPRESA";
                this.CboEmpresas.DataBind();
    

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.InnerText = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_Empresas", "Hubo un error al cargar empresas", t.loginname));
                sinresultado.Visible = true;

            }

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
            this.IsAllowAccess();
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

            this.TxtLineaNaviera.Text = Server.HtmlEncode(this.TxtLineaNaviera.Text);

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
                        var ruc_cgsa = System.Configuration.ConfigurationManager.AppSettings["ruc_cgsa"];
                        if (user.ruc.Trim().Equals(ruc_cgsa.Trim()))
                        {
                            this.TxtLineaNaviera.Text = string.Empty;
                            this.buscar_linea.Visible = true;
                            this.AuxLinea_Naviera.Value = string.Empty;
                            rucempresa = string.Empty;
                        }
                        else
                        {
                            this.TxtLineaNaviera.Text = user.ruc;
                            this.buscar_linea.Visible = false;
                            this.AuxLinea_Naviera.Value = user.ruc;
                            rucempresa = user.ruc;
                        }

                        this.Carga_Empresas(this.TxtLineaNaviera.Text);
                        this.CboTipo.Items.Clear();
                        this.CboTipo.Items.Add("1-Vehículos");
                        this.CboTipo.Items.Add("2-Choferes");

                        this.CboFiltro.Items.Clear();
                        this.CboFiltro.Items.Add("1-Autorizados");
                        this.CboFiltro.Items.Add("2-No Autorizados");
                        this.CboFiltro.Items.Add("3-Todos");

                        this.banmsg_det.Visible = false;
                    }
                    else
                    {
                        this.TxtLineaNaviera.Text = string.Empty;
                        this.buscar_linea.Visible = false;
                        this.AuxLinea_Naviera.Value = string.Empty;
                        rucempresa = string.Empty;
                    }


                    sinresultado.Visible = false;

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Page_Load", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
            else
            {
                this.TxtLineaNaviera.Text = this.AuxLinea_Naviera.Value;
                if (rucempresa != this.AuxLinea_Naviera.Value)
                {
                    this.Carga_Empresas(this.TxtLineaNaviera.Text);
                    this.vehiculo.Visible = false;
                    this.choferes.Visible = false;
                }
                rucempresa = this.AuxLinea_Naviera.Value;
            }

        }

        #endregion

        #region "Eventos Controles Vehiculo"

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        OcultarLoading();
                    }
                    alerta.Visible = true;

                   
                    sinresultado.Visible = false;

                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";

                    if (string.IsNullOrEmpty(this.TxtLineaNaviera.Text))
                    {
                        OcultarLoading();
                        this.Alerta("Debe seleccionar la líneas naviera");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtLineaNaviera.Focus();
                        return;
                    }

                    var user = Page.getUserBySesion();

                    string LINEA_NAVIERA = this.TxtLineaNaviera.Text.Trim();
                    string ID_EMPRESA = this.CboEmpresas.SelectedValue;
                    Int32 TIPO = this.CboTipo.SelectedIndex + 1;
                    Int32 AUTORIZADOS = this.CboFiltro.SelectedIndex + 1;

                    XDocument XMLEmpresas=null;
                    XDocument XMLVehiculo=null;
                    XDocument XMLChofer=null;
                    XDocument XMLMensaje;

                    //listado empresas autorizadas
                    List<MantenimientoEmpresa> ListadoEmp = MantenimientoEmpresa.ComboBox_Empresas(LINEA_NAVIERA, out v_mensaje);
                    //listado de choferes
                    List<MantenimientoVehiculo> ListadoVehi = MantenimientoVehiculo.Listado_Vehiculos(string.Empty, LINEA_NAVIERA, out v_mensaje);
                    //listado de choferes
                    List<MantenimientoChoferes> ListadoChof = MantenimientoChoferes.Listado_Choferes(string.Empty, LINEA_NAVIERA, out v_mensaje);
                    //lista mensajes
                    List<Mensajes> ListMsg = Mensajes.Listar_Mensajes(out v_mensaje);


                    //mensajes
                    XMLMensaje = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                       new XElement("MENSAJES", from p in ListMsg.AsEnumerable().AsParallel()
                                                                select new XElement("MENSAJE",
                                                        new XAttribute("ID_MENSAJE", p.ID_MENSAJE == 0 ? "0" : p.ID_MENSAJE.ToString().Trim()),
                                                         new XAttribute("TIPO", p.TIPO == 0 ? "0" : p.TIPO.ToString().Trim()),
                                                          new XAttribute("CAMPO", p.CAMPO == null ? "" : p.CAMPO.Trim()),
                                                          new XAttribute("MENSAJE", p.MENSAJE == null ? "" : p.MENSAJE.Trim())
                                                        )));



                    //empresas
                    if (ID_EMPRESA.Equals("Todos"))
                    {

                        XMLEmpresas = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                        new XElement("EMPRESAS", from p in ListadoEmp.Where(a => !a.Equals("Todos")).AsEnumerable().AsParallel()
                                                                 select new XElement("EMPRESA",
                                                         new XAttribute("ID_EMPRESA", p.ID_EMPRESA == null ? "" : p.ID_EMPRESA),
                                                          new XAttribute("RAZON_SOCIAL", p.RAZON_SOCIAL == null ? "" : p.RAZON_SOCIAL)
                                                         )));

                    }
                    else
                    {
                        XMLEmpresas = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                        new XElement("EMPRESAS", from p in ListadoEmp.Where(a => a.ID_EMPRESA.Trim() == ID_EMPRESA.Trim()).AsEnumerable().AsParallel()
                                                                 select new XElement("EMPRESA",
                                                         new XAttribute("ID_EMPRESA", p.ID_EMPRESA == null ? "" : p.ID_EMPRESA),
                                                          new XAttribute("RAZON_SOCIAL", p.RAZON_SOCIAL == null ? "" : p.RAZON_SOCIAL)
                                                         )));
                    }

                    //1 = vehiculos
                    if (TIPO == 1)
                    {
                        if (ID_EMPRESA.Equals("Todos"))
                        {
                            XMLVehiculo = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                       new XElement("VEHICULOS", from p in ListadoVehi.AsEnumerable().AsParallel()
                                                                 select new XElement("VEHICULO",
                                                                  new XAttribute("ID_VEHICULO", p.ID_VEHICULO == null ? "" : p.ID_VEHICULO),
                                                                  new XAttribute("PLACA", p.PLACA == null ? "" : p.PLACA),
                                                                  new XAttribute("ID_EMPRESA", p.ID_EMPRESA == null ? "" : p.ID_EMPRESA)
                                                         )));
                        }
                        else
                        {
                            XMLVehiculo = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                       new XElement("VEHICULOS", from p in ListadoVehi.Where(a => a.ID_EMPRESA.Trim() == ID_EMPRESA.Trim()).AsEnumerable().AsParallel()
                                                                 select new XElement("VEHICULO",
                                                                  new XAttribute("ID_VEHICULO", p.ID_VEHICULO == null ? "" : p.ID_VEHICULO),
                                                                  new XAttribute("PLACA", p.PLACA == null ? "" : p.PLACA),
                                                                  new XAttribute("ID_EMPRESA", p.ID_EMPRESA == null ? "" : p.ID_EMPRESA)
                                                         )));
                        }
                            
                    }
                    else//choferes
                    {
                        if (ID_EMPRESA.Equals("Todos"))
                        {
                            XMLChofer = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                       new XElement("CHOFERES", from p in ListadoChof.AsEnumerable().AsParallel()
                                                                 select new XElement("CHOFER",
                                                                  new XAttribute("ID_CHOFER", p.ID_CHOFER == 0 ? "0" : p.ID_CHOFER.ToString().Trim()),
                                                                  new XAttribute("LICENCIA", p.LICENCIA == null ? "" : p.LICENCIA),
                                                                  new XAttribute("ID_EMPRESA", p.ID_EMPRESA == null ? "" : p.ID_EMPRESA)
                                                         )));
                        }
                        else
                        {
                            XMLChofer = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                                       new XElement("CHOFERES", from p in ListadoChof.Where(a => a.ID_EMPRESA.Trim() == ID_EMPRESA.Trim()).AsEnumerable().AsParallel()
                                                                 select new XElement("CHOFER",
                                                                  new XAttribute("ID_CHOFER", p.ID_CHOFER == 0 ? "0" : p.ID_CHOFER.ToString().Trim()),
                                                                  new XAttribute("LICENCIA", p.LICENCIA == null ? "" : p.LICENCIA),
                                                                  new XAttribute("ID_EMPRESA", p.ID_EMPRESA == null ? "" : p.ID_EMPRESA)
                                                         )));
                        }
                    }

                    string pXMLEmpresas = (XMLEmpresas == null ? "<EMPRESAS><EMPRESA ID_EMPRESA='0' RAZON_SOCIAL='.' /></EMPRESAS>" : XMLEmpresas.ToString().Trim());
                    string pXMLVehiculo = (XMLVehiculo == null ? "'<VEHICULOS><VEHICULO ID_VEHICULO='0' PLACA='0' ID_EMPRESA='0' /></VEHICULOS>" : XMLVehiculo.ToString().Trim());
                    string pXMLChofer = (XMLChofer == null ? "<CHOFERES><CHOFER ID_CHOFER='0' LICENCIA='0' ID_EMPRESA='0' /></CHOFERES>" : XMLChofer.ToString().Trim());
                    string pXMLMensaje = (XMLMensaje == null ? "<MENSAJES><MENSAJE ID_MENSAJE='0' TIPO='1' CAMPO='TAGID' MENSAJE='.'/></MENSAJES>" : XMLMensaje.ToString().Trim());

                    Session["XMLEmpresas"] = pXMLEmpresas;
                    Session["XMLVehiculo"] = pXMLVehiculo; 
                    Session["XMLChofer"] = pXMLChofer;
                    Session["XMLMensaje"] = pXMLMensaje;

                    Session["TIPO"] = TIPO.ToString();
                    Session["AUTORIZADOS"] = AUTORIZADOS.ToString();

                    string cUsuario = string.Format("Usuario: {0} {1} ", user.nombres, user.apellidos);
                    ReportParameter ReportParameter_filtro_ = new ReportParameter("ReportParameter_filtro", cUsuario);

                    if (this.CboTipo.SelectedIndex == 0)
                    {
                        this.vehiculo.Visible = true;
                        this.choferes.Visible = false;
                        ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });
                        ReportViewer1.SizeToReportContent = true;
                        ReportViewer1.ShowToolBar = true;
                        ReportViewer1.ShowPrintButton = true;
                        ReportViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        this.vehiculo.Visible = false;
                        this.choferes.Visible = true;
                        ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { ReportParameter_filtro_ });
                        ReportViewer2.SizeToReportContent = true;
                        ReportViewer2.ShowToolBar = true;
                        ReportViewer2.ShowPrintButton = true;
                        ReportViewer2.LocalReport.Refresh();
                    }


                    this.banmsg_det.Visible = true;
                    Session["usuarioemail"] = useremail;
                    Session["rucempresa"] = rucempresa;
                    OcultarLoading();
                   
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "BtnBuscar_Click", "Hubo un error al buscar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                    OcultarLoading();
                }
            }
        }
   
        
        #endregion

      


      


        }
}