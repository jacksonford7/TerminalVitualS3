using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClsNotasCreditos;


using System.Runtime.Serialization.Json;
using System.Web.Script.Services;
using CSLSite.N4Object;
using CSLSite.XmlTool;
using ConectorN4;
using Newtonsoft.Json;
using csl_log;
using System.Text;
using System.Data;
using System.Globalization;

using System.Web.Services;
using System.IO;
using System.Configuration;
using System.Collections;

namespace CSLSite
{
    public partial class archivo_sellos : System.Web.UI.Page
    {
        private usuario u;
        private DataTable dtDocumentos = new DataTable();

        private string xmlDocumentos
        {
            get
            {
                return (string)Session["pdfDocumentos"];
            }
            set
            {
                Session["pdfDocumentos"] = value;
            }

        }


        private void New_ExportFileUpload()
        {
            string crutaCompleta = string.Empty;

            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("Ruta");
            bool agrego = false;

            if (fsuploadarchivo.HasFile)
            {
                string rutafile = Server.MapPath(fsuploadarchivo.FileName);
                string finalname;
                crutaCompleta = fsuploadarchivo.PostedFile.FileName.ToString();
                var p = CSLSite.app_start.CredencialesHelper.UploadFile_CertificadoSellos(Server.MapPath(fsuploadarchivo.FileName), fsuploadarchivo.PostedFile.InputStream, out finalname);
                if (!p)
                {
                    sinresultado.Visible = true;
                    sinresultado.InnerText = string.Format("Error..{0}", finalname);
                    return;
                }
                else
                {
                    this.ruta_completa.Value = finalname;
                    agrego = true;
                    dtDocumentos.Rows.Add(finalname);
                }

            }

            
            if (agrego)
            {
                dtDocumentos.AcceptChanges();
                dtDocumentos.TableName = "Documentos";
                StringWriter sw = new StringWriter();
                dtDocumentos.WriteXml(sw);
                xmlDocumentos = sw.ToString();
   
                sinresultado.Visible = true;
                sinresultado.InnerText = string.Format("Archivo agregado exitosamente..{0}", crutaCompleta);

            }
            else
            {
                xmlDocumentos = null;    
            }

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                try
                {
                    if (Response.IsClientConnected)
                    {
                        xmlDocumentos = null;
                     
                        sinresultado.Visible = true;
                        this.ruta_completa.Value = string.Empty;
                       
                        this.nombre_archivo1.Value = string.Empty;
                        
                    }
                }
                catch (Exception ex)
                {
                  
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "archivo_sellos", "find_Click", "", u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }

            }

        }
        protected void find_Click(object sender, EventArgs e)
        {
            try
            {              
                if (Response.IsClientConnected)
                {
                    if (!this.fsuploadarchivo.HasFile)
                    {
                        sinresultado.Visible = true;
                        sinresultado.InnerText = string.Format("Por favor debe cargar por lo menos un archivo (.PDF)");
                        return;
                    }

                    if (this.fsuploadarchivo.HasFile)
                    {
                        var nombrefile = fsuploadarchivo.PostedFile.FileName;
                        if (!string.IsNullOrEmpty(nombrefile))
                        {
                            if (System.IO.Path.GetExtension(nombrefile).ToUpper() != ".PDF")
                            {
                                sinresultado.Visible = true;
                                sinresultado.InnerText = string.Format("La extensión del archivo # 1 debe ser PDF");
                            }
                        }

                        this.ruta_completa.Value = fsuploadarchivo.PostedFile.FileName;
                        this.nombre_archivo1.Value = fsuploadarchivo.PostedFile.FileName;

                        if (fsuploadarchivo.PostedFile.ContentLength > 2500000)
                        {

                            sinresultado.Visible = true;
                            sinresultado.InnerText = string.Format("El archivo # 1 pdf excede el tamaño límite 2.5 megabyte");

                            return;
                        }
                    }

                    



                    /*agrega archivo*/
                    this.New_ExportFileUpload();

 
                    sinresultado.Visible = true;


                }
            }
            catch (Exception ex)
            {
               
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la carga de archivo, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "archivo_sellos", "find_Click", "", u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
        }
    }
}