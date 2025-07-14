using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Configuration;
using System.Globalization;
using ConectorN4;

namespace CSLSite.logincliente
{
    public partial class consultasolicitudrechazoempresa : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private DataTable dtDocumentos = new DataTable();
        private String xmlLogisticaExpo;
        private String xmlDatosExpo;
        private String xmlDocumentos;
        private String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudconsultasolicitudrechazoempresa"]; }
            set { Session["numsolicitudconsultasolicitudrechazoempresa"] = value; }
        }
        private string rucempresa
        {
            get { return (string)Session["rucempresaconsultasolicitudrechazoempresa"]; }
            set { Session["rucempresaconsultasolicitudrechazoempresa"] = value; }
        }
        private string useremail
        {
            get { return (string)Session["useremailconsultasolicitudrechazoempresa"]; }
            set { Session["useremailconsultasolicitudrechazoempresa"] = value; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                //Page.SslOn();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
                try
                {
                    ConsultaInfoSolicitud();
                }
                catch (Exception ex)
                {
                    this.Alerta(ex.Message);
                }
            }
        }
        private void ConsultaInfoSolicitud()
        {
            numsolicitudempresa = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
            if (string.IsNullOrEmpty(numsolicitudempresa))
            {
                Response.Write("<script language='JavaScript'>var r=alert('Ocurrió un problema al cargar la pagina, la URL ha cambiado.');if(r==true){window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';};</script>");
            }
            else
            {
                numsolicitudempresa = QuerySegura.DecryptQueryString(Request.QueryString["sid"]).Replace("\0", string.Empty).Trim();
            }
            try
            {
                var idusuario = Request.QueryString["sid1"].ToString();
            }
            catch (Exception)
            {
                Response.Write("<script language='JavaScript'>var r=alert('Ocurrió un problema al cargar la pagina, la URL ha cambiado.');if(r==true){window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';};</script>");
            }
            var tablixDocumentos = credenciales.GetDocumentosEmpresaXNumSolicitudCliente(numsolicitudempresa);
            tablePaginationDocumentos.DataSource = tablixDocumentos;
            tablePaginationDocumentos.DataBind();
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                if (chkRevisado.Checked)
                {
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    tcomentario.ToolTip = tcomentario.Text;
                }
                else
                {
                    FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                    fsupload.Enabled = false;
                }
            }
            var tablixSolicitudEmpresa = credenciales.GetSolicitudEmpresa(numsolicitudempresa);
            for (int i = 0; i <= tablixSolicitudEmpresa.Rows.Count - 1; i++)
            {
                txttipcli.Text = tablixSolicitudEmpresa.Rows[i][0].ToString();
                txtrazonsocial.Text = tablixSolicitudEmpresa.Rows[i][3].ToString();
                txtruccipas.Text = tablixSolicitudEmpresa.Rows[i][4].ToString();
                txtactividadcomercial.Text = tablixSolicitudEmpresa.Rows[i][5].ToString();
                txtdireccion.Text = tablixSolicitudEmpresa.Rows[i][6].ToString();
                txttelofi.Text = tablixSolicitudEmpresa.Rows[i][7].ToString();
                txtcontacto.Text = tablixSolicitudEmpresa.Rows[i][8].ToString();
                txttelcelcon.Text = tablixSolicitudEmpresa.Rows[i][9].ToString();
                txtmailinfocli.Text = tablixSolicitudEmpresa.Rows[i][10].ToString();
                txtcertificaciones.Text = tablixSolicitudEmpresa.Rows[i][11].ToString();
                turl.Text = tablixSolicitudEmpresa.Rows[i][12].ToString();
                txtafigremios.Text = tablixSolicitudEmpresa.Rows[i][13].ToString();
                txtrefcom.Text = tablixSolicitudEmpresa.Rows[i][14].ToString();
                txtreplegal.Text = tablixSolicitudEmpresa.Rows[i][15].ToString();
                txttelreplegal.Text = tablixSolicitudEmpresa.Rows[i][16].ToString();
                txtdirdomreplegal.Text = tablixSolicitudEmpresa.Rows[i][17].ToString();
                txtci.Text = tablixSolicitudEmpresa.Rows[i][18].ToString();
                tmailRepLegal.Text = tablixSolicitudEmpresa.Rows[i][19].ToString();
                txtmailebilling.Text = tablixSolicitudEmpresa.Rows[i][20].ToString();
                txtmotivorechazo.Text = tablixSolicitudEmpresa.Rows[i]["OBSERVACION"].ToString();
                txtmotivorechazo.ToolTip = txtmotivorechazo.Text;
            }
        }
        //protected void btnSalir_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //Response.Write("<script language='JavaScript'>window.close();</script>");
        //        this.Salir();
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
                numsolicitudempresa = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (string.IsNullOrEmpty(numsolicitudempresa))
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Ocurrió un problema al cargar la pagina, la URL ha cambiado.');if(r==true){window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';};</script>");
                }
                else
                {
                    numsolicitudempresa = QuerySegura.DecryptQueryString(Request.QueryString["sid"]).Replace("\0", string.Empty).Trim();
                }
                string idusuario = string.Empty;
                try
                {
                    idusuario = Request.QueryString["sid1"].ToString();
                }
                catch (Exception)
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Ocurrió un problema al cargar la pagina, la URL ha cambiado.');if(r==true){window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';};</script>");
                }
                New_ExportFileUpload();
                string mensaje = null;
                if (!credenciales.ReenviaSolicitudEmpresa(
                    idusuario,
                    numsolicitudempresa,
                    txtrazonsocial.Text.Trim(),
                    txtruccipas.Text.Trim(),
                    txtactividadcomercial.Text.Trim(),
                    txtdireccion.Text.Trim(),
                    txttelofi.Text.Trim(),
                    txtcontacto.Text.Trim(),
                    txttelcelcon.Text.Trim(),
                    txtmailinfocli.Text,
                    txtcertificaciones.Text.Trim(),
                    turl.Text.Trim(),
                    txtafigremios.Text.Trim(),
                    txtrefcom.Text.Trim(),
                    txtreplegal.Text.Trim(),
                    txttelreplegal.Text.Trim(),
                    txtdirdomreplegal.Text.Trim(),
                    txtci.Text.Trim(),
                    tmailRepLegal.Text,
                    txtmailebilling.Text,
                    xmlDocumentos,
                    Page.User.Identity.Name,
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud reenviada exitosamente.');if(r==true){window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}</script>");
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
            dtDocumentos.Columns.Add("IdDocEmp");
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                Label txtidsolicitud = item.FindControl("lblNumSolicitud") as Label;
                Label txtiddocemp = item.FindControl("lblSolDoc") as Label;
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
                    dtDocumentos.Rows.Add(Convert.ToInt64(txtiddocemp.Text), Convert.ToInt64(txtidsolicitud.Text), finalname);
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
                Label txtidsolicitud = item.FindControl("lblNumSolicitud") as Label; ;
                Label txtiddocemp = item.FindControl("lblSolDoc") as Label;
                if (fsupload.HasFile)
                {
                    string rutafile = Server.MapPath(fsupload.FileName);
                    string extensionfile = Path.GetExtension(fsupload.PostedFile.FileName);
                    string nombrearchivo = Path.GetFileNameWithoutExtension(fsupload.FileName);
                    string substring = ConfigurationManager.AppSettings["rutadocempcli"].ToString() + "\\";
                    rutafile = rutafile.Replace(substring, string.Empty);
                    string directorio = Path.GetDirectoryName(rutafile);
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
    }
}