﻿using System;
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
  

    public partial class appEventos : System.Web.UI.Page
    {

        #region "Clases"

        usuario ClsUsuario;
       
        private MantenimientoEventos objEvento = new MantenimientoEventos();

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
            this.TXTMRN.Text = string.Empty;
            this.IdEvento.Value = "0";
           
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



        private void Carga_Eventos()
        {
            try
            {

                List<MantenimientoEventos> Listado = MantenimientoEventos.Listado_Eventos(out OError);
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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Eventos), "Carga_Eventos", false, null, null, ex.StackTrace, ex);
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
                    this.TXTMRN.Focus();

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnNuevo_Click), "BtnNuevo_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));
                }
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

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingresar el nombre del evento"));
                        this.TXTMRN.Focus();
                        return;
                    }

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    Int64 Id = 0;
                    string Accion = "G";

                    if (!Int64.TryParse(this.IdEvento.Value, out Id))
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
                    objEvento = new MantenimientoEventos();
                    objEvento.Id = Id;
                    objEvento.Name = this.TXTMRN.Text.Trim();
                    objEvento.Create_user = ClsUsuario.loginname.Length > 10 ? ClsUsuario.loginname.Substring(0,10) : ClsUsuario.loginname;
                    objEvento.isPhoto = false;
                    objEvento.Action = Accion;

                    var nIdRegistro = objEvento.Save(out OError);

                    if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                    {

                        this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo grabar el evento: {0}, {1}</b>", this.TXTMRN.Text.Trim(), OError));
                        return;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Se procedió con {0} evento {1}",(Accion=="G" ? "el registro" : " la actualización del") ,this.TXTMRN.Text.Trim()));
                       
                    }

                    this.Limpia_Campos();
                    this.Carga_Eventos();



                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnBuscar_Click), "BtnBuscar_Click", false, null, null, ex.StackTrace, ex);
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
                        this.IdEvento.Value = t.ToString();

                        Int64 Id = 0;
                      
                        if (!Int64.TryParse(this.IdEvento.Value, out Id))
                        {
                            Id = 0;
                        }

                        objEvento = new MantenimientoEventos();
                        objEvento.Id = Id;
                        if (objEvento.PopulateMyData(out OError))
                        {
                            this.TXTMRN.Text = objEvento.Name;
                            this.TXTMRN.Focus();
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo cargar datos del evento:{0} </b>", Id));
                        }

                        this.Carga_Eventos();
                        this.Actualiza_Paneles();

                    }//fin actualizar

                    if (e.CommandName == "Eliminar")
                    {
                        this.IdEvento.Value = t.ToString();

                        Int64 Id = 0;

                        if (!Int64.TryParse(this.IdEvento.Value, out Id))
                        {
                            Id = 0;
                        }

                        objEvento = new MantenimientoEventos();
                        objEvento.Id = Id;
                        if (objEvento.Delete(out OError))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Evento inactivado con éxito {0} </b>", ""));
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo inactivar el evento:{0}-{1} </b>", Id, OError));
                        }

                        this.Carga_Eventos();
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

                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                

                if (!Page.IsPostBack)
                {
                   
                    this.Carga_Eventos();
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }


   
     
    }

}