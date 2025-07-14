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
using BillionEntidades;
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
    public partial class liberacion_pase_zal : System.Web.UI.Page
    {
     
        private Cls_Bil_Libera_Pases_Zal objZal = new Cls_Bil_Libera_Pases_Zal();
        public static string v_mensaje = string.Empty;

        private void Limpiar()
        {
            this.TxtContenedor.Text = string.Empty;
            this.TxtMotivo.Text = string.Empty;
            this.TxtNumero.Text = string.Empty;

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
            this.TxtMotivo.Text = Server.HtmlEncode(this.TxtMotivo.Text);
          

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
               
                this.Limpiar();
              
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
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TxtMotivo.Text))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Debe ingresar el motivo");
                        sinresultado.Visible = true;
                        return;
                    }

                    Int64 Id_PPZal = 0;
                    int nLiberados = 0;

                    foreach (RepeaterItem item in tablePagination.Items)
                    {
                        
                        Label txtpase = item.FindControl("txtpase") as Label;

                        if (!Int64.TryParse(txtpase.Text, out Id_PPZal))
                        {
                           
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-info";
                            this.sinresultado.InnerText = "Número de pase de la zal no es valido, para grabar";
                            sinresultado.Visible = true;
                            return;
                        }

                       
                        objZal = new Cls_Bil_Libera_Pases_Zal();
                        objZal.id_ppzal = Id_PPZal;
                        objZal.IV_USUARIO_CREA = user.loginname;
                        objZal.comentario = this.TxtMotivo.Text.Trim();
                        objZal.Update(out v_mensaje);

                        if (v_mensaje != string.Empty)
                        {
                            sinresultado.Attributes["class"] = string.Empty;
                            sinresultado.Attributes["class"] = "msg-critico";
                            this.sinresultado.InnerText = string.Format("Error al liberar pase de puerta # {0}-{1}", Id_PPZal, v_mensaje);
                            sinresultado.Visible = true;
                            return;

                        }
                        else
                        {
                            nLiberados++;
                        }

                    }

                    if (nLiberados == 0)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        sinresultado.InnerText = string.Format("No existen pases de puerta zal para su liberación de pago");
                        sinresultado.Visible = true;
                        return;
                    }
                    else
                    {

                        this.Limpiar();
                        this.tablePagination.DataSource = null;
                        this.tablePagination.DataBind();
                        xfinder.Visible = false;
                        this.botones.Visible = false;
                        this.motivo.Visible = false;

                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        sinresultado.InnerText = string.Format("Se realizaron la liberación de {0} pases de puerta zal con éxito", nLiberados);
                        sinresultado.Visible = true;
                        return;

                    }

                   

                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la liberación de pases, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "liberacion_pase_zal", "btnprocesar_Click", "Hubo un error al grabar", t.loginname));
                    sinresultado.Visible = true;
                    this.botones.Visible = false;
                   
                    
                }
            }
        }


        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
           
        }

       private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
        
            Int64 Id_PPZal = 0;
            string Liquidacion = string.Empty;
            Int64? Id_PPZal_New = 0;

            string vr = string.Empty;
            try
           {
              

                if (string.IsNullOrEmpty(TxtNumero.Text))
                {
                    this.TxtNumero.Text = "";
                    Id_PPZal = 0;
                    
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


                if (string.IsNullOrEmpty(this.TxtContenedor.Text) && string.IsNullOrEmpty(TxtNumero.Text))
                {
                   
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    this.sinresultado.InnerText = string.Format("Debe ingresar el número de liquidación o pase a liberar");
                    sinresultado.Visible = true;
                    return;
                }

                if (string.IsNullOrEmpty(this.TxtContenedor.Text))
                {

                    Liquidacion = null;
                }
                else
                {
                    Liquidacion = this.TxtContenedor.Text;
                }

                if (Id_PPZal == 0)
                {
                    Id_PPZal_New = null;
                }
                else
                {
                    Id_PPZal_New = Id_PPZal;
                }

             

                var table = Cls_Bil_Libera_Pases_Zal.Listado_Pagos_Terceros(Id_PPZal_New, Liquidacion, out vr);
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

                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "";
                this.sinresultado.InnerText = "";
                sinresultado.Visible = false;

                Session["resultado"] = table;
               this.tablePagination.DataSource = table;
               this.tablePagination.DataBind();
               xfinder.Visible = true;

               this.botones.Visible = true;
               this.motivo.Visible = true;

           
                this.UdBotones.Update();

                

            }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
                vr = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "buscarpases", "populate()", "Hubo un error al CARGAR DATOS", t != null ? t.loginname : "Nologin"));
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