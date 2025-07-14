using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using ConectorN4;

namespace CSLSite
{
    public partial class mantenimientoOperario : System.Web.UI.Page
    {
        public List<consultaSolicitudOperador> GridContenedoresOperador
        {
            get { return (List<consultaSolicitudOperador>)Session["GridContenedoresOperador"]; }
            set { Session["GridContenedoresOperador"] = value; }
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
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
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
                populateDrop(dpestados, CslHelperServicios.getDetalleCatalogo(ConfigurationManager.AppSettings["TipoEstado"]));
                if (dpestados.Items.Count > 0)
                {  
                    dpestados.Items.Remove(dpestados.Items.FindByValue("RE")); //Elimina el estado rechazado del combo de estados
                    dpestados.Items.Remove(dpestados.Items.FindByValue("FI")); //Elimina el estado finalizado del combo de estados
                    dpestados.SelectedValue = "IN";
                }
                
                populateDrop(dptiposervicios, CslHelperServicios.getServicios());
                dptiposervicios.SelectedValue = "0";

                cargarTabla(0, null, null, null, dpestados.SelectedValue.ToString(), false);
            }
        }

        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
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

                    int idContenedor = String.IsNullOrEmpty(idContenedorU.Value.Trim()) ? 0 : int.Parse(idContenedorU.Value.Trim());
                    string fDesde = String.IsNullOrEmpty(desded.Text.Trim()) ? null : desded.Text.Trim();
                    string fHasta = String.IsNullOrEmpty(hastad.Text.Trim()) ? null : hastad.Text.Trim();
                    bool cTodos = chkTodos.Checked;

