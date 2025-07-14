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
    public partial class solicitudcolaboradordocumentos : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private DataTable dtDocumentos = new DataTable();
        public DataTable dtDocumentosSolCol
        {
            get { return (DataTable)Session["dtDocumentosSolCol"]; }
            set { Session["dtDocumentosSolCol"] = value; }
        }
        private String xmlDocumentos;
        public String rucci
        {
            get { return (String)Session["ruccisolcoldoc"]; }
            set { Session["ruccisolcoldoc"] = value; }
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
                        var tablix3 = credenciales.GetDocumentosColaborador(Session["srucempresacol"].ToString(), Session["stipoempresacoloborador"].ToString());
                        tablix3.Columns.Add("RUTADOCUMENTO");
                        tablePaginationDocumentos.DataSource = tablix3;
                        tablePaginationDocumentos.DataBind();

                        //foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                        //{
                        //    var cab = item.FindControl("TO_HIDE");
                        //    cab.Visible = false;
                        //    var col = item.FindControl("COL_TO_HIDE");
                        //    col.Visible = false;
                        //}

                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "msg-info";

                        foreach (string var in Request.QueryString)
                        {
                            rucci = Request.QueryString[var];
                        }

                        DataTable dt = new DataTable();
                        dt = Session["dtDocumentosColaboradoresSolicitud"] as DataTable;
                        var results = from myRow in dt.AsEnumerable()
                                      where myRow.Field<string>("CIPas") == rucci
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

                        Session["dtDocumentosSolCol"] = new DataTable();
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
            dtDocumentos.Columns.Add("CIPas");
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
                    //if (File.Exists(rutafile))
                    //{
                    //    File.Delete(rutafile);
                    //}
                    //if (Directory.Exists(directorio))
                    //{
                    //    Directory.Delete(directorio);
                    //}
                    //System.IO.Directory.CreateDirectory(@directorio);
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
                    //if (File.Exists(rutafile))
                    //{
                    //    File.Delete(rutafile);
                    //}
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
            DataTable dtPadre = Session["dtDocumentosColaboradoresSolicitud"] as DataTable;
            DataTable dtHijo = new DataTable();
            dtHijo.Columns.Add("CIPas");
            dtHijo.Columns.Add("IdDocEmp");
            dtHijo.Columns.Add("IdSolicitud");
            dtHijo.Columns.Add("Ruta");
            dtHijo.Columns.Add("NombreArchivo");
            dtHijo.Columns.Add("Extension");
            for (int i = 0; i < dtDocumentos.Rows.Count; i++)
            {
                dtHijo.Rows.Add(dtDocumentos.Rows[i]["CIPas"], dtDocumentos.Rows[i]["IdDocEmp"], dtDocumentos.Rows[i]["IdSolicitud"], dtDocumentos.Rows[i]["Ruta"], dtDocumentos.Rows[i]["NombreArchivo"], dtDocumentos.Rows[i]["Extension"]);
            }
            if (dtPadre != null)
            {
                for (int i = 0; i < dtPadre.Rows.Count; i++)
                {
                    foreach (DataRow x in dtPadre.Rows)
                    {
                        if ((string)x[0] == rucci)
                        {
                            x.Delete();
                            i--;
                            break;
                        }
                    }
                }
                for (int i = 0; i < dtPadre.Rows.Count; i++)
                {
                    //var results = from myRow in dtPadre.AsEnumerable()
                    //              where myRow.Field<string>("Placa") == rucci
                    //              select new
                    //              {
                    //                  Placa = myRow.Field<string>("Placa")
                    //              };
                    //foreach (var item in results)
                    //{
                    //    if (item.Placa == rucci)
                    //    {
                            
                    //    }
                    //}
                    //if (dtPadre.Rows[i]["Placa"] == rucci)
                    //{
                    //    dtPadre.Rows.RemoveAt(i);
                    //}
                    //else
                    //{
                    dtHijo.Rows.Add(dtPadre.Rows[i]["CIPas"], dtPadre.Rows[i]["IdDocEmp"], dtPadre.Rows[i]["IdSolicitud"], dtPadre.Rows[i]["Ruta"], dtPadre.Rows[i]["NombreArchivo"], dtPadre.Rows[i]["Extension"]);
                    //}
                }
            }
            Session["dtDocumentosColaboradoresSolicitud"] = new DataTable();
            Session["dtDocumentosSolCol"] = new DataTable();
            //var resultshijo = from myRow in dtHijo.AsEnumerable()
            //                  where myRow.Field<string>("Placa") == rucci
            //                  select new
            //                  {
            //                    Placa = myRow.Field<string>("Placa")
            //                  };



            Session["dtDocumentosColaboradoresSolicitud"] = dtHijo;
            Session["dtDocumentosSolCol"] = dtHijo;
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
                    dtDocumentos.Rows.Add(rucci, itemlistaDoc.iddocemp, itemlistaDoc.idtipemp, path, name, extensionfile);
                }
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