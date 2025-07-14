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

namespace CSLSite.autorizaciones
{
    public partial class excluir_veh_chof : System.Web.UI.Page
    {
        public static string v_mensaje = string.Empty;
        private string EmpresaSelect = string.Empty;
        private MantenimientoEmpresa objEmpresa = new MantenimientoEmpresa();
        private Auditoria objAuditoria = new Auditoria();
        private MantenimientoVehiculo objVehiculo = new MantenimientoVehiculo();
        private MantenimientoChoferes objChofer = new MantenimientoChoferes();
        Vehiculos Vehiculo;
        Choferes Chofer;

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

        private DataTable DtVehiculos
        {
            get
            {
                return (DataTable)Session["DataGridVehiculo"];
            }
            set
            {
                Session["DataGridVehiculo"] = value;
            }

        }
        private DataTable DtChoferes
        {
            get
            {
                return (DataTable)Session["DataGridChofer"];
            }
            set
            {
                Session["DataGridChofer"] = value;
            }

        }

        #region "Metodos"
        private void Carga_Vehiculo(string Criterio, string LINEA_NAVIERA)
        {
            try
            {
                DataSet dsRetorno = new DataSet();

                List<MantenimientoVehiculo> Listado = MantenimientoVehiculo.Listado_Vehiculos(Criterio, LINEA_NAVIERA, out v_mensaje);
                if (Listado != null)
                {
                    var Tmp = (from Vehiculo in Listado.AsEnumerable()
                               select new
                               {
                                   ID = Vehiculo.ID,
                                   ID_VEHICULO = Vehiculo.ID_VEHICULO,
                                   PLACA = Vehiculo.PLACA,
                                   TAG = Vehiculo.TAG,
                                   STATUS = Vehiculo.STATUS,
                                   LICENCIA_EXPIRACION = (!Vehiculo.LICENCIA_EXPIRACION.HasValue ? null : Vehiculo.LICENCIA_EXPIRACION),
                                   CABEZAL_EXPIRACION = (!Vehiculo.CABEZAL_EXPIRACION.HasValue ? null : Vehiculo.CABEZAL_EXPIRACION),
                                   ID_EMPRESA = Vehiculo.ID_EMPRESA,
                                   RAZON_SOCIAL = Vehiculo.RAZON_SOCIAL,
                                   USUARIO_CRE = Vehiculo.USUARIO_CRE,
                                   FECHA_CREA = Vehiculo.FECHA_CREA,
                                   LINEA_NAVIERA = Vehiculo.LINEA_NAVIERA,
                                   MENSAJE = Vehiculo.MENSAJE,
                                   MOTIVO = Vehiculo.MOTIVO
                               }).ToList().OrderBy(p => p.RAZON_SOCIAL);


                    dsRetorno.Tables.Add(MantenimientoVehiculo.LINQToDataTable(Tmp));

                    DtVehiculos = dsRetorno.Tables[0];
                    tablePagination.DataSource = dsRetorno.Tables[0];
                    tablePagination.DataBind();


                }
                else
                {
                    DtVehiculos = null;
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();


                }

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_Vehiculo", "Hubo un error al cargar listado", t.loginname));
                sinresultado.Visible = true;

            }

        }

