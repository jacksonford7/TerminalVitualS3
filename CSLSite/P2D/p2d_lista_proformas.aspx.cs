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


namespace CSLSite.backoffice
{
    public partial class p2d_lista_proformas : System.Web.UI.Page
    {
        #region "Clases"
        private P2D_Proforma_Cabecera objProformaCab = new P2D_Proforma_Cabecera();
        private P2D_Tiene_Factura objExiste = new P2D_Tiene_Factura();
        #endregion

        #region "Variables"

        private string cMensajes;


        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();

        #endregion


        #region "Metodos"

        protected string jsarguments(object id)
        {
            return string.Format("{0}", id != null ? id.ToString().Trim() : "0");
        }

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
            UPCARGA.Update();
           
        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
        }

        
        private void OcultarLoading()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader();", true);
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
                OcultarLoading();
           

            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg.Visible = false;   
            this.Actualiza_Paneles();
            OcultarLoading();

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

                    this.TxtFechaDesde.Text = fdesde.ToString("MM/dd/yyyy");
                    this.TxtFechaHasta.Text = DateTime.Today.ToString("MM/dd/yyyy");

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

        protected void chkFacturar_CheckedChanged(object sender, EventArgs e)
        {

            

        }
       
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            
            if (Response.IsClientConnected)
            {
                try
                {
                    
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading();
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
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar las proformas de hasta un mes."));
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    var TablaCotizacion = P2D_Lista_Proforma.Listado_Cotizaciones(ClsUsuario.ruc, fechadesde, fechahasta, out cMensajes);

                   
                    if (TablaCotizacion == null)
                    {
                        grilla.DataSource = null;
                        grilla.DataBind();

                        this.Actualiza_Paneles();
                 
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        return;
                    }
                    if (TablaCotizacion.Count <= 0)
                    {
                        grilla.DataSource = null;
                        grilla.DataBind();

                        this.Actualiza_Paneles();

                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados."));
                        return;
                    }

                    grilla.DataSource = TablaCotizacion;
                    grilla.DataBind();


                    this.Actualiza_Paneles();
                    this.Ocultar_Mensaje();
                   

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));                   
                    
                }
            }



            
        }



        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe cotización para realizar la anulación"));
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var xpars = e.CommandArgument.ToString();

                    //anula cotización
                    Int64 ID = 0;
                    if (!Int64.TryParse(xpars, out ID))
                    {
                        ID = 0;
                    }

                    //valida si tiene factura, no podra anular la proforma
                    objExiste = new P2D_Tiene_Factura();
                    objExiste.ID_PROFORMA = ID;
                    if (objExiste.PopulateMyData(out cMensajes))
                    {
                        this.Ocultar_Mensaje();
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b> Ya existe una factura emitida, para la proforma, no podrá anular la misma."));
                        return;
                       
                       
                    }


                    objProformaCab = new P2D_Proforma_Cabecera();
                    objProformaCab.ID_PROFORMA = ID;
                    objProformaCab.USUARIO_CREA = ClsUsuario.loginname;

                    objProformaCab.Delete(out cMensajes);
                    if (cMensajes != string.Empty)
                    {

                        this.Mostrar_Mensaje(string.Format("<b>Error! </b> {0}", cMensajes));
                        return;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Se inactivo cotización con éxito."));
                       
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
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar las facturas de hasta un mes."));
                        return;
                    }

                    //vuelve a cargar datos
                    var TablaCotizacion = P2D_Lista_Proforma.Listado_Cotizaciones(ClsUsuario.ruc, fechadesde, fechahasta, out cMensajes);
                    if (TablaCotizacion == null)
                    {
                        grilla.DataSource = TablaCotizacion;
                        grilla.DataBind();

                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        return;
                    }
                    if (TablaCotizacion.Count <= 0)
                    {
                        grilla.DataSource = TablaCotizacion;
                        grilla.DataBind();
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados."));
                        return;
                    }

                    grilla.DataSource = TablaCotizacion;
                    grilla.DataBind();


                    this.Actualiza_Paneles();
                    this.Ocultar_Mensaje();
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                   
                    string error = string.Format("Ha ocurrido un problema durante la actualizacion, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "listado_cotizaciones", "Item_comand", "Hubo un error al anular", t.loginname));
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", error));


                }
            }
        }
      
    }
}