using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ConectorN4;
using System.Text;

namespace CSLSite
{
    public partial class listadoEmbarque : System.Web.UI.Page
    {
        public static usuario sUser = null;
        public static UsuarioSeguridad us;
        public List<contenedorBuque> GridContenedoresOperador
        {
            get { return (List<contenedorBuque>)Session["GridContenedoresOperador"]; }
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
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                Seguridad s = new Seguridad();
                us = s.consultarUsuarioPorId(sUser.id);
                sinresultado.Visible = false;
                alerta.Visible = false;
                btgenerar.Visible = false;
                divBotonera.Visible = false;

                if (us.tipoUsuario == ConfigurationManager.AppSettings["tipoAdministradorInterno"])
                {
                    Session["identificacionAgencia"] = "0";
                }
                else
                {
                    Session["identificacionAgencia"] = us.codigoEmpresa;
                }
            }
        }

        public void cargarContenedores()
        {
            //AQUI VA EL CARGADO DE LOS CONTAINERS EN LA PANTALLA 
              var u = this.getUserBySesion();
            List<contenedorBuque> table = CslHelperServicios.consultaBuquesContenedor(txtReferencia.Text.Trim(),u.codigoempresa);
          
            try
            {
                if (Response.IsClientConnected)
                {

                    DataTable dtEmbarque = new DataTable();
                    dtEmbarque.Columns.AddRange(new DataColumn[] { new DataColumn("Linea", typeof(string)), new DataColumn("Trafico", typeof(string)),
                                                                   new DataColumn("Unidad", typeof(string)), new DataColumn("Booking", typeof(string)),
                                                                    new DataColumn("DAE", typeof(string)), new DataColumn("Exportador", typeof(string)),
                                                                    new DataColumn("Tipo", typeof(string)), new DataColumn("Stuff", typeof(string)),
                                                                    new DataColumn("IN", typeof(string)), new DataColumn("Cliente AISV", typeof(string)),  new DataColumn("Tipo de Cliente", typeof(string)),
                                                                    new DataColumn("Tipo Roleo", typeof(string)), new DataColumn("Tipo Bloqueo", typeof(string)),
                                                                    new DataColumn("Falta de Pago", typeof(string)), new DataColumn("Asume/Garantiza costos", typeof(string)), new DataColumn("Tipo de Reporte", typeof(string))});



                    if (table.Count > 0)
                    {

                        foreach (contenedorBuque item in table)
                        {
                            dtEmbarque.Rows.Add(item.linea, item.trafico, item.unidad, item.booking, item.dae, item.exportador, item.tipo, item.stuff, item.ingreso, item.cliente_aisv, item.tipoCliente,
                                item.tipo_roleo, item.tipo_bloqueo, (item.bloqueo ? "Si" : "No"), (item.asumoCosto ? "Si" : "No"), txtReporte.Text.Trim().ToUpper());
                        }

                        Session["resultadoLista"] = dtEmbarque;

                        grvContenedores.PageIndex = 0;
                        this.grvContenedores.DataSource = table;
                        this.grvContenedores.DataBind();
                        GridContenedoresOperador = table;
                        hdfBooking.Value = "";
                        hdfContenedor.Value = "";
                        hdfExportador.Value = "";
                        hdfFaltaPago.Value = "T";
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        if (us.tipoUsuario == ConfigurationManager.AppSettings["tipoAdministradorInterno"])
                        {
                            btgenerar.Visible = false;
                            dataexport.Visible = true;
                        }
                        else
                        {
                            if (txtReporte.Text.Trim().ToUpper() == "DEFINITIVO")
                            {
                                btgenerar.Visible = false;
                                dataexport.Visible = true;
                            }
                            else
                            {
                                btgenerar.Visible = true;
                                dataexport.Visible = true;
                            }
                        }
                        divBotonera.Visible = true;

                        return;
                    }
                    else
                    {
                        btgenerar.Visible = false;
                        divBotonera.Visible = false;
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "No hay contenedores asociados a la referencia seleccionada.";
                    }
                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                }
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "referencias", "find_Click", txtReferencia.Text, u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
        }

        protected void btgenerar_Click(object sender, EventArgs e)
        {
            //SOLO SE GUARDAN LOS REGISTROS INHABILITADOS A LOS CUALES SE VA ASUMIR LOS COSTOS (CHECK EN TRUE)
            var u = this.getUserBySesion();
            try
            {

                this.sinresultado.Visible = false;
                this.sinresultado.InnerText = "";

                this.alerta.Visible = false;
                this.alerta.InnerText = "";
                actualizarDataTableSession();

                DataTable dtEmbarque = new DataTable();
                dtEmbarque.Columns.AddRange(new DataColumn[] { new DataColumn("Gkey", typeof(int)), new DataColumn("AsumoCosto", typeof(bool)) });

                List<contenedorBuque> contenedores = GridContenedoresOperador.Where(x => x.asumoCosto == true && x.habilitar == true).ToList();
                if (contenedores.Count > 0)
                {
                    string mailContenedores = string.Empty;
                    string errorDesbloqueos = string.Empty;
                    errorDesbloqueos="En estos contenedores no se pudo realizar el desbloqueo: \n";
                    int contadorErrores = 0;

                    var tk = HttpContext.Current.Request.Cookies["token"];
                    ConectorN4.ObjectSesion sesObj = new ObjectSesion();
                    sesObj.clase = "ListadoEmbarque";
                    sesObj.metodo = "btgenerar_Click";
                    sesObj.transaccion = "ListadoEmbarque";
                    sesObj.usuario = sUser.loginname;
                    sesObj.token = tk.Value;

                    foreach (contenedorBuque cb in contenedores)
                    {
                        string resultadoDesbloqueo = desbloquearN4(sesObj,cb.unidad);
                        if (resultadoDesbloqueo == "ok")
                        {
                            
                            dtEmbarque.Rows.Add(cb.gkey, cb.asumoCosto);
                            invocacionEvento(sesObj,cb.unidad);
                            mailContenedores = string.Concat(mailContenedores, string.Format("<strong>Unidad: </strong>{0}<strong>     DAE: </strong>{1}<strong>     Booking: </strong>{2}<strong>     Cliente: </strong>{3}<br/>", cb.unidad, cb.dae, cb.booking, cb.cliente_aisv));
                        }
                        else
                        {
                            contadorErrores++;
                            errorDesbloqueos += "Unidad: " + cb.unidad+ " Error: " + resultadoDesbloqueo;
                        }
                    }

                    if (dtEmbarque.Rows.Count > 0)
                    {
                        int resultado = CslHelperServicios.ingresoListadoEmbarque(dtEmbarque, txtReferencia.Text.Trim(), u.id);
                        if (resultado == 1)
                        {
                            var jmsg = new jMessage();
                            string mail = string.Empty;
      
                        
                            
                            var cfgs = dbconfig.GetActiveConfig(null, null, null);
                            var correoBackUp = cfgs.Where(a => a.config_name.Contains("correoBackUp")).FirstOrDefault();
                            var mail_em = cfgs.Where(n => n.config_name.Contains("mail_embarque")).FirstOrDefault();

                            mail = string.Concat(mail, string.Format("Estimado/a {0} {1}:<br/><br/>Este es un mensaje del Sistema de Solicitud de Servicios de Contecon Guayaquil S.A, para comunicarle lo siguiente:<br/><br/>A continuación el listado de contenedores asumidos o garantizados para embarque:<br/>", u.nombres, u.apellidos));
                            mail = string.Concat(mail, string.Format("<strong>Referencia: </strong>{0}<br/><strong>Nave: </strong>{1}<br/>", txtReferencia.Text.Trim(), txtNave.Text.Trim()));
                            mail = string.Concat(mail, string.Format("<br/><strong>Contenedores: </strong><br/>"));
                            mail = string.Concat(mail, mailContenedores);
                            mail = string.Concat(mail, "<br/><br/> Agradecemos notificar a la casilla exponavios@cgsa.com.ec los pagos respectivos de los servicios garantizados/asumidos hasta 48 horas hábiles siguientes. Luego del plazo indicado se emitirá la factura automáticamente a nombre de la Agencia Naviera.");
                            string error = string.Empty;
                            var destinatarios = string.Format("{0};{1}", correoBackUp!=null?correoBackUp.config_value:"no_cfg",mail_em!=null?mail_em.config_value:"no_cfg");
                            CLSDataCentroSolicitud.addMail(out error, sUser.email, "Lista de Embarque " + txtReferencia.Text.Trim() + "", mail, destinatarios, u.loginname, "", "");
  
                            if (!string.IsNullOrEmpty(error))
                            {
                                alerta.Visible = true;
                                alerta.InnerText = error;
                                return;
                            }
                            else
                            {
                                if (contadorErrores > 0)
                                {
                                    Utility.mostrarMensajeRedireccionando(this.Page, errorDesbloqueos, "../servicios/listadoembarque");
                                }
                                else
                                {
                                    Utility.mostrarMensajeRedireccionando(this.Page, "Se ha guardado con éxito los contenedores", "../servicios/listadoembarque");
                                }
                            }
                        }
                        else
                        {
                            Utility.mostrarMensaje(this.Page, "Ocurrió un error al querer guardar los contenedores");
                        }
                    }
                    else
                    {
                        Utility.mostrarMensajeRedireccionando(this.Page, errorDesbloqueos.Replace("\n","\\n").Replace("\r","\\r").Replace("'",""), "../servicios/listadoembarque");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "referencias", "find_Click", txtReferencia.Text, u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
                cargarContenedores();
        }

        protected void grvContenedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvContenedores.PageIndex = e.NewPageIndex;
            actualizarDataTableSession();
            bool bloqueo = hdfFaltaPago.Value.ToString() == "S" ? true : hdfFaltaPago.Value.ToString() == "N" ? false : false;
            this.grvContenedores.DataSource = GridContenedoresOperador.Where(x => x.booking.Trim().ToUpper().Contains(hdfBooking.Value.ToString()) && x.exportador.Trim().ToUpper().Contains(hdfExportador.Value.ToString()) && x.unidad.Trim().ToUpper().Contains(hdfContenedor.Value.ToString()) && (hdfFaltaPago.Value.ToString() == "T" || (hdfFaltaPago.Value.ToString() != "T" && x.bloqueo == bloqueo))).OrderBy(x => x.unidad).ToList();
            this.grvContenedores.DataBind();
        }

        public void actualizarDataTableSession()
        {

            foreach (GridViewRow item in grvContenedores.Rows)
            {
                Label _lblGkey = item.FindControl("lblGkey") as Label;
                CheckBox chckContenedor = item.FindControl("chkRow") as CheckBox;
                int gkey = int.Parse(_lblGkey.Text.Trim());
                contenedorBuque tempCont = new contenedorBuque();
                tempCont = GridContenedoresOperador.Where(x => x.gkey == gkey).FirstOrDefault();
                GridContenedoresOperador.Remove(tempCont);
                tempCont.asumoCosto = chckContenedor.Checked;
                GridContenedoresOperador.Add(tempCont);
            }
        }

        protected void btnBuscarContenedor_Click(object sender, EventArgs e)
        {
            hdfBooking.Value = txtBooking.Text.Trim().ToUpper();
            hdfContenedor.Value = txtContenedor.Text.Trim().ToUpper();
            hdfExportador.Value = txtExportador.Text.Trim().ToUpper();
            hdfFaltaPago.Value = ddlFaltaPago.SelectedValue;
            actualizarDataTableSession();
            bool bloqueo = ddlFaltaPago.SelectedValue == "S" ? true : ddlFaltaPago.SelectedValue == "N" ? false : false;
           
            this.grvContenedores.DataSource = GridContenedoresOperador.Where(x => x.booking.Trim().ToUpper().Contains(txtBooking.Text.Trim().ToUpper()) && x.exportador.Trim().ToUpper().Contains(txtExportador.Text.Trim().ToUpper()) && x.unidad.Trim().ToUpper().Contains(txtContenedor.Text.Trim().ToUpper()) && (ddlFaltaPago.SelectedValue == "T" || (ddlFaltaPago.SelectedValue != "T" && x.bloqueo == bloqueo))).OrderBy(x => x.unidad).ToList();
            this.grvContenedores.DataBind();
        }

        protected void grvContenedores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    Label _lblBloqueo = e.Row.FindControl("lblBloqueoBool") as Label;
                    Label _lblHabilitar = e.Row.FindControl("lblHabilitar") as Label;
                    bool bloqueo = bool.Parse(_lblBloqueo.Text.Trim());
                    bool habilitar = bool.Parse(_lblHabilitar.Text.Trim());

                    if (bloqueo && habilitar)
                    {
                        e.Row.BackColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        e.Row.BackColor = System.Drawing.Color.White;
                    }

                    if (us.tipoUsuario == ConfigurationManager.AppSettings["tipoAdministradorInterno"])
                    {
                        CheckBox _cbxCosto = e.Row.FindControl("chkRow") as CheckBox;
                        _cbxCosto.Enabled = false;
                    }
                    else
                    {
                        if (habilitar)
                        {
                            CheckBox _cbxCosto = e.Row.FindControl("chkRow") as CheckBox;
                            _cbxCosto.Enabled = true;
                        }
                    }

                    if (txtReporte.Text.Trim().ToUpper() == "DEFINITIVO")
                    {
                        CheckBox _cbxCosto = e.Row.FindControl("chkRow") as CheckBox;
                        _cbxCosto.Enabled = false;
                    }

                    if (bloqueo)
                    {
                        CheckBox _cbxCosto = e.Row.FindControl("chkRow") as CheckBox;
                        _cbxCosto.Visible = true;
                    }
                }
                finally
                {

                }
            }
        }

