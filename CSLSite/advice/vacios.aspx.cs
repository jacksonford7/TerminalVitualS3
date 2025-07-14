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

namespace CSLSite.aisv
{
    public partial class vacios : System.Web.UI.Page
    {
        private string _Id_Opcion_Servicio = string.Empty;

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
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
           this.IsTokenAlive();
           this.IsAllowAccess();
           Page.Tracker();
           if (!IsPostBack)
           {
               this.IsCompatibleBrowser();
               Page.SslOn();
                //para que puedar dar click en el titulo de la pantalla y regresar al menu principal de la zona
                _Id_Opcion_Servicio = Request.QueryString["opcion"];
                this.opcion_principal.InnerHtml = string.Format("<a href=\"../cuenta/subopciones.aspx?opcion={0}\">{1}</a>", _Id_Opcion_Servicio, "Autorización de Ingreso y Salida de Vehículos");

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.sinresultado.Visible = IsPostBack;
            this.procesar.Value = "0";
            dataexport.Visible = IsPostBack;
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
             var  ux = new mailserviceSoapClient();
             var listado = new ArrayOfUnidad();
             var listabien = ViewState["preavisar"] as List<unidadAdvice>;
             var user = this.getUserBySesion();
             var bokingData = Newtonsoft.Json.JsonConvert.DeserializeObject<bkitem>(itemT4.Value);


             //la cultura del server
             CultureInfo enUS = new CultureInfo("en-US");
             DateTime fecha;
             //CutOff*
             if (DateTime.TryParseExact(bokingData.cutOff, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fecha))
             {
                 if (DateTime.Now > fecha)
                 {
                     this.sinresultado.Attributes["class"] = string.Empty;
                     this.sinresultado.InnerText = "El CuttOff de este Booking se ha cumplido, por favor reinte con otro, o llame a CGSA.";
                     this.sinresultado.Attributes["class"] = "msg-critico";
                     return;
                 }
             }
             else
             {
                 this.sinresultado.Attributes["class"] = string.Empty;
                 this.sinresultado.InnerText = "La fecha de CuttOff tiene un formato de fecha no admitido.";
                 this.sinresultado.Attributes["class"] = "msg-critico";
                 return;
             }

            if (user.loginname == null || user.loginname.Trim().Length <= 0)
             {
                 this.sinresultado.Attributes["class"] = string.Empty;
                 this.sinresultado.Attributes["class"] = "msg-critico";
                 this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A00-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El usuario era null!"), "vacios", "btbuscar_Click", string.Format("{0},{1}", bokingData.number, bokingData.referencia), user.loginname));
                 return;
             }
             if (listabien == null)
             {
                 this.sinresultado.Attributes["class"] = string.Empty;
                 this.sinresultado.Attributes["class"] = "msg-critico";
                 this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [A01-{0}] ", csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("La lista de unidades de ViewState era null"), "vacios", "btbuscar_Click", string.Format("{0},{1}", bokingData.number, bokingData.referencia), user.loginname));
                 return;
             }
             var token = HttpContext.Current.Request.Cookies["token"];
             //Validacion 3 -> Si su token existe
             if (token == null)
             {
                string pOpcion = HttpContext.Current.Request.QueryString["opcion"];
                var id = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "vacios", "btbuscar_Click", token.Value, user.loginname);
                 var personalms = string.Format("Su formulario ha expirado, será redireccionado a la página de menú, su id={0} expiró..", id);
                 this.PersonalResponse(personalms, "../cuenta/subopciones.aspx?opcion='" + pOpcion, true);
                 return;
             }

             var limite = 0;
             if (!string.IsNullOrEmpty(bokingData.dispone) && !int.TryParse(bokingData.dispone, out limite))
             {
                 limite = 20;
             }
             foreach (var item in listabien.Take(limite))
             {
                 var xxunit = new unidad();
                 xxunit.unidadID = item.id;
                 xxunit.bokingID = bokingData.number;
                 xxunit.itemID = bokingData.gkey;
                 xxunit.uniISO = bokingData.iso;
                 xxunit.lineaID = bokingData.linea;
                 xxunit.referencia = bokingData.referencia;
                 xxunit.imo = bokingData.imo;
                 xxunit.pod = bokingData.pod;
                 xxunit.pod1 = bokingData.pod1;
                 xxunit.shiper = bokingData.shiperID;
                 xxunit.reefer = !string.IsNullOrEmpty(bokingData.refer) && bokingData.refer.ToLower().Contains("true") ? "Y" : "N";
                 xxunit.temp = bokingData.temp;
                 xxunit.hume = bokingData.hume;
                 xxunit.vent = bokingData.vent_pc;
                 xxunit.ventU = bokingData.ventU;
                 xxunit.group = (!string.IsNullOrEmpty(bokingData.linea) ? (bokingData.linea == "MSC" && consolida.Checked ? "MSC_CONS" : string.Empty) : string.Empty);
                 listado.Add(xxunit);
             }
            //Si todo esta listo envío el trabajo al servicio de preavisos y el resto es de el...
            var mailes = string.Empty;
            if (textbox1.Value != null && textbox1.Value.Trim().Length > 0)
            {
                mailes = string.Format("{0};{1}", user.email, textbox1.Value);
            }
            else
            {
                mailes = user.email;
            }
            //  http://192.168.0.84:50501/al/mailservice.asmx

