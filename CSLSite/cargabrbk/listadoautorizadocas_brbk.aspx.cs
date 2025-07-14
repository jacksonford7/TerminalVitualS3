using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using BillionEntidades;
using N4;
using N4Ws;
using N4Ws.Entidad;
using System.Xml.Linq;
using System.Xml;
using BillionReglasNegocio;
using CSLSite;
using BreakBulk;

namespace CSLSite
{
    public partial class listadoautorizadocas_brbk : System.Web.UI.Page
    {
        #region "Clases"
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private List<Cls_Bil_CargaSuelta_Gkey> Lista { set; get; }
        usuario ClsUsuario;
        #endregion

        #region "Variables"

        private string cMensajes;

        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();
      

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

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
            UPCARGA.Update();
           
        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
            UPCARGA.Update();
        }

        
        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('"+valor+"');", true);
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

        //private void Crear_Sesion()
        //{
        //    var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;
        //    objSesion.USUARIO_CREA = ClsUsuario.Usuario;
        //    nSesion = objSesion.SaveTransaction(out cMensajes);
        //    if (!nSesion.HasValue || nSesion.Value <= 0)
        //    {
        //        this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
        //        return;
        //    }
        //    this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

        //    //cabecera de transaccion
        //    Lista = new List<Cls_Bil_CargaSuelta_Gkey>();
        //    Session["ConsultaAutCFS" + this.hf_BrowserWindowName.Value] = Lista;
        //}


        #endregion

        #region "Eventos del Repetidor"
   
        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

  
        protected void grilla_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
        }

        #endregion


        #region "Eventos del formulario"

    

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    string IdDesConsolidadora = string.Empty;

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
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha inicial"));
                        this.TxtFechaDesde.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TxtFechaHasta.Text))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha final"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    CultureInfo enUS = new CultureInfo("en-US");

                    if (!string.IsNullOrEmpty(TxtFechaDesde.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha inicial debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha final debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text));
                            this.TxtFechaHasta.Focus();
                            return;

                        }
                    }

                    TimeSpan tsDias = fechahasta - fechadesde;
                    int diferenciaEnDias = tsDias.Days;
                    if (diferenciaEnDias < 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>La Fecha de Ingreso: {0} \\nNO deber ser mayor a la\\nFecha final: {1}", TxtFechaDesde.Text, TxtFechaDesde.Text));
                        return;
                    }
                    if (diferenciaEnDias > 31)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar los pases de hasta un mes."));
                        return;
                    }

                    ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    /*var Desconsolidadora = N4.Entidades.Forwarder.ObtenerForwarderPorRUC(ClsUsuario.loginname, ClsUsuario.ruc);
                    if (Desconsolidadora.Exitoso)
                    {
                        var ListaDesconsolidadora = Desconsolidadora.Resultado;
                        if (ListaDesconsolidadora != null)
                        {
                            IdDesConsolidadora = ListaDesconsolidadora.CODIGO_FW;
                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se pudo obtener desconsolidadora relacionada a su usuario: {0} - {1}", ClsUsuario.loginname, Desconsolidadora.MensajeProblema));
                        return;
                    }*/

                    var Desconsolidadora = carrier.GetCurrier(ClsUsuario.ruc);
                   
                    if (Desconsolidadora != null)
                    {
                        var ListaDesconsolidadora = Desconsolidadora.carrier_id;
                        IdDesConsolidadora = ListaDesconsolidadora;

                    }
                    else
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se pudo obtener desconsolidadora relacionada a su usuario: {0} - {1}", ClsUsuario.loginname, Desconsolidadora.carrier_id));
                        return;
                    }


                    //IdDesConsolidadora = "09909182";
                    var TablaAutorizacion = Cls_Bil_CasManual.Listado_Autorizaciones_Brbk(fechadesde, fechahasta, IdDesConsolidadora, out cMensajes);
                    if (TablaAutorizacion == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        return;
                    }
                    if (TablaAutorizacion.Count <= 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados."));
                        return;
                    }

                    grilla.DataSource = TablaAutorizacion;
                    grilla.DataBind();

                   

                    this.Actualiza_Panele_Detalle();

                    this.Ocultar_Mensaje();


                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));

                }
            }




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

            this.banmsg.Visible = IsPostBack;
          
            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
              
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Response.IsClientConnected)
                {
                   
                    //banmsg.Visible = false;
                }

                this.TxtFechaDesde.Text = Server.HtmlEncode(this.TxtFechaDesde.Text);
                this.TxtFechaHasta.Text = Server.HtmlEncode(this.TxtFechaHasta.Text);

                if (!Page.IsPostBack)
                {

                   

                    string desde = DateTime.Today.Month.ToString("D2") + "/01/"  + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha desde"));
                        return;
                    }

                    this.TxtFechaDesde.Text = DateTime.Today.ToString("MM/dd/yyyy");//fdesde.ToString("MM/dd/yyyy");
                    this.TxtFechaHasta.Text = DateTime.Today.ToString("MM/dd/yyyy");
                    //this.Crear_Sesion();
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }


       
    
       
    }
}