        protected void btnBuscarHF_Click(object sender, EventArgs e)
        {
            //grvContenedores.PageIndex = grvContenedores.PageIndex;
            actualizarDataTableSession();
            bool bloqueo = hdfFaltaPago.Value.ToString() == "S" ? true : hdfFaltaPago.Value.ToString() == "N" ? false : false;
            this.grvContenedores.DataSource = GridContenedoresOperador.Where(x => x.booking.Trim().ToUpper().Contains(hdfBooking.Value.ToString()) && x.exportador.Trim().ToUpper().Contains(hdfExportador.Value.ToString()) && x.unidad.Trim().ToUpper().Contains(hdfContenedor.Value.ToString()) && (hdfFaltaPago.Value.ToString() == "T" || (hdfFaltaPago.Value.ToString() != "T" && x.bloqueo == bloqueo))).OrderBy(x => x.unidad).ToList();
            this.grvContenedores.DataBind();
        }

        public string desbloquearN4(ObjectSesion sesObj, string contenedor)
        {
            wsN4 g = new wsN4();
            String a = string.Format("<hpu><entities><units><unit id=\"{0}\"/></units></entities><flags><flag hold-perm-id=\"{1}\" action=\"{2}\"/></flags></hpu>", contenedor.ToString(), "CGSA_EXPO_PAGO", "RELEASE_HOLD");
            string me = string.Empty;

            //Invocación del web service para desbloquear contenedor en el sistema N4.

            string errorValidacion = string.Empty;
            n4WebService n4s = new n4WebService();
            var i = n4s.InvokeN4Service(sesObj, a, ref errorValidacion, DateTime.Now.ToString("yyyyMMddHHmm"));
            
            if (i > 0)
            {
                return errorValidacion.Replace("\n", "").Replace("\r", "");
            }
            else
            {
                return "ok";
            }
 
        
        }

        public string invocacionEvento(ObjectSesion sesObj, string descripcionContenedor)
        {
            wsN4 g = new wsN4();

            string me = string.Empty;
            string errorN4 = string.Empty;

            StringBuilder newa = new StringBuilder();
            newa.Append(string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"/></units><properties><property tag=\"{1}\" value=\"Y\"/></properties></icu>", descripcionContenedor, ConfigurationManager.AppSettings["CostoN4"].ToString()));
            string errorValidacion = string.Empty;
            n4WebService n4s = new n4WebService();
            var i = n4s.InvokeN4Service(sesObj, newa.ToString(), ref errorValidacion, DateTime.Now.ToString("yyyyMMddHHmm"));

            /*I ES LA RESPUESTA QUE DEVUELVE EL N4 Y errorValidacion ES LA DESCRIPCION DEL MENSAJE DE ERROR*/
            if (i > 0)
            {
                errorN4 = errorValidacion.ToString(); ;
            }

            return errorN4;
        }

    }
}