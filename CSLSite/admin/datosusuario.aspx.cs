using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.admin
{
    public partial class datosusuario : System.Web.UI.Page
    {
        public static int contadorIdentificacion;
        public static usuario sUser = null;
        public static UsuarioSeguridad us;
        public static UsuarioSeguridad usAutenticado;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }


        /// <summary>
        /// Código que se ejeutará al iniciar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "datosusuario", "Page_Init", "No autenticado", "No disponible");
                //Response.Redirect("../cuenta/menu.aspx", false);
                Response.Redirect("../login.aspx", false);
                return;
            }
            Page.SslOn();
        }

        /// <summary>
        /// Código que se ejecutará al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                contadorIdentificacion = 0;
                Seguridad s = new Seguridad();
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                usAutenticado = s.consultarUsuarioPorId(sUser.id);
                cargarTipoUsuarios();
                cargarIdTipoUsuarios();
                cargarPais();
                cargarProvincia();
                cargarCiudad();
                alerta.Visible = false;
                td_buscar.Visible = false;

                if (usAutenticado.codigoEmpresa == ConfigurationManager.AppSettings["RUC_CGSA"].ToString() || usAutenticado.codigoEmpresa == ConfigurationManager.AppSettings["INI_CGSA"].ToString())
                {
                    cmbTipoUsuario.Visible = true;
                    lbltipo.Visible = true;
                    td_buscar.Visible = true;
                }

                if (Session["usuarioId"] != null)
                {
                    int idUsuario = int.Parse(Session["usuarioId"].ToString());

                    us = s.consultarUsuarioPorId(idUsuario);

                    if (us != null)
                    {
                        txtUsuario.Text = us.usuario;
                        ddlTipoUsuario.SelectedValue = us.tipoUsuario;
                        cmbTipoUsuario.SelectedValue = us.IdTipoUsuario.ToString();
                        ddlEstado.SelectedValue = us.estado;
                        txtUsuarioNombre.Text = us.nombreUsuario;
                        txtUsuarioApellido.Text = us.apellidoUsuario;
                        txtUsuarioRuc.Text = us.usuarioIdentificacion;
                        cargarPais();
                        ddlPais.SelectedValue = us.pais;
                        cargarProvincia();
                        ddlProvincia.SelectedValue = us.idProvincia.ToString();
                        cargarCiudad();
                        ddlCiudad.SelectedValue = us.ciudad.ToString();
                        txtEmail.Text = us.usuarioCorreo;
                        txtEmpresaIdentificacion.Text = us.codigoEmpresa.Trim();
                        txtEmpresaNombre.Text = us.nombreEmpresa.Trim();
                        txtEmpresaDireccion.Text = us.direccionEmpresa.Trim();
                        txtEmpresaTelefono.Text = us.telefonoEmpresa.Trim();
                        txtEmpresaFax.Text = us.faxEmpresa.Trim();
                        txtEmpresaWebSite.Text = us.websiteEmpresa.Trim();
                        txtEmpresaCorreo.Text = us.correoEmpresa.Trim();
                        txtPassword.Text = us.password.Trim();
                        txtUsuario.Enabled = false;
                        txtPassword.Enabled = false;
                        //td_buscar.Visible = false;

                        cmbTipoUsuario.SelectedValue = us.IdTipoUsuario.ToString();

                        if (usAutenticado.tipoUsuario != ConfigurationManager.AppSettings["tipoAdministradorInterno"].ToString())
                        {
                            HabilitarDeshabilitarEmpresa(false, false);
                        }
                        else
                        {
                            HabilitarDeshabilitarEmpresa(false, true);
                        }

                    }
                }
                else
                {
                    if (usAutenticado.tipoUsuario != ConfigurationManager.AppSettings["tipoAdministradorInterno"].ToString())
                    {
                        //td_buscar.Visible = false;
                        txtEmpresaIdentificacion.Text = usAutenticado.codigoEmpresa.Trim();
                        txtEmpresaNombre.Text = usAutenticado.nombreEmpresa.Trim();
                        txtEmpresaDireccion.Text = usAutenticado.direccionEmpresa.Trim();
                        txtEmpresaTelefono.Text = usAutenticado.telefonoEmpresa.Trim();
                        txtEmpresaFax.Text = usAutenticado.faxEmpresa.Trim();
                        txtEmpresaWebSite.Text = usAutenticado.websiteEmpresa.Trim();
                        txtEmpresaCorreo.Text = usAutenticado.correoEmpresa.Trim();
                        HabilitarDeshabilitarEmpresa(false, false);
                        cmbTipoUsuario.SelectedValue = usAutenticado.IdTipoUsuario.ToString();

                    }
                }
            }
        }

        /// <summary>
        /// Código que deshabilita o habilita los campos de la información de la empresa
        /// </summary>
        /// <param name="habilitarIdentificacion"></param>
        /// <param name="habilitar"></param>
        private void HabilitarDeshabilitarEmpresa(bool habilitarIdentificacion, bool habilitar)
        {
            txtEmpresaIdentificacion.Enabled = habilitarIdentificacion;
            txtEmpresaNombre.Enabled = habilitar;
            txtEmpresaDireccion.Enabled = habilitar;
            txtEmpresaTelefono.Enabled = habilitar;
            txtEmpresaFax.Enabled = habilitar;
            txtEmpresaWebSite.Enabled = habilitar;
            txtEmpresaCorreo.Enabled = habilitar;
        }

        /// <summary>
        /// Código que carga los tipos de usuarios
        /// </summary>
        private void cargarTipoUsuarios()
        {
            HashSet<Tuple<string, string>> tipoUsuariosTemp = new HashSet<Tuple<string, string>>();
            tipoUsuariosTemp = Seguridad.getDetalleCatalogosSeguridad(ConfigurationManager.AppSettings["catalogoTipoUsuarios"]);
            HashSet<Tuple<string, string>> tipoUsuarios = new HashSet<Tuple<string, string>>();
            if (usAutenticado.tipoUsuario != ConfigurationManager.AppSettings["tipoAdministradorInterno"])
            {
                foreach (Tuple<string, string> item in tipoUsuariosTemp)
                {
                    if (item.Item1 != ConfigurationManager.AppSettings["tipoAdministradorInterno"] && item.Item1 != ConfigurationManager.AppSettings["tipoAdministradorEmpresa"])
                    {
                        tipoUsuarios.Add(item);
                    }
                    else
                    {
                        if (usAutenticado.codigoEmpresa == ConfigurationManager.AppSettings["RUC_CGSA"].ToString() || usAutenticado.codigoEmpresa == ConfigurationManager.AppSettings["INI_CGSA"].ToString())
                        {
                            tipoUsuarios.Add(item);
                        }
                    }
                }
            }
            else
            {
                ddlTipoUsuario.Items.Add(new ListItem("SELECCIONE UNO", "0"));
                tipoUsuarios = tipoUsuariosTemp;
            }

            ddlTipoUsuario.DataSource = tipoUsuarios;
            ddlTipoUsuario.DataValueField = "item1";
            ddlTipoUsuario.DataTextField = "item2";
            ddlTipoUsuario.DataBind();
        }

        private void cargarIdTipoUsuarios()
        {
            HashSet<Tuple<int, string>> TipoUsuariosTemp = new HashSet<Tuple<int, string>>();
            TipoUsuariosTemp = Seguridad.getTipoUsuarios();
            HashSet<Tuple<int, string>> tiposUsuarios = new HashSet<Tuple<int, string>>();

            cmbTipoUsuario.Items.Add(new ListItem("SELECCIONE UNO", "-1"));
            cmbTipoUsuario.DataSource = TipoUsuariosTemp;
            cmbTipoUsuario.DataValueField = "item1";
            cmbTipoUsuario.DataTextField = "item2";
            cmbTipoUsuario.DataBind();


        }

            /// <summary>
            /// Código que carga el catálogo de país
            /// </summary>
        private void cargarPais()
        {
            HashSet<Tuple<string, string>> paisesTemp = new HashSet<Tuple<string, string>>();
            paisesTemp = Seguridad.getPaises();
            HashSet<Tuple<string, string>> paises = new HashSet<Tuple<string, string>>();

            ddlPais.Items.Add(new ListItem("SELECCIONE UNO", "-1"));
            ddlPais.DataSource = paisesTemp;
            ddlPais.DataValueField = "item1";
            ddlPais.DataTextField = "item2";
            ddlPais.DataBind();
        }

        /// <summary>
        /// Código que carga el catálogo de provincia
        /// </summary>
        private void cargarProvincia()
        {
            ddlProvincia.Items.Clear();
            if (ddlPais.SelectedValue == ConfigurationManager.AppSettings["codigoEcuador"].ToString())
            {
                HashSet<Tuple<string, string>> provinciaTemp = new HashSet<Tuple<string, string>>();
                provinciaTemp = Seguridad.getProvincias();
                HashSet<Tuple<string, string>> ciudades = new HashSet<Tuple<string, string>>();
                ddlProvincia.Items.Add(new ListItem("SELECCIONE UNO", "-1"));
                ddlProvincia.DataSource = provinciaTemp;
                ddlProvincia.DataValueField = "item1";
                ddlProvincia.DataTextField = "item2";
                ddlProvincia.DataBind();
            }
            else
            {

                if (ddlPais.SelectedValue == "-1")
                {
                    HashSet<Tuple<string, string>> provinciaTemp = new HashSet<Tuple<string, string>>();
                    provinciaTemp = Seguridad.getProvincias();
                    ddlProvincia.Items.Add(new ListItem("SELECCIONE UNO", "-1"));
                    ddlProvincia.DataBind();
                }
                else
                {
                    HashSet<Tuple<string, string>> provinciaTemp = new HashSet<Tuple<string, string>>();
                    provinciaTemp = Seguridad.getProvincias();
                    ddlProvincia.Items.Add(new ListItem("SELECCIONE UNO", "-1"));
                    Tuple<string, string> provincia = provinciaTemp.FirstOrDefault(x => x.Item2.ToUpper().Trim() == "OTROS");
                    ddlProvincia.Items.Add(new ListItem(provincia.Item2, provincia.Item1));
                    ddlProvincia.DataBind();
                }
            }
        }

        /// <summary>
        /// Código que carga el catálogo de ciudad
        /// </summary>
        private void cargarCiudad()
        {
            ddlCiudad.Items.Clear();
            HashSet<Tuple<string, string>> ciudadTemp = new HashSet<Tuple<string, string>>();
            ciudadTemp = Seguridad.getCiudades(int.Parse(ddlProvincia.SelectedValue));
            HashSet<Tuple<string, string>> ciudades = new HashSet<Tuple<string, string>>();

            ddlCiudad.Items.Add(new ListItem("SELECCIONE UNO", "-1"));
            ddlCiudad.DataSource = ciudadTemp;
            ddlCiudad.DataValueField = "item1";
            ddlCiudad.DataTextField = "item2";
            ddlCiudad.DataBind();

        }

        /// <summary>
        /// Código que regresa a la pantalla de consulta del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("../admin/consultausuario.aspx");
        }

        /// <summary>
        /// Código que realiza el guardado de la información del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                alerta.Visible = false;
                alerta.InnerText = "";

                error.Visible = false;
                error.InnerText = "";





                txtEmpresaIdentificacion.Enabled = true;
                Seguridad seguridad = new Seguridad();
                if (string.IsNullOrEmpty(txtUsuario.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (Session["usuarioId"] == null)
                {
                    if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "Por favor llene los campos obligatorios.";
                        return;
                    }
                }

                if (ddlTipoUsuario.SelectedValue == "0")
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (cmbTipoUsuario.SelectedValue == "0")
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (string.IsNullOrEmpty(txtEmpresaIdentificacion.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (string.IsNullOrEmpty(txtUsuarioNombre.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (string.IsNullOrEmpty(txtUsuarioApellido.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (string.IsNullOrEmpty(txtUsuarioRuc.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                //if (string.IsNullOrEmpty(txtEmpresaNombre.Text.Trim()))
                //{
                //    alerta.Visible = true;
                //    alerta.InnerText = "Por favor llene los campos obligatorios.";
                //    return;
                //}

                //if (string.IsNullOrEmpty(txtEmpresaCorreo.Text.Trim()))
                //{
                //    alerta.Visible = true;
                //    alerta.InnerText = "Por favor llene los campos obligatorios.";
                //    return;
                //}

                if (contadorIdentificacion < int.Parse(ConfigurationManager.AppSettings["contadorMaximoIdentificacion"].ToString()))
                {
                    if (contadorIdentificacion + 1 == int.Parse(ConfigurationManager.AppSettings["contadorMaximoIdentificacion"].ToString()))
                    {
                        if ((hdfIdentificacion.Value.ToString() == "0"))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Por favor verifique la identificación del usuario que esta ingresando. Si está completamente seguro que la identificación es correcta, por favor de click en GUARDAR.";
                            contadorIdentificacion++;
                            return;
                        }
                    }
                    else
                    {
                        if ((hdfIdentificacion.Value.ToString() == "0"))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Por favor verifique la identificación del usuario que esta ingresando.";
                            contadorIdentificacion++;
                            return;
                        }
                    }
                }

                if ((hdfCorreoEmpresa.Value.ToString() == "0") || (hdfCorreoUsuario.Value.ToString() == "0"))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor verifique el formato de los correos que esta ingresando.";
                    return;
                }


                UsuarioSeguridad usuario = new UsuarioSeguridad();
                UsuarioSeguridad userTemp = null;
                if (Session["usuarioId"] == null)
                {
                    usuario.idUsuario = 0;
                    userTemp = seguridad.consultarUsuarioPorUserName(txtUsuario.Text.Trim());
                    if (userTemp != null)
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "Ya existe un usuario creado con el USERNAME ingresado.";
                        return;
                    }
                }
                else
                    usuario.idUsuario = int.Parse(Session["usuarioId"].ToString());

                if (ddlTipoUsuario.SelectedValue == ConfigurationManager.AppSettings["tipoAdministradorEmpresa"])
                {
                    userTemp = seguridad.consultarUsuarioPorCodigoEmpresa(usuario.idUsuario, txtEmpresaIdentificacion.Text.Trim());
                    if (userTemp != null)
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "Ya existe un usuario activo con el rol de ADMINISTRADOR para la empresa con la identificación: " + txtEmpresaIdentificacion.Text.Trim();
                        return;
                    }
                }

                usuario.usuario = txtUsuario.Text.Trim();
                usuario.password = txtPassword.Text.Trim();
                usuario.tipoUsuario = ddlTipoUsuario.SelectedValue;
                usuario.IdTipoUsuario = int.Parse(cmbTipoUsuario.SelectedValue);
                usuario.estado = ddlEstado.SelectedValue;
                usuario.nombreUsuario = txtUsuarioNombre.Text.Trim();
                usuario.apellidoUsuario = txtUsuarioApellido.Text.Trim();
                usuario.usuarioIdentificacion = txtUsuarioRuc.Text.Trim();
                usuario.pais = ddlPais.SelectedValue.ToString();
                usuario.ciudad = int.Parse(ddlCiudad.SelectedValue);
                usuario.usuarioCorreo = txtEmail.Text.Trim();
                usuario.codigoEmpresa = txtEmpresaIdentificacion.Text.Trim();
                usuario.nombreEmpresa = txtEmpresaNombre.Text.Trim();
                usuario.direccionEmpresa = txtEmpresaDireccion.Text.Trim();
                usuario.telefonoEmpresa = txtEmpresaTelefono.Text.Trim();
                usuario.faxEmpresa = txtEmpresaFax.Text.Trim();
                usuario.websiteEmpresa = txtEmpresaWebSite.Text.Trim();
                usuario.correoEmpresa = txtEmpresaCorreo.Text.Trim();
                usuario.ipCreacion = Request.UserHostAddress;
                usuario.registradoPor = sUser.loginname;
                usuario.IdTipoUsuario = int.Parse(cmbTipoUsuario.SelectedValue.ToString());

                if (Session["usuarioId"] == null)
                {
                    Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
                    if (componente == null)
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El componente AISVUtils.GSS ha fallado"), "usuario", "consulta_usuario_id", usuario.usuario, usuario.usuario);
                        alerta.Visible = true;
                        alerta.InnerText = "Hubo un problema durante el guardado de los datos del usuario.";
                        return;
                    }
                    else
                    {
                        dynamic instancia = Activator.CreateInstance(componente);
                        var dcpass = instancia.Encrypt(usuario.usuario.Trim(), usuario.password.Trim()) as string;
                        if (string.IsNullOrEmpty(dcpass))
                        {
                            csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado"), "usuario", "consulta_usuario_id", usuario.password, usuario.usuario);
                            alerta.Visible = true;
                            alerta.InnerText = "Hubo un problema durante el guardado de los datos del usuario.";
                            return;

                        }
                        usuario.password = dcpass;
                        instancia = null;
                        componente = null;
                    }
                }


                string resultado = seguridad.GuardarModificarUsuario(usuario, sUser.id, sUser.loginname);
                alerta.Visible = false;
                alerta.InnerText = "";
                if (resultado == "ok")
                {
                    if (Session["usuarioId"] == null)
                    {
                        var jmsg = new jMessage();
                        string mail = string.Empty;


                        mail = Utility.nuevo_usuario(txtUsuario.Text.Trim(), txtPassword.Text.Trim());

                        string errorMail = string.Empty;
                        var user_email = usAutenticado.usuarioCorreo;
                        var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
                        //  destinatarios = string.Format("{0};{1}",user_email,correoBackUp);

                        CLSDataSeguridad.addMail(out errorMail, txtEmail.Text, "Nuevo usuario creado.", mail, correoBackUp, sUser.loginname, "", "");

                        if (!string.IsNullOrEmpty(errorMail))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = errorMail;
                            return;

                        }
                        else
                        {
                            Utils.mostrarMensajeRedireccionando(this.Page, "Datos del usuario guardados correctamente.", "../admin/datosusuario.aspx");
                        }

                    }
                    else
                    {
                        Utils.mostrarMensajeRedireccionando(this.Page, "Datos del usuario guardados correctamente.", "../admin/datosusuario.aspx");
                    }
                }
                else
                {
                    Utils.mostrarMensaje(this.Page, resultado);
                }
            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante el guardado de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuario", "guardar_usuario", txtUsuario.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }


        }

        /// <summary>
        /// Código que limpia los controles de la página
        /// </summary>
        private void limpiar()
        {
            Session["usuarioId"] = null;
            Response.Redirect("../admin/datosusuario.aspx");
        }

        /// <summary>
        /// Código que llama a la función limpiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        /// <summary>
        /// Código que realiza la carga de provincia y ciudad cuando se hace un cambio de valor en el país.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarProvincia();
            cargarCiudad();
        }

        /// <summary>
        /// Codigo que realiza la carga de las ciudades cuando se hace un cambio de valor en la provincia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarCiudad();
        }
    }
}