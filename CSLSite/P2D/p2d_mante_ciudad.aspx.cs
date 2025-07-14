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

using System.Data;
using System.Web.Script.Services;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using PasePuerta;
using CSLSite;
using ClsAppCgsa;

namespace CSLSite
{
  

    public partial class p2d_mante_ciudad : System.Web.UI.Page
    {

        #region "Clases"

        usuario ClsUsuario;
       
        private P2D_Ciudad ObjCiudad = new P2D_Ciudad();

        #endregion

        #region "Variables"

        private static Int64? lm = -3;
        private string OError;
    
        #endregion

        #region "Propiedades"

     
        #endregion

      


        #region "Metodos"

        private void Actualiza_Paneles()
        {
            UPCARGA.Update();
           

        }


        private void Limpia_Campos()
        {
          
            this.IdPaquete.Value = "0";
            this.TxtCiudad.Text = string.Empty;

            this.Actualiza_Paneles();

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
           
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            OcultarLoading("1");
           
          
            OcultarLoading("2");
      
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
          
            this.banmsg.Visible = false;
          
            this.Actualiza_Paneles();
            OcultarLoading("1");
            OcultarLoading("2");
        }



        private void Carga_Ciudades()
        {
            try
            {

                List<P2D_Ciudad> Listado = P2D_Ciudad.Listado_Ciudades(out OError);
                if (Listado != null)
                {
                    tablePagination.DataSource = Listado;
                    tablePagination.DataBind();
                   
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                  
                }

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Ciudades), "Carga_Ciudades", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

            }

        }


        #endregion



        #region "Eventos del Formulario"


        protected void BtnNuevo_Click(object sender, EventArgs e)
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

                    this.Limpia_Campos();
                    this.Ocultar_Mensaje();
                    this.TxtCiudad.Focus();

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnNuevo_Click), "BtnNuevo_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));
                }
            }

        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
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

                    if (string.IsNullOrEmpty(this.TxtCiudad.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingresar el nombre de la ciudad"));
                        this.TxtCiudad.Focus();
                        return;
                    }

                  

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    Int64 Id = 0;
                    string Accion = "G";

                    if (!Int64.TryParse(this.IdPaquete.Value, out Id))
                    {
                        Id = 0;
                    }

                    if (Id == 0)
                    {
                        Accion = "G";
                    }
                    else
                    {
                        Accion = "M";
                    }

                    

                    ObjCiudad = new P2D_Ciudad();
                    ObjCiudad.ID_CIUDAD = Id;
                    ObjCiudad.DESC_CIUDAD = this.TxtCiudad.Text.Trim();
                    ObjCiudad.USUARIO_CREA = ClsUsuario.loginname.Length > 10 ? ClsUsuario.loginname.Substring(0,10) : ClsUsuario.loginname;

                    ObjCiudad.ACCION = Accion;

                    var nIdRegistro = ObjCiudad.Save(out OError);

                    if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                    {

                        this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo grabar la ciudad: {0}, {1}</b>", this.TxtCiudad.Text.Trim(), OError));
                        return;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Se procedió con {0} ciudad {1}",(Accion=="G" ? "el registro" : " la actualización de la") ,this.TxtCiudad.Text.Trim()));
                       
                    }

                    this.Limpia_Campos();
                    this.Carga_Ciudades();

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "BtnGrabar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

                }
            }

        }



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

                    if (e.CommandName == "Actualizar")
                    {
                        this.IdPaquete.Value = t.ToString();

                        Int64 Id = 0;
                      
                        if (!Int64.TryParse(this.IdPaquete.Value, out Id))
                        {
                            Id = 0;
                        }

                        ObjCiudad = new P2D_Ciudad();
                        ObjCiudad.ID_CIUDAD = Id;
                        if (ObjCiudad.PopulateMyData(out OError))
                        {
                            this.TxtCiudad.Text = ObjCiudad.DESC_CIUDAD.Trim();
                            this.TxtCiudad.Focus();
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo cargar datos de la Ciudad:{0} </b>", Id));
                        }

                        this.Carga_Ciudades();
                        this.Actualiza_Paneles();

                    }//fin actualizar

                    if (e.CommandName == "Eliminar")
                    {
                        this.IdPaquete.Value = t.ToString();

                        Int64 Id = 0;

                        if (!Int64.TryParse(this.IdPaquete.Value, out Id))
                        {
                            Id = 0;
                        }

                        ObjCiudad = new P2D_Ciudad();
                        ObjCiudad.ID_CIUDAD = Id;
                        ObjCiudad.USUARIO_CREA = ClsUsuario.loginname;
                        if (ObjCiudad.Delete(out OError))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Ciudad inactivado con éxito {0} </b>", ""));
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo inactivar la Ciudad:{0}-{1} </b>", Id, OError));
                        }

                        this.Carga_Ciudades();
                        this.Actualiza_Paneles();

                    }//fin actualizar


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

                }
            }

        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            
        }

      
        #endregion

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

     

            ClsUsuario = Page.Tracker();
            if (ClsUsuario != null)
            {
                if (!Page.IsPostBack)
                {
                    this.Limpia_Campos();
                }
                   
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

                Server.HtmlEncode(this.TxtCiudad.Text.Trim());
                

                if (!Page.IsPostBack)
                {
                   
                    this.Carga_Ciudades();
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }


   
     
    }

}