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
  

    public partial class appPaquetesEventos : System.Web.UI.Page
    {

        #region "Clases"

        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private MantenimientoPaquetes appPaquete = new MantenimientoPaquetes();
        private MantenimientoPaquetesEventos objCab = new MantenimientoPaquetesEventos();
        private DetalleEventos objDetalle = new DetalleEventos();

        #endregion

        #region "Variables"

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
            UPCARGA.Update();
           

        }


        private void Limpia_Campos()
        {
          
            this.CboPaquete.SelectedIndex = 0;
            this.TxtDescPaquete.Text = string.Empty;
            
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

        private void Carga_Paquetes()
        {
            try
            {
                DataSet dsRetorno = new DataSet();

                List<MantenimientoPaquetes> Listado = MantenimientoPaquetes.Combo_Paquetes( out OError);

                this.CboPaquete.DataSource = Listado;
                this.CboPaquete.DataTextField = "Name";
                this.CboPaquete.DataValueField = "Id";
                this.CboPaquete.DataBind();


            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Paquetes), "Carga_Paquetes", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

            }

        }


        private void Carga_Eventos()
        {
            try
            {
                objCab = Session["PaquetesEventos" + this.hf_BrowserWindowName.Value] as MantenimientoPaquetesEventos;
                objCab.Detalle.Clear();

                List<MantenimientoEventos> Listado = MantenimientoEventos.Listado_Eventos(out OError);
                if (Listado != null)
                {
                    foreach (var Det in Listado)
                    {
                        objDetalle = new DetalleEventos();
                        objDetalle.Id = Det.Id;
                        objDetalle.Name = Det.Name;
                        objDetalle.Create_user = Det.Create_user;
                        objDetalle.Create_date = Det.Create_date;
                        objDetalle.Modifie_user = Det.Modifie_user;
                        objDetalle.Modifie_date = Det.Modifie_date;
                        objDetalle.Check = false;

                        objCab.Detalle.Add(objDetalle);
                    }

                    tablePagination.DataSource = objCab.Detalle;
                    tablePagination.DataBind();

                    Session["PaquetesEventos" + this.hf_BrowserWindowName.Value] = objCab;
                   
                   
                }
                else
                {
                    tablePagination.DataSource = null;
                    tablePagination.DataBind();
                  
                }

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Eventos), "Carga_Paquetes", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

            }

        }

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out OError);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", OError));
                return;
            }

            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCab = new MantenimientoPaquetesEventos();
            Session["PaquetesEventos" + this.hf_BrowserWindowName.Value] = objCab;
        }

        public void Carga_PaquetesEventos()
        {
            try
            {
                Int64 Id = 0;
              
                if (!Int64.TryParse(this.CboPaquete.SelectedValue.ToString(), out Id))
                {
                    Id = 0;
                }


                objCab = Session["PaquetesEventos" + this.hf_BrowserWindowName.Value] as MantenimientoPaquetesEventos;
                
                List<MantenimientoEventos> Listado = MantenimientoEventos.Listado_PaquetesEventos(Id, out OError);
                if (Listado != null)
                {
                    foreach (var Det in Listado)
                    {
                        var Detalle = objCab.Detalle.FirstOrDefault(f => f.Id.Equals(Det.Id));
                        if (Detalle != null)
                        {
                            Detalle.Check = true;

                        }
                       
                    }

                    tablePagination.DataSource = objCab.Detalle;
                    tablePagination.DataBind();

                    Session["PaquetesEventos" + this.hf_BrowserWindowName.Value] = objCab;


                }
               
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_PaquetesEventos), "Carga_PaquetesEventos", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

            }

        }
        #endregion



        #region "Eventos del Formulario"
        protected void CboPaquete_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (this.CboPaquete.SelectedIndex != -1)
                {
                    this.Carga_PaquetesEventos();

                    Int64 Id = 0;

                    if (!Int64.TryParse(this.CboPaquete.SelectedValue.ToString(), out Id))
                    {
                        Id = 0;
                    }


                    appPaquete = new MantenimientoPaquetes();
                    appPaquete.Id = Id;
                    if (appPaquete.PopulateMyData(out OError))
                    {
                        this.TxtDescPaquete.Text = appPaquete.EventoN4;
                    }
                  
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(CboPaquete_SelectedIndexChanged), "CboPaquete_SelectedIndexChanged", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

            }
           
        }

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
                    this.Carga_Eventos();




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

                    if (this.CboPaquete.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor seleccionar el paque a relacionar eventos"));
                        this.CboPaquete.Focus();
                        return;
                    }

                    if (this.CboPaquete.SelectedIndex == 0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor seleccionar el paque a relacionar eventos"));
                        this.CboPaquete.Focus();
                        return;
                    }

                    objCab = Session["PaquetesEventos" + this.hf_BrowserWindowName.Value] as MantenimientoPaquetesEventos;
                    if (objCab == null)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Debe generar la consulta, para poder asignar eventos </b>"));
                        return;
                    }
                    if (objCab.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe detalle de eventos para poder asignar</b>"));
                        return;
                    }

                    var LinqList = (from p in objCab.Detalle.Where(x => x.Check == true)
                                              select p.EventoN4).ToList();

                    if (LinqList.Count == 0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Debe seleccionar los eventos para asignar al paquete </b>"));
                        return;
                    }

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    Int64 Id = 0;
                   

                    if (!Int64.TryParse(this.CboPaquete.SelectedValue.ToString(), out Id))
                    {
                        Id = 0;
                    }

                    var Linq = (from p in objCab.Detalle.Where(x => x.Check == true)
                                select new
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Create_user = ClsUsuario.loginname.Length > 10 ? ClsUsuario.loginname.Substring(0, 10) : ClsUsuario.loginname,
                                    Check = p.Check
                                }
                                ).ToList();
                                            

                    objCab = new MantenimientoPaquetesEventos();
                    objCab.PackageId = Id;
                    objCab.Create_user = ClsUsuario.loginname.Length > 10 ? ClsUsuario.loginname.Substring(0,10) : ClsUsuario.loginname;

                    foreach (var Det in Linq)
                    {
                        objDetalle = new DetalleEventos();
                        objDetalle.Id = Det.Id;
                        objDetalle.Name = Det.Name;
                        objDetalle.Create_user = Det.Create_user;
                        objDetalle.Check = Det.Check;
                        objDetalle.PackageId = Id;
                        objDetalle.EventsId = Det.Id;
                        objCab.Detalle.Add(objDetalle);

                       
                    }


                    var nIdRegistro = objCab.SaveTransaction(out OError);

                    if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                    {

                        this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo grabar paquetes y eventos: {0}, {1}</b>", this.TxtDescPaquete.Text.Trim(), OError));
                        return;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Se procedió con la asignación de eventos al paquete {0}",this.TxtDescPaquete.Text.Trim()));
                       
                    }

                    this.Limpia_Campos();
                    this.Carga_Eventos();

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
        protected void Check_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                Label LblId = (Label)item.FindControl("LblId");

                Int64 Id = 0;


                if (!Int64.TryParse(LblId.Text, out Id))
                {
                    Id = 0;
                }


                objCab = Session["PaquetesEventos" + this.hf_BrowserWindowName.Value] as MantenimientoPaquetesEventos;
                var Detalle = objCab.Detalle.FirstOrDefault(f => f.Id == Id);
                if (Detalle != null)
                {
                    Detalle.Check = chkPase.Checked;

                }

                tablePagination.DataSource = objCab.Detalle;
                tablePagination.DataBind();

                Session["PaquetesEventos" + this.hf_BrowserWindowName.Value] = objCab;


            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Check_CheckedChanged), "Check_CheckedChanged", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

            }
        }


        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                   


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

                Server.HtmlEncode(this.TxtDescPaquete.Text.Trim());
                

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();

                    this.Carga_Paquetes();
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