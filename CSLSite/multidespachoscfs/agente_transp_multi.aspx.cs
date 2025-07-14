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
  

    public partial class agente_transp_multi : System.Web.UI.Page
    {

        #region "Clases"

        usuario ClsUsuario;
       
        private cfs_agente_transportista Objeto = new cfs_agente_transportista();

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
           
            this.TxtEmpresaTransporte.Text = string.Empty;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "clearTextBox();", true);

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



        private void Carga_Transportistas()
        {
            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                List<cfs_agente_transportista> Listado = cfs_agente_transportista.Listado_Transportistas(ClsUsuario.ruc.Trim(),out OError);

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
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Carga_Transportistas), "Carga_Transportistas", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

            }

        }


        #endregion

        #region "Metodos Web Services"
        [System.Web.Services.WebMethod]
        public static string[] GetEmpresas(string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Transportista = N4.Entidades.CompaniaTransporte.ObtenerCompanias(ClsUsuario.loginname, prefix);
                if (Transportista.Exitoso)
                {
                    var LinqQuery = (from Tbl in Transportista.Resultado.Where(Tbl => Tbl.ruc != null)
                                     select new
                                     {
                                         EMPRESA = string.Format("{0} - {1}", Tbl.ruc.Trim(), Tbl.razon_social.Trim()),
                                         RUC = Tbl.ruc.Trim(),
                                         NOMBRE = Tbl.razon_social.Trim(),
                                         ID = Tbl.ruc.Trim()
                                     });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }

                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
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

                   

                    if (string.IsNullOrEmpty(this.TxtEmpresaTransporte.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingrese la empresa de transporte"));
                        this.TxtEmpresaTransporte.Focus();
                        return;
                    }

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    string IdEmpresa = string.Empty;
                    string DesEmpresa = string.Empty;
                    string EmpresaSelect = string.Empty;
                    //valida que exista Empresa Transporte 
                    if (!string.IsNullOrEmpty(this.TxtEmpresaTransporte.Text))
                    {
                        EmpresaSelect = this.TxtEmpresaTransporte.Text.Trim();
                        if (EmpresaSelect.Split('-').ToList().Count > 1)
                        {

                            IdEmpresa = EmpresaSelect.Split('-').ToList()[0].Trim();

                            DesEmpresa = (EmpresaSelect.Split('-').ToList().Count >= 3 ? string.Format("{0} - {1}", EmpresaSelect.Split('-').ToList()[1].Trim(), EmpresaSelect.Split('-').ToList()[2].Trim()) : EmpresaSelect.Split('-').ToList()[1].Trim());

                            var EmpresaTransporte = N4.Entidades.CompaniaTransporte.ObtenerCompania(ClsUsuario.loginname, IdEmpresa);
                            if (!EmpresaTransporte.Exitoso)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una compañía de transporte valida para agregar la información </b>"));
                                this.Txtempresa.Focus();
                                return;
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una compañía de transporte valida para agregar la información </b>"));
                            this.Txtempresa.Focus();
                            return;
                        }
                    }



                    Objeto = new cfs_agente_transportista();
                    Objeto.IDUSUARIO = ClsUsuario.id;
                    Objeto.RUC_AGENTE = ClsUsuario.ruc.Trim();
                    Objeto.DESC_AGENTE = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos); ;
                    Objeto.RUC_TRANSPORTE = IdEmpresa;
                    Objeto.DESC_TRANSPORTE = DesEmpresa;
                    Objeto.USUARIO_ING = ClsUsuario.loginname;

                  
                    var nIdRegistro = Objeto.Save(out OError);

                    if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                    {

                        this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo asignar empresa de transporte: {0}, {1}</b>", EmpresaSelect, OError));
                        return;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Se procedió con la asignación de la empresa de transporte  {0}" , EmpresaSelect));
                       
                    }

                    this.Limpia_Campos();
                    this.Carga_Transportistas();
                    this.TxtEmpresaTransporte.Focus();

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

                   
                    if (e.CommandName == "Eliminar")
                    {
                        //this.IdEvento.Value = t.ToString();

                        Int64 Id = 0;

                        if (!Int64.TryParse(t, out Id))
                        {
                            Id = 0;
                        }

                        Objeto = new cfs_agente_transportista();
                        Objeto.ID = Id;
                        Objeto.USARIO_MOD = ClsUsuario.loginname;
                        if (Objeto.Delete(out OError))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Empresa de Transporte inactivada con éxito {0} </b>", ""));
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo inactivar la Empresa de Transporte:{0}-{1} </b>", Id, OError));
                        }

                        this.Carga_Transportistas();
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

#if !DEBUG
                this.IsAllowAccess();
#endif


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

                    this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                    this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                    this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);

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

               // Server.HtmlEncode(this.TXTMRN.Text.Trim());
                

                if (!Page.IsPostBack)
                {
                   
                    this.Carga_Transportistas();
                }
                else
                {
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    if (ClsUsuario != null)
                    {
                        this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                        this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                        this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                    }

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }


   
     
    }

}