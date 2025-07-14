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
using CSLSite;
using System.Text.RegularExpressions;
using CasManual;

namespace CSLSite
{


    public partial class damagedescuentos : System.Web.UI.Page
    {


        #region "Clases"
     

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
    
     

        private Damage_Descuentos_Cab objCabeceraDescuento = new Damage_Descuentos_Cab();
        private Damage_Descuentos_Det objDetalleDescuento = new Damage_Descuentos_Det();

        private Cls_Novedades objNovedad = new Cls_Novedades();
        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;

       
        private string Fecha = string.Empty;
       
     
      
        private DateTime FechaDescDesde;
        private DateTime FechaDescHasta;
       
        private string LoginName = string.Empty;
      

        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;

        

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
            UPBOTONES.Update();
            UPDESCUENTO.Update();

        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
            UPDESCUENTO.Update();
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

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)//cabecera
            {
                this.banmsg_pie.Visible = false;
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("1");
            }
            if (Tipo == 2)//detalle
            {
                this.banmsg_pie.Visible = true;
                this.banmsg.Visible = false;
                this.banmsg_pie.InnerHtml = Mensaje;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("2");
            }
            if (Tipo == 3)//detalle
            {
             
                OcultarLoading("2");
            }


            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg_pie.InnerText = string.Empty;
          
            this.banmsg.Visible = false;
            this.banmsg_pie.Visible = false;

