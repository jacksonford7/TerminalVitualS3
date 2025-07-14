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


    public partial class p2d_excluir_pase : System.Web.UI.Page
    {


        #region "Clases"
        //private static Int64? lm = -3;
        //private string OError;

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
     
        private P2D_Lista_Pases objCabecera = new P2D_Lista_Pases();
       
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
        //private bool SinDesconsolidar = false;
        //private bool SinAutorizacion = false;
        //private bool Bloqueos = false;
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

        }

        private void Actualiza_Panele_Detalle()
        {
            UPDETALLE.Update();
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

       
        #endregion

        #region "Eventos del formulario"

       
      

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

              

                if (!Page.IsPostBack)
                {
                    /*secuencial de sesion*/
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                 
                  

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

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

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
                        objCabecera = new P2D_Lista_Pases();
                        objCabecera.ID_PASE = SECUENCIA;
                        objCabecera.USUARIO_CREA = ClsUsuario.loginname;

                        objCabecera.Delete(out cMensajes);
                        if (cMensajes != string.Empty)
                        {

                            this.Mostrar_Mensaje(2,string.Format("<b>Error! </b> {0}", cMensajes));
                            return;
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2,string.Format("<b>Informativo! </b>Se desactivo el pase de puerta # {0} de la orden # {1} con éxito.", SECUENCIA, this.TXTORDEN.Text));

                        }

                        //vuelve a cargar datos
                        var TablaCotizacion = P2D_Lista_Pases.Listado_Pases(this.TXTORDEN.Text, out cMensajes);
                        if (TablaCotizacion == null)
                        {
                            tablePagination.DataSource = TablaCotizacion;
                            tablePagination.DataBind();

                            this.Mostrar_Mensaje(2,string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados. {0}", cMensajes));
                            return;
                        }
                        if (TablaCotizacion.Count <= 0)
                        {
                            tablePagination.DataSource = TablaCotizacion;
                            tablePagination.DataBind();
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados."));
                            return;
                        }

                        tablePagination.DataSource = TablaCotizacion;
                        tablePagination.DataBind();


                        this.Actualiza_Paneles();
                        this.Ocultar_Mensaje();


                    }

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", ex.Message));
                    return;

                }
            }
        }


        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
               
                Label LblEstado = e.Item.FindControl("LblEstado") as Label;
               
               
                Button BtnEvento = e.Item.FindControl("BtnActualizar") as Button;
                if (LblEstado.Text.Equals("CARGA NO AUTORIZADA") )
                {
                   // BtnEvento.Enabled = true;
                    BtnEvento.Attributes.Remove("disabled");
                }
                else
                {
                    BtnEvento.Attributes["disabled"] = "disabled";
                   // BtnEvento.Enabled = false;
                }

            }
        }


        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {

           
        }
        #endregion

        #region "Evento Botones"


        //agregar al detalle
        protected void BtnBuscarOrden_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {

                    this.LabelTotal.InnerText = string.Format("DETALLE DE PASES");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TXTORDEN.Text))
                    {
                        this.Mostrar_Mensaje(2,string.Format("<b>Informativo! </b>Por favor ingresar el número de orden"));
                        this.TXTORDEN.Focus();
                        return;
                    }


                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    var TablaCotizacion = P2D_Lista_Pases.Listado_Pases(this.TXTORDEN.Text, out cMensajes);

                    if (TablaCotizacion == null)
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        this.Actualiza_Paneles();

                        this.Mostrar_Mensaje(2,string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados. {0}", cMensajes));
                        return;
                    }
                    if (TablaCotizacion.Count <= 0)
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();

                        this.Actualiza_Paneles();

                        this.Mostrar_Mensaje(2,string.Format("<b>Informativo! </b>No existe información que mostrar, con los criterios de búsquedas ingresados."));
                        return;
                    }

                    tablePagination.DataSource = TablaCotizacion;
                    tablePagination.DataBind();


                    this.Actualiza_Paneles();
                    this.Ocultar_Mensaje();


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
               
                Response.Redirect("~/p2d/p2d_excluir_pase.aspx", false);



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

            }

        }

   

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
           
        }

   
        #endregion

     

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

     
      



        }
}