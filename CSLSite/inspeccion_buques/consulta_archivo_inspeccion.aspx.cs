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
    public partial class consulta_archivo_inspeccion : System.Web.UI.Page
    {
       
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
           
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
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la búsqueda, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta_archivo_inspeccion", "btbuscar_Click", "Hubo un error al buscar", t.loginname));
                    sinresultado.Visible = true;
                }
            }
        }
       
     
        public static string tipos(object tipo)
        {
            if (tipo == null)
            {
                return "!error";
            }
            var tt = tipo.ToString().Trim();
            if (string.IsNullOrEmpty(tt))
            {
                return "!error";
            }

            if (tt.ToUpper().Contains("T"))
            {
                return "Logística";
            }
            if (tt.ToUpper().Contains("P"))
            {
                return "Terminal";
            }
            return "!Desconocido";
        }
        public static string securetext(object number)
        {
            if (number == null || number.ToString().Length <= 0)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }
    



        private void populate()
       {
           System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
           Session["resultado"] = null;
           var table = new Catalogos.bil_consulta_certificadosDataTable();
           var ta = new CatalogosTableAdapters.bil_consulta_certificadosTableAdapter();
           try
           {
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

               TimeSpan ts = desde - hasta;
               // Difference in days.
               if (ts.Days > 31)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-alerta";
                   this.sinresultado.InnerText = "El rango máximo de consulta es de 1 mes, gracias por entender.";
                   sinresultado.Visible = true;
                   return;
               }

               var user = Page.getUserBySesion();
               ta.ClearBeforeFill = true;
             
               ta.Fill(table,    desde, hasta , user.ruc);
               if (table.Rows.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise la unidad, documento o # certificado";
                   sinresultado.Visible = true;
                   return;
               }
               this.tablePagination.DataSource = table;
               this.tablePagination.DataBind();
               xfinder.Visible = true;
           }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
               sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta_archivo_inspeccion", "populate", "Hubo un error al buscar", t.loginname));
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