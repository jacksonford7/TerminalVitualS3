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
    public partial class visitantes : System.Web.UI.Page
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
                this.hastad.Text = DateTime.Now.ToString("dd/MM/yyyy");
                this.desded.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
        private void populate()
       {
          
           Session["resultado"] = null;
           var table = new Catalogos.VISITANTESDataTable();
           var ta = new CatalogosTableAdapters.VISITANTESTableAdapter();
           try
           {


               var user = Page.getUserBySesion();
               ta.ClearBeforeFill = true;
 
              ta.Fill(table,DateTime.Now, DateTime.Now,tcedula.Text.Trim(),txapellidos.Text.Trim(),-24,true);
               if (table.Rows.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                   this.sinresultado.InnerText = "No se encontraron resultados, revise el rango de fechas";
                   sinresultado.Visible = true;
                   return;
               }
               this.alerta.InnerText = string.Format("Para su comodidad exporte los resultados y revíselos en excel, ya que se han encontrado {0} registros",table.Rows.Count);
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
               sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "populate", "Hubo un error al buscar", t.loginname));
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