        private void Carga_Chofer(string Criterio, string LINEA_NAVIERA)
        {
            try
            {
                DataSet dsRetorno = new DataSet();

                List<MantenimientoChoferes> Listado = MantenimientoChoferes.Listado_Choferes(Criterio, LINEA_NAVIERA, out v_mensaje);
                if (Listado != null)
                {
                    var Tmp = (from Choferes in Listado.AsEnumerable()
                               select new
                               {
                                   ID = Choferes.ID,
                                   ID_CHOFER = Choferes.ID_CHOFER,
                                   LICENCIA = Choferes.LICENCIA,
                                   NOMBRES = Choferes.NOMBRES,
                                   STATUS = Choferes.STATUS,
                                   LICENCIA_EXPIRACION = (!Choferes.LICENCIA_EXPIRACION.HasValue ? null : Choferes.LICENCIA_EXPIRACION),
                                   CHOFER_SUSPENDIDO = (!Choferes.CHOFER_SUSPENDIDO.HasValue ? null : Choferes.CHOFER_SUSPENDIDO),
                                   ID_EMPRESA = Choferes.ID_EMPRESA,
                                   RAZON_SOCIAL = Choferes.RAZON_SOCIAL,
                                   USUARIO_CRE = Choferes.USUARIO_CRE,
                                   FECHA_CREA = Choferes.FECHA_CREA,
                                   LINEA_NAVIERA = Choferes.LINEA_NAVIERA,
                                   MENSAJE = Choferes.MENSAJE,
                                   MOTIVO = Choferes.MOTIVO
                               }).ToList().OrderBy(p => p.RAZON_SOCIAL);


                    dsRetorno.Tables.Add(MantenimientoVehiculo.LINQToDataTable(Tmp));

                    DtChoferes = dsRetorno.Tables[0];
                    tablePagination2.DataSource = dsRetorno.Tables[0];
                    tablePagination2.DataBind();


                }
                else
                {
                    DtChoferes = null;
                    tablePagination2.DataSource = null;
                    tablePagination2.DataBind();
                }

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_Chofer", "Hubo un error al cargar listado", t.loginname));
                sinresultado.Visible = true;

            }

        }

