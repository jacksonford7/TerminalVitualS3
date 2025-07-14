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
    public partial class frm_nivel_aprobacion : System.Web.UI.Page
    {
        private group objGroup= new group();
        private group_detail objGroup_Detail = new group_detail();

        private level_approval objNivel = new level_approval();
        private level_approval_detail objNivel_Detail = new level_approval_detail();

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

        private void Genera_Id_Nivel()
        {
            try
            {
                List<level_approval> Lista = level_approval.Max_Id_Nivel();
                if (Lista != null)
                {
                    var xList = Lista.FirstOrDefault();

                    if (xList != null)
                    {
                        this.TxtID.Text = xList.id_level.ToString("D3");
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

        private void Carga_ListadoConceptos()
        {
            try
            {

                List<concepts> ListConceptos = concepts.ListConceptosActivos();
                if (ListConceptos != null)
                {
                    this.CboConcepto.DataSource = ListConceptos;
                    this.CboConcepto.DataBind();
                }
                else
                {
                    this.CboConcepto.DataSource = null;
                    this.CboConcepto.DataBind();
                }



            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "CboConcepto", "Carga_ListadoConceptos", "Hubo un error al cargar conceptos", user2 != null ? user2.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }

        }

        private void Limpiar()
        {

            Session["Action"] = "I";//NUEVO INGRESO 
            this.Accion.Value = "I";
            this.Nombre_Nivel.Value = null;
            //this.IdUsuario.Value = null;
            //this.Usuario.Value = null;  
            //this.Nombre.Value = null;
            //this.Apellido.Value = null;
            //this.Email.Value = null;
            //this.Nombre_Grupo.Value = null;


            this.llave1.InnerText = "Seleccionar";
            this.llave1.Style.Value  = "cursor: pointer; color: black; width:100px;";
           
            this.sel_g_id_group.Value = null;
            this.sel_g_group_name.Value = null;
            this.sel_g_estado.Value = null;
            this.sel_g_create_user.Value = null;
            this.sel_g_usuarios.Value = null;

            
            this.TxtID.Text = null;
            this.TxtUsuario.Text = null;
            this.TxtNombreNivel.Text = null;
            this.TxtValorInicial.Text = null;
            this.TxtValorFinal.Text = null;
            //this.TxtCantidad.Text = null;
            this.CboCantidad.SelectedIndex = -1;
            this.TxtID.Enabled = false;
            this.TxtUsuario.Enabled = false;
            this.TxtNombreNivel.Enabled = true;

            tablePagination.DataSource = null;
            tablePagination.DataBind();

            this.CboEstado.Items.Clear();
            this.CboEstado.Items.Add("Activo");
            this.CboEstado.Items.Add("Inactivo");

          
           

            //cabecera del grupo
            objNivel = new level_approval();
            Session["Nivel"] = objNivel;
           

            this.tablePagination2.DataSource = null;
            this.tablePagination2.DataBind();

            this.Genera_Id_Nivel();
         
            this.CboConcepto.SelectedIndex = -1;

            this.TxtNombreNivel.Focus();

        }

        private void Carga_ListadoNiveles()
        {
            try
            {

                List<level_approval> ListGroup = level_approval.ListNivelesAprobacion();
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
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "group", "Carga_ListadoNiveles", "Hubo un error al cargar niveles de aprobación", user2 != null ? user2.loginname : "Nologin"));
                this.Alerta(sg);
                return;
            }

        }

        private void Carganiveles()
        {

            try
            {

                List<level> Lista = level.ListNiveles();
                if (Lista != null && Lista.Count > 0)
                {
                    this.CboCantidad.DataSource = Lista;
                    this.CboCantidad.DataBind();

                }
                else
                {
                    CboCantidad.DataSource = null;
                    CboCantidad.DataBind();
                }
            }
            catch (Exception ex)
            {
                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "CboCantidad", "Carganiveles", "Hubo un error al cargar niveles de aprobación", user2 != null ? user2.loginname : "Nologin"));
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
                this.Carga_ListadoNiveles();
                this.Carganiveles();
                this.Carga_ListadoConceptos();
                this.CboConcepto.SelectedIndex = -1;


            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

            try
            {

                if (this.sel_g_id_group.Value == string.Empty)
                {
                    this.Alerta("Debe seleccionar un grupo de usuarios");
                    return;
                }
                if (this.TxtNombreNivel.Text == string.Empty)
                {
                    this.TxtUsuario.Text = this.sel_g_group_name.Value;

                    this.Alerta("Debe ingresar el nombre del nivel de aprobación");
                    return;
                }

                if (this.TxtValorInicial.Text == string.Empty)
                {
                    this.TxtUsuario.Text = this.sel_g_group_name.Value;

                    this.Alerta("debe ingresar el valor inicial");
                    return;
                }
                if (this.TxtValorFinal.Text == string.Empty)
                {
                    this.TxtUsuario.Text = this.sel_g_group_name.Value;

                    this.Alerta("debe ingresar el valor final");
                    return;
                }
                if (this.CboConcepto.SelectedValue == string.Empty)
                {
                    this.TxtUsuario.Text = this.sel_g_group_name.Value;
                    this.Alerta("Debe seleccionar el motivo o concepto");
                    return;
                }

                if (this.CboCantidad.SelectedValue == string.Empty)
                {
                    this.TxtUsuario.Text = this.sel_g_group_name.Value;

                    this.Alerta("debe seleccionar el valor del nivel de aprobación");
                    return;
                }

                usuario sUser = null;
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                objNivel = Session["Nivel"] as level_approval;

                //valida si ya fue ingresado el grupo
                int _IdGrupo = int.Parse(this.sel_g_id_group.Value);
                if (objNivel.Detalle.Where(p => p.id_group == _IdGrupo).Count() > 0)
                {
                    this.Alerta(String.Format("El grupo {0} ya se encuentra registrado", this.sel_g_group_name.Value.Trim()));
                    return;
                }

                //valida si ya fue ingresado el nivel
                int _Nivel = int.Parse(this.CboCantidad.SelectedValue);
                if (objNivel.Detalle.Where(p => p.level == _Nivel).Count() > 0)
                {
                    this.Alerta(String.Format("El Nivel {0} ya se encuentra registrado", this.CboCantidad.SelectedValue));
                    return;
                }


                objNivel_Detail.id_level = 0;
                objNivel_Detail.id_group = int.Parse(this.sel_g_id_group.Value);
                objNivel_Detail.sequence = objNivel.Detalle.Count + 1;
                objNivel_Detail.level = int.Parse(this.CboCantidad.SelectedValue);
                objNivel_Detail.group_name = this.sel_g_group_name.Value.Trim();
                objNivel_Detail.usuarios = this.sel_g_usuarios.Value.Trim();
                objNivel_Detail.Estado = this.sel_g_estado.Value.Trim();
                objNivel_Detail.Create_user = this.sel_g_create_user.Value.Trim();
                objNivel_Detail.Mod_user = string.Empty;
                objNivel_Detail.state = true;
                objNivel_Detail.Estado = "ACTIVO";
              

                objNivel.Detalle.Add(objNivel_Detail);
                //objNivel.Detalle.OrderBy(p => p.level); 

               

                tablePagination2.DataSource = objNivel.Detalle.OrderBy(p => p.level);
                tablePagination2.DataBind();
      

                this.TxtUsuario.Text = null;
                this.CboCantidad.SelectedIndex = -1;

                this.sel_g_create_user.Value = "";
                this.sel_g_estado.Value = "";
                this.sel_g_group_name.Value = "";
                this.sel_g_id_group.Value = "";
                this.sel_g_usuarios.Value = "";
                //this.Nombre_Nivel.Value = "";

                Session["Nivel"] = objNivel;

            }
            catch (Exception exc)
            {

                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(exc, "frm_nivel_aprobacion", "BtnAgregar_Click", "Hubo un error al agregar grupos", user2 != null ? user2.loginname : "Nologin"));
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

                objNivel = Session["Nivel"] as level_approval;
                if (objNivel == null)
                {
                    sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No se obtuvo Session[Nivel]"), "frm_nivel_aprobacion", "BtnGrabar_Click", string.Format("Hubo un error al grabar:{0}", this.TxtID.Text), sUser != null ? sUser.loginname : "Nologin"));
                    this.Alerta(sg);
                    return;
                }

                int id_Nivel = 0;
                if (this.TxtID.Text == String.Empty) { id_Nivel = 0; } else { id_Nivel = int.Parse(this.TxtID.Text); }

                decimal init_value = Convert.ToDecimal(this.TxtValorInicial.Text == string.Empty ? "0" : this.TxtValorInicial.Text);
                decimal init_end = Convert.ToDecimal(this.TxtValorFinal.Text == string.Empty ? "0" : this.TxtValorFinal.Text);


                objNivel.id_level = id_Nivel;
                objNivel.level_name = this.TxtNombreNivel.Text.Trim();
                objNivel.level_state = this.CboEstado.SelectedIndex == 0 ? true : false;
                objNivel.Create_user= sUser.loginname;
                objNivel.init_value = init_value;
                objNivel.init_end = init_end;
                objNivel.Action = Session["Action"].ToString();
                objNivel.Mod_user = sUser.loginname;
                objNivel.id_concept = this.CboConcepto.SelectedValue == string.Empty ? Int16.Parse("0") : Int16.Parse(this.CboConcepto.SelectedValue);

                /*si es nuevo registro*/
                if (Session["Action"].ToString() == "I")
                {
                    objNivel.aux_level_name = this.TxtNombreNivel.Text.Trim(); 
                }
                else {
                    objNivel.aux_level_name = this.Nombre_Nivel.Value;
                }

                var nIdRegistro = objNivel.SaveTransaction(out sg);
                if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                {
                    this.Alerta(sg);
                    return;
                }
                var s = string.Format("alert('Éxito al {0} niveles de aprobación {1}');", Session["Action"].ToString() != "I" ? "Actualizar" : "Guardar", nIdRegistro.Value.ToString("D3"));
                ClientScript.RegisterStartupScript(typeof(Page), "closePage", s, true);

                this.Limpiar();
                this.Carga_ListadoNiveles();
              
            }
            catch (Exception exc)
            {

                sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(exc, "frm_nivel_aprobacion", "BtnGrabar_Click", "Hubo un error al grabar nivel de aprobación", user2 != null ? user2.loginname : "Nologin"));
                this.Alerta(sg);
                return;

            }


        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.Limpiar();
            this.Carga_ListadoNiveles();
        }

        protected void RemoverGrupos_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "frm_nivel_aprobacion", "RemoverGrupos_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        this.Alerta(cMensaje2);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "frm_nivel_aprobacion", "RemoverGrupos_ItemCommand", "CommandArgument is NULL", user.loginname));
                        this.Alerta(cMensaje2);
                        return;
                    }
                    //
                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "frm_nivel_aprobacion", "RemoverGrupos_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user.loginname));
                        this.Alerta(cMensaje2);
                        return;
                    }

                    int idGrupo = int.Parse(xpars.ToString());

                    objNivel = Session["Nivel"] as level_approval;

                    //elimina
                    objNivel.Detalle.Remove(objNivel.Detalle.Where(p => p.id_group == idGrupo).FirstOrDefault());

                    //salvar objeto
                    Session["Nivel"] = objNivel;
                    //asignar
                    tablePagination2.DataSource = objNivel.Detalle;
                    tablePagination2.DataBind();

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la eliminación de grupos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_nivel_aprobacion", "RemoverGrupos_ItemCommand", "Hubo un error al eliminar", t.loginname));
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
                    ((Label)e.Item.FindControl("lbl_id_level")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_level_name")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_init_value")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_init_end")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_create_user")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_grupos")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_estado")).ForeColor = System.Drawing.Color.Red;
                    ((Label)e.Item.FindControl("lbl_description")).ForeColor = System.Drawing.Color.Red;
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

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de nivel de aprobación es nulo, posiblemente ha caducado"), "frm_nivel_aprobacion", "Seleccionar_ItemCommand", "No se pudo obtener nivel aprobación", "anónimo"));
                        this.Alerta(cMensaje2);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "frm_nivel_aprobacion", "Seleccionar_ItemCommand", "CommandArgument is NULL", user2.loginname));
                        this.Alerta(cMensaje2);
                        return;
                    }

                    var xpars = e.CommandArgument.ToString();
                    if (xpars.Length <= 0)
                    {

                        var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El arreglo de parametros tiene un número incorrecto de longitud"), "frm_nivel_aprobacion", "Seleccionar_ItemCommand", "CommandArgument longitud errada: " + xpars.Length.ToString(), user2.loginname));
                        this.Alerta(cMensaje2);
                        return;
                    }

                    int _id_Nivel = int.Parse(xpars.ToString());
                    List<level_approval> Lista = level_approval.GetLevel_head(_id_Nivel);
                    var xList = Lista.FirstOrDefault();
                    if (xList != null)
                    {
        
                        this.TxtID.Text = xList.id_level.ToString("D3");
                        this.TxtNombreNivel.Text = xList.level_name;
                        this.TxtValorInicial.Text = xList.init_value.ToString();
                        this.TxtValorFinal.Text = xList.init_end.ToString();
                        this.Nombre_Nivel.Value = xList.level_name;
                        this.CboEstado.SelectedIndex = xList.Indice;
                        this.CboConcepto.SelectedValue = xList.id_concept.ToString();

                        objNivel = Session["Nivel"] as level_approval;
                        objNivel.Detalle.Clear();
                        List<level_approval_detail> ListaDetalle = level_approval_detail.GetLevel_detail(_id_Nivel);

                        foreach (level_approval_detail Lista2 in ListaDetalle)
                        {
                            objNivel_Detail = new level_approval_detail();
                            objNivel_Detail.id_level = Lista2.id_level;
                            objNivel_Detail.level = Lista2.level;
                            objNivel_Detail.id_group = Lista2.id_group;
                            objNivel_Detail.sequence = Lista2.sequence;
                            objNivel_Detail.group_name = Lista2.group_name;
                            objNivel_Detail.usuarios = Lista2.usuarios;
                            objNivel_Detail.state = Lista2.state;
                            objNivel_Detail.Estado = Lista2.Estado;
                            objNivel_Detail.Create_user = Lista2.Create_user;
                            objNivel_Detail.Mod_user = Lista2.Mod_user;
                            objNivel.Detalle.Add(objNivel_Detail);

                        }

                    
                        Session["Nivel"] = objNivel;

                        Session["Action"] = "U";//SE VISUALIZA PARA ACTUALIZAR
                        this.Accion.Value = "U";

                       // tablePagination2.DataSource = objNivel.Detalle;
                        tablePagination2.DataSource = objNivel.Detalle.OrderBy(p => p.level);
                        tablePagination2.DataBind();


                    }
                    else
                    {
                        Session["Action"] = "I";
                        this.MessageBox("No existe el nivel de aprobación seleccionado, no podra realizar modificaciones..", this);
                        return;
                    }



                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    var cMensaje2 = string.Format("Ha ocurrido un problema durante la modificación del nivel de aprobación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_nivel_aprobacion", "Seleccionar_ItemCommand", "Hubo un error al seleccionar", t.loginname));
                    this.MessageBox(cMensaje2.ToString(), this);

                }

            }
        }

        #endregion


    } 
}