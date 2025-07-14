using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using csl_log;
using ControlOPC.Entidades;
using System.Text;

namespace CSLSite
{
    public partial class proforma_preview : System.Web.UI.Page
    {

        //AntiXRCFG
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        private string sid = string.Empty;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                var number = log_csl.save_log<Exception>(ex, "printplan", "Init", sid, Request.UserHostAddress);
                this.AbortResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);                   
                return;
            }

            try
            {


                sid = QuerySegura.DecryptQueryString(Request.QueryString["sid"]);
                if (Request.QueryString["sid"] == null || string.IsNullOrEmpty(sid))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de plan no valido", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "printplan", "Init", sid, Request.UserHostAddress);
                    this.AbortResponse( string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}",Request.UserHostAddress, Request.Url, Request.HttpMethod), null);                   
                    return;
                }

            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "printplan", "Page_Init", sid, User.Identity.Name);
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
                      
                        sid = sid.Trim().Replace("\0", string.Empty);

                        if (!Int64.TryParse(sid, out id))
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, sid no es numerico", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "plapreview", "Page_Load", sid == null ? "sid is no es numerico" : sid, User.Identity.Name);
                            this.AbortResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                            return;
                        }
                        var vv = new ProformaCab(Math.Abs(id));
                        var msg = string.Empty;
                        //recupero el objeto
                        if ( !vv.PopulateMyData(out msg))
                        {
                            var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                            var number = log_csl.save_log<Exception>(ex, "plapreview", "Page_Load", sid == null ? "sid is null" : sid, User.Identity.Name);
                            this.AbortResponse(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio:{0}", number), null);
                            return;
                        }
                        //recupero detalle
                        vv.LoadDetalle();
                       
                        this.barras.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=200&height=60&size=50", vv.Id.ToString("D8"));
                        if (!string.IsNullOrEmpty( vv.Tipo_carga))
                        {
                            if (vv.Tipo_carga.Equals("Contenedor")) { full.InnerText = "( X )"; }
                            else if (vv.Tipo_carga.Equals("BreakBull")) { csuelta.InnerText = "( X )"; }

                            full.Visible = false;
                            csuelta.Visible = false;
                        }
                        referencia.InnerText = vv.Vessel_visit_reference;
                        num_proforma.InnerText = "PROFORMA #" + vv.Id.ToString("D8");
                        anumber.InnerText = vv.Vessel_visit_reference;
                        FecProforma.InnerText = vv.Create_date.HasValue ? vv.Create_date.Value.ToString("dd/MM/yyyy HH:mm") : "...";
                        EstProforma.InnerText = vv.Status;
                        ruc.InnerText = vv.Opc_id;
                        proveedor.InnerText = vv.Opc_name;
                        generado.InnerText = vv.Create_user;
                        observacion.InnerText = vv.Observacion;
                        var vs = Vessel.ListaVessel(vv.Vessel_visit_reference).FirstOrDefault();
                        if (vs != null)
                        {                  
                            buque.InnerText = vs.NAME;
                        }
                        this.barcode2.ImageUrl = string.Format(@"../handler/barcode.ashx?code={0}&format=E9&width=200&height=60&size=50", vv.Id.ToString("D8"));
                        this.detalle_data.InnerHtml = detalle_table(vv.ProformaDetalle);
                        this.fechagenera.InnerText = string.Format("{0}", vv.Create_date.HasValue ? vv.Create_date.Value.ToString("dd/MM/yyyy HH:mm") : "...");
                        this.fechaimprime.InnerText = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        this.Total.InnerText = vv.ProformaDetalle.Sum(c => c.Total).ToString();
                    }
                    catch (Exception ex)
                    {
                        var number = log_csl.save_log<Exception>(ex , "planpreview", "Page_Load", sid, User.Identity.Name);
                        string close = CSLSite.CslHelper.ExitForm(string.Format("Hubo un problema durante la impresión, por favor repórtelo con este código: A00-{0}", number));
                        base.Response.Write(close);
                    }
                }
            }
        }



        private static string detalle_table(List<ProformaDet> Detalle)
        {
            if (Detalle == null)
            {
                return "<p>Hubo un error al Cargar los datos comuniquese con el área de IT de Cgsa</p>";
            }

            if (Detalle.Count <=0)
            {
                return "<p><strong> No existen registros de grúas que mostrar.</strong></p>";
            }

            StringBuilder tab = new StringBuilder();
            tab.Append("<table class='print_table'>");
            tab.Append("<thead><tr><th>#</th><th>Grúa</th><th>Concpeto</th><th>Inicio</th><th>Fin</th><th>Cantidad/Horas</th><th>Valor/Horas</th><th>Subtotal</th></tr><thead>");
            tab.Append("<tbody>");

            foreach (var f in Detalle)
            {
                string cGrua = "";
                var vs = Crane.GetCrane(f.Vessel_crane_gkey);
                if (vs != null)
                {
                    cGrua = vs.Id;
                }

                tab.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>",
                    f.Line==0?"...":f.Line.ToString(),
                    String.IsNullOrEmpty(cGrua) ? "..." : cGrua,
                    f.Concepto_name,
                    f.Turn_time_start.HasValue?f.Turn_time_start.Value.ToString("dd/MMM/yyyy HH:mm"):  "...",
                    f.Turn_time_end.HasValue ? f.Turn_time_end.Value.ToString("dd/MMM/yyyy HH:mm") : "...",
                    f.Total_horas == 0 ? "..." : f.Total_horas.ToString(),
                    f.Precio_hora == 0 ? "..." : f.Precio_hora.ToString(),
                     f.Total == 0 ? "..." : f.Total.ToString()
                    );
            }
            tab.Append("<tbody>");
            tab.Append("<tfoot><tr><th></th><th></th><th></th><th></th><th></th><th></th><th>TOTAL $</th><th>" + Detalle.Sum(c => c.Total).ToString() +"</th></tr><tfoot>");
            tab.Append("</table>");

            return tab.ToString();
        }

    }
}