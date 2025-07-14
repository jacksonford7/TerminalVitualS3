using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Web.Script.Services;
using ConectorN4;
using Logger;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Text.RegularExpressions;

namespace CSLSite
{
    public partial class solicitudUsuariosCerrojoElectronico : System.Web.UI.Page
    {
        public string contenedoresFileName
        {
            get { return (string)Session["contenedoresFileName_vacios"]; }
            set { Session["contenedoresFileName_vacios"] = value; }
        }

        public string contenedoresFilePath
        {
            get { return (string)Session["contenedoresFilePath_vacios"]; }
            set { Session["contenedoresFilePath_vacios"] = value; }
        }

        public string Grid
        {
            get { return (string)Session["Grid_vacios"]; }
            set { Session["Grid_vacios"] = value; }
        }

        public List<contenedoresCerrojoElectronico> GridContenedores
        {
            get { return (List<contenedoresCerrojoElectronico>)Session["GridContenedores"]; }
            set { Session["GridContenedores"] = value; }
        }

        string estadoIngresado = "IN";
        string tipoGrupo = ConfigurationManager.AppSettings["CerrojoElectronico"];

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
                //this.agencia.Value = user.ruc;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                alerta.Visible = false;
                sinresultado.Visible = false;
                populateDrop(dptiposervicios, CslHelperServicios.getServicios());
                if (dptiposervicios.Items.Count > 0)
                {
                    if (dptiposervicios.Items.FindByValue("000") != null)
                    {
                        dptiposervicios.Items.FindByValue("000").Selected = true;
                    }
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("RES"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("SEL"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("LA"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("BAS"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("IMO"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("RTR"));
                    dptiposervicios.Items.Remove(dptiposervicios.Items.FindByValue("CIE"));

                    dptiposervicios.SelectedValue = "CRJ";
                    dptiposervicios.Enabled = false;
                }
            }
        }

        private void populateDrop(DropDownList dp, HashSet<Tuple<string, string>> origen)
        {
            dp.DataSource = origen;
            dp.DataValueField = "item1";
            dp.DataTextField = "item2";
            dp.DataBind();
        }

        protected void btnSubirArchivoChoferes_Click(object sender, EventArgs e)
        {
            //obtengo la sesión del usuario logeado
            this.sinresultado.InnerText = string.Empty;
            sinresultado.Visible = false;
            var str = string.Empty;

            usuario sUser = null;
            sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

            if (DateTime.Now.Hour > int.Parse(ConfigurationManager.AppSettings["tiempoMaximoSubida"])){
                tablePagination.DataSource = null;
                tablePagination.DataBind();
                string mensajeErrorEx = "No puede subir documentos pasadas las " + ConfigurationManager.AppSettings["tiempoMaximoSubida"];
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la subida del excel, no puede subir documentos pasadas las " + ConfigurationManager.AppSettings["tiempoMaximoSubida"] + " , por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(new Exception(mensajeErrorEx), "cerrojo_electronico", "cargarExcel", "", sUser.loginname));
                sinresultado.Visible = true;
                return;
            }


            if (fuContenedores.PostedFile.ContentLength <=0)
            {
                this.sinresultado.InnerText = "Seleccione el  archivo csv antes de proceder";
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.Visible = true;
                return;
            }
            //subir el archivo validar q sea csv, si existe reemplazarlo
            var nombrefile = fuContenedores.PostedFile.FileName;
            if (System.IO.Path.GetExtension(nombrefile).ToUpper() != ".CSV")
            {
                this.sinresultado.InnerText = "La extensión del archivo debe ser .CSV [Microsoft Excel/OpenOffice separado por comas]";
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.Visible = true;
                return;
            }
            if (fuContenedores.PostedFile.ContentLength > 1500000)
            {
                this.sinresultado.InnerText = "El tamaño del archivo excede el limite [2mb]";
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.Visible = true;
                return;
            }

            try
            {
                //leo toda la cadena como string.
                str = new StreamReader(fuContenedores.PostedFile.InputStream).ReadToEnd();
                str.Replace(",", ";");
                str = Regex.Replace(str, @"\r\n?|\n", ";");
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
                    str = Regex.Replace(str, Environment.NewLine, string.Empty);
                }
                //intento separado por saltos
                str = Regex.Replace(str, @"'|!|<|>|-|_|""|#|", string.Empty);
                List<string> getList = str.Split(';').ToList<String>();
                if (getList.Count <= 1)
                {
                    //intento separado por comas
                    getList = str.Split(',').ToList<String>();
                }
                if (getList.Count > 100)
                {
                    this.sinresultado.InnerText = string.Format("La cantidad máxima de contenedores que puede asociar es [100] por transacción, el archivo presenta: {0}, por favor corríjalo", getList.Count);
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.Visible = true;
                    return;
                }
                //nuevo solo leer el CSV
                Import_To_Grid(sUser, getList);
                Grid = tablePagination.ID.ToString();
            }
            catch (Exception ex)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante el procesamiento, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "vacios", "btup_Click", str, "N4"));
                sinresultado.Visible = true;
                return;
            }

        }
        private void Import_To_Grid(string FilePath, string Extension, string isHDR, usuario sUser)
        {   
            try
            {
                //string xml = Ge_Xml_NumDoc("029092002015CI000498P", "CXRU1579530", "CMA2015071", "123456");
                //text_cont.Value = "";

                string conStr = "";
                switch (Extension)
                {
                    case ".xls": //Excel 97-03
                        conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07
                        conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }
                conStr = String.Format(conStr, FilePath, isHDR);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                DataTable dt = new DataTable();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();

                //Read Data from First Sheet
                connExcel.Open();
                //cmdExcel.CommandText = "select distinct * from [" + SheetName + "] sn where sn.F1 is not null and sn.F2 is not null and sn.F3 is not null and sn.F4 is not null";
                cmdExcel.CommandText = "select distinct sn.F2 from [" + SheetName + "] sn where sn.F1 is not null and sn.F2 is not null";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();
                List <contenedoresCerrojoElectronico> tabla = CslHelperServicios.consultarGrupoExcel(dt);
                this.tablePagination.DataSource = tabla;                
                this.tablePagination.DataBind();
                GridContenedores = tabla;
                xfinder.Visible = true;
                sinresultado.Visible = false;
                return;                
                
            }
            catch (Exception ex)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la subida, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "cerrojo_electronico", "cargarExcel", "", sUser.loginname));
                sinresultado.Visible = true;
                return;
            }
        }
        private void Import_To_Grid( usuario sUser , List<string> unidades)
        {
            try
            {

                DataTable dt = new DataTable();
                dt.TableName = "tablaContenedores";
                dt.Columns.Add("descripcionContenedor", typeof(string));
                //popular el datatable
                foreach (var c in unidades.Distinct())
                {
                    if (!string.IsNullOrEmpty(c))
                    {
                        var dr = dt.NewRow();
                        dr["descripcionContenedor"] = c.Trim();
                         dt.Rows.Add(dr);
                    }
                }
                List<contenedoresCerrojoElectronico> tabla = CslHelperServicios.consultarGrupoExcel(dt);
                //nuevo bloqueso por otros servicios
                var cfgs = dbconfig.GetActiveConfig(null, null, "grupos");
                var cfg = cfgs.FirstOrDefault();
                string[] lsg = null;
                if (cfg != null)
                {
                    lsg = cfg.config_value.Split(',');
                }
               if (lsg != null && lsg.Length > 0)
               {
                   foreach (var tb in tabla)
                   {
                       if (!string.IsNullOrEmpty(tb.grupo))
                       {
                           if (lsg.Where(c => c.Contains(tb.grupo)).Count() > 0)
                           {
                               tb.observacion = "Este contenedor tiene un servicio activo.";
                           }
                       }
                   }
               }
                //fin de la nueva opcion para bloquear si tiene servicios
               
                
                this.tablePagination.DataSource = tabla;
                this.tablePagination.DataBind();
                GridContenedores = tabla;
                xfinder.Visible = true;
                sinresultado.Visible = false;
                return;

            }
            catch (Exception ex)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la subida, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "cerrojo_electronico", "Import_To_Grid_cg", "", sUser.loginname));
                sinresultado.Visible = true;
                return;
            }
        }
        protected void btgenerarServer_Click(object sender, EventArgs e)
        {
            int retorno = 0;
            string mensajeErrorPorServicio = "Su contenedor no ha arribado en la Terminal.";

            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }

                    string TipoServicio = dptiposervicios.SelectedValue;
                    string descripcionServicio = dptiposervicios.Items.FindByValue(dptiposervicios.SelectedValue).Text;
                    string mensajeErrorN4 = string.Empty;
                    string mensajeContenedor = "";
                    string evento = CslHelperServicios.consultaEventoPorServicio(dptiposervicios.SelectedValue.Trim());
                    List<datosCabecera> datosCab = new List<datosCabecera>();

                    //obtengo la sesión del usuario logeado
                    usuario sUser = null;
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //Validación 1 -> valida cada contenedor contra N4
                    //foreach (RepeaterItem item in tablePagination.Items)
                    foreach (var item in GridContenedores)
                    {
                        string lblNombreContenedor = item.descripcion.Trim();
                        string lblObservacion = item.observacion.Trim();

                        if (string.IsNullOrEmpty(lblObservacion))
                        {
                            /*Utility.mostrarMensaje(this.Page, "Verifique que el excel que subió no tenga ninguna observación.");
                            return;*/
                        /*}
                        else {*/
                            var tk = HttpContext.Current.Request.Cookies["token"];
                            ConectorN4.ObjectSesion sesObj = new ObjectSesion();
                            sesObj.clase = "solicitudUsuariosCerrojoElectronico";
                            sesObj.metodo = "btgenerar_Click";
                            sesObj.transaccion = "generarSolicitudCerrojoElectronico";
                            sesObj.usuario = sUser.loginname;
                            sesObj.token = tk.Value;

                            
                            string a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"></unit-identity></units><properties><property tag=\"RoutingGroupRef\" value=\"{1}\"/></properties></icu>", lblNombreContenedor, dptiposervicios.SelectedValue.Trim());
                            
                            
                           
                             
                            //No agregar grupo a contenedores
                            //mensajeErrorN4 = Utility.validacionN4(sesObj, a);

                            mensajeErrorN4 = string.Empty;

                            //mensajeErrorN4 = validacionN4(lblNombreContenedor.Text.Trim(), dptiposervicios.SelectedValue.Trim());
                            if (!string.IsNullOrEmpty(mensajeErrorN4))
                            {
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = string.Format(mensajeErrorN4 + "Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(new Exception(mensajeErrorN4), "solicitudUsuariosCerrojoElectronico", "btgenerar", lblNombreContenedor, sUser.loginname));
                                sinresultado.Visible = true;
                                return;
                            }
                            
                            mensajeContenedor = invocacionEvento(lblNombreContenedor, evento);
                            if (!string.IsNullOrEmpty(mensajeContenedor))
                            {
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";
                                this.sinresultado.InnerText = string.Format(mensajeContenedor + "Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(new Exception(mensajeContenedor), "solicitudUsuariosCerrojoElectronico", "btgenerar", lblNombreContenedor, sUser.loginname));
                                sinresultado.Visible = true;
                                return;
                            }
                        }                        
                    }

                    //Validación 2 -> Si no hubo problema guardo la cabecera de la solicitud de reestiba                    
                    try
                    {
                        datosCab = CslHelperServicios.cabeceraSolicitud(0, TipoServicio, null, null, null, null, sUser.loginname, estadoIngresado, sUser.grupo.ToString());
                    }
                    catch (Exception ex)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = string.Format("Se produjo un error durante la generación de la solicitud, por favor repórtelo con el siguiente código [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "solicitud", "solicitud_cabecera", retorno.ToString().Trim(), sUser.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    //foreach (RepeaterItem item in tablePagination.Items)
                    foreach (var item in GridContenedores)
                    {
                        string lblNombreContenedor = item.descripcion.Trim();
                        string lblObservacion = item.observacion.Trim();
                        string lblIDContenedor = item.idCodigoContenedor.Trim();
                    
                        /*Label lblIDContenedor = item.FindControl("lblIDContenedor") as Label;
                        Label lblNombreContenedor = item.FindControl("lblNombreContenedor") as Label;
                        Label lblObservacion = item.FindControl("lblObservacion") as Label;*/

                        if (string.IsNullOrEmpty(lblObservacion))
                        {
                            //Validación 3 --> Se guarda los detalles con el id de la solicitud generada.
                            int retornoD = CslHelperServicios.detalleSolicitud(int.Parse(datosCab[0].idSolicitud.Trim()), int.Parse(lblIDContenedor), sUser.loginname);
                        }
                    }

                    //Validación 4 --> Se envía un correo al usuario. (Se guarda en las tablas)
                    envioCorreo(sUser, int.Parse(datosCab[0].idSolicitud.Trim()), descripcionServicio, datosCab[0].codigoSolicitud.Trim());
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format(mensajeErrorPorServicio + " " +"Ha ocurrido un problema durante la generación de la solicitud, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btgenerar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
        public void envioCorreo(usuario sUser, int idSolicitud, string descripcionServicio, string codigoSolicitud)
        {
            var jmsg = new jMessage();
            string mail = string.Empty;
            string destinatarios = turnoConsolidacion.GetMails();
            //consulta todos los contenedores de la solicitud
            List<consultaDetalleUsuario> listaDetalle = CslHelperServicios.consultaSolicitudDetalleUsuario(idSolicitud, null);

            string error = string.Empty;
            //el mail del usuario logueado

            mail = Utility.cerrojo_msg(listaDetalle.Select(u => u.descripcionContenedor).ToList(), "I");

            var user_email = sUser.email;
            //destinatarios = user_email;


            var cfgs = dbconfig.GetActiveConfig(null, null, null);
            var mail_destino = cfgs.Where(a => a.config_name.Contains("mail_cerrojo")).FirstOrDefault();
            var correoBackUp = cfgs.Where(a => a.config_name.Contains("correoBackUpI")).FirstOrDefault();
            destinatarios = string.Format("{0};{1};{2}", correoBackUp != null ? correoBackUp.config_value : "no_cfg", user_email, mail_destino != null ? mail_destino.config_value : "no_cfg");

            CLSDataCentroSolicitud.addMail(out error, user_email, codigoSolicitud + " - Solicitud de " + descripcionServicio, mail, destinatarios, sUser.loginname, "", "");
            if (!string.IsNullOrEmpty(error))
            {
                alerta.Visible = true;
                alerta.InnerText = error;
                return;
            }
            else
            {
                imagenSolicitud.InnerHtml = "";
                Utility.mostrarMensajeRedireccionando(this.Page, "Se generó el código " + codigoSolicitud + " para su solicitud del servicio de " + descripcionServicio + ", revise su correo en unos minutos para mayor información.", "../csl/menudefault");
            }
        }
        public string invocacionEvento(string descripcionContenedor, string evento)
        {
            wsN4 g = new wsN4();
           
            string me = string.Empty;
            string errorN4 = string.Empty;

            StringBuilder newa = new StringBuilder();
            newa.Append("<icu><units>");
            newa.Append(string.Format("<unit-identity id=\"{0}\" type=\"{1}\">", descripcionContenedor, "CONTAINERIZED"));
            newa.Append("</unit-identity></units>");
            newa.Append("<properties><property tag=\"UnitRemark\" value=\"SERVICIO CERROJO ELECTRÓNICO\"/></properties>");
            newa.Append(string.Format("<event id=\"{1}\" note=\"SERVICIO CERROJO ELECTRÓNICO\" time-event-applied=\"{0}\" user-id=\"{2}\"/>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), evento, "MID_SEGURIDAD"));
            newa.Append("</icu>");

            var i = g.CallBasicService("ICT/ECU/GYE/CGSA", newa.ToString(), ref me);

            /*I ES LA RESPUESTA QUE DEVUELVE EL N4 Y me ES LA DESCRIPCION DEL MENSAJE DE ERROR*/
            if (i > 0)
            {
                errorN4 = me;
            }

            return errorN4;
        }

    }
}
