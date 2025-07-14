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
    public partial class solicitudvehiculo : System.Web.UI.Page
    {
        private string _Id_Opcion_Servicio = string.Empty;

        private GetFile.Service getFile = new GetFile.Service();
        public string porcentaje
        {
            get { return (string)Session["porcentajeveh"]; }
            set { Session["porcentajeveh"] = value; }
        }
        public bool bandera
        {
            get { return (bool)Session["banberaveh"]; }
            set { Session["banberaveh"] = value; }
        }
        public int contador
        {
            get { return (int)Session["contadorveh"]; }
            set { Session["contadorveh"] = value; }
        }
        public String ErrorVehiculos
        {
            get { return (String)Session["ErrorVeh"]; }
            set { Session["ErrorVeh"] = value; }
        }
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosVehiculosSolicitud"]; }
            set { Session["dtDocumentosVehiculosSolicitud"] = value; }
        }
        public DataTable dtVehiculos
        {
            get { return (DataTable)Session["dtSolicitudVehiculos"]; }
            set { Session["dtSolicitudVehiculos"] = value; }
        }
        public DataTable myDataTable_
        {
            get { return (DataTable)Session["myDataTable_Veh"]; }
            set { Session["myDataTable_Veh"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["rucempresasolicitudvehiculo"]; }
            set { Session["rucempresasolicitudvehiculo"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailsolicitudvehiculo"]; }
            set { Session["useremailsolicitudvehiculo"] = value; }
        }
        private String xmlDocumentos;
        private String xmlVehiculos;
        private String itemTipoSolicitud;
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
                LoadIni();
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
                DataTable myDataTable = Session["dtDocumentosVehiculosSolicitud"] as DataTable;
                if (dtVehiculos.Rows.Count <= 0)
                {
                    this.Alerta("Agregue al menos un Vehiculo.");
                    return;
                }
                for (int i = 0; i <= dtVehiculos.Rows.Count - 1; i++)
                {
                    Label Placa = (Label)gvVehiculos.Rows[i].FindControl("tplaca");
                    CheckBox chkEstadoDocumentos = (CheckBox)gvVehiculos.Rows[i].FindControl("chkEstadoDocumentos");
                    var result = from myRow in myDataTable.AsEnumerable()
                                 where myRow.Field<string>("Placa") == Placa.Text
                                 select myRow;
                    if (result.AsDataView().Count <= 0)
                    {
                        PlacasFaltantes = PlacasFaltantes + " \\n *" + Placa.Text;
                        gvVehiculos.Columns[9].Visible = true;
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
                    this.Alerta("Las siguientes Placa / No. Serie (para Montacargas), NO tienen documentos cargados:" + PlacasFaltantes);
                    return;
                }
                var vscript = "<script language='JavaScript'>Panel();</script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "script", vscript, false);
                string mensaje = null;
                Int32 identity = 0;
                string nombreempresa = CslHelper.getShiperName(rucempresa);
                if (!credenciales.AddSolicitudVehiculoCab(
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
                    ErrorVehiculos = string.Empty;
                    myDataTable_ = Session["dtDocumentosVehiculosSolicitud"] as DataTable;
                    if (myDataTable_.Columns.Count == 6)
                    {
                        myDataTable_.Columns.Add("RutaOriginal");
                        myDataTable_.Columns.Add("NombreOriginal");
                    }
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudvehiculo.aspx.cs", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        private void ExportFileUpload(Int32 identity)
        {
            bool enviaemail = false;
            //String ErrorColoboradores = string.Empty;
            DataTable myDataTable = Session["myDataTable_Veh"] as DataTable;
            //if (myDataTable.Columns.Count == 6)
            //{
            //    myDataTable.Columns.Add("RutaOriginal");
            //    myDataTable.Columns.Add("NombreOriginal");
            //}
            string nomdoc = string.Empty;
            for (int a = contador; a < dtVehiculos.Rows.Count; a++)
            {
                var resultdoc = from myRow in myDataTable.AsEnumerable()
                                where myRow.Field<string>("Placa") == dtVehiculos.Rows[a]["Placa"].ToString()
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
                DataTable dtCol = dtVehiculos;
                var resultcol = from myRow in dtCol.AsEnumerable()
                                where myRow.Field<string>("Placa") == dtVehiculos.Rows[a]["Placa"].ToString()
                                select myRow;
                dtCol = resultcol.AsDataView().ToTable();
                dtCol.TableName = "Vehiculos";
                StringWriter swCol = new StringWriter();
                dtCol.WriteXml(swCol);
                xmlVehiculos = swCol.ToString();

                DataTable dtDoc = dtDocumentos;
                dtDoc.AcceptChanges();
                dtDoc.TableName = "Documentos";
                StringWriter swDocumentos = new StringWriter();
                dtDoc.WriteXml(swDocumentos);
                xmlDocumentos = swDocumentos.ToString();

                var valida = dtVehiculos.Rows.Count - contador;
                if (valida == 1)
                {
                    enviaemail = true;
                }

                string nombreempresa = CslHelper.getShiperName(rucempresa);
                string mensaje = null;
                if (!credenciales.AddSolicitudVehiculoDet(
                   nombreempresa,
                   identity,
                   useremail,
                   cbltiposolicitud.SelectedValue,
                   ddlTipoEmpresa.SelectedValue,
                   rucempresa,
                   xmlVehiculos,
                   xmlDocumentos,
                   Page.User.Identity.Name.ToUpper().ToUpper(),
                   enviaemail,
                   out mensaje))
                {
                    if (!string.IsNullOrEmpty(mensaje))
                    {
                        if (mensaje != "OK")
                        {
                            this.Alerta(mensaje);
                        }
                    }
                    ErrorVehiculos = ErrorVehiculos + " \\n *" + dtVehiculos.Rows[a]["Placa"].ToString();
                }
                else
                {
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
                        //if (!string.IsNullOrEmpty(ErrorVehiculos))
                        //{
                        //    var msgerror = "Solicitud registrada exitosamente.\\n\\n*Vehículo(s) no registrados por favor realice un nueva solicitud para los mismos:" + ErrorVehiculos;
                        //    var script = "<script language='JavaScript'>var r=alert('" + msgerror + "');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>";
                        //    scriptAlert(script);
                        //}
                        //else
                        //{
                        //    var script = "<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>";
                        //    scriptAlert(script);
                        //}
                    }
                }
                a = dtVehiculos.Rows.Count;
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
            porcentaje = Math.Truncate(((Convert.ToDecimal(contador) / Convert.ToDecimal(dtVehiculos.Rows.Count)) * 100)).ToString();
            var vscript = "<script language='JavaScript'>move(" + inicio.ToString() + "," + porcentaje.ToString() + ");</script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "script", vscript, false);
            //***********************************************************************
            //for (int i = 0; i <= myDataTable.Rows.Count - 1; i++)
            //{
            //    var rutaoriginal = myDataTable.Rows[i][3];
            //    var nombreoriginal = myDataTable.Rows[i][4];
            //    ExportFiles(myDataTable.Rows[i][3].ToString(), myDataTable.Rows[i][4].ToString(), myDataTable.Rows[i][5].ToString(), out nomdoc);
            //    String dateServer = credenciales.GetDateServer();
            //    String rutaServer = dateServer;
            //    myDataTable.Rows[i][2] = cbltiposolicitud.SelectedValue;
            //    myDataTable.Rows[i][3] = rutaServer;
            //    myDataTable.Rows[i][4] = nomdoc;
            //    myDataTable.Rows[i][6] = rutaoriginal;
            //    myDataTable.Rows[i][7] = nombreoriginal;
            //}
            //for (int i = 0; i <= myDataTable.Rows.Count - 1; i++)
            //{
            //    var pathDoc = myDataTable.Rows[i][6].ToString() + myDataTable.Rows[i][7].ToString() + myDataTable.Rows[i][5].ToString();
            //    if (File.Exists(pathDoc))
            //    {
            //        File.Delete(pathDoc);
            //    }
            //}
            //dtVehiculos.AcceptChanges();
            //dtVehiculos.TableName = "Vehiculos";
            //StringWriter swVehiculos = new StringWriter();
            //dtVehiculos.WriteXml(swVehiculos);
            //xmlVehiculos = swVehiculos.ToString();
            //myDataTable.AcceptChanges();
            //myDataTable.TableName = "Documentos";
            //StringWriter swDocumentos = new StringWriter();
            //myDataTable.WriteXml(swDocumentos);
            //xmlDocumentos = swDocumentos.ToString();
        }
        private void ExportFiles(string path, string filename, string extension, out string nomdoc)
        {
            /*
            string rutafile = Server.MapPath(path + filename + extension);
            string finalname2;
            string cpath = path + filename + extension;
            FileStream fileStream;
            credenciales.ReadBinaryFile(path, filename + extension, out fileStream);
            var p = CSLSite.app_start.CredencialesHelper.UploadFile(Server.MapPath(cpath), fileStream, out finalname2);
            if (!p)
            {
               // this.Alerta(finalname2);   
            }*/
            nomdoc = filename;
            //nomdoc = finalname2;
            /*nomdoc = string.Empty;
            if (File.Exists(path + filename + extension))
            {
                FileStream fileStream;
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
               
                var id = DateTime.Now.ToString("ddMMyyyyhhmmssff");
                var nombrearchivo = filename + "_" + id + extension;
                nomdoc = filename + "_" + id;
                getFile.UploadFile(credenciales.ReadBinaryFile(path, filename + extension, out fileStream), rutaServer, nombrearchivo);
                fileStream.Close();
            }*/
        }
        private void LoadIni()
        {
            try
            {
                populateDrop(cbltiposolicitud, credenciales.getTipoSolicitudVeh(4));
                if (cbltiposolicitud.Items.Count > 0)
                {
                    if (cbltiposolicitud.Items.FindByValue("0") != null)
                    {
                        cbltiposolicitud.Items.FindByValue("0").Selected = true;
                    }
                    cbltiposolicitud.SelectedValue = "0";
                }
                populateDrop(ddlCategoria, credenciales.getCategoriaVehiculo(23));
                if (ddlCategoria.Items.Count > 0)
                {
                    if (ddlCategoria.Items.FindByValue("0") != null)
                    {
                        ddlCategoria.Items.FindByValue("0").Selected = true;
                    }
                    ddlCategoria.SelectedValue = "0";
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
                DataTable myDataTable = Session["dtDocumentosVehiculosSolicitud"] as DataTable;
                if (myDataTable != null)
                {
                    for (int i = 0; i <= myDataTable.Rows.Count - 1; i++)
                    {
                        var pathDoc = myDataTable.Rows[i][3].ToString() + myDataTable.Rows[i][4].ToString() + myDataTable.Rows[i][5].ToString();
                        if (File.Exists(pathDoc))
                        {
                            File.Delete(pathDoc);
                        }
                    }
                }
                //var areaOnlyControl = credenciales.GetConsultaArea();
                //var error_consulta = string.Empty;
                //error_consulta = string.Empty;
                //var actividadOnlyControl = credenciales.GetActividadOnlyControl(rucempresa, out error_consulta);
                //if (!string.IsNullOrEmpty(error_consulta))
                //{
                //    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                //}
                //populateDropDownList(ddlAreaOnlyControl, areaOnlyControl, "* Elija *", "AREA_ID", "AREA_NOM", false);
                //populateDropDownList(ddlActividadOnlyControl, actividadOnlyControl, "* Elija *", "ACT_ID", "ACT_NOM", false);
                //ddlAreaOnlyControl.SelectedItem.Text = "* Elija *";
                //ddlActividadOnlyControl.SelectedItem.Text = "* Elija *";

                var error_consulta = string.Empty;
                var actividadOnlyControl = credenciales.GetActividadPermitidaPermisoDeAcceso();
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                }
                populateDropDownList(ddlActividadOnlyControl, actividadOnlyControl, "* Elija *", "TIPO", "DESCRIPCION", false);
                ddlActividadOnlyControl.SelectedValue = "0";

                Session["dtDocumentosVehiculosSolicitud"] = new DataTable();
                Session["dtSolicitudVehiculos"] = new DataTable();
                dtVehiculos.Columns.Add("ClaseTipo");
                dtVehiculos.Columns.Add("Marca");
                dtVehiculos.Columns.Add("Modelo");
                dtVehiculos.Columns.Add("Color");
                dtVehiculos.Columns.Add("Placa");
                dtVehiculos.Columns.Add("TipoCertificado");
                dtVehiculos.Columns.Add("Certificado");
                dtVehiculos.Columns.Add("Categoria");
                dtVehiculos.Columns.Add("FechaPoliza");
                dtVehiculos.Columns.Add("FechaMtop");
                dtVehiculos.Columns.Add("Tipo");
                dtVehiculos.Columns.Add("DesCategoria");
                //dtVehiculos.Columns.Add("FechaIngreso");
                //dtVehiculos.Columns.Add("FechaSalida");
                dtVehiculos.Columns.Add("Area");
                dtVehiculos.Columns.Add("IdTipoEmpresa");
                dtVehiculos.Columns.Add("IdCategoria");
                dtVehiculos.Columns.Add("Nota");
                //dtVehiculos.Columns.Add("ActividadPermitida");
                this.gvVehiculos.DataSource = dtVehiculos;
                this.gvVehiculos.DataBind();
                txtClaseTipo.Text = "";
                txtMarca.Text = "";
                txtModelo.Text = "";
                txtColor.Text = "";
                txtPlaca.Text = "";
                gvVehiculos.Columns[5].Visible = false;
                contador = 0;
                porcentaje = "1";
                TimerPb.Enabled = false;
                //txtPlaca.Focus();
            }
            catch (Exception ex)
            {
                Session["dtDocumentosVehiculosSolicitud"] = new DataTable();
                Session["dtSolicitudVehiculos"] = new DataTable();
                dtVehiculos.Columns.Add("ClaseTipo");
                dtVehiculos.Columns.Add("Marca");
                dtVehiculos.Columns.Add("Modelo");
                dtVehiculos.Columns.Add("Color");
                dtVehiculos.Columns.Add("Placa");
                dtVehiculos.Columns.Add("TipoCertificado");
                dtVehiculos.Columns.Add("Certificado");
                dtVehiculos.Columns.Add("Categoria");
                dtVehiculos.Columns.Add("FechaPoliza");
                dtVehiculos.Columns.Add("FechaMtop");
                dtVehiculos.Columns.Add("Tipo");
                dtVehiculos.Columns.Add("DesCategoria");
                //dtVehiculos.Columns.Add("FechaIngreso");
                //dtVehiculos.Columns.Add("FechaSalida");
                dtVehiculos.Columns.Add("Area");
                dtVehiculos.Columns.Add("IdTipoEmpresa");
                dtVehiculos.Columns.Add("IdCategoria");
                dtVehiculos.Columns.Add("Nota");
                //dtVehiculos.Columns.Add("ActividadPermitida");
                gvVehiculos.DataSource = dtVehiculos;
                gvVehiculos.DataBind();
                txtClaseTipo.Text = "";
                txtMarca.Text = "";
                txtModelo.Text = "";
                txtColor.Text = "";
                txtPlaca.Text = "";
                gvVehiculos.Columns[5].Visible = false;
                contador = 0;
                porcentaje = "1";
                TimerPb.Enabled = false;
                var number = log_csl.save_log<Exception>(ex, "solicitudvehiculo.aspx.cs", "LoadIni()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
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
        //private void populateDropDownList(DropDownList dp, DataTable origen, string mensaje, string id, string descripcion, bool val)
        //{
        //    if (val)
        //    {
        //        origen.Rows.Add("0", "0", mensaje);
        //    }
        //    else
        //    {
        //        origen.Rows.Add("0", mensaje);
        //    }
        //    DataView dvorigen = new DataView();
        //    dvorigen = origen.DefaultView;
        //    dvorigen.Sort = descripcion;
        //    dp.DataSource = dvorigen;
        //    dp.DataValueField = id;
        //    dp.DataTextField = descripcion;
        //    dp.DataBind();
        //}
        protected void gvVehiculos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName.Equals("Add") == true)
            //{
            //    try
            //    {
            //        int rowcount = dtVehiculos.Rows.Count - 1;
            //        for (int i = 0; i <= rowcount; i++)
            //        {
            //            TextBox ClaseTipo = (TextBox)gvVehiculos.Rows[i].FindControl("tclasetipo");
            //            TextBox Marca = (TextBox)gvVehiculos.Rows[i].FindControl("tmarca");
            //            TextBox Modelo = (TextBox)gvVehiculos.Rows[i].FindControl("tmodelo");
            //            TextBox Color = (TextBox)gvVehiculos.Rows[i].FindControl("tcolor");
            //            TextBox Placa = (TextBox)gvVehiculos.Rows[i].FindControl("tplaca");
            //            if (string.IsNullOrEmpty(ClaseTipo.Text))
            //            {
            //                this.Alerta("El campo Clase/Tipo no debe ser nulo");
            //                ClaseTipo.Focus();
            //                return;
            //            }
            //            if (string.IsNullOrEmpty(Marca.Text))
            //            {
            //                this.Alerta("El campo Marca no debe ser nulo");
            //                Marca.Focus();
            //                return;
            //            }
            //            if (string.IsNullOrEmpty(Modelo.Text))
            //            {
            //                this.Alerta("El campo Modelo no debe ser nulo");
            //                Modelo.Focus();
            //                return;
            //            }
            //            if (string.IsNullOrEmpty(Color.Text))
            //            {
            //                this.Alerta("El campo Color no debe ser nulo");
            //                Color.Focus();
            //                return;
            //            }
            //            if (string.IsNullOrEmpty(Placa.Text))
            //            {
            //                this.Alerta("El campo Placa no debe ser nulo");
            //                Placa.Focus();
            //                return;
            //            }
            //            dtVehiculos.Rows[i]["ClaseTipo"] = ClaseTipo.Text;
            //            dtVehiculos.Rows[i]["Marca"] = Marca.Text;
            //            dtVehiculos.Rows[i]["Modelo"] = Modelo.Text;
            //            dtVehiculos.Rows[i]["Color"] = Color.Text;
            //            dtVehiculos.Rows[i]["Placa"] = Placa.Text;
            //        }
            //        LoadGrid(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            //    }
            //    catch (Exception ex)
            //    {
            //        this.Alerta(ex.Message);
            //        //dtVehiculos.Rows.RemoveAt(grid_row.RowIndex);
            //        //return;
            //    }
            //}
        }
        protected void gvVehiculos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                DataTable myDataTable = Session["dtDocumentosVehiculosSolicitud"] as DataTable;
                var result = from myRow in myDataTable.AsEnumerable()
                             where myRow.Field<string>("Placa") != dtVehiculos.Rows[e.RowIndex][4].ToString()
                             select myRow;
                if (result.AsDataView().Count > 0)
                {
                    Session["dtDocumentosVehiculosSolicitud"] = result.AsDataView().ToTable();
                }
                else if (result.AsDataView().Count == 0)
                {
                    Session["dtDocumentosVehiculosSolicitud"] = new DataTable();
                }
                DataTable dt = Session["dtDocumentosVehiculosSolicitud"] as DataTable;
                dtVehiculos.Rows.RemoveAt(e.RowIndex);
                gvVehiculos.DataSource = dtVehiculos;
                gvVehiculos.DataBind();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudvehiculo.aspx.cs", "gvVehiculos_RowDeleting()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string tipo = "";
                var nomina = credenciales.GetConsultaVehiculo(rucempresa, txtPlaca.Text.Trim());
                if (nomina.Rows[0]["MENSAJE"].ToString() == "NUEVO" || nomina.Rows[0]["MENSAJE"].ToString() == "SEMPRESA" || nomina.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                {
                    if (nomina.Rows[0]["MENSAJE"].ToString() == "SEMPRESA")
                    {
                        tipo = "2"; //Renovación
                    }
                    else if (nomina.Rows[0]["MENSAJE"].ToString() == "NUEVO" || nomina.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                    {
                        tipo = "1"; //Emisión
                    }
                }
                else
                {
                    if (ddlCategoria.SelectedItem.Value == "CAT" || ddlCategoria.SelectedItem.Value == "MON")
                    {
                        var datos = credenciales.GetConsultaDiasSolicitud();
                        var dias_sol = Convert.ToInt32(datos.Rows[0]["DIAS_SOLICITUD"].ToString());
                        var fecha_actual = Convert.ToDateTime(datos.Rows[0]["GETDATENOW"].ToString()).ToString("yyyy-MM-dd");
                        var fecha_poliza = Convert.ToDateTime(nomina.Rows[0]["VE_POLIZA"].ToString()).ToString("yyyy-MM-dd");

                        // Difference in days, hours, and minutes.
                        TimeSpan ts = Convert.ToDateTime(fecha_poliza) - Convert.ToDateTime(fecha_actual);

                        // Difference in days.
                        var differenceInDays = ts.Days;

                        if (differenceInDays > dias_sol)
                        {
                            if (nomina.Rows[0]["MENSAJE"].ToString() == "SVIGENTE_SEMPRESA" || nomina.Rows[0]["MENSAJE"].ToString() == "SVIGENTE_NEMPRESA")
                            {
                                this.Alerta("La Placa:\\n *" + txtPlaca.Text + "\\nEmpresa:\\n *" + nomina.Rows[0]["EMPE_NOM"].ToString() + "\\nTiene un permiso vehicular vigente."); //:\\n *" + "Fecha Vigencia Poliza: " + Convert.ToDateTime(nomina.Rows[0]["VE_POLIZA"]).ToString("dd-MM-yyyy") + "\\nContáctese con el Dpto. de Credenciales:\\n *046006300 Ext: 6004-6005-6007-6009\\n *PermisosyCredenciales@cgsa.com.ec");
                                ddlCategoria.SelectedValue = "0";
                                txtPlaca.Text = "";
                                txtClaseTipo.Text = "";
                                txtMarca.Text = "";
                                txtModelo.Text = "";
                                txtColor.Text = "";
                                txttipocertificado.Text = "";
                                txtnumcertificado.Text = "";
                                txtfechamtop.Text = "";
                                txtfechapoliza.Text = "";
                                var script = "fValidaEmision();";
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", script, true);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (nomina.Rows[0]["MENSAJE"].ToString() == "SVIGENTE_SEMPRESA" || nomina.Rows[0]["MENSAJE"].ToString() == "SVIGENTE_NEMPRESA")
                        {
                            this.Alerta("La Placa:\\n *" + txtPlaca.Text + "\\nEmpresa:\\n *" + nomina.Rows[0]["EMPE_NOM"].ToString() + "\\nTiene un permiso vehicular vigente."); //:\\n *" + "Fecha Vigencia Poliza: " + Convert.ToDateTime(nomina.Rows[0]["VE_POLIZA"]).ToString("dd-MM-yyyy") + "\\nContáctese con el Dpto. de Credenciales:\\n *046006300 Ext: 6004-6005-6007-6009\\n *PermisosyCredenciales@cgsa.com.ec");
                            ddlCategoria.SelectedValue = "0";
                            txtPlaca.Text = "";
                            txtClaseTipo.Text = "";
                            txtMarca.Text = "";
                            txtModelo.Text = "";
                            txtColor.Text = "";
                            txttipocertificado.Text = "";
                            txtnumcertificado.Text = "";
                            txtfechamtop.Text = "";
                            txtfechapoliza.Text = "";
                            var script = "fValidaEmision();";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", script, true);
                            return;
                        }
                    }
                }
                //if (ddlTipoEmpresa.SelectedValue.ToString() == "0")
                //{
                //    this.Alerta("Seleccione el tipo de Empresa.");
                //    return;
                //}

                if (gvVehiculos.Rows.Count == 0)
                {
                    Session["stipoempresavehiculo"] = ddlTipoEmpresa.SelectedValue;
                }
                if (gvVehiculos.Rows.Count >= 1)
                {
                    if (Session["stipoempresavehiculo"].ToString() != ddlTipoEmpresa.SelectedValue.ToString())
                    {
                        ddlTipoEmpresa.SelectedValue = Session["stipoempresavehiculo"].ToString();
                    }
                }

                DateTime fechapoliza = new DateTime();
                DateTime fechamtop = new DateTime();
                bool banderacategoria = false;
                //if (string.IsNullOrEmpty(hfcategoria.Value))
                //{
                if (ddlCategoria.SelectedValue.ToString() == "CAT")
                {
                    banderacategoria = true;
                    CultureInfo enUS = new CultureInfo("en-US");
                    if (!DateTime.TryParseExact(txtfechapoliza.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechapoliza))
                    {
                        this.Alerta(string.Format("El formato de la fecha de poliza debe ser dia/Mes/Anio {0}", txtfechapoliza.Text));
                        txtfechapoliza.Focus();
                        return;
                    }
                    if (!DateTime.TryParseExact(txtfechamtop.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechamtop))
                    {
                        this.Alerta(string.Format("El formato de la fecha mtop debe ser dia/Mes/Anio {0}", txtfechamtop.Text));
                        txtfechamtop.Focus();
                        return;
                    }
                }
                //}
                //else
                //{
                //    if (hfcategoria.Value != "LIV")
                //    {
                //        banderacategoria = true;
                //        CultureInfo enUS = new CultureInfo("en-US");
                //        if (!DateTime.TryParseExact(txtfechapoliza.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechapoliza))
                //        {
                //            this.Alerta(string.Format("El formato de la fecha de poliza debe ser dia/Mes/Anio {0}", txtfechapoliza.Text));
                //            txtfechapoliza.Focus();
                //            return;
                //        }
                //        if (!DateTime.TryParseExact(txtfechamtop.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechamtop))
                //        {
                //            this.Alerta(string.Format("El formato de la fecha mtop debe ser dia/Mes/Anio {0}", txtfechamtop.Text));
                //            txtfechamtop.Focus();
                //            return;
                //        }
                //    }
                //}               
                var results = from myRow in dtVehiculos.AsEnumerable()
                              where myRow.Field<string>("Placa") == txtPlaca.Text
                              select new
                              {
                                  Placa = myRow.Field<string>("Placa")
                              };
                foreach (var item in results)
                {
                    if (item.Placa == txtPlaca.Text)
                    {
                        this.Alerta("Ya se registro la Placa: " + txtPlaca.Text + ", revise por favor.");
                        return;
                    }
                }
                if (banderacategoria == false)
                {
                    dtVehiculos.Rows.Add(txtClaseTipo.Text.ToUpper(), txtMarca.Text.ToUpper(), txtModelo.Text.ToUpper(), txtColor.Text.ToUpper(), txtPlaca.Text.ToUpper(),
                                     null, null,
                                     ddlCategoria.SelectedValue.ToString(), null, null, tipo,
                                     ddlCategoria.SelectedItem.Text, ddlActividadOnlyControl.SelectedItem.Text == "* Elija *" ? null : ddlActividadOnlyControl.SelectedItem.Text,
                                     ddlTipoEmpresa.SelectedItem.Value, ddlCategoria.SelectedItem.Value, txtNota.Text);
                }
                else
                {
                    dtVehiculos.Rows.Add(txtClaseTipo.Text.ToUpper(), txtMarca.Text.ToUpper(), txtModelo.Text.ToUpper(), txtColor.Text.ToUpper(), txtPlaca.Text.ToUpper(),
                                     txttipocertificado.Text.ToUpper(), txtnumcertificado.Text,
                                     ddlCategoria.SelectedValue.ToString(), fechapoliza.ToString("yyyy-MM-dd"), fechamtop.ToString("yyyy-MM-dd"), tipo,
                                     ddlCategoria.SelectedItem.Text, null,
                                     ddlTipoEmpresa.SelectedItem.Value, ddlCategoria.SelectedItem.Value, txtNota.Text);
                }
                this.gvVehiculos.DataSource = dtVehiculos;
                this.gvVehiculos.DataBind();
                txtClaseTipo.Text = "";
                txtMarca.Text = "";
                txtModelo.Text = "";
                txtColor.Text = "";
                txtPlaca.Text = "";
                txtnumcertificado.Text = "";
                txttipocertificado.Text = "";
                txtfechapoliza.Text = "";
                txtfechamtop.Text = "";
                Session["ddlCategoria"] = ddlCategoria.SelectedValue;
                //if (ddlCategoria.Items.FindByValue("0") != null)
                //{
                //    ddlCategoria.Items.FindByValue("0").Selected = true;
                //}
                ddlCategoria.SelectedValue = "0";
                ddlActividadOnlyControl.SelectedValue = "0";
                ddlActividadOnlyControl.Enabled = false;
                ddlActividadOnlyControl.ForeColor = Color.Gray;
                gvVehiculos.Columns[9].Visible = false;
                rbemision.Checked = true;
                var scriptf = "fValidaEmisionf();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", scriptf, true);
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudvehiculo.aspx.cs", "btnAgregar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
                //return;
            }
        }
        protected void itemSelected(object sender, EventArgs e)
        {
            itemTipoSolicitud = cbltiposolicitud.SelectedItem.Value.ToString();
            Session["tipodocsol"] = itemTipoSolicitud;
        }
        private class listaExpo
        {
            public string tmail { get; set; }
            public string txttelexp { get; set; }
            public string txtejecta { get; set; }
            public string txtnombreexp { get; set; }
            public listaExpo(string tmail, string txttelexp, string txtejecta, string txtnombreexp) { this.tmail = tmail.Trim(); this.txttelexp = txttelexp.Trim(); this.txtejecta = txtejecta.Trim(); this.txtnombreexp = txtnombreexp.Trim(); }
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

        }
        //protected void btnBuscar_Click(object sender, EventArgs e)
        //{
        //    //if (!string.IsNullOrEmpty(txtPlaca.Text))
        //    //{
        //    //    this.Alerta("Escriba la Placa o No. Serie (para Montacargas).");
        //    //    txtPlaca.Focus();
        //    //}
        //    //Session["placasolicitudvehiculo"] = txtPlaca.Text;
        //}
        private void scriptAlert(string script)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
        }
        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            var error_consulta = string.Empty;
            var actividadOnlyControl = credenciales.GetActividadPermitidaPermisoDeAcceso();
            if (!string.IsNullOrEmpty(error_consulta))
            {
                Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
            }
            populateDropDownList(ddlActividadOnlyControl, actividadOnlyControl, "* Elija *", "TIPO", "DESCRIPCION", false);
            ddlActividadOnlyControl.SelectedValue = "0";
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
                var valida = dtVehiculos.Rows.Count - contador;
                if (valida == 0)
                {
                    if (bandera)
                    {
                        if (!string.IsNullOrEmpty(ErrorVehiculos))
                        {
                            var msgerror = "Solicitud registrada con novedades.\\n\\n*Colaborador(es) no registrados por favor realice un nueva solicitud para los mismos:" + ErrorVehiculos;
                            var script = "<script language='JavaScript'>var r=alert('" + msgerror + "');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>";
                            scriptAlert(script);
                            TimerPb.Enabled = false;
                        }
                        else
                        {
                            var script = "<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='../cuenta/menu.aspx';}else{window.location='../cuenta/menu.aspx';}</script>";
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
                var number = log_csl.save_log<Exception>(ex, "solicitudvehiculo.aspx.cs", "btsalvar_Click()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                var script = "<script language='JavaScript'>alert('" + string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()) + "');</script>";
                scriptAlert(script);
            }
        }
    }
}