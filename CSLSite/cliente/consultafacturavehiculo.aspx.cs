using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Data;
using System.IO;

namespace CSLSite.cliente
{
    public partial class consultafacturavehiculo : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private DataTable dtDocumentos = new DataTable();
        private String xmlDocumentos;
        public string rucempresa
        {
            get { return (string)Session["rucempresaconsultafactura"]; }
            set { Session["rucempresaconsultafactura"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailconsultafactura"]; }
            set { Session["useremailconsultafactura"] = value; }
        }
        public string numsolicitud
        {
            get { return (string)Session["numsolicitudconsultafactura"]; }
            set { Session["numsolicitudconsultafactura"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            //this.IsAllowAccess();
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
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sinresultado.Visible = false;
                ConsultaInformacion();
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
                ConsultaInformacion();
            }
        }
        private void ConsultaInformacion()
        {
            try
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                }
                try
                {
                    var numsolicitud = Request.QueryString["sid1"];
                    txtsolicitud.Text = numsolicitud;
                }
                catch (Exception)
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Ocurrió un problema al cargar la pagina, la URL ha cambiado.');if(r==true){window.location='http://www.cgsa.com.ec/inicio.aspx';}else{window.location='http://www.cgsa.com.ec/inicio.aspx';};</script>");
                }
                //rucempresa = Request.QueryString["sid2"];
                //Request.QueryString["numsolicitud"].Equals(numsolicitud);
                /*if (!chkTodos.Checked)
                {
                    if (string.IsNullOrEmpty(txtsolicitud.Text) && string.IsNullOrEmpty(this.tfechaini.Text) && string.IsNullOrEmpty(this.tfechafin.Text))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;Ingrese al menos un criterio de consulta. {0}</strong>", "");
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = false;
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.tfechaini.Text) && string.IsNullOrEmpty(this.tfechafin.Text))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;Ingrese la fecha hasta. {0}</strong>", this.tfechafin.Text);
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = true;
                        this.tfechafin.Focus();
                        return;
                    }
                    else if (string.IsNullOrEmpty(this.tfechaini.Text) && !string.IsNullOrEmpty(this.tfechafin.Text))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                        this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;Ingrese la fecha desde. {0}</strong>", this.tfechaini.Text);
                        sinresultado.Visible = true;
                        //btnera.Visible = false;
                        alerta.Visible = false;
                        xfinder.Visible = true;
                        this.tfechaini.Focus();
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.tfechaini.Text) && !string.IsNullOrEmpty(this.tfechafin.Text))
                    {
                        CultureInfo enUS = new CultureInfo("en-US");
                        DateTime fechaini;
                        DateTime fechafin;
                        if (!DateTime.TryParseExact(this.tfechaini.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaini))
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
                            this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;El formato de la Fecha Desde, debe ser dia/Mes/Anio {0}</strong>", this.tfechaini.Text);
                            sinresultado.Visible = true;
                            //btnera.Visible = false;
                            alerta.Visible = false;
                            xfinder.Visible = true;
                            return;
                        }
                        if (!DateTime.TryParseExact(this.tfechafin.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechafin))
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
                            this.sinresultado.InnerHtml = string.Format("<strong>&nbsp;El formato de la Fecha Hasta, debe ser dia/Mes/Anio {0}</strong>", this.tfechafin.Text);
                            sinresultado.Visible = true;
                            //btnera.Visible = false;
                            alerta.Visible = false;
                            xfinder.Visible = true;
                            return;
                        }
                        this.tfechaini.Text = fechaini.ToString();
                        this.tfechafin.Text = fechafin.ToString();
                    }
                }*/
                var DescripcionTipoSolicitud = credenciales.GetDescripcionTipoSolicitudXNumSolicitud(txtsolicitud.Text);
                for (int i = 0; i <= DescripcionTipoSolicitud.Rows.Count - 1; i++)
                {
                    txttipcli.Text = DescripcionTipoSolicitud.Rows[i][0].ToString();
                }
                var tablix = credenciales.GetSolicitudFacturas(rucempresa, txtsolicitud.Text, this.tfechaini.Text, this.tfechafin.Text);
                if (tablix.Rows.Count == 0)
                {
                    sinresultado.Visible = false;
                    //tablePagination.DataSource = null;
                    this.alerta.InnerHtml = "Si los datos que busca no aparecen, revise los criterios de consulta.";
                    alerta.Visible = true;
                    //xfinder.Visible = true;
                    botonera.Visible = false;
                    //divsalir.Visible = true;
                    return;
                }
                if (tablix.Rows[0][4].ToString() == "1")
                {
                    tablePagado.DataSource = tablix;
                    tablePagado.DataBind();
                    sinresultado.Visible = false;
                    this.alertapagado.InnerHtml = "El comprobante de pago ha sido enviado.";
                    xfinderpagado.Style.Value = "Block";
                    xfinderpagado.Visible = true;
                    xfinder.Visible = false;
                    botonera.Visible = false;
                    //divsalir.Visible = true;
                }
                else
                {
                    tablePagination.DataSource = tablix;
                    tablePagination.DataBind();
                    sinresultado.Visible = false;
                    //tablePagination.DataSource = null;
                    this.alerta.InnerHtml = "Nota: Adjunte comprobante de pago y retención en el mismo archivo .pdf. Si se trata de retención electrónica por favor remitirla a la siguiente casilla: </br>" + "<a href='mailto:retencioneselectronicas@cgsa.com.ec'>retencioneselectronicas@cgsa.com.ec</a>";
                    alerta.Visible = true;
                    //xfinder.Visible = true;
                    xfinderpagado.Style.Value = "None";
                    xfinderpagado.Visible = false;
                    botonera.Visible = true;
                    //divsalir.Visible = false;
                }
                this.alerta.InnerHtml = "Confirme que los documentos del colaborador(es) sean los correctos.";
                var tablixVehiculo = credenciales.GetSolicitudVehiculoCliente(txtsolicitud.Text);
                rpVehiculos.DataSource = tablixVehiculo;
                rpVehiculos.DataBind();
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
        //protected void btnSalir_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Response.Write("<script language='JavaScript'>window.close();</script>");
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Alerta(ex.Message);
        //    }
        //}
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                New_ExportFileUpload();
                string mensaje = null;
                if (!credenciales.AddConfirmacionDePago(
                    txtsolicitud.Text.Trim(),
                    CslHelper.getShiperName(rucempresa),
                    rucempresa,
                    useremail,
                    xmlDocumentos,
                    Page.User.Identity.Name,
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    //ConsultaInformacion();
                    //this.Alerta("Comprobante de pago enviado exitosamente, en unos minutos recibirá una notificación via mail.");
                    Response.Write("<script language='JavaScript'>var r=alert('Comprobante de pago enviado exitosamente.');if(r==true){window.close();}else{window.close();}</script>");
                }
            }
            catch (Exception ex)
            {
                this.Alerta(ex.Message);
            }
        }

        private void New_ExportFileUpload()
        {

            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");
            foreach (RepeaterItem item in tablePagination.Items)
            {
                FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                // TextBox txtidsolicitud = item.FindControl("lblIdSolicitud") as TextBox;
                Label txtidsolicitud = item.FindControl("lblIdSolicitud") as Label;
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
                    dtDocumentos.Rows.Add(Convert.ToInt32(txtidsolicitud.Text), finalname);
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
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");
            foreach (RepeaterItem item in tablePagination.Items)
            {
                FileUpload fuAdjuntarFactura = item.FindControl("fsupload") as FileUpload;
                Label lblIdSolicitud = item.FindControl("lblIdSolicitud") as Label;
                if (fuAdjuntarFactura.HasFile)
                {
                    string rutafile = Server.MapPath(fuAdjuntarFactura.FileName);
                    string extensionfile = Path.GetExtension(fuAdjuntarFactura.PostedFile.FileName);
                    string nombrearchivo = Path.GetFileNameWithoutExtension(fuAdjuntarFactura.PostedFile.FileName);
                    string substring = ConfigurationManager.AppSettings["rutaconfacveh"].ToString() + "\\";
                    rutafile = rutafile.Replace(substring, string.Empty);
                    string directorio = Path.GetDirectoryName(rutafile);
                    if (File.Exists(rutafile))
                    {
                        File.Delete(rutafile);
                    }
                    fuAdjuntarFactura.SaveAs(rutafile);
                    string[] files = Directory.GetFiles(directorio, "*" + nombrearchivo + extensionfile);
                    foreach (string s in files)
                    {
                        FileInfo fi = null;
                        try
                        {
                            fi = new FileInfo(s);
                            ExportFiles(fi.Directory.ToString(), fi.Name, lblIdSolicitud.Text, nombrearchivo, extensionfile);
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
                    //fsupload.SaveAs(savePath += fsupload.FileName);
                    //savePath = @"C:\TemporalFileSystemCsaCGSA\";
                    //this.Alerta("Your file was saved as " + fileName);
                }
            }
            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            StringWriter sw = new StringWriter();
            dtDocumentos.WriteXml(sw);
            xmlDocumentos = sw.ToString();
            //System.IO.File.Delete(savePath);
        }
        private void ExportFiles(string path, string filename, string sidsolicitud, string nombrearchivo, string extension)
        {
            path = path + "\\";
            if (File.Exists(path + filename))
            {
                FileStream fileStream;
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                nombrearchivo = nombrearchivo + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssff") + extension;
                dtDocumentos.Rows.Add(sidsolicitud, dateServer + nombrearchivo);
                getFile.UploadFile(credenciales.ReadBinaryFile(path, filename, out fileStream), rutaServer, nombrearchivo);
                fileStream.Close();
            }
        }
    }
}