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


    public partial class checklist_digital : System.Web.UI.Page
    {


        #region "Clases"
        //private static Int64? lm = -3;
        //private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
    
        //private List<Cls_Bil_Cas_Manual> List_Autorizacion { set; get; }

        private Cls_CheckList_Cab objCabeceraCheckList = new Cls_CheckList_Cab();
        private Cls_CheckList_Det objDetalleCheckList = new Cls_CheckList_Det();
        private Cls_CheckList_Det_Tarea objDetalleCheckTareas = new Cls_CheckList_Det_Tarea();
        private Cls_Novedades objNovedad = new Cls_Novedades();
        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;

       
        private string Fecha = string.Empty;
       
        //private int Fila = 1;
      
        private DateTime FechaFactura;
        private string HoraHasta = "00:00";
        private string LoginName = string.Empty;
      

        private string MensajesErrores = string.Empty;
        private string MensajeCasos = string.Empty;

        public Int64 pID_TAREA
        {
            get { return (Int64)Session["ID_TAREA"]; }
            set { Session["ID_TAREA"] = value; }
        }

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

        private Boolean valida_email(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
            UPCARGA.Update();
            UPBOTONES.Update();
            UPTAREAS.Update();
        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
            UPTAREAS.Update();
        }

        

      
        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        private void Limpiar_Motivos()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "limpiar('');", true);
        }

        private void Limpiar_Todo()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "limpiar_todo('');", true);
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
                this.banmsg_det.Visible = true;
                this.banmsg_det.InnerHtml = Mensaje;
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
            objCabeceraCheckList = new Cls_CheckList_Cab();
            Session["CheckList" + this.hf_BrowserWindowName.Value] = objCabeceraCheckList;
        }


        #endregion

        #region "Eventos del formulario"

        #region"Tipo de Equipos"
        private void Carga_TipoEquipo()
        {
            try
            {
               
                List<Cls_TipoEquipos> Listado = Cls_TipoEquipos.Lista_TipoEquipos( out cMensajes);

                this.CboTipoEquipo.DataSource = Listado;
                this.CboTipoEquipo.DataTextField = "DESCRIPCION";
                this.CboTipoEquipo.DataValueField = "ID_TIPO_EQUIPO";
                this.CboTipoEquipo.DataBind();


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>{0}", ex.Message));

            
            }

        }

        protected void CboTipoEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
              
                if (CboTipoEquipo.SelectedIndex != -1)
                {
                    Int64 ID_TIPO_EQUIPO;

                    if(!Int64.TryParse(this.CboTipoEquipo.SelectedValue.ToString(), out ID_TIPO_EQUIPO))
                    {
                        ID_TIPO_EQUIPO = 0;
                    }

                    List<Cls_Equipos> Listado = Cls_Equipos.Lista_Equipos(ID_TIPO_EQUIPO,out cMensajes);

                    this.CboEquipo.DataSource = Listado;
                    this.CboEquipo.DataTextField = "NOMBRE";
                    this.CboEquipo.DataValueField = "ID_EQUIPO";
                    this.CboEquipo.DataBind();

                    this.Carga_Tareas();
                }

               
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>{0}", ex.Message));
            }
        }

        protected void CboEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (CboEquipo.SelectedIndex != -1)
                {
                    this.Carga_Tareas();
                }


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>{0}", ex.Message));
            }
        }
        #endregion

        #region"Turnos"
        private void Carga_Turnos()
        {
            try
            {

                List<Cls_Turnos> Listado = Cls_Turnos.Lista_Turnos(out cMensajes);

                this.CboTurno.DataSource = Listado;
                this.CboTurno.DataTextField = "DESCRIPCION";
                this.CboTurno.DataValueField = "ID_TURNO";
                this.CboTurno.DataBind();


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>{0}", ex.Message));


            }

        }
        #endregion

        #region "Carga tareas por tipo de equipos"
        private void Carga_Tareas()
        {
            try
            {
                Int64 ID_TIPO_EQUIPO = (this.CboTipoEquipo.SelectedValue == null ? 0 : Int64.Parse(this.CboTipoEquipo.SelectedValue.ToString()));

                objCabeceraCheckList = Session["CheckList" + this.hf_BrowserWindowName.Value] as Cls_CheckList_Cab;
                if (objCabeceraCheckList == null)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! no se puede agregar detalle de tareas al check list</b>"));
                    return;
                }

                List<Cls_CheckList_Det_Tarea> Listado = Cls_CheckList_Det_Tarea.Lista_Tareas(ID_TIPO_EQUIPO, out cMensajes);

                if (Listado != null)
                {
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objCabeceraCheckList.Detalle_Tareas.Clear();
                    int i = 1;
                    foreach (var Det in Listado)
                    {
                        objDetalleCheckTareas = new Cls_CheckList_Det_Tarea();
                        objDetalleCheckTareas.ID_TIPO_EQUIPO = ID_TIPO_EQUIPO;
                        objDetalleCheckTareas.NOMBRE_TIPO_EQUIPO = Det.NOMBRE_TIPO_EQUIPO;
                        objDetalleCheckTareas.ID_TAREA = Det.ID_TAREA;
                        objDetalleCheckTareas.TAREA = Det.TAREA;
                        objDetalleCheckTareas.SELECCION = false;
                        objDetalleCheckTareas.SECUENCIA = i;
                        objCabeceraCheckList.Detalle_Tareas.Add(objDetalleCheckTareas);
                        i++;
                    }


                    //agrega a la grilla
                    tablePaginationTareas.DataSource = objCabeceraCheckList.Detalle_Tareas;
                    tablePaginationTareas.DataBind();

                    this.LabelTotalTareas.InnerText = string.Format("DETALLE DE TAREAS - Total Registros: {0}", objCabeceraCheckList.Detalle_Tareas.Count);

                    Session["CheckList" + this.hf_BrowserWindowName.Value] = objCabeceraCheckList;

                }

                this.Actualiza_Panele_Detalle();

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

                Server.HtmlEncode(this.TxtOperador.Text.Trim());
                Server.HtmlEncode(this.TxtFechaHasta.Text.Trim());
                Server.HtmlEncode(this.TxtCodNovedad.Text.Trim());
                Server.HtmlEncode(this.TxtDescNovedad.Text.Trim());
                Server.HtmlEncode(this.TxtMotivo.Text.Trim());

                if (!Page.IsPostBack)
                {
                    /*secuencial de sesion*/
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (ClsUsuario != null)
                    {
                        this.TxtOperador.Text = string.Format("{0} {1}", (string.IsNullOrEmpty(ClsUsuario.nombres) ? "" : ClsUsuario.nombres.Trim()), (string.IsNullOrEmpty(ClsUsuario.apellidos) ? "" : ClsUsuario.apellidos.Trim()));
                    }

                    this.Crear_Sesion();

                    this.Carga_TipoEquipo();

                    CboTipoEquipo_SelectedIndexChanged(sender, e);
                    CboEquipo_SelectedIndexChanged(sender, e);

                    this.Carga_Turnos();

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

                    //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;

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
                        objCabeceraCheckList = Session["CheckList" + this.hf_BrowserWindowName.Value] as Cls_CheckList_Cab;

                        //existe pase a remover
                        var Detalle = objCabeceraCheckList.Detalle.FirstOrDefault(f => f.SECUENCIA == SECUENCIA);
                        if (Detalle != null)
                        {
                            Int32 Llave = Detalle.SECUENCIA;
                            //remover pase
                            objCabeceraCheckList.Detalle.Remove(objCabeceraCheckList.Detalle.Where(p => p.SECUENCIA == Llave).FirstOrDefault());

                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información de la novedad para quitar: {0} </b>", t.ToString()));
                            return;
                        }

                        tablePagination.DataSource = objCabeceraCheckList.Detalle;
                        tablePagination.DataBind();

                        this.UPDETALLE.Update();

                        Session["CheckList" + this.hf_BrowserWindowName.Value] = objCabeceraCheckList;


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

        #region "Eventos de la grilla de tareas"
        protected void tablePaginationTareas_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                        this.Mostrar_Mensaje(3, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(3, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Ver")
                    {
                        Int64 ID_TAREA = Int64.Parse(t.ToString());
                        Int64 ID_TIPO_EQUIPO = (this.CboTipoEquipo.SelectedValue == null ? 0 : Int64.Parse(this.CboTipoEquipo.SelectedValue.ToString()));
                        pID_TAREA = ID_TAREA;
                        string v_mensaje = string.Empty;

                        var lookup = Cls_Novedades.Buscador_Novedades("", ID_TIPO_EQUIPO, out v_mensaje);

                        if (lookup != null && lookup.Count > 0)
                        {
                            this.tablePaginationBuscador.DataSource = lookup;
                            this.tablePaginationBuscador.DataBind();

                            banmsg_det.InnerText = string.Empty;
                            banmsg_det.Visible = false;
                            UPBUSCADOR.Update();
                        }

                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

                }

            }
        }

        protected void chkMarcar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                Label SECUENCIA = (Label)item.FindControl("LblCarga");
                TextBox COMENTARIO = (TextBox)item.FindControl("Txtcomentario");

                //actualiza datos del contenedor
                objCabeceraCheckList = Session["CheckList" + this.hf_BrowserWindowName.Value] as Cls_CheckList_Cab;

                var Detalle = objCabeceraCheckList.Detalle_Tareas.FirstOrDefault(f => f.SECUENCIA == int.Parse(SECUENCIA.Text));
                if (Detalle != null)
                {
                    Detalle.SELECCION = chkPase.Checked;
                    Detalle.COMENTARIO = COMENTARIO.Text.Trim();


                }

                tablePaginationTareas.DataSource = objCabeceraCheckList.Detalle_Tareas;
                tablePaginationTareas.DataBind();

                Session["CheckList" + this.hf_BrowserWindowName.Value] = objCabeceraCheckList;



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }

        #endregion

        #region "Eventos de la grilla del buscador de novedades"
        protected void tablePaginationBuscador_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                        this.Mostrar_Mensaje(3, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(3, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Ver")
                    {
                       
                        Int64 ID_NOVEDAD = Int64.Parse(t.ToString());
                        string v_mensaje = string.Empty;

                        objNovedad = new Cls_Novedades();
                        objNovedad.ID_NOVEDAD = ID_NOVEDAD;

                        if (objNovedad.PopulateMyData(out v_mensaje))
                        {
                            //agregar novedad
                            objCabeceraCheckList = Session["CheckList" + this.hf_BrowserWindowName.Value] as Cls_CheckList_Cab;

                            //existe pase a remover
                            var Detalle = objCabeceraCheckList.Detalle_Tareas.FirstOrDefault(f => f.ID_TAREA == this.pID_TAREA);
                            if (Detalle != null)
                            {
                                Detalle.ID_NOVEDAD = ID_NOVEDAD;
                                Detalle.NOVEDAD = objNovedad.NOVEDAD;

                                tablePaginationTareas.DataSource = objCabeceraCheckList.Detalle_Tareas;
                                tablePaginationTareas.DataBind();

                                Session["CheckList" + this.hf_BrowserWindowName.Value] = objCabeceraCheckList;


                                this.Actualiza_Paneles();

                            }
                            else
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información de la novedad para actualizar: {0} </b>", t.ToString()));
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información de la novedad para actualizar: {0} </b>", t.ToString()));
                            return;
                        }

                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

                }

            }
        }

        protected void find_Click(object sender, EventArgs e)
        {

            try
            {
                Int64 ID_TIPO_EQUIPO = (this.CboTipoEquipo.SelectedValue == null ? 0 : Int64.Parse(this.CboTipoEquipo.SelectedValue.ToString()));

                string v_mensaje = string.Empty;

                var lookup = Cls_Novedades.Buscador_Novedades(txtfinder.Text, ID_TIPO_EQUIPO, out v_mensaje);

                if (lookup != null && lookup.Count > 0)
                {
                    this.tablePaginationBuscador.DataSource = lookup;
                    this.tablePaginationBuscador.DataBind();

                    banmsg_det.InnerText = string.Empty;
                    banmsg_det.Visible = false;
                    UPBUSCADOR.Update();
                }
            }
            catch (Exception ex)
            {

                this.Mostrar_Mensaje(3, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}</b>", ex.Message));

            }

        }

        protected void TxtNewCantidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox COMENTARIO = (TextBox)sender;
                RepeaterItem item = (RepeaterItem)COMENTARIO.NamingContainer;
                Label SECUENCIA = (Label)item.FindControl("LblCarga");
               

                //actualiza datos del contenedor
                objCabeceraCheckList = Session["CheckList" + this.hf_BrowserWindowName.Value] as Cls_CheckList_Cab;

                var Detalle = objCabeceraCheckList.Detalle_Tareas.FirstOrDefault(f => f.SECUENCIA == int.Parse(SECUENCIA.Text));
                if (Detalle != null)
                {

                    Detalle.COMENTARIO = COMENTARIO.Text.Trim();


                }

                tablePaginationTareas.DataSource = objCabeceraCheckList.Detalle_Tareas;
                tablePaginationTareas.DataBind();

                Session["CheckList" + this.hf_BrowserWindowName.Value] = objCabeceraCheckList;



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }


        }

        protected void tablePaginationTareas_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
               
            }
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

                   // this.BtnCotizar.Attributes.Remove("disabled");
                  
  
                    this.LabelTotal.InnerText = string.Format("DETALLE DE NOVEDADES");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (this.CboTipoEquipo.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el tipo de equipo</b>"));
                        this.CboTipoEquipo.Focus();
                        return;
                    }

                    if (this.CboEquipo.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el equipo</b>"));
                        this.CboEquipo.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtOperador.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el operador</b>"));
                        this.TxtOperador.Focus();
                        return;
                    }

                    HoraHasta = "00:00";
                    Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                    {
                      
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (this.CboTurno.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el turno</b>"));
                        this.CboTurno.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtCodNovedad.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor seleccionar el tipo de novedad</b>"));
                        this.TxtCodNovedad.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtMotivo.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor seleccionar el motivo para la novedad</b>"));
                        this.TxtMotivo.Focus();
                        return;
                    }


                    objCabeceraCheckList = Session["CheckList" + this.hf_BrowserWindowName.Value] as Cls_CheckList_Cab;
                    if (objCabeceraCheckList == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! no se puede agregar detalle al check list</b>"));
                        return;
                    }


                        

                    //agrego todos los contenedores a la clase cabecera
                    objCabeceraCheckList = Session["CheckList" + this.hf_BrowserWindowName.Value] as Cls_CheckList_Cab;

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objCabeceraCheckList.ID_TIPO_EQUIPO = Int64.Parse(this.CboTipoEquipo.SelectedValue);
                    objCabeceraCheckList.ID_EQUIPO = Int64.Parse(this.CboEquipo.SelectedValue);
                    objCabeceraCheckList.OPERADOR = this.TxtOperador.Text.Trim();
                    objCabeceraCheckList.ID_TURNO = Int64.Parse(this.CboTurno.SelectedValue);
                    objCabeceraCheckList.ESTADO = true;
                    objCabeceraCheckList.FECHA = FechaFactura;
                    objCabeceraCheckList.USUARIO_CREA = ClsUsuario.loginname;

                    objDetalleCheckList = new Cls_CheckList_Det();
                    objDetalleCheckList.ID_TIPO_EQUIPO = Int64.Parse(this.CboTipoEquipo.SelectedValue);

                    var ExisteTipo = CboTipoEquipo.Items.FindByValue(objDetalleCheckList.ID_TIPO_EQUIPO.ToString());
                    if (ExisteTipo != null)
                    {
                        objDetalleCheckList.NOMBRE_TIPO_EQUIPO = ExisteTipo.Text; 
                    }

                    objDetalleCheckList.ID_EQUIPO = Int64.Parse(this.CboEquipo.SelectedValue);

                    var ExisteEquipo = CboEquipo.Items.FindByValue(objDetalleCheckList.ID_EQUIPO.ToString());
                    if (ExisteEquipo != null)
                    {
                        objDetalleCheckList.NOMBRE_EQUIPO = ExisteEquipo.Text;
                    }

                    objDetalleCheckList.OPERADOR = this.TxtOperador.Text.Trim();
                    objDetalleCheckList.FECHA = FechaFactura;
                    objDetalleCheckList.USUARIO_CREA = ClsUsuario.loginname;

                    objDetalleCheckList.ID_NOVEDAD = Int64.Parse(this.TxtCodNovedad.Text);
                    objDetalleCheckList.NOVEDAD = this.TxtDescNovedad.Text.Trim();
                    objDetalleCheckList.MOTIVO = this.TxtMotivo.Text.Trim();
                    objDetalleCheckList.SECUENCIA = objCabeceraCheckList.Detalle.Count + 1;

                    objCabeceraCheckList.Detalle.Add(objDetalleCheckList);


                    //agrega a la grilla
                    tablePagination.DataSource = objCabeceraCheckList.Detalle;
                    tablePagination.DataBind();

                    this.LabelTotal.InnerText = string.Format("DETALLE DE NOVEDADES - Total Registros: {0}", objCabeceraCheckList.Detalle.Count);

                    Session["CheckList" + this.hf_BrowserWindowName.Value] = objCabeceraCheckList;

                  
                    this.CboTipoEquipo.Attributes["disabled"] = "disabled";
                    this.CboEquipo.Attributes["disabled"] = "disabled";

                    this.Limpiar_Motivos();

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
                this.CboTipoEquipo.Attributes.Remove("disabled");
                this.CboEquipo.Attributes.Remove("disabled");
                Response.Redirect("~/checklist/checklist_digital.aspx", false);



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

                    if (this.CboTipoEquipo.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el tipo de equipo</b>"));
                        this.CboTipoEquipo.Focus();
                        return;
                    }

                    if (this.CboEquipo.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el equipo</b>"));
                        this.CboEquipo.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtOperador.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Por favor ingresar el operador</b>"));
                        this.TxtOperador.Focus();
                        return;
                    }

                    HoraHasta = "00:00";
                    Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                    if (!DateTime.TryParseExact(Fecha, "MM/dd/yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                    {

                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar una fecha valida</b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (this.CboTurno.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe seleccionar el turno</b>"));
                        this.CboTurno.Focus();
                        return;
                    }

                    objCabeceraCheckList = Session["CheckList" + this.hf_BrowserWindowName.Value] as Cls_CheckList_Cab;
                    if (objCabeceraCheckList == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..no existen datos para grabar"));
                        return;
                    }
                    else
                    {
                        if (objCabeceraCheckList.Detalle_Tareas.Count == 0)
                        {
                            this.Mostrar_Mensaje(2,string.Format("<b>Informativo! </b>No existe detalle de tareas para grabar"));
                            return;
                        }


                        var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                        objCabeceraCheckList.ID_TIPO_EQUIPO = Int64.Parse(this.CboTipoEquipo.SelectedValue);
                        objCabeceraCheckList.ID_EQUIPO = Int64.Parse(this.CboEquipo.SelectedValue);
                        objCabeceraCheckList.OPERADOR = this.TxtOperador.Text.Trim();
                        objCabeceraCheckList.ID_TURNO = Int64.Parse(this.CboTurno.SelectedValue);
                        objCabeceraCheckList.ESTADO = true;
                        objCabeceraCheckList.FECHA = FechaFactura;
                        objCabeceraCheckList.USUARIO_CREA = ClsUsuario.loginname;



                        var nIdRegistro = objCabeceraCheckList.SaveTransaction(out cMensajes);
                        if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! No se pudo grabar datos de ingreso de novedades..{0}</b>", cMensajes));
                            return;
                        }
                        else
                        {
                            objCabeceraCheckList.Detalle.Clear();
                            objCabeceraCheckList.Detalle_Tareas.Clear();

                            Session["CheckList" + this.hf_BrowserWindowName.Value] = objCabeceraCheckList;

                            tablePagination.DataSource = null;
                            tablePagination.DataBind();

                            tablePaginationTareas.DataSource = null;
                            tablePaginationTareas.DataBind();

                           

                            this.CboTipoEquipo.SelectedIndex = 0;
                            this.CboEquipo.SelectedIndex = 0;
                            this.TxtOperador.Text = string.Empty;
                            this.TxtFechaHasta.Text = string.Empty;
                            this.CboTurno.SelectedIndex = 0;
                            this.TxtCodNovedad.Text = string.Empty;
                            this.TxtDescNovedad.Text = string.Empty;
                            this.TxtMotivo.Text = string.Empty;

                            this.CboTipoEquipo.Attributes.Remove("disabled");
                            this.CboEquipo.Attributes.Remove("disabled");

                            Limpiar_Todo();

                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>Se registro con éxito la novedad # {0}</b>", nIdRegistro.Value));

                            Carga_Tareas();

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