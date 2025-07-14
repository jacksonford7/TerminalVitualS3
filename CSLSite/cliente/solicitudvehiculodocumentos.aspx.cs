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

namespace CSLSite.cliente
{
    public partial class solicitudvehiculodocumentos : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private DataTable dtDocumentos = new DataTable();
        public DataTable dtDocumentosSolVeh
        {
            get { return (DataTable)Session["dtDocumentosVehiculosSolVeh"]; }
            set { Session["dtDocumentosVehiculosSolVeh"] = value; }
        }
        private String xmlDocumentos;
        public String splaca
        {
            get { return (String)Session["splacadocveh"]; }
            set { Session["splacadocveh"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
                try
                {
                    //if (false/*Session["tipodocsol"] == null*/)
                    //{
                    //    //this.Page.RegisterStartupScrip("invocar", "cerrarpagina();");
                    //    //Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "cerrarpagina();");
                    //    sinresultado.Visible = true;
                    //    return;
                    //}
                    //else 
                    if (true/*Session["tipodocsol"].ToString() != "0"*/)
                    {
                        //List<string> tipo = new List<string>();
                        //tipo.Add(Session["tipodocsol"].ToString());
                        //if (tipo.Count == 0)
                        //{
                        //    //ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "ocultarsecexp();", true);
                        //    //this.Alerta("Seleccione al menos un Tipo de Cliente.");
                        //    sinresultado.Visible = true;
                        //    return;
                        //}
                        //XElement xmlElements = new XElement("EMP", tipo.Select(i => new XElement("COD", i)));
                        var sid = Request.QueryString["SID"];
                        var tablix3 = credenciales.GetDocumentosVehiculo(Session["rucempresasolicitudvehiculo"].ToString(), Session["stipoempresavehiculo"].ToString(), Request.QueryString["CATEGORIA"].ToString());
                        tablix3.Columns.Add("RUTADOCUMENTO");
                        tablePaginationDocumentos.DataSource = tablix3;
                        tablePaginationDocumentos.DataBind();

                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "msg-info";

                        //foreach (string var in Request.QueryString)
                        //{
                        splaca = Request.QueryString["PLACA"];
                        //}
                        DataTable dt = new DataTable();
                        dt = Session["dtDocumentosVehiculosSolicitud"] as DataTable;
                        var results = from myRow in dt.AsEnumerable()
                                      where myRow.Field<string>("Placa") == splaca
                                      select myRow;
                        dt = new DataTable();
                        dt = results.AsDataView().ToTable();
                        foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                        {
                            FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                            if (dt.Rows.Count > 0)
                            {
                                var ruta = dt.Rows[item.ItemIndex]["NombreArchivo"].ToString() + dt.Rows[item.ItemIndex]["Extension"].ToString();
                                tablix3.Rows[item.ItemIndex]["RUTADOCUMENTO"] = ruta;
                                fsupload.Enabled = false;
                            }
                            else
                            {
                                fsupload.Enabled = true;
                                tablix3.Rows[item.ItemIndex]["RUTADOCUMENTO"] = "";
                            }
                        }
                        tablePaginationDocumentos.DataSource = tablix3;
                        tablePaginationDocumentos.DataBind();
                        Session["dtDocumentosVehiculosSolVeh"] = new DataTable();
                        //this.alerta.InnerHtml = info;
                        //return;
                    }
                    //else
                    //{
                    //    sinresultado.Visible = true;
                    //    return;
                    //}
                }
                catch (Exception ex)
                {
                    this.Alerta(ex.Message);
                }
            }
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ExportFileUpload();
            }
            catch (Exception ex)
            {
                this.Alerta(ex.Message);
            }
        }
        private void ExportFileUpload()
        {
            xmlDocumentos = null;
            string directorio = string.Empty;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("Placa");
            dtDocumentos.Columns.Add("IdDocEmp");
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");
            dtDocumentos.Columns.Add("NombreArchivo");
            dtDocumentos.Columns.Add("Extension");
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                TextBox txtidsolicitud = item.FindControl("txtidsolicitud") as TextBox;
                TextBox txtiddocemp = item.FindControl("txtiddocemp") as TextBox;
                if (fsupload.HasFile)
                {
                    string rutafile = Server.MapPath(fsupload.FileName);
                    string extensionfile = Path.GetExtension(fsupload.PostedFile.FileName);
                    directorio = Path.GetDirectoryName(rutafile);
                    string nombrearchivo = Path.GetFileNameWithoutExtension(fsupload.FileName);
                   /*
                    fsupload.SaveAs(rutafile);
                    string[] files = Directory.GetFiles(directorio, "*" + nombrearchivo + extensionfile);
                    foreach (string s in files)
                    {
                        FileInfo fi = null;
                        try
                        {
                            fi = new FileInfo(s);
                            ExportFiles(fi.Directory.ToString(), fi.Name, nombrearchivo, extensionfile, txtidsolicitud.Text, txtiddocemp.Text);
                        }
                        catch (FileNotFoundException ex)
                        {
                            this.Alerta(ex.Message);
                            return;
                        }
                    }
                    */
                    
                    string finalname;
                    var p = CSLSite.app_start.CredencialesHelper.UploadFile(Server.MapPath(fsupload.FileName), fsupload.PostedFile.InputStream, out finalname);
                    if (!p)
                    {
                        this.Alerta(finalname);
                        return;
                    }


                  
                    var llistaDoc = new List<listaDoc>();
                    var ilistaDoc = new listaDoc(Convert.ToInt32(txtidsolicitud.Text), Convert.ToInt32(txtiddocemp.Text), finalname);
                    llistaDoc.Add(ilistaDoc);
                    foreach (var itemlistaDoc in llistaDoc)
                    {

                        dtDocumentos.Rows.Add(splaca, itemlistaDoc.iddocemp, itemlistaDoc.idtipemp, rutafile, finalname, extensionfile);
                    }

                }
                else
                {
                    //this.Alerta("Seleccione los documentos requeridos.");
                    //return;
                }
            }
            //if (Directory.Exists(directorio))
            //{
            //    Directory.Delete(directorio);
            //}
            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            DataTable dtPadre = Session["dtDocumentosVehiculosSolicitud"] as DataTable;
            DataTable dtHijo = new DataTable();
            dtHijo = dtDocumentos.Copy().Clone();
            //for (int i = 0; i < dtDocumentos.Rows.Count; i++)
            //{
            //    dtHijo.Rows.Add(dtDocumentos.Rows[i]["Placa"], dtDocumentos.Rows[i]["IdDocEmp"], dtDocumentos.Rows[i]["IdSolicitud"], dtDocumentos.Rows[i]["Ruta"], dtDocumentos.Rows[i]["NombreArchivo"], dtDocumentos.Rows[i]["Extension"]);
            //}
            foreach (DataRow drh in dtDocumentos.Rows)
            {
                dtHijo.Rows.Add(drh.ItemArray);
            }
            if (dtPadre != null)
            {
                if (dtPadre.Rows.Count > 0)
                {
                    //for (int i = 0; i < dtPadre.Rows.Count; i++)
                    //{
                    //    foreach (DataRow x in dtPadre.Rows)
                    //    {
                    //        if ((string)x[0] == splaca)
                    //        {
                    //            x.Delete();
                    //            break;
                    //        }
                    //    }
                    //}
                    DataRow[] rows;
                    rows = dtPadre.Select("Placa='" + splaca + "'");
                    foreach (DataRow r in rows)
                        r.Delete();
                    //for (int i = 0; i < dtPadre.Rows.Count; i++)
                    //{
                    //    dtHijo.Rows.Add(dtPadre.Rows[i]["Placa"], dtPadre.Rows[i]["IdDocEmp"], dtPadre.Rows[i]["IdSolicitud"], dtPadre.Rows[i]["Ruta"], dtPadre.Rows[i]["NombreArchivo"], dtPadre.Rows[i]["Extension"]);
                    //}
                    foreach (DataRow drp in dtPadre.Rows)
                    {
                        dtHijo.Rows.Add(drp.ItemArray);
                    }
                }
            }
            Session["dtDocumentosVehiculosSolicitud"] = new DataTable();
            Session["dtDocumentosVehiculosSolVeh"] = new DataTable();
            Session["dtDocumentosVehiculosSolicitud"] = dtHijo;
            Session["dtDocumentosVehiculosSolVeh"] = dtHijo;
            //DataTable datos = Session["dtDocumentosVehiculosSolicitud"] as DataTable;
            //StringWriter sw = new StringWriter();
            //dtDocumentos.WriteXml(sw);
            //xmlDocumentos = sw.ToString();
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "cerrarventana();", true);
            //this.Salir(false);
            Response.Write("<script language='JavaScript'>window.close();</script>");
        }
        private void ExportFiles(string path, string filename, string name, string extensionfile, string sidsolicitud, string siddocemp)
        {
            path = path + "\\";
            if (File.Exists(path + filename))
            {
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                var llistaDoc = new List<listaDoc>();
                var ilistaDoc = new listaDoc(Convert.ToInt32(sidsolicitud), Convert.ToInt32(siddocemp), rutaServer + filename);
                llistaDoc.Add(ilistaDoc);
                foreach (var itemlistaDoc in llistaDoc)
                {
                    var rutafile = path + filename /*+ '_' + DateTime.Now.ToString("dd-MM-yyyy")*/;
                    dtDocumentos.Rows.Add(splaca, itemlistaDoc.iddocemp, itemlistaDoc.idtipemp, path, name, extensionfile);
                }
            }
        }
        public class listaDoc
        {
            public int idtipemp { get; set; }
            public int iddocemp { get; set; }
            public string rutafile { get; set; }
            public listaDoc(int idtipemp, int iddocemp, string rutafile) { this.idtipemp = idtipemp; this.iddocemp = iddocemp; this.rutafile = rutafile.Trim(); }
        }
    }
}