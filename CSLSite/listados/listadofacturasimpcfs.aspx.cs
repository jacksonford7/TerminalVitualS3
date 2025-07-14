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


namespace CSLSite
{
    public partial class listadofacturasimpcfs : System.Web.UI.Page
    {
        #region "Clases"
        //Cls_Usuario ClsUsuario;
       
        #endregion

        #region "Variables"

        private string cMensajes;


        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();

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

#if !DEBUG
                this.IsAllowAccess();
#endif

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
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar las facturas de hasta un mes."));
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    RN_Bill_Invoice_Listado _obj = new RN_Bill_Invoice_Listado();
                    var TablaFactura = _obj.Entidad_Listado_Facturas_Impo_CFS(fechadesde, fechahasta, ClsUsuario.loginname, ClsUsuario.ruc, out cMensajes);

                    if (TablaFactura == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        return;
                    }
                    if (TablaFactura.Count <= 0)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados."));
                        return;
                    }

                    grilla.DataSource = TablaFactura;
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

        }
        //protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Cells[1].Text = String.Format("{0:c}", e.Row.Cells[8].Text);
        //        e.Row.Cells[3].Text = String.Format("{0:c}", e.Row.Cells[9].Text);
        //    }
        //}
    }
}