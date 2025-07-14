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
    public partial class lookup_subir_archivo : System.Web.UI.Page
    {
        private usuario u;
        private DataTable dtDocumentos = new DataTable();

        private string xmlDocumentos
        {
            get
            {
                return (string)Session["xmlDocumentos"];
            }
            set
            {
                Session["xmlDocumentos"] = value;
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
                var p = CSLSite.app_start.CredencialesHelper.UploadFile_NotaCredito(Server.MapPath(fsuploadarchivo.FileName), fsuploadarchivo.PostedFile.InputStream, out finalname);
                if (!p)
                {
                    this.Alerta(finalname);
                    return;
                }
                else
                {
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
   
                xfinder.Visible = true;
                sinresultado.Visible = true;

                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-info";
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
                        xfinder.Visible = false;
                        sinresultado.Visible = true;
                        this.ruta_completa.Value = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lookup_usuario_grupos", "find_Click", "", u != null ? u.loginname : "userNull"));
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
                        xfinder.Visible = true;
                        sinresultado.Visible = true;

                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        sinresultado.InnerText = string.Format("Por favor seleccione el archivo  (.PDF)");

                        return;
                    }

                    this.ruta_completa.Value = fsuploadarchivo.PostedFile.FileName;
                   
                    /*if (System.IO.Path.GetExtension(this.ruta_completa.Value).ToUpper() != ".PDF")
                    {
                        xfinder.Visible = true;
                        sinresultado.Visible = true;

                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        sinresultado.InnerText = string.Format("La extensión del archivo debe ser pdf");

                        return;
                    }*/

                    if (fsuploadarchivo.PostedFile.ContentLength > 10000000)
                    {
                        xfinder.Visible = true;
                        sinresultado.Visible = true;

                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        sinresultado.InnerText = string.Format("El archivo pdf excede el tamaño límite 10 megabyte");
   
                        return;
                    }

                    string cRuta = fsuploadarchivo.PostedFile.FileName.ToString();

                    /*agrega archivo*/
                    this.New_ExportFileUpload();

                    xfinder.Visible = false;
                    sinresultado.Visible = true;


                }
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la carga de archivo, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lookup_subir_archivos", "find_Click", "", u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
        }
    }
}