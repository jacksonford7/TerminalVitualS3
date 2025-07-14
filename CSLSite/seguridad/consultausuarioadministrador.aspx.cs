using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CSLSite
{
    public partial class consultausuarioadministrador : System.Web.UI.Page
    {
        public usuario sUser = null;
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
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "consultausuarioadministrador", "Page_Init", "No autenticado", "No disponible");
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
            Session["idUsuarioPadre"] = null;
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
        /// Carga el combo de tipo de usuario
        /// </summary>
        private void cargarTipoUsuarios()
        {


            /* HashSet<Tuple<string, string>> tipoUsuariosTemp = new HashSet<Tuple<string, string>>();
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
             ddlTipoUsuario.DataBind();*/
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

            if (!string.IsNullOrEmpty(txtUsuario.Text.Trim()) || !string.IsNullOrEmpty(txtNombres.Text.Trim()) ||
                !string.IsNullOrEmpty(txtIdentificacion.Text.Trim()) || !string.IsNullOrEmpty(txtNombreEmpresa.Text.Trim())
                )
            {
                cargarResultados(txtUsuario.Text.Trim(), txtNombres.Text.Trim(), txtIdentificacion.Text.Trim(), txtNombreEmpresa.Text.Trim(), "A", "A");
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
            Session["idUsuarioPadre"] = int.Parse(hdfIdUsuario.Value.ToString());
            //Response.Redirect("../seguridad/datosusuario");

        }


    }
}