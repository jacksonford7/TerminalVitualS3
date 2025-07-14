using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Globalization;
using CSLSite.app_start;

namespace CSLSite.Pago_en_Linea
{
    public partial class Compensacion : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null)
            {
                idCliente.Value = user.codigoempresa;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
            desded.Text = Server.HtmlEncode(desded.Text);
            desded.Text = Server.HtmlEncode(hastad.Text);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Response.IsClientConnected) return;
            xfinder.Visible = IsPostBack;
            sinresultado.Visible = false;
            if (!IsPostBack)
            {
                desded.Text = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                hastad.Text = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            ConsultarDatos();
            ConsultarFacturasPendientesPago();
        }
        protected void Buscar(object sender, EventArgs e)
        {
            if (!Response.IsClientConnected) return;
            try
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    return;
                }
                ConsultarDatos();
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
        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    var user = new usuario();

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }
                    user = Page.getUserBySesion();

                    if (user == null)
                    {
                        return;
                    }
                    if (e.CommandArgument == null)
                    {
                        return;
                    }
                    Int64 secuencia = 0;
                    var argumento = e.CommandArgument.ToString();
                    if (string.IsNullOrEmpty(argumento))
                    {
                        return;
                    }

                    var param = argumento.Split(';');
                    if (param.Length != 3)
                    {
                        return;
                    }

                    if (!Int64.TryParse(param[1], out secuencia))
                    {
                        return;
                    }
                    ConsultarFacturasPendientesPago();
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();

                }
            }
        }
        private void ConsultarDatos()
        {
            var enUS = new CultureInfo("en-US");
            Session["resultado"] = null;
            try
            {
                DateTime desde;
                DateTime hasta;
                if (!DateTime.TryParseExact(desded.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out desde))
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    sinresultado.InnerText = "No se encontraron resultados, revise la fecha desde";
                    sinresultado.Visible = true;
                    return;
                }
                if (!DateTime.TryParseExact(hastad.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out hasta))
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    sinresultado.InnerText = "No se encontraron resultados, revise la fecha hasta";
                    sinresultado.Visible = true;
                    return;
                }
                var ts = desde - hasta;
                if (ts.Days > 30)
                {
                    xfinder.Visible = false;
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-alerta";
                    sinresultado.InnerText = "El rango máximo de consulta es de 1 mes, gracias por entender.";
                    sinresultado.Visible = true;
                    return;
                }
                var data = new PagoEnLineaData();
                var resultado = data.ConsultaAnticipoConfirmadosParaCompensacion(idCliente.Value, desde, hasta, TextBox2.Text);
                if (resultado == null)
                    this.Alerta("Ha ocurrido un error.\nComunicarse con el departamento de sistema de Contecon.");
                else
                {
                    if (resultado.Rows.Count == 0)
                        this.Alerta("No existen datos de anticipos para presentar.");
                    Session["resultado"] = resultado;
                    tablePagination.DataSource = resultado;
                    tablePagination.DataBind();
                    xfinder.Visible = resultado.Rows.Count != 0;
                }
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                sinresultado.Attributes["class"] = string.Empty;
                sinresultado.Attributes["class"] = "msg-critico";
                sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la carga de datos, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta-proforma", "ConsultarDatos", "Hubo un error al buscar", t.loginname));
                sinresultado.Visible = true;
            }
        }

        protected void tablePagination_ItemCommand1(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    var user = new usuario();

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }
                    user = Page.getUserBySesion();

                    if (user == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination_ItemCommand1", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        return;
                    }
                    if (e.CommandArgument == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "tablePagination_ItemCommand1", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }
                    Int64 secuencia = 0;
                    var argumento = e.CommandArgument.ToString();
                    if (string.IsNullOrEmpty(argumento))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "Proforma", "tablePagination_ItemCommand1", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    var param = argumento.Split(';');
                    if (param.Length != 3)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "Proforma", "tablePagination_ItemCommand1", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    if (!Int64.TryParse(param[1], out secuencia))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "Proforma", "tablePagination_ItemCommand1", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-info";
                    sinresultado.InnerText = string.Format("La anulación de la proforma  No.{0} ha resultado exitosa.", param[2]);
                    sinresultado.Visible = true;
                    ConsultarDatos();
                }
                catch (Exception ex)
                {
                    var t = this.getUserBySesion();
                    sinresultado.Attributes["class"] = string.Empty;
                    sinresultado.Attributes["class"] = "msg-critico";
                    sinresultado.InnerText = string.Format("Ha ocurrido un problema durante la anulación de este documento, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consultapro", "Item_comand", "Hubo un error al anular", t.loginname));
                    sinresultado.Visible = true;

                }
            }
        }
        private void ConsultarFacturasPendientesPago()
        {
            try
            {
                var data = new PagoEnLineaData();
                var resultado = data.ConsultaFacturasNoCanceladasPorClientes(idCliente.Value);
                if (resultado == null)
                    this.Alerta("Ha ocurrido un error.\nComunicarse con el departamento de sistema de Contecon.");
                else
                {
                    if (resultado.Rows.Count == 0)
                        this.Alerta("No existen facturas pendientes de pago para presentar.");
                    tableFacturas.DataSource = resultado;
                    tableFacturas.DataBind();
                    xfinderFacturas.Visible = resultado.Rows.Count != 0;
                }
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
            }
        }

        protected void Compensar(object sender, EventArgs e)
        {
            if (!Response.IsClientConnected) return;
            try
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    return;
                }
                IngresarCompensacion(idAnticipo.Value, facturas.Value.Substring(0, facturas.Value.Length - 1));
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {

            }
        }
        private void IngresarCompensacion(string liquidacionAnticipo, string datos)
        {
            try
            {
                var data = new PagoEnLineaData();
                data.IngresarCompensacion(liquidacionAnticipo, datos);
                this.Alerta("Proceso OK.");
                ConsultarDatos();
                ConsultarFacturasPendientesPago();
            }
            catch (Exception ex)
            {

            }
        }
    }
}