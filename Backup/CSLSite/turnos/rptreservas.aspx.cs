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
    public partial class rptreservas : System.Web.UI.Page
    {
        private string sid = string.Empty;
        protected void Page_Init(object sender, EventArgs e)
        {

            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "detalleaisv", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
            try
            {
                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "QuerySegura", "DecryptQueryString", sid, Request.UserHostAddress);
                    this.PersonalResponse("Está intentando acceder a un área restringida, por su seguridad los datos de su equipo han quedado registrados, gracias.", "../csl/login", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "detalleaisv", "Page_Init", sid, Request.UserHostAddress);
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
                    if (arrglo.Length != 4)
                    {
                        var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                        var number = log_csl.save_log<Exception>(ex, "turno_reporte", "Page_Load", sid, Request.UserHostAddress);
                        this.PersonalResponse("Hubo un problema durante la carga de datos, por favor intente mas tarde");
                        return;
                    }

                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime fechaini;
                    if (!DateTime.TryParseExact(arrglo[2], "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaini))
                    {
                        this.Response.ClearContent();
                        return;
                    }
                    DateTime fechafin;
                    if (!DateTime.TryParseExact(arrglo[3], "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechafin))
                    {
                        this.Response.ClearContent();
                        return;
                    }
                    if (arrglo[0] == "0")
                    {
                        arrglo[0] = null;
                    }
                    var t = turno.GetRptReservas(fechaini, fechafin, arrglo[0], arrglo[1]);

                  tablePagination.DataSource = t;
                  tablePagination.DataBind();

                  //this.cfecha.InnerText = arrglo[1];
                  //this.cbook.InnerText = arrglo[0];
                  //this.clinea.InnerText = arrglo[2];

                    //var tb = jAisvContainer.getMyDetails(sid);
                    //if (tb == null || tb.Count<=0)
                    //{
                    //    var ex = new ApplicationException(string.Format("No se encontraron detalles del aisv:{0}",sid));
                    //    var number = log_csl.save_log<Exception>(ex, "detalleaisv", "Page_Load", sid, Request.UserHostAddress);
                    //    this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                    //    return;
                    //}
                    //this.tablePagination.DataSource = tb;
                    //this.tablePagination.DataBind();
                }
                catch (Exception ex)
                {
                    var number = log_csl.save_log<Exception>(ex, "detalleaisv", "Page_Init", sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                    return;
                }
                finally
                {
                    
                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }

      
    }
}