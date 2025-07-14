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

namespace CSLSite.exportables
{
    public partial class vacios_despacho : System.Web.UI.Page
    {
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            this.IsAllowAccess();
            Page.Tracker();
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
                
                this.desded.Text = Server.HtmlEncode(this.desded.Text);
                this.desded.Text = Server.HtmlEncode(this.hastad.Text);
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
                    sinresultado.Attributes["class"] = "alert alert-warning";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta_cn", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
       
     

        private void populate()
       {
            xfinder.Visible = true;
            sinresultado.Visible = true;
            System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
           var table = new Catalogos.mty_units_depot_dispatchDataTable();
           var ta = new CatalogosTableAdapters.mty_units_depot_dispatchTableAdapter();
           try
           {
               DateTime desde;
               DateTime hasta;
               if (!DateTime.TryParseExact(  desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
               {
                 
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "alert alert-warning";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
                  
                   return;
               }
               if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
               {
               
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
             
                   return;
               }

               TimeSpan ts = desde - hasta;
               // Difference in days.
               if (Math.Abs( ts.Days) > 3)
               {
                
                    sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "alert alert-warning";
                   this.sinresultado.InnerHtml = "El rango máximo de consulta es de 3 dias, gracias por entender.";
                  
                   return;
               }
               var user = Page.getUserBySesion();
               ta.ClearBeforeFill = true;
               //user.loginname = "MSK";
                try
                {

#if DEBUG
                    user.ruc = "MSK";
#endif

                    ta.Fill(table, user.ruc, desde, hasta);
                    if (table.Rows.Count > 0)
                    {
                        string fname = string.Format("DISPATCH_{0}", DateTime.Now.ToString("ddMMyyyHHmmss"));
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "alert alert-warning";
                        Session["MTY_DISPACH"] = table;
                        string llamada = string.Format("'descarga(\"{0}\",\"{1}\",\"{2}\");'", fname, "WITHDRAWALS", "MTY_DISPACH");
                        sinresultado.InnerHtml = string.Format("Se ha generado el archivo {0}.xlsx, con {1} filas<br/><a href='#' class='btn btn-link' onclick={2} >Clic Aquí para descargarlo</a>", fname, table.Rows.Count, llamada);
                       
                    }
                    else
                    {
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerHtml = "Hubo un problema y no se encontraron registros que mostrar ";
                    }
                }
                catch
                {
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = "Hubo un problema tratando de acceder a los datos ";
                }

            }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
               sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta_cn", "populate", "Hubo un error al buscar", t.loginname));
          
           }
           finally
           {
               ta.Dispose();
               table.Dispose();
           }
       }
    }
}