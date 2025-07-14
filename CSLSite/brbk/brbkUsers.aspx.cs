using BreakBulk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.brbk
{
    public partial class brbkUsers : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;

        #endregion

        private users oUsers
        {
            get
            {
                return (users)Session["SessionUsers"];
            }
            set
            {
                Session["SessionUsers"] = value;
            }
        }


        #region "Variables"
        private static Int64? lm = -3;
        private string OError;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Server.HtmlEncode(txtNombre.Text.Trim());

                if (!Page.IsPostBack)
                {
                    LlenaComboPosition();
                    LlenaComboRoles();
                    LlenaComboCompany();
                    Listado_Usuarios();
                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";
                    cmbPosicion.Items.Add(item);
                    cmbRol.Items.Add(item);
                    cmbCompañia.Items.Add(item);
                    cmbPosicion.SelectedValue = "0";
                    cmbRol.SelectedValue = "0";
                    cmbCompañia.SelectedValue = "0";

                    this.Ocultar_Mensaje();
                }
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.SslOn();
            }

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);
                return;
            }

            this.banmsg.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
                ClsUsuario = Page.Tracker();
                txtNombre.Text = string.Empty;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        #region "Metodos"

        public void LlenaComboPosition()
        {
            try
            {
                cmbPosicion.DataSource = positions.consultaPositions();
                cmbPosicion.DataValueField = "ID";
                cmbPosicion.DataTextField = "Description";
                cmbPosicion.DataBind();
                cmbPosicion.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboPosition), "brbkUsers.LlenadoComboPosition", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboRoles()
        {
            try
            {
                cmbRol.DataSource = roles.consultaRoles();
                cmbRol.DataValueField = "ID";
                cmbRol.DataTextField = "Name";
                cmbRol.DataBind();
                cmbRol.Enabled = true;

                
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboRoles), "brbkUsers.LlenaComboRoles", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboCompany()
        {
            try
            {
                cmbCompañia.DataSource = company.consultaCompany();
                cmbCompañia.DataValueField = "ID";
                cmbCompañia.DataTextField = "Names";
                cmbCompañia.DataBind();
                cmbCompañia.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboRoles), "brbkUsers.LlenaComboCompany", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void Listado_Usuarios()
        {
            try
            {
                var Resultado = users.listadoUsers(out OError);

                if (OError != string.Empty)
                {
                    this.Alerta(OError);
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                    this.txtNombre.Focus();
                }

                if (Resultado != null)
                {
                    foreach (var a in Resultado)
                    {
                        a.Position = positions.GetPositions(long.Parse(a.PositionId?.ToString()));
                        a.Role = roles.GetRoles(long.Parse(a.RoleId?.ToString()));
                        a.Company = company.GetCompany(long.Parse(a.CompanyId?.ToString()));
                    }

                    var LinqQuery = from Tbl in Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Username))
                                    select new
                                    {
                                        id = Tbl.Id,
                                        nombre = Tbl.Names.Trim(),
                                        username = Tbl.Username.Trim(),
                                        password = Tbl.Password.Trim(),
                                        identification = Tbl.Identification.Trim(),
                                        phone = Tbl.Phone.Trim(),
                                        usuarioCrea = Tbl.Create_user.Trim(),
                                        fechaCreacion = Tbl.Create_date.Value.ToString("dd/MM/yyyy HH:mm"),
                                        estado = Tbl.Status,
                                        position = Tbl.Position.Description.Trim(),
                                        rol = Tbl.Role.Name.Trim(),
                                        company = Tbl.Company?.Names.Trim(),
                                        email = Tbl.Email.Trim(),
                                    };
                    if (LinqQuery != null && LinqQuery.Count() > 0)
                    {
                        tablePagination.DataSource = LinqQuery;
                        tablePagination.DataBind();
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                    }

                    this.Actualiza_Paneles();
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Listado_Usuarios), "brbkUsers.ListaUsuarios", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                return;
            }
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)//cabecera
            {
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("1");
            }
            this.Actualiza_Paneles();
        }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        private void Actualiza_Paneles()
        {
            this.UPCARGA.Update();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
        }
        #endregion

        #region "Eventos"
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

                    try
                    {
                        var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        ClsUsuario = ClsUsuario_;
                    }
                    catch
                    {
                        Response.Redirect("../login.aspx", false);
                        return;
                    }

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos"));
                        return;
                    }

                    Int64 id;
                    var t = e.CommandArgument.ToString();
                    if (!Int64.TryParse(t, out id))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no se puede convertir id"));
                        return;
                    }

                    if (e.CommandName == "Modificar")
                    {
                        var Resultado = users.GetUsuario(long.Parse(id.ToString()));
                        if (Resultado != null)
                        {
                            oUsers = Resultado;
                            txtNombre.Text = Resultado.Names.Trim();
                            txtUsers.Text = Resultado.Username.Trim();
                            txtIdentificacion.Text = Resultado.Identification.Trim();
                            txtPhone.Text = Resultado.Phone.Trim();
                            txtEmail.Text = Resultado.Email.Trim();
                            txtPassword.Text = Resultado.Password.Trim();
                            cmbEstado.SelectedValue = Resultado.Status.ToString();
                            cmbPosicion.SelectedValue = Resultado.Position?.Id.ToString();
                            cmbRol.SelectedValue = Resultado.Role?.Id.ToString();
                            cmbCompañia.SelectedValue = Resultado.Company?.Id.ToString();
                        }
                        else
                        {
                            oUsers = null;
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0} ", "No se pudo editar para modificar"));
                            return;
                        }
                        Actualiza_Paneles();
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "brbkUsers.tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                    return;
                }
            }
        }
        #endregion

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    txtNombre.Text = txtNombre.Text.ToUpper().Trim();
                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbPosicion.SelectedValue) || cmbPosicion.SelectedValue == "0")
                    {
                        this.Alerta("Seleccione una área.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una área."));
                        cmbPosicion.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbRol.SelectedValue) || cmbPosicion.SelectedValue == "0")
                    {
                        this.Alerta("Seleccione un Rol.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione un rol."));
                        cmbRol.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbCompañia.SelectedValue) || cmbCompañia.SelectedValue == "0")
                    {
                        this.Alerta("Seleccione una compañia.");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultagiffecha();", true);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione una compañia."));
                        cmbCompañia.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtNombre.Text))
                    {
                        this.Alerta("Ingrese el nombre del usuario.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el nombre del usuario"));
                        this.txtNombre.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtIdentificacion.Text))
                    {
                        this.Alerta("Ingrese la Identificación.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la Identificación"));
                        this.txtNombre.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtUsers.Text))
                    {
                        this.Alerta("Ingrese el usuario.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el usuario"));
                        this.txtNombre.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtPhone.Text))
                    {
                        this.Alerta("Ingrese el número de teléfono.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el número de teléfono"));
                        this.txtNombre.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtEmail.Text))
                    {
                        this.Alerta("Ingrese Email.");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el Email"));
                        this.txtNombre.Focus();
                        return;
                    }

                    try
                    {
                        var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        ClsUsuario = ClsUsuario_;
                    }
                    catch
                    {
                        Response.Redirect("../login.aspx", false);
                        return;
                    }

                    users oUsr = new users();
                    if (oUsers != null)
                    {
                        oUsr.Id = oUsers.Id;
                        oUsr.Names = txtNombre.Text;
                        oUsr.Username = txtUsers.Text;
                        oUsr.Password = txtPassword.Text;
                        oUsr.Identification = txtIdentificacion.Text;
                        oUsr.Phone = txtPhone.Text;
                        oUsr.PositionId = long.Parse(cmbPosicion.SelectedValue);
                        oUsr.RoleId = long.Parse(cmbRol.SelectedValue);
                        oUsr.CompanyId =long.Parse(cmbCompañia.SelectedValue);
                        oUsr.Status = bool.Parse(cmbEstado.SelectedValue);
                        oUsr.Modifie_user = ClsUsuario.loginname;
                        oUsr.Email = txtEmail.Text;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(this.txtPassword.Text))
                        {
                            this.Alerta("Ingrese la clave.");
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la clave"));
                            this.txtNombre.Focus();
                            return;
                        }

                        oUsr.Names = txtNombre.Text;
                        oUsr.Username = txtUsers.Text;
                        oUsr.Password = txtPassword.Text;
                        oUsr.Identification = txtIdentificacion.Text;
                        oUsr.Phone = txtPhone.Text;
                        oUsr.PositionId = long.Parse(cmbPosicion.SelectedValue);
                        oUsr.RoleId = long.Parse(cmbRol.SelectedValue);
                        oUsr.CompanyId = long.Parse(cmbCompañia.SelectedValue);
                        oUsr.Status = bool.Parse(cmbEstado.SelectedValue);
                        oUsr.Create_user = ClsUsuario.loginname;
                        oUsr.Email = txtEmail.Text;
                    }

                    oUsr.Save_Update(out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.txtNombre.Focus();
                        return;
                    }
                    else
                    {
                        this.Alerta("Registro exitoso");
                        this.Ocultar_Mensaje();
                        btnLimpiar_Click(null, null);
                    }
                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAdd), "brbkUsers.BtnAdd", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
                Listado_Usuarios();
                oUsers = null;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            oUsers = null;
            txtIdentificacion.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtUsers.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            cmbEstado.SelectedValue = "True";
            cmbPosicion.SelectedValue = "0";
            cmbRol.SelectedValue = "0";
            cmbCompañia.SelectedValue = "0";
            Ocultar_Mensaje();
        }
    }
}