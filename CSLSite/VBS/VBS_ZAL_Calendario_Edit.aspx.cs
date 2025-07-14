using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using BillionEntidades;
using System.Data;
using VBSEntidades;
using VBSEntidades.ClaseEntidades;
using System.Web.Services;
using VBSEntidades.Calendario;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using ProcesaUnits;

namespace CSLSite
{


    public partial class VBS_ZAL_Calendario_Edit : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;

        private string cMensajes;
        VBS_CabeceraPlantilla objPlantilla = new VBS_CabeceraPlantilla();
        VBS_TurnosDetalle _objTurnos = new VBS_TurnosDetalle();
        List<VBS_TurnosDetalle> ListTurnos = new List<VBS_TurnosDetalle>();


        #endregion

        #region "Variables"

        private string LoginName = string.Empty;


        /*variables control de credito*/

        private Cls_Bil_PasePuertaCFS_Cabecera objCas = new Cls_Bil_PasePuertaCFS_Cabecera();
        private Cls_Bil_CasManual objDetalleCas = new Cls_Bil_CasManual();

        private VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
        private VBS_DetallePlantilla objDet = new VBS_DetallePlantilla();
        #endregion

        #region "Propiedades"

        private Int64? nSesion
        {
            get
            {
                return (Int64)Session["nSesion"];
            }
            set
            {
                Session["nSesion"] = value;
            }

        }




        #endregion

