using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class referenciaBuque : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                alerta.InnerText = "";
                alerta.Visible = false;
                sinresultado.InnerText = "";
                sinresultado.Visible = false;
            }
        }

        protected void find_Click(object sender, EventArgs e)
        {
            alerta.InnerText = "";
            alerta.Visible = false;

            if(string.IsNullOrEmpty(txtLinea.Text.Trim()))
            {
                alerta.InnerText = "Por favor llene el campo Línea";
                alerta.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(txtAnio.Text.Trim()))
            {
                alerta.InnerText = "Por favor llene el campo año";
                alerta.Visible = true;
                return;
            }

            string identificacionAgencia = Session["identificacionAgencia"].ToString();


            List<Buque> table = CslHelperServicios.consultaReferenciaBuques(txtLinea.Text.Trim().ToUpper(), txtAnio.Text.Trim(), txtBuque.Text.Trim().ToUpper(), identificacionAgencia);//.catalogosDataTable();
            //var ta = new CatalogosTableAdapters.catalogosTableAdapter();
            var u = this.getUserBySesion();
            try
            {
                if (Response.IsClientConnected)
                {
                    //ta.ClearBeforeFill = true;
                    //ta.Fill(table, "REFERENCIA", txtfinder.Text.Trim(), txtfinder.Text.Trim());
                    if (table.Count > 0)
                    {
                        this.tablePagination.DataSource = table;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        return;
                    }
                    xfinder.Visible = false;
                    this.sinresultado.InnerText = "No se encontraron buques con los filtros de búsqueda ingresados.";
                    sinresultado.Visible = true;
                }
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "referencias", "find_Click", txtLinea.Text, u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
            finally
            {

            }
        }
    }
}