using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

namespace CSLSite
{
    public partial class datosgruposusuarios : System.Web.UI.Page
    {

        public static usuario sUser = null;
        public static UsuarioSeguridad us;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        /// <summary>
        /// Código que se ejecuta al iniciar la página (verifica que el usuario este autenticado)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "datosgruposusuarios", "Page_Init", "No autenticado", "No disponible");
                Response.Redirect("../csl/menudefault", false);
                return;
            }
            Page.SslOn();
        }
        
        /// <summary>
        /// Inicialización de controles, carga de combos
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
                divBotonGuardar.Visible = false;
                cargarGrupos();
                //cargarResultados(txtUsuario.Text.Trim(), txtNombres.Text.Trim(), txtIdentificacion.Text.Trim(), txtNombreEmpresa.Text.Trim(), ddlEstado.SelectedValue, ddlTipoUsuario.SelectedValue);

            }
        }
        
        /// <summary>
        /// Código que carga en el grid el resultado de la búsqueda 
        /// </summary>
        /// <param name="usuario">Username del usuario a buscar</param>
        /// <param name="nombreUsuario">Nombre del usuario a buscar</param>
        /// <param name="identificacionUsuario">Identificación del usuario a buscar</param>
        /// <param name="nombreEmpresa">Nombre de la empresa relacionado al usuario a buscar</param>
        /// <param name="estado">Estado del usuario a buscar</param>
        /// <param name="tipoUsuario">Tipo de usuario a buscar</param>
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
                List<UsuariosRolesSeguridad> usuarios = new List<UsuariosRolesSeguridad>();
                Seguridad seguridad = new Seguridad();
                usuarios = seguridad.consultarUsuariosRolesAdministradores(usuario, nombreUsuario, identificacionUsuario, nombreEmpresa, estado, tipoUsuario, identificacionEmpresa, int.Parse(ddlGrupo.SelectedValue));
                if (usuarios != null)
                {
                    if (usuarios.Count > 0)
                    {
                        xfinder.Visible = true;
                        sinresultado.InnerText = "";
                        sinresultado.Visible = false;
                        divBotonGuardar.Visible = true;
                        tablePagination.DataSource = usuarios;
                        tablePagination.DataBind();
                    }
                    else
                    {
                        xfinder.Visible = false;
                        sinresultado.Visible = true;
                        divBotonGuardar.Visible = false;
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
        /// Código que llama al método de búsqueda de asignaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text.Trim()) || !string.IsNullOrEmpty(txtNombreUsuario.Text.Trim()) ||
                !string.IsNullOrEmpty(txtIdentificacion.Text.Trim()) || !string.IsNullOrEmpty(txtEmpresa.Text.Trim()) 
                )
            {
                if (ddlGrupo.SelectedValue != "0")
                {
                    cargarResultados(txtUsuario.Text.Trim(), txtNombreUsuario.Text.Trim(), txtIdentificacion.Text.Trim(), txtEmpresa.Text.Trim(), "A", "A");
                    alerta.Visible = false;
                    alerta.InnerText = "";
                }
                else
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor es obligatorio escoger el rol a asignar.";
                }
            }
            else
            {
                alerta.Visible = true;
                alerta.InnerText = "Por favor llene por lo menos un criterio para poder realizar la búsqueda.";
            }
        }
        
        /// <summary>
        /// Código que carga el combo de grupos
        /// </summary>
        private void cargarGrupos()
        {
            List<Grupo> grupos = new List<Grupo>();
            Seguridad s = new Seguridad();
            grupos = s.consultarGrupo("", "A");
            ddlGrupo.Items.Clear();
            ddlGrupo.Items.Add(new ListItem("Seleccione el rol", "0"));
            ddlGrupo.DataSource = grupos.OrderBy(x => x.descripcion);
            ddlGrupo.DataValueField = "codigo";
            ddlGrupo.DataTextField = "descripcion";
            ddlGrupo.DataBind();
        }
        
        /// <summary>
        /// Código que llama al método de guardado de información de la asignación de grupos - usuarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            this.error.Visible = false;
            this.error.InnerText = "";

            this.alerta.Visible = false;
            this.alerta.InnerText = "";

            DataTable dtUsuario = new DataTable();
            dtUsuario.Columns.AddRange(new DataColumn[] { new DataColumn("idUsuario", typeof(int)), new DataColumn("idGrupo", typeof(int)), new DataColumn("rol", typeof(bool)) });
            int idGrupo = 0;
            foreach (RepeaterItem item in tablePagination.Items)
            {
                int idUsuario = 0;
                bool rol = false;
                Label _lblIdUsuario = item.FindControl("lblIdUsuario") as Label;
                Label _lblIdGrupo = item.FindControl("lblIdGrupo") as Label;
                CheckBox _cbxSeleccion = item.FindControl("chkRow") as CheckBox;
                idUsuario = int.Parse(_lblIdUsuario.Text.Trim());
                idGrupo = int.Parse(_lblIdGrupo.Text.Trim());
                rol = _cbxSeleccion.Checked;
                dtUsuario.Rows.Add(idUsuario, idGrupo, rol);
            }


            Seguridad seguridad = new Seguridad();
            string resultado = seguridad.GuardarModificarRolesUsuarioAdministrador(idGrupo, dtUsuario,sUser.id, sUser.loginname);
            alerta.Visible = false;
            alerta.InnerText = "";
            if (resultado == "ok")
            {
                Utils.mostrarMensajeRedireccionando(this.Page, "Datos de roles - usuarios guardados correctamente.", "../seguridad/grupousuario");

            }
            else
            {
                Utils.mostrarMensaje(this.Page, resultado);
            }
            
        }

    }
}