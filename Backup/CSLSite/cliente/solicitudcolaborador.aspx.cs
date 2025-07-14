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
    public partial class solicitudcolaborador : System.Web.UI.Page
    {
        public string porcentaje
        {
            get { return (string)Session["porcentajecol"]; }
            set { Session["porcentajecol"] = value; }
        }
        public bool bandera
        {
            get { return (bool)Session["banberacol"]; }
            set { Session["banberacol"] = value; }
        }
        public int contador
        {
            get { return (int)Session["contadorcol"]; }
            set { Session["contadorcol"] = value; }
        }
        private Repeater tablePaginationDocumentos = new Repeater();
        private GetFile.Service getFile = new GetFile.Service();
        public String stipoempresa
        {
            get { return (String)Session["stipoempresacoloborador"]; }
            set { Session["stipoempresacoloborador"] = value; }
        }
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosColaboradoresSolicitud"]; }
            set { Session["dtDocumentosColaboradoresSolicitud"] = value; }
        }
        private String xmlDocumentos;
        private String xmlColaboradores;
        public String ErrorColoboradores
        {
            get { return (String)Session["ErrorCol"]; }
            set { Session["ErrorCol"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["srucempresacol"]; }
            set { Session["srucempresacol"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailsolcol"]; }
            set { Session["useremailsolcol"] = value; }
        }
        public DataTable dtColaboradores
        {
            get { return (DataTable)Session["dtSolicitudColaboradores"]; }
            set { Session["dtSolicitudColaboradores"] = value; }
        }
        public DataTable myDataTable_
        {
            get { return (DataTable)Session["myDataTable_Colaboradores"]; }
            set { Session["myDataTable_Colaboradores"] = value; }
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
                populateDrop(ddlTipoLicencia, credenciales.getTipoLicencia(ddlTipoEmpresa.SelectedItem.Value));
                if (ddlTipoLicencia.Items.Count > 0)
                {
                    if (ddlTipoLicencia.Items.FindByValue("0") != null)
                    {
                        ddlTipoLicencia.Items.FindByValue("0").Selected = true;
                    }
                    ddlTipoLicencia.SelectedValue = "0";
                }
                populateDrop(cbltiposolicitud, credenciales.getTipoSolicitud(3));
                if (cbltiposolicitud.Items.Count > 0)
                {
                    if (cbltiposolicitud.Items.FindByValue("0") != null)
                    {
                        cbltiposolicitud.Items.FindByValue("0").Selected = true;
                    }
                    cbltiposolicitud.SelectedValue = "0";
                }
                populateDrop(ddlTipoEmpresa, credenciales.getTipoEmpresa(rucempresa));
                if (ddlTipoEmpresa.Items.Count > 1)
                {
                    if (ddlTipoEmpresa.Items.FindByValue("0") != null)
                    {
                        ddlTipoEmpresa.Items.FindByValue("0").Selected = true;
                    }
                    ddlTipoEmpresa.SelectedValue = "0";
                    ddlTipoEmpresa.Enabled = true;
                }
                else
                {
                    for (int i = 0; i < ddlTipoEmpresa.Items.Count; i++)
                    {
                        if (ddlTipoEmpresa.Items[i].Value != "0")
                        {
                            ddlTipoEmpresa.SelectedValue = ddlTipoEmpresa.Items[i].Value;
                            ddlTipoEmpresa.Enabled = false;
                        }
                    }
                }
                infotipcre.InnerHtml = credenciales.GetInfoTipoCredencial();
                Session["dtDocumentosColaboradoresSolicitud"] = new DataTable();
                Session["dtSolicitudColaboradores"] = new DataTable();
                dtColaboradores.Columns.Add("Tipo");
                dtColaboradores.Columns.Add("Nombres");
                dtColaboradores.Columns.Add("Apellidos");
                dtColaboradores.Columns.Add("CIPasaporte");
                dtColaboradores.Columns.Add("TipoSangre");
                dtColaboradores.Columns.Add("DireccionDomicilio");
                dtColaboradores.Columns.Add("TelefonoDomicilio");
                dtColaboradores.Columns.Add("Mail");
                dtColaboradores.Columns.Add("LugarNacimiento");
                dtColaboradores.Columns.Add("FechaNacimiento");
                dtColaboradores.Columns.Add("TipoLicencia");
                dtColaboradores.Columns.Add("Cargo");
                dtColaboradores.Columns.Add("FecExpLicencia");
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
                txtcargo.Text = "";
                txttiempoestadia.Text = "";
                //txtnombres.Focus();
                gvColaboradores.Columns[13].Visible = false;
                contador = 0;
                porcentaje = "1";
                TimerPb.Enabled = false;
                //if (credenciales.GetTipoSolicitudXRUC(rucempresa, Session["stipoempresacoloborador"].ToString()))
                //{
                //    txttiposangre.Enabled = true;
                //    txtfecexplic.Enabled = true;
                //}
                //else
                //{
                //    txttiposangre.Enabled = false;
                //    txtfecexplic.Enabled = false;
                //}
            }
            catch (Exception ex)
            {
                Session["dtSolicitudColaboradores"] = new DataTable();
                dtColaboradores.Columns.Add("Tipo");
                dtColaboradores.Columns.Add("Nombres");
                dtColaboradores.Columns.Add("Apellidos");
                dtColaboradores.Columns.Add("CIPasaporte");
                dtColaboradores.Columns.Add("TipoSangre");
                dtColaboradores.Columns.Add("DireccionDomicilio");
                dtColaboradores.Columns.Add("TelefonoDomicilio");
                dtColaboradores.Columns.Add("Mail");
                dtColaboradores.Columns.Add("LugarNacimiento");
                dtColaboradores.Columns.Add("FechaNacimiento");
                dtColaboradores.Columns.Add("TipoLicencia");
                dtColaboradores.Columns.Add("Cargo");
                dtColaboradores.Columns.Add("FecExpLicencia");
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
                txtcargo.Text = "";
                txttiempoestadia.Text = "";
                gvColaboradores.Columns[13].Visible = false;
                contador = 0;
                porcentaje = "1";
                TimerPb.Enabled = false;
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaborador.aspx.cs", "LoadIni()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                //guardar--->
                Boolean banderadoc = false;
                String PlacasFaltantes = string.Empty;
                DataTable myDataTable = Session["dtDocumentosColaboradoresSolicitud"] as DataTable;
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
                    if (result.AsDataView().Count <= 0)
                    {
                        PlacasFaltantes = PlacasFaltantes + " \\n *" + tcipas.Text;
                        gvColaboradores.Columns[13].Visible = true;
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
                var vscript = "<script language='JavaScript'>Panel();</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "script", vscript, false);
                string mensaje = null;
                Int32 identity = 0;
                if (!credenciales.AddSolicitudColaboradorCab(
                  cbltiposolicitud.SelectedValue,
                  Page.User.Identity.Name.ToUpper().ToUpper(),
                  out mensaje,
                  out identity))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    Session["identity"] = identity;
                    Session["valor"] = 0;
                    TimerPb.Enabled = true;
                    bandera = true;
                    ErrorColoboradores = string.Empty;
                    myDataTable_ = Session["dtDocumentosColaboradoresSolicitud"] as DataTable;
                    myDataTable_.Columns.Add("RutaOriginal");
                    myDataTable_.Columns.Add("NombreOriginal");
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaborador.aspx.cs", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        public void setVariables()
        {
            txtcipas.BackColor = System.Drawing.Color.White;
            txtnombres.BackColor = System.Drawing.Color.White;
            txtapellidos.BackColor = System.Drawing.Color.White;
            txttiposangre.BackColor = System.Drawing.Color.White;
            txtdirdom.BackColor = System.Drawing.Color.White;
            txtteldom.BackColor = System.Drawing.Color.White;
            tmailinfocli.Attributes["style"] = "width:400px; background-color:White";
            txtlugarnacimiento.BackColor = System.Drawing.Color.White;
            txtfechanacimiento.BackColor = System.Drawing.Color.White;
            txtcargo.BackColor = System.Drawing.Color.White;
            txtfecexplic.BackColor = System.Drawing.Color.White;
            ddlTipoLicencia.BackColor = System.Drawing.Color.White;
        }
        private void ExportFileUpload(Int32 identity)
        {
            bool enviaemail = false;
            DataTable myDataTable = Session["myDataTable_Colaboradores"] as DataTable;
            //myDataTable.Columns.Add("RutaOriginal");
            //myDataTable.Columns.Add("NombreOriginal");
            string nomdoc = string.Empty;
            for (int a = contador; a < dtColaboradores.Rows.Count; a++)
            {
                var resultdoc = from myRow in myDataTable.AsEnumerable()
                             where myRow.Field<string>("CIPas") == dtColaboradores.Rows[a]["CIPasaporte"].ToString()
                             select myRow;
                DataTable dtDocumentos = resultdoc.AsDataView().ToTable();
                for (int i = 0; i < dtDocumentos.Rows.Count; i++)
                {
                    var rutaoriginal = dtDocumentos.Rows[i][3];
                    var nombreoriginal = dtDocumentos.Rows[i][4];
                    ExportFiles(dtDocumentos.Rows[i][3].ToString(), dtDocumentos.Rows[i][4].ToString(), dtDocumentos.Rows[i][5].ToString(), out nomdoc);
                    String dateServer = credenciales.GetDateServer();
                    String rutaServer = dateServer;
                    dtDocumentos.Rows[i][2] = cbltiposolicitud.SelectedValue;
                    dtDocumentos.Rows[i][3] = rutaServer;
                    dtDocumentos.Rows[i][4] = nomdoc;
                    dtDocumentos.Rows[i][6] = rutaoriginal;
                    dtDocumentos.Rows[i][7] = nombreoriginal;
                }
                DataTable dtCol = dtColaboradores;
                var resultcol = from myRow in dtCol.AsEnumerable()
                                where myRow.Field<string>("CIPasaporte") == dtColaboradores.Rows[a]["CIPasaporte"].ToString()
                                select myRow;
                dtCol = resultcol.AsDataView().ToTable();
                dtCol.TableName = "Colaboradores";
                StringWriter swCol= new StringWriter();
                dtCol.WriteXml(swCol);
                xmlColaboradores = swCol.ToString();

                DataTable dtDoc = dtDocumentos;
                dtDoc.AcceptChanges();
                dtDoc.TableName = "Documentos";
                StringWriter swDocumentos = new StringWriter();
                dtDoc.WriteXml(swDocumentos);
                xmlDocumentos = swDocumentos.ToString();

                var valida = dtColaboradores.Rows.Count - contador;
                if (valida == 1)
                {
                    enviaemail = true;
                }

                string nombreempresa = CslHelper.getShiperName(rucempresa);
                string mensaje = null;
                if (!credenciales.AddSolicitudColaboradorDet(
                   nombreempresa,
                   identity,
                   useremail,
                   cbltiposolicitud.SelectedValue,
                   rucempresa,
                   xmlColaboradores,
                   xmlDocumentos,
                   Page.User.Identity.Name.ToUpper().ToUpper(),
                   enviaemail,
                   out mensaje))
                {
                    //this.Alerta(mensaje);
                    ErrorColoboradores = ErrorColoboradores + " \\n *" + dtColaboradores.Rows[a]["CIPasaporte"].ToString() + " - " + dtColaboradores.Rows[a]["Nombres"].ToString() + " " + dtColaboradores.Rows[a]["Apellidos"].ToString();
                }
                else
                {
                    //LoadIni();
                    //for (int i = 0; i <= myDataTable.Rows.Count - 1; i++)
                    //{
                    //    var pathDoc = myDataTable.Rows[i][3].ToString() + myDataTable.Rows[i][4].ToString() + myDataTable.Rows[i][5].ToString();
                    //    if (File.Exists(pathDoc))
                    //    {
                    //        File.Delete(pathDoc);
                    //    }
                    //}
                    //Response.Write("<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='../csl/menu';}else{window.location='../csl/menu';}</script>");
                    if (enviaemail)
                    {
                        for (int d = 0; d < myDataTable.Rows.Count; d++)
                        {
                            var pathDoc = myDataTable.Rows[d]["Ruta"].ToString() + myDataTable.Rows[d]["NombreArchivo"].ToString() + myDataTable.Rows[d]["Extension"].ToString();
                            if (File.Exists(pathDoc))
                            {
                                File.Delete(pathDoc);
                            }
                        }
                        //if (!string.IsNullOrEmpty(ErrorColoboradores))
                        //{
                        //    var msgerror = "Solicitud registrada exitosamente.\\n\\n*Colaborador(es) no registrados por favor realice un nueva solicitud para los mismos:" + ErrorColoboradores;
                        //    var script = "<script language='JavaScript'>var r=alert('" + msgerror + "');if(r==true){window.location='../csl/menu';}else{window.location='../csl/menu';}</script>";
                        //    scriptAlert(script);
                        //    //var script2 = "<script language='JavaScript'>modalPanel('" + msgerror + "');</script>";
                        //    //ScriptManager.RegisterStartupScript(this, typeof(Page), "script", script2, false);
                        //    TimerPb.Enabled = false;
                        //}
                        //else
                        //{
                        //    var script = "<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='../csl/menu';}else{window.location='../csl/menu';}</script>";
                        //    scriptAlert(script);
                        //    //var script2 = "<script language='JavaScript'>modalPanel('" + "" + "');</script>";
                        //    //ScriptManager.RegisterStartupScript(this, typeof(Page), "script", script2, false);
                        //    TimerPb.Enabled = false;
                        //}
                    }
                }
                a = dtColaboradores.Rows.Count;
                contador = contador + 1;
            }
            string inicio = string.Empty;
            if (contador == 1)
            {
                inicio = contador.ToString();
            }
            else
            {
                inicio = porcentaje;
            }
            porcentaje = Math.Truncate(((Convert.ToDecimal(contador) / Convert.ToDecimal(dtColaboradores.Rows.Count)) * 100)).ToString();
            var vscript = "<script language='JavaScript'>move(" + inicio.ToString() + "," + porcentaje.ToString() + ");</script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "script", vscript, false);
            
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
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaborador.aspx.cs", "btnbuscardoc_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
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
                DataTable myDataTable = Session["dtSolicitudColaboradores"] as DataTable;
                var result = from myRow in myDataTable.AsEnumerable()
                             where myRow.Field<string>("CIPasaporte") != dtColaboradores.Rows[e.RowIndex][4].ToString()
                             select myRow;
                if (result.AsDataView().Count > 0)
                {
                    Session["dtSolicitudColaboradores"] = result.AsDataView().ToTable();
                }
                else if (result.AsDataView().Count == 0)
                {
                    Session["dtSolicitudColaboradores"] = new DataTable();
                }
                DataTable dt = Session["dtSolicitudColaboradores"] as DataTable;
                dtColaboradores.Rows.RemoveAt(e.RowIndex);
                gvColaboradores.DataSource = dtColaboradores;
                gvColaboradores.DataBind();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaborador.aspx.cs", "gvColaboradores_RowDeleting()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvColaboradores.Rows.Count == 0)
                {
                    Session["stipoempresacoloborador"] = ddlTipoEmpresa.SelectedValue;
                }
                if (gvColaboradores.Rows.Count >= 1)
                {
                    if (Session["stipoempresacoloborador"].ToString() != ddlTipoEmpresa.SelectedValue.ToString())
                    {
                        ddlTipoEmpresa.SelectedValue = Session["stipoempresacoloborador"].ToString();
                    }
                }
                var cedulaconlaborador = txtcipas.Text.Trim();
                var nomina = credenciales.GetConsultaNomina(rucempresa, cedulaconlaborador);
                if (nomina.Rows[0]["MENSAJE"].ToString() == "NUEVO" || nomina.Rows[0]["MENSAJE"].ToString() == "SEMPRESA" || nomina.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                {
                    string tipo = "";
                    if (nomina.Rows[0]["MENSAJE"].ToString() == "SEMPRESA")
                    {
                        tipo = "2"; //Renovación
                    }
                    else if (nomina.Rows[0]["MENSAJE"].ToString() == "NUEVO" || nomina.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                    {
                        tipo = "1"; //Emisión
                    }
                    //var colaborador = credenciales.GetConsultaColaboradorXCedula(cedulaconlaborador);
                    //if (colaborador.Rows[0]["SOLICITUD"].ToString() == "0")
                    //{
                    //    this.Alerta("El Colaborador(a):\\n*" + cedulaconlaborador.ToString() + " - " + txtnombres.Text + " " + txtapellidos.Text + "\\nYa se encuentra registrado en otra Solictud.\\n *" + "Nº de Solicitud - " + colaborador.Rows[0]["SOLICITUD"].ToString());
                    //    return;
                    //}
                    Agregar(tipo);
                    return;
                }
                else
                {
                    if (cbltiposolicitud.SelectedItem.Value == "2")
                    {
                        var datos = credenciales.GetConsultaDiasSolicitud();
                        var dias_sol = Convert.ToInt32(datos.Rows[0]["DIAS_SOLICITUD"].ToString());
                        var fecha_actual = Convert.ToDateTime(datos.Rows[0]["GETDATENOW"].ToString()).ToString("yyyy-MM-dd");
                        var fecha_poliza = Convert.ToDateTime(nomina.Rows[0]["NOMINA_FCARD"].ToString()).ToString("yyyy-MM-dd");

                        // Difference in days, hours, and minutes.
                        TimeSpan ts = Convert.ToDateTime(fecha_poliza) - Convert.ToDateTime(fecha_actual);

                        // Difference in days.
                        var differenceInDays = ts.Days;

                        if (differenceInDays > dias_sol)
                        {
                            if (nomina.Rows[0]["MENSAJE"].ToString() == "SVIGENTE_SEMPRESA" || nomina.Rows[0]["MENSAJE"].ToString() == "SVIGENTE_NEMPRESA")
                            {
                                this.Alerta("El Colaborador(a):\\n *" + cedulaconlaborador.ToString() + " - " + nomina.Rows[0]["NOMBRES"].ToString() + "\\nEmpresa:\\n *" + nomina.Rows[0]["EMPE_NOM"].ToString() + "\\nTiene una credencial vigente."); //:\\n *" + "Fecha Inicio: " + Convert.ToDateTime(nomina.Rows[0]["NOMINA_FING"]).ToString("dd-MM-yyyy") + "\\n *Fecha Caducidad: " + Convert.ToDateTime(nomina.Rows[0]["NOMINA_FCARD"]).ToString("dd-MM-yyyy") + "\\nContáctese con el Dpto. de Credenciales:\\n *046006300 Ext: 6004-6005-6007-6009\\n *PermisosyCredenciales@cgsa.com.ec");
                                var script = "fEmpresa();";
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", script, true);
                                setVariables();
                                txtcipas.Focus();
                                return;
                            }
                        }
                        else
                        {
                            var tipo = "2"; //Renovación
                            Agregar(tipo);
                            return;
                        }
                    }
                    else
                    {
                        if (nomina.Rows[0]["MENSAJE"].ToString() == "SVIGENTE_SEMPRESA" || nomina.Rows[0]["MENSAJE"].ToString() == "SVIGENTE_NEMPRESA")
                        {
                            this.Alerta("El Colaborador(a):\\n *" + cedulaconlaborador.ToString() + " - " + nomina.Rows[0]["NOMBRES"].ToString() + "\\nEmpresa:\\n *" + nomina.Rows[0]["EMPE_NOM"].ToString() + "\\nTiene una credencial vigente."); //:\\n *" + "Fecha Inicio: " + Convert.ToDateTime(nomina.Rows[0]["NOMINA_FING"]).ToString("dd-MM-yyyy") + "\\n *Fecha Caducidad: " + Convert.ToDateTime(nomina.Rows[0]["NOMINA_FCARD"]).ToString("dd-MM-yyyy") + "\\nContáctese con el Dpto. de Credenciales:\\n *046006300 Ext: 6004-6005-6007-6009\\n *PermisosyCredenciales@cgsa.com.ec");
                            var script = "fEmpresa();";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", script, true);
                            setVariables();
                            txtcipas.Focus();
                            return;
                        }
                    }
                }
                //if (nomina.Rows.Count > 0)
                //{
                //    for (int n = 0; n < nomina.Rows.Count; n++)
                //    {
                //        var nompuerta = credenciales.GetConsultaNom_Puerta(nomina.Rows[n]["NOMINA_ID"].ToString());
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
                //            Agregar(tipo);
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
                //    Agregar(tipo);
                //}
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaborador.aspx.cs", "btnAgregar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
                //return;
            }
        }
        public void Agregar(string tipo)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime fechaexplic = new DateTime();

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
            var script = "fEmpresa();fValidaEmision();";
            if (credenciales.GetTipoSolicitudXRUC(rucempresa, Session["stipoempresacoloborador"].ToString()))
            {
                if (ddlTipoLicencia.SelectedItem.Value == "0")
                {
                    this.Alerta("Seleccione el Tipo de Licencia.");
                    txtaredes.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtfecexplic.Text))
                {
                    this.Alerta("Escriba la Fecha de Expiración de la Licencia.");
                    txttiempoestadia.Focus();
                    return;
                }
                else
                {
                    if (!DateTime.TryParseExact(txtfecexplic.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaexplic))
                    {
                        this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", txtfecexplic.Text));
                        txtfecexplic.Focus();
                        return;
                    }
                }
                dtColaboradores.Rows.Add(
                tipo,
                txtnombres.Text,
                txtapellidos.Text,
                txtcipas.Text,
                txttiposangre.Text,
                txtdirdom.Text,
                txtteldom.Text,
                tmailinfocli.Value,
                txtlugarnacimiento.Text,
                fechanac.ToString("yyyy-MM-dd"),
                ddlTipoLicencia.SelectedItem.Value,
                txtcargo.Text,
                fechaexplic.ToString("yyyy-MM-dd"),
                txtNota.Text
                );
            }
            else
            {
                if (credenciales.GetTipoSolicitudXRUCOPC(rucempresa, Session["stipoempresacoloborador"].ToString()))
                {
                    var cargoopc = ConfigurationManager.AppSettings["cargoopc"].ToString();
                    if (txtcargo.Text.Contains(cargoopc))
                    {
                        if (ddlTipoLicencia.SelectedItem.Value == "0")
                        {
                            this.Alerta("Escriba el Tipo de Licencia.");
                            txtaredes.Focus();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", script, true);
                            return;
                        }
                        if (string.IsNullOrEmpty(txtfecexplic.Text))
                        {
                            this.Alerta("Escriba la Fecha de Expiración de la Licencia.");
                            txttiempoestadia.Focus();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", script, true);
                            return;
                        }
                        else
                        {
                            if (!DateTime.TryParseExact(txtfecexplic.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaexplic))
                            {
                                this.Alerta(string.Format("<strong>&nbsp;EL FORMATO DE FECHA DEBE SER dia/Mes/Anio {0}</strong>", txtfecexplic.Text));
                                txtfecexplic.Focus();
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", script, true);
                                return;
                            }
                        }
                    }
                }
                dtColaboradores.Rows.Add(
                tipo,
                txtnombres.Text,
                txtapellidos.Text,
                txtcipas.Text,
                txttiposangre.Text,
                txtdirdom.Text,
                txtteldom.Text,
                tmailinfocli.Value,
                txtlugarnacimiento.Text,
                fechanac.ToString("yyyy-MM-dd"),
                string.Empty,
                txtcargo.Text,
                string.Empty,
                txtNota.Text
                );
            }
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
            if (ddlTipoLicencia.Items.Count > 0)
            {
                if (ddlTipoLicencia.Items.FindByValue("0") != null)
                {
                    ddlTipoLicencia.Items.FindByValue("0").Selected = true;
                }
                ddlTipoLicencia.SelectedValue = "0";
            }
            txtcargo.Text = "";
            txtfecexplic.Text = "";
            rbemision.Checked = true;
            rbrenovacion.Checked = false;
            gvColaboradores.Columns[13].Visible = false;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", script, true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string error = string.Empty;
                var t = credenciales.GetNominaOnlyControl(rucempresa, out error);
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
                    cedula = txtcriterioconsulta.Text;
                    var result = from rows in t.AsEnumerable()
                                 where rows.Field<string>("CEDULA").Contains(cedula)
                                 select rows;
                      t = result.AsDataView().ToTable();
                    /*
                    if (t.Rows.Count > 0)
                    {
                        cedula = txtcriterioconsulta.Text;
                        var consulta = "CEDULA Like '%" + cedula + "%'";
                        var resultado = t.Select(consulta);
                        if (resultado.AsEnumerable().Count() > 0)
                        {
                            t = resultado.AsEnumerable().CopyToDataTable();   
                        }
                    }*/
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
                    txtcipas.Text = "";
                    txtnombres.Text = "";
                    txtapellidos.Text = "";
                    txttiposangre.Text = "";
                    txtdirdom.Text = "";
                    txtteldom.Text = "";
                    tmailinfocli.Value = "";
                    txtlugarnacimiento.Text = "";
                    txtfechanacimiento.Text = "";
                    txtcargo.Text = "";
                    //ddlTipoLicencia.SelectedItem.Value = "0";
                    txtfecexplic.Text = "";
                    
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
                    txtcargo.BackColor = System.Drawing.Color.Gray;
                    txtfecexplic.BackColor = System.Drawing.Color.Gray;
                    ddlTipoLicencia.BackColor = System.Drawing.Color.Gray;
                    */
                    var script = "fValidaRenovacion();fEmpresa();";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", script, true);

                    rbrenovacion.Checked = true;
                    txtcriterioconsulta.Focus();
                    
                    populateDrop(ddlTipoLicencia, credenciales.getTipoLicencia(ddlTipoEmpresa.SelectedItem.Value));
                    if (ddlTipoLicencia.Items.Count > 0)
                    {
                        if (ddlTipoLicencia.Items.FindByValue("0") != null)
                        {
                            ddlTipoLicencia.Items.FindByValue("0").Selected = true;
                        }
                        ddlTipoLicencia.SelectedValue = "0";
                    }
                    this.Alerta("No se encontraron resultados, asegurese que ha escrito correctamente los criterios de consulta.");
                    return;
                }
                if (t.Rows.Count == 1)
                {
                    txtcipas.Text = t.Rows[0]["CEDULA"].ToString();
                    txtnombres.Text = t.Rows[0]["NOMBRES"].ToString();
                    txtapellidos.Text = t.Rows[0]["APELLIDOS"].ToString();
                    txtcargo.Text = t.Rows[0]["CARGO"].ToString();
                    txtcargo.Enabled = false;
                    var detalle = credenciales.GetConsultaDatosColaboradorSCA(rucempresa, t.Rows[0]["CEDULA"].ToString());
                    if (detalle.Rows.Count > 0)
                    {
                        txttiposangre.Text = detalle.Rows[0]["TIPOSANGRE"].ToString();
                        txtdirdom.Text = detalle.Rows[0]["DIRECCIONDOM"].ToString();
                        txtteldom.Text = detalle.Rows[0]["TELFDOM"].ToString();
                        tmailinfocli.Value = detalle.Rows[0]["EMAIL"].ToString();
                        txtlugarnacimiento.Text = detalle.Rows[0]["LUGARNAC"].ToString();
                        txtfechanacimiento.Text = Convert.ToDateTime(detalle.Rows[0]["FECHANAC"]).ToString("dd/MM/yyyy");
                        txtcargo.Text = detalle.Rows[0]["CARGO"].ToString();
                        ddlTipoLicencia.SelectedItem.Value = detalle.Rows[0]["TIPOLICENCIA"].ToString();
                        txtfecexplic.Text = Convert.ToDateTime(detalle.Rows[0]["FECHAEXPLICENCIA"]).ToString("dd/MM/yyyy");
                    }

                    
                    txtcipas.BackColor = System.Drawing.Color.White;
                    txtnombres.BackColor = System.Drawing.Color.White;
                    txtapellidos.BackColor = System.Drawing.Color.White;
                    txttiposangre.BackColor = System.Drawing.Color.White;
                    txtdirdom.BackColor = System.Drawing.Color.White;
                    txtteldom.BackColor = System.Drawing.Color.White;
                    tmailinfocli.Attributes["style"] = "width:400px; background-color:White";
                    txtlugarnacimiento.BackColor = System.Drawing.Color.White;
                    txtfechanacimiento.BackColor = System.Drawing.Color.White;
                    
                    txtcargo.BackColor = System.Drawing.Color.White;
                    if(ddlTipoEmpresa.SelectedItem.Value == "12"){
                        ddlTipoLicencia.BackColor = System.Drawing.Color.White;
                        txtfecexplic.BackColor = System.Drawing.Color.White;
                        ddlTipoLicencia.Enabled = true;
                        txtfecexplic.Enabled = true;
                    }
                    else
                    {
                        ddlTipoLicencia.BackColor = System.Drawing.Color.Gray;
                        txtfecexplic.BackColor = System.Drawing.Color.Gray;
                        ddlTipoLicencia.Enabled = false;
                        txtfecexplic.Enabled = false;
                    }
                    
                    //ddlTipoLicencia.BackColor = System.Drawing.Color.White;
                    rbrenovacion.Checked = true;
                    txtcriterioconsulta.Focus();
                    txtcriterioconsulta.Text = "";
                    //txtcipas.Enabled = false;
                    //txtnombres.Enabled = false;
                    //txtapellidos.Enabled = false;
                    //txttiposangre.Enabled = false;
                    //txtdirdom.Enabled = false;
                    //txtteldom.Enabled = false;
                    //tmailinfocli.Disabled = true;
                    //txtlugarnacimiento.Enabled = false;
                    //txtfechanacimiento.Enabled = false;
                    //txtcargo.Enabled = false;
                    //txttiplic.Enabled = false;
                    //txtfecexplic.Enabled = false;
                    return;
                }
                if (rbcedula.Checked)
                {
                    var url = "window.open('" + "../catalogo/colaboradores?sidcedula=" + txtcriterioconsulta.Text + "','_blank', 'width=850,height=480')";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", url, true);
                    return;
                }
                if (rbnombres.Checked)
                {
                    var url = "window.open('" + "../catalogo/colaboradores?sidnombres=" + txtcriterioconsulta.Text + "','_blank', 'width=850,height=480')";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", url, true);
                    return;
                }
                if (rbapellidos.Checked)
                {
                    var url = "window.open('" + "../catalogo/colaboradores?sidapellidos=" + txtcriterioconsulta.Text + "','_blank', 'width=850,height=480')";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", url, true);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaborador.aspx.cs", "btnBuscar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        private void scriptAlert(string script)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
        }
        protected void TimerPb_Tick(object sender, EventArgs e)
        {
            if (TimerPb.Enabled)
            {
            //    if (Convert.ToInt32(Session["valor"].ToString()) == dtColaboradores.Rows.Count)
            //    {
            //        Session["valor"] = "0";
            //        var script2 = "<script language='JavaScript'>modalPanel();</script>";
            //        ScriptManager.RegisterStartupScript(this, typeof(Page), "script", script2, false);
            //        TimerPb.Enabled = false;
            //    }
            //    else
            //    {
                    System.Threading.Thread.Sleep(1650);
                    var valida = dtColaboradores.Rows.Count - contador;
                    if (valida == 0)
                    {
                        if (bandera)
                        {
                            if (!string.IsNullOrEmpty(ErrorColoboradores))
                            {
                                var msgerror = "Solicitud registrada exitosamente.\\n\\n*Colaborador(es) no registrados por favor realice un nueva solicitud para los mismos:" + ErrorColoboradores;
                                var script = "<script language='JavaScript'>var r=alert('" + msgerror + "');if(r==true){window.location='../csl/menu';}else{window.location='../csl/menu';}</script>";
                                scriptAlert(script);
                                TimerPb.Enabled = false;
                            }
                            else
                            {
                                var script = "<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='../csl/menu';}else{window.location='../csl/menu';}</script>";
                                scriptAlert(script);
                                TimerPb.Enabled = false;
                            }   
                        }
                        else
                        {
                            bandera = true;
                        }
                    }
                    else
                    {
                        insertar(Convert.ToInt32(Session["identity"]));
                    }
                //}
            }
        }
        private void insertar(Int32 identity)
        {
            try
            {
                ExportFileUpload(identity);
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudcolaborador.aspx.cs", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        protected void ddlTipoEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {

            //ddlTipoLicencia.Enabled = true;
        }
    }
}