            //http://localhost:31248/mailservice.asmx

            string tipo = string.Empty;
            tipo = vacio.Checked ? "MTY" : "LCL";

            if (bokingData.linea == "MSC")
            {
                if (consolida.Checked)
                {
                    tipo = "FCL";
                }
            }

            //ux.preavisar(listado, user.loginname, mailes, token.Value, vacio.Checked ? "MTY" : "LCL");
            ux.preavisar(listado, user.loginname,  mailes, token.Value, tipo);
            Response.Redirect("~/cuenta/menu.aspx", false);
        }
        protected string getOklist(List<unidadAdvice> listado, ref List<unidadAdvice> listaok, int cuantos)
        {
            var t = new StringBuilder();
            var i = 0;
            t.Append("<p>Correctas </p><table id=\"tablasort\" cellpadding=\"1\" cellspacing=\"0\" class=\"table table-bordered table-sm table-contecon\"><thead><tr><th>Container</th></tr></thead><tbody>");
            foreach (var item in listado)
            {
                 if (item.status == "0")
                {
                    if (i < cuantos)
                    {
                        t.AppendFormat("<tr><td class=\"oki\">{0}</td></tr>", item.id);
                        listaok.Add(item);
                        i++;
                    }
                    else
                    {
                        item.status = "1";
                        item.data = "Sin disponibilidad de items";
                    }
                }
            }
            t.Append("</tbody></table>");//</div><div id=\"pageok\" >Registros<select class=\"pagesize\"><option selected=\"selected\" value=\"10\">10</option></select><img alt=\"\" src=\"../shared/imgs/first.gif\" class=\"first\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/prev.gif\" class=\"prev\"/>&nbsp;<input  type=\"text\" class=\"pagedisplay\" size=\"5px\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/next.gif\" class=\"next\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/last.gif\" class=\"last\"/></div>");
            return i > 0 ? t.ToString() : "<p>No se encontraron registros</p>";
        }
        protected string getBadlist(List<unidadAdvice> listado)
        {
            var t = new StringBuilder();
            var i = 0;
            t.Append("<p>Con Problemas </p><table id=\"tablasort1\" cellpadding=\"1\" cellspacing=\"0\" class=\"table table-bordered table-sm table-contecon\"><thead><tr><th>Container</th><th>Error</th></tr></thead><tbody>");
            foreach (var item in listado)
            {
                if (item.status == "1")
                {
                    t.AppendFormat(" <tr><td class= \"bad\">{0}</td><td>{1}</td></tr>",item.id,item.data);
                    i++;
                }
               
            }

            t.Append("</tbody></table>");//</div><div id=\"pagefail\" >Registros<select class=\"pagesize\"><option selected=\"selected\" value=\"10\">10</option></select><img alt=\"\" src=\"../shared/imgs/first.gif\" class=\"first\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/prev.gif\" class=\"prev\"/>&nbsp;<input  type=\"text\" class=\"pagedisplay\" size=\"5px\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/next.gif\" class=\"next\"/>&nbsp;<img alt=\"\" src=\"../shared/imgs/last.gif\" class=\"last\"/></div>");
            return i > 0 ? t.ToString() : "<p>No se encontraron registros</p>";
        }
        protected void btup_Click(object sender, EventArgs e)
        {
            var ss = string.Empty;
            if (Response.IsClientConnected)
            {
                //se obtiene el booking elegido
                var bokingData = Newtonsoft.Json.JsonConvert.DeserializeObject<bkitem>(itemT4.Value);
                this.sinresultado.InnerText = string.Empty;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-alerta";
                this.refnumber.InnerText = string.Format("{0}/{1}", bokingData.number, bokingData.referencia);


                if (bokingData.cutOff==null)
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.InnerText = "El CuttOff de este Booking se ha cumplido, por favor reintente con otro, o llame a CGSA.";
                    this.sinresultado.Attributes["class"] = "msg-critico";
                }


                //la cultura del server
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fecha;

                //CutOff*
                if (DateTime.TryParseExact(bokingData.cutOff, "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fecha))
                {
                    if (DateTime.Now > fecha)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.InnerText = "El CuttOff de este Booking se ha cumplido, por favor reinte con otro, o llame a CGSA.";
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        return;
                    }
                }
                else
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.InnerText = "La fecha de CuttOff tiene un formato de fecha no admitido.";
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    return;
                }
                if (bokingData.fkind == null)
                {
                    this.sinresultado.InnerText = "El FreightKind del booking no se ha establecido, comuníquese con CGSA.";
                    return;
                }
                if (vacio.Checked)
                {
                    if (!bokingData.fkind.ToLower().Contains("mty"))
                    {
                        this.sinresultado.InnerText = string.Format("El freightKind del booking [{0}] y la operación [Evacuar] elegidos son diferentes, favor revise",bokingData.fkind);
                        return;
                    }
                }
                else
                {
                    if (bokingData.linea.Equals("MSC"))
                    {
                        if (!bokingData.fkind.ToLower().Contains("fcl"))
                        {
                            this.sinresultado.InnerText = string.Format("El freightKind del booking [{0}] y la operación [Consolidación] elegidos son diferentes, favor revise", bokingData.fkind);
                            return;
                        }
                    }
                    else
                    {
                        if (!bokingData.fkind.ToLower().Contains("lcl"))
                        {
                            this.sinresultado.InnerText = string.Format("El freightKind del booking [{0}] y la operación [Consolidación] elegidos son diferentes, favor revise", bokingData.fkind);
                            return;
                        }
                    }
                    
                }
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

                    //nuevo normalizo el string.
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
                        //intento separado por comas
                        getList = str.Split(',').ToList<String>();
                    }
 
