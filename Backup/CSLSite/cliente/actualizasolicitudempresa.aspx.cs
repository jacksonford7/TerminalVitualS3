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

namespace CSLSite.cliente
{
    public partial class actualizasolicitudempresa : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private DataTable dtDocumentos = new DataTable();
        private String xmlLogisticaExpo;
        private String xmlDatosExpo;
        private String xmlDocumentos;
        private String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudclienteactualizasolicitudempresa"]; }
            set { Session["numsolicitudclienteactualizasolicitudempresa"] = value; }
        }
        private string rucempresa
        {
            get { return (string)Session["rucempresaclienteactualizasolicitudempresa"]; }
            set { Session["rucempresaclienteactualizasolicitudempresa"] = value; }
        }
        private string useremail
        {
            get { return (string)Session["useremailclienteactualizasolicitudempresa"]; }
            set { Session["useremailclienteactualizasolicitudempresa"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
            //this.IsAllowAccess();
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
                    Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
                }
            }
        }
        private void ConsultaInfoSolicitud()
        {
            foreach (string var in Request.QueryString)
            {
                numsolicitudempresa = QuerySegura.DecryptQueryString(Request.QueryString[var]);
                if (string.IsNullOrEmpty(numsolicitudempresa))
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Ocurrió un problema al cargar la pagina, la URL ha cambiado.');if(r==true){window.close()}else{window.close()};</script>");
                }
                else
                {
                    numsolicitudempresa = QuerySegura.DecryptQueryString(Request.QueryString[var]).Replace("\0", string.Empty).Trim();
                }
            }
            //if (!credenciales.GetSolicitudEstado(numsolicitudempresa))
            //{
            //    botonera.Visible = false;
            //    //factura.Visible = false;
            //    salir.Visible = true;
            //}
            var tablixDocumentos = credenciales.GetDocumentosEmpresaXNumSolicitud(numsolicitudempresa);
            tablePaginationDocumentos.DataSource = tablixDocumentos;
            tablePaginationDocumentos.DataBind();
            //xfinder.Visible = true;
            alerta.Attributes["class"] = string.Empty;
            alerta.Attributes["class"] = "msg-info";

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
                tmailinfocli.Text = tablixSolicitudEmpresa.Rows[i][10].ToString();
                txtcertificaciones.Text = tablixSolicitudEmpresa.Rows[i][11].ToString();
                turl.Text = tablixSolicitudEmpresa.Rows[i][12].ToString();
                txtafigremios.Text = tablixSolicitudEmpresa.Rows[i][13].ToString();
                txtrefcom.Text = tablixSolicitudEmpresa.Rows[i][14].ToString();
                txtreplegal.Text = tablixSolicitudEmpresa.Rows[i][15].ToString();
                txttelreplegal.Text = tablixSolicitudEmpresa.Rows[i][16].ToString();
                txtdirdomreplegal.Text = tablixSolicitudEmpresa.Rows[i][17].ToString();
                txtci.Text = tablixSolicitudEmpresa.Rows[i][18].ToString();
                tmailRepLegal.Text = tablixSolicitudEmpresa.Rows[i][19].ToString();
                tmailebilling.Text = tablixSolicitudEmpresa.Rows[i][20].ToString();
            }
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                //return;
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = QuerySegura.DecryptQueryString(Request.QueryString[var]);
                    if (string.IsNullOrEmpty(numsolicitudempresa))
                    {
                        Response.Write("<script language='JavaScript'>var r=alert('Ocurrió un problema al cargar la pagina, la URL ha cambiado.');if(r==true){window.close()}else{window.close()};</script>");
                    }
                    else
                    {
                        numsolicitudempresa = QuerySegura.DecryptQueryString(Request.QueryString[var]).Replace("\0", string.Empty).Trim();
                    }
                }
                ExportFileUpload();
                string mensaje = null;
                if (!credenciales.UpdateSolicitudEmpresa(
                   numsolicitudempresa,
                   txtrazonsocial.Text.Trim(),
                   txtruccipas.Text.Trim(),
                   txtactividadcomercial.Text.Trim(),
                   txtdireccion.Text.Trim(),
                   txttelofi.Text.Trim(),
                   txtcontacto.Text.Trim(),
                   txttelcelcon.Text.Trim(),
                   tmailinfocli.Text,
                   txtcertificaciones.Text.Trim(),
                   turl.Text.Trim(),
                   txtafigremios.Text.Trim(),
                   txtrefcom.Text.Trim(),
                   txtreplegal.Text.Trim(),
                   txttelreplegal.Text.Trim(),
                   txtdirdomreplegal.Text.Trim(),
                   txtci.Text.Trim(),
                   tmailRepLegal.Text,
                   tmailebilling.Text,
                   xmlDocumentos,
                   Page.User.Identity.Name,
                   out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud actualizada exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Write("<script language='JavaScript'>window.close();</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
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
                    string substring = ConfigurationManager.AppSettings["rutaactdatcli"].ToString() + "\\";
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