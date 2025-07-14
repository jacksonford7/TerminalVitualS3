using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class certificado_impo : System.Web.UI.Page
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
                        var cfgs = dbconfig.GetActiveConfig("csl_services", "tv", null);
                        string CFECHA = TextFinal(nameof(CFECHA), cfgs, lg);
                        string CTITULO = TextFinal(nameof(CTITULO), cfgs, lg);
                        string CSUB_P = TextFinal(nameof(CSUB_P), cfgs, lg);
                        string CSUB_T = TextFinal(nameof(CSUB_T), cfgs, lg);
                        string CEMISOR_P = TextFinal(nameof(CEMISOR_P), cfgs, lg);
                        string CEMISOR_T = TextFinal(nameof(CEMISOR_T), cfgs, lg);
                        string CCOM_T = TextFinal(nameof(CCOM_T), cfgs, lg);
                        string CCOM_P = TextFinal(nameof(CCOM_P), cfgs, lg);
                        string CNUM = TextFinal(nameof(CNUM), cfgs, lg);
                        string CCID = TextFinal(nameof(CCID), cfgs, lg);
                        string CIN = TextFinal(nameof(CIN), cfgs, lg);
                        string CLOAD = TextFinal(nameof(CLOAD), cfgs, lg);
                        string CTRIP = TextFinal(nameof(CTRIP), cfgs, lg);
                        string CCOM = TextFinal(nameof(CCOM), cfgs, lg);
                        string CCEO = TextFinal(nameof(CCEO), cfgs, lg);
                        string CGER = TextFinal(nameof(CGER), cfgs, lg);
                        string CCERTI = TextFinal(nameof(CCERTI), cfgs, lg);
                        string CFORMAT = TextFinal(nameof(CFORMAT), cfgs, lg);
                        string CDES = TextFinal(nameof(CDES), cfgs, lg);
                        string CFESAL = TextFinal(nameof(CFESAL), cfgs, lg);
                        string CSUB_S = TextFinal(nameof(CSUB_S), cfgs, lg);

                        CultureInfo enUS = new CultureInfo(CFECHA);

                        var tabla = new Catalogos.aisv_carbono_certificado_imprimeDataTable();
                        var ta = new CatalogosTableAdapters.aisv_carbono_certificado_imprimeTableAdapter();
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
                        this.xtitulo.InnerText = CTITULO;
                        if (!string.IsNullOrEmpty(fila.cert_tipo))
                        {
                            this.cert_tipo.InnerText = fila.cert_tipo.Trim().ToUpper().Contains("T") ? CSUB_T : CSUB_P;
                        }
                       
                        this.nombres_exportador.InnerText = string.IsNullOrEmpty(fila.nombres_exportador) ? "??????????" : fila.nombres_exportador;




                        this.XNUM.InnerText = fila.notas=="CFS"? CNUM=="Number:"? "Load Number:":"Número Carga:" : CNUM;
                        this.aisv_contenedor.InnerText = string.IsNullOrEmpty(fila.aisv_contenedor) ? "??????????" : fila.aisv_contenedor;
                        this.XCID.InnerText = CCID;
                        this.cert_secuencia.InnerText = string.IsNullOrEmpty(fila.cert_secuencia) ? "??????????" : fila.cert_secuencia;
                        this.XIN.InnerText = CDES;
                        this.unidad_fecha_ingreso.InnerText =fila.Isunidad_fecha_ingresoNull() ? "??????????" : fila.unidad_fecha_ingreso.ToString(CFORMAT);
                        this.XLOAD.InnerText = CFESAL;
                        this.unidad_fecha_embarque.InnerText = fila.Isunidad_fecha_embarqueNull() ? "??????????" : fila.unidad_fecha_embarque.ToString(CFORMAT);
                        this.XTRIP.InnerText = CTRIP;
                        string bv = string.Format("{0} {1}", (string.IsNullOrEmpty(fila.unidad_buque) ? "" : fila.unidad_buque), (string.IsNullOrEmpty(fila.unidad_viaje) ? "" : fila.unidad_viaje));
                        this.buque_unidad_viaje.InnerText = bv;
                        //this.XCOM.InnerText = CCOM;
                        //this.aisv_producto.InnerText = string.IsNullOrEmpty(fila.aisv_producto) ? "No declarado" : fila.aisv_producto;
                        this.cert_generado.InnerText = string.Format("Ecuador, {0}", fila.Iscert_generadoNull() ? "??????????" : fila.cert_generado.ToString("D", enUS));
                        this.comp.InnerText = fila.cert_tipo.Contains("T") ? CCOM_T: CCOM_P;

                        this.siguiente.InnerText = fila.cert_tipo.Contains("T") ? CSUB_S : CSUB_S;

                        this.basic_line.InnerText = fila.cert_tipo.Contains("T") ? CEMISOR_T : CEMISOR_P;

                        //XCERTI.InnerText = CCERTI;

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