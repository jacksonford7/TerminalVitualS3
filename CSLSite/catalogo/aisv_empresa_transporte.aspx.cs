using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class aisv_empresa_transporte : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
            }
        }
        protected void find_Click(object sender, EventArgs e)
        {
           
            var u = new usuario();
#if !DEBUG

           
             u = this.getUserBySesion();
#endif

           

            try
            {
                if (Response.IsClientConnected)
                {

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var Transportista = N4.Entidades.CompaniaTransporte.ObtenerCompanias(ClsUsuario.loginname, txtfinder.Text.Trim());
                    if (Transportista.Exitoso)
                    {
                        var LinqQuery = (from Tbl in Transportista.Resultado.Where(Tbl => Tbl.ruc != null)
                                         select new
                                         {
                                             EMPRESA = string.Format("{0} - {1}", Tbl.ruc.Trim(), Tbl.razon_social.Trim()),
                                             RUC = Tbl.ruc.Trim(),
                                             NOMBRE = Tbl.razon_social.Trim(),
                                             ID = Tbl.ruc.Trim()
                                         }).ToList();

                        this.tablePagination.DataSource = LinqQuery;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        return;

                    }
                    else 
                    {
                        xfinder.Visible = false;
                        sinresultado.Visible = true;
                    }


                   
                }
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "empresas", "find_Click", txtfinder.Text, u != null ? u.loginname : "catalogoChoferes"));
                sinresultado.Visible = true;
            }
            finally
            {
                //table.Dispose();
                //ta.Dispose();
            }
        }
    }
}