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
    public partial class frm_grupos : System.Web.UI.Page
    {
        private group objGroup= new group();
        private group_detail objGroup_Detail = new group_detail();

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

        private void Genera_Id_Grupos()
        {
            try
            {
                List<group> Lista = group.Max_Id_Group();
                if (Lista != null)
                {
                    var xList = Lista.FirstOrDefault();

                    if (xList != null)
                    {
                        this.TxtID.Text = xList.id_group.ToString("D3");
                    }
                    else
                    {
                        this.TxtID.Text = "0";
                    }
                }
                else { this.TxtID.Text = "0"; }

            }
            catch (Exception exc)
            {
                this.MessageBox(exc.Message, this);
            }
        }


        private void Limpiar()
        {

            Session["Action"] = "I";//NUEVO INGRESO 
            this.Accion.Value = "I";
            this.IdUsuario.Value = null;
            this.Usuario.Value = null;  
            this.Nombre.Value = null;
            this.Apellido.Value = null;
            this.Email.Value = null;
            this.llave1.InnerText = "Seleccionar";
            this.llave1.Style.Value  = "cursor: pointer; color: black; width:100px;";
            this.Nombre_Grupo.Value = null;

            this.TxtID.Text = null;
            this.TxtUsuario.Text = null;
            this.TxtNombreGrupo.Text = null;
            
            this.TxtID.Enabled = false;
            this.TxtUsuario.Enabled = false;
            this.TxtNombreGrupo.Enabled = true;

            tablePagination.DataSource = null;
            tablePagination.DataBind();

            this.CboEstado.Items.Clear();
            this.CboEstado.Items.Add("Activo");
            this.CboEstado.Items.Add("Inactivo");

            this.TxtNombreGrupo.Focus();

            //cabecera del grupo
            objGroup = new group();
            Session["Group"] = objGroup;

            this.tablePagination2.DataSource = null;
            this.tablePagination2.DataBind();

            this.Genera_Id_Grupos();

        }

        private void Carga_ListadoGrupoUsuarios()
        {
            try
            {

                List<group> ListGroup = group.ListGrupos();
                if (ListGroup != null)
                {
                    tablePagination.DataSource = ListGroup;
                    tablePagination.DataBind();
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                }         
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "group", "Carga_ListadoGrupoUsuarios", "Hubo un error al cargar grupos de usuarios", user2 != null ? user2.loginname : "Nologin"));
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
                /*carga listado de usuarios*/
                this.xfinder.Visible = true;
                this.Carga_ListadoGrupoUsuarios();
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

            try
            {

                if (this.IdUsuario.Value == string.Empty)
                {
                    this.Alerta("Debe seleccionar un usuario, que conformara el grupo");
                    return;
                }
                if (this.TxtNombreGrupo.Text == string.Empty)
                {
                    this.TxtUsuario.Text = this.Usuario.Value;

                    this.Alerta("Debe ingresar el nombre del grupo");
                    return;
                }

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                objGroup = Session["Group"] as group;

                //valida si ya fue ingresado el usuario
                int _IdUsuario = int.Parse(this.IdUsuario.Value);
                if (objGroup.Detalle.Where(p => p.IdUsuario == _IdUsuario).Count() > 0)
                {
                    this.Alerta(String.Format("El usuario {0} ya se encuentra registrado", this.Usuario.Value.Trim()));
                    return;
                }

                objGroup_Detail.id_group = 0;
                objGroup_Detail.sequence = objGroup.Detalle.Count + 1;
                objGroup_Detail.IdUsuario = int.Parse(this.IdUsuario.Value);
                objGroup_Detail.Usuario = this.Usuario.Value.Trim();
                objGroup_Detail.Nombre = this.Nombre.Value.Trim();
                objGroup_Detail.Apellido = this.Apellido.Value.Trim();
                objGroup_Detail.Email = this.Email.Value.Trim();
                objGroup_Detail.Create_user = sUser.loginname;
                objGroup_Detail.Mod_user = string.Empty;
                objGroup_Detail.state = true;
                objGroup_Detail.Estado = "ACTIVO";

                objGroup.Detalle.Add(objGroup_Detail);

                tablePagination2.DataSource = objGroup.Detalle;
                tablePagination2.DataBind();

                this.TxtUsuario.Text = null;
                this.Usuario.Value = "";
                this.Nombre.Value = "";
                this.Apellido.Value = "";
                this.Email.Value = "";
                this.IdUsuario.Value = "";

                Session["Group"] = objGroup;

            }
            catch (Exception exc)
            {

                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(exc, "user", "BtnAgregar_Click", "Hubo un error al agregar usuario", user2 != null ? user2.loginname : "Nologin"));
                this.Alerta(sg);
                return;

            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            try
            {

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString()); 

                objGroup = Session["Group"] as group;
                if (objGroup == null)
                {
                    sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo Session[Group]"), "frm_grupos", "BtnGrabar_Click", string.Format("Hubo un error al grabar:{0}", this.TxtID.Text), sUser != null ? sUser.loginname : "Nologin"));
                    this.Alerta(sg);
                    return;
                }

                int id_Grupo = 0;
                if (this.TxtID.Text == String.Empty) { id_Grupo = 0; } else { id_Grupo = int.Parse(this.TxtID.Text); }

                objGroup.id_group = id_Grupo;
                objGroup.Create_user= sUser.loginname;
                objGroup.group_name = this.TxtNombreGrupo.Text.Trim();
                objGroup.state = this.CboEstado.SelectedIndex == 0 ? true : false;
                objGroup.group_state = this.CboEstado.SelectedIndex == 0 ? true : false;
                objGroup.Action = Session["Action"].ToString();
                objGroup.Mod_user = sUser.loginname;
                /*si es nuevo registro*/
                if (Session["Action"].ToString() == "I")
                {
                    objGroup.aux_group_name = this.TxtNombreGrupo.Text.Trim(); 
                }
                else {
                    objGroup.aux_group_name = this.Nombre_Grupo.Value;
                }

                var nIdRegistro = objGroup.SaveTransaction(out sg);
                if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                {
                    this.Alerta(sg);
                    return;
                }
                var s = string.Format("alert('Éxito al {0} grupo de usuario {1}');", Session["Action"].ToString() != "I" ? "Actualizar" : "Guardar", nIdRegistro.Value.ToString("D3"));
                ClientScript.RegisterStartupScript(typeof(Page), "closePage", s, true);

                this.Limpiar();
                this.Carga_ListadoGrupoUsuarios();
              
            }
            catch (Exception exc)
            {

                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(exc, "frm_grupos", "BtnGrabar_Click", "Hubo un error al grabar grupos usuario", user2 != null ? user2.loginname : "Nologin"));
                this.Alerta(sg);
                return;

            }


        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.Limpiar();
            this.Carga_ListadoGrupoUsuarios();
        }

        protected void RemoverUser_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "frm_grupos", "RemoverUser_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.Alerta(cMensaje2);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "frm_grupos", "RemoverUser_ItemCommand", "CommandArgument is NULL", user.loginname));
                        this.Alerta(cMensaje2);
                        return;
                    }
                    //
                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "frm_grupos", "RemoverUser_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        this.Alerta(cMensaje2);
                        return;
                    }

                    int idUser = int.Parse(xpars.ToString());
                    objGroup = Session["Group"] as group;

                    //elimina
                    objGroup.Detalle.Remove(objGroup.Detalle.Where(p => p.IdUsuario == idUser).FirstOrDefault());

                    //salvar objeto
                    Session["Group"] = objGroup;
                    //asignar
                    tablePagination2.DataSource = objGroup.Detalle;
                    tablePagination2.DataBind();

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la eliminación de usuarios, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_grupos", "RemoverUser_ItemCommand", "Hubo un error al eliminar", t.loginname));
                    this.Alerta(cMensaje2);

                }

            }
        }


        protected void Opciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
               
                Label lbl = (Label)e.Item.FindControl("lbl_estado");
                if (lbl.Text == "INACTIVO")
                {
                    lbl.ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_id_group")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_group_name")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_create_user")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_usuarios")).ForeColor = System.Drawing.Color.Red;
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
                        this.Alerta(cMensaje2);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "frm_grupos", "Seleccionar_ItemCommand", "CommandArgument is NULL", user2.loginname));
                        this.Alerta(cMensaje2);
                        return;
                    }

                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "frm_grupos", "Seleccionar_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user2.loginname));
                        this.Alerta(cMensaje2);
                        return;
                    }

                    int _id_group = int.Parse(xpars.ToString());
                    List<group> Lista = group.GetGroup_head(_id_group);
                    var xList = Lista.FirstOrDefault();
                    if (xList != null)
                    {
        
                        this.TxtID.Text = xList.id_group.ToString("D3");
                        this.TxtNombreGrupo.Text = xList.group_name;
                        this.Nombre_Grupo.Value = xList.group_name;
                        this.CboEstado.SelectedIndex = xList.Indice;

                        objGroup = Session["Group"] as group;
                        objGroup.Detalle.Clear();
                        List<group_detail> ListaDetalle = group_detail.GetGroup_detail(_id_group);

                        foreach (group_detail Lista2 in ListaDetalle)
                        {
                            objGroup_Detail = new group_detail();
                            objGroup_Detail.id_group = Lista2.id_group;
                            objGroup_Detail.sequence = Lista2.sequence;
                            objGroup_Detail.IdUsuario = Lista2.IdUsuario;
                            objGroup_Detail.Usuario = Lista2.Usuario;
                            objGroup_Detail.Nombre = Lista2.Nombre;
                            objGroup_Detail.Apellido = Lista2.Apellido;
                            objGroup_Detail.Email = Lista2.Email;
                            objGroup_Detail.Create_user = Lista2.Create_user;
                            objGroup_Detail.Mod_user = Lista2.Mod_user;
                            objGroup_Detail.state = Lista2.state;
                            objGroup_Detail.Estado = Lista2.Estado;

                            objGroup.Detalle.Add(objGroup_Detail); 

                        }

                        Session["Group"] = objGroup;

                        Session["Action"] = "U";//SE VISUALIZA PARA ACTUALIZAR
                        this.Accion.Value = "U";
                        tablePagination2.DataSource = objGroup.Detalle;
                        tablePagination2.DataBind();

                    }
                    else
                    {
                        Session["Action"] = "I";
                        this.MessageBox("No existe el grupo de usuario seleccionado, no podra realizar modificaciones..", this);
                        return;
                    }



                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la modificación del grupo de usuario, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "group", "Seleccionar_ItemCommand", "Hubo un error al seleccionar", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }

            }
        }

        #endregion


    } 
}