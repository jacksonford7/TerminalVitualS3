using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.escaner
{
    public partial class certificado_scan : System.Web.UI.Page
    {
        string sid;
        string lg;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
            }

            try
            {
                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                lg = Request.QueryString["lg"];
                lg = string.IsNullOrEmpty(lg) ? "E" : lg;

                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de AISV nó válido", Request.UserHostAddress, Request.Url, Request.HttpMethod));               
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
            }
            catch
            {
               
                string close = CSLSite.CslHelper.ExitForm("Hubo un problema durante el inicio de la impresión");
                base.Response.Write(close);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                if (!IsPostBack)

                {
                    string v_num = "";
                    try
                    {
                        var cfgs = dbconfig.GetActiveConfig("csl_services", "tv-", null);
                        string SCFECHA = TextFinal(nameof(SCFECHA), cfgs, lg);
                        string SCTITULO = TextFinal(nameof(SCTITULO), cfgs, lg);
                        string SCSUB_P = TextFinal(nameof(SCSUB_P), cfgs, lg);
                        string SCTRAFIC = TextFinal(nameof(SCTRAFIC), cfgs, lg);
                        string SCTRAFIC_E = TextFinal(nameof(SCTRAFIC_E), cfgs, lg);
                        string SCTRAFIC_I = TextFinal(nameof(SCTRAFIC_I), cfgs, lg);

                        string SDATO01 = TextFinal(nameof(SDATO01), cfgs, lg);
                        string SDATO02 = TextFinal(nameof(SDATO02), cfgs, lg);
                        string SDATO03 = TextFinal(nameof(SDATO03), cfgs, lg);
                        string SDATO04 = TextFinal(nameof(SDATO04), cfgs, lg);
                        string SDATO05 = TextFinal(nameof(SDATO05), cfgs, lg);
                        string SDATO06 = TextFinal(nameof(SDATO06), cfgs, lg);
                        string SDATO07 = TextFinal(nameof(SDATO07), cfgs, lg);
                        string SDATO08 = TextFinal(nameof(SDATO08), cfgs, lg);
                        string SDATO09 = TextFinal(nameof(SDATO09), cfgs, lg);
                        string SDATO10 = TextFinal(nameof(SDATO10), cfgs, lg);
                        string SDATO11 = TextFinal(nameof(SDATO11), cfgs, lg);
                        string SDATO12 = TextFinal(nameof(SDATO12), cfgs, lg);
                        string SDATO13 = TextFinal(nameof(SDATO13), cfgs, lg);
                        string SDATO14 = TextFinal(nameof(SDATO14), cfgs, lg);
                        string SDATO15 = TextFinal(nameof(SDATO15), cfgs, lg);
                        string SDATO16 = TextFinal(nameof(SDATO16), cfgs, lg);
                        string SCSTATUS = TextFinal(nameof(SCSTATUS), cfgs, lg);
                        string SCFORMAT = TextFinal(nameof(SCFORMAT), cfgs, lg);

                        CultureInfo enUS = new CultureInfo(SCFECHA);

                        var tabla = new Catalogos.aisv_escaner_certificado_imprimeDataTable();
                        var ta = new CatalogosTableAdapters.aisv_escaner_certificado_imprimeTableAdapter();
                        sid = sid.Trim().Replace("\0", string.Empty);
                        Int64 id;
                        if (!Int64.TryParse(sid, out id))
                        {
                            string close = CSLSite.CslHelper.ExitForm("Hubo un problema durante la conversión para la búsqueda..");
                            base.Response.Write(close);
                        }
                        ta.Fill(tabla, id);
                        if (tabla.Rows.Count <= 0)
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            this.PersonalResponse("La busqueda no obtuvo resultados, favor intente mas tarde");
                            return;
                        }

                        var fila = tabla.FirstOrDefault();

                        
                        this.xtitulo.InnerText = SCTITULO;
                        this.XSUB_P.InnerText = SCSUB_P;
                        if (!string.IsNullOrEmpty(fila.cert_tipo))
                        {
                            this.cert_tipo.InnerText = SCTRAFIC +" " + (fila.cert_tipo.Trim().ToUpper().Contains("E") ? SCTRAFIC_E : SCTRAFIC_I);
                        }

                        //this.nombres_exportador.InnerText = string.IsNullOrEmpty(fila.nombres_exportador) ? "??????????" : fila.nombres_exportador;

                        string bv = string.Format("{0} {1}", (fila.Isunidad_buqueNull() ? "????" : fila.unidad_buque), (fila.Isunidad_viajeNull() ? "" : fila.unidad_viaje));
                        this.cert_generado.InnerText = string.Format("Ecuador, {0}", fila.Iscert_generadoNull() ? "??????????" : fila.cert_generado.ToString("D", enUS));

                        this.XDATO01.InnerText = SDATO01;
                        this.cert_secuencia.InnerText = string.IsNullOrEmpty(fila.cert_secuencia) ? "??????????" : fila.cert_secuencia;

                        this.XDATO02.InnerText = SDATO02;
                        this.aisv_booking.InnerText =     string.IsNullOrEmpty(fila.aisv_booking) ? "??????????" : fila.aisv_booking;
                        this.XDATO03.InnerText = SDATO03;
                        this.aisv_contenedor.InnerText = string.IsNullOrEmpty(fila.aisv_contenedor) ? "??????????" : fila.aisv_contenedor;
                        this.XDATO04.InnerText = SDATO04;
                        this.aisv_vessel.InnerText = bv;
                        this.XDATO05.InnerText = SDATO05;
                        this.aisv_dae.InnerText = string.IsNullOrEmpty(fila.dae) ? "??????????" : fila.dae;
                        this.XDATO06.InnerText = SDATO06;
                        this.aisv_seal1.InnerText = string.IsNullOrEmpty(fila.aisv_sello1) ? "??????????" : fila.aisv_sello1;
                        this.XDATO07.InnerText = SDATO07;
                        this.aisv_seal2.InnerText = string.IsNullOrEmpty(fila.aisv_sello2) ? "??????????" : fila.aisv_sello2;
                        this.XDATO08.InnerText = SDATO08;
                        this.aisv_seal3.InnerText = string.IsNullOrEmpty(fila.aisv_sello3) ? "??????????" : fila.aisv_sello3;
                        this.XDATO09.InnerText = SDATO09;
                        this.aisv_seal4.InnerText = string.IsNullOrEmpty(fila.aisv_sello4) ? "??????????" : fila.aisv_sello4;
                        this.XDATO10.InnerText = SDATO10;
                        this.aisv_transportista.InnerText = string.IsNullOrEmpty(fila.transporteCia) ? "??????????" : fila.transporteCia;
                        this.XDATO11.InnerText = SDATO11;
                        this.aisv_chofer.InnerText = string.IsNullOrEmpty(fila.chofer) ? "??????????" : fila.chofer;
                        this.XDATO12.InnerText = SDATO12;
                        this.aisv_licencia.InnerText = string.IsNullOrEmpty(fila.id_chofer) ? "??????????" : fila.id_chofer;
                        this.XDATO13.InnerText = SDATO13;
                        this.aisv_placa.InnerText = string.IsNullOrEmpty(fila.placa) ? "??????????" : fila.placa;
                        this.XDATO14.InnerText = SDATO14;
                        this.scan_status.InnerText = SCSTATUS;
                        this.XDATO15.InnerText = SDATO15;
                        this.scan_date.InnerText = fila.Isunidad_fecha_ingresoNull() ? "??????????" : fila.unidad_fecha_ingreso.ToString(SCFORMAT);
                        this.XDATO16.InnerText = SDATO16;
                    }
                    catch(Exception Ex)
                    {
                        string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema general durante la conversión para la búsqueda.{0}-{1}-{2}-{3}", Ex.Message,Ex.Source,Ex.StackTrace, v_num));
                        base.Response.Write(close);
                    }

                    
                }
            }
        }

        private string TextFinal(string cfname, List<dbconfig> cfgs, string lan)
        {
            string[] textos;
            string salida;
            bool arl = false;
            var sebusca = cfname.ToString().Trim().ToUpper();
            var cfg = cfgs.Where(a => a.config_name.Trim().ToUpper().Equals(sebusca)).FirstOrDefault();
            salida = cfg != null ? cfg.config_value : "";
            textos = salida.Split('+');
            arl = textos.Length > 1;
            salida = lan == "S" ? textos[0] : arl ? textos[1] : "-";
            return salida.Trim();
        }
    }
}