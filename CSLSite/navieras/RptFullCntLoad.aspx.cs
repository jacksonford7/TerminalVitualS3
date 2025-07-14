using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;
using System.Globalization;

namespace CSLSite
{
    public partial class rpt_fcont_load : System.Web.UI.Page
    {
        private string sid = string.Empty;
        protected void Page_Init(object sender, EventArgs e)
        {

            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "detalleaisv", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
            
            //todo ver si esta autenticada
            try
            {
                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "QuerySegura", "DecryptQueryString", sid, Request.UserHostAddress);
                    this.PersonalResponse("Está intentando acceder a un área restringida, por su seguridad los datos de su equipo han quedado registrados, gracias.", "../login.aspx", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "carga_contain_report", "Page_Init", sid, Request.UserHostAddress);
                this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}",number), null);
                return;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    sid = sid.Trim().Replace("\0", string.Empty);
                    var arrglo = sid.Split(';');
                    if (arrglo.Length != 2)
                    {
                        var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                        var number = log_csl.save_log<Exception>(ex, "carga_contain_report", "Page_Load", sid, Request.UserHostAddress);
                        this.PersonalResponse("Hubo un problema durante la carga de datos, por favor intente mas tarde");
                        return;
                    }

                    if (arrglo[1] == "0")
                    {
                        arrglo[1] = null;
                    }
                    var t = repNavieras.GetRptFullCntLoad(arrglo[0], arrglo[1]);

                  tablePagination.DataSource = t;
                  tablePagination.DataBind();

                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "carga_contain_report", "Page_Load", sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                    return;
                }
                finally
                {
                    
                }
            }
        }

      
    }
}