        private void Limpiar()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "clearTextBox();", true);
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
                //rucempresa = user.ruc;
                useremail = user.email;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }

            this.TxtLineaNaviera.Text = Server.HtmlEncode(this.TxtLineaNaviera.Text);
            this.Txtempresa.Text = Server.HtmlEncode(this.Txtempresa.Text);
            this.TxtVehiculo.Text = Server.HtmlEncode(this.TxtVehiculo.Text);
            this.TxtChofer.Text = Server.HtmlEncode(this.TxtChofer.Text);
            this.TxtBuscarVehiculo.Text = Server.HtmlEncode(this.TxtBuscarVehiculo.Text);
            this.TxtBuscarChofer.Text = Server.HtmlEncode(this.TxtBuscarChofer.Text);
            this.TxtMotivoChofer.Text = Server.HtmlEncode(this.TxtMotivoChofer.Text);
            this.TxtMotivoVehiculo.Text = Server.HtmlEncode(this.TxtMotivoVehiculo.Text);
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
                        this.Carga_Vehiculo(this.TxtBuscarVehiculo.Text, this.TxtLineaNaviera.Text);
                        this.Carga_Chofer(this.TxtBuscarVehiculo.Text, this.TxtLineaNaviera.Text);
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
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Page_Load", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
            else
            {
                this.TxtLineaNaviera.Text = this.AuxLinea_Naviera.Value;
                if (rucempresa != this.AuxLinea_Naviera.Value)
                {
                    this.Carga_Vehiculo(this.TxtBuscarVehiculo.Text, this.TxtLineaNaviera.Text);
                    this.Carga_Chofer(this.TxtBuscarVehiculo.Text, this.TxtLineaNaviera.Text);
                }
                rucempresa = this.AuxLinea_Naviera.Value;
            }
        }

        #endregion

        #region "Eventos Controles Vehiculo"

        protected void BtnAgregar_Click(object sender, EventArgs e)
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

                    string IdEmpresa = this.RucEmpresa.Value;
                    string IdVehiculo = this.IdVehiculo.Value;
                    string DesEmpresa = string.Empty;
                    string Placa = TxtVehiculo.Text;
                    string cLinea_Naviera = this.TxtLineaNaviera.Text.Trim();

                    if (string.IsNullOrEmpty(cLinea_Naviera))
                    {
                        this.Alerta("Debe seleccionar la líneas naviera");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtLineaNaviera.Focus();
                        return;
                    }


                    if (string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        this.Alerta("Debe seleccionar la Compañía de Transporte para poder agregar la información");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.Txtempresa.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(IdEmpresa))
                    {
                        this.Alerta("Debe volver a seleccionar la Compañía de Transporte para poder agregar la información");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.Txtempresa.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(Placa))
                    {
                        this.Alerta("Debe seleccionar el vehículo a excluir");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtVehiculo.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(IdVehiculo))
                    {
                        this.Alerta("Debe volver a seleccionar el vehículo a excluir");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtVehiculo.Focus();
                        return;
                    }

                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objVehiculo = new MantenimientoVehiculo();
                    objVehiculo.ID_EMPRESA = IdEmpresa;
                    objVehiculo.ID_VEHICULO = IdVehiculo;
                    objVehiculo.PLACA = Placa;
                    objVehiculo.LINEA_NAVIERA = cLinea_Naviera;

                    if (objVehiculo.Existe_Vehiculo(out v_mensaje))
                    {
                        this.Alerta(string.Format("El Vehículo: {0}, ya se encuentra registrado para la empresa de transporte: {1}", Placa, Txtempresa.Text));
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtVehiculo.Focus();
                        return;

                    }


                    Vehiculo = new Vehiculos();
                    Vehiculo.ID_EMPRESA = IdEmpresa;
                    Vehiculo.ID = IdVehiculo;
                    Vehiculo.PLACA = Placa;
                    if (!Vehiculo.PopulateMyData(out v_mensaje))
                    {
                        string Error = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se pudo cargar datos del vehiculo"), "Vehiculo", "BtnAgregar_Click", "Hubo un error al agregar vehiculo", sUser != null ? sUser.loginname : "Nologin"));
                        this.Alerta(Error);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtVehiculo.Focus();
                        return;
                    }

                    objVehiculo = new MantenimientoVehiculo();
                    objVehiculo.ID_VEHICULO = IdVehiculo;
                    objVehiculo.PLACA = Placa.Trim();
                    objVehiculo.TAG = Vehiculo.TAG;
                    objVehiculo.STATUS = Vehiculo.STATUS;

                    objVehiculo.LICENCIA_EXPIRACION = Vehiculo.LICENCIA_EXPIRACION;
                    objVehiculo.CABEZAL_EXPIRACION = Vehiculo.CABEZAL_EXPIRACION;
                    objVehiculo.ID_EMPRESA = IdEmpresa;
                    objVehiculo.USUARIO_CRE = sUser.loginname;
                    objVehiculo.ESTADO = true;
                    objVehiculo.LINEA_NAVIERA = cLinea_Naviera;
                    objVehiculo.MENSAJE = Vehiculo.MENSAJE;
                    objVehiculo.MOTIVO = this.TxtMotivoVehiculo.Text.Trim();

                    objVehiculo.Save(out v_mensaje);
                    if (v_mensaje != string.Empty)
                    {
                        this.Alerta(v_mensaje.ToString());

                    }
                    else
                    {
                        /************************************** auditoria *****************************************/
                        objAuditoria = new Auditoria();
                        objAuditoria.OPCION = "Autorizacion de Vehiculos y Choferes";
                        objAuditoria.ACCION = string.Format("Agrego Vehiculo {0} - Empresa {1}-{2}, linea naviera: {3}", Placa, IdEmpresa, Vehiculo.RAZON_SOCIAL, cLinea_Naviera);
                        objAuditoria.USUARIO = sUser.loginname;
                        objAuditoria.Save(out v_mensaje);
                        /************************************** auditoria *****************************************/

                        this.TxtVehiculo.Text = string.Empty;
                        this.IdVehiculo.Value = string.Empty;
                        this.Placa.Value = string.Empty;
                        this.TxtMotivoVehiculo.Text = string.Empty;
                        this.Alerta("Vehículos registrado con éxito.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                        this.Carga_Vehiculo(this.TxtBuscarVehiculo.Text, cLinea_Naviera);


                        Limpiar();

                    }
  
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema al grabar, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "BtnAgregar_Click", "Hubo un error al grabar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                }


            }



        }

        protected void BtnBuscarVehiculo_Click(object sender, EventArgs e)
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

                    string cLinea_Naviera = this.TxtLineaNaviera.Text.Trim();

                    if (string.IsNullOrEmpty(cLinea_Naviera))
                    {
                        this.Alerta("Debe seleccionar la líneas naviera");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtLineaNaviera.Focus();
                        return;
                    }


                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    this.Carga_Vehiculo(this.TxtBuscarVehiculo.Text, cLinea_Naviera);

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema al consultar, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "BtnBuscarVehiculo_Click", "Hubo un error al consultar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                }
            }

        }

        #endregion

        #region "Eventos Controles Choferes"

        protected void BtnAgregarChofer_Click(object sender, EventArgs e)
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

                    string IdEmpresa = this.RucEmpresa.Value;
                    string IdChofer = this.IdChofer.Value;
                    string NombreChofer = this.NombreChofer.Value;
                    string Licencia = this.Licencia.Value;
                    string EmpresaTransporte = this.Txtempresa.Text;
                    string Transportista = this.TxtChofer.Text;

                    string cLinea_Naviera = this.TxtLineaNaviera.Text.Trim();

                    if (string.IsNullOrEmpty(cLinea_Naviera))
                    {
                        this.Alerta("Debe seleccionar la líneas naviera");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtLineaNaviera.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        this.Alerta("Debe seleccionar la Compañía de Transporte para poder agregar la información");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.Txtempresa.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(IdEmpresa))
                    {
                        this.Alerta("Debe volver a seleccionar la Compañía de Transporte para poder agregar la información");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.Txtempresa.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(Licencia))
                    {
                        this.Alerta("Debe seleccionar el chofer a excluir");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtChofer.Focus();
                        return;
                    }

                    Int64 nIdChofer = 0;

                    if (!Int64.TryParse(IdChofer, out nIdChofer))
                    {
                        this.Alerta("Debe volver a seleccionar el Chofer a excluir");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtChofer.Focus();
                        return;
                    }

                   

                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objChofer = new MantenimientoChoferes();
                    objChofer.ID_EMPRESA = IdEmpresa;
                    objChofer.ID_CHOFER = nIdChofer;
                    objChofer.LICENCIA = Licencia;
                    objChofer.LINEA_NAVIERA = cLinea_Naviera;

                    if (objChofer.Existe_Chofer(out v_mensaje))
                    {
                        this.Alerta(string.Format("El Chofer: {0}, ya se encuentra registrado para la empresa de transporte: {1}", NombreChofer, Txtempresa.Text));
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtChofer.Focus();
                        return;

                    }


                    Chofer = new Choferes();
                    Chofer.ID_EMPRESA = IdEmpresa;
                    Chofer.ID = nIdChofer;
                    Chofer.ID_CHOFER = Licencia;
                    if (!Chofer.PopulateMyData(out v_mensaje))
                    {
                        string Error = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se pudo cargar datos del chofer"), "Choferes", "BtnAgregarChofer_Click", "Hubo un error al agregar chofer", sUser != null ? sUser.loginname : "Nologin"));
                        this.Alerta(Error);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtVehiculo.Focus();
                        return;
                    }

                    objChofer = new MantenimientoChoferes();
                    objChofer.ID_CHOFER = nIdChofer;
                    objChofer.LICENCIA = Licencia;
                    objChofer.NOMBRES = Chofer.NOMBRE_CHOFER;
                    objChofer.ID_EMPRESA = IdEmpresa;
                    objChofer.STATUS = Chofer.STATUS;

                    objChofer.LICENCIA_EXPIRACION = Chofer.LICENCIA_EXPIRACION;
                    objChofer.CHOFER_SUSPENDIDO = Chofer.LICENCIA_SUSPENDIDA;
                   
                    objChofer.USUARIO_CRE = sUser.loginname;
                    objChofer.ESTADO = true;
                    objChofer.LINEA_NAVIERA = cLinea_Naviera;
                    objChofer.MENSAJE = Chofer.MENSAJE;
                    objChofer.MOTIVO = this.TxtMotivoChofer.Text.Trim();
                    objChofer.Save(out v_mensaje);
                    if (v_mensaje != string.Empty)
                    {
                        this.Alerta(v_mensaje.ToString());

                    }
                    else
                    {
                        /************************************** auditoria *****************************************/
                        objAuditoria = new Auditoria();
                        objAuditoria.OPCION = "Autorizacion de Vehiculos y Choferes";
                        objAuditoria.ACCION = string.Format("Agrego Chofer {0} - Empresa {1}, linea naviera: {2}", Transportista, EmpresaTransporte, cLinea_Naviera);
                        objAuditoria.USUARIO = sUser.loginname;
                        objAuditoria.Save(out v_mensaje);
                        /************************************** auditoria *****************************************/

                        this.TxtChofer.Text = string.Empty;
                        this.IdChofer.Value = "0";
                        this.Licencia.Value = string.Empty;
                        this.NombreChofer.Value = string.Empty;
                        this.TxtMotivoChofer.Text = string.Empty;
                        this.Alerta("Chofer registrado con éxito.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                        this.Carga_Chofer(this.TxtBuscarChofer.Text, cLinea_Naviera);
                       

                        Limpiar();

                    }

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema al grabar, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "Agregar", "BtnAgregarChofer_Click", "Hubo un error al grabar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                }


            }
        }

        protected void BtnBuscarChofer_Click(object sender, EventArgs e)
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

                    string cLinea_Naviera = this.TxtLineaNaviera.Text.Trim();

                    if (string.IsNullOrEmpty(cLinea_Naviera))
                    {
                        this.Alerta("Debe seleccionar la líneas naviera");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtLineaNaviera.Focus();
                        return;
                    }

                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    this.Carga_Chofer(this.TxtBuscarChofer.Text, cLinea_Naviera);

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema al consultar, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "BtnBuscarChofer_Click", "Hubo un error al consultar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                }
            }

        }
        #endregion


        #region "Eventos de la Grilla de Vehiculos"

        protected void tablePagination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    tablePagination.PageIndex = e.NewPageIndex;
                    tablePagination.DataSource = DtVehiculos.AsDataView();
                    tablePagination.DataBind();

                 
                }


            }
            catch (Exception ex)
            {
                 var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "paginar", "tablePagination_PageIndexChanging", "Hubo un error al buscar", t.loginname));
                sinresultado.Visible = true;
            }
        }

        protected void tablePagination_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination_RowCommand", "No se pudo obtener usuario", "anónimo"));

                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = cMensaje2;
                        sinresultado.Visible = true;
                        return;
                    }

                    if (e.CommandName == "Seleccionar")
                    {
                        string Id = e.CommandArgument.ToString();
                        Int64 nId = 0;

                        if (!Int64.TryParse(Id, out nId))
                        {
                            var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "tablePagination_RowCommand", "CommandArgument longitud errada: " + Id.Length.ToString(), user.loginname));
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = cMensaje2;
                            sinresultado.Visible = true;
                            return;
                        }

                        usuario sUser = null;
                        sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        objVehiculo = new MantenimientoVehiculo();
                        objVehiculo.ID = nId;
                        objVehiculo.USUARIO_CRE = sUser.loginname;

                        objVehiculo.Delete(out v_mensaje);
                        if (v_mensaje != string.Empty)
                        {
                            this.Alerta(v_mensaje.ToString());

                        }
                        else
                        {
                            string cLinea_Naviera = this.TxtLineaNaviera.Text.Trim();
                            /************************************** auditoria *****************************************/
                            var currentStatRow = (from currentStat in DtVehiculos.AsEnumerable()
                                                  where currentStat.Field<Int64>("ID") == nId
                                                  select currentStat).FirstOrDefault();

                            String ID_EMPRESA = currentStatRow["ID_EMPRESA"].ToString();
                            String PLACA = currentStatRow["PLACA"].ToString();
                            String RAZON_SOCIAL = currentStatRow["RAZON_SOCIAL"].ToString();

                            objAuditoria = new Auditoria();
                            objAuditoria.OPCION = "Excluir Vehículos y choferes";
                            objAuditoria.ACCION = string.Format("Inactivo Vehículos {0} Empresa: {1}-{2}, linea naviera: {3}", PLACA, ID_EMPRESA,RAZON_SOCIAL, cLinea_Naviera);
                            objAuditoria.USUARIO = sUser.loginname;
                            objAuditoria.Save(out v_mensaje);
                            /************************************** auditoria *****************************************/

                            this.Alerta("Se inactivo Vehículos con éxito.");

                            this.Carga_Vehiculo(this.TxtBuscarChofer.Text, sUser.ruc);
                          
                        }

                    }

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la eliminación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "eliminar", "tablePagination_RowCommand", "Hubo un error al eliminar", t.loginname));
                    sinresultado.Visible = true;
                }


            }

        }

        #endregion

        #region "Eventos de la Grilla de Choferes"

        protected void tablePagination2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    tablePagination2.PageIndex = e.NewPageIndex;
                    tablePagination2.DataSource = DtChoferes.AsDataView();
                    tablePagination2.DataBind();
                }
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "paginar", "tablePagination2_PageIndexChanging", "Hubo un error al buscar", t.loginname));
                sinresultado.Visible = true;
            }
        }

        protected void tablePagination2_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination2_RowCommand", "No se pudo obtener usuario", "anónimo"));

                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = cMensaje2;
                        sinresultado.Visible = true;
                        return;
                    }

                    if (e.CommandName == "Seleccionar")
                    {
                        string Id = e.CommandArgument.ToString();
                        Int64 nId = 0;

                        if (!Int64.TryParse(Id, out nId))
                        {
                            var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "tablePagination2_RowCommand", "CommandArgument longitud errada: " + Id.Length.ToString(), user.loginname));
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = cMensaje2;
                            sinresultado.Visible = true;
                            return;
                        }

                        usuario sUser = null;
                        sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        objChofer = new MantenimientoChoferes();
                        objChofer.ID = nId;
                        objChofer.USUARIO_CRE = sUser.loginname;

                        objChofer.Delete(out v_mensaje);
                        if (v_mensaje != string.Empty)
                        {
                            this.Alerta(v_mensaje.ToString());

                        }
                        else
                        {
                            string cLinea_Naviera = this.TxtLineaNaviera.Text.Trim();
                            /************************************** auditoria *****************************************/
                            var currentStatRow = (from currentStat in DtChoferes.AsEnumerable()
                                                  where currentStat.Field<Int64>("ID") == nId
                                                  select currentStat).FirstOrDefault();

                            String ID_EMPRESA = currentStatRow["ID_EMPRESA"].ToString();
                            String CHOFER = currentStatRow["NOMBRES"].ToString();
                            String RAZON_SOCIAL = currentStatRow["RAZON_SOCIAL"].ToString();

                            objAuditoria = new Auditoria();
                            objAuditoria.OPCION = "Excluir Vehículos y choferes";
                            objAuditoria.ACCION = string.Format("Inactivo Chofer {0} Empresa: {1}-{2}, linea naviera: {3}", CHOFER, ID_EMPRESA, RAZON_SOCIAL, cLinea_Naviera);
                            objAuditoria.USUARIO = sUser.loginname;
                            objAuditoria.Save(out v_mensaje);
                            /************************************** auditoria *****************************************/

                            this.Alerta("Se inactivo Chofer con éxito.");

                            this.Carga_Chofer(this.TxtBuscarChofer.Text, sUser.ruc);

                        }

                    }

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la eliminación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "eliminar", "tablePagination2_RowCommand", "Hubo un error al eliminar", t.loginname));
                    sinresultado.Visible = true;
                }


            }
        }

            #endregion


        }
}