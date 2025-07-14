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

using System.Data;

using System.Reflection;
using System.ComponentModel;

namespace CSLSite
{
    public partial class rpt_listadoplanificaciones : System.Web.UI.Page
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

        private void Carga_CboBodega()
        {
            try
            {
                List<brbk_depositos> Listado = brbk_depositos.CboBodega(true, out cMensajes);

                this.CboBodega.DataSource = Listado;
                this.CboBodega.DataTextField = "DESCRIPCION";
                this.CboBodega.DataValueField = "CODIGO";
                this.CboBodega.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "brbk_mantenimiento_turnos", "Carga_CboBodega", "Hubo un error al cargar bodegas", t.loginname));
                this.Mostrar_Mensaje(Error);
            }
        }

        private void Carga_CboTipoProducto()
        {
            try
            {
                List<brbk_tipoproducto> Listado = brbk_tipoproducto.CboTipoProducto(true, out cMensajes);

                this.CboTipoProducto.DataSource = Listado;
                this.CboTipoProducto.DataTextField = "DESCRIPCION";
                this.CboTipoProducto.DataValueField = "CODIGO";
                this.CboTipoProducto.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "brbk_mantenimiento_turnos", "Carga_CboTipoProducto", "Hubo un error al cargar tipo productos", t.loginname));
                this.Mostrar_Mensaje(Error);
            }
        }


        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable table = new DataTable();

            try
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
                return table;

            }
            catch (Exception)
            {
                throw;
            }

        }

        protected string jsarguments(object id)
        {
            return string.Format("{0}", id != null ? id.ToString().Trim() : "0");
        }

        private void Actualiza_Paneles()
        {
            UPCARGA.Update();  
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

                    this.Carga_CboBodega();

                    this.Carga_CboTipoProducto();

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
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Por favor seleccione la fecha FINAL"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (this.CboTipoProducto.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor seleccionar el tipo de producto"));
                        this.CboTipoProducto.Focus();
                        return;
                    }

                    if (this.CboBodega.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor seleccionar la bodega"));
                        this.CboBodega.Focus();
                        return;
                    }



                    CultureInfo enUS = new CultureInfo("en-US");
                   
                    if (!string.IsNullOrEmpty(TxtFechaDesde.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaDesde.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha inicial debe ser: dia/Mes/Anio {0}", TxtFechaDesde.Text));
                            this.TxtFechaDesde.Focus();
                            return;
                        }
                    }


                    if (!string.IsNullOrEmpty(this.TxtFechaHasta.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha final debe ser: dia/Mes/Anio {0}", TxtFechaHasta.Text));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                    }


                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                   

                    List<brbk_rpt_planificacion_despachos> TablaCotizacion = brbk_rpt_planificacion_despachos.Listado_Solicitudes(Int64.Parse(this.CboTipoProducto.SelectedValue.ToString()), fechadesde, fechahasta,this.CboBodega.SelectedValue.ToString(), out cMensajes); ;

                    if (TablaCotizacion == null)
                    {
                     

                        this.Actualiza_Paneles();
                 
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        return;
                    }
                    if (TablaCotizacion.Count <= 0)
                    {

                        this.Actualiza_Paneles();

                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados."));
                        return;
                    }

                    DataTable tb = ConvertToDataTable(TablaCotizacion);

                    if (tb.Rows.Count > 0)
                    {
                        string fname = string.Format("DESPACHOS{0}", DateTime.Now.ToString("ddMMyyyHHmmss"));

                        Session["DESPACHOS"] = tb;

                        string llamada = string.Format("'descarga(\"{0}\",\"{1}\",\"{2}\");'", fname, "DESPACHOS", "DESPACHOS");
                        banmsg.InnerHtml = string.Format("Se ha generado el archivo {0}.xlsx, con {1} filas<br/><a class='btn btn-link' href='#' onclick={2} >Clic Aquí para descargarlo</a>", fname, tb.Rows.Count, llamada);
                        banmsg.Visible = true;
                    }
                    else
                    {

                        banmsg.InnerHtml = "No existen registros para exportar";
                        banmsg.Visible = true;
                    }


             

                    this.Actualiza_Paneles();
                    //this.Ocultar_Mensaje();
                   

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));                   
                    
                }
            }



            
        }


      
    }
}