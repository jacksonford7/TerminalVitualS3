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


    public partial class brbk_excluir_tipo_producto : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;
        private Cls_Bil_PasePuertaCFS_Cabecera objCas = new Cls_Bil_PasePuertaCFS_Cabecera();
        private Cls_Bil_CasManual objDetalleCas = new Cls_Bil_CasManual();
        private string cMensajes;

        private brbk_turnos_cab objCab = new brbk_turnos_cab();
        private brbk_turnos_det objDet = new brbk_turnos_det();

        private brbk_excluir_productos objTipo = new brbk_excluir_productos();

        #endregion

        #region "Variables"

       
        private string LoginName = string.Empty;
     
        private static Int64? lm = -3;
        private string OError;

        #endregion

        #region "Propiedades"

    


        #endregion

        #region "Metodos"

       
        private void Actualiza_Paneles()
        {         
            this.UPCARGA.Update();
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

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)//cabecera
            {
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("1");
            }
           
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;          
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
          
        }

       

        private void Carga_CboTipoProducto()
        {
            try
            {
                List<brbk_tipoproducto> Listado = brbk_tipoproducto.CboTipoProducto(false,out cMensajes);

                this.CboTipoProducto.DataSource = Listado;
                this.CboTipoProducto.DataTextField = "DESCRIPCION";
                this.CboTipoProducto.DataValueField = "CODIGO";
                this.CboTipoProducto.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "brbk_generar_turnos", "Carga_CboTipoProducto", "Hubo un error al cargar tipo productos", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
        }


        private void Carga_Lista()
        {
            try
            {
                List<brbk_excluir_productos> Listado = brbk_excluir_productos.brbk_Excluir_Producto( out cMensajes);

                this.tablePagination.DataSource = Listado;
               
                this.tablePagination.DataBind();

            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema , por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "brbk_excluir_tipo_producto", "Carga_Lista", "Hubo un error al cargar tipo productos", t.loginname));
                this.Mostrar_Mensaje(1, Error);
            }
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

            this.IsAllowAccess();

            this.banmsg.Visible = IsPostBack;

         

            if (!Page.IsPostBack)
            {

                this.banmsg.InnerText = string.Empty;

                ClsUsuario = Page.Tracker();
                
            }

        }

       

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

               
                if (!Page.IsPostBack)
                {     
               
                    
                    this.Carga_CboTipoProducto();
                    this.Carga_Lista();
                }
                else
                {
                  

                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

 
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            
            if (Response.IsClientConnected)
            {
                try
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                   
                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                  

                    if (this.CboTipoProducto.SelectedIndex == -1)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor seleccionar el tipo de producto a excluir"));
                        this.CboTipoProducto.Focus();
                        return;
                    }



                    Int64 ID = 0;
                    if (!Int64.TryParse(this.CboTipoProducto.SelectedValue, out ID))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor seleccione el tipo de producto"));
                        this.CboTipoProducto.Focus();
                        return;
                    }

                    objTipo = new brbk_excluir_productos();
                    objTipo.ID = ID;
                    if (objTipo.Existe_Producto(out cMensajes))
                    {
                        this.CboTipoProducto.Focus();
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>ya se encuentra excluido el tipo de producto: {0}", objTipo.DESCRIPCION));
                        return;

                    }


                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    objTipo = new brbk_excluir_productos();
                    objTipo.ID = ID;
                    objTipo.USUARIO_CREA = ClsUsuario.loginname;
                    objTipo.Save(out cMensajes);

                    if (cMensajes != string.Empty)
                    {
                        
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>No se puede exluir un tipo de producto: {0}", cMensajes));
                        return;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Tipo de producto excluido con éxito..{0}", cMensajes));
                        this.Carga_Lista();

                    }


                    this.Actualiza_Paneles();

                    //this.Ocultar_Mensaje();

                }
                catch (Exception ex)
                {
                 
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnBuscar_Click), "BtnBuscar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }    
        }

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

                    if (e.CommandName == "Eliminar")
                    {
                       

                        Int64 SECUENCIA = Int64.Parse(t.ToString());

                        objTipo = new brbk_excluir_productos();
                        objTipo.ID = SECUENCIA;

                        objTipo.Delete(out cMensajes);
                        if (cMensajes != string.Empty)
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar información del tipo de producto a eliminar: {0} </b>", t.ToString()));
                            return;
                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Tipo de producto incluido con éxito..{0}", cMensajes));
                            this.Carga_Lista();

                        }


                        this.Actualiza_Paneles();

                    }

                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(tablePagination_ItemCommand), "tablePagination_ItemCommand", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));

                    

                }
            }

        }


       

       


      
   }
}