using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class certificado_sellos_impo : System.Web.UI.Page
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
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de certificado nó válido", Request.UserHostAddress, Request.Url, Request.HttpMethod));               
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

                        string S_CFORMAT = TextFinal(nameof(S_CFORMAT), cfgs, lg);
                        string S_CFECHA = TextFinal(nameof(S_CFECHA), cfgs, lg);
                        string S_CTITULO = TextFinal(nameof(S_CTITULO), cfgs, lg);
                        string S_CSUB_P = TextFinal(nameof(S_CSUB_P), cfgs, lg);
                        string S_CSUB_T = TextFinal(nameof(S_CSUB_T), cfgs, lg);
                        string S_CEMISOR_P = TextFinal(nameof(S_CEMISOR_P), cfgs, lg);
                        string S_FECHA = TextFinal(nameof(S_FECHA), cfgs, lg);
                        string S_IMPORTADOR = TextFinal(nameof(S_IMPORTADOR), cfgs, lg);
                        string S_CONTENEDOR = TextFinal(nameof(S_CONTENEDOR), cfgs, lg);
                        string S_CARGA = TextFinal(nameof(S_CARGA), cfgs, lg);
                        string S_NUMCERTI = TextFinal(nameof(S_NUMCERTI), cfgs, lg);
                        string S_SELLO1 = TextFinal(nameof(S_SELLO1), cfgs, lg);
                        string S_SELLO2 = TextFinal(nameof(S_SELLO2), cfgs, lg);
                        string S_SELLO3 = TextFinal(nameof(S_SELLO3), cfgs, lg);
                        string S_CSUB_S = TextFinal(nameof(S_CSUB_S), cfgs, lg);

                        CultureInfo enUS = new CultureInfo(CFECHA);

                        var tabla = new Catalogos.sellos_certificado_imprime_impoDataTable();
                        var ta = new CatalogosTableAdapters.sellos_certificado_imprime_impoTableAdapter();
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

                        this.xtitulo.InnerText = S_CTITULO;

                        this.nombres_exportador.InnerText = string.IsNullOrEmpty(fila.NOMBRES) ? "??????????" : fila.NOMBRES;

                        //this.cert_tipo.InnerText = "&nbsp;";//S_CSUB_T;
                        
                        //titulos
                        this.FECHA.InnerText = S_FECHA;
                        this.NOMBRE.InnerText = S_IMPORTADOR;
                        this.NUMERO.InnerText = S_CONTENEDOR;
                        this.CARGA.InnerText = S_CARGA;
                        this.CERTIFICADO.InnerText = S_NUMCERTI;
                        this.SELLO_UNO.InnerText = S_SELLO1;
                        this.SELLO_DOS.InnerText = S_SELLO2;
                        this.SELLO_TRES.InnerText = S_SELLO3;

                        //datos
                        this.fecha_certificado.InnerText = fila.FECHAING.ToString(S_CFORMAT); ;
                        this.nombre_importador.InnerText = string.IsNullOrEmpty(fila.NOMBRES) ? "??????????" : fila.NOMBRES;
                        this.numero_contenedor.InnerText = string.IsNullOrEmpty(fila.CNTR) ? "??????????" : fila.CNTR;
                        this.numero_carga.InnerText = string.IsNullOrEmpty(fila.NUMERO_CARGA) ? "??????????" : fila.NUMERO_CARGA;
                        this.cert_secuencia.InnerText = string.IsNullOrEmpty(fila.NUMERO_CEROS) ? "??????????" : fila.NUMERO_CEROS;
                        this.sello_uno_data.InnerText = string.IsNullOrEmpty(fila.SEAL_1) ? "" : fila.SEAL_1;
                        this.sello_dos_data.InnerText = string.IsNullOrEmpty(fila.SEAL_2) ? "" : fila.SEAL_2;
                        this.sello_tres_data.InnerText = string.IsNullOrEmpty(fila.SEAL_3) ? "" : fila.SEAL_3;




                        this.cert_generado.InnerText = string.Format("Ecuador, {0}",  fila.FECHAING.ToString("D", enUS));

                        //this.comp.InnerText = fila.cert_tipo.Contains("T") ? CCOM_T: CCOM_P;

                        this.siguiente.InnerText = S_CSUB_S;

                        this.basic_line.InnerText = S_CEMISOR_P;

                        

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