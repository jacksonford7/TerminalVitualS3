using BillionEntidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClsAppCgsa;
using System.IO;


namespace CSLSite
{
    public partial class notificacionesConsultaPruebas : System.Web.UI.Page
    {
        #region "Variables"
        private string cMensajes;
        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();
        usuario ClsUsuario;
        private const int v_RegistrosPorPagina = 5;
        private string OError;
        private static Int64? lm = -3;
        #endregion

        #region "Metodos"

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
            UPCARGA.Update();
        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
            //upLabel.Update();
        }

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


        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            OcultarLoading("1");

            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");

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
                Response.Redirect("../stc/inicioSesion.aspx", false);

                return;
            }

            this.banmsg.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;

            }

        }

        private void Carga_Inicial_Contenedores()
        {
            try
            {
                usuario ClsUsuario = null;
                ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                StringBuilder tab = new StringBuilder();

                string v_cuerpo = string.Empty;
                string v_detalle = string.Empty;
                string v_html = @"  <div class='bs-example'>
                                            <div class='accordion' id='accordionExample'>";

                //string cRuc = "0992500913001";
                //ClsUsuario.ruc
                List<Lista_Contenedores> TablaCasos = Lista_Contenedores.Listado_Contenedores(ClsUsuario.ruc, out OError);
                if (TablaCasos != null)
                {
                    foreach (var Det in TablaCasos)
                    {
                        v_detalle = string.Empty;

                        List<Lista_Contenedores> TablaEventos = Lista_Contenedores.Listado_Eventos_Contenedores(ClsUsuario.ruc, (Det.UnitGkey.HasValue ? Det.UnitGkey.Value : 0) ,out OError);
                        if (TablaEventos != null)
                        {
                            foreach (var Eve in TablaEventos)
                            {
                               
                                v_detalle = v_detalle + @"<p><div class='bs-example'><div class='accordion' id='accordionExampledet'><div class='card'><div class='card-header' id='heading{X2}'><h2 class='mb-0'>";
                                v_detalle = v_detalle + "<button type = 'button' class='btn btn-link text-red' data-toggle='collapse' data-target='#collapse{X2}'><i class='fa fa-chevron-right'></i><span style='color:#999999;font-weight:bold;'>" + Eve.Name + "</span></button></h2></div>";
                                v_detalle = v_detalle + "<div id = 'collapse{X2}' class='collapse' aria-labelledby='heading{X2}' data-parent='#accordionExampledet'>";
                                v_detalle = v_detalle + "<div class='card-body'>";
                                v_detalle = v_detalle + Eve.cuerpo;



                                //caerbono neutro exportaciones
                                if (Eve.id_certificado_carbo.HasValue && !string.IsNullOrEmpty(Eve.cert_secuencia))
                                {
                                    string IdCodificado = securetext(Eve.id_certificado_carbo.Value);

                                    //botones de carbono neutro
                                    v_detalle = v_detalle + "<br/><p>";
                                    v_detalle = v_detalle + "<table cellpadding='0' cellspacing='1' border='0'>";
                                    v_detalle = v_detalle + "<tr>";
                                    v_detalle = v_detalle + "<td width='254' height='19' valign='top'>";
                                    v_detalle = v_detalle + "<div class='tcomand'>";

                                    if (Eve.categoria.Equals("EXPRT"))
                                    {
                                        v_detalle = v_detalle + "<span class='btn btn-link text-red' onclick=\"mostrar('" + IdCodificado + "', '" + Eve.cert_secuencia + "')\">Ver Certificado</span>";
                                    }
                                    else
                                    {
                                        v_detalle = v_detalle + "<span class='btn btn-link text-red' onclick=\"mostrar_impo('" + IdCodificado + "', '" + Eve.cert_secuencia + "')\">Ver Certificado</span>";
                                    }
                                   
                                    v_detalle = v_detalle + "</div>";
                                    v_detalle = v_detalle + "</td>";
                                    v_detalle = v_detalle + "<td width='254' valign='top'>";
                                    v_detalle = v_detalle + "<div class='tcomand'>";

                                    if (Eve.categoria.Equals("EXPRT"))
                                    {
                                        v_detalle = v_detalle + "<span class='btn btn-secondary ml-2' onclick=\"descarga('" + IdCodificado + "','" + Eve.cert_secuencia + "')\">Descargar Certificado</span>";
                                    }
                                    else
                                    {
                                        v_detalle = v_detalle + "<span class='btn btn-secondary ml-2' onclick=\"descarga_impo('" + IdCodificado + "','" + Eve.cert_secuencia + "')\">Descargar Certificado</span>";
                                    }
                                       
                                    v_detalle = v_detalle + "</div>";
                                    v_detalle = v_detalle + "</td>";
                                    v_detalle = v_detalle + "</table>";
                                    v_detalle = v_detalle + "</p>";
                                   
                                }
                                //fin carbono neutro

                                //imagenes
                                if(!string.IsNullOrEmpty(Eve.Photo1) || !string.IsNullOrEmpty(Eve.Photo2) || !string.IsNullOrEmpty(Eve.Photo3) || !string.IsNullOrEmpty(Eve.Photo4))
                                {
                                    v_detalle = v_detalle + "<div class=\"row\">";
                                    if (!string.IsNullOrEmpty(Eve.Photo1))
                                    {
                                        v_detalle = v_detalle + "<div class=\"col-sm-5\"><img src=\"" + Eve.Photo1 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></div>";


                                        /*byte[] Foto;
                                        if (File.Exists(Eve.Photo1))
                                        {
                                            //PROCESO DE CONVERSION
                                            Foto = File.ReadAllBytes(Eve.Photo1);

                                            var foto1 = "";
                                            foto1 = Convert.ToBase64String(Foto, 0, Foto.Length);
                                            v_detalle = v_detalle + "<div class=\"col-sm-5\"><a data-remote=\"data:image/jpeg;base64, " + Eve.Photo1 + "\" data-toggle=\"lightbox\" data-title=\"Fotos\" data-gallery=\"gallery\"><img src=\"data:image/jpeg, " + Eve.Photo1 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></a></div>";
                                        }*/

                                        
                                    }

                                    if (!string.IsNullOrEmpty(Eve.Photo2))
                                    {
                                        v_detalle = v_detalle + "<div class=\"col-sm-5\"><img src=\"" + Eve.Photo2 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></div>";
                                        /*
                                        byte[] Foto;
                                        if (File.Exists(Eve.Photo2))
                                        {
                                            //PROCESO DE CONVERSION
                                            Foto = File.ReadAllBytes(Eve.Photo2);

                                            var foto1 = "";
                                            foto1 = Convert.ToBase64String(Foto, 0, Foto.Length);
                                            v_detalle = v_detalle + "<div class=\"col-sm-5\"><a data-remote=\"data:image/jpeg;base64, " + foto1 + "\" data-toggle=\"lightbox\" data-title=\"Fotos\" data-gallery=\"gallery\"><img src=\"data:image/jpeg;base64, " + foto1 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></a></div>";
                                        }*/


                                    }

                                    if (!string.IsNullOrEmpty(Eve.Photo3))
                                    {
                                        v_detalle = v_detalle + "<div class=\"col-sm-5\"><img src=\"" + Eve.Photo3 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></div>";
                                        /*
                                        byte[] Foto;
                                        if (File.Exists(Eve.Photo3))
                                        {
                                            //PROCESO DE CONVERSION
                                            Foto = File.ReadAllBytes(Eve.Photo3);

                                            var foto1 = "";
                                            foto1 = Convert.ToBase64String(Foto, 0, Foto.Length);
                                            v_detalle = v_detalle + "<div class=\"col-sm-5\"><a data-remote=\"data:image/jpeg;base64, " + foto1 + "\" data-toggle=\"lightbox\" data-title=\"Fotos\" data-gallery=\"gallery\"><img src=\"data:image/jpeg;base64, " + foto1 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></a></div>";
                                        }
                                        */

                                    }

                                    if (!string.IsNullOrEmpty(Eve.Photo4))
                                    {
                                        v_detalle = v_detalle + "<div class=\"col-sm-5\"><img src=\"" + Eve.Photo4 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></div>";
                                        /*
                                        byte[] Foto;
                                        if (File.Exists(Eve.Photo4))
                                        {
                                            //PROCESO DE CONVERSION
                                            Foto = File.ReadAllBytes(Eve.Photo4);

                                            var foto1 = "";
                                            foto1 = Convert.ToBase64String(Foto, 0, Foto.Length);
                                            v_detalle = v_detalle + "<div class=\"col-sm-5\"><a data-remote=\"data:image/jpeg;base64, " + foto1 + "\" data-toggle=\"lightbox\" data-title=\"Fotos\" data-gallery=\"gallery\"><img src=\"data:image/jpeg;base64, " + foto1 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></a></div>";
                                        }
                                        */

                                    }


                                    v_detalle = v_detalle + "</div>";
                                }
                               
                                //fin de imagenes


                                v_detalle = v_detalle + "</div>";
                                v_detalle = v_detalle + "</div>";
                                v_detalle = v_detalle + "</div></div></div></p>";
                                v_detalle = v_detalle.Replace("{X2}", Eve.idNotification.ToString());

                            }
                        }

                       
                        string HtmlTabla = string.Format("<span style='color:#000000;font-weight:bold;'>{0}</span>      ||    <span style='color:#999999;font-weight:bold;'>Categoría: </span><span style='color:#000000;font-weight:bold;'>{1}</span>", Det.Container?.ToUpper() , Det.categoria?.ToUpper()) ;
                       

                        v_cuerpo = v_cuerpo + @"
                                                    <div class='card'>
                                                        <div class='card-header' id='heading{X1}'>
                                                            <h2 class='mb-0'>
                                                                <button type = 'button' class='btn btn-link text-red' data-toggle='collapse' data-target='#collapse{X1}'><i class='fa fa-chevron-right'></i> <span style='color:#999999;font-weight:bold;'>Número de Carga: </span> " + string.Format("{0}", HtmlTabla) + @"</button>
                                                            </h2>
                                                        </div>
                                                        <div id = 'collapse{X1}' class='collapse' aria-labelledby='heading{X1}' data-parent='#accordionExample'>
                                                            <div class='card-body'>
                                                                " + v_detalle + @"
                                                            </div>
                                                        </div>
                                                    </div>
                                                    ";


                        v_cuerpo = v_cuerpo.Replace("{X1}", Det.UnitGkey.ToString());
                    }

                }

                else
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existen eventos de trazabilidad de su carga, con los criterios de búsquedas ingresados. {0}", cMensajes));
                    xfinder.Visible = false;
                    return;
                }

                if (TablaCasos.Count <= 0)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existen eventos de trazabilidad de su carga, con los criterios de búsquedas ingresados. {0}", cMensajes));
                    xfinder.Visible = false;
                    return;
                }


                v_html = v_html + v_cuerpo;
                v_html = v_html + @" </div>
                            </div>";
                tab.Append(v_html);
                this.htmlcasos.InnerHtml = tab.ToString();
               
                xfinder.Visible = true;
              
                this.Actualiza_Panele_Detalle();
                this.Ocultar_Mensaje();

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Inicial_Contenedores), "Carga_Inicial_Contenedores", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

              
                this.Mostrar_Mensaje(string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {

                    banmsg.Visible = false;
                }

                this.TxtFechaDesde.Text = Server.HtmlEncode(this.TxtFechaDesde.Text);
                this.txtFechaHasta.Text = Server.HtmlEncode(this.txtFechaHasta.Text);

                if (!Page.IsPostBack)
                {
                    ClsUsuario = Page.Tracker();
                    if (ClsUsuario == null)
                    {
                        return;
                    }

                    //Session["Registros"] = int.Parse("0");
                    //Session["Contador"] = int.Parse("0");

                    string desde = DateTime.Today.Month.ToString("D2") + "/01/" + DateTime.Today.Year.ToString();
                    string hasta = DateTime.Today.Month.ToString("D2") + "/01/" + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;
                    DateTime fhasta;

                    if (!DateTime.TryParseExact(desde, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha desde"));
                        return;
                    }

                    if (!DateTime.TryParseExact(hasta, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fhasta))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha hasta"));
                        return;
                    }
                    int v_dias = int.Parse(Cls_Stc_configuracion.obtenerConfiguracion("stcDayFirst", out cMensajes).FirstOrDefault().c_valor);
                    DateTime FechaInicio = DateTime.Today.AddDays(-v_dias);
                    this.TxtFechaDesde.Text = FechaInicio.ToString("dd/MM/yyyy");
                    this.txtFechaHasta.Text = DateTime.Today.ToString("dd/MM/yyyy");

                    //var TablaUser = Cls_Stc_evento.Listado_Evento("IMPO",out cMensajes);

                    //this.CboEventos.DataSource = TablaUser;
                    //this.CboEventos.DataTextField = "ev_descripcion";
                    //this.CboEventos.DataValueField = "dex_row_id";
                    //this.CboEventos.DataBind();


                    //BtnBuscar_Click(sender, e);
                    //PONER NUEVO SP PARA 
                    this.Carga_Inicial_Contenedores();
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            //Session["Contador"] = 0;
            //Session["Registros"] = 0;
            //Session["Resultado"] = null;

            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtFechaDesde.Text))
                    {

                        xfinder.Visible = false;

                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha inicial"));
                        this.TxtFechaDesde.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtFechaHasta.Text))
                    {

                        xfinder.Visible = false;

                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha final"));
                        this.TxtFechaDesde.Focus();
                        return;
                    }

                    CultureInfo enUS = new CultureInfo("en-US");

                    if (!string.IsNullOrEmpty(TxtFechaDesde.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaDesde.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha inicial debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text));
                            this.TxtFechaDesde.Focus();
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(txtFechaHasta.Text))
                    {
                        if (!DateTime.TryParseExact(txtFechaHasta.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha final debe ser: dia/Mes/Anio {0}", txtFechaHasta.Text));
                            this.txtFechaHasta.Focus();
                            return;
                        }
                    }

                    TimeSpan tsDias = fechahasta - fechadesde;
                    int diferenciaEnDias = tsDias.Days;
                    if (diferenciaEnDias < 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>La Fecha de Ingreso: {0} No deber ser mayor a la Fecha final: {1}", TxtFechaDesde.Text, TxtFechaDesde.Text));
                        return;
                    }
                    if (diferenciaEnDias > 365)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar informacion de hasta un año."));
                        return;
                    }

                   

                    usuario ClsUsuario = null;
                    ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    StringBuilder tab = new StringBuilder();

                    string v_cuerpo = string.Empty;
                    string v_detalle = string.Empty;
                    string v_html = @"  <div class='bs-example'>
                                            <div class='accordion' id='accordionExample'>";

                   // string cRuc = "0992500913001";

                    List<Lista_Contenedores> TablaCasos = Lista_Contenedores.Listado_Contenedores_Filtro(ClsUsuario.ruc, txtcontenedor.Text.Trim(), fechadesde, fechahasta, out OError);
                    if (TablaCasos != null)
                    {
                        foreach (var Det in TablaCasos)
                        {
                            v_detalle = string.Empty;

                            List<Lista_Contenedores> TablaEventos = Lista_Contenedores.Listado_Eventos_Contenedores(ClsUsuario.ruc, (Det.UnitGkey.HasValue ? Det.UnitGkey.Value : 0), out OError);
                            if (TablaEventos != null)
                            {
                                foreach (var Eve in TablaEventos)
                                {
                                 
                                    v_detalle = v_detalle + @"<p><div class='bs-example'><div class='accordion' id='accordionExampledet'><div class='card'><div class='card-header' id='heading{X2}'><h2 class='mb-0'>";
                                    v_detalle = v_detalle + "<button type = 'button' class='btn btn-link text-red' data-toggle='collapse' data-target='#collapse{X2}'><i class='fa fa-chevron-right'></i><span style='color:#999999;font-weight:bold;'>" + Eve.Name + "</span></button></h2></div>";
                                    v_detalle = v_detalle + "<div id = 'collapse{X2}' class='collapse' aria-labelledby='heading{X2}' data-parent='#accordionExampledet'>";
                                    v_detalle = v_detalle + "<div class='card-body'>";
                                    v_detalle = v_detalle + Eve.cuerpo;

                                    //caerbono neutro exportaciones
                                    if (Eve.id_certificado_carbo.HasValue && !string.IsNullOrEmpty(Eve.cert_secuencia))
                                    {
                                        string IdCodificado = securetext(Eve.id_certificado_carbo.Value);

                                        //botones de carbono neutro
                                        v_detalle = v_detalle + "<br/><p>";
                                        v_detalle = v_detalle + "<table cellpadding='0' cellspacing='1' border='0'>";
                                        v_detalle = v_detalle + "<tr>";
                                        v_detalle = v_detalle + "<td width='254' height='19' valign='top'>";
                                        v_detalle = v_detalle + "<div class='tcomand'>";

                                        if (Eve.categoria.Equals("EXPRT"))
                                        {
                                            v_detalle = v_detalle + "<span class='btn btn-link text-red' onclick=\"mostrar('" + IdCodificado + "', '" + Eve.cert_secuencia + "')\">Ver Certificado</span>";
                                        }
                                        else
                                        {
                                            v_detalle = v_detalle + "<span class='btn btn-link text-red' onclick=\"mostrar_impo('" + IdCodificado + "', '" + Eve.cert_secuencia + "')\">Ver Certificado</span>";
                                        }

                                        v_detalle = v_detalle + "</div>";
                                        v_detalle = v_detalle + "</td>";
                                        v_detalle = v_detalle + "<td width='254' valign='top'>";
                                        v_detalle = v_detalle + "<div class='tcomand'>";

                                        if (Eve.categoria.Equals("EXPRT"))
                                        {
                                            v_detalle = v_detalle + "<span class='btn btn-secondary ml-2' onclick=\"descarga('" + IdCodificado + "','" + Eve.cert_secuencia + "')\">Descargar Certificado</span>";
                                        }
                                        else
                                        {
                                            v_detalle = v_detalle + "<span class='btn btn-secondary ml-2' onclick=\"descarga_impo('" + IdCodificado + "','" + Eve.cert_secuencia + "')\">Descargar Certificado</span>";
                                        }

                                        v_detalle = v_detalle + "</div>";
                                        v_detalle = v_detalle + "</td>";
                                        v_detalle = v_detalle + "</table>";
                                        v_detalle = v_detalle + "</p>";

                                    }
                                    //fin carbono neutro

                                    //imagenes
                                    if (!string.IsNullOrEmpty(Eve.Photo1) || !string.IsNullOrEmpty(Eve.Photo2) || !string.IsNullOrEmpty(Eve.Photo3) || !string.IsNullOrEmpty(Eve.Photo4))
                                    {
                                        v_detalle = v_detalle + "<div class=\"row\">";
                                        if (!string.IsNullOrEmpty(Eve.Photo1))
                                        {
                                            v_detalle = v_detalle + "<div class=\"col-sm-5\"><img src=\"" + Eve.Photo1 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></div>";

                                        }

                                        if (!string.IsNullOrEmpty(Eve.Photo2))
                                        {
                                            v_detalle = v_detalle + "<div class=\"col-sm-5\"><img src=\"" + Eve.Photo2 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></div>";
                                           
                                        }

                                        if (!string.IsNullOrEmpty(Eve.Photo3))
                                        {
                                            v_detalle = v_detalle + "<div class=\"col-sm-5\"><img src=\"" + Eve.Photo3 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></div>";

                                        }

                                        if (!string.IsNullOrEmpty(Eve.Photo4))
                                        {
                                            v_detalle = v_detalle + "<div class=\"col-sm-5\"><img src=\"" + Eve.Photo4 + "\" class=\"img-fluid mb-2\" alt=\"CGSApp\" /></div>";
                                            
                                        }


                                        v_detalle = v_detalle + "</div>";
                                    }

                                    //fin de imagenes

                                    v_detalle = v_detalle + "</div>";
                                    v_detalle = v_detalle + "</div>";
                                    v_detalle = v_detalle + "</div></div></div></p>";
                                    v_detalle = v_detalle.Replace("{X2}", Eve.idNotification.ToString());
                                }
                            }

                          

                            string HtmlTabla = string.Format("<span style='color:#000000;font-weight:bold;'>{0}</span>      ||    <span style='color:#999999;font-weight:bold;'>Categoría: </span><span style='color:#000000;font-weight:bold;'>{1}</span>", Det.Container?.ToUpper(), Det.categoria?.ToUpper());


                            v_cuerpo = v_cuerpo + @"
                                                    <div class='card'>
                                                        <div class='card-header' id='heading{X1}'>
                                                            <h2 class='mb-0'>
                                                                <button type = 'button' class='btn btn-link text-red' data-toggle='collapse' data-target='#collapse{X1}'><i class='fa fa-chevron-right'></i> <span style='color:#999999;font-weight:bold;'>Número de Carga: </span> " + string.Format("{0}", HtmlTabla) + @"</button>
                                                            </h2>
                                                        </div>
                                                        <div id = 'collapse{X1}' class='collapse' aria-labelledby='heading{X1}' data-parent='#accordionExample'>
                                                            <div class='card-body'>
                                                                " + v_detalle + @"
                                                            </div>
                                                        </div>
                                                    </div>
                                                    ";


                            v_cuerpo = v_cuerpo.Replace("{X1}", Det.UnitGkey.ToString());
                        }

                    }
                    else
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existen eventos de trazabilidad de su carga, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        xfinder.Visible = false;
                        return;
                    }

                    if (TablaCasos.Count <= 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existen eventos de trazabilidad de su carga, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        xfinder.Visible = false;
                        return;
                    }

                   

                    v_html = v_html + v_cuerpo;
                    v_html = v_html +  @" </div>
                            </div>";
                    tab.Append(v_html);
                    this.htmlcasos.InnerHtml = tab.ToString();
                   
                    xfinder.Visible = true;
                  
                    this.Actualiza_Panele_Detalle();
                    this.Ocultar_Mensaje();
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));

                }
            }
        }

        protected void BtnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(Session["Contador"].ToString()) == int.Parse(Session["Registros"].ToString()))
                {
                    return;
                }

                StringBuilder tab = new StringBuilder();

                int v_contadorRegistrado = 0;
                int v_contadorActual = 0;
                int v_contador = int.Parse(Session["Contador"].ToString());
                string v_cuerpo = string.Empty;
                string v_detalle = string.Empty;
                string v_html = @"  <div class='bs-example'>
                                            <div class='accordion' id='accordionExample'>";

                var TablaCasos = Session["Resultado"] as List<Cls_Stc_ListaNotificaciones>;
                foreach (var Det in TablaCasos)
                {
                    v_contadorActual = v_contadorActual + 1;
                    if (v_contador >= v_contadorActual)
                    {
                        continue;
                    }



                    Session["Contador"] = int.Parse(Session["Contador"].ToString()) + 1;
///////                    v_detalle = "<div class='room-box'>";
                    //v_detalle = v_detalle + "<h5 class='text-primary'>" + Det.ms_asunto + "</h5>";
                    v_detalle = string.Empty;
                    v_detalle = v_detalle + "<p>" + Det.ms_mensaje + "</p>";
                    v_detalle = v_detalle + "<p><span class='text-muted'>Tipo Carga :</span> " + Det.tipoCarga + " <br/> <span class='text-muted'>Fecha Registro :</span> " + Det.fecha_reg.Value.ToString("dd/MM/yyyy hh:mm");
                    //v_detalle = v_detalle + " &nbsp &nbsp &nbsp &nbsp <asp:Button runat='server' ID='IncreaseButton{X1}' Text='Historial' class='btn btn-secondary' data-toggle='modal' data-target='#myModal2'> Ver Imagenes </asp:Button> </p>";

                    //nuevo 17-02-2020 jalvarado
                    if (Det.tipoNotificacion.Equals("AFOROCOMPLETO"))
                    {
                        v_detalle = v_detalle + " &nbsp &nbsp &nbsp &nbsp <input type='button' name='e' value='Ver Imágenes' id='{X1}'  class='btn btn-secondary' data-toggle='modal' data-target='#myModal2'  onclick=clickaction(this)>";
                    }
                    else
                    {
                        v_detalle = v_detalle + " &nbsp &nbsp &nbsp &nbsp <input type='button' name='e' value='Ver Imágenes' id='{X1}' style='display: none;' class='btn btn-secondary' data-toggle='modal' data-target='#myModal2'  onclick=clickaction(this)>";
                    }

                    // v_detalle = v_detalle + " &nbsp &nbsp &nbsp &nbsp <input type='button' name='e' value='Ver Imágenes' id='{X1}' class='btn btn-secondary' data-toggle='modal' data-target='#myModal2'  onclick=clickaction(this)>";

                    // <button type = 'button' class='btn btn-link' data-toggle='collapse' data-target='#collapse{X1}'><i class='fa fa-play'></i> " + string.Format("{0} | {1} | {2} | {3}:{4}-{5}-{6} {7}", Det.categoria.ToUpper(), Det.tipoCarga.ToUpper(), Det.fechaEvento.ToString().Substring(0, 16), Det.tituloNotificacion.ToUpper(), Det.mrn.ToUpper(), Det.msn.ToUpper(), Det.hsn.ToUpper(), Det.contenedor.ToUpper()) + @"</button>

                    string barra = @"\";
                    v_cuerpo = v_cuerpo + @"
                                                <div class='card'>
                                                    <div class='card-header' id='heading{X1}'>
                                                        <h2 class='mb-0'>
                                                            <button type = 'button' class='btn btn-link text-black' data-toggle='collapse' data-target='#collapse{X1}'><i class='fa fa-plus'></i> " + string.Format("{0}  {1} {2}  {3} {4}  {5} {6}: {7}-{8}-{9}  {10} {11}", Det.categoria?.ToUpper(), barra, Det.tipoCarga?.ToUpper(), barra, Det.fechaEvento?.ToString("dd/MM/yyyy hh:mm"), barra, Det.tituloNotificacion?.ToUpper(), Det.mrn?.ToUpper(), Det.msn?.ToUpper(), Det.hsn?.ToUpper(), (!string.IsNullOrEmpty(Det.contenedor) ? barra: ""), Det.contenedor?.ToUpper()) + @"</button>
                                                        </h2>
                                                    </div>
                                                    <div id = 'collapse{X1}' class='collapse' aria-labelledby='heading{X1}' data-parent='#accordionExample'>
                                                        <div class='card-body'>
                                                            " + v_detalle + @"
                                                        </div>
                                                    </div>
                                                </div>
                                                ";
                    v_contadorRegistrado = v_contadorRegistrado + 1;
                    v_cuerpo = v_cuerpo.Replace("{X1}", Det.dex_row_id.ToString());

                    if (v_contadorRegistrado == v_RegistrosPorPagina)
                    {
                        break;
                    }
                }

                v_html = v_html + v_cuerpo;
                v_html = v_html + @" </div>
                            </div>";
                tab.Append(v_html);

                this.htmlcasos.InnerHtml = tab.ToString();

                //grilla.DataSource = TablaCasos;
                //grilla.DataBind();

                xfinder.Visible = true;
                //sinresultado.Visible = false;
               // this.lblContador.InnerHtml = string.Format("{0} Registros de {1}", Session["Contador"], Session["Registros"]);
                this.Actualiza_Panele_Detalle();
                this.Ocultar_Mensaje();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }

        protected void BtnAtras_Click(object sender, EventArgs e)
        {
            try
            {
                Session["Contador"] = 0;
                
                if (int.Parse(Session["Contador"].ToString()) == int.Parse(Session["Registros"].ToString()))
                {
                    return;
                }

                StringBuilder tab = new StringBuilder();

                int v_contadorRegistrado = 0;
                int v_contadorActual = 0;
                int v_contador = int.Parse(Session["Contador"].ToString());
                string v_cuerpo = string.Empty;
                string v_detalle = string.Empty;
                string v_html = @"  <div class='bs-example'>
                                            <div class='accordion' id='accordionExample'>";

                var TablaCasos = Session["Resultado"] as List<Cls_Stc_ListaNotificaciones>;
                foreach (var Det in TablaCasos)
                {
                    v_contadorActual = v_contadorActual + 1;
                    if (v_contador >= v_contadorActual)
                    {
                        continue;
                    }

                    Session["Contador"] = int.Parse(Session["Contador"].ToString()) + 1;
                    ///////v_detalle = "<div class='room-box'>";
                    //v_detalle = v_detalle + "<h5 class='text-primary'>" + Det.ms_asunto + "</h5>";
                    v_detalle = string.Empty;
                    v_detalle = v_detalle + "<p>" + Det.ms_mensaje + "</p>";
                    v_detalle = v_detalle + "<p><span class='text-muted'>Tipo Carga :</span> " + Det.tipoCarga + " <br/> <span class='text-muted'>Fecha Registro :</span> " + Det.fecha_reg.Value.ToString("dd/MM/yyyy hh:mm");
                    //v_detalle = v_detalle + " &nbsp &nbsp &nbsp &nbsp <asp:Button runat='server' ID='IncreaseButton{X1}' Text='Historial' class='btn btn-secondary' data-toggle='modal' data-target='#myModal2'> Ver Imagenes </asp:Button> </p>";

                    //nuevo 17-02-2020 jalvarado
                    if (Det.tipoNotificacion.Equals("AFOROCOMPLETO"))
                    {
                        v_detalle = v_detalle + " &nbsp &nbsp &nbsp &nbsp <input type='button' name='e' value='Ver Imágenes' id='{X1}'  class='btn btn-secondary' data-toggle='modal' data-target='#myModal2'  onclick=clickaction(this)>";
                    }
                    else
                    {
                        v_detalle = v_detalle + " &nbsp &nbsp &nbsp &nbsp <input type='button' name='e' value='Ver Imágenes' id='{X1}' style='display: none;' class='btn btn-secondary' data-toggle='modal' data-target='#myModal2'  onclick=clickaction(this)>";
                    }


                    // v_detalle = v_detalle + " &nbsp &nbsp &nbsp &nbsp <input type='button' name='e' value='Ver Imágenes' id='{X1}' class='btn btn-secondary' data-toggle='modal' data-target='#myModal2'  onclick=clickaction(this)>";
                    //<button type = 'button' class='btn btn-link' data-toggle='collapse' data-target='#collapse{X1}'><i class='fa fa-play'></i> " + string.Format("{0} | {1} | {2} | {3}:{4}-{5}-{6} {7}", Det.categoria.ToUpper(),Det.tipoCarga.ToUpper(),Det.fechaEvento.ToString().Substring(0, 16), Det.tituloNotificacion.ToUpper(), Det.mrn.ToUpper(), Det.msn.ToUpper(), Det.hsn.ToUpper(), Det.contenedor.ToUpper()) + @"</button>

                    string barra = @"\";
                    v_cuerpo = v_cuerpo + @"
                                                <div class='card'>
                                                    <div class='card-header' id='heading{X1}'>
                                                        <h2 class='mb-0'>
                                                        <button type = 'button' class='btn btn-link text-black' data-toggle='collapse' data-target='#collapse{X1}'><i class='fa fa-plus'></i> " + string.Format("{0}  {1} {2}  {3} {4}  {5} {6}: {7}-{8}-{9}  {10} {11}", Det.categoria?.ToUpper(), barra, Det.tipoCarga?.ToUpper(), barra, Det.fechaEvento?.ToString("dd/MM/yyyy hh:mm"), barra, Det.tituloNotificacion?.ToUpper(), Det.mrn?.ToUpper(), Det.msn?.ToUpper(), Det.hsn?.ToUpper(), (!string.IsNullOrEmpty(Det.contenedor) ? barra : ""), Det.contenedor?.ToUpper()) + @"</button>
                                                        </h2>
                                                    </div>
                                                    <div id = 'collapse{X1}' class='collapse' aria-labelledby='heading{X1}' data-parent='#accordionExample'>
                                                        <div class='card-body'>
                                                            " + v_detalle + @"
                                                        </div>
                                                    </div>
                                                </div>
                                                ";
                    v_contadorRegistrado = v_contadorRegistrado + 1;
                    v_cuerpo = v_cuerpo.Replace("{X1}", Det.dex_row_id.ToString());

                    if (v_contadorRegistrado == v_RegistrosPorPagina)
                    {
                        break;
                    }
                }

                v_html = v_html + v_cuerpo;
                v_html = v_html + @" </div>
                            </div>";

                tab.Append(v_html);
                this.htmlcasos.InnerHtml = tab.ToString();
                //grilla.DataSource = TablaCasos;
                //grilla.DataBind();
                xfinder.Visible = true;
                //sinresultado.Visible = false;

                //this.lblContador.InnerHtml = string.Format("{0} Registros de {1}", Session["Contador"], Session["Registros"]);
                this.Actualiza_Panele_Detalle();
                this.Ocultar_Mensaje();
                

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }

        protected void txtContainers_TextChanged(object sender, EventArgs e)
        {
            
        }

        protected void BtnCargarImagenes_Click(object sender, EventArgs e)
        {
            int v_contador = 0;
            //this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", txtContainers.Text));

            //se obtiene la información de la notificacion seleccionada
            var TablaCasos = Session["Resultado"] as List<Cls_Stc_ListaNotificaciones>;
            var NotificacionSeleccionada = TablaCasos.Where(a => a.dex_row_id == long.Parse(txtContainers.Text.ToString())).FirstOrDefault();

            //se obtiene la lista de imagenes de la notificación seleccionada
            var TablaImagenes = Cls_Stc_Imagen.obtenerImagenes(NotificacionSeleccionada.ruc, NotificacionSeleccionada.mrn, out cMensajes);

            string v_divImagenes = string.Empty;
            foreach (var Det in TablaImagenes)
            {
                if (v_contador == 0)
                {
                    v_contador = 1;
                    v_divImagenes += @"<div class='carousel-item active'>
                                                  <img src = '" + Det.imagen + @"' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                                <div class='carousel-caption d-none d-md-block'>
                                                    <!-- <h5>Second slide label</h5>
                                                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                                </div>
                                            </div> ";
                }
                else
                {
                    v_divImagenes += @"<div class='carousel-item'>
                                                <img src = '" + Det.imagen + @"' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                                <div class='carousel-caption d-none d-md-block'>
                                                    <!-- <h5>Second slide label</h5>
                                                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                                </div>
                                            </div> ";
                }
            }

            StringBuilder tab = new StringBuilder();

            string v_cuerpo = string.Empty;
            v_cuerpo  = v_cuerpo + @"<div class='mb-5'>
                                        <div id='carouselExampleCaptions' class='carousel slide' data-ride='carousel'>
                                        <ol class='carousel-indicators'>
                                            <li data-target='#carouselExampleCaptions' data-slide-to='0' class='active'></li>
                                            <li data-target='#carouselExampleCaptions' data-slide-to='1'></li>
                                            <li data-target='#carouselExampleCaptions' data-slide-to='2'></li>
                                            <li data-target='#carouselExampleCaptions' data-slide-to='3'></li>
                                        </ol>
                                        <div class='carousel-inner'>
                                           
                                            " + v_divImagenes +@"
                                        </div>
                                        <a class='carousel-control-prev' href='#carouselExampleCaptions' role='button' data-slide='prev'>
                                            <span class='carousel-control-prev-icon' aria-hidden='true'></span>
                                            <span class='sr-only'>Previous</span>
                                        </a>
                                        <a class='carousel-control-next' href='#carouselExampleCaptions' role='button' data-slide='next'>
                                            <span class='carousel-control-next-icon' aria-hidden='true'></span>
                                            <span class='sr-only'>Next</span>
                                        </a>
                                    </div>
                                </div>
                                ";


            string v_html = string.Empty;
            v_html = v_html + v_cuerpo;
            //v_html = v_html + @" </div>
            //                </div>";
            tab.Append(v_html);

            this.htmlImagenes.InnerHtml = tab.ToString();


            xfinde2.Visible = true;
            txtContainers.Visible = false;
            UPMODAL.Update();
        }

       

    }
}