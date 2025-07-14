using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Xml.Linq;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
using System.Globalization;
using csl_log;

namespace CSLSite.cliente
{
    public partial class solicitudcolaboradorprovisional : System.Web.UI.Page
    {
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        private Repeater tablePaginationDocumentos = new Repeater();
        private GetFile.Service getFile = new GetFile.Service();
        public String stipoempresa
        {
            get { return (String)Session["stipoempresacoloboradorpro"]; }
            set { Session["stipoempresacoloboradorpro"] = value; }
        }
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosColaboradoresSolicitudPro"]; }
            set { Session["dtDocumentosColaboradoresSolicitudPro"] = value; }
        }
        private String xmlDocumentos;
        private String xmlColaboradores;
        public string rucempresa
        {
            get { return (string)Session["srucempresacolpro"]; }
            set { Session["srucempresacolpro"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailsolcolpro"]; }
            set { Session["useremailsolcolpro"] = value; }
        }
        public DataTable dtColaboradores
        {
            get { return (DataTable)Session["dtSolicitudColaboradoresPro"]; }
            set { Session["dtSolicitudColaboradoresPro"] = value; }
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
                rucempresa = user.ruc;
                useremail = user.email;
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
                LoadIni();
            }
        }
        private void LoadIni()
        {
            try
            {
                string error_consulta = string.Empty;
                //var areaOnlyControl = credenciales.GetAreaOnlyControl(rucempresa, out error_consulta);
                //if (!string.IsNullOrEmpty(error_consulta))
                //{
                //    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                //}

                
                var dtDatosRepLegal = credenciales.GetConsultaDatosRepresentanteLegal(rucempresa);
                if (dtDatosRepLegal.Rows.Count == 0)
                {
                    var script = "<script language='JavaScript'>alert('No se encontraron datos del Representante Legal, consulte con su Empresa.');</script>";
                    scriptAlert(script);
                    return;
                }
                txtusuariosolicita.Text = dtDatosRepLegal.Rows[0]["NOMREPLEGAL"].ToString();
                txtci.Text = dtDatosRepLegal.Rows[0]["CIREPLEGAL"].ToString();
                
                var areaOnlyControl = credenciales.GetConsultaArea();
                error_consulta = string.Empty;
                var cargoOnlyControl = credenciales.GetCargoOnlyControl(rucempresa, out error_consulta);
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    var script = "<script language='JavaScript'>alert('" + error_consulta + "');</script>";
                    scriptAlert(script);
                    return;
                }
                error_consulta = string.Empty;
                var actividadOnlyControl = credenciales.GetActividadPermitida();
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    var script = "<script language='JavaScript'>alert('" + error_consulta + "');</script>";
                    scriptAlert(script);
                    return;
                }
                populateDropDownList(ddlAreaOnlyControl, areaOnlyControl, "* Elija *", "AREA_ID", "AREA_NOM", false);
                populateDropDownList(ddlCargoOnlyControl, cargoOnlyControl, "* Elija *", "CALI_ID", "CALI_NOM", false);
                populateDropDownList(ddlActividadOnlyControl, actividadOnlyControl, "* Elija *", "TIPO", "DESCRIPCION", false);
                //ddlAreaOnlyControl.SelectedItem.Text = "* Elija *";
                //ddlCargoOnlyControl.SelectedItem.Text = "* Elija *";
                //ddlActividadOnlyControl.SelectedItem.Text = "* Elija *";
                ddlAreaOnlyControl.SelectedValue = "0";//SelectedIndex = areaOnlyControl.Rows.Count;
                ddlActividadOnlyControl.SelectedValue = "0";//SelectedIndex = 1;
                ddlCargoOnlyControl.SelectedValue = "0"; //SelectedIndex = 6;
                Session["dtDocumentosColaboradoresSolicitudPro"] = new DataTable();
                Session["dtSolicitudColaboradoresPro"] = new DataTable();
                dtColaboradores.Columns.Add("Nombres");
                dtColaboradores.Columns.Add("Apellidos");
                dtColaboradores.Columns.Add("CIPasaporte");
                dtColaboradores.Columns.Add("TipoSangre");
                dtColaboradores.Columns.Add("DireccionDomicilio");
                dtColaboradores.Columns.Add("TelefonoDomicilio");
                dtColaboradores.Columns.Add("Mail");
                dtColaboradores.Columns.Add("LugarNacimiento");
                dtColaboradores.Columns.Add("FechaNacimiento");
                //dtColaboradores.Columns.Add("Area");
                dtColaboradores.Columns.Add("Cargo");
                dtColaboradores.Columns.Add("Nota");
                gvColaboradores.DataSource = dtColaboradores;
                gvColaboradores.DataBind();
                txtnombres.Text = "";
                txtapellidos.Text = "";
                txtcipas.Text = "";
                txttiposangre.Text = "";
                txtdirdom.Text = "";
                txtteldom.Text = "";
                tmailinfocli.Value = "";
                txtlugarnacimiento.Text = "";
                txtfechanacimiento.Text = "";
                txtaredes.Text = "";
                txttiempoestadia.Text = "";
            }
            catch (Exception ex)
            {
                Session["dtSolicitudColaboradoresPro"] = new DataTable();
                dtColaboradores.Columns.Add("Nombres");
                dtColaboradores.Columns.Add("Apellidos");
                dtColaboradores.Columns.Add("CIPasaporte");
                dtColaboradores.Columns.Add("TipoSangre");
                dtColaboradores.Columns.Add("DireccionDomicilio");
                dtColaboradores.Columns.Add("TelefonoDomicilio");
                dtColaboradores.Columns.Add("Mail");
                dtColaboradores.Columns.Add("LugarNacimiento");
                dtColaboradores.Columns.Add("FechaNacimiento");
                //dtColaboradores.Columns.Add("Area");
                dtColaboradores.Columns.Add("Cargo");
                dtColaboradores.Columns.Add("Nota");
                gvColaboradores.DataSource = dtColaboradores;
                gvColaboradores.DataBind();
                txtnombres.Text = "";
                txtapellidos.Text = "";
                txtcipas.Text = "";
                txttiposangre.Text = "";
                txtdirdom.Text = "";
                txtteldom.Text = "";
                tmailinfocli.Value = "";
                txtlugarnacimiento.Text = "";
                txtfechanacimiento.Text = "";
                txtaredes.Text = "";
                txttiempoestadia.Text = "";
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaboradorprovisional.aspx.cs", "LoadIni()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
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
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                //guardar--->
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechaing;
                if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaing))
                {
                    this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", txtfecing.Text));
                    txtfecing.Focus();
                    return;
                }
                DateTime fechacad;
                if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacad))
                {
                    this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", txtfecsal.Text));
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
                Boolean banderadoc = false;
                String PlacasFaltantes = string.Empty;
                DataTable myDataTable = Session["dtDocumentosColaboradoresSolicitudPro"] as DataTable;
                if (dtColaboradores.Rows.Count <= 0)
                {
                    this.Alerta("Agregue al menos un Colaborador.");
                    return;
                }
                //if (cbltiposolicitud.SelectedValue == "%")
                //{
                //    this.Alerta("Seleccione un tipo de solicitud.");
                //}
                for (int i = 0; i <= dtColaboradores.Rows.Count - 1; i++)
                {
                    Label tcipas = (Label)gvColaboradores.Rows[i].FindControl("tcipas");
                    CheckBox chkEstadoDocumentos = (CheckBox)gvColaboradores.Rows[i].FindControl("chkEstadoDocumentos");
                    var result = from myRow in myDataTable.AsEnumerable()
                                 where myRow.Field<string>("CIPas") == tcipas.Text
                                 select myRow;
                    if (result.AsDataView().Table.Rows.Count <= 0)
                    {
                        PlacasFaltantes = PlacasFaltantes + " \\n *" + tcipas.Text;
                        gvColaboradores.Columns[10].Visible = true;
                        chkEstadoDocumentos.Checked = true;
                        banderadoc = true;
                    }
                    else
                    {
                        chkEstadoDocumentos.Checked = false;
                    }
                }
                if (banderadoc == true)
                {
                    this.Alerta("Los siguientes CI o Pasaporte, NO tienen documentos cargados:" + PlacasFaltantes);
                    return;
                } 
                ExportFileUpload();
                string mensaje = null;
                //CultureInfo enUS = new CultureInfo("en-US");
                //DateTime fechanac;
                //if (!DateTime.TryParseExact(txtfechanacimiento.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechanac))
                //{
                //    this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", txtfechanacimiento.Text));
                //}
                string nombreempresa = CslHelper.getShiperName(rucempresa);
                if (!credenciales.AddSolicitudColaboradorPaseProvisional(
                   nombreempresa,
                   txtci.Text,
                   txtusuariosolicita.Text,
                   fechaing.ToString("yyyy-MM-dd"),
                   fechacad.ToString("yyyy-MM-dd"),
                   useremail,
                   cbltiposolicitud.SelectedValue,
                   rucempresa,
                   ddlAreaOnlyControl.SelectedItem.Text,
                   ddlActividadOnlyControl.SelectedItem.Text,
                   xmlColaboradores,
                   xmlDocumentos,
                   Page.User.Identity.Name,
                   out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    LoadIni();
                    for (int i = 0; i <= myDataTable.Rows.Count - 1; i++)
                    {
                        var pathDoc = myDataTable.Rows[i][3].ToString() + myDataTable.Rows[i][4].ToString() + myDataTable.Rows[i][5].ToString();
                        if (File.Exists(pathDoc))
                        {
                            File.Delete(pathDoc);
                        }
                    }
                    //this.Alerta("Solicitud registrada exitosamente, en unos minutos recibirá una notificación via mail.");
                    //Response.Write("<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente, en unos minutos recibirá una notificación via mail.');if(r==true){window.open='../credenciales/solicitud-colaborador')}else{window.open='../credenciales/solicitud-colaborador};</script>");
                    //CslHelper.JsonNewResponse(true, true, "window.location='../credenciales/solicitud-colaborador'", "");
                    //Response.Write("<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='../csl/menu';}else{window.location='../csl/menu';}</script>");
                    var script = "<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='../csl/menu';}else{window.location='../csl/menu';}</script>";
                    scriptAlert(script);
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaboradorprovisional.aspx.cs", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        private void ExportFileUpload()
        {
            DataTable myDataTable = Session["dtDocumentosColaboradoresSolicitudPro"] as DataTable;
            myDataTable.Columns.Add("RutaOriginal");
            myDataTable.Columns.Add("NombreOriginal");
            string nomdoc = string.Empty;
            for (int i = 0; i <= myDataTable.Rows.Count - 1; i++)
            {
                var rutaoriginal = myDataTable.Rows[i][3];
                var nombreoriginal = myDataTable.Rows[i][4];
                ExportFiles(myDataTable.Rows[i][3].ToString(), myDataTable.Rows[i][4].ToString(), myDataTable.Rows[i][5].ToString(), out nomdoc);
                String dateServer = credenciales.GetDateServer();
                String rutaServer = dateServer;
                myDataTable.Rows[i][2] = cbltiposolicitud.SelectedValue;
                myDataTable.Rows[i][3] = rutaServer;
                myDataTable.Rows[i][4] = nomdoc;
                myDataTable.Rows[i][6] = rutaoriginal;
                myDataTable.Rows[i][7] = nombreoriginal;
            }
            for (int i = 0; i <= myDataTable.Rows.Count - 1; i++)
            {
                var pathDoc = myDataTable.Rows[i][6].ToString() + myDataTable.Rows[i][7].ToString() + myDataTable.Rows[i][5].ToString();
                if (File.Exists(pathDoc))
                {
                    File.Delete(pathDoc);
                }
            }
            //for (int i = 0; i <= dtColaboradores.Rows.Count - 1; i++)
            //{
            //    CultureInfo enUS = new CultureInfo("en-US");
            //    DateTime fechanac;
            //    if (!DateTime.TryParseExact(dtColaboradores.Rows[i][7].ToString(), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechanac))
            //    {
            //        this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", txtfechanacimiento.Text));
            //    }
            //    dtColaboradores.Rows[i][7] = fechanac.ToString("yyyy-MM-dd");
            //}
            dtColaboradores.AcceptChanges();
            dtColaboradores.TableName = "Colaboradores";
            StringWriter swVehiculos = new StringWriter();
            dtColaboradores.WriteXml(swVehiculos);
            xmlColaboradores = swVehiculos.ToString();
            myDataTable.AcceptChanges();
            myDataTable.TableName = "Documentos";
            StringWriter swDocumentos = new StringWriter();
            myDataTable.WriteXml(swDocumentos);
            xmlDocumentos = swDocumentos.ToString();
        }
        private void ExportFiles(string path, string filename, string extension, out string nomdoc)
        {
            //path = path + "\\";
            nomdoc = string.Empty;
            if (File.Exists(path + filename + extension))
            {
                FileStream fileStream;
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                /*var llistaDoc= new List<listaDoc>();
                var ilistaDoc = new listaDoc(Convert.ToInt32(sidsolicitud), Convert.ToInt32(siddocemp), rutaServer + filename);
                llistaDoc.Add(ilistaDoc);
                foreach (var itemlistaDoc in llistaDoc)
                {
                    dtDocumentos.Rows.Add(itemlistaDoc.iddocemp, itemlistaDoc.idtipemp, itemlistaDoc.rutafile+DateTime.Now.ToShortDateString());
                }*/
                var id = DateTime.Now.ToString("ddMMyyyyhhmmssff");
                var nombrearchivo = filename + "_" + id + extension;
                nomdoc = filename + "_" + id;
                getFile.UploadFile(credenciales.ReadBinaryFile(path, filename + extension, out fileStream), rutaServer, nombrearchivo);
                fileStream.Close();
            }
        }
        protected void btnbuscardoc_Click(object sender, EventArgs e)
        {
            try
            {
                //List<string> tipo = new List<string>();
                //for (int i = 0; i < cbltiposolicitud.Items.Count; i++)
                //{
                //    if (cbltiposolicitud.Items[i].Selected)
                //    {
                //        tipo.Add(cbltiposolicitud.Items[i].Value);
                //    }
                //}
                //if (tipo[0].ToString() == "0")
                //{
                //    tablePaginationDocumentos.Visible = false;
                //    this.Alerta("Seleccione al menos un Tipo de Solicitud.");
                //    return;
                //}
                tablePaginationDocumentos.Visible = true;
                //XElement xmlElements = new XElement("EMP", tipo.Select(i => new XElement("COD", i)));
                //var tablix2 = credenciales.GetDocumentosColaborador(rucempresa);
                //tablePaginationDocumentos.DataSource = tablix2;
                //tablePaginationDocumentos.DataBind();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaboradorprovisional.aspx.cs", "btnbuscardoc_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        protected void gvColaboradores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        protected void gvColaboradores_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                DataTable myDataTable = Session["dtSolicitudColaboradoresPro"] as DataTable;
                var result = from myRow in myDataTable.AsEnumerable()
                             where myRow.Field<string>("CIPasaporte") != dtColaboradores.Rows[e.RowIndex][4].ToString()
                             select myRow;
                if (result.AsDataView().Count > 0)
                {
                    Session["dtSolicitudColaboradoresPro"] = result.AsDataView().ToTable();
                }
                else if (result.AsDataView().Count == 0)
                {
                    Session["dtSolicitudColaboradoresPro"] = new DataTable();
                }
                DataTable dt = Session["dtSolicitudColaboradoresPro"] as DataTable;
                dtColaboradores.Rows.RemoveAt(e.RowIndex);
                gvColaboradores.DataSource = dtColaboradores;
                gvColaboradores.DataBind();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaboradorprovisional.aspx.cs", "gvColaboradores_RowDeleting()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Agregar();
                //if (nomina.Rows.Count > 0)
                //{
                //    for (int n = 0; n < nomina.Rows.Count; n++)
                //    {
                //        var nompuerta = credenciales.GetConsultaNom_Puerta(credenciales.GetConsultaScriptNom_Puerta(), nomina.Rows[n]["NOMINA_ID"].ToString());
                //        if (nompuerta.Rows.Count > 0)
                //        {
                //            for (int p = 0; p < nompuerta.Rows.Count; p++)
                //            {
                //                this.Alerta("El Colaborador: " + cedulaconlaborador.ToString() + " - " + txtnombres.Text + " " + txtapellidos.Text + ", tiene un permiso de acceso vigente:\\n" + "Fecha Inicio: " + nompuerta.Rows[p]["TURN_FECI"].ToString() + "\\nFecha Caducidad: " + nompuerta.Rows[p]["TURN_FECF"].ToString());
                //                return;
                //            }
                //        }
                //        var fechaing = Convert.ToDateTime(nomina.Rows[n]["NOMINA_FING"]);
                //        var fechacad = Convert.ToDateTime(nomina.Rows[n]["NOMINA_FCARD"]);
                //        int diferenciaEnAños = fechacad.Year - fechaing.Year;
                //        diferenciaEnAños = fechacad.Year - fechaing.Year;
                //        TimeSpan tsTiempo = fechacad - fechaing;
                //        int tiempoCaducidad = tsTiempo.Days;
                //        if (tiempoCaducidad > credenciales.GetTiempoCaducidadCredencialPermanente())
                //        {
                //            Agregar();
                //        }
                //        else
                //        {
                //            this.Alerta("Su credencial NO esta caducada, no puede realizar una solicitud de\\nEmisión/Renovación de credencial.\\n" + "Fecha Inicio: " + nomina.Rows[n]["NOMINA_FING"].ToString() + "\\nFecha Caducidad: " + nomina.Rows[n]["NOMINA_FCARD"].ToString());
                //            return;
                //        }
                //    }
                //}
                //else
                //{
                //    Agregar();
                //}
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaboradorprovisional.aspx.cs", "btnAgregar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
                //return;
            }
        }
        public void Agregar()
        {
            CultureInfo enUS = new CultureInfo("en-US");
            //DateTime fechaexplic = new DateTime();

            var results = from myRow in dtColaboradores.AsEnumerable()
                          where myRow.Field<string>("CIPasaporte") == txtcipas.Text
                          select new
                          {
                              Placa = myRow.Field<string>("CIPasaporte")
                          };
            foreach (var item in results)
            {
                if (item.Placa == txtcipas.Text)
                {
                    this.Alerta("Ya se registro la C.I. o Pasaporte: " + txtcipas.Text + ", revise por favor.");
                    return;
                }
            }
            DateTime fechanac = new DateTime();
            if (!DateTime.TryParseExact(txtfechanacimiento.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechanac))
            {
                this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", txtfechanacimiento.Text));
                txtfechanacimiento.Focus();
                return;
            }
            //if (credenciales.GetTipoSolicitudXRUC(rucempresa, Session["stipoempresacoloborador"].ToString()))
            //{
                //if (string.IsNullOrEmpty(txttiplic.Text))
                //{
                //    this.Alerta("Escriba el Tipo de Licencia.");
                //    txtaredes.Focus();
                //    return;
                //}
                //if (string.IsNullOrEmpty(txtfecexplic.Text))
                //{
                //    this.Alerta("Escriba la Fecha de Expiración de la Licencia.");
                //    txttiempoestadia.Focus();
                //    return;
                //}
                //else
                //{
                //    if (!DateTime.TryParseExact(txtfecexplic.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaexplic))
                //    {
                //        this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", txtfecexplic.Text));
                //        txtfecexplic.Focus();
                //        return;
                //    }
                //}
                //dtColaboradores.Columns.Add("Nombres");
                //dtColaboradores.Columns.Add("Apellidos");
                //dtColaboradores.Columns.Add("CIPasaporte");
                //dtColaboradores.Columns.Add("TipoSangre");
                //dtColaboradores.Columns.Add("DireccionDomicilio");
                //dtColaboradores.Columns.Add("TelefonoDomicilio");
                //dtColaboradores.Columns.Add("Mail");
                //dtColaboradores.Columns.Add("LugarNacimiento");
                //dtColaboradores.Columns.Add("FechaNacimiento");
                //dtColaboradores.Columns.Add("Area");
                //dtColaboradores.Columns.Add("Cargo");
                dtColaboradores.Rows.Add(
                txtnombres.Text,
                txtapellidos.Text,
                txtcipas.Text,
                txttiposangre.Text,
                txtdirdom.Text,
                txtteldom.Text,
                tmailinfocli.Value,
                txtlugarnacimiento.Text,
                fechanac.ToString("yyyy-MM-dd"),
                //ddlAreaOnlyControl.SelectedItem.Text,
                ddlCargoOnlyControl.SelectedItem.Text,
                txtNota.Text
                );
            //}
            //else
            //{
            //dtColaboradores.Rows.Add(
            //    txtnombres.Text,
            //    txtapellidos.Text,
            //    txtcipas.Text,
            //    txttiposangre.Text,
            //    txtdirdom.Text,
            //    txtteldom.Text,
            //    tmailinfocli.Value,
            //    txtlugarnacimiento.Text,
            //    fechanac.ToString("yyyy-MM-dd"),
            //    ddlAreaOnlyControl.SelectedItem.Text,
            //    ddlCargoOnlyControl.SelectedItem.Text
            //    );
            //}
            //var dsPersonasOnlyControl = onlyControl.AC_C_PERSONA(empresa, string.Empty, 1, ref error_consulta);
            //if (!string.IsNullOrEmpty(error_consulta))
            //{
            //    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", error_consulta));
            //    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
            //    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            //    validacolaboradores = true;
            //    return;
            this.gvColaboradores.DataSource = dtColaboradores;
            this.gvColaboradores.DataBind();
            txtnombres.Text = "";
            txtapellidos.Text = "";
            txtcipas.Text = "";
            txttiposangre.Text = "";
            txtdirdom.Text = "";
            txtteldom.Text = "";
            tmailinfocli.Value = "";
            txtlugarnacimiento.Text = "";
            txtfechanacimiento.Text = "";
            txttiplic.Text = "";
            txtfecexplic.Text = "";
            //ddlAreaOnlyControl.SelectedItem.Text = "* Elija *";
            //ddlCargoOnlyControl.SelectedItem.Text = "* Elija *";
        }
        private void RegistraPermisoProvisionalOnlyControl(string tipocredencial, out bool validacolaboradores)
        {
            validacolaboradores = false;
            String errorvehiculo = string.Empty;
            DataTable dtAprobados = new DataTable();
            List<string> listCedulas = new List<string>();
            Int32 registros_actualizados_correcto = 0;
            Int32 registros_actualizados_incorrecto = 0;
            String error_consulta = string.Empty;
            String error = string.Empty;
            DataSet dsColaboradores = new DataSet();
            DataSet dsErrorAC_R_PERSONA_PEATON = new DataSet();

            var empresa = credenciales.GetDescripcionEmpresaOnlyControl(rucempresa, out error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraColaboradoresOnlyControl", "1", Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                validacolaboradores = true;
                return;
            }
            DataTable dtAC_R_PERSONA_PEATON = new DataTable();
            DataSet dsAC_R_PERSONA_PEATON = new DataSet();
            DataSet dsErrorAC_R_PERSONA_PEATONN = new DataSet();
            dtAC_R_PERSONA_PEATON.Columns.Add("CEDULA");
            dtAC_R_PERSONA_PEATON.Columns.Add("APELLIDOS");
            dtAC_R_PERSONA_PEATON.Columns.Add("NOMBRES");
            dtAC_R_PERSONA_PEATON.Columns.Add("EMPRESA");
            dtAC_R_PERSONA_PEATON.Columns.Add("CARGO");
            dtAC_R_PERSONA_PEATON.Rows.Add("0926286121", "REYES MOGROVEJO", "ROGER ROBERTO", empresa, "SUPERVISOR");
            dsAC_R_PERSONA_PEATON.Tables.Add(dtAC_R_PERSONA_PEATON);
            dsErrorAC_R_PERSONA_PEATONN = onlyControl.AC_R_PERSONA_PEATON(dsAC_R_PERSONA_PEATON, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;

            DataTable dtAC_R_PERMISO_TEMPORAL = new DataTable();
            DataSet dsAC_R_PERMISO_TEMPORAL = new DataSet();
            DataSet dsErrorAC_R_PERMISO_TEMPORAL = new DataSet();
            dtAC_R_PERMISO_TEMPORAL.Columns.Add("ID_PERMISO");
            dtAC_R_PERMISO_TEMPORAL.Columns.Add("EMPRESA");
            dtAC_R_PERMISO_TEMPORAL.Columns.Add("AREA");
            dtAC_R_PERMISO_TEMPORAL.Columns.Add("ID_SOLICITANTE");
            dtAC_R_PERMISO_TEMPORAL.Columns.Add("CEDULA_SOLICITANTE");
            dtAC_R_PERMISO_TEMPORAL.Columns.Add("SOLICITANTE");
            dtAC_R_PERMISO_TEMPORAL.Columns.Add("ACTIVIDAD");
            dtAC_R_PERMISO_TEMPORAL.Columns.Add("FECHA_INICIO");
            dtAC_R_PERMISO_TEMPORAL.Columns.Add("FECHA_FIN");
            dtAC_R_PERMISO_TEMPORAL.Rows.Add("", empresa, "OFICINA DE NETTEL", "", "0916262538", "ENRIQUE DANIEL ARCE ESPAÑA", "AUDITORIA", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));
            dsAC_R_PERMISO_TEMPORAL.Tables.Add(dtAC_R_PERMISO_TEMPORAL);
            dsErrorAC_R_PERMISO_TEMPORAL = onlyControl.AC_R_PERMISO_TEMPORAL(dsAC_R_PERMISO_TEMPORAL, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;

            DataTable dtAC_R_ASIGNA_PERSONA = new DataTable();
            DataSet dsAC_R_ASIGNA_PERSONA = new DataSet();
            DataSet dsErrorAC_R_ASIGNA_PERSONA = new DataSet();
            dtAC_R_ASIGNA_PERSONA.Columns.Add("ID_PERMISO");
            dtAC_R_ASIGNA_PERSONA.Columns.Add("CEDULA");
            dtAC_R_ASIGNA_PERSONA.Columns.Add("ID_PERSONA");
            dtAC_R_ASIGNA_PERSONA.Columns.Add("PERSONA");
            dtAC_R_ASIGNA_PERSONA.Columns.Add("USUARIO");
            dtAC_R_ASIGNA_PERSONA.Rows.Add(dsErrorAC_R_PERMISO_TEMPORAL.Tables[0].Rows[0]["ID_PERMISO"], "0926286121", "036047", "ROGER ROBERTO REYES MOGROVEJO", "000316");
            dsAC_R_ASIGNA_PERSONA.Tables.Add(dtAC_R_ASIGNA_PERSONA);
            dsErrorAC_R_ASIGNA_PERSONA = onlyControl.AC_R_ASIGNA_PERSONA(dsAC_R_ASIGNA_PERSONA, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string cedula = null;
                string nombres = null;
                string apellidos = null;
                if (rbcedula.Checked)
                {
                    cedula = txtcriterioconsulta.Text;
                }
                if (rbnombres.Checked)
                {
                    nombres = txtcriterioconsulta.Text;
                }
                if (rbapellidos.Checked)
                {
                    apellidos = txtcriterioconsulta.Text;
                }
                var t = credenciales.GetConsultaNominaPeaton(rucempresa, cedula, nombres, apellidos);
                if (t.Rows.Count == 0)
                {
                    txtcipas.Text = "";
                    txtnombres.Text = "";
                    txtapellidos.Text = "";
                    txttiposangre.Text = "";
                    txtdirdom.Text = "";
                    txtteldom.Text = "";
                    tmailinfocli.Value = "";
                    txtlugarnacimiento.Text = "";
                    txtfechanacimiento.Text = "";
                    txttiplic.Text = "";
                    txtfecexplic.Text = "";
                    ddlCargoOnlyControl.SelectedValue = "0";
                    /*
                    txtcipas.BackColor = System.Drawing.Color.Gray;
                    txtnombres.BackColor = System.Drawing.Color.Gray;
                    txtapellidos.BackColor = System.Drawing.Color.Gray;
                    txttiposangre.BackColor = System.Drawing.Color.Gray;
                    txtdirdom.BackColor = System.Drawing.Color.Gray;
                    txtteldom.BackColor = System.Drawing.Color.Gray;
                    tmailinfocli.Attributes["style"] = "width:400px; background-color:Gray";
                    txtlugarnacimiento.BackColor = System.Drawing.Color.Gray;
                    txtfechanacimiento.BackColor = System.Drawing.Color.Gray;
                    txttiplic.BackColor = System.Drawing.Color.Gray;
                    txtfecexplic.BackColor = System.Drawing.Color.Gray;*/
                    txtcriterioconsulta.Focus();
                    this.Alerta("No se encontraron resultados, asegurese que ha escrito correctamente los criterios de consulta.");
                    return;
                }
                if (t.Rows.Count == 1)
                {
                    txtcipas.Text = t.Rows[0]["CEDULA"].ToString();
                    txtnombres.Text = t.Rows[0]["NOMBRES"].ToString();
                    txtapellidos.Text = t.Rows[0]["APELLIDOS"].ToString();
                    var detalle = credenciales.GetConsultaDatosColaboradorSCA(rucempresa, t.Rows[0]["CEDULA"].ToString());
                    if (detalle.Rows.Count > 0)
                    {
                        txttiposangre.Text = detalle.Rows[0]["TIPOSANGRE"].ToString();
                        txtdirdom.Text = detalle.Rows[0]["DIRECCIONDOM"].ToString();
                        txtteldom.Text = detalle.Rows[0]["TELFDOM"].ToString();
                        tmailinfocli.Value = detalle.Rows[0]["EMAIL"].ToString();
                        txtlugarnacimiento.Text = detalle.Rows[0]["LUGARNAC"].ToString();
                        txtfechanacimiento.Text = Convert.ToDateTime(detalle.Rows[0]["FECHANAC"]).ToString("dd/MM/yyyy");
                        txttiplic.Text = detalle.Rows[0]["TIPOLICENCIA"].ToString();
                        txtfecexplic.Text = Convert.ToDateTime(detalle.Rows[0]["FECHAEXPLICENCIA"]).ToString("dd/MM/yyyy");
                        ddlCargoOnlyControl.SelectedValue = t.Rows[0]["NOMINA_CARGO"].ToString();
                    }
                    /*txtcipas.BackColor = System.Drawing.Color.White;
                    txtnombres.BackColor = System.Drawing.Color.White;
                    txtapellidos.BackColor = System.Drawing.Color.White;
                    txttiposangre.BackColor = System.Drawing.Color.White;
                    txtdirdom.BackColor = System.Drawing.Color.White;
                    txtteldom.BackColor = System.Drawing.Color.White;
                    tmailinfocli.Attributes["style"] = "width:400px; background-color:White";
                    txtlugarnacimiento.BackColor = System.Drawing.Color.White;
                    txtfechanacimiento.BackColor = System.Drawing.Color.White;
                    txttiplic.BackColor = System.Drawing.Color.White;
                    txtfecexplic.BackColor = System.Drawing.Color.White;
                    txtcriterioconsulta.Focus();
                    txtcipas.Enabled = false;
                    txtnombres.Enabled = false;
                    txtapellidos.Enabled = false;
                    txttiposangre.Enabled = false;
                    txtdirdom.Enabled = false;
                    txtteldom.Enabled = false;
                    tmailinfocli.Disabled = true;
                    txtlugarnacimiento.Enabled = false;
                    txtfechanacimiento.Enabled = false;
                    txttiplic.Enabled = false;
                    txtfecexplic.Enabled = false;*/
                    txtcriterioconsulta.Text = "";
                    return;
                }
                if (rbcedula.Checked)
                {
                    var url = "window.open('" + "../catalogo/peaton?sidcedula=" + txtcriterioconsulta.Text + "','_blank', 'width=850,height=480')";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", url, true);
                    return;
                }
                if (rbnombres.Checked)
                {
                    var url = "window.open('" + "../catalogo/peaton?sidnombres=" + txtcriterioconsulta.Text + "','_blank', 'width=850,height=480')";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", url, true);
                    return;
                }
                if (rbapellidos.Checked)
                {
                    var url = "window.open('" + "../catalogo/peaton?sidapellidos=" + txtcriterioconsulta.Text + "','_blank', 'width=850,height=480')";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", url, true);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaboradorprovisional.aspx.cs", "btnBuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        private void scriptAlert(string script)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
        }
    }
}