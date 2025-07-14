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

namespace CSLSite
{
    public partial class consola_impo : System.Web.UI.Page
    {
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
#if !DEBUG
            this.IsAllowAccess();
#endif
            Page.Tracker();
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
             
                this.txtruc.Text =Server.HtmlEncode(this.txtruc.Text);
                this.txtnombre.Text =Server.HtmlEncode(this.txtnombre.Text);
              
             
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                xfinder.Visible = IsPostBack;
                sinresultado.Visible = false;
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
                    populate();
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consolasna", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
        public static string anulado(object estado)
        {
            if (estado == null)
            {
                return "<span>sin estado!</span>";
            }
            if (estado.ToString().ToLower() == "true")
            {
                return "<span>Activo</span>";
            }
            else
            {
                return "<span class='red' >Inactivo</span>";
            }
            
        }
        public static string boton(object estado)
        { 
          return  estado.ToString().ToLower() == "true" ? "ver":"xver";
        }
        public static string botonText(object estado)
        {
            return estado.ToString().ToLower() == "true" ? "Inactivar" : "Activar";
        }
        public static string securetext(object number)
        {
            if (number == null )
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
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
                        //sinresultado.Attributes["class"] = string.Empty;
                        //sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consolasna", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        return;
                    }
                    if (e.CommandArgument == null)
                    {
                        //sinresultado.Attributes["class"] = string.Empty;
                        //sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consolasna", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    TextBox Txtcomentario =  e.Item.FindControl("Txtcomentario") as TextBox;

                    if (string.IsNullOrEmpty(Txtcomentario.Text))
                    {
                        
                        sinresultado.InnerText = "Debe ingresar un comentario";
                        sinresultado.Visible = true;
                        return;
                    }

                    var xpars = e.CommandArgument.ToString().Split(';');
                    //aqui el metodo para cambiar el estado del registro directo sin verlo
                    var us = new app_start.SNA_Usuarios();
                    us.cliente_estado = xpars[1] == "True" ? false : true;
                    us.cliente_categoria = xpars[2];
                    us.notas = Txtcomentario.Text;

                    if (!string.IsNullOrEmpty(xpars[0]))
                    {
                        us.cliente_modificadopor = user.loginname;
                        int idd = 0;
                        if (int.TryParse(xpars[0], out idd))
                        {
                            us.cliente_id = idd;
                        }
                    }
                    else
                    {
                        us.cliente_creadopor = user.loginname;
                    }
                    var rr = us.Guardar_App();
                    if (rr < 0)
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        this.sinresultado.InnerText = "Ha ocurrido una excepcion al guardar, favor intente mas tarde";
                        return;

                    }
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    sinresultado.Visible = true;
                    populate();
                    sinresultado.InnerText = string.Format("La actualizacion del registro  No.{0} ha resultado exitosa.", xpars[0]);
                }
                catch (Exception ex)
                {
                     var t = this.getUserBySesion();
                     sinresultado.Attributes["class"] = string.Empty;
                     sinresultado.Attributes["class"] = "msg-critico";
                     sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la actualizacion, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consolasna", "Item_comand", "Hubo un error al anular", t.loginname));
                     sinresultado.Visible = true;

                }
            }
        }
        protected string jsarguments(object id, object estado, object categoria)
        {
            return string.Format("{0};{1};{2}", id != null ? id.ToString().Trim() : "0", estado != null ? estado.ToString().Trim() : "NI", categoria != null ? categoria.ToString().Trim() : "Fail");
        }
        private void populate()
       {
         
           Session["resultado"] = null;
           var table = new Catalogos.sna_buscar_clientesDataTable();
           var ta = new CatalogosTableAdapters.sna_buscar_clientesTableAdapter();
           try
           {

           
   
               var user = Page.getUserBySesion();
               ta.ClearBeforeFill = true;
               //user.loginname = "botrosa";
               ta.Fill(table, this.txtruc.Text, this.txtnombre.Text);
               if (table.Rows.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise los campos";
                   sinresultado.Visible = true;
                   return;
               }

               Session["resultado"] = table;
               this.tablePagination.DataSource = table;
               this.tablePagination.DataBind();
               xfinder.Visible = true;
           }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
               sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consolasna", "populate", "Hubo un error al buscar", t.loginname));
               sinresultado.Visible = true;
           }
           finally
           {
               ta.Dispose();
               table.Dispose();
           }
       }
    }
}