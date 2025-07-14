using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

namespace CSLSite
{
    public partial class datosarea : System.Web.UI.Page
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
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "datosarea", "Page_Init", "No autenticado", "No disponible");
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
                if (idServicio.Value.ToString() == "")
                    cargarServicios();
                cargarResultados(txtNombreAreaBuscar.Text.Trim(), txtServicioBuscar.Text.Trim(), ddlEstadoBuscar.SelectedValue);
            }
            else
            {
                if (idServicio.Value.ToString() != "")
                {
                    ddlServicio.Enabled = false;
                    imgIconoArea.ImageUrl = hdfImagen.Value.ToString();
                }
            }
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
        /// Código que carga el combo de los servicios
        /// </summary>
        private void cargarServicios()
        {
            if (idServicio.Value.ToString() == "")
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
            txtNombreArea.Text = "";
            txtTituloArea.Text = "";
            imgIconoArea.ImageUrl = "";
            ddlEstado.SelectedValue = "A";
            idServicio.Value = "";
            ddlEstado.Enabled = true;
            this.error.Visible = false;
            this.error.InnerText = "";
            cbxAdministrativo.Checked = false;
            alerta.Visible = false;
            alerta.InnerText = "";
            cargarServicios();
        }

        /// <summary>
        /// Código que guarda la información del área a ingresar o modificar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            try
            {
                Seguridad seguridad = new Seguridad();
                if (string.IsNullOrEmpty(txtNombreArea.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }


                if (string.IsNullOrEmpty(txtTituloArea.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }

                /*if (string.IsNullOrEmpty(txtIcono.Text.Trim()))
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }*/



                if (ddlServicio.SelectedValue == "0")
                {
                    alerta.Visible = true;
                    alerta.InnerText = "Por favor llene los campos obligatorios.";
                    return;
                }


                string fileExt = string.Empty;
                if (string.IsNullOrEmpty(idServicio.Value.ToString()))
                {
                    if (fuIcono.HasFile)
                    {
                        fileExt = System.IO.Path.GetExtension(fuIcono.FileName);

                        if (fileExt == ".jpg" || fileExt == ".gif" || fileExt == ".png")
                        {

                        }
                        else
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Solo se permiten imágenes de extensión .jpg, .gif o .png";
                            return;
                        }
                    }
                    else
                    {
                        alerta.Visible = true;
                        alerta.InnerText = "No ha especificado ninguna imagen para el ícono del área.";
                        return;
                    }


                    using (MemoryStream str = new MemoryStream(fuIcono.FileBytes))
                    {
                        System.Drawing.Image bmp = System.Drawing.Image.FromStream(str);
                        if (bmp.Width > int.Parse(ConfigurationManager.AppSettings["anchoImagenIcono"]) || bmp.Height > int.Parse(ConfigurationManager.AppSettings["altoImagenIcono"]))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "El ícono del área debe ser de las siguientes dimensiones: " + ConfigurationManager.AppSettings["anchoImagenIcono"].ToString() + "px de ancho y " + ConfigurationManager.AppSettings["altoImagenIcono"] + "px de alto.";
                            return;
                        }
                    }
                }
                else
                {
                    if (fuIcono.HasFile)
                    {
                        fileExt = System.IO.Path.GetExtension(fuIcono.FileName);

                        if (fileExt == ".jpg" || fileExt == ".gif" || fileExt == ".png")
                        {

                        }
                        else
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Solo se permiten imágenes de extensión .jpg, .gif o .png";
                            return;
                        }

                       
                        using( MemoryStream str = new MemoryStream(fuIcono.FileBytes))
                        {
                            System.Drawing.Image bmp = System.Drawing.Image.FromStream(str);
                            if (bmp.Width > int.Parse(ConfigurationManager.AppSettings["anchoImagenIcono"]) || bmp.Height > int.Parse(ConfigurationManager.AppSettings["altoImagenIcono"]))
                            {
                                alerta.Visible = true;
                                alerta.InnerText = "El ícono del área debe ser de las siguientes dimensiones: " + ConfigurationManager.AppSettings["anchoImagenIcono"].ToString() + "px de ancho y " + ConfigurationManager.AppSettings["altoImagenIcono"] + "px de alto.";
                                return;
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(idServicio.Value.ToString()))
                {
                    int contadorAreas = seguridad.consultarAreaPorIdServicio(int.Parse(ddlServicio.SelectedValue));

                    if (contadorAreas != -1)
                    {
                        if (contadorAreas > 0)
                        {
                             this.error.Visible = true;
                             this.error.InnerText  = "Ya existe un área asociado a ese servicio, no se puede asociar más de un área a un servicio.";
                            return;
                        }
                    }
                }


                Area a = new Area();
                a.nombreArea = txtNombreArea.Text.Trim();
                a.estado = ddlEstado.SelectedValue;
                a.titulo = txtTituloArea.Text.Trim();
                //a.icono = txtIcono.Text.Trim();
                a.areaAdministrativa = cbxAdministrativo.Checked;
                a.idServicio = string.IsNullOrEmpty(idServicio.Value.ToString()) ? int.Parse(ddlServicio.SelectedValue) : int.Parse(idServicio.Value.ToString());


                if (fuIcono.HasFile)
                {
                    string nombreArchivo = "ic_" + a.nombreArea + fileExt;
                    string rutaGuardado = ConfigurationManager.AppSettings["rutaArchivoIcono"] + "\\" + nombreArchivo;
                    if (File.Exists(rutaGuardado))
                        File.Delete(rutaGuardado);
                    fuIcono.SaveAs(rutaGuardado);
                    a.icono = "../shared/imgs/" + nombreArchivo;
                }
                else
                {
                    a.icono = "0";
                }
                string resultado = seguridad.GuardarModificarArea(a, sUser.id, sUser.loginname);
                alerta.Visible = false;
                alerta.InnerText = "";
                if (resultado == "ok")
                {
                    limpiar();
                    Utils.mostrarMensajeRedireccionando(this.Page, "Datos del área guardados correctamente.", "../csl/menudefault");

                }
                else
                {
                    Utils.mostrarMensaje(this.Page, resultado);
                }
            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante el guardado de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "area", "guardar_area", txtNombreArea.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }


        /// <summary>
        /// Código que realiza la búsqueda del área
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            cargarResultados(txtNombreAreaBuscar.Text.Trim(), txtServicioBuscar.Text.Trim(), ddlEstadoBuscar.SelectedValue);
        }


        /// <summary>
        /// Código que realiza la carga de los resultados en el grid según los filtros de búsqueda
        /// </summary>
        /// <param name="nombreOpcion">Nombre del área</param>
        /// <param name="nombreServicio">Nombre del servicio relacionado al área</param>
        /// <param name="estado">Estado del área</param>
        public void cargarResultados(string nombreOpcion, string nombreServicio, string estado)
        {
            try
            {
                List<Area> areas = new List<Area>();
                Seguridad seguridad = new Seguridad();
                areas = seguridad.consultarAreas(nombreOpcion.Trim(), nombreServicio.Trim(), estado.Trim());
                if (areas != null)
                {
                    if (areas.Count > 0)
                    {

                        xfinder.Visible = true;
                        sinresultado.InnerText = "";
                        sinresultado.Visible = false;
                        tablePagination.DataSource = areas;
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
                this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "areas", "consultar_areas", txtNombreArea.Text.Trim(), sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }


        /// <summary>
        /// Código que deshabilita el combo de servicio cuando se va a modificar un área
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