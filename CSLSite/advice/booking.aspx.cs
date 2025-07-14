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
using BillionEntidades;
using System.Web.UI.HtmlControls;
using System.Data;

namespace CSLSite.aisv
{
    public partial class booking : System.Web.UI.Page
    {
        
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        #endregion
        //AntiXRCFG
        #region "Variables"
        private string cMensajes;
        private static Int64? lm = -3;
        private string OError;
        private string _Id_Opcion_Servicio = string.Empty;
        #endregion

        #region "Propiedades"
        private Int64? nSesion { get { return (Int64)Session["nSesion"]; } set { Session["nSesion"] = value; } }
        #endregion

        #region "Metodos"
        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();
        }

        private void Mostrar_Mensaje(string Mensaje)
        {
            this.sinresultado.Visible = true;
            this.sinresultado.InnerHtml = Mensaje;
        }

        private void Ocultar_Mensaje()
        {
            this.sinresultado.InnerText = string.Empty;
            this.sinresultado.Visible = false;
        }

        private System.Data.DataTable ConvertCSVToDataTable(List<string> lista, int vMaxColumn)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            dataTable.TableName = "booking";
            int numColumnas = -1;
            int numDato = -1;
            System.Data.DataRow row = null;

            //SE VALIDA QUE SI SE ELIGIO UNA NAVE DE LA LISTA EN PANTALLA
            List<string> vNaveSeleccionada = null;
            string Vsl_Name = string.Empty;
            string Voyage_IB = string.Empty;
            string Voyage_OB = string.Empty;

