using System;
using CSLSite.app_start;
using csl_log;

namespace CSLSite.Pago_en_Linea
{
    public partial class ImprimirFacturasPagadas : System.Web.UI.Page
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
                log_csl.save_log<Exception>(ex, "printproforma", "Init", _sid, Request.UserHostAddress);
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
            }

            try
            {
                _sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(_sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de AISV nó válido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    log_csl.save_log<Exception>(ex, "printproforma", "Init", _sid, Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log(ex, "printproforma", "Page_Init", _sid, User.Identity.Name);
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
                            log_csl.save_log<Exception>(ex, "printproforma", "Page_Load", _sid, Request.UserHostAddress);
                            this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                            return;
                        }
                        _sid = _sid.Trim().Replace("\0", string.Empty);
                        //Int64 secuencia;
                        //if (!Int64.TryParse(_sid, out secuencia))
                        //{
                        //    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                        //    var number = log_csl.save_log<Exception>(ex, "printproforma", "Page_Load", _sid, User.Identity.Name);
                        //    this.PersonalResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number), null);
                        //    return;
                        //}
                        var data = new PagoEnLineaData();
                        var resultado = data.ConsultarFacturasPagadasPorAnticipo(_sid);
                        if (resultado == null)
                        {
                            this.Alerta("Ha ocurrido un error.\nComnunicarse con el departamento de sistema de Contecon.");
                            return;
                        }
                        if (resultado.Rows.Count <= 0)
                        {
                            this.Alerta(string.Format("No existen facturas pagadas con este anticipo"));
                            return;
                        }
                        var cabecera = data.ConsultarFacturaPorCodigoAnticipo(_sid);
                        if (cabecera == null)
                        {
                            this.Alerta("Ha ocurrido un error.\nComnunicarse con el departamento de sistema de Contecon.");
                            return;
                        }
                        if (cabecera.Rows.Count <= 0)
                        {
                            this.PersonalResponse(string.Format("No existen anticipo."));
                            return;
                        }
                        var fila = cabecera;
                        var montoTotal = 0.0;
                        for (var i = 0; i < resultado.Rows.Count; i++)
                            montoTotal = montoTotal + Convert.ToDouble(resultado.Rows[i]["MONTO_PAGADO"]);
                        numprofpie.InnerHtml = string.Format("<strong>{0}</strong>", 1);
                        var clienteInfo = fila.Rows[0];
                        cliruc.InnerText = clienteInfo["NUMERO_IDENTIFICACION"].ToString();
                        clinombre.InnerText = clienteInfo["RAZON_SOCIAL"].ToString();
                        tablaNueva.DataSource = resultado;
                        tablaNueva.DataBind();

                        anumber.InnerText = clienteInfo["NUMERO_LIQUIDACION"].ToString();
                        fechagenera.InnerText = string.Format("{0}",  DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        sttal.InnerText = montoTotal.ToString("C");

                    }
                    catch (Exception ex)
                    {
                        var number = log_csl.save_log(ex, "printaisv", "Page_Load", _sid, User.Identity.Name);
                        var close = CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                        Response.Write(close);
                    }
                }
            }
        }
    }
}