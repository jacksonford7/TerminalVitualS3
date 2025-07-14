using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;
using System.Text;
using ClsNotasCreditos;

namespace CSLSite
{
    public partial class nota_credito_preview : System.Web.UI.Page
    {

        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        //private string sid = string.Empty;
        private string nc_id = string.Empty;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                var number = log_csl.save_log<Exception>(ex, "printncredito", "Init", nc_id, Request.UserHostAddress);
                this.AbortResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);                   
                return;
            }

            try
            {


                nc_id = QuerySegura.DecryptQueryString(Request.QueryString["nc_id"]);
                if (Request.QueryString["nc_id"] == null || string.IsNullOrEmpty(nc_id))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, nc_id de nota de credito no valido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "printncredito", "Init", nc_id, Request.UserHostAddress);
                    this.AbortResponse( string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}",Request.UserHostAddress, Request.Url, Request.HttpMethod), null);                   
                    return;
                }

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "printncredito", "Page_Init", nc_id, User.Identity.Name);
                string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código:{0}", number));
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

                        /*Recuperar toda la referencia*/
                        Int64 id = 0;

                        nc_id = nc_id.Trim().Replace("\0", string.Empty);

                        if (!Int64.TryParse(nc_id, out id))
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, nc_id no es numerico", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "nota_credito_preview", "Page_Load", nc_id == null ? "nc_id no es numerico" : nc_id, User.Identity.Name);
                            this.AbortResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                            return;
                        }
                        var vv = new credit_head(Math.Abs(id));
                        var msg = string.Empty;
                        //recupero el objeto
                        if ( !vv.PopulateMyData(out msg))
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "nota_credito_preview", "Page_Load", nc_id == null ? "nc_id is null" : nc_id, User.Identity.Name);
                            this.AbortResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                            return;
                        }
                        //recupero detalle
                        vv.LoadDetalle();
                       
                        this.barras.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=200&height=60&size=50", vv.nc_id.ToString("D8"));
                       

                        numero_factura.InnerText = "00" + vv.num_factura;
                        num_nota_credito.InnerText = "NOTA DE CREDITO # INTERNO " + vv.nc_id.ToString("D8");
                        cliente.InnerText = String.Format("[ {0} ] - {1}", vv.ruc_cliente,vv.nombre_cliente);
                        fecha_emision.InnerText = vv.nc_date.HasValue ? vv.nc_date.Value.ToString("dd/MM/yyyy") : "...";
                        concepto.InnerText = vv.description;   
                        glosa.InnerText = vv.nc_concept;
                        generado_por.InnerText = vv.Create_user;
                       
                       
                        this.barcode2.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=200&height=60&size=50", vv.nc_id.ToString("D8"));
                        this.detalle_data.InnerHtml = detalle_table(vv.Detalle, vv.nc_subtotal.ToString(), vv.nc_iva.ToString(), vv.nc_total.ToString());
                        this.fechagenera.InnerText = string.Format("{0}", vv.Create_date.HasValue ? vv.Create_date.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                        this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        this.Subtotal.InnerText = vv.nc_subtotal.ToString();
                        this.Iva.InnerText = vv.nc_iva.ToString();
                        this.Total.InnerText = vv.nc_total.ToString();

                    }
                    catch (Exception ex)
                    {
                        var number = log_csl.save_log<Exception>(ex , "planpreview", "Page_Load", nc_id, User.Identity.Name);
                        string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                        base.Response.Write(close);
                    }
                }
            }
        }



        private static string detalle_table(List<credit_detail> Detalle, string pValor1, string pValor2, string pValor3)
        {
            if (Detalle == null)
            {
                return "<p>Hubo un error al Cargar los datos comuniquese con el área de IT de Cgsa</p>";
            }

            if (Detalle.Count <=0)
            {
                return "<p><strong> No existen registros de nota de crédito para mostrar.</strong></p>";
            }

            StringBuilder tab = new StringBuilder();
            tab.Append("<table class='print_table'>");
            tab.Append("<thead><tr><th>#</th><th>Cód. Servicio</th><th>Servicio</th><th>Cantidad</th><th>Precio</th><th>Subtotal</th><th>Iva</th><th>Carga</th></tr><thead>");
            tab.Append("<tbody>");

            foreach (var f in Detalle)
            {
                
                tab.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>",
                    f.sequence==0?"...":f.sequence.ToString(),
                    String.IsNullOrEmpty(f.codigo_servicio) ? "..." : f.codigo_servicio,
                    f.desc_servicio,
                    f.nc_cantidad == 0 ? "..." : f.nc_cantidad.ToString() ,
                    f.nc_precio == 0 ? "..." : f.nc_precio.ToString(),
                    f.nc_subtotal == 0 ? "..." : f.nc_subtotal.ToString(),
                    f.nc_iva == 0 ? "..." : f.nc_iva.ToString(),
                    String.IsNullOrEmpty(f.numero_carga) ? "..." : f.numero_carga
                    );
            }
            tab.Append("<tbody>");
            tab.Append("<tfoot><tr><th></th><th></th><th></th><th></th><th></th><th></th><th>SUBTOTAL $</th><th>" + pValor1 + "</th></tr>" +
                "<tr><th></th><th></th><th></th><th></th><th></th><th></th><th>IVA $</th><th>" + pValor2 + "</th></tr>"+
                "<tr><th></th><th></th><th></th><th></th><th></th><th></th><th>TOTAL $</th><th>" + pValor3 + "</th></tr><tfoot>");
            tab.Append("</table>");

            return tab.ToString();
        }

    }
}