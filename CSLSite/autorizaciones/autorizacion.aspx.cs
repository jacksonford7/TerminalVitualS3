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
using ClsAutorizaciones;

namespace CSLSite.autorizaciones
{
    public partial class autorizacion : System.Web.UI.Page
    {
        public static string v_mensaje = string.Empty;
        private string EmpresaSelect = string.Empty;
        private MantenimientoEmpresa objEmpresa = new MantenimientoEmpresa();
        private Auditoria objAuditoria = new Auditoria();

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

        #region "Metodos Web Services"

        [System.Web.Services.WebMethod]
        public static string[] GetEmpresas(string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {

                List<EmpresaTransporte> ListTransporte = EmpresaTransporte.Lista_Empresa(prefix, out v_mensaje);
                if (ListTransporte != null)
                {
                    var LinqQuery = (from Tbl in ListTransporte.Where(Tbl => Tbl.ruc != null)
                                     select new
                                     {
                                         EMPRESA = string.Format("{0} - {1}", Tbl.ruc.Trim(), Tbl.razon_social.Trim()),
                                         RUC = Tbl.ruc.Trim(),
                                         NOMBRE = Tbl.razon_social.Trim(),
                                         ID = Tbl.ruc.Trim()
                                     });

                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }

                }
                
            
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        #endregion


        #region "Metodos"
        private void Carga_EmpresaTransporte(string LINEA_NAVIERA)
        {
            try
            {

                List<MantenimientoEmpresa> Listado = MantenimientoEmpresa.Listado_Empresas(LINEA_NAVIERA, out v_mensaje);
                if (Listado != null)
                {
                    tablePagination.DataSource = Listado;
                    tablePagination.DataBind();
                    xfinder.Visible = true;
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                    xfinder.Visible = false;
                }

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.InnerText = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_EmpresaTransporte", "Hubo un error al cargar listado", t.loginname));
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
               // rucempresa = user.ruc;
                useremail = user.email;
            }
            if (!IsPostBack)
            {

                this.IsCompatibleBrowser();
                Page.SslOn();
            }
           

            this.TxtLineaNaviera.Text = Server.HtmlEncode(this.TxtLineaNaviera.Text);
            this.Txtempresa.Text = Server.HtmlEncode(this.Txtempresa.Text);
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
                            rucempresa= user.ruc;
                        }

                        this.Carga_EmpresaTransporte(this.TxtLineaNaviera.Text);
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
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
            else
            {
                this.TxtLineaNaviera.Text = this.AuxLinea_Naviera.Value;
                if (rucempresa != this.AuxLinea_Naviera.Value)
                {
                    this.Carga_EmpresaTransporte(this.TxtLineaNaviera.Text);
                }
                rucempresa = this.AuxLinea_Naviera.Value;
            }
        }

        #endregion

        #region "Eventos Controles"
   
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

                    string IdEmpresa = string.Empty;
                    string DesEmpresa = string.Empty;

                    if (string.IsNullOrEmpty(this.TxtLineaNaviera.Text))
                    {
                        this.Alerta("Debe seleccionar la líneas naviera");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.TxtLineaNaviera.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        this.Alerta("Debe seleccionar la Compañía de Transporte para poder agregar la información ");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                        this.Txtempresa.Focus();
                        return;
                    }

