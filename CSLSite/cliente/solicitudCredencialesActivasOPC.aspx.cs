using csl_log;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Globalization;

namespace CSLSite
{
    public partial class solicitudCredencialesActivasOPC : System.Web.UI.Page
    {
        private string v_empresaSeleccionada = string.Empty;
        private DataTable dtColaboradores;
        private string ChoferSelect = string.Empty;
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();

        public string rucempresa
        {
            get { return (string)Session["srucempresaOPC"]; }
            set { Session["srucempresaOPC"] = value; }
        }
        public string nomEmpresa
        {
            get { return (string)Session["snomempresaOPC"]; }
            set { Session["snomempresaOPC"] = value; }
        }

        public usuario usuaerioOPC
        {
            get { return (usuario)Session["UsuarioOPC"]; }
            set { Session["UsuarioOPC"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {

            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                //this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
                Response.Redirect("../login.aspx", false);
                return;
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
                usuaerioOPC = user;
                rucempresa = user.ruc;
                
                string error_consulta = string.Empty;
                var dtDatosRepLegal = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
                TxtEmpresa.Text = dtDatosRepLegal;
                IdTxtempresa.Value = dtDatosRepLegal; //rucempresa;
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
                dtColaboradores = new DataTable();
                string error_consulta = string.Empty;
                
                var dtDatosRepLegal = credenciales.GetConsultaDatosRepresentanteLegal(rucempresa);
                if (dtDatosRepLegal.Rows.Count == 0)
                {
                    var script = "<script language='JavaScript'>alert('No se encontraron datos del Representante Legal, consulte con su Empresa.');</script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
                    return;
                }
                //TxtEmpresa.Text = dtDatosRepLegal.Rows[0]["NOMREPLEGAL"].ToString();
                //IdTxtempresa.Value = dtDatosRepLegal.Rows[0]["CIREPLEGAL"].ToString();

                dtColaboradores.Columns.Add("ID");
                dtColaboradores.Columns.Add("APELLIDOS");
                dtColaboradores.Columns.Add("NOMBRES");
                dtColaboradores.Columns.Add("TIPO");
                dtColaboradores.Columns.Add("CEDULA");
                dtColaboradores.Columns.Add("EMPRESA");
                dtColaboradores.Columns.Add("AREA");
                dtColaboradores.Columns.Add("CARGO");
               
                tablePagination.DataSource = dtColaboradores;
                tablePagination.DataBind();

                Session["ListaColaboradoresOPC"] = dtColaboradores;
            }
        }

        public static DataTable ObtenerCompanias(string empresaStr)
        {
            string error_consulta = string.Empty;
            DataTable dt = new DataTable();
            var onlyControl = new OnlyControl.OnlyControlService();
            dt = onlyControl.AC_C_EMPRESA("%"+ empresaStr+"%", 1, ref error_consulta).Tables[0];// onlyControl.AC_C_PERSONA(empresaStr, "%", 1, ref error_consulta).Tables[0];
            return dt;
        }

        public static DataTable ObtenerColaboradores(string empresaStr)
        {
            string error_consulta = string.Empty;
            DataTable dt = new DataTable();
            var onlyControl = new OnlyControl.OnlyControlService();
            dt = onlyControl.AC_C_PERSONA(empresaStr, "%", 1, ref error_consulta).Tables[0];
            return dt;
        }

        [System.Web.Services.WebMethod]
        public static string[] GetEmpresas(string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                string error_consulta = string.Empty;

                DataTable dt = ObtenerCompanias(prefix);

                foreach (DataRow dr in dt.Select("EMPE_NOM LIKE '%" + prefix+"%'") )
                {
                    StringResultado.Add(string.Format("{0}+{1}", dr["EMPE_NOM"], dr["EMPE_RUC"] + " " +  dr["EMPE_NOM"]));
                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        [System.Web.Services.WebMethod]
        public static string[] GetColaborador(string prefix, string empresa)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(empresa))
                {
                    throw new Exception("Debe seleccionar la empresa");
                }
                string error_consulta = string.Empty;

                DataTable dt = ObtenerColaboradores(empresa);

                foreach (DataRow dr in dt.Select("APELLIDOS LIKE '%" + prefix + "%'"))
                {
                    StringResultado.Add(string.Format("{0}+{1}", dr["ID"], dr["ID"] + " - " + dr["CEDULA"] + " - " + dr["APELLIDOS"] + " | " + dr["NOMBRES"]));
                }

                //var onlyControl = new OnlyControl.OnlyControlService();
                ////dt = onlyControl.AC_C_PERSONA(empresaStr, "%", 1, ref error_consulta).Tables[0];
                //var A  = onlyControl.AC_C_PERSONA
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

               
        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)
            {
                this.banmsg.Attributes["class"] = string.Empty;
                this.banmsg.Attributes["class"] = "alert alert-info";

                this.banmsg_det.Attributes["class"] = string.Empty;
                this.banmsg_det.Attributes["class"] = "alert alert-info";
                //this.sinresultado.InnerText = "Se debe seleccionar un deposito ";
            }
            else
            {
                this.banmsg.Attributes["class"] = string.Empty;
                this.banmsg.Attributes["class"] = "alert alert-danger";

                this.banmsg_det.Attributes["class"] = string.Empty;
                this.banmsg_det.Attributes["class"] = "alert alert-danger";
                //this.sinresultado.InnerText = "Se debe seleccionar un deposito ";
            }
            //class="alert alert-danger"
            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.Actualiza_Paneles();
        }

        private void Actualiza_Paneles()
        {
            UPCAB.Update();
            UPDET.Update();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                this.Ocultar_Mensaje();

                if (string.IsNullOrEmpty(TxtChofer.Text))
                {
                    this.Alerta("Debe seleccionar un colaborador. ");
                    this.Mostrar_Mensaje(2, "<b>Informativo!</b> Debe seleccionar un colaborador valido para agregar a la lista");
                    return;
                }

                string error_consulta = string.Empty;
                string IdChofer = string.Empty;
                string CedChofer = string.Empty;
                string DesChofer = string.Empty;
                string Apellido = string.Empty;
                string Nombre = string.Empty;
                string v_select = string.Empty;

                v_select = txtColaborador.Value;
                TxtChofer.Text = string.Empty;
                txtColaborador.Value = string.Empty;

                if (v_select.Split('-').ToList().Count > 1)
                {
                    IdChofer = v_select.Split('-').ToList()[0].Trim();
                    CedChofer = v_select.Split('-').ToList()[1].Trim();
                    DesChofer = v_select.Split('-').ToList()[2].Trim();
                    Apellido = DesChofer.Split('|').ToList()[0].Trim();
                    Nombre = DesChofer.Split('|').ToList()[1].Trim();
                }
                else
                {
                    this.Alerta("No se ha seleccionado un colaborador. ");
                    this.Mostrar_Mensaje(2, "<b>Informativo!</b> Debe seleccionar un colaborador valido para agregar a la lista");
                    return;
                }

                if (string.IsNullOrEmpty(CedChofer))
                {
                    this.Alerta("Colaborador no tiene identificación registrada");
                    this.Mostrar_Mensaje(2, "<b>Informativo!</b> Colaborador no tiene identificación registrada");
                    return;
                }

                //VALIDA CADUCIDAD DE PERMISO
                var dtEstado = credenciales.GetConsultaAc_Nomina("select case when NOMINA_FCARD <= getdate() then convert(bit,1) else convert(bit,0) end as estado from [dbo].[NOMINA] where nomina_id=", IdChofer);
                bool v_caducado = true ;
                if (!(dtEstado is null))
                {
                    if (dtEstado.Rows.Count > 0)
                    {
                        var valor = dtEstado.Rows[0].ItemArray[0];
                        v_caducado = (bool)valor;
                    }
                }

                if (v_caducado)
                {
                    this.Alerta("Colaborador tiene su credencial caducada");
                    this.Mostrar_Mensaje(2, "<b>Informativo!</b> Colaborador tiene su credencial caducada");
                    return;
                }

                dtColaboradores = Session["ListaColaboradoresOPC"] as DataTable;

                if (dtColaboradores is null)
                {
                    this.PersonalResponse("Sesion Caducada", "../login.aspx", true);
                    return;
                }

                if (dtColaboradores.Select("CEDULA='" + CedChofer + "'").Count() > 0)
                {
                    this.Alerta("Colaborador ya esta agregado en la lista");
                    this.Mostrar_Mensaje(2, "<b>Informativo!</b> Colaborador ya esta agregado en la lista");
                    return;
                }

                DataRow dr = dtColaboradores.NewRow();
                dr["ID"] = IdChofer;
                dr["APELLIDOS"] = Apellido;
                dr["NOMBRES"] = Nombre;
                dr["TIPO"] = "";
                dr["CEDULA"] = CedChofer;
                dr["EMPRESA"] = rucempresa;
                dr["AREA"] = "";
                dr["CARGO"] = "";
                dtColaboradores.Rows.Add(dr);

                tablePagination.DataSource = dtColaboradores;
                tablePagination.DataBind();

                Session["ListaColaboradoresOPC"] = dtColaboradores;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
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

                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechaing;
                if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaing))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER <strong>&nbsp;dia/Mes/Anio {0}</strong>", txtfecing.Text));
                    txtfecing.Focus();
                    return;
                }
                DateTime fechacad;
                if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacad))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER <strong>&nbsp;dia/Mes/Anio {0}</strong>", txtfecsal.Text));
                    txtfecsal.Focus();
                    return;
                }
                TimeSpan tsDias = fechacad - fechaing;
                int diferenciaEnDias = tsDias.Days;
                if (diferenciaEnDias < 0)
                {
                    this.Alerta("La Fecha de Ingreso: " + txtfecing.Text + "\\nNO deber ser mayor a la\\nFecha de Caducidad: " + txtfecsal.Text);
                    return;
                }


                dtColaboradores = Session["ListaColaboradoresOPC"] as DataTable;
                dtColaboradores.TableName = "Colaboradores";
                if (dtColaboradores.Rows.Count <= 0)
                {
                    this.Alerta("Agregue al menos un Colaborador.");
                    return;
                }

                string mensaje = null;
                var usr = Session["UsuarioOPC"] as usuario;
                string nombreempresa = CslHelper.getShiperName(rucempresa);

                var dtDatosRepLegal = credenciales.GetConsultaDatosRepresentanteLegal(rucempresa);
                if (dtDatosRepLegal.Rows.Count == 0)
                {
                    this.Alerta("No se encontraron datos del Representante Legal, consulte con su Empresa.");
                    return;
                }
                var txtusuariosolicita = dtDatosRepLegal.Rows[0]["NOMREPLEGAL"].ToString();
                var txtci = dtDatosRepLegal.Rows[0]["CIREPLEGAL"].ToString();
                var useremail = usr.email;

                StringWriter strColaboradores = new StringWriter();
                dtColaboradores.WriteXml(strColaboradores);
                string xmlColaboradores = strColaboradores.ToString();

                if (!credenciales.AddSolicitudOPC(
                   nombreempresa,
                   txtci,
                   txtusuariosolicita,
                   fechaing.ToString("yyyy-MM-dd"),
                   fechacad.ToString("yyyy-MM-dd"),
                   useremail,
                   "",//cbltiposolicitud.SelectedValue,
                   rucempresa,
                   "",//ddlAreaOnlyControl.SelectedItem.Text,
                   "",//ddlActividadOnlyControl.SelectedItem.Text,
                   xmlColaboradores,
                   Page.User.Identity.Name,
                   out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    this.Alerta("Solicitud registrada exitosamente");

                    this.Alerta("Solicitud registrada exitosamente ");
                    this.Mostrar_Mensaje(1, "<b>Informativo!</b> Solicitud registrada exitosamente");
                    this.btnSalvar.Attributes["disabled"] = "disabled";
                    Actualiza_Paneles();
                    return;
                    //var script = "<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>";
                    //scriptAlert(script);
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaboradorprovisional.aspx.cs", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                //var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                //scriptAlert(script);
                this.Alerta(string.Format("Algo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
           
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Ocultar_Mensaje();
            IdTxtChofer.Value = string.Empty;
            TxtChofer.Text = string.Empty;
            dtColaboradores = Session["ListaColaboradoresOPC"] as DataTable;
            if (dtColaboradores != null) { dtColaboradores.Clear(); }
            Session["ListaColaboradoresOPC"] = dtColaboradores;
            //this.btnSalvar.Attributes["disabled"] = "disabled";
            tablePagination.DataSource = dtColaboradores;
            tablePagination.DataBind();

            this.btnSalvar.Attributes.Remove("disabled");
            Actualiza_Paneles();
        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //msjErrores.InnerHtml = "";
            //msjErrores.Visible = false;
            //UPFotos.Update();
            try
            {
                /*valida cotizacion*/
                var usn = new usuario();
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                usn = this.getUserBySesion();

                if (usn == null)
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "pases_zal", "RepeaterBooking_ItemCommand", "No usuario", Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
                if (token == null)
                {
                    this.Alerta("Estimado Cliente,Su sesión ha expirado, sera redireccionado a la pagina de login", true);
                    System.Web.Security.FormsAuthentication.SignOut();
                    Session.Clear();
                    return;
                }


                if (e.CommandName == "Quitar")
                {
                    var v_argumentos = e.CommandArgument.ToString().Split(',');
                    var codigo = v_argumentos[0].ToString();
                    var cedula = v_argumentos[1].ToString();

                    dtColaboradores = Session["ListaColaboradoresOPC"] as DataTable;
                    dtColaboradores.Rows.Remove(dtColaboradores.Select("CEDULA = '" + cedula + "'").FirstOrDefault());

                    tablePagination.DataSource = dtColaboradores;
                    tablePagination.DataBind();

                    Session["ListaColaboradoresOPC"] = dtColaboradores;
                }
                //UPFotos.Update();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "RepeaterBooking_ItemCommand()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }
    }
}