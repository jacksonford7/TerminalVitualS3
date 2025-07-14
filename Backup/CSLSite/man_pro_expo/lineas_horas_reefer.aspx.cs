using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CSLSite.mantenimientos_pro_expo
{
    public partial class lineas_horas_reefer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
                var u = this.getUserBySesion();
                var t = man_pro_expo.GetCatalagoLineaAsumeHorasReefer();
                try
                {
                    if (t.Rows.Count > 0)
                    {   
                        this.tablePagination.DataSource = t;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;

                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "msg-info";
                        this.alerta.InnerHtml = "Listado de Lineas Navieras:"; //"Estimado cliente, si la fecha requerida para su reserva no aparece en esta lista. <br />" +
                                       //"Favor comunicarse con nuestra Àrea de Logistica y Almacenamiento <br />" +
                                       //           "Pbx: +593 (04) 6006300, 3901700 ext. 4002, 4021, 4005. <br />" +
                                       //           "Email: <a href='mailto:CGSA-Consolidaciones@cgsa.com.ec'>CGSA-Consolidaciones@cgsa.com.ec</a>;";
                        return;
                    }
                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "agente", "find_Click", "Sin parametro", u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }
            }
        }
      
    }
}