            this.Actualiza_Paneles();
            OcultarLoading("1");
            OcultarLoading("2");
        }

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCabeceraDescuento = new Damage_Descuentos_Cab();
            Session["Descuentos" + this.hf_BrowserWindowName.Value] = objCabeceraDescuento;
        }


        #endregion

        #region "Eventos del formulario"

        #region "Carga Combo líneas"
        private void CargaLineas()
        {
            try
            {

                List<Damage_ListaLineas> Listado = Damage_ListaLineas.ComboLineas(out cMensajes);

                this.CboLineaNaviera.DataSource = Listado;
                this.CboLineaNaviera.DataTextField = "LIN_DESCRIP";
                this.CboLineaNaviera.DataValueField = "LIN_ID";
                this.CboLineaNaviera.DataBind();


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>{0}", ex.Message));


            }

        }
        #endregion



        #region "Eventos Page"

        protected void Page_Init(object sender, EventArgs e)
        {

            try
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
                this.banmsg_pie.Visible = IsPostBack;
             

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_pie.InnerText = string.Empty;
                 
                }

               
                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {
                    if (!Page.IsPostBack)
                    {
                       
                        this.Actualiza_Paneles();

                    }

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>{0}", ex.Message));
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

                Server.HtmlEncode(this.TxtFechaDescuentoDesde.Text.Trim());
                Server.HtmlEncode(this.TxtFechaDescuentoHasta.Text.Trim());
                Server.HtmlEncode(this.TxtMotivo.Text.Trim());
                Server.HtmlEncode(this.CboEstados.Text.Trim());
                Server.HtmlEncode(this.TxtCantidad.Text.Trim());

                if (!Page.IsPostBack)
                {
                    /*secuencial de sesion*/
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (ClsUsuario != null)
                    {
                       
                    }

                    this.Crear_Sesion();

                    string desde = string.Format("{0}/{1}/{2} 00:00",DateTime.Today.Month.ToString("D2"), DateTime.Today.Day.ToString("D2"), DateTime.Today.Year.ToString());

                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    DateTime fdesde;

                    if (!DateTime.TryParseExact(desde, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.None, out fdesde))
                    {
                        this.Mostrar_Mensaje(4, string.Format("<b>Error! </b>Ingrese una fecha valida, revise la fecha desde"));
                        return;
                    }

                    this.TxtFechaDescuentoDesde.Text = fdesde.ToString("MM/dd/yyyy HH:mm");

                    DateTime today = DateTime.Today;
                    int lastDay = DateTime.DaysInMonth(today.Year, today.Month);
                    DateTime lastDate = new DateTime(today.Year, today.Month, lastDay);


                    this.TxtFechaDescuentoHasta.Text = lastDate.ToString("MM/dd/yyyy HH:mm");

                  
                    this.CargaLineas();

                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(4, string.Format("<i class='fa fa-warning'></i><b> Error! {0}</b>", ex.Message));
            }
        }


        #endregion
      
       #region "Eventos de la grilla"
 
        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    Int32 SECUENCIA = 0;

                    if (!Int32.TryParse(t, out SECUENCIA))
                    {
                        SECUENCIA = 0;
                    }

                    if (e.CommandName == "Eliminar")
                    {
                        objCabeceraDescuento = Session["Descuentos" + this.hf_BrowserWindowName.Value] as Damage_Descuentos_Cab;

                        //existe pase a remover
                        var Detalle = objCabeceraDescuento.Detalle.FirstOrDefault(f => f.DESC_FILA == SECUENCIA);
                        if (Detalle != null)
                        {
                            Int32 Llave = Detalle.DESC_FILA;
                            //remover pase
                            objCabeceraDescuento.Detalle.Remove(objCabeceraDescuento.Detalle.Where(p => p.DESC_FILA == Llave).FirstOrDefault());

                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información del descuento para quitar: {0} </b>", t.ToString()));
                            return;
                        }

                        tablePagination.DataSource = objCabeceraDescuento.Detalle;
                        tablePagination.DataBind();

                        this.UPDETALLE.Update();

                        Session["Descuentos" + this.hf_BrowserWindowName.Value] = objCabeceraDescuento;


                        this.Actualiza_Paneles();

                    }

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", ex.Message));
                    return;

                }
            }
        }


        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {

           
        }

        #endregion

      

       


        #region "Evento Botones"


        //agregar al detalle
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {

                    CultureInfo enUS = new CultureInfo("en-US");

                    int DescuentoMin = 0;
                    int DescuentoMax = 0;

                    Ocultar_Mensaje();

                    this.LabelTotal.InnerText = string.Format("DETALLE DE DESCUENTOS");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (this.CboLineaNaviera.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar la línea naviera</b>"));
                        this.CboLineaNaviera.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtCantidad.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! debe ingresar el % de descuento</b>"));
                        this.TxtCantidad.Focus();
                        return;
                    }


                    int nDescuento = 0;
                    if (!int.TryParse(this.TxtCantidad.Text, out nDescuento))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el % de descuento</b>"));
                        this.TxtCantidad.Focus();
                        return;
                    }

                    //valida rango de descuentos
                    List<Cls_Bil_Configuraciones> Parametros = Cls_Bil_Configuraciones.Parametros(out cMensajes);
                    if (!String.IsNullOrEmpty(cMensajes))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, obtener parámetros.....{0}</b>", cMensajes));
                        this.Actualiza_Panele_Detalle();
                        return;
                    }

                    var LinqMinDescuentos = (from Descuento in Parametros.Where(Dias => Dias.NOMBRE.Equals("MIN_DESCUENTO"))
                                          select new
                                          {
                                              VALOR = Descuento.VALOR == null ? "10" : Descuento.VALOR
                                          }).FirstOrDefault();

                    var LinqMaxDescuentos = (from Descuento in Parametros.Where(Dias => Dias.NOMBRE.Equals("MAX_DESCUENTO"))
                                             select new
                                             {
                                                 VALOR = Descuento.VALOR == null ? "100" : Descuento.VALOR
                                             }).FirstOrDefault();

                    DescuentoMin = LinqMinDescuentos != null ? int.Parse(LinqMinDescuentos.VALOR) : 10;
                    DescuentoMax = LinqMaxDescuentos != null ? int.Parse(LinqMaxDescuentos.VALOR) : 100;

                    if (nDescuento < DescuentoMin) 
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! El % {0} ingresado no puede ser menor a {1}, ingrese un valor mayor..</b>", nDescuento, DescuentoMin));
                        this.TxtCantidad.Focus();
                        return;
                    }

                    if (nDescuento > DescuentoMax)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! El % {0} ingresado no puede ser mayor a {1}, ingrese un valor menor..</b>", nDescuento, DescuentoMax));
                        this.TxtCantidad.Focus();
                        return;
                    }


                    objCabeceraDescuento = Session["Descuentos" + this.hf_BrowserWindowName.Value] as Damage_Descuentos_Cab;
                    if (objCabeceraDescuento == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! no se puede agregar detalle de la transacción</b>"));
                        return;
                    }
                   

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    //yes existe 
                    var Detalle = objCabeceraDescuento.Detalle.FirstOrDefault(f => f.LIN_ID == Int64.Parse(this.CboLineaNaviera.SelectedValue));
                    if (Detalle == null)
                    {
                        objCabeceraDescuento.DESC_USER_CREA = ClsUsuario.loginname;
                        objDetalleDescuento = new Damage_Descuentos_Det();
                        objDetalleDescuento.DESC_FILA = objCabeceraDescuento.Detalle.Count + 1;
                        objDetalleDescuento.LIN_ID = Int64.Parse(this.CboLineaNaviera.SelectedValue);

                        var ExisteLinea = CboLineaNaviera.Items.FindByValue(objDetalleDescuento.LIN_ID.ToString());
                        if (ExisteLinea != null)
                        {
                            objDetalleDescuento.DESC_LIN_DESCRIP = ExisteLinea.Text;
                        }

                        objDetalleDescuento.DESC_PORCENTAJE = nDescuento;
                        objCabeceraDescuento.Detalle.Add(objDetalleDescuento);


                    }
                    else
                    {
                        Int64 LIN_ID = Int64.Parse(this.CboLineaNaviera.SelectedValue);
                        string DESC_LIN_DESCRIP = string.Empty;

                        var ExisteLinea = CboLineaNaviera.Items.FindByValue(LIN_ID.ToString());
                        if (ExisteLinea != null)
                        {
                            DESC_LIN_DESCRIP = ExisteLinea.Text;
                        }

                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Ya existe una línea naviera agregada con descuentos.: {0} </b>", DESC_LIN_DESCRIP));
                        return;

                    }


                    //agrega a la grilla
                    tablePagination.DataSource = objCabeceraDescuento.Detalle;
                    tablePagination.DataBind();

                    this.LabelTotal.InnerText = string.Format("DETALLE DE DESCUENTOS - Total Líneas Navieras: {0}", objCabeceraDescuento.Detalle.Count);

                    Session["Descuentos" + this.hf_BrowserWindowName.Value] = objCabeceraDescuento;


                    this.Actualiza_Panele_Detalle();

                   
                   
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

                }
            }

        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                
                Response.Redirect("~/damagecontrol/damagedescuentos.aspx", false);



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

   

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {

                CultureInfo enUS = new CultureInfo("en-US");
               
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

                   

                    if (string.IsNullOrEmpty(this.TxtFechaDescuentoDesde.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar la fecha inicial del descuento</b>"));
                        this.TxtFechaDescuentoDesde.Focus();
                        return;
                    }

                    
                    Fecha = string.Format("{0}", this.TxtFechaDescuentoDesde.Text.Trim());
                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaDescDesde))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha y hora valida, para descuento inicial</b>"));
                        this.TxtFechaDescuentoDesde.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtFechaDescuentoHasta.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar la fecha final del descuento</b>"));
                        this.TxtFechaDescuentoHasta.Focus();
                        return;
                    }


                    Fecha = string.Format("{0}", this.TxtFechaDescuentoHasta.Text.Trim());
                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaDescHasta))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha y hora valida, para descuento final</b>"));
                        this.TxtFechaDescuentoHasta.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtMotivo.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe ingresar un concepto</b>"));
                        this.TxtMotivo.Focus();
                        return;
                    }


                    objCabeceraDescuento = Session["Descuentos" + this.hf_BrowserWindowName.Value] as Damage_Descuentos_Cab;
                    if (objCabeceraDescuento == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para grabar"));
                        return;
                    }
                    else
                    {
                        if (objCabeceraDescuento.Detalle.Count == 0)
                        {
                            this.Mostrar_Mensaje(2,string.Format("<b>Informativo! </b>No existe detalle de descuentos para grabar"));
                            return;
                        }


                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        objCabeceraDescuento.DESC_ID = 0;
                        objCabeceraDescuento.DESC_DESDE = FechaDescDesde;
                        objCabeceraDescuento.DESC_HASTA = FechaDescHasta;
                        objCabeceraDescuento.DESC_NOTA = this.TxtMotivo.Text.Trim();
                        objCabeceraDescuento.DESC_ESTADO = this.CboEstados.SelectedValue.ToString();
                        objCabeceraDescuento.DESC_USER_CREA = ClsUsuario.loginname;



                        var nIdRegistro = objCabeceraDescuento.SaveTransaction(out cMensajes);
                        if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo grabar datos de descuentos..{0}</b>", cMensajes));
                            return;
                        }
                        else
                        {

                            objCabeceraDescuento.Detalle.Clear();
                           

                            Session["Descuentos" + this.hf_BrowserWindowName.Value] = objCabeceraDescuento;

                            tablePagination.DataSource = null;
                            tablePagination.DataBind();

                            this.LabelTotal.InnerText = string.Format("DETALLE DE DESCUENTOS");

                            this.TxtMotivo.Text = string.Empty;
                            this.CboEstados.SelectedIndex = 0;
                            this.CboLineaNaviera.SelectedIndex = 0;
                            this.TxtCantidad.Text = string.Empty;
                           

                         

                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Se registro con éxito POLÍTICAS DE DESCUENTOS # {0}</b>", nIdRegistro.Value));

                           

                        }
                    }


                   

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

                }

            }
        }

   
        #endregion

        #region "Eventos Check"
      
        #endregion

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

     
      
      

      
   
       


        }
}