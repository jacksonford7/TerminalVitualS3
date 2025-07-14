using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class datosopcionservicios : System.Web.UI.Page
    {
        public static usuario sUser = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        /// <summary>
        /// Código que se ejecutará al iniciar la página (verifica si el usuario esta autenticado)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "datosopcionservicios", "Page_Init", "No autenticado", "No disponible");
                Response.Redirect("../csl/menudefault", false);
                return;
            }
            Page.SslOn();
        }

        /// <summary>
        /// Código que se ejecutará al cargar la página (inicializar controles, cargar combos, búsqueda inicial)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                xfinder.Visible = true;
                alerta.Visible = false;
                if (idOpcion.Value.ToString() == "")
                    cargarServicios();
                cargarResultados(txtNombreOpcionBuscar.Text.Trim(), txtServicioBuscar.Text.Trim(), ddlEstadoBuscar.SelectedValue);
            }
            else
            {
                if (idOpcion.Value.ToString() != "")
                    ddlServicio.Enabled = false;
            }
        }

        /// <summary>
        /// Código que llama a la función limpiar()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        /// <summary>
        /// Código que carga el combo de servicios
        /// </summary>
        private void cargarServicios()
        {
            if (idOpcion.Value.ToString() == "")
            {
                ddlServicio.Items.Clear();
                ddlServicio.Items.Add(new ListItem("Seleccione un servicio", "0"));
                List<Servicio> servicios = new List<Servicio>();
                Seguridad seguridad = new Seguridad();
                servicios = seguridad.consultarServicio("", "A");
                ddlServicio.Enabled = true;
                ddlServicio.DataSource = servicios.OrderBy(a => a.descripcion); ;
                ddlServicio.DataTextField = "descripcion";
                ddlServicio.DataValueField = "codigo";
                ddlServicio.DataBind();
            }
        }

        /// <summary>
        /// Código que limpia los controles de la página
        /// </summary>
        private void limpiar()
        {
            txtOpcionNombre.Text = "";
            txtTextoIntro.Text = "";
            txtFormASP.Text = "";
            txtTitulo.Text = "";
            ddlEstado.SelectedValue = "A";
            idOpcion.Value = "";
            ddlEstado.Enabled = true;
            this.error.Visible = false;
            this.error.InnerText = "";
            alerta.Visible = false;
            alerta.InnerText = "";
            cargarServicios();
        }

        /// <summary>
        /// Código que llama al método de guardado de información de la opción
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(txtOpcionNombre.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }


                if (string.IsNullOrEmpty(txtTextoIntro.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (string.IsNullOrEmpty(txtFormASP.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (string.IsNullOrEmpty(txtTitulo.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                if (ddlServicio.SelectedValue == "0")
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                OpcionesServicio os = new OpcionesServicio();
                os.nombreOpcion = txtOpcionNombre.Text.Trim();
                os.estado = ddlEstado.SelectedValue;
                os.idServicio = int.Parse(ddlServicio.SelectedValue);
                os.titulo = txtTitulo.Text.Trim();
                os.descripcion = txtTextoIntro.Text.Trim();
                os.formulario = txtFormASP.Text.Trim();
                os.idOpcion = string.IsNullOrEmpty(idOpcion.Value.ToString().Trim()) ? 0 : int.Parse(idOpcion.Value.ToString().Trim());

                Seguridad seguridad = new Seguridad();
                string resultado = seguridad.GuardarModificarOpcionesServicio(os,sUser.id, sUser.loginname);
                alerta.Visible = false;
                alerta.InnerText = "";
                if (resultado == "ok")
                {
                    limpiar();
                    Utils.mostrarMensajeRedireccionando(this.Page, "Datos de la opción guardados correctamente.", "../seguridad/opcion");

                }
                else
                {
                    Utils.mostrarMensaje(this.Page, resultado);
                }
            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante el guardado de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "opciones", "guardar_opciones", txtOpcionNombre.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }

        /// <summary>
        /// Código que llama al método de busqueda de opciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            cargarResultados(txtNombreOpcionBuscar.Text.Trim(), txtServicioBuscar.Text.Trim(), ddlEstadoBuscar.SelectedValue);
        }

        /// <summary>
        /// Código que carga en el grid el resultado de la consulta de opciones
        /// </summary>
        /// <param name="nombreOpcion">Nombre de la opción</param>
        /// <param name="nombreServicio">Nombre del servicio asociado a una opción</param>
        /// <param name="estado">Estado de la opción</param>
        public void cargarResultados(string nombreOpcion, string nombreServicio, string estado)
        {
            try
            {
                List<OpcionesServicio> opciones = new List<OpcionesServicio>();
                Seguridad seguridad = new Seguridad();
                opciones = seguridad.consultarOpciones(nombreOpcion.Trim(), nombreServicio.Trim(), estado.Trim());
                if (opciones != null)
                {
                    if (opciones.Count > 0)
                    {

                        xfinder.Visible = true;
                        sinresultado.InnerText = "";
                        sinresultado.Visible = false;
                        tablePagination.DataSource = opciones;
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
                this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "opciones", "consultar_opciones", txtOpcionNombre.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }

        /// <summary>
        /// Código que deshabilita el combo de servicio cuando se quiere modificar una opción
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btDeshabilitar_Click(object sender, EventArgs e)
        {

            ddlServicio.Enabled = false;
            ddlServicio.DataBind();

        }



    }
}