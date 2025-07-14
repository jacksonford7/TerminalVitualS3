using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CSLSite.N4Object;
using ConectorN4;
using System.Globalization;
using ControlOPC.Entidades;
using ClsProformas;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Web.Script.Services;
using csl_log;
using System.Data;

using System.Web.Services;
using System.IO;
using System.Configuration;
using System.Collections;

namespace CSLSite
{
    public partial class subirarchivo : System.Web.UI.Page
    {
     
        private Archivos_Zal objZal = new Archivos_Zal();
          
        private Int64 Id_Pase
        {
            get
            {
                return (Int64)Session["Id_Pase"];
            }
            set
            {
                Session["Id_Pase"] = value;
            }

        }

      
        private GetFile.Service getFile = new GetFile.Service();
        private DataTable dtDocumentos = new DataTable();
        private String cRutaArchivo = string.Empty;

        private string xmlDocumentos
        {
            get
            {
                return (string)Session["xmlDocumentos"];
            }
            set
            {
                Session["xmlDocumentos"] = value;
            }

        }

    

        private void Limpiar()
        {
          
            this.Id_Pase = 0;
          
            xmlDocumentos = null;
            this.TxtArchivo.Text = null;

        }

       


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //this.IsAllowAccess();
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "~/login.aspx", true);
            }

            Page.Tracker();
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }

            this.TxtNumero.Text =Server.HtmlEncode(this.TxtNumero.Text);
            this.TxtContenedor.Text = Server.HtmlEncode(this.TxtContenedor.Text);
            this.desded.Text = Server.HtmlEncode(this.desded.Text);
            this.hastad.Text = Server.HtmlEncode(this.hastad.Text);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                xfinder.Visible = IsPostBack;
                sinresultado.Visible = false;
            }

            if (!IsPostBack)
            {
                /*oculta paneles de motivo de anulacion*/
                this.botones.Visible = false;
                this.motivo.Visible = false;
                this.BtnAgregar.Visible = false;
                this.LblRuta.Visible = false;
                this.TxtArchivo.Enabled = false;
                this.Limpiar();
                this.desded.Text = DateTime.Today.ToString("dd/MM/yyyy");
                this.hastad.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
            
        }
        protected void btbuscar_Click(object sender, EventArgs e)
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
                        return;
                    }

                    this.botones.Visible = false;
                    this.motivo.Visible = false;

                    this.UdBotones.Update();

                    this.Limpiar();

                    populate();
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_aprobar_nota_credito", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }


        protected void btnprocesar_Click(object sender, EventArgs e)
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
                        return;
                    }

                    var user = Page.getUserBySesion();
                    if (user == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "btnprocesar_Click", "btnprocesar_Click", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        this.botones.Visible = false;
                        this.motivo.Visible = false;
                        this._nc_id.Value = string.Empty;
                        return;
                    }

                    if (this.TxtArchivo.Text.Trim() == string.Empty)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Debe seleccionar el archivo a guardar..");
                        sinresultado.Visible = true;
                        return;
                    }


                    objZal = new Archivos_Zal();
                        objZal.id_ppzal = this.Id_Pase;
                        objZal.usr_ing_archivo = user.loginname;
                       
                        if (this.xmlDocumentos == null)
                        {
                            objZal.ruta_documento = string.Empty;
                        }
                        else
                        {
                            objZal.ruta_documento = this.xmlDocumentos;

                        }

                        if (objZal.Grabar_Archivo())
                        {

                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-info";
                            sinresultado.InnerText = string.Format("Subida de archivo del pase puerta zal No. {0} ,realizada con éxito.", this.Id_Pase.ToString());
                            sinresultado.Visible = true;
                            this.TxtArchivo.Text = string.Empty;
                            this.UdBotones.Update();
                        }
                        else
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            sinresultado.InnerText = string.Format("ERROR AL realizar subida de archivo del pase puerta zal No.  {0}", this.Id_Pase.ToString());
                            sinresultado.Visible = true;
                            return;

                        }
                    

                    populate();

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "frm_aprobar_nota_credito", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                    this.botones.Visible = false;
                   
                    
                }
            }
        }


        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                    var user = Page.getUserBySesion();
                    if (user == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "subirarchivo", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        return;
                    }
                   
                    populate();

                }
                catch (Exception ex)
                {
                     var t = this.getUserBySesion();
                     sinresultado.Attributes["class"] = string.Empty;
                     sinresultado.Attributes["class"] = "msg-critico";
                     sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la anulación, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Item_comand", "Hubo un error al anular", t.loginname));
                     sinresultado.Visible = true;

                }
            }
        }

       private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
        
            Int64 Id_PPZal = 0;
            string vr = string.Empty;
            try
           {
              

                if (string.IsNullOrEmpty(TxtNumero.Text))
                {
                    this.TxtNumero.Text = "";
                    Id_PPZal = 0;
                    this.Id_Pase = 0;
                }
                else
                {
                    if (!Int64.TryParse(this.TxtNumero.Text, out Id_PPZal))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "Número de pase de la zal no es valido..";
                        sinresultado.Visible = true;
                        return;
                    }
                }

                if (string.IsNullOrEmpty(TxtContenedor.Text))
                {
                   
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = string.Format("Debe ingresar el número de contenedor a buscar");
                    sinresultado.Visible = true;
                    return;
                }

                DateTime fechadesde = new DateTime();
                DateTime fechahasta = new DateTime();
                if (!string.IsNullOrEmpty(desded.Text))
                {
                    if (!DateTime.TryParseExact(desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechadesde))
                    {

                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = string.Format("El formato de la fecha desde debe ser: dia/Mes/Anio {0}", desded.Text);
                        sinresultado.Visible = true;
                        return;

                    }
                }
                if (!string.IsNullOrEmpty(hastad.Text))
                {
                    if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechahasta))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = string.Format("El formato de la fecha hasta debe ser: dia/Mes/Anio {0}", hastad.Text);
                        sinresultado.Visible = true;
                        return;
                    }
                }

                TimeSpan tsDias = fechahasta - fechadesde;
                int diferenciaEnDias = tsDias.Days;
                if (diferenciaEnDias < 0)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = string.Format("La Fecha de Ingreso: {0}, NO deber ser mayor a la Fecha final {1}", desded.Text, hastad.Text);
                    sinresultado.Visible = true;
                    return;

                }
                if (diferenciaEnDias > 31)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = string.Format("Solo puede consultar las solicitudes de hasta un mes.");
                    sinresultado.Visible = true;
                    return;
                }

                this.Id_Pase = Id_PPZal;

                var user = Page.getUserBySesion();
               
                var table = Archivos_Zal.Detalle_Pases_Subir_Archivo (Id_PPZal, fechadesde, fechahasta, TxtContenedor.Text, out vr);
                if (table == null)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = vr;
                    sinresultado.Visible = true;
                    return;
                }
                if (table.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise los parámetros";
                   sinresultado.Visible = true;
                   return;
               }


               Session["resultado"] = table;
               this.tablePagination.DataSource = table;
               this.tablePagination.DataBind();
               xfinder.Visible = true;

               this.botones.Visible = true;
               this.motivo.Visible = true;

                this.BtnAgregar.Visible = true;
                this.LblRuta.Visible = true;
                this.TxtArchivo.Enabled = true;

                this.UdBotones.Update();

                foreach (var t in table)
                {
                    this.Id_Pase = t.id_ppzal;
                }

            }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
                vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "subirarchivo", "populate()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
                sinresultado.Visible = true;
           }

       }

     
        #region "metodos_repeater"

        public static string formatPro(object id)
        {
            Int64 idm = 0;
            if (id != null)
            {
                if (Int64.TryParse(id.ToString(), out idm))
                {
                    return idm.ToString("D8");
                }
            }
            return "undefined";
        }
        public static string anulado(object estado)
        {
            if (estado == null)
            {
                return "<span>sin estado!</span>";
            }
            var es = estado.ToString();
            es = es.ToLower();

            if (es.Equals("n")) {
                return "<span class='azul' >Generada</span>";
            }
            if (es.Equals("f"))
            {
                return "<span class='naranja' >Facturada</span>";
            }
            if (es.Equals("a"))
            {
                return "<span class='red' >Anulada</span>";
            }
            return "<span>sin estado!</span>";
        }
        public static bool boton(object estado)
        {
            var t = estado as string;
            if (!string.IsNullOrEmpty(t))
            {
                if (t.ToLower().Contains("a") || t.ToLower().Contains("f"))
                {
                    return false;
                }
            }

            return true;
        }
        public static string securetext(object number)
        {
            if (number == null )
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }
        public static string formatProDate(object fecha)
        {
            DateTime dt;
            if (fecha != null)
            {
                if (DateTime.TryParse(fecha.ToString(), out dt))
                {
                    return dt.ToString("dd/MM/yyyy HH:mm");
                }
            }

            return "undefined";
        }
        public static string xver(object est)
        {
            if (est != null)
            {
                return est.ToString().ToLower().Equals("a") ? "ocultar" : "mostrar";
            }
            return null;
        }


        #endregion
    }
}