                    var xml = new StringBuilder();
                    xml.AppendLine("<CSL>");
                    if (getList.Count > 102)
                    {
                        this.sinresultado.InnerText = string.Format("La cantidad máxima de contenedores que puede preavisar es [100] por transacción, el archivo presenta: {0}, por favor corríjalo",getList.Count);
                        return;
                    }
                    foreach (var a in getList)
                    {
                        if (!string.IsNullOrEmpty(a))
                        {
                        //  bokingData.number = bokingData.number    
                            xml.AppendFormat("<VACIOS Contenedor=\"{0}\" Booking=\"{1}\" Booking_Item=\"{2}\" Iso_Item=\"{3}\" />", Server.HtmlEncode(a.Replace("?", string.Empty)), bokingData.number, bokingData.gkey, bokingData.iso);

                        }
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
                         xresultado = jAisvContainer.ValidatePreadvices(stry);
                    }
                    if (xresultado == null ||  xresultado.Count <= 0)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Hubo un problema durante la lectura del archivo y las unidades no pudieron ser procesadas, por favor revise que el nombre del contenedor no tenga espacios en blanco o caracteres especiales e intente en unos minutos, para ayudarnos por favor reporte este código de servicio [AP0-{0}] ", csl_log.log_csl.save_log<Exception>(new ApplicationException("La tabla de contenedores es VACIO"), "vacios", "btup_Click", xml.ToString(), "N4"));
                        return;
                    }
                    ss = xml.ToString();
                    Session["resultado"] = xresultado;
                    var listaBien = new List<unidadAdvice>();
                    int tdipo = 0;
                    if (!int.TryParse(bokingData.dispone, out tdipo))
                    {
                        tdipo = 100;
                    }
                    this.unit_ok.InnerHtml = this.getOklist(xresultado, ref listaBien,tdipo);
                    this.unit_error.InnerHtml = this.getBadlist(xresultado);
                    var ok = listaBien.Count;
                    ViewState["preavisar"] = null;
                    ViewState["preavisar"] = listaBien;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-info";
                    sinresultado.InnerHtml = string.Format("<span><strong>Contenedores para poner en preaviso:</strong> [{0}]</span>/<span><strong>Contenedores con errores:</strong> [{1}]</span>/<span><strong>Total de contenedores en archivo:</strong> [{2}]</span>", ok, xresultado.Count - ok, xresultado.Count);
                    this.procesar.Value = "1";
                    //this.btbuscar.Enabled = ok > 0;
                    if (ok > 0)
                    {
                        this.btbuscar.Attributes.Remove("disabled");

                    }
                    else
                    {
                        this.btbuscar.Attributes["disabled"] = "disabled";
                    }
                  
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