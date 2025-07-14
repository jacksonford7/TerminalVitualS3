using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class datosservicio : System.Web.UI.Page
    {
        public static usuario sUser = null;

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
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "datosservicio", "Page_Init", "No autenticado", "No disponible");
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
            if (!IsPostBack)
            {
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                xfinder.Visible = true;
                alerta.Visible = false;
                cargarResultados(txtDescripcionBuscar.Text.Trim(), ddlEstadoBuscar.SelectedValue);
            }
        }

        /// <summary>
        /// Código que hace el llamado del método limpiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        /// <summary>
        /// Código que limpia los controles de la página
        /// </summary>
        private void limpiar()
        {
            txtDescripcion.Text = "";
            txtIdServicio.Text = "";
            ddlEstado.SelectedValue = "A";
            this.error.Visible = false;
            this.error.InnerText = "";
            alerta.Visible = false;
            alerta.InnerText = "";
        }

        /// <summary>
        /// Código que realiza el guardado de los servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(txtDescripcion.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }


                Servicio s = new Servicio();
                s.descripcion = txtDescripcion.Text.Trim();
                s.estado = ddlEstado.SelectedValue;
                s.codigo = string.IsNullOrEmpty(txtIdServicio.Text.Trim()) ? 0 : int.Parse(txtIdServicio.Text.Trim());
                Seguridad seguridad = new Seguridad();
                string resultado = seguridad.GuardarModificarServicio(s, sUser.id, sUser.loginname);
                alerta.Visible = false;
                alerta.InnerText = "";
                if (resultado == "ok")
                {
                    limpiar();
                    Utils.mostrarMensajeRedireccionando(this.Page, "Datos del servicio guardados correctamente.", "../csl/menudefault");

                }
                else
                {
                    Utils.mostrarMensaje(this.Page, resultado);
                }
            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante el guardado de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "servicio", "guardar_servicio", txtDescripcion.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }

        /// <summary>
        /// Código que realiza la búsqueda de los servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            cargarResultados(txtDescripcionBuscar.Text.Trim(), ddlEstadoBuscar.SelectedValue);
        }

        /// <summary>
        /// Código que carga los resultados de la consulta en el grid
        /// </summary>
        /// <param name="descripcion">Nombre del servicio</param>
        /// <param name="estado">Estado del servicio</param>
        public void cargarResultados(string descripcion, string estado)
        {
            try
            {
                List<Servicio> servicios = new List<Servicio>();
                Seguridad seguridad = new Seguridad();
                servicios = seguridad.consultarServicio(descripcion.Trim(), estado.Trim());
                if (servicios != null)
                {
                    if (servicios.Count > 0)
                    {

                        xfinder.Visible = true;
                        sinresultado.InnerText = "";
                        sinresultado.Visible = false;
                        tablePagination.DataSource = servicios;
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
                this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "servicio", "consultar_servicio", txtDescripcion.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }


    }
}