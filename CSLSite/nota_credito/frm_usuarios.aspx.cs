using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.Web.Script.Services;
using CSLSite.N4Object;
using CSLSite.XmlTool;
using ConectorN4;
using Newtonsoft.Json;
using csl_log;
using System.Text;
using ClsNotasCreditos;
using System.Data;
using System.Globalization;


namespace CSLSite
{
    public partial class frm_usuarios : System.Web.UI.Page
    {

        private user objUsuarios = new user();
        usuario user2;
        string sg;

        #region "Propiedades"

        public static string v_mensaje = string.Empty;

        #endregion

        #region "Metodos"


        private void MessageBox(String msg, Page page)
        {
            StringBuilder s = new StringBuilder();
            msg = msg.Replace("'", " ");
            msg = msg.Replace(System.Environment.NewLine, " ");
            msg = msg.Replace("/", "");
            s.Append(" alert('").Append(msg).Append("');");
            System.Web.UI.ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), s.ToString(), true);

        }

        private void Limpiar()
        {

            Session["Action"] = "I";//NUEVO INGRESO 
            this.Accion.Value = "I";
            this.IdUsuario.Value = null;
            this.Usuario.Value = null;
            this.Nombres.Value = null;
            this.Nombre.Value = null;
            this.Apellido.Value = null;
            this.llave1.InnerText = "Click aqui para Agregar Usuarios";
            this.llave1.Style.Value  = "cursor: pointer; color: black";

            this.TxtID.Text = null;
            this.TxtUsuario.Text = null;
            this.TxtNombres.Text = null;
            this.TxtApellidos.Text = null;
            this.TxtDescripcion.Text = null;//email

            this.TxtID.Enabled = false;
            this.TxtUsuario.Enabled = false;
            this.TxtNombres.Enabled = false;
            this.TxtApellidos.Enabled = false;


            TableUsuarios.DataSource = null;
            TableUsuarios.DataBind();

            this.CboEstado.Items.Clear();
            this.CboEstado.Items.Add("Activo");
            this.CboEstado.Items.Add("Inactivo");

            this.TxtDescripcion.Focus();

        }


    
        private void Carga_ListadoUsuarios()
        {
            try
            {

                List<user> ListUusuarios = user.ListUsuarios();
                if (ListUusuarios != null)
                {
                    TableUsuarios.DataSource = ListUusuarios;
                    TableUsuarios.DataBind();
                }
                else
                {
                    TableUsuarios.DataSource = null;
                    TableUsuarios.DataBind();
                }         
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "user", "Carga_ListadoUsuarios", "Hubo un error al cargar listado de usuarios", user2 != null ? user2.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }

        }

        


        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }



        #region "Eventos del Form"

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }

            user2 = Page.Tracker();
            if (user2 != null)
            {

                this.dtlo.InnerText = string.Format("Estimado {0} {1} :", user2.nombres, user2.apellidos);

            }
            if (user2 != null && !string.IsNullOrEmpty(user2.nombregrupo))
            {

                var sp = string.IsNullOrEmpty(user2.codigoempresa) ? user2.ruc : user2.codigoempresa;
                string t = null;
                if (!string.IsNullOrEmpty(sp))
                {
                    t = CslHelper.getShiperName(sp);
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

                Limpiar();
                this.xfinder.Visible = true;
                /*carga listado de usuarios*/
                this.Carga_ListadoUsuarios();
            }
        }


        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            try
            {

                string cMensaje = "";

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                objUsuarios = new user();
                int id_Usuario = 0;

                if (this.IdUsuario.Value == String.Empty) { id_Usuario = 0; } else { id_Usuario=int.Parse(this.IdUsuario.Value); }

                /*valida si existe*/
                if (this.Accion.Value == "I")
                {
                    List<user> Lista = user.GetUsuario(id_Usuario);
                    var xList = Lista.FirstOrDefault();
                    if (xList != null)
                    {
                        this.IdUsuario.Value = xList.IdUsuario.ToString();
                        this.Usuario.Value = xList.Usuario;
                        this.Nombres.Value = xList.Nombres;
                        this.Nombre.Value = xList.Nombre;
                        this.Apellido.Value = xList.Apellido;

                        this.TxtID.Text = xList.IdUsuario.ToString();
                        this.TxtUsuario.Text = xList.Usuario;
                        this.TxtNombres.Text = xList.Nombre;
                        this.TxtApellidos.Text = xList.Apellido;

                        this.TxtDescripcion.Text = xList.Email;
                        this.CboEstado.SelectedIndex = xList.Indice;

                        this.MessageBox(String.Format("El usuario: {0}, ya se encuentra registrado", xList.Usuario.Trim()), this);
                        return;
                    }                 
                }

                objUsuarios.IdUsuario = id_Usuario;
                objUsuarios.Usuario = this.Usuario.Value.Trim();
                objUsuarios.Nombre = this.Nombre.Value.Trim();
                objUsuarios.Apellido = this.Apellido.Value.Trim();
                objUsuarios.Email = this.TxtDescripcion.Text.Trim();
                objUsuarios.Create_user = sUser.loginname;
                objUsuarios.Mod_user = sUser.loginname;
                objUsuarios.state = this.CboEstado.SelectedIndex == 0 ? true : false;

                if (this.Accion.Value == "I") { Session["Action"] = this.Accion.Value; }

                objUsuarios.Action = Session["Action"].ToString();

                objUsuarios.Save(out cMensaje);
                if (cMensaje != string.Empty)
                {
                    this.MessageBox(cMensaje.ToString(), this);
                }
                else
                {
                    this.MessageBox("Se grabo usuario con éxito", this);
                    this.Limpiar();
                    this.Carga_ListadoUsuarios();
                }

            }
            catch (Exception exc)
            {

                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(exc, "user", "BtnGrabar_Click", "Hubo un error al grabar usuario", user2 != null ? user2.loginname : "Nologin"));
                this.Alerta(sg);
                return;

            }


        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.Limpiar();
            this.Carga_ListadoUsuarios();
        }

        protected void Opciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
               
                Label lbl = (Label)e.Item.FindControl("lbl_estado");
                if (lbl.Text == "INACTIVO")
                {
                    lbl.ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_estado")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_id_usuario")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_usuario")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_nombres")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_email")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_estado")).ForeColor = System.Drawing.Color.Red;
                   
                }

            }

        }


        protected void Seleccionar_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                    var user2 = Page.getUserBySesion();
                    if (user2 == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "Seleccionar_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "Seleccionar_ItemCommand", "CommandArgument is NULL", user2.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "consulta", "Seleccionar_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user2.loginname));
                        this.MessageBox(cMensaje2.ToString(), this);
                        return;
                    }

                    int _Id_Usuario = int.Parse(xpars.ToString());
                    List<user> Lista = user.GetUsuario(_Id_Usuario);
                    var xList = Lista.FirstOrDefault();
                    if (xList != null)
                    {

                        this.IdUsuario.Value = xList.IdUsuario.ToString();
                        this.Usuario.Value = xList.Usuario;
                        this.Nombres.Value = xList.Nombres;
                        this.Nombre.Value = xList.Nombre;
                        this.Apellido.Value = xList.Apellido;
                        this.llave1.InnerText =  xList.Nombres;
                        
                        this.TxtID.Text = xList.IdUsuario.ToString();
                        this.TxtUsuario.Text = xList.Usuario;
                        this.TxtNombres.Text = xList.Nombre;
                        this.TxtApellidos.Text = xList.Apellido;

                        this.TxtDescripcion.Text = xList.Email;
                        this.CboEstado.SelectedIndex = xList.Indice;

                        Session["Action"] = "U";//SE VISUALIZA PARA ACTUALIZAR
                        this.Accion.Value = "U";
                        this.llave1.Style.Value = "cursor: pointer; color: blue";
                    }
                    else
                    {
                        Session["Action"] = "I";
                        this.MessageBox("No existe el usuario seleccionado, no podra realizar modificaciones..", this);
                        return;
                    }



                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la modificación del usuario, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "user", "Seleccionar_ItemCommand", "Hubo un error al seleccionar", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }

            }
        }

        #endregion


    } 
}