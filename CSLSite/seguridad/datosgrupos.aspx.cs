using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class datosgrupos : System.Web.UI.Page
    {
        public static usuario sUser = null;

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
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "datosgrupos", "Page_Init", "No autenticado", "No disponible");
                Response.Redirect("../csl/menudefault", false);
                return;
            }
            Page.SslOn();
        }


        /// <summary>
        /// Código que se ejecuta al cargar la página
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
        /// Código que llama al método de limpiar()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        /// <summary>
        /// Método que limpia los controles de la página
        /// </summary>
        private void limpiar()
        {
            txtDescripcion.Text = "";
            txtIdGrupo.Text = "";
            ddlEstado.SelectedValue = "A";
            this.error.Visible = false;
            this.error.InnerText = "";
            alerta.Visible = false;
            alerta.InnerText = "";
        }

        /// <summary>
        /// Método que llama al método de guardado de los grupos
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

                Grupo g = new Grupo();
                g.descripcion = txtDescripcion.Text.Trim();
                g.estado = ddlEstado.SelectedValue;
                g.codigo = string.IsNullOrEmpty(txtIdGrupo.Text.Trim()) ? 0 : int.Parse(txtIdGrupo.Text.Trim());
                Seguridad seguridad = new Seguridad();
                string resultado = seguridad.GuardarModificarGrupo(g, sUser.id, sUser.loginname);
                alerta.Visible = false;
                alerta.InnerText = "";
                if (resultado == "ok")
                {
                    limpiar();

                    Utils.mostrarMensajeRedireccionando(this.Page, "Datos del rol guardados correctamente.", "../csl/menudefault");
                    
                }
                else
                {
                    Utils.mostrarMensaje(this.Page, resultado);
                }
            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante el guardado de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "grupo", "guardar_grupo", txtDescripcion.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }

        /// <summary>
        /// Método que llama al método de búsqueda de los grupos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            cargarResultados(txtDescripcionBuscar.Text.Trim(), ddlEstadoBuscar.SelectedValue);
        }

        /// <summary>
        /// Método que carga el grid con los resultados de la búsqueda 
        /// </summary>
        /// <param name="descripcion">Nombre del grupo</param>
        /// <param name="estado">Estado del grupo</param>
        public void cargarResultados(string descripcion, string estado)
        {
            try
            {
                List<Grupo> grupos = new List<Grupo>();
                Seguridad seguridad = new Seguridad();
                grupos = seguridad.consultarGrupo(descripcion.Trim(), estado.Trim());
                if (grupos != null)
                {
                    if (grupos.Count > 0)
                    {

                        xfinder.Visible = true;
                        sinresultado.InnerText = "";
                        sinresultado.Visible = false;
                        tablePagination.DataSource = grupos;
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
                this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "grupo", "consultar_grupo", txtDescripcion.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());

            }
        }


    }
}
