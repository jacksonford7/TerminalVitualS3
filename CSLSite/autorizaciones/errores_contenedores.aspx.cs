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
using System.Text;

namespace CSLSite.autorizaciones
{
    public partial class errores_contenedores : System.Web.UI.Page
    {
        public static string v_mensaje = string.Empty;
        private Contenedor objContenedor = new Contenedor();
        private Auditoria objAuditoria = new Auditoria();


        #region "Metodos"
        private void Carga_Errores()
        {
            try
            {

                List<Contenedor> Listado = Contenedor.CONSULTA_ERRORES_CONTENEDORES( out v_mensaje);
                if (Listado != null && Listado.Count != 0)
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
                sinresultado.InnerText = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_Errores", "Hubo un error al cargar listado", t.loginname));
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

                        this.Carga_Errores();
                    }
                    else
                    {
                        
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
                    string Tipo = e.CommandName.ToString();

                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    Label ID_ORDEN = e.Item.FindControl("ID") as Label;

                    objContenedor = new Contenedor();
                    objContenedor.ID = Int64.Parse(ID_ORDEN.Text);
                    objContenedor.SECUENCIA = Id;
                    objContenedor.TIPO = (Tipo.Equals("Remover")? "P" : "E");
                    objContenedor.Activar(out v_mensaje);
                    if (v_mensaje != string.Empty)
                    {
                        this.Alerta(v_mensaje.ToString());
                       
                    }
                    else
                    {

                        
                        Label CONTENEDOR = e.Item.FindControl("CONTENEDOR") as Label;
                        Label LINEA_NAVIERA = e.Item.FindControl("LINEA_NAVIERA") as Label;
                       
                        objAuditoria = new Auditoria();
                        objAuditoria.OPCION = "Detalle de Errores al Procesar EDO/Orden de Retiro";
                        objAuditoria.ACCION = string.Format("{0} contenedor: {1}, linea naviera: {2} para reprocesar EDO, usuario: {3}", (Tipo.Equals("Remover") ? "Activo" : "Elimino") ,CONTENEDOR.Text, LINEA_NAVIERA.Text, sUser.ruc);
                        objAuditoria.USUARIO = sUser.loginname;
                        objAuditoria.Save(out v_mensaje);
 

                        this.Alerta("Proceso realizado con éxito.");

                        this.Carga_Errores();
                       
                        this.UPDETALLE.Update();
                    }



                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la activación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "eliminar", "tablePagination_ItemCommand", "Hubo un error al activar", t.loginname));
                  
                    sinresultado.Visible = true;

                 

                }

            }
        }
        #endregion

        protected void ChkTodos_CheckedChanged(object sender, EventArgs e)
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

                    bool estado = this.ChkTodos.Checked;

                    foreach (RepeaterItem item in tablePagination.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            var checkBox = (CheckBox)item.FindControl("chkMarcar");
                          
                            checkBox.Checked = estado;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.InnerText = string.Format("Ha ocurrido un problema al marcar registros, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "eliminar", "ChkTodos_CheckedChanged", "Hubo un error al marcar", t.loginname));

                sinresultado.Visible = true;

            }
        }

        protected void BtnProcesarTodos_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }

                    bool Existen = false;
                    StringBuilder tab = new StringBuilder();
                    tab.Append("<CONTENEDORES>");

                    foreach (RepeaterItem item in tablePagination.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            CheckBox Chk = item.FindControl("chkMarcar") as CheckBox;
                            Label SECUENCIA = item.FindControl("SECUENCIA") as Label;
                            if (Chk.Checked)
                            {
                                Existen = true;
                                tab.Append(string.Format("<CONTENEDOR  SECUENCIA='{0}'  />", SECUENCIA.Text.Trim()));

                            }

                        }
                    }

                    tab.Append("</CONTENEDORES>");
                    string XMLActualizar = tab.ToString();


                    if (Existen)
                    {
                        objContenedor = new Contenedor();
                        objContenedor.Activar_Unidades_PorLote(XMLActualizar, out v_mensaje);
                        if (v_mensaje != string.Empty)
                        {
                            this.Alerta(v_mensaje.ToString());

                        }
                        else
                        {
                            this.ChkTodos.Checked = false;

                            this.Alerta("Se activaron contenedores con éxito.");

                            this.Carga_Errores();

                            this.UPDETALLE.Update();
                        }
                    }
                    else
                    {
                        this.Alerta("Debe seleccionar los contenedores a realizar el re-poroceso.");
                    }
                   
                    

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la activación por lote, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "eliminar", "BtnProcesarTodos_Click", "Hubo un error al activar", t.loginname));

                    sinresultado.Visible = true;

                }
            }

        }

        protected void BtnEliminarTodos_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }


                    bool Existen = false;
                    StringBuilder tab = new StringBuilder();
                    tab.Append("<CONTENEDORES>");

                    foreach (RepeaterItem item in tablePagination.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            CheckBox Chk = item.FindControl("chkMarcar") as CheckBox;
                            Label SECUENCIA = item.FindControl("SECUENCIA") as Label;
                            if (Chk.Checked)
                            {
                                Existen = true;
                                tab.Append(string.Format("<CONTENEDOR  SECUENCIA='{0}'  />", SECUENCIA.Text.Trim()));

                            }

                        }
                    }

                    tab.Append("</CONTENEDORES>");
                    string XMLActualizar = tab.ToString();

                    if (Existen)
                    {
                        objContenedor = new Contenedor();
                        objContenedor.Eliminar_Unidades_PorLote(XMLActualizar, out v_mensaje);
                        if (v_mensaje != string.Empty)
                        {
                            this.Alerta(v_mensaje.ToString());

                        }
                        else
                        {
                            this.ChkTodos.Checked = false;

                            this.Alerta("Se Inactivaron contenedores con éxito.");

                            this.Carga_Errores();

                            this.UPDETALLE.Update();
                        }
                    }
                    else
                    {
                        this.Alerta("Debe seleccionar los contenedores a realizar la Inactivación");
                    }


                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la inactivación por lote, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "eliminar", "BtnEliminarTodos_Click", "Hubo un error al eliminar", t.loginname));

                    sinresultado.Visible = true;
                }
            }

        }
    }
}