            try
            {
                if (cmbNave.SelectedValue != "0")
                {
                    vNaveSeleccionada = cmbNave.SelectedValue.Split('@').ToList<String>();
                    Vsl_Name = vNaveSeleccionada[0];
                    Voyage_OB = vNaveSeleccionada[1];
                    Voyage_IB = vNaveSeleccionada[2];
                }
            }
            catch { }
            foreach (var item in lista)
            {
                numColumnas = numColumnas + 1;
                if (numColumnas < vMaxColumn)
                {
                    dataTable.Columns.Add(item.ToLower().Trim());
                    continue;
                }

                if (numDato == (vMaxColumn - 1))
                {
                    numDato = -1;
                }

                if (numDato == -1)
                {
                    row = dataTable.NewRow();
                }

                if (numDato < (vMaxColumn - 1))
                {
                    numDato = numDato + 1;

                    //SE VALIDA QUE SI SE ELIGIO UNA NAVE DE LA LISTA EN PANTALLA
                    if (vNaveSeleccionada != null)
                    {
                        if (numDato >= 2 && numDato <= 4)
                        {
                            if (numDato == 2)
                            {
                                row[numDato] = Vsl_Name;
                            }
                            else if(numDato == 3)
                            {
                                row[numDato] = Voyage_IB;
                            }
                            else if (numDato == 4)
                            {
                                row[numDato] = Voyage_OB;
                            }
                        }
                        else
                        {
                            row[numDato] = item.Trim();
                        }
                    }
                    else
                    {
                        row[numDato] = item.Trim();
                    }
                }

                if (numDato == (vMaxColumn - 1))
                {
                    dataTable.Rows.Add(row);
                }

            }
            return dataTable;
        }

        private string ConvertDataTableToXML(System.Data.DataTable dataTable)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                dataTable.WriteXml(stringWriter, System.Data.XmlWriteMode.IgnoreSchema, false);
                return stringWriter.ToString();
            }
        }

        public static string ConvertXmlToJson(string xmlContent)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xmlContent);

            string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, true);

            return jsonText;
        }

        public System.Data.DataTable CreateDataTableFromXML(string xmlContent)
        {
            System.Data.DataTable dt = new System.Data.DataTable("booking");

            // Definir las columnas del DataTable según el esquema del XML
            dt.Columns.Add("numero", typeof(string));
            dt.Columns.Add("visita", typeof(string));
            dt.Columns.Add("estado", typeof(bool));
            dt.Columns.Add("resultado", typeof(string));
            dt.Columns.Add("secuencia", typeof(int));

            // Llenar el DataTable con el contenido del XML
            using (StringReader stringReader = new StringReader(xmlContent))
            {
                using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stringReader))
                {
                    System.Data.DataSet ds = new System.Data.DataSet();
                    ds.ReadXml(xmlReader);

                    // Verificar que exista la tabla "booking" en el DataSet
                    if (ds.Tables["booking"] != null)
                    {
                        dt = ds.Tables["booking"];
                    }
                }
            }

            return dt;
        }

        public void LlenaComboNave()
        {
            try
            {
                var oEntidad = EDI_bookingCab.ConsultarListaNaves();
                cmbNave.DataSource = oEntidad;
                cmbNave.DataValueField = "valor";
                cmbNave.DataTextField = "id";
                cmbNave.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboNave), "Booking.LlenaComboNave", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }
        #endregion

        #region "Forms"
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
                //                this.opcion_principal.InnerHtml = string.Format("<a href=\"../cuenta/subopciones.aspx?opcion={0}\">{1}</a>", _Id_Opcion_Servicio, "Autorización de Ingreso y Salida de Vehículos");

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    /*secuencial de sesion*/
                    ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();
                    LlenaComboNave();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";

                    cmbNave.Items.Add(item);
                    cmbNave.SelectedValue = "0";
                    this.btnProceasr.Attributes["disabled"] = "disabled";
                }
                this.sinresultado.Visible = IsPostBack;
                this.procesar.Value = "0";
                dataexport.Visible = IsPostBack;
                Ocultar_Mensaje();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        #endregion

        #region "Grill"
        protected void tablePagination_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                int totalBoxes = 0;
                totalBoxes = int.Parse(Session["Transaccion_booking_totalBoxes" + this.hf_BrowserWindowName.Value].ToString());

                // Asigna el total al Label en el FooterTemplate
                Label lblTotalBoxes = (Label)e.Item.FindControl("lblTotalBoxes");
                lblTotalBoxes.Text = totalBoxes.ToString();
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblEstado = e.Item.FindControl("lblEstado") as Label;
                Label lblresultado = e.Item.FindControl("lblResultado") as Label;
                CheckBox chkStatus = e.Item.FindControl("chkEstado") as CheckBox;

                string vEstado   = DataBinder.Eval(e.Item.DataItem, "estado").ToString();
                try
                {
                    lblresultado.Font.Bold = true;
                    lblresultado.ForeColor = System.Drawing.Color.Red;

                    if (vEstado == "0")
                    {
                        chkStatus.Checked = false;
                    }
                    else
                    {
                        chkStatus.Checked = true;
                    }
                }
                catch { }

                this.UPDETALLE.Update();
            }
        }
        #endregion

        #region "Controles"
        protected void btnProceasr_Click(object sender, EventArgs e)
        {
            Ocultar_Mensaje();
            string xml = string.Empty;
            usuario user;
            string vEmail;
            try
            {
                try
                {
                    var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    ClsUsuario = ClsUsuario_;
                    user = this.getUserBySesion();
                }
                catch
                {
                    Response.Redirect("../login.aspx", false);
                    return;
                }


                if (string.IsNullOrEmpty(txtEmail.Value))
                {
                    this.Alerta("ingrese email");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se debe ingresar el email"));
                    return;
                }

                if (txtEmail.Value != null && txtEmail.Value.Trim().Length > 0)
                {
                    vEmail = string.Format("{0};{1}", user.email, txtEmail.Value);
                }
                else
                {
                    vEmail = user.email;
                }

                DataTable dataTable = Session["Transaccion_booking_data" + this.hf_BrowserWindowName.Value] as DataTable;
                DataTable ItemSinErrores = Session["Transaccion_data_sinErrores" + this.hf_BrowserWindowName.Value] as DataTable;

                if (ItemSinErrores is null)
                {
                    this.Alerta("No se puede procesar, no se encontró registros validos");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se puede procesar, todos los registros tienen errores"));
                    return;
                }

                var token = HttpContext.Current.Request.Cookies["token"];
                if (token == null)
                {
                    string pOpcion = HttpContext.Current.Request.QueryString["opcion"];
                    var id = csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Token expirado"), "vacios", "btbuscar_Click", token.Value, user.loginname);
                    var personalms = string.Format("Su formulario ha expirado, será redireccionado a la página de menú, su id={0} expiró..", id);
                    this.PersonalResponse(personalms, "../cuenta/subopciones.aspx?opcion='" + pOpcion, true);
                    return;
                }

                string vCondicion = string.Empty;
                string vErrores = string.Empty;

                foreach (var itemsAprobado in ItemSinErrores.Select(" estado = 1 "))
                {
                    vCondicion = string.Empty;
                    vCondicion = string.Format(" number = '{0}'", itemsAprobado["numero"].ToString());
                    var item = dataTable.Select(vCondicion).FirstOrDefault();

                    EDI_bookingCab.Save_Masivo(item["number"].ToString(),
                                               item["liner"].ToString(),
                                               item["vsl_name"].ToString(),
                                               item["voyage_ib"].ToString(),
                                               item["voyage_ob"].ToString(),
                                               item["pod"].ToString(),
                                               item["pod1"].ToString(),
                                               item["opod1"].ToString(),
                                               item["fkind"].ToString(),
                                               item["shipper"].ToString(),
                                               item["stw"].ToString(),
                                               item["imo"].ToString(),
                                               string.IsNullOrEmpty(item["it_qty"].ToString())? "0" : item["it_qty"].ToString(),
                                               item["it_iso"].ToString(),
                                               item["it_comm"].ToString(),
                                               string.IsNullOrEmpty(item["it_temp"].ToString()) ? "0" : item["it_temp"].ToString(),
                                               string.IsNullOrEmpty(item["it_vent"].ToString()) ? "0" : item["it_vent"].ToString(),
                                               string.IsNullOrEmpty(item["it_co2"].ToString()) ? "0" : item["it_co2"].ToString(),
                                               string.IsNullOrEmpty(item["it_humm"].ToString()) ? "0" : item["it_humm"].ToString(),
                                               item["it_o2"].ToString(),
                                               itemsAprobado["visita"].ToString(),
                                               vEmail,
                                               string.IsNullOrEmpty(itemsAprobado["secuencia"].ToString()) ? "0" : itemsAprobado["secuencia"].ToString(),
                                               ClsUsuario.loginname,
                                               out OError
                                               );

                    if (!(string.IsNullOrEmpty(OError)))
                    {
                        vErrores = vErrores + " - " + OError;
                    }
                }

                if (vErrores != string.Empty)
                {
                    this.Alerta(vErrores);
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", vErrores));
                    return;
                }
                else
                {
                    this.Alerta("Transacción exitosa");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se registro con exito los booking"));
                    this.btnProceasr.Attributes["disabled"] = "disabled";
                    Session["Transaccion_booking_totalBoxes" + this.hf_BrowserWindowName.Value] = null;
                    Session["Transaccion_booking_data" + this.hf_BrowserWindowName.Value] = null;
                    Session["Transaccion_data_sinErrores" + this.hf_BrowserWindowName.Value] = null;
                }

            }
            catch (Exception x)
            {
                csl_log.log_csl.save_log<Exception>(x, "cancel", "btnProceasr_Click", xml, "sin usuario");
                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar la transacción, por favor el archivo... {0}</b>", x.Message));
                return;
            }
            
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            Ocultar_Mensaje();
            var ss = string.Empty;
            if (Response.IsClientConnected)
            {
                //######################################################
                //SE OBTIENE EL BOOKING ELEGIDO
                //######################################################
                var bokingData = Newtonsoft.Json.JsonConvert.DeserializeObject<bkitem>(itemT4.Value);

                if (fsupload.PostedFile == null)
                {
                    this.Alerta("La carga del archivo a fallado!");
                    Mostrar_Mensaje("La carga del archivo a fallado!");
                    return;
                }
                //############################################################
                //SUBIR EL ARCHIVO VALIDAR Y Q SEA CSV, SI EXISTE REEMPLAZARLO
                //############################################################
                var nombrefile = fsupload.PostedFile.FileName;
                if (string.IsNullOrEmpty(nombrefile))
                {
                    this.Alerta("Debe elegir un archivo CSV");
                    Mostrar_Mensaje("Debe elegir un archivo CSV");
                    return;
                }
                if (System.IO.Path.GetExtension(nombrefile).ToUpper() != ".CSV")
                {
                    this.Alerta("La extensión del archivo debe ser .CSV [Microsoft Excel/OpenOffice separado por comas]");
                    Mostrar_Mensaje("La extensión del archivo debe ser .CSV [Microsoft Excel/OpenOffice separado por comas]");
                    return;
                }
                if (fsupload.PostedFile.ContentLength > 1500000)
                {
                    this.Alerta("El tamaño del archivo excede el limite [2mb]");
                    Mostrar_Mensaje("El tamaño del archivo excede el limite [2mb]");
                    return;
                }

                int vMaxItem = 20;
                int vMaxColum = 20;
                try { vMaxItem = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MAX_ITEM_BK"].ToString()); } catch { vMaxItem = 20; }
                try { vMaxColum = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MAX_COLUMN_BK"].ToString()); } catch { vMaxColum = 20; }
                try
                {
                    //################################
                    //LEO TODA LA CADENA COMO STRING.
                    //################################
                    var str = new StreamReader(fsupload.PostedFile.InputStream).ReadToEnd();

                    //###################################################
                    // VALIDAR NUMERO DE LINEAS
                    // Detectar y dividir la cadena por saltos de línea
                    //###################################################

                    int numLineas = 0;
                    int index = -1;
                    string[] lines = str.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                    foreach (string line in lines)
                    {
                        index = index + 1;
                        if (!line.Equals("") && index > 0)
                        {
                            numLineas = numLineas + 1;
                        }
                    }

                    if (numLineas > vMaxItem)
                    {
                        this.Alerta(string.Format("La cantidad de items a procesar es mayor a lo permitido [{0}]", vMaxItem.ToString()));
                        Mostrar_Mensaje( string.Format("La cantidad de items a procesar es mayor a lo permitido [{0}]", vMaxItem.ToString()));
                        return;
                    }

                    //###################################################
                    //         DEPURACIÓN Y LIMPIEZA DE DATA
                    //###################################################
                    str.Replace(",", ";");
                    str = Regex.Replace(str, @"\r\n?|\n", ";");
                    str = Regex.Replace(str, @"\t", string.Empty);//str = Regex.Replace(str, @"\t|\s", string.Empty);
                                                                  //str = Regex.Replace(str, ";;", ";");
                    str = Regex.Replace(str, ",", ".");
                    str = Regex.Replace(str, @"'|!|<|>|/|""|#|&|", string.Empty);

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
                    str = Regex.Replace(str, @"'|!|<|>|/|""|#|", string.Empty);
                    List<string> getList = str.Split(';').ToList<String>();
                    if (getList.Count <= 1)
                    {
                        //intento separado por comas
                        getList = str.Split(',').ToList<String>();
                    }

                    var xml = new StringBuilder();
                    xml.AppendLine("<CSL>");

                    try
                    {
                        //######################################################
                        // LEER EL ARCHIVO CSV Y CONVERTIRLO A DATATABLE
                        //######################################################
                        System.Data.DataTable dataTable = ConvertCSVToDataTable(getList, vMaxColum);
                        Session["Transaccion_booking_data" + this.hf_BrowserWindowName.Value] = dataTable;

                        //######################################################
                        // CONVERTIR DATATABLE A XML
                        //######################################################
                        string xmlData = ConvertDataTableToXML(dataTable);
                        xmlData = xmlData.Replace("DocumentElement", "bookings");

                        string oError = string.Empty;
                        var resultadoValidacion = EDI_bookingCab.GetBookingValidations(xmlData, out oError);

                        if (oError != string.Empty)
                        {
                            this.Alerta(oError);
                            return;
                        }

                        var result = CreateDataTableFromXML(resultadoValidacion);

                        DataTable ItemSinErrores = null;

                        if (result.Select(" estado = 1 ").Count() > 0)
                        {
                            ItemSinErrores = result.Select(" estado = 1 ").CopyToDataTable();
                            Session["Transaccion_data_sinErrores" + this.hf_BrowserWindowName.Value] = ItemSinErrores;
                        }
                        else
                        {
                            Session["Transaccion_data_sinErrores" + this.hf_BrowserWindowName.Value] = null;
                        }

                        int totalBoxes = result.Select(" estado = 1 ").Count();
                        Session["Transaccion_booking_totalBoxes" + this.hf_BrowserWindowName.Value] = totalBoxes;

                        if (result.Rows.Count == 0)
                        {
                            sinResult.Visible = true;
                        }
                        else
                        {
                            sinResult.Visible = false;
                            tablePagination.DataSource = result;
                            tablePagination.DataBind();
                        }

                        if (result.Select(" estado = 1").Count() > 0)
                        {
                            this.btnProceasr.Attributes.Remove("disabled");

                        }
                        else
                        {
                            this.btnProceasr.Attributes["disabled"] = "disabled";
                        }

                        UPDETALLE.Update();
                    }
                    catch (Exception ex)
                    {
                        csl_log.log_csl.save_log<Exception>(ex, "cancel", "btnCargar_Click-exception", xml.ToString(), "sin usuario");
                        Mostrar_Mensaje(string.Format("El archivo .csv, esta incorrecto por favor revise que el número de cada contenedor no tenga caracteres inválidos o espacios."));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Mostrar_Mensaje(string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "vacios", "btnCargar_Click", ss, "N4")));
                    return;
                }
            }
        }
        #endregion

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

    }
}