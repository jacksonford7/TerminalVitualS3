using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class placas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                var ruc = Session["rucempresasolicitudvehiculo"].ToString();
                var placa = Request.QueryString["sidpons"];
                sinresultado.Visible = false;
                var u = this.getUserBySesion();
                var t = credenciales.GetConsultaVehiculosXEmpresaMda(ruc, placa);
                try
                {
                    if (t.Rows.Count > 0)
                    {
                        t.Columns.Add("CODCATEGORIA");
                        t.Columns.Add("DESCRIPCIONCATEGORIA");
                        for (int i = 0; i < t.Rows.Count; i++)
                        {
                            var v = credenciales.GetConsultaVehiculosXEmpresaSca(ruc, t.Rows[i]["VE_PLACA"].ToString());
                            if (v.Rows.Count > 0)
                            {
                                t.Rows[i]["DESCRIPCIONCATEGORIA"] = v.Rows[0]["DESCRIPCIONCATEGORIA"].ToString();
                                t.Rows[i]["CODCATEGORIA"] = v.Rows[0]["CODCATEGORIA"].ToString();
                            }
                            else
                            {
                                t.Rows[i]["DESCRIPCIONCATEGORIA"] = t.Rows[i]["VE_CATEGORIA"].ToString();
                                t.Rows[i]["CODCATEGORIA"] = t.Rows[i]["VE_CODCATEGORIA"].ToString();
                            }
                        }
                        
                        this.tablePagination.DataSource = t;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;

                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "msg-info";
                        this.alerta.InnerHtml = "Listado de Placa(s) / No. Serie(s) (para Montacargas):"; //"Estimado cliente, si la fecha requerida para su reserva no aparece en esta lista. <br />" +
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