using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class datospermisos : System.Web.UI.Page
    {
        public static usuario sUser = null;
        public static UsuarioSeguridad us;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        /// <summary>
        /// Código que se ejecutará al iniciar la página (verifica que el usuario se encuentre autenticado)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "datospermisos", "Page_Init", "No autenticado", "No disponible");
                Response.Redirect("../csl/menudefault", false);
                return;
            }
            Page.SslOn();
        }

        /// <summary>
        /// Código que se ejecutará al cargar la página (inicialización de controles y carga de combos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                // xfinder.Visible = true;
                alerta.Visible = false;
                sinresultado.Visible = false;
                Seguridad s = new Seguridad();
                us = s.consultarUsuarioPorId(sUser.id);
                cargarGrupos();
                cargarAreas();
                divbotoneraguardar.Visible = false;
                //cargarResultados(txtUsuario.Text.Trim(), txtNombres.Text.Trim(), txtIdentificacion.Text.Trim(), txtNombreEmpresa.Text.Trim(), ddlEstado.SelectedValue, ddlTipoUsuario.SelectedValue);

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
        /// Código que carga el combo de áreas
        /// </summary>
        private void cargarAreas()
        {
            List<Area> areas = new List<Area>();
            Seguridad s = new Seguridad();
            areas = s.consultarAreasServicios("", "", "A");
            ddlArea.Items.Clear();
            ddlArea.Items.Add(new ListItem("Seleccione el área", "0"));
            ddlArea.DataSource = areas.OrderBy(x => x.nombreArea);
            ddlArea.DataValueField = "idServicio";
            ddlArea.DataTextField = "nombreArea";
            ddlArea.DataBind();
        }


        /// <summary>
        /// Código que cargará el grid con los resultados de la consulta
        /// </summary>
        /// <param name="idGrupo">Id del grupo que desea consultar</param>
        /// <param name="idServicio">Id del servicio que desea consultar</param>
        private void cargarResultados(int idGrupo, int idServicio)
        {
            try
            {
                List<Permiso> permisos = new List<Permiso>();
                Seguridad seguridad = new Seguridad();
                permisos = seguridad.consultarPermisos(idGrupo, idServicio);
                if (permisos != null)
                {
                    if (permisos.Count > 0)
                    {

                        xfinder.Visible = true;
                        sinresultado.InnerText = "";
                        sinresultado.Visible = false;
                        tablePagination.DataSource = permisos;
                        tablePagination.DataBind();
                        divbotoneraguardar.Visible = true;
                    }
                    else
                    {
                        divbotoneraguardar.Visible = false;
                        xfinder.Visible = false;
                        sinresultado.Visible = true;
                        sinresultado.InnerText = "No hay información relacionada según los criterios de búsqueda establecidos";
                    }
                }
            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "permisos", "consultar_permisos", ddlArea.SelectedValue + " - " + ddlGrupo.SelectedValue, sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }

        /// <summary>
        /// Código que llamará al método de búsqueda de permisos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btbuscar_Click(object sender, EventArgs e)
        {

            this.error.Visible = false;
            this.error.InnerText = "";

            this.alerta.Visible = false;
            this.alerta.InnerText = "";



            if (ddlArea.SelectedValue == "0")
            {
                alerta.Visible = true;
                alerta.InnerText = "Por favor llene los campos obligatorios.";
                return;
            }

            if (ddlGrupo.SelectedValue == "0")
            {
                alerta.Visible = true;
                alerta.InnerText = "Por favor llene los campos obligatorios.";
                return;
            }

            cargarResultados(int.Parse(ddlGrupo.SelectedValue), int.Parse(ddlArea.SelectedValue));
        }


        /// <summary>
        /// Código que llamará al método de guardado de los permisos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int idServicio = 0;
                int idGrupo = 0;
                string permisos = string.Empty;

                foreach (RepeaterItem item in tablePagination.Items)
                {
                    Label _lblIdOpcion = item.FindControl("lblIdOpcion") as Label;
                    Label _lblIdServicio = item.FindControl("lblIdServicio") as Label;
                    Label _lblIdGrupo = item.FindControl("lblIdGrupo") as Label;
                    CheckBox _cbxSeleccion = item.FindControl("chkRow") as CheckBox;
                    idServicio = int.Parse(_lblIdServicio.Text.Trim());
                    idGrupo = int.Parse(_lblIdGrupo.Text.Trim());
                    permisos += _cbxSeleccion.Checked ? "1" : "0";
                }


                Seguridad seguridad = new Seguridad();
                string resultado = seguridad.GuardarModificarPermisos(idGrupo, idServicio, permisos, sUser.id, sUser.loginname);
                alerta.Visible = false;
                alerta.InnerText = "";
                if (resultado == "ok")
                {
                    Utils.mostrarMensajeRedireccionando(this.Page, "Datos de permisos guardados correctamente.", "../seguridad/permiso");

                }
                else
                {
                    Utils.mostrarMensaje(this.Page, resultado);
                }
                

            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante el guardado de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "permisos", "guardar_permisos", ddlArea.SelectedValue + " - " + ddlGrupo.SelectedValue, sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }


        /// <summary>
        /// Código que llamará al método limpiar()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Response.Redirect("../seguridad/permiso");
        }
    }
}