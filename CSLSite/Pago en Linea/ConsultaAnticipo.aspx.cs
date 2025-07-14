using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSLSite.app_start;

namespace CSLSite.Pago_en_Linea
{
    public partial class ConsultaAnticipo : Page
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
                rolCliente.Value = user.grupo.HasValue ? user.grupo.Value.ToString() : "";
                nombreCliente.Value = string.Format("{0} {1}", user.nombres, user.apellidos);
                userName.Value = user.loginname;
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
                ComboBox1.SelectedIndex = 0;
                ConsultarDatos();
            }
            else
            {
                if (Request.Params["__EVENTTARGET"] == "ctl00$placebody$btbuscar")
                    ConsultarDatos();
            }
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
        public static string securetext(object number)
        {
            if (number == null || number.ToString().Length <= 2)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
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
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("El retorno de usuario es nulo, posiblemente ha caducado"), "consulta", "tablePagination_ItemCommand", "No se pudo obtener usuario", "anónimo"));
                        sinresultado.Visible = true;
                        return;
                    }
                    if (e.CommandArgument == null)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "consulta", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }
                    Int64 secuencia = 0;
                    var argumento = e.CommandArgument.ToString();
                    if (string.IsNullOrEmpty(argumento))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "Proforma", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    var param = argumento.Split(';');
                    if (param.Length != 3)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "Proforma", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    if (!Int64.TryParse(param[1], out secuencia))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio  A01-{0}.", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException("CommandArgument=NULL"), "Proforma", "tablePagination_ItemCommand", "CommandArgument is NULL", user.loginname));
                        sinresultado.Visible = true;
                        return;
                    }

                    //boking , secuencia, proforma
                    var aisvs = ProformaHelper.AisvActivos(param[0]);
                    if (aisvs > 0)
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerHtml = string.Format("El Booking {1} tiene {0} documentos AISV activos, no es posible anular esta proforma.<br/>Si aún desea continuar con el proceso, por favor ANULE todos los AISV amparados con el booking {1} ", aisvs, param[0]);
                        sinresultado.Visible = true;
                        return;
                    }


                    string vt = string.Empty;
                    if (!Proforma.Borrar(secuencia, user.loginname, out vt))
                    {
                        sinresultado.Attributes["class"] = string.Empty;
                        sinresultado.Attributes["class"] = "msg-critico";
                        sinresultado.InnerText = "No fúe posible anular este documento, es probable que la carga ya este ingresada o que el transporte ya este fuera de la terminal, confirme con planificación"; ;
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
        private void ConsultarDatos(bool presentarImpresion = false)
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
                var resultado = data.ConsultaAnticipo(idCliente.Value, desde, hasta, TextBox2.Text, ComboBox1.SelectedValue == "0" ? "" : ComboBox1.SelectedItem.Text.ToUpper());
                if(resultado == null)
                    this.Alerta("Ha ocurrido un error.\nComunicarse con el departamento de sistema de Contecon.");
                else
                {
                    if (resultado.Rows.Count == 0)
                        this.Alerta("No existen datos para presentar.");
                    Session["resultado"] = resultado;
                    tablePagination.DataSource = resultado;
                    tablePagination.DataBind();
                    xfinder.Visible = resultado.Rows.Count != 0;
                    if (presentarImpresion)
                    {
                        var numeroLiquidacion = resultado.Rows[0].ItemArray[3].ToString();
                        var url = string.Format("../Pago En Linea/ImprimirAnticipo.aspx?sid={0}", securetext(numeroLiquidacion));
                        url = string.Format("window.open('{0}','_blank');", url);
                        Page.ClientScript.RegisterStartupScript(GetType(), "OpenWindow", url , true);
                    }
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
        protected void GrabarAnticipo(object sender, EventArgs e)
        {
            var data = new PagoEnLineaData();
            var resultado = data.IngresoAnticipo(idCliente.Value, CslHelper.getShiperName(idCliente.Value), TextBox1.Text, userName.Value, TextBox4.Text, rolCliente.Value);
            this.Alerta(resultado == "" ? "Proceso Ok." : resultado);
            TextBox1.Text = "";
            TextBox4.Text = "";
            ConsultarDatos(true);
        }
        protected void AnularAnticipo(object sender, EventArgs e)
        {
            var data = new PagoEnLineaData();
            data.AnularAnticipo(codigoAnticipo.Value);
            codigoAnticipo.Value = "";
            this.Alerta("Proceso Ok.");
            ConsultarDatos();
        }
    }
}