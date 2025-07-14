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
using System.Web.Script.Services;
using System.Web.Services;
using MSCaptcha;
using System.Text;
using System.Drawing.Imaging;
using csl_log;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSLSite.cliente
{
    public partial class solicitudempresa : System.Web.UI.Page
    {

        protected static string ReCaptcha_Key = ConfigurationManager.AppSettings["ReCaptcha_Key"].ToString();
        protected static string ReCaptcha_Secret = ConfigurationManager.AppSettings["ReCaptcha_Secret"].ToString();

        private GetFile.Service getFile = new GetFile.Service();
        private DataTable dtDocumentos = new DataTable();
        private String xmlLogisticaExpo;
        private String xmlDatosExpo;
        private String xmlDocumentos;
        private String xmlDocumentos2;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //ViewStateUserKey = Session.SessionID;
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
            if (!Page.IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
                ConsultaInfoSolicitud();
                //HttpContext.Current.Session["deserializedProduct"] = false;
                //FillCapctha();
            }
        }
        void FillCapctha()
        {
            try
            {
                // Create a random code and store it in the Session object.
                Random random = new Random();
                string combination = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                StringBuilder captcha = new StringBuilder();
                for (int i = 0; i < 6; i++)
                    captcha.Append(combination[random.Next(combination.Length)]);
                this.Session["CaptchaImageText"] = captcha.ToString();
                imagencaptcha.Value = Session["CaptchaImageText"].ToString();
                //imgCaptcha.ImageUrl = "generatecaptcha.aspx?" + DateTime.Now.Ticks.ToString();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudempresa", "FillCapctha()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }
        private void ConsultaInfoSolicitud()
        {
            try
            {
                populateDrop(cbltipousuario, credenciales.getTipoClientEmpresa());
                if (cbltipousuario.Items.Count > 0)
                {
                    if (cbltipousuario.Items.FindByValue("0") != null)
                    {
                        cbltipousuario.Items.FindByValue("0").Selected = true;
                    }
                    cbltipousuario.SelectedValue = "0";
                }
                txtrazonsocial.Text = string.Empty;
                txtruccipas.Text = string.Empty;
                txtactividadcomercial.Text = string.Empty;
                txtdireccion.Text = string.Empty;
                txttelofi.Text = string.Empty;
                txtcontacto.Text = string.Empty;
                txttelcelcon.Text = string.Empty;
                tmailinfocli.Text = string.Empty;
                txtcertificaciones.Text = string.Empty;
                turl.Text = string.Empty;
                txtafigremios.Text = string.Empty;
                txtrefcom.Text = string.Empty;
                txtreplegal.Text = string.Empty;
                txttelreplegal.Text = string.Empty;
                txtdirdomreplegal.Text = string.Empty;
                txtci.Text = string.Empty;
                tmailRepLegal.Text = string.Empty;
                tablePaginationDocumentos.DataSource = null;
                tablePaginationDocumentos.DataBind();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudempresa", "ConsultaInfoSolicitud()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }
        private void populateDrop(CheckBoxList dp, HashSet<Tuple<string, string>> origen)
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

                //var valreCAPTCHA = Convert.ToBoolean(HttpContext.Current.Session["deserializedProduct"]);
                //if (!valreCAPTCHA)
                //{
                //    this.Alerta("* Validación de reCaptcha incorrecta, vuelva a intentar por favor.");
                //    return;
                //}
                //if (valreCAPTCHA)
                //{
                //    HttpContext.Current.Session["deserializedProduct"] = false;
                //    txtCaptcha.Text = "";
                //}

              
                if (credenciales.GetConsultaEmpresa(txtruccipas.Text.Trim()) == "1")
                {
                    this.Alerta("La siguiente Empresa ya se encuentra registrada: \\n " +
                                "RUC/CI/PAS: " + txtruccipas.Text.Trim() + " \\n " +
                                "Nombre/Razón Social: " + CslHelper.getShiperName(txtruccipas.Text.Trim()));
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    return;
                }
             
               New_ExportFileUpload();

                string mensaje = null;
                if (!credenciales.AddSolicitudEmpresa(
                    "",
                    Session["stringXmlEmpresa"].ToString(),//hftipocliente.Value,//stringXml,//cbltipousuario.SelectedValue,
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
                    //xmlDatosExpo,
                    //xmlLogisticaExpo,
                    //txtgps.Text,
                    xmlDocumentos,
                    Page.User.Identity.Name.ToUpper(),
                    out mensaje))
                {
                    this.Alerta(mensaje);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                }
                else
                {
                    
                    
                    //var usuario_sna = System.Configuration.ConfigurationManager.AppSettings["usuario_sna"];
                    //var clave_sna = System.Configuration.ConfigurationManager.AppSettings["clave_sna"];
                    //string Estado_Servicio = "0";
                    //string Problema_Servicio = string.Empty;

                    //if (this.rbpSiAcepto.Checked == true)
                    //{
                    //    string email = this.tmailinfocli.Text;
                    //    string celular = this.txttelcelcon.Text.Trim();

                       
                    //    string XMLSna = string.Format("<cliente><ruc>{0}</ruc><nombre>{1}</nombre><activar>{2}</activar><categoria>{3}</categoria><telefono>{4}</telefono><email>{5}</email><parametros/></cliente>",
                    //        txtruccipas.Text.Trim(), txtrazonsocial.Text.Trim().Replace("&", " "), 1, "IMPO", celular, email);

                    //    var WSCNA = new SNA.CRMService();
                    //    var Resultado = WSCNA.Invocar(usuario_sna.ToString(), clave_sna.ToString(), XMLSna.ToString());

                    //    if (Resultado != null)
                    //    {
                    //        string Res = Resultado.ToString();
                    //        var XMLResult = new XDocument();
                    //        try
                    //        {
                    //            XMLResult = XDocument.Parse(Res);
                    //        }
                    //        catch (Exception ex)
                    //        {
                               
                    //            var number = log_csl.save_log<Exception>(ex, "solicitudempresa", "btsalvar_Click()", DateTime.Now.ToShortDateString(), "sistemas");
                               
                    //            var error = "Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} " + number.ToString();
                    //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    //            return;
                    //        }

                    //        XElement XElemResult = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("resultado")).FirstOrDefault();
                    //        if (XElemResult != null)
                    //        {
                    //            XElement estado;
                    //            estado = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("estado")).FirstOrDefault();
                    //            Estado_Servicio = estado.Value;

                    //            XElement problema;
                    //            problema = XMLResult.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("problema")).FirstOrDefault();
                    //            Problema_Servicio = problema?.Value;

                    //            //levanta popup si no tiene el servicio
                    //            if (Estado_Servicio.Equals("1"))
                    //            {

                    //            }
                    //            else
                    //            {
                    //                this.Alerta(Problema_Servicio);
                    //                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                    //            }
                    //        }
                    //    }
                    //}
                    


                    //this.Alerta("Solicitud registrada exitosamente, en unos minutos recibirá una notificación via mail.");
                    //Response.Write("<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente, en unos minutos recibirá una notificación via mail.');if(r==true){window.location='../cuenta/menu.aspx'}else{window.location='../cuenta/menu.aspx'};</script>");
                    //ConsultaInfoSolicitud();
                    //CslHelper.JsonNewResponse(true, true, "window.location='../cuenta/menu.aspx'", "");
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud registrada exitosamente.');if(r==true){window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}</script>");
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudempresa", "btsalvar_Click()", DateTime.Now.ToShortDateString(), "sistemas");
                //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                var error = "Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0} " + number.ToString();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
                Response.Write("<script language='JavaScript'>var r=alert('" + error + "');if(r==true){window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}else{window.location='https://apps.cgsa.com.ec/Terminal/login.aspx';}</script>");
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
                TextBox txtidsolicitud = item.FindControl("txtidsolicitud") as TextBox;
                TextBox txtiddocemp = item.FindControl("txtiddocemp") as TextBox;
                if (fsupload.HasFile)
                {
                    string rutafile = Server.MapPath(fsupload.FileName);
                    string finalname;
                     var p = CSLSite.app_start.CredencialesHelper.UploadFile(Server.MapPath(fsupload.FileName), fsupload.PostedFile.InputStream,out finalname);
                    if (!p)
                    {
                        this.Alerta(finalname);
                        return;
                    }
                    dtDocumentos.Rows.Add( Convert.ToInt32(txtiddocemp.Text),0, finalname);
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
            //String savePath = @"C:\TemporalFileSystemCsaCGSA\";
            //System.IO.Directory.CreateDirectory(savePath);
            //listaDocumentos = new ArrayList();
            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("IdDocEmp");
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                FileUpload fsupload = item.FindControl("fsupload") as FileUpload;
                TextBox txtidsolicitud = item.FindControl("txtidsolicitud") as TextBox;
                TextBox txtiddocemp = item.FindControl("txtiddocemp") as TextBox;
                if (fsupload.HasFile)
                {
                    string rutafile = Server.MapPath(fsupload.FileName);
                    string extensionfile = Path.GetExtension(fsupload.PostedFile.FileName);
                    string directorio = Path.GetDirectoryName(rutafile);
                    string nombrearchivo = Path.GetFileNameWithoutExtension(fsupload.FileName);

                    //CREADO PARA EVITAR DUPLICIDAD//
                    string cleanname = new Guid().ToString();
                    string test_name = string.Format("{0}\\{1}{2}",directorio,cleanname,extensionfile);


                    fsupload.SaveAs(test_name);
                     extensionfile = Path.GetExtension(test_name);
                     directorio = Path.GetDirectoryName(test_name);
                     nombrearchivo = Path.GetFileNameWithoutExtension(test_name);

                    string[] files = Directory.GetFiles(directorio, "*" + nombrearchivo + extensionfile);
                    foreach (string s in files)
                    {
                        FileInfo fi = null;
                        try
                        {
                            fi = new FileInfo(s);
                            ExportFiles(fi.Directory.ToString(), fi.Name, "0"/*txtidsolicitud.Text*/, txtiddocemp.Text, nombrearchivo, extensionfile);
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
                    if (File.Exists(test_name))
                    {
                        File.Delete(test_name);
                    }
                    //fsupload.SaveAs(savePath += fsupload.FileName);
                    //savePath = @"C:\TemporalFileSystemCsaCGSA\";
                    //this.Alerta("Your file was saved as " + fileName);
                }
                else
                {
                    //this.Alerta("Seleccione los documentos requeridos.");
                    //fsupload.Focus();
                    //return;
                    //return;
                }
            }
            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            StringWriter sw = new StringWriter();
            dtDocumentos.WriteXml(sw);
            xmlDocumentos = sw.ToString();
            //System.IO.File.Delete(savePath);
        }
        private void ExportFiles(string path, string filename, string sidsolicitud, string siddocemp, string nombrearchivo, string extension)
        {
            path = path + "\\";
            if (File.Exists(path + filename))
            {
                FileStream fileStream;
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                //String rutaDataBase = ConfigurationManager.AppSettings["rutadatabase"].ToString() + dateServer;
                /*List<listaDoc> tlistaDocumentos = new List<listaDoc>(){
                        new listaDoc(){
                            idtipemp=Convert.ToInt32(sidsolicitud),
                            iddocemp=Convert.ToInt32(siddocemp),
                            rutafile=rutaServer+filename}
                        listaDocumentos.Add(tlistaDocumentos.ToArray());
                        };*/
                nombrearchivo = nombrearchivo + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssff") + extension;
                var llistaDoc= new List<listaDoc>();
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


        private void ValidaListaExpo()
        {
            for (int i = 0; i < cbltipousuario.Items.Count; i++)
            {
                if (cbltipousuario.Items[i].Selected)
                {
                    if (cbltipousuario.Items[i].Value == secexportador.Value)
                    {
                        //listaExportador = new ArrayList();
                        xmlDatosExpo = null;
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Mail");
                        dt.Columns.Add("Telf");
                        dt.Columns.Add("EjeCta");
                        dt.Columns.Add("Nombre");
                        //foreach (RepeaterItem item in tablePaginationDatosExp.Items)
                        //{
                        //    TextBox tmailExp = item.FindControl("tmail") as TextBox;
                        //    TextBox txttelexp = item.FindControl("txttelexp") as TextBox;
                        //    TextBox txtejecta = item.FindControl("txtejecta") as TextBox;
                        //    TextBox txtnombreexp = item.FindControl("txtnombreexp") as TextBox;
                        //    /*List<listaExpo> tlistaExpo = new List<listaExpo>(){
                        //        new listaExpo(){
                        //            tmail=tmailExp.Text,
                        //            txttelexp=txttelexp.Text,
                        //            txtejecta=txtejecta.Text,
                        //            txtnombreexp=txtnombreexp.Text}
                        //        };
                        //    listaExportador.Add(tlistaExpo.ToArray());*/
                        //    var llistaExpo = new List<listaExpo>();
                        //    var itlistaExpo = new listaExpo(tmailExp.Text, txttelexp.Text, txtejecta.Text, txtnombreexp.Text);
                        //    llistaExpo.Add(itlistaExpo);
                        //    foreach (var itemlistaExpo in llistaExpo)
                        //    {
                        //        dt.Rows.Add(itemlistaExpo.tmail, itemlistaExpo.txttelexp, itemlistaExpo.txtejecta, itemlistaExpo.txttelexp);
                        //    }
                        //}
                        xmlLogisticaExpo = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new System.Xml.Linq.XElement("SCA",
                        from p in tableexportador.Value.Replace("'", "").Split(',')
                        select new System.Xml.Linq.XElement("EXPORTADOR",
                        new System.Xml.Linq.XAttribute("VALOR", p)))).ToString();
                        dt.AcceptChanges();
                        dt.TableName = "DatosExpo";
                        StringWriter sw = new StringWriter();
                        dt.WriteXml(sw);
                        xmlDatosExpo = sw.ToString();
                    }
                }
            }
        }
        protected void btnbuscardoc_Click(object sender, EventArgs e)
        {
            try
            {
                //var valreCAPTCHA = Convert.ToBoolean(HttpContext.Current.Session["deserializedProduct"]);
                //if (valreCAPTCHA)
                //{
                //    HttpContext.Current.Session["deserializedProduct"] = false;
                //    txtCaptcha.Text = "";
                //}

                List<string> tipo = new List<string>();
                for (int i = 0; i < cbltipousuario.Items.Count; i++)
                {
                    if (cbltipousuario.Items[i].Selected)
                    {
                        tipo.Add(cbltipousuario.Items[i].Value);
                    }
                }
                if (tipo.Count == 0)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "ocultarsecexp();", true);
                    this.Alerta("Seleccione al menos un Tipo de Cliente.");
                    return;
                }
                XElement xmlElements = new XElement("EMP", tipo.Select(i => new XElement("COD", i)));
                //this.hftipocliente.Value = xmlElements.ToString();
                Session["stringXmlEmpresa"] = xmlElements.ToString();
                var tablix3 = credenciales.GetDocumentosEmpresa(xmlElements.ToString());
                tablePaginationDocumentos.DataSource = tablix3;
                tablePaginationDocumentos.DataBind();
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "mostrarsecexp();", true);
                btnbuscardoc.Focus();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "solicitudempresa", "btnbuscardoc_Click()", "1", Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
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
            public listaDoc(int idtipemp, int iddocemp, string rutafile) { this.idtipemp = idtipemp; this.iddocemp = iddocemp; this.rutafile = rutafile.Trim();}
        }
        private class JSonreCAPTCHA
        {
            public string success { get; set; }
            public string hostname { get; set; }
            public string challenge_ts { get; set; }
            public string error { get; set; }
        }
        [WebMethod]
        public static string IsCaptchaAvailable(string valorcaptchat, string valorcaja)
        {
            if (valorcaptchat == valorcaja)
            {
                return "1";
            }
            else
            {
                return "2";
            }
        }
        [WebMethod]
        public static string VerifyCaptcha(string response)
        {
            string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + ReCaptcha_Secret + "&response=" + response;
            var json = (new WebClient()).DownloadString(url);
            
            JSonreCAPTCHA deserializedProduct = JsonConvert.DeserializeObject<JSonreCAPTCHA>(json);
            HttpContext.Current.Session["deserializedProduct"] = deserializedProduct.success;
            
            return json;
        }

        //protected void BtnInformacion_Click(object sender, EventArgs e)
        //{
        //    //mpedit.Show();
        //}
    }
}