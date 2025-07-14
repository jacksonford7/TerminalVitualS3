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
using CSLSite.app_start;

using System.Xml;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace CSLSite.ecuapass
{
    public partial class consulta : System.Web.UI.Page
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
                
                this.docnum.Text = Server.HtmlEncode(this.docnum.Text);

           
               
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
                this.docnum.Text = Session["cosulta_dae"] as string;
            }

        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.docnum.Text))
            {
                sinresultado.InnerText = "El Campo número de DAE es obligatorio";
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                this.tablePagination.DataSource = null;
                this.tablePagination.DataBind();
                xfinder.Visible = false;
                sinresultado.Visible = true;
                return;
            }
            this.docnum.Text = this.docnum.Text.Trim().ToUpper();
            if (this.docnum.Text.Length!=17)
            {
                sinresultado.InnerText = "La longitud del número de DAE es de 17 caracteres";
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                this.tablePagination.DataSource = null;
                this.tablePagination.DataBind();
                xfinder.Visible = false;
                sinresultado.Visible = true;
                return;
            }


            if (Response.IsClientConnected)
            {
                try
                {
#if !DEBUG

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        return;
                    }
#endif
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
            var mesa_present = new StringBuilder();
            mesa_present.Append("Resultado de la búsqueda:<br/>");
            string ec_ruta = System.Configuration.ConfigurationManager.AppSettings["ec_ruta"] as string;
            ec_ruta = Server.MapPath(string.Format("~{0}", ec_ruta));
            string ono = string.Empty;
            ecuapass_dae.buscar_dae_ecuapass(this.docnum.Text, ec_ruta, out ono);
            mesa_present.AppendLine(string.Format("{0}<br/>", ono));
           Session["resultado_dae"] = null;
            var table = new Catalogos.pc_list_dae_rucDataTable();
            var ta = new CatalogosTableAdapters.pc_list_dae_rucTableAdapter();
           try
           {
                //aca debe traer lo de aduana la consulta--->
               var user = Page.getUserBySesion();
               ta.ClearBeforeFill = true;
#if DEBUG
                user.ruc = "0992506717001";//0992506717001
#endif
                ta.Fill(table, this.docnum.Text, user.ruc);
               if (table.Rows.Count <= 0)
               {
                   xfinder.Visible = false;
                   sinresultado.Attributes["class"] = string.Empty;
                   sinresultado.Attributes["class"] = "msg-info";
                    mesa_present.AppendLine("Revise que el número de la DAE se encuentre escrito correctamente.<br/>");
                    this.sinresultado.InnerHtml = mesa_present.ToString();
                   sinresultado.Visible = true;
                   return;
               }
               Session["resultado_dae"] = table;
               this.tablePagination.DataSource = table;
               this.tablePagination.DataBind();
               xfinder.Visible = true;
                this.sinresultado.InnerHtml = "Estimado Cliente: <br/>Verifique que todos los datos sean correctos, cualquier consulta adicional contáctese con la casilla <a href='mailto:'ec.sac@contecon.com.ec'>ec.sac@contecon.com.ec</a>.<br/>Ahora puede continuar realizando el AISV.";
                sinresultado.Visible = true;
            }
           catch (Exception ex)
           {
               var t = this.getUserBySesion();
               sinresultado.Attributes["class"] = string.Empty;
               sinresultado.Attributes["class"] = "msg-critico";
               sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta_dae", "populate", "Hubo un error al buscar", t.loginname));
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