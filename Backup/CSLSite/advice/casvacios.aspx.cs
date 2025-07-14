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
using Newtonsoft.Json;
using System.Globalization;
using ConectorN4;

namespace CSLSite.aisv
{
    public partial class casvacios : System.Web.UI.Page
    {
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "vacios", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
           this.IsTokenAlive();
           this.IsAllowAccess();
           Page.Tracker();
           if (!IsPostBack)
           {
               this.IsCompatibleBrowser();
               Page.SslOn();
           }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.sinresultado.Visible = IsPostBack;
            this.procesar.Value = "0";
            dataexport.Visible = IsPostBack;

            var seu = this.getUserBySesion();
            var t = CslHelper.getShiperName(seu.ruc);
            refnumber.InnerText = t;
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            
             var listado = new ArrayOfUnidad();
             var listabien = ViewState["preavisar"] as List<unidadAdvice>;
             var user = this.getUserBySesion();
            if (user.loginname == null || user.loginname.Trim().Length <= 0)
             {
                 this.sinresultado.Attributes["class"] = string.Empty;
                 this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El usuario era null!"), "retornos", "btbuscar_Click","ERROR", user.loginname));
                 return;
             }
             if (listabien == null)
             {
                 this.sinresultado.Attributes["class"] = string.Empty;
                 this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A01-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La lista de unidades de ViewState era null"), "retornos", "btbuscar_Click", "ERROR", user.loginname));
                 return;
             }
             var token = HttpContext.Current.Request.Cookies["token"];
             //Validacion 3 -> Si su token existe
             if (token == null)
             {
                 var id = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "vacios", "btbuscar_Click", token.Value, user.loginname);
                 var personalms = string.Format("Su formulario ha expirado, será redireccionado a la página de menú, su id={0} expiró..", id);
                 this.PersonalResponse(personalms, "~/csl/menu", true);
                 return;
             }
            var mailes = string.Empty;
            if (textbox1.Value != null && textbox1.Value.Trim().Length > 0)
            {
                mailes = string.Format("{0};{1}", user.email, textbox1.Value);
            }
            else
            {
                mailes = user.email;
            }
             // AQUI SE DEBE PROCESAR LOS CONTENEDORES