                    if (!string.IsNullOrEmpty(Txtempresa.Text))
                    {
                        EmpresaSelect = this.Txtempresa.Text.Trim();
                        if (EmpresaSelect.Split('-').ToList().Count > 1)
                        {

                            IdEmpresa = EmpresaSelect.Split('-').ToList()[0].Trim();
                            DesEmpresa = EmpresaSelect.Split('-').ToList()[1].Trim();

                            List<EmpresaTransporte> ValidaEmpresa = ClsAutorizaciones.EmpresaTransporte.Valida_Empresa(IdEmpresa, out v_mensaje);
                            if (ValidaEmpresa != null)
                            {
                                if (ValidaEmpresa.Count != 0)
                                {
                                    foreach (var Det in ValidaEmpresa)
                                    {
                                        DesEmpresa = Det.razon_social;
                                    }

                                    usuario sUser = null;
                                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                                    objEmpresa = new MantenimientoEmpresa();
                                    objEmpresa.ID_EMPRESA = IdEmpresa;
                                    objEmpresa.RAZON_SOCIAL = DesEmpresa;
                                    objEmpresa.USUARIO_CRE = sUser.loginname;
                                    objEmpresa.ESTADO = true;
                                    objEmpresa.LINEA_NAVIERA = this.TxtLineaNaviera.Text.Trim();

                                    if (objEmpresa.Existe_EmpresaTransporte(out v_mensaje))
                                    {
                                        this.Alerta(string.Format("La empresa de transporte: {0}-{1}, ya se encuentra registrada", IdEmpresa, DesEmpresa));
                                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                                        this.Txtempresa.Focus();
                                        return;

                                    }
                                    else
                                    {
                                        objEmpresa.Save(out v_mensaje);
                                        if (v_mensaje != string.Empty)
                                        {
                                            this.Alerta(v_mensaje.ToString());

                                        }
                                        else
                                        {
                                            /************************************** auditoria *****************************************/
                                            objAuditoria = new Auditoria();
                                            objAuditoria.OPCION = "Autorizacion de Compañias de Transportes";
                                            objAuditoria.ACCION = string.Format("Agrego empresa {0}-{1}, linea naviera: {2}", IdEmpresa, DesEmpresa, this.TxtLineaNaviera.Text.Trim());
                                            objAuditoria.USUARIO = sUser.loginname;
                                            objAuditoria.Save(out v_mensaje);
                                            /************************************** auditoria *****************************************/

                                            this.Txtempresa.Text = string.Empty;
                                            this.Alerta("Empresa de transporte registrada con éxito.");
                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);

                                            this.Carga_EmpresaTransporte(this.TxtLineaNaviera.Text.Trim());
                                            this.UPEMPRESA.Update();
                                            this.UPDETALLE.Update();

                                            Limpiar();

                                        }

                                    }


                                }
                                else
                                {
                                    this.Alerta("Debe seleccionar una compañía de transporte valida para agregar la información");
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                                    this.Txtempresa.Focus();
                                    return;
                                }
                            }
                            else
                            {
                                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda: {0}", v_mensaje);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                                sinresultado.Visible = true;
                            }

                        }
                        else
                        {
                            this.Alerta("Debe seleccionar una compañía de transporte valida para agregar la información");
                            this.Txtempresa.Focus();
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema al grabar, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "BtnAgregar_Click", "Hubo un error al grabar", t.loginname));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    sinresultado.Visible = true;
                }


            }



        }

        protected void BtnLimpiar_Click(object sender, EventArgs e)
        {
            this.Txtempresa.Text = string.Empty;
            this.IdTxtempresa.Value = string.Empty;
            this.Txtempresa.Focus();
        }

        #endregion

        #region "Eventos de la Grilla"
        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.InnerText = cMensaje2;
                        sinresultado.Visible = true;
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        
                        sinresultado.InnerText = cMensaje2;
                        sinresultado.Visible = true;
                        return;
                    }
                    //
                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "tablePagination_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        sinresultado.InnerText = cMensaje2;
                        sinresultado.Visible = true;
                        return;
                    }

                    Int64 Id = int.Parse(xpars.ToString());


                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objEmpresa = new MantenimientoEmpresa();
                    objEmpresa.ID = Id;
                    objEmpresa.USUARIO_CRE = sUser.loginname;

                    objEmpresa.Delete(out v_mensaje);
                    if (v_mensaje != string.Empty)
                    {
                        this.Alerta(v_mensaje.ToString());
                       
                    }
                    else
                    {

                        /************************************** auditoria *****************************************/
                        Label ID_EMPRESA = e.Item.FindControl("ID_EMPRESA") as Label;
                        Label RAZON_SOCIAL = e.Item.FindControl("RAZON_SOCIAL") as Label;
                       
                        objAuditoria = new Auditoria();
                        objAuditoria.OPCION = "Autorizacion de Compañias de Transportes";
                        objAuditoria.ACCION = string.Format("Inactivo empresa {0}-{1}, linea naviera: {2}", ID_EMPRESA.Text, RAZON_SOCIAL.Text,this.TxtLineaNaviera.Text.Trim());
                        objAuditoria.USUARIO = sUser.loginname;
                        objAuditoria.Save(out v_mensaje);
                        /************************************** auditoria *****************************************/

                        this.Alerta("Se inactivo empresa de transporte con éxito.");

                        this.Carga_EmpresaTransporte(sUser.ruc);
                        this.UPEMPRESA.Update();
                        this.UPDETALLE.Update();
                    }



                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la eliminación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "eliminar", "tablePagination_ItemCommand", "Hubo un error al eliminar", t.loginname));
                  
                    sinresultado.Visible = true;

                 

                }

            }
        }
        #endregion



    }
}