using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Globalization;

namespace CSLSite
{
    public partial class preimpresion : System.Web.UI.Page
    {
        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "preimpresion", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            Page.Tracker();
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.usuario.Value = this.Master.LoginPage;
            this.sinresultado.Visible = IsPostBack;
           
        }
        protected void btbuscar_Click(object sender, EventArgs e)
        {
            this.xfinder.Visible = false;
            this.sinresultado.Visible = true;
            this.btprinter.Disabled = true;
            if (pupulateTabla())
            {
                this.xfinder.Visible = true;
                this.sinresultado.Visible = false;
                this.btprinter.Disabled = false;
            }
            this.sinresultado.InnerText = "No se han encontrado resultados, asegúrese que los parámetros sean corrrectos";
        }
        public bool pupulateTabla()
        {
            try
            {
                htmlstring.InnerText = string.Empty;
                var flag = false;
                htmlstring.InnerText = "<table class=\"table table-bordered invoice\" id=\"tablasort\"><thead><tr><th>Contenedor</th><th>Booking</th><th>Referencia</th><th>Usuario</th><th>Fecha ingreso</th></tr></thead><tbody>";
                if (Response.IsClientConnected)
                {
                    DateTime desde;
                    DateTime hasta;
                    System.Globalization.CultureInfo enUS = new System.Globalization.CultureInfo("en-US");
                    if (!DateTime.TryParseExact(desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
                        sinresultado.Visible = true;
                        return false;
                    }
                    if (!DateTime.TryParseExact(desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                    {
                        xfinder.Visible = false;
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
                        sinresultado.Visible = true;
                        return false;
                    } 
                    foreach (var item in adviceHelper.getMyAdvices(desded.Text, hastad.Text, usuario.Value, cntrn.Text, docnum.Text, refer.Text))
                    {
                        htmlstring.InnerText += string.Format("<tr class=\"point\"><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", item.Item1, item.Item2, item.Item5, item.Item4, item.Item3);
                        flag = true;
                    }
                }
                htmlstring.InnerText += "</tbody></table>";
                if (flag == false)
                {
                    htmlstring.InnerText = string.Empty;
                }
                else
                {
                    postable.InnerHtml = htmlstring.InnerText;
                }
                return flag;
            }
            catch (Exception ex)
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "preimpresion", "pupulateTabla", desded.Text, usuario.Value));
                sinresultado.Visible = true;
                return false;
            }

        }
    }
}