                var n4user = new ObjectSesion();
                n4user.clase = "casvacios"; n4user.metodo = "btbuscar_Click";
                n4user.transaccion = "Retorno"; n4user.usuario = user.loginname;
                token = HttpContext.Current.Request.Cookies["token"];
                jAisvContainer.procesar_retorno(listabien, mailes, user.codigoempresa,n4user);
                Response.Redirect("~/csl/menu", false);
        }
        protected string getOklist(List<unidadAdvice> listado, ref List<unidadAdvice> listaok)
        {
            var t = new StringBuilder();
            var i = 0;
            t.Append("<p>Unidades que serán Marcadas <a href='#' class='topopup' onclick=\"SelectContent('divok');\"  >Seleccionar</a></p><div id='divok'><table id=\"tablaok\" cellpadding=\"1\" cellspacing=\"0\"><thead><tr><th>Container</th></tr></thead><tbody>");
            foreach (var item in listado)
            {
                if (item.status.Contains("0"))
                {
                    t.AppendFormat("<tr><td class=\"oki\">{0}</td></tr>", item.id);
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
            t.Append("<p>Unidades que presentan problemas <a href='#' class='topopup' onclick=\"SelectContent('divmal');\"  >Seleccionar</a></p><div id='divmal'><table id=\"tablamal\" cellpadding=\"1\" cellspacing=\"0\"><thead><tr><th>Container</th><th>Error</th></tr></thead><tbody>");
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
            string ss = string.Empty;
            if (Response.IsClientConnected)
            {
                //se obtiene el booking elegido
                this.sinresultado.InnerText = string.Empty;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-alerta";
                var user = this.getUserBySesion();
                if (fsupload.PostedFile == null)
                {
                    this.sinresultado.InnerText = "La carga del archivo a fallado!";
                    return;
                }
                //subir el archivo validar q sea csv, si existe reemplazarlo
                var nombrefile = fsupload.PostedFile.FileName;  
                if (System.IO.Path.GetExtension(nombrefile).ToUpper() != ".CSV")
                {
                    this.sinresultado.InnerText = "La extensión del archivo debe ser .CSV [Microsoft Excel/OpenOffice separado por comas]";
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
                   str = Regex.Replace(str, @"\t|\s", string.Empty);
                   str = Regex.Replace(str, ";;", ";");
                   str = Regex.Replace(str, @"'|!|<|>|-|_|""|#|", string.Empty);
                   try
                   {
                       byte[] bytes = Encoding.Default.GetBytes(str);
                       str = Encoding.UTF8.GetString(bytes);
                   }
                   catch 
                   {
                       str = Regex.Replace(str,Environment.NewLine,string.Empty);
                   }
                    //intento separado por saltos
                   str = Regex.Replace(str, @"'|!|<|>|-|_|""|#|", string.Empty);
                    List<string> getList = str.Split(';').ToList<String>();
                    if (getList.Count <= 1)
                    {
                            getList = str.Split(',').ToList<String>();
                    }
 
                    var xml = new StringBuilder();
                    xml.AppendLine("<cntrs>");
                    if (getList.Count > 100)
                    {
                        this.sinresultado.InnerText = string.Format("La cantidad máxima de contenedores que puede preavisar es [100] por transacción, el archivo presenta: {0}, por favor corríjalo",getList.Count);
                        return;
                    }
                    foreach (var a in getList)
                    {
                        if (!string.IsNullOrEmpty(a))
                        {
                            xml.AppendFormat("<cntr id=\"{0}\"  line=\"{1}\" />", a, user.codigoempresa);
                        }
                    }
                    xml.AppendLine("</cntrs>");

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
                        csl_log.log_csl.save_log<System.Xml.XmlException>(x, "cancel", "btup_Click", xml.ToString(), "sin usuario");
                        this.sinresultado.InnerText = string.Format("El archivo .csv, esta incorrecto por favor revise que el número de cada contenedor no tenga caracteres inválidos o espacios.");
                        return;
                    }
                    catch (Exception x)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        csl_log.log_csl.save_log<Exception>(x, "cancel", "btup_Click-exception", xml.ToString(), "sin usuario");
                        this.sinresultado.InnerText = string.Format("El archivo .csv, esta incorrecto por favor revise que el número de cada contenedor no tenga caracteres inválidos o espacios.");
                        return;
                    }
                    List<unidadAdvice> xresultado = null;
                    if (xml.Length > 8)
                    {
                        xresultado = jAisvContainer.ValidarRetornos(stry);
                    }
                    else
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Hubo un problema durante la lectura del archivo y el archivo parece estar vacío, por favor revise que el nombre del contenedor no tenga espacios en blanco o caracteres especiales e intente en unos minutos, para ayudarnos por favor reporte este código de servicio [AP0-{0}] ", csl_log.log_csl.save_log<Exception>(new ApplicationException("La lectura del archivo resulto en cadena menor que 8"), "cas_retorno", "btup_Click", xml.ToString(), "N4"));
                        return;
                     }

                    if (xresultado == null ||  xresultado.Count <= 0)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Hubo un problema durante la lectura del archivo y las unidades no pudieron ser procesadas, por favor revise que el nombre del contenedor no tenga espacios en blanco o caracteres especiales e intente en unos minutos, para ayudarnos por favor reporte este código de servicio [AP0-{0}] ", csl_log.log_csl.save_log<Exception>(new ApplicationException("La tabla de contenedores es VACIO"), "cas_retorno", "btup_Click", xml.ToString(), "N4"));
                        return;
                    }
                    
                    ss = xml.ToString();
                    Session["resultado"] = xresultado;
                    var listaBien = new List<unidadAdvice>();
                      
                    this.unit_ok.InnerHtml = this.getOklist(xresultado, ref listaBien);
                    this.unit_error.InnerHtml = this.getBadlist(xresultado);
                    var ok = listaBien.Count;
                    ViewState["preavisar"] = null;
                    ViewState["preavisar"] = listaBien;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-info";
                    sinresultado.InnerHtml = string.Format("<span><strong>Contenedores para retorno:</strong> [{0}]</span><br/><span><strong>Contenedores con problemas:</strong> [{1}]</span><br/><span><strong>Total de contenedores en archivo:</strong> [{2}]</span>", ok, xresultado.Count - ok, xresultado.Count);
                    this.procesar.Value = "1";
                    this.btbuscar.Enabled = ok > 0;
                }
                catch (Exception ex)
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "vacios", "btup_Click", ss, "N4"));
                    return;
                }
            }
        }
    }
}