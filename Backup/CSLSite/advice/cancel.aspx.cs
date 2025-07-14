using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Text;
using System.IO;
using CSLSite.unitService;
using System.Text.RegularExpressions;

namespace CSLSite
{
    public partial class cancel : System.Web.UI.Page
    {
        //AntiXRCFG--CIFRADO EL VIEWSTATE
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
  
        }
        protected void Page_Init(object sender, EventArgs e)
        {

            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "cancel", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
            this.IsTokenAlive();
            this.IsAllowAccess();
            Page.Tracker();
           this.dataexport.Visible = IsPostBack;
           if (!IsPostBack)
           {
               this.IsCompatibleBrowser();
               Page.SslOn();
             
           }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lineaCI.Value = string.Empty;
            this.numexport.Value = string.Empty;
            this.sinresultado.Visible = IsPostBack;
            this.procesar.Value = "0";
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
             var  ux = new mailserviceSoapClient();
             var listado = new ArrayOfUnidad();
             var listabien = ViewState["preavisar"] as List<unidadAdvice>;
             var user = this.getUserBySesion();
             if (user.loginname == null || user.loginname.Trim().Length <= 0)
             {
                 this.sinresultado.Attributes["class"] = string.Empty;
                 this.sinresultado.Attributes["class"] = "msg-critico";
                 this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El usuario era null!"), "cancelar", "btbuscar_Click", string.Format("{0},{1}", boking.Value, nave.Value), ""));
                 return;
             }
             if (listabien == null)
             {
                 this.sinresultado.Attributes["class"] = string.Empty;
                 this.sinresultado.Attributes["class"] = "msg-critico";
                 this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A01-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La lista de unidades de ViewState era null"), "cancelar", "btbuscar_Click", string.Format("{0},{1}", boking.Value, nave.Value), ""));
                 return;
             }
             var token = HttpContext.Current.Request.Cookies["token"];
             if (token == null)
             {
                 var id = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "cancelar", "btbuscar_Click", token.Value, user.loginname);
                 var personalms = string.Format("Su formulario ha expirado, será redireccionado a la página de menú, su id={0} expiró..", id);
                 this.PersonalResponse(personalms, "~/csl/menu", true);
                 return;
             }
             foreach (var item in listabien.Take(100))
             {
                 var xxunit = new unidad();
                 xxunit.unidadID = item.id;
                 xxunit.bokingID = this.boking.Value;
                 xxunit.itemID = this.bkitem.Value;
                 xxunit.uniISO = this.unitiso.Value;
                 xxunit.lineaID = this.lineabok.Value;
                 xxunit.referencia = this.referencia.Value;
                 listado.Add(xxunit);
             }
            var mailes = string.Empty;
            if (textbox1.Value != null && textbox1.Value.Trim().Length > 0)
            {
                mailes = string.Format("{0};{1}",user.email,textbox1.Value);
            }
            else
            {
                mailes = user.email;
            }
             ux.cancelar(listado,user.loginname,mailes, token.Value);
             Response.Redirect("~/csl/menu", false);
        }
        protected string getOklist(List<unidadAdvice> listado, ref List<unidadAdvice> listaok)
        {
            var t = new StringBuilder();
            var i = 0;
            t.Append("<p>Unidades que serán canceladas <a href='#' class='topopup' onclick=\"SelectContent('divok');\"  >Seleccionar</a></p><div id='divok'><table id=\"tablaok\" cellpadding=\"1\" cellspacing=\"0\"><thead><tr><th>Container</th></tr></thead><tbody>");
            foreach (var item in listado)
            {
                if (item.status == "0")
                {
                    t.AppendFormat("<tr><td class=\"oki\">{0}</td></tr>",item.id);
                    listaok.Add(item);
                    i++;
                }
              
            }
            t.Append("</tbody></table></div><div id=\"pageok\" >Registros<select class=\"pagesize\"><option selected=\"selected\" value=\"10\">10</option></select><img alt=\"\" src=\"../shared/imgs/first.gif\" class=\"first\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/prev.gif\" class=\"prev\"/>&nbsp;<input  type=\"text\" class=\"pagedisplay\" size=\"5px\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/next.gif\" class=\"next\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/last.gif\" class=\"last\"/></div>");
            return i > 0 ? t.ToString() : "<p>No se encontraron registros</p>";
        }
        protected string getBadlist(List<unidadAdvice> listado)
        {
            var t = new StringBuilder();
            var i = 0;
            t.Append("<p>Unidades que presentan errores <a href='#' class='topopup' onclick=\"SelectContent('divmal');\"  >Seleccionar</a></p><div id='divmal'><table id=\"tablamal\" cellpadding=\"1\" cellspacing=\"0\"><thead><tr><th>Container</th><th>Error</th></tr></thead><tbody>");
            foreach (var item in listado)
            {
                if (item.status == "1")
                {
                    t.AppendFormat(" <tr><td class= \"bad\">{0}</td><td>{1}</td></tr>",item.id,item.data);
                    i++;
                }
               
            }
            t.Append("</tbody></table></div><div id=\"pagefail\" >Registros<select class=\"pagesize\"><option selected=\"selected\" value=\"10\">10</option></select><img alt=\"\" src=\"../shared/imgs/first.gif\" class=\"first\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/prev.gif\" class=\"prev\"/>&nbsp;<input  type=\"text\" class=\"pagedisplay\" size=\"5px\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/next.gif\" class=\"next\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/last.gif\" class=\"last\"/></div>");
            return i > 0 ? t.ToString() : "<p>No se encontraron registros</p>";
        }
        protected void btup_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                this.sinresultado.InnerText = string.Empty;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-alerta";
                this.refnumber.InnerText = nave.Value;
                if (fsupload.PostedFile == null)
                {
                    this.sinresultado.InnerText = "La carga del archivo a fallado!";
                    return;
                }
                //subir el archivo validar q sea csv, si existe reemplazarlo
                var nombrefile = fsupload.PostedFile.FileName;  
                if (System.IO.Path.GetExtension(nombrefile).ToUpper() != ".CSV")
                {
                    this.sinresultado.InnerText = "La extensión del archivo debe ser .CSV [Microsoft Excel separado por comas]";
                    return;
                }
                if (fsupload.PostedFile.ContentLength > 1500000)
                {
                    this.sinresultado.InnerText = "El tamaño del archivo excede el limite [2mb]";
                    return;
                }
                try
                {
                    //leo toda la cadena como string.
                    var str = new StreamReader(fsupload.PostedFile.InputStream).ReadToEnd();
                   str.Replace(",",";");
                   str=   Regex.Replace(str, @"\r\n?|\n", ";");
                   str = Regex.Replace(str, ";;", ";");
                   str = Regex.Replace(str, @"'|!|<|>|-|_|""|#|", string.Empty);
                   //nuevo normalizo el string.
                   try
                   {
                       byte[] bytes = Encoding.Default.GetBytes(str);
                       str = Encoding.UTF8.GetString(bytes);
                   }
                   catch
                   {
                       str = Regex.Replace(str, Environment.NewLine, string.Empty);
                   }
                   //intento separado por saltos
                   str = Regex.Replace(str, @"'|!|<|>|-|_|""|#|", string.Empty);

                    //intento separado por saltos
                    List<string> getList = str.Split(';').ToList<String>();
                    if (getList.Count <= 1)
                    {
                        //intento separado por comas
                        getList = str.Split(',').ToList<String>();
                    }
                    //armado de xml parametros
                    var xml = new StringBuilder();
                    xml.AppendLine("<CSL>");
                    if (getList.Count > 102)
                    {
                        this.sinresultado.InnerText = string.Format("La cantidad máxima de contenedores que puede cancelar es [100] por transacción, el archivo presenta: {0}, por favor corríjalo",getList.Count);
                        return;
                    }
                    var ux = this.getUserBySesion();
                    if (ux == null)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El usuario era null"), "cancelar", "btup_Click", string.Format("{0},{1}", boking.Value, nave.Value), ""));
                        return;
                    }
                    foreach (var a in getList)
                    {
                        if(!string.IsNullOrEmpty(a))
                        xml.AppendFormat("<VACIOS Contenedor=\"{0}\" Referencia=\"{1}\" Usuario=\"{2}\"/>",Server.HtmlEncode(a),this.referencia.Value, ux.loginname);
                    }
                    xml.AppendLine("</CSL>");
                    var stry = string.Empty;
                    try
                    {
                        System.Xml.XmlDocument xmi = new System.Xml.XmlDocument();
                        xmi.LoadXml(xml.ToString());
                        stry = xmi.OuterXml;
                    }
                    catch (System.Xml.XmlException x)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        csl_log.log_csl.save_log<System.Xml.XmlException>(x, "cancel", "btup_Click", xml.ToString(), ux.loginname != null ? ux.loginname : "sin usuario");
                        this.sinresultado.InnerText = string.Format("El archivo .csv, esta incorrecto por favor revise que el número de cada contenedor no tenga caracteres inválidos o espacios.");
                        return;
                    }
                    catch (Exception x)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("El archivo .csv, esta incorrecto por favor revise que el número de cada contenedor no tenga caracteres inválidos o espacios.");
                        csl_log.log_csl.save_log<Exception>(x, "cancel", "btup_Click-exception", xml.ToString(), ux.loginname != null ? ux.loginname : "sin usuario");
                        return;
                    }
                    List<unidadAdvice> xresultado = null;
                    if (xml.Length > 8)
                    {
                        xresultado = jAisvContainer.ValidateCancelAdvice(stry);
                    }
                    if (xresultado == null || xresultado.Count <= 0)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Hubo un problema durante la lectura y las unidades no pudieron ser procesadas, por favor repórtelo con el siguiente código [AP0-{0}] ", csl_log.log_csl.save_log<Exception>(new ApplicationException("timeOut de la base de datos"), "cancel", "btup_Click", string.Format("{0},{1}", boking.Value, nave.Value), ux.loginname));
                        return;
                    }
                    var listaBien = new List<unidadAdvice>();
                    this.unit_ok.InnerHtml = this.getOklist(xresultado, ref listaBien);
                    this.unit_error.InnerHtml = this.getBadlist(xresultado);
                    var ok = listaBien.Count;

                    ViewState["preavisar"] = listaBien;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-info";
                    sinresultado.InnerHtml = string.Format("<span><strong>Contenedores para cancelar su previso:</strong> [{0}]</span><br/><span><strong>Contenedores con errores:</strong> [{1}]</span><br/><span><strong>Total de contenedores en archivo:</strong> [{2}]</span>", ok, xresultado.Count - ok, xresultado.Count);
                    this.procesar.Value = "1";
                    this.btbuscar.Enabled = ok > 0;
                }
                catch (Exception ex)
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "cancelar", "btup_Click", string.Format("{0},{1}",boking.Value, nave.Value), ""));
                    return;
                }
            }
        }
    }
}