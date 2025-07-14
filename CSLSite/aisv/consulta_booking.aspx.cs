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
    public partial class consultabook : System.Web.UI.Page
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
             
                this.bonum.Text =Server.HtmlEncode(this.bonum.Text);
               // this.cntrn.Text =Server.HtmlEncode(this.cntrn.Text);
              
                this.desded.Text = Server.HtmlEncode(this.desded.Text);
                this.desded.Text = Server.HtmlEncode(this.hastad.Text);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
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
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }


        public static string TipoB(object esy)
        {
            if (esy == null)
            {
                return "<span>Sin definir!</span>";
            }
            if (esy.ToString().Contains("1"))
            {
                return "<span>SI</span>";
            }
            else
            {
                return "<span>NO</span>";
            }

        }


            public static string NaveEstado(object estado)
        {
            DateTime fce;
            if (estado == null)
            {
                return "<span>Sin definir!</span>";
            }
            if (!DateTime.TryParse(estado.ToString(), out fce))
            {
                return "<span>ERROR!</span>";
            }
            if (fce > DateTime.Now)
            {
                return string.Format( "<span style='color:green;' >{0}</span>",fce.ToString("dd/MM/yyyy HH:mm"));
            }
            else
            {
                return string.Format("<span style='color:red;' >{0}</span>", fce.ToString("dd/MM/yyyy HH:mm"));
            }
   
        }

        public static string securetext(object number)
        {
            if (number == null || number.ToString().Length <= 2)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }
      

        private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
           var table = new Catalogos.pc_buscar_bookingDataTable();
           var ta = new CatalogosTableAdapters.pc_buscar_bookingTableAdapter();
            sinresultado.Visible = false;
           
           try
           {

                if (string.IsNullOrEmpty(bonum.Text))
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-alerta";
                    this.sinresultado.InnerText = "Escriba una o varias letras del número de reserva";
                    sinresultado.Visible = true;
                    return;
                }

                DateTime desde;
               DateTime hasta;
               if (!DateTime.TryParseExact(  desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
                   sinresultado.Visible = true;
                   return;
               }
               if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
                   sinresultado.Visible = true;
                   return;
               }
               if (desde.Year != hasta.Year)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-alerta";
                   this.sinresultado.InnerText = "El rango máximo de consulta es de 1 año, gracias por entender.";
                   sinresultado.Visible = true;
                   return;
               }

              

    
               var user = Page.getUserBySesion();
               ta.ClearBeforeFill = true;
           
               ta.Fill(table,   desde, hasta,bonum.Text,dptipo.SelectedValue);
               if (table.Rows.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise las fechas y/o numero de reserva";
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
               sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta_booking", "populate", "Hubo un error al buscar", t.loginname));
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