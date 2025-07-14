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

namespace CSLSite.backoffice
{


    public partial class backoffice_registra_dae : System.Web.UI.Page
    {

        #region "Clases"


        usuario ClsUsuario;
        private Cls_Registra_Dae obj = new Cls_Registra_Dae();

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
       
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;

        private string TipoServicio = string.Empty;

        private string LoginName = string.Empty;
        

        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
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
           
            this.UPMensaje.Update();
            this.UPDATOSCLIENTE.Update();
           // this.UPAGENTE.Update();

        }


        //private void OcultarLoading(string valor)
        //{
        //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('"+valor+"');", true);
        //}

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
               // OcultarLoading("1");
            }
           
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;          
            this.banmsg.Visible = false;
            this.Actualiza_Paneles();
           // OcultarLoading("1");
          
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

            this.banmsg.Visible = IsPostBack;
         

            if (!Page.IsPostBack)
            {

                this.banmsg.InnerText = string.Empty;
                

                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {

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


                Server.HtmlEncode(this.TxtDae.Text.Trim());
                Server.HtmlEncode(this.TxtRucExportador.Text.Trim());
                Server.HtmlEncode(this.TxtDescExportador.Text.Trim());

                Server.HtmlEncode(this.TxtCantidad.Text.Trim());
                Server.HtmlEncode(this.TxtRuta1.Text.Trim());

                if (!Page.IsPostBack)
                {

                    this.TxtDae.Focus();

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

 
        protected void BtnAsumir_Click(object sender, EventArgs e)
        {
            
            if (Response.IsClientConnected)
            {
                try
                {
                   // OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                       // OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtDae.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor debe ingresar el # de la DAE"));
                        this.TxtDae.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtRucExportador.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor debe ingresar el RUC del exportador"));
                        this.TxtRucExportador.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtDescExportador.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor debe ingresar el nombre del exportador"));
                        this.TxtDescExportador.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtCantidad.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor debe ingresar la cantidad"));
                        this.TxtCantidad.Focus();
                        return;
                    }

                    int Cantidad = 0;


                    if (!int.TryParse(this.TxtCantidad.Text, out Cantidad))
                    {
                        Cantidad = 0;
                    }

                    if (Cantidad==0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor debe ingresar la cantidad"));
                        this.TxtCantidad.Focus();
                        return;
                    }


                    //if (string.IsNullOrEmpty(this.TxtRuta1.Text))
                    //{
                    //    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor debe cargar un archivo pdf de soporte de registro"));
                    //    this.TxtRuta1.Focus();
                    //    return;
                    //}

                    //if (string.IsNullOrEmpty(this.ruta_completa.Value))
                    //{
                    //    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor debe cargar un archivo pdf de soporte de registro"));
                    
                    //    this.TxtRuta1.Focus();
                    //    return;
                    //}

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    obj = new Cls_Registra_Dae();
                    obj.dae = this.TxtDae.Text.Trim();
                    obj.ruc = this.TxtRucExportador.Text.Trim();
                    obj.empresa = this.TxtDescExportador.Text.Trim();
                    obj.qty = Cantidad;
                    obj.tipo = this.CboTipo.SelectedValue.ToString();
                    obj.usuario = ClsUsuario.loginname;
                    obj.ruta_pdf = string.IsNullOrEmpty(this.ruta_completa.Value) ? "" : this.ruta_completa.Value ;

                    var nIdRegistro = obj.Save(out OError);

                    if (!nIdRegistro.HasValue || nIdRegistro.Value <= 0)
                    {
                        //OcultarLoading("1");
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Se presentaron las siguientes observaciones: {0}", OError));
                        return;
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-info'></i><b> Informativo! </b>DAE registrada con éxito..."));
                    }

                   // OcultarLoading("1");

                    this.TxtDae.Text = string.Empty;
                    this.TxtRucExportador.Text = string.Empty;
                    this.TxtDescExportador.Text = string.Empty;
                    this.TxtCantidad.Text = string.Empty;
                    this.TxtRuta1.Text = string.Empty;
                    this.ruta_completa.Value = string.Empty;

                    this.Actualiza_Paneles();

                }
                catch (Exception ex)
                {
                 
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAsumir_Click), " Aduanas.Entidades.AduanaRidt", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
            }



            
        }


    }
}