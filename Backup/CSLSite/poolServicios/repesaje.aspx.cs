using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using ConectorN4;
using System.Text;

namespace CSLSite
{
    public partial class repesaje : System.Web.UI.Page
    {
        string estadoIngresado = "IN";

        public List<unidadN4> GridContenedores
        {
            get { return (List<unidadN4>)Session["GridContenedores"]; }
            set { Session["GridContenedores"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../csl/login", true);
                }
                //this.agencia.Value = user.ruc;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sinresultado.Visible = false;
                populateDrop(dptipotrafico, CslHelperServicios.getDetalleCatalogo(ConfigurationManager.AppSettings["TipoTrafico"]));
                if (dptipotrafico.Items.Count > 0)
                {
                    if (dptipotrafico.Items.FindByValue("000") != null)
                    {
                        dptipotrafico.Items.FindByValue("000").Selected = true;
                    }
                    dptipotrafico.SelectedValue = "0";
                }
                populateDrop(dptipocarga, CslHelperServicios.getDetalleCatalogo(ConfigurationManager.AppSettings["TipoCarga"]));
                if (dptipocarga.Items.Count > 0)
                {
                    if (dptipocarga.Items.FindByValue("CO") != null)
                    {
                        dptipocarga.Items.FindByValue("CO").Selected = true;
                        dptipocarga.Enabled = false;
                    }
                    //dptipocarga.SelectedValue = "0";
                }
                populateDrop(dptiposervicios, CslHelperServicios.getServicios());
                if (dptiposervicios.Items.Count > 0)
                {
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("CRJ"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("CIE"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("IMO"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("LA"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("RES"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("RTR"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("SEL"));

                    /*
                        CRJ	Cerrojo Electronico -
                        CIE	Corrección de Ingreso de Exportación -
                        IMO	Etiquetado - Desetiquetado Unidades --este
                        LA	Late Arrival -
                        RES	Reestiba
                        BAS	Repesaje
                        RTR	Revisión Técnica Refrigerada
                        SEL	Verificación de Sellos
                     
                     */
                    if (dptiposervicios.Items.FindByValue("BAS") != null)
                    {
                        dptiposervicios.Items.FindByValue("BAS").Selected = true;
                        dptiposervicios.Enabled = false;
                    }
                    else
                    {
                        dptiposervicios.SelectedValue = "0";
                    }
                }
            }
        }
        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }
        public void btgenerarServer_Click(object sender, EventArgs e)
        {
            int retorno = 0;
            string mensajeErrorPorServicio = "";

            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    string descripcionGrupo = "";
                    string TipoServicio = dptiposervicios.SelectedValue;
                    string descripcionServicio = dptiposervicios.SelectedValue == "0" ? "" : dptiposervicios.Items.FindByValue(dptiposervicios.SelectedValue).Text;
                    string TipoTrafico = dptipotrafico.SelectedValue;
                    string descripcionTrafico = dptipotrafico.SelectedValue == "0" ? "" : dptipotrafico.Items.FindByValue(dptipotrafico.SelectedValue).Text;
                    string TipoCarga = dptipocarga.SelectedValue;
                    string descripcionCarga = dptipocarga.SelectedValue == "0" ? "" : dptipocarga.Items.FindByValue(dptipocarga.SelectedValue).Text;
                    string booking = txtNumBooking.Text.Trim();
                    string numCarga = string.Empty;
                    string a = "";
                    if (!string.IsNullOrEmpty(txtnumcarga1.Text.Trim()) && !string.IsNullOrEmpty(txtnumcarga2.Text.Trim()) && !string.IsNullOrEmpty(txtnumcarga3.Text.Trim()))
                    {
                        numCarga = txtnumcarga1.Text.Trim() + " - " + txtnumcarga2.Text.Trim() + " - " + txtnumcarga3.Text.Trim();
                    }
                    string mensajeContenedor = "";
                    string evento = CslHelperServicios.consultaEventoPorServicio(dptiposervicios.SelectedValue.Trim());
                    List<datosCabecera> datosCab = new List<datosCabecera>();
                    descripcionGrupo = dptiposervicios.SelectedValue.Trim();

                    switch (descripcionServicio)
                    {
                        case "Repesaje":
                            estadoIngresado = "EP";
                            if (TipoTrafico != "EXPRT")
                            {
                                descripcionGrupo = "REP";
                                mensajeErrorPorServicio = "Su contenedor esta en proceso de descarga y pesaje.";
                            }
                            else
                            {
                                mensajeErrorPorServicio = "Su contenedor no ha arribado en la Terminal.";
                            }
                            break;
                        case "Verificación de Sellos":
                            estadoIngresado = "EP";
                            if (TipoTrafico != "EXPRT")
                            {
                                mensajeErrorPorServicio = "Su contenedor esta en proceso de descarga y pesaje.";
                            }
                            else
                            {
                                mensajeErrorPorServicio = "Su contenedor no ha arribado en la Terminal.";
                            }
                            break;
                        case "Revisión Técnica Refrigerada":
                            estadoIngresado = "IN";
                            mensajeErrorPorServicio = "Su contenedor no ha arribado en la Terminal.";
                            break;
                        case "Etiquetado - Desetiquetado Unidades":
                            mensajeErrorPorServicio = "Su contenedor no ha arribado en la Terminal.";
                            break;
                    }

                    //Actualizo el data table en sesion con la página más reciente del grid.
                    actualizarDataTableSession();

                    //obtengo la sesión del usuario logeado
                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //Validación 1 -> valida cada contenedor seleccionado contra N4
                    List<unidadN4> GridContenedoresFiltrado = GridContenedores.Where(x => x.check).ToList();
                    foreach (var item in GridContenedoresFiltrado)
                    {
                        string lblNombreContenedor = string.IsNullOrEmpty(item.cntr) ? string.Empty : item.cntr.Trim().ToUpper();
                        bool chckContenedor = item.check;
                        string mensajeN4 = "";

                        var tk = HttpContext.Current.Request.Cookies["token"];
                        ConectorN4.ObjectSesion sesObj = new ObjectSesion();
                        sesObj.clase = "solicitudUsuarios" + descripcionServicio;
                        sesObj.metodo = "btgenerar_Click";
                        sesObj.transaccion = "generarSolicitud" + descripcionServicio;
                        sesObj.usuario = sUser.loginname;
                        sesObj.token = tk.Value;

                        //Si el check de todos está en TRUE, envio todos los contenedores a verificar con N4
                        if (chkTodos.Checked)
                        {
                            if (descripcionServicio != "Reestiba")
                            {
                                a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"></unit-identity></units><properties><property tag=\"RoutingGroupRef\" value=\"{1}\"/></properties></icu>", lblNombreContenedor, descripcionGrupo);
                                mensajeN4 = Utility.validacionN4(sesObj, a);
                            }
                        }
                        else if (chckContenedor) //Aquí solo envio los contenedores que estén chequeado cuando el check de todos está en FALSE
                        {
                            if (descripcionServicio != "Reestiba")
                            {
                                a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"></unit-identity></units><properties><property tag=\"RoutingGroupRef\" value=\"{1}\"/></properties></icu>", lblNombreContenedor, descripcionGrupo);
                                mensajeN4 = Utility.validacionN4(sesObj, a);
                            }
                        }

                        if (descripcionServicio != "Repesaje" && descripcionServicio != "Verificación de Sellos")
                        {

                            StringBuilder newa = new StringBuilder();
                            newa.Append("<icu><units>");
                            newa.Append(string.Format("<unit-identity id=\"{0}\" type=\"{1}\">", lblNombreContenedor, "CONTAINERIZED"));
                            newa.Append("</unit-identity></units>");
                            var n4remark = string.Format("{0}, usuario:{1}",descripcionServicio.Replace("á","a").Replace("é","e").Replace("í","i").Replace("ú","u").Replace("o","ó"),sUser.loginname);
                            newa.Append(string.Format("<properties><property tag=\"UnitRemark\" value=\"{0}\"/></properties>", "Servicio: " + n4remark));
                            newa.Append(string.Format("<event id=\"{1}\" note=\"{3}\" time-event-applied=\"{0}\" user-id=\"{2}\"/>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), evento, "MID_SEGURIDAD", "SERVICIO " + descripcionServicio));
                            newa.Append("</icu>");

                            mensajeContenedor = Utility.validacionN4(sesObj, newa.ToString());


                            if (!String.IsNullOrEmpty(mensajeContenedor))
                            {
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = string.Format(mensajeContenedor + " - Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(new Exception(mensajeContenedor), "solicitudUsuarios", "btGenerarSolicitud", lblNombreContenedor, sUser.loginname));
                                sinresultado.Visible = true;
                                return;
                            }
                        }

                        if (!String.IsNullOrEmpty(mensajeN4))
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            Exception errorN4 = new Exception(mensajeN4);
                            this.sinresultado.InnerText = string.Format(mensajeN4 + " - Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(new Exception(mensajeN4), "solicitudUsuarios", "btGenerarSolicitud", lblNombreContenedor, sUser.loginname));
                            sinresultado.Visible = true;
                            return;
                        }
                    }

                    if (GridContenedoresFiltrado.Count > 0)
                    {

                        //Validación 2 -> Si no hubo problema guardo la cabecera de la solicitud de reestiba                    
                        try
                        {
                            datosCab = CslHelperServicios.cabeceraSolicitud(0, TipoServicio, TipoTrafico, TipoCarga, booking, numCarga, sUser.loginname, estadoIngresado, sUser.grupo.ToString());
                        }
                        catch (Exception ex)
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = string.Format(mensajeErrorPorServicio + " " + "Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "solicitud", "solicitud_cabecera", retorno.ToString().Trim(), sUser.loginname));
                            sinresultado.Visible = true;
                            return;
                        }

                        foreach (var item in GridContenedoresFiltrado)
                        {
                            int lblIDContenedor = int.Parse(item.gkey.ToString());
                            string lblNombreContenedor = item.cntr;
                            string lblPesoContenedor = item.peso.ToString();
                            bool chckContenedor = item.check;

                            bool ckEti = item.esImo;
                            string imoS = item.esImo ? "1":"0";


                            if (chckContenedor)
                            {
                                //Validación 3 --> Se guarda los detalles con el id de la solicitud generada.
                                int retornoD = CslHelperServicios.detalleSolicitud(int.Parse(datosCab[0].idSolicitud.Trim()), lblIDContenedor, sUser.loginname,imoS);
                            }
                        }

                        //Validación 4 --> Se envía un correo al usuario. (Se guarda en las tablas)
                        envioCorreoSolicitud(int.Parse(datosCab[0].idSolicitud.Trim()), sUser, descripcionServicio, datosCab[0].codigoSolicitud.Trim());
                    }
                    else
                    {
                        var t = this.getUserBySesion();
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = "Debe de seleccionar al menos un contenedor para poder generar la solicitud.";
                        sinresultado.Visible = true;
                    }

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format(mensajeErrorPorServicio + " " + "Ha ocurrido un problema durante la generación de la solicitud, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btgenerar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }
                    imagen.InnerHtml = "<img alt='' src='../shared/imgs/loader.gif'>";
                    llenarGridView();

                }
                catch (Exception ex)
                {
                    imagen.InnerHtml = "";
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
        public void llenarGridView()
        {

            var listaGrupos = "";
            List<string> grupos = new List<string>();

            if (ConfigurationManager.AppSettings["grupos"] != null)
            {
                listaGrupos = ConfigurationManager.AppSettings["grupos"];
                grupos = listaGrupos.Split(',').ToList();
            }
            ViewState["grupos"] = grupos;

            this.alerta.Visible = false;
            this.alerta.InnerText = string.Empty;

            string tipoTrafico = dptipotrafico.SelectedValue;
            string tipoCarga = dptipocarga.SelectedValue;
            string contenedorID = txtcontenedor.Text.Trim();
            string msr = txtnumcarga1.Text.Trim();
            string msn = txtnumcarga2.Text.Trim();
            string hsn = txtnumcarga3.Text.Trim();
            string booking = txtNumBooking.Text.Trim();
            string mensaje = "";

            if (tipoTrafico.Equals("EXPRT"))
            {

                switch (tipoCarga)
                {
                    case "CO":
                        if (booking == "" && contenedorID == "")
                        {
                            mensaje = "Debe de seleccionar un contenedor o booking cuando el trafico es de Exportación y la carga es contenedor.";
                        }
                        break;
                }
            }
            else if (tipoTrafico.Equals("IMPRT"))
            {

                switch (tipoCarga)
                {
                    case "CA":
                        if (msr == "" || msn == "" || hsn == "")
                        {
                            mensaje = "Debe de seleccionar un número de carga cuando el trafico es de Importación y la carga es carga suelta.";
                        }
                        break;
                }
            }

            if (mensaje != "")
            {
                imagen.InnerHtml = "";
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format(mensaje, csl_log.log_csl.save_log<Exception>(new Exception("Error"), "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                sinresultado.Visible = true;
                return;
            }

            //Para poder hacer la busqueda del contenedor con IMDT
            string descripcionServicio = dptiposervicios.SelectedValue == "0" ? "" : dptiposervicios.Items.FindByValue(dptiposervicios.SelectedValue).Text;
            switch (descripcionServicio)
            {
                case "Verificación de Sellos":
                    if (tipoTrafico.Equals("IMPRT"))
                    {
                        tipoTrafico = "IMPO";
                    }
                    break;
                case "Repesaje":
                    if (tipoTrafico.Equals("IMPRT"))
                    {
                        tipoTrafico = "IMPO";
                    }
                    break;
            }

            usuario sUser = null;
            sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

            // List<contenedor> table = CslHelperServicios.contenedores(txtcontenedor.Text.Trim(), "", tipoTrafico, msr, msn, hsn, booking, sUser.ruc);//.catalogosDataTable();
            List<unidadN4> table = new List<unidadN4>();

            if (tipoTrafico.Contains("IMPO"))
            {
                table = unidadN4.consultaPortalIMPO(msr, msn, hsn, txtcontenedor.Text, sUser.ruc, null);
            }
            else
            {
                //Revisión Técnica Refrigerada
                if (descripcionServicio.Contains("Revisión Técnica Refrigerada") || descripcionServicio.Contains("Etiquetado - Desetiquetado Unidades"))
                {
                    table = unidadN4.consultaPortalEXPO(booking, txtcontenedor.Text, sUser.ruc, sUser.codigoempresa);
                }
                else
                {
                    table = unidadN4.consultaPortalEXPO(booking, txtcontenedor.Text, sUser.ruc, null); 
                }
                
            }


            if (descripcionServicio.Equals("Verificación de Sellos") || descripcionServicio.Equals("Repesaje") || descripcionServicio.Equals("Etiquetado - Desetiquetado Unidades") || descripcionServicio.Equals("Revisión Técnica Refrigerada"))
            {
                table.RemoveAll(x => x.fk != "FCL");
            }

            string mensajeErrorPorServicio = "Verifique que los datos ingresados sean correctos o que el contenedor no se encuentre asociado a una solicitud activa.";
        
            
            string TipoTrafico = dptipotrafico.SelectedValue;

            switch (descripcionServicio)
            {
                case "Repesaje":
                    if (TipoTrafico != "EXPRT")
                    {

                        mensajeErrorPorServicio = "Verifique que los datos del contenedor sean correctos y el mismo se encuentre dentro de la terminal.Si el problema persiste comuníquese con Yard Planners a las extensiones 4042/4060.";
                    }
                    else
                    {
                        mensajeErrorPorServicio = "Verifique que los datos del contenedor sean correctos y el mismo se encuentre dentro de la terminal.Si el problema persiste comuníquese con Yard Planners a las extensiones 4042/4060.";
                    }
                    break;
                case "Verificación de Sellos":
                    if (TipoTrafico != "EXPRT")
                    {

                        mensajeErrorPorServicio = "Verifique que los datos del contenedor sean correctos y el mismo se encuentre dentro de la terminal.Si el problema persiste comuníquese con Yard Planners Ecuapass a las extensión 4058.";
                    }
                    else
                    {
                        mensajeErrorPorServicio = "Verifique que los datos del contenedor sean correctos y el mismo se encuentre dentro de la terminal.Si el problema persiste comuníquese con Auxliares Yard Planners a la extensión 4043.";
                    }
                    break;
                case "Revisión Técnica Refrigerada":

                        mensajeErrorPorServicio = "Verifique que los datos del contenedor sean correctos y el mismo se encuentre dentro de la terminal.Si el problema persiste comuníquese con Auxiliares Yard Planners a las extensión 4043.";
                    break;
                case "Etiquetado - Desetiquetado Unidades":
                    mensajeErrorPorServicio = "Verifique que los datos del contenedor sean correctos y el mismo se encuentre dentro de la terminal.Si el problema persiste comuníquese con Yard Planners a las extensiones 4042/4060.";
                    break;
            }
            // this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
            if (table.Count > 0)
            {
                this.grvContenedores.DataSource = table.OrderBy(x => x.gkey).ToList();

                this.grvContenedores.DataBind();

                var se = table.Where(u => !string.IsNullOrEmpty(u.grupo) && grupos.Contains(u.grupo)).Select(o => o.cntr).ToArray();
                if (se != null && se.Length > 0)
                {
                    this.alerta.Visible = true;
                    this.alerta.InnerText = "Su solicitud no puede ser generada, debido a que el contenedor tiene un servicio pendiente. Por favor comuníquese con el área de Yard Planners a las extensiones 4042/4060.";
                }
                else
                {
                    this.alerta.Visible = false;
                    this.alerta.InnerText = string.Empty;
                }

                xfinder.Visible = true;
                sinresultado.Visible = false;
                GridContenedores = table;
                imagen.InnerHtml = "";
                return;
            }
            else
            {
                imagen.InnerHtml = "";
                xfinder.Visible = false;
                sinresultado.Visible = false;
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                //  sinresultado.InnerText = string.Format(mensajeErrorPorServicio, csl_log.log_csl.save_log<Exception>(new Exception("Error"), "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));

                sinresultado.InnerText = mensajeErrorPorServicio;
                sinresultado.Visible = true;
                return;
            }

        }
        protected void grvContenedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvContenedores.PageIndex = e.NewPageIndex;
            actualizarDataTableSession();
            this.grvContenedores.DataSource = GridContenedores.OrderBy(x => x.gkey).ToList();
            this.grvContenedores.DataBind();
        }
        public void actualizarDataTableSession()
        {

            foreach (GridViewRow item in grvContenedores.Rows)
            {
                Label lblIDContenedor = item.FindControl("lblIdContenedor") as Label;
                Label lblNombreContenedor = item.FindControl("lblNombreContenedor") as Label;
                Label lblPesoContenedor = item.FindControl("lblPesoContenedor") as Label;
                CheckBox chckContenedor = item.FindControl("chkRow") as CheckBox;

                CheckBox ckImo = item.FindControl("cImo") as CheckBox;

                unidadN4 tempCont = new unidadN4();
                tempCont = GridContenedores.Where(x => x.gkey.ToString() == lblIDContenedor.Text).FirstOrDefault();
                GridContenedores.Remove(tempCont);
                tempCont.check = chckContenedor.Checked;
                tempCont.esImo = ckImo.Checked;
                GridContenedores.Add(tempCont);
            }
        }
        public void envioCorreoSolicitud(int idSolicitud, usuario sUser, string descripcionServicio, string codigoSolicitud)
        {

            string correoUsuario = "";

            List<consultaCabeceraUsuario> solicitudUsuario = CslHelperServicios.consultaSolicitudUsuario(null, 0, null, null, null, null, false, null, idSolicitud.ToString());
            foreach (var datoSolicitud in solicitudUsuario)
            {
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();

            }

            var jmsg = new jMessage();
            string mail = string.Empty;
            string destinatarios= string.Empty;  //=  turnoConsolidacion.GetMails();
            var user_email = correoUsuario;

            var cfgs = dbconfig.GetActiveConfig(null, null, null);
            CSLSite.dbconfig mail_destino = null;
            var correoBackUp = cfgs.Where(a => a.config_name.Contains("correoBackUp")).FirstOrDefault();
            switch (descripcionServicio)
            {
                case "Repesaje":
                    mail = getBodyRepesaje(solicitudUsuario, sUser);
                    mail_destino = cfgs.Where(a => a.config_name.Contains("mail_repesaje")).FirstOrDefault();
                    destinatarios = string.Format("{0};{1}", correoBackUp != null ? correoBackUp.config_value : "no_cfg",mail_destino != null ? mail_destino.config_value : "no_cfg");
                    break;
                case "Verificación de Sellos":
                    mail = getBodyVerificacion(solicitudUsuario, sUser);
                    mail_destino = cfgs.Where(a => a.config_name.Contains("mail_sellos")).FirstOrDefault();
                    destinatarios = string.Format("{0};{1}", correoBackUp != null ? correoBackUp.config_value : "no_cfg",  mail_destino != null ? mail_destino.config_value : "no_cfg");
                    break;
                case "Etiquetado - Desetiquetado Unidades":
                     mail = getBodyEtiquetadoDes(solicitudUsuario, sUser);
                     mail_destino = cfgs.Where(a => a.config_name.Contains("mail_imo")).FirstOrDefault();
                     destinatarios = string.Format("{0};{1}", correoBackUp != null ? correoBackUp.config_value : "no_cfg",  mail_destino != null ? mail_destino.config_value : "no_cfg");
                     break;
                case "Revisión Técnica Refrigerada":
                    mail = getBodyRevisionTecnica(solicitudUsuario, sUser);
                    mail_destino = cfgs.Where(a => a.config_name.Contains("mail_rtr")).FirstOrDefault();
                    destinatarios = string.Format("{0};{1}", correoBackUp != null ? correoBackUp.config_value : "no_cfg", mail_destino != null ? mail_destino.config_value : "no_cfg");
                    break;
            }

            string mensaje = string.Empty;
            //string clase = "";
           // destinatarios = user_email;

            CLSDataCentroSolicitud.addMail(out mensaje, user_email, codigoSolicitud + " - Solicitud de " + descripcionServicio, mail, destinatarios, sUser.loginname, "", "");
            string error = string.Empty;
            if (!string.IsNullOrEmpty(error))
            {
                alerta.Visible = true;
                alerta.InnerText = error;
                return;
            }
            else
            {
                imagenSolicitud.InnerHtml = "";
                Utility.mostrarMensajeRedireccionando(this.Page, "Se genero el código " + codigoSolicitud + "  para su solicitud del servicio de " + descripcionServicio + ", revise su correo en unos minutos para mayor información.", "../csl/menudefault");
            }
        }
        public string getBodyRepesaje(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {

            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string numCarga = "";
            string numBooking = "";

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                numCarga = String.IsNullOrEmpty(datoSolicitud.noCarga) ? null : datoSolicitud.noCarga.Trim();
                numBooking = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
            }
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            return Utility.repesaje_msg_ingreso(listaDetalle.Select(a => a.descripcionContenedor).ToList(), trafico, descripcionEstado, numBooking, numCarga);
        }
        public string getBodyVerificacion(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string numCarga = "";
            string numBooking = "";

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                numCarga = String.IsNullOrEmpty(datoSolicitud.noCarga) ? null : datoSolicitud.noCarga.Trim();
                numBooking = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
            }

            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            return Utility.verificacion_msg_ingreso(listaDetalle.Select(a => a.descripcionContenedor).ToList(), trafico, descripcionEstado, numBooking, numCarga);
        }
        public string getBodyEtiquetadoDes(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string numCarga = "";
            string numBooking = "";

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                numCarga = String.IsNullOrEmpty(datoSolicitud.noCarga) ? null : datoSolicitud.noCarga.Trim();
                numBooking = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
            }
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            return Utility.etiqueta_msg_ingreso(listaDetalle.Select(a => a.descripcionContenedor).ToList(), trafico, descripcionEstado, numBooking, numCarga);
        }
        public string getBodyRevisionTecnica(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
        {
            string descripcionEstado = "";
            string descripcionServicio = "";
            string nombreUsuario = "";
            string correoUsuario = "";
            string codigoSolicitud = "";
            string trafico = "";
            string productoEmbalaje = "";
            string fechaPropuesta = "";
            string comentarios = "";
            string mail = string.Empty;
            string observacion = "";
            int idSolicitud = 0;
            string numCarga = "";
            string numBooking = "";

            foreach (var datoSolicitud in solicitudUsuario)
            {
                descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                trafico = String.IsNullOrEmpty(datoSolicitud.trafico) ? null : datoSolicitud.trafico.Trim();
                productoEmbalaje = String.IsNullOrEmpty(datoSolicitud.tipoEmbalaje) ? null : datoSolicitud.tipoEmbalaje.Trim();
                fechaPropuesta = String.IsNullOrEmpty(datoSolicitud.fechaPropuesta) ? null : datoSolicitud.fechaPropuesta.Trim();
                comentarios = String.IsNullOrEmpty(datoSolicitud.comentarios) ? null : datoSolicitud.comentarios.Trim();
                observacion = String.IsNullOrEmpty(datoSolicitud.observacion) ? null : datoSolicitud.observacion.Trim();
                idSolicitud = int.Parse(datoSolicitud.idSolicitud);
                numCarga = String.IsNullOrEmpty(datoSolicitud.noCarga) ? null : datoSolicitud.noCarga.Trim();
                numBooking = String.IsNullOrEmpty(datoSolicitud.noBooking) ? null : datoSolicitud.noBooking.Trim();
            }

            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
            return Utility.tecnica_msg_ingreso(listaDetalle.Select(a => a.descripcionContenedor).ToList(), trafico, descripcionEstado, numBooking, numCarga);
        }
        protected void grvContenedores_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            //evaluar aqui el servicio y mostrar y ocultar dinamicamente las columnas del grid
            var servicio = dptiposervicios.SelectedItem.Text;
            try
            {


                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Visible = false; //gkey
                    e.Row.Cells[1].Visible = true; //cntr
                    e.Row.Cells[2].Visible = false; //categoria
                    e.Row.Cells[3].Visible = false; //fk
                    e.Row.Cells[4].Visible = false; //s1
                    e.Row.Cells[5].Visible = false; //s2
                    e.Row.Cells[6].Visible = false; //s3
                    e.Row.Cells[7].Visible = false; //s4
                    e.Row.Cells[8].Visible = false; //iso
                    e.Row.Cells[9].Visible = true; //grupo
                    e.Row.Cells[10].Visible = true; //peso
                    e.Row.Cells[11].Visible = false; //boking
                    e.Row.Cells[12].Visible = false; //mrn
                    e.Row.Cells[13].Visible = false; //msn
                    e.Row.Cells[14].Visible = false; //hsn
                    e.Row.Cells[15].Visible = false; //propID
                    e.Row.Cells[16].Visible = false; //PropNombre
                    e.Row.Cells[17].Visible = false; //doc
                    e.Row.Cells[18].Visible = false; //aisv
                    e.Row.Cells[19].Visible = false; //referencia
                    e.Row.Cells[20].Visible = false; //expoUser
                    e.Row.Cells[21].Visible = false; //imokey
                    e.Row.Cells[22].Visible = false; //esImo
                    e.Row.Cells[23].Visible = false; //esRefer
                    e.Row.Cells[24].Visible = false; //linea
                    e.Row.Cells[25].Visible = false; //patio
                    e.Row.Cells[26].Visible = true; //check

                    //if (servicio.Contains("Repesaje") || servicio.Contains("Etiquetado - Desetiquetado Unidades") )
                    //{
                        e.Row.Cells[10].Visible = false; //peso
                    //}

                    if (servicio.Contains("Etiquetado - Desetiquetado Unidades"))
                    {
                        e.Row.Cells[22].Visible = true; //imo
                    }


                    if (servicio.Contains("Revisión Técnica Refrigerada"))
                    {
                        e.Row.Cells[23].Visible = true; //reefer
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if (e.Row.Cells[9].Controls != null && e.Row.Cells[9].Controls.Count > 0)
                        {
                            var cs = e.Row.Cells[26].FindControl("chkRow") as CheckBox;
                            var cv = e.Row.Cells[9].FindControl("grupoReal") as HiddenField;

                            var cim = e.Row.Cells[22].FindControl("cImo") as CheckBox;
                            if (cs != null && cv != null)
                            {
                                var grupos = ViewState["grupos"] as List<string>;
                                if (grupos != null && grupos.Count > 0 && grupos.Contains(cv.Value))
                                {
                                    cs.Enabled = false;
                                 }
                                else
                                {
                                    cs.Enabled = true;
                                }
                                if (cim != null)
                                {
                                    cim.Enabled = cs.Enabled;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.IndexOutOfRangeException)
            {


            }

        }
        public static string Ogrupo(Object gr)
        {
            var listaGrupos = "";
            List<string> grupos = new List<string>();

            if (ConfigurationManager.AppSettings["grupos"] != null)
            {
                listaGrupos = ConfigurationManager.AppSettings["grupos"];
                grupos = listaGrupos.Split(',').ToList();
            }
            if (gr != null && grupos.Count > 0)
            {
                var ss = gr.ToString();
                if (grupos.Contains(ss))
                {
                    return "SI";
                }
            }
            return string.Empty;
        }
    }
}