        #region "Metodos"




        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)//cabecera
            {
                //      this.banmsg.Visible = true;
                //         this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("1");
            }


        }

        private void Ocultar_Mensaje()
        {
             
            OcultarLoading("1");

        }


        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            objPlantilla.IV_USUARIO_CREA = ClsUsuario.nombres + "-" + ClsUsuario.apellidos;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }


            //cabecera de transaccion

            objPlantilla = new VBS_CabeceraPlantilla();
            Session["Plantilla" + this.hf_BrowserWindowName.Value] = objPlantilla;

            _objTurnos = new VBS_TurnosDetalle();
            Session["ListTurnos" + this.hf_BrowserWindowName2.Value] = _objTurnos;
        }

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.SslOn();
            }

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);

                return;
            }

            this.IsAllowAccess();


            if (!Page.IsPostBack)
            {
                ClsUsuario = Page.Tracker();
            }

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (!Page.IsPostBack)
                {
                    this.Carga_CboTipoCargas();
                    // this.Carga_CboTipoContenedor();
                    this.Crear_Sesion();
                    this.Carga_CboBloques();
                    this.Carga_CboLineas();
                    //DropDownList1.SelectedValue = "2";
                }
                else
                {


                }

                string tipoCargaId = Request.QueryString["tipoCargaId"];

                // Verificar si el parámetro tiene un valor válido y llamar a FiltrarDatosSegundoDropdown
                if (!string.IsNullOrEmpty(tipoCargaId))
                {
                    int idCarga = int.Parse(tipoCargaId);
                    FiltrarDatosSegundoDropdown(idCarga);
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }

        private void FiltrarDatosSegundoDropdown(int idCarga)
        {
            try
            {
                List<VBS_ConsultarTipoContenedor> ListadoFiltrado = VBS_ConsultarTipoContenedor.ConsultarTipoContenedor(out cMensajes);

                var list = ListadoFiltrado.Where(x => x.Id_Carga == idCarga).ToList();

                // Crear una cadena de texto con los elementos del DropDownList
                StringBuilder sb = new StringBuilder();
                foreach (var item in list)
                {
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Id_Contenedor, item.Desc_Contenedor);
                }

                // Generar la respuesta como cadena de texto
                string response = sb.ToString();

                // Enviar la respuesta al cliente
                Response.Write(response);
                Response.End();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "FiltrarDatosSegundoDropdown", "Hubo un error al filtrar datos", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }


        private void Carga_CboTipoCargas()
        {
            try
            {
                List<VBS_ConsultarTipoCargas> Listado = VBS_ConsultarTipoCargas.ConsultarTipoCargas(out cMensajes);

                var idcarga = Listado.FirstOrDefault().Id_Carga;

                Carga_CboTipoContenedor(idcarga);
                this.cboTipoCarga.DataSource = Listado;
                this.cboTipoCarga.DataTextField = "Desc_Carga";
                this.cboTipoCarga.DataValueField = "Id_Carga";
                this.cboTipoCarga.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboTipoCargas", "Hubo un error al cargar Tipo de cargas", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }

        private void Carga_CboBloques()
        {
            try
            {
                List<VBS_ConsultarTipoBloques> Listado = VBS_ConsultarTipoBloques.ConsultarTipoBloquesZAL(out cMensajes);

             
                this.cboBloque.DataSource = Listado;
                this.cboBloque.DataTextField = "Codigo";
                this.cboBloque.DataValueField = "IdBloque";
                this.cboBloque.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboTipoCargas", "Hubo un error al cargar Tipo de cargas", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }


        private void Carga_CboTipoContenedor(int idcarga)
        {
            try
            {
                List<VBS_ConsultarTipoContenedor> Listado = VBS_ConsultarTipoContenedor.ConsultarTipoContenedor(out cMensajes);

                var list = Listado.Where(x => x.Id_Carga == idcarga).ToList();

                this.cboTipoContenedor.DataSource = list;
                this.cboTipoContenedor.DataTextField = "Desc_Contenedor";
                this.cboTipoContenedor.DataValueField = "Id_Contenedor";
                this.cboTipoContenedor.DataBind();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboTipoContenedor", "Hubo un error al cargar Tipo Contenedor", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }


        #region "Turnos Exportaciones por lineas"
        private void Carga_CboLineas()
        {
            try
            {
                List<VBS_ConsultarLineas> Listado = VBS_ConsultarLineas.ConsultarLineas(out cMensajes);

                   
                this.CboLineas.DataSource = Listado;
                this.CboLineas.DataTextField = "Linea";
                this.CboLineas.DataValueField = "IdLinea";
                this.CboLineas.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboTipoContenedor", "Hubo un error al cargar Tipo de cargas", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }
        #endregion


        [WebMethod]
        public static string GetCalendarEvents(int year, int month, bool inicial)
        {
            List<EventoCalendario> eventos = new List<EventoCalendario>();

            VBS_CabeceraPlantilla objPlantilla1 = new VBS_CabeceraPlantilla();
            var detalles = objPlantilla1.GetListaCalendario(year, month);

            try
            {
                if (inicial)
                {
                    if (month == 12)
                    {
                        year = year + 1;
                        month = 1;
                    }
                    else
                    {
                        month =  month + 1;
                    }

                    var detalles1 = objPlantilla1.GetListaCalendario(year, month);

                    if (detalles1.Exitoso)
                    {
                        if (!detalles.Exitoso)
                        {
                            detalles = detalles1;
                        }
                        else
                        {
                            detalles.Resultado.AddRange(detalles1.Resultado);
                        }
                    }
                }
            }
            catch { }

            if (detalles.Resultado == null)
            {
                return null;
            }
            else
            {
                foreach (Calendario_Turnos detalle in detalles.Resultado)
                {
                    EventoCalendario evento = new EventoCalendario();
                    evento.title = $"{detalle.Tipo_Cargas} - {detalle.Tipo_Contenedor} - {detalle.Total_Turnos} {"Disponible"} ({detalle.Total_Disponibles})"; // Combinar tipo_contenedor y total_turnos
                    evento.start = detalle.Dia.ToString("yyyy-MM-dd");
                    evento.end = detalle.Dia.AddDays(1).ToString("yyyy-MM-dd");
                    evento.idDetalle = detalle.Detalle;
                    if (detalle.TipoCargaId == 1)
                        evento.color = "#336BFF";
                    if (detalle.TipoCargaId == 2)
                        evento.color = "#17a2b8";
                    if (detalle.TipoCargaId == 3)
                        evento.color = "#dc3545";


                    eventos.Add(evento);
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(eventos.ToArray());
                return json;
            }

        }
        [WebMethod]
        public static string GetCalendarEventsImport(int year, int month, bool inicial, int pageIndex, int pageSize)
        {
            List<EventoCalendario> eventos = new List<EventoCalendario>();

            VBS_CabeceraPlantilla objPlantilla1 = new VBS_CabeceraPlantilla();
            var detalles = objPlantilla1.GetListaCalendarioImportZAL(year, month);

            try
            {
                int startIndex = pageIndex * pageSize;
                if (inicial)
                {
                    if (month == 12)
                    {
                        year = year + 1;
                        month = 1;
                    }
                    else
                    {
                        month = month + 1;
                    }

                    var detalles1 = objPlantilla1.GetListaCalendarioImportZAL(year, month);

                    if (detalles1.Exitoso)
                    {
                        if (!detalles.Exitoso)
                        {
                            detalles = detalles1;
                        }
                        else
                        {
                            //detalles.Resultado.Clear();
                            detalles.Resultado.AddRange(detalles1.Resultado);
                        }
                    }
                }
                else
                {
                    if (month == 12)
                    {
                        year = year + 1;
                        month = 1;
                    }
                    else
                    {
                        month = month + 1;
                    }

                    var detalles1 = objPlantilla1.GetListaCalendarioImportZAL(year, month);

                    if (detalles1.Exitoso)
                    {
                        if (detalles.Exitoso)
                        {
                            detalles.Resultado.Clear();
                        }

                        detalles = detalles1;
                       //detalles.Resultado.AddRange(detalles1.Resultado);
                        
                    }

                }
            }
            catch { }


            if (detalles.Resultado == null)
            {
                return null;
            }
            else
            {

                var query = detalles.Resultado.Distinct();//  DistinctBy(p => new { p.TypeId, p.Name });

                int startIndex = pageIndex * pageSize;
                var eventosPaginados = query.Skip(startIndex).Take(pageSize);

                foreach (Calendario_Turnos_Import detalle in eventosPaginados)
                {
                    EventoCalendario evento = new EventoCalendario();
                    evento.title = $"{detalle.Bloque}  - {detalle.Total_Turnos} Disponible ({detalle.Total_Disponibles})";
                    evento.start = detalle.Dia.ToString("yyyy-MM-dd");
                    evento.end = detalle.Dia.AddDays(1).ToString("yyyy-MM-dd");
                    evento.color = "#336BFF";
                    evento.idDetalle = detalle.idBloque;

                    eventos.Add(evento);
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(eventos.ToArray());
                return json;
            }

        }

        [WebMethod]
        public static string GetCalendarEventsExpoLineas(int year, int month, bool inicial, int pageIndex, int pageSize)
        {
            List<EventoCalendario> eventos = new List<EventoCalendario>();

            VBS_CabeceraPlantilla objPlantilla1 = new VBS_CabeceraPlantilla();
            var detalles = objPlantilla1.GetCalendarEventsExpoLineas(year, month);

            try
            {
                int startIndex = pageIndex * pageSize;
                if (inicial)
                {
                    if (month == 12)
                    {
                        year = year + 1;
                        month = 1;
                    }
                    else
                    {
                        month = month + 1;
                    }

                    var detalles1 = objPlantilla1.GetCalendarEventsExpoLineas(year, month);

                    if (detalles1.Exitoso)
                    {
                        if (!detalles.Exitoso)
                        {
                            detalles = detalles1;
                        }
                        else
                        {
                           // detalles.Resultado.Clear();
                            detalles.Resultado.AddRange(detalles1.Resultado);
                        }
                    }
                }
                else
                {
                    if (month == 12)
                    {
                        year = year + 1;
                        month = 1;
                    }
                    else
                    {
                        month = month + 1;
                    }

                    var detalles1 = objPlantilla1.GetCalendarEventsExpoLineas(year, month);

                    if (detalles1.Exitoso)
                    {
                        if (detalles.Exitoso)
                        {
                            detalles.Resultado.Clear();
                        }

                        detalles = detalles1;
                        
                    }

                }
            }
            catch { }


            if (detalles.Resultado == null)
            {
                return null;
            }
            else
            {

                var query = detalles.Resultado.Distinct();//  DistinctBy(p => new { p.TypeId, p.Name });

                int startIndex = pageIndex * pageSize;
                var eventosPaginados = query.Skip(startIndex).Take(pageSize);

                foreach (Calendario_Turnos_Lineas detalle in eventosPaginados)
                {
                    EventoCalendario evento = new EventoCalendario();
                    evento.title = $"{detalle.DescLinea}  - {detalle.Total_Turnos} Disponible ({detalle.Total_Disponibles})";
                    evento.start = detalle.Dia.ToString("yyyy-MM-dd");
                    evento.end = detalle.Dia.AddDays(1).ToString("yyyy-MM-dd");
                    evento.color = "#336BFF";
                    evento.IdLinea = detalle.IdLinea;

                    eventos.Add(evento);
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(eventos.ToArray());
                return json;
            }

        }


        public class Detalle
        {
            public string secuencia { get; set; }
            public string tipoContenedor { get; set; }
            public string tipoCargas { get; set; }
            public string cantidad { get; set; }
            public string categoria { get; set; }
            public string tipoCargaId { get; set; }
            public string tipoContenedorId { get; set; }

        }
        public class DetalleImport
        {
            public string secuencia { get; set; }
            public string bloqueId { get; set; }
            public string tipoBloque { get; set; }
            public string frecuencia { get; set; }

        }

        public class DetalleLineas
        {
            public string secuencia { get; set; }
            public string tipoLineaId { get; set; }
            public string descLinea { get; set; }
            public string cantidad { get; set; }
            

        }

        [WebMethod]
        public static string GuardarDatosTabla1(string vigenciaInicial, string vigenciaFinal, int banderaDayClick, List<Detalle> detalles)
        {



            if (detalles == null)
            {
                throw new ArgumentNullException(nameof(detalles));
            }

            try
            {

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                string fechaFormateadaInicial = string.Empty;
                string fechaFormateadaFinal = string.Empty;
                VBS_Calendario_Edit pageCalendario = new VBS_Calendario_Edit();
                if (banderaDayClick == 0)
                {
                    string formato = "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz";
                    DateTimeOffset fechaVigenciaInicial = DateTimeOffset.ParseExact(vigenciaInicial, formato, CultureInfo.InvariantCulture);
                    DateTimeOffset fechaVigenciaFinal = DateTimeOffset.ParseExact(vigenciaFinal, formato, CultureInfo.InvariantCulture);

                    fechaVigenciaFinal = fechaVigenciaFinal.AddDays(-1);

                    fechaFormateadaInicial = fechaVigenciaInicial.ToString("yyyy-MM-dd");
                    fechaFormateadaFinal = fechaVigenciaFinal.ToString("yyyy-MM-dd");
                }
                else
                {
                    fechaFormateadaInicial = vigenciaInicial;
                    fechaFormateadaFinal = vigenciaFinal;
                }



                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                objCab.FECHA_CREACION = DateTime.Now;
                objCab.ESTADO = true;
                objCab.NOMBRE_PLANTILLA = ClsUsuario.nombres + "-" + ClsUsuario.apellidos;
                objCab.VIGENCIA_INICIAL = Convert.ToDateTime(fechaFormateadaInicial);
                objCab.VIGENCIA_FINAL = Convert.ToDateTime(fechaFormateadaFinal);
                objCab.USUARIO_CREACION = ClsUsuario.nombres + "-" + ClsUsuario.apellidos;


                System.Xml.Linq.XDocument XMLTurnos = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                    new System.Xml.Linq.XElement("TURNOS", from p in detalles.AsEnumerable().AsParallel()
                                                           select new System.Xml.Linq.XElement("DETALLE",
                                                           new System.Xml.Linq.XAttribute("SECUENCIA", p.secuencia),
                                                           new System.Xml.Linq.XAttribute("FECHA_CREACION", DateTime.Now),
                                                           new System.Xml.Linq.XAttribute("TIPO_CONTENEDOR", p.tipoContenedorId),
                                                           new System.Xml.Linq.XAttribute("TIPO_CARGAS", p.tipoCargaId),
                                                           new System.Xml.Linq.XAttribute("CANTIDAD", p.cantidad),
                                                           new System.Xml.Linq.XAttribute("ESTADO", 1),
                                                           new System.Xml.Linq.XAttribute("VIGENCIA_INICIAL", fechaFormateadaInicial),
                                                           new System.Xml.Linq.XAttribute("VIGENCIA_FINAL", fechaFormateadaFinal),
                                                           new System.Xml.Linq.XAttribute("flag", "I"))));
                objCab.xmlTurnos = XMLTurnos.ToString();

                var _cMensajes = String.Empty;

                var nProceso = objCab.SaveTransaction(out _cMensajes);



                var consultarTurnosGenerados = objCab.DetallePlantillas("", Convert.ToInt32(nProceso));
                if (consultarTurnosGenerados.Resultado != null)
                {
                    foreach (var ListDetalle in consultarTurnosGenerados.Resultado)
                    {

                        var consultaTurnosDetalle = objCab.VerificarCombinacionTipoCargaContenedor(Convert.ToInt32(ListDetalle.TIPO_CARGAS), ListDetalle.TIPO_CONTENEDOR,vigenciaInicial);

                        if (consultaTurnosDetalle.Resultado == null)
                        {
                            var Transportista = objCab.GuardarTurnos(Convert.ToInt32(nProceso),
                                ListDetalle.ID_DETALLEPLANTILLA,
                                ListDetalle.TIPO_CONTENEDOR, ListDetalle.TIPO_CARGAS,
                                ListDetalle.CANTIDAD,
                                ListDetalle.CATEGORIA,

                                ClsUsuario.nombres + "-" + ClsUsuario.apellidos,
                                Convert.ToDateTime(fechaFormateadaInicial),
                                Convert.ToDateTime(fechaFormateadaFinal));


                        }

                    }

                }
                else
                {

                }
                return "Datos guardados correctamente";
            }
            catch (Exception ex)
            {

                return "Error al procesar los detalles: " + ex.Message;
            }

        }

        [WebMethod]
        public static string GuardarDatosTablaImport(string vigenciaInicial, string vigenciaFinal, int banderaDayClick, List<DetalleImport> detalles)
        {
            if (detalles == null)
            {
                throw new ArgumentNullException(nameof(detalles));
            }

            try
            {

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                string fechaFormateadaInicial = string.Empty;
                string fechaFormateadaFinal = string.Empty;
                VBS_Calendario_Edit pageCalendario = new VBS_Calendario_Edit();
                if (banderaDayClick == 0)
                {
                    string formato = "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz";
                    DateTimeOffset fechaVigenciaInicial = DateTimeOffset.ParseExact(vigenciaInicial, formato, CultureInfo.InvariantCulture);
                    DateTimeOffset fechaVigenciaFinal = DateTimeOffset.ParseExact(vigenciaFinal, formato, CultureInfo.InvariantCulture);

                    fechaVigenciaFinal = fechaVigenciaFinal.AddDays(-1);

                    fechaFormateadaInicial = fechaVigenciaInicial.ToString("yyyy-MM-dd");
                    fechaFormateadaFinal = fechaVigenciaFinal.ToString("yyyy-MM-dd");
                }
                else
                {
                    fechaFormateadaInicial = vigenciaInicial;
                    fechaFormateadaFinal = vigenciaFinal;
                }

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();

                foreach (var ListDetalle in detalles)
                {

                    var consultarTipoBloqueExiste = objCab.VerificarCombinacionTipoBloque(Convert.ToInt32(ListDetalle.bloqueId), ListDetalle.tipoBloque, vigenciaInicial);

                    if(consultarTipoBloqueExiste.Resultado == null)
                    {
                        var Transportista = objCab.GuardarTurnosImport(
                      ListDetalle.bloqueId,
                      ListDetalle.tipoBloque,
                      Convert.ToInt32(ListDetalle.frecuencia),

                      ClsUsuario.nombres + "-" + ClsUsuario.apellidos,
                        Convert.ToDateTime(fechaFormateadaInicial),
                       Convert.ToDateTime(fechaFormateadaFinal));
                    }
                  
                }


                return "Datos guardados correctamente";
            }
            catch (Exception ex)
            {

                return "Error al procesar los detalles: " + ex.Message;
            }
        }

        [WebMethod]
        public static string GuardarDatosTablaLineas(string vigenciaInicial, string vigenciaFinal, int banderaDayClick, List<DetalleLineas> detalles)
        {



            if (detalles == null)
            {
                throw new ArgumentNullException(nameof(detalles));
            }

            try
            {

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                string fechaFormateadaInicial = string.Empty;
                string fechaFormateadaFinal = string.Empty;

                VBS_Calendario_Edit pageCalendario = new VBS_Calendario_Edit();

                if (banderaDayClick == 0)
                {

                    string formato = "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz";
                    DateTimeOffset fechaVigenciaInicial = DateTimeOffset.ParseExact(vigenciaInicial, formato, CultureInfo.InvariantCulture);
                    DateTimeOffset fechaVigenciaFinal = DateTimeOffset.ParseExact(vigenciaFinal, formato, CultureInfo.InvariantCulture);

                    
                    fechaVigenciaFinal = fechaVigenciaFinal.AddDays(-1);

                    fechaFormateadaInicial = fechaVigenciaInicial.ToString("yyyy-MM-dd");
                    fechaFormateadaFinal = fechaVigenciaFinal.ToString("yyyy-MM-dd");
                }
                else
                {
                    fechaFormateadaInicial = vigenciaInicial;
                    fechaFormateadaFinal = vigenciaFinal;
                }



                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                objCab.FECHA_CREACION = DateTime.Now;
                objCab.ESTADO = true;
                objCab.NOMBRE_PLANTILLA = ClsUsuario.nombres + "-" + ClsUsuario.apellidos;
                objCab.VIGENCIA_INICIAL = Convert.ToDateTime(fechaFormateadaInicial);
                objCab.VIGENCIA_FINAL = Convert.ToDateTime(fechaFormateadaFinal);
                objCab.USUARIO_CREACION = ClsUsuario.nombres + "-" + ClsUsuario.apellidos;


                System.Xml.Linq.XDocument XMLTurnos = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                    new System.Xml.Linq.XElement("TURNOS", from p in detalles.AsEnumerable().AsParallel()
                                                           select new System.Xml.Linq.XElement("DETALLE",
                                                           new System.Xml.Linq.XAttribute("SECUENCIA", p.secuencia),
                                                           new System.Xml.Linq.XAttribute("FECHA_CREACION", DateTime.Now),
                                                           new System.Xml.Linq.XAttribute("ID_LINEA", p.tipoLineaId),
                                                           new System.Xml.Linq.XAttribute("DESC_LINEA", p.descLinea),
                                                           new System.Xml.Linq.XAttribute("CANTIDAD", p.cantidad),
                                                           new System.Xml.Linq.XAttribute("ESTADO", 1),
                                                           new System.Xml.Linq.XAttribute("VIGENCIA_INICIAL", fechaFormateadaInicial),
                                                           new System.Xml.Linq.XAttribute("VIGENCIA_FINAL", fechaFormateadaFinal),
                                                           new System.Xml.Linq.XAttribute("flag", "I"))));
                objCab.xmlTurnos = XMLTurnos.ToString();

                var _cMensajes = String.Empty;

                var nProceso = objCab.SaveTransaction_Expo_Lineas(out _cMensajes);



                var consultarTurnosGenerados = objCab.DetallePlantillas_Expo_Lineas("", Convert.ToInt64(nProceso));
                if (consultarTurnosGenerados.Resultado != null)
                {
                    foreach (var ListDetalle in consultarTurnosGenerados.Resultado)
                    {

                        var consultaTurnosDetalle = objCab.VerificarCombinacionLineas(ListDetalle.ID_LINEA, vigenciaInicial);

                        if (consultaTurnosDetalle.Resultado == null)
                        {
                            var Transportista = objCab.GuardarTurnos_PorLineas(Convert.ToInt64(nProceso),
                                ListDetalle.ID_DETALLEPLANTILLA,
                                ListDetalle.ID_LINEA,
                                ListDetalle.CANTIDAD,
                                ListDetalle.CATEGORIA,
                                ClsUsuario.nombres + "-" + ClsUsuario.apellidos,
                                Convert.ToDateTime(fechaFormateadaInicial),
                                Convert.ToDateTime(fechaFormateadaFinal));

                        }

                    }

                }
                else
                {

                }
                return "Datos guardados correctamente";
            }
            catch (Exception ex)
            {

                return "Error al procesar los detalles: " + ex.Message;
            }

        }

        [WebMethod]
        public static string ConsultarEventos(int idDetalle, string start, string end)
        {
            try
            {
                DateTime fecha = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime fechaRestada = fecha.AddDays(-1);
                string fechaRestadaString = fechaRestada.ToString("yyyy-MM-dd");

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var consultaTurnosDetalle = objCab.GetListaTurnosPorFechas(idDetalle, start, fechaRestadaString);


                // Serializar los datos de eventos paginados a JSON
                string jsonEventos = JsonConvert.SerializeObject(consultaTurnosDetalle.Resultado);

                return jsonEventos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }

        [WebMethod]
        public static string ConsultarEventosImport(int idDetalle, string start, string end)
        {
            try
            {
                DateTime fecha = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime fechaRestada = fecha.AddDays(-1);
                string fechaRestadaString = fechaRestada.ToString("yyyy-MM-dd");

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var consultaTurnosDetalle = objCab.GetListaTurnosImportPorFechas(idDetalle, start, fechaRestadaString);

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                // Serializar los datos de eventos paginados a JSON
                string jsonEventos = JsonConvert.SerializeObject(consultaTurnosDetalle.Resultado);

                return jsonEventos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }

        [WebMethod]
        public static string ConsultarEventosLineas(string IdLinea, string start, string end)
        {
            try
            {
                DateTime fecha = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime fechaRestada = fecha.AddDays(-1);
                string fechaRestadaString = fechaRestada.ToString("yyyy-MM-dd");

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var consultaTurnosDetalle = objCab.GetListaTurnosLineasPorFechas(IdLinea, start, fechaRestadaString);

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                // Serializar los datos de eventos paginados a JSON
                string jsonEventos = JsonConvert.SerializeObject(consultaTurnosDetalle.Resultado);

                return jsonEventos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }

        [WebMethod]
        public static string GuardarDatos(List<ActuEvento> datosTabla)
        {
            try
            {

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();

                System.Xml.Linq.XDocument XMLTurnos = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                    new System.Xml.Linq.XElement("TURNOS", from p in datosTabla.AsEnumerable().AsParallel()
                                                           select new System.Xml.Linq.XElement("DETALLE",
                                                           new System.Xml.Linq.XAttribute("IDTURNO", Convert.ToInt64(p.idTurno)),
                                                           new System.Xml.Linq.XAttribute("CANTIDAD", p.cantidad),
                                                           new System.Xml.Linq.XAttribute("FECHAACTU", DateTime.Now),
                                                           new System.Xml.Linq.XAttribute("USUARIOMODIFI", ClsUsuario.nombres + "-" + ClsUsuario.apellidos),

                                                           //new System.Xml.Linq.XAttribute("DISPONIBLE", p.disponible),
                                                           new System.Xml.Linq.XAttribute("DISPONIBLE", p.disponible),
                                                           new System.Xml.Linq.XAttribute("ASIGNADOS", p.asignados),
                                                           new System.Xml.Linq.XAttribute("flag", "I"))));
                objCab.xmlTurnos = XMLTurnos.ToString();

                var _cMensajes = String.Empty;

                var nProceso = objCab.SaveTransactionUPDATE(out _cMensajes);
                if (nProceso > 0)
                {

                    return "success";
                }

                return "fail";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return JsonConvert.SerializeObject(new { result = "error" });
            }
        }

        //graba turnos importacion
        [WebMethod]
        public static string GuardarDatosImport(List<ActuEvento> datosTabla)
        {
            try
            {

                //proceso validacion

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();

                System.Xml.Linq.XDocument XMLTurnos = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                    new System.Xml.Linq.XElement("TURNOS", from p in datosTabla.AsEnumerable().AsParallel()
                                                           select new System.Xml.Linq.XElement("DETALLE",
                                                           new System.Xml.Linq.XAttribute("IDTURNO", Convert.ToInt64(p.idTurno)),
                                                           new System.Xml.Linq.XAttribute("CANTIDAD", p.cantidad),
                                                           new System.Xml.Linq.XAttribute("FECHAACTU", DateTime.Now),
                                                           new System.Xml.Linq.XAttribute("USUARIOMODIFI", ClsUsuario.nombres + "-" + ClsUsuario.apellidos),

                                                          // new System.Xml.Linq.XAttribute("DISPONIBLE", p.disponible),
                                                            new System.Xml.Linq.XAttribute("DISPONIBLE", p.disponible),

                                                           new System.Xml.Linq.XAttribute("ASIGNADOS", p.asignados),
                                                           new System.Xml.Linq.XAttribute("flag", "I"))));
                objCab.xmlTurnos = XMLTurnos.ToString();

                var _cMensajes = String.Empty;

                var nProceso = objCab.SaveTransactionUPDATEIMPORT(out _cMensajes);
                if (nProceso > 0)
                {

                    return "success";
                }

                return "fail";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return JsonConvert.SerializeObject(new { result = "error" });
            }
        }

        [WebMethod]
        public static string GuardarDatosPorlineas(List<ActuEventoLineas> datosTabla)
        {
            try
            {

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();

                System.Xml.Linq.XDocument XMLTurnos = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                    new System.Xml.Linq.XElement("TURNOS", from p in datosTabla.AsEnumerable().AsParallel()
                                                           select new System.Xml.Linq.XElement("DETALLE",
                                                           new System.Xml.Linq.XAttribute("IDTURNO", Convert.ToInt64(p.idTurno)),
                                                           new System.Xml.Linq.XAttribute("CANTIDAD", p.cantidad),
                                                           new System.Xml.Linq.XAttribute("FECHAACTU", DateTime.Now),
                                                           new System.Xml.Linq.XAttribute("USUARIOMODIFI", ClsUsuario.nombres + "-" + ClsUsuario.apellidos),
                                                           new System.Xml.Linq.XAttribute("DISPONIBLE", p.disponible),
                                                           new System.Xml.Linq.XAttribute("ASIGNADOS", p.asignados),
                                                           new System.Xml.Linq.XAttribute("flag", "I"))));
                objCab.xmlTurnos = XMLTurnos.ToString();

                var _cMensajes = String.Empty;

                var nProceso = objCab.SaveTransactionUPDATE_LINEAS(out _cMensajes);
                if (nProceso > 0)
                {

                    return "success";
                }

                return "fail";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return JsonConvert.SerializeObject(new { result = "error" });
            }
        }

        [WebMethod]
        public static string GuardarDatosVacios(List<ActuEvento> datosTabla)
        {
            try
            {

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();

                System.Xml.Linq.XDocument XMLTurnos = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                    new System.Xml.Linq.XElement("TURNOS", from p in datosTabla.AsEnumerable().AsParallel()
                                                           select new System.Xml.Linq.XElement("DETALLE",
                                                           new System.Xml.Linq.XAttribute("IDTURNO", Convert.ToInt64(p.idTurno)),
                                                           new System.Xml.Linq.XAttribute("CANTIDAD", p.cantidad),
                                                           new System.Xml.Linq.XAttribute("FECHAACTU", DateTime.Now),
                                                           new System.Xml.Linq.XAttribute("USUARIOMODIFI", ClsUsuario.nombres + "-" + ClsUsuario.apellidos),

                                                           new System.Xml.Linq.XAttribute("flag", "I"))));
                objCab.xmlTurnos = XMLTurnos.ToString();

                var _cMensajes = String.Empty;

                var nProceso = objCab.SaveTransactionUPDetalleVacios(out _cMensajes);
                if (nProceso > 0)
                {

                    return "success";
                }

                return "fail";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return JsonConvert.SerializeObject(new { result = "error" });
            }
        }

        [WebMethod]
        public static string ConsultarEventosPorDia(string start)
        {
            try
            {

                List<EventoCalendario> eventos = new List<EventoCalendario>();


                DateTime fecha = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                string fechaRestadaString = fecha.ToString("yyyy-MM-dd");

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var consultaTurnosDetalle = objCab.GetListaTurnosPorDia(fechaRestadaString);

                if (consultaTurnosDetalle.Resultado != null)
                {
                    foreach (VBS_TurnosDetalle detalle in consultaTurnosDetalle.Resultado)
                    {
                        EventoCalendario evento = new EventoCalendario();
                        evento.title = $"{detalle.TipoCargas} - {detalle.TipoContenedor} - {detalle.Cantidad} {"Disponible"} - ({detalle.Disponible})"; // Combinar tipo_contenedor y total_turnos
                        evento.start = fechaRestadaString;
                        evento.end = fechaRestadaString;
                        if (detalle.TipoCargaId == 1)
                            evento.color = "#336BFF";
                        if (detalle.TipoCargaId == 2)
                            evento.color = "#17a2b8";
                        if (detalle.TipoCargaId == 3)
                            evento.color = "#dc3545";
                        evento.horario = detalle.Horario.ToString(@"hh\:mm");

                        eventos.Add(evento);
                    }

                }


                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(eventos.ToArray());

                // Serializar los datos de eventos paginados a JSON

                return json;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }

        [WebMethod]
        public static string ConsultarEventosPorDiaImport(string start)
        {
            try
            {

                List<EventoCalendario> eventos = new List<EventoCalendario>();


                DateTime fecha = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                string fechaRestadaString = fecha.ToString("yyyy-MM-dd");

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var consultaTurnosDetalle = objCab.GetListaTurnosPorDiaImport(fechaRestadaString);

                if (consultaTurnosDetalle.Resultado != null)
                {
                    foreach (VBS_TurnosDetalle_Import detalle in consultaTurnosDetalle.Resultado)
                    {
                        EventoCalendario evento = new EventoCalendario();
                        evento.title = $"{detalle.CodigoBloque}  - {detalle.Cantidad} {"Disponible"} - ({detalle.Disponible})"; // Combinar tipo_contenedor y total_turnos
                        evento.start = fechaRestadaString;
                        evento.end = fechaRestadaString;
                        evento.color = "#336BFF";

                        evento.horario = detalle.Horario.ToString(@"hh\:mm");

                        eventos.Add(evento);
                    } 

                }


                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(eventos.ToArray());

                // Serializar los datos de eventos paginados a JSON

                return json;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }

        [WebMethod]
        public static string ConsultarEventosPorDiaLineas(string start)
        {
            try
            {

                List<EventoCalendario> eventos = new List<EventoCalendario>();


                DateTime fecha = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                string fechaRestadaString = fecha.ToString("yyyy-MM-dd");

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var consultaTurnosDetalle = objCab.GetListaTurnosPorDiaLineas(fechaRestadaString);

                if (consultaTurnosDetalle.Resultado != null)
                {
                    foreach (VBS_TurnosDetalleLineas detalle in consultaTurnosDetalle.Resultado)
                    {
                        EventoCalendario evento = new EventoCalendario();
                        evento.title = $"{detalle.IdLinea}  - {detalle.Cantidad} {"Disponible"} - ({detalle.Disponible})"; // Combinar tipo_contenedor y total_turnos
                        evento.start = fechaRestadaString;
                        evento.end = fechaRestadaString;
                        evento.color = "#336BFF";

                        evento.horario = detalle.Horario.ToString(@"hh\:mm");

                        eventos.Add(evento);
                    }

                }


                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(eventos.ToArray());

                // Serializar los datos de eventos paginados a JSON

                return json;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }

        [WebMethod]

        public static string ConsultarBookingCalendario(string fecha)
        {
            try
            {
                DataHelper obj = new DataHelper();
                
                var tablaNave = obj.GetTablaNaves(fecha);
                string jsonTablaNave = ConvertDataTableToJson(tablaNave);
                return jsonTablaNave;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }

        public static string ConvertDataTableToJson(DataTable dataTable)
        {
            string json = JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented);
            return json;
        }

        [WebMethod]
        public static string ConsultarTablaExpoPorDia(string start)
        {
            try
            {

                List<EventoCalendario> eventos = new List<EventoCalendario>();


                DateTime fecha = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                string fechaRestadaString = fecha.ToString("yyyy-MM-dd");

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var consultaTurnosDetalle = objCab.GetTablaExpoPorDia(fechaRestadaString);

                if (consultaTurnosDetalle.Resultado != null)
                {
                    foreach (VBS_TurnosDetalle detalle in consultaTurnosDetalle.Resultado)
                    {
                        EventoCalendario evento = new EventoCalendario();
                        evento.title = $"{detalle.TipoCargas} - {detalle.TipoContenedor} - {detalle.Cantidad} {"Disponible"} - ({detalle.Disponible})"; // Combinar tipo_contenedor y total_turnos
                        evento.start = fechaRestadaString;
                        evento.end = fechaRestadaString;
                        evento.tipoCarga = detalle.TipoCargas;
                        evento.tipoContenedor = detalle.TipoContenedor;
                        evento.cantidad = detalle.Cantidad;
                        evento.tipoCargaId = detalle.TipoCargaId;
                        evento.tipoContenedorId = detalle.TipoContenedorId;

                        eventos.Add(evento);
                    }

                }


                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(eventos.ToArray());

                // Serializar los datos de eventos paginados a JSON

                return json;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }

        [WebMethod]
        public static string ConsultarTablaImportPorDia(string start)
        {
            try
            {

                List<EventoCalendario> eventos = new List<EventoCalendario>();

                DateTime fecha = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                string fechaRestadaString = fecha.ToString("yyyy-MM-dd");

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var consultaTurnosDetalle = objCab.GetTablaImportPorDiaZAL(fechaRestadaString);

                if (consultaTurnosDetalle.Resultado != null)
                {
                    foreach (VBS_TurnosDetalle_Import detalle in consultaTurnosDetalle.Resultado)
                    {
                        EventoCalendario evento = new EventoCalendario();
                        evento.title = $"{detalle.CodigoBloque}  - {detalle.Cantidad} {"Disponible"} - ({detalle.Disponible})"; // Combinar tipo_contenedor y total_turnos
                        evento.start = fechaRestadaString;
                        evento.end = fechaRestadaString;
                        evento.cantidad = detalle.Frecuencia;
                        evento.tipoBloqueId = detalle.idBloque;
                        evento.codigoBloque = detalle.CodigoBloque;

                        eventos.Add(evento);
                    }
                    
                }

                
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(eventos.ToArray());


                return json;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }

        [WebMethod]
        public static string ConsultarTablaLineasPorDia(string start)
        {
            try
            {

                List<EventoCalendario> eventos = new List<EventoCalendario>();

                DateTime fecha = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                string fechaRestadaString = fecha.ToString("yyyy-MM-dd");

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var consultaTurnosDetalle = objCab.GetTablaLineastPorDia(fechaRestadaString);

                if (consultaTurnosDetalle.Resultado != null)
                {
                    foreach (VBS_TurnosDetalleLineas detalle in consultaTurnosDetalle.Resultado)
                    {
                        EventoCalendario evento = new EventoCalendario();
                        evento.title = $"{detalle.IdLinea}  - {detalle.Cantidad} {"Disponible"} - ({detalle.Disponible})"; // Combinar tipo_contenedor y total_turnos
                        evento.start = fechaRestadaString;
                        evento.end = fechaRestadaString;
                        evento.cantidad = detalle.Cantidad;
                        evento.IdLinea = detalle.IdLinea;
                        evento.descLinea = detalle.descLinea;
                        eventos.Add(evento);
                    }

                }


                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(eventos.ToArray());


                return json;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }
        [WebMethod]

        public static string GetDetalleVacios(string idTurno)
        {
            try
            {
                
                if (Convert.ToInt32(idTurno) > 0)
                {
                    VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();

                    var consultarDetalle = objCab.ConsultarDetalleVacios(Convert.ToInt32(idTurno));

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(consultarDetalle.Resultado.ToArray());


                    return json;
                }
                else
                {
                    throw new Exception("no existen datos a consultar");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }

         }
    }

}