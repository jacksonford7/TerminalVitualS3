using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using csl_log;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace CSLSite.cliente
{
    public partial class solicitudpermisodeacceso : System.Web.UI.Page
    {
        private string _Id_Opcion_Servicio = string.Empty;

        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        private String xmlDocumentos;
        public DataTable dtNominaOnyControl
        {
            get { return (DataTable)Session["dtNominaOnyControlsolicitudpermisodeacceso"]; }
            set { Session["dtNominaOnyControlsolicitudpermisodeacceso"] = value; }
        }
        public DataTable dtColaboradores
        {
            get { return (DataTable)Session["dtSolicitudColaboradoressolicitudpermisodeacceso"]; }
            set { Session["dtSolicitudColaboradoressolicitudpermisodeacceso"] = value; }
        }
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentossolicitudpermisodeacceso"]; }
            set { Session["dtDocumentossolicitudpermisodeacceso"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["rucempresadatossolicitudcolaborador"]; }
            set { Session["rucempresadatossolicitudcolaborador"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailsolicitudpermisodeacceso"]; }
            set { Session["useremailsolicitudpermisodeacceso"] = value; }
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
                rucempresa = user.ruc;
                useremail = user.email;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();

                //para que puedar dar click en el titulo de la pantalla y regresar al menu principal de la zona
                //_Id_Opcion_Servicio = Request.QueryString["opcion"];
                //this.opcion_principal.InnerHtml = string.Format("<a href=\"../cuenta/subopciones.aspx?opcion={0}\">{1}</a>", _Id_Opcion_Servicio, "Permisos");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    ////resultado.Visible = false;
                    //txtRucCiPas.Text = rucempresa;
                    ConsultaNomina();
                    var error_consulta = string.Empty;
                    var actividadOnlyControl = credenciales.GetActividadPermitidaPermisoDeAcceso();
                    if (!string.IsNullOrEmpty(error_consulta))
                    {
                        Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                    }
                    populateDropDownList(ddlActividadOnlyControl, actividadOnlyControl, "* Elija *", "TIPO", "DESCRIPCION", false);
                    ddlActividadOnlyControl.SelectedValue = "0";
                    //hfpermisopermanente.Value = credenciales.GetTiempoPermisoPermanente();
                    //txtarea.Text = "";
                    txtfecing.Text = "";
                    txtfecsal.Text = "";
                    dtColaboradores = new DataTable();
                    dtColaboradores.Columns.Add("Cedula");
                    dtColaboradores.Columns.Add("NombresApellidos");
                    dtColaboradores.Columns.Add("Nombres");
                    dtColaboradores.Columns.Add("Apellidos");
                    dtColaboradores.Columns.Add("Area");
                    dtColaboradores.Columns.Add("FechaInicio");
                    dtColaboradores.Columns.Add("FechaCaducidad");
                    dtColaboradores.Columns.Add("IdPermiso");
                    dtColaboradores.Columns.Add("Permiso");
                    dtColaboradores.Columns.Add("Cargo");
                    dtColaboradores.Columns.Add("Nota");
                    gvColaboradores.DataSource = dtColaboradores;
                    gvColaboradores.DataBind();
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "mostrarsecexp();", true);
                    ////btnbuscardoc.Focus();
                    txtfecing.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtfecsal.Text = DateTime.Now.AddDays(+1).ToString("dd/MM/yyyy");
                }
                catch (Exception ex)
                {
                    //txtarea.Text = "";
                    txtfecing.Text = "";
                    txtfecsal.Text = "";
                    dtColaboradores = new DataTable();
                    dtColaboradores.Columns.Add("Cedula");
                    dtColaboradores.Columns.Add("NombresApellidos");
                    dtColaboradores.Columns.Add("Nombres");
                    dtColaboradores.Columns.Add("Apellidos");
                    dtColaboradores.Columns.Add("Area");
                    dtColaboradores.Columns.Add("FechaInicio");
                    dtColaboradores.Columns.Add("FechaCaducidad");
                    dtColaboradores.Columns.Add("IdPermiso");
                    dtColaboradores.Columns.Add("Permiso");
                    dtColaboradores.Columns.Add("Cargo");
                    dtColaboradores.Columns.Add("Nota");
                    gvColaboradores.DataSource = dtColaboradores;
                    gvColaboradores.DataBind();
                    var number = log_csl.save_log<Exception>(ex, "solicitudpermisodeacceso.aspx.cs", "Page_Load()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                    var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                    scriptAlert(script);
                }
            }
        }
        private void populateDropDownList(DropDownList dp, DataTable origen, string mensaje, string id, string descripcion, bool val)
        {
            if (val)
            {
                origen.Rows.Add("0", "0", mensaje);
            }
            else
            {
                origen.Rows.Add("0", mensaje);
            }
            DataView dvorigen = new DataView();
            dvorigen = origen.DefaultView;
            dvorigen.Sort = descripcion;
            dp.DataSource = dvorigen;
            dp.DataValueField = id;
            dp.DataTextField = descripcion;
            dp.DataBind();
        }
        private void ConsultaNomina()
        {
            string error = string.Empty;
            var nominaOnlyControl = credenciales.GetNominaOnlyControl(rucempresa, out error);
            dtNominaOnyControl = nominaOnlyControl;
            if (string.IsNullOrEmpty(error) || nominaOnlyControl.Rows.Count > 0)
            {
                //populateDropNomina(ddlNominaOnlyControl, nominaOnlyControl);
                //if (ddlNominaOnlyControl.Items.Count > 0)
                //{
                //    if (ddlNominaOnlyControl.Items.FindByValue("000") != null)
                //    {
                //        ddlNominaOnlyControl.Items.FindByValue("000").Selected = true;
                //    }
                //    ddlNominaOnlyControl.SelectedItem.Text = "* Elija un Colaborador *";
                //}
            }
        }
        private void populateDropNomina(DropDownList dp, DataTable origen)
        {
            var resultNominaOnlyControl = from myRow in origen.AsEnumerable()
                                          orderby myRow.Field<string>("APELLIDOS") + " " + myRow.Field<string>("NOMBRES") ascending
                                          select new 
                                          {
                                              id=myRow.Field<string>("CEDULA"),
                                              name = myRow.Field<string>("CEDULA") + " - "+ myRow.Field<string>("APELLIDOS") + " " + myRow.Field<string>("NOMBRES")
                                          };
            origen.Rows.Add("", "* Elija un Colaborador *", "", "", "");
            dp.DataSource = resultNominaOnlyControl;
            dp.DataValueField = "id";
            dp.DataTextField = "name";
            dp.DataBind();
        }

        private void populateDropPermiso(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }
        //protected void btbuscar_Click(object sender, EventArgs e)
        //{
        //    if (Response.IsClientConnected)
        //    {
        //        try
        //        {
        //            ConsultaInformacion();
        //        }
        //        catch (Exception ex)
        //        {
        //            var t = this.getUserBySesion();
        //            sinresultado.Attributes["class"] = string.Empty;
        //            sinresultado.Attributes["class"] = "msg-critico";
        //            sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
        //            sinresultado.Visible = true;
        //        }
        //    }
        //}
        //private void ConsultaInformacion()
        //{
        //    if (HttpContext.Current.Request.Cookies["token"] == null)
        //    {
        //        System.Web.Security.FormsAuthentication.SignOut();
        //        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        //    }
        //    tablePagination.DataSource = null;
        //    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
        //    if (
        //            string.IsNullOrEmpty(txtfecing.Text) &&
        //            string.IsNullOrEmpty(txtfecsal.Text) &&
        //            ddlNominaOnlyControl.SelectedValue.ToString() == "0"
        //            )
        //        {
        //            this.alerta.InnerHtml = string.Format("<strong>&nbsp;Seleccione al menos un criterio de consulta. {0}</strong>", "");
        //            sinresultado.Visible = false;
        //            return;
        //    }
        //    CultureInfo enUS = new CultureInfo("en-US");
        //    if (!string.IsNullOrEmpty(txtfecing.Text))
        //    {
        //        DateTime fechadesde;
        //        if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechadesde))
        //        {
        //            this.alerta.InnerHtml = string.Format("<strong>&nbsp;El formato de la fecha desde debe ser: dia/Mes/Anio {0}</strong>", txtfecing.Text);
        //            txtfecing.Focus();
        //            return;
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(txtfecsal.Text))
        //    {
        //        DateTime fechahasta;
        //        if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechahasta))
        //        {
        //            this.alerta.InnerHtml = string.Format("<strong>&nbsp;El formato de la fecha hasta debe ser: dia/Mes/Anio {0}</strong>", txtfecsal.Text);
        //            txtfecsal.Focus();
        //            return;
        //        }
        //    }
        //    //var tablix = credenciales.GetPermisoColaborador(txtsolicitud.Text.Trim(), txtColaborador.Text.Trim(), rucempresa, txtfecing.Text, txtfecsal.Text, chkTodos.Checked);
        //    //tablePagination.DataSource = tablix;
        //    //tablePagination.DataBind();
        //    foreach (RepeaterItem item in tablePagination.Items)
        //    {
        //        Label lruccipas = item.FindControl("lruccipas") as Label;
        //        Label lempresa = item.FindControl("lempresa") as Label;
        //        var t = CslHelper.getShiperName(lruccipas.Text);
        //        lempresa.Text = t;
        //    }
        //    //xfinder.Visible = true;
        //    sinresultado.Visible = false;
        //    alerta.Visible = true;
        //    resultado.Visible = true;
        //}
        protected void gvColaboradores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        protected void gvColaboradores_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                DataTable myDataTable = Session["dtSolicitudColaboradoressolicitudpermisodeacceso"] as DataTable;
                var result = from myRow in myDataTable.AsEnumerable()
                             where myRow.Field<string>("Cedula") != dtColaboradores.Rows[e.RowIndex][3].ToString()
                             select myRow;
                if (result.AsDataView().Count > 0)
                {
                    Session["dtSolicitudColaboradoressolicitudpermisodeacceso"] = result.AsDataView().ToTable();
                }
                else if (result.AsDataView().Count == 0)
                {
                    Session["dtSolicitudColaboradoressolicitudpermisodeacceso"] = new DataTable();
                }
                DataTable dt = Session["dtSolicitudColaboradoressolicitudpermisodeacceso"] as DataTable;
                dtColaboradores.Rows.RemoveAt(e.RowIndex);
                gvColaboradores.DataSource = dtColaboradores;
                gvColaboradores.DataBind();
                txtci.Text = "";
                txtNombres.Text = "";
                txtApellidos.Text = "";
                //ddlActividadOnlyControl.SelectedValue = "0";
                //var tablix = credenciales.GetDocumentosOtros(ddlActividadOnlyControl.SelectedValue);
                //tablePaginationDocumentos.DataSource = tablix;
                //tablePaginationDocumentos.DataBind();
                txtcriterioconsulta.Focus();
                txtfecing.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtfecsal.Text = DateTime.Now.AddDays(+1).ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudpermisodeacceso.aspx.cs", "gvColaboradores_RowDeleting()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                txtci.BackColor = Color.White;
                txtNombres.BackColor = Color.White;
                txtApellidos.BackColor = Color.White;
                var nominap = credenciales.GetConsultaNominaPermiso(rucempresa,  txtci.Text);
                if (nominap.Rows[0]["MENSAJE"].ToString() == "NUEVO" || nominap.Rows[0]["MENSAJE"].ToString() == "SEMPRESA" || nominap.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                {
                    string tipo = "";
                    if (nominap.Rows[0]["MENSAJE"].ToString() == "SEMPRESA")
                    {
                        this.Alerta("El Colaborador(a):\\n *" + txtci.Text + " - " + nominap.Rows[0]["NOMBRES"].ToString() + "\\nEmpresa:\\n *" + nominap.Rows[0]["EMPE_NOM"].ToString() + "\\nTiene la credencial caducada.\\n" + /*+ "Fecha Inicio: " + Convert.ToDateTime(nominap.Rows[0]["NOMINA_FING"]).ToString("dd-MM-yyyy") + "\\n *Fecha Caducidad: " + Convert.ToDateTime(nominap.Rows[0]["NOMINA_FCARD"]).ToString("dd-MM-yyyy") +*/
                                    "Por favor realizar una Solicitud de Emisión/Renovación de Credencial.");
                        txtci.Text = "";
                        txtNombres.Text = "";
                        txtApellidos.Text = "";
                        //txtarea.Text = "";
                        ddlActividadOnlyControl.SelectedValue = "0";
                        txtcriterioconsulta.Focus();
                        txtfecing.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtfecsal.Text = DateTime.Now.AddDays(+1).ToString("dd/MM/yyyy");
                        return;
                    }
                    //else if (nominap.Rows[0]["MENSAJE"].ToString() == "NUEVO" || nominap.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                    //{
                    //    tipo = "1"; //Emisión
                    //}                    
                }
                else if (nominap.Rows[0]["MENSAJE"].ToString() == "SVIGENTE_SEMPRESA" || nominap.Rows[0]["MENSAJE"].ToString() == "SVIGENTE_NEMPRESA")
                {
                    if (nominap.Rows[0]["TURN_FECI"].ToString() != "" && nominap.Rows[0]["TURN_FECF"].ToString() != "")
                    {
                        this.Alerta("El Colaborador(a):\\n *" + txtci.Text + " - " + nominap.Rows[0]["NOMBRES"].ToString() + "\\nEmpresa:\\n *" + nominap.Rows[0]["EMPE_NOM"].ToString() + "\\nTiene un permiso de acceso vigente."); /*+ "Fecha Inicio: " + Convert.ToDateTime(nominap.Rows[0]["TURN_FECI"]).ToString("dd-MM-yyyy") + "\\n *Fecha Caducidad: " + Convert.ToDateTime(nominap.Rows[0]["TURN_FECF"]).ToString("dd-MM-yyyy") + "\\nContáctese con el Dpto. de Credenciales:\\n *046006300 Ext: 6004-6005-6007-6009\\n *PermisosyCredenciales@cgsa.com.ec");*/
                        txtci.Text = "";
                        txtNombres.Text = "";
                        txtApellidos.Text = "";
//                        txtarea.Text = "";
                        ddlActividadOnlyControl.SelectedValue = "0";
                        var tablix = credenciales.GetDocumentosOtros(ddlActividadOnlyControl.SelectedValue);
                        tablePaginationDocumentos.DataSource = tablix;
                        tablePaginationDocumentos.DataBind();
                        txtcriterioconsulta.Focus();
                        txtfecing.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtfecsal.Text = DateTime.Now.AddDays(+1).ToString("dd/MM/yyyy");
                        return;
                    }
                    //this.Alerta("El Colaborador:\\n *" + ddlNominaOnlyControl.SelectedItem.Value.ToString() + " - " + nominap.Rows[0]["NOMBRES"].ToString() + "\\nEmpresa:\\n *" + nominap.Rows[0]["EMPE_NOM"].ToString() + "\\nTiene una credencial vigente:\\n *" + "Fecha Inicio: " + Convert.ToDateTime(nominap.Rows[0]["NOMINA_FING"]).ToString("dd-MM-yyyy") + "\\n *Fecha Caducidad: " + Convert.ToDateTime(nominap.Rows[0]["NOMINA_FCARD"]).ToString("dd-MM-yyyy") + "\\nContáctese con el Dpto. de Credenciales:\\n *046006300 Ext: 6004-6005-6007-6009\\n *PermisosyCredenciales@cgsa.com.ec");
                    //return;
                }

                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fecing;
                if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecing))
                {
                    this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecing.Text));
                    txtfecing.Focus();
                    return;
                }
                DateTime facsal;
                if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out facsal))
                {
                    this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecsal.Text));
                    txtfecsal.Focus();
                    return;
                }
                Boolean validasolicitud = false;
                //RegistraPermisoPermanenteOnlyControl(out validasolicitud);
                TimeSpan tsDias = facsal - fecing;
                int diferenciaEnDias = tsDias.Days;
                if (diferenciaEnDias < 0)
                {
                    this.Alerta("La Fecha de Ingreso: " + txtfecing.Text + "\\nNO deber ser mayor a la\\nFecha de Caducidad: " + txtfecsal.Text);
                    validasolicitud = true;
                }
                if (!validasolicitud)
                {
                    var results = from myRow in dtColaboradores.AsEnumerable()
                                  where myRow.Field<string>("Cedula") == txtci.Text
                                  select new
                                  {
                                      Cedula = myRow.Field<string>("Cedula")
                                  };
                    foreach (var item in results)
                    {
                        if (item.Cedula == txtci.Text)
                        {
                            this.Alerta("Ya se agrego el CI: " + txtci.Text + ", revise por favor.");
                            return;
                        }
                    }
                    var cedulaconlaborador = txtci.Text;
                    var nomina = credenciales.GetConsultaNomina(credenciales.GetConsultaScriptNomina(), cedulaconlaborador);
                    if (nomina.Rows.Count > 0)
                    {
                        for (int n = 0; n < nomina.Rows.Count; n++)
                        {
                            var nompuerta = credenciales.GetConsultaNom_Puerta(nomina.Rows[n]["NOMINA_ID"].ToString());
                            if (nompuerta.Rows.Count > 0)
                            {
                                for (int p = 0; p < nompuerta.Rows.Count; p++)
                                {
                                    this.Alerta("El Colaborador: " + txtci.Text + ", tiene un permiso de acceso vigente:\\n" + "Fecha Inicio: " + nompuerta.Rows[p]["TURN_FECI"].ToString() + "\\nFecha Caducidad: " + nompuerta.Rows[p]["TURN_FECF"].ToString());
                                    return;
                                }
                            }
                            var fechaing = Convert.ToDateTime(nomina.Rows[n]["NOMINA_FING"]);
                            var fechacad = Convert.ToDateTime(nomina.Rows[n]["NOMINA_FCARD"]);
                            int diferenciaEnAños = fechacad.Year - fechaing.Year;
                            diferenciaEnAños = fechacad.Year - fechaing.Year;
                            TimeSpan tsTiempo = fechacad - fechaing;
                            int tiempoCaducidad = tsTiempo.Days;
                            if (tiempoCaducidad > credenciales.GetTiempoCaducidadCredencialPermanente())
                            {
                                this.Alerta("Su credencial esta caducada, por favor realice una solicitud de\\nEmisión/Renovación de credencial.\\n" + "Fecha Inicio: " + nomina.Rows[n]["NOMINA_FING"].ToString() + "\\nFecha Caducidad: " + nomina.Rows[n]["NOMINA_FCARD"].ToString());
                                return;
                            }
                        }
                    }
                    var resultsonlycontrol = from myRow in dtNominaOnyControl.AsEnumerable()
                                             where myRow.Field<string>("CEDULA") == cedulaconlaborador
                                             select new
                                             {
                                                 Nombres = myRow.Field<string>("NOMBRES"),
                                                 Apellidos = myRow.Field<string>("APELLIDOS"),
                                                 Cargo = myRow.Field<string>("CARGO")
                                             };
                    foreach (var item2 in resultsonlycontrol)
                    {
                        
                        dtColaboradores.Rows.Add(
                        txtci.Text,
                        item2.Nombres + " " + item2.Apellidos,    //resultsonlycontrol.AsDataView().Table.Rows[0]["NOMBRES"],
                        item2.Nombres,    //resultsonlycontrol.AsDataView().Table.Rows[0]["APELLIDOS"],
                        item2.Apellidos,
                        ddlActividadOnlyControl.SelectedItem.Text,
                        fecing.ToString("yyyy-MM-dd"),
                        facsal.ToString("yyyy-MM-dd"),
                        item2.Cargo,
                        txtNota.Text
                        );
                        this.gvColaboradores.DataSource = dtColaboradores;
                        this.gvColaboradores.DataBind();
                        //ddlNominaOnlyControl.SelectedValue = "0";
                        //dptipoevento.SelectedValue = "0";
                        txtci.Text = "";
                        txtNombres.Text = "";
                        txtApellidos.Text = "";
                        //ddlActividadOnlyControl.SelectedValue = "0";
                        txtci.BackColor = Color.Gray;
                        txtNombres.BackColor = Color.Gray;
                        txtApellidos.BackColor = Color.Gray;
                        txtcriterioconsulta.Focus();
                        //txtfecing.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        //txtfecsal.Text = DateTime.Now.AddDays(+1).ToString("dd/MM/yyyy");
                    }
                    hftablecol.Value = dtColaboradores.Rows.Count.ToString();
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta('imgagrega');", true);
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudpermisodeacceso.aspx.cs", "btnAgregar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable myDataTable = Session["dtSolicitudColaboradoressolicitudpermisodeacceso"] as DataTable;
                if (dtColaboradores.Rows.Count <= 0)
                {
                    this.Alerta("Agregue al menos un Colaborador.");
                    return;
                }
                String xmlColaboradoresPermisosDeAcceso = string.Empty;
                myDataTable.AcceptChanges();
                myDataTable.TableName = "PermisosDeAcceso";
                StringWriter sw = new StringWriter();
                myDataTable.WriteXml(sw);
                xmlColaboradoresPermisosDeAcceso = sw.ToString();
                string nombreempresa = CslHelper.getShiperName(rucempresa);
                string mensaje = string.Empty;

                New_ExportFileUpload();//23-11-2020
                //ExportFileUpload();
                if (!credenciales.AddSolicitudPermisosDeAcceso(
                   nombreempresa,
                   useremail,
                   rucempresa,
                   xmlColaboradoresPermisosDeAcceso,
                   xmlDocumentos,
                   Page.User.Identity.Name.ToUpper(),
                   out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    //this.Alerta("Solicitud registrada exitosamente, en unos minutos recibirá una notificación via mail.");
                    //Response.Write("<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente, en unos minutos recibirá una notificación via mail.');if(r==true){window.open='../credenciales/solicitud-colaborador')}else{window.open='../credenciales/solicitud-colaborador};</script>");
                    //CslHelper.JsonNewResponse(true, true, "window.location='../credenciales/solicitud-colaborador'", "");
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta('imgenvia');", true);
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>");
                    //var script = "<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>";
                    //scriptAlert(script);
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudpermisodeacceso.aspx.cs", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        protected void btnbuscardoc_Click(object sender, EventArgs e)
        {
            try
            {
                var tablix3 = credenciales.GetDocumentosOtros("0");
                tablePaginationDocumentos.DataSource = tablix3;
                tablePaginationDocumentos.DataBind();
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "mostrarsecexp();", true);
                ////btnbuscardoc.Focus();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudpermisodeacceso.aspx.cs", "btnbuscardoc_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }

        private void New_ExportFileUpload()
        {

            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("IdDocEmp");
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                TextBox txtidsolicitud = item.FindControl("txtidsolicitud") as TextBox;
                TextBox txtiddocemp = item.FindControl("txtiddocemp") as TextBox;
                if (fsupload.HasFile)
                {
                    string rutafile = Server.MapPath(fsupload.FileName);
                    string finalname;
                    var p = CSLSite.app_start.CredencialesHelper.UploadFile(Server.MapPath(fsupload.FileName), fsupload.PostedFile.InputStream, out finalname);
                    if (!p)
                    {
                        this.Alerta(finalname);
                        return;
                    }
                    dtDocumentos.Rows.Add(Convert.ToInt32(txtiddocemp.Text), 0, finalname);
                }
            }
            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            StringWriter sw = new StringWriter();
            dtDocumentos.WriteXml(sw);
            xmlDocumentos = sw.ToString();
        }


        private void ExportFileUpload()
        {
            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("IdDocEmp");
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                TextBox txtidsolicitud = item.FindControl("txtidsolicitud") as TextBox;
                TextBox txtiddocemp = item.FindControl("txtiddocemp") as TextBox;
                if (fsupload.HasFile)
                {
                    string rutafile = Server.MapPath(fsupload.FileName);
                    string extensionfile = Path.GetExtension(fsupload.PostedFile.FileName);
                    string directorio = Path.GetDirectoryName(rutafile);
                    string nombrearchivo = Path.GetFileNameWithoutExtension(fsupload.FileName);
                    if (File.Exists(rutafile))
                    {
                        File.Delete(rutafile);
                    }
                    fsupload.SaveAs(rutafile);
                    string[] files = Directory.GetFiles(directorio, "*" + nombrearchivo + extensionfile);
                    foreach (string s in files)
                    {
                        FileInfo fi = null;
                        try
                        {
                            fi = new FileInfo(s);
                            ExportFiles(fi.Directory.ToString(), fi.Name, txtidsolicitud.Text, txtiddocemp.Text, nombrearchivo, extensionfile);
                            if (File.Exists(rutafile))
                            {
                                File.Delete(rutafile);
                            }
                        }
                        catch (FileNotFoundException ex)
                        {
                            this.Alerta(ex.Message);
                            return;
                        }
                    }
                    if (File.Exists(rutafile))
                    {
                        File.Delete(rutafile);
                    }
                }
                else
                {
                }
            }
            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            StringWriter sw = new StringWriter();
            dtDocumentos.WriteXml(sw);
            xmlDocumentos = sw.ToString();
        }
        private void ExportFiles(string path, string filename, string sidsolicitud, string siddocemp, string nombrearchivo, string extension)
        {
            path = path + "\\";
            if (File.Exists(path + filename))
            {
                FileStream fileStream;
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                nombrearchivo = nombrearchivo + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssff") + extension;
                var llistaDoc = new List<listaDoc>();
                var ilistaDoc = new listaDoc(Convert.ToInt32(sidsolicitud), Convert.ToInt32(siddocemp), dateServer + nombrearchivo);
                llistaDoc.Add(ilistaDoc);
                foreach (var itemlistaDoc in llistaDoc)
                {
                    dtDocumentos.Rows.Add(itemlistaDoc.iddocemp, itemlistaDoc.idtipemp, itemlistaDoc.rutafile);
                }
                getFile.UploadFile(credenciales.ReadBinaryFile(path, filename, out fileStream), rutaServer, nombrearchivo);
                fileStream.Close();
            }
        }
        private class listaDoc
        {
            public int idtipemp { get; set; }
            public int iddocemp { get; set; }
            public string rutafile { get; set; }
            public listaDoc(int idtipemp, int iddocemp, string rutafile) { this.idtipemp = idtipemp; this.iddocemp = iddocemp; this.rutafile = rutafile.Trim(); }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string error = string.Empty;
                var t = dtNominaOnyControl;//credenciales.GetNominaOnlyControl(rucempresa, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    this.Alerta(error);
                    return;
                }
                string cedula = null;
                string nombres = null;
                string apellidos = null;
                if (rbcedula.Checked)
                {
                    if (t.Rows.Count > 0)
                    {
                        cedula = txtcriterioconsulta.Text;
                        var result = from rows in t.AsEnumerable()
                                     where rows.Field<string>("CEDULA").Contains(cedula)
                                     select rows;
                        t = result.AsDataView().ToTable();

                        /*
                        var consulta = "CEDULA Like '%" + cedula + "%'";
                        var resultado = t.Select(consulta);
                        t = resultado.AsEnumerable().CopyToDataTable();
                        */
                    }
                }
                if (rbnombres.Checked)
                {
                    nombres = txtcriterioconsulta.Text;
                    var result = from rows in t.AsEnumerable()
                                 where rows.Field<string>("NOMBRES").Contains(nombres)
                                 select rows;
                    t = result.AsDataView().ToTable();
                }
                if (rbapellidos.Checked)
                {
                    apellidos = txtcriterioconsulta.Text;
                    var result = from rows in t.AsEnumerable()
                                 where rows.Field<string>("APELLIDOS").Contains(apellidos)
                                 select rows;
                    t = result.AsDataView().ToTable();
                }
                if (t.Rows.Count == 0)
                {
                    txtci.Text = "";
                    txtNombres.Text = "";
                    txtApellidos.Text = "";
                    txtci.BackColor = System.Drawing.Color.Gray;
                    txtNombres.BackColor = System.Drawing.Color.Gray;
                    txtApellidos.BackColor = System.Drawing.Color.Gray;
                    txtcriterioconsulta.Focus();
                    this.Alerta("No se encontraron resultados, asegurese que ha escrito correctamente los criterios de consulta.");
                    return;
                }
                if (t.Rows.Count == 1)
                {
                    txtci.Text = t.Rows[0]["CEDULA"].ToString();
                    txtNombres.Text = t.Rows[0]["NOMBRES"].ToString();
                    txtApellidos.Text = t.Rows[0]["APELLIDOS"].ToString();
                    txtci.BackColor = System.Drawing.Color.White;
                    txtNombres.BackColor = System.Drawing.Color.White;
                    txtApellidos.BackColor = System.Drawing.Color.White;
                    txtcriterioconsulta.Text = "";
                    return;
                }
                if (rbcedula.Checked)
                {
                    var url = "window.open('" + "../catalogo/consultaColaboradorNominaOnlyControl.aspx?sidcedula=" + txtcriterioconsulta.Text + "','_blank', 'width=850,height=880')";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", url, true);
                    return;
                }
                if (rbnombres.Checked)
                {
                    var url = "window.open('" + "../catalogo/consultaColaboradorNominaOnlyControl.aspx?sidnombres=" + txtcriterioconsulta.Text + "','_blank', 'width=850,height=880')";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", url, true);
                    return;
                }
                if (rbapellidos.Checked)
                {
                    var url = "window.open('" + "../catalogo/consultaColaboradorNominaOnlyControl.aspx?sidapellidos=" + txtcriterioconsulta.Text + "','_blank', 'width=850,height=880')";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", url, true);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudpermisodeacceso.aspx.cs", "btnBuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        private void RegistraPermisoPermanenteOnlyControl(out bool validacolaboradores)
        {
            DataTable dtDocumentos = new DataTable();
            validacolaboradores = false;
            Int32 registros_actualizados_correcto = 0;
            Int32 registros_actualizados_incorrecto = 0;
            String error_consulta = string.Empty;
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime fecing;
            if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecing))
            {
                this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecing.Text));
                txtfecing.Focus();
                validacolaboradores = true;
                return;
            }
            DateTime facsal;
            if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out facsal))
            {
                this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecsal.Text));
                txtfecsal.Focus();
                validacolaboradores = true;
                return;
            }
            var empresa = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                validacolaboradores = true;
                return;
            }
            var dsPersonasOnlyControl = onlyControl.AC_C_PERSONA(empresa, string.Empty, 1, ref error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", error_consulta));
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                validacolaboradores = true;
                return;
            }
            //Label numsolicitud = new Label();
            var dtPermisosDeAcceso = credenciales.GetPermisoAccesoColaborador(txtci.Text, rucempresa);
            //if (dtPermisosDeAcceso.Rows.Count == )
            //{
            //}
            DataTable dtPermiso = new DataTable();
            dtPermiso.Columns.Add("ID");
            dtPermiso.Columns.Add("F_INGRESO");
            dtPermiso.Columns.Add("F_SALIDA");
            for (int i = 0; i < dtPermisosDeAcceso.Rows.Count; i++)
            {
                var resultPersonasOnlyControl = from myRow in dsPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0].AsEnumerable()
                                                where myRow.Field<string>("CEDULA") == dtPermisosDeAcceso.Rows[i]["CEDULA"].ToString()
                                                select myRow;
                DataTable dt = resultPersonasOnlyControl.AsDataView().ToTable();
                if (resultPersonasOnlyControl.AsDataView().ToTable().Rows.Count == 0)
                {
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", "No se encontro el colaborador: " + dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString()));
                    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    validacolaboradores = true;
                    return;
                }
                var id = resultPersonasOnlyControl.AsDataView().ToTable().Rows[i]["ID"];
                dtPermiso.Rows.Add(id, fecing.ToString("yyyy-MM-dd"), facsal.ToString("yyyy-MM-dd"));
            }
            DataSet dsPermiso = new DataSet();
            DataSet dsErrorAC_R_PERMISO = new DataSet();
            dsPermiso.Tables.Add(dtPermiso);
            dsErrorAC_R_PERMISO = onlyControl.AC_R_PERMISO(dsPermiso, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            if (dsErrorAC_R_PERMISO.Tables[0].Rows.Count != 0)
            {
                if (dsErrorAC_R_PERMISO.Tables[0].Rows[0]["ERROR"].ToString().Substring(0, 15) != "Error Duplicado")
                {
                    if (registros_actualizados_incorrecto > 0)
                    {
                        if (string.IsNullOrEmpty(error_consulta))
                        {
                            var dserror = dsErrorAC_R_PERMISO.DefaultViewManager.DataSet.Tables;
                            var dterror = dserror[0];
                            var error = string.Empty;
                            for (int i = 0; i < dterror.Rows.Count; i++)
                            {
                                error = error + " \\n *" +
                                       "Error: " + dterror.Rows[0]["ERROR"].ToString() + " \\n ";
                            }
                            var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERMISO():{0}", error));
                            var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                            this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                            validacolaboradores = true;
                            return;
                        }
                        var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERMISO():{0}", error_consulta));
                        var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                        validacolaboradores = true;
                        return;
                    }
                    else if (!string.IsNullOrEmpty(error_consulta))
                    {
                        var ex2 = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_R_PERMISO():{0}", error_consulta));
                        var number2 = log_csl.save_log<Exception>(ex2, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", registros_actualizados_correcto.ToString(), Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                        validacolaboradores = true;
                        return;
                    }
                }
            }
        }
        [WebMethod]
        public static string[] GetCustomers(string prefix)
        {
            var arrglo = prefix.Split(';');
            List<string> customers = new List<string>();
            string error = string.Empty;
            var nominaOnlyControl = credenciales.GetNominaOnlyControl(arrglo[1], out error);
            if (string.IsNullOrEmpty(error) || nominaOnlyControl.Rows.Count > 0)
            {
                var resultNominaOnlyControl = from myRow in nominaOnlyControl.AsEnumerable()
                                              where (myRow.Field<string>("NOMBRES") + myRow.Field<string>("APELLIDOS")) == arrglo[0]
                                              select myRow;

                for (int i = 0; i < resultNominaOnlyControl.AsDataView().Table.Rows.Count; i++)
                {
                    customers.Add(string.Format("{0}-{1}", 
                                 (resultNominaOnlyControl.AsDataView().Table.Rows[i]["NOMBRES"].ToString() + 
                                  resultNominaOnlyControl.AsDataView().Table.Rows[i]["APELLIDOS"].ToString()),
                                  resultNominaOnlyControl.AsDataView().Table.Rows[i]["CEDULA"]));
                }
            }
            /*
            using (SqlConnection conn = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["sca"].ConnectionString;
                    //cmd.CommandText = "select ContactName, CustomerId from Customers where ContactName like @SearchText + '%'";
                    cmd.CommandText = "SELECT descripcion, codigo FROM SCA_C_TIPO_SOLICITUD where descripcion like @SearchText + '%'";
                    cmd.Parameters.AddWithValue("@SearchText", prefix);
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            //customers.Add(string.Format("{0}-{1}", sdr["ContactName"], sdr["CustomerId"]));
                            customers.Add(string.Format("{0}-{1}", sdr["descripcion"], sdr["codigo"]));
                        }
                    }
                    conn.Close();
                }
            }*/
            return customers.ToArray();
        }
        protected void txtfecing_TextChanged(object sender, EventArgs e)
        {
            //CultureInfo enUS = new CultureInfo("en-US");
            //DateTime fecing;
            //if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecing))
            //{
            //    this.Alerta(string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", txtfecing.Text));
            //    txtfecing.Focus();
            //    return;
            //}
            //txtfecsal.Text = fecing.ToString("dd/MM/yyyy");
            //txtfecsal.Enabled = false;
            //txtfecsal.BackColor = System.Drawing.Color.White;
        }
        protected void ddlActividadOnlyControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlActividadOnlyControl.SelectedValue == "0")
            {
                var tablix = credenciales.GetDocumentosOtros("0");
                tablePaginationDocumentos.DataSource = tablix;
                tablePaginationDocumentos.DataBind();
            }
            else
            {
                var tablix = credenciales.GetDocumentosOtros(ddlActividadOnlyControl.SelectedValue);
                tablePaginationDocumentos.DataSource = tablix;
                tablePaginationDocumentos.DataBind();
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta('imagen');", true);
        }
        private void scriptAlert(string script)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
        }
    }
}