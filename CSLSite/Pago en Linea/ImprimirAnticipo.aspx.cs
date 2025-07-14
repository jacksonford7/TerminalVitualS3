using System;
using System.Linq;
using System.Web.UI;
using CSLSite.app_start;
using csl_log;

namespace CSLSite.Pago_en_Linea
{
    public partial class ImprimirAnticipo : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        private string _sid = string.Empty;
        protected void Page_Init(object sender, EventArgs e)
        {

            if (!Request.IsAuthenticated)
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                log_csl.save_log<Exception>(ex, "printAnticipo", "Init", _sid, Request.UserHostAddress);
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
            }

            try
            {
                _sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(_sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de AISV nó válido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    log_csl.save_log<Exception>(ex, "printAnticipo", "Init", _sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log(ex, "printAnticipo", "Page_Init", _sid, User.Identity.Name);
                var close = CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                Response.Write(close);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                if (!IsPostBack)
                {
                    try
                    {
                        var usn = this.getUserBySesion();
                        if (usn == null)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            log_csl.save_log<Exception>(ex, "printAnticipo", "Page_Load", _sid, Request.UserHostAddress);
                            this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                            return;
                        }
                        _sid = _sid.Trim().Replace("\0", string.Empty);
                        var data = new PagoEnLineaData();
                        var resultado = data.ConsultaAnticipoPorNumeroLiquidacion(_sid);
                        if (resultado == null)
                        {
                            this.Alerta("Ha ocurrido un error.\nComnunicarse con el departamento de sistema de Contecon.");
                            return;
                        }
                        if (resultado.Rows.Count <= 0)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "printAnticipo", "Page_Load", _sid, User.Identity.Name);
                            this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                            return;
                        }
                        var fila = resultado;
                        
                        numprofpie.InnerHtml = string.Format("<strong>{0}</strong>", 1);
                        var clienteInfo = fila.Rows[0];
                        cliruc.InnerText = clienteInfo["NUMERO_IDENTIFICACION"].ToString();
                        clinombre.InnerText = clienteInfo["RAZON_SOCIAL"].ToString();
                        cliNumeroBooking.InnerText = clienteInfo["NUMERO_BOOKING"].ToString();
                        tablaNueva.DataSource = resultado;
                        tablaNueva.DataBind();

                        anumber.InnerText = clienteInfo["NUMERO_LIQUIDACION"].ToString();
                        fechagenera.InnerText = string.Format("{0}", clienteInfo["FECHA_REGISTRO"]);
                        fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        sttal.InnerText = clienteInfo["MONTO_TOTAL"].ToString();

                    }
                    catch (Exception ex)
                    {
                        var number = log_csl.save_log(ex, "printAnticipo", "Page_Load", _sid, User.Identity.Name);
                        var close = CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                        Response.Write(close);
                    }
                }
            }
        }
    }
}