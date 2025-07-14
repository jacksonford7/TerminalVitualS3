using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class certificado_inspeccion : System.Web.UI.Page
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
                    try
                    {
                        var cfgs = dbconfig.GetActiveConfig("billion", "tv", null);
                        string CFECHA_INS = TextFinal(nameof(CFECHA_INS), cfgs, lg);
                        string CTITULO_INS = TextFinal(nameof(CTITULO_INS), cfgs, lg);
                        string CCID_INS = TextFinal(nameof(CCID_INS), cfgs, lg);
                        string CCOM_P_INS = TextFinal(nameof(CCOM_P_INS), cfgs, lg);
                        string CLINEA_INS = TextFinal(nameof(CLINEA_INS), cfgs, lg);
                        string CREFERENCIA_INS = TextFinal(nameof(CREFERENCIA_INS), cfgs, lg);
                        string CBANDERA_INS = TextFinal(nameof(CBANDERA_INS), cfgs, lg);
                        string CINSPE_INS = TextFinal(nameof(CINSPE_INS), cfgs, lg);
                        string CEMISOR_P_INS = TextFinal(nameof(CEMISOR_P_INS), cfgs, lg);
                        string CCEO_INS = TextFinal(nameof(CCEO_INS), cfgs, lg);
                        string CGER_INS = TextFinal(nameof(CGER_INS), cfgs, lg);
                        string CFORMAT_INS = TextFinal(nameof(CFORMAT_INS), cfgs, lg);
                      

                        CultureInfo enUS = new CultureInfo(CFECHA_INS);

                        var tabla = new Catalogos.bil_imprime_certificadosDataTable();
                        var ta = new CatalogosTableAdapters.bil_imprime_certificadosTableAdapter();
                        sid = sid.Trim().Replace("\0", string.Empty);
                        Int64 id;
                        if (!Int64.TryParse(sid, out id))
                        {
                            string close = CSLSite.CslHelper.ExitForm("Hubo un problema durante la conversión para la búsqueda");
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
                        this.xtitulo.InnerText = CTITULO_INS;
                    
                       
                        this.nombres_exportador.InnerText = string.IsNullOrEmpty(fila.DESC_NAVE) ? "??????????" : fila.DESC_NAVE;




                        this.XNUM.InnerText = CCID_INS;
                        this.aisv_contenedor.InnerText = string.IsNullOrEmpty(fila.SECUENCIA) ? "??????????" : fila.SECUENCIA;
                        this.XCID.InnerText = CLINEA_INS;
                        this.cert_secuencia.InnerText = string.IsNullOrEmpty(fila.LINEA) ? "??????????" : fila.LINEA;
                        this.XIN.InnerText = CREFERENCIA_INS;
                        this.unidad_fecha_ingreso.InnerText = string.IsNullOrEmpty(fila.REFERENCIA) ? "??????????" : fila.REFERENCIA;
                        this.XLOAD.InnerText = CBANDERA_INS;
                        this.unidad_fecha_embarque.InnerText = string.IsNullOrEmpty(fila.BANDERA) ? "??????????" : fila.BANDERA;

                        this.XTRIP.InnerText = CINSPE_INS;
                        this.buque_unidad_viaje.InnerText =fila.IsFECHA_INSPENull() ? "??????????" : fila.FECHA_INSPE.ToString(CFORMAT_INS);
                       
                        this.cert_generado.InnerText = string.Format("Ecuador, {0}", fila.IsFECHA_INSPENull() ? "??????????" : System.DateTime.Now.ToString("D", enUS));
                        this.comp.InnerText = CCOM_P_INS;

                       

                        this.basic_line.InnerText = CEMISOR_P_INS;

                       

                    }
                    catch
                    {
                        string close = CSLSite.CslHelper.ExitForm("Hubo un problema general durante la conversión para la búsqueda");
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