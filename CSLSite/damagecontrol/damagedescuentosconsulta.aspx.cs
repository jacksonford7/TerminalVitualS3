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
using ControlPagos.Importacion;
using Salesforces;
using System.Data;
using System.Net;
using SqlConexion;
using CasManual;


using System.Reflection;
using System.ComponentModel;

namespace CSLSite
{


    public partial class damagedescuentosconsulta : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;
       
        private string cMensajes;

        private Damage_Descuentos_Cab objCab = new Damage_Descuentos_Cab();
        private Damage_Descuentos_Det objDet = new Damage_Descuentos_Det();

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private DateTime fechadesde = new DateTime();
        private DateTime fechahasta = new DateTime();

        private string LoginName = string.Empty;
        

        private static Int64? lm = -3;
        private string OError;

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
           
            this.UPCARGA.Update();
            this.UPDETTURNOS.Update();
            this.UPTITULO.Update();
            this.UPMENSAJE.Update();
        }


        private void Cargar_Descuentos_Pendientes_N2()
        {
            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Tabla = Damage_Descuentos_Cab.Descuentos_Pendientes_Aprobar_Gerente(out cMensajes);
                if (Tabla == null)
                {
                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de descuentos pendientes. {0}", cMensajes));
                    return;
                }
                if (Tabla.Count <= 0)
                {
                    grilla.DataSource = null;
                    grilla.DataBind();

                    this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de descuentos pendientes."));
                    return;
                }

                grilla.DataSource = Tabla;
                grilla.DataBind();

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Cargar_Descuentos_Pendientes_N2), "damagedescuentosnivel2", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));



            }
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
              
            
           
            this.Actualiza_Paneles();
        }

        private void Mostrar_Mensaje_Pendiente(string Mensaje)
        {

            this.banmsg2.Visible = true;
            this.banmsg2.InnerHtml = Mensaje;
         
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;          
            this.banmsg.Visible = false;

            this.banmsg2.InnerText = string.Empty;
            this.banmsg2.Visible = false;

            this.Actualiza_Paneles();

        }

      
        

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

            //cabecera de transaccion
            objCab = new Damage_Descuentos_Cab();           
            Session["Descuentos" + this.hf_BrowserWindowName.Value] = objCab;
            
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
            this.banmsg2.Visible = IsPostBack;


            if (!Page.IsPostBack)
            {

                this.banmsg.InnerText = string.Empty;
                this.banmsg2.InnerText = string.Empty;

                ClsUsuario = Page.Tracker();
                
            }

        }

       

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Server.HtmlEncode(this.TxtNumeroPolitica.Text.Trim());
                Server.HtmlEncode(this.TxtFecha.Text.Trim());
                Server.HtmlEncode(this.TxtFechaDescuentoDesde.Text.Trim());
                Server.HtmlEncode(this.TxtFechaDescuentoHasta.Text.Trim());
                Server.HtmlEncode(this.TxtMotivo.Text.Trim());
                Server.HtmlEncode(this.TxtElaborado.Text.Trim());
                Server.HtmlEncode(this.TxtFechaCrea.Text.Trim());
                this.TxtFechaDesde.Text = Server.HtmlEncode(this.TxtFechaDesde.Text);
                this.TxtFechaHasta.Text = Server.HtmlEncode(this.TxtFechaHasta.Text);

                if (!Page.IsPostBack)
                {
                    string desde = DateTime.Today.Month.ToString("D2") + "/01/" + DateTime.Today.Year.ToString();

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fdesde))
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha desde"));
                        return;
                    }

                    this.TxtFechaDesde.Text = fdesde.ToString("MM/dd/yyyy");
                    this.TxtFechaHasta.Text = DateTime.Today.ToString("MM/dd/yyyy");

                    this.CboEstadoConsulta.SelectedIndex = 0;

                    this.Crear_Sesion();

                   

                }
                else
                {
                  

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}",ex.Message));
            }
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
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha inicial debe ser: Mes/día/Año {0}", TxtFechaDesde.Text));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        if (!DateTime.TryParseExact(TxtFechaHasta.Text, "MM/dd/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                        {
                            this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>El formato de la fecha final debe ser: Mes/día/Año {0}", TxtFechaDesde.Text));
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
                    if (diferenciaEnDias > 365)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>Solo puede consultar las facturas de hasta un año."));
                        return;
                    }

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var Tabla = Damage_Descuentos_Cab.Descuentos_Consultas(fechadesde, fechahasta, this.CboEstadoConsulta.SelectedValue.ToString() ,out cMensajes);
                    if (Tabla == null)
                    {
                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de descuentos con los criterios ingresados. {0}", cMensajes));
                        return;
                    }
                    if (Tabla.Count <= 0)
                    {
                        grilla.DataSource = null;
                        grilla.DataBind();

                        this.Mostrar_Mensaje(string.Format("<b>Informativo! </b>No existe información que mostrar, de descuentos con los criterios ingresados."));
                        return;
                    }

                    grilla.DataSource = Tabla;
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

        protected void grilla_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    this.Ocultar_Mensaje();
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje_Pendiente( string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje_Pendiente(string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Ver")
                    {
                        Int64 DESC_ID = Int64.Parse(t.ToString());
                        objCab = new Damage_Descuentos_Cab();
                        objCab.DESC_ID = DESC_ID;
                        if (!objCab.PopulateMyData(out OError))
                        {

                            return;
                        }
                        else
                        {

                            this.TxtNumeroPolitica.Text = objCab.DESC_ID.ToString();
                            this.TxtFecha.Text = objCab.DESC_DATE_CREA.Value.ToString("dd/MM/yyyy");
                            this.TxtFechaDescuentoDesde.Text = objCab.DESC_DESDE.Value.ToString("dd/MM/yyyy HH:mm");
                            this.TxtFechaDescuentoHasta.Text = objCab.DESC_HASTA.Value.ToString("dd/MM/yyyy HH:mm");
                            this.TxtMotivo.Text = objCab.DESC_NOTA.ToString();
                            this.CboEstados.SelectedValue = objCab.DESC_ESTADO.ToString();
                            this.TxtElaborado.Text = objCab.DESC_USER_CREA.ToString();
                            this.TxtFechaCrea.Text = objCab.DESC_DATE_CREA.Value.ToString("dd/MM/yyyy HH:mm");


                            this.CboEstados.Attributes["disabled"] = "disabled";
                            this.TxtNumeroPolitica.Attributes["disabled"] = "disabled";
                            this.TxtFecha.Attributes["disabled"] = "disabled";
                            this.TxtFechaDescuentoDesde.Attributes["disabled"] = "disabled";
                            this.TxtFechaDescuentoHasta.Attributes["disabled"] = "disabled";

                            tablePagination.DataSource = objCab.Detalle;
                            tablePagination.DataBind();

                            Session["Descuentos" + this.hf_BrowserWindowName.Value] = objCab;

                            this.Actualiza_Paneles();
                        }

                    }
                  


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(grilla_ItemCommand), "grilla_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                    

                }
            }

        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {

            try
            {
                
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnNuevo_Click), "brbk_generar_turnos", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }

        }

        protected void BtnExcel_Click(object sender, EventArgs e)
        {

            try
            {
                objCab = Session["Descuentos" + this.hf_BrowserWindowName.Value] as Damage_Descuentos_Cab;

                List<Damage_Descuentos_Det> lst = objCab.Detalle;
                DataTable tb = ConvertToDataTable(lst);

                if (tb.Rows.Count > 0)
                {
                    string fname = string.Format("POLITICA_DESCUENTO_{0}", DateTime.Now.ToString("ddMMyyyHHmmss"));

                    Session["politicas"] = tb;

                    string llamada = string.Format("'descarga(\"{0}\",\"{1}\",\"{2}\");'", fname, "politicas", "politicas");
                    banmsg2.InnerHtml = string.Format("Se ha generado el archivo {0}.xlsx, con {1} filas<br/><a class='btn btn-link' href='#' onclick={2} >Clic Aquí para descargarlo</a>", fname, tb.Rows.Count, llamada);
                    banmsg2.Visible = true;
                }
                else
                {

                    banmsg2.InnerHtml = "No existen registros para exportar";
                    banmsg2.Visible = true;
                }


                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnExcel_Click), "damagedescuentosnivel2", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
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

    }
}