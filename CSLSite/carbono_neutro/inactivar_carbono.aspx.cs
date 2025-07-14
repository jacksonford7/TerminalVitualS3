using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ClsAppCgsa;
using System.Globalization;
using BillionEntidades;

namespace CSLSite
{
    public partial class inactivar_carbono : System.Web.UI.Page
    {

        public static usuario sUser = null;
        public static UsuarioSeguridad us;
        private Cls_ImpoContenedor obj = new Cls_ImpoContenedor();
        private Cls_EnviarMailAppCgsa objMail = new Cls_EnviarMailAppCgsa();

        #region "Variables"

        private static Int64? lm = -3;
        private string OError;

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        //private void Carga_Paquetes()
        //{
        //    try
        //    {
               

        //        List<MantenimientoPaquetes> Listado = MantenimientoPaquetes.Combo_Paquetes(out OError);

        //        if (Listado != null)
        //        {
        //            this.CboPaquete.DataSource = Listado;
        //            this.CboPaquete.DataTextField = "Name";
        //            this.CboPaquete.DataValueField = "Id";
        //            this.CboPaquete.DataBind();

        //        }
        //        else
        //        {
        //            this.CboPaquete.DataSource = null;
        //            this.CboPaquete.DataBind();
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Paquetes), "Carga_Paquetes", false, null, null, ex.StackTrace, ex);
        //        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

        //        this.error.Visible = true;
        //        this.error.InnerText = string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError);
        //        Utils.mostrarMensaje(this.Page, ex.Message.ToString());

               

        //    }

        //}

        //protected void CboEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        if (this.CboEmpresa.SelectedIndex != -1)
        //        {

        //            int Id = 0;

        //            if (!int.TryParse(this.CboEmpresa.SelectedValue.ToString(), out Id))
        //            {
        //                Id = 0;
        //            }

        //            this.BuscaEmpresa_ConPaquetes(Id);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.error.Visible = true;
        //        this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "CboEmpresa_SelectedIndexChanged", txtUsuario.Text.Trim(), sUser.loginname));
        //        Utils.mostrarMensaje(this.Page, ex.Message.ToString());

        //    }

        //}


        private void BuscaEmpresa_ConPaquetes(int IdUsuario)
        {
            
            
            try
            {
                List<UsuarioSeguridad> usuarios = new List<UsuarioSeguridad>();
                Seguridad seguridad = new Seguridad();
                usuarios = seguridad.consultarUsuariosUnicoApp(IdUsuario);
                if (usuarios != null)
                {
                    if (usuarios.Count > 0)
                    {
                        string Ruc = string.Empty;
                        foreach (var Det in usuarios)
                        {
                            Ruc = Det.codigoEmpresa.Trim();
                        }

                        //busco si la empresa ya tiene algun paquete asignado
                        List<MantenimientoPaqueteCliente> Listado = MantenimientoPaqueteCliente.Listado_clientes(Ruc, out OError);
                        if (Listado != null)
                        {
                            foreach (var det in Listado)
                            {
                                //this.CboPaquete.SelectedValue = det.PackageId.ToString().Trim();
                            }
                        }

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
                this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "BuscaEmpresa_ConPaquetes", sUser.loginname, sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());
            }
        }

        protected void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
          
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }

                    this.Carga_Paquetes_Clientes();


                }
                catch (Exception ex)
                {
                    this.error.Visible = true;
                    this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "buscar", "BtnBuscarCliente_Click", sUser.loginname, sUser.loginname));
                    Utils.mostrarMensaje(this.Page, ex.Message.ToString());

                   

                }
            }
        }

        private void Carga_Paquetes_Clientes()
        {
            try
            {

                string criterio = this.TxtBusCliente.Text.Trim();
                System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");

                if (string.IsNullOrEmpty(criterio))
                {
                    criterio = null;
                }

                DateTime desde;
                DateTime hasta;

                DateTime? pdesde;
                DateTime? phasta;

                if (!DateTime.TryParseExact(this.TxtFechaDesde.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                {
                    pdesde = null;
                }
                else
                {
                    pdesde = desde;
                }
                if (!DateTime.TryParseExact(this.TxtFechaHasta.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                {
                    phasta = null;
                }
                else
                {
                    phasta = hasta;
                }

                if (pdesde.HasValue && phasta.HasValue)
                {
                    if (pdesde.Value.Year != phasta.Value.Year)
                    {
                        xfinder.Visible = false;
                        this.sinresultado.InnerText = "El rango máximo de consulta es de 1 año, gracias por entender.";
                        sinresultado.Visible = true;
                        return;
                    }
                }

                List<Cls_ImpoContenedor> Listado = Cls_ImpoContenedor.Listado_Paquetes_Clientes_filtro(pdesde, phasta, criterio, out OError);
                if (Listado != null)
                {
                    xfinder.Visible = true;
                    tablePagination.DataSource = Listado;
                    tablePagination.DataBind();


                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                }

                ///this.upresult.Update();

            }
            catch (Exception ex)
            {
                this.error.Visible = true;
                this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "Carga_Paquetes_Clientes", sUser.loginname, sUser.loginname));
                Utils.mostrarMensaje(this.Page, ex.Message.ToString());

            }

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
                Response.Redirect("../cuenta/menu.aspx", false);
                return;
            }
            Page.SslOn();

            this.TxtFechaDesde.Text = Server.HtmlEncode(this.TxtFechaDesde.Text);
            this.TxtFechaHasta.Text = Server.HtmlEncode(this.TxtFechaHasta.Text);
            this.TxtBusCliente.Text = Server.HtmlEncode(this.TxtBusCliente.Text);
            this.TxtRuta1.Text = Server.HtmlEncode(this.TxtRuta1.Text);
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
                //cargarTipoUsuarios();
                //this.Carga_Paquetes();

                this.Carga_Paquetes_Clientes();
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
            Response.Redirect("../admin/datosusuario.aspx");
        }


        /// <summary>
        /// Carga el combo de tipo de usuario
        /// </summary>
        //private void cargarTipoUsuarios()
        //{


        //    HashSet<Tuple<string, string>> tipoUsuariosTemp = new HashSet<Tuple<string, string>>();
        //    tipoUsuariosTemp = Seguridad.getDetalleCatalogosSeguridadApp(ConfigurationManager.AppSettings["catalogoTipoUsuarios"]);
        //    HashSet<Tuple<string, string>> tipoUsuarios = new HashSet<Tuple<string, string>>();
        //    if (us.tipoUsuario != ConfigurationManager.AppSettings["tipoAdministradorInterno"])
        //    {
        //        foreach (Tuple<string, string> item in tipoUsuariosTemp)
        //        {
        //            if (item.Item1 != ConfigurationManager.AppSettings["tipoAdministradorInterno"] && item.Item1 != ConfigurationManager.AppSettings["tipoAdministradorEmpresa"])
        //            {
        //                tipoUsuarios.Add(item);
        //            }
        //            else
        //            {
        //                if (us.codigoEmpresa == ConfigurationManager.AppSettings["RUC_CGSA"].ToString() || us.codigoEmpresa == ConfigurationManager.AppSettings["INI_CGSA"].ToString())
        //                {
        //                    tipoUsuarios.Add(item);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ddlTipoUsuario.Items.Add(new ListItem("TODOS", "0"));
        //        tipoUsuarios = tipoUsuariosTemp;
        //    }

        //    ddlTipoUsuario.DataSource = tipoUsuarios;
        //    ddlTipoUsuario.DataValueField = "item1";
        //    ddlTipoUsuario.DataTextField = "item2";
        //    ddlTipoUsuario.DataBind();
        //}

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Session["usuarioId"] = null;
            Response.Redirect("../appcgsa/appPaquetesClientes.aspx");
        }

        //protected void BtnGrabar_Click(object sender, EventArgs e)
        //{

        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {

        //            if (HttpContext.Current.Request.Cookies["token"] == null)
        //            {
        //                System.Web.Security.FormsAuthentication.SignOut();
        //                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //                Session.Clear();
        //                return;
        //            }

        //            //if (this.CboPaquete.SelectedIndex == -1)
        //            //{
        //            //    alerta.Visible = true;
        //            //    alerta.InnerText = "Por favor seleccione el paquete a relacionar con el cliente";
        //            //    this.CboPaquete.Focus();
        //            //    return;
        //            //}

        //            //if (this.CboPaquete.SelectedIndex == 0)
        //            //{
        //            //    alerta.Visible = true;
        //            //    alerta.InnerText = "Por favor seleccione el paquete a relacionar con el cliente";
        //            //    this.CboPaquete.Focus();
        //            //    return;
        //            //}

        //            //if (this.CboEmpresa.SelectedIndex == -1)
        //            //{
        //            //    alerta.Visible = true;
        //            //    alerta.InnerText = "Por favor seleccione el cliente a realizar la asignación de un paquete";
        //            //    this.CboEmpresa.Focus();
        //            //    return;
        //            //}

        //            if (string.IsNullOrEmpty(this.TxtRuta1.Text))
        //            {
        //                alerta.Visible = true;
        //                alerta.InnerText = "Por favor debe cargar un archivo pdf de autorización de registro";
        //                this.TxtRuta1.Focus();
        //                return;
        //            }

        //            if (string.IsNullOrEmpty(this.ruta_completa.Value))
        //            {
        //                alerta.Visible = true;
        //                alerta.InnerText = "Por favor debe cargar un archivo pdf de autorización de registro";
        //                this.TxtRuta1.Focus();
        //                return;
        //            }

        //            //busca contenedores por ruc de usuario
        //            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

        //            Int64 Id = 0;


        //            //if (!Int64.TryParse(this.CboPaquete.SelectedValue.ToString(), out Id))
        //            //{
        //            //    Id = 0;
        //            //}

        //            //int IdUsuario = 0;
        //            //if (!int.TryParse(this.CboEmpresa.SelectedValue.ToString(), out IdUsuario))
        //            //{
        //            //    IdUsuario = 0;
        //            //}

        //            string Ruc = string.Empty;
        //            string Empresa = string.Empty;
        //            string Email = string.Empty;
        //            List<UsuarioSeguridad> usuarios = new List<UsuarioSeguridad>();
        //            Seguridad seguridad = new Seguridad();
        //            usuarios = seguridad.consultarUsuariosUnicoApp(IdUsuario);
        //            if (usuarios != null)
        //            {
        //                if (usuarios.Count > 0)
        //                {
                            
        //                    foreach (var Det in usuarios)
        //                    {
        //                        Ruc = Det.codigoEmpresa.Trim();
        //                        Empresa=Det.nombreEmpresa.Trim();
        //                        Email = string.IsNullOrEmpty(Det.usuarioCorreo) ? Det.correoEmpresa : Det.usuarioCorreo;
        //                    }
        //                }
        //                else
        //                {
        //                    Ruc = string.Empty;
        //                    Empresa = string.Empty;
        //                    Email = string.Empty;
        //                }
        //            }

        //            if (string.IsNullOrEmpty(Ruc))
        //            {
        //                alerta.Visible = true;
        //                alerta.InnerText = "Por favor seleccione el cliente/ruc a realizar la asignación de un paquete";
        //               // this.CboEmpresa.Focus();
        //                return;
        //            }


        //            obj = new MantenimientoPaqueteCliente();
        //            obj.PackageId = Id;
        //            obj.ClientId = Ruc;
        //            obj.Create_user = ClsUsuario.loginname.Length > 10 ? ClsUsuario.loginname.Substring(0, 10) : ClsUsuario.loginname;
        //            obj.Client = Empresa;
        //            obj.file_pdf = this.ruta_completa.Value;
        //            obj.Email = Email;

        //            var nIdRegistro = obj.Save(out OError);

        //            if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
        //            {
        //                alerta.Visible = true;
        //                alerta.InnerText = "Error! No se pudo grabar cliente";
                        
        //                return;
        //            }
        //            else
        //            {
        //                string Paquete = string.Empty;
        //                var Existe = this.CboPaquete.Items.FindByValue(this.CboPaquete.SelectedValue.ToString());
        //                if (Existe != null)
        //                {
        //                    Paquete = Existe.ToString();
        //                }
        //                alerta.Visible = true;
        //                alerta.InnerText = string.Format("Se procedió a realizar la asignación del paquete:{0} al cliente:{1}, ", Paquete, Empresa);

        //                //sp enviar correo
        //                objMail.IdCliente = IdUsuario;
        //                string error;
        //                var nProceso = objMail.SaveTransaction(out error);
        //                /*fin de nuevo proceso de grabado*/
        //                if (!nProceso.HasValue || nProceso.Value <= 0)
        //                {

        //                }


        //                this.txtUsuario.Text = string.Empty;
        //                this.txtNombres.Text = string.Empty;
        //                this.txtIdentificacion.Text = string.Empty;
        //                this.txtNombreEmpresa.Text = string.Empty;
        //                this.TxtRuta1.Text = string.Empty;
        //                this.ruta_completa.Value = string.Empty;

        //                this.CboEmpresa.Items.Clear();
        //                this.CboEmpresa.DataSource = null;
        //                this.CboEmpresa.DataBind();

        //                this.Carga_Paquetes_Clientes();
        //            }



        //        }
        //        catch (Exception ex)
        //        {
        //            this.error.Visible = true;
        //            this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "BtnGrabar_Click", txtUsuario.Text.Trim(), sUser.loginname));
        //            Utils.mostrarMensaje(this.Page, ex.Message.ToString());

        //        }
        //    }

        //}

    
        //private void cargarResultados(string usuario, string nombreUsuario, string identificacionUsuario, string nombreEmpresa, string estado, string tipoUsuario)
        //{
        //    string identificacionEmpresa = string.Empty;
        //    if (us.tipoUsuario == ConfigurationManager.AppSettings["tipoAdministradorEmpresa"])
        //    {
        //        identificacionEmpresa = us.codigoEmpresa;
        //        //identificacionEmpresa = identificacionUsuario;
        //    }
        //    else
        //    {
        //        identificacionEmpresa = "0";
        //    }

        //    try
        //    {
        //        List<UsuarioSeguridad> usuarios = new List<UsuarioSeguridad>();
        //        Seguridad seguridad = new Seguridad();
        //        usuarios = seguridad.consultarUsuariosApp(usuario, nombreUsuario, identificacionUsuario, nombreEmpresa, estado, tipoUsuario, identificacionEmpresa);
        //        if (usuarios != null)
        //        {
        //            if (usuarios.Count > 0)
        //            {
        //                xfinder.Visible = true;
        //                sinresultado.InnerText = "";
        //                sinresultado.Visible = false;
        //                //tablePagination.DataSource = usuarios.OrderBy(x => x.usuario);
        //                //tablePagination.DataBind();

        //                this.CboEmpresa.Items.Clear();
        //                this.CboEmpresa.DataSource = usuarios;
        //                this.CboEmpresa.DataTextField = "nombreEmpresa";
        //                this.CboEmpresa.DataValueField = "IdUsuario";
        //                this.CboEmpresa.DataBind();

        //                if (this.CboEmpresa.SelectedIndex != -1)
        //                {

        //                    int Id = 0;

        //                    if (!int.TryParse(this.CboEmpresa.SelectedValue.ToString(), out Id))
        //                    {
        //                        Id = 0;
        //                    }

        //                    this.BuscaEmpresa_ConPaquetes(Id);
        //                }

        //            }
        //            else
        //            {
        //                xfinder.Visible = false;
        //                sinresultado.Visible = true;
        //                sinresultado.InnerText = "No hay información relacionada según los criterios de búsqueda establecidos";
        //                this.Carga_Paquetes_Clientes();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.error.Visible = true;
        //        this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "consultar_usuarios", txtUsuario.Text.Trim(), sUser.loginname));
        //        Utils.mostrarMensaje(this.Page, ex.Message.ToString());
        //    }
        //}

        /// <summary>
        /// Código que realiza la búsqueda de los usuarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btbuscar_Click(object sender, EventArgs e)
        //{
        //    error.InnerText = "";
        //    error.Visible = false;

        //    if (us.codigoEmpresa == ConfigurationManager.AppSettings["RUC_CGSA"].ToString() || us.codigoEmpresa == ConfigurationManager.AppSettings["INI_CGSA"].ToString())
        //    {
        //        if  (string.IsNullOrEmpty(txtNombreEmpresa.Text) && string.IsNullOrEmpty(txtUsuario.Text) && string.IsNullOrEmpty(txtNombres.Text) && string.IsNullOrEmpty(txtIdentificacion.Text))
        //        {
        //            alerta.Visible = true;
        //            alerta.InnerText = "Por favor llene por lo menos la empresa para poder realizar la búsqueda.";
        //            return;
        //        }
                        
        //    }


        //    if (!string.IsNullOrEmpty(txtUsuario.Text.Trim()) || !string.IsNullOrEmpty(txtNombres.Text.Trim()) ||
        //        !string.IsNullOrEmpty(txtIdentificacion.Text.Trim()) || !string.IsNullOrEmpty(txtNombreEmpresa.Text.Trim()) ||
        //        ddlEstado.SelectedValue != "0" || ddlTipoUsuario.SelectedValue != "0"
        //        )
        //    {
        //        cargarResultados(txtUsuario.Text.Trim(), txtNombres.Text.Trim(), txtIdentificacion.Text.Trim(), txtNombreEmpresa.Text.Trim(), ddlEstado.SelectedValue, ddlTipoUsuario.SelectedValue);
        //        alerta.Visible = false;
        //        alerta.InnerText = "";

        //        //busca si la empresa tienen un paquete ingresado

        //    }
        //    else
        //    {
        //        alerta.Visible = true;
        //        alerta.InnerText = "Por favor llene por lo menos un criterio para poder realizar la búsqueda.";
        //    }
        //}


        /// <summary>
        /// Código que setea la variable de sesión para la modificación del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btSetearId_Click(object sender, EventArgs e)
        //{
        //    Session["usuarioId"] = int.Parse(hdfIdUsuario.Value.ToString());
        //    //Response.Redirect("../seguridad/datosusuario");

        //}

        /// <summary>
        /// Código que realiza el reseteo de la clave de un determinado usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btResetear_Click(object sender, EventArgs e)
        //{
        //    int idUsuario = int.Parse(hdfIdUsuario.Value.ToString());
        //    string username = hdfUserName.Value.ToString();
        //    Seguridad s = new Seguridad();

        //    UsuarioSeguridad usRecuperacion = s.consultarUsuarioPorId(idUsuario);
        //    if (usRecuperacion != null)
        //    {
        //        try
        //        {

        //            if (string.IsNullOrEmpty(usRecuperacion.usuarioCorreo.Trim()))
        //            {
        //                alerta.Visible = true;
        //                alerta.InnerText = "El usuario no posee una cuenta de correo configurada, no se puede proseguir con el proceso de recuperación de contraseña.";
        //                return;
        //            }

        //            string passwordPlano = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10);
        //            string password = string.Empty;
        //            Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
        //            if (componente == null)
        //            {
        //                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El componente AISVUtils.GSS ha fallado"), "usuario", "recuperacion_contraseña", usRecuperacion.usuario, usRecuperacion.usuario);
        //                alerta.Visible = true;
        //                alerta.InnerText = "Hubo un problema durante la recuperación de la contraseña";
        //                return;
        //            }
        //            else
        //            {
        //                dynamic instancia = Activator.CreateInstance(componente);
        //                var dcpass = instancia.Encrypt(usRecuperacion.usuario, passwordPlano) as string;
        //                if (string.IsNullOrEmpty(dcpass))
        //                {
        //                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado"), "usuario", "recuperacion_contraseña", usRecuperacion.usuario, usRecuperacion.usuario);
        //                    alerta.Visible = true;
        //                    alerta.InnerText = "Hubo un problema durante el cambio de contraseña.";
        //                    return;

        //                }
        //                password = dcpass;
        //                instancia = null;
        //                componente = null;
        //            }



        //            string resultado = s.CambioPasswordUsuario(password, usRecuperacion.idUsuario, "S", "RC", sUser.id, sUser.loginname);
        //            alerta.Visible = false;
        //            alerta.InnerText = "";
        //            if (resultado == "ok")
        //            {

        //                var jmsg = new jMessage();
        //                string mail = string.Empty;
        //                string destinatarios = turnoConsolidacion.GetMails();

        //                /*nuevo 25-06-2019*/
        //                string fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        //                string firmaNo = "<p>Es un placer servirle,</p><p>Atentamente,<br/><strong>Terminal Virtual</strong><br/>Contecon Guayaquil S.A. CGSA<br/>An ICTSI Group Company.</p><p><i>El contenido de este mensaje es informativo, por favor no responda este correo.</i></p>";

        //                /*fin*/

        //                mail = string.Concat(mail, string.Format("<h4>Estimado Cliente:</h4>"));
        //                mail = string.Concat(mail, string.Format("<p>Se ha recibido su solicitud para realizar la recuperación de la contraseña del usuario {0} en la siguiente fecha y hora {1}.</p>", usRecuperacion.usuario, fecha));
        //                mail = string.Concat(mail, string.Format("<p>La contraseña que deberá utilizar para acceder a nuestro Sistema de Terminal Virtual es:  {0}</p>", passwordPlano));
        //                mail = string.Concat(mail, string.Format("<p>Por su seguridad al iniciar sesión, automáticamente se le solicitará el cambio de contraseña.</p>"));
        //                mail = string.Concat(mail, string.Format(firmaNo));
                    

        //                /*mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/>Este es un mensaje del Centro de Servicios en Linea de Contecon S.A, para comunicarle lo siguiente.<br/>Se ha recibido su solicitud para realizar la recuperación de la contraseña del usuario <strong> {2} </strong> en la siguiente fecha y hora {3}<br/>", usRecuperacion.nombreUsuario, usRecuperacion.apellidoUsuario, usRecuperacion.usuario, DateTime.Now.ToString()));
        //                mail = string.Concat(mail, string.Format("La contraseña que deberá usar para iniciar sesión es: <strong> {0}</strong><br/> </br> Por su seguridad al momento de iniciar sesión, automáticamente se le solicitará el cambio de contraseña. Muchas Gracias por su comprensión", passwordPlano));
        //                mail = string.Concat(mail, string.Format("<br/><br/>Es un placer servirle, <br/><br/>"));
        //                mail = string.Concat(mail, string.Format("Atentamente, <br/><br/>"));
        //                mail = string.Concat(mail, string.Format("<strong>S3 - Sistema de Solicitud de Servicios </strong> <br/>"));
        //                mail = string.Concat(mail, string.Format("Contecon Guayaquil S.A. CGSA <br/>"));
        //                mail = string.Concat(mail, string.Format("An ICTSI Group Company <br/><br/>"));*/

        //                string errorMail = string.Empty;
        //                var user_email = usRecuperacion.usuarioCorreo;// usRecuperacion.usuarioCorreo;
        //                var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
        //                destinatarios = user_email+";"+correoBackUp;


        //                //var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";

        //                CLSDataSeguridad.addMail(out errorMail, sUser.email, "Recuperación de contraseña CGSA", mail, destinatarios, usRecuperacion.usuario, "", "");

        //                if (!string.IsNullOrEmpty(errorMail))
        //                {
        //                    alerta.Visible = true;
        //                    alerta.InnerText = errorMail;
        //                    return;

        //                }
        //                else
        //                {
        //                    Utils.mostrarMensaje(this.Page, "Se le enviará un correo al usuario " + usRecuperacion.usuario + " cuyo correo es " + usRecuperacion.usuarioCorreo + " con su nueva contraseña generada.");
        //                }
        //            }
        //            else
        //            {
        //                Utils.mostrarMensaje(this.Page, resultado);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.error.Visible = true;
        //            this.error.InnerText = string.Format("Se produjo un error durante la recuperación de contraseña, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuario", "recuperacion_contraseña", usRecuperacion.usuario, usRecuperacion.usuario));
        //            Utils.mostrarMensaje(this.Page, ex.Message.ToString());
        //        }
        //    }
        //}


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

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (e.CommandArgument == null)
                    {
                        alerta.Visible = true;
                        alerta.InnerText = string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "no existen argumentos");
                      
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        alerta.Visible = true;
                        alerta.InnerText = string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", "argumento null");
                        return;

                    }

                    if (e.CommandName == "Eliminar")
                    {
                        string pId = t.ToString();

                        Int64 Id = 0;

                        if (!Int64.TryParse(pId, out Id))
                        {
                            Id = 0;
                        }

                        TextBox Txtcomentario = e.Item.FindControl("Txtcomentario") as TextBox;
                        Label LblClientId = e.Item.FindControl("LblClientId") as Label;

                        if (string.IsNullOrEmpty(Txtcomentario.Text))
                        {
                            alerta.InnerText = "Debe ingresar un comentario";
                            alerta.Visible = true;
                            return;
                        }

                        if (string.IsNullOrEmpty(this.TxtRuta1.Text))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Por favor debe cargar un archivo pdf de autorización, para desactivar el servicio";
                            this.TxtRuta1.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(this.ruta_completa.Value))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = "Por favor debe cargar un archivo pdf de autorización, para desactivar el servicio";
                            this.TxtRuta1.Focus();
                            return;
                        }


                        obj = new Cls_ImpoContenedor();
                        obj.ID = Id;
                        obj.Comment = Txtcomentario.Text;
                        obj.file_pdf = this.ruta_completa.Value;
                        obj.Create_user = ClsUsuario.loginname;
                        if (obj.Delete(out OError))
                        {
                            alerta.Visible = true;
                            alerta.InnerText = string.Format("Servicio de Carbono Neutro [{0}], inactivado con éxito ", LblClientId.Text);
                            this.TxtRuta1.Text = string.Empty;
                            this.ruta_completa.Value = string.Empty;

                            ////sp enviar correo del servicio inactivo
                            //objMail.Ruc = LblClientId.Text;
                            //string error;
                            //var nProceso = objMail.SaveTransactionCancelar(out error);
                            ///*fin de nuevo proceso de grabado*/
                            //if (!nProceso.HasValue || nProceso.Value <= 0)
                            //{

                            //}

                        }
                        else
                        {
                            alerta.Visible = true;
                            alerta.InnerText = string.Format("Error! No se pudo inactivar el Servicio de Carbono Neutro de la empresa:{0}-{1}", Id, OError);
                        }

                        this.Carga_Paquetes_Clientes();


                    }//fin actualizar

                }
                catch (Exception ex)
                {
                    this.error.Visible = true;
                    this.error.InnerText = string.Format("Se produjo un error durante la eliminación, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "eliminar", "tablePagination_ItemCommand", sUser.loginname, sUser.loginname));

                }
            }

        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {

        }
    }
}