                    cargarTabla(idContenedor, dptiposervicios.SelectedValue.ToString(), fDesde, fHasta, dpestados.SelectedValue.ToString(), cTodos);
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }

        public void cargarTabla(int idContenedor, string tipoServicio, string fDesde, string fHasta, string estado, bool cTodos) {

            List<consultaSolicitudOperador> table = CslHelperServicios.consultaSolicitudOperador(idContenedor, tipoServicio, fDesde, fHasta, estado, cTodos);//.catalogosDataTable();
            table.RemoveAll(x => x.estadoSolicitud == "IN" && x.servicio.Trim() == "Reestiba");
            table.RemoveAll(x => x.confirmacion == "SI" && x.servicio.Trim() == "Verificación de Sellos");
            table.RemoveAll(x => x.confirmacion == "SI" && x.servicio.Trim() == "Repesaje");
            //table.RemoveAll(x => x.servicio == "");
            
            var u = this.getUserBySesion();
            if (Response.IsClientConnected) {
                if (table.Count > 0) {


                    foreach (var d in table)
                    {
                        if (d.servicio.Contains("Etiquetado - Desetiquetado Unidades"))
                        {
                            if (d.imo)
                            {
                                d.servicio = "ETIQUETAR";
                            }
                            else
                            {
                                d.servicio = "DESETIQUETAR";
                            }
                        }
                    }

                    this.grvContenedores.DataSource = table;
                    this.grvContenedores.DataBind();
                    GridContenedoresOperador = table;
                    xfinder.Visible = true;
                    sinresultado.Visible = false;
                    alerta.Visible = false;
                    return;
                }
                else {
                    xfinder.Visible = false;
                    sinresultado.Visible = false;
                    alerta.Visible = true;
                    grvContenedores.DataSource = null;
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
                }
            }
        }

        public void btconfirmar_Click(object sender, EventArgs e)
        {
            bool enviarMail = false;
            
            string retorno = "";
            string actualizacionS = "";
            usuario sUser = null;
            sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            string tipoVerificacion = null;
            string mensajeN4 = "";

            try
            {
                actualizarDataTableSession();
                List<consultaSolicitudOperador> GridContenedoresOperadorFiltrado = GridContenedoresOperador.Where(x => x.confirmacion != "PE").ToList();
                foreach (var item in GridContenedoresOperadorFiltrado)
                {
                    int IdDetalle = int.Parse(item.idDetalleSolicitud);
                    string lblServicio = item.servicio;
                    string observacion = item.observacion;
                    string confirmacion = item.confirmacion;
                    string ddlTipoVerificacion = item.tipoVerificacion;
                    int idSolicitud = int.Parse(item.numSolicitud);
                    string a = "";

                    var tk = HttpContext.Current.Request.Cookies["token"];
                    ConectorN4.ObjectSesion sesObj = new ObjectSesion();
                    sesObj.clase = "solicitudUsuarios" + lblServicio;
                    sesObj.metodo = "btgenerar_Click";
                    sesObj.transaccion = "generarSolicitud" + lblServicio;
                    sesObj.usuario = sUser.loginname;
                    sesObj.token = tk.Value;

                    if (!confirmacion.Equals("PE"))
                    {
                        //Validación 1, verifica que el campo de observación este lleno cuando el combo de confirmado es negativo (NO)
                        if (confirmacion.Equals("NO"))
                        {
                            if (String.IsNullOrEmpty(observacion))
                            {
                                alerta.Visible = true;
                                this.alerta.InnerHtml = "La observación es de caracter obligatorio cuando la confirmación es negativa.";
                                return;
                            }
                        }

                        //Validación 2, actualizo el estado de confirmación y observación de los containers.
                        try
                        {
                            if (!ddlTipoVerificacion.Equals("SELE"))
                            {
                                tipoVerificacion = ddlTipoVerificacion.Trim();
                            }

                            if (lblServicio.Equals("Verificación de Sellos") || lblServicio.Equals("Repesaje"))
                            {

                                if (lblServicio.Contains("Repesaje"))
                                {
                                    enviarMail = false;
                                }
                                
                                if (!confirmacion.Equals("SELE"))
                                {
                                    a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"></unit-identity></units><properties><property tag=\"RoutingGroupRef\" value=\"{1}\"/></properties></icu>", item.contenedor.Trim(), "");
                                    mensajeN4 = Utility.validacionN4(sesObj, a);
                                }
                            }

                            retorno = CslHelperServicios.actualizacionSolicitudOperario(item.idDetalleSolicitud, observacion, confirmacion, sUser.loginname, tipoVerificacion);
                        }
                        catch (Exception ex)
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = string.Format("Se produjo un error durante al grabar la cabecera, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "operadorSolicitud", "solicitud_contenedor", IdDetalle.ToString().Trim(), sUser.loginname));
                            sinresultado.Visible = true;
                            return;
                        }

                        //Validación 3, se verifica  que todos los containers de la solicitud esten trabajados, si es así se cambia el estado de la solicitud a finalizado.
                        try
                        {
                            actualizacionS = CslHelperServicios.actualizacionEstadoSolicitudOperador(retorno, sUser.loginname);

                            if (lblServicio.Contains("Revisión Técnica Refrigerada"))
                            {
                                enviarMail = true;
                            }
                            if (lblServicio.Equals("Verificación de Sellos"))
                            {

                                enviarMail = false;
                                List<consultaDetalleUsuario> listaDetalleVS = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
                                List<consultaDetalleUsuario> listaDetalleVSConfirmados = listaDetalleVS.Where(x => x.confirmado == "Si").ToList();
                                if (listaDetalleVS.Count == listaDetalleVSConfirmados.Count)
                                {
                                    generarVerificacion(idSolicitud);
                                }
                            }
                            else if (!lblServicio.Equals("Verificación de Sellos") && !lblServicio.Equals("Repesaje"))
                            {

                                if (!confirmacion.Equals("SELE"))
                                {
                                    a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"></unit-identity></units><properties><property tag=\"RoutingGroupRef\" value=\"{1}\"/></properties></icu>", item.contenedor, "");
                                    mensajeN4 = Utility.validacionN4(sesObj, a);
                                }
                            }
                            else if (lblServicio.Equals("Etiquetado - Desetiquetado Unidades"))
                            {
                                    
                                //Linea x linea.
                            }

                            if (actualizacionS != "0")
                            {
                                //Envio el correo al usuario cuando todos los contenedores están trabajados.
                                if (enviarMail)
                                    envioCorreoSolicitud(int.Parse(actualizacionS), sUser);
                            }
                            else
                            {
                                if (enviarMail)
                                    envioCorreo(idSolicitud, int.Parse(retorno), sUser);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.sinresultado.Attributes["class"] = string.Empty;
                            this.sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = string.Format("Se produjo un error durante al grabar el detalle, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "operadorSolicitud", "solicitud_contenedor", IdDetalle.ToString().Trim(), sUser.loginname));
                            sinresultado.Visible = true;
                            return;
                        }
                    }
                }

                //Vuelve a cargar la tabla para sacar de la misma los contenedores con solicitud en estado finalizado.
                int idContenedor = String.IsNullOrEmpty(idContenedorU.Value.Trim()) ? 0 : int.Parse(idContenedorU.Value.Trim());
                string fDesde = String.IsNullOrEmpty(desded.Text.Trim()) ? null : desded.Text.Trim();
                string fHasta = String.IsNullOrEmpty(hastad.Text.Trim()) ? null : hastad.Text.Trim();
                bool cTodos = chkTodos.Checked;

                cargarTabla(idContenedor, dptiposervicios.SelectedValue.ToString(), fDesde, fHasta, dpestados.SelectedValue.ToString(), cTodos);
                Utility.mostrarMensaje(this.Page, "Se ha actualizado con éxito la solicitud.");

            }
            catch (Exception ex) {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante al grabar, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "operadorSolicitud", "solicitud_contenedor", retorno.ToString().Trim(), sUser.loginname));
                sinresultado.Visible = true;
                return;
            }            
        }

        protected void grvContenedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvContenedores.PageIndex = e.NewPageIndex;
            actualizarDataTableSession();
            this.grvContenedores.DataSource = GridContenedoresOperador.OrderBy(x => x.idDetalleSolicitud).ToList();
            this.grvContenedores.DataBind();
        }

        protected void grvContenedores_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlTipoVerificacion = e.Row.FindControl("tipoDDL") as DropDownList;
                if (ddlTipoVerificacion.SelectedValue.Equals("SELE") || ddlTipoVerificacion.SelectedValue.Equals(""))
                {
                    ddlTipoVerificacion.Attributes["class"] = "ddlTipoMO";
                }
                else
                {
                    ddlTipoVerificacion.Attributes["class"] = "";
                }
            }
        }

        public string quitarGrupo(string descripcionContenedor, string grupo)
        {
            wsN4 g = new wsN4();
            String a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"></unit-identity></units><properties><property tag=\"RoutingGroupRef\" value=\"{1}\"/></properties></icu>", descripcionContenedor, grupo);
            string me = string.Empty;
            string errorN4 = string.Empty;
            var i = g.CallBasicService("ICT/ECU/GYE/CGSA", a, ref me);

            /*I ES LA RESPUESTA QUE DEVUELVE EL N4 Y me ES LA DESCRIPCION DEL MENSAJE DE ERROR*/
            if (i > 0) {
                errorN4 = me;
                //throw new Exception(me);                        
            }

            return errorN4;
        }

        public void actualizarDataTableSession()
        {
            string tipoVerificacion = "SELE";
            foreach (GridViewRow item in grvContenedores.Rows)
            {   
                Label lblContenedor = item.FindControl("lblContenedor") as Label;
                TextBox txtObservacion = item.FindControl("txtArea") as TextBox;
                DropDownList ddlConfirmacion = item.FindControl("confirmacionddl") as DropDownList;
                DropDownList ddlTipoVerificacion = item.FindControl("tipoDDL") as DropDownList;

                //if (ddlTipoVerificacion.Attributes["class"] != "ddlTipoMO")
                if (!ddlTipoVerificacion.SelectedValue.Equals("SELE"))
                {
                    tipoVerificacion = ddlTipoVerificacion.SelectedValue.Trim();
                }

                consultaSolicitudOperador tempCont = new consultaSolicitudOperador();
                tempCont = GridContenedoresOperador.Where(x => x.contenedor.Trim() == lblContenedor.Text.Trim()).FirstOrDefault();
                GridContenedoresOperador.Remove(tempCont);
                tempCont.confirmacion = ddlConfirmacion.SelectedValue;
                tempCont.tipoVerificacion = tipoVerificacion;
                tempCont.observacion = txtObservacion.Text.Trim();
                GridContenedoresOperador.Add(tempCont);
            }
        }

        public void generarVerificacion(int idSolicitud) {
            var numero = string.Empty;
            usuario sUser = null;
            sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud,null);
            string mensaje = "";

            foreach (var item in listaDetalle)
            {
                if (item.tipoVerificacion.Equals("CIIS")) {
                   // mensaje = generarCIIS(int.Parse(item.idContainer), item.descripcionContenedor.Trim(), sUser, out numero);
                    CslHelperServicios.actualizacionSolicitudOperario(item.idDetalleSolicitud, mensaje, null, sUser.loginname, string.Format("C-{0}", numero), null);
                }
                else if (item.tipoVerificacion.Equals("IMPDT")) {
                   
                   // mensaje = generarIMPDT(int.Parse(item.idContainer), item.descripcionContenedor.Trim(), sUser, idSolicitud, out numero);
                    CslHelperServicios.actualizacionSolicitudOperario(item.idDetalleSolicitud, mensaje, null, sUser.loginname, string.Format("I-{0}",numero),null);
                }
            }
        }

        public string agenerarCIIS(int idContainer, string descripcionContainer, usuario sUser, out string number)
        {
           
            //return string.Empty;
            //number = string.Empty;
            number = string.Empty;
            string mensajeError = "";
            //string codigoSolicitudTransaccion = "";
            //string codigoSolicitudTransaccionRellenada = "";
            //string codigoSolicitudTransaccionGenerada = "";
            //string secuenciaNumerica = "";
            //string codigoConstante = "05909025";
            //List<camposSolicitudTransaccion> camposSolicitud = CslHelperServicios.consultaIMDT(idContainer.ToString(), descripcionContainer);

            //if (camposSolicitud.Count != 0) {
            //    try
            //    {
            //        //Guarda en la tabla de ECU_SOLICITUD_TRANSACCION y obtiene el nuevo código de solicitud para generar un nuevo código
            //        codigoSolicitudTransaccion = CslHelperServicios.ingresoSOLI(idContainer.ToString(), descripcionContainer, camposSolicitud[0].mrn, camposSolicitud[0].msn, camposSolicitud[0].hsn, sUser.loginname);
            //        codigoSolicitudTransaccionRellenada = getCodigoSolicitudRellenada(codigoSolicitudTransaccion);
            //        codigoSolicitudTransaccionGenerada = codigoConstante + DateTime.Now.Year.ToString() + codigoSolicitudTransaccionRellenada + "S";

            //        //Guarda en la tabla de ECU_SOLICITUD y obtiene la secuencia del número para los argumentos
            //        secuenciaNumerica = CslHelperServicios.ingresoTRAN(idContainer.ToString(), codigoSolicitudTransaccionGenerada, codigoSolicitudTransaccion, camposSolicitud[0].numeroEntrega, descripcionContainer, camposSolicitud[0].mrn, camposSolicitud[0].msn, camposSolicitud[0].hsn);

            //        //Guarda en la tabla de ECU_SOLICITUD_TRANSACCION_ARGS
            //        guardarArgumentos(idContainer.ToString(), codigoSolicitudTransaccion, codigoSolicitudTransaccionGenerada, secuenciaNumerica);

            //        number = codigoSolicitudTransaccionGenerada;
            //    }
            //    catch (Exception ex) {
            //        this.sinresultado.Attributes["class"] = string.Empty;
            //        this.sinresultado.Attributes["class"] = "msg-critico";
            //        this.sinresultado.InnerText = string.Format("Se produjo un error durante al grabar, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "generarCIIS", "generarCIIS", idContainer.ToString(), sUser.loginname));
            //        sinresultado.Visible = true;
            //        //return;
            //    }
            //}
            //else {
            //    mensajeError = "Este contenedor no tiene una solicitud de IMPDT";       
            //}

            return mensajeError;
        }

        public void guardarArgumentos(string idContainer, string codigoSolicitudTransaccion, string codigoSolicitudTransaccionGenerada, string secuenciaNumerica)
        {
            List<string> argumentos = new List<string>();
            argumentos.Add("CarryInOutTypeCode");
            argumentos.Add("ConsignmentItemSequenceNumericIKJ");
            argumentos.Add("CommonInformationItemNameIKJ");
            argumentos.Add("CommonInformationPreviousContentIKJ");
            argumentos.Add("CommonInformationAfterContentIKJ");
            argumentos.Add("AmendmentAmendmentChangeReason");
            argumentos.Add("ConsignmentWarehouseID");
            argumentos.Add("DeclarationID");

            List<string> valores = new List<string>();
            valores.Add("I"); //quemado
            valores.Add(secuenciaNumerica);
            valores.Add("IKJ"); //por defecto IKJ, que es el primero sello
            valores.Add("0"); //Valor anterior del sello, el cual cambia
            valores.Add("0"); //Valor nuevo del sello, el cual cambia
            valores.Add("CORRECCION DE SELLOS");
            valores.Add("05909025");
            valores.Add(codigoSolicitudTransaccionGenerada);

            //obtiene el nuevo valor de los sellos
            List<contenedor> contenedoresConsulta = CslHelperServicios.contenedoresConsulta(null, null, idContainer.ToString());

            //obtiene el valor anterior de los sellos
            List<listaContenedoresSellos> consultaSellosAnteriores = CslHelperServicios.consultaSellosAnteriores(idContainer.ToString());
            
            //sello 1
            if (!String.IsNullOrEmpty(contenedoresConsulta[0].sello1.ToString().Trim())) {
                valores[3].Replace("0", contenedoresConsulta[0].sello1.ToString().Trim());

                if (consultaSellosAnteriores.Count > 0) {
                    if (!String.IsNullOrEmpty(consultaSellosAnteriores[0].valor.ToString().Trim()))
                    {
                        valores[4].Replace("0", consultaSellosAnteriores[0].valor.ToString().Trim());
                    }
                }
                
                saveArguments(codigoSolicitudTransaccion, argumentos, valores, "IKJ");
                //CslHelperServicios.ingresoARGU(codigoSolicitudTransaccion, argumento, valor, numeroEntrega,"IKJ");
            }

            //sello 2
            if (!String.IsNullOrEmpty(contenedoresConsulta[0].sello2.ToString().Trim())) {
                valores[3].Replace("0", contenedoresConsulta[0].sello2.ToString().Trim());

                if (consultaSellosAnteriores.Count > 0) {
                    if (!String.IsNullOrEmpty(consultaSellosAnteriores[1].valor.ToString().Trim()))
                    {
                        valores[4].Replace("0", consultaSellosAnteriores[1].valor.ToString().Trim());
                    }
                }
                saveArguments(codigoSolicitudTransaccion, argumentos, valores, "IKK");
                //CslHelperServicios.ingresoARGU(codigoSolicitudTransaccion, argumento, valor, numeroEntrega,"IKJ");
            }

            //sello 3
            if (!String.IsNullOrEmpty(contenedoresConsulta[0].sello3.ToString().Trim())) {
                valores[3].Replace("0", contenedoresConsulta[0].sello3.ToString().Trim());

                if (consultaSellosAnteriores.Count > 0) {
                    if (!String.IsNullOrEmpty(consultaSellosAnteriores[2].valor.ToString().Trim()))
                    {
                        valores[4].Replace("0", consultaSellosAnteriores[2].valor.ToString().Trim());
                    }
                }
                saveArguments(codigoSolicitudTransaccion, argumentos, valores, "IKL");
                //CslHelperServicios.ingresoARGU(codigoSolicitudTransaccion, argumento, valor, numeroEntrega,"IKJ");
            }

            //sello 4
            if (!String.IsNullOrEmpty(contenedoresConsulta[0].sello4.ToString().Trim())) {
                valores[3].Replace("0", contenedoresConsulta[0].sello4.ToString().Trim());

                if (consultaSellosAnteriores.Count > 0) {
                    if (!String.IsNullOrEmpty(consultaSellosAnteriores[3].valor.ToString().Trim()))
                    {
                        valores[4].Replace("0", consultaSellosAnteriores[3].valor.ToString().Trim());
                    }
                }
                saveArguments(codigoSolicitudTransaccion, argumentos, valores, "IKM");
                //CslHelperServicios.ingresoARGU(codigoSolicitudTransaccion, argumento, valor, numeroEntrega,"IKJ");
            }
            
        }

        public void saveArguments(string codigoSolicitudTransaccion, List<string> argumentos, List<string> valores, string numSello)
        {
            string newArgumento = "";
            for (int i = 0; i < argumentos.Count; i++) {
                
                newArgumento = argumentos[i].ToString().Replace("IKJ",numSello);
                CslHelperServicios.ingresoARGU(codigoSolicitudTransaccion, newArgumento, valores[i].ToString());
            }
        }

        public string getCodigoSolicitudRellenada(string codigoSolicitudTransaccion) {
            string codigoSolicitudTransaccionRellenada = "";
            codigoSolicitudTransaccionRellenada = codigoSolicitudTransaccion;

            for (int x = codigoSolicitudTransaccion.Length; x < 9; x++)
            {
                codigoSolicitudTransaccionRellenada = "0" + codigoSolicitudTransaccionRellenada;
            }

            return codigoSolicitudTransaccionRellenada;
        }

        public string generarIMPDT(int idContainer, string descripcionContainer, usuario sUser, int idSolicitud, out string msgs)
        {

            string mensajeError = "";
            msgs = string.Empty;
            consultaIMDT camposSolicitudIMDT = CslHelperServicios.consultaProcesoIMDTUno(idContainer.ToString(), descripcionContainer);

            if (!String.IsNullOrEmpty(camposSolicitudIMDT.mrn)) {
                generarSolicitudIMPDT(camposSolicitudIMDT, idContainer.ToString(), descripcionContainer, sUser, idSolicitud,out msgs);
            }
            else
            {
                camposSolicitudIMDT = CslHelperServicios.consultaProcesoIMDTDos(idContainer.ToString(), descripcionContainer);
                if (!String.IsNullOrEmpty(camposSolicitudIMDT.mrn)) {
                    generarSolicitudIMPDT(camposSolicitudIMDT, idContainer.ToString(), descripcionContainer, sUser, idSolicitud, out msgs);                    
                }
                else {
                    camposSolicitudIMDT = CslHelperServicios.consultaProcesoIMDTTres(idContainer.ToString(), descripcionContainer);
                    if (!String.IsNullOrEmpty(camposSolicitudIMDT.mrn)) {
                        generarSolicitudIMPDT(camposSolicitudIMDT, idContainer.ToString(), descripcionContainer, sUser, idSolicitud, out msgs);
                    }
                    else {
                        mensajeError = "Este contenedor no tiene una solicitud de IMPDT";
                    }
                }                
            }
           
            return mensajeError;
        }

        public void generarSolicitudIMPDT(consultaIMDT camposSolicitudIMDT, string idContainer, string descripcionContainer, usuario sUser, int idSolicitud, out string secqId)
        {
            string codigoSolicitudTransaccion = "";
            string codigoSolicitudTransaccionRellenada = "";
            string codigoSolicitudTransaccionGenerada = "";
            string secuenciaNumerica = "";
            string codigoConstante = "05909025";

            try
            {
                //Guarda en la tabla de ECU_SOLICITUD_TRANSACCION y obtiene el nuevo código de solicitud para generar un nuevo código
                codigoSolicitudTransaccion = CslHelperServicios.ingresoIMPDTSOLI(idContainer.ToString(), descripcionContainer, camposSolicitudIMDT.mrn, camposSolicitudIMDT.msn, camposSolicitudIMDT.hsn, sUser.loginname);
                consultaIMDT camposSolicitudIMDTTres = CslHelperServicios.consultaProcesoIMDTTres(idContainer.ToString(), descripcionContainer);
                codigoSolicitudTransaccionRellenada = getCodigoSolicitudRellenada(codigoSolicitudTransaccion);
                codigoSolicitudTransaccionGenerada = codigoConstante + DateTime.Now.Year.ToString() + codigoSolicitudTransaccionRellenada + "S";

                //Guarda en la tabla de ECU_SOLICITUD y obtiene la secuencia del número para los argumentos
                secuenciaNumerica = CslHelperServicios.ingresoIMPDTTRAN(idContainer.ToString(), codigoSolicitudTransaccionGenerada, codigoSolicitudTransaccion, camposSolicitudIMDT.numeroEntrega, descripcionContainer, camposSolicitudIMDT.mrn, camposSolicitudIMDT.msn, camposSolicitudIMDT.hsn);

                //Guarda en la tabla de ECU_SOLICITUD_TRANSACCION_ARGS
                guardarArgumentosIMPDT(idContainer.ToString(), codigoSolicitudTransaccion, codigoSolicitudTransaccionGenerada, secuenciaNumerica, camposSolicitudIMDT.mrn, camposSolicitudIMDT.msn, camposSolicitudIMDT.hsn, descripcionContainer, camposSolicitudIMDTTres.secuencia, idSolicitud);
                secqId = codigoSolicitudTransaccionGenerada;
            }
            catch (Exception ex)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante al grabar, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "generarCIIS", "generarCIIS", idContainer.ToString(), sUser.loginname));
                sinresultado.Visible = true;
                secqId = ex.Message;
                return;
            }
        }

        public void guardarArgumentosIMPDT(string idContainer, string codigoSolicitudTransaccion, string codigoSolicitudTransaccionGenerada, string secuenciaNumerica, string mrn, string msn, string hsn, string descripcionContainer, string numeroSecuencia, int idSolicitud)
        {
            DateTime fechaActual = DateTime.Now;
            CultureInfo ci = CultureInfo.InvariantCulture;
            string fechaFormato = fechaActual.ToString("yyyy-m-ddThh:mm:ss", ci);
            string iso = "";
            string sello1 = "";
            string sello2 = "";
            string sello3 = "";
            string sello4 = "";

            //obtiene el nuevo valor de los sellos y el iso
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null, "0", int.Parse(idContainer));

            foreach (var item in listaDetalle)
            {
                iso = item.iso;
                sello1 = item.sello1;
                sello2 = item.sello2;
                sello3 = item.sello3;
                sello4 = item.sello4;
            }

            List<string> argumentos = new List<string>();
            argumentos.Add("ConsignmentAbnormalProcessTypeCode");
            argumentos.Add("AbnormalStatusTypeCode");
            argumentos.Add("ConsignmentItemPackagingMarksNumbers");
            argumentos.Add("ConsignmentTransactionDateTime");
            argumentos.Add("ConsignmentWarehouseID");
            argumentos.Add("TraderAssignedReferenceID");
            argumentos.Add("UcrMasterLandingBillSequenceNumeric");
            argumentos.Add("UcrHouseLandingBillSequenceNumeric");
            argumentos.Add("TransportEquipmentCharacteristicCode");
            argumentos.Add("ConsignmentItemTransportEquipmentID");
            argumentos.Add("ConsignmentItemTransportEquipmentSealID");
            argumentos.Add("ConsignmentItemTransportEquipmentSealID");
            argumentos.Add("ConsignmentItemTransportEquipmentSealID");
            argumentos.Add("ConsignmentItemTransportEquipmentSealID");
            argumentos.Add("GoodsMeasureNetNetWeightMeasure");
            argumentos.Add("ConsignmentItemPackagingQuantityQuantity");
            argumentos.Add("ConsignmentItemAdditionalInformationContent");
            argumentos.Add("ConsignmentItemSequenceNumeric");
            argumentos.Add("DeclarationID");

            List<string> valores = new List<string>();
            valores.Add("001"); //quemado
            valores.Add("SLN");//quemado
            valores.Add("NULL"); 
            valores.Add(fechaFormato);
            valores.Add("05909025"); //quemado
            valores.Add(mrn);
            valores.Add(msn);
            valores.Add(hsn);
            valores.Add(iso);
            valores.Add(descripcionContainer);
            valores.Add(sello1);
            valores.Add(sello2);
            valores.Add(sello3);
            valores.Add(sello4);
            valores.Add("0");//quemado
            valores.Add("0");//quemado
            valores.Add("DIFERENCIA DE NUMERO DE SELLO");//quemado
            valores.Add(numeroSecuencia);
            valores.Add(codigoSolicitudTransaccionGenerada);
            
            saveArgumentsIMPDT(codigoSolicitudTransaccion, argumentos, valores);                           
        }

        public void saveArgumentsIMPDT(string codigoSolicitudTransaccion, List<string> argumentos, List<string> valores)
        {   
            for (int i = 0; i < argumentos.Count; i++)
            {
                CslHelperServicios.ingresoIMPDTARGU(codigoSolicitudTransaccion, argumentos[i].ToString(), valores[i].ToString());
            }
        }

        #region Funciones para enviar correos y crear sus cuerpos
            public void envioCorreo(int idSolicitud, int idDetalleSolicitud, usuario sUser)
            {
                string descripcionEstado = "";
                string descripcionServicio = "";
                string nombreUsuario = "";
                string correoUsuario = "";
                string codigoSolicitud = "";

                List<consultaCabeceraUsuario> solicitudUsuario = CslHelperServicios.consultaSolicitudUsuario(null, 0, null, null, null, null, false, null, idSolicitud.ToString());
                foreach (var datoSolicitud in solicitudUsuario)
                {
                    descripcionEstado = String.IsNullOrEmpty(datoSolicitud.estado) ? null : datoSolicitud.estado.ToString();
                    descripcionServicio = String.IsNullOrEmpty(datoSolicitud.servicio) ? null : datoSolicitud.servicio.ToString();
                    nombreUsuario = String.IsNullOrEmpty(datoSolicitud.nombreUsuario) ? null : datoSolicitud.nombreUsuario.ToString();
                    correoUsuario = String.IsNullOrEmpty(datoSolicitud.correoUsuario) ? null : datoSolicitud.correoUsuario.ToString();
                    codigoSolicitud = String.IsNullOrEmpty(datoSolicitud.codigoSolicitud) ? null : datoSolicitud.codigoSolicitud.ToString();
                }

                var jmsg = new jMessage();
                string mail = string.Empty;
                string destinatarios = turnoConsolidacion.GetMails();
                mail = string.Concat(mail, string.Format("Estimado/a {0} :<br/><br/>Este es un mensaje del Centro de Servicios en Linea de Contecon S.A, para comunicarle lo siguiente.<br/>A continuacion el detalle de su solicitud:<br/><br/>", nombreUsuario));
                mail = string.Concat(mail, string.Format("Se ha actualizado el servicio: <strong>{0}</strong><br/>Con código de solicitud: <strong>{1}</strong><br/><br/>", descripcionServicio, codigoSolicitud));

                //consulta todos los contenedores de la solicitud
                List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(0, null, idDetalleSolicitud.ToString());

                //Añade los n contenedores al correo.
                foreach (var item in listaDetalle)
                {
                    if (descripcionServicio.Equals("Verificación de Sellos")) {
                        mail = string.Concat(mail, string.Format("<strong>Contenedor: </strong>{0}<br/><strong> Observación: </strong>{1}<br/><strong>Tipo Verificación: </strong>{2}<br/>", item.descripcionContenedor, item.observacion, item.tipoVerificacion));
                    }
                    else {
                        mail = string.Concat(mail, string.Format("<strong>Contenedor: </strong>{0}<br/><strong> Observación: </strong>{1}<br/>", item.descripcionContenedor, item.observacion));
                    }
                }

                string error = string.Empty;
                var user_email = sUser.email;
           


                var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
                destinatarios = string.Format("{0}", correoBackUp);

                CLSDataCentroSolicitud.addMail(out error, sUser.email, "Actualización de contenedor.", mail, destinatarios, sUser.loginname, "", "");
                if (!string.IsNullOrEmpty(error))
                {
                    alerta.Visible = true;
                    alerta.InnerText = error;
                    return;
                }
            }

            public void envioCorreoSolicitud(int idSolicitud, usuario sUser)
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

                List<consultaCabeceraUsuario> solicitudUsuario = CslHelperServicios.consultaSolicitudUsuario(null, 0, null, null, null, null, false, null, idSolicitud.ToString());
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
                }

                var jmsg = new jMessage();
                string mail = string.Empty;
                string destinatarios = string.Empty;
                var user_email = correoUsuario;
                var correoBackUp = ConfigurationManager.AppSettings["correoBackUp"] != null ? ConfigurationManager.AppSettings["correoBackUp"] : "contecon.s3@gmail.com";
                
                switch (descripcionServicio)
                {
                    case "Reestiba":
                        mail = getBodyReestiba(solicitudUsuario, sUser);
                        destinatarios = string.Format("CGSA-Consolidaciones@cgsa.com.ec;YardPlanners@cgsa.com.ec;{0}", correoBackUp);
                        break;
                    case "Repesaje":
                        mail = getBodyRepesaje(solicitudUsuario, sUser);
                        destinatarios = string.Format("YardPlanners@cgsa.com.ec;{0}", correoBackUp);;
                        break;
                    case "Verificación de Sellos":
                        mail = getBodyVerificacion(solicitudUsuario, sUser);
                        destinatarios = string.Format("YardPlanners@cgsa.com.ec;{0}", correoBackUp);
                        break;
                    case "Etiquetado - Desetiquetado Unidades":
                        mail = getBodyEtiquetadoDes(solicitudUsuario, sUser);
                        destinatarios = string.Format("YardPlanners@cgsa.com.ec;{0}", correoBackUp);
                        break;
                    case "Revisión Técnica Refrigerada":
                        mail = getBodyRevisionTecnica(solicitudUsuario, sUser);
                        destinatarios = string.Format("yardplanners@cgsa.com.ec;{0}", correoBackUp);
                        break;
                    case "Late Arrival":
                        mail = getBodyLateArrival(solicitudUsuario, sUser);
                        destinatarios = string.Format("YardPlanners@cgsa.com.ec;{0}",correoBackUp);
                        break;
                }

                string mensaje = string.Empty;
                string clase = "";
                CLSDataCentroSolicitud.addMail(out mensaje, user_email, codigoSolicitud + " - Solicitud de " + descripcionServicio, mail,destinatarios, sUser.loginname, "", "");

                if (!string.IsNullOrEmpty(mensaje)) {
                    clase = "msg-critico";
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = clase;
                    this.sinresultado.InnerText = mensaje;
                    sinresultado.Visible = true;
                }
                else {
                    //clase = "msg-info";
                    Utility.mostrarMensaje(this.Page, "Datos actualizados con éxito. Se le notificará al usuario por correo el cambio de estado de la solicitud.");
                    //mensaje = string.Format("Datos actualizados con éxito. Se le notificará al usuario por correo el cambio de estado de la solicitud.");
                }

                //xfinder.Visible = false;
                /*this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = clase;
                this.sinresultado.InnerText = mensaje;
                sinresultado.Visible = true;*/
            }

            public string getBodyReestiba(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
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
                }

                //consulta todos los contenedores de la solicitud
                List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
                string contenedorUno = "";
                string contenedorDos = "";

                //Añade los n contenedores al correo.
                foreach (var item in listaDetalle)
                {
                    if (item.noFila == "1")
                    {
                        contenedorUno = item.descripcionContenedor.Trim();
                    }

                    if (item.noFila == "2")
                    {
                        contenedorDos = item.descripcionContenedor.Trim();
                    }
                }

                mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/><br/> De acuerdo a su requerimiento, le informamos que se ha programado la Solicitud de Reestiba de mercancías, de acuerdo a lo detallado a continuación: <br/><br/>", sUser.nombres, sUser.apellidos));
                mail = string.Concat(mail, string.Format("<b> Observación: </b> {0} <br/>", observacion));
                mail = string.Concat(mail, string.Format("<strong>Solicitud de reestiba SENAE: </strong>{0}<br/><strong>Tráfico: </strong>{1}<br/> <strong>Contenedor Lleno: </strong>{2}<br/><strong>Contenedor Vacío: </strong>{3}<br/><strong>Producto y embalaje: </strong>{4}<br/><strong>Fecha y hora propuesta: </strong>{5}<br/><strong>Comentarios: </strong>{6}<br/><br/>", "019013162015RE000001P", trafico, contenedorUno, contenedorDos, productoEmbalaje, fechaPropuesta, comentarios));

                mail = string.Concat(mail, "<br/><br/>Es un placer servirle, <br/><br/> Atentamente, <br/> <b>Terminal Virtual</b> <br/> Contecon Guayaquil S.A. CGSA <br/> An ICTSI Group Company <br/><br/> <b>El contenido de este mensaje es informativo, por favor no responda este correo.</b>");

                return mail;
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

                mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/><br/> De acuerdo a su solicitud, le informamos que se ha programado el repesaje de la(s) unidad(es) detalladas a continuación: <br/><br/>", sUser.nombres, sUser.apellidos));
                mail = string.Concat(mail, "<strong>INFORMACIÓN DE LA CARGA:</strong><br/><br/>");

                mail = string.Concat(mail, string.Format("<strong>Tráfico: </strong>{0}<br/>", trafico));

                if (!String.IsNullOrEmpty(numCarga))
                {
                    mail = string.Concat(mail, string.Format("<strong>Número de Carga: </strong>{0}<br/>", numCarga));
                }

                if (!String.IsNullOrEmpty(numBooking))
                {
                    mail = string.Concat(mail, string.Format("<strong>Número de Booking: </strong>{0}<br/>", numBooking));
                }

                mail = string.Concat(mail, string.Format("<strong>Estado: </strong>{0}<br/>", descripcionEstado));

                //consulta todos los contenedores de la solicitud
                List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);

                mail = string.Concat(mail, "<strong>CONTENEDOR(ES):</strong><br><br/>");

                //Añade los n contenedores al correo.
                foreach (var item in listaDetalle)
                {
                    mail = string.Concat(mail, string.Format("{0}<br/>", item.descripcionContenedor));
                }

                mail = string.Concat(mail, "<br/><br/>Sírvase verificar el estado de la misma en el transcurso de 24 a 48 horas. <br><br/>");

                mail = string.Concat(mail, "<br/><br/>Es un placer servirle, <br/> Atentamente, <br/> <b>Terminal Virtual</b> <br/> Contecon Guayaquil S.A. CGSA <br/> An ICTSI Group Company <br/><br/> <b>El contenido de este mensaje es informativo, por favor no responda este correo.</b>");

                return mail;
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

                mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/><br/> De acuerdo a su solicitud, le informamos que se ha programado la verificación de sellos de la(s) unidad(es) detalladas a continuación: <br/><br/>", sUser.nombres, sUser.apellidos));
                mail = string.Concat(mail, "<strong>INFORMACIÓN DE LA CARGA:</strong><br/><br/>");
                mail = string.Concat(mail, string.Format("<strong>Tráfico: </strong>{0}<br/>", trafico));

                if (!String.IsNullOrEmpty(numCarga))
                {
                    mail = string.Concat(mail, string.Format("<strong>Número de Carga: </strong>{0}<br/>", numCarga));
                }

                if (!String.IsNullOrEmpty(numBooking))
                {
                    mail = string.Concat(mail, string.Format("<strong>Número de Booking: </strong>{0}<br/>", numBooking));
                }

                mail = string.Concat(mail, string.Format("<strong>Estado: </strong>{0}<br/>", descripcionEstado));

                //consulta todos los contenedores de la solicitud
                List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
                mail = string.Concat(mail, "<br/><strong>CONTENEDOR(ES) ASOCIADO(S):</strong><br/>");

                //Añade los n contenedores al correo.
                foreach (var item in listaDetalle)
                {
                    mail = string.Concat(mail, string.Format("{0}<br/>", item.descripcionContenedor));
                }

                mail = string.Concat(mail, "<br/><br/>Sírvase verificar el estado de la misma en el transcurso de 24 a 48 horas.");
                mail = string.Concat(mail, "<br/><br/>Es un placer servirle, <br/> Atentamente, <br/> <b>Terminal Virtual</b> <br/> Contecon Guayaquil S.A. CGSA <br/> An ICTSI Group Company <br/><br/> <b>El contenido de este mensaje es informativo, por favor no responda este correo.</b>");

                return mail;
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

                mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br><br/> De acuerdo a su solicitud, le informamos que se ha programado la verificación de sellos de la(s) unidad(es) detalladas a continuación: <br><br/>", sUser.nombres, sUser.apellidos));
                mail = string.Concat(mail, "<strong>INFORMACIÓN DE LA CARGA:</strong><br><br/>");
                mail = string.Concat(mail, string.Format("<strong>Tráfico: </strong>{0}<br/>", trafico));

                if (!String.IsNullOrEmpty(numCarga))
                {
                    mail = string.Concat(mail, string.Format("<strong>Número de Carga: </strong>{0}<br/>", numCarga));
                }

                if (!String.IsNullOrEmpty(numBooking))
                {
                    mail = string.Concat(mail, string.Format("<strong>Número de Booking: </strong>{0}<br/>", numBooking));
                }

                mail = string.Concat(mail, string.Format("<strong>Estado: </strong>{0}<br/>", descripcionEstado));

                //consulta todos los contenedores de la solicitud
                List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
                mail = string.Concat(mail, "<br><br/><strong>CONTENEDOR(ES) ASOCIADO(S):</strong><br><br/>");

                //Añade los n contenedores al correo.
                foreach (var item in listaDetalle)
                {
                    mail = string.Concat(mail, string.Format("{0}<br><br/>", item.descripcionContenedor));
                }

                mail = string.Concat(mail, "<br><br/>Sírvase verificar el estado de la misma en el transcurso de 24 a 48 horas. </br>");
                mail = string.Concat(mail, "<br><br/>Es un placer servirle, <br><br/><br><br/> Atentamente, <br><br/> <b>Terminal Virtual</b> <br><br/> Contecon Guayaquil S.A. CGSA <br><br/> An ICTSI Group Company <br><br/><br><br/> <b>El contenido de este mensaje es informativo, por favor no responda este correo.</b>");

                return mail;
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

                //mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/><br/> De acuerdo a su solicitud, le informamos que se ha programado la revisión técnica refrigerada de la(s) unidad(es) detalladas a continuación: <br/><br/>", sUser.nombres, sUser.apellidos));
                //mail = string.Concat(mail, "<strong>INFORMACIÓN DE LA CARGA:</strong><br/><br/>");
                //mail = string.Concat(mail, string.Format("<strong>Tráfico: </strong>{0}<br/>", trafico));

                //if (!String.IsNullOrEmpty(numCarga))
                //{
                //    mail = string.Concat(mail, string.Format("<strong>Número de Carga: </strong>{0}<br/>", numCarga));
                //}

                //if (!String.IsNullOrEmpty(numBooking))
                //{
                //    mail = string.Concat(mail, string.Format("<strong>Número de Booking: </strong>{0}<br/>", numBooking));
                //}

                //mail = string.Concat(mail, string.Format("<strong>Estado: </strong>{0}<br/>", descripcionEstado));

                //consulta todos los contenedores de la solicitud
                List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);
                //mail = string.Concat(mail, "<br/><strong>CONTENEDOR(ES) ASOCIADO(S):</strong><br/><br/>");

                //Añade los n contenedores al correo.
                //foreach (var item in listaDetalle)
                //{
                //    mail = string.Concat(mail, string.Format("{0}<br/>", item.descripcionContenedor));
                //}

                //mail = string.Concat(mail, "<br/><br/><b>Nota:</b>Las unidades que no constan en este listado no fueron programadas, debido a que no se encuentran registradas en nuestro sistema.");
                //mail = string.Concat(mail, "<br/><br/>Sírvase verificar el estado de la solicitud en el transcurso de 6 horas.");
                //mail = string.Concat(mail, "<br/><br/>Es un placer servirle, <br/> Atentamente, <br/> <b>CSL - CENTRO DE SERVICIOS EN LINEA</b> <br/> Contecon Guayaquil S.A. CGSA <br/> An ICTSI Group Company <br/><br/> <b>El contenido de este mensaje es informativo, por favor no responda este correo.</b>");


                mail = Utility.tecnica_msg_proceso(listaDetalle.Select(u => u.descripcionContenedor).ToList(), trafico, descripcionEstado, numBooking, numCarga);

                return mail;
            }

            public string getBodyLateArrival(List<consultaCabeceraUsuario> solicitudUsuario, usuario sUser)
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
                string referencia = "";

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
                    referencia = String.IsNullOrEmpty(datoSolicitud.referencia) ? null : datoSolicitud.referencia.Trim();

                }


                mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/><br/>De acuerdo a su solicitud, le informamos que ha registrado la solicitud de late arrival para las unidades detalladas a continuación: <br/><br/>", sUser.nombres, sUser.apellidos));

                mail = string.Concat(mail, "<strong>INFORMACIÓN DE SOLICITUD:</strong><br/><br/>");
                mail = string.Concat(mail, string.Format("<strong>Referencia: </strong>{0}<br/>", referencia));
                mail = string.Concat(mail, string.Format("<strong>Estado: </strong>{0}<br/>", descripcionEstado));

                //consulta todos los contenedores de la solicitud
                List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null, "0", 0, "1");

                mail = string.Concat(mail, "<br/><b>CONTENEDOR(ES) PROGRAMADOS:</b><br/><br/>");

                //Añade los n contenedores al correo.
                foreach (var item in listaDetalle)
                {
                    mail = string.Concat(mail, string.Format("{0}<br/>", item.descripcionContenedor));
                }

                mail = string.Concat(mail, "<br/><br/>Todo contenedor debe ser pre avisado (AISV) para programar una solicitud de Late Arrival.");
                mail = string.Concat(mail, "<br/><br/><b>Los contenedores que no se detallan en la solicitud presentaron novedades al momento de generar la solicitud.</b>");
                mail = string.Concat(mail, "<br/><br/>Recordar que el límite máximo para ingreso de un contenedor con servicio de Late Arrival es hasta 4 horas posterior al cut off de la nave.");
                mail = string.Concat(mail, "<br/><br/>Cualquier consulta adicional envíe un correo a la(s) casilla(s): VslPlanners@cgsa.com.ec");

                mail = string.Concat(mail, "<br/><br/>Es un placer servirle, <br/> Atentamente, <br/> <b>Terminal Virtual</b> <br/> Contecon Guayaquil S.A. CGSA <br/> An ICTSI Group Company <br/><br/> <b>El contenido de este mensaje es informativo, por favor no responda este correo.</b>");

                return mail;
            }
        #endregion
    }
}