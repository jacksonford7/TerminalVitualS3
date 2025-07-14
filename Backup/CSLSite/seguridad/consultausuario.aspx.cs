using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CSLSite
{
    public partial class consultausuario : System.Web.UI.Page
    {

        public static usuario sUser = null;
        public static UsuarioSeguridad us;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        /// <summary>
        /// Código que se ejecutará al iniciar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "consultausuario", "Page_Init", "No autenticado", "No disponible");
                Response.Redirect("../csl/menudefault", false);
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
            Session["usuarioId"] = null;
            if (!IsPostBack)
            {
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                // xfinder.Visible = true;
                alerta.Visible = false;
                sinresultado.Visible = false;
                Seguridad s = new Seguridad();
                us = s.consultarUsuarioPorId(sUser.id);
                cargarTipoUsuarios();
                //cargarResultados(txtUsuario.Text.Trim(), txtNombres.Text.Trim(), txtIdentificacion.Text.Trim(), txtNombreEmpresa.Text.Trim(), ddlEstado.SelectedValue, ddlTipoUsuario.SelectedValue);

            }
        }


        /// <summary>
        /// Consulta los usuarios según los filtros de búsqueda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["usuarioId"] = null;
            Response.Redirect("../seguridad/datosusuario");
        }


        /// <summary>
        /// Carga el combo de tipo de usuario
        /// </summary>
        private void cargarTipoUsuarios()
        {


            HashSet<Tuple<string, string>> tipoUsuariosTemp = new HashSet<Tuple<string, string>>();
            tipoUsuariosTemp = Seguridad.getDetalleCatalogosSeguridad(ConfigurationManager.AppSettings["catalogoTipoUsuarios"]);
            HashSet<Tuple<string, string>> tipoUsuarios = new HashSet<Tuple<string, string>>();
            if (us.tipoUsuario != ConfigurationManager.AppSettings["tipoAdministradorInterno"])
            {
                foreach (Tuple<string, string> item in tipoUsuariosTemp)
                {
                    if (item.Item1 != ConfigurationManager.AppSettings["tipoAdministradorInterno"] && item.Item1 != ConfigurationManager.AppSettings["tipoAdministradorEmpresa"])
                    {
                        tipoUsuarios.Add(item);
                    }
                }
            }
            else
            {
                ddlTipoUsuario.Items.Add(new ListItem("TODOS", "0"));
                tipoUsuarios = tipoUsuariosTemp;
            }

            ddlTipoUsuario.DataSource = tipoUsuarios;
            ddlTipoUsuario.DataValueField = "item1";
            ddlTipoUsuario.DataTextField = "item2";
            ddlTipoUsuario.DataBind();
        }


        /// <summary>
        /// Código que carga el grid con los resultados de la consulta
        /// </summary>
        /// <param name="usuario">Username del usuario</param>
        /// <param name="nombreUsuario">Nombre personal del usuario</param>
        /// <param name="identificacionUsuario">Identificacón del usuario</param>
        /// <param name="nombreEmpresa">Nombre de empresa relacionado con el usuario</param>
        /// <param name="estado">Estado del usuario</param>
        /// <param name="tipoUsuario">Tipo de usuario </param>
        private void cargarResultados(string usuario, string nombreUsuario, string identificacionUsuario, string nombreEmpresa, string estado, string tipoUsuario)
        {
            string identificacionEmpresa = string.Empty;
            if (us.tipoUsuario == ConfigurationManager.AppSettings["tipoAdministradorEmpresa"])
            {
                identificacionEmpresa = us.codigoEmpresa;
            }
            else
            {
                identificacionEmpresa = "0";
            }

            try
            {
                List<UsuarioSeguridad> usuarios = new List<UsuarioSeguridad>();
                Seguridad seguridad = new Seguridad();
                usuarios = seguridad.consultarUsuarios(usuario, nombreUsuario, identificacionUsuario, nombreEmpresa, estado, tipoUsuario, identificacionEmpresa);
                if (usuarios != null)
                {
                    if (usuarios.Count > 0)
                    {
                        xfinder.Visible = true;
                        sinresultado.InnerText = "";
                        sinresultado.Visible = false;
                        tablePagination.DataSource = usuarios.OrderBy(x => x.usuario);
                        tablePagination.DataBind();
                    }
                    else
                    {
                        xfinder.Visible = false;
                        sinresultado.Visible = true;
                        sinresultado.InnerText = "No hay información relacionada según los criterios de búsqueda establecidos";
                    }
                }
            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "consultar_usuarios", txtUsuario.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }

        /// <summary>
        /// Código que realiza la búsqueda de los usuarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            error.InnerText = "";
            error.Visible = false;
            if (!string.IsNullOrEmpty(txtUsuario.Text.Trim()) || !string.IsNullOrEmpty(txtNombres.Text.Trim()) ||
                !string.IsNullOrEmpty(txtIdentificacion.Text.Trim()) || !string.IsNullOrEmpty(txtNombreEmpresa.Text.Trim()) ||
                ddlEstado.SelectedValue != "0" || ddlTipoUsuario.SelectedValue != "0"
                )
            {
                cargarResultados(txtUsuario.Text.Trim(), txtNombres.Text.Trim(), txtIdentificacion.Text.Trim(), txtNombreEmpresa.Text.Trim(), ddlEstado.SelectedValue, ddlTipoUsuario.SelectedValue);
                alerta.Visible = false;
                alerta.InnerText = "";
            }
            else
            {
                alerta.Visible = true;
                alerta.InnerText = "Por favor llene por lo menos un criterio para poder realizar la búsqueda.";
            }
        }


        /// <summary>
        /// Código que setea la variable de sesión para la modificación del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSetearId_Click(object sender, EventArgs e)
        {
            Session["usuarioId"] = int.Parse(hdfIdUsuario.Value.ToString());
            //Response.Redirect("../seguridad/datosusuario");

        }

        /// <summary>
        /// Código que realiza el reseteo de la clave de un determinado usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btResetear_Click(object sender, EventArgs e)
        {
            int idUsuario = int.Parse(hdfIdUsuario.Value.ToString());
            string username = hdfUserName.Value.ToString();
            Seguridad s = new Seguridad();

            UsuarioSeguridad usRecuperacion = s.consultarUsuarioPorId(idUsuario);
            if (usRecuperacion != null)
            {
                try
                {

                    if (string.IsNullOrEmpty(usRecuperacion.usuarioCorreo.Trim()))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "El usuario no posee una cuenta de correo configurada, no se puede proseguir con el proceso de recuperación de contraseña.";
                        return;
                    }

                    string passwordPlano = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10);
                    string password = string.Empty;
                    Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
                    if (componente == null)
                    {
                        csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El componente AISVUtils.GSS ha fallado"), "usuario", "recuperacion_contraseña", usRecuperacion.usuario, usRecuperacion.usuario);
                        alerta.Visible = true;
                        alerta.InnerText = "Hubo un problema durante la recuperación de la contraseña";
                        return;
                    }
                    else
                    {
                        dynamic instancia = Activator.CreateInstance(componente);
                        var dcpass = instancia.Encrypt(usRecuperacion.usuario, passwordPlano) as string;
                        if (string.IsNullOrEmpty(dcpass))
                        {
                            csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado"), "usuario", "recuperacion_contraseña", usRecuperacion.usuario, usRecuperacion.usuario);
                            alerta.Visible = true;
                            alerta.InnerText = "Hubo un problema durante el cambio de contraseña.";
                            return;

                        }
                        password = dcpass;
                        instancia = null;
                        componente = null;
                    }



                    string resultado = s.CambioPasswordUsuario(password, usRecuperacion.idUsuario, "S", "RC", sUser.id, sUser.loginname);
                    alerta.Visible = false;
                    alerta.InnerText = "";
                    if (resultado == "ok")
                    {

                        var jmsg = new jMessage();
                        string mail = string.Empty;
                        string destinatarios = turnoConsolidacion.GetMails();
                        mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/>Este es un mensaje del Centro de Servicios en Linea de Contecon S.A, para comunicarle lo siguiente.<br/>Se ha recibido su solicitud para realizar la recuperación de la contraseña del usuario <strong> {2} </strong> en la siguiente fecha y hora {3}<br/>", usRecuperacion.nombreUsuario, usRecuperacion.apellidoUsuario, usRecuperacion.usuario, DateTime.Now.ToString()));
                        mail = string.Concat(mail, string.Format("La contraseña que deberá usar para iniciar sesión es: <strong> {0}</strong><br/> </br> Por su seguridad al momento de iniciar sesión, automáticamente se le solicitará el cambio de contraseña. Muchas Gracias por su comprensión", passwordPlano));
                        mail = string.Concat(mail, string.Format("<br/><br/>Es un placer servirle, <br/><br/>"));
                        mail = string.Concat(mail, string.Format("Atentamente, <br/><br/>"));
                        mail = string.Concat(mail, string.Format("<strong>S3 - Sistema de Solicitud de Servicios </strong> <br/>"));
                        mail = string.Concat(mail, string.Format("Contecon Guayaquil S.A. CGSA <br/>"));
                        mail = string.Concat(mail, string.Format("An ICTSI Group Company <br/><br/>"));
                       
                        string errorMail = string.Empty;
                        var user_email = usRecuperacion.usuarioCorreo;// usRecuperacion.usuarioCorreo;
                        var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
                        destinatarios = user_email+";"+correoBackUp;


                        //var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";

                        CLSDataSeguridad.addMail(out errorMail, sUser.email, "Recuperación de contraseña CGSA", mail, destinatarios, usRecuperacion.usuario, "", "");

                        if (!string.IsNullOrEmpty(errorMail))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = errorMail;
                            return;

                        }
                        else
                        {
                            Utils.mostrarMensaje(this.Page, "Se le enviará un correo al usuario " + usRecuperacion.usuario + " cuyo correo es " + usRecuperacion.usuarioCorreo + " con su nueva contraseña generada.");
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
                    this.error.InnerText = string.Format("Se produjo un error durante la recuperación de contraseña, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuario", "recuperacion_contraseña", usRecuperacion.usuario, usRecuperacion.usuario));
                    Utils.mostrarMensaje(this.Page, ex.Message.ToString());
                }
